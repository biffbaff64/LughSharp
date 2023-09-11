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
public class PolylineMapObject : MapObject
{
    /// <returns> polyline shape </returns>
    public virtual Polyline Polyline { get; set; }


    /// <summary>
    /// Creates empty polyline </summary>
    public PolylineMapObject() : this(new float[0])
    {
    }

    /// <param name="vertices"> polyline defining vertices </param>
    public PolylineMapObject(float[] vertices)
    {
        this.Polyline = new Polyline(vertices);
    }

    /// <param name="polyline"> the polyline </param>
    public PolylineMapObject(Polyline polyline)
    {
        this.Polyline = polyline;
    }
}