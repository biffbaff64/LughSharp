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


using LibGDXSharp.Gdx.Maths;

namespace LibGDXSharp.Gdx.Maps.Objects;

public class PolygonMapObject : MapObject
{
    /// <summary>
    ///     Creates empty polygon map object
    /// </summary>
    public PolygonMapObject() : this( Array.Empty< float >() )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="vertices">polygon defining vertices (at least 3)</param>
    public PolygonMapObject( float[]? vertices ) => Polygon = new Polygon( vertices );

    /// <summary>
    /// </summary>
    /// <param name="polygon">the polygon</param>
    public PolygonMapObject( Polygon polygon ) => Polygon = polygon;

    public Polygon Polygon { get; set; }

    /// <summary>
    ///     <param name="polygon">new object's polygon shape</param>
    /// </summary>
    public void SetPolygon( Polygon polygon ) => Polygon = polygon;
}
