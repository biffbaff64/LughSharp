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

namespace LibGDXSharp.Backends.Desktop.Audio;

[PublicAPI]
public class OpenALSound : ISound
{
    public float Duration { get; set; }

    private int           _bufferID = -1;
    private OpenALGLAudio _audio;

    public OpenALSound( OpenALGLAudio audio )
    {
        this._audio = audio;
    }

    private void Setup( byte[] pcm, int channels, int sampleRate )
    {
        var bytes   = pcm.Length - ( pcm.Length % ( channels > 1 ? 4 : 2 ) );
        var samples = bytes / ( 2 * channels );

        Duration = samples / ( float )sampleRate;

        ByteBuffer buffer = ByteBuffer.AllocateDirect( bytes );

        buffer.Order( ByteOrder.NativeOrder )
              .Put( pcm, 0, bytes )
              .Flip();

        if ( _bufferID == -1 )
        {
            _bufferID = Al.GenBuffers();

            Al.BufferData( _bufferID,
                           channels > 1
                               ? Al.FormatStereo16
                               : Al.FormatMono16,
                           buffer.AsShortBuffer(),
                           sampleRate );
        }
    }

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

        Al.Sourcei( sourceID, Al.Buffer, _bufferID );
        Al.Sourcei( sourceID, Al.Looping, Al.False );
        Al.Sourcef( sourceID, Al.Gain, volume );
        Al.SourcePlay( sourceID );

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

        Al.Sourcei( sourceID, Al.Buffer, _bufferID );
        Al.Sourcei( sourceID, Al.Looping, Al.True );
        Al.Sourcef( sourceID, Al.Gain, volume );
        Al.SourcePlay( sourceID );

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
        Al.DeleteBuffers( _bufferID );

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
}
