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

using System.Text.RegularExpressions;

namespace LibGDXSharp.Graphics.G2D;

/// <summary>
///     Packs <see cref="Pixmap" />s into one or more <see cref="Page" /> to generate
///     an atlas of pixmap instances. Provides means to directly convert the pixmap
///     atlas to a <see cref="TextureAtlas" />. The packer supports padding and border
///     pixel duplication, specified during construction. The packer supports incremental
///     inserts and updates of TextureAtlases generated with this class.
///     <para>
///         How bin packing is performed can be customized via <see cref="IPackStrategy" />.
///     </para>
///     <para>
///         All methods can be called from any thread unless otherwise noted.
///     </para>
///     <para>
///         One-off usage:
///         <code>
/// // 512x512 pixel pages, RGB565 format, 2 pixels of padding, border duplication
/// PixmapPacker packer = new PixmapPacker(512, 512, Format.RGB565, 2, true);
/// packer.Pack(&quot;First Pixmap&quot;, pixmap1);
/// packer.Pack(&quot;Second Pixmap&quot;, pixmap2);
/// TextureAtlas atlas = packer.GenerateTextureAtlas(TextureFilter.Nearest, TextureFilter.Nearest, false);
/// packer.Dispose();
/// // ...
/// atlas.Dispose();
/// </code>
///         With this usage pattern, disposing the packer will not dispose any pixmaps
///         used by the texture atlas. The texture atlas must also be disposed when no
///         longer needed.
///         Incremental texture atlas usage:
///         <code>
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
///         Pixmap-only usage:
///         <code>
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
///     </para>
/// </summary>
public class PixmapPacker : IDisposable
{
    private readonly IPackStrategy _packStrategy;
    private readonly bool          _stripWhitespaceX;
    private readonly bool          _stripWhitespaceY;
    private          int           _alphaThreshold;

    private bool _disposed;

//    private Pattern       _indexPattern = Pattern.Compile( "(.+)_(\\d+)$" );

    /// <summary>
    ///     Uses <see cref="GuillotineStrategy" />.
    ///     <see cref="PixmapPacker(int, int, Pixmap.Format, int, bool, bool, bool, IPackStrategy)" />
    /// </summary>
    public PixmapPacker( int pageWidth, int pageHeight, Pixmap.Format pageFormat, int padding, bool duplicateBorder )
        : this( pageWidth, pageHeight, pageFormat, padding, duplicateBorder, false, false, new GuillotineStrategy() )
    {
    }

    /// <summary>
    ///     <see cref="PixmapPacker(int, int, Pixmap.Format, int, bool, bool, bool, IPackStrategy)" />
    /// </summary>
    public PixmapPacker( int pageWidth,
                         int pageHeight,
                         Pixmap.Format pageFormat,
                         int padding,
                         bool duplicateBorder,
                         IPackStrategy packStrategy )
        : this(
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
    ///     Creates a new ImagePacker which will insert all supplied pixmaps into
    ///     one or more <tt>pageWidth</tt> by <tt>pageHeight</tt> pixmaps using the
    ///     specified strategy.
    /// </summary>
    /// <param name="pageWidth"></param>
    /// <param name="pageHeight"></param>
    /// <param name="pageFormat"></param>
    /// <param name="padding"> the number of blank pixels to insert between pixmaps. </param>
    /// <param name="duplicateBorder">
    ///     duplicate the border pixels of the inserted images to avoid seams when
    ///     rendering with bi-linear filtering on.
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
        PageWidth         = pageWidth;
        PageHeight        = pageHeight;
        PageFormat        = pageFormat;
        Padding           = padding;
        DuplicateBorder   = duplicateBorder;
        _stripWhitespaceX = stripWhitespaceX;
        _stripWhitespaceY = stripWhitespaceY;
        _packStrategy     = packStrategy;
    }

    public int           PageWidth        { get; set; }
    public int           PageHeight       { get; set; }
    public Pixmap.Format PageFormat       { get; set; }
    public Color         TransparentColor { get; set; } = new( 0f, 0f, 0f, 0f );
    public bool          PackToTexture    { get; set; }
    public bool          DuplicateBorder  { get; set; }
    public int           Padding          { get; set; }
    public List< Page >  Pages            { get; set; } = new();

    /// <summary>
    ///     Performs application-defined tasks associated with freeing,
    ///     releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        foreach ( Page page in Pages )
        {
            if ( page.Texture == null )
            {
                page.Image.Dispose();
            }
        }

        _disposed = true;
    }

    /// <summary>
    ///     Sorts the images to the optimal order they should be packed.
    ///     Some packing strategies rely heavily on the images being sorted.
    /// </summary>
    public void Sort( List< Pixmap > images ) => _packStrategy.Sort( images );

    /// <summary>
    ///     Inserts the pixmap without a name. It cannot be looked up by name.
    /// </summary>
    public RectangleShape? Pack( Pixmap image ) => Pack( null, image );

    /// <summary>
    ///     Inserts the pixmap. If name was not null, you can later retrieve
    ///     the image's position in the output image via <see cref="GetRect(string)" />".
    /// </summary>
    /// <param name="name"> If null, the image cannot be looked up by name. </param>
    /// <param name="image"></param>
    /// <returns> RectangleShape describing the area the pixmap was rendered to. </returns>
    /// <exception cref="GdxRuntimeException">
    ///     in case the image did not fit due to the page size being too small
    ///     or providing a duplicate name.
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
        Pixmap?               pixmapToDispose = null;

        if ( ( name != null ) && name.EndsWith( ".9" ) )
        {
            rect            = new PixmapPackerRectangle( 0, 0, image.Width - 2, image.Height - 2 );
            pixmapToDispose = new Pixmap( image.Width - 2, image.Height - 2, image.GetFormat() );

            pixmapToDispose.Blending = Pixmap.BlendTypes.None;

            rect.Splits = GetSplits( image );
            rect.Pads   = GetPads( image, rect.Splits );

            pixmapToDispose.DrawPixmap( image, 0, 0, 1, 1, image.Width - 1, image.Height - 1 );

            image = pixmapToDispose;
            name  = name.Split( "\\." )[ 0 ];
        }
        else
        {
            if ( _stripWhitespaceX || _stripWhitespaceY )
            {
                var originalWidth  = image.Width;
                var originalHeight = image.Height;

                // Strip whitespace, manipulate the pixmap and return corrected Rect
                var top    = 0;
                var bottom = image.Height;

                if ( _stripWhitespaceY )
                {
                    outer1:

                    for ( var y = 0; y < image.Height; y++ )
                    {
                        for ( var x = 0; x < image.Width; x++ )
                        {
                            var pixel = image.GetPixel( x, y );
                            var alpha = pixel & 0x000000ff;

                            if ( alpha > _alphaThreshold )
                            {
                                goto outer1;
                            }
                        }

                        top++;
                    }

                    outer2:

                    for ( var y = image.Height; --y >= top; )
                    {
                        for ( var x = 0; x < image.Width; x++ )
                        {
                            var pixel = image.GetPixel( x, y );
                            var alpha = pixel & 0x000000ff;

                            if ( alpha > _alphaThreshold )
                            {
                                goto outer2;
                            }
                        }

                        bottom--;
                    }
                }

                var left  = 0;
                var right = image.Width;

                if ( _stripWhitespaceX )
                {
                    outer3:

                    for ( var x = 0; x < image.Width; x++ )
                    {
                        for ( var y = top; y < bottom; y++ )
                        {
                            var pixel = image.GetPixel( x, y );
                            var alpha = pixel & 0x000000ff;

                            if ( alpha > _alphaThreshold )
                            {
                                goto outer3;
                            }
                        }

                        left++;
                    }

                    outer4:

                    for ( var x = image.Width; --x >= left; )
                    {
                        for ( var y = top; y < bottom; y++ )
                        {
                            var pixel = image.GetPixel( x, y );
                            var alpha = pixel & 0x000000ff;

                            if ( alpha > _alphaThreshold )
                            {
                                goto outer4;
                            }
                        }

                        right--;
                    }
                }

                var newWidth  = right - left;
                var newHeight = bottom - top;

                pixmapToDispose          = new Pixmap( newWidth, newHeight, image.GetFormat() );
                pixmapToDispose.Blending = Pixmap.BlendTypes.None;
                pixmapToDispose.DrawPixmap( image, 0, 0, left, top, newWidth, newHeight );

                image = pixmapToDispose;

                rect = new PixmapPackerRectangle( 0, 0, newWidth, newHeight, left, top, originalWidth, originalHeight );
            }
            else
            {
                rect = new PixmapPackerRectangle( 0, 0, image.Width, image.Height );
            }
        }

        if ( ( rect.Width > PageWidth ) || ( rect.Height > PageHeight ) )
        {
            if ( name == null )
            {
                throw new GdxRuntimeException( "Page size too small for pixmap." );
            }

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

        if ( PackToTexture && !DuplicateBorder && page is { Texture: not null, Dirty: false } )
        {
            page.Texture?.Bind();

            Gdx.GL.GLTexSubImage2D(
                page.Texture!.GLTarget,
                0,
                rectX,
                rectY,
                rectWidth,
                rectHeight,
                image.GLFormat,
                image.GLType,
                image.Pixels
                );
        }
        else
        {
            page.Dirty = true;
        }

        page.Image.DrawPixmap( image, rectX, rectY );

        if ( DuplicateBorder )
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

    public RectangleShape? GetRect( string name )
    {
        foreach ( Page page in Pages )
        {
            RectangleShape? rect = page.Rects[ name ];

            if ( rect != null )
            {
                return rect;
            }
        }

        return null;
    }

    public Page? GetPage( string name )
    {
        foreach ( Page page in Pages )
        {
            if ( page.Rects[ name ] != null )
            {
                return page;
            }
        }

        return null;
    }

    /// <summary>
    ///     Returns the index of the page containing the given packed rectangle.
    /// </summary>
    /// <param name="name"> the name of the image </param>
    /// <returns> the index of the page the image is stored in or -1 </returns>
    public int GetPageIndex( string name )
    {
        for ( var i = 0; i < Pages.Count; i++ )
        {
            if ( Pages[ i ].Rects[ name ] != null )
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    ///     Generates a new <see cref="TextureAtlas" /> from the pixmaps inserted
    ///     so far. After calling this method, disposing the packer will no longer
    ///     dispose the page pixmaps.
    /// </summary>
    public TextureAtlas GenerateTextureAtlas( TextureFilter minFilter, TextureFilter magFilter, bool useMipMaps )
    {
        var atlas = new TextureAtlas();

        UpdateTextureAtlas( atlas, minFilter, magFilter, useMipMaps );

        return atlas;
    }

    /**
     * Updates the {@link TextureAtlas}, adding any new {@link Pixmap} instances packed since the last call to this method. This
     * can be used to insert Pixmap instances on a separate thread via {@link #pack(String, Pixmap)} and update the TextureAtlas on
     * the rendering thread. This method must be called on the rendering thread. After calling this method, disposing the packer
     * will no longer dispose the page pixmaps. Has useIndexes on by default so as to keep backwards compatibility
     */
    public void UpdateTextureAtlas( TextureAtlas atlas,
                                    TextureFilter minFilter,
                                    TextureFilter magFilter,
                                    bool useMipMaps ) => UpdateTextureAtlas( atlas, minFilter, magFilter, useMipMaps, true );

    /// <summary>
    ///     Updates the <see cref="TextureAtlas" />, adding any new <see cref="Pixmap" />
    ///     instances packed since the last call to this method. This can be used to
    ///     insert Pixmap instances on a separate thread via <see cref="Pack(String, Pixmap)" />
    ///     and update the TextureAtlas on the rendering thread. This method must be
    ///     called on the rendering thread. After calling this method, disposing the
    ///     packer will no longer dispose the page pixmaps.
    /// </summary>
    public void UpdateTextureAtlas( TextureAtlas atlas,
                                    TextureFilter minFilter,
                                    TextureFilter magFilter,
                                    bool useMipMaps,
                                    bool useIndexes )
    {
        UpdatePageTextures( minFilter, magFilter, useMipMaps );

        foreach ( Page page in Pages )
        {
            if ( page.AddedRects.Count > 0 )
            {
                foreach ( var name in page.AddedRects )
                {
                    PixmapPackerRectangle? rect = page.Rects[ name ];

                    if ( rect == null )
                    {
                        continue;
                    }

                    var region = new AtlasRegion( page.Texture, ( int )rect.X, ( int )rect.Y, ( int )rect.Width, ( int )rect.Height );

                    if ( rect.Splits != null )
                    {
                        region.Names  = new[] { "split", "pad" };
                        region.values = new[] { rect.Splits, rect.Pads };
                    }

                    var imageIndex = -1;
                    var imageName  = name;

                    if ( useIndexes )
                    {
                        var rx = new Regex( "(.+)_(\\d+)$" );

                        MatchCollection matches = rx.Matches( imageName );

                        if ( matches.Count > 0 )
                        {
                            // The image filename
                            imageName = matches[ 1 ].Name;

                            // The number at the end of the image filename, or -1 if none.
                            imageIndex = int.Parse( matches[ 2 ].Name );
                        }
                    }

                    region.Name           = imageName;
                    region.Index          = imageIndex;
                    region.OffsetX        = rect.OffsetX;
                    region.OffsetY        = ( int )( rect.OriginalHeight - rect.Height - rect.OffsetY );
                    region.OriginalWidth  = rect.OriginalWidth;
                    region.OriginalHeight = rect.OriginalHeight;

                    atlas.Regions.Add( region );
                }

                page.AddedRects.Clear();
                atlas.Textures.Add( page.Texture! );
            }
        }
    }

    /// <summary>
    ///     Calls <see cref="Page.UpdateTexture(TextureFilter, TextureFilter, bool)" />
    ///     for each page and adds a region to the specified array for each page texture.
    /// </summary>
    public void UpdateTextureRegions( List< TextureRegion > regions,
                                      TextureFilter minFilter,
                                      TextureFilter magFilter,
                                      bool useMipMaps )
    {
        UpdatePageTextures( minFilter, magFilter, useMipMaps );

        while ( regions.Count < Pages.Count )
        {
            Texture? texture = Pages[ regions.Count ].Texture;

            if ( texture != null )
            {
                regions.Add( new TextureRegion( texture ) );
            }
        }
    }

    /// <summary>
    ///     Calls <see cref="Page.UpdateTexture(TextureFilter, TextureFilter, bool)" />"
    ///     for each page.
    /// </summary>
    public void UpdatePageTextures( TextureFilter minFilter, TextureFilter magFilter, bool useMipMaps )
    {
        foreach ( Page page in Pages )
        {
            page.UpdateTexture( minFilter, magFilter, useMipMaps );
        }
    }

    private int[]? GetSplits( Pixmap raster )
    {
        var startX = GetSplitPoint( raster, 1, 0, true, true );
        var endX   = GetSplitPoint( raster, startX, 0, false, true );
        var startY = GetSplitPoint( raster, 0, 1, true, false );
        var endY   = GetSplitPoint( raster, 0, startY, false, false );

        // Ensure pixels after the end are not invalid.
        GetSplitPoint( raster, endX + 1, 0, true, true );
        GetSplitPoint( raster, 0, endY + 1, true, false );

        // No splits, or all splits.
        if ( ( startX == 0 ) && ( endX == 0 ) && ( startY == 0 ) && ( endY == 0 ) )
        {
            return null;
        }

        // Subtraction here is because the coordinates were computed
        // before the 1px border was stripped.
        if ( startX != 0 )
        {
            startX--;
            endX = raster.Width - 2 - ( endX - 1 );
        }
        else
        {
            // If no start point was ever found, we assume full stretch.
            endX = raster.Width - 2;
        }

        if ( startY != 0 )
        {
            startY--;
            endY = raster.Height - 2 - ( endY - 1 );
        }
        else
        {
            // If no start point was ever found, we assume full stretch.
            endY = raster.Height - 2;
        }

        return new[] { startX, endX, startY, endY };
    }

    private int[]? GetPads( Pixmap raster, int[]? splits )
    {
        var bottom = raster.Height - 1;
        var right  = raster.Width - 1;

        var startX = GetSplitPoint( raster, 1, bottom, true, true );
        var startY = GetSplitPoint( raster, right, 1, true, false );

        // No need to hunt for the end if a start was never found.
        var endX = 0;
        var endY = 0;

        if ( startX != 0 )
        {
            endX = GetSplitPoint( raster, startX + 1, bottom, false, true );
        }

        if ( startY != 0 )
        {
            endY = GetSplitPoint( raster, right, startY + 1, false, false );
        }

        // Ensure pixels after the end are not invalid.
        GetSplitPoint( raster, endX + 1, bottom, true, true );
        GetSplitPoint( raster, right, endY + 1, true, false );

        // No pads.
        if ( ( startX == 0 ) && ( endX == 0 ) && ( startY == 0 ) && ( endY == 0 ) )
        {
            return null;
        }

        // -2 here is because the coordinates were computed before the 1px border was stripped.
        if ( ( startX == 0 ) && ( endX == 0 ) )
        {
            startX = -1;
            endX   = -1;
        }
        else
        {
            if ( startX > 0 )
            {
                startX--;
                endX = raster.Width - 2 - ( endX - 1 );
            }
            else
            {
                // If no start point was ever found, we assume full stretch.
                endX = raster.Width - 2;
            }
        }

        if ( ( startY == 0 ) && ( endY == 0 ) )
        {
            startY = -1;
            endY   = -1;
        }
        else
        {
            if ( startY > 0 )
            {
                startY--;
                endY = raster.Height - 2 - ( endY - 1 );
            }
            else
            {
                // If no start point was ever found, we assume full stretch.
                endY = raster.Height - 2;
            }
        }

        var pads = new[] { startX, endX, startY, endY };

        if ( ( splits != null ) && splits.Equals( pads ) )
        {
            return null;
        }

        return pads;
    }

    private int GetSplitPoint( Pixmap raster, int startX, int startY, bool startPoint, bool xAxis )
    {
        var rgba = new int[ 4 ];

        var next   = xAxis ? startX : startY;
        var end    = xAxis ? raster.Width : raster.Height;
        var breakA = startPoint ? 255 : 0;

        var x = startX;
        var y = startY;

        while ( next != end )
        {
            if ( xAxis )
            {
                x = next;
            }
            else
            {
                y = next;
            }

            Color c = new();

            c.Set( ( uint )raster.GetPixel( x, y ) );

            rgba[ 0 ] = ( int )( c.R * 255 );
            rgba[ 1 ] = ( int )( c.G * 255 );
            rgba[ 2 ] = ( int )( c.B * 255 );
            rgba[ 3 ] = ( int )( c.A * 255 );

            if ( rgba[ 3 ] == breakA )
            {
                return next;
            }

            if ( !startPoint
              && ( ( rgba[ 0 ] != 0 )
                || ( rgba[ 1 ] != 0 )
                || ( rgba[ 2 ] != 0 )
                || ( rgba[ 3 ] != 255 ) ) )
            {
                Console.WriteLine( $@"{x}  {y} {rgba}" );
            }

            next++;
        }

        return 0;
    }

// ========================================================================


    public class Page
    {
        /// <summary>
        ///     Creates a new page filled with the color provided by the
        ///     <see cref="PixmapPacker.TransparentColor" />"
        /// </summary>
        public Page( PixmapPacker packer )
        {
            Image          = new Pixmap( packer.PageWidth, packer.PageHeight, packer.PageFormat );
            Image.Blending = Pixmap.BlendTypes.None;
            Image.SetColor( packer.TransparentColor );
            Image.FillWithCurrentColor();
        }

        public Dictionary< string, PixmapPackerRectangle? > Rects { get; set; } = new();

        public Pixmap         Image      { get; set; }
        public Texture?       Texture    { get; set; }
        public List< string > AddedRects { get; set; } = new();
        public bool           Dirty      { get; set; }

        /// <summary>
        ///     Creates the texture if it has not been created, else reuploads the
        ///     entire page pixmap to the texture if the pixmap has changed since
        ///     this method was last called.
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
                    new PixmapTextureData(
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
    ///     Does bin packing by inserting to the right or below previously packed
    ///     rectangles. This is good at packing arbitrarily sized images.
    /// </summary>
    public class GuillotineStrategy : IPackStrategy
    {
        public void Sort( List< Pixmap > images )
        {
        }

        /// <summary>
        ///     Returns the page the rectangle should be placed in and
        ///     modifies the specified rectangle position.
        /// </summary>
        public Page Pack( PixmapPacker packer, string? name, RectangleShape rect ) => null!;
    }

    /// <summary>
    ///     Does bin packing by inserting in rows. This is good at
    ///     packing images that have similar heights.
    /// </summary>
    public class SkylineStrategy : IPackStrategy
    {
        public void Sort( List< Pixmap > images )
        {
        }

        /// <summary>
        ///     Returns the page the rectangle should be placed in and
        ///     modifies the specified rectangle position.
        /// </summary>
        public Page Pack( PixmapPacker packer, string? name, RectangleShape rect ) => null!;
    }


    public class PixmapPackerRectangle : RectangleShape
    {
        public PixmapPackerRectangle( int x, int y, int width, int height )
            : base( x, y, width, height )
        {
            OffsetX        = 0;
            OffsetY        = 0;
            OriginalWidth  = width;
            OriginalHeight = height;
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
            OffsetX        = left;
            OffsetY        = top;
            OriginalWidth  = originalWidth;
            OriginalHeight = originalHeight;
        }

        public int[]? Splits         { get; set; }
        public int[]? Pads           { get; set; }
        public int    OffsetX        { get; set; }
        public int    OffsetY        { get; set; }
        public int    OriginalWidth  { get; set; }
        public int    OriginalHeight { get; set; }
    }

    /// <summary>
    ///     Choose the page and location for each rectangle.
    /// </summary>
    public interface IPackStrategy
    {
        void Sort( List< Pixmap > images );

        /// <summary>
        ///     Returns the page the rectangle should be placed in and
        ///     modifies the specified rectangle position.
        /// </summary>
        Page Pack( PixmapPacker packer, string? name, RectangleShape rect );
    }
}
