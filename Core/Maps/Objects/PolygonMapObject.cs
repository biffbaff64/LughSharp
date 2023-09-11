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

using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps.Objects;

[PublicAPI]
public class PolygonMapObject : MapObject
{        
    public Polygon Polygon { get; set; }

    /// <summary>
    /// Creates empty polygon map object
    /// </summary>
    public PolygonMapObject() : this( new float[0] )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="vertices">polygon defining vertices (at least 3)</param>
    public PolygonMapObject( float[] vertices )
    {
        this.Polygon = new Polygon(vertices);
    }

    /// <summary>
    /// </summary>
    /// <param name="polygon">the polygon</param>
    public PolygonMapObject( Polygon polygon )
    {
        this.Polygon = polygon;
    }

    /// <summary>
    /// <param name="polygon">new object's polygon shape</param>
    /// </summary>
    public void SetPolygon( Polygon polygon )
    {
        this.Polygon = polygon;
    }
}