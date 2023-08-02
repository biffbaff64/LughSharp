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

using Monitor = LibGDXSharp.Graphics.Monitor;

namespace LibGDXSharp.Backends.Desktop;

public class GLGraphics : AbstractGraphics, IDisposable
{
    public GLWindow? Window { get; set; }

    private IGL20? _gl20;
    private IGL30? _gl30;

    public GLGraphics( GLWindow window )
    {
        this.Window = window;

//        if ( window.GetConfig().useGL30 )
//        {
//            this._gl30 = new GL30();
//            this._gl20 = this._gl30;
//        }
//        else
        {
            this._gl20 = new GL20();
            this._gl30 = null;
        }

        UpdateFramebufferInfo();
        InitiateGL();

//        Glfw.GetApi().SetFramebufferSizeCallback( window.WindowHandle, resizeCallback );
    }

    [SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
    public class GLDisplayMode : DisplayMode
    {
        public long MonitorHandle { get; set; }

        public GLDisplayMode( long monitor, int width, int height, int refreshRate, int bitsPerPixel )
            : base( width, height, refreshRate, bitsPerPixel )
        {
            this.MonitorHandle = monitor;
        }
    }

    private void UpdateFramebufferInfo()
    {
    }

    private void InitiateGL()
    {
    }

    public override bool SupportsDisplayModeChange() => false;

    public override Monitor GetPrimaryMonitor() => default;

    public override Monitor GetMonitor() => default;

    public override Monitor[] GetMonitors()
    {
        return new Monitor[]
        {
        };
    }

    public override DisplayMode[] GetDisplayModes()
    {
        return new DisplayMode[]
        {
        };
    }

    public override DisplayMode[] GetDisplayModes( Monitor monitor )
    {
        return new DisplayMode[]
        {
        };
    }

    public override DisplayMode GetDisplayMode() => default;

    public override DisplayMode GetDisplayMode( Monitor monitor ) => default;

    public override bool SetFullscreenMode( DisplayMode displayMode ) => false;

    public override bool SetWindowedMode( int width, int height ) => false;

    public override void SetTitle( string title )
    {
    }

    public override void SetUndecorated( bool undecorated )
    {
    }

    public override void SetResizable( bool resizable )
    {
    }

    public override void SetVSync( bool vsync )
    {
    }

    public override void SetForegroundFps( int fps )
    {
    }

    public override BufferFormat GetBufferFormat() => null;

    public override bool SupportsExtension( string extension ) => false;

    public override void SetContinuousRendering( bool isContinuous )
    {
    }

    public override bool IsContinuousRendering() => false;

    public override void RequestRendering()
    {
    }

    /// <summary>Whether the app is full screen or not.</summary>
    public override bool IsFullscreen() => false;

    /// <summary>
    /// </summary>
    /// <param name="pixmap"></param>
    /// <param name="xHotspot"></param>
    /// <param name="yHotspot"></param>
    /// <returns></returns>
    public override ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot ) => null;

    /// <summary>
    /// Only viable on the lwjgl-backend and on the gwt-backend.
    /// Browsers that support cursor:url() and support the png format (the pixmap
    /// is converted to a data-url of type image/png) should also support custom
    /// cursors. Will set the mouse cursor image to the image represented by the
    /// Cursor. It is recommended to call this function in the main render thread,
    /// and maximum one time per frame.
    /// </summary>
    /// <param name="cursor">The mouse cursor as a <see cref="T:LibGDXSharp.Graphics.ICursor" /></param>
    public override void SetCursor( ICursor cursor )
    {
    }

    /// <summary>
    /// Sets one of the predefined <see cref="T:LibGDXSharp.Graphics.ICursor.SystemCursor" />s.
    /// </summary>
    /// <param name="systemCursor">The system cursor to use.</param>
    public override void SetSystemCursor( ICursor.SystemCursor systemCursor )
    {
    }

    /// <summary>
    /// Returns whether OpenGL ES 3.0 is available.
    /// If it is you can get an instance of GL30 via GetGL30() to access
    /// OpenGL ES 3.0 functionality. Note that this functionality will
    /// only be available if you instructed the Application instance
    /// to use OpenGL ES 3.0!
    /// </summary>
    /// <returns>TRUE if available.</returns>
    public override bool IsGL30Available() => false;

    /// <summary>
    /// Returns the <see cref="T:LibGDXSharp.Graphics.IGL20" /> instance.
    /// </summary>
    /// <remarks>MAY be replaced by a property.</remarks>
    public override IGL20 GetGL20() => null;

    /// <summary>
    /// Returns the <see cref="T:LibGDXSharp.Graphics.IGL30" /> instance or null if unsupported.
    /// </summary>
    /// <remarks>MAY be replaced by a property.</remarks>
    public override IGL30? GetGL30() => null;

    /// <summary>Sets the IGL20 instance.</summary>
    /// <remarks>MAY be replaced by a property.</remarks>
    public override void SetGL20( IGL20 gl20 )
    {
    }

    /// <summary>Sets the IGL30 instance.</summary>
    /// <remarks>MAY be replaced by a property.</remarks>
    public override void SetGL30( IGL30 gl30 )
    {
    }

    public override int Width                { get; }
    public override int Height               { get; }
    public override int GetBackBufferWidth() => 0;

    public override int GetBackBufferHeight() => 0;

    public override int GetSafeInsetLeft() => 0;

    public override int GetSafeInsetTop() => 0;

    public override int GetSafeInsetBottom() => 0;

    public override int GetSafeInsetRight() => 0;

    public override long GetFrameId() => 0;

    public override float GetDeltaTime() => 0;

    public override int GetFramesPerSecond() => 0;

    public override GraphicsType GetGraphicsType() => GraphicsType.GL2;

    public override GLVersion GetGLVersion() => null;

    public override float GetPpiX() => 0;

    public override float GetPpiY() => 0;

    public override float GetPpcX() => 0;

    public override float GetPpcY() => 0;

    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this._gl20 = null;
            this._gl30 = null;
        }
    }

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }
}