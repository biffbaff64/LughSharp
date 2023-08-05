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

namespace LibGDXSharp.Maps.Tiled.Tiles;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class StaticTiledMapTile : ITiledMapTile
{
    public int                     ID            { get; set; }
    public float                   OffsetX       { get; set; }
    public float                   OffsetY       { get; set; }
    public TextureRegion           TextureRegion { get; set; }
    public ITiledMapTile.Blendmode BlendMode     { get; set; } = ITiledMapTile.Blendmode.Alpha;

    private MapProperties? _properties;
    private MapObjects?    _mapObjects;

    public MapProperties GetProperties()
    {
        return _properties ??= new MapProperties();

    }

    public MapObjects GetObjects()
    {
        return _mapObjects ??= new MapObjects();
    }

    /// <summary>
    /// Creates a static tile with the given region
    /// </summary>
    /// <param name="texture">The <see cref="G2D.TextureRegion"/> to use.</param>
    public StaticTiledMapTile( TextureRegion texture )
    {
        this.TextureRegion = texture;
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="copy">The StaticTiledMapTile to copy.</param>
    public StaticTiledMapTile( StaticTiledMapTile copy )
    {
        if ( copy._properties != null )
        {
            GetProperties().PutAll( copy._properties );
        }

        this._mapObjects   = copy._mapObjects;
        this.TextureRegion = copy.TextureRegion;
        this.ID            = copy.ID;
    }
}