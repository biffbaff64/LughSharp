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

using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Graphics.OpenGL;

namespace Corelib.LibCore.Core;

[PublicAPI]
public partial interface IGraphics
{
    #region properties

    GLVersion?             GLVersion             { get; set; }
    GraphicsBackend.Type   GraphicsType          { get; }
    BufferFormatDescriptor BufferFormat          { get; set; }
    IGLBindings            GL                    { get; set; }
    float                  DeltaTime             { get; set; }
    int                    Width                 { get; }
    int                    Height                { get; }
    int                    BackBufferWidth       { get; set; }
    int                    BackBufferHeight      { get; set; }
    bool                   IsFullscreen          { get; }
    Color                  WindowBackgroundColor { get; set; }

    #endregion properties

    // ========================================================================
    // ========================================================================

    #region methods

    /// <summary>
    /// Returns the amount of pixels per logical pixel (point).
    /// </summary>
    float GetBackBufferScale();

    /// <summary>
    /// Returns the time span between the current frame and the last frame in seconds, without smoothing.
    /// </summary>
    float GetRawDeltaTime();

    /// <summary>
    /// This is a scaling factor for the Density Independent Pixel
    /// unit, following the convention where one DIP is one pixel on
    /// an approximately 160 dpi screen. Thus on a 160dpi screen this
    /// density value will be 1; on a 120 dpi screen it would be .75; etc.
    /// </summary>
    /// <returns>the Density Independent Pixel factor of the display.</returns>
    float GetDensity();

    int GetSafeInsetLeft();
    int GetSafeInsetTop();
    int GetSafeInsetBottom();
    int GetSafeInsetRight();

    long GetFrameID();
    int GetFramesPerSecond();

    (float X, float Y) GetPpcXY();
    (float X, float Y) GetPpiXY();

    bool SupportsDisplayModeChange();
    bool SupportsExtension( string extension );

    DisplayMode[] GetDisplayModes();
    DisplayMode GetDisplayMode();
    DisplayMode[] GetDisplayModes( DotGLFW.Monitor monitor );
    DisplayMode GetDisplayMode( DotGLFW.Monitor monitor );

    bool SetFullscreenMode( DisplayMode displayMode );

    bool SetWindowedMode( int width, int height );
    void SetUndecorated( bool undecorated );
    void SetResizable( bool resizable );

    void SetVSync( bool vsync );
    void SetForegroundFps( int fps );

    bool ContinuousRendering { get; }
    void RequestRendering();

    /// <summary>
    /// Create a new cursor represented by the <see cref="Pixmap"/>. The Pixmap must be
    /// in RGBA8888 format, Width &amp; height must be powers-of-two greater than zero (not
    /// necessarily equal) and of a certain minimum size (32x32 is a safe bet), and alpha
    /// transparency must be single-bit (i.e., 0x00 or 0xFF only).
    /// <para>
    /// This function returns a Cursor object that can be set as the system cursor
    /// by calling <see cref="SetCursor(ICursor)"/>.
    /// </para>
    /// </summary>
    /// <param name="pixmap"> the mouse cursor image as a <see cref="Pixmap"/>. </param>
    /// <param name="xHotspot">
    /// The x location of the hotspot pixel within the cursor image (origin top-left corner)
    /// </param>
    /// <param name="yHotspot">
    /// The y location of the hotspot pixel within the cursor image (origin top-left corner)
    /// </param>
    /// <returns>
    /// a cursor object that can be used by calling <see cref="SetCursor(ICursor)"/> or null
    /// if not supported
    /// </returns>
    ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot );

    /// <summary>
    /// Only viable on the lwjgl-backend and on the gwt-backend.
    /// Browsers that support cursor:url() and support the png format (the pixmap
    /// is converted to a data-url of type image/png) should also support custom
    /// cursors. Will set the mouse cursor image to the image represented by the
    /// Cursor. It is recommended to call this function in the main render thread,
    /// and maximum one time per frame.
    /// </summary>
    /// <param name="cursor">The mouse cursor as a <see cref="ICursor"/></param>
    void SetCursor( ICursor cursor );

    /// <summary>
    /// Sets one of the predefined <see cref="ICursor.SystemCursor"/>s.
    /// </summary>
    /// <param name="systemCursor"> The system cursor to use. </param>
    void SetSystemCursor( ICursor.SystemCursor systemCursor );

    #endregion methods
}