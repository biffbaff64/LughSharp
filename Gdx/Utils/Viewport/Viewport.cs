using LibGDXSharp.Core;
using LibGDXSharp.Maths;
using LibGDXSharp.Maths.Collision;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Utils.Viewport
{
    /// <summary>
    /// Manages a <see cref="Camera"/> and determines how world coordinates
    /// are mapped to and from the screen.
    /// </summary>
    public abstract class Viewport
    {
        public Camera Camera       { get; set; } = null!;
        public float  WorldWidth   { get; set; }
        public float  WorldHeight  { get; set; }
        public int    ScreenX      { get; set; }
        public int    ScreenY      { get; set; }
        public int    ScreenWidth  { get; set; }
        public int    ScreenHeight { get; set; }

        private readonly Vector3 _tmp = new Vector3();

        /// <summary>
        /// Applies the viewport to the camera and sets the glViewport.
        /// </summary>
        /// <param name="centerCamera">
        /// If true, the camera position is set to the center of the world.
        /// </param>
        public virtual void Apply( bool centerCamera = false )
        {
            HdpiUtils.GLViewport( ScreenX, ScreenY, ScreenWidth, ScreenHeight );

            Camera.ViewportWidth  = WorldWidth;
            Camera.ViewportHeight = WorldHeight;

            if ( centerCamera )
            {
                Camera.Position.Set( WorldWidth / 2, WorldHeight / 2, 0 );
            }

            Camera.Update();
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
        /// Transforms the specified screen coordinate to world coordinates.
        /// </summary>
        /// <returns> The vector that was passed in, transformed to world coordinates.</returns>
        /// <seealso cref="Camera.Unproject(Vector3)"/>
        public virtual Vector2 Unproject( Vector2 screenCoords )
        {
            _tmp.Set( screenCoords.X, screenCoords.Y, 1f );

            Camera.Unproject( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );
            screenCoords.Set( _tmp.X, _tmp.Y );

            return screenCoords;
        }

        /// <summary>
        /// Transforms the specified world coordinate to screen coordinates.
        /// </summary>
        /// <returns> The vector that was passed in, transformed to screen coordinates.</returns>
        /// <seealso cref="Camera.Project(Vector3) "/>
        public virtual Vector2 Project( Vector2 worldCoords )
        {
            _tmp.Set( worldCoords.X, worldCoords.Y, 1 );

            Camera.Project( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );
            worldCoords.Set( _tmp.X, _tmp.Y );

            return worldCoords;
        }

        /// <summary>
        /// Transforms the specified screen coordinate to world coordinates.
        /// </summary>
        /// <returns> The vector that was passed in, transformed to world coordinates.</returns>
        /// <seealso cref="Camera.Unproject(Vector3)"/>
        public virtual Vector3 Unproject( Vector3 screenCoords )
        {
            Camera.Unproject( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );

            return screenCoords;
        }

        /// <summary>
        /// Transforms the specified world coordinate to screen coordinates. </summary>
        /// <returns> The vector that was passed in, transformed to screen coordinates. </returns>
        /// <seealso cref="Camera.Project(Vector3) "/>
        public virtual Vector3 Project( Vector3 worldCoords )
        {
            Camera.Project( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );

            return worldCoords;
        }

        /// <summary>
        /// </summary>
        /// <seealso cref="Camera.GetPickRay(float, float, float, float, float, float) "/>
        public virtual Ray GetPickRay( float screenX, float screenY )
        {
            return Camera.GetPickRay( screenX, screenY, this.ScreenX, this.ScreenY, ScreenWidth, ScreenHeight );
        }

        /// <summary>
        /// </summary>
        /// <seealso cref="ScissorStack.CalculateScissors"/>
        public virtual void CalculateScissors( Matrix4 batchTransform, Rectangle area, Rectangle scissor )
        {
            ScissorStack.CalculateScissors
                (
                 Camera,
                 ScreenX,
                 ScreenY,
                 ScreenWidth,
                 ScreenHeight,
                 batchTransform,
                 area,
                 scissor
                );
        }

        /// <summary>
        /// Transforms a point to real screen coordinates (as opposed to OpenGL
        /// window coordinates), where the origin is in the top left and the
        /// the y-axis is pointing downwards. 
        /// </summary>
        public virtual Vector2 ToScreenCoordinates( Vector2 worldCoords, Matrix4 transformMatrix )
        {
            _tmp.Set( worldCoords.X, worldCoords.Y, 0 );
            _tmp.Mul( transformMatrix );

            Camera.Project( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );

            _tmp.Y = Gdx.Graphics.GetHeight() - _tmp.Y;

            worldCoords.X = _tmp.X;
            worldCoords.Y = _tmp.Y;

            return worldCoords;
        }

        public virtual void SetWorldSize( float worldWidth, float worldHeight )
        {
            this.WorldWidth  = worldWidth;
            this.WorldHeight = worldHeight;
        }

        /// <summary>
        /// Sets the viewport's position in screen coordinates.
        /// This is typically set by <seealso cref="Update(int, int, bool)"/>.
        /// </summary>
        public virtual void SetScreenPosition( int screenX, int screenY )
        {
            this.ScreenX = screenX;
            this.ScreenY = screenY;
        }

        /// <summary>
        /// Sets the viewport's size in screen coordinates.
        /// This is typically set by <seealso cref="Update(int, int, bool)"/>.
        /// </summary>
        public virtual void SetScreenSize( int screenWidth, int screenHeight )
        {
            this.ScreenWidth  = screenWidth;
            this.ScreenHeight = screenHeight;
        }

        /// <summary>
        /// Sets the viewport's bounds in screen coordinates.
        /// This is typically set by <seealso cref="Update(int, int, bool)"/>.
        /// </summary>
        public virtual void SetScreenBounds( int screenX, int screenY, int screenWidth, int screenHeight )
        {
            this.ScreenX      = screenX;
            this.ScreenY      = screenY;
            this.ScreenWidth  = screenWidth;
            this.ScreenHeight = screenHeight;
        }

        /// <summary>
        /// Returns the left gutter (black bar) width in screen coordinates.
        /// </summary>
        public virtual int LeftGutterWidth => ScreenX;

        /// <summary>
        /// Returns the right gutter (black bar) x in screen coordinates.
        /// </summary>
        public virtual int RightGutterX => ScreenX + ScreenWidth;

        /// <summary>
        /// Returns the right gutter (black bar) width in screen coordinates.
        /// </summary>
        public virtual int RightGutterWidth => ( Gdx.Graphics.GetWidth() - ( ScreenX + ScreenWidth ) );

        /// <summary>
        /// Returns the bottom gutter (black bar) height in screen coordinates.
        /// </summary>
        public virtual int BottomGutterHeight => ScreenY;

        /// <summary>
        /// Returns the top gutter (black bar) y in screen coordinates.
        /// </summary>
        public virtual int TopGutterY => ScreenY + ScreenHeight;

        /// <summary>
        /// Returns the top gutter (black bar) height in screen coordinates.
        /// </summary>
        public virtual int TopGutterHeight => ( Gdx.Graphics.GetHeight() - ( ScreenY + ScreenHeight ) );
    }
}
