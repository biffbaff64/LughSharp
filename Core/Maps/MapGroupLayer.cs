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

namespace LibGDXSharp.Maps;

/// <summary>
/// Map layer containing a set of MapLayers, objects and properties.
/// </summary>
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class MapGroupLayer : MapLayer
{
    public MapLayers Layers { get; set; } = new();

    public override void InvalidateRenderOffset()
    {
        base.InvalidateRenderOffset();

        for ( var i = 0; i < Layers.Size(); i++ )
        {
            MapLayer child = Layers.Get( i );
            child.InvalidateRenderOffset();
        }
    }
}