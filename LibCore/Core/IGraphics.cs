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


namespace LughSharp.LibCore.Core;

[PublicAPI]
public interface IGraphics
{
    #region nested classes

    /// <summary>
    ///     Describes a fullscreen display mode, having the properties <see cref="Width" />,
    ///     <see cref="Height" />, <see cref="RefreshRate" />, and <see cref="BitsPerPixel" />.
    /// </summary>
    [PublicAPI]
    public class DisplayMode
    {
        public int Width        { get; set; }
        public int Height       { get; set; }
        public int RefreshRate  { get; set; }
        public int BitsPerPixel { get; set; }

        /// <summary>
        ///     Creates a new DisplayMode object, using the specified width, height,
        ///     refresh rate and bits per pixel values.
        /// </summary>
        /// <param name="width"> Width of this display mode in pixels. </param>
        /// <param name="height"> Height of this display mode in pixels. </param>
        /// <param name="refreshRate"> The refresh rate. </param>
        /// <param name="bitsPerPixel"> Bits per Pixel. </param>
        public DisplayMode( int width, int height, int refreshRate, int bitsPerPixel )
        {
            Width        = width;
            Height       = height;
            RefreshRate  = refreshRate;
            BitsPerPixel = bitsPerPixel;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Width}x{Height}, bpp: {BitsPerPixel}, hz: {RefreshRate}";
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Describes a monitor, with X, Y, and Name properties.
    /// </summary>

    //TODO: This may no longer be needed when GL is properly implemented
    [PublicAPI]
    public class GdxMonitor
    {
        public int     VirtualX { get; set; }
        public int     VirtualY { get; set; }
        public string? Name     { get; set; }

        public GdxMonitor( int x, int y, string name )
        {
            VirtualX = x;
            VirtualY = y;
            Name     = name;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Class describing the bits per pixel, depth buffer precision,
    ///     stencil precision and number of MSAA samples.
    /// </summary>
    [PublicAPI]
    public class BufferFormatDescriptor
    {
        public int R       { get; set; } // number of bits per color channel.
        public int G       { get; set; } // ...
        public int B       { get; set; } // ...
        public int A       { get; set; } // ...
        public int Depth   { get; set; } // number of bits for depth and stencil buffer.
        public int Stencil { get; set; } // ...
        public int Samples { get; set; } // number of samples for multi-sample anti-aliasing (MSAA).

        /// <summary>
        ///     Whether coverage sampling anti-aliasing is used. If so, you have
        ///     to clear the coverage buffer as well!
        /// </summary>
        public bool CoverageSampling { get; set; }

        public override string ToString()
        {
            return $"r - {R}, g - {G}, b - {B}, a - {A}, depth - {Depth}, stencil - "
                 + $"{Stencil}, num samples - {Samples}, coverage sampling - {CoverageSampling}";
        }
    }

    #endregion nested classes

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region properties

    GLVersion              GLVersion    { get; set; }
    GLVersion.GLType       GraphicsType { get; }
    BufferFormatDescriptor BufferFormat { get; set; }

    float DeltaTime        { get; set; }
    int   Width            { get; }
    int   Height           { get; }
    int   BackBufferWidth  { get; }
    int   BackBufferHeight { get; }
    bool  IsFullscreen     { get; }

    #endregion properties

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region methods

    bool IsGL30Available();
    
    /// <summary>
    ///     Returns the amount of pixels per logical pixel (point).
    /// </summary>
    float GetBackBufferScale();

    int GetSafeInsetLeft();

    int GetSafeInsetTop();

    int GetSafeInsetBottom();

    int GetSafeInsetRight();

    long GetFrameID();

    int GetFramesPerSecond();

    /// <summary>
    ///     Returns the time span between the current frame and the last frame in seconds, without smoothing.
    /// </summary>
    float GetRawDeltaTime();

    (float X, float Y) GetPpcXY();

    (float X, float Y) GetPpiXY();

    float GetPpiX();

    float GetPpiY();

    float GetPpcX();

    float GetPpcY();

    /// <summary>
    ///     This is a scaling factor for the Density Independent Pixel
    ///     unit, following the convention where one DIP is one pixel on
    ///     an approximately 160 dpi screen. Thus on a 160dpi screen this
    ///     density value will be 1; on a 120 dpi screen it would be .75; etc.
    /// </summary>
    /// <returns>the Density Independent Pixel factor of the display.</returns>
    float GetDensity();

    bool SupportsDisplayModeChange();

    DisplayMode[] GetDisplayModes();

    DisplayMode[] GetDisplayModes( GdxMonitor gdxMonitor );

    DisplayMode GetDisplayMode();

    DisplayMode GetDisplayMode( GdxMonitor gdxMonitor );

    bool SetFullscreenMode( DisplayMode displayMode );

    bool SetWindowedMode( int width, int height );

    void SetTitle( string title );

    void SetUndecorated( bool undecorated );

    void SetResizable( bool resizable );

    void SetVSync( bool vsync );

    void SetForegroundFps( int fps );

    bool SupportsExtension( string extension );

    bool ContinuousRendering { get; }

    void RequestRendering();

    #endregion methods
}
