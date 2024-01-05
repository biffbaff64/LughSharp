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

using LibGDXSharp.Files;
using LibGDXSharp.Files.Buffers;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Backends.Desktop.Audio;

public abstract class OpenALMusic : IMusic
{
    public const int INVALID_SOURCE_ID = -1;

    private readonly static int         _bufferSize     = 4096 * 10;
    private readonly static int         _bufferCount    = 3;
    private readonly static int         _bytesPerSample = 2;
    private readonly static byte[]      _tempBytes      = new byte[ _bufferSize ];
    private readonly static ByteBuffer  _tempBuffer     = BufferUtils.NewByteBuffer( _bufferSize );
    private readonly        OpenALAudio _audio;
    private                 uint[]?     _buffers;
    private                 int         _format;
    private                 bool        _isPlaying;
    private                 float       _maxSecondsPerBuffer;
    private                 float       _pan = 0;
    private                 float       _renderedSeconds;

    private readonly List< float > _renderedSecondsQueue = new( _bufferCount );
    private          int           _sampleRate;
    private          int           _sourceID = INVALID_SOURCE_ID;
    private          float         _volume   = 1;

    protected FileHandle file;

    protected OpenALMusic( OpenALAudio audio, FileHandle file )
    {
        _audio               = audio;
        this.file            = file;
        OnCompletionListener = null;
    }

    public void Play()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _sourceID == INVALID_SOURCE_ID )
        {
            _sourceID = _audio.ObtainSource( true );

            if ( _sourceID == INVALID_SOURCE_ID )
            {
                return;
            }

            _audio.Music.Add( this );

            if ( _buffers == null )
            {
                _buffers = new uint[ _bufferCount ];

                AL.GenBuffers( 1, out _buffers );

                var errorCode = AL.GetError();

                if ( errorCode != AL.NO_ERROR )
                {
                    throw new GdxRuntimeException( $"Unable to allocate audio buffers. AL Error: {errorCode}" );
                }
            }

            AL.Sourcei( ( uint )_sourceID, AL.DIRECT_CHANNELS_SOFT, AL.TRUE );
            AL.Sourcei( ( uint )_sourceID, AL.LOOPING, AL.FALSE );

            SetPan( _pan, _volume );

            AL.SourceQueueBuffers( ( uint )_sourceID, _bufferCount, _buffers );

            //TODO: 
//            var filled = false; // Check if there's anything to actually play.
//
//            for ( var i = 0; i < _bufferCount; i++ )
//            {
//                var bufferID = ( int )_buffers[ i ];
//
//                if ( !Fill( bufferID ) )
//                {
//                    break;
//                }
//
//                filled = true;
//                
//                AL.SourceQueueBuffers( ( uint )_sourceID, 1, bufferID );
//            }
//
//            if ( !filled && ( OnCompletionListener != null ) )
//            {
//                OnCompletionListener.OnCompletion( this );
//            }

            if ( AL.GetError() != AL.NO_ERROR )
            {
                Stop();

                return;
            }
        }

        if ( !IsPlaying )
        {
            AL.SourcePlay( ( uint )_sourceID );
            _isPlaying = true;
        }
    }

    public void Stop()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _sourceID == INVALID_SOURCE_ID )
        {
            return;
        }

        _audio.Music.Remove( this );

        Reset();

        _audio.FreeSource( _sourceID );

        _sourceID        = INVALID_SOURCE_ID;
        _renderedSeconds = 0;

        _renderedSecondsQueue.Clear();

        _isPlaying = false;
    }

    public void Pause()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _sourceID != INVALID_SOURCE_ID )
        {
            alSourcePause( _sourceID );
        }

        _isPlaying = false;
    }

    public void SetVolume( float volume )
    {
        _volume = volume;

        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _sourceID != INVALID_SOURCE_ID )
        {
            AL.Sourcef( _sourceID, AL.GAIN, volume );
        }
    }

    public float GetVolume() => _volume;

    public void SetPan( float pan, float volume )
    {
        _volume = volume;
        _pan    = pan;

        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _sourceID == INVALID_SOURCE_ID )
        {
            return;
        }

        alSource3f( _sourceID,
                    AL.POSITION,
                    MathUtils.cos( ( pan - 1 ) * MathUtils.HALF_PI ),
                    0,
                    MathUtils.sin( ( pan + 1 ) * MathUtils.HALF_PI ) );

        AL.Sourcef( _sourceID, AL.GAIN, volume );
    }

    public void SetPosition( float position )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _sourceID == INVALID_SOURCE_ID )
        {
            return;
        }

        var wasPlaying = IsPlaying;
        IsPlaying = false;
        alSourceStop( _sourceID );
        alSourceUnqueueBuffers( _sourceID, _buffers );

        while ( _renderedSecondsQueue.size > 0 )
        {
            _renderedSeconds = _renderedSecondsQueue.pop();
        }

        if ( position <= _renderedSeconds )
        {
            Reset();
            _renderedSeconds = 0;
        }

        while ( _renderedSeconds < ( position - _maxSecondsPerBuffer ) )
        {
            if ( Read( _tempBytes ) <= 0 )
            {
                break;
            }

            _renderedSeconds += _maxSecondsPerBuffer;
        }

        _renderedSecondsQueue.add( _renderedSeconds );
        var filled = false;

        for ( var i = 0; i < _bufferCount; i++ )
        {
            int bufferID = _buffers.get( i );

            if ( !Fill( bufferID ) )
            {
                break;
            }

            filled = true;
            alSourceQueueBuffers( _sourceID, bufferID );
        }

        _renderedSecondsQueue.pop();

        if ( !filled )
        {
            Stop();

            if ( OnCompletionListener != null )
            {
                OnCompletionListener.onCompletion( this );
            }
        }

        AL.Sourcef( _sourceID, AL11.Al.SEC_OFFSET, position - _renderedSeconds );

        if ( wasPlaying )
        {
            AL.SourcePlay( _sourceID );
            IsPlaying = true;
        }
    }

    public float GetPosition()
    {
        if ( _audio.NoDevice )
        {
            return 0;
        }

        if ( _sourceID == INVALID_SOURCE_ID )
        {
            return 0;
        }

        return _renderedSeconds + AL.GetSourcef( _sourceID, AL.SEC_OFFSET );
    }

    public void Dispose()
    {
        Stop();

        if ( _audio.NoDevice || ( _buffers == null ) )
        {
            return;
        }

        AL.DeleteBuffers( _buffers );

        _buffers             = null;
        OnCompletionListener = null;
    }

    protected void Setup( int channels, int sampleRate )
    {
        _format              = channels > 1 ? AL.FORMAT_STEREO16 : AL.FORMAT_MONO16;
        _sampleRate          = sampleRate;
        _maxSecondsPerBuffer = ( float )_bufferSize / ( _bytesPerSample * channels * sampleRate );
    }

    /// <summary>
    ///     Fills as much of the buffer as possible and returns the number of
    ///     bytes filled. Returns &lt;= 0 to indicate the end of the stream.
    /// </summary>
    public abstract int Read( byte[] buffer );

    /// <summary>
    ///     Resets the stream to the beginning.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    ///     By default, does just the same as reset().
    ///     Used to add special behaviour in Ogg.Music.
    /// </summary>
    protected void Loop() => Reset();

    public int GetChannels() => _format == AL.FORMAT_STEREO16 ? 2 : 1;

    public int GetRate() => _sampleRate;

    public void Update()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _sourceID == INVALID_SOURCE_ID )
        {
            return;
        }

        var end     = false;
        int buffers = AL.GetSourcei( _sourceID, AL.BUFFERS_PROCESSED );

        while ( buffers-- > 0 )
        {
            int bufferID = AL.SourceUnqueueBuffers( _sourceID );

            if ( bufferID == AL.INVALID_VALUE )
            {
                break;
            }

            if ( _renderedSecondsQueue.Count > 0 )
            {
                _renderedSeconds = _renderedSecondsQueue.Pop();
            }

            if ( end )
            {
                continue;
            }

            if ( Fill( bufferID ) )
            {
                AL.SourceQueueBuffers( _sourceID, bufferID );
            }
            else
            {
                end = true;
            }
        }

        if ( end && ( AL.GetSourcei( _sourceID, AL.BUFFERS_QUEUED ) == 0 ) )
        {
            Stop();

            if ( OnCompletionListener != null )
            {
                OnCompletionListener.OnCompletion( this );
            }
        }

        // A buffer underflow will cause the source to stop.
        if ( _isPlaying && ( AL.GetSourcei( _sourceID, AL.SOURCE_STATE ) != AL.PLAYING ) )
        {
            AL.SourcePlay( _sourceID );
        }
    }

    private bool Fill( int bufferID )
    {
        _tempBuffer.Clear();

        var length = Read( _tempBytes );

        if ( length <= 0 )
        {
            if ( IsLooping )
            {
                Loop();
                length = Read( _tempBytes );

                if ( length <= 0 )
                {
                    return false;
                }

                if ( _renderedSecondsQueue.Count > 0 )
                {
                    _renderedSecondsQueue[ 0 ] = 0;
                }
            }
            else
            {
                return false;
            }
        }

        var previousLoadedSeconds = _renderedSecondsQueue.Count > 0 ? _renderedSecondsQueue.First() : 0;
        var currentBufferSeconds  = ( _maxSecondsPerBuffer * length ) / _bufferSize;

        _renderedSecondsQueue.Insert( 0, previousLoadedSeconds + currentBufferSeconds );

        _tempBuffer.Put( _tempBytes, 0, length ).Flip();

        AL.BufferData( bufferID, _format, _tempBuffer, _sampleRate );

        return true;
    }

    public int GetSourceId() => _sourceID;

    #region properties

    public IMusic.IOnCompletionListener? OnCompletionListener { get; set; }
    public bool                          IsLooping            { get; set; }

    public bool IsPlaying
    {
        get
        {
            if ( _audio.NoDevice )
            {
                return false;
            }

            if ( _sourceID == INVALID_SOURCE_ID )
            {
                return false;
            }

            return _isPlaying;
        }
        set => _isPlaying = value;
    }

    #endregion properties

}
