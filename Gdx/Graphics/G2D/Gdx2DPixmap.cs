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

namespace LibGDXSharp.Graphics.G2D;

public class Gdx2DPixmap : IDisposable
{
    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <param name="requestedFormat"></param>
    /// <exception cref="IOException"></exception>
    public Gdx2DPixmap( byte[] encodedData, int offset, int len, int requestedFormat )
    {
        pixelPtr = Load( nativeData, encodedData, offset, len );

        if ( pixelPtr == null )
        {
            throw new IOException( $"Error loading pixmap: {GetFailureReason()}" );
        }

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
    /// </summary>
    /// <param name="inStream"></param>
    /// <param name="requestedFormat"></param>
    /// <exception cref="IOException"></exception>
    public Gdx2DPixmap( StreamReader inStream, int requestedFormat )
    {
        MemoryStream memoryStream = new( 1024 );
        StreamWriter writer       = new( memoryStream );

        int readBytes;

        while ( ( readBytes = inStream.Read() ) != -1 )
        {
            writer.Write( readBytes );
        }

        var buffer = memoryStream.ToArray();

        pixelPtr = Load( nativeData, buffer, 0, buffer.Length );

        if ( pixelPtr == null )
        {
            throw new IOException( $"Error loading pixmap: {GetFailureReason()}" );
        }

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

        basePtr     = nativeData[ 0 ];
        Width       = ( int )nativeData[ 1 ];
        Height      = ( int )nativeData[ 2 ];
        this.format = ( int )nativeData[ 3 ];
    }

    /// <summary>
    /// </summary>
    /// <param name="pixelPtr"></param>
    /// <param name="nativeData"></param>
    public Gdx2DPixmap( ByteBuffer pixelPtr, long[] nativeData )
    {
        this.pixelPtr = pixelPtr;
        basePtr       = nativeData[ 0 ];
        Width         = ( int )nativeData[ 1 ];
        Height        = ( int )nativeData[ 2 ];
        format        = ( int )nativeData[ 3 ];
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing,
    ///     releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLFormat( int format ) => format switch
                                                  {
                                                      GDX_2D_FORMAT_ALPHA           => IGL20.GL_ALPHA,
                                                      GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL20.GL_LUMINANCE_ALPHA,
                                                      GDX_2D_FORMAT_RGB888          => IGL20.GL_RGB,
                                                      GDX_2D_FORMAT_RGB565          => IGL20.GL_RGB,
                                                      GDX_2D_FORMAT_RGBA8888        => IGL20.GL_RGBA,
                                                      GDX_2D_FORMAT_RGBA4444        => IGL20.GL_RGBA,
                                                      _                             => throw new GdxRuntimeException( $"unknown format: {format}" )
                                                  };

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLType( int format ) => format switch
                                                {
                                                    GDX_2D_FORMAT_ALPHA           => IGL20.GL_UNSIGNED_BYTE,
                                                    GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL20.GL_UNSIGNED_BYTE,
                                                    GDX_2D_FORMAT_RGB888          => IGL20.GL_UNSIGNED_BYTE,
                                                    GDX_2D_FORMAT_RGBA8888        => IGL20.GL_UNSIGNED_BYTE,
                                                    GDX_2D_FORMAT_RGB565          => IGL20.GL_UNSIGNED_SHORT_5_6_5,
                                                    GDX_2D_FORMAT_RGBA4444        => IGL20.GL_UNSIGNED_SHORT_4_4_4_4,
                                                    _                             => throw new GdxRuntimeException( $"unknown format: {format}" )
                                                };

    /// <summary>
    /// </summary>
    /// <param name="requestedFormat"></param>
    private void Convert( int requestedFormat )
    {
        Gdx2DPixmap pixmap = new( Width, Height, requestedFormat );

        pixmap.Blend = GDX_2D_BLEND_NONE;
        pixmap.DrawPixmap( this, 0, 0, 0, 0, Width, Height );

        Dispose();

        basePtr    = pixmap.basePtr;
        format     = pixmap.format;
        Height     = pixmap.Height;
        nativeData = pixmap.nativeData;
        pixelPtr   = pixmap.pixelPtr;
        Width      = pixmap.Width;
    }

    public void Clear( int color ) => Clear( basePtr, color );

    public int GetPixel( int x, int y ) => GetPixel( basePtr, x, y );

    public void SetPixel( int x, int y, int color ) => SetPixel( basePtr, x, y, color );

    public void DrawLine( int x, int y, int x2, int y2, int color ) => DrawLine( basePtr, x, y, x2, y2, color );

    public void DrawRect( int x, int y, int width, int height, int color ) => DrawRect( basePtr, x, y, width, height, color );

    public void DrawCircle( int x, int y, int radius, int color ) => DrawCircle( basePtr, x, y, radius, color );

    public void FillRect( int x, int y, int width, int height, int color ) => FillRect( basePtr, x, y, width, height, color );

    public void FillCircle( int x, int y, int radius, int color ) => FillCircle( basePtr, x, y, radius, color );

    public void FillTriangle( int x1, int y1, int x2, int y2, int x3, int y3, int color ) => FillTriangle( basePtr, x1, y1, x2, y2, x3, y3, color );

    public void DrawPixmap( Gdx2DPixmap src, int srcX, int srcY, int dstX, int dstY, int width, int height )
        => DrawPixmap( src.basePtr, basePtr, srcX, srcY, width, height, dstX, dstY, width, height );

    public void DrawPixmap( Gdx2DPixmap src,
                            int srcX,
                            int srcY,
                            int srcWidth,
                            int srcHeight,
                            int dstX,
                            int dstY,
                            int dstWidth,
                            int dstHeight ) => DrawPixmap( src.basePtr, basePtr, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight );

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

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    private static string GetFormatString( int format ) => format switch
                                                           {
                                                               GDX_2D_FORMAT_ALPHA           => "Alpha",
                                                               GDX_2D_FORMAT_LUMINANCE_ALPHA => "Luminance alpha",
                                                               GDX_2D_FORMAT_RGB888          => "Rgb888",
                                                               GDX_2D_FORMAT_RGBA8888        => "Rgba8888",
                                                               GDX_2D_FORMAT_RGB565          => "Rgb565",
                                                               GDX_2D_FORMAT_RGBA4444        => "Rgba4444",
                                                               _                             => "Unknown"
                                                           };

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    // ########################################################################
    // Abstract Methods which will be implemented in Backend classes.
    // ########################################################################

    private static extern ByteBuffer Load( long[] nativeData, byte[] buffer, int offset, int len );
    private static extern ByteBuffer NewPixmap( long[] nativeData, int width, int height, int format );

    private static extern void Free( long pixmap );
    private static extern void Clear( long pixmap, int color );
    private static extern void SetPixel( long pixmap, int x, int y, int color );
    private static extern int  GetPixel( long pixmap, int x, int y );
    private static extern void DrawLine( long pixmap, int x, int y, int x2, int y2, int color );
    private static extern void DrawRect( long pixmap, int x, int y, int width, int height, int color );
    private static extern void DrawCircle( long pixmap, int x, int y, int radius, int color );
    private static extern void FillRect( long pixmap, int x, int y, int width, int height, int color );
    private static extern void FillCircle( long pixmap, int x, int y, int radius, int color );
    private static extern void FillTriangle( long pixmap, int x1, int y1, int x2, int y2, int x3, int y3, int color );

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

    private static extern void SetBlend( long src, int blend );
    private static extern void SetScale( long src, int scale );

    private static extern string GetFailureReason();

    // ------------------------------------------------------------------------

    #region constants

    public const int GDX_2D_FORMAT_ALPHA           = 1;
    public const int GDX_2D_FORMAT_LUMINANCE_ALPHA = 2;
    public const int GDX_2D_FORMAT_RGB888          = 3;
    public const int GDX_2D_FORMAT_RGBA8888        = 4;
    public const int GDX_2D_FORMAT_RGB565          = 5;
    public const int GDX_2D_FORMAT_RGBA4444        = 6;
    public const int GDX_2D_SCALE_NEAREST          = 0;
    public const int GDX_2D_SCALE_LINEAR           = 1;
    public const int GDX_2D_BLEND_NONE             = 0;
    public const int GDX_2D_BLEND_SRC_OVER         = 1;

    #endregion constants

    // ------------------------------------------------------------------------

    #region properties

    public int Width  { get; set; }
    public int Height { get; set; }

    public int    GLInternalFormat => ToGLFormat( format );
    public int    GLFormat         => ToGLFormat( format );
    public int    GLType           => ToGLType( format );
    public string FormatString     => GetFormatString( format );

    public int Blend
    {
        set => SetBlend( basePtr, value );
    }

    public int Scale
    {
        set => SetScale( basePtr, value );
    }

    #endregion properties

    // ------------------------------------------------------------------------

    #region data

    public long       basePtr;                    // 
    public int        format;                     // The actual pixmap format.
    public ByteBuffer pixelPtr;                   //
    public long[]     nativeData = new long[ 4 ]; //

    #endregion data
}
