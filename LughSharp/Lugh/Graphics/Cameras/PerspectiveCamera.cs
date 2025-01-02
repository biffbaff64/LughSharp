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


using Matrix4 = LughSharp.Lugh.Maths.Matrix4;

namespace LughSharp.Lugh.Graphics.Cameras;

/// <summary>
/// A Camera with Perspective Projection.
/// </summary>
[PublicAPI]
public class PerspectiveCamera : Camera
{
    private readonly Vector3 _tmp = new();

    // the field of view of the height, in degrees.
    public readonly float FieldOfView = 67;

    // ========================================================================

    public PerspectiveCamera()
    {
    }

    /// <summary>
    /// Constructs a new <see cref="PerspectiveCamera"/> with the given field
    /// of view and viewport size. The aspect ratio is derived from the viewport size.
    /// </summary>
    /// <param name="fieldOfViewY">
    /// The field of view of the height, in degrees. The field of view for the
    /// width will be calculated according to the aspect ratio.
    /// </param>
    /// <param name="viewportWidth">Viewport width in pixels.</param>
    /// <param name="viewportHeight">Viewport height in pixels.</param>
    /// <remarks>Call <see cref="Update"/> immediately after this constructor.</remarks>
    public PerspectiveCamera( float fieldOfViewY, float viewportWidth, float viewportHeight )
    {
        FieldOfView    = fieldOfViewY;
        ViewportWidth  = viewportWidth;
        ViewportHeight = viewportHeight;
    }

    /// <summary>
    /// Updates the camera.
    /// Also updates the frustrum if <paramref name="updateFrustrum"/> is true.
    /// </summary>
    public override void Update( bool updateFrustrum = true )
    {
        var aspect = ViewportWidth / ViewportHeight;

        Projection.SetToProjection( Math.Abs( Near ), Math.Abs( Far ), FieldOfView, aspect );

        View.SetToLookAt( Position, _tmp.Set( Position ).Add( Direction ), Up );

        Combined.Set( Projection );

        Matrix4.Mul( Combined.Val, View.Val );

        if ( updateFrustrum )
        {
            InvProjectionView.Set( Combined );
            Matrix4.Invert( InvProjectionView.Val );
            Frustum.Update( InvProjectionView );
        }
    }
}
