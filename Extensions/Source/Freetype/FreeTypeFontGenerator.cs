// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using LughSharp.Lugh.Files;
using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

using JetBrains.Annotations;

using Color = LughSharp.Lugh.Graphics.Color;

namespace Extensions.Source.Freetype;

[PublicAPI]
public class FreeTypeFontGenerator : IDisposable
{
    public static readonly string DefaultChars =
        "\0ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890\"!`?'.,;:"
        + "()[]{}<>|/@\\^$€-%+=#_&~*\u0080\u0081\u0082\u0083\u0084\u0085\u0086\u0087"
        + "\u0088\u0089\u008A\u008B\u008C\u008D\u008E\u008F\u0090\u0091\u0092\u0093\u0094"
        + "\u0095\u0096\u0097\u0098\u0099\u009A\u009B\u009C\u009D\u009E\u009F\u00A0\u00A1"
        + "\u00A2\u00A3\u00A4\u00A5\u00A6\u00A7\u00A8\u00A9\u00AA\u00AB\u00AC\u00AD\u00AE"
        + "\u00AF\u00B0\u00B1\u00B2\u00B3\u00B4\u00B5\u00B6\u00B7\u00B8\u00B9\u00BA\u00BB"
        + "\u00BC\u00BD\u00BE\u00BF\u00C0\u00C1\u00C2\u00C3\u00C4\u00C5\u00C6\u00C7\u00C8"
        + "\u00C9\u00CA\u00CB\u00CC\u00CD\u00CE\u00CF\u00D0\u00D1\u00D2\u00D3\u00D4\u00D5"
        + "\u00D6\u00D7\u00D8\u00D9\u00DA\u00DB\u00DC\u00DD\u00DE\u00DF\u00E0\u00E1\u00E2"
        + "\u00E3\u00E4\u00E5\u00E6\u00E7\u00E8\u00E9\u00EA\u00EB\u00EC\u00ED\u00EE\u00EF"
        + "\u00F0\u00F1\u00F2\u00F3\u00F4\u00F5\u00F6\u00F7\u00F8\u00F9\u00FA\u00FB\u00FC"
        + "\u00FD\u00FE\u00FF";

    // A hint to scale the texture as needed, without capping it at any maximum size
    public const int NO_MAXIMUM = -1;

    // The maximum texture size allowed by generateData, when storing in a texture atlas.
    // Multiple texture pages will be created if necessary.
    // Default is 1024.
    private static int _maxTextureSize = 1024;

    private FreeType.Library _library;
    private FreeType.Face    _face;
    private string           _name;
    private bool             _bitmapped = false;
    private int              _pixelWidth;
    private int              _pixelHeight;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new generator from the given font file. If the file length could not be
    /// determined (it was 0), an extra copy of the font bytes is performed.
    /// </summary>
    /// <exception cref="GdxRuntimeException"> Thrown if loading failed. </exception>
    public FreeTypeFontGenerator( FileHandle fontFile, int faceIndex = 0 )
    {
        _name    = fontFile.NameWithoutExtension();
        _library = FreeType.InitFreeType();
        _face    = _library.NewFace( fontFile, faceIndex );

        if ( CheckForBitmapFont() ) return;

        SetPixelSizes( 0, 15 );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public BitmapFont GenerateFont( FreeTypeFontParameter parameter )
    {
        return GenerateFont( parameter, new FreeTypeBitmapFontData() );
    }

    /// <summary>
    /// Generates a new <see cref="BitmapFont"/>. The size is expressed in pixels. Throws
    /// a GdxRuntimeException if the font could not be generated. Using big sizes might
    /// cause such an exception.
    /// </summary>
    /// <param name="parameter"> configures how the font is generated </param>
    /// <param name="data"></param>
    public BitmapFont GenerateFont( FreeTypeFontParameter parameter, FreeTypeBitmapFontData data )
    {
        var updateTextureRegions = ( data.Regions == null ) && ( parameter.Packer != null );

        if ( updateTextureRegions ) data.Regions = [ ];

        GenerateData( parameter, data );

        if ( updateTextureRegions )
        {
            parameter.Packer?.UpdateTextureRegions( data.Regions!,
                                                    parameter.MinFilter,
                                                    parameter.MagFilter,
                                                    parameter.GenMipMaps );
        }

        if ( data.Regions?.Count == 0 )
        {
            throw new GdxRuntimeException( "Unable to create a font with no texture regions." );
        }

        var font = NewBitmapFont( data, data.Regions!, true );
        font.OwnsTexture = ( parameter.Packer == null );

        return font;
    }

    /// <summary>
    /// Called by generateFont to create a new <see cref="BitmapFont"/> instance. This allows
    /// injecting a customized <see cref="BitmapFont"/>, eg for a RTL font.
    /// </summary>
    protected static BitmapFont NewBitmapFont( BitmapFont.BitmapFontData data, List< TextureRegion > pageRegions, bool integer )
    {
        return new BitmapFont( data, pageRegions, integer );
    }

    /// <summary>
    /// Uses ascender and descender of font to calculate real height that makes all glyphs to
    /// fit in given pixel size.
    /// </summary>
    public int ScaleForPixelHeight( int height )
    {
        SetPixelSizes( 0, height );

        var fontMetrics = _face.GetSize().GetMetrics();
        var ascent      = FreeType.ToInt( fontMetrics.GetAscender() );
        var descent     = FreeType.ToInt( fontMetrics.GetDescender() );

        return ( height * height ) / ( ascent - descent );
    }

    /// <summary>
    /// Uses max advance, ascender and descender of font to calculate real height that makes
    /// any n glyphs to fit in given pixel width.
    /// </summary>
    /// <param name="width"> the max width to fit (in pixels) </param>
    /// <param name="numChars"> max number of characters that to fill width </param>
    public int ScaleForPixelWidth( int width, int numChars )
    {
        var fontMetrics    = _face.GetSize().GetMetrics();
        var advance        = FreeType.ToInt( fontMetrics.GetMaxAdvance() );
        var ascent         = FreeType.ToInt( fontMetrics.GetAscender() );
        var descent        = FreeType.ToInt( fontMetrics.GetDescender() );
        var unscaledHeight = ascent - descent;
        var height         = ( unscaledHeight * width ) / ( advance * numChars );

        SetPixelSizes( 0, height );

        return height;
    }

    /// <summary>
    /// Uses max advance, ascender and descender of font to calculate real height that
    /// makes any n glyphs to fit in given pixel width and height.
    /// </summary>
    /// <param name="width"> the max width to fit (in pixels) </param>
    /// <param name="height"> the max height to fit (in pixels) </param>
    /// <param name="numChars"> max number of characters that to fill width </param>
    public int ScaleToFitSquare( int width, int height, int numChars )
    {
        return Math.Min( ScaleForPixelHeight( height ), ScaleForPixelWidth( width, numChars ) );
    }

    /// <summary>
    /// Returns null if glyph was not found in the font. If there is nothing to render, for example
    /// with various space characters, then <see cref="GlyphAndBitmap.Bitmap"/> will be null.
    /// </summary>
    public GlyphAndBitmap? GenerateGlyphAndBitmap( int c, int size, bool flip )
    {
        SetPixelSizes( 0, size );

        var fontMetrics = _face.GetSize().GetMetrics();
        var baseline    = FreeType.ToInt( fontMetrics.GetAscender() );

        // Check if character exists in this font.
        // 0 means 'undefined character code'
        if ( _face.GetCharIndex( c ) == 0 )
        {
            return null;
        }

        // Try to load character
        if ( !LoadChar( c ) )
        {
            throw new GdxRuntimeException( "Unable to load character!" );
        }

        var slot = _face.GetGlyph();

        // Try to render to bitmap
        FreeType.Bitmap? bitmap;

        if ( _bitmapped )
        {
            bitmap = slot.GetBitmap();
        }
        else if ( !slot.RenderGlyph( FreeType.FT_RENDER_MODE_NORMAL ) )
        {
            bitmap = null;
        }
        else
        {
            bitmap = slot.GetBitmap();
        }

        var metrics = slot.GetMetrics();
        var glyph   = new BitmapFont.Glyph();

        if ( bitmap != null )
        {
            glyph.Width  = bitmap.GetWidth();
            glyph.Height = bitmap.GetRows();
        }
        else
        {
            glyph.Width  = 0;
            glyph.Height = 0;
        }

        glyph.Xoffset  = slot.GetBitmapLeft();
        glyph.Yoffset  = flip ? -slot.GetBitmapTop() + baseline : -( glyph.Height - slot.GetBitmapTop() ) - baseline;
        glyph.Xadvance = FreeType.ToInt( metrics.GetHoriAdvance() );
        glyph.SrcX     = 0;
        glyph.SrcY     = 0;
        glyph.ID       = c;

        var result = new GlyphAndBitmap
        {
            Glyph  = glyph,
            Bitmap = bitmap,
        };

        return result;
    }

    /// <summary>
    /// Generates a new <see cref="BitmapFont.BitmapFontData"/> instance, expert usage only.
    /// Throws a <see cref="GdxRuntimeException"/> if something went wrong.
    /// </summary>
    /// <param name="size"> the size in pixels. </param>
    public FreeTypeBitmapFontData GenerateData( int size )
    {
        var parameter = new FreeTypeFontParameter
        {
            Size = size,
        };

        return GenerateData( parameter );
    }

    public FreeTypeBitmapFontData GenerateData( FreeTypeFontParameter parameter )
    {
        return GenerateData( parameter, new FreeTypeBitmapFontData() );
    }

    /** Generates a new {@link BitmapFontData} instance, expert usage only. Throws a GdxRuntimeException if something went wrong.
     * @param parameter configures how the font is generated */
    public FreeTypeBitmapFontData GenerateData( FreeTypeFontParameter parameter, FreeTypeBitmapFontData data )
    {
        data.Name = _name + "-" + parameter.Size;
        var characters       = parameter.Characters.ToCharArray();
        var charactersLength = characters.Length;
        var incremental      = parameter.Incremental;
        var flags            = GetLoadingFlags( parameter );

        SetPixelSizes( 0, parameter.Size );

        // set general font data
        var fontMetrics = _face.GetSize().GetMetrics();
        data.Flipped    = parameter.Flip;
        data.Ascent     = FreeType.ToInt( fontMetrics.GetAscender() );
        data.Descent    = FreeType.ToInt( fontMetrics.GetDescender() );
        data.LineHeight = FreeType.ToInt( fontMetrics.GetHeight() );
        var baseLine = data.Ascent;

        // if bitmapped
        if ( _bitmapped && ( data.LineHeight == 0 ) )
        {
            for ( var c = 32; c < ( 32 + _face.GetNumGlyphs() ); c++ )
            {
                if ( LoadChar( c, flags ) )
                {
                    var lh = FreeType.ToInt( _face.GetGlyph().GetMetrics().GetHeight() );
                    data.LineHeight = ( lh > data.LineHeight ) ? lh : data.LineHeight;
                }
            }
        }

        data.LineHeight += parameter.SpaceY;

        // determine space width
        if ( LoadChar( ' ', flags ) || LoadChar( 'l', flags ) )
        {
            data.SpaceXadvance = FreeType.ToInt( _face.GetGlyph().GetMetrics().GetHoriAdvance() );
        }
        else
        {
            data.SpaceXadvance = _face.GetMaxAdvanceWidth(); // Possibly very wrong.
        }

        // determine x-height
        foreach ( var xChar in data.XChars )
        {
            if ( !LoadChar( xChar, flags ) ) continue;

            data.XHeight = FreeType.ToInt( _face.GetGlyph().GetMetrics().GetHeight() );

            break;
        }

        if ( data.XHeight == 0 )
        {
            throw new GdxRuntimeException( "No x-height character found in font" );
        }

        // determine cap height
        foreach ( var capChar in data.CapChars )
        {
            if ( !LoadChar( capChar, flags ) ) continue;

            data.CapHeight = FreeType.ToInt( _face.GetGlyph().GetMetrics().GetHeight() ) + Math.Abs( parameter.ShadowOffsetY );

            break;
        }

        if ( !_bitmapped && ( data.CapHeight == 1 ) )
        {
            throw new GdxRuntimeException( "No cap character found in font" );
        }

        data.Ascent -= data.CapHeight;
        data.Down   =  -data.LineHeight;

        if ( parameter.Flip )
        {
            data.Ascent = -data.Ascent;
            data.Down   = -data.Down;
        }

        var ownsAtlas = false;
        var packer    = parameter.Packer;

        if ( packer == null )
        {
            // Create a packer.
            int                        size;
            PixmapPacker.IPackStrategy packStrategy;

            if ( incremental )
            {
                size         = _maxTextureSize;
                packStrategy = new PixmapPacker.GuillotineStrategy();
            }
            else
            {
                var maxGlyphHeight = ( int )Math.Ceiling( data.LineHeight );
                size = MathUtils.NextPowerOfTwo( ( int )Math.Sqrt( maxGlyphHeight * maxGlyphHeight * charactersLength ) );
                if ( _maxTextureSize > 0 ) size = Math.Min( size, _maxTextureSize );
                packStrategy = new PixmapPacker.SkylineStrategy();
            }

            ownsAtlas = true;
            packer    = new PixmapPacker( size, size, Pixmap.ColorFormat.RGBA8888, 1, false, packStrategy );

            packer.TransparentColor   = ( parameter.BorderWidth > 0 ) ? parameter.BorderColor : parameter.Color;
            packer.TransparentColor.A = 0;
        }

        if ( incremental ) data.GlyphsList = new List< BitmapFont.Glyph >( charactersLength + 32 );

        FreeType.Stroker stroker = null!;

        if ( parameter.BorderWidth > 0 )
        {
            stroker = _library.CreateStroker();
            stroker.Set( ( int )( parameter.BorderWidth * 64f ),
                         parameter.BorderStraight ? FreeType.FT_STROKER_LINECAP_BUTT : FreeType.FT_STROKER_LINECAP_ROUND,
                         parameter.BorderStraight ? FreeType.FT_STROKER_LINEJOIN_MITER_FIXED : FreeType.FT_STROKER_LINEJOIN_ROUND, 0 );
        }

        // Create glyphs largest height first for best packing.
        var heights = new int[ charactersLength ];

        for ( var i = 0; i < charactersLength; i++ )
        {
            var c = characters[ i ];

            var height = LoadChar( c, flags ) ? FreeType.ToInt( _face.GetGlyph().GetMetrics().GetHeight() ) : 0;
            heights[ i ] = height;

            if ( c == '\0' )
            {
                var missingGlyph = CreateGlyph( '\0', data, parameter, stroker, baseLine, packer );

                if ( ( missingGlyph != null ) && ( missingGlyph.Width != 0 ) && ( missingGlyph.Height != 0 ) )
                {
                    data.SetGlyph( '\0', missingGlyph );
                    data.MissingGlyph = missingGlyph;
                    if ( incremental ) data.GlyphsList.Add( missingGlyph );
                }
            }
        }

        var heightsCount = heights.Length;

        while ( heightsCount > 0 )
        {
            int best = 0, maXHeight = heights[ 0 ];

            for ( var i = 1; i < heightsCount; i++ )
            {
                var height = heights[ i ];

                if ( height > maXHeight )
                {
                    maXHeight = height;
                    best      = i;
                }
            }

            var c = characters[ best ];

            if ( data.GetGlyph( c ) == null )
            {
                var glyph = CreateGlyph( c, data, parameter, stroker, baseLine, packer );

                if ( glyph != null )
                {
                    data.SetGlyph( c, glyph );
                    if ( incremental ) data.GlyphsList.Add( glyph );
                }
            }

            heightsCount--;
            heights[ best ] = heights[ heightsCount ];

            ( characters[ best ], characters[ heightsCount ] ) = ( characters[ heightsCount ], characters[ best ] );
        }

        if ( ( stroker != null ) && !incremental ) stroker.Dispose();

        if ( incremental )
        {
            data.Generator = this;
            data.Parameter = parameter;
            data.Stroker   = stroker;
            data.Packer    = packer;
        }

        // Generate kerning.
        parameter.Kerning &= _face.HasKerning();

        if ( parameter.Kerning )
        {
            for ( var i = 0; i < charactersLength; i++ )
            {
                var firstChar = characters[ i ];
                var first     = data.GetGlyph( firstChar );

                if ( first == null ) continue;

                var firstIndex = _face.GetCharIndex( firstChar );

                for ( var ii = i; ii < charactersLength; ii++ )
                {
                    var secondChar = characters[ ii ];
                    var second     = data.GetGlyph( secondChar );

                    if ( second == null ) continue;

                    var secondIndex = _face.GetCharIndex( secondChar );

                    var kerning = _face.GetKerning( firstIndex, secondIndex, 0 ); // FT_KERNING_DEFAULT (scaled then rounded).
                    if ( kerning != 0 ) first.SetKerning( secondChar, FreeType.ToInt( kerning ) );

                    kerning = _face.GetKerning( secondIndex, firstIndex, 0 ); // FT_KERNING_DEFAULT (scaled then rounded).
                    if ( kerning != 0 ) second.SetKerning( firstChar, FreeType.ToInt( kerning ) );
                }
            }
        }

        // Generate texture regions.
        if ( ownsAtlas )
        {
            data.Regions = [ ];
            packer.UpdateTextureRegions( data.Regions, parameter.MinFilter, parameter.MagFilter, parameter.GenMipMaps );
        }

        // Set space glyph.
        var spaceGlyph = data.GetGlyph( ' ' );

        if ( spaceGlyph == null )
        {
            spaceGlyph = new BitmapFont.Glyph
            {
                Xadvance = ( int )data.SpaceXadvance + parameter.SpaceX,
                ID       = ( int )' ',
            };

            data.SetGlyph( ' ', spaceGlyph );
        }

        if ( spaceGlyph.Width == 0 ) spaceGlyph.Width = ( int )( spaceGlyph.Xadvance + data.PadRight );

        return data;
    }

    protected BitmapFont.Glyph? CreateGlyph( char c,
                                             FreeTypeBitmapFontData data,
                                             FreeTypeFontParameter parameter,
                                             FreeType.Stroker stroker,
                                             float baseLine,
                                             PixmapPacker packer )
    {
        var missing = ( _face.GetCharIndex( c ) == 0 ) && ( c != 0 );

        if ( missing ) return null;

        if ( !LoadChar( c, GetLoadingFlags( parameter ) ) ) return null;

        var slot      = _face.GetGlyph();
        var mainGlyph = slot.GetGlyph();

        try
        {
            mainGlyph.ToBitmap( parameter.Mono ? FreeType.FT_RENDER_MODE_MONO : FreeType.FT_RENDER_MODE_NORMAL );
        }
        catch ( Exception )
        {
            mainGlyph.Dispose();
            Logger.Debug( $"Couldn't render char: c" );

            return null;
        }

        var mainBitmap = mainGlyph.GetBitmap();
        var mainPixmap = mainBitmap.GetPixmap( Pixmap.ColorFormat.RGBA8888, parameter.Color, parameter.Gamma );

        if ( ( mainBitmap.GetWidth() != 0 ) && ( mainBitmap.GetRows() != 0 ) )
        {
            if ( parameter.BorderWidth > 0 )
            {
                // execute stroker; this generates a glyph "extended" along the outline
                int top = mainGlyph.GetTop(), left = mainGlyph.GetLeft();

                var borderGlyph = slot.GetGlyph();

                borderGlyph.StrokeBorder( stroker, false );
                borderGlyph.ToBitmap( parameter.Mono ? FreeType.FT_RENDER_MODE_MONO : FreeType.FT_RENDER_MODE_NORMAL );
                var offsetX = left - borderGlyph.GetLeft();
                var offsetY = -( top - borderGlyph.GetTop() );

                // Render border (pixmap is bigger than main).
                var borderBitmap = borderGlyph.GetBitmap();
                var borderPixmap = borderBitmap.GetPixmap( Pixmap.ColorFormat.RGBA8888, parameter.BorderColor, parameter.BorderGamma );

                // Draw main glyph on top of border.
                for ( int i = 0, n = parameter.RenderCount; i < n; i++ )
                {
                    borderPixmap.DrawPixmap( mainPixmap, offsetX, offsetY );
                }

                mainPixmap.Dispose();
                mainGlyph.Dispose();
                mainPixmap = borderPixmap;
                mainGlyph  = borderGlyph;
            }

            if ( ( parameter.ShadowOffsetX != 0 ) || ( parameter.ShadowOffsetY != 0 ) )
            {
                var mainW         = mainPixmap.Width;
                var mainH         = mainPixmap.Height;
                var shadowOffsetX = Math.Max( parameter.ShadowOffsetX, 0 );
                var shadowOffsetY = Math.Max( parameter.ShadowOffsetY, 0 );
                var shadowW       = mainW + Math.Abs( parameter.ShadowOffsetX );
                var shadowH       = mainH + Math.Abs( parameter.ShadowOffsetY );
                var shadowPixmap  = new Pixmap( shadowW, shadowH, mainPixmap.Format );

                var shadowColor = parameter.ShadowColor;
                var a           = shadowColor.A;

                if ( a != 0 )
                {
                    byte r = ( byte )( shadowColor.R * 255 ), g = ( byte )( shadowColor.G * 255 ), b = ( byte )( shadowColor.B * 255 );

                    var mainPixels   = mainPixmap.ByteBuffer;
                    var shadowPixels = shadowPixmap.ByteBuffer;

                    for ( var y = 0; y < mainH; y++ )
                    {
                        var shadowRow = ( shadowW * ( y + shadowOffsetY ) ) + shadowOffsetX;

                        for ( var x = 0; x < mainW; x++ )
                        {
                            var mainPixel = ( ( mainW * y ) + x ) * 4;
                            var mainA     = mainPixels.Get( mainPixel + 3 );

                            if ( mainA == 0 ) continue;

                            var shadowPixel = ( shadowRow + x ) * 4;
                            shadowPixels.Put( shadowPixel, r );
                            shadowPixels.Put( shadowPixel + 1, g );
                            shadowPixels.Put( shadowPixel + 2, b );
                            shadowPixels.Put( shadowPixel + 3, ( byte )( ( mainA & 0xff ) * a ) );
                        }
                    }
                }

                // Draw main glyph (with any border) on top of shadow.
                for ( int i = 0, n = parameter.RenderCount; i < n; i++ )
                {
                    shadowPixmap.DrawPixmap( mainPixmap,
                                             Math.Max( -parameter.ShadowOffsetX, 0 ),
                                             Math.Max( -parameter.ShadowOffsetY, 0 ) );
                }

                mainPixmap.Dispose();
                mainPixmap = shadowPixmap;
            }
            else if ( parameter.BorderWidth == 0 )
            {
                // No shadow and no border, draw glyph additional times.
                for ( int i = 0, n = parameter.RenderCount - 1; i < n; i++ )
                {
                    mainPixmap.DrawPixmap( mainPixmap, 0, 0 );
                }
            }

            if ( ( parameter.PadTop > 0 ) || ( parameter.PadLeft > 0 ) || ( parameter.PadBottom > 0 ) || ( parameter.PadRight > 0 ) )
            {
                var padPixmap = new Pixmap( mainPixmap.Width + parameter.PadLeft + parameter.PadRight,
                                            mainPixmap.Height + parameter.PadTop + parameter.PadBottom,
                                            mainPixmap.Format );

                padPixmap.Blending = Pixmap.BlendTypes.None;
                padPixmap.DrawPixmap( mainPixmap, parameter.PadLeft, parameter.PadTop );
                mainPixmap.Dispose();
                mainPixmap = padPixmap;
            }
        }

        var metrics = slot.GetMetrics();
        var glyph = new BitmapFont.Glyph
        {
            ID      = c,
            Width   = mainPixmap.Width,
            Height  = mainPixmap.Height,
            Xoffset = mainGlyph.GetLeft(),
        };

        if ( parameter.Flip )
        {
            glyph.Yoffset = -mainGlyph.GetTop() + ( int )baseLine;
        }
        else
        {
            glyph.Yoffset = -( glyph.Height - mainGlyph.GetTop() ) - ( int )baseLine;
        }

        glyph.Xadvance = FreeType.ToInt( metrics.GetHoriAdvance() ) + ( int )parameter.BorderWidth + parameter.SpaceX;

        if ( _bitmapped )
        {
            mainPixmap.SetColor( Color.Clear );
            mainPixmap.FillWithCurrentColor();

            var buf = mainBitmap.GetBuffer();

            for ( var h = 0; h < glyph.Height; h++ )
            {
                var idx = h * mainBitmap.GetPitch();

                for ( var w = 0; w < ( glyph.Width + glyph.Xoffset ); w++ )
                {
                    var bit = ( buf.Get( idx + ( w / 8 ) ) >>> ( 7 - ( w % 8 ) ) ) & 1;
                    mainPixmap.DrawPixel( w, h, ( ( bit == 1 ) ? Color.White : Color.Clear ) );
                }
            }
        }

        var rect = packer.Pack( mainPixmap );
        glyph.Page = packer.Pages.Count - 1; // Glyph is always packed into the last page for now.
        glyph.SrcX = ( int )rect!.X;
        glyph.SrcY = ( int )rect.Y;

        // If a page was added, create a new texture region for the incrementally added glyph.
        if ( parameter.Incremental && ( data.Regions != null ) && ( data.Regions.Count <= glyph.Page ) )
        {
            packer.UpdateTextureRegions( data.Regions, parameter.MinFilter, parameter.MagFilter, parameter.GenMipMaps );
        }

        mainPixmap.Dispose();
        mainGlyph.Dispose();

        return glyph;
    }

    /// <summary>
    /// Check the font glyph exists for single UTF-32 code point
    /// </summary>
    public bool HasGlyph( int charCode )
    {
        // 0 stands for undefined character code
        return _face.GetCharIndex( charCode ) != 0;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return _name;
    }

    /// <summary>
    /// Cleans up all resources of the generator. Call this if you no longer use the generator.
    /// </summary>
    public void Dispose()
    {
        _face.Dispose();
        _library.Dispose();

        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// Sets the maximum size that will be used when generating texture atlases for glyphs
    /// with <see cref="GenerateData(int)"/>. The default is 1024.
    /// By specifying <see cref="NO_MAXIMUM"/>, the texture atlas will scale as needed.
    /// The power-of-two square texture size will be capped to the given <see cref="texSize"/>.
    /// It is recommended that a power-of-two value be used here.
    /// Multiple pages may be used to fit all the generated glyphs.
    /// You can query the resulting number of pages by calling <see cref="BitmapFont.GetRegions().Length"/>
    /// or <see cref="FreeTypeFontGenerator.FreeTypeBitmapFontData.Regions.Length"/>.
    /// If PixmapPacker is specified when calling generateData, this parameter is ignored.
    /// </summary>
    /// <param name="texSize"> the maximum texture size for one page of glyphs </param>
    public static void SetMaxTextureSize( int texSize )
    {
        _maxTextureSize = texSize;
    }

    /// <summary>
    /// Returns the maximum texture size that will be used by generateData() when creating
    /// a texture atlas for the glyphs.
    /// </summary>
    /// <returns> the power-of-two max texture size </returns>
    public static int GetMaxTextureSize()
    {
        return _maxTextureSize;
    }

    private static int GetLoadingFlags( FreeTypeFontParameter parameter )
    {
        var loadingFlags = FreeType.FT_LOAD_DEFAULT;

        loadingFlags |= parameter.Hinting switch
        {
            Hinting.None       => FreeType.FT_LOAD_NO_HINTING,
            Hinting.Slight     => FreeType.FT_LOAD_TARGET_LIGHT,
            Hinting.Medium     => FreeType.FT_LOAD_TARGET_NORMAL,
            Hinting.Full       => FreeType.FT_LOAD_TARGET_MONO,
            Hinting.AutoSlight => FreeType.FT_LOAD_FORCE_AUTOHINT | FreeType.FT_LOAD_TARGET_LIGHT,
            Hinting.AutoMedium => FreeType.FT_LOAD_FORCE_AUTOHINT | FreeType.FT_LOAD_TARGET_NORMAL,
            Hinting.AutoFull   => FreeType.FT_LOAD_FORCE_AUTOHINT | FreeType.FT_LOAD_TARGET_MONO,
            var _              => 0,
        };

        return loadingFlags;
    }

    private void SetPixelSizes( int width, int height )
    {
        this._pixelWidth  = width;
        this._pixelHeight = height;

        if ( !_bitmapped && !_face.SetPixelSizes( _pixelWidth, _pixelHeight ) )
        {
            throw new GdxRuntimeException( "Couldn't set size for font" );
        }
    }

    private bool LoadChar( int c, int flags = FreeType.FT_LOAD_DEFAULT | FreeType.FT_LOAD_FORCE_AUTOHINT )
    {
        return _face.LoadChar( c, flags );
    }

    private bool CheckForBitmapFont()
    {
        var faceFlags = _face.GetFaceFlags();

        if ( ( ( faceFlags & FreeType.FT_FACE_FLAG_FIXED_SIZES ) == FreeType.FT_FACE_FLAG_FIXED_SIZES )
             && ( ( faceFlags & FreeType.FT_FACE_FLAG_HORIZONTAL ) == FreeType.FT_FACE_FLAG_HORIZONTAL ) )
        {
            if ( LoadChar( 32 ) ) //TODO:
            {
                var slot = _face.GetGlyph();

                if ( slot.GetFormat() == 1651078259 ) //TODO:
                {
                    _bitmapped = true;
                }
            }
        }

        return _bitmapped;
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class GlyphAndBitmap
    {
        public BitmapFont.Glyph? Glyph  { get; set; }
        public FreeType.Bitmap?  Bitmap { get; set; }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// <see cref="BitmapFont.BitmapFontData"/> used for fonts generated via the
    /// <see cref="FreeTypeFontGenerator"/>. The texture storing the glyphs is held in
    /// memory, thus the <see cref="BitmapFont.BitmapFontData.ImagePaths"/> and
    /// <see cref="BitmapFont.BitmapFontData.FontFile"/> methods will return null.
    /// </summary>
    [PublicAPI]
    public class FreeTypeBitmapFontData : BitmapFont.BitmapFontData, IDisposable
    {
        public List< TextureRegion >?   Regions    { get; set; }
        public FreeTypeFontGenerator?   Generator  { get; set; }
        public FreeTypeFontParameter    Parameter  { get; set; } = new();
        public FreeType.Stroker?        Stroker    { get; set; }
        public PixmapPacker?            Packer     { get; set; }
        public List< BitmapFont.Glyph > GlyphsList { get; set; } = [ ];

        private bool _dirty;

        // ====================================================================

        public override BitmapFont.Glyph? GetGlyph( char ch )
        {
            var glyph = base.GetGlyph( ch );

            if ( ( glyph == null ) && ( Generator != null ) )
            {
                GdxRuntimeException.ThrowIfNull( Stroker );
                GdxRuntimeException.ThrowIfNull( Packer );
                GdxRuntimeException.ThrowIfNull( Regions );

                Generator.SetPixelSizes( 0, Parameter.Size );
                var baseline = ( ( Flipped ? -Ascent : Ascent ) + CapHeight ) / ScaleY;
                glyph = Generator.CreateGlyph( ch, this, Parameter, Stroker, baseline, Packer );

                if ( glyph == null ) return MissingGlyph;

                SetGlyphRegion( glyph, Regions[ glyph.Page ] );
                SetGlyph( ch, glyph );

                GlyphsList.Add( glyph );
                _dirty = true;

                var face = Generator._face;

                if ( Parameter.Kerning )
                {
                    var glyphIndex = face.GetCharIndex( ch );

                    for ( int i = 0, n = GlyphsList.Count; i < n; i++ )
                    {
                        var other      = GlyphsList[ i ];
                        var otherIndex = face.GetCharIndex( other.ID );

                        var kerning = face.GetKerning( glyphIndex, otherIndex, 0 );

                        if ( kerning != 0 )
                        {
                            glyph.SetKerning( other.ID, FreeType.ToInt( kerning ) );
                        }

                        kerning = face.GetKerning( otherIndex, glyphIndex, 0 );

                        if ( kerning != 0 )
                        {
                            other.SetKerning( ch, FreeType.ToInt( kerning ) );
                        }
                    }
                }
            }

            return glyph;
        }

        public override void GetGlyphs( GlyphLayout.GlyphRun? run, string str, int start, int end, BitmapFont.Glyph? lastGlyph )
        {
            if ( Packer != null )
            {
                // All glyphs added after this are packed directly to the texture.
                Packer.PackToTexture = true;
            }

            base.GetGlyphs( run, str, start, end, lastGlyph );

            if ( _dirty )
            {
                _dirty = false;

                Packer?.UpdateTextureRegions( Regions!,
                                              Parameter.MinFilter,
                                              Parameter.MagFilter,
                                              Parameter.GenMipMaps );
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if ( Stroker != null )
            {
                Stroker.Dispose();
                Stroker = null;
            }

            if ( Packer != null )
            {
                Packer.Dispose();
                Packer = null;
            }

            GC.SuppressFinalize( this );
        }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Font smoothing algorithm.
    /// </summary>
    [PublicAPI]
    public enum Hinting
    {
        /// <summary>
        /// Disable hinting. Generated glyphs will look blurry.
        /// </summary>
        None,

        /// <summary>
        /// Light hinting with fuzzy edges, but close to the original shape
        /// </summary>
        Slight,

        /// <summary>
        /// Average hinting
        /// </summary>
        Medium,

        /// <summary>
        /// Strong hinting with crisp edges at the expense of shape fidelity
        /// </summary>
        Full,

        /// <summary>
        /// Light hinting with fuzzy edges, but close to the original shape. Uses the FreeType auto-hinter.
        /// </summary>
        AutoSlight,

        /// <summary>
        /// Average hinting. Uses the FreeType auto-hinter.
        /// </summary>
        AutoMedium,

        /// <summary>
        /// Strong hinting with crisp edges at the expense of shape fidelity.
        /// Uses the FreeType auto-hinter.
        /// </summary>
        AutoFull,
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Parameter container class that helps configure how <see cref="FreeTypeBitmapFontData"/>
    /// and <see cref="BitmapFont"/> instances are generated.
    /// <para>
    /// The packer field is for advanced usage, where it is necessary to pack multiple BitmapFonts
    /// (i.e. styles, sizes, families) into a single Texture atlas. If no packer is specified, the
    /// generator will use its own PixmapPacker to pack the glyphs into a power-of-two sized texture,
    /// and the resulting <see cref="FreeTypeBitmapFontData"/> will have a valid <see cref="TextureRegion"/>
    /// which can be used to construct a new <see cref="BitmapFont"/>.
    /// </para>
    /// </summary>
    [PublicAPI]
    public class FreeTypeFontParameter
    {
        /// <summary>
        /// The size in pixels.
        /// </summary>
        public int Size { get; set; } = 16;

        /// <summary>
        /// If true, font smoothing is disabled.
        /// </summary>
        public bool Mono { get; set; }

        /// <summary>
        /// Strength of hinting 
        /// </summary>
        public Hinting Hinting { get; set; } = Hinting.AutoMedium;

        /// <summary>
        /// Foreground color (required for non-black borders) 
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Glyph gamma. Values &gt; 1 reduce antialiasing. 
        /// </summary>
        public float Gamma { get; set; } = 1.8f;

        /// <summary>
        /// Number of times to render the glyph. Useful with a shadow or border,
        /// so it doesn't show through the glyph. 
        /// </summary>
        public int RenderCount { get; set; } = 2;

        /// <summary>
        /// Border width in pixels, 0 to disable 
        /// </summary>
        public float BorderWidth { get; set; } = 0;

        /// <summary>
        /// Border color; only used if borderWidth &gt; 0 
        /// </summary>
        public Color BorderColor { get; set; } = Color.Black;

        /// <summary>
        /// true for straight (mitered), false for rounded borders 
        /// </summary>
        public bool BorderStraight { get; set; } = false;

        /// <summary>
        /// Values &lt; 1 increase the border size. 
        /// </summary>
        public float BorderGamma { get; set; } = 1.8f;

        /// <summary>
        /// Offset of text shadow on X axis in pixels, 0 to disable 
        /// </summary>
        public int ShadowOffsetX { get; set; } = 0;

        /// <summary>
        /// Offset of text shadow on Y axis in pixels, 0 to disable 
        /// </summary>
        public int ShadowOffsetY { get; set; } = 0;

        /// <summary>
        /// Shadow color; only used if shadowOffset > 0. If alpha component is 0, no shadow
        /// is drawn but characters are still offset by shadowOffset. 
        /// </summary>
        public Color ShadowColor { get; set; } = new( 0, 0, 0, 0.75f );

        /// <summary>
        /// Pixels to add to glyph spacing when text is rendered. Can be negative. 
        /// </summary>
        public int SpaceX { get; set; }

        /// <summary>
        /// Pixels to add to glyph spacing when text is rendered. Can be negative. 
        /// </summary>
        public int SpaceY { get; set; }

        /// <summary>
        /// Pixels to add to the glyph in the texture. Cannot be negative. 
        /// </summary>

        public int PadTop { get; set; }

        public int PadLeft   { get; set; }
        public int PadBottom { get; set; }
        public int PadRight  { get; set; }

        /// <summary>
        /// The characters the font should contain.
        /// If '\0' is not included then <see cref="BitmapFont.BitmapFontData.MissingGlyph"/>
        /// is not set. 
        /// </summary>
        public string Characters { get; set; } = DefaultChars;

        /// <summary>
        /// Whether the font should include kerning 
        /// </summary>
        public bool Kerning { get; set; } = true;

        /// <summary>
        /// The optional PixmapPacker to use for packing multiple fonts into a single texture.
        /// </summary>
        public PixmapPacker? Packer { get; set; } = null!;

        /// <summary>
        /// Whether to flip the font vertically 
        /// </summary>
        public bool Flip { get; set; } = false;

        /// <summary>
        /// Whether to generate mip maps for the resulting texture 
        /// </summary>
        public bool GenMipMaps { get; set; } = false;

        /// <summary>
        /// Minification filter 
        /// </summary>
        public Texture.TextureFilter MinFilter { get; set; } = Texture.TextureFilter.Nearest;

        /// <summary>
        /// Magnification filter 
        /// </summary>
        public Texture.TextureFilter MagFilter { get; set; } = Texture.TextureFilter.Nearest;

        /// <summary>
        /// When true, glyphs are rendered on the fly to the font's glyph page textures as
        /// they are needed. The FreeTypeFontGenerator must not be Disposed until the font is
        /// no longer needed. The FreeTypeBitmapFontData must be Disposed (separately from the
        /// generator) when the font is no longer needed. The FreeTypeFontParameter should not
        /// be modified after creating a font. If a PixmapPacker is not specified, the font
        /// glyph page textures will use <see cref="FreeTypeFontGenerator.GetMaxTextureSize()"/>. 
        /// </summary>
        public bool Incremental { get; set; }
    }
}