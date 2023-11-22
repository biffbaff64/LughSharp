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

using BufferFormatDescriptor = IGraphics.BufferFormatDescriptor;

[PublicAPI]
public abstract class AbstractGraphics : IGraphics
{
    public float GetRawDeltaTime()    => DeltaTime;
    public float GetDensity()         => GetPpiX() / 160f;
    public float GetBackBufferScale() => BackBufferWidth / ( float )Width;

    public BufferFormatDescriptor BufferFormat     { get; set; } = null!;
    public int                    BackBufferWidth  { get; protected set; }
    public int                    BackBufferHeight { get; protected set; }
    public int                    LogicalWidth     { get; set; }
    public int                    LogicalHeight    { get; set; }
    public int                    Width            { get; }
    public int                    Height           { get; }
    public IGL20?                 GL20             { get; set; }
    public IGL30?                 GL30             { get; set; }
    public float                  DeltaTime        { get; set; }
    public GLVersion              GLVersion        { get; set; } = null!;

    // ========================================================================
    // Abstract methods because C# insists this is done to fulfill the contract
    // between the class and interface, which just makes everything annoying tbh.

    public abstract IGraphics.Monitor GetPrimaryMonitor();

    public abstract IGraphics.Monitor GetMonitor();

    public abstract IGraphics.Monitor[] GetMonitors();

    public abstract IGraphics.DisplayMode[] GetDisplayModes();

    public abstract IGraphics.DisplayMode[] GetDisplayModes( IGraphics.Monitor monitor );

    public abstract IGraphics.DisplayMode GetDisplayMode();

    public abstract IGraphics.DisplayMode GetDisplayMode( IGraphics.Monitor monitor );

    public abstract bool SetFullscreenMode( IGraphics.DisplayMode displayMode );

    public abstract bool SetWindowedMode( int width, int height );

    public abstract void SetTitle( string title );

    public abstract void SetUndecorated( bool undecorated );

    public abstract void SetResizable( bool resizable );

    public abstract void SetVSync( bool vsync );

    public abstract void SetForegroundFps( int fps );

    public abstract bool SupportsExtension( string extension );

    public abstract bool SupportsDisplayModeChange();

    public abstract void SetContinuousRendering( bool isContinuous );

    public abstract bool IsContinuousRendering();

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
}
