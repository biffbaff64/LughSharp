// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace LughSharp.Lugh.Graphics.G2D;

[PublicAPI]
public class PolygonRegion
{
    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a PolygonRegion by triangulating the polygon coordinates in vertices
    /// and calculates uvs based on that. TextureRegion can come from an atlas.
    /// </summary>
    /// <param name="region"> the region used for drawing </param>
    /// <param name="vertices">
    /// contains 2D polygon coordinates in pixels relative to source region.
    /// </param>
    /// <param name="triangles"></param>
    public PolygonRegion( TextureRegion region, float[]? vertices, short[] triangles )
    {
        Region        = region;
        Vertices      = vertices;
        Triangles     = triangles;
        TextureCoords = new float[ vertices!.Length ];

        var u        = region.U;
        var v        = region.V;
        var uvWidth  = region.U2 - u;
        var uvHeight = region.V2 - v;
        var width    = region.RegionWidth;
        var height   = region.RegionHeight;

        for ( int i = 0, n = vertices.Length; i < n; i += 2 )
        {
            TextureCoords[ i ]     = u + ( uvWidth * ( vertices[ i ] / width ) );
            TextureCoords[ i + 1 ] = v + ( uvHeight * ( 1 - ( vertices[ i + 1 ] / height ) ) );
        }
    }

    public TextureRegion Region        { get; set; }
    public float[]       TextureCoords { get; set; } // texture coordinates in atlas coordinates
    public float[]?      Vertices      { get; set; } // pixel coordinates relative to source image.
    public short[]       Triangles     { get; set; }
}
