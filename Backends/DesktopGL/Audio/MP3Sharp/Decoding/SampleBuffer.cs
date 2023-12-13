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

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;


/// <summary>
/// The SampleBuffer class implements an output buffer
/// that provides storage for a fixed size block of samples.
/// </summary>
public class SampleBuffer : ABuffer {
    private readonly short[] _Buffer;
    private readonly int[]   _Bufferp;
    private readonly int     _Channels;
    private readonly int     _Frequency;

    public SampleBuffer(int sampleFrequency, int numberOfChannels) {
        _Buffer    = new short[OBUFFERSIZE];
        _Bufferp   = new int[MAXCHANNELS];
        _Channels  = numberOfChannels;
        _Frequency = sampleFrequency;

        for (int i = 0; i < numberOfChannels; ++i)
            _Bufferp[i] = (short)i;
    }

    public virtual int ChannelCount => _Channels;

    public virtual int SampleFrequency => _Frequency;

    public virtual short[] Buffer => _Buffer;

    public virtual int BufferLength => _Bufferp[0];

    /// <summary>
    /// Takes a 16 Bit PCM sample.
    /// </summary>
    protected override void Append(int channel, short valueRenamed) {
        _Buffer[_Bufferp[channel]] =  valueRenamed;
        _Bufferp[channel]          += _Channels;
    }

    public override void AppendSamples(int channel, float[] samples) {
        int pos = _Bufferp[channel];

        short s;
        float fs;
        for (int i = 0; i < 32;) {
            fs = samples[i++];
            fs = fs > 32767.0f ? 32767.0f : fs < -32767.0f ? -32767.0f : fs;

            //UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
            s            =  (short)fs;
            _Buffer[pos] =  s;
            pos          += _Channels;
        }

        _Bufferp[channel] = pos;
    }

    /// <summary>
    /// Write the samples to the file (Random Acces).
    /// </summary>
    public override void WriteBuffer(int val) {
        // for (int i = 0; i < channels; ++i) 
        // bufferp[i] = (short)i;
    }

    public override void Close() { }

    /// <summary>
    /// 
    /// </summary>
    public override void ClearBuffer() {
        for (int i = 0; i < _Channels; ++i)
            _Bufferp[i] = (short)i;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void SetStopFlag() { }
}