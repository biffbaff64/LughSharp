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

namespace Corelib.LibCore.Audio.OpenAL;

[PublicAPI]
public class ALC
{
    // ------------------------------------------------------------------------

    public static readonly int[] AttrList =
    [
        FREQUENCY,
        REFRESH,
        SYNC,
        MONO_SOURCES,
        STEREO_SOURCES
    ];

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    #region Context Management Functions

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCreateContext" )]
    private static extern IntPtr _CreateContext( IntPtr device, IntPtr attrlist );

    public static IntPtr CreateContext( IntPtr device, int[] attrList )
    {
        unsafe
        {
            fixed ( int* arrayPtr = attrList )
            {
                return _CreateContext( device, new IntPtr( arrayPtr ) );
            }
        }
    }

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcMakeContextCurrent" )]
    public static extern bool MakeContextCurrent( IntPtr context );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcProcessContext" )]
    public static extern void ProcessContext( IntPtr context );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcSuspendContext" )]
    public static extern void SuspendContext( IntPtr context );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcDestroyContext" )]
    public static extern void DestroyContext( IntPtr context );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetCurrentContext" )]
    public static extern IntPtr GetCurrentContext();

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetContextsDevice" )]
    public static extern IntPtr GetContextsDevice( IntPtr context );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcOpenDevice" )]
    public static extern IntPtr OpenDevice( string deviceName );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCloseDevice" )]
    public static extern bool CloseDevice( IntPtr device );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetError" )]
    public static extern int GetError( IntPtr device );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcIsExtensionPresent" )]
    public static extern bool IsExtensionPresent( IntPtr device, string extname );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetProcAddress" )]
    public static extern IntPtr GetProcAddress( IntPtr device, string funcname );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetEnumValue" )]
    public static extern int GetEnumValue( IntPtr device, string enumname );

    #endregion Context Management Functions

    // ------------------------------------------------------------------------

    #region Query Functions

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetString" )]
    private static extern IntPtr _GetString( IntPtr device, int param );

    public static string GetString( IntPtr device, int param )
    {
        return Marshal.PtrToStringAnsi( _GetString( device, param ) )!;
    }

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcGetIntegerv" )]
    public static extern void GetIntegerv( IntPtr device, int param, int size, IntPtr data );

    #endregion Query Functions

    // ------------------------------------------------------------------------

    #region Capture Functions

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureOpenDevice" )]
    public static extern IntPtr CaptureOpenDevice( string devicename, uint frequency, int format, int buffersize );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureCloseDevice" )]
    public static extern bool CaptureCloseDevice( IntPtr device );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureStart" )]
    public static extern void CaptureStart( IntPtr device );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureStop" )]
    public static extern void CaptureStop( IntPtr device );

    [DllImport( OPEN_AL_DLL, EntryPoint = "alcCaptureSamples" )]
    public static extern unsafe void CaptureSamples( IntPtr device, void* buffer, int samples );

    #endregion Capture Functions

    // ------------------------------------------------------------------------
}
