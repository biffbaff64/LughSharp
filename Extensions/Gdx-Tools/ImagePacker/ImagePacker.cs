// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Drawing;

using Color = LibGDXSharp.Graphics.Color;

namespace LibGDXSharp.Extensions.Tools.ImagePacker;

/// <summary>
/// A simple image packer class based on the nice algorithm by blackpawn.
/// <para>
/// See <a href="http://www.blackpawn.com/texts/lightmaps/default.html"> here </a> for details.
/// </para>
/// <para>
/// <b>Usage:</b> instanciate an <tt>ImagePacker</tt> instance, load and optionally sort
/// the images you want to add by size (e.g. area) then insert each image via a call to
/// <see cref="InsertImage(string, BufferedImage)"/>. When you are done with inserting
/// images you can call <see cref="GetImage()"/> for the <see cref="BufferedImage"/> that
/// holds the packed images.
/// </para>
/// <para>
/// Additionally you can get a <tt>Dictionary&lt;string, RectangleShape&gt;</tt>
/// where the keys the names you specified when inserting and the values are the rectangles
/// within the packed image where that specific image is located. All things are given
/// in pixels.
/// </para>
/// <para>
/// See the <see cref="Main(string[])"/> method for an example that will generate 100
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
public class ImagePacker
{
    internal class Node
    {
        public readonly RectangleShape rect;
        public          Node?          leftChild;
        public          Node?          rightChild;
        public          string?        leaveName;

        public Node()
        {
            this.rect       = new RectangleShape();
            this.leftChild  = null!;
            this.rightChild = null!;
            this.leaveName  = string.Empty;
        }

        public Node( int x, int y, int width, int height, Node? leftChild, Node? rightChild, string? leaveName )
        {
            this.rect       = new RectangleShape( x, y, width, height );
            this.leftChild  = leftChild;
            this.rightChild = rightChild;
            this.leaveName  = leaveName;
        }
    }

    private BufferedImage                        _image;
    private int                                  _padding;
    private bool                                 _duplicateBorder;
    private Node                                 _root;
    private Dictionary< string, RectangleShape > _rects;

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
        this._image           = new BufferedImage( width, height, BufferedImage.TYPE_4BYTE_ABGR );
        this._padding         = padding;
        this._duplicateBorder = duplicateBorder;
        this._root            = new Node( 0, 0, width, height, null, null, null );
        this._rects           = new Dictionary< string, RectangleShape >();
    }

    /// <summary>
    /// Inserts the given image. You can later on retrieve the images position in the output
    /// image via the supplied name and the method <see cref="GetRects()"/>.
    /// </summary>
    /// <param name="name"> the name of the image </param>
    /// <param name="image"> the image </param>
    /// <exception cref="GdxRuntimeException">
    /// If the image did not fit or you specified a duplicate name
    /// </exception>
    public void InsertImage( string name, BufferedImage image )
    {
        if ( _rects.ContainsKey( name ) )
        {
            throw new GdxRuntimeException( "Key with name '" + name + "' is already in map" );
        }

        var borderPixels = _padding + ( _duplicateBorder ? 1 : 0 );
        borderPixels <<= 1;
        var  rect = new RectangleShape( 0, 0, image.getWidth() + borderPixels, image.getHeight() + borderPixels );
        Node node = Insert( _root, rect );

        if ( node == null )
        {
            throw new GdxRuntimeException( "Image didn't fit" );
        }

        node.leaveName =   name;
        rect           =   new RectangleShape( node.rect );
        rect.Width     -=  borderPixels;
        rect.Height    -=  borderPixels;
        borderPixels   >>= 1;
        rect.X         +=  borderPixels;
        rect.Y         +=  borderPixels;
        _rects[ name ] =   rect;

        Graphics2D g = this._image.CreateGraphics();
        g.drawImage( image, rect.X, rect.Y, null );

        // not terribly efficient (as the rest of the code) but will do :p
        if ( _duplicateBorder )
        {
            g.drawImage( image, rect.X, rect.Y - 1, rect.X + rect.Width, rect.Y, 0, 0, image.getWidth(), 1, null );

            g.drawImage( image,
                         rect.X, rect.Y + rect.Height,
                         rect.X + rect.Width,
                         rect.Y + rect.Height + 1,
                         0,
                         image.getHeight() - 1,
                         image.getWidth(),
                         image.getHeight(),
                         null );

            g.drawImage( image, rect.X - 1, rect.Y, rect.X, rect.Y + rect.Height, 0, 0, 1, image.getHeight(), null );

            g.drawImage( image,
                         rect.X + rect.Width,
                         rect.Y,
                         rect.X + rect.Width + 1,
                         rect.Y + rect.Height,
                         image.getWidth() - 1,
                         0,
                         image.getWidth(),
                         image.getHeight(),
                         null );

            g.drawImage( image, rect.X - 1, rect.Y - 1, rect.X, rect.Y, 0, 0, 1, 1, null );

            g.drawImage( image,
                         rect.X + rect.Width,
                         rect.Y - 1,
                         rect.X + rect.Width + 1,
                         rect.Y,
                         image.getWidth() - 1,
                         0,
                         image.getWidth(),
                         1,
                         null );

            g.drawImage( image,
                         rect.X - 1,
                         rect.Y + rect.Height,
                         rect.X,
                         rect.Y + rect.Height + 1,
                         0,
                         image.getHeight() - 1,
                         1,
                         image.getHeight(),
                         null );

            g.drawImage( image,
                         rect.X + rect.Width,
                         rect.Y + rect.Height,
                         rect.X + rect.Width + 1,
                         rect.Y + rect.Height + 1,
                         image.getWidth() - 1,
                         image.getHeight() - 1,
                         image.getWidth(),
                         image.getHeight(),
                         null );
        }

        g.dispose();
    }

    private Node? Insert( Node node, RectangleShape rect )
    {
        if ( ( node.leaveName == null ) && ( node.leftChild != null ) && ( node.rightChild != null ) )
        {
            Node? newNode = null;

            newNode = Insert( node.leftChild, rect );
            if ( newNode == null ) newNode = Insert( node.rightChild, rect );

            return newNode;
        }
        else
        {
            if ( node.leaveName != null ) return null;

            if ( ( node.rect.Width == rect.Width ) && ( node.rect.Height == rect.Height ) ) return node;

            if ( ( node.rect.Width < rect.Width ) || ( node.rect.Height < rect.Height ) ) return null;

            node.leftChild  = new Node();
            node.rightChild = new Node();

            int deltaWidth  = node.rect.Width - rect.Width;
            int deltaHeight = node.rect.Height - rect.Height;

            if ( deltaWidth > deltaHeight )
            {
                node.leftChild.rect.X      = node.rect.X;
                node.leftChild.rect.Y      = node.rect.Y;
                node.leftChild.rect.Width  = rect.Width;
                node.leftChild.rect.Height = node.rect.Height;

                node.rightChild.rect.X      = node.rect.X + rect.Width;
                node.rightChild.rect.Y      = node.rect.Y;
                node.rightChild.rect.Width  = node.rect.Width - rect.Width;
                node.rightChild.rect.Height = node.rect.Height;
            }
            else
            {
                node.leftChild.rect.X      = node.rect.X;
                node.leftChild.rect.Y      = node.rect.Y;
                node.leftChild.rect.Width  = node.rect.Width;
                node.leftChild.rect.Height = rect.Height;

                node.rightChild.rect.X      = node.rect.X;
                node.rightChild.rect.Y      = node.rect.Y + rect.Height;
                node.rightChild.rect.Width  = node.rect.Width;
                node.rightChild.rect.Height = node.rect.Height - rect.Height;
            }

            return Insert( node.leftChild, rect );
        }
    }

    public BufferedImage GetImage()
    {
        return _image;
    }

    /** @return the rectangle in the output image of each inserted image */
    public Dictionary< string, RectangleShape > GetRects()
    {
        return _rects;
    }

    public static void Main( string[] argv )
    {
        Random      rand   = new Random( 0 );
        ImagePacker packer = new ImagePacker( 512, 512, 1, true );

        BufferedImage[] images = new BufferedImage[ 100 ];

        for ( int i = 0; i < images.length; i++ )
        {
            Color color = new Color( ( float )Math.random(), ( float )Math.random(), ( float )Math.random(), 1 );
            images[ i ] = createImage( rand.nextInt( 50 ) + 10, rand.nextInt( 50 ) + 10, color );
        }

        Arrays.sort( images, new Comparator< BufferedImage >()
        {
            @Override
            public int compare( BufferedImage o1, BufferedImage o2 )
            {
            return o2.getWidth() * o2.getHeight() - o1.getWidth() * o1.getHeight();
        }

        });

        for ( int i = 0; i < images.length; i++ )
        {
            packer.insertImage( "" + i, images[ i ] );
        }

        ImageIO.write( packer.getImage(), "png", new File( "packed.png" ) );
    }

    private static BufferedImage CreateImage( int width, int height, Color color )
    {
        BufferedImage image = new BufferedImage( width, height, BufferedImage.TYPE_4BYTE_ABGR );
        Graphics2D    g     = image.createGraphics();
        g.setColor( color );
        g.fillRect( 0, 0, width, height );
        g.dispose();

        return image;
    }
}
