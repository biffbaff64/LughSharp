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

using Corelib.LibCore.Audio;
using Corelib.LibCore.Audio.OpenAL;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;

using Exception = System.Exception;

namespace DesktopGLBackend.Audio;

[PublicAPI]
public class OpenALAudio : IGLAudio
{
    public bool                NoDevice { get; set; } = false;
    public List< OpenALMusic > Music    { get; set; } = new( 1 );

    private readonly uint[]?                    _allSources;
    private readonly IntPtr                     _context;
    private readonly IntPtr                     _device;
    private readonly int                        _deviceBufferCount;
    private readonly int                        _deviceBufferSize;
    private readonly Dictionary< string, Type > _extensionToMusicClass = new();
    private readonly Dictionary< string, Type > _extensionToSoundClass = new();
    private readonly List< uint >?              _idleSources;
    private readonly OpenALSound?[]?            _recentSounds;
    private readonly Dictionary< long, int >?   _soundIdToSource;
    private readonly Dictionary< int, long >?   _sourceToSoundId;

    private int  _mostRecentSound = -1;
    private long _nextSoundId     = 0;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// </summary>
    /// <param name="simultaneousSources"></param>
    /// <param name="deviceBufferCount"></param>
    /// <param name="deviceBufferSize"></param>
    public OpenALAudio( int simultaneousSources = 16,
                        int deviceBufferCount = 9,
                        int deviceBufferSize = 512 )
    {
        _deviceBufferSize  = deviceBufferSize;
        _deviceBufferCount = deviceBufferCount;

        RegisterSound( "ogg", typeof( Ogg.Sound ) );
        RegisterMusic( "ogg", typeof( Ogg.Music ) );
        RegisterSound( "wav", typeof( Wav.Sound ) );
        RegisterMusic( "wav", typeof( Wav.Music ) );
        RegisterSound( "mp3", typeof( Mp3.Sound ) );
        RegisterMusic( "mp3", typeof( Mp3.Music ) );

        _device = ALC.OpenDevice( "" ); //TODO: Find which value to use for default device

        if ( _device == 0L )
        {
            NoDevice = true;

            return;
        }

        //TODO: Don't think this is needed?
//        ALC.Capabilities deviceCapabilities = ALC.CreateCapabilities( _device );

        _context = ALC.CreateContext( _device, ALC.AttrList );

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

        _allSources = new uint[ simultaneousSources ];

        AL.GenSources( simultaneousSources, out _allSources );

        _idleSources     = [ .._allSources ];
        _soundIdToSource = new Dictionary< long, int >();
        _sourceToSoundId = new Dictionary< int, long >();

        AL.Listenerfv( AL.ORIENTATION, [ 0.0f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f ] );
        AL.Listenerfv( AL.VELOCITY, [ 0.0f, 0.0f, 0.0f ] );
        AL.Listenerfv( AL.POSITION, [ 0.0f, 0.0f, 0.0f ] );

        _recentSounds = new OpenALSound[ simultaneousSources ];
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public ISound NewSound( FileInfo? file )
    {
        ArgumentNullException.ThrowIfNull( file );

        var soundClass = _extensionToSoundClass.Get( file.Extension.ToLower() );

        if ( soundClass == null )
        {
            throw new GdxRuntimeException( $"Unknown file extension for sound: {file}" );
        }

        try
        {
            var snd = Activator.CreateInstance( soundClass ) as OpenALSound;

            return snd ?? throw new NullReferenceException();
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( $"Error creating sound {soundClass.Name} for file: {file}", ex );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"> If the provided FieHandle is null </exception>
    /// <exception cref="GdxRuntimeException"> If an unknown file extension is provided </exception>
    /// <exception cref="NullReferenceException"></exception>
    public IMusic NewMusic( FileInfo? file )
    {
        ArgumentNullException.ThrowIfNull( file );

        var musicClass = _extensionToMusicClass.Get( file.Extension.ToLower() );

        if ( musicClass == null )
        {
            throw new GdxRuntimeException( "Unknown file extension for music: " + file );
        }

        try
        {
            var snd = Activator.CreateInstance( musicClass ) as OpenALMusic;

            return snd ?? throw new NullReferenceException();
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( $"Error creating music {musicClass.Name} for file: {file}", ex );
        }
    }

    /// <summary>
    /// </summary>
    public void Update()
    {
        if ( NoDevice )
        {
            return;
        }

        foreach ( var alm in Music )
        {
            alm.Update();
        }
    }

    // ========================================================================

    public IAudioDevice NewAudioDevice( int sampleRate, bool isMono )
    {
//        if ( NoDevice )
//        {
//            return new IAudioDevice()
//            {
//                public void writeSamples (float[] samples, int offset, int numSamples)
//                {
//            }
//            }
//            public void WriteSamples( short[] samples, int offset, int numSamples )
//            {
//            }
//            public void SetVolume( float volume )
//            {
//            }
//            public bool ISMono()
//            {
//                return ISMono;
//            }
//            public int GetLatency()
//            {
//                return 0;
//            }
//            public void Dispose()
//            {
//            }
//        }
//        
//        return new OpenALAudioDevice( this, sampleRate, isMono, _deviceBufferSize, _deviceBufferCount );

        return null!;
    }

    public IAudioRecorder NewAudioRecorder( int samplingRate, bool isMono )
    {
//        if ( NoDevice )
//        {
//            return new IAudioRecorder()
//            {
//                public void read (short[] samples, int offset, int numSamples) {
//            }
//            }
//            public void Dispose()
//            {
//            }
//        }
//        
//        return new GdxSoundAudioRecorder( samplingRate, isMono );

        return null!;
    }

    /// <summary>
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="soundClass"></param>
    public void RegisterSound( string extension, Type soundClass )
    {
        ArgumentNullException.ThrowIfNull( extension );
        ArgumentNullException.ThrowIfNull( soundClass );

        _extensionToSoundClass.Put( extension, soundClass );
    }

    /// <summary>
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="musicClass"></param>
    public void RegisterMusic( string extension, Type musicClass )
    {
        ArgumentNullException.ThrowIfNull( extension );
        ArgumentNullException.ThrowIfNull( musicClass );

        _extensionToMusicClass.Put( extension, musicClass );
    }

    /// <summary>
    /// </summary>
    /// <param name="isMusic"></param>
    /// <returns></returns>
    public int ObtainSource( bool isMusic )
    {
        if ( NoDevice || ( _idleSources == null ) )
        {
            return 0;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            var sourceId = _idleSources[ i ];

            AL.GetSourcei( sourceId, AL.SOURCE_STATE, out var state );

            if ( ( state != AL.PLAYING ) && ( state != AL.PAUSED ) )
            {
                if ( isMusic )
                {
                    _idleSources.RemoveAt( i );
                }
                else
                {
                    var oldSoundId = _sourceToSoundId?[ ( int ) sourceId ];

                    if ( oldSoundId != null )
                    {
                        _soundIdToSource?.Remove( ( long ) oldSoundId );
                    }

                    var soundId = _nextSoundId++;

                    _sourceToSoundId?.Put( ( int ) sourceId, soundId );
                    _soundIdToSource?.Put( soundId, ( int ) sourceId );
                }

                AL.SourceStop( sourceId );
                AL.Sourcei( sourceId, AL.BUFFER, 0 );
                AL.Sourcef( sourceId, AL.GAIN, 1 );
                AL.Sourcef( sourceId, AL.PITCH, 1 );
                AL.Source3F( sourceId, AL.POSITION, 0, 0, 1f );

                return ( int ) sourceId;
            }
        }

        return -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="sourceID"></param>
    public void FreeSource( int sourceID )
    {
        if ( NoDevice )
        {
            return;
        }

        AL.SourceStop( ( uint ) sourceID );
        AL.Sourcei( ( uint ) sourceID, AL.BUFFER, 0 );

        var soundId = _sourceToSoundId?[ sourceID ];

        _sourceToSoundId?.Remove( sourceID );

        if ( soundId != null )
        {
            _soundIdToSource?.Remove( ( long ) soundId );
        }

        _idleSources?.Add( ( uint ) sourceID );
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferID"></param>
    public void FreeBuffer( int bufferID )
    {
        if ( NoDevice || ( _idleSources == null ) )
        {
            return;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            var sourceID = _idleSources?[ i ];

            if ( sourceID == null )
            {
                continue;
            }

            AL.GetSourcei( ( uint ) sourceID, AL.BUFFER, out var outval );

            if ( outval == bufferID )
            {
                var soundId = _sourceToSoundId?[ ( int ) sourceID ];

                _sourceToSoundId?.Remove( ( int ) sourceID );

                if ( soundId != null )
                {
                    _soundIdToSource?.Remove( ( int ) soundId );
                }

                AL.SourceStop( ( uint ) sourceID );
                AL.Sourcei( ( uint ) sourceID, AL.BUFFER, 0 );
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferID"></param>
    public void StopSourcesWithBuffer( int bufferID )
    {
        if ( NoDevice || ( _idleSources == null ) )
        {
            return;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            var sourceID = _idleSources[ i ];

            AL.GetSourcei( sourceID, AL.BUFFER, out var outval );

            if ( outval == bufferID )
            {
                var soundId = _sourceToSoundId?[ ( int ) sourceID ];

                _sourceToSoundId?.Remove( ( int ) sourceID );

                if ( soundId != null )
                {
                    _soundIdToSource?.Remove( ( long ) soundId );
                }

                AL.SourceStop( sourceID );
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferID"></param>
    public void PauseSourcesWithBuffer( int bufferID )
    {
        if ( NoDevice || ( _idleSources == null ) )
        {
            return;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            var sourceID = _idleSources[ i ];

            AL.GetSourcei( sourceID, AL.BUFFER, out var outval );

            if ( outval == bufferID )
            {
                AL.SourcePause( sourceID );
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferID"></param>
    public void ResumeSourcesWithBuffer( int bufferID )
    {
        if ( NoDevice || ( _idleSources == null ) )
        {
            return;
        }

        for ( int i = 0, n = _idleSources.Count; i < n; i++ )
        {
            var sourceID = _idleSources[ i ];

            AL.GetSourcei( sourceID, AL.BUFFER, out var outval );

            if ( outval == bufferID )
            {
                AL.GetSourcei( sourceID, AL.SOURCE_STATE, out var state );

                if ( state == AL.PAUSED )
                {
                    AL.SourcePlay( sourceID );
                }
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="sourceId"></param>
    /// <returns></returns>
    public long GetSoundId( int sourceId )
    {
        var soundId = _sourceToSoundId?[ sourceId ];

        return soundId ?? -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="soundId"></param>
    /// <returns></returns>
    public int GetSourceId( long soundId )
    {
        var sourceId = _soundIdToSource?.Get( soundId );

        return sourceId ?? -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="soundId"></param>
    public void StopSound( long soundId )
    {
        var sourceId = _soundIdToSource?.Get( soundId );

        if ( sourceId != null )
        {
            AL.SourceStop( ( uint ) sourceId );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="soundId"></param>
    public void PauseSound( long soundId )
    {
        var sourceId = _soundIdToSource?.Get( soundId );

        if ( sourceId != null )
        {
            AL.SourcePause( ( uint ) sourceId );
        }
    }

    public void ResumeSound( long soundId )
    {
        var sourceId = _soundIdToSource?.Get( soundId, -1 );

        if ( ( sourceId != null ) && ( sourceId != -1 ) )
        {
            AL.GetSourcei( ( uint ) sourceId, AL.SOURCE_STATE, out var outval );

            if ( outval == AL.PAUSED )
            {
                AL.SourcePlay( ( uint ) sourceId );
            }
        }
    }

    public void SetSoundGain( long soundId, float volume )
    {
        var sourceId = _soundIdToSource?.Get( soundId );

        if ( sourceId != null )
        {
            AL.Sourcef( ( uint ) sourceId, AL.GAIN, volume );
        }
    }

    public void SetSoundLooping( long soundId, bool looping )
    {
        var sourceId = _soundIdToSource?.Get( soundId );

        if ( sourceId != null )
        {
            AL.Sourcei( ( uint ) sourceId, AL.LOOPING, looping ? AL.TRUE : AL.FALSE );
        }
    }

    public void SetSoundPitch( long soundId, float pitch )
    {
        var sourceId = _soundIdToSource?.Get( soundId );

        if ( sourceId != null )
        {
            AL.Sourcef( ( uint ) sourceId, AL.PITCH, pitch );
        }
    }

    public void SetSoundPan( long soundId, float pan, float volume )
    {
        var sourceId = _soundIdToSource?.Get( soundId, -1 );

        if ( ( sourceId != null ) && ( sourceId != -1 ) )
        {
            AL.Source3F( ( uint ) sourceId,
                         AL.POSITION,
                         MathUtils.Cos( ( pan - 1 ) * MathUtils.HALF_PI ),
                         0,
                         MathUtils.Sin( ( pan + 1 ) * MathUtils.HALF_PI ) );

            AL.Sourcef( ( uint ) sourceId, AL.GAIN, volume );
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

        if ( _recentSounds != null )
        {
            _recentSounds[ _mostRecentSound ] = sound;
        }
    }

    /// <summary>
    /// Removes the disposed sound from the least recently played list.
    /// </summary>
    public void Forget( OpenALSound sound )
    {
        for ( var i = 0; i < _recentSounds?.Length; i++ )
        {
            if ( _recentSounds[ i ] == sound )
            {
                _recentSounds[ i ] = null;
            }
        }
    }

    // ========================================================================

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
                for ( int i = 0, n = _allSources.Length; i < n; i++ )
                {
                    var sourceID = _allSources[ i ];

                    AL.GetSourcei( sourceID, AL.SOURCE_STATE, out var state );

                    if ( state != AL.STOPPED )
                    {
                        AL.SourceStop( sourceID );
                    }

                    AL.DeleteSources( ( int ) sourceID, _allSources.ToArray() );
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
    }

    #endregion IDisposable implementation
}