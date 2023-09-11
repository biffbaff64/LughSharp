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

namespace LibGDXSharp.Graphics;

/// <summary>
/// A Camera with Perspective Projection.
/// </summary>
[PublicAPI]
public class PerspectiveCamera : Camera
{
    // the field of view of the height, in degrees.
    public float fieldOfView = 67;

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
        this.fieldOfView    = fieldOfViewY;
        this.ViewportWidth  = viewportWidth;
        this.ViewportHeight = viewportHeight;
    }

    private readonly Vector3 _tmp = new();

    /// <summary>
    /// </summary>
    /// <param name="updateFrustum"></param>
    public override void Update( bool updateFrustum = true )
    {
        var aspect = ViewportWidth / ViewportHeight;

        Projection.SetToProjection( Math.Abs( Near ), Math.Abs( Far ), fieldOfView, aspect );
        
        View.SetToLookAt( Position, _tmp.Set( Position ).Add( Direction ), Up );
        
        Combined.Set( Projection );
        
        Matrix4.Mul( Combined.val, View.val );

        if ( updateFrustum )
        {
            InvProjectionView.Set( Combined );
            Matrix4.Inv( InvProjectionView.val );
            Frustum.Update( InvProjectionView );
        }
    }
}
