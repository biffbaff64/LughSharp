// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Audio.Maponus.Support;
using Exception = System.Exception;

namespace Corelib.Lugh.Audio.Maponus.Decoding;

/// <summary>
/// The Bistream class is responsible for parsing an MPEG audio bitstream.
/// </summary>
[PublicAPI]
public sealed class Bitstream
{
//TODO: much of the parsing currently occurs in the various decoders. This should be moved into this class and associated inner classes.

    /// <summary>
    /// Synchronization control constant for the initial
    /// synchronization to the start of a frame.
    /// </summary>
    public const sbyte INITIAL_SYNC = 0;

    /// <summary>
    /// Synchronization control constant for non-inital frame
    /// synchronizations.
    /// </summary>
    public const sbyte STRICT_SYNC = 1;

    /// <summary>
    /// Maximum size of the frame buffer:
    /// 1730 bytes per frame: 144 * 384kbit/s / 32000 Hz + 2 Bytes CRC
    /// </summary>
    private const int BUFFER_INT_SIZE = 433;

    private readonly int[] _bitmask =
    [
        0x00000000, 0x00000001, 0x00000003, 0x00000007, 0x0000000F, 0x0000001F,
        0x0000003F, 0x0000007F, 0x000000FF, 0x000001FF, 0x000003FF, 0x000007FF,
        0x00000FFF, 0x00001FFF, 0x00003FFF, 0x00007FFF, 0x0000FFFF, 0x0001FFFF,
    ];

    private readonly Crc16[] _crc;

    /// <summary>
    /// The frame buffer that holds the data for the current frame.
    /// </summary>
    private readonly int[] _frameBuffer;

    /// <summary>
    /// The bytes read from the stream.
    /// </summary>
    private readonly sbyte[] _frameBytes;

    private readonly Header         _header;
    private readonly PushbackStream _sourceStream;
    private readonly sbyte[]        _syncBuffer;

    /// <summary>
    /// Number (0-31, from MSB to LSB) of next bit for get_bits()
    /// </summary>
    private int _bitIndex;

    /// <summary>
    /// Number of valid bytes in the frame buffer.
    /// </summary>
    private int _frameSize;

    private bool _singleChMode;

    /// <summary>
    /// The current specified syncword
    /// </summary>
    private int _syncWord;

    /// <summary>
    /// Index into framebuffer where the next bits are retrieved.
    /// </summary>
    private int _wordPointer;

    /// <summary>
    /// Create a IBitstream that reads data from a given InputStream.
    /// </summary>
    public Bitstream( PushbackStream stream )
    {
        _crc         = new Crc16[ 1 ];
        _syncBuffer  = new sbyte[ 4 ];
        _frameBytes  = new sbyte[ BUFFER_INT_SIZE * 4 ];
        _frameBuffer = new int[ BUFFER_INT_SIZE ];
        _header      = new Header();

        _sourceStream = stream ?? throw new NullReferenceException( "in stream is null" );

        CloseFrame();
    }

    public void Close()
    {
        try
        {
            _sourceStream.Close();
        }
        catch ( IOException ex )
        {
            throw new BitstreamException( BitstreamErrors.STREA_ERROR, ex );
        }
    }

    /// <summary>
    /// Reads and parses the next frame from the input source.
    /// </summary>
    /// <returns>
    /// The Header describing details of the frame read,
    /// or null if the end of the stream has been reached.
    /// </returns>
    public Header? ReadFrame()
    {
        Header? result = null;

        try
        {
            result = ReadNextFrame();
        }
        catch ( BitstreamException ex )
        {
            if ( ex.ErrorCode != BitstreamErrors.STREAM_EOF )
            {
                // wrap original exception so stack trace is maintained.
                throw new BitstreamException( ex.ErrorCode, ex );
            }
        }

        return result;
    }

    private Header ReadNextFrame()
    {
        if ( _frameSize == -1 )
        {
            // entire frame is read by the header class.
            _header.ReadHeader( this, _crc );
        }

        return _header;
    }

    /// <summary>
    /// Unreads the bytes read from the frame.
    /// Throws BitstreamException.
    /// TODO: add new error codes for this.
    /// </summary>
    public void UnreadFrame()
    {
        if ( ( _wordPointer == -1 ) && ( _bitIndex == -1 ) && ( _frameSize > 0 ) )
        {
            try
            {
                _sourceStream.UnRead( _frameSize );
            }
            catch
            {
                throw new BitstreamException( BitstreamErrors.STREA_ERROR );
            }
        }
    }

    public void CloseFrame()
    {
        _frameSize   = -1;
        _wordPointer = -1;
        _bitIndex    = -1;
    }

    /// <summary>
    /// Determines if the next 4 bytes of the stream represent a frame header.
    /// </summary>
    public bool IsSyncCurrentPosition( int syncmode )
    {
        var read = ReadBytes( _syncBuffer, 0, 4 );

        var headerstring = ( ( _syncBuffer[ 0 ] << 24 ) & ( int ) SupportClass.Identity( 0xFF000000 ) )
                         | ( ( _syncBuffer[ 1 ] << 16 ) & 0x00FF0000 )
                         | ( ( _syncBuffer[ 2 ] << 8 ) & 0x0000FF00 )
                         | ( ( _syncBuffer[ 3 ] << 0 ) & 0x000000FF );

        try
        {
            _sourceStream.UnRead( read );
        }
        catch ( Exception e )
        {
            throw new Mp3SharpException( "Could not restore file after reading frame header.", e );
        }

        var sync = false;

        switch ( read )
        {
            case 0:
            {
                sync = true;

                break;
            }

            case 4:
            {
                sync = IsSyncMark( headerstring, syncmode, _syncWord );

                break;
            }
        }

        return sync;
    }

    /// <summary>
    /// Get next 32 bits from bitstream. which are stored in the headerstring.
    /// The returned value is False at the end of stream.
    /// </summary>
    /// <param name="syncmode"> allows Synchro flag ID </param>
    public int SyncHeader( sbyte syncmode )
    {
        var sync = false;

        // read additional 2 bytes
        var bytesRead = ReadBytes( _syncBuffer, 0, 3 );

        if ( bytesRead != 3 )
        {
            throw new BitstreamException( BitstreamErrors.STREAM_EOF );
        }

        var headerstring = ( ( _syncBuffer[ 0 ] << 16 ) & 0x00FF0000 )
                         | ( ( _syncBuffer[ 1 ] << 8 ) & 0x0000FF00 )
                         | ( ( _syncBuffer[ 2 ] << 0 ) & 0x000000FF );

        do
        {
            headerstring <<= 8;

            if ( ReadBytes( _syncBuffer, 3, 1 ) != 1 )
            {
                throw new BitstreamException( BitstreamErrors.STREAM_EOF );
            }

            headerstring |= _syncBuffer[ 3 ] & 0x000000FF;

            if ( CheckAndSkipId3Tag( headerstring ) )
            {
                bytesRead = ReadBytes( _syncBuffer, 0, 3 );

                if ( bytesRead != 3 )
                {
                    throw new BitstreamException( BitstreamErrors.STREAM_EOF );
                }

                headerstring = ( ( _syncBuffer[ 0 ] << 16 ) & 0x00FF0000 )
                             | ( ( _syncBuffer[ 1 ] << 8 ) & 0x0000FF00 )
                             | ( ( _syncBuffer[ 2 ] << 0 ) & 0x000000FF );

                continue;
            }

            sync = IsSyncMark( headerstring, syncmode, _syncWord );
        }
        while ( !sync );

        return headerstring;
    }

    /// <summary>
    /// check and skip the id3v2 tag.
    /// mp3 frame sync inside id3 tag may led false decodeing.
    /// id3 tag do have a flag for "unsynchronisation", indicate there are no
    /// frame sync inside tags, scence decoder don't care about tags, we just
    /// skip all tags.
    /// </summary>
    public bool CheckAndSkipId3Tag( int headerstring )
    {
        var id3 = ( headerstring & 0xFFFFFF00 ) == 0x49443300;

        if ( id3 )
        {
            var id3Header = new sbyte[ 6 ];

            if ( ReadBytes( id3Header, 0, 6 ) != 6 )
            {
                throw new BitstreamException( BitstreamErrors.STREAM_EOF );
            }

            // id3 header uses 4 bytes to store the size of all tags,
            // but only the low 7 bits of each byte is used, to avoid
            // mp3 frame sync.
            var id3TagSize = 0;
            id3TagSize |=  id3Header[ 2 ] & 0x0000007F;
            id3TagSize <<= 7;
            id3TagSize |=  id3Header[ 3 ] & 0x0000007F;
            id3TagSize <<= 7;
            id3TagSize |=  id3Header[ 4 ] & 0x0000007F;
            id3TagSize <<= 7;
            id3TagSize |=  id3Header[ 5 ] & 0x0000007F;

            var id3Tag = new sbyte[ id3TagSize ];

            if ( ReadBytes( id3Tag, 0, id3TagSize ) != id3TagSize )
            {
                throw new BitstreamException( BitstreamErrors.STREAM_EOF );
            }
        }

        return id3;
    }

    public bool IsSyncMark( int headerstring, int syncmode, int word )
    {
        bool sync;

        if ( syncmode == INITIAL_SYNC )
        {
            //sync =  ((headerstring & 0xFFF00000) == 0xFFF00000);
            sync = ( headerstring & 0xFFE00000 ) == 0xFFE00000; // SZD: MPEG 2.5
        }
        else
        {
            //sync = ((headerstring & 0xFFF80C00) == word) 
            sync = ( ( headerstring & 0xFFE00000 ) == 0xFFE00000 ) // -- THIS IS PROBABLY WRONG. A WEAKER CHECK.
                && ( ( headerstring & 0x000000C0 ) == 0x000000C0 == _singleChMode );
        }

        // filter out invalid sample rate
        if ( sync )
        {
            sync = ( SupportClass.URShift( headerstring, 10 ) & 3 ) != 3;

            // if (!sync) Trace.WriteLine("INVALID SAMPLE RATE DETECTED", "Bitstream");
        }

        // filter out invalid layer
        if ( sync )
        {
            sync = ( SupportClass.URShift( headerstring, 17 ) & 3 ) != 0;

            // if (!sync) Trace.WriteLine("INVALID LAYER DETECTED", "Bitstream");
        }

        // filter out invalid version
        if ( sync )
        {
            sync = ( SupportClass.URShift( headerstring, 19 ) & 3 ) != 1;

            if ( !sync )
            {
                Console.WriteLine( @"INVALID VERSION DETECTED" );
            }
        }

        return sync;
    }

    /// <summary>
    /// Reads the data for the next frame. The frame is not parsed
    /// until parse frame is called.
    /// </summary>
    public void Read_frame_data( int bytesize )
    {
        ReadFully( _frameBytes, 0, bytesize );
        _frameSize   = bytesize;
        _wordPointer = -1;
        _bitIndex    = -1;
    }

    /// <summary>
    /// Parses the data previously read with read_frame_data().
    /// </summary>
    public void ParseFrame()
    {
        // Convert Bytes read to int
        var b        = 0;
        var byteread = _frameBytes;
        var bytesize = _frameSize;

        for ( var k = 0; k < bytesize; k += 4 )
        {
            var   b0 = byteread[ k ];
            sbyte b1 = 0;
            sbyte b2 = 0;
            sbyte b3 = 0;

            if ( ( k + 1 ) < bytesize )
            {
                b1 = byteread[ k + 1 ];
            }

            if ( ( k + 2 ) < bytesize )
            {
                b2 = byteread[ k + 2 ];
            }

            if ( ( k + 3 ) < bytesize )
            {
                b3 = byteread[ k + 3 ];
            }

            _frameBuffer[ b++ ] = ( ( b0 << 24 ) & ( int ) SupportClass.Identity( 0xFF000000 ) )
                                | ( ( b1 << 16 ) & 0x00FF0000 )
                                | ( ( b2 << 8 ) & 0x0000FF00 )
                                | ( b3 & 0x000000FF );
        }

        _wordPointer = 0;
        _bitIndex    = 0;
    }

    /// <summary>
    /// Read bits from buffer into the lower bits of an unsigned int.
    /// The LSB contains the latest read bit of the stream.
    /// (between 1 and 16, inclusive).
    /// </summary>
    public int GetBitsFromBuffer( int countBits )
    {
        int returnvalue;
        var sum = _bitIndex + countBits;

        if ( _wordPointer < 0 )
        {
            _wordPointer = 0;
        }

        if ( sum <= 32 )
        {
            // all bits contained in *wordpointer
            returnvalue = SupportClass.URShift( _frameBuffer[ _wordPointer ], 32 - sum ) & _bitmask[ countBits ];

            if ( ( _bitIndex += countBits ) == 32 )
            {
                _bitIndex = 0;
                _wordPointer++;
            }

            return returnvalue;
        }

        var right = _frameBuffer[ _wordPointer ] & 0x0000FFFF;

        _wordPointer++;

        var left = _frameBuffer[ _wordPointer ] & ( int ) SupportClass.Identity( 0xFFFF0000 );

        returnvalue = ( ( right << 16 ) & ( int ) SupportClass.Identity( 0xFFFF0000 ) )
                    | ( SupportClass.URShift( left, 16 ) & 0x0000FFFF );

        returnvalue =  SupportClass.URShift( returnvalue, 48 - sum );
        returnvalue &= _bitmask[ countBits ];

        _bitIndex = sum - 32;

        return returnvalue;
    }

    /// <summary>
    /// Set the word we want to sync the header to.
    /// In Big-Endian byte order
    /// </summary>
    public void SetSyncWord( int syncword0 )
    {
        _syncWord     = syncword0 & unchecked( ( int ) 0xFFFFFF3F );
        _singleChMode = ( syncword0 & 0x000000C0 ) == 0x000000C0;
    }

    /// <summary>
    /// Reads the exact number of bytes from the source input stream into a byte array.
    /// </summary>
    private void ReadFully( sbyte[] b, int offs, int len )
    {
        try
        {
            while ( len > 0 )
            {
                var bytesread = _sourceStream.Read( b, offs, len );

                if ( bytesread is -1 or 0 ) // t/DD -- .NET returns 0 at end-of-stream!
                {
                    // t/DD: this really SHOULD throw an exception here...
                    // Trace.WriteLine("readFully -- returning success at EOF? (" + bytesread + ")", "Bitstream");
                    while ( len-- > 0 )
                    {
                        b[ offs++ ] = 0;
                    }

                    break;

                    //throw newBitstreamException(UNEXPECTED_EOF, new EOFException());
                }

                offs += bytesread;
                len  -= bytesread;
            }
        }
        catch ( IOException ex )
        {
            throw new BitstreamException( BitstreamErrors.STREA_ERROR, ex );
        }
    }

    /// <summary>
    /// Simlar to readFully, but doesn't throw exception when EOF is reached.
    /// </summary>
    private int ReadBytes( sbyte[] b, int offs, int len )
    {
        var totalBytesRead = 0;

        try
        {
            while ( len > 0 )
            {
                var bytesread = _sourceStream.Read( b, offs, len );

                // for (int i = 0; i < len; i++) b[i] = (sbyte)Temp[i];
                if ( bytesread is -1 or 0 )
                {
                    break;
                }

                totalBytesRead += bytesread;
                offs           += bytesread;
                len            -= bytesread;
            }
        }
        catch ( IOException ex )
        {
            throw new BitstreamException( BitstreamErrors.STREA_ERROR, ex );
        }

        return totalBytesRead;
    }
}
