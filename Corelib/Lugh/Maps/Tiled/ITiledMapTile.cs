// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.Lugh.Graphics.G2D;

namespace Corelib.Lugh.Maps.Tiled;

/// <summary>
/// Generalises the concept of tile in a TiledMap
/// </summary>
[PublicAPI]
public interface ITiledMapTile
{
    [PublicAPI]
    public enum Blendmode
    {
        None,
        Alpha,
    }

    /// <summary>
    /// The tile ID
    /// </summary>
    int ID { get; set; }

    /// <summary>
    /// The <see cref="Blendmode"/> to use when rendering a tile.
    /// </summary>
    Blendmode BlendMode { get; set; }

    /// <summary>
    /// The <see cref="TextureRegion"/> to display.
    /// </summary>
    TextureRegion TextureRegion { get; set; }

    /// <summary>
    /// X co-ordinate of a tile image in a tileset.
    /// </summary>
    float OffsetX { get; set; }

    /// <summary>
    /// Y co-ordinate of a tile image in a tileset.
    /// </summary>
    float OffsetY { get; set; }

    /// <summary>
    /// Returns this tiles properties set.
    /// </summary>
    MapProperties Properties { get; }

    /// <summary>
    /// Returns the collection of objects contained within a tile.
    /// </summary>
    MapObjects MapObjects { get; }
}
