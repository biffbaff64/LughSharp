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

using LughSharp.Lugh.Audio;
using LughSharp.Lugh.Audio.OpenAL;
using LughSharp.Lugh.Utils.Exceptions;

namespace DesktopGLBackend.Audio;

public abstract class OpenALMusic : IMusic
{
    public const int INVALID_SOURCE_ID = AL.INVALID_VALUE;

    protected readonly FileInfo File;

    private static readonly int           _bufferSize     = 4096 * 10;
    private static readonly int           _bufferCount    = 3;
    private static readonly int           _bytesPerSample = 2;
    private static readonly byte[]        _tempBytes      = new byte[ _bufferSize ];
    private static readonly byte[]        _tempBuffer     = new byte[ _bufferSize ];
    private readonly        OpenALAudio   _audio;
    private readonly        float         _pan                  = 0;
    private readonly        List< float > _renderedSecondsQueue = new( _bufferCount );
    private readonly        float         _volume               = 1;

    private uint[]? _buffers;
    private int     _format;
    private bool    _isPlaying;
    private float   _maxSecondsPerBuffer;
    private float   _renderedSeconds;
    private int     _sampleRate;

    protected OpenALMusic()
    {
        _audio               = null!;
        File                 = null!;
        OnCompletionListener = null;
    }

    protected OpenALMusic( OpenALAudio audio, FileInfo file )
    {
        _audio               = audio;
        this.File            = file;
        OnCompletionListener = null;
    }

    public void Play()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( SourceId == INVALID_SOURCE_ID )
        {
            SourceId = _audio.ObtainSource( true );

            if ( SourceId == INVALID_SOURCE_ID )
            {
                return;
            }

            _audio.Music.Add( this );

            if ( _buffers == null )
            {
                _buffers = new uint[ _bufferCount ];

                AL.GenBuffers( 1, out _buffers );

                var err = AL.GetError();

                if ( err != AL.NO_ERROR )
                {
                    throw new GdxRuntimeException( $"Unable to allocate audio buffers. AL Error: {err}" );
                }
            }

            AL.Sourcei( ( uint ) SourceId, AL.DIRECT_CHANNELS_SOFT, AL.TRUE );
            AL.Sourcei( ( uint ) SourceId, AL.LOOPING, AL.FALSE );

            SetPan( _pan, _volume );

            AL.SourceQueueBuffers( ( uint ) SourceId, _bufferCount, _buffers );

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
            AL.SourcePlay( ( uint ) SourceId );
            _isPlaying = true;
        }
    }

    public void Stop()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( SourceId == INVALID_SOURCE_ID )
        {
            return;
        }

        _audio.Music.Remove( this );

        Reset();

        _audio.FreeSource( SourceId );

        SourceId         = INVALID_SOURCE_ID;
        _renderedSeconds = 0;

        _renderedSecondsQueue.Clear();

        _isPlaying = false;
    }

    public void Pause()
    {
//        if ( _audio.NoDevice )
//        {
//            return;
//        }
//
//        if ( SourceId != INVALID_SOURCE_ID )
//        {
//            AL.SourcePause( SourceId );
//        }
//
//        _isPlaying = false;
    }

    public void SetVolume( float volume )
    {
//        _volume = volume;
//
//        if ( _audio.NoDevice )
//        {
//            return;
//        }
//
//        if ( SourceId != INVALID_SOURCE_ID )
//        {
//            AL.Sourcef( SourceId, AL.GAIN, volume );
//        }
    }

    public float GetVolume()
    {
        return _volume;
    }

    public void SetPan( float pan, float volume )
    {
//        _volume = volume;
//        _pan    = pan;
//
//        if ( _audio.NoDevice )
//        {
//            return;
//        }
//
//        if ( SourceId == INVALID_SOURCE_ID )
//        {
//            return;
//        }
//
//        AL.Source3F( SourceId,
//                    AL.POSITION,
//                    MathUtils.Cos( ( pan - 1 ) * MathUtils.HALF_PI ),
//                    0,
//                    MathUtils.Sin( ( pan + 1 ) * MathUtils.HALF_PI ) );
//
//        AL.Sourcef( SourceId, AL.GAIN, volume );
    }

    public void SetPosition( float position )
    {
//        if ( _audio.NoDevice )
//        {
//            return;
//        }
//
//        if ( SourceId == INVALID_SOURCE_ID )
//        {
//            return;
//        }
//
//        var wasPlaying = IsPlaying;
//        IsPlaying = false;
//        alSourceStop( SourceId );
//        alSourceUnqueueBuffers( SourceId, _buffers );
//
//        while ( _renderedSecondsQueue.size > 0 )
//        {
//            _renderedSeconds = _renderedSecondsQueue.pop();
//        }
//
//        if ( position <= _renderedSeconds )
//        {
//            Reset();
//            _renderedSeconds = 0;
//        }
//
//        while ( _renderedSeconds < ( position - _maxSecondsPerBuffer ) )
//        {
//            if ( Read( _tempBytes ) <= 0 )
//            {
//                break;
//            }
//
//            _renderedSeconds += _maxSecondsPerBuffer;
//        }
//
//        _renderedSecondsQueue.add( _renderedSeconds );
//        var filled = false;
//
//        for ( var i = 0; i < _bufferCount; i++ )
//        {
//            int bufferID = _buffers.get( i );
//
//            if ( !Fill( bufferID ) )
//            {
//                break;
//            }
//
//            filled = true;
//            alSourceQueueBuffers( SourceId, bufferID );
//        }
//
//        _renderedSecondsQueue.pop();
//
//        if ( !filled )
//        {
//            Stop();
//
//            if ( OnCompletionListener != null )
//            {
//                OnCompletionListener.onCompletion( this );
//            }
//        }
//
//        AL.Sourcef( SourceId, AL11.Al.SEC_OFFSET, position - _renderedSeconds );
//
//        if ( wasPlaying )
//        {
//            AL.SourcePlay( SourceId );
//            IsPlaying = true;
//        }
    }

    public float GetPosition()
    {
        if ( _audio.NoDevice )
        {
            return 0;
        }

        if ( SourceId == INVALID_SOURCE_ID )
        {
            return 0;
        }

        AL.GetSourcef( ( uint ) SourceId, AL.SEC_OFFSET, out var value );

        return _renderedSeconds + value;
    }

    public void Dispose()
    {
//        Stop();
//
//        if ( _audio.NoDevice || ( _buffers == null ) )
//        {
//            return;
//        }
//
//        AL.DeleteBuffers( _buffers );
//
//        _buffers             = null;
//        OnCompletionListener = null;
    }

    protected void Setup( int channels, int sampleRate )
    {
        _format              = channels > 1 ? AL.FORMAT_STEREO16 : AL.FORMAT_MONO16;
        _sampleRate          = sampleRate;
        _maxSecondsPerBuffer = ( float ) _bufferSize / ( _bytesPerSample * channels * sampleRate );
    }

    /// <summary>
    /// Fills as much of the buffer as possible and returns the number of
    /// bytes filled. Returns &lt;= 0 to indicate the end of the stream.
    /// </summary>
    public abstract int Read( byte[] buffer );

    /// <summary>
    /// Resets the stream to the beginning.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// By default, does just the same as reset().
    /// Used to add special behaviour in Ogg.Music.
    /// </summary>
    protected void Loop()
    {
        Reset();
    }

    public int GetChannels()
    {
        return _format == AL.FORMAT_STEREO16 ? 2 : 1;
    }

    public int GetRate()
    {
        return _sampleRate;
    }

    public void Update()
    {
//        if ( _audio.NoDevice )
//        {
//            return;
//        }
//
//        if ( SourceId == INVALID_SOURCE_ID )
//        {
//            return;
//        }
//
//        var end     = false;
//        int buffers = AL.GetSourcei( SourceId, AL.BUFFERS_PROCESSED );
//
//        while ( buffers-- > 0 )
//        {
//            int bufferID = AL.SourceUnqueueBuffers( SourceId );
//
//            if ( bufferID == AL.INVALID_VALUE )
//            {
//                break;
//            }
//
//            if ( _renderedSecondsQueue.Count > 0 )
//            {
//                _renderedSeconds = _renderedSecondsQueue.Pop();
//            }
//
//            if ( end )
//            {
//                continue;
//            }
//
//            if ( Fill( bufferID ) )
//            {
//                AL.SourceQueueBuffers( SourceId, bufferID );
//            }
//            else
//            {
//                end = true;
//            }
//        }
//
//        if ( end && ( AL.GetSourcei( SourceId, AL.BUFFERS_QUEUED ) == 0 ) )
//        {
//            Stop();
//
//            if ( OnCompletionListener != null )
//            {
//                OnCompletionListener.OnCompletion( this );
//            }
//        }
//
//        // A buffer underflow will cause the source to stop.
//        if ( _isPlaying && ( AL.GetSourcei( SourceId, AL.SOURCE_STATE ) != AL.PLAYING ) )
//        {
//            AL.SourcePlay( SourceId );
//        }
    }

    private bool Fill( uint bufferID )
    {
//        _tempBuffer.Clear();
//
//        int length;
//
//        if ( ( length = Read( _tempBytes ) ) <= 0 )
//        {
//            if ( IsLooping )
//            {
//                Loop();
//
//                if ( ( length = Read( _tempBytes ) ) <= 0 )
//                {
//                    return false;
//                }
//
//                if ( _renderedSecondsQueue.Count > 0 )
//                {
//                    _renderedSecondsQueue[ 0 ] = 0;
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }
//
//        var previousLoadedSeconds = _renderedSecondsQueue.Count > 0 ? _renderedSecondsQueue.First() : 0;
//        var currentBufferSeconds  = ( _maxSecondsPerBuffer * length ) / _bufferSize;
//
//        _renderedSecondsQueue.Insert( 0, previousLoadedSeconds + currentBufferSeconds );
//
//        _tempBuffer.Put( _tempBytes, 0, length ).Flip();
//
//        AL.BufferData( bufferID,
//                       _format,
//                       _tempBuffer,
//                       _tempBuffer.BackingArray().Length,
//                       _sampleRate );

        return true;
    }

    // ========================================================================
    // ========================================================================

    #region properties

    public int                           SourceId             { get; private set; } = INVALID_SOURCE_ID;
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

            return ( SourceId != INVALID_SOURCE_ID ) && _isPlaying;
        }
        set => _isPlaying = value;
    }

    #endregion properties
}