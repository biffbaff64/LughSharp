// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LibGDXSharp.Backends.DesktopGL.Window;
using LibGDXSharp.LibCore.Graphics;

namespace LibGDXSharp.Backends.DesktopGL.Utils;

public class DesktopGLCursor : ICursor
{
    public static List< DesktopGLCursor >                    cursors       = new();
    public static Dictionary< ICursor.SystemCursor, Cursor > systemCursors = new();

    public DesktopGLCursor( DesktopGLWindow window, Pixmap pixmap, int xHotspot, int yHotspot )
    {
        Window = window;

        if ( pixmap.GetFormat() != Pixmap.Format.RGBA8888 )
        {
            throw new GdxRuntimeException( "Cursor image pixmap should be in RGBA8888 format." );
        }

        if ( ( pixmap.Width & ( pixmap.Width - 1 ) ) != 0 )
        {
            throw new GdxRuntimeException( $"Cursor image pixmap width of {pixmap.Width} is "
                                         + $"not a power-of-two greater than zero." );
        }

        if ( ( pixmap.Height & ( pixmap.Height - 1 ) ) != 0 )
        {
            throw new GdxRuntimeException( $"Cursor image pixmap height of {pixmap.Height} "
                                         + $"is not a power-of-two greater than zero." );
        }

        if ( ( xHotspot < 0 ) || ( xHotspot >= pixmap.Width ) )
        {
            throw new GdxRuntimeException( $"xHotspot coordinate of {xHotspot} is not within "
                                         + $"image width bounds: [0, {pixmap.Width})." );
        }

        if ( ( yHotspot < 0 ) || ( yHotspot >= pixmap.Height ) )
        {
            throw new GdxRuntimeException( $"yHotspot coordinate of {yHotspot} is not within "
                                         + $"image height bounds: [0, {pixmap.Height})." );
        }

        PixmapCopy          = new Pixmap( pixmap.Width, pixmap.Height, Pixmap.Format.RGBA8888 );
        PixmapCopy.Blending = Pixmap.BlendTypes.None;
        PixmapCopy.DrawPixmap( pixmap, 0, 0 );

        GLFWImage  = new Image( PixmapCopy.Width, PixmapCopy.Height, ( IntPtr )PixmapCopy.gdx2DPixmap.basePtr );
        GLFWCursor = Glfw.CreateCursor( GLFWImage, xHotspot, yHotspot );

        cursors.Add( this );
    }

    public DesktopGLWindow Window     { get; set; }
    public Pixmap          PixmapCopy { get; set; }
    public Image           GLFWImage  { get; set; }
    public Cursor          GLFWCursor { get; set; }

    public static void SetSystemCursor( GLFWWindow window, ICursor.SystemCursor systemCursor )
    {
        //@formatter:off
        Cursor glCursor = systemCursor switch
        {
            ICursor.SystemCursor.Ibeam            => Glfw.CreateStandardCursor( CursorType.Beam ),
            ICursor.SystemCursor.Crosshair        => Glfw.CreateStandardCursor( CursorType.Crosshair ),
            ICursor.SystemCursor.Hand             => Glfw.CreateStandardCursor( CursorType.Hand ),
            ICursor.SystemCursor.HorizontalResize => Glfw.CreateStandardCursor( CursorType.ResizeHorizontal ),
            ICursor.SystemCursor.VerticalResize   => Glfw.CreateStandardCursor( CursorType.ResizeVertical ),
            _                                     => Glfw.CreateStandardCursor( CursorType.Arrow )
        };
        //@formatter:on

        systemCursors[ systemCursor ] = glCursor;

        Glfw.SetCursor( window, glCursor );
    }

    public void Dispose()
    {
        if ( PixmapCopy == null )
        {
            throw new GdxRuntimeException( "Cursor already disposed" );
        }

        cursors.Remove( this );
        PixmapCopy.Dispose();
        PixmapCopy = null!;
        GLFWImage  = default( Image );
        Glfw.DestroyCursor( GLFWCursor );
    }

    public static void Dispose( DesktopGLWindow glWindow )
    {
        for ( var i = cursors.Count - 1; i >= 0; i-- )
        {
            DesktopGLCursor cursor = cursors[ i ];

            if ( cursor.Window.Equals( glWindow ) )
            {
                cursors.RemoveAt( i );
            }
        }
    }

    public static void DisposeSystemCursors()
    {
        foreach ( Cursor systemCursor in systemCursors.Values )
        {
            Glfw.DestroyCursor( systemCursor );
        }

        systemCursors.Clear();
    }
}
