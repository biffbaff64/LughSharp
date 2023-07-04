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

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class PixmapTextureData : ITextureData
{
    public Pixmap        Pixmap        { get; set; }
    public Pixmap.Format Format        { get; set; }
    public bool          UseMipMaps    { get; set; }
    public bool          DisposePixmap { get; set; }
    public bool          Managed       { get; set; }

    public PixmapTextureData( Pixmap pixmap,
                              Pixmap.Format? format,
                              bool useMipMaps,
                              bool disposePixmap,
                              bool managed = false )
    {
        this.Pixmap        = pixmap;
        this.UseMipMaps    = useMipMaps;
        this.DisposePixmap = disposePixmap;
        this.Managed       = managed;
        this.Format        = format ?? pixmap.GetFormat();
    }

    public Pixmap ConsumePixmap() => Pixmap;

    public int Width
    {
        get => Pixmap.Width;
        set { }
    }

    public int Height
    {
        get => Pixmap.Height;
        set { }
    }

    public bool IsPrepared
    {
        get => true;
        set { }
    }

    public ITextureData.TextureType TextureDataType => ITextureData.TextureDataType.Pixmap;

    /// <returns>
    /// whether the caller of <see cref="ITextureData.ConsumePixmap"/> should dispose the
    /// Pixmap returned by <see cref="ITextureData.ConsumePixmap"/>
    /// </returns>
    bool ITextureData.DisposePixmap() => DisposePixmap;

    /// <returns> the <see cref="Pixmap.Format"/> of the pixel data </returns>
    public Pixmap.Format GetFormat() => Format;

    /// <returns> whether to generate mipmaps or not. </returns>
    bool ITextureData.UseMipMaps() => UseMipMaps;

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged() => Managed;

    public void ConsumeCustomData( int target )
    {
        throw new GdxRuntimeException( "This TextureData implementation does not upload data itself" );
    }

    public void Prepare()
    {
        throw new GdxRuntimeException
            ( "prepare() must not be called on a PixmapTextureData instance as it is already prepared." );
    }
}