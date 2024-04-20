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


namespace LughSharp.LibCore.Maths;

/// <summary>
///     A truncated rectangular pyramid. Used to define the viewable
///     region and its projection onto the screen.
/// </summary>
public class Frustrum
{
    protected readonly static Vector3[] ClipSpacePlanePoints =
    {
        new( -1, -1, -1 ),
        new( 1, -1, -1 ),
        new( 1, 1, -1 ),
        new( -1, 1, -1 ),
        new( -1, -1, 1 ),
        new( 1, -1, 1 ),
        new( 1, 1, 1 ),
        new( -1, 1, 1 )
    };

    protected readonly static float[] ClipSpacePlanePointsArray = new float[ 8 * 3 ];

    /// <system>
    ///     eight points making up the near and far clipping "rectangles".
    ///     Order is counter clockwise, starting at bottom left.
    /// </system>
    public readonly Vector3[] planePoints =
    {
        new(), new(), new(), new(),
        new(), new(), new(), new()
    };

    protected readonly float[] planePointsArray = new float[ 8 * 3 ];

    static Frustrum()
    {
        var j = 0;

        foreach ( Vector3 v in ClipSpacePlanePoints )
        {
            ClipSpacePlanePointsArray[ j++ ] = v.X;
            ClipSpacePlanePointsArray[ j++ ] = v.Y;
            ClipSpacePlanePointsArray[ j++ ] = v.Z;
        }
    }

    public Frustrum()
    {
        for ( var i = 0; i < 6; i++ )
        {
            Planes[ i ] = new Plane( new Vector3(), 0 );
        }
    }

    /// <system>
    ///     the six clipping planes, near, far, left, right, top, bottom
    /// </system>
    public Plane[] Planes { get; set; } = new Plane[ 6 ];

    /// <summary>
    ///     Updates the clipping plane's based on the given inverse combined
    ///     projection and view matrix, e.g. from an <see cref="OrthographicCamera" />
    ///     or <see cref="PerspectiveCamera" />.
    /// </summary>
    /// <param name="inverseProjectionView">The combined projection and view matrices.</param>
    public virtual void Update( Matrix4 inverseProjectionView )
    {
        Array.Copy( ClipSpacePlanePointsArray, 0, planePointsArray, 0, ClipSpacePlanePointsArray.Length );

        Matrix4.Prj( inverseProjectionView.val, planePointsArray, 0, 8, 3 );

        for ( int i = 0, j = 0; i < 8; i++ )
        {
            Vector3 v = planePoints[ i ];

            v.X = planePointsArray[ j++ ];
            v.Y = planePointsArray[ j++ ];
            v.Z = planePointsArray[ j++ ];
        }

        Planes[ 0 ].Set( planePoints[ 1 ], planePoints[ 0 ], planePoints[ 2 ] );
        Planes[ 1 ].Set( planePoints[ 4 ], planePoints[ 5 ], planePoints[ 7 ] );
        Planes[ 2 ].Set( planePoints[ 0 ], planePoints[ 4 ], planePoints[ 3 ] );
        Planes[ 3 ].Set( planePoints[ 5 ], planePoints[ 1 ], planePoints[ 6 ] );
        Planes[ 4 ].Set( planePoints[ 2 ], planePoints[ 3 ], planePoints[ 6 ] );
        Planes[ 5 ].Set( planePoints[ 4 ], planePoints[ 0 ], planePoints[ 1 ] );
    }

    /// <summary>
    ///     Returns whether the point is in the frustum.
    /// </summary>
    /// <param name="point"> The point </param>
    /// <returns> Whether the point is in the frustum.  </returns>
    public virtual bool PointInFrustum( Vector3 point )
    {
        foreach ( Plane t in Planes )
        {
            Plane.PlaneSide result = t.TestPoint( point );

            if ( result == Plane.PlaneSide.Back )
            {
                return false;
            }
        }

        return true;
    }
}
