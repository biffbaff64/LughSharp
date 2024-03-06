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


using LibGDXSharp.LibCore.Graphics.G2D;

namespace LibGDXSharp.LibCore.Maps.Tiled.Tiles;

public class StaticTiledMapTile : ITiledMapTile
{
    private MapObjects? _mapObjects;

    private MapProperties? _properties;

    /// <summary>
    ///     Creates a static tile with the given region
    /// </summary>
    /// <param name="texture">The <see cref="TextureRegion" /> to use.</param>
    public StaticTiledMapTile( TextureRegion texture )
    {
        TextureRegion = texture;
    }

    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="copy">The StaticTiledMapTile to copy.</param>
    public StaticTiledMapTile( StaticTiledMapTile copy )
    {
        if ( copy._properties != null )
        {
            GetProperties().PutAll( copy._properties );
        }

        _mapObjects   = copy._mapObjects;
        TextureRegion = copy.TextureRegion;
        ID            = copy.ID;
    }

    public int                     ID            { get; set; }
    public float                   OffsetX       { get; set; }
    public float                   OffsetY       { get; set; }
    public TextureRegion           TextureRegion { get; set; }
    public ITiledMapTile.Blendmode BlendMode     { get; set; } = ITiledMapTile.Blendmode.Alpha;

    public MapProperties GetProperties()
    {
        return _properties ??= new MapProperties();
    }

    public MapObjects GetObjects()
    {
        return _mapObjects ??= new MapObjects();
    }
}
