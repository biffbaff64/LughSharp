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

using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Utils.Exceptions;

using DesktopGLBackend.Window;

namespace DesktopGLBackend.Utils;

[PublicAPI]
public unsafe class DesktopGLCursor : ICursor, IDisposable
{
    public static readonly List< DesktopGLCursor >                    Cursors       = new();
    public static readonly Dictionary< ICursor.SystemCursor, Cursor > SystemCursors = new();

    public DesktopGLWindow Window     { get; set; }
    public Pixmap          PixmapCopy { get; set; }
    public Image           GlfwImage  { get; set; }
    public GLFW.Cursor     GlfwCursor { get; set; }

    // ========================================================================
    // ========================================================================

    public DesktopGLCursor( DesktopGLWindow window, Pixmap pixmap, int xHotspot, int yHotspot )
    {
        Window = window;

        if ( pixmap.Format != Pixmap.ColorFormat.RGBA8888 )
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

        PixmapCopy          = new Pixmap( pixmap.Width, pixmap.Height, Pixmap.ColorFormat.RGBA8888 );
        PixmapCopy.Blending = Pixmap.BlendTypes.None;
        PixmapCopy.DrawPixmap( pixmap, 0, 0 );

        GlfwImage = new Image
        {
            Pixels = PixmapCopy.PixelData,
            Width  = PixmapCopy.Width,
            Height = PixmapCopy.Height,
        };

        GlfwCursor = Glfw.CreateCursor( GlfwImage, xHotspot, yHotspot );

        Cursors.Add( this );
    }

    /// <summary>
    /// Sets the system cursor for the given <see cref="GLFW.Window"/> to the
    /// cursor specified by the parameter <paramref name="systemCursor"/>.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="systemCursor"></param>
    public static void SetSystemCursor( GLFW.Window window, ICursor.SystemCursor systemCursor )
    {
        //@formatter:off
        var glCursor = systemCursor switch
        {
            ICursor.SystemCursor.Ibeam            => Glfw.CreateStandardCursor( CursorShape.Ibeam ),
            ICursor.SystemCursor.Crosshair        => Glfw.CreateStandardCursor( CursorShape.Crosshair ),
            ICursor.SystemCursor.Hand             => Glfw.CreateStandardCursor( CursorShape.PointingHand ),
            ICursor.SystemCursor.HorizontalResize => Glfw.CreateStandardCursor( CursorShape.Hresize ),
            ICursor.SystemCursor.VerticalResize   => Glfw.CreateStandardCursor( CursorShape.Vresize ),
            var _                                 => Glfw.CreateStandardCursor( CursorShape.Arrow ),
        };
        //@formatter:on

        SystemCursors[ systemCursor ] = glCursor;

        Glfw.SetCursor( window, glCursor );
    }

    // ========================================================================

    #region Dispose methods

    public void Dispose()
    {
        if ( PixmapCopy == null )
        {
            throw new GdxRuntimeException( "Cursor already disposed" );
        }

        Cursors.Remove( this );
        PixmapCopy.Dispose();
        PixmapCopy = null!;
        GlfwImage  = default( Image )!;
        Glfw.DestroyCursor( GlfwCursor );
    }

    public static void Dispose( DesktopGLWindow glWindow )
    {
        for ( var i = Cursors.Count - 1; i >= 0; i-- )
        {
            var cursor = Cursors[ i ];

            if ( cursor.Window.Equals( glWindow ) )
            {
                Cursors.RemoveAt( i );
            }
        }
    }

    public static void DisposeSystemCursors()
    {
        foreach ( var systemCursor in SystemCursors.Values )
        {
            Glfw.DestroyCursor( systemCursor );
        }

        SystemCursors.Clear();
    }

    #endregion Dispose methods
}