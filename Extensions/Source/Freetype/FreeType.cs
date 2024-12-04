// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

using Corelib.LibCore.Files;
using Corelib.LibCore.Graphics;
using Corelib.LibCore.Utils.Buffers;

using JetBrains.Annotations;

namespace Extensions.Source.Freetype;

[PublicAPI]
public class FreeType
{
    public const int FT_PIXEL_MODE_NONE                  = 0;
    public const int FT_PIXEL_MODE_MONO                  = 1;
    public const int FT_PIXEL_MODE_GRAY                  = 2;
    public const int FT_PIXEL_MODE_GRAY2                 = 3;
    public const int FT_PIXEL_MODE_GRAY4                 = 4;
    public const int FT_PIXEL_MODE_LCD                   = 5;
    public const int FT_PIXEL_MODE_LCD_V                 = 6;
    public const int FT_FACE_FLAG_SCALABLE               = ( 1 << 0 );
    public const int FT_FACE_FLAG_FIXED_SIZES            = ( 1 << 1 );
    public const int FT_FACE_FLAG_FIXED_WIDTH            = ( 1 << 2 );
    public const int FT_FACE_FLAG_SFNT                   = ( 1 << 3 );
    public const int FT_FACE_FLAG_HORIZONTAL             = ( 1 << 4 );
    public const int FT_FACE_FLAG_VERTICAL               = ( 1 << 5 );
    public const int FT_FACE_FLAG_KERNING                = ( 1 << 6 );
    public const int FT_FACE_FLAG_FAST_GLYPHS            = ( 1 << 7 );
    public const int FT_FACE_FLAG_MULTIPLE_MASTERS       = ( 1 << 8 );
    public const int FT_FACE_FLAG_GLYPH_NAMES            = ( 1 << 9 );
    public const int FT_FACE_FLAG_EXTERNAL_STREAM        = ( 1 << 10 );
    public const int FT_FACE_FLAG_HINTER                 = ( 1 << 11 );
    public const int FT_FACE_FLAG_CID_KEYED              = ( 1 << 12 );
    public const int FT_FACE_FLAG_TRICKY                 = ( 1 << 13 );
    public const int FT_STYLE_FLAG_ITALIC                = ( 1 << 0 );
    public const int FT_STYLE_FLAG_BOLD                  = ( 1 << 1 );
    public const int FT_LOAD_DEFAULT                     = 0x0;
    public const int FT_LOAD_NO_SCALE                    = 0x1;
    public const int FT_LOAD_NO_HINTING                  = 0x2;
    public const int FT_LOAD_RENDER                      = 0x4;
    public const int FT_LOAD_NO_BITMAP                   = 0x8;
    public const int FT_LOAD_VERTICAL_LAYOUT             = 0x10;
    public const int FT_LOAD_FORCE_AUTOHINT              = 0x20;
    public const int FT_LOAD_CROP_BITMAP                 = 0x40;
    public const int FT_LOAD_PEDANTIC                    = 0x80;
    public const int FT_LOAD_IGNORE_GLOBAL_ADVANCE_WIDTH = 0x200;
    public const int FT_LOAD_NO_RECURSE                  = 0x400;
    public const int FT_LOAD_IGNORE_TRANSFORM            = 0x800;
    public const int FT_LOAD_MONOCHROME                  = 0x1000;
    public const int FT_LOAD_LINEAR_DESIGN               = 0x2000;
    public const int FT_LOAD_NO_AUTOHINT                 = 0x8000;
    public const int FT_LOAD_TARGET_NORMAL               = 0x0;
    public const int FT_LOAD_TARGET_LIGHT                = 0x10000;
    public const int FT_LOAD_TARGET_MONO                 = 0x20000;
    public const int FT_LOAD_TARGET_LCD                  = 0x30000;
    public const int FT_LOAD_TARGET_LCD_V                = 0x40000;
    public const int FT_RENDER_MODE_NORMAL               = 0;
    public const int FT_RENDER_MODE_LIGHT                = 1;
    public const int FT_RENDER_MODE_MONO                 = 2;
    public const int FT_RENDER_MODE_LCD                  = 3;
    public const int FT_RENDER_MODE_LCD_V                = 4;
    public const int FT_RENDER_MODE_MAX                  = 5;
    public const int FT_KERNING_DEFAULT                  = 0;
    public const int FT_KERNING_UNFITTED                 = 1;
    public const int FT_KERNING_UNSCALED                 = 2;
    public const int FT_STROKER_LINECAP_BUTT             = 0;
    public const int FT_STROKER_LINECAP_ROUND            = 1;
    public const int FT_STROKER_LINECAP_SQUARE           = 2;
    public const int FT_STROKER_LINEJOIN_ROUND           = 0;
    public const int FT_STROKER_LINEJOIN_BEVEL           = 1;
    public const int FT_STROKER_LINEJOIN_MITER_VARIABLE  = 2;
    public const int FT_STROKER_LINEJOIN_MITER           = FT_STROKER_LINEJOIN_MITER_VARIABLE;
    public const int FT_STROKER_LINEJOIN_MITER_FIXED     = 3;

    public readonly int FT_ENCODING_NONE           = 0;
    public readonly int FT_ENCODING_MS_SYMBOL      = encode( 's', 'y', 'm', 'b' );
    public readonly int FT_ENCODING_UNICODE        = encode( 'u', 'n', 'i', 'c' );
    public readonly int FT_ENCODING_SJIS           = encode( 's', 'j', 'i', 's' );
    public readonly int FT_ENCODING_GB2312         = encode( 'g', 'b', ' ', ' ' );
    public readonly int FT_ENCODING_BIG5           = encode( 'b', 'i', 'g', '5' );
    public readonly int FT_ENCODING_WANSUNG        = encode( 'w', 'a', 'n', 's' );
    public readonly int FT_ENCODING_JOHAB          = encode( 'j', 'o', 'h', 'a' );
    public readonly int FT_ENCODING_ADOBE_STANDARD = encode( 'A', 'D', 'O', 'B' );
    public readonly int FT_ENCODING_ADOBE_EXPERT   = encode( 'A', 'D', 'B', 'E' );
    public readonly int FT_ENCODING_ADOBE_CUSTOM   = encode( 'A', 'D', 'B', 'C' );
    public readonly int FT_ENCODING_ADOBE_LATIN_1  = encode( 'l', 'a', 't', '1' );
    public readonly int FT_ENCODING_OLD_LATIN_2    = encode( 'l', 'a', 't', '2' );
    public readonly int FT_ENCODING_APPLE_ROMAN    = encode( 'a', 'r', 'm', 'n' );

    // ========================================================================
    // ========================================================================

    public static Library InitFreeType()
    {
        throw new NotImplementedException();
    }

    public static int ToInt( int value )
    {
        return ( ( value + 63 ) & -64 ) >> 6;
    }

    // ========================================================================
    // ========================================================================

    private static int encode( char a, char b, char c, char d )
    {
        return ( a << 24 ) | ( b << 16 ) | ( c << 8 ) | d;
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class Pointer( long address )
    {
        internal long Address = address;
    }

    [PublicAPI]
    public class Library( long address ) : Pointer( address ), IDisposable
    {
        public Dictionary< long, ByteBuffer > FontData { get; private set; } = [ ];

        public void Dispose()
        {
            GC.SuppressFinalize( this );
        }

        public Face NewFace( FileHandle fontFile, int faceIndex )
        {
            throw new NotImplementedException();
        }

        public Stroker CreateStroker()
        {
            throw new NotImplementedException();
        }
    }

    [PublicAPI]
    public class Face( long address, Library library ) : Pointer( address )
    {
        public Library Library { get; private set; } = library;

        public Size GetSize()
        {
            return new Size( _getSize( Address ) );
        }

        public int GetCharIndex( int i )
        {
            throw new NotImplementedException();
        }

        public GlyphSlot GetGlyph()
        {
            throw new NotImplementedException();
        }

        private static extern long _getSize( long face );

        public bool SetPixelSizes( int pixelWidth, int pixelHeight )
        {
            throw new NotImplementedException();
        }

        public float GetMaxAdvanceWidth()
        {
            throw new NotImplementedException();
        }

        public int GetNumGlyphs()
        {
            throw new NotImplementedException();
        }

        public int GetKerning( int otherIndex, int glyphIndex, int p2 )
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool HasKerning()
        {
            throw new NotImplementedException();
        }

        public bool LoadChar( int i, int flags )
        {
            throw new NotImplementedException();
        }

        public int GetFaceFlags()
        {
            throw new NotImplementedException();
        }
    }

    [PublicAPI]
    public class Size( long address ) : Pointer( address )
    {
        public SizeMetrics Metrics => new( _getMetrics( base.Address ) );

        private static extern long _getMetrics( long address );

        public SizeMetrics GetMetrics()
        {
            return new SizeMetrics( _getMetrics( base.Address ) );
        }
    }

    [PublicAPI]
    public class SizeMetrics( long address ) : Pointer( address )
    {
        public int GetAscender()
        {
            return 0;
        }

        public int GetDescender()
        {
            throw new NotImplementedException();
        }

        public int GetHeight()
        {
            throw new NotImplementedException();
        }

        public int GetMaxAdvance()
        {
            throw new NotImplementedException();
        }
    }

    [PublicAPI]
    public class GlyphSlot( long address ) : Pointer( address )
    {
        public Bitmap GetBitmap()
        {
            throw new NotImplementedException();
        }

        public bool RenderGlyph( int ftRenderModeNormal )
        {
            throw new NotImplementedException();
        }

        public GlyphMetrics GetMetrics()
        {
            throw new NotImplementedException();
        }

        public int GetBitmapLeft()
        {
            throw new NotImplementedException();
        }

        public int GetBitmapTop()
        {
            throw new NotImplementedException();
        }

        public Glyph GetGlyph()
        {
            throw new NotImplementedException();
        }

        public int GetFormat()
        {
            throw new NotImplementedException();
        }
    }

    [PublicAPI]
    public class Glyph( long address ) : Pointer( address )
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ToBitmap( int ftRenderModeNormal )
        {
            throw new NotImplementedException();
        }

        public FreeType.Bitmap GetBitmap()
        {
            throw new NotImplementedException();
        }

        public int GetTop()
        {
            throw new NotImplementedException();
        }

        public int GetLeft()
        {
            throw new NotImplementedException();
        }

        public void StrokeBorder( Stroker stroker, bool b )
        {
            throw new NotImplementedException();
        }
    }

    [PublicAPI]
    public class Bitmap( long address ) : Pointer( address )
    {
        public int GetWidth()
        {
            throw new NotImplementedException();
        }
        
        public int GetRows()
        {
            throw new NotImplementedException();
        }

        public Pixmap GetPixmap( Pixmap.ColorFormat rgba8888, Color parameterColor, float parameterGamma )
        {
            throw new NotImplementedException();
        }

        public ByteBuffer GetBuffer()
        {
            throw new NotImplementedException();
        }

        public int GetPitch()
        {
            throw new NotImplementedException();
        }
    }

    [PublicAPI]
    public class GlyphMetrics( long address ) : Pointer( address )
    {
        public int GetHoriAdvance()
        {
            throw new NotImplementedException();
        }

        public int GetHeight()
        {
            throw new NotImplementedException();
        }
    }

    [PublicAPI]
    public class Stroker( long address ) : Pointer( address )
    {
        public void Set( int parameterBorderWidth, int ftStrokerLinecapRound, int ftStrokerLinejoinMiterFixed, int i )
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}