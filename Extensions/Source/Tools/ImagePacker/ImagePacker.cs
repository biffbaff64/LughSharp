// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using Corelib.LibCore.Utils.Exceptions;
using JetBrains.Annotations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Extensions.Source.Tools.ImagePacker;

/// <summary>
/// A simple image packer class based on the nice algorithm by blackpawn.
/// See <see href="http://www.blackpawn.com/texts/lightmaps/default.html">here</see>
/// for details.
/// <para>
/// <b>Usage:</b> instanciate an ImagePacker instance, load and optionally sort the
/// images you want to add by size (e. g. area) then insert each image via a call to
/// <see cref="InsertImage(String, Image{Rgba32})"/>. When you are done with inserting
/// images you can reference the property <see cref="Image"/> for the actual Image that
/// holds the packed images. Additionally you can get a Dictionary where a) the keys are
/// the names you specified when inserting and b) the values are the rectangles within
/// the packed image where that specific image is located. All things are given in pixels.
/// </para>
/// <para>
/// See the <see cref="main(string[])"/> method for an example that will generate 100
/// random images, pack them and then output the packed image as a png along with a json
/// file holding the image descriptors.
/// </para>
/// <para>
/// In some cases it is beneficial to add padding and to duplicate the border pixels of
/// an inserted image so that there is no bleeding of neighbouring pixels when using the
/// packed image as a texture. You can specify the padding as well as whether to duplicate
/// the border pixels in the constructor.
/// </para>
/// </summary>
[PublicAPI]
public class ImagePacker
{
    public Image< Rgba32 >                 Image { get; }
    public Dictionary< string, Rectangle > Rects { get; } = new();

    private readonly int  _padding;
    private readonly bool _duplicateBorder;
    private readonly Node _root;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new ImagePacker which will insert all supplied images into a <tt>width</tt>
    /// by <tt>height</tt> image. <tt>padding</tt> specifies the minimum number of pixels to
    /// insert between images. <tt>border</tt> will duplicate the border pixels of the inserted
    /// images to avoid seams when rendering with bi-linear filtering on.
    /// </summary>
    /// <param name="width"> the width of the output image </param>
    /// <param name="height"> the height of the output image </param>
    /// <param name="padding"> the number of padding pixels </param>
    /// <param name="duplicateBorder"> whether to duplicate the border </param>
    public ImagePacker( int width, int height, int padding, bool duplicateBorder )
    {
        Image            = new Image< Rgba32 >( width, height );
        _padding         = padding;
        _duplicateBorder = duplicateBorder;
        _root            = new Node( 0, 0, width, height );
    }

    /// <summary>
    /// Inserts an image into the current image packer with the specified name.
    /// </summary>
    /// <param name="name">The name to associate with the inserted image.</param>
    /// <param name="image">The image to be inserted.</param>
    /// <exception cref="GdxRuntimeException">
    /// Thrown when the image does not fit or when an image with the same name already exists.
    /// </exception>
    public void InsertImage( string name, Image< Rgba32 > image )
    {
        if ( Rects.ContainsKey( name ) )
        {
            throw new GdxRuntimeException( $"Key with name '{name}' is already in map" );
        }

        var borderPixels = ( _padding + ( _duplicateBorder ? 1 : 0 ) ) * 2;
        var rect         = new Rectangle( 0, 0, image.Width + borderPixels, image.Height + borderPixels );
        var node         = Insert( rect );

        if ( node == null )
        {
            throw new GdxRuntimeException( "Image didn't fit" );
        }

        node.LeaveName = name;

        rect = new Rectangle( node.Rect.X + borderPixels / 2,
                              node.Rect.Y + borderPixels / 2,
                              node.Rect.Width - borderPixels,
                              node.Rect.Height - borderPixels );

        Rects[ name ] = rect;

        Image.Mutate( ctx =>
        {
            ctx.DrawImage( image, new Point( rect.X, rect.Y ), 1f );

            if ( _duplicateBorder )
            {
                DuplicateBorder( ctx, image, rect );
            }
        } );
    }

    /// <summary>
    /// Attempts to insert a rectangle into the image packer and returns the node
    /// where the rectangle was inserted.
    /// </summary>
    /// <param name="rect">The rectangle to insert.</param>
    /// <returns>
    /// A node representing the position where the rectangle was inserted, or null
    /// if the rectangle did not fit.
    /// </returns>
    private Node? Insert( Rectangle rect )
    {
        var stack = new Stack< Node >();
        
        stack.Push( _root );

        while ( stack.Count > 0 )
        {
            var node = stack.Pop();

            if ( node.LeaveName == null && node is { LeftChild: not null, RightChild: not null } )
            {
                stack.Push( node.RightChild );
                stack.Push( node.LeftChild );
                continue;
            }

            if ( ( node.LeaveName != null ) || ( node.Rect.Width < rect.Width ) || ( node.Rect.Height < rect.Height ) )
            {
                continue;
            }

            if ( ( node.Rect.Width == rect.Width ) && ( node.Rect.Height == rect.Height ) )
            {
                return node;
            }

            node.Split( rect );
            return node.LeftChild;
        }

        return null;
    }

    /// <summary>
    /// Duplicates the border of the given image around the specified rectangle to handle padding.
    /// </summary>
    /// <param name="ctx">The image processing context.</param>
    /// <param name="image">The image whose border is to be duplicated.</param>
    /// <param name="rect">
    /// The rectangle that outlines where the image should be drawn and the border duplicated.
    /// </param>
    private static void DuplicateBorder( IImageProcessingContext ctx, Image< Rgba32 > image, Rectangle rect )
    {
        // Top
        ctx.DrawImage( image, new Point( rect.X, rect.Y - 1 ), new Rectangle( 0, 0, image.Width, 1 ), 1 );
        // Bottom
        ctx.DrawImage( image, new Point( rect.X, rect.Bottom ), new Rectangle( 0, image.Height - 1, image.Width, 1 ), 1 );
        // Left
        ctx.DrawImage( image, new Point( rect.X - 1, rect.Y ), new Rectangle( 0, 0, 1, image.Height ), 1 );
        // Right
        ctx.DrawImage( image, new Point( rect.Right, rect.Y ), new Rectangle( image.Width - 1, 0, 1, image.Height ), 1 );
        // Top-left corner
        ctx.DrawImage( image, new Point( rect.X - 1, rect.Y - 1 ), new Rectangle( 0, 0, 1, 1 ), 1 );
        // Top-right corner
        ctx.DrawImage( image, new Point( rect.Right, rect.Y - 1 ), new Rectangle( image.Width - 1, 0, 1, 1 ), 1 );
        // Bottom-left corner
        ctx.DrawImage( image, new Point( rect.X - 1, rect.Bottom ), new Rectangle( 0, image.Height - 1, 1, 1 ), 1 );
        // Bottom-right corner
        ctx.DrawImage( image, new Point( rect.Right, rect.Bottom ), new Rectangle( image.Width - 1, image.Height - 1, 1, 1 ), 1 );
    }

    /// <summary>
    /// Creates a new image with the specified width, height, and background color.
    /// </summary>
    /// <param name="width">The width of the image to create.</param>
    /// <param name="height">The height of the image to create.</param>
    /// <param name="color">The background color of the image.</param>
    /// <returns>A new image with the specified dimensions and background color.</returns>
    private static Image< Rgba32 > CreateImage( int width, int height, Color color )
    {
        return new Image< Rgba32 >( width, height, color );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Entry point for the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void main( string[] args )
    {
        var rand   = new Random( 0 );
        var packer = new ImagePacker( 512, 512, 1, true );
        var images = new Image< Rgba32 >[ 100 ];

        for ( var i = 0; i < images.Length; i++ )
        {
            var color = Color.FromRgba( ( byte )rand.Next( 256 ), ( byte )rand.Next( 256 ), ( byte )rand.Next( 256 ), 255 );
            images[ i ] = CreateImage( rand.Next( 10, 61 ), rand.Next( 10, 61 ), color );
        }

        Array.Sort( images, ( a, b ) => b.Width * b.Height - a.Width * a.Height );

        for ( var i = 0; i < images.Length; i++ )
        {
            packer.InsertImage( $"image_{i}", images[ i ] );
        }

        packer.Image.Save( "packed.png" );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Represents a node used in the image packing process.
    /// Each node can either be a leaf node containing a rectangle region or an internal node with children.
    /// </summary>
    /// <param name="x">The x-coordinate of the node's region.</param>
    /// <param name="y">The y-coordinate of the node's region.</param>
    /// <param name="width">The width of the node's region.</param>
    /// <param name="height">The height of the node's region.</param>
    private class Node( int x, int y, int width, int height )
    {
        public Rectangle Rect       { get; } = new( x, y, width, height );
        public Node?     LeftChild  { get; private set; }
        public Node?     RightChild { get; private set; }
        public string?   LeaveName  { get; set; }

        // --------------------------------------------------------------------

        /// <summary>
        /// Splits the current node into two children based on the specified rectangle dimensions.
        /// </summary>
        /// <param name="rect">The rectangle that defines the dimensions for splitting the node.</param>
        public void Split( Rectangle rect )
        {
            LeftChild = new Node( Rect.X, Rect.Y, rect.Width, rect.Height );

            var deltaWidth  = Rect.Width - rect.Width;
            var deltaHeight = Rect.Height - rect.Height;

            RightChild = deltaWidth > deltaHeight
                ? new Node( Rect.X + rect.Width, Rect.Y, deltaWidth, Rect.Height )
                : new Node( Rect.X, Rect.Y + rect.Height, Rect.Width, deltaHeight );
        }
    }
}