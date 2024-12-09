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

using Corelib.Lugh.Graphics.Images;

namespace Corelib.Lugh.Graphics.G2D;

/// <summary>
/// A PolygonBatch is an extension of the Batch interface that provides additional
/// render methods specifically for rendering polygons.
/// </summary>
[PublicAPI]
public interface IPolygonBatch : IBatch
{
    /// <summary>
    /// Draws a polygon region with the bottom left corner at x,y
    /// having the width and height of the region.
    /// </summary>
    void Draw( PolygonRegion region, float x, float y );

    /// <summary>
    /// Draws a polygon region with the bottom left corner at x,y and stretching
    /// the region to cover the given width and height.
    /// </summary>
    void Draw( PolygonRegion region, float x, float y, float width, float height );

    /// Draws the polygon region with the bottom left corner at x,y and stretching
    /// the region to cover the given width and height. The polygon region is offset
    /// by originX, originY relative to the origin. Scale specifies the scaling factor
    /// by which the polygon region should be scaled around originX, originY. Rotation
    /// specifies the angle of counter clockwise rotation of the rectangle around originX
    /// originY.
    void Draw( PolygonRegion region,
               float x,
               float y,
               float originX,
               float originY,
               float width,
               float height,
               float scaleX,
               float scaleY,
               float rotation );

    /// <summary>
    /// Draws the polygon using the given vertices and triangles. Each vertices must be
    /// made up of 5 elements in this order: x, y, color, u, v.
    /// </summary>
    void Draw( Texture texture,
               float[] polygonVertices,
               int verticesOffset,
               int verticesCount,
               short[] polygonTriangles,
               int trianglesOffset,
               int trianglesCount );
}
