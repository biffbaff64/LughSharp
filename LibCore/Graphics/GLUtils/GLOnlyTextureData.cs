// ///////////////////////////////////////////////////////////////////////////////
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


namespace LughSharp.LibCore.Graphics.GLUtils;

/// <summary>
///     A <see cref="ITextureData" /> implementation which should be used to create
///     gl only textures. This TextureData fits perfectly for <see cref="FrameBuffer" />s.
///     The data is not managed.
/// </summary>
[PublicAPI]
public class GLOnlyTextureData : ITextureData
{
    public GLOnlyTextureData( int width,
                              int height,
                              int mipMapLevel,
                              int internalFormat,
                              int format,
                              int type )
    {
        Width          = width;
        Height         = height;
        MipLevel       = mipMapLevel;
        InternalFormat = internalFormat;
        Format         = format;
        Type           = type;
    }

    public int  MipLevel       { get; set; } = 0;
    public int  InternalFormat { get; set; }
    public int  Format         { get; set; }
    public int  Type           { get; set; }
    public int  Width          { get; set; } = 0;
    public int  Height         { get; set; } = 0;
    public bool IsPrepared     { get; set; } = false;

    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;

    public void Prepare()
    {
        if ( IsPrepared )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }

        IsPrepared = true;
    }

    public unsafe void ConsumeCustomData( int target )
    {
        Gdx.GL.glTexImage2D( target, MipLevel, InternalFormat, Width, Height, 0, Format, Type, null! );
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
        return Pixmap.Format.RGBA8888;
    }

    public bool UseMipMaps { get; set; }

    public bool IsManaged()
    {
        return false;
    }
}
