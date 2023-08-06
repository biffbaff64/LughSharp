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

namespace LibGDXSharp.Graphics;

/// <summary>
/// Used by a <see cref="Cubemap"/> to load the pixel data. The Cubemap will
/// request the CubemapData to prepare itself through <see cref="Prepare()"/>
/// and upload its data using <see cref="ConsumeCubemapData()"/>. These are
/// the first methods to be called by Cubemap.
/// <para>
/// After that the Cubemap will invoke the other methods to find out about the
/// size of the image data, the format, whether the CubemapData is able to
/// manage the pixel data if the OpenGL ES context is lost.
/// </para>
/// <para>
/// Before a call to either <see cref="ConsumeCubemapData()"/>, Cubemap will
/// bind the OpenGL ES texture.
/// </para>
/// Look at <see cref="KtxTextureData"/> for example implementation of this
/// interface.
/// </summary>
public interface ICubemapData
{
    /// <summary>
    /// whether the TextureData is prepared or not.
    /// </summary>
    bool Prepared { get; }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ConsumeCubemapData()"/>.
    /// This method can be called from a non OpenGL thread and should thus not
    /// interact with OpenGL. 
    /// </summary>
    void Prepare();

    /// <summary>
    /// Uploads the pixel data for the 6 faces of the cube to the OpenGL ES texture.
    /// The caller must bind an OpenGL ES texture. A call to <see cref="Prepare()"/>
    /// must preceed a call to this method. Any internal data structures created
    /// in <see cref="Prepare()"/> should be disposed of here. 
    /// </summary>
    void ConsumeCubemapData();

    /// <summary>
    /// The width of the pixel data.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The height of the pixel data.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Returns true if this implementation can cope with a EGL context loss.
    /// </summary>
    bool Managed { get; }
}
