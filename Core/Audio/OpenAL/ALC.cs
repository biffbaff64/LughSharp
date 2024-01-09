///////////////////////////////////////////////////////////////////////////////
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
///////////////////////////////////////////////////////////////////////////////

using System.Reflection;
using System.Runtime.InteropServices;

namespace LibGDXSharp.Audio.OpenAL;

[PublicAPI]
public class ALC
{
    public readonly static int[] AttrList =
    {
        FREQUENCY,
        REFRESH,
        SYNC,
        MONO_SOURCES,
        STEREO_SOURCES
    };

    #region Dll Import Handling

    public const string OPEN_AL_DLL = "OpenAL";

    static ALC() => NativeLibrary.SetDllImportResolver( typeof( AL ).Assembly, ImportResolver );

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

    #endregion

    #region Enum

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

    #endregion

    #region Context Management Functions

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcCreateContext" )]
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

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcMakeContextCurrent" )]
    public static extern bool MakeContextCurrent( IntPtr context );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcProcessContext" )]
    public static extern void ProcessContext( IntPtr context );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcSuspendContext" )]
    public static extern void SuspendContext( IntPtr context );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcDestroyContext" )]
    public static extern void DestroyContext( IntPtr context );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcGetCurrentContext" )]
    public static extern IntPtr GetCurrentContext();

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcGetContextsDevice" )]
    public static extern IntPtr GetContextsDevice( IntPtr context );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcOpenDevice" )]
    public static extern IntPtr OpenDevice( string deviceName );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcCloseDevice" )]
    public static extern bool CloseDevice( IntPtr device );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcGetError" )]
    public static extern int GetError( IntPtr device );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcIsExtensionPresent" )]
    public static extern bool IsExtensionPresent( IntPtr device, string extname );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcGetProcAddress" )]
    public static extern IntPtr GetProcAddress( IntPtr device, string funcname );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcGetEnumValue" )]
    public static extern int GetEnumValue( IntPtr device, string enumname );

    #endregion

    #region Query Functions

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcGetString" )]
    private static extern IntPtr _GetString( IntPtr device, int param );

    public static string GetString( IntPtr device, int param ) => Marshal.PtrToStringAnsi( _GetString( device, param ) )!;

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcGetIntegerv" )]
    public static extern void GetIntegerv( IntPtr device, int param, int size, IntPtr data );

    #endregion

    #region Capture Functions

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcCaptureOpenDevice" )]
    public static extern IntPtr CaptureOpenDevice( string devicename, uint frequency, int format, int buffersize );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcCaptureCloseDevice" )]
    public static extern bool CaptureCloseDevice( IntPtr device );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcCaptureStart" )]
    public static extern void CaptureStart( IntPtr device );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcCaptureStop" )]
    public static extern void CaptureStop( IntPtr device );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alcCaptureSamples" )]
    public static extern unsafe void CaptureSamples( IntPtr device, void* buffer, int samples );

    #endregion
}
