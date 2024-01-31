// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LibGDXSharp.Gdx.Audio.MP3Sharp.Decoding;

/// <summary>
///     The SampleBuffer class implements an output buffer that provides storage for a
///     fixed size block of samples.
/// </summary>
[PublicAPI]
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
