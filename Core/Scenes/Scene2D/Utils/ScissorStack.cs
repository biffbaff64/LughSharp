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

using System.Drawing;

using LibGDXSharp.Maths;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// A stack of {@link Rectangle} objects to be used for clipping via
/// <see cref="IGL20.GLScissor(int, int, int, int)"/>. When a new Rectangle is
/// pushed onto the stack, it will be merged with the current top of stack. The
/// minimum area of overlap is then set as the real top of the stack.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class ScissorStack
{
    private readonly static List< RectangleShape > Scissors = new();
    private readonly static Vector3                Tmp      = new();
    private readonly static RectangleShape         Viewport = new();

    /// <summary>
    /// Pushes a new scissor <see cref="Rectangle"/> onto the stack, merging it with
    /// the current top of the stack. The minimal area of overlap between the top of
    /// stack rectangle and the provided rectangle is pushed onto the stack. This will
    /// invoke <see cref="IGL20.GLScissor(int, int, int, int)"/> with the final top of
    /// stack rectangle. In case no scissor is yet on the stack this will also enable
    /// <see cref="IGL20.GL_SCISSOR_TEST"/> automatically.
    /// <para>
    /// Any drawing should be flushed before pushing scissors.
    /// </para>
    /// </summary>
    /// <returns>
    /// true if the scissors were pushed. false if the scissor area was zero, in this
    /// case the scissors were not pushed and no drawing should occur.
    /// ....</returns>
    public static bool PushScissors( RectangleShape scissor )
    {
        Fix( scissor );

        if ( Scissors.Count == 0 )
        {
            if ( ( scissor.Width < 1 ) || ( scissor.Height < 1 ) ) return false;
            Gdx.GL.GLEnable( IGL20.GL_SCISSOR_TEST );
        }
        else
        {
            // merge scissors
            RectangleShape parent = Scissors[ ^1 ];

            var minX = Math.Max( parent.X, scissor.X );
            var maxX = Math.Min( parent.X + parent.Width, scissor.X + scissor.Width );

            if ( ( maxX - minX ) < 1 ) return false;

            var minY = Math.Max( parent.Y, scissor.Y );
            var maxY = Math.Min( parent.Y + parent.Height, scissor.Y + scissor.Height );

            if ( ( maxY - minY ) < 1 ) return false;

            scissor.X      = minX;
            scissor.Y      = minY;
            scissor.Width  = maxX - minX;
            scissor.Height = Math.Max( 1, maxY - minY );
        }

        Scissors.Add( scissor );
        HdpiUtils.GLScissor( ( int )scissor.X, ( int )scissor.Y, ( int )scissor.Width, ( int )scissor.Height );

        return true;
    }

    /// <summary>
    /// Pops the current scissor rectangle from the stack and sets the new scissor
    /// area to the new top of stack rectangle. In case no more rectangles are on
    /// the stack, <see cref="IGL20.GL_SCISSOR_TEST"/> is disabled.
    /// <para>
    /// Any drawing should be flushed before popping scissors.
    /// </para>
    /// </summary>
    public static RectangleShape PopScissors()
    {
        RectangleShape old = Scissors.Pop();

        if ( Scissors.Count == 0 )
        {
            Gdx.GL.GLDisable( IGL20.GL_SCISSOR_TEST );
        }
        else
        {
            RectangleShape scissor = Scissors.Peek();
            HdpiUtils.GLScissor( ( int )scissor.X, ( int )scissor.Y, ( int )scissor.Width, ( int )scissor.Height );
        }

        return old;
    }

    public static RectangleShape? PeekScissors()
    {
        return Scissors.Count == 0 ? null : Scissors.Peek();
    }

    private static void Fix( RectangleShape rect )
    {
        rect.X      = (float)Math.Round( rect.X );
        rect.Y      = (float)Math.Round( rect.Y );
        rect.Width  = (float)Math.Round( rect.Width );
        rect.Height = (float)Math.Round( rect.Height );

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
    /// of this method will not be suitable for <see cref="IGL20.GLScissor(int, int, int, int)"/>.
    /// </summary>
    /// <param name="viewportX"></param>
    /// <param name="viewportY"></param>
    /// <param name="viewportWidth"></param>
    /// <param name="viewportHeight"></param>
    /// <param name="camera"> the <see cref="Camera"/> </param>
    /// <param name="batchTransform"> the transformation <seealso cref="Matrix4"/> </param>
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
        Tmp.Set( area.X, area.Y, 0 );
        Tmp.Mul( batchTransform );
        
        camera.Project( Tmp, viewportX, viewportY, viewportWidth, viewportHeight );
        
        scissor.X = Tmp.X;
        scissor.Y = Tmp.Y;

        Tmp.Set( area.X + area.Width, area.Y + area.Height, 0 );
        Tmp.Mul( batchTransform );
        
        camera.Project( Tmp, viewportX, viewportY, viewportWidth, viewportHeight );
        scissor.Width  = Tmp.X - scissor.X;
        scissor.Height = Tmp.Y - scissor.Y;
    }

    /// <summary>
    /// Return the current viewport in OpenGL ES window coordinates based
    /// on the currently applied scissor.
    /// </summary>
    public static RectangleShape GetViewport()
    {
        if ( Scissors.Count == 0 )
        {
            Viewport.Set( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );

            return Viewport;
        }

        RectangleShape scissor = Scissors.Peek();
        Viewport.Set( scissor );

        return Viewport;
    }
}
