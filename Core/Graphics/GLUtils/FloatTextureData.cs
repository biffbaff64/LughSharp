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

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// A <see cref="ITextureData"/> implementation which should be used
/// to create float textures.
/// </summary>
public sealed class FloatTextureData : ITextureData
{
    public FloatBuffer Buffer     { get; private set; } = null!;
    public int         Width      { get; set; }         = 0;
    public int         Height     { get; set; }         = 0;
    public bool        IsPrepared { get; set; }         = false;

    private readonly int  _internalFormat;
    private readonly int  _format;
    private readonly int  _type;
    private readonly bool _isGpuOnly;

    public FloatTextureData( int w, int h, int internalFormat, int format, int type, bool isGpuOnly )
    {
        this.Width           = w;
        this.Height          = h;
        this._internalFormat = internalFormat;
        this._format         = format;
        this._type           = type;
        this._isGpuOnly      = isGpuOnly;
    }

    public void Prepare()
    {
        if ( IsPrepared ) throw new GdxRuntimeException( "Already prepared" );

        if ( !_isGpuOnly )
        {
            var amountOfFloats = 4;

            if ( Gdx.Graphics.GetGLVersion().GLtype.Equals( GLVersion.GLType.OpenGL ) )
            {
                if ( ( _internalFormat == IGL30.GL_RGBA16F ) || ( _internalFormat == IGL30.GL_RGBA32F ) )
                    amountOfFloats = 4;

                if ( ( _internalFormat == IGL30.GL_RGB16F ) || ( _internalFormat == IGL30.GL_RGB32F ) )
                    amountOfFloats = 3;

                if ( ( _internalFormat == IGL30.GL_Rg16F ) || ( _internalFormat == IGL30.GL_Rg32F ) )
                    amountOfFloats = 2;

                if ( ( _internalFormat == IGL30.GL_R16F ) || ( _internalFormat == IGL30.GL_R32F ) )
                    amountOfFloats = 1;
            }

            this.Buffer = BufferUtils.NewFloatBuffer( Width * Height * amountOfFloats );
        }

        IsPrepared = true;
    }

    public void ConsumeCustomData( int target )
    {
        if ( ( Gdx.App.AppType == IApplication.ApplicationType.Android )
             || ( Gdx.App.AppType == IApplication.ApplicationType.IOS )
             || ( Gdx.App.AppType == IApplication.ApplicationType.WebGL ) )
        {

            if ( !Gdx.Graphics.SupportsExtension( "OES_texture_float" ) )
            {
                throw new GdxRuntimeException( "Extension OES_texture_float not supported!" );
            }

            // GLES and WebGL defines texture format by 3rd and 8th argument,
            // so to get a float texture one needs to supply GL_RGBA and GL_FLOAT there.
            Gdx.GL.GLTexImage2D
                (
                target, 0, IGL20.GL_Rgba, Width, Height, 0,
                IGL20.GL_Rgba, IGL20.GL_Float, Buffer
                );
        }
        else
        {
            if ( !Gdx.Graphics.IsGL30Available() )
            {
                if ( !Gdx.Graphics.SupportsExtension( "GL_ARB_texture_float" ) )
                {
                    throw new GdxRuntimeException( "Extension GL_ARB_texture_float not supported!" );
                }
            }

            // in desktop OpenGL the texture format is defined only by the third argument,
            // hence we need to use GL_RGBA32F there (this constant is unavailable in GLES/WebGL)
            Gdx.GL.GLTexImage2D
                (
                target, 0, _internalFormat, Width, Height,
                0, _format, IGL20.GL_Float, Buffer
                );
        }
    }

    public Pixmap ConsumePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    public bool DisposePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    public Pixmap.Format GetFormat()
    {
        throw new UnsupportedOperationException();
    }

    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;

    public bool UseMipMaps() => false;

    public bool IsManaged() => true;
}