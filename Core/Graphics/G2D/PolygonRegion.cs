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

namespace LibGDXSharp.Graphics.G2D;

public class PolygonRegion
{

    /// <summary>
    ///     Creates a PolygonRegion by triangulating the polygon coordinates in vertices
    ///     and calculates uvs based on that. TextureRegion can come from an atlas.
    /// </summary>
    /// <param name="region"> the region used for drawing </param>
    /// <param name="vertices">
    ///     contains 2D polygon coordinates in pixels relative to source region.
    /// </param>
    /// <param name="triangles"></param>
    public PolygonRegion( TextureRegion region, float[]? vertices, short[] triangles )
    {
        Region        = region;
        Vertices      = vertices;
        Triangles     = triangles;
        TextureCoords = new float[ vertices.Length ];

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
