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

using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.GLUtils;

[PublicAPI]
public class ETC1TextureData : ITextureData
{
    /// <inheritdoc />
    public int Width { get; set; }

    /// <inheritdoc />
    public int Height { get; set; }

    /// <inheritdoc />
    public bool UseMipMaps { get; set; } = false;

    /// <inheritdoc />
    public bool IsPrepared { get; set; }

    // ========================================================================

    private readonly FileInfo?      _file;
    private          ETC1.ETC1Data? _data;
    private          ETC1           _etc1;

    // ========================================================================

    public ETC1TextureData( FileInfo file, bool useMipMaps = false )
    {
        _file      = file;
        _etc1      = new ETC1();
        UseMipMaps = useMipMaps;
    }

    public ETC1TextureData( ETC1.ETC1Data encodedImage, bool useMipMaps )
    {
        _data      = encodedImage;
        _etc1      = new ETC1();
        UseMipMaps = useMipMaps;
    }

    /// <inheritdoc />
    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;

    /// <inheritdoc />
    public void Prepare()
    {
        if ( IsPrepared )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }

        if ( ( _file == null ) && ( _data == null ) )
        {
            throw new GdxRuntimeException( "Can only load once from ETC1Data" );
        }

        if ( _file != null )
        {
            _data = new ETC1.ETC1Data( _file, _etc1 );
        }

        if ( _data == null )
        {
            throw new GdxRuntimeException( "No data to prepare!" );
        }

        Width      = _data.Width;
        Height     = _data.Height;
        IsPrepared = true;
    }

    /// <inheritdoc />
    public Pixmap ConsumePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    /// <inheritdoc />
    public bool ShouldDisposePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    /// <inheritdoc />
    public unsafe void ConsumeCustomData( int target )
    {
        if ( !IsPrepared )
        {
            throw new GdxRuntimeException( "Call prepare() before calling consumeCompressedData()" );
        }

        if ( _data is null )
        {
            throw new GdxRuntimeException( "No data to consume!" );
        }

        if ( !Gdx.Graphics.SupportsExtension( "GL_OES_compressed_ETC1_RGB8_texture" ) )
        {
            var pixmap = _etc1.DecodeImage( _data, Pixmap.ColorFormat.RGB565 );

            fixed ( void* ptr = &pixmap.PixelData[ 0 ] )
            {
                Gdx.GL.glTexImage2D( target,
                                     0,
                                     pixmap.GLInternalFormat,
                                     pixmap.Width,
                                     pixmap.Height,
                                     0,
                                     pixmap.GLFormat,
                                     pixmap.GLType,
                                     ptr );
            }

            if ( UseMipMaps )
            {
                MipMapGenerator.GenerateMipMap( target, pixmap, pixmap.Width, pixmap.Height );
            }

            pixmap.Dispose();
            UseMipMaps = false;
        }
        else
        {
            fixed ( void* ptr = &_data.CompressedData.BackingArray()[ 0 ] )
            {
                Gdx.GL.glCompressedTexImage2D( target,
                                               0,
                                               ETC1.ETC1_RGB8_OES,
                                               Width,
                                               Height,
                                               0,
                                               _data.CompressedData.Capacity - _data.DataOffset,
                                               ptr );
            }

            if ( UseMipMaps )
            {
                Gdx.GL.glGenerateMipmap( IGL.GL_TEXTURE_2D );
            }
        }

        _data.Dispose();

        _data      = null;
        IsPrepared = false;
    }

    /// <inheritdoc />
    public Pixmap.ColorFormat Format { get; set; } = Pixmap.ColorFormat.Alpha;

    /// <inheritdoc />
    public bool IsManaged
    {
        get => false;
        set { }
    }
}