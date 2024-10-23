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

namespace Corelib.LibCore.Graphics;

/// <summary>
/// Used by a Texture to load the pixel data. A TextureData can either return a Pixmap or
/// upload the pixel data itself. It signals it's type via getType() to the Texture that's
/// using it. The Texture will then either invoke consumePixmap() or consumeCustomData(int).
/// These are the first methods to be called by Texture. After that the Texture will invoke
/// the other methods to find out about the size of the image data, the format, whether
/// mipmaps should be generated and whether the TextureData is able to manage the pixel data
/// if the OpenGL ES context is lost. In case the TextureData implementation has the type
/// TextureData.TextureDataType.Custom, the implementation has to generate the mipmaps itself
/// if necessary. See MipMapGenerator. Before a call to either consumePixmap() or
/// consumeCustomData(int), Texture will bind the OpenGL ES texture.
/// <para>
/// Look at FileTextureData and ETC1TextureData for example implementations of this interface.
/// </para>
/// </summary>
[PublicAPI]
public interface ITextureData : IManageable
{
    public enum TextureType
    {
        Pixmap,
        Custom,
    }

    /// <summary>
    /// Returns the <see cref="TextureDataType"/>.
    /// </summary>
    public TextureType TextureDataType { get; }

    /// <summary>
    ///     <returns> whether the TextureData is prepared or not.</returns>
    /// </summary>
    public bool IsPrepared { get; set; }

    /// <summary>
    /// Returns whether to generate mipmaps or not.
    /// </summary>
    public bool UseMipMaps { get; set; }

    /// <summary>
    /// Returns the width of the pixel data.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Returns the height of the pixel data.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Returns the <see cref="Pixmap.ColorFormat"/> of the pixel data.
    /// </summary>
    public Pixmap.ColorFormat Format { get; set; }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ConsumePixmap()"/> or
    /// <see cref="ConsumeCustomData"/>. This method can be called from a non
    /// OpenGL thread and should not interact with OpenGL.
    /// </summary>
    public void Prepare();

    /// <summary>
    /// Returns the <see cref="Pixmap"/> for upload by Texture.
    /// <para>
    /// A call to <see cref="Prepare()"/> must precede a call to this method. Any
    /// internal data structures created in <see cref="Prepare()"/> should be
    /// disposed of here.
    /// </para>
    /// </summary>
    /// <returns> the pixmap.</returns>
    public Pixmap? ConsumePixmap();

    /// <summary>
    /// Returns whether the caller of <see cref="ConsumePixmap()"/> should
    /// dispose the Pixmap returned by <see cref="ConsumePixmap()"/>.
    /// </summary>
    public bool ShouldDisposePixmap();

    /// <summary>
    /// Uploads the pixel data to the OpenGL ES texture. The caller must bind an
    /// OpenGL ES texture. A call to <see cref="Prepare()"/> must preceed a call
    /// to this method.
    /// <para>
    /// Any internal data structures created in <see cref="Prepare()"/> should be
    /// disposed of here.
    /// </para>
    /// </summary>
    public void ConsumeCustomData( int target );
}
