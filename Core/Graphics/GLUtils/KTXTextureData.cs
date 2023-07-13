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
public class KTXTextureData : ITextureData, ICubemapData
{
    public KTXTextureData( FileInfo? file, bool useMipMaps )
    {
        throw new NotImplementedException();
    }

    /// <value> the <see cref="ITextureData.TextureDataType"/>
    /// </value>
    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Pixmap;

    /// <returns> whether the TextureData is prepared or not.</returns>
    public bool IsPrepared { get; set; }

    /// <summary>
    /// whether the TextureData is prepared or not.
    /// </summary>
    public bool Prepared { get; set; }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ICubemapData.ConsumeCubemapData"/>.
    /// This method can be called from a non OpenGL thread and should thus not
    /// interact with OpenGL. 
    /// </summary>
    void ICubemapData.Prepare()
    {
    }

    /// <summary>
    /// Uploads the pixel data for the 6 faces of the cube to the OpenGL ES texture.
    /// The caller must bind an OpenGL ES texture. A call to <see cref="ICubemapData.Prepare"/>
    /// must preceed a call to this method. Any internal data structures created
    /// in <see cref="ICubemapData.Prepare"/> should be disposed of here. 
    /// </summary>
    public void ConsumeCubemapData()
    {
    }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ITextureData.ConsumePixmap"/> or
    /// <see cref="ITextureData.ConsumeCustomData"/>. This method can be called from a non
    /// OpenGL thread and should thus not interact with OpenGL. 
    /// </summary>
    void ITextureData.Prepare()
    {
    }

    /// <summary>
    /// Returns the <see cref="Pixmap"/> for upload by Texture.
    /// <para>
    /// A call to <see cref="ITextureData.Prepare"/> must precede a call to this method. Any
    /// internal data structures created in <see cref="ITextureData.Prepare"/> should be
    /// disposed of here.
    /// </para>
    /// </summary>
    /// <returns> the pixmap.</returns>
    public Pixmap ConsumePixmap() => null;

    /// <returns>
    /// whether the caller of <see cref="ITextureData.ConsumePixmap"/> should dispose the
    /// Pixmap returned by <see cref="ITextureData.ConsumePixmap"/>
    /// </returns>
    public bool DisposePixmap() => false;

    /// <summary>
    /// Uploads the pixel data to the OpenGL ES texture. The caller must bind an
    /// OpenGL ES texture. A call to <see cref="ITextureData.Prepare"/> must preceed a call
    /// to this method.
    /// <para>
    /// Any internal data structures created in <see cref="ITextureData.Prepare"/> should be
    /// disposed of here. 
    /// </para>
    /// </summary>
    public void ConsumeCustomData( int target )
    {
    }

    /// <returns> the width of the pixel data </returns>
    public int Width { get; set; }

    /// <returns> the height of the pixel data </returns>
    public int Height { get; set; }

    /// <summary>
    /// Returns true if this implementation can cope with a EGL context loss.
    /// </summary>
    public bool Managed { get; set; }

    /// <returns> the <see cref="Pixmap.Format"/> of the pixel data </returns>
    public Pixmap.Format GetFormat() => Pixmap.Format.Alpha;

    /// <returns> whether to generate mipmaps or not. </returns>
    public bool UseMipMaps() => false;

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged() => false;
}