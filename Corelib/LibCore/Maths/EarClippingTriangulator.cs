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


namespace Corelib.LibCore.Maths;

[PublicAPI]
public class EarClippingTriangulator
{
    private const int CONCAVE = -1;
    private const int CONVEX  = 1;

    private readonly List< short > _indicesArray = [ ];
    private readonly List< short > _triangles    = [ ];
    private readonly List< int >   _vertexTypes  = [ ];
    private          short[]?      _indices;
    private          int           _vertexCount;
    private          float[]?      _vertices;

    // ========================================================================

    public List< short > ComputeTriangles( List< float > vertices )
    {
        return ComputeTriangles( vertices.ToArray(), 0, vertices.Count );
    }

    public List< short > ComputeTriangles( float[] vertices )
    {
        return ComputeTriangles( vertices, 0, vertices.Length );
    }

    /// <summary>
    /// Triangulates the given (convex or concave) simple polygon to a
    /// list of triangle vertices.
    /// </summary>
    /// <param name="vertices">
    /// pairs describing vertices of the polygon, in either clockwise
    /// or counterclockwise order.
    /// </param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns>
    /// triples of triangle indices in clockwise order. Note the returned
    /// array is reused for later calls to the same method.
    /// </returns>
    public List< short > ComputeTriangles( float[] vertices, int offset, int count )
    {
        _vertices = vertices;

        var vertexCount  = _vertexCount = count / 2;
        var vertexOffset = offset / 2;

        _indicesArray.Clear();
        _indicesArray.EnsureCapacity( vertexCount );

        if ( GeometryUtils.IsClockwise( vertices, offset, count ) )
        {
            for ( short i = 0; i < vertexCount; i++ )
            {
                _indicesArray[ i ] = ( short ) ( vertexOffset + i );
            }
        }
        else
        {
            for ( int i = 0, n = vertexCount - 1; i < vertexCount; i++ )
            {
                _indicesArray[ i ] = ( short ) ( ( vertexOffset + n ) - i ); // Reversed.
            }
        }

        _indices = _indicesArray.ToArray();

        _vertexTypes.Clear();
        _vertexTypes.EnsureCapacity( vertexCount );

        for ( var i = 0; i < vertexCount; ++i )
        {
            _vertexTypes.Add( ClassifyVertex( i ) );
        }

        // A polygon with n vertices has a triangulation of n-2 triangles.
        _triangles.Clear();
        _triangles.EnsureCapacity( Math.Max( 0, vertexCount - 2 ) * 3 );

        Triangulate();

        return _triangles;
    }

    private void Triangulate()
    {
//        var vertexTypes = this._vertexTypes.ToArray();

        while ( _vertexCount > 3 )
        {
            var earTipIndex = FindEarTip();
            CutEarTip( earTipIndex );

            // The type of the two vertices adjacent to the clipped vertex may have changed.
            var previousIndex = PreviousIndex( earTipIndex );
            var nextIndex     = earTipIndex == _vertexCount ? 0 : earTipIndex;

            _vertexTypes[ previousIndex ] = ClassifyVertex( previousIndex );
            _vertexTypes[ nextIndex ]     = ClassifyVertex( nextIndex );
        }

        if ( _vertexCount == 3 )
        {
            _triangles.Add( _indices![ 0 ] );
            _triangles.Add( _indices[ 1 ] );
            _triangles.Add( _indices[ 2 ] );
        }
    }

    /// <summary>
    /// Returns <see cref="CONCAVE"/> or <see cref="CONVEX"/>.
    /// </summary>
    private int ClassifyVertex( int index )
    {
        var previous = _indices![ PreviousIndex( index ) ] * 2;
        var current  = _indices[ index ] * 2;
        var next     = _indices[ NextIndex( index ) ] * 2;

        return ComputeSpannedAreaSign( _vertices![ previous ],
                                       _vertices[ previous + 1 ],
                                       _vertices[ current ],
                                       _vertices[ current + 1 ],
                                       _vertices[ next ],
                                       _vertices[ next + 1 ] );
    }

    private int FindEarTip()
    {
        for ( var i = 0; i < _vertexCount; i++ )
        {
            if ( IsEarTip( i ) )
            {
                return i;
            }
        }

        // Desperate mode: if no vertex is an ear tip, we are dealing with a degenerate
        // polygon (e.g. nearly collinear).
        // Note that the input was not necessarily degenerate, but we could have made
        // it so by clipping some valid ears.

        // Idea taken from Martin Held, "FIST: Fast industrial-strength triangulation
        // of polygons", Algorithmica (1998),
        // http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.115.291

        // Return a convex or tangential vertex if one exists.
        for ( var i = 0; i < _vertexCount; i++ )
        {
            if ( _vertexTypes[ i ] != CONCAVE )
            {
                return i;
            }
        }

        return 0; // If all vertices are concave, just return the first one.
    }

    private bool IsEarTip( int earTipIndex )
    {
        if ( _vertexTypes[ earTipIndex ] == CONCAVE )
        {
            return false;
        }

        var previousIndex = PreviousIndex( earTipIndex );
        var nextIndex     = NextIndex( earTipIndex );
        var p1            = _indices![ previousIndex ] * 2;
        var p2            = _indices[ earTipIndex ] * 2;
        var p3            = _indices[ nextIndex ] * 2;
        var p1X           = _vertices![ p1 ];
        var p1Y           = _vertices[ p1 + 1 ];
        var p2X           = _vertices[ p2 ];
        var p2Y           = _vertices[ p2 + 1 ];
        var p3X           = _vertices[ p3 ];
        var p3Y           = _vertices[ p3 + 1 ];

        // Check if any point is inside the triangle formed by previous,
        // current and next vertices. Only consider vertices that are not
        // part of this triangle, or else we'll always find one inside.
        for ( var i = NextIndex( nextIndex ); i != previousIndex; i = NextIndex( i ) )
        {
            // Concave vertices can obviously be inside the candidate ear, but so can
            // tangential vertices if they coincide with one of the triangle's vertices.
            if ( _vertexTypes[ i ] != CONVEX )
            {
                var v  = _indices[ i ] * 2;
                var vx = _vertices[ v ];
                var vy = _vertices[ v + 1 ];

                // Because the polygon has clockwise winding order, the area sign will
                // be positive if the point is strictly inside. It will be 0 on the edge,
                // which we want to include as well.
                // note: check the edge defined by p1->p3 first since this fails _far_
                // more then the other 2 checks.
                if ( ComputeSpannedAreaSign( p3X, p3Y, p1X, p1Y, vx, vy ) >= 0 )
                {
                    if ( ComputeSpannedAreaSign( p1X, p1Y, p2X, p2Y, vx, vy ) >= 0 )
                    {
                        if ( ComputeSpannedAreaSign( p2X, p2Y, p3X, p3Y, vx, vy ) >= 0 )
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private void CutEarTip( int earTipIndex )
    {
        _triangles.Add( _indices![ PreviousIndex( earTipIndex ) ] );
        _triangles.Add( _indices[ earTipIndex ] );
        _triangles.Add( _indices[ NextIndex( earTipIndex ) ] );

        _indicesArray.RemoveAt( earTipIndex );
        _vertexTypes.RemoveAt( earTipIndex );
        _vertexCount--;
    }

    private int PreviousIndex( int index )
    {
        return ( index == 0 ? _vertexCount : index ) - 1;
    }

    private int NextIndex( int index )
    {
        return ( index + 1 ) % _vertexCount;
    }

    private static int ComputeSpannedAreaSign( float p1X,
                                               float p1Y,
                                               float p2X,
                                               float p2Y,
                                               float p3X,
                                               float p3Y )
    {
        var area = p1X * ( p3Y - p2Y );

        area += p2X * ( p1Y - p3Y );
        area += p3X * ( p2Y - p1Y );

        return Math.Sign( area );
    }
}
