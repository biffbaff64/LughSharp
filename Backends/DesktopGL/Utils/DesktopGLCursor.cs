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

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class DesktopGLCursor : ICursor
{
    public List< DesktopGLCursor >                         cursors       = new();
    public Dictionary< ICursor.SystemCursor, GLFW.Cursor > systemCursors = new();

    public DesktopGLWindow Window     { get; set; }
    public Pixmap          PixmapCopy { get; set; }
    public GLFW.Image      GLFWImage  { get; set; }
    public GLFW.Cursor     GLFWCursor { get; set; }

    public DesktopGLCursor( DesktopGLWindow window, Pixmap pixmap, int xHotspot, int yHotspot )
    {
        this.Window = window;

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

        this.PixmapCopy          = new Pixmap( pixmap.Width, pixmap.Height, Pixmap.Format.RGBA8888 );
        this.PixmapCopy.Blending = Pixmap.BlendTypes.None;
        this.PixmapCopy.DrawPixmap( pixmap, 0, 0 );

        GLFWImage  = new GLFW.Image( PixmapCopy.Width, PixmapCopy.Height, ( IntPtr )PixmapCopy.gdx2DPixmap.basePtr );
        GLFWCursor = Glfw.CreateCursor( GLFWImage, xHotspot, yHotspot );

        cursors.Add( this );
    }

    public void SetSystemCursor( GLFW.Window window, ICursor.SystemCursor systemCursor )
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
        GLFWImage = default( GLFW.Image );
        Glfw.DestroyCursor( GLFWCursor );
    }

    public void Dispose( DesktopGLWindow glWindow )
    {
        for ( var i = cursors.Count - 1; i >= 0; i-- )
        {
            DesktopGLCursor cursor = cursors[ i ];

            if ( cursor.Window.Equals( Window ) )
            {
                cursors.RemoveAt( i );
            }
        }
    }

    public void DisposeSystemCursors()
    {
        foreach ( Cursor systemCursor in systemCursors.Values )
        {
            Glfw.DestroyCursor( systemCursor );
        }

        systemCursors.Clear();
    }
}
