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

using LibGDXSharp.Core.Files;
using LibGDXSharp.Core.Files.Buffers;
using LibGDXSharp.Core.Utils.Collections;

namespace LibGDXSharp.Backends.Desktop.Audio;

[PublicAPI]
public class OpenALGLAudio : IGLAudio
{
    public bool                NoDevice { get; set; } = false;
    public List< OpenALMusic > Music    { get; set; } = new( 1 );

    private int                      _deviceBufferSize;
    private int                      _deviceBufferCount;
    private List< uint >?            _idleSources;
    private List< uint >?            _allSources;
    private Dictionary< long, int >? _soundIdToSource;
    private Dictionary< int, long >? _sourceToSoundId;
    private long                     _nextSoundId = 0;
    private OpenALSound?[]?          _recentSounds;
    private int                      _mostRecentSound = -1;

    private ObjectMap< string, Type > _extensionToSoundClass = new();
    private ObjectMap< string, Type > _extensionToMusicClass = new();

    private IntPtr _device;
    private IntPtr _context;

    public OpenALGLAudio( int simultaneousSources = 16,
                          int deviceBufferCount = 9,
                          int deviceBufferSize = 512 )
    {
        this._deviceBufferSize  = deviceBufferSize;
        this._deviceBufferCount = deviceBufferCount;

        unsafe
        {
            RegisterSound( "ogg", typeof( Ogg.Sound ) );
            RegisterMusic( "ogg", typeof( Ogg.Music ) );
            RegisterSound( "wav", typeof( Wav.Sound ) );
            RegisterMusic( "wav", typeof( Wav.Music ) );
            RegisterSound( "mp3", typeof( Mp3.Sound ) );
            RegisterMusic( "mp3", typeof( Mp3.Music ) );
        }

        _device = ALC.OpenDevice( null );

        if ( _device == 0L )
        {
            NoDevice = true;

            return;
        }

        ALCCapabilities deviceCapabilities = ALC.CreateCapabilities( _device );

        _context = ALC.CreateContext( _device, null );

        if ( _context == 0L )
        {
            ALC.CloseDevice( _device );
            NoDevice = true;

            return;
        }

        if ( !ALC.MakeContextCurrent( _context ) )
        {
            NoDevice = true;

            return;
        }

        AL.CreateCapabilities( deviceCapabilities );

        _allSources = new List< uint >( simultaneousSources );

        for ( var i = 0; i < simultaneousSources; i++ )
        {
            uint sourceID = AL.GenSources();

            if ( AL.GetError() != AL.NO_ERROR )
            {
                break;
            }

            _allSources.Add( sourceID );
        }

        _idleSources     = new List< uint >( _allSources );
        _soundIdToSource = new Dictionary< long, int >();
        _sourceToSoundId = new Dictionary< int, long >();

        FloatBuffer orientation = BufferUtils.NewFloatBuffer( 6 )
                                             .Put( new[] { 0.0f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f } );

        orientation.Flip();

        AL.Listenerfv( AL.ORIENTATION, orientation );

        FloatBuffer velocity = BufferUtils.NewFloatBuffer( 3 ).Put( new[] { 0.0f, 0.0f, 0.0f } );

        velocity.Flip();

        AL.Listenerfv( AL.VELOCITY, velocity );

        FloatBuffer position = BufferUtils.NewFloatBuffer( 3 ).Put( new[] { 0.0f, 0.0f, 0.0f } );

        position.Flip();

        AL.Listenerfv( AL.POSITION, position );

        _recentSounds = new OpenALSound[ simultaneousSources ];
    }

    public void RegisterSound( String extension, Type soundClass )
    {
        ArgumentNullException.ThrowIfNull( extension );
        ArgumentNullException.ThrowIfNull( soundClass );

        _extensionToSoundClass.Put( extension, soundClass );
    }

    public void RegisterMusic( String extension, Type musicClass )
    {
        ArgumentNullException.ThrowIfNull( extension );
        ArgumentNullException.ThrowIfNull( musicClass );

        _extensionToMusicClass.Put( extension, musicClass );
    }

    public OpenALSound NewSound( FileHandle file )
    {
        ArgumentNullException.ThrowIfNull( file );

        Type? soundClass = _extensionToSoundClass.Get( file.Extension().ToLower() );

        if ( soundClass == null )
        {
            throw new GdxRuntimeException( $"Unknown file extension for sound: {file}" );
        }

        try
        {
            return Activator.CreateInstance()


            return soundClass.GetConstructor( new[] { typeof( OpenALGLAudio ), typeof( FileHandle ) } ).NewInstance( this, file );
        }
        catch ( System.Exception ex )
        {
            throw new GdxRuntimeException( $"Error creating sound {soundClass.Name} for file: {file}", ex );
        }
    }

    public OpenALMusic NewMusic( FileHandle file )
    {
        if ( file == null )
        {
            throw new ArgumentException( "file cannot be null." );
        }

        Class < ? extends OpenALMusic > musicClass = _extensionToMusicClass.get( file.extension().toLowerCase() );

        if ( musicClass == null )
        {
            throw new GdxRuntimeException( "Unknown file extension for music: " + file );
        }

        try
        {
            return musicClass.getConstructor( new Class[] { OpenALLwjgl3Audio.class, FileHandle.class } ).newInstance( this, file );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error creating music " + musicClass.getName() + " for file: " + file, ex );
        }
    }

    public int ObtainSource( bool isMusic )
    {
        if ( NoDevice )
        {
            return 0;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            int sourceId = _idleSources[ i ];
            int state    = AL.GetSourcei( sourceId, AL_SOURCE_STATE );

            if ( state != AL_PLAYING && state != AL_PAUSED )
            {
                if ( isMusic )
                {
                    _idleSources.RemoveAt( i );
                }
                else
                {
                    long? oldSoundId = _sourceToSoundId?.Remove( sourceId );

                    if ( oldSoundId != null )
                    {
                        _soundIdToSource.Remove( oldSoundId );
                    }

                    long soundId = _nextSoundId++;
                    _sourceToSoundId.put( sourceId, soundId );
                    _soundIdToSource.put( soundId, sourceId );
                }

                alSourceStop( sourceId );
                alSourcei( sourceId, AL_BUFFER, 0 );
                AL10.alSourcef( sourceId, AL10.AL_GAIN, 1 );
                AL10.alSourcef( sourceId, AL10.AL_PITCH, 1 );
                AL10.alSource3f( sourceId, AL10.AL_POSITION, 0, 0, 1f );

                return sourceId;
            }
        }

        return -1;
    }

    private void FreeSource( int sourceID )
    {
        if ( NoDevice )
        {
            return;
        }

        AL.SourceStop( sourceID );
        AL.Sourcei( sourceID, AL.BUFFER, 0 );
        var soundId = _sourceToSoundId?.Remove( sourceID );

        if ( soundId != null )
        {
            _soundIdToSource.Remove( soundId );
        }

        _idleSources.Add( sourceID );
    }

    public void FreeBuffer( int bufferID )
    {
        if ( NoDevice )
        {
            return;
        }

        for ( int i = 0, n = _idleSources!.Count; i < n; i++ )
        {
            var sourceID = _idleSources?[ i ];

            if ( AL.GetSourcei( sourceID, AL.BUFFER ) == bufferID )
            {
                var soundId = _sourceToSoundId?.Remove( sourceID );

                if ( soundId != null )
                {
                    _soundIdToSource.Remove( soundId );
                }

                AL.SourceStop( sourceID );
                AL.Sourcei( sourceID, AL.BUFFER, 0 );
            }
        }
    }

    public void StopSourcesWithBuffer( int bufferID )
    {
        if ( NoDevice )
        {
            return;
        }

        for ( int i = 0, n = _idleSources!.Count; i < n; i++ )
        {
            var sourceID = _idleSources[ i ];

            if ( AL.GetSourcei( sourceID, AL.BUFFER ) == bufferID )
            {
                var soundId = _sourceToSoundId?.Remove( sourceID );

                if ( soundId != null )
                {
                    _soundIdToSource.Remove( soundId );
                }

                AL.SourceStop( sourceID );
            }
        }
    }

    public void PauseSourcesWithBuffer( int bufferID )
    {
        if ( NoDevice )
        {
            return;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            var sourceID = _idleSources[ i ];

            if ( AL.GetSourcei( sourceID, AL.BUFFER ) == bufferID )
            {
                AL.SourcePause( sourceID );
            }
        }
    }

    public void ResumeSourcesWithBuffer( int bufferID )
    {
        if ( NoDevice )
        {
            return;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            var sourceID = _idleSources[ i ];

            if ( AL.GetSourcei( sourceID, AL.BUFFER ) == bufferID )
            {
                if ( AL.GetSourcei( sourceID, AL.SOURCE_STATE ) == AL.PAUSED )
                {
                    AL.SourcePlay( sourceID );
                }
            }
        }
    }

    public void Update()
    {
        if ( NoDevice )
        {
            return;
        }

        foreach ( OpenALMusic alm in Music )
        {
            alm.Update();
        }
    }

    public long GetSoundId( int sourceId )
    {
        var soundId = _sourceToSoundId?[ sourceId ];

        return soundId != null ? soundId : -1;
    }

    public int GetSourceId( long soundId )
    {
        Integer sourceId = _soundIdToSource.get( soundId );

        return sourceId != null ? sourceId : -1;
    }

    public void StopSound( long soundId )
    {
        Integer sourceId = _soundIdToSource.get( soundId );

        if ( sourceId != null )
        {
            alSourceStop( sourceId );
        }
    }

    public void PauseSound( long soundId )
    {
        Integer sourceId = _soundIdToSource.get( soundId );

        if ( sourceId != null )
        {
            alSourcePause( sourceId );
        }
    }

    public void ResumeSound( long soundId )
    {
        int sourceId = _soundIdToSource.get( soundId, -1 );

        if ( sourceId != -1 && AL.GetSourcei( sourceId, AL_SOURCE_STATE ) == AL_PAUSED )
        {
            AL.SourcePlay( sourceId );
        }
    }

    public void SetSoundGain( long soundId, float volume )
    {
        Integer sourceId = _soundIdToSource.get( soundId );

        if ( sourceId != null )
        {
            AL10.alSourcef( sourceId, AL10.AL_GAIN, volume );
        }
    }

    public void SetSoundLooping( long soundId, bool looping )
    {
        Integer sourceId = _soundIdToSource.get( soundId );

        if ( sourceId != null )
        {
            alSourcei( sourceId, AL10.AL_LOOPING, looping ? AL10.AL_TRUE : AL10.AL_FALSE );
        }
    }

    public void SetSoundPitch( long soundId, float pitch )
    {
        Integer sourceId = _soundIdToSource.get( soundId );

        if ( sourceId != null )
        {
            AL10.alSourcef( sourceId, AL10.AL_PITCH, pitch );
        }
    }

    public void SetSoundPan( long soundId, float pan, float volume )
    {
        var sourceId = ( uint )_soundIdToSource!.Get( soundId, -1 );

        if ( sourceId != -1 )
        {
            AL.Source3f( sourceId,
                         AL.POSITION,
                         MathUtils.Cos( ( pan - 1 ) * MathUtils.HALF_PI ),
                         0,
                         MathUtils.Sin( ( pan + 1 ) * MathUtils.HALF_PI ) );

            AL10.alSourcef( sourceId, AL10.AL_GAIN, volume );
        }
    }

    /// <summary>
    /// Retains a list of the most recently played sounds and stops the sound played
    /// least recently if necessary for a new sound to play.
    /// </summary>
    public void Retain( OpenALSound sound, bool stop )
    {
        if ( _recentSounds == null )
        {
            return;
        }

        // Move the pointer ahead and wrap
        _mostRecentSound++;
        _mostRecentSound %= _recentSounds.Length;

        if ( stop )
        {
            // Stop the least recent sound (the one we are about to bump off the buffer)
            if ( _recentSounds[ _mostRecentSound ] != null )
            {
                _recentSounds?[ _mostRecentSound ]?.Stop();
            }
        }

        _recentSounds![ _mostRecentSound ] = sound;
    }

    /// <summary>
    /// Removes the disposed sound from the least recently played list.
    /// </summary>
    public void Forget( OpenALSound sound )
    {
        for ( int i = 0; i < _recentSounds?.Length; i++ )
        {
            if ( _recentSounds[ i ] == sound )
            {
                _recentSounds[ i ] = null!;
            }
        }
    }

    // ------------------------------------------------------------------------

    public IAudioDevice NewAudioDevice( int sampleRate, final bool isMono )
    {
        if ( NoDevice )
        {
            return new IAudioDevice()
            {
 

                public void writeSamples (float[] samples, int offset, int numSamples)
                {
            }

            }

            public void WriteSamples( short[] samples, int offset, int numSamples )
            {
            }

            public void SetVolume( float volume )
            {
            }

            public bool ISMono()
            {
                return ISMono;
            }

            public int GetLatency()
            {
                return 0;
            }

            public void Dispose()
            {
            }
        }

        return new OpenALAudioDevice( this, sampleRate, isMono, _deviceBufferSize, _deviceBufferCount );
    }

    public IAudioRecorder NewAudioRecorder( int samplingRate, bool isMono )
    {
        if ( NoDevice )
        {
            return new IAudioRecorder()
            {
 

                public void read (short[] samples, int offset, int numSamples) {
            }

            }

            public void Dispose()
            {
            }
        }

        return new JavaSoundAudioRecorder( samplingRate, isMono );
    }

    // ------------------------------------------------------------------------

    #region IDisposable implementation

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( NoDevice )
            {
                return;
            }

            if ( _allSources != null )
            {
                for ( int i = 0, n = _allSources.Count; i < n; i++ )
                {
                    var sourceID = _allSources[ i ];

                    AL.GetSourcei( sourceID, AL.SOURCE_STATE, out var state );

                    if ( state != AL.STOPPED )
                    {
                        AL.SourceStop( sourceID );
                    }

                    AL.DeleteSources( ( int )sourceID, _allSources.ToArray() );
                }
            }

            _sourceToSoundId?.Clear();
            _soundIdToSource?.Clear();

            ALC.DestroyContext( _context );
            ALC.CloseDevice( _device );
        }
    }

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    #endregion IDisposable implementation

}
