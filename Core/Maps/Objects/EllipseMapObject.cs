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

namespace LibGDXSharp.Maps.Objects;

[PublicAPI]
public class EllipseMapObject : MapObject
{
    public Ellipse Ellipse { get; set; }

    /// <symmary>
    /// Creates an <see cref="Ellipse"/> object whose lower left corner
    /// is at(0, 0) with width=1 and height=1
    /// </symmary>
    public EllipseMapObject() : this( 0.0f, 0.0f, 1.0f, 1.0f )
    {
    }

    /// <summary>
    /// Creates an <see cref="Ellipse"/> object with the given X and Y coordinates
    /// along with a specified width and height.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    public EllipseMapObject( float x, float y, float width, float height )
    {
        Ellipse = new Ellipse( x, y, width, height );
    }
}