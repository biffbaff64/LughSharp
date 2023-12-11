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

using LibGDXSharp.Core.Files.Buffers;
using LibGDXSharp.Core.Utils.Collections;

namespace LibGDXSharp.Backends.Desktop.Audio;

[PublicAPI]
public class OpenALGLAudio : IGLAudio
{
    private int                     deviceBufferSize;
    private int                     deviceBufferCount;
    private List< int >             idleSources;
    private List< int >             allSources;
    private Dictionary< long, int > soundIdToSource;
    private Dictionary< int, long > sourceToSoundId;
    private long                    nextSoundId = 0;
    private OpenALSound[]           recentSounds;
    private int                     mostRecetSound = -1;

    private ObjectMap< string, OpenALSound > extensionToSoundClass = new();
    private ObjectMap< string, OpenALMusic > extensionToMusicClass = new();

    private List< OpenALMusic > music    = new( 1 );
    private bool                noDevice = false;
    private long                device;
    private long                context;

    public OpenALGLAudio( int simultaneousSources = 16,
                          int deviceBufferCount = 9,
                          int deviceBufferSize = 512 )
    {
        this.deviceBufferSize  = deviceBufferSize;
        this.deviceBufferCount = deviceBufferCount;

        registerSound( "ogg", Ogg.Sound.class);
        registerMusic( "ogg", Ogg.Music.class);
        registerSound( "wav", Wav.Sound.class);
        registerMusic( "wav", Wav.Music.class);
        registerSound( "mp3", Mp3.Sound.class);
        registerMusic( "mp3", Mp3.Music.class);

        device = alcOpenDevice( ( ByteBuffer )null );

        if ( device == 0L )
        {
            noDevice = true;

            return;
        }

        ALCCapabilities deviceCapabilities = ALC.createCapabilities( device );
        context = alcCreateContext( device, ( IntBuffer )null );

        if ( context == 0L )
        {
            alcCloseDevice( device );
            noDevice = true;

            return;
        }

        if ( !alcMakeContextCurrent( context ) )
        {
            noDevice = true;

            return;
        }

        AL.createCapabilities( deviceCapabilities );

        allSources = new IntArray( false, simultaneousSources );

        for ( int i = 0; i < simultaneousSources; i++ )
        {
            int sourceID = alGenSources();

            if ( alGetError() != AL_NO_ERROR ) break;
            allSources.add( sourceID );
        }

        idleSources     = new IntArray( allSources );
        soundIdToSource = new LongMap< Integer >();
        sourceToSoundId = new IntMap< Long >();

        FloatBuffer orientation = ( FloatBuffer )BufferUtils.createFloatBuffer( 6 )
                                                            .put( new float[] { 0.0f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f } );

        ( ( Buffer )orientation ).flip();
        alListenerfv( AL_ORIENTATION, orientation );
        FloatBuffer velocity = ( FloatBuffer )BufferUtils.createFloatBuffer( 3 ).put( new float[] { 0.0f, 0.0f, 0.0f } );
        ( ( Buffer )velocity ).flip();
        alListenerfv( AL_VELOCITY, velocity );
        FloatBuffer position = ( FloatBuffer )BufferUtils.createFloatBuffer( 3 ).put( new float[] { 0.0f, 0.0f, 0.0f } );
        ( ( Buffer )position ).flip();
        alListenerfv( AL_POSITION, position );

        recentSounds = new OpenALSound[ simultaneousSources ];
    }

    /// <inheritdoc />
    public void Update()
    {
    }

    /// <inheritdoc />
    public IAudioDevice NewAudioDevice( int samplingRate, bool isMono )
    {
        return null;
    }

    /// <inheritdoc />
    public IAudioRecorder NewAudioRecorder( int samplingRate, bool isMono )
    {
        return null;
    }

    /// <inheritdoc />
    public ISound NewSound( FileInfo? fileHandle )
    {
        return null;
    }

    /// <inheritdoc />
    public IMusic NewMusic( FileInfo? file )
    {
        return null;
    }

    // ------------------------------------------------------------------------

    #region IDisposable implementation

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    #endregion IDisposable implementation

}
