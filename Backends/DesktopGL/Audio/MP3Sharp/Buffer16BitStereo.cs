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
/// public class used to queue samples that are being obtained from an Mp3 stream. 
/// This class handles stereo 16-bit output, and can double 16-bit mono to stereo.
/// </summary>
[PublicAPI]
public class Buffer16BitStereo : ABuffer
{
    public bool DoubleMonoToStereo { get; set; } = false;

    private const int OUTPUT_CHANNELS = 2;

    // Write offset used in append_bytes
    private readonly byte[] _buffer               = new byte[ OBUFFERSIZE * 2 ]; // all channels interleaved
    private readonly int[]  _bufferChannelOffsets = new int[ MAXCHANNELS ];      // contains write offset for each channel.

    // end marker, one past end of array. Same as bufferp[0], but
    // without the array bounds check.
    private int _end;

    // Read offset used to read from the stream, in bytes.
    private int _offset;

    public Buffer16BitStereo()
    {
        OnStart();
    }

    private void OnStart()
    {
        // Initialize the buffer pointers
        ClearBuffer();
    }
    
    /// <summary>
    /// Gets the number of bytes remaining from the current position on the buffer.
    /// </summary>
    public int BytesLeft => _end - _offset;

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
        _buffer[ _bufferChannelOffsets[ channel ] ]     =  ( byte )( sampleValue & 0xff );
        _buffer[ _bufferChannelOffsets[ channel ] + 1 ] =  ( byte )( sampleValue >> 8 );
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
            throw new System.Exception( "Song is closing..." );
        }

        var pos = _bufferChannelOffsets[ channel ];

        // Always, 32 samples are appended
        for ( var i = 0; i < 32; i++ )
        {
            var fs = samples[ i ];

            // clamp values
            if ( fs > 32767.0f )
            {
                fs = 32767.0f;
            }
            else if ( fs < -32767.0f )
            {
                fs = -32767.0f;
            }

            var sample = ( int )fs;
            
            _buffer[ pos ]     = ( byte )( sample & 0xff );
            _buffer[ pos + 1 ] = ( byte )( sample >> 8 );

            if ( DoubleMonoToStereo )
            {
                _buffer[ pos + 2 ] = ( byte )( sample & 0xff );
                _buffer[ pos + 3 ] = ( byte )( sample >> 8 );
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
