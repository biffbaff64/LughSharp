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

using System.Drawing;
using Corelib.Lugh.Graphics.Cameras;
using Corelib.Lugh.Graphics.GLUtils;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils.Collections;

using Matrix4 = Corelib.Lugh.Maths.Matrix4;

namespace Corelib.Lugh.Scenes.Scene2D.Utils;

/// <summary>
/// A stack of {@link Rectangle} objects to be used for clipping via
/// <see cref="GLBindings.glScissor(int, int, int, int)"/>. When a new Rectangle is
/// pushed onto the stack, it will be merged with the current top of stack. The
/// minimum area of overlap is then set as the real top of the stack.
/// </summary>
[PublicAPI]
public class ScissorStack
{
    private static readonly List< RectangleShape > _scissors = [ ];
    private static readonly Vector3                _tmp      = new();
    private static readonly RectangleShape         _viewport = new();

    /// <summary>
    /// Pushes a new scissor <see cref="Rectangle"/> onto the stack, merging it with
    /// the current top of the stack. The minimal area of overlap between the top of
    /// stack rectangle and the provided rectangle is pushed onto the stack. This will
    /// invoke <see cref="GLBindings.glScissor(int, int, int, int)"/> with the final top of
    /// stack rectangle. In case no scissor is yet on the stack this will also enable
    /// <see cref="IGL.GL_SCISSOR_TEST"/> automatically.
    /// <para>
    /// Any drawing should be flushed before pushing scissors.
    /// </para>
    /// </summary>
    /// <returns>
    /// true if the scissors were pushed. false if the scissor area was zero, in this
    /// case the scissors were not pushed and no drawing should occur.
    /// </returns>
    public static bool PushScissors( RectangleShape scissor )
    {
        Fix( scissor );

        if ( _scissors.Count == 0 )
        {
            if ( ( scissor.Width < 1 ) || ( scissor.Height < 1 ) )
            {
                return false;
            }

            Gdx.GL.Enable( IGL.GL_SCISSOR_TEST );
        }
        else
        {
            // merge scissors
            var parent = _scissors[ ^1 ];

            var minX = Math.Max( parent.X, scissor.X );
            var maxX = Math.Min( parent.X + parent.Width, scissor.X + scissor.Width );

            if ( ( maxX - minX ) < 1 )
            {
                return false;
            }

            var minY = Math.Max( parent.Y, scissor.Y );
            var maxY = Math.Min( parent.Y + parent.Height, scissor.Y + scissor.Height );

            if ( ( maxY - minY ) < 1 )
            {
                return false;
            }

            scissor.X      = minX;
            scissor.Y      = minY;
            scissor.Width  = maxX - minX;
            scissor.Height = Math.Max( 1, maxY - minY );
        }

        _scissors.Add( scissor );

        HdpiUtils.GLScissor( ( int ) scissor.X,
                             ( int ) scissor.Y,
                             ( int ) scissor.Width,
                             ( int ) scissor.Height );

        return true;
    }

    /// <summary>
    /// Pops the current scissor rectangle from the stack and sets the new scissor
    /// area to the new top of stack rectangle. In case no more rectangles are on
    /// the stack, <see cref="IGL.GL_SCISSOR_TEST"/> is disabled.
    /// <para>
    /// Any drawing should be flushed before popping scissors.
    /// </para>
    /// </summary>
    public static RectangleShape PopScissors()
    {
        var old = _scissors.Pop();

        if ( _scissors.Count == 0 )
        {
            Gdx.GL.Disable( IGL.GL_SCISSOR_TEST );
        }
        else
        {
            var scissor = _scissors.Peek();

            HdpiUtils.GLScissor( ( int ) scissor.X,
                                 ( int ) scissor.Y,
                                 ( int ) scissor.Width,
                                 ( int ) scissor.Height );
        }

        return old;
    }

    public static RectangleShape? PeekScissors()
    {
        return _scissors.Count == 0 ? null : _scissors.Peek();
    }

    private static void Fix( RectangleShape rect )
    {
        rect.X      = ( float ) Math.Round( rect.X );
        rect.Y      = ( float ) Math.Round( rect.Y );
        rect.Width  = ( float ) Math.Round( rect.Width );
        rect.Height = ( float ) Math.Round( rect.Height );

        if ( rect.Width < 0 )
        {
            rect.Width =  -rect.Width;
            rect.X     -= rect.Width;
        }

        if ( rect.Height < 0 )
        {
            rect.Height =  -rect.Height;
            rect.Y      -= rect.Height;
        }
    }

    /// <summary>
    /// Calculates a scissor rectangle using 0, 0, Gdx.graphics.getWidth(),
    /// and Gdx.graphics.getHeight() as the viewport.
    /// </summary>
    public static void CalculateScissors( Camera camera,
                                          Matrix4 batchTransform,
                                          RectangleShape area,
                                          RectangleShape scissor )
    {
        CalculateScissors( camera, 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height, batchTransform, area, scissor );
    }

    /// <summary>
    /// Calculates a scissor rectangle in OpenGL ES window coordinates from a <see cref="Camera"/>,
    /// a transformation <see cref="Matrix4"/> and an axis aligned <see cref="RectangleShape"/>.
    /// The rectangle will get transformed by the camera and transform matrices and is then
    /// projected to screen coordinates. Note that only axis aligned rectangles will work with
    /// this method. If either the Camera or the Matrix4 have rotational components, the output
    /// of this method will not be suitable for <see cref="GLBindings.glScissor(int, int, int, int)"/>.
    /// </summary>
    /// <param name="viewportX"></param>
    /// <param name="viewportY"></param>
    /// <param name="viewportWidth"></param>
    /// <param name="viewportHeight"></param>
    /// <param name="camera"> the <see cref="Camera"/> </param>
    /// <param name="batchTransform"> the transformation <see cref="Matrix4"/> </param>
    /// <param name="area"> the <see cref="RectangleShape"/> to transform to window coordinates </param>
    /// <param name="scissor"> the Rectangle to store the result in  </param>
    public static void CalculateScissors( Camera camera,
                                          float viewportX,
                                          float viewportY,
                                          float viewportWidth,
                                          float viewportHeight,
                                          Matrix4 batchTransform,
                                          RectangleShape area,
                                          RectangleShape scissor )
    {
        _tmp.Set( area.X, area.Y, 0 );
        _tmp.Mul( batchTransform );

        camera.Project( _tmp, viewportX, viewportY, viewportWidth, viewportHeight );

        scissor.X = _tmp.X;
        scissor.Y = _tmp.Y;

        _tmp.Set( area.X + area.Width, area.Y + area.Height, 0 );
        _tmp.Mul( batchTransform );

        camera.Project( _tmp, viewportX, viewportY, viewportWidth, viewportHeight );
        scissor.Width  = _tmp.X - scissor.X;
        scissor.Height = _tmp.Y - scissor.Y;
    }

    /// <summary>
    /// Return the current viewport in OpenGL ES window coordinates based
    /// on the currently applied scissor.
    /// </summary>
    public static RectangleShape GetViewport()
    {
        if ( _scissors.Count == 0 )
        {
            _viewport.Set( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );

            return _viewport;
        }

        var scissor = _scissors.Peek();
        _viewport.Set( scissor );

        return _viewport;
    }
}
