// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Graphics.OpenGL;

namespace Corelib.LibCore.Graphics.GLUtils;

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
    /// Calls <see cref="GLBindings.glScissor(int, int, int, int)"/>, expecting the
    /// coordinates and sizes given in logical coordinates and automatically
    /// converts them to backbuffer coordinates, which may be bigger on HDPI screens.
    /// </summary>
    public static void GLScissor( int x, int y, int width, int height )
    {
        if ( ( _mode == HdpiMode.Logical )
          && ( ( Gdx.Graphics.Width != Gdx.Graphics.BackBufferWidth )
            || ( Gdx.Graphics.Height != Gdx.Graphics.BackBufferHeight ) ) )
        {
            Gdx.GL.glScissor( ToBackBufferX( x ),
                              ToBackBufferY( y ),
                              ToBackBufferX( width ),
                              ToBackBufferY( height ) );
        }
        else
        {
            Gdx.GL.glScissor( x, y, width, height );
        }
    }

    /// <summary>
    /// Calls <see cref="GLBindings.glViewport(int, int, int, int)"/>, expecting
    /// the coordinates and sizes given in logical coordinates and automatically
    /// converts them to backbuffer coordinates, which may be bigger on HDPI screens.
    /// </summary>
    public static void GLViewport( int x, int y, int width, int height )
    {
        if ( ( _mode == HdpiMode.Logical )
          && ( ( Gdx.Graphics.Width != Gdx.Graphics.BackBufferWidth )
            || ( Gdx.Graphics.Height != Gdx.Graphics.BackBufferHeight ) ) )
        {
            Gdx.GL.glViewport( ToBackBufferX( x ),
                               ToBackBufferY( y ),
                               ToBackBufferX( width ),
                               ToBackBufferY( height ) );
        }
        else
        {
            Gdx.GL.glViewport( x, y, width, height );
        }
    }

    /// <summary>
    /// Converts an x-coordinate given in backbuffer coordinates to
    /// logical screen coordinates.
    /// </summary>
    public static int ToLogicalX( int backBufferX )
    {
        return ( int ) ( ( backBufferX * Gdx.Graphics.Width ) / ( float ) Gdx.Graphics.BackBufferWidth );
    }

    /// <summary>
    /// Convers an y-coordinate given in backbuffer coordinates to
    /// logical screen coordinates
    /// </summary>
    public static int ToLogicalY( int backBufferY )
    {
        return ( int ) ( ( backBufferY * Gdx.Graphics.Height ) / ( float ) Gdx.Graphics.BackBufferHeight );
    }

    /// <summary>
    /// Converts an x-coordinate given in logical screen coordinates to
    /// backbuffer coordinates.
    /// </summary>
    public static int ToBackBufferX( int logicalX )
    {
        return ( int ) ( ( logicalX * Gdx.Graphics.BackBufferWidth ) / ( float ) Gdx.Graphics.Width );
    }

    /// <summary>
    /// Convers an y-coordinate given in backbuffer coordinates to
    /// logical screen coordinates
    /// </summary>
    public static int ToBackBufferY( int logicalY )
    {
        return ( int ) ( ( logicalY * Gdx.Graphics.BackBufferHeight ) / ( float ) Gdx.Graphics.Height );
    }
}
