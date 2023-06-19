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

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class FacedCubemapData : ICubemapData
{
    public FacedCubemapData (ITextureData? positiveX, ITextureData? negativeX, ITextureData? positiveY,
                             ITextureData? negativeY, ITextureData? positiveZ, ITextureData? negativeZ)
    {
    }

    /// <summary>
    /// whether the TextureData is prepared or not.
    /// </summary>
    public bool Prepared { get; set; }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ICubemapData.ConsumeCubemapData"/>.
    /// This method can be called from a non OpenGL thread and should thus not
    /// interact with OpenGL. 
    /// </summary>
    public void Prepare()
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
    /// The width of the pixel data.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height of the pixel data.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Returns true if this implementation can cope with a EGL context loss.
    /// </summary>
    public bool Managed { get; set; }
}