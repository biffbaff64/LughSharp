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


namespace LughSharp.LibCore.Graphics;

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
public interface ITextureData
{
    public enum TextureType
    {
        Pixmap,
        Custom
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

    /// <summary>
    /// Returns the <see cref="Pixmap.Format"/> of the pixel data.
    /// </summary>
    public Pixmap.Format GetFormat();

    /// <summary>
    /// Returns whether this implementation can cope with a EGL context loss.
    /// </summary>
    public bool IsManaged();

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Factory class for creating instances of ITextureData based on file types.
    /// Provides static methods to instantiate the right implementation (Pixmap, ETC1, KTX).
    /// </summary>
    [PublicAPI]
    public static class TextureDataFactory
    {
        /// <summary>
        /// Loads texture data from the specified file with default format and mipmaps settings.
        /// </summary>
        /// <param name="file">The file to load texture data from.</param>
        /// <param name="useMipMaps">Specifies whether to use mipmaps.</param>
        /// <returns>The loaded texture data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the file parameter is null.</exception>
        public static ITextureData LoadFromFile( FileInfo file, bool useMipMaps = true )
        {
            ArgumentNullException.ThrowIfNull( file );

            Pixmap.Format? format = null; // Default format if not specified

            return LoadFromFile( file, format );
        }

        /// <summary>
        /// Loads texture data from the specified file with the given format and mipmaps settings.
        /// </summary>
        /// <param name="file">The file to load texture data from.</param>
        /// <param name="format">The format of the texture data.</param>
        /// <param name="useMipMaps">Specifies whether to use mipmaps.</param>
        /// <returns>The loaded texture data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the file parameter is null.</exception>
        public static ITextureData LoadFromFile( FileInfo file, Pixmap.Format? format, bool useMipMaps = true )
        {
            ArgumentNullException.ThrowIfNull( file );

            return file.Extension.ToLower() switch
            {
                ".cim"            => new FileTextureData( file, PixmapIO.ReadCIM( file ), format, useMipMaps ),
                ".etc1"           => new ETC1TextureData( file, useMipMaps ),
                ".ktx" or ".zktx" => new KtxTextureData( file, useMipMaps ),
                var _             => new FileTextureData( file, new Pixmap( file ), format, useMipMaps )
            };
        }
    }
}
