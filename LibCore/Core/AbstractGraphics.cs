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


using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Graphics.GLUtils;

namespace LibGDXSharp.LibCore.Core;

using BufferFormatDescriptor = IGraphics.BufferFormatDescriptor;

[PublicAPI]
public abstract class AbstractGraphics : IGraphics
{
    #region implemented methods

    public float GetRawDeltaTime()    => DeltaTime;
    public float GetDensity()         => GetPpiX() / 160f;
    public float GetBackBufferScale() => BackBufferWidth / ( float )Width;

    #endregion implemented methods

    #region properties

    public BufferFormatDescriptor BufferFormat        { get; set; } = null!;
    public int                    BackBufferWidth     { get; protected set; }
    public int                    BackBufferHeight    { get; protected set; }
    public int                    LogicalWidth        { get; set; }
    public int                    LogicalHeight       { get; set; }
    public int                    Width               { get; }
    public int                    Height              { get; }
    public IGL20?                 GL20                { get; set; }
    public IGL30?                 GL30                { get; set; }
    public float                  DeltaTime           { get; set; }
    public GLVersion              GLVersion           { get; set; } = null!;
    public bool                   ContinuousRendering { get; set; } = true;

    #endregion properties

    #region abstract methods

    // ========================================================================
    // Abstract methods because C# insists this is done to fulfill the contract
    // between the class and interface, which just makes everything annoying tbh.

    public abstract IGraphics.MonitorDescriptor GetPrimaryMonitor();

    public abstract IGraphics.MonitorDescriptor GetMonitor();

    public abstract IGraphics.MonitorDescriptor[] GetMonitors();

    public abstract IGraphics.DisplayModeDescriptor[] GetDisplayModes();

    public abstract IGraphics.DisplayModeDescriptor[] GetDisplayModes( IGraphics.MonitorDescriptor monitor );

    public abstract IGraphics.DisplayModeDescriptor GetDisplayMode();

    public abstract IGraphics.DisplayModeDescriptor GetDisplayMode( IGraphics.MonitorDescriptor monitor );

    public abstract bool SetFullscreenMode( IGraphics.DisplayModeDescriptor displayMode );

    public abstract bool SetWindowedMode( int width, int height );

    public abstract void SetTitle( string title );

    public abstract void SetUndecorated( bool undecorated );

    public abstract void SetResizable( bool resizable );

    public abstract void SetVSync( bool vsync );

    public abstract void SetForegroundFps( int fps );

    public abstract bool SupportsExtension( string extension );

    public abstract bool SupportsDisplayModeChange();

    public abstract void RequestRendering();

    public abstract bool IsFullscreen();

    public abstract ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot );

    public abstract void SetCursor( ICursor cursor );

    public abstract void SetSystemCursor( ICursor.SystemCursor systemCursor );

    public abstract bool IsGL30Available();

    public abstract int GetSafeInsetLeft();

    public abstract int GetSafeInsetTop();

    public abstract int GetSafeInsetBottom();

    public abstract int GetSafeInsetRight();

    public abstract long GetFrameId();

    public abstract int GetFramesPerSecond();

    public abstract GLVersion.GLType GetGraphicsType();

    public abstract float GetPpiX();

    public abstract float GetPpiY();

    public abstract float GetPpcX();

    public abstract float GetPpcY();

    #endregion abstract methods
}
