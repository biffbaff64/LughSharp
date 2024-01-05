// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Reflection;
using System.Runtime.InteropServices;

namespace LibGDXSharp.Audio.OpenAL;

public static class AL
{

    #region Dll Import Handling

    public const string OPEN_AL_DLL = "OpenAL";

    static AL() => NativeLibrary.SetDllImportResolver( typeof( AL ).Assembly, ImportResolver );

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

    public const int NONE                      = 0;
    public const int FALSE                     = 0;
    public const int TRUE                      = 1;
    public const int SOURCE_RELATIVE           = 0x202;
    public const int CONE_INNER_ANGLE          = 0x1001;
    public const int CONE_OUTER_ANGLE          = 0x1002;
    public const int PITCH                     = 0x1003;
    public const int POSITION                  = 0x1004;
    public const int DIRECTION                 = 0x1005;
    public const int VELOCITY                  = 0x1006;
    public const int LOOPING                   = 0x1007;
    public const int BUFFER                    = 0x1009;
    public const int GAIN                      = 0x100A;
    public const int MIN_GAIN                  = 0x100D;
    public const int MAX_GAIN                  = 0x100E;
    public const int ORIENTATION               = 0x100F;
    public const int SOURCE_STATE              = 0x1010;
    public const int INITIAL                   = 0x1011;
    public const int PLAYING                   = 0x1012;
    public const int PAUSED                    = 0x1013;
    public const int STOPPED                   = 0x1014;
    public const int BUFFERS_QUEUED            = 0x1015;
    public const int BUFFERS_PROCESSED         = 0x1016;
    public const int SEC_OFFSET                = 0x1024;
    public const int SAMPLE_OFFSET             = 0x1025;
    public const int BYTE_OFFSET               = 0x1026;
    public const int SOURCE_TYPE               = 0x1027;
    public const int STATIC                    = 0x1028;
    public const int STREAMING                 = 0x1029;
    public const int UNDETERMINED              = 0x1030;
    public const int DIRECT_CHANNELS_SOFT      = 0x1033;
    public const int FORMAT_MONO8              = 0x1100;
    public const int FORMAT_MONO16             = 0x1101;
    public const int FORMAT_STEREO8            = 0x1102;
    public const int FORMAT_STEREO16           = 0x1103;
    public const int REFERENCE_DISTANCE        = 0x1020;
    public const int ROLLOFF_FACTOR            = 0x1021;
    public const int CONE_OUTER_GAIN           = 0x1022;
    public const int MAX_DISTANCE              = 0x1023;
    public const int FREQUENCY                 = 0x2001;
    public const int BITS                      = 0x2002;
    public const int CHANNELS                  = 0x2003;
    public const int SIZE                      = 0x2004;
    public const int UNUSED                    = 0x2010;
    public const int PENDING                   = 0x2011;
    public const int PROCESSED                 = 0x2012;
    public const int NO_ERROR                  = FALSE;
    public const int INVALID_NAME              = 0xA001;
    public const int INVALID_ENUM              = 0xA002;
    public const int INVALID_VALUE             = 0xA003;
    public const int INVALID_OPERATION         = 0xA004;
    public const int OUT_OF_MEMORY             = 0xA005;
    public const int VENDOR                    = 0xB001;
    public const int VERSION                   = 0xB002;
    public const int RENDERER                  = 0xB003;
    public const int EXTENSIONS                = 0xB004;
    public const int ENUM_DOPPLER_FACTOR       = 0xC000;
    public const int ENUM_DOPPLER_VELOCITY     = 0xC001;
    public const int ENUM_SPEED_OF_SOUND       = 0xC003;
    public const int ENUM_DISTANCE_MODEL       = 0xD000;
    public const int INVERSE_DISTANCE          = 0xD001;
    public const int INVERSE_DISTANCE_CLAMPED  = 0xD002;
    public const int LINEAR_DISTANCE           = 0xD003;
    public const int LINEAR_DISTANCE_CLAMPED   = 0xD004;
    public const int EXPONENT_DISTANCE         = 0xD005;
    public const int EXPONENT_DISTANCE_CLAMPED = 0xD006;

    #endregion

    #region Functions

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alEnable" )]
    public static extern void Enable( int capability );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alDisable" )]
    public static extern void Disable( int capability );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alIsEnabled" )]
    public static extern bool IsEnabled( int capability );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetString" )]
    private static extern IntPtr _GetString( int param );

    public static string GetString( int param ) => Marshal.PtrToStringAnsi( _GetString( param ) )!;

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetBooleanv" )]
    public static extern void GetBooleanv( int param, out bool data );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetIntegerv" )]
    public static extern void GetIntegerv( int param, out int data );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetFloatv" )]
    public static extern void GetFloatv( int param, out float data );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetDoublev" )]
    public static extern void GetDoublev( int param, out double data );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetBoolean" )]
    public static extern bool GetBoolean( int param );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetInteger" )]
    public static extern int GetInteger( int param );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetFloat" )]
    public static extern float GetFloat( int param );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetDouble" )]
    public static extern double GetDouble( int param );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetError" )]
    public static extern int GetError();

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alIsExtensionPresent" )]
    public static extern bool IsExtensionPresent( string extname );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetProcAddress" )]
    public static extern unsafe void* GetProcAddress( string fname );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetEnumValue" )]
    public static extern int GetEnumValue( string ename );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alListenerf" )]
    public static extern void Listenerf( int param, float value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alListener3f" )]
    public static extern void Listener3F( int param, float value1, float value2, float value3 );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alListenerfv" )]
    public static extern void Listenerfv( int param, float[] values );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetListenerf" )]
    public static extern void GetListenerf( int param, out float value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetListener3f" )]
    public static extern void GetListener3F( int param, out float value1, out float value2, out float value3 );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetListenerfv" )]
    private static extern unsafe void _GetListenerfv( int param, float* values );

    public static void GetListenerfv( int param, out float[] values )
    {
        unsafe
        {
            int len;

            switch ( param )
            {
                case GAIN:
                    len = 1;

                    break;

                case POSITION:
                case VELOCITY:
                    len = 3;

                    break;

                case ORIENTATION:
                    len = 6;

                    break;

                default:
                    len = 0;

                    break;
            }

            values = new float[ len ];

            fixed ( float* arrayPtr = values )
            {
                _GetListenerfv( param, arrayPtr );
            }
        }
    }

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGenSources" )]
    private static extern unsafe void _GenSources( int n, uint* sources );

    public static void GenSources( int n, out uint[] sources )
    {
        unsafe
        {
            sources = new uint[ n ];

            fixed ( uint* arrayPtr = sources )
            {
                _GenSources( n, arrayPtr );
            }
        }
    }

    public static void GenSource( out uint source )
    {
        GenSources( 1, out var sources );
        source = sources[ 0 ];
    }

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alDeleteSources" )]
    public static extern void DeleteSources( int n, uint[] sources );

    public static void DeleteSource( uint source ) => DeleteSources( 1, new[] { source } );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alIsSource" )]
    public static extern bool IsSource( uint sid );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourcef" )]
    public static extern void Sourcef( uint sid, int param, float value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSource3f" )]
    public static extern void Source3F( uint sid, int param, float value1, float value2, float value3 );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourcefv" )]
    public static extern void Sourcefv( uint sid, int param, float[] values );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourcei" )]
    public static extern void Sourcei( uint sid, int param, int value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSource3i" )]
    public static extern void Source3I( uint sid, int param, int value1, int value2, int value3 );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourceiv" )]
    public static extern void Sourceiv( uint sid, int param, int[] values );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetSourcef" )]
    public static extern void GetSourcef( uint sid, int param, out float value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetSource3f" )]
    public static extern void GetSource3F( uint sid, int param, out float value1, out float value2, out float value3 );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetSourcefv" )]
    private static extern unsafe void _GetSourcefv( uint sid, int param, float* values );

    public static void GetSourcefv( uint sid, int param, out float[] values )
    {
        unsafe
        {
            int len;

            switch ( param )
            {
                case PITCH:
                case GAIN:
                case MAX_DISTANCE:
                case ROLLOFF_FACTOR:
                case REFERENCE_DISTANCE:
                case MIN_GAIN:
                case MAX_GAIN:
                case CONE_OUTER_GAIN:
                case CONE_INNER_ANGLE:
                case CONE_OUTER_ANGLE:
                case SEC_OFFSET:
                case SAMPLE_OFFSET:
                case BYTE_OFFSET:
                    len = 1;

                    break;

                case POSITION:
                case VELOCITY:
                case DIRECTION:
                    len = 3;

                    break;

                default:
                    len = 0;

                    break;
            }

            values = new float[ len ];

            fixed ( float* arrayPtr = values )
            {
                _GetSourcefv( sid, param, arrayPtr );
            }
        }
    }

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetSourcei" )]
    public static extern void GetSourcei( uint sid, int param, out int value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetSource3i" )]
    public static extern void GetSource3I( uint sid, int param, out int value1, out int value2, out int value3 );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetSourceiv" )]
    private static extern unsafe void _GetSourceiv( uint sid, int param, int* values );

    public static void GetSourceiv( uint sid, int param, out int[] values )
    {
        unsafe
        {
            int len;

            switch ( param )
            {
                case MAX_DISTANCE:
                case ROLLOFF_FACTOR:
                case REFERENCE_DISTANCE:
                case CONE_INNER_ANGLE:
                case CONE_OUTER_ANGLE:
                case SOURCE_RELATIVE:
                case SOURCE_TYPE:
                case LOOPING:
                case BUFFER:
                case SOURCE_STATE:
                case BUFFERS_QUEUED:
                case BUFFERS_PROCESSED:
                case SEC_OFFSET:
                case SAMPLE_OFFSET:
                case BYTE_OFFSET:
                    len = 1;

                    break;

                case DIRECTION:
                    len = 3;

                    break;

                default:
                    len = 0;

                    break;
            }

            values = new int[ len ];

            fixed ( int* arrayPtr = values )
            {
                _GetSourceiv( sid, param, arrayPtr );
            }
        }
    }

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourcePlayv" )]
    public static extern void SourcePlayv( int ns, uint[] sids );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourceStopv" )]
    public static extern void SourceStopv( int ns, uint[] sids );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourceRewindv" )]
    public static extern void SourceRewindv( int ns, uint[] sids );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourcePausev" )]
    public static extern void SourcePausev( int ns, uint[] sids );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourcePlay" )]
    public static extern void SourcePlay( uint sid );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourceStop" )]
    public static extern void SourceStop( uint sid );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourceRewind" )]
    public static extern void SourceRewind( uint sid );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourcePause" )]
    public static extern void SourcePause( uint sid );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourceQueueBuffers" )]
    public static extern void SourceQueueBuffers( uint sid, int numEntries, uint[] bids );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSourceUnqueueBuffers" )]
    public static extern void SourceUnqueueBuffers( uint sid, int numEntries, uint[] bids );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGenBuffers" )]
    private static extern unsafe void _GenBuffers( int n, uint* buffers );

    public static void GenBuffers( int n, out uint[] buffers )
    {
        unsafe
        {
            buffers = new uint[ n ];

            fixed ( uint* arrayPtr = buffers )
            {
                _GenBuffers( n, arrayPtr );
            }
        }
    }

    public static void GenBuffer( out uint buffer )
    {
        GenBuffers( 1, out var buffers );
        buffer = buffers[ 0 ];
    }

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alDeleteBuffers" )]
    public static extern void DeleteBuffers( int n, uint[] buffers );

    public static void DeleteBuffer( uint buffer ) => DeleteBuffers( 1, new[] { buffer } );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alIsBuffer" )]
    public static extern bool IsBuffer( uint bid );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alBufferData" )]
    public static extern unsafe void BufferData( uint bid, int format, void* data, int size, int freq );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alBufferi" )]
    public static extern void Bufferi( uint bid, int param, int value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alGetBufferi" )]
    public static extern void GetBufferi( uint bid, int param, out int value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alDopplerFactor" )]
    public static extern void DopplerFactor( float value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alDopplerVelocity" )]
    public static extern void DopplerVelocity( float value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alSpeedOfSound" )]
    public static extern void SpeedOfSound( float value );

    [LibraryImport( OPEN_AL_DLL, EntryPoint = "alDistanceModel" )]
    public static extern void DistanceModel( int distanceModel );

    #endregion

}
