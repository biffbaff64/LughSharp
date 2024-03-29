﻿// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Utils.Buffers;

namespace LughSharp.LibCore.Graphics.GLUtils;

/// <summary>
///     A <see cref="ITextureData" /> implementation which should be used
///     to create float textures.
/// </summary>
public class FloatTextureData : ITextureData
{
    private readonly int _format;

    private readonly int  _internalFormat;
    private readonly bool _isGpuOnly;
    private readonly int  _type;

    public FloatTextureData( int w, int h, int internalFormat, int format, int type, bool isGpuOnly )
    {
        Width           = w;
        Height          = h;
        _internalFormat = internalFormat;
        _format         = format;
        _type           = type;
        _isGpuOnly      = isGpuOnly;
    }

    public FloatBuffer Buffer     { get; private set; } = null!;
    public int         Width      { get; set; }         = 0;
    public int         Height     { get; set; }         = 0;
    public bool        IsPrepared { get; set; }         = false;

    public void Prepare()
    {
        if ( IsPrepared )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }

        if ( !_isGpuOnly )
        {
            var amountOfFloats = 4;

            if ( Gdx.Graphics.GLVersion.GLtype.Equals( GLVersion.GLType.OpenGL ) )
            {
                if ( ( _internalFormat == IGL30.GL_RGBA16_F ) || ( _internalFormat == IGL30.GL_RGBA32_F ) )
                {
                    amountOfFloats = 4;
                }

                if ( ( _internalFormat == IGL30.GL_RGB16_F ) || ( _internalFormat == IGL30.GL_RGB32_F ) )
                {
                    amountOfFloats = 3;
                }

                if ( ( _internalFormat == IGL30.GL_RG16_F ) || ( _internalFormat == IGL30.GL_RG32_F ) )
                {
                    amountOfFloats = 2;
                }

                if ( ( _internalFormat == IGL30.GL_R16_F ) || ( _internalFormat == IGL30.GL_R32_F ) )
                {
                    amountOfFloats = 1;
                }
            }

            Buffer = BufferUtils.NewFloatBuffer( Width * Height * amountOfFloats );
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
            Gdx.GL.GLTexImage2D( target,
                                 0,
                                 IGL20.GL_RGBA,
                                 Width,
                                 Height,
                                 0,
                                 IGL20.GL_RGBA,
                                 IGL20.GL_FLOAT,
                                 Buffer );
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
            Gdx.GL.GLTexImage2D( target,
                                 0,
                                 _internalFormat,
                                 Width,
                                 Height,
                                 0,
                                 _format,
                                 IGL20.GL_FLOAT,
                                 Buffer );
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
        throw new NotSupportedException();
    }

    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;

    public bool UseMipMaps { get; set; }

    public bool IsManaged()
    {
        return true;
    }
}
