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


namespace Corelib.Lugh.Audio;

/// <summary>
/// Encapsulates an audio device in mono or stereo mode. Use the
/// WriteSamples(float[], int, int) and WriteSamples(short[], int, int)
/// methods to write float or 16-bit signed short PCM data directly to
/// the audio device. Stereo samples are interleaved in the order left
/// channel sample, right channel sample. The dispose() method must be
/// called when this AudioDevice is no longer needed.
/// </summary>
[PublicAPI]
public interface IAudioDevice : IDisposable
{
    /// <summary>
    /// Returns whether this AudioDevice is in mono or stereo mode.
    /// </summary>
    /// <returns>TRUE for mono, FALSE for stereo.</returns>
    bool IsMono();

    /// <summary>
    /// Writes the array of 16-bit signed PCM samples to the audio
    /// device and blocks until they have been processed.
    /// </summary>
    /// <param name="samples">The samples.</param>
    /// <param name="offset">The offset into the samples array.</param>
    /// <param name="numSamples">The number of samples to write to the device.</param>
    void WriteSamples( int[] samples, int offset, int numSamples );

    /// <summary>
    /// Writes the array of float PCM samples to the audio device and
    /// blocks until they have been processed.
    /// </summary>
    /// <param name="samples">The samples.</param>
    /// <param name="offset">The offset into the samples array.</param>
    /// <param name="numSamples">The number of samples to write to the device.</param>
    void WriteSamples( float[] samples, int offset, int numSamples );

    /// <summary>
    /// </summary>
    /// <returns>The latency in samples.</returns>
    int GetLatency();

    /// <summary>
    /// Sets the volume in the range [0,1].
    /// </summary>
    void SetVolume( float volume );
}
