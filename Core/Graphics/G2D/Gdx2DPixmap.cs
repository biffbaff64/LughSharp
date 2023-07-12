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

using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.G2D;

// -------------------------------------------------------------------
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassWithVirtualMembersNeverInherited.Global" )]
// -------------------------------------------------------------------
public class Gdx2DPixmap : IDisposable
{
    public const int Gdx2D_Format_Alpha           = 1;
    public const int Gdx2D_Format_Luminance_Alpha = 2;
    public const int Gdx2D_Format_RGB888          = 3;
    public const int Gdx2D_Format_RGBA8888        = 4;
    public const int Gdx2D_Format_RGB565          = 5;
    public const int Gdx2D_Format_RGBA4444        = 6;

    public const int Gdx2D_Scale_Nearest = 0;
    public const int Gdx2D_Scale_Linear  = 1;

    public const int Gdx2D_Blend_None     = 0;
    public const int Gdx2D_Blend_Src_Over = 1;

    public long       basePtr;
    public int        format;
    public ByteBuffer pixelPtr;
    public long[]     nativeData = new long[ 4 ];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <param name="requestedFormat"></param>
    /// <exception cref="IOException"></exception>
    public Gdx2DPixmap( byte[] encodedData, int offset, int len, int requestedFormat )
    {
        pixelPtr = Load( nativeData, encodedData, offset, len );

        if ( pixelPtr == null ) throw new IOException( $"Error loading pixmap: {GetFailureReason()}" );

        basePtr = nativeData[ 0 ];
        Width   = ( int )nativeData[ 1 ];
        Height  = ( int )nativeData[ 2 ];
        format  = ( int )nativeData[ 3 ];

        if ( ( requestedFormat != 0 ) && ( requestedFormat != format ) )
        {
            Convert( requestedFormat );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inStream"></param>
    /// <param name="requestedFormat"></param>
    /// <exception cref="IOException"></exception>
    // -------------------------------------------------------------------
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    // ReSharper disable once MemberCanBePrivate.Global
    // -------------------------------------------------------------------
    public Gdx2DPixmap( StreamReader inStream, int requestedFormat )
    {
        MemoryStream memoryStream = new(1024);
        StreamWriter writer       = new(memoryStream);

        int readBytes;

        while ( ( readBytes = inStream.Read() ) != -1 )
        {
            writer.Write( readBytes );
        }

        var buffer = memoryStream.ToArray();

        pixelPtr = Load( nativeData, buffer, 0, buffer.Length );

        if ( pixelPtr == null )
            throw new IOException( $"Error loading pixmap: {GetFailureReason()}" );

        basePtr = nativeData[ 0 ];
        Width   = ( int )nativeData[ 1 ];
        Height  = ( int )nativeData[ 2 ];
        format  = ( int )nativeData[ 3 ];

        if ( ( requestedFormat != 0 ) && ( requestedFormat != format ) )
        {
            Convert( requestedFormat );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="format"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public Gdx2DPixmap( int width, int height, int format )
    {
        pixelPtr = NewPixmap( nativeData, width, height, format );

        if ( pixelPtr == null )
        {
            throw new GdxRuntimeException
                (
                 $"Unable to allocate memory for pixmap: "
                 + $"{width} x {height}: {GetFormatString( format )}"
                );
        }

        this.basePtr = nativeData[ 0 ];
        this.Width   = ( int )nativeData[ 1 ];
        this.Height  = ( int )nativeData[ 2 ];
        this.format  = ( int )nativeData[ 3 ];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pixelPtr"></param>
    /// <param name="nativeData"></param>
    public Gdx2DPixmap( ByteBuffer pixelPtr, long[] nativeData )
    {
        this.pixelPtr = pixelPtr;
        this.basePtr  = nativeData[ 0 ];
        this.Width    = ( int )nativeData[ 1 ];
        this.Height   = ( int )nativeData[ 2 ];
        this.format   = ( int )nativeData[ 3 ];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLFormat( int format )
    {
        switch ( format )
        {
            case Gdx2D_Format_Alpha:
                return IGL20.GL_Alpha;

            case Gdx2D_Format_Luminance_Alpha:
                return IGL20.GL_Luminance_Alpha;

            case Gdx2D_Format_RGB888:
            case Gdx2D_Format_RGB565:
                return IGL20.GL_Rgb;

            case Gdx2D_Format_RGBA8888:
            case Gdx2D_Format_RGBA4444:
                return IGL20.GL_Rgba;

            default:
                throw new GdxRuntimeException( "unknown format: " + format );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLType( int format )
    {
        switch ( format )
        {
            case Gdx2D_Format_Alpha:
            case Gdx2D_Format_Luminance_Alpha:
            case Gdx2D_Format_RGB888:
            case Gdx2D_Format_RGBA8888:
                return IGL20.GL_Unsigned_Byte;

            case Gdx2D_Format_RGB565:
                return IGL20.GL_Unsigned_Short_5_6_5;

            case Gdx2D_Format_RGBA4444:
                return IGL20.GL_Unsigned_Short_4_4_4_4;

            default:
                throw new GdxRuntimeException( "unknown format: " + format );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestedFormat"></param>
    private void Convert( int requestedFormat )
    {
        Gdx2DPixmap pixmap = new(Width, Height, requestedFormat);

        pixmap.Blend = Gdx2D_Blend_None;
        pixmap.DrawPixmap( this, 0, 0, 0, 0, Width, Height );

        Dispose();

        this.basePtr    = pixmap.basePtr;
        this.format     = pixmap.format;
        this.Height     = pixmap.Height;
        this.nativeData = pixmap.nativeData;
        this.pixelPtr   = pixmap.pixelPtr;
        this.Width      = pixmap.Width;
    }

    public void Clear( int color )
    {
        Clear( basePtr, color );
    }

    public void SetPixel( int x, int y, int color )
    {
        SetPixel( basePtr, x, y, color );
    }

    public int GetPixel( int x, int y )
    {
        return GetPixel( basePtr, x, y );
    }

    public void DrawLine( int x, int y, int x2, int y2, int color )
    {
        DrawLine( basePtr, x, y, x2, y2, color );
    }

    public void DrawRect( int x, int y, int width, int height, int color )
    {
        DrawRect( basePtr, x, y, width, height, color );
    }

    public void DrawCircle( int x, int y, int radius, int color )
    {
        DrawCircle( basePtr, x, y, radius, color );
    }

    public void FillRect( int x, int y, int width, int height, int color )
    {
        FillRect( basePtr, x, y, width, height, color );
    }

    public void FillCircle( int x, int y, int radius, int color )
    {
        FillCircle( basePtr, x, y, radius, color );
    }

    public void FillTriangle( int x1, int y1, int x2, int y2, int x3, int y3, int color )
    {
        FillTriangle( basePtr, x1, y1, x2, y2, x3, y3, color );
    }

    public void DrawPixmap( Gdx2DPixmap src, int srcX, int srcY, int dstX, int dstY, int width, int height )
    {
        DrawPixmap( src.basePtr, basePtr, srcX, srcY, width, height, dstX, dstY, width, height );
    }

    public void DrawPixmap( Gdx2DPixmap src,
                            int srcX,
                            int srcY,
                            int srcWidth,
                            int srcHeight,
                            int dstX,
                            int dstY,
                            int dstWidth,
                            int dstHeight )
    {
        DrawPixmap( src.basePtr, basePtr, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight );
    }

    public int Blend
    {
        set => SetBlend( basePtr, value );
    }

    public int Scale
    {
        set => SetScale( basePtr, value );
    }

    public int Width  { get; set; }
    public int Height { get; set; }

    public static Gdx2DPixmap? NewPixmap( StreamReader inStream, int requestedFormat )
    {
        try
        {
            return new Gdx2DPixmap( inStream, requestedFormat );
        }
        catch ( IOException e )
        {
            Gdx.App.Log( "Gdx2DPixmap", e.Message );
            
            return null;
        }
    }

    public static Gdx2DPixmap? NewPixmap( int width, int height, int format )
    {
        try
        {
            return new Gdx2DPixmap( width, height, format );
        }
        catch ( ArgumentException e )
        {
            Gdx.App.Log( "Gdx2DPixmap", e.Message );

            return null;
        }
    }

    public int    GLInternalFormat => ToGLFormat( format );
    public int    GLFormat         => GLInternalFormat;
    public int    GLType           => ToGLType( format );
    public string FormatString     => GetFormatString( format );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    private static string GetFormatString( int format )
    {
        return format switch
               {
                   Gdx2D_Format_Alpha           => "Alpha",
                   Gdx2D_Format_Luminance_Alpha => "Luminance alpha",
                   Gdx2D_Format_RGB888          => "Rgb888",
                   Gdx2D_Format_RGBA8888        => "Rgba8888",
                   Gdx2D_Format_RGB565          => "Rgb565",
                   Gdx2D_Format_RGBA4444        => "Rgba4444",
                   _                            => "Unknown"
               };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    // ########################################################################
    // Abstract Methods which will be implemented in Backend classes.
    // ########################################################################

    private static extern ByteBuffer Load( long[] nativeData, byte[] buffer, int offset, int len );
    private static extern ByteBuffer NewPixmap( long[] nativeData, int width, int height, int format );
    private static extern void       Free( long pixmap );
    private static extern void       Clear( long pixmap, int color );
    private static extern void       SetPixel( long pixmap, int x, int y, int color );
    private static extern int        GetPixel( long pixmap, int x, int y );
    private static extern void       DrawLine( long pixmap, int x, int y, int x2, int y2, int color );
    private static extern void       DrawRect( long pixmap, int x, int y, int width, int height, int color );
    private static extern void       DrawCircle( long pixmap, int x, int y, int radius, int color );
    private static extern void       FillRect( long pixmap, int x, int y, int width, int height, int color );
    private static extern void       FillCircle( long pixmap, int x, int y, int radius, int color );
    private static extern void       FillTriangle( long pixmap, int x1, int y1, int x2, int y2, int x3, int y3, int color );

    private static extern void DrawPixmap( long src,
                                           long dst,
                                           int srcX,
                                           int srcY,
                                           int srcWidth,
                                           int srcHeight,
                                           int dstX,
                                           int dstY,
                                           int dstWidth,
                                           int dstHeight );

    private static extern void   SetBlend( long src, int blend );
    private static extern void   SetScale( long src, int scale );
    private static extern string GetFailureReason();
}
