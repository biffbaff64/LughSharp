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


using Environment = System.Environment;

namespace Corelib.Lugh.Audio.OpenAL;

[PublicAPI]
public class ALC
{
    // ========================================================================

    public static readonly int[] AttrList =
    [
        FREQUENCY,
        REFRESH,
        SYNC,
        MONO_SOURCES,
        STEREO_SOURCES,
    ];

    // ========================================================================

    #region Dll Import Handling

    public const string OPEN_AL_DLL = "OpenAL";

    static ALC()
    {
        NativeLibrary.SetDllImportResolver( typeof( AL ).Assembly, ImportResolver );
    }

    private static IntPtr ImportResolver( string libraryName, Assembly assembly, DllImportSearchPath? searchPath )
    {
        var libHandle = IntPtr.Zero;

        if ( libraryName == OPEN_AL_DLL )
        {
            if ( Environment.OSVersion.Platform == PlatformID.Unix )
            {
                const string OSX   = "OpenAL.framework/OpenAL";
                const string LINUX = "libopenal.so.1";
                NativeLibrary.TryLoad( OSX, assembly, DllImportSearchPath.System32, out libHandle );

                if ( libHandle == IntPtr.Zero ) //If OSX path didn't work
                {
                    NativeLibrary.TryLoad( LINUX, assembly, DllImportSearchPath.System32, out libHandle );
                }
            }
            else
            {
                const string WINDOWS = "openal32.dll";
                NativeLibrary.TryLoad( WINDOWS, assembly, DllImportSearchPath.System32, out libHandle );
            }
        }

        return libHandle;
    }

    #endregion Dll Import Handling

    // ========================================================================

    #region Constants

    public const int FALSE                            = 0;
    public const int TRUE                             = 1;
    public const int FREQUENCY                        = 0x1007;
    public const int REFRESH                          = 0x1008;
    public const int SYNC                             = 0x1009;
    public const int MONO_SOURCES                     = 0x1010;
    public const int STEREO_SOURCES                   = 0x1011;
    public const int NO_ERROR                         = FALSE;
    public const int INVALID_DEVICE                   = 0xA001;
    public const int INVALID_CONTEXT                  = 0xA002;
    public const int INVALID_ENUM                     = 0xA003;
    public const int INVALID_VALUE                    = 0xA004;
    public const int OUT_OF_MEMORY                    = 0xA005;
    public const int DEFAULT_DEVICE_SPECIFIER         = 0x1004;
    public const int DEVICE_SPECIFIER                 = 0x1005;
    public const int EXTENSIONS                       = 0x1006;
    public const int MAJOR_VERSION                    = 0x1000;
    public const int MINOR_VERSION                    = 0x1001;
    public const int ATTRIBUTES_SIZE                  = 0x1002;
    public const int ALL_ATTRIBUTES                   = 0x1003;
    public const int DEFAULT_ALL_DEVICES_SPECIFIER    = 0x1012;
    public const int ALL_DEVICES_SPECIFIER            = 0x1013;
    public const int CAPTURE_DEVICE_SPECIFIER         = 0x310;
    public const int CAPTURE_DEFAULT_DEVICE_SPECIFIER = 0x311;
    public const int ENUM_CAPTURE_SAMPLES             = 0x312;

    #endregion Constants

    // ========================================================================

    #region Context Management Functions

    public static IntPtr CreateContext( IntPtr device, int[] attrList )
    {
        unsafe
        {
            fixed ( int* arrayPtr = attrList )
            {
                return _createContext( device, new IntPtr( arrayPtr ) );
            }
        }
    }

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCreateContext" )]
    private static extern IntPtr _createContext( IntPtr device, IntPtr attrlist );

    // ========================================================================

    public static bool MakeContextCurrent( IntPtr context )
    {
        return _makeContextCurrent( context );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcMakeContextCurrent" )]
    private static extern bool _makeContextCurrent( IntPtr context );

    // ========================================================================

    public static void ProcessContext( IntPtr context )
    {
        _processContext( context );
    }

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcProcessContext" )]
    private static extern void _processContext( IntPtr context );

    // ========================================================================

    public static void SuspendContext( IntPtr context )
    {
        _suspendContext( context );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcSuspendContext" )]
    private static extern void _suspendContext( IntPtr context );

    // ========================================================================

    public static void DestroyContext( IntPtr context )
    {
        _destroyContext( context );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcDestroyContext" )]
    private static extern void _destroyContext( IntPtr context );

    // ========================================================================

    public static IntPtr GetCurrentContext()
    {
        return _getCurrentContext();
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetCurrentContext" )]
    private static extern IntPtr _getCurrentContext();

    // ========================================================================

    public static IntPtr GetContextsDevice( IntPtr context )
    {
        return _getContextsDevice( context );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetContextsDevice" )]
    private static extern IntPtr _getContextsDevice( IntPtr context );

    // ========================================================================

    public static IntPtr OpenDevice( string deviceName )
    {
        return _openDevice( deviceName );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcOpenDevice" )]
    private static extern IntPtr _openDevice( string deviceName );

    // ========================================================================

    public static void CloseDevice( IntPtr context )
    {
        _closeDevice( context );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCloseDevice" )]
    private static extern bool _closeDevice( IntPtr device );

    // ========================================================================

    public int GetError( IntPtr device )
    {
        return _getError( device );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetError" )]
    private static extern int _getError( IntPtr device );

    // ========================================================================

    public static bool IsExtensionPresent( IntPtr context, string extension )
    {
        return _isExtensionPresent( context, extension );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcIsExtensionPresent" )]
    private static extern bool _isExtensionPresent( IntPtr device, string extname );

    // ========================================================================

    public static IntPtr GetProcAddress( IntPtr context, string procname )
    {
        return _getProcAddress( context, procname );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetProcAddress" )]
    private static extern IntPtr _getProcAddress( IntPtr device, string funcname );

    // ========================================================================

    public static int GetEnumValue( IntPtr device, string enumname )
    {
        return _getEnumValue( device, enumname );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetEnumValue" )]
    private static extern int _getEnumValue( IntPtr device, string enumname );

    #endregion Context Management Functions

    // ========================================================================

    #region Query Functions


    public static string GetString( IntPtr device, int param )
    {
        return Marshal.PtrToStringAnsi( _getString( device, param ) )!;
    }

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetString" )]
    private static extern IntPtr _getString( IntPtr device, int param );
    // ========================================================================

    public void GetIntegerv( IntPtr device, int param, int size, IntPtr data )
    {
        _getIntegerv( device, param, size, data );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetIntegerv" )]
    private static extern void _getIntegerv( IntPtr device, int param, int size, IntPtr data );

    #endregion Query Functions

    // ========================================================================

    #region Capture Functions

    public static IntPtr CaptureOpenDevice( string deviceName, uint frequency, int format, int buffersize )
    {
        return _captureOpenDevice( deviceName, frequency, format, buffersize );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureOpenDevice" )]
    private static extern IntPtr _captureOpenDevice( string devicename, uint frequency, int format, int buffersize );

    // ========================================================================

    public static bool CaptureCloseDevice( IntPtr device )
    {
        return _captureCloseDevice( device );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureCloseDevice" )]
    private static extern bool _captureCloseDevice( IntPtr device );

    // ========================================================================

    public static void CaptureStart( IntPtr device )
    {
        _captureStart( device );
    }

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureStart" )]
    private static extern void _captureStart( IntPtr device );

    // ========================================================================

    public static void CaptureStop( IntPtr device )
    {
        _captureStop( device );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureStop" )]
    private static extern void _captureStop( IntPtr device );

    // ========================================================================

    public static unsafe void CaptureSamples( IntPtr device, void* buffer, int samples )
    {
        _captureSamples( device, buffer, samples );
    }
    
    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureSamples" )]
    private static extern unsafe void _captureSamples( IntPtr device, void* buffer, int samples );

    #endregion Capture Functions

    // ========================================================================
}
