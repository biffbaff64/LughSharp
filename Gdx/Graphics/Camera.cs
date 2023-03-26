using LibGDXSharp.Backends.Desktop;
using LibGDXSharp.Core;
using LibGDXSharp.Maths;
using LibGDXSharp.Maths.Collision;

namespace LibGDXSharp.Graphics
{
    public abstract class Camera
    {
        /// the position of the camera
        public Vector3 Position { get; set; } = new Vector3();

        /// the unit length direction vector of the camera
        public Vector3 Direction { get; set; } = new Vector3( 0, 0, -1 );

        /// the unit length up vector of the camera
        public Vector3 Up { get; set; } = new Vector3( 0, 1, 0 );

        public Matrix4 Projection        { get; set; } = new Matrix4();
        public Matrix4 View              { get; set; } = new Matrix4();
        public Matrix4 Combined          { get; set; } = new Matrix4();
        public Matrix4 InvProjectionView { get; set; } = new Matrix4();

        /// the near clipping plane distance, has to be positive
        public float Near { get; set; } = 1;

        /// the far clipping plane distance, has to be positive
        public float Far { get; set; } = 100;

        public float    ViewportWidth  { get; set; } = 0;
        public float    ViewportHeight { get; set; } = 0;
        public Frustrum Frustum        { get; set; } = new Frustrum();

        private readonly Vector3 _tmpVec = new Vector3();
        private readonly Ray     _ray    = new Ray( new Vector3(), new Vector3() );

        /// <summary>
        /// Recalculates the projection and view matrix of this camera and the Frustum
        /// planes. Use this after you've manipulated any of the attributes of the camera.
        /// </summary>
        public abstract void Update( bool updateFrustrum = true );

        /// <summary>
        /// Recalculates the direction of the camera to look at the point (x, y, z).
        /// This function assumes the up vector is normalized.
        /// </summary>
        /// <param name="x"> the x-coordinate of the point to look at.</param>
        /// <param name="y"> the y-coordinate of the point to look at.</param>
        /// <param name="z"> the z-coordinate of the point to look at.</param>
        public virtual void LookAt( float x, float y, float z )
        {
            _tmpVec.Set( x, y, z ).Sub( Position ).Nor();

            if ( !_tmpVec.IsZero() )
            {
                // up and direction must ALWAYS be orthonormal vectors
                var dot = _tmpVec.Dot( Up );

                if ( Math.Abs( dot - 1 ) < 0.000000001f )
                {
                    // Collinear
                    Up.Set( Direction ).Scl( -1 );
                }
                else if ( Math.Abs( dot + 1 ) < 0.000000001f )
                {
                    // Collinear opposite
                    Up.Set( Direction );
                }

                Direction.Set( _tmpVec );
                NormalizeUp();
            }
        }

        /// <summary>
        /// Recalculates the direction of the camera to look at the point (x, y, z).
        /// </summary>
        /// <param name="target">the point to look at.</param>
        public virtual void LookAt( Vector3 target )
        {
            LookAt( target.X, target.Y, target.Z );
        }

        /// <summary>
        /// Normalizes the up vector by first calculating the right vector via a
        /// cross product between direction and up, and then recalculating the up
        /// vector via a cross product between right and direction. 
        /// </summary>
        public virtual void NormalizeUp()
        {
            _tmpVec.Set( Direction ).Crs( Up );
            Up.Set( _tmpVec ).Crs( Direction ).Nor();
        }

        /// <summary>
        /// Rotates the direction and up vector of this camera by the given
        /// angle around the given axis. The direction and up vector will
        /// not be orthogonalized.
        /// </summary>
        /// <param name="angle">the angle.</param>
        /// <param name="axisX">the x-component of the axis.</param>
        /// <param name="axisY">the y-component of the axis.</param>
        /// <param name="axisZ">the z-component of the axis.</param>
        public virtual void Rotate( float angle, float axisX, float axisY, float axisZ )
        {
            Direction.Rotate( angle, axisX, axisY, axisZ );
            Up.Rotate( angle, axisX, axisY, axisZ );
        }

        /// <summary>
        /// Rotates the direction and up vector of this camera by the given
        /// angle around the given axis. The direction and up vector will
        /// not be orthogonalized.
        /// </summary>
        /// <param name="axis">the axis to rotate around</param>
        /// <param name="angle">the angle, in degrees</param>
        public virtual void Rotate( Vector3 axis, float angle )
        {
            Direction.Rotate( axis, angle );
            Up.Rotate( axis, angle );
        }

        /// <summary>
        /// Rotates the direction and up vector of this camera by the given rotation
        /// matrix. The direction and up vector will not be orthogonalized.
        /// </summary>
        /// <param name="transform"> The rotation matrix  </param>
        public virtual void Rotate( in Matrix4 transform )
        {
            Direction.Rot( transform );
            Up.Rot( transform );
        }

        /// <summary>
        /// Rotates the direction and up vector of this camera by the given
        /// <seealso cref="Quaternion"/>. The direction and up vector will not
        /// be orthogonalized.
        /// </summary>
        /// <param name="quat">The quaternion.</param>
        public virtual void Rotate( in Quaternion quat )
        {
            quat.Transform( Direction );
            quat.Transform( Up );
        }

        /// <summary>
        /// Rotates the direction and up vector of this camera by the given angle around the
        /// given axis, with the axis attached to given point. The direction and up vector
        /// will not be orthogonalized.
        /// </summary>
        /// <param name="point"> the point to attach the axis to </param>
        /// <param name="axis"> the axis to rotate around </param>
        /// <param name="angle"> the angle, in degrees  </param>
        public virtual void RotateAround( Vector3 point, Vector3 axis, float angle )
        {
            _tmpVec.Set( point );
            _tmpVec.Sub( Position );

            Translate( _tmpVec );
            Rotate( axis, angle );

            _tmpVec.Rotate( axis, angle );

            Translate( -_tmpVec.X, -_tmpVec.Y, -_tmpVec.Z );
        }

        /// <summary>
        /// Transform the position, direction and up vector by the given matrix
        /// </summary>
        /// <param name="transform"> The transform matrix</param>
        public virtual void Transform( in Matrix4 transform )
        {
            Position.Mul( transform );
            Rotate( transform );
        }

        /// <summary>
        /// Moves the camera by the given amount on each axis.
        /// </summary>
        /// <param name="x"> the displacement on the x-axis</param>
        /// <param name="y"> the displacement on the y-axis</param>
        /// <param name="z"> the displacement on the z-axis</param>
        public virtual void Translate( float x, float y, float z )
        {
            Position.Add( x, y, z );
        }

        /// <summary>
        /// Moves the camera by the given vector. </summary>
        /// <param name="vec"> the displacement vector  </param>
        public virtual void Translate( Vector3 vec )
        {
            Position.Add( vec );
        }

        /// <summary>
        /// Function to translate a point given in screen coordinates to world space.
        /// It's the same as GLU gluUnProject, but does not rely on OpenGL. The x- and
        /// y-coordinate of vec are assumed to be in screen coordinates (origin is the
        /// top left corner, y pointing down, x pointing to the right) as reported by the
        /// touch methods in <seealso cref="Input"/>. A z-coordinate of 0 will return a
        /// point on the near plane, a z-coordinate of 1 will return a point on the far
        /// plane.
        /// This method allows you to specify the viewport position and dimensions in the
        /// coordinate system expected by <see cref="GL20.GlViewport(int, int, int, int)"/>,
        /// with the origin in the bottom left corner of the screen.
        /// </summary>
        /// <param name="screenCoords">The point in screen coordinates (origin top left)</param>
        /// <param name="viewportX">The coord of the bottom left corner of the viewport in glViewport coords.</param>
        /// <param name="viewportY">The coord of the bottom left corner of the viewport in glViewport coords.</param>
        /// <param name="viewportWidth">The width of the viewport in pixels </param>
        /// <param name="viewportHeight">The height of the viewport in pixels </param>
        /// <returns> the mutated and unprojected screenCoords <see cref="Vector3"/>  </returns>
        public virtual Vector3 Unproject( Vector3 screenCoords, float viewportX, float viewportY,
                                          float viewportWidth, float viewportHeight )
        {
            var x = screenCoords.X;
            var y = screenCoords.Y;

            x -= viewportX;
            y = Gdx.Graphics.GetHeight() - y;
            y -= viewportY;

            screenCoords.X = ( 2 * x ) / viewportWidth - 1;
            screenCoords.Y = ( 2 * y ) / viewportHeight - 1;
            screenCoords.Z = 2 * screenCoords.Z - 1;
            screenCoords.Prj( InvProjectionView );

            return screenCoords;
        }

        /// <summary>
        /// Function to translate a point given in screen coordinates to world space.
        /// It's the same as GLU gluUnProject but does not rely on OpenGL.
        /// The viewport is assumed to span the whole screen and is fetched from
        /// <see cref="Gdx.Graphics.GetWidth()"/> and <see cref="Gdx.Graphics.GetHeight()"/>.
        /// The x- and y-coordinate of vec are assumed to be in screen coordinates (origin
        /// is the top left corner, y pointing down, x pointing to the right) as reported by
        /// the touch methods in <seealso cref="Input"/>. A z-coordinate of 0 will return a
        /// point on the near plane, a z-coordinate of 1 will return a point on the far plane.
        /// </summary>
        /// <param name="screenCoords">The point in screen coordinates.</param>
        /// <returns> the mutated and unprojected screenCoords <seealso cref="Vector3"/></returns>
        public virtual Vector3 Unproject( Vector3 screenCoords )
        {
            Unproject( screenCoords, 0, 0, Gdx.Graphics.GetWidth(), Gdx.Graphics.GetHeight() );

            return screenCoords;
        }

        /// <summary>
        /// Projects the <seealso cref="Vector3"/> given in world space to screen coordinates.
        /// It's the same as GLU gluProject with one small deviation: The viewport is assumed
        /// to span the whole screen. The screen coordinate system has its origin in the
        /// <b>bottom</b> left, with the y-axis pointing <b>upwards</b> and the x-axis pointing
        /// to the right. This makes it easily useable in conjunction with <seealso cref="Batch"/>
        /// and similar classes.
        /// </summary>
        /// <returns>The mutated and projected worldCoords <seealso cref="Vector3"/>.</returns>
        public virtual Vector3 Project( Vector3 worldCoords )
        {
            Project( worldCoords, 0, 0, Gdx.Graphics.GetWidth(), Gdx.Graphics.GetHeight() );

            return worldCoords;
        }

        /// <summary>
        /// Projects the <seealso cref="Vector3"/> given in world space to screen coordinates.
        /// It's the same as GLU gluProject with one small deviation: The viewport is assumed
        /// to span the whole screen. The screen coordinate system has its origin in the
        /// <b>bottom</b> left, with the y-axis pointing <b>upwards</b> and the x-axis pointing
        /// to the right. This makes it easily useable in conjunction with <seealso cref="Batch"/>
        /// and similar classes.
        /// This method allows you to specify the viewport position and dimensions in the coordinate
        /// system expected by <seealso cref="GL20.GlViewport(int, int, int, int)"/>, with the origin
        /// in the bottom left corner of the screen.
        /// </summary>
        /// <param name="worldCoords"></param>
        /// <param name="viewportX"> the coordinate of the bottom left corner of the viewport in glViewport coordinates.</param>
        /// <param name="viewportY"> the coordinate of the bottom left corner of the viewport in glViewport coordinates.</param>
        /// <param name="viewportWidth"> the width of the viewport in pixels.</param>
        /// <param name="viewportHeight"> the height of the viewport in pixels.</param>
        /// <returns> the mutated and projected worldCoords <seealso cref="Vector3"/>.</returns>
        public virtual Vector3 Project( Vector3 worldCoords, float viewportX, float viewportY,
                                        float viewportWidth, float viewportHeight )
        {
            worldCoords.Prj( Combined );
            worldCoords.X = viewportWidth * ( worldCoords.X + 1 ) / 2 + viewportX;
            worldCoords.Y = viewportHeight * ( worldCoords.Y + 1 ) / 2 + viewportY;
            worldCoords.Z = ( worldCoords.Z + 1 ) / 2;

            return worldCoords;
        }

        /// <summary>
        /// Creates a picking <seealso cref="Ray"/> from the coordinates given in screen
        /// coordinates. It is assumed that the viewport spans the whole screen. The screen
        /// coordinates origin is assumed to be in the top left corner, its y-axis pointing
        /// down, the x-axis pointing to the right. The returned instance is not a new
        /// instance but an internal member only accessible via this function.
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <param name="viewportX">
        /// The coordinate of the bottom left corner of the viewport in glViewport coordinates.
        /// </param>
        /// <param name="viewportY">
        /// The coordinate of the bottom left corner of the viewport in glViewport coordinates.
        /// </param>
        /// <param name="viewportWidth">The width of the viewport in pixels</param>
        /// <param name="viewportHeight">The height of the viewport in pixels</param>
        /// <returns>The picking Ray.</returns>
        public virtual Ray GetPickRay( float screenX, float screenY, float viewportX,
                                       float viewportY, float viewportWidth, float viewportHeight )
        {
            Unproject( _ray.origin.Set( screenX, screenY, 0 ), viewportX, viewportY, viewportWidth, viewportHeight );
            Unproject( _ray.direction.Set( screenX, screenY, 1 ), viewportX, viewportY, viewportWidth, viewportHeight );

            _ray.direction.Sub( _ray.origin ).Nor();

            return _ray;
        }

        /// <summary>
        /// Creates a picking <seealso cref="Ray"/> from the coordinates given in screen
        /// coordinates. It is assumed that the viewport spans the whole screen. The screen
        /// coordinates origin is assumed to be in the top left corner, its y-axis pointing
        /// down, the x-axis pointing to the right. The returned instance is not a new
        /// instance but an internal member only accessible via this function.
        /// </summary>
        /// <returns>The picking Ray.</returns>
        public virtual Ray GetPickRay( float screenX, float screenY )
        {
            return GetPickRay( screenX, screenY, 0, 0, Gdx.Graphics.GetWidth(), Gdx.Graphics.GetHeight() );
        }
    }
}
