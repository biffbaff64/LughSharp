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

using Corelib.LibCore.Graphics.G2D;

namespace Corelib.LibCore.Maps.Tiled;

/// <summary>
/// Represents a TiledMap layer which is a TextureRegion.
/// </summary>
[PublicAPI]
public class TiledMapImageLayer : MapLayer
{
    public TextureRegion? Region { get; set; }
    public float          X      { get; set; }
    public float          Y      { get; set; }

    // ========================================================================
    
    /// <summary>
    /// Creates a new TiledMapImageLayer, using the supplied TextureRegion as its
    /// background, and the given X and Y as the coordinates of the image in the map,
    /// </summary>
    /// <param name="region"> The <see cref="TextureRegion"/>. </param>
    /// <param name="x"> Textureregion Layer X coordinate. </param>
    /// <param name="y"> Textureregion Layer X coordinate. </param>
    public TiledMapImageLayer( TextureRegion? region, float x, float y )
    {
        Region = region;
        X      = x;
        Y      = y;
    }
}
