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

namespace LibGDXSharp.Graphics.GLUtils;

[PublicAPI]
public class FileTextureArrayData : ITextureArrayData
{
    private readonly ITextureData?[] _textureData;

    private Pixmap.Format _format;
    private int           _depth;
    private bool          _useMipMaps;
    private bool          _prepared;

    public FileTextureArrayData( Pixmap.Format format, bool useMipMaps, FileInfo[] files )
    {
        this._format      = format;
        this._useMipMaps  = useMipMaps;
        this._depth       = files.Length;
        this._textureData = new ITextureData?[ files.Length ];

        for ( var i = 0; i < files.Length; i++ )
        {
            _textureData[ i ] = ITextureData.Factory.LoadFromFile( files[ i ], format, useMipMaps );
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

    /// <returns> the internal format of this TextureArray </returns>
    public int InternalFormat => _format.ToGLFormat();

    /// <returns> the GL type of this TextureArray </returns>
    public int GLType => _format.ToGLType();

    /// <summary>
    /// Prepares the TextureArrayData for a call to <see cref="ITextureArrayData.ConsumeTextureArrayData"/>.
    /// This method can be called from a non OpenGL thread and should thus not interact
    /// with OpenGL. 
    /// </summary>
    public void Prepare()
    {
        var width  = -1;
        var height = -1;

        foreach ( ITextureData? data in _textureData )
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
                throw new GdxRuntimeException
                    (
                     "Error whilst preparing TextureArray:"
                   + "TextureArray Textures must have equal dimensions."
                    );
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
                _textureData[ i ]?.ConsumeCustomData( IGL30.GL_TEXTURE_2D_ARRAY );
            }
            else
            {
                ITextureData? texData       = _textureData[ i ];
                Pixmap?       pixmap        = texData?.ConsumePixmap();
                var           disposePixmap = texData?.DisposePixmap() ?? false;

                Debug.Assert( texData != null, nameof( texData ) + " != null" );
                Debug.Assert( pixmap != null, nameof( pixmap ) + " != null" );

                if ( texData.GetFormat() != pixmap.GetFormat() )
                {
                    var temp = new Pixmap( pixmap.Width, pixmap.Height, texData.GetFormat() );

                    temp.Blending = Pixmap.BlendTypes.None;
                    temp.DrawPixmap( pixmap, 0, 0, 0, 0, pixmap.Width, pixmap.Height );

                    if ( texData.DisposePixmap() )
                    {
                        pixmap.Dispose();
                    }

                    pixmap        = temp;
                    disposePixmap = true;
                }

                Gdx.GL30.GLTexSubImage3D
                    (
                     IGL30.GL_TEXTURE_2D_ARRAY,
                     0,
                     0,
                     0,
                     i,
                     pixmap.Width,
                     pixmap.Height,
                     1,
                     pixmap.GLInternalFormat,
                     pixmap.GLType,
                     pixmap.Pixels
                    );

                if ( _useMipMaps )
                {
                    Gdx.GL20.GLGenerateMipmap( IGL30.GL_TEXTURE_2D_ARRAY );
                }

                if ( disposePixmap )
                {
                    pixmap.Dispose();
                }
            }
        }
    }

    public bool IsManaged()
    {
        foreach ( ITextureData? data in _textureData )
        {
            if ( ( data != null ) && !data.IsManaged() )
            {
                return false;
            }
        }

        return true;
    }
}
