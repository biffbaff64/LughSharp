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

using Corelib.LibCore.Audio.Maponus.Decoding;
using Exception = System.Exception;

namespace Corelib.LibCore.Audio.Maponus;

/// <summary>
/// public class used to queue samples that are being obtained from an Mp3 stream.
/// This class handles stereo 16-bit output, and can double 16-bit mono to stereo.
/// </summary>
[PublicAPI]
public class Buffer16BitStereo : AudioBase
{
    // ========================================================================

    private const int OUTPUT_CHANNELS = 2;

    // Write offset used in append_bytes
    private readonly byte[] _buffer               = new byte[ OBUFFERSIZE * 2 ]; // all channels interleaved
    private readonly int[]  _bufferChannelOffsets = new int[ MAXCHANNELS ];      // contains write offset for each channel.

    // end marker, one past end of array. Same as bufferp[0], but
    // without the array bounds check.
    private int _end;

    // Read offset used to read from the stream, in bytes.
    private int _offset;

    // ========================================================================

    public Buffer16BitStereo()
    {
        OnStart();
    }

    public bool DoubleMonoToStereo { get; set; } = false;

    /// <summary>
    /// Gets the number of bytes remaining from the current position on the buffer.
    /// </summary>
    public int BytesLeft => _end - _offset;

    /// <summary>
    /// Initialisation method. Called from constructor as the method <see cref="ClearBuffer"/>
    /// is virtual and cannot by called from constructors.
    /// </summary>
    private void OnStart()
    {
        // Initialize the buffer pointers
        ClearBuffer();
    }

    /// <summary>
    /// Reads a sequence of bytes from the buffer and advances the position of the
    /// buffer by the number of bytes read.
    /// </summary>
    /// <returns>
    /// The total number of bytes read in to the buffer. This can be less than the
    /// number of bytes requested if that many bytes are not currently available, or
    /// zero if th eend of the buffer has been reached.
    /// </returns>
    public int Read( byte[] bufferOut, int offset, int count )
    {
        ArgumentNullException.ThrowIfNull( bufferOut );

        if ( ( count + offset ) > bufferOut.Length )
        {
            throw new ArgumentException( "The sum of offset and count is larger than the buffer length" );
        }

        var remaining = BytesLeft;
        int copySize;

        if ( count > remaining )
        {
            copySize = remaining;
        }
        else
        {
            // Copy an even number of sample frames
            var remainder = count % ( 2 * OUTPUT_CHANNELS );
            copySize = count - remainder;
        }

        Array.Copy( _buffer, _offset, bufferOut, offset, copySize );
        _offset += copySize;

        return copySize;
    }

    /// <summary>
    /// Writes a single sample value to the buffer.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="sampleValue">The sample value.</param>
    protected override void Append( int channel, short sampleValue )
    {
        _buffer[ _bufferChannelOffsets[ channel ] ]     =  ( byte ) ( sampleValue & 0xff );
        _buffer[ _bufferChannelOffsets[ channel ] + 1 ] =  ( byte ) ( sampleValue >> 8 );
        _bufferChannelOffsets[ channel ]                += OUTPUT_CHANNELS * 2;
    }

    /// <summary>
    /// Writes 32 samples to the buffer.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="samples">An array of sample values.</param>
    /// <remarks>
    /// The <paramref name="samples"/> parameter must have a length equal to
    /// or greater than 32.
    /// </remarks>
    public override void AppendSamples( int channel, float[] samples )
    {
        // samples is required.
        ArgumentNullException.ThrowIfNull( samples );

        if ( samples.Length < 32 )
        {
            throw new ArgumentException( "samples must have 32 values" );
        }

        if ( ( _bufferChannelOffsets == null ) || ( channel >= _bufferChannelOffsets.Length ) )
        {
            throw new Exception( "Song is closing..." );
        }

        var pos = _bufferChannelOffsets[ channel ];

        // Always, 32 samples are appended
        for ( var i = 0; i < 32; i++ )
        {
            var fs = samples[ i ];

            fs = fs switch
            {
                // clamp values
                > 32767.0f  => 32767.0f,
                < -32767.0f => -32767.0f,
                var _       => fs,
            };

            var sample = ( int ) fs;

            _buffer[ pos ]     = ( byte ) ( sample & 0xff );
            _buffer[ pos + 1 ] = ( byte ) ( sample >> 8 );

            if ( DoubleMonoToStereo )
            {
                _buffer[ pos + 2 ] = ( byte ) ( sample & 0xff );
                _buffer[ pos + 3 ] = ( byte ) ( sample >> 8 );
            }

            pos += OUTPUT_CHANNELS * 2;
        }

        _bufferChannelOffsets[ channel ] = pos;
    }

    /// <summary>
    /// This implementation does not clear the buffer.
    /// </summary>
    public override void ClearBuffer()
    {
        _offset = 0;
        _end    = 0;

        for ( var i = 0; i < OUTPUT_CHANNELS; i++ )
        {
            _bufferChannelOffsets[ i ] = i * 2; // two bytes per channel
        }
    }

    /// <inheritdoc />
    public override void SetStopFlag()
    {
    }

    public override void WriteBuffer( int val )
    {
        _offset = 0;

        // speed optimization - save end marker, and avoid
        // array access at read time. Can you believe this saves
        // like 1-2% of the cpu on a PIII? I guess allocating
        // that temporary "new int(0)" is expensive, too.
        _end = _bufferChannelOffsets[ 0 ];
    }

    public override void Close()
    {
    }
}
