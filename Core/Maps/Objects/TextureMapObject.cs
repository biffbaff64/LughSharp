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

using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Objects;

public class TextureMapObject : MapObject
{
    public float          X             { get; set; } = 0.0f;
    public float          Y             { get; set; } = 0.0f;
    public float          OriginX       { get; set; } = 0.0f;
    public float          OriginY       { get; set; } = 0.0f;
    public float          ScaleX        { get; set; } = 1.0f;
    public float          ScaleY        { get; set; } = 1.0f;
    public float          Rotation      { get; set; } = 0.0f;
    public TextureRegion? TextureRegion { get; set; } = null;

    /// <summary>
    /// Creates an empty texture map object
    /// </summary>
    public TextureMapObject() : this( null )
    {
    }

    /// <summary>
    /// Creates texture map object with the given region
    /// </summary>
    /// <param name="textureRegion">the <see cref="TextureRegion"/> to use.</param>
    public TextureMapObject (TextureRegion? textureRegion)
    {
        this.TextureRegion = textureRegion;
    }
}