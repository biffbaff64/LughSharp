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


using LibGDXSharp.LibCore.Utils;

namespace LibGDXSharp.LibCore.Graphics.GLUtils;

public class PixmapTextureData : ITextureData
{
    public PixmapTextureData( Pixmap pixmap,
                              Pixmap.Format? format,
                              bool useMipMaps,
                              bool disposePixmap,
                              bool managed = false )
    {
        Pixmap        = pixmap;
        UseMipMaps    = useMipMaps;
        DisposePixmap = disposePixmap;
        Managed       = managed;
        Format        = format ?? pixmap.GetFormat();
    }

    public Pixmap        Pixmap        { get; set; }
    public Pixmap.Format Format        { get; set; }
    public bool          DisposePixmap { get; set; }
    public bool          Managed       { get; set; }

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

    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Pixmap;

    /// <returns>
    ///     whether the caller of <see cref="ITextureData.ConsumePixmap" /> should dispose the
    ///     Pixmap returned by <see cref="ITextureData.ConsumePixmap" />
    /// </returns>
    bool ITextureData.DisposePixmap() => DisposePixmap;

    /// <returns> the <see cref="Pixmap.Format" /> of the pixel data </returns>
    public Pixmap.Format GetFormat() => Format;

    /// <returns> whether to generate mipmaps or not. </returns>
    public bool UseMipMaps { get; set; }

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged() => Managed;

    public void ConsumeCustomData( int target ) => throw new GdxRuntimeException( "This TextureData implementation does not upload data itself" );

    public void Prepare() => throw new GdxRuntimeException
        ( "prepare() must not be called on a PixmapTextureData instance as it is already prepared." );
}
