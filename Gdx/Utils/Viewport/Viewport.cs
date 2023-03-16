using LibGDXSharp.Core;

namespace LibGDXSharp.Utils.Viewport
{
    public abstract class Viewport
    {
        private Camera _camera;
        private float  _worldWidth;
        private float  _worldHeight;
        private int    _screenX;
        private int    _screenY;
        private int    _screenWidth;
        private int    _screenHeight;

        public void Apply()
        {
        }

        /// <summary>
        /// Applies the viewport to the camera and sets the glViewport.
        /// </summary>
        /// <param name="centerCamera">
        /// If true, the camera position is set to the center of the world.
        /// </param>
        public virtual void Apply( bool centerCamera )
        {
            HdpiUtils.GLViewport( _screenX, _screenY, _screenWidth, _screenHeight );

            _camera.ViewportWidth  = _worldWidth;
            _camera.ViewportHeight = _worldHeight;

            if ( centerCamera )
            {
                _camera.Position.Set( _worldWidth / 2, _worldHeight / 2, 0 );
            }

            _camera.Update();
        }

        /// <summary>
        /// Configures this viewport's screen bounds using the specified screen
        /// size and calls <seealso cref="Apply(bool)"/>. Typically called
        /// from <seealso cref="IApplicationListener.Resize(int, int)"/> or
        /// <seealso cref="IScreen.Resize(int, int)"/>.
        /// </summary>
        /// <para>
        /// The default implementation only calls <seealso cref="Apply(bool)"/>. 
        /// </para>
        public virtual void Update( int screenWidth, int screenHeight, bool centerCamera = false )
        {
            Apply( centerCamera );
        }

        /// <summary>
        /// Transforms the specified screen coordinate to world coordinates. </summary>
        /// <returns> The vector that was passed in, transformed to world coordinates. </returns>
        /// <seealso cref="Camera.Unproject(Vector3) "/>
        public virtual Vector2 Unproject( Vector2 screenCoords )
        {
            tmp.set( screenCoords.x, screenCoords.y, 1 );
            camera.unproject( tmp, screenX, screenY, screenWidth, screenHeight );
            screenCoords.set( tmp.x, tmp.y );

            return screenCoords;
        }

        /// <summary>
        /// Transforms the specified world coordinate to screen coordinates. </summary>
        /// <returns> The vector that was passed in, transformed to screen coordinates. </returns>
        /// <seealso cref="Camera.project(Vector3) "/>
        public virtual Vector2 Project( Vector2 worldCoords )
        {
            tmp.set( worldCoords.x, worldCoords.y, 1 );
            camera.project( tmp, screenX, screenY, screenWidth, screenHeight );
            worldCoords.set( tmp.x, tmp.y );

            return worldCoords;
        }

        /// <summary>
        /// Transforms the specified screen coordinate to world coordinates. </summary>
        /// <returns> The vector that was passed in, transformed to world coordinates. </returns>
        /// <seealso cref="Camera.unproject(Vector3) "/>
        public virtual Vector3 Unproject( Vector3 screenCoords )
        {
            camera.unproject( screenCoords, screenX, screenY, screenWidth, screenHeight );

            return screenCoords;
        }

        /// <summary>
        /// Transforms the specified world coordinate to screen coordinates. </summary>
        /// <returns> The vector that was passed in, transformed to screen coordinates. </returns>
        /// <seealso cref="Camera.project(Vector3) "/>
        public virtual Vector3 Project( Vector3 worldCoords )
        {
            camera.project( worldCoords, screenX, screenY, screenWidth, screenHeight );

            return worldCoords;
        }

        /// <seealso cref="Camera.getPickRay(float, float, float, float, float, float) "/>
        public virtual Ray GetPickRay( float screenX, float screenY )
        {
            return camera.getPickRay( screenX, screenY, this.screenX, this.screenY, screenWidth, screenHeight );
        }

        /// <seealso cref="ScissorStack.calculateScissors(Camera, float, float, float, float, Matrix4, Rectangle, Rectangle) "/>
        public virtual void CalculateScissors( Matrix4 batchTransform, Rectangle area, Rectangle scissor )
        {
            ScissorStack.calculateScissors( camera, screenX, screenY, screenWidth, screenHeight, batchTransform, area, scissor );
        }

        /// <summary>
        /// Transforms a point to real screen coordinates (as opposed to OpenGL ES window coordinates), where the origin is in the top
        /// left and the the y-axis is pointing downwards. 
        /// </summary>
        public virtual Vector2 ToScreenCoordinates( Vector2 worldCoords, Matrix4 transformMatrix )
        {
            tmp.set( worldCoords.x, worldCoords.y, 0 );
            tmp.mul( transformMatrix );
            camera.project( tmp, screenX, screenY, screenWidth, screenHeight );
            tmp.y         = Gdx.graphics.getHeight() - tmp.y;
            worldCoords.x = tmp.x;
            worldCoords.y = tmp.y;

            return worldCoords;
        }

        public virtual Camera Camera
        {
            get { return camera; }
            set { this.camera = value; }
        }


        public virtual float WorldWidth
        {
            get { return worldWidth; }
            set { this.worldWidth = value; }
        }


        public virtual float WorldHeight
        {
            get { return worldHeight; }
            set { this.worldHeight = value; }
        }


        public virtual void SetWorldSize( float worldWidth, float worldHeight )
        {
            this.worldWidth  = worldWidth;
            this.worldHeight = worldHeight;
        }
    }
}
