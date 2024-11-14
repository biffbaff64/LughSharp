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

using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Graphics.GLUtils;

[PublicAPI]
public class PixmapTextureData : ITextureData
{
    public Pixmap             Pixmap        { get; set; }
    public Pixmap.ColorFormat Format        { get; set; }
    public bool               DisposePixmap { get; set; }
    public bool               IsManaged     { get; set; }
    public bool               UseMipMaps    { get; set; }
    public bool               IsPrepared    { get; set; } = true;

    // ========================================================================

    public PixmapTextureData( Pixmap pixmap,
                              Pixmap.ColorFormat? format,
                              bool useMipMaps,
                              bool disposePixmap,
                              bool managed = false )
    {
        Logger.Checkpoint();

        Pixmap        = pixmap;
        UseMipMaps    = useMipMaps;
        DisposePixmap = disposePixmap;
        IsManaged     = managed;
        Format        = format ?? pixmap.Format;
    }

    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Pixmap;

    /// <returns>
    /// whether the caller of <see cref="ITextureData.ConsumePixmap"/> should dispose the
    /// Pixmap returned by <see cref="ITextureData.ConsumePixmap"/>
    /// </returns>
    bool ITextureData.ShouldDisposePixmap() => DisposePixmap;

    Pixmap ITextureData.ConsumePixmap() => Pixmap;

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

    // ========================================================================

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