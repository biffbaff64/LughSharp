// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Audio.MP3Sharp;

/// <summary>
///     Base Class for audio output.
/// </summary>
public abstract class ABuffer
{
    public const int OBUFFERSIZE = 2 * 1152; // max. 2 * 1152 samples per frame
    public const int MAXCHANNELS = 2;        // max. number of channels

    /// <summary>
    ///     Takes a 16 Bit PCM sample.
    /// </summary>
    protected abstract void Append( int channel, short sampleValue );

    /// <summary>
    ///     Accepts 32 new PCM samples.
    /// </summary>
    public virtual void AppendSamples( int channel, float[] samples )
    {
        for ( var i = 0; i < 32; i++ )
        {
            Append( channel, Clip( samples[ i ] ) );
        }
    }

    /// <summary>
    ///     Clip Sample to 16 Bits
    /// </summary>
    private static short Clip( float sample ) => sample > 32767.0f
        ? ( short )32767
        : sample < -32768.0f
            ? ( short )-32768
            : ( short )sample;

    /// <summary>
    ///     Write the samples to the file or directly to the audio hardware.
    /// </summary>
    public abstract void WriteBuffer( int val );

    public abstract void Close();

    /// <summary>
    ///     Clears all data in the buffer (for seeking).
    /// </summary>
    public abstract void ClearBuffer();

    /// <summary>
    ///     Notify the buffer that the user has stopped the stream.
    /// </summary>
    public abstract void SetStopFlag();
}
