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

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// To deal with HDPI monitors properly, use the glViewport and glScissor functions
/// of this class instead of directly calling OpenGL yourself. The logical coordinate
/// system provided by the operating system may not have the same resolution as the
/// actual drawing surface to which OpenGL draws, also known as the backbuffer. This
/// class will ensure, that you pass the correct values to OpenGL for any function
/// that expects backbuffer coordinates instead of logical coordinates.
/// </summary>
[PublicAPI]
public class HdpiUtils
{
    private static HdpiMode _mode = HdpiMode.Logical;

    /// <summary>
    /// Allows applications to override HDPI coordinate conversion for glViewport and
    /// glScissor calls. This function can be used to ignore the default behavior, for
    /// example when rendering a UI stage to an off-screen framebuffer:
	///
	/// <code>
	///     HdpiUtils.setMode(HdpiMode.Pixels);
	///     fb.begin();
	///     stage.draw();
	///     fb.end();
	///     HdpiUtils.setMode(HdpiMode.Logical);
	/// </code>
    /// </summary>
	/// <param name="mode">
	/// set to HdpiMode.Pixels to ignore HDPI conversion for glViewport and glScissor functions
	/// </param>
    public static void SetMode( HdpiMode mode )
    {
        _mode = mode;
    }

    /// <summary>
    /// Calls <see cref="IGL20.GLScissor(int, int, int, int)"/>, expecting the
    /// coordinates and sizes given in logical coordinates and automatically
    /// converts them to backbuffer coordinates, which may be bigger on HDPI screens.
    /// </summary>
    public static void GLScissor( int x, int y, int width, int height )
    {
        if ( ( _mode == HdpiMode.Logical )
             && ( ( Gdx.Graphics.Width != Gdx.Graphics.BackBufferWidth )
                  || (Gdx.Graphics.Height != Gdx.Graphics.BackBufferHeight ) ) )
        {
            Gdx.GL.GLScissor( ToBackBufferX( x ), ToBackBufferY( y ),
                              ToBackBufferX( width ), ToBackBufferY( height ) );
        }
        else
        {
            Gdx.GL.GLScissor( x, y, width, height );
        }
    }

    /// <summary>
    /// Calls <see cref="IGL20.GLViewport(int, int, int, int)"/>, expecting
    /// the coordinates and sizes given in logical coordinates and automatically
    /// converts them to backbuffer coordinates, which may be bigger on HDPI screens.
    /// </summary>
    public static void GLViewport( int x, int y, int width, int height )
    {
        if ( ( _mode == HdpiMode.Logical )
             && ( ( Gdx.Graphics.Width != Gdx.Graphics.BackBufferWidth )
                  || ( Gdx.Graphics.Height != Gdx.Graphics.BackBufferHeight ) ) )
        {
            Gdx.GL.GLViewport( ToBackBufferX( x ), ToBackBufferY( y ),
                               ToBackBufferX( width ), ToBackBufferY( height ) );
        }
        else
        {
            Gdx.GL.GLViewport( x, y, width, height );
        }
    }

    /// <summary>
	/// Converts an x-coordinate given in backbuffer coordinates to
	/// logical screen coordinates.
    /// </summary>
    public static int ToLogicalX( int backBufferX )
    {
        return ( int )( ( backBufferX * Gdx.Graphics.Width ) / ( float )Gdx.Graphics.BackBufferWidth );
    }

    /// <summary>
    /// Convers an y-coordinate given in backbuffer coordinates to
    /// logical screen coordinates
    /// </summary>
    public static int ToLogicalY( int backBufferY )
    {
        return ( int )( ( backBufferY * Gdx.Graphics.Height ) / ( float )Gdx.Graphics.BackBufferHeight );
    }

    /// <summary>
    /// Converts an x-coordinate given in logical screen coordinates to
    /// backbuffer coordinates.
    /// </summary>
    public static int ToBackBufferX( int logicalX )
    {
        return ( int )( ( logicalX * Gdx.Graphics.BackBufferWidth ) / ( float )Gdx.Graphics.Width );
    }

    /// <summary>
    /// Convers an y-coordinate given in backbuffer coordinates to
    /// logical screen coordinates
    /// </summary>
    public static int ToBackBufferY( int logicalY )
    {
        return ( int )( ( logicalY * Gdx.Graphics.BackBufferHeight ) / ( float )Gdx.Graphics.Height );
    }
}