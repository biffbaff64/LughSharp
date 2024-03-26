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


using LughSharp.LibCore.Graphics;

namespace LughSharp.LibCore.Core;

using BufferFormatDescriptor = IGraphics.BufferFormatDescriptor;

[PublicAPI]
public abstract class AbstractGraphics : IGraphics
{
    #region implemented methods

    /// <inheritdoc />
    public float GetRawDeltaTime()
    {
        return DeltaTime;
    }

    /// <inheritdoc />
    public float GetDensity()
    {
        return GetPpiX() / 160f;
    }

    /// <inheritdoc />
    public float GetBackBufferScale()
    {
        return BackBufferWidth / ( float )Width;
    }

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

    [Obsolete]
    public abstract IGraphics.MonitorDescriptor GetPrimaryMonitor();

    [Obsolete]
    public abstract IGraphics.MonitorDescriptor GetMonitor();

    [Obsolete]
    public abstract IGraphics.MonitorDescriptor[] GetMonitors();

    // ------------------------------------------------------------------------
    
    /// <inheritdoc />
    public abstract IGraphics.DisplayModeDescriptor[] GetDisplayModes();

    /// <inheritdoc />
    public abstract IGraphics.DisplayModeDescriptor[] GetDisplayModes( IGraphics.MonitorDescriptor monitor );

    /// <inheritdoc />
    public abstract IGraphics.DisplayModeDescriptor GetDisplayMode();

    /// <inheritdoc />
    public abstract IGraphics.DisplayModeDescriptor GetDisplayMode( IGraphics.MonitorDescriptor monitor );

    /// <inheritdoc />
    public abstract bool SetFullscreenMode( IGraphics.DisplayModeDescriptor displayMode );

    /// <inheritdoc />
    public abstract bool SetWindowedMode( int width, int height );

    /// <inheritdoc />
    public abstract void SetTitle( string title );

    /// <inheritdoc />
    public abstract void SetUndecorated( bool undecorated );

    /// <inheritdoc />
    public abstract void SetResizable( bool resizable );

    /// <inheritdoc />
    public abstract void SetVSync( bool vsync );

    /// <inheritdoc />
    public abstract void SetForegroundFps( int fps );

    /// <inheritdoc />
    public abstract bool SupportsExtension( string extension );

    /// <inheritdoc />
    public abstract bool SupportsDisplayModeChange();

    /// <inheritdoc />
    public abstract void RequestRendering();

    /// <inheritdoc />
    public abstract bool IsFullscreen();

    /// <inheritdoc />
    public abstract ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot );

    /// <inheritdoc />
    public abstract void SetCursor( ICursor cursor );

    /// <inheritdoc />
    public abstract void SetSystemCursor( ICursor.SystemCursor systemCursor );

    /// <inheritdoc />
    public abstract bool IsGL30Available();

    /// <inheritdoc />
    public abstract int GetSafeInsetLeft();

    /// <inheritdoc />
    public abstract int GetSafeInsetTop();

    /// <inheritdoc />
    public abstract int GetSafeInsetBottom();

    /// <inheritdoc />
    public abstract int GetSafeInsetRight();

    /// <inheritdoc />
    public abstract long GetFrameId();

    /// <inheritdoc />
    public abstract int GetFramesPerSecond();

    /// <inheritdoc />
    public abstract GLVersion.GLType GetGraphicsType();

    /// <inheritdoc />
    public abstract float GetPpiX();

    /// <inheritdoc />
    public abstract (float X, float Y) GetPpcXY();

    /// <inheritdoc />
    public abstract float GetPpiY();

    /// <inheritdoc />
    public abstract float GetPpcX();

    /// <inheritdoc />
    public abstract float GetPpcY();

    #endregion abstract methods
}
