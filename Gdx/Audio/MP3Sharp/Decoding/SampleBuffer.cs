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

namespace LibGDXSharp.Audio.MP3Sharp.Decoding;

/// <summary>
///     The SampleBuffer class implements an output buffer that provides storage for a
///     fixed size block of samples.
/// </summary>
public class SampleBuffer : AudioBase
{
    private readonly short[] _buffer;
    private readonly int[]   _bufferp;
    private readonly int     _channels;

    public SampleBuffer( int sampleFrequency, int numberOfChannels )
    {
        _buffer   = new short[ OBUFFERSIZE ];
        _bufferp  = new int[ MAXCHANNELS ];
        _channels = numberOfChannels;

        Init( sampleFrequency );

        for ( var i = 0; i < numberOfChannels; ++i )
        {
            _bufferp[ i ] = ( short )i;
        }
    }

    public virtual int SampleFrequency { get; set; }

    public virtual int     ChannelCount => _channels;
    public virtual short[] Buffer       => _buffer;
    public virtual int     BufferLength => _bufferp[ 0 ];

    private void Init( int sampleFrequency ) => SampleFrequency = sampleFrequency;

    /// <summary>
    ///     Takes a 16 Bit PCM sample.
    /// </summary>
    protected override void Append( int channel, short valueRenamed )
    {
        _buffer[ _bufferp[ channel ] ] =  valueRenamed;
        _bufferp[ channel ]            += _channels;
    }

    public override void AppendSamples( int channel, float[] samples )
    {
        var pos = _bufferp[ channel ];

        for ( var i = 0; i < 32; )
        {
            var fs = samples[ i++ ];
            fs = fs > 32767.0f ? 32767.0f : fs < -32767.0f ? -32767.0f : fs;

            //UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#.
            //'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
            var s = ( short )fs;

            _buffer[ pos ] =  s;
            pos            += _channels;
        }

        _bufferp[ channel ] = pos;
    }

    /// <summary>
    ///     Write the samples to the file (Random Acces).
    /// </summary>
    public override void WriteBuffer( int val )
    {
        // for (int i = 0; i < channels; ++i) 
        // bufferp[i] = (short)i;
    }

    public override void Close()
    {
    }

    /// <summary>
    /// </summary>
    public override void ClearBuffer()
    {
        for ( var i = 0; i < _channels; ++i )
        {
            _bufferp[ i ] = ( short )i;
        }
    }

    /// <summary>
    /// </summary>
    public override void SetStopFlag()
    {
    }
}
