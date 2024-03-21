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


namespace LughSharp.LibCore.Graphics;

/// <summary>
///     Used by a Texture to load the pixel data. A TextureData can either return a Pixmap or
///     upload the pixel data itself. It signals it's type via getType() to the Texture that's
///     using it. The Texture will then either invoke consumePixmap() or consumeCustomData(int).
///     These are the first methods to be called by Texture. After that the Texture will invoke
///     the other methods to find out about the size of the image data, the format, whether
///     mipmaps should be generated and whether the TextureData is able to manage the pixel data
///     if the OpenGL ES context is lost. In case the TextureData implementation has the type
///     TextureData.TextureDataType.Custom, the implementation has to generate the mipmaps itself
///     if necessary. See MipMapGenerator. Before a call to either consumePixmap() or
///     consumeCustomData(int), Texture will bind the OpenGL ES texture.
///     <para>
///         Look at FileTextureData and ETC1TextureData for example implementations of this interface.
///     </para>
/// </summary>
[PublicAPI]
public interface ITextureData
{
    public enum TextureType
    {
        Pixmap,
        Custom
    }

    /// <returns> the <see cref="TextureDataType" /> </returns>
    public TextureType TextureDataType { get; }

    /// <returns> whether the TextureData is prepared or not.</returns>
    public bool IsPrepared { get; set; }

    /// <returns> whether to generate mipmaps or not. </returns>
    public bool UseMipMaps { get; set; }

    /// <returns> the width of the pixel data </returns>
    public int Width { get; set; }

    /// <returns> the height of the pixel data </returns>
    public int Height { get; set; }

    /// <summary>
    ///     Prepares the TextureData for a call to <see cref="ConsumePixmap()" /> or
    ///     <see cref="ConsumeCustomData" />. This method can be called from a non
    ///     OpenGL thread and should thus not interact with OpenGL.
    /// </summary>
    public void Prepare();

    /// <summary>
    ///     Returns the <see cref="Pixmap" /> for upload by Texture.
    ///     <para>
    ///         A call to <see cref="Prepare()" /> must precede a call to this method. Any
    ///         internal data structures created in <see cref="Prepare()" /> should be
    ///         disposed of here.
    ///     </para>
    /// </summary>
    /// <returns> the pixmap.</returns>
    public Pixmap? ConsumePixmap();

    /// <returns>
    ///     whether the caller of <see cref="ConsumePixmap()" /> should dispose the
    ///     Pixmap returned by <see cref="ConsumePixmap()" />
    /// </returns>
    public bool DisposePixmap();

    /// <summary>
    ///     Uploads the pixel data to the OpenGL ES texture. The caller must bind an
    ///     OpenGL ES texture. A call to <see cref="Prepare()" /> must preceed a call
    ///     to this method.
    ///     <para>
    ///         Any internal data structures created in <see cref="Prepare()" /> should be
    ///         disposed of here.
    ///     </para>
    /// </summary>
    public void ConsumeCustomData( int target );

    /// <returns> the <see cref="Pixmap.Format" /> of the pixel data </returns>
    public Pixmap.Format GetFormat();

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged();

    /// <summary>
    ///     Provides static methods to instantiate the right implementation (Pixmap, ETC1, KTX).
    /// </summary>
    [PublicAPI]
    public static class Factory
    {
        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="useMipMaps"></param>
        /// <returns></returns>
        public static ITextureData? LoadFromFile( FileInfo? file, bool useMipMaps )
        {
            return LoadFromFile( file, null, useMipMaps );
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="format"></param>
        /// <param name="useMipMaps"></param>
        /// <returns></returns>
        public static ITextureData LoadFromFile( FileInfo? file, Pixmap.Format? format, bool useMipMaps )
        {
            ArgumentNullException.ThrowIfNull( file );

            if ( file.Name.EndsWith( ".cim" ) )
            {
                return new FileTextureData( file, PixmapIO.ReadCIM( file ), format, useMipMaps );
            }

            if ( file.Name.EndsWith( ".etc1" ) )
            {
                return new ETC1TextureData( file, useMipMaps );
            }

            if ( file.Name.EndsWith( ".ktx" ) || file.Name.EndsWith( ".zktx" ) )
            {
                return new KtxTextureData( file, useMipMaps );
            }

            return new FileTextureData( file, new Pixmap( file ), format, useMipMaps );
        }
    }
}
