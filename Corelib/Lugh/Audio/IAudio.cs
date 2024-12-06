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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Audio;

[PublicAPI]
public interface IAudio
{
    /// <summary>
    /// Creates a new <see cref="IAudioDevice"/> either in mono or stereo mode.
    /// The AudioDevice has to be disposed via its Dispose() method when it is
    /// no longer used.
    /// </summary>
    /// <param name="samplingRate">The sampling rate.</param>
    /// <param name="isMono">Whether the AudioDevice should be in mono or stereo mode.</param>
    /// <returns>The AudioDevice.</returns>
    /// <exception cref="GdxRuntimeException">Thrown if the device could not be created.</exception>
    IAudioDevice NewAudioDevice( int samplingRate, bool isMono );

    /// <summary>
    /// Creates a new <see cref="IAudioRecorder"/>. The AudioRecorder has to be disposed
    /// after it is no longer used.
    /// </summary>
    /// <param name="samplingRate"> the sampling rate in Hertz </param>
    /// <param name="isMono"> whether the recorder records in mono or stereo </param>
    /// <returns> the AudioRecorder </returns>
    /// <exception cref="GdxRuntimeException"> in case the recorder could not be created</exception>
    IAudioRecorder NewAudioRecorder( int samplingRate, bool isMono );

    /// <summary>
    /// Creates a new <see cref="ISound"/> which is used to play back audio effects such as
    /// gun shots or explosions. The Sound's audio data is retrieved from the file specified
    /// via the <see cref="FileInfo"/>. Note that the complete audio data is loaded into
    /// RAM. You should therefore not load big audio files with this methods. The current upper
    /// limit for decoded audio is 1 MB. Currently supported formats are WAV, MP3 and OGG.
    /// The Sound has to be disposed if it is no longer used via the <see cref="IDisposable.Dispose()"/> method.
    /// </summary>
    /// <returns>The new sound.</returns>
    /// <exception cref="GdxRuntimeException">in case the sound could not be loaded.</exception>
    ISound NewSound( FileInfo? fileHandle );

    /// <summary>
    /// Creates a new <see cref="IMusic"/> instance which is used to play back a music stream
    /// from a file. Currently supported formats are WAV, MP3 and OGG. The Music instance has
    /// to be disposed if it is no longer used via the <see cref="IDisposable.Dispose()"/> method.
    /// Music instances are automatically paused when <see cref="IApplicationListener.Pause"/> is
    /// called and resumed when <see cref="IApplicationListener.Resume()"/> is called.
    /// </summary>
    /// <param name="file">The filehandle.</param>
    /// <return>the new Music or null if the Music could not be loaded.</return>
    /// <exception cref="GdxRuntimeException">in case the music could not be loaded.</exception>
    IMusic NewMusic( FileInfo? file );
}
