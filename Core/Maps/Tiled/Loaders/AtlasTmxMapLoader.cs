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

namespace LibGDXSharp.Maps.Tiled;

/// <summary>
/// A TiledMap Loader which loads tiles from a TextureAtlas instead of separate images.
/// It requires a map-level property called 'atlas' with its value being the relative
/// path to the TextureAtlas.
/// <p>
/// The atlas must have in it indexed regions named after the tilesets used in the map.
/// The indexes shall be local to the tileset (not the global id). Strip whitespace and
/// rotation should not be used when creating the atlas.
/// </p>
/// </summary>
public class AtlasTmxMapLoader : BaseTmxMapLoader<AtlasTmxMapLoader.AtlasTiledMapLoaderParameters>
{
    public class AtlasTiledMapLoaderParameters : BaseTmxMapLoader.Parameters
    {
        public bool ForceTextureFilters { get; set; } = false;
    }

}