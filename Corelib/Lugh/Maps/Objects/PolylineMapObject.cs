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

using Corelib.Lugh.Maths;

namespace Corelib.Lugh.Maps.Objects;

[PublicAPI]
public class PolylineMapObject : MapObject
{
    /// <summary>
    /// Creates a new, empty, PolylineMapObject.
    /// </summary>
    public PolylineMapObject()
        : this( [ ] )
    {
    }

    /// <summary>
    /// Creates a new PolylineMapObject using the supplied vertices.
    /// </summary>
    /// <param name="vertices"> polyline defining vertices </param>
    public PolylineMapObject( float[] vertices )
    {
        Polyline = new Polyline( vertices );
    }

    /// <summary>
    /// Creates a new PolylineMapObject using the supplied <see cref="Polyline"/>
    /// </summary>
    /// <param name="polyline"> the polyline </param>
    public PolylineMapObject( Polyline polyline )
    {
        Polyline = polyline;
    }

    public Polyline Polyline { get; set; }
}
