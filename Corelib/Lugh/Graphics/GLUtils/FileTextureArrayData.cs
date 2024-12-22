// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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
public class FileTextureArrayData : ITextureArrayData
{
    private readonly ITextureData?[] _textureData;
    private readonly bool            _useMipMaps;
    private          int             _depth;

    public FileTextureArrayData( Pixmap.ColorFormat format, bool useMipMaps, FileInfo[] files )
    {
        Format      = format;
        _useMipMaps  = useMipMaps;
        _depth       = files.Length;
        _textureData = new ITextureData?[ files.Length ];

        for ( var i = 0; i < files.Length; i++ )
        {
            _textureData[ i ] = TextureDataFactory.LoadFromFile( files[ i ], format, useMipMaps );
        }
    }

    /// <returns> whether the TextureArrayData is prepared or not. </returns>
    public bool Prepared { get; set; }

    /// <returns> the width of this TextureArray </returns>
    public int Width => _textureData[ 0 ]!.Width;

    /// <returns> the height of this TextureArray </returns>
    public int Height => _textureData[ 0 ]!.Height;

    /// <returns> the layer count of this TextureArray </returns>
    public int Depth { get; set; }

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool Managed { get; set; }

    public Pixmap.ColorFormat Format { get; set; }

    /// <returns> the internal format of this TextureArray </returns>
    public int InternalFormat => PixmapFormat.ToGLFormat( Format );

    /// <returns> the GL type of this TextureArray </returns>
    public int GLType => PixmapFormat.ToGLType( Format );

    /// <summary>
    /// Prepares the TextureArrayData for a call to <see cref="ITextureArrayData.ConsumeTextureArrayData"/>.
    /// This method can be called from a non OpenGL thread and should thus not interact
    /// with OpenGL.
    /// </summary>
    public void Prepare()
    {
        var width  = -1;
        var height = -1;

        foreach ( var data in _textureData )
        {
            if ( data == null )
            {
                continue;
            }

            data.Prepare();

            if ( width == -1 )
            {
                width  = data.Width;
                height = data.Height;

                continue;
            }

            if ( ( width != data.Width ) || ( height != data.Height ) )
            {
                throw new GdxRuntimeException( "Error whilst preparing TextureArray:"
                                             + "TextureArray Textures must have equal dimensions." );
            }
        }

        Prepared = true;
    }

    /// <summary>
    /// Uploads the pixel data of the TextureArray layers of the TextureArray to the OpenGL
    /// ES texture. The caller must bind an OpenGL ES texture.
    /// <para></para>
    /// <para>
    /// A call to <see cref="ITextureArrayData.Prepare"/> must preceed a call to this method.
    /// </para>
    /// <para>
    /// Any internal data structures created in <see cref="ITextureArrayData.Prepare"/>
    /// should be disposed of here.
    /// </para>
    /// </summary>
    public void ConsumeTextureArrayData()
    {
        for ( var i = 0; i < _textureData.Length; i++ )
        {
            if ( _textureData[ i ]?.TextureDataType == ITextureData.TextureType.Custom )
            {
                _textureData[ i ]?.ConsumeCustomData( IGL.GL_TEXTURE_2D_ARRAY );
            }
            else
            {
                var texData       = _textureData[ i ];
                var pixmap        = texData?.ConsumePixmap();
                var disposePixmap = texData?.ShouldDisposePixmap() ?? false;

                Debug.Assert( texData != null, nameof( texData ) + " != null" );
                Debug.Assert( pixmap != null, nameof( pixmap ) + " != null" );

                if ( texData.Format != pixmap.Format )
                {
                    var temp = new Pixmap( pixmap.Width, pixmap.Height, texData.Format );

                    temp.Blending = Pixmap.BlendTypes.None;
                    temp.DrawPixmap( pixmap, 0, 0, 0, 0, pixmap.Width, pixmap.Height );

                    if ( texData.ShouldDisposePixmap() )
                    {
                        pixmap.Dispose();
                    }

                    pixmap        = temp;
                    disposePixmap = true;
                }

                unsafe
                {
                    fixed ( void* ptr = &pixmap.PixelData[ 0 ] )
                    {
                        GdxApi.Bindings.TexSubImage3D( IGL.GL_TEXTURE_2D_ARRAY,
                                                0,
                                                0,
                                                0,
                                                i,
                                                pixmap.Width,
                                                pixmap.Height,
                                                1,
                                                pixmap.GLInternalFormat,
                                                pixmap.GLType,
                                                ptr );
                    }
                }

                if ( _useMipMaps )
                {
                    GdxApi.Bindings.GenerateMipmap( IGL.GL_TEXTURE_2D_ARRAY );
                }

                if ( disposePixmap )
                {
                    pixmap.Dispose();
                }
            }
        }
    }

    public bool IsManaged
    {
        get
        {
            foreach ( var data in _textureData )
            {
                if ( data is { IsManaged: false } )
                {
                    return false;
                }
            }
            return true;
        }
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }
}
