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

namespace LibGDXSharp.G2D;

/// <summary>
/// Packs <see cref="Pixmap"/>s into one or more <see cref="Page"/> to generate
/// an atlas of pixmap instances. Provides means to directly convert the pixmap
/// atlas to a <see cref="TextureAtlas"/>. The packer supports padding and border
/// pixel duplication, specified during construction. The packer supports incremental
/// inserts and updates of TextureAtlases generated with this class.
/// <para>
/// How bin packing is performed can be customized via <see cref="IPackStrategy"/>.
/// </para>
/// <para>
/// All methods can be called from any thread unless otherwise noted.
/// </para>
/// <para>
/// One-off usage:
/// 
/// <code>
/// // 512x512 pixel pages, RGB565 format, 2 pixels of padding, border duplication
/// PixmapPacker packer = new PixmapPacker(512, 512, Format.RGB565, 2, true);
/// packer.Pack(&quot;First Pixmap&quot;, pixmap1);
/// packer.Pack(&quot;Second Pixmap&quot;, pixmap2);
/// TextureAtlas atlas = packer.GenerateTextureAtlas(TextureFilter.Nearest, TextureFilter.Nearest, false);
/// packer.Dispose();
/// // ...
/// atlas.Dispose();
/// </code>
/// 
/// With this usage pattern, disposing the packer will not dispose any pixmaps
/// used by the texture atlas. The texture atlas must also be disposed when no
/// longer needed.
/// 
/// Incremental texture atlas usage:
/// 
/// <code>
/// // 512x512 pixel pages, RGB565 format, 2 pixels of padding, no border duplication
/// PixmapPacker packer = new PixmapPacker(512, 512, Format.RGB565, 2, false);
/// TextureAtlas atlas = new TextureAtlas();
/// 
/// // potentially on a separate thread, e.g. downloading thumbnails
/// packer.Pack(&quot;thumbnail&quot;, thumbnail);
/// 
/// // on the rendering thread, every frame
/// packer.UpdateTextureAtlas(atlas, TextureFilter.Linear, TextureFilter.Linear, false);
/// 
/// // once the atlas is no longer needed, make sure you get the final additions. This might
/// // be more elaborate depending on your threading model.
/// packer.UpdateTextureAtlas(atlas, TextureFilter.Linear, TextureFilter.Linear, false);
/// // ...
/// atlas.Dispose();
/// </code>
/// 
/// Pixmap-only usage:
/// 
/// <code>
/// PixmapPacker packer = new PixmapPacker(512, 512, Format.RGB565, 2, true);
/// packer.Pack(&quot;First Pixmap&quot;, pixmap1);
/// packer.Pack(&quot;Second Pixmap&quot;, pixmap2);
/// 
/// // do something interesting with the resulting pages
/// foreach (Page page in packer.GetPages())
/// {
/// 	// ...
/// }
/// 
/// packer.Dispose();
/// </code>
/// </para>
/// </summary>
[PublicAPI]
public class PixmapPacker : IDisposable
{
    public int           PageWidth        { get; set; }
    public int           PageHeight       { get; set; }
    public Pixmap.Format PageFormat       { get; set; }
    public Color         TransparentColor { get; set; } = new( 0f, 0f, 0f, 0f );

    private bool          _packToTexture;
    private bool          _disposed;
    private int           _padding;
    private bool          _duplicateBorder;
    private bool          _stripWhitespaceX;
    private bool          _stripWhitespaceY;
    private int           _alphaThreshold;
    private List< Page >  _pages = new();
    private IPackStrategy _packStrategy;

//    private Pattern       _indexPattern = Pattern.Compile( "(.+)_(\\d+)$" );

    /// <summary>
    /// Uses <see cref="GuillotineStrategy"/>.
    /// <see cref="PixmapPacker(int, int, Pixmap.Format, int, bool, bool, bool, IPackStrategy)"/>
    /// </summary>
    public PixmapPacker( int pageWidth, int pageHeight, Pixmap.Format pageFormat, int padding, bool duplicateBorder )
        : this( pageWidth, pageHeight, pageFormat, padding, duplicateBorder, false, false, new GuillotineStrategy() )
    {
    }

    /// <summary>
    /// <see cref="PixmapPacker(int, int, Pixmap.Format, int, bool, bool, bool, IPackStrategy)"/>
    /// </summary>
    public PixmapPacker( int pageWidth,
                         int pageHeight,
                         Pixmap.Format pageFormat,
                         int padding,
                         bool duplicateBorder,
                         IPackStrategy packStrategy )
        : this
            (
             pageWidth,
             pageHeight,
             pageFormat,
             padding,
             duplicateBorder,
             false,
             false,
             packStrategy
            )
    {
    }

    /// <summary>
    /// Creates a new ImagePacker which will insert all supplied pixmaps into
    /// one or more <tt>pageWidth</tt> by <tt>pageHeight</tt> pixmaps using the
    /// specified strategy.
    /// </summary>
    /// <param name="pageWidth"></param>
    /// <param name="pageHeight"></param>
    /// <param name="pageFormat"></param>
    /// <param name="padding"> the number of blank pixels to insert between pixmaps. </param>
    /// <param name="duplicateBorder">
    /// duplicate the border pixels of the inserted images to avoid seams when
    /// rendering with bi-linear filtering on.
    /// </param>
    /// <param name="stripWhitespaceX"> strip whitespace in x axis </param>
    /// <param name="stripWhitespaceY"> strip whitespace in y axis </param>
    /// <param name="packStrategy"></param>
    public PixmapPacker( int pageWidth,
                         int pageHeight,
                         Pixmap.Format pageFormat,
                         int padding,
                         bool duplicateBorder,
                         bool stripWhitespaceX,
                         bool stripWhitespaceY,
                         IPackStrategy packStrategy )
    {
        this.PageWidth         = pageWidth;
        this.PageHeight        = pageHeight;
        this.PageFormat        = pageFormat;
        this._padding          = padding;
        this._duplicateBorder  = duplicateBorder;
        this._stripWhitespaceX = stripWhitespaceX;
        this._stripWhitespaceY = stripWhitespaceY;
        this._packStrategy     = packStrategy;
    }

    /// <summary>
    /// Sorts the images to the optimal order they should be packed.
    /// Some packing strategies rely heavily on the images being sorted.
    /// </summary>
    public void Sort( List< Pixmap > images )
    {
        _packStrategy.Sort( images );
    }

    /// <summary>
    /// Inserts the pixmap without a name. It cannot be looked up by name.
    /// </summary>
    public RectangleShape Pack( Pixmap image )
    {
        return Pack( null, image );
    }

    /// <summary>
    /// Inserts the pixmap. If name was not null, you can later retrieve
    /// the image's position in the output image via <see cref="GetRect(string)"/>".
    /// </summary>
    /// <param name="name"> If null, the image cannot be looked up by name. </param>
    /// <param name="image"></param>
    /// <returns> RectangleShape describing the area the pixmap was rendered to. </returns>
    /// <exception cref="GdxRuntimeException">
    /// in case the image did not fit due to the page size being too small
    /// or providing a duplicate name.
    /// </exception>
    public RectangleShape? Pack( string? name, Pixmap image )
    {
        if ( _disposed )
        {
            return null;
        }

        if ( ( name != null ) && ( GetRect( name ) != null ) )
        {
            throw new GdxRuntimeException( $"Pixmap has already been packed with name: {name}" );
        }

        PixmapPackerRectangle rect;
        Pixmap                pixmapToDispose = null;

        if ( name != null && name.endsWith( ".9" ) )
        {
            rect            = new PixmapPackerRectangle( 0, 0, image.getWidth() - 2, image.getHeight() - 2 );
            pixmapToDispose = new Pixmap( image.getWidth() - 2, image.getHeight() - 2, image.getFormat() );
            pixmapToDispose.setBlending( Blending.None );
            rect.splits = getSplits( image );
            rect.pads   = getPads( image, rect.splits );
            pixmapToDispose.drawPixmap( image, 0, 0, 1, 1, image.getWidth() - 1, image.getHeight() - 1 );
            image = pixmapToDispose;
            name  = name.split( "\\." )[ 0 ];
        }
        else
        {
            if ( stripWhitespaceX || stripWhitespaceY )
            {
                int originalWidth  = image.getWidth();
                int originalHeight = image.getHeight();

                //Strip whitespace, manipulate the pixmap and return corrected Rect
                int top    = 0;
                int bottom = image.getHeight();

                if ( stripWhitespaceY )
                {
                    outer:

                    for ( int y = 0; y < image.getHeight(); y++ )
                    {
                        for ( int x = 0; x < image.getWidth(); x++ )
                        {
                            int pixel = image.getPixel( x, y );
                            int alpha = ( ( pixel & 0x000000ff ) );

                            if ( alpha > alphaThreshold ) break
                            outer;
                        }

                        top++;
                    }

                    outer:

                    for ( int y = image.getHeight(); --y >= top; )
                    {
                        for ( int x = 0; x < image.getWidth(); x++ )
                        {
                            int pixel = image.getPixel( x, y );
                            int alpha = ( ( pixel & 0x000000ff ) );

                            if ( alpha > alphaThreshold ) break
                            outer;
                        }

                        bottom--;
                    }
                }

                int left  = 0;
                int right = image.getWidth();

                if ( stripWhitespaceX )
                {
                    outer:

                    for ( int x = 0; x < image.getWidth(); x++ )
                    {
                        for ( int y = top; y < bottom; y++ )
                        {
                            int pixel = image.getPixel( x, y );
                            int alpha = ( ( pixel & 0x000000ff ) );

                            if ( alpha > alphaThreshold ) break
                            outer;
                        }

                        left++;
                    }

                    outer:

                    for ( int x = image.getWidth(); --x >= left; )
                    {
                        for ( int y = top; y < bottom; y++ )
                        {
                            int pixel = image.getPixel( x, y );
                            int alpha = ( ( pixel & 0x000000ff ) );

                            if ( alpha > alphaThreshold ) break
                            outer;
                        }

                        right--;
                    }
                }

                int newWidth  = right - left;
                int newHeight = bottom - top;

                pixmapToDispose = new Pixmap( newWidth, newHeight, image.getFormat() );
                pixmapToDispose.setBlending( Blending.None );
                pixmapToDispose.drawPixmap( image, 0, 0, left, top, newWidth, newHeight );
                image = pixmapToDispose;

                rect = new PixmapPackerRectangle( 0, 0, newWidth, newHeight, left, top, originalWidth, originalHeight );
            }
            else
            {
                rect = new PixmapPackerRectangle( 0, 0, image.getWidth(), image.getHeight() );
            }
        }

        if ( rect.getWidth() > pageWidth || rect.getHeight() > pageHeight )
        {
            if ( name == null ) throw new GdxRuntimeException( "Page size too small for pixmap." );

            throw new GdxRuntimeException( "Page size too small for pixmap: " + name );
        }

        Page page = _packStrategy.Pack( this, name, rect );

        if ( name != null )
        {
            page.Rects[ name ] = rect;
            page.AddedRects.Add( name );
        }

        var rectX      = ( int )rect.X;
        var rectY      = ( int )rect.Y;
        var rectWidth  = ( int )rect.Width;
        var rectHeight = ( int )rect.Height;

        if ( _packToTexture && !_duplicateBorder && ( page.Texture != null ) && !page.Dirty )
        {
            page.Texture?.Bind();

            Gdx.GL.GLTexSubImage2D
                (
                 page.Texture.GLTarget, 0, rectX, rectY, rectWidth, rectHeight, image.GLFormat,
                 image.GLType, image.Pixels
                );
        }
        else
        {
            page.Dirty = true;
        }

        page.Image.DrawPixmap( image, rectX, rectY );

        if ( _duplicateBorder )
        {
            var imageWidth  = image.Width;
            var imageHeight = image.Height;

            // Copy corner pixels to fill corners of the padding.
            page.Image.DrawPixmap( image, 0, 0, 1, 1, rectX - 1, rectY - 1, 1, 1 );
            page.Image.DrawPixmap( image, imageWidth - 1, 0, 1, 1, rectX + rectWidth, rectY - 1, 1, 1 );
            page.Image.DrawPixmap( image, 0, imageHeight - 1, 1, 1, rectX - 1, rectY + rectHeight, 1, 1 );
            page.Image.DrawPixmap( image, imageWidth - 1, imageHeight - 1, 1, 1, rectX + rectWidth, rectY + rectHeight, 1, 1 );

            // Copy edge pixels into padding.
            page.Image.DrawPixmap( image, 0, 0, imageWidth, 1, rectX, rectY - 1, rectWidth, 1 );
            page.Image.DrawPixmap( image, 0, imageHeight - 1, imageWidth, 1, rectX, rectY + rectHeight, rectWidth, 1 );
            page.Image.DrawPixmap( image, 0, 0, 1, imageHeight, rectX - 1, rectY, 1, rectHeight );
            page.Image.DrawPixmap( image, imageWidth - 1, 0, 1, imageHeight, rectX + rectWidth, rectY, 1, rectHeight );
        }

        if ( pixmapToDispose != null )
        {
            pixmapToDispose.Dispose();
        }

        return rect;
    }

    // ========================================================================

    public class Page
    {
        public Dictionary< string, PixmapPackerRectangle > Rects { get; set; } = new();

        public Pixmap         Image      { get; set; }
        public Texture?       Texture    { get; set; }
        public List< string > AddedRects { get; set; } = new();
        public bool           Dirty      { get; set; }

        /** Creates a new page filled with the color provided by the {@link PixmapPacker#getTransparentColor()} */
        public Page( PixmapPacker packer )
        {
            Image       = new Pixmap( packer.PageWidth, packer.PageHeight, packer.PageFormat );
            Image.Blend = Pixmap.Blending.None;
            Image.SetColor( packer.TransparentColor );
            Image.FillWithCurrentColor();
        }

        /// <summary>
        /// Creates the texture if it has not been created, else reuploads the
        /// entire page pixmap to the texture if the pixmap has changed since
        /// this method was last called.
        /// </summary>
        /// <returns> true if the texture was created or reuploaded. </returns>
        public bool UpdateTexture( TextureFilter minFilter, TextureFilter magFilter, bool useMipMaps )
        {
            if ( Texture != null )
            {
                if ( !Dirty )
                {
                    return false;
                }

                Texture.Load( Texture.TextureData );
            }
            else
            {
                Texture = new Texture
                    (
                     new PixmapTextureData
                         (
                          Image,
                          Image.GetFormat(),
                          useMipMaps,
                          false,
                          true
                         )
                    );

//                {
//                    @Override
//                    public void dispose ()
//                    {
//                        super.dispose();
//                        image.dispose();
//                    }
//                };

                Texture.SetFilter( minFilter, magFilter );
            }

            Dirty = false;

            return true;
        }
    }

    /// <summary>
    /// Does bin packing by inserting to the right or below previously packed
    /// rectangles. This is good at packing arbitrarily sized images.
    /// </summary>
    public class GuillotineStrategy : IPackStrategy
    {
        public void Sort( List< Pixmap > images )
        {
        }

        /// <summary>
        /// Returns the page the rectangle should be placed in and
        /// modifies the specified rectangle position.
        /// </summary>
        public Page Pack( PixmapPacker packer, string name, RectangleShape rect )
        {
            return null!;
        }
    }

    /// <summary>
    /// Does bin packing by inserting in rows. This is good at
    /// packing images that have similar heights.
    /// </summary>
    public class SkylineStrategy : IPackStrategy
    {
        public void Sort( List< Pixmap > images )
        {
        }

        /// <summary>
        /// Returns the page the rectangle should be placed in and
        /// modifies the specified rectangle position.
        /// </summary>
        public Page Pack( PixmapPacker packer, string name, RectangleShape rect )
        {
            return null!;
        }
    }

    public class PixmapPackerRectangle : RectangleShape
    {
        public int[]? Splits         { get; set; }
        public int[]? Pads           { get; set; }
        public int    OffsetX        { get; set; }
        public int    OffsetY        { get; set; }
        public int    OriginalWidth  { get; set; }
        public int    OriginalHeight { get; set; }

        public PixmapPackerRectangle( int x, int y, int width, int height )
            : base( x, y, width, height )
        {
            this.OffsetX        = 0;
            this.OffsetY        = 0;
            this.OriginalWidth  = width;
            this.OriginalHeight = height;
        }

        public PixmapPackerRectangle( int x,
                                      int y,
                                      int width,
                                      int height,
                                      int left,
                                      int top,
                                      int originalWidth,
                                      int originalHeight )
            : base( x, y, width, height )
        {
            this.OffsetX        = left;
            this.OffsetY        = top;
            this.OriginalWidth  = originalWidth;
            this.OriginalHeight = originalHeight;
        }
    }

    /// <summary>
    /// Choose the page and location for each rectangle.
    /// </summary>
    public interface IPackStrategy
    {
        void Sort( List< Pixmap > images );

        /// <summary>
        /// Returns the page the rectangle should be placed in and
        /// modifies the specified rectangle position.
        /// </summary>
        Page Pack( PixmapPacker packer, string name, RectangleShape rect );
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        // TODO release managed resources here
    }
}
