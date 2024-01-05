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

/// <summary>
///     A <see cref="ITextureData" /> implementation which should be used to create
///     gl only textures. This TextureData fits perfectly for <see cref="FrameBuffer" />s.
///     The data is not managed.
/// </summary>
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

    public void ConsumeCustomData( int target ) => Gdx.GL.GLTexImage2D( target, MipLevel, InternalFormat, Width, Height, 0, Format, Type, null! );

    public Pixmap ConsumePixmap() => throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );

    public bool DisposePixmap() => throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );

    public Pixmap.Format GetFormat() => Pixmap.Format.RGBA8888;

    public bool UseMipMaps { get; set; }

    public bool IsManaged() => false;
}
