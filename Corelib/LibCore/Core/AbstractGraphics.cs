﻿// ///////////////////////////////////////////////////////////////////////////////
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
public abstract class AbstractGraphics : IGraphics
{
    // ------------------------------------------------------------------------

    #region properties

    public IGraphics.BufferFormatDescriptor BufferFormat { get; set; } = null!;
    public IGLBindings                      GL           { get; set; } = null!;

    public virtual int                  BackBufferWidth     { get; protected set; }
    public virtual int                  BackBufferHeight    { get; protected set; }
    public virtual int                  LogicalWidth        { get; set; }
    public virtual int                  LogicalHeight       { get; set; }
    public virtual int                  Width               { get; }
    public virtual int                  Height              { get; }
    public virtual float                DeltaTime           { get; set; }
    public virtual GLVersion?           GLVersion           { get; set; } = null!;
    public virtual GraphicsBackend.Type GraphicsType        { get; set; }
    public virtual bool                 ContinuousRendering { get; set; } = true;
    public virtual bool                 IsFullscreen        { get; }

    #endregion properties

    // ------------------------------------------------------------------------

    #region implemented methods

    /// <summary>
    /// Returns the time span between the current frame and the last frame
    /// in seconds, without smoothing.
    /// </summary>
    public virtual float GetRawDeltaTime()
    {
        return DeltaTime;
    }

    /// <summary>
    /// This is a scaling factor for the Density Independent Pixel unit, following
    /// the convention where one DIP is one pixel on an approximately 160 dpi screen.
    /// Thus on a 160dpi screen this density value will be 1; on a 120 dpi screen it
    /// would be .75; etc.
    /// </summary>
    /// <returns>the Density Independent Pixel factor of the display.</returns>
    public virtual float GetDensity()
    {
        return GetPpiXY().X / 160f;
    }

    /// <summary>
    /// Returns the amount of pixels per logical pixel (point).
    /// </summary>
    public float GetBackBufferScale()
    {
        return BackBufferWidth / ( float ) Width;
    }

    #endregion implemented methods

    // ------------------------------------------------------------------------

    #region abstract methods

    // ------------------------------------------------------------------------
    // Abstract methods because C# insists this is done to fulfill the contract
    // between the class and interface, which just makes everything annoying tbh.

    public abstract IGraphics.DisplayMode[] GetDisplayModes();
    public abstract IGraphics.DisplayMode   GetDisplayMode();
    public abstract IGraphics.DisplayMode[] GetDisplayModes( GLFW.Monitor monitor );
    public abstract IGraphics.DisplayMode   GetDisplayMode( GLFW.Monitor monitor );

    // ------------------------------------------------------------------------
    public abstract bool SetWindowedMode( int width, int height );
    public abstract void SetUndecorated( bool undecorated );
    public abstract void SetResizable( bool resizable );
    public abstract void SetVSync( bool vsync );
    public abstract void SetForegroundFps( int fps );
    public abstract bool SetFullscreenMode( IGraphics.DisplayMode displayMode );

    // ------------------------------------------------------------------------
    public abstract bool SupportsExtension( string extension );
    public abstract bool SupportsDisplayModeChange();
    public abstract void RequestRendering();
    public abstract int  GetSafeInsetLeft();
    public abstract int  GetSafeInsetTop();
    public abstract int  GetSafeInsetBottom();
    public abstract int  GetSafeInsetRight();
    public abstract long GetFrameID();
    public abstract int  GetFramesPerSecond();

    // ------------------------------------------------------------------------
    public abstract ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot );
    public abstract void    SetCursor( ICursor cursor );
    public abstract void    SetSystemCursor( ICursor.SystemCursor systemCursor );

    // ------------------------------------------------------------------------
    public abstract (float X, float Y) GetPpcXY();
    public abstract (float X, float Y) GetPpiXY();

    #endregion abstract methods
}