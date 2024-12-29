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

using System.Text;

using Corelib.Lugh.Core;
using Corelib.Lugh.Graphics.GLUtils;
using Corelib.Lugh.Graphics.OpenGL;

using DesktopGLBackend.Graphics;
using DesktopGLBackend.Window;

namespace DesktopGLBackend.Core;

/// <summary>
/// Configuration data and methods for the Desktop OpenGL backend.
/// </summary>
[PublicAPI]
public class DesktopGLApplicationConfiguration : DesktopGLWindowConfiguration
{
    [PublicAPI]
    public enum GLEmulationType
    {
        AngleGles20,
        GL20,
        GL30,
        GL31,
        GL32,
    }

    #region properties

    public bool            DisableAudio                   { get; set; } = false;
    public int             AudioDeviceSimultaneousSources { get; set; } = 16;
    public int             AudioDeviceBufferSize          { get; set; } = 512;
    public int             AudioDeviceBufferCount         { get; set; } = 9;
    public bool            Debug                          { get; set; } = false;
    public StreamWriter?   DebugStream                    { get; set; } = new( Console.OpenStandardOutput(), Encoding.UTF8 );
    public bool            TransparentFramebuffer         { get; set; } = false;
    public HdpiMode        HdpiMode                       { get; set; } = HdpiMode.Logical;
    public int             Depth                          { get; set; } = 16;
    public int             Stencil                        { get; set; } = 0;
    public int             Samples                        { get; set; } = 0;
    public int             IdleFPS                        { get; set; } = 60;
    public int             ForegroundFPS                  { get; set; } = 0;
    public int             GLContextMajorVersion          { get; set; } = GLData.DEFAULT_GL_MAJOR;
    public int             GLContextMinorVersion          { get; set; } = GLData.DEFAULT_GL_MINOR;
    public int             GLContextRevision              { get; set; } = 0;
    public GLEmulationType GLEmulation                    { get; set; } = GLEmulationType.GL20;
    public int             Red                            { get; set; } = 8;
    public int             Green                          { get; set; } = 8;
    public int             Blue                           { get; set; } = 8;
    public int             Alpha                          { get; set; } = 8;
    public string          PreferencesDirectory           { get; set; } = ".prefs/";
    public PathTypes       PreferencesFileType            { get; set; } = PathTypes.External;
    public bool            PauseWhenLostFocus             { get; set; }
    public bool            PauseWhenMinimized             { get; set; } = true;
    public bool            GLProfilingEnabled             { get; set; } = true;

    /// <summary>
    /// The maximum number of threads to use for network requests. Default is <see cref="int.MaxValue"/>.
    /// </summary>
    public int MaxNetThreads { get; set; } = int.MaxValue;

    #endregion properties

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates, and returns, a new DesktopApplicationConfiguration, using settings
    /// from the supplied DesktopApplicationConfiguratrion object.
    /// </summary>
    public static DesktopGLApplicationConfiguration Copy( DesktopGLApplicationConfiguration config )
    {
        var copy = new DesktopGLApplicationConfiguration();

        copy.Set( config );

        return copy;
    }

    /// <summary>
    /// Sets this DesktopApplicationConfiguration settings, using settings from the supplied
    /// DesktopApplicationConfiguratrion object.
    /// </summary>
    private void Set( DesktopGLApplicationConfiguration config )
    {
        SetWindowConfiguration( config );

        DisableAudio                   = config.DisableAudio;
        AudioDeviceSimultaneousSources = config.AudioDeviceSimultaneousSources;
        AudioDeviceBufferSize          = config.AudioDeviceBufferSize;
        AudioDeviceBufferCount         = config.AudioDeviceBufferCount;
        Debug                          = config.Debug;
        DebugStream                    = config.DebugStream;
        TransparentFramebuffer         = config.TransparentFramebuffer;
        HdpiMode                       = config.HdpiMode;
        Depth                          = config.Depth;
        Stencil                        = config.Stencil;
        Samples                        = config.Samples;
        IdleFPS                        = config.IdleFPS;
        ForegroundFPS                  = config.ForegroundFPS;
        GLContextMajorVersion          = config.GLContextMajorVersion;
        GLContextMinorVersion          = config.GLContextMinorVersion;
        GLContextRevision              = config.GLContextRevision;
        Red                            = config.Red;
        Green                          = config.Green;
        Blue                           = config.Blue;
        Alpha                          = config.Alpha;
        PreferencesDirectory           = config.PreferencesDirectory;
        PreferencesFileType            = config.PreferencesFileType;
    }

    /// <summary>
    /// Sets the audio device configuration.
    /// </summary>
    /// <param name="simultaniousSources">
    /// the maximum number of sources that can be played simultaniously (default 16)
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
    /// Sets which OpenGL version to use to emulate OpenGL ES. If the given major/minor version
    /// is not supported, the backend falls back to OpenGL ES 2.0 emulation through OpenGL 2.0.
    /// The default parameters for major and minor should be 3 and 2 respectively to be compatible
    /// with Mac OS X. Specifying major version 4 and minor version 2 will ensure that all OpenGL ES
    /// 3.0 features are supported. Note however that Mac OS X does only support 3.2.
    /// </summary>
    /// <param name="glVersion"> which OpenGL ES emulation version to use </param>
    /// <param name="glesMajorVersion"> OpenGL ES major version, use 3 as default </param>
    /// <param name="glesMinorVersion"> OpenGL ES minor version, use 2 as default </param>
    public void SetOpenGLEmulation( GLEmulationType glVersion, int glesMajorVersion, int glesMinorVersion )
    {
        this.GLEmulation           = glVersion;
        this.GLContextMajorVersion = glesMajorVersion;
        this.GLContextMinorVersion = glesMinorVersion;
    }

    /// <summary>
    /// Sets the bit depth of the color, depth and stencil buffer as well as multi-sampling.
    /// </summary>
    /// <param name="r"> red bits (default 8) </param>
    /// <param name="g"> green bits (default 8) </param>
    /// <param name="b"> blue bits (default 8) </param>
    /// <param name="a"> alpha bits (default 8) </param>
    /// <param name="depth"> depth bits (default 16) </param>
    /// <param name="stencil"> stencil bits (default 0) </param>
    /// <param name="samples"> MSAA samples (default 0) </param>
    public void SetBackBufferConfig( int r = 8,
                                     int g = 8,
                                     int b = 8,
                                     int a = 8,
                                     int depth = 16,
                                     int stencil = 0,
                                     int samples = 0 )
    {
        Red     = r;
        Green   = g;
        Blue    = b;
        Alpha   = a;
        Depth   = depth;
        Stencil = stencil;
        Samples = samples;
    }

    /// <summary>
    /// Sets the directory where <see cref="IPreferences"/> will be stored, as well as
    /// the file type to be used to store them. Defaults to "$USER_HOME/.prefs/"
    /// and <see cref="PathTypes"/>.
    /// </summary>
    public void SetPreferencesConfig( string preferencesDirectory, PathTypes preferencesFileType )
    {
        PreferencesDirectory = preferencesDirectory;
        PreferencesFileType  = preferencesFileType;
    }

    /// <summary>
    /// Sets the vorrect values for <see cref="GLContextMajorVersion"/> and
    /// <see cref="GLContextMinorVersion"/>. Defaults to 4 (major) and 6 (minor)
    /// </summary>
    public void SetGLContextVersion( int major, int minor )
    {
        GLContextMajorVersion = major;
        GLContextMinorVersion = minor;
    }

    /// <summary>
    /// Enables use of OpenGL debug message callbacks. If not supported by the core GL driver
    /// (since GL 4.3), this uses the KHR_debug, ARB_debug_output or AMD_debug_output extension
    /// if available. By default, debug messages with NOTIFICATION severity are disabled to
    /// avoid log spam.
    /// </summary>
    public void EnableGLDebugOutput( bool enable, StreamWriter debugOutputStream )
    {
        Debug       = enable;
        DebugStream = debugOutputStream;
    }

    /// <summary>
    /// Gets the currently active display mode for the primary monitor.
    /// </summary>
    public static IGraphics.DisplayMode GetDisplayMode()
    {
        return GetDisplayMode( Glfw.GetPrimaryMonitor() );
    }

    /// <summary>
    /// Gets the currterntly active display mode for the given monitor.
    /// </summary>
    public static IGraphics.DisplayMode GetDisplayMode( GLFW.Monitor monitor )
    {
        var videoMode = Glfw.GetVideoMode( monitor );

        return new DesktopGLGraphics.DesktopGLDisplayMode( monitor,
                                                           videoMode.Width,
                                                           videoMode.Height,
                                                           videoMode.RefreshRate,
                                                           videoMode.RedBits + videoMode.GreenBits + videoMode.BlueBits );
    }

    /// <summary>
    /// Return the available <see cref="IGraphics.DisplayMode"/>s of the primary monitor
    /// </summary>
    public static IGraphics.DisplayMode[] GetDisplayModes()
    {
        var videoModes = Glfw.GetVideoModes( Glfw.GetPrimaryMonitor() );

        var result = new IGraphics.DisplayMode[ videoModes.Length ];

        for ( var i = 0; i < result.Length; i++ )
        {
            var videoMode = videoModes[ i ];

            result[ i ] = new DesktopGLGraphics.DesktopGLDisplayMode( Glfw.GetPrimaryMonitor(),
                                                                      videoMode.Width,
                                                                      videoMode.Height,
                                                                      videoMode.RefreshRate,
                                                                      videoMode.RedBits + videoMode.GreenBits + videoMode.BlueBits );
        }

        return result;
    }

    /// <summary>
    /// Returns a list of the available <see cref="IGraphics.DisplayMode"/>s of the given monitor.
    /// </summary>
    public static IGraphics.DisplayMode[] GetDisplayModes( GLFW.Monitor monitor )
    {
        var videoModes = Glfw.GetVideoModes( monitor );

        var result = new IGraphics.DisplayMode[ videoModes.Length ];

        for ( var i = 0; i < result.Length; i++ )
        {
            var videoMode = videoModes[ i ];

            result[ i ] = new DesktopGLGraphics.DesktopGLDisplayMode( monitor,
                                                                      videoMode.Width,
                                                                      videoMode.Height,
                                                                      videoMode.RefreshRate,
                                                                      videoMode.RedBits + videoMode.GreenBits + videoMode.BlueBits );
        }

        return result;
    }
}