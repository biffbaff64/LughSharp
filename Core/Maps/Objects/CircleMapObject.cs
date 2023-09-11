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
public class CircleMapObject : MapObject
{
    public Circle Circle { get; set; }

    /// <summary>
    /// Creates a circle map object at (0,0) with r=1.0
    /// </summary>
    public CircleMapObject() : this( 0.0f, 0.0f, 1.0f )
    {
    }

    /// <summary>
    /// Creates a circle map object
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="radius">Radius of the circle object.</param>
    public CircleMapObject( float x, float y, float radius )
    {
        Circle = new Circle( x, y, radius );
    }
}