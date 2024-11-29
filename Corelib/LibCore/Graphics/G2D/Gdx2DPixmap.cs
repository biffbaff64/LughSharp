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

using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Buffers;
using Corelib.LibCore.Utils.Exceptions;

using StbiSharp;

namespace Corelib.LibCore.Graphics.G2D;

// ============================================================================
// ============================================================================
// ============================================================================

/// <summary>
/// Simple pixmap struct holding the pixel data, the dimensions and the
/// format of the pixmap.
/// The <see cref="Format"/> is one of the GDX_2D_FORMAT_XXX constants.
/// </summary>
[PublicAPI, StructLayout( LayoutKind.Sequential )]
public class PixmapDataType
{
    public uint   Width  { get; set; }
    public uint   Height { get; set; }
    public uint   Format { get; set; }
    public uint   Blend  { get; set; }
    public uint   Scale  { get; set; }
    public byte[] Pixels { get; set; } = [ ];
}

// ========================================================================
// ========================================================================

/// <summary>
/// 
/// </summary>
[PublicAPI]
public partial class Gdx2DPixmap : IDisposable
{
    // ========================================================================
    // ========================================================================

    public ByteBuffer PixmapBuffer { get; set; }
    public uint       Width        { get; set; }
    public uint       Height       { get; set; }
    public uint       Format       { get; set; }
    public uint       Blend        { get; set; }
    public uint       Scale        { get; set; }

    // ========================================================================

    private PixmapDataType _pixmapDataType;

    // ========================================================================
    // ========================================================================

    #region constructors

    /// <summary>
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <param name="requestedFormat"></param>
    public Gdx2DPixmap( ByteBuffer encodedData, int offset, int len, int requestedFormat )
        : this( encodedData.BackingArray(), offset, len, requestedFormat )
    {
    }

    /// <summary>
    /// Creates a new Gdx2DPixmap instance using data from the supplied buffer.
    /// <paramref name="len"/> bytes are copied from <paramref name="buffer"/>, starting
    /// at position specified by <paramref name="offset"/>.
    /// </summary>
    /// <param name="buffer"> The source byte buffer. </param>
    /// <param name="offset"> The position in buffer to start copying data from. </param>
    /// <param name="len"> The number of bytes to copy from buffer. </param>
    /// <param name="requestedFormat"> The desired color format. </param>
    /// <exception cref="IOException"></exception>
    public Gdx2DPixmap( byte[] buffer, int offset, int len, int requestedFormat )
    {
        ( PixmapBuffer, _pixmapDataType ) = LoadPixmapDataType( buffer, offset, len );

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            ConvertFormatTo( requestedFormat );
        }

        if ( PixmapBuffer == null )
        {
            throw new GdxRuntimeException( "Failed to create PixmapDef object." );
        }
        
        this.Width  = _pixmapDataType.Width;
        this.Height = _pixmapDataType.Height;
        this.Format = _pixmapDataType.Format;
        this.Blend  = _pixmapDataType.Blend;
        this.Scale  = _pixmapDataType.Scale;
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

        ( PixmapBuffer, _pixmapDataType ) = LoadPixmapDataType( buffer, 0, buffer.Length );

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            ConvertFormatTo( requestedFormat );
        }

        if ( PixmapBuffer == null )
        {
            throw new GdxRuntimeException( "Failed to create PixmapDef object." );
        }

        this.Width  = _pixmapDataType.Width;
        this.Height = _pixmapDataType.Height;
        this.Format = _pixmapDataType.Format;
        this.Blend  = _pixmapDataType.Blend;
        this.Scale  = _pixmapDataType.Scale;
    }

    /// <summary>
    /// Creates a new Gdx2DPixmap object with the given width, height, and pixel format.
    /// </summary>
    /// <param name="width"> Width in pixels. </param>
    /// <param name="height"> Height in pixels. </param>
    /// <param name="format"> The requested GDX_2D_FORMAT_xxx color format. </param>
    /// <exception cref="GdxRuntimeException"></exception>
    public Gdx2DPixmap( int width, int height, int format )
    {
        this.Width  = ( uint )width;
        this.Height = ( uint )height;
        this.Format = ( uint )format;
        this.Blend  = PixmapFormat.DEFAULT_BLEND;
        this.Scale  = PixmapFormat.DEFAULT_SCALE;

        var length = width * height * PixmapFormat.Gdx2dBytesPerPixel( format );

        _pixmapDataType = new PixmapDataType
        {
            Width  = this.Width,
            Height = this.Height,
            Format = this.Format,
            Blend  = this.Blend,
            Scale  = this.Scale,
            Pixels = new byte[ length ],
        };

        PixmapBuffer = new HeapByteBuffer( _pixmapDataType.Pixels, 0, length );

        if ( PixmapBuffer == null )
        {
            throw new GdxRuntimeException( $"Unable to allocate memory for pixmap: "
                                           + $"{width} x {height}: {PixmapFormat.GetFormatString( format )}" );
        }
    }

    #endregion constructors

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Loads the data in the supplied byte array into a <see cref="PixmapDataType"/>
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    /// <exception cref="IOException"></exception>
    private ( ByteBuffer, PixmapDataType ) LoadPixmapDataType( byte[] buffer, int offset, int len )
    {
        var image = Stbi.LoadFromMemory( buffer, PixmapFormat.Gdx2dBytesPerPixel( ( int )Format ) );

        var pixmapDef = new PixmapDataType
        {
            Width  = ( uint )image.Width,
            Height = ( uint )image.Height,
            Format = ( uint )image.NumChannels,
            Pixels = image.Data.ToArray(),
        };

        return ( new HeapByteBuffer( pixmapDef.Pixels, 0, pixmapDef.Pixels.Length ), pixmapDef );
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLFormat( int format )
    {
        return format switch
        {
            PixmapFormat.GDX_2D_FORMAT_ALPHA           => IGL.GL_ALPHA,
            PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL.GL_LUMINANCE_ALPHA,
            PixmapFormat.GDX_2D_FORMAT_RGB888          => IGL.GL_RGB,
            PixmapFormat.GDX_2D_FORMAT_RGB565          => IGL.GL_RGB,
            PixmapFormat.GDX_2D_FORMAT_RGBA8888        => IGL.GL_RGBA,
            PixmapFormat.GDX_2D_FORMAT_RGBA4444        => IGL.GL_RGBA,
            var _                                      => throw new GdxRuntimeException( $"unknown format: {format}" ),
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLType( int format )
    {
        return format switch
        {
            PixmapFormat.GDX_2D_FORMAT_ALPHA           => IGL.GL_UNSIGNED_BYTE,
            PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL.GL_UNSIGNED_BYTE,
            PixmapFormat.GDX_2D_FORMAT_RGB888          => IGL.GL_UNSIGNED_BYTE,
            PixmapFormat.GDX_2D_FORMAT_RGBA8888        => IGL.GL_UNSIGNED_BYTE,
            PixmapFormat.GDX_2D_FORMAT_RGB565          => IGL.GL_UNSIGNED_SHORT_5_6_5,
            PixmapFormat.GDX_2D_FORMAT_RGBA4444        => IGL.GL_UNSIGNED_SHORT_4_4_4_4,
            var _                                      => throw new GdxRuntimeException( $"unknown format: {format}" ),
        };
    }

    /// <summary>
    /// Converts this Pixmaps <see cref="Format"/> to the requested format.
    /// </summary>
    /// <param name="requestedFormat"> The new Format. </param>
    private void ConvertFormatTo( int requestedFormat )
    {
        var pixmap = new Gdx2DPixmap( ( int )Width, ( int )Height, requestedFormat );

        pixmap.SetBlend( PixmapFormat.GDX_2D_BLEND_NONE );
        pixmap.DrawPixmap( this, 0, 0, 0, 0, ( int )Width, ( int )Height );

//        Dispose();

        this.Width        = pixmap.Width;
        this.Height       = pixmap.Height;
        this.Format       = pixmap.Format;
        this.PixmapBuffer = pixmap.PixmapBuffer;
    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="inStream"></param>
//    /// <param name="requestedFormat"></param>
//    /// <returns></returns>
//    public static Gdx2DPixmap NewPixmap( StreamReader inStream, int requestedFormat )
//    {
//        Logger.CheckPoint();
//
//        try
//        {
//            return new Gdx2DPixmap( inStream, requestedFormat );
//        }
//        catch ( IOException e )
//        {
//            throw new GdxRuntimeException( e.Message );
//        }
//    }
//
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="width"></param>
//    /// <param name="height"></param>
//    /// <param name="format"></param>
//    /// <returns></returns>
//    /// <exception cref="GdxRuntimeException"></exception>
//    public static Gdx2DPixmap NewPixmap( int width, int height, int format )
//    {
//        Logger.CheckPoint();
//
//        try
//        {
//            return new Gdx2DPixmap( width, height, format );
//        }
//        catch ( ArgumentException e )
//        {
//            throw new GdxRuntimeException( e.Message );
//        }
//    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Sets this pixmaps blending value.
    /// </summary>
    /// <param name="blend"></param>
    public void SetBlend( int blend )
    {
        this.Blend = ( uint )blend;
    }

    /// <summary>
    /// Sets this pixmaps scaling value.
    /// </summary>
    /// <param name="scale"></param>
    public void SetScale( int scale )
    {
        this.Scale = ( uint )scale;
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// Centralise all logic related to releasing unmanaged resources.
    /// </summary>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    // ========================================================================
    // ========================================================================
}