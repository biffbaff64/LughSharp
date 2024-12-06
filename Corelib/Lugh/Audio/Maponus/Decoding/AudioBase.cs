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


namespace Corelib.Lugh.Audio.Maponus.Decoding;

/// <summary>
/// Base Class for audio output.
/// </summary>
[PublicAPI]
public abstract class AudioBase
{
    public const int OBUFFERSIZE = 2 * 1152; // max. 2 * 1152 samples per frame
    public const int MAXCHANNELS = 2;        // max. number of channels

    /// <summary>
    /// Takes a 16 Bit PCM sample.
    /// </summary>
    protected abstract void Append( int channel, short sampleValue );

    /// <summary>
    /// Accepts 32 new PCM samples.
    /// </summary>
    public virtual void AppendSamples( int channel, float[] samples )
    {
        for ( var i = 0; i < 32; i++ )
        {
            Append( channel, Clip( samples[ i ] ) );
        }
    }

    /// <summary>
    /// Clip Sample to 16 Bits
    /// </summary>
    private static short Clip( float sample )
    {
        return sample > 32767.0f
                   ? ( short ) 32767
                   : sample < -32768.0f
                       ? ( short ) -32768
                       : ( short ) sample;
    }

    /// <summary>
    /// Write the samples to the file or directly to the audio hardware.
    /// </summary>
    public abstract void WriteBuffer( int val );

    public abstract void Close();

    /// <summary>
    /// Clears all data in the buffer (for seeking).
    /// </summary>
    public abstract void ClearBuffer();

    /// <summary>
    /// Notify the buffer that the user has stopped the stream.
    /// </summary>
    public abstract void SetStopFlag();
}
