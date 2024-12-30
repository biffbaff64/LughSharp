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

using Corelib.Lugh.Files;
using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Utils.Buffers;

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

    public readonly int FtEncodingNone          = 0;
    public readonly int FtEncodingMsSymbol      = Encode( 's', 'y', 'm', 'b' );
    public readonly int FtEncodingUnicode       = Encode( 'u', 'n', 'i', 'c' );
    public readonly int FtEncodingSjis          = Encode( 's', 'j', 'i', 's' );
    public readonly int FtEncodingGb2312        = Encode( 'g', 'b', ' ', ' ' );
    public readonly int FtEncodingBig5          = Encode( 'b', 'i', 'g', '5' );
    public readonly int FtEncodingWansung       = Encode( 'w', 'a', 'n', 's' );
    public readonly int FtEncodingJohab         = Encode( 'j', 'o', 'h', 'a' );
    public readonly int FtEncodingAdobeStandard = Encode( 'A', 'D', 'O', 'B' );
    public readonly int FtEncodingAdobeExpert   = Encode( 'A', 'D', 'B', 'E' );
    public readonly int FtEncodingAdobeCustom   = Encode( 'A', 'D', 'B', 'C' );
    public readonly int FtEncodingAdobeLatin1   = Encode( 'l', 'a', 't', '1' );
    public readonly int FtEncodingOldLatin2     = Encode( 'l', 'a', 't', '2' );
    public readonly int FtEncodingAppleRoman    = Encode( 'a', 'r', 'm', 'n' );

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

    private static int Encode( char a, char b, char c, char d )
    {
        return ( a << 24 ) | ( b << 16 ) | ( c << 8 ) | d;
    }

    // ========================================================================
    // ========================================================================

//    private static extern int _getLastErrorCode();

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class Pointer( long address )
    {
        internal long Address = address;
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class Library( long address ) : Pointer( address ), IDisposable
    {
        public Dictionary< long, ByteBuffer > FontData { get; private set; } = [ ];

        public void Dispose()
        {
            throw new NotImplementedException();

//            _doneFreeType( Address );
//
//            foreach ( var buffer in FontData.Values )
//            {
//                if ( BufferUtils.IsUnsafeByteBuffer( buffer ) )
//                {
//                    BufferUtils.DisposeUnsafeByteBuffer( buffer );
//                }
//            }
//
//            GC.SuppressFinalize( this );
        }

        public Face NewFace( FileHandle fontFile, int faceIndex )
        {
            throw new NotImplementedException();

//            ByteBuffer? buffer = null;
//
//            try
//            {
//                buffer = fontFile.Map();
//            }
//            catch ( GdxRuntimeException )
//            {
//                // OK to ignore, some platforms do not support file mapping.
//            }
//
//            if ( buffer == null )
//            {
//                InputStream input = fontFile.Read();
//
//                try
//                {
//                    var fileSize = ( int )fontFile.Length();
//
//                    if ( fileSize == 0 )
//                    {
//                        // Copy to a byte[] to get the size, then copy to the buffer.
//                        byte[] data = StreamUtils.CopyStreamToByteArray( input, 1024 * 16 );
//
//                        buffer = BufferUtils.NewUnsafeByteBuffer( data.Length );
//                        BufferUtils.Copy( data, 0, buffer, data.Length );
//                    }
//                    else
//                    {
//                        // Trust the specified file size.
//                        buffer = BufferUtils.NewUnsafeByteBuffer( fileSize );
//                        StreamUtils.CopyStream( input, buffer );
//                    }
//                }
//                catch ( IOException ex )
//                {
//                    throw new GdxRuntimeException( ex );
//                }
//                finally
//                {
//                    StreamUtils.CloseQuietly( input );
//                }
//            }
//
//            return NewMemoryFace( buffer, faceIndex );
        }

        public Face NewMemoryFace( byte[] data, int dataSize, int faceIndex )
        {
            var buffer = BufferUtils.NewUnsafeByteBuffer( data.Length );
            BufferUtils.Copy( data, 0, buffer, data.Length );

            return NewMemoryFace( buffer, faceIndex );
        }

        public Face NewMemoryFace( ByteBuffer buffer, int faceIndex )
        {
            throw new NotImplementedException();

//            var face = _newMemoryFace( Address, buffer, buffer.Remaining(), faceIndex );
//
//            if ( face == 0 )
//            {
//                if ( BufferUtils.IsUnsafeByteBuffer( buffer ) )
//                {
//                    BufferUtils.DisposeUnsafeByteBuffer( buffer );
//                }
//
//                throw new GdxRuntimeException( $"Couldn't load font, FreeType error code: {_getLastErrorCode()}" );
//            }
//
//            FontData.Put( face, buffer );
//
//            return new Face( face, this );
        }

        public Stroker CreateStroker()
        {
            throw new NotImplementedException();

//            var stroker = _strokerNew( Address );
//
//            if ( stroker == 0 )
//            {
//                throw new GdxRuntimeException( $"Couldn't create FreeType stroker, FreeType error code: {_getLastErrorCode()}" );
//            }
//
//            return new Stroker( stroker );
        }

//        private static extern void _doneFreeType( long library );
//        private static extern long _newMemoryFace( long library, ByteBuffer data, int dataSize, int faceIndex );
//        private static extern long _strokerNew( long library );
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class Face( long address, Library library ) : Pointer( address )
    {
        public Library Library { get; private set; } = library;

        public Size GetSize()
        {
            throw new NotImplementedException();

//            return new Size( _getSize( Address ) );
        }

        public int GetCharIndex( int i )
        {
            throw new NotImplementedException();
        }

        public GlyphSlot GetGlyph()
        {
            throw new NotImplementedException();
        }

//        private static extern long _getSize( long face );

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

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class Size( long address ) : Pointer( address )
    {
        public SizeMetrics? Metrics;
//        {
//            get { return new( _getMetrics( base.Address ) ); }
//        }

        //        private static extern long _getMetrics( long address );

        public SizeMetrics GetMetrics()
        {
            throw new NotImplementedException();

//            return new SizeMetrics( _getMetrics( base.Address ) );
        }
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class SizeMetrics( long address ) : Pointer( address )
    {
        public int GetAscender()
        {
            throw new NotImplementedException();
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

    // ========================================================================
    // ========================================================================

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

    // ========================================================================
    // ========================================================================

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

    // ========================================================================
    // ========================================================================

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

        public int GetPitch()
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
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class GlyphMetrics( long address ) : Pointer( address )
    {
        public int GetWidth()
        {
            throw new NotImplementedException();

//            return _getWidth( Address );
        }

        public int GetHeight()
        {
            throw new NotImplementedException();

//            return _getHeight( Address );
        }

        public int GetHoriBearingX()
        {
            throw new NotImplementedException();

//            return _getHoriBearingX( Address );
        }

        public int GetHoriBearingY()
        {
            throw new NotImplementedException();

//            return _getHoriBearingY( Address );
        }

        public int GetHoriAdvance()
        {
            throw new NotImplementedException();

//            return _getHoriAdvance( Address );
        }

        public int GetVertBearingX()
        {
            throw new NotImplementedException();

//            return _getVertBearingX( Address );
        }

        public int GetVertBearingY()
        {
            throw new NotImplementedException();

//            return _getVertBearingY( Address );
        }

        public int GetVertAdvance()
        {
            throw new NotImplementedException();

//            return _getVertAdvance( Address );
        }

        // ====================================================================

//        private static extern int _getWidth( long metrics );
//        private static extern int _getHeight( long metrics );
//        private static extern int _getHoriBearingX( long metrics );
//        private static extern int _getHoriBearingY( long metrics );
//        private static extern int _getHoriAdvance( long metrics );
//        private static extern int _getVertBearingX( long metrics );
//        private static extern int _getVertBearingY( long metrics );
//        private static extern int _getVertAdvance( long metrics );
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class Stroker( long address ) : Pointer( address )
    {
        public void Set( int radius, int lineCap, int lineJoin, int miterLimit )
        {
            throw new NotImplementedException();

            //            _set( Address, radius, lineCap, lineJoin, miterLimit );
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

//        private static extern void _set( long stroker, int radius, int lineCap, int lineJoin, int miterLimit );
//        private static extern void _done( long stroker );
    }

    // ========================================================================
    // ========================================================================
}