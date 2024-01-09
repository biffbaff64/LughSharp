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

using LibGDXSharp.Maths.Collision;
using LibGDXSharp.Scenes.Scene2D.Utils;

using Matrix4 = LibGDXSharp.Maths.Matrix4;

namespace LibGDXSharp.Utils.Viewport;

/// <summary>
///     Manages a <see cref="Camera" /> and determines how world coordinates
///     are mapped to and from the screen.
///     Extending classes should initialise <see cref="Camera" /> to avoid
///     causing exceptions.
/// </summary>
public abstract class Viewport
{
    private Vector3 _tmp = Vector3.Zero;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    protected Viewport( Camera camera ) => Camera = camera;

    /// <summary>
    ///     Returns the left gutter (black bar) width in screen coordinates.
    /// </summary>
    public virtual int LeftGutterWidth => ScreenX;

    /// <summary>
    ///     Returns the right gutter (black bar) x in screen coordinates.
    /// </summary>
    public virtual int RightGutterX => ScreenX + ScreenWidth;

    /// <summary>
    ///     Returns the right gutter (black bar) width in screen coordinates.
    /// </summary>
    public virtual int RightGutterWidth => Gdx.Graphics.Width - ( ScreenX + ScreenWidth );

    /// <summary>
    ///     Returns the bottom gutter (black bar) height in screen coordinates.
    /// </summary>
    public virtual int BottomGutterHeight => ScreenY;

    /// <summary>
    ///     Returns the top gutter (black bar) y in screen coordinates.
    /// </summary>
    public virtual int TopGutterY => ScreenY + ScreenHeight;

    /// <summary>
    ///     Returns the top gutter (black bar) height in screen coordinates.
    /// </summary>
    public virtual int TopGutterHeight => Gdx.Graphics.Height - ( ScreenY + ScreenHeight );

    /// <summary>
    ///     Applies the viewport to the camera and sets the glViewport.
    /// </summary>
    /// <param name="centerCamera">
    ///     If true, the camera position is set to the center of the world.
    /// </param>
    public virtual void Apply( bool centerCamera = false )
    {
        if ( Camera == null )
        {
            throw new NullReferenceException();
        }

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
    ///     Configures this viewport's screen bounds using the specified screen
    ///     size and calls <see cref="Apply(bool)" />. Typically called
    ///     from <see cref="IApplicationListener.Resize(int, int)" /> or
    ///     <see cref="IScreen.Resize(int, int)" />.
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    /// <param name="centerCamera"></param>
    /// <remarks>
    ///     The default implementation only calls <see cref="Apply(bool)" />.
    /// </remarks>
    public virtual void Update( int screenWidth, int screenHeight, bool centerCamera = false ) => Apply( centerCamera );

    /// <summary>
    ///     Transforms the specified screen coordinate to world coordinates.
    /// </summary>
    /// <returns> The vector that was passed in, transformed to world coordinates.</returns>
    /// <see cref="Camera.Unproject(Vector3)" />
    public virtual Vector2 Unproject( Vector2 screenCoords )
    {
        if ( Camera == null )
        {
            throw new NullReferenceException();
        }

        _tmp = new Vector3
        {
            X = screenCoords.X,
            Y = screenCoords.Y,
            Z = 1.0f
        };

        Camera.Unproject( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );

        screenCoords.Set( _tmp.X, _tmp.Y );

        return screenCoords;
    }

    /// <summary>
    ///     Transforms the specified world coordinate to screen coordinates.
    /// </summary>
    /// <returns> The vector that was passed in, transformed to screen coordinates.</returns>
    /// <see cref="Camera.Project(Vector3) " />
    public virtual Vector2 Project( Vector2 worldCoords )
    {
        if ( Camera == null )
        {
            throw new NullReferenceException();
        }

        _tmp.Set( worldCoords.X, worldCoords.Y, 1 );

        Camera.Project( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );
        worldCoords.Set( _tmp.X, _tmp.Y );

        return worldCoords;
    }

    /// <summary>
    ///     Transforms the specified screen coordinate to world coordinates.
    /// </summary>
    /// <returns> The vector that was passed in, transformed to world coordinates.</returns>
    /// <see cref="Camera.Unproject(Vector3)" />
    public virtual Vector3 Unproject( Vector3 screenCoords )
    {
        if ( Camera == null )
        {
            throw new NullReferenceException();
        }

        Camera.Unproject( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );

        return screenCoords;
    }

    /// <summary>
    ///     Transforms the specified world coordinate to screen coordinates.
    /// </summary>
    /// <returns> The vector that was passed in, transformed to screen coordinates. </returns>
    /// <see cref="Camera.Project(Vector3) " />
    public virtual Vector3 Project( Vector3 worldCoords )
    {
        if ( Camera == null )
        {
            throw new NullReferenceException();
        }

        Camera.Project( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );

        return worldCoords;
    }

    /// <summary>
    /// </summary>
    /// <see cref="Camera.GetPickRay(float, float, float, float, float, float) " />
    public virtual Ray GetPickRay( float screenX, float screenY )
    {
        if ( Camera == null )
        {
            throw new NullReferenceException();
        }

        return Camera.GetPickRay(
            screenX,
            screenY,
            ScreenX,
            ScreenY,
            ScreenWidth,
            ScreenHeight
            );
    }

    /// <summary>
    /// </summary>
    public virtual void CalculateScissors( Matrix4 batchTransform,
                                           RectangleShape area,
                                           RectangleShape scissor ) => ScissorStack.CalculateScissors(
        Camera,
        ScreenX,
        ScreenY,
        ScreenWidth,
        ScreenHeight,
        batchTransform,
        area,
        scissor
        );

    /// <summary>
    ///     Transforms a point to real screen coordinates (as opposed to OpenGL
    ///     window coordinates), where the origin is in the top left and the
    ///     the y-axis is pointing downwards.
    /// </summary>
    public virtual Vector2 ToScreenCoordinates( Vector2 worldCoords, Matrix4 transformMatrix )
    {
        if ( Camera == null )
        {
            throw new NullReferenceException();
        }

        _tmp.Set( worldCoords.X, worldCoords.Y, 0 );
        _tmp.Mul( transformMatrix );

        Camera.Project( _tmp, ScreenX, ScreenY, ScreenWidth, ScreenHeight );

        Debug.Assert( Gdx.Graphics != null );

        _tmp.Y = Gdx.Graphics.Height - _tmp.Y;

        worldCoords.X = _tmp.X;
        worldCoords.Y = _tmp.Y;

        return worldCoords;
    }

    /// <summary>
    /// </summary>
    /// <param name="worldWidth"></param>
    /// <param name="worldHeight"></param>
    public virtual void SetWorldSize( float worldWidth, float worldHeight )
    {
        WorldWidth  = worldWidth;
        WorldHeight = worldHeight;
    }

    /// <summary>
    ///     Sets the viewport's position in screen coordinates.
    ///     This is typically set by <see cref="Update(int, int, bool)" />.
    /// </summary>
    public virtual void SetScreenPosition( int screenX, int screenY )
    {
        ScreenX = screenX;
        ScreenY = screenY;
    }

    /// <summary>
    ///     Sets the viewport's size in screen coordinates.
    ///     This is typically set by <see cref="Update(int, int, bool)" />.
    /// </summary>
    public virtual void SetScreenSize( int screenWidth, int screenHeight )
    {
        ScreenWidth  = screenWidth;
        ScreenHeight = screenHeight;
    }

    /// <summary>
    ///     Sets the viewport's bounds in screen coordinates.
    ///     This is typically set by <see cref="Update(int, int, bool)" />.
    /// </summary>
    public virtual void SetScreenBounds( int screenX, int screenY, int screenWidth, int screenHeight )
    {
        ScreenX      = screenX;
        ScreenY      = screenY;
        ScreenWidth  = screenWidth;
        ScreenHeight = screenHeight;
    }

    // ------------------------------------------------------------------------

    #region properties

    public Camera? Camera       { get; set; }
    public float   WorldWidth   { get; set; }
    public float   WorldHeight  { get; set; }
    public int     ScreenX      { get; set; }
    public int     ScreenY      { get; set; }
    public int     ScreenWidth  { get; set; }
    public int     ScreenHeight { get; set; }

    #endregion properties
}
