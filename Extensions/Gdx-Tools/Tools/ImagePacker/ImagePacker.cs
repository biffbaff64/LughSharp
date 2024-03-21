// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using System.Drawing;

using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Maths;

using Color = System.Drawing.Color;

namespace LughSharp.Extensions.Gdx_Tools.ImagePacker;

public class ImagePacker
{
    [PublicAPI]
    public class Node
    {
        public Node?           leftChild;
        public Node?           rightChild;
        public RectangleShape? rect;
        public string?         leaveName;

        public Node( int x, int y, int width, int height, Node leftChild, Node rightChild, string leaveName )
        {
            this.rect       = new RectangleShape( x, y, width, height );
            this.leftChild  = leftChild;
            this.rightChild = rightChild;
            this.leaveName  = leaveName;
        }

        public Node()
        {
            rect = new RectangleShape();
        }
    }

    private BufferedImage                        _image;
    private int                                  _padding;
    private bool                                 _duplicateBorder;
    private Node                                 _root;
    private Dictionary< string, RectangleShape > _rects;

    /// <summary>
    /// Creates a new ImagePacker which will Insert all supplied images into a width by height image.
    /// padding specifies the minimum number of pixels to Insert between images. border will duplicate the
    /// border pixels of the inserted images to avoid seams when rendering with bi-linear filtering on.
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
    public void InsertImage( string name, BufferedImage image )
    {
        if ( _rects.ContainsKey( name ) )
        {
            throw new GdxRuntimeException( $"Key with name '{name}' is already in map" );
        }

        var borderPixels = _padding + ( _duplicateBorder ? 1 : 0 );
        borderPixels <<= 1;
        var rect = new RectangleShape( 0, 0, image.getWidth() + borderPixels, image.getHeight() + borderPixels );
        Node           node = Insert( _root, rect );

        if ( node == null ) throw new GdxRuntimeException( "Image didn't fit" );

        node.leaveName =   name;
        rect           =   new RectangleShape( node.rect );
        rect.Width     -=  borderPixels;
        rect.Height    -=  borderPixels;
        borderPixels   >>= 1;
        rect.X         +=  borderPixels;
        rect.Y         +=  borderPixels;

        _rects[ name ] = rect;

        Graphics g = this._image.createGraphics();
        g.DrawImage( image, rect.X, rect.Y, null );

        // not terribly efficient (as the rest of the code) but will do :p
        if ( _duplicateBorder )
        {
            g.drawImage( image, rect.X, rect.Y - 1, rect.X + rect.Width, rect.Y, 0, 0, image.getWidth(), 1, null );

            g.drawImage( image,
                         rect.X,
                         rect.Y + rect.Height,
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

        g.Dispose();
    }

    private Node Insert( Node node, RectangleShape rect )
    {
        if ( node.leaveName == null && node.leftChild != null && node.rightChild != null )
        {
            Node? newNode;

            if ( ( newNode = Insert( node.leftChild, rect ) ) == null )
            {
                newNode = Insert( node.rightChild, rect );
            }

            return newNode;
        }
        else
        {
            if ( node.leaveName != null ) return null;

            if ( node.rect.Width == rect.Width && node.rect.Height == rect.Height ) return node;

            if ( node.rect.Width < rect.Width || node.rect.Height < rect.Height ) return null;

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

    /** @return the output image */
    public BufferedImage GetImage()
    {
        return _image;
    }

    /// <summary>
    /// Returns the rectangle in the output image of each inserted image.
    /// </summary>
    public Dictionary< string, RectangleShape > GetRects()
    {
        return _rects;
    }

//    public static void main (string[] argv)
//    {
//        Random rand = new Random(0);
//        ImagePacker packer = new ImagePacker(512, 512, 1, true);
//
//        BufferedImage[] images = new BufferedImage[100];
//        for (int i = 0; i < images.length; i++) {
//            Color color = new Color((float)Math.random(), (float)Math.random(), (float)Math.random(), 1);
//            images[i] = createImage(rand.nextInt(50) + 10, rand.nextInt(50) + 10, color);
//        }
//// BufferedImage[] images = { ImageIO.read( new File( "test.png" ) ) };
//
////      Arrays.sort(images, new Comparator<BufferedImage>() {
////          @Override
////          public int compare (BufferedImage o1, BufferedImage o2) {
////              return o2.getWidth() * o2.getHeight() - o1.getWidth() * o1.getHeight();
////          }
////      });
//
//    for ( int i = 0; i < images.length; i++ )
//    {
//        packer.insertImage("" + i, images[i]);
//    }
//
//        ImageIO.write(packer.getImage(), "png", new File("packed.png"));
//    }

    private static BufferedImage CreateImage( int width, int height, Color color )
    {
        var image = new BufferedImage( width, height, BufferedImage.TYPE_4BYTE_ABGR );
        Graphics2D    g     = image.createGraphics();
        g.setColor( color );
        g.fillRect( 0, 0, width, height );
        g.dispose();

        return image;
    }
}
