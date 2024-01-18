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

using Matrix4 = LibGDXSharp.Maths.Matrix4;

namespace LibGDXSharp.Graphics;

/// <summary>
///     A Camera with Orthographic Projection.
/// </summary>
public class OrthographicCamera : Camera
{
    private readonly Vector3 _tmp = new();

    public OrthographicCamera()
    {
        Near = 0;
        Zoom = 1;
    }

    /// <summary>
    ///     Constructs a new OrthographicCamera, using the given viewport width and height.
    ///     For pixel perfect 2D rendering just supply the screen size, for other unit scales
    ///     (e.g. meters for box2d) proceed accordingly. The camera will show the region
    ///     [-viewportWidth/2, -(viewportHeight/2-1)] - [(viewportWidth/2-1), viewportHeight/2].
    ///     <para>
    ///         IMPORTANT: <see cref="Update" /> MUST be called after the call to this constructor.
    ///     </para>
    /// </summary>
    /// <param name="viewportWidth"></param>
    /// <param name="viewportHeight"></param>
    public OrthographicCamera( float viewportWidth, float viewportHeight )
    {
        ViewportWidth  = viewportWidth;
        ViewportHeight = viewportHeight;
        Near           = 0;
        Zoom           = 1;
    }

    public float Zoom { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="updateFrustrum"></param>
    public override void Update( bool updateFrustrum = true )
    {
        Projection.SetToOrtho( ( Zoom * -ViewportWidth ) / 2,
                               Zoom * ( ViewportWidth / 2 ),
                               Zoom * -( ViewportHeight / 2 ),
                               ( Zoom * ViewportHeight ) / 2,
                               Near,
                               Far );

        View.SetToLookAt( Position, _tmp.Set( Position ).Add( Direction ), Up );

        Combined.Set( Projection );

        Matrix4.Mul( Combined.val, View.val );

        if ( updateFrustrum )
        {
            InvProjectionView.Set( Combined );
            Matrix4.Inv( InvProjectionView.val );
            Frustum.Update( InvProjectionView );
        }
    }

    /// <summary>
    ///     Sets this camera to an orthographic projection using a viewport fitting
    ///     the screen resolution, centered at:-
    ///     <para>
    ///         <tt>(Gdx.graphics.getWidth()/2, Gdx.graphics.getHeight()/2)</tt>
    ///     </para>
    ///     with the y-axis pointing up or down.
    /// </summary>
    /// <param name="yDown">whether y should be pointing down.</param>
    public void SetToOrtho( bool yDown ) => SetToOrtho( yDown, Gdx.Graphics.Width, Gdx.Graphics.Height );

    /// <summary>
    ///     Sets this camera to an orthographic projection, centered at
    ///     (viewportWidth/2, viewportHeight/2), with the y-axis pointing up or down.
    /// </summary>
    /// <param name="yDown">whether y should be pointing down.</param>
    /// <param name="viewportWidth"></param>
    /// <param name="viewportHeight"></param>
    public void SetToOrtho( bool yDown, float viewportWidth, float viewportHeight )
    {
        if ( yDown )
        {
            Up.Set( 0, -1, 0 );
            Direction.Set( 0, 0, 1 );
        }
        else
        {
            Up.Set( 0, 1, 0 );
            Direction.Set( 0, 0, -1 );
        }

        Position.Set( ( Zoom * viewportWidth ) / 2.0f, ( Zoom * viewportHeight ) / 2.0f, 0 );
        ViewportWidth  = viewportWidth;
        ViewportHeight = viewportHeight;

        Update();
    }

    /// <summary>
    ///     Rotates the camera by the given angle around the direction vector.
    ///     The direction and up vector will not be orthogonalized.
    /// </summary>
    /// <param name="angle"></param>
    public void Rotate( float angle ) => Rotate( Direction, angle );

    /// <summary>
    ///     Moves the camera by the given amount on each axis.
    /// </summary>
    /// <param name="x" />
    /// the displacement on the x-axis
    /// <param name="y" />
    /// the displacement on the y-axis
    public void Translate( float x, float y ) => Translate( x, y, 0 );

    /// <summary>
    ///     Moves the camera by the given vector.
    /// </summary>
    /// <param name="vec">the displacement vector</param>
    public void Translate( Vector2 vec ) => Translate( vec.X, vec.Y, 0 );
}
