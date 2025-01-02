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
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Buffers;
using JetBrains.Annotations;

namespace DesktopGLBackend.Audio;

[PublicAPI]
public class OpenALSound : ISound
{
    private readonly OpenALAudio _audio;

    private int _bufferID = -1;

    public OpenALSound( OpenALAudio audio )
    {
        _audio = audio;
    }

    public float Duration { get; set; }

    public long Play( float volume = 1f )
    {
        if ( _audio.NoDevice )
        {
            return 0;
        }

        var sourceID = _audio.ObtainSource( false );

        if ( sourceID == -1 )
        {
            // Attempt to recover by stopping the least recently played sound
            _audio.Retain( this, true );
            sourceID = _audio.ObtainSource( false );
        }
        else
        {
            _audio.Retain( this, false );
        }

        // In case it still didn't work
        if ( sourceID == -1 )
        {
            return -1;
        }

        var soundId = _audio.GetSoundId( sourceID );

        AL.Sourcei( ( uint ) sourceID, AL.BUFFER, _bufferID );
        AL.Sourcei( ( uint ) sourceID, AL.LOOPING, AL.FALSE );
        AL.Sourcef( ( uint ) sourceID, AL.GAIN, volume );
        AL.SourcePlay( ( uint ) sourceID );

        return soundId;
    }

    public long Loop( float volume = 1f )
    {
        if ( _audio.NoDevice )
        {
            return 0;
        }

        var sourceID = _audio.ObtainSource( false );

        if ( sourceID == -1 )
        {
            return -1;
        }

        var soundId = _audio.GetSoundId( sourceID );

        AL.Sourcei( ( uint ) sourceID, AL.BUFFER, _bufferID );
        AL.Sourcei( ( uint ) sourceID, AL.LOOPING, AL.TRUE );
        AL.Sourcef( ( uint ) sourceID, AL.GAIN, volume );
        AL.SourcePlay( ( uint ) sourceID );

        return soundId;
    }

    public void Stop()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.StopSourcesWithBuffer( _bufferID );
    }

    public void Dispose()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        if ( _bufferID == -1 )
        {
            return;
        }

        _audio.FreeBuffer( _bufferID );

//TODO: AL.DeleteBuffers( _bufferID );

        _bufferID = -1;

        _audio.Forget( this );
    }

    public void Stop( long soundId )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.StopSound( soundId );
    }

    public void Pause()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.PauseSourcesWithBuffer( _bufferID );
    }

    public void Pause( long soundId )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.PauseSound( soundId );
    }

    public void Resume()
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.ResumeSourcesWithBuffer( _bufferID );
    }

    public void Resume( long soundId )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.ResumeSound( soundId );
    }

    public void SetPitch( long soundId, float pitch )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.SetSoundPitch( soundId, pitch );
    }

    public void SetVolume( long soundId, float volume )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.SetSoundGain( soundId, volume );
    }

    public void SetLooping( long soundId, bool looping )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.SetSoundLooping( soundId, looping );
    }

    public void SetPan( long soundId, float pan, float volume )
    {
        if ( _audio.NoDevice )
        {
            return;
        }

        _audio.SetSoundPan( soundId, pan, volume );
    }

    public long Play( float volume, float pitch, float pan )
    {
        var id = Play();

        SetPitch( id, pitch );
        SetPan( id, pan, volume );

        return id;
    }

    public long Loop( float volume, float pitch, float pan )
    {
        var id = Loop();

        SetPitch( id, pitch );
        SetPan( id, pan, volume );

        return id;
    }

    protected void Setup( byte[] pcm, int channels, int sampleRate )
    {
        var bytes   = pcm.Length - ( pcm.Length % ( channels > 1 ? 4 : 2 ) );
        var samples = bytes / ( 2 * channels );

        Duration = samples / ( float ) sampleRate;

        var buffer = ByteBuffer.Allocate( bytes );

        buffer.Order( ByteOrder.NativeOrder ).Put( pcm, 0, bytes ).Flip();

//TODO:
//        if ( _bufferID == -1 )
//        {
//            _bufferID = AL.GenBuffers();
//
//            AL.BufferData( _bufferID,
//                           channels > 1
//                               ? AL.FORMAT_STEREO16
//                               : AL.FORMAT_MONO16,
//                           buffer.AsShortBuffer(),
//                           sampleRate );
//        }
    }
}
