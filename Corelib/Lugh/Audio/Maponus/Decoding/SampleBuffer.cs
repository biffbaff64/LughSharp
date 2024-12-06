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
/// The SampleBuffer class implements an output buffer that provides storage for a
/// fixed size block of samples.
/// </summary>
[PublicAPI]
public class SampleBuffer : AudioBase
{
    /// <summary>
    /// Gets or sets the sample frequency of the audio samples.
    /// </summary>
    public virtual int SampleFrequency { get; set; }

    /// <summary>
    /// Gets the number of channels in the audio samples.
    /// </summary>
    public virtual int ChannelCount => _channels;

    /// <summary>
    /// Gets the buffer containing the audio samples.
    /// </summary>
    public virtual short[] Buffer => _buffer;

    /// <summary>
    /// Gets the length of the buffer.
    /// </summary>
    public virtual int BufferLength => _bufferp[ 0 ];

    private readonly short[] _buffer;
    private readonly int[]   _bufferp;
    private readonly int     _channels;

    // ========================================================================

    /// <summary>
    /// Initializes a new instance of the <see cref="SampleBuffer"/> class with the
    /// specified sample frequency and number of channels.
    /// </summary>
    /// <param name="sampleFrequency">The sample frequency of the audio samples.</param>
    /// <param name="numberOfChannels">The number of channels in the audio samples.</param>
    public SampleBuffer( int sampleFrequency, int numberOfChannels )
    {
        _buffer   = new short[ OBUFFERSIZE ];
        _bufferp  = new int[ MAXCHANNELS ];
        _channels = numberOfChannels;

        Init( sampleFrequency );

        for ( var i = 0; i < numberOfChannels; ++i )
        {
            _bufferp[ i ] = ( short ) i;
        }
    }

    private void Init( int sampleFrequency )
    {
        SampleFrequency = sampleFrequency;
    }

    /// <summary>
    /// Appends a 16-bit PCM sample to the buffer.
    /// </summary>
    /// <param name="channel">The channel index.</param>
    /// <param name="valueRenamed">The 16-bit PCM sample value.</param>
    protected override void Append( int channel, short valueRenamed )
    {
        _buffer[ _bufferp[ channel ] ] =  valueRenamed;
        _bufferp[ channel ]            += _channels;
    }

    /// <summary>
    /// Appends an array of floating-point samples to the buffer.
    /// </summary>
    /// <param name="channel">The channel index.</param>
    /// <param name="samples">The array of floating-point samples.</param>
    public override void AppendSamples( int channel, float[] samples )
    {
        var pos = _bufferp[ channel ];

        for ( var i = 0; i < 32; )
        {
            var fs = samples[ i++ ];
            
            fs = fs > 32767.0f ? 32767.0f : fs < -32767.0f ? -32767.0f : fs;

            // Converting to short
            var s = ( short ) fs;

            _buffer[ pos ] =  s;
            pos            += _channels;
        }

        _bufferp[ channel ] = pos;
    }

    /// <summary>
    /// Writes the buffer to the file (Random Access).
    /// </summary>
    public override void WriteBuffer( int val )
    {
        // Implementation omitted
    }

    /// <summary>
    /// Closes the buffer.
    /// </summary>
    public override void Close()
    {
        // Implementation omitted
    }

    /// <summary>
    /// Clears the buffer.
    /// </summary>
    public override void ClearBuffer()
    {
        for ( var i = 0; i < _channels; ++i )
        {
            _bufferp[ i ] = ( short ) i;
        }
    }

    /// <summary>
    /// Sets the stop flag.
    /// </summary>
    public override void SetStopFlag()
    {
        // Implementation omitted
    }
}
