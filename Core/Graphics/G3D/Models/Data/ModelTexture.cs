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

namespace LibGDXSharp.Graphics.G3D.Models.Data;

[PublicAPI]
public class ModelTexture
{
    public const int USAGE_UNKNOWN      = 0;
    public const int USAGE_NONE         = 1;
    public const int USAGE_DIFFUSE      = 2;
    public const int USAGE_EMISSIVE     = 3;
    public const int USAGE_AMBIENT      = 4;
    public const int USAGE_SPECULAR     = 5;
    public const int USAGE_SHININESS    = 6;
    public const int USAGE_NORMAL       = 7;
    public const int USAGE_BUMP         = 8;
    public const int USAGE_TRANSPARENCY = 9;
    public const int USAGE_REFLECTION   = 10;

    public string?  ID            { get; set; }
    public string?  FileName      { get; set; }
    public Vector2? UVTranslation { get; set; }
    public Vector2? UVScaling     { get; set; }
    public int      Usage         { get; set; }
}
