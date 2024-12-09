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


namespace Corelib.Lugh.Audio;

[PublicAPI]
public interface ISound : IDisposable
{
    /// <summary>
    /// Plays the sound. If the sound is already playing, it will be played again, concurrently.
    /// </summary>
    /// <param name="volume"> the volume in the range [0,1] </param>
    /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
    long Play( float volume );

    /// <summary>
    /// Plays the sound. If the sound is already playing, it will be played again, concurrently.
    /// </summary>
    /// <param name="volume"> the volume in the range [0,1] </param>
    /// <param name="pitch">
    /// the pitch multiplier, 1 == default, greater than 1 == faster, less than 1 == slower,
    /// the value has to be between '0.5' and '2.0'
    /// </param>
    /// <param name="pan">
    /// panning in the range -1 (full left) to 1 (full right). 0 is center position.
    /// </param>
    /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
    long Play( float volume, float pitch, float pan );

    /// <summary>
    /// Plays the sound, looping. If the sound is already playing, it will be played
    /// again, concurrently. You need to stop the sound via a call to <see cref="Stop(long)"/>
    /// using the returned id.
    /// </summary>
    /// <param name="volume"> the volume in the range [0, 1] </param>
    /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
    long Loop( float volume );

    /// <summary>
    /// Plays the sound, looping. If the sound is already playing, it will be played again,
    /// concurrently. You need to stop the sound via a call to <see cref="Stop(long)"/>
    /// using the returned id.
    /// </summary>
    /// <param name="volume"> the volume in the range [0,1] </param>
    /// <param name="pitch">
    /// the pitch multiplier, 1 == default, greater than 1 == faster, less than 1 == slower,
    /// the value has to be between 0.5 and 2.0
    /// </param>
    /// <param name="pan">
    /// panning in the range -1 (full left) to 1 (full right). 0 is center position.
    /// </param>
    /// <returns> the id of the sound instance if successful, or -1 on failure.  </returns>
    long Loop( float volume, float pitch, float pan );

    /// <summary>
    /// Stops playing all instances of this sound.
    /// </summary>
    void Stop();

    /// <summary>
    /// Pauses all instances of this sound.
    /// </summary>
    void Pause();

    /// <summary>
    /// Resumes all paused instances of this sound.
    /// </summary>
    void Resume();

    /// <summary>
    /// Stops the sound instance with the given id as returned by <see cref="Play(float)"/> or
    /// <see cref="Play(float,float,float)"/>. If the sound is no longer playing, this has no effect.
    /// </summary>
    /// <param name="soundId"> the sound id  </param>
    void Stop( long soundId );

    /// <summary>
    /// Pauses the sound instance with the given id as returned by <see cref="Play(float)"/>
    /// or <see cref="Play(float,float,float)"/>. If the sound is no longer playing, this has no effect.
    /// </summary>
    /// <param name="soundId"> the sound id  </param>
    void Pause( long soundId );

    /// <summary>
    /// Resumes the sound instance with the given id as returned by <see cref="Play(float)"/>
    /// or <see cref="Play(float,float,float)"/>. If the sound is not paused, this has no effect.
    /// </summary>
    /// <param name="soundId"> the sound id  </param>
    void Resume( long soundId );

    /// <summary>
    /// Sets the sound instance with the given id to be looping. If the sound is no longer
    /// playing this has no effect.
    /// </summary>
    /// <param name="soundId"> the sound id </param>
    /// <param name="looping"> whether to loop or not.  </param>
    void SetLooping( long soundId, bool looping );

    /// <summary>
    /// Changes the pitch multiplier of the sound instance with the given id as returned
    /// by <see cref="Play(float)"/> or <see cref="Play(float,float,float)"/>.
    /// If the sound is no longer playing, this has no effect.
    /// </summary>
    /// <param name="soundId"> the sound id </param>
    /// <param name="pitch">
    /// the pitch multiplier, 1 == default, greater than 1 == faster, less than 1 == slower,
    /// the value has to be between 0.5 and 2.0
    /// </param>
    void SetPitch( long soundId, float pitch );

    /// <summary>
    /// Changes the volume of the sound instance with the given id as returned by
    /// <see cref="Play(float)"/> or <see cref="Play(float,float,float)"/>. If the
    /// sound is no longer playing, this has no effect.
    /// </summary>
    /// <param name="soundId"> the sound id </param>
    /// <param name="volume"> the volume in the range 0 (silent) to 1 (max volume).  </param>
    void SetVolume( long soundId, float volume );

    /// <summary>
    /// Sets the panning and volume of the sound instance with the given id as returned
    /// by <see cref="Play(float)"/> or <see cref="Play(float,float,float)"/>.
    /// If the sound is no longer playing, this has no effect. Note that panning only works
    /// for mono sounds, not for stereo sounds!
    /// </summary>
    /// <param name="soundId"> the sound id </param>
    /// <param name="pan"> panning in the range -1 (full left) to 1 (full right). 0 is center position. </param>
    /// <param name="volume"> the volume in the range [0,1].  </param>
    void SetPan( long soundId, float pan, float volume );
}
