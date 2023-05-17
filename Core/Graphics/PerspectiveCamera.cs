using LibGDXSharp.Maths;

namespace LibGDXSharp.Graphics;

/// <summary>
/// A Camera with Perspective Projection.
/// </summary>
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
    public PerspectiveCamera( float fieldOfViewY, float viewportWidth, float viewportHeight )
    {
        this.fieldOfView    = fieldOfViewY;
        this.ViewportWidth  = viewportWidth;
        this.ViewportHeight = viewportHeight;

        this.Update( true );
    }

    private readonly Vector3 _tmp = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="updateFrustum"></param>
    public new void Update( bool updateFrustum )
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
