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

namespace LibGDXSharp.Core;

[PublicAPI]
public interface IGraphics
{

    #region nested classes

    /// <summary>
    /// Describes a fullscreen display mode.
    /// </summary>
    [PublicAPI]
    public class DisplayModeDescriptor
    {
        public int Width        { get; set; }
        public int Height       { get; set; }
        public int RefreshRate  { get; set; }
        public int BitsPerPixel { get; set; }

        public DisplayModeDescriptor( int width, int height, int refreshRate, int bitsPerPixel )
        {
            this.Width        = width;
            this.Height       = height;
            this.RefreshRate  = refreshRate;
            this.BitsPerPixel = bitsPerPixel;
        }

        public override string ToString()
        {
            return $"{Width}x{Height}, bpp: {BitsPerPixel}, hz: {RefreshRate}";
        }
    }

    /// <summary>
    /// Describes a monitor, with X, Y, and Name properties.
    /// </summary>
    [PublicAPI]
    public class MonitorDescriptor
    {
        public int          VirtualX      { get; set; }
        public int          VirtualY      { get; set; }
        public string?      Name          { get; set; }
//        public GLFW.Monitor MonitorHandle { get; set; }

        public MonitorDescriptor( int x, int y, string name )
        {
            this.VirtualX = x;
            this.VirtualY = y;
            this.Name     = name;
        }
    }

    /// <summary>
    /// Class describing the bits per pixel, depth buffer precision,
    /// stencil precision and number of MSAA samples.
    /// </summary>
    [PublicAPI]
    public record BufferFormatDescriptor
    {
        public int R       { get; set; } // number of bits per color channel.
        public int G       { get; set; } // ...
        public int B       { get; set; } // ...
        public int A       { get; set; } // ...
        public int Depth   { get; set; } // number of bits for depth and stencil buffer.
        public int Stencil { get; set; } // ...
        public int Samples { get; set; } // number of samples for multi-sample anti-aliasing (MSAA).

        // whether coverage sampling anti-aliasing is used.
        // If so, you have to clear the coverage buffer as well!
        public bool CoverageSampling { get; set; }

        public override string ToString()
        {
            return $"r - {R}, g - {G}, b - {B}, a - {A}, depth - {Depth}, stencil - "
                 + $"{Stencil}, num samples - {Samples}, coverage sampling - {CoverageSampling}";
        }
    }

    #endregion nested classes

    #region properties

    IGL20?                 GL20             { get; set; }
    IGL30?                 GL30             { get; set; }
    float                  DeltaTime        { get; set; }
    GLVersion              GLVersion        { get; set; }
    int                    Width            { get; }
    int                    Height           { get; }
    BufferFormatDescriptor BufferFormat     { get; set; }
    int                    BackBufferWidth  { get; }
    int                    BackBufferHeight { get; }

    #endregion properties

    #region methods

    /// <summary>
    /// Returns whether OpenGL ES 3.0 is available.
    /// If it is you can get an instance of GL30 via GetGL30() to access
    /// OpenGL ES 3.0 functionality. Note that this functionality will
    /// only be available if you instructed the Application instance
    /// to use OpenGL ES 3.0!
    /// </summary>
    /// <returns>TRUE if available.</returns>
    bool IsGL30Available();

    float GetBackBufferScale();

    int GetSafeInsetLeft();

    int GetSafeInsetTop();

    int GetSafeInsetBottom();

    int GetSafeInsetRight();

    long GetFrameId();

    int GetFramesPerSecond();

    GLVersion.GLType GetGraphicsType();

    float GetPpiX();

    float GetPpiY();

    float GetPpcX();

    float GetPpcY();

    /// <summary>
    /// This is a scaling factor for the Density Independent Pixel
    /// unit, following the convention where one DIP is one pixel on
    /// an approximately 160 dpi screen. Thus on a 160dpi screen this
    /// density value will be 1; on a 120 dpi screen it would be .75; etc.
    /// </summary>
    /// <returns>the Density Independent Pixel factor of the display.</returns>
    float GetDensity();

    bool SupportsDisplayModeChange();

    MonitorDescriptor GetPrimaryMonitor();

    MonitorDescriptor GetMonitor();

    MonitorDescriptor[] GetMonitors();

    DisplayModeDescriptor[] GetDisplayModes();

    DisplayModeDescriptor[] GetDisplayModes( MonitorDescriptor monitor );

    DisplayModeDescriptor GetDisplayMode();

    DisplayModeDescriptor GetDisplayMode( MonitorDescriptor monitor );

    bool SetFullscreenMode( DisplayModeDescriptor displayMode );

    bool SetWindowedMode( int width, int height );

    void SetTitle( string title );

    void SetUndecorated( bool undecorated );

    void SetResizable( bool resizable );

    void SetVSync( bool vsync );

    void SetForegroundFps( int fps );

    bool SupportsExtension( string extension );

    bool ContinuousRendering { get; }

    void RequestRendering();

    /// <summary>
    /// Whether the app is full screen or not.
    /// </summary>
    bool IsFullscreen();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pixmap"></param>
    /// <param name="xHotspot"></param>
    /// <param name="yHotspot"></param>
    /// <returns></returns>
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
    /// <param name="systemCursor">The system cursor to use.</param>
    void SetSystemCursor( ICursor.SystemCursor systemCursor );

    #endregion methods

}
