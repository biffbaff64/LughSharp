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


namespace Corelib.LibCore.Audio;

[PublicAPI]
public interface IMusic : IDisposable
{
    /// <summary>
    /// Returns whether or not this music stream is playing.
    /// </summary>
    bool IsPlaying { get; set; }

    /// <summary>
    /// Returns whether or not the music stream is set to loop.
    /// </summary>
    bool IsLooping { get; set; }

    /// <summary>
    /// Register a callback to be invoked when the end of a music stream has been reached during playback.
    /// </summary>
    IOnCompletionListener? OnCompletionListener { get; set; }

    /// <summary>
    /// Starts the play back of the music stream. In case the stream was paused
    /// this will resume the play back. In case the music stream is finished playing
    /// this will restart the play back.
    /// </summary>
    void Play();

    /// <summary>
    /// Pauses the play back. If the music stream has not been started yet or has
    /// finished playing a call to this method will be ignored.
    /// </summary>
    void Pause();

    /// <summary>
    /// Stops a playing or paused Music instance. Next time play() is invoked the
    /// Music will start from the beginning.
    /// </summary>
    void Stop();

    /// <summary>
    /// Sets the volume of this music stream. The volume must be given in the
    /// range [0,1] with 0 being silent and 1 being the maximum volume.
    /// </summary>
    void SetVolume( float volume );

    /// <summary>
    /// Returns the volume of this music stream.
    /// </summary>
    float GetVolume();

    /// <summary>
    /// Sets the panning and volume of this music stream.
    /// </summary>
    /// <param name="pan"> panning in the range -1 (full left) to 1 (full right). 0 is center position.</param>
    /// <param name="volume"> the volume in the range [0,1]. </param>
    void SetPan( float pan, float volume );

    /// <summary>
    /// Set the playback position in seconds.
    /// </summary>
    void SetPosition( float position );

    /// <summary>
    /// Returns the playback position in seconds.
    /// </summary>
    float GetPosition();

    /// <summary>
    /// Interface definition for a callback to be invoked when playback of a music stream has completed.
    /// </summary>
    [PublicAPI]
    interface IOnCompletionListener
    {
        /// <summary>
        /// Called when the end of a media source is reached during playback.
        /// </summary>
        /// <param name="music"> the Music that reached the end of the file</param>
        void OnCompletion( IMusic music );
    }
}
