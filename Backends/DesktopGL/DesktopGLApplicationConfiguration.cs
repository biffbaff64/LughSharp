﻿// ///////////////////////////////////////////////////////////////////////////////
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


using LughSharp.Backends.DesktopGL.Graphics;
using LughSharp.Backends.DesktopGL.Window;

namespace LughSharp.Backends.DesktopGL;

[PublicAPI]
public class DesktopGLApplicationConfiguration : DesktopGLWindowConfiguration
{
    public bool DisableAudio           { get; set; } = false;
    public bool UseGL30                { get; set; } = false;
    public bool Debug                  { get; set; } = false;
    public bool TransparentFramebuffer { get; set; }

    public int MaxNetThreads                  { get; set; } = int.MaxValue;
    public int AudioDeviceSimultaneousSources { get; set; } = 16;
    public int AudioDeviceBufferSize          { get; set; } = 512;
    public int AudioDeviceBufferCount         { get; set; } = 9;
    public int Gles30ContextMajorVersion      { get; set; } = 3;
    public int Gles30ContextMinorVersion      { get; set; } = 2;
    public int R                              { get; set; } = 8;
    public int G                              { get; set; } = 8;
    public int B                              { get; set; } = 8;
    public int A                              { get; set; } = 8;
    public int Depth                          { get; set; } = 16;
    public int Stencil                        { get; set; } = 0;
    public int Samples                        { get; set; } = 0;
    public int IdleFPS                        { get; set; } = 60;
    public int ForegroundFPS                  { get; set; } = 0;

    public string        PreferencesDirectory { get; set; } = ".prefs/";
    public FileType      PreferencesFileType  { get; set; } = FileType.External;
    public HdpiMode      HdpiMode             { get; set; } = HdpiMode.Logical;
    public StreamWriter? DebugStream          { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static DesktopGLApplicationConfiguration Copy( DesktopGLApplicationConfiguration config )
    {
        var copy = new DesktopGLApplicationConfiguration();

        copy.Set( config );

        return copy;
    }

    /// <summary>
    /// </summary>
    /// <param name="config"></param>
    private void Set( DesktopGLApplicationConfiguration config )
    {
        SetWindowConfiguration( config );

        DisableAudio                   = config.DisableAudio;
        AudioDeviceSimultaneousSources = config.AudioDeviceSimultaneousSources;
        AudioDeviceBufferSize          = config.AudioDeviceBufferSize;
        AudioDeviceBufferCount         = config.AudioDeviceBufferCount;
        UseGL30                        = config.UseGL30;
        Gles30ContextMajorVersion      = config.Gles30ContextMajorVersion;
        Gles30ContextMinorVersion      = config.Gles30ContextMinorVersion;
        R                              = config.R;
        G                              = config.G;
        B                              = config.B;
        A                              = config.A;
        Depth                          = config.Depth;
        Stencil                        = config.Stencil;
        Samples                        = config.Samples;
        TransparentFramebuffer         = config.TransparentFramebuffer;
        IdleFPS                        = config.IdleFPS;
        ForegroundFPS                  = config.ForegroundFPS;
        PreferencesDirectory           = config.PreferencesDirectory;
        PreferencesFileType            = config.PreferencesFileType;
        HdpiMode                       = config.HdpiMode;
        Debug                          = config.Debug;
        DebugStream                    = config.DebugStream;
    }

    /// <summary>
    ///     Sets the audio device configuration.
    /// </summary>
    /// <param name="simultaniousSources">
    ///     the maximum number of sources that can be played simultaniously (default 16)
    /// </param>
    /// <param name="bufferSize">the audio device buffer size in samples (default 512)</param>
    /// <param name="bufferCount">the audio device buffer count (default 9)</param>
    public void SetAudioConfig( int simultaniousSources, int bufferSize, int bufferCount )
    {
        AudioDeviceSimultaneousSources = simultaniousSources;
        AudioDeviceBufferSize          = bufferSize;
        AudioDeviceBufferCount         = bufferCount;
    }

    /// <summary>
    ///     Sets whether to use OpenGL ES 3.0. If the given major/minor version is not
    ///     supported, the backend falls back to OpenGL ES 2.0.
    /// </summary>
    /// <param name="useGL30">whether to use OpenGL ES 3.0</param>
    /// <param name="gles3MajorVersion">OpenGL ES major version, use 3 as default</param>
    /// <param name="gles3MinorVersion">OpenGL ES minor version, use 2 as default</param>
    public void UseOpenGL30( bool useGL30, int gles3MajorVersion = 3, int gles3MinorVersion = 2 )
    {
        UseGL30                   = useGL30;
        Gles30ContextMajorVersion = gles3MajorVersion;
        Gles30ContextMinorVersion = gles3MinorVersion;
    }

    /// <summary>
    ///     Sets the bit depth of the color, depth and stencil buffer as well as
    ///     multi-sampling.
    /// </summary>
    /// <param name="r">red bits (default 8)</param>
    /// <param name="g">green bits (default 8)</param>
    /// <param name="b">blue bits (default 8)</param>
    /// <param name="a">alpha bits (default 8)</param>
    /// <param name="depth">depth bits (default 16)</param>
    /// <param name="stencil">stencil bits (default 0)</param>
    /// <param name="samples">MSAA samples (default 0)</param>
    public void SetBackBufferConfig( int r, int g, int b, int a, int depth, int stencil, int samples )
    {
        R       = r;
        G       = g;
        B       = b;
        A       = a;
        Depth   = depth;
        Stencil = stencil;
        Samples = samples;
    }

    /// <summary>
    ///     Sets the directory where <see cref="IPreferences" /> will be stored, as well as
    ///     the file type to be used to store them. Defaults to "$USER_HOME/.prefs/"
    ///     and <see cref="FileType" />.
    /// </summary>
    public void SetPreferencesConfig( string preferencesDirectory, FileType preferencesFileType )
    {
        PreferencesDirectory = preferencesDirectory;
        PreferencesFileType  = preferencesFileType;
    }

    /// <summary>
    ///     Enables use of OpenGL debug message callbacks. If not supported by the core GL driver
    ///     (since GL 4.3), this uses the KHR_debug, ARB_debug_output or AMD_debug_output extension
    ///     if available. By default, debug messages with NOTIFICATION severity are disabled to
    ///     avoid log spam.
    ///     Use <see cref="DesktopGLApplication.SetGLDebugMessageControl(DesktopGLApplication.GLDebugMessageSeverity, bool)" />
    ///     to enable or disable other severity debug levels.
    /// </summary>
    public void EnableGLDebugOutput( bool enable, StreamWriter debugOutputStream )
    {
        Debug       = enable;
        DebugStream = debugOutputStream;
    }

    /// <summary>
    ///     Gets the currently active display mode for the primary monitor.
    /// </summary>
    public static IGraphics.DisplayModeDescriptor GetDisplayMode()
    {
        DesktopGLApplication.InitialiseGL();

        VideoMode videoMode = Glfw.GetVideoMode( Glfw.GetPrimaryMonitor() );

        return new DesktopGLGraphics.DesktopGLDisplayMode( Glfw.GetPrimaryMonitor(),
                                                           videoMode.Width,
                                                           videoMode.Height,
                                                           videoMode.RefreshRate,
                                                           videoMode.RedBits + videoMode.GreenBits + videoMode.BlueBits );
    }

    public static IGraphics.DisplayModeDescriptor GetDisplayMode( GLFWMonitor monitor )
    {
        DesktopGLApplication.InitialiseGL();

        VideoMode videoMode = Glfw.GetVideoMode( monitor );

        return new DesktopGLGraphics.DesktopGLDisplayMode( monitor,
                                                           videoMode.Width,
                                                           videoMode.Height,
                                                           videoMode.RefreshRate,
                                                           videoMode.RedBits + videoMode.GreenBits + videoMode.BlueBits );
    }

    /// <summary>
    ///     Return the available <see cref="IGraphics.DisplayModeDescriptor" />s of the primary monitor
    /// </summary>
    public static IGraphics.DisplayModeDescriptor[] GetDisplayModes()
    {
        DesktopGLApplication.InitialiseGL();

        VideoMode[] videoModes = Glfw.GetVideoModes( Glfw.GetPrimaryMonitor() );

        var result = new IGraphics.DisplayModeDescriptor[ videoModes.Length ];

        for ( var i = 0; i < result.Length; i++ )
        {
            VideoMode videoMode = videoModes[ i ];

            result[ i ] = new DesktopGLGraphics.DesktopGLDisplayMode( Glfw.GetPrimaryMonitor(),
                                                                      videoMode.Width,
                                                                      videoMode.Height,
                                                                      videoMode.RefreshRate,
                                                                      videoMode.RedBits + videoMode.GreenBits + videoMode.BlueBits );
        }

        return result;
    }

    /// <summary>
    ///     Return the available <see cref="IGraphics.DisplayModeDescriptor" />"s
    ///     of the given <see cref="GLFWMonitor" />
    /// </summary>
    public static IGraphics.DisplayModeDescriptor[] GetDisplayModes( GLFWMonitor monitor )
    {
        DesktopGLApplication.InitialiseGL();

        VideoMode[] videoModes = Glfw.GetVideoModes( monitor );

        var result = new IGraphics.DisplayModeDescriptor[ videoModes.Length ];

        for ( var i = 0; i < result.Length; i++ )
        {
            VideoMode videoMode = videoModes[ i ];

            result[ i ] = new DesktopGLGraphics.DesktopGLDisplayMode( monitor,
                                                                      videoMode.Width,
                                                                      videoMode.Height,
                                                                      videoMode.RefreshRate,
                                                                      videoMode.RedBits + videoMode.GreenBits + videoMode.BlueBits );
        }

        return result;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region To Be Removed

    //TODO: Refactor so that this can be removed
    [Obsolete]
    public static GLFWMonitor GetPrimaryMonitor()
    {
        DesktopGLApplication.InitialiseGL();

        return Glfw.GetPrimaryMonitor();
    }

    //TODO: Refactor so that this can be removed
    [Obsolete]
    public static IGraphics.MonitorDescriptor[] GetMonitors()
    {
        DesktopGLApplication.InitialiseGL();

        var monitors = new IGraphics.MonitorDescriptor[ Glfw.GetMonitors().Length ];

        for ( var i = 0; i < Glfw.GetMonitors().Length; i++ )
        {
            monitors[ i ] = ToGLMonitor( Glfw.GetMonitors()[ i ] );
        }

        return monitors;
    }

    //TODO: Refactor so that this can be removed
    [Obsolete]
    public static IGraphics.MonitorDescriptor ToGLMonitor( GLFWMonitor monitor )
    {
        Glfw.GetMonitorPos( monitor, out var virtualX, out var virtualY );

        var name = Glfw.GetMonitorName( monitor );

        return new IGraphics.MonitorDescriptor( virtualX, virtualY, name );
    }

    #endregion To Be Removed
}
