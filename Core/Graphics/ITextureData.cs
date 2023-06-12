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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics;

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
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public interface ITextureData
{
    public enum TextureDataType
    {
        Pixmap,
        Custom
    }

    /// <returns> the <see cref="TextureDataType"/> </returns>
    public TextureDataType GetType();

    /// <returns> whether the TextureData is prepared or not.</returns>
    public bool IsPrepared { get; set; }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ConsumePixmap()"/> or
    /// <see cref="ConsumeCustomData(int)"/>. This method can be called from a non
    /// OpenGL thread and should thus not interact with OpenGL. 
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
    public Pixmap ConsumePixmap();

    /// <returns>
    /// whether the caller of <see cref="ConsumePixmap()"/> should dispose the
    /// Pixmap returned by <see cref="ConsumePixmap()"/>
    /// </returns>
    public bool DisposePixmap();

    /// <summary>
    /// Uploads the pixel data to the OpenGL ES texture. The caller must bind an
    /// OpenGL ES texture. A call to <see cref="Prepare()"/> must preceed a call
    /// to this method.
    /// <para>
    /// Any internal data structures created in <see cref="Prepare()"/> should be
    /// disposed of here. 
    /// </para>
    /// </summary>
    public void ConsumeCustomData(int target);

    /// <returns> the width of the pixel data </returns>
    public int Width { get; set; }

    /// <returns> the height of the pixel data </returns>
    public int Height { get; set; }

    /// <returns> the <see cref="Pixmap.Format"/> of the pixel data </returns>
    public Pixmap.Format GetFormat();

    /// <returns> whether to generate mipmaps or not. </returns>
    public bool UseMipMaps();

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged();

    /// <summary>
    /// Provides static methods to instantiate the right implementation (Pixmap, ETC1, KTX).
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="useMipMaps"></param>
        /// <returns></returns>
        public static ITextureData? LoadFromFile( FileInfo file, bool useMipMaps )
        {
            return LoadFromFile( file, null, useMipMaps );
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="format"></param>
        /// <param name="useMipMaps"></param>
        /// <returns></returns>
        public static ITextureData? LoadFromFile( FileInfo? file, Pixmap.Format? format, bool useMipMaps )
        {
            if ( file == null ) return null;

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
                return new KTXTextureData( file, useMipMaps );
            }

            return new FileTextureData( file, new Pixmap( file ), format, useMipMaps );
        }
    }
}