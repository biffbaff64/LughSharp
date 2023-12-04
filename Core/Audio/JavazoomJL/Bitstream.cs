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

using System.Text;

using LibGDXSharp.Core.Files;

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// The <tt>Bistream</tt> class is responsible for parsing an MPEG audio bitstream.
/// <para>
/// <b>REVIEW:</b> much of the parsing currently occurs in the various decoders.
/// This should be moved into this class and associated inner classes.
/// </para>
/// </summary>
[PublicAPI]
public class Bitstream
{

    #region constants

    // Synchronization control constant for the initial synchronization to the start of a frame.
    public const byte INITIAL_SYNC = 0;

    // Synchronization control constant for non-initial frame synchronizations.
    public const byte STRICT_SYNC = 1;

    // max. 1730 bytes per frame: 144 * 384kbit/s / 32000 Hz + 2 Bytes CRC
    // Maximum size of the frame buffer.
    public const int BUFFER_INT_SIZE = 433;

    // The first bitstream error code. See the DecoderErrors interface for other bitstream error codes.
    public const int BITSTREAM_ERROR = 0x100;

    // An undeterminable error occurred.
    public const int UNKNOWN_ERROR = BITSTREAM_ERROR + 0;

    // The header describes an unknown sample rate.
    public const int UNKNOWN_SAMPLE_RATE = BITSTREAM_ERROR + 1;

    // A problem occurred reading from the stream.
    public const int STREAM_ERROR = BITSTREAM_ERROR + 2;

    // The end of the stream was reached prematurely.
    public const int UNEXPECTED_EOF = BITSTREAM_ERROR + 3;

    // The end of the stream was reached.
    public const int STREAM_EOF = BITSTREAM_ERROR + 4;

    // Frame data are missing.
    public const int INVALIDFRAME = BITSTREAM_ERROR + 5;

    public const int BITSTREAM_LAST = 0x1ff;

    #endregion constants

    #region properties

    public int   HeaderPos       { get; set; } = 0;
    public float ReplayGainScale { get; set; }

    #endregion properties

    #region private readonly

    private readonly int[] _bitmask =
    {
        0, // dummy
        0x00000001, 0x00000003, 0x00000007, 0x0000000F, 0x0000001F, 0x0000003F, 0x0000007F, 0x000000FF,
        0x000001FF, 0x000003FF, 0x000007FF, 0x00000FFF, 0x00001FFF, 0x00003FFF, 0x00007FFF, 0x0000FFFF,
        0x0001FFFF
    };

    private readonly PushbackStream? _source;

    #endregion private readonly

    private int     _wordpointer;                                    // Index into framebuffer where the next bits are retrieved.
    private int     _bitindex;                                       // Number (0-31, from MSB to LSB) of next bit for get_bits()
    private int     _framesize;                                      // Number of valid bytes in the frame buffer.
    private byte[]  _frameBytes   = new byte[ BUFFER_INT_SIZE * 4 ]; // The bytes read from the stream.
    private int[]   _framebuffer  = new int[ BUFFER_INT_SIZE ];      // The frame buffer that holds the data for the current frame.
    private Header  _header       = new();
    private byte[]  _syncbuf      = new byte[ 4 ];
    private Crc16[] _crc          = new Crc16[ 1 ];
    private byte[]? _rawid3V2     = null;
    private bool    _firstframe   = false;
    private int     _syncword     = 0;
    private bool    _singleChMode = false;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Construct a IBitstream that reads data from a given InputStream.  
    /// </summary>
    /// <param name="inStream"> The InputStream to read from. </param>
    public Bitstream( PushbackStream inStream )
    {
        ArgumentNullException.ThrowIfNull( inStream );

        _crc         = new Crc16[ 1 ];
        _syncbuf     = new byte[ 4 ];
        _frameBytes  = new byte[ BUFFER_INT_SIZE * 4 ];
        _framebuffer = new int[ BUFFER_INT_SIZE ];
        _header      = new Header();

        _firstframe = true;

        _source = new PushbackStream( inStream, BUFFER_INT_SIZE * 4 );

        CloseFrame();
    }

    private unsafe string ParseText( byte[] bframes, int offset, int size, int skip )
    {
        ArgumentNullException.ThrowIfNull( bframes );

        var value = string.Empty;

        try
        {
            string[] encodingTypes =
            {
                "ISO-8859-1",
                "UTF16",
                "UTF-16BE",
                "UTF-8"
            };

            var encodingType = encodingTypes[ bframes[ offset ] ];

            Encoding encoding = encodingType switch
                                {
                                    "ISO-8859-1" => Encoding.GetEncoding( "ISO-8859-1" ),
                                    "UTF16"      => Encoding.Unicode,
                                    "UTF-16BE"   => Encoding.BigEndianUnicode,
                                    "UTF-8"      => Encoding.UTF8,
                                    _            => throw new ArgumentException( @"Invalid encoding type", nameof( encodingType ) )
                                };

            fixed ( byte* ptr = &bframes[ 0 ] )
            {
                var sptr = ( sbyte* )ptr;

                value = new string( sptr, offset + skip, size - skip, encoding );
            }
        }
        catch ( UnsupportedEncodingException )
        {
            // ignored
        }

        return value;
    }

    /// <summary>
    /// Close the Bitstream.
    /// </summary>
    public void Close()
    {
        try
        {
            _source?.Close();
        }
        catch ( IOException )
        {
            throw new BitstreamException( STREAM_ERROR );
        }
    }

    /// <summary>
    /// Reads and parses the next frame from the input source.
    /// </summary>
    /// <returns>
    /// the Header describing details of the frame read, or null if the
    /// end of the stream has been reached.
    /// </returns>
    public Header? ReadFrame()
    {
        Header? result = null;

        try
        {
            result = ReadNextFrame();

            // E.B, Parse VBR (if any) first frame.
            if ( _firstframe )
            {
                result.ParseVbr( _frameBytes );
                _firstframe = false;
            }
        }
        catch ( BitstreamException ex )
        {
            if ( ex.Errorcode == INVALIDFRAME )
            {
                try
                {
                    CloseFrame();
                    result = ReadNextFrame();
                }
                catch ( BitstreamException e )
                {
                    // wrap original exception so stack trace is maintained.
                    if ( e.Errorcode != STREAM_EOF )
                    {
                        throw new BitstreamException( e.Errorcode, e );
                    }
                }
            }
            else if ( ex.Errorcode != STREAM_EOF )
            {
                // wrap original exception so stack trace is maintained.
                throw new BitstreamException( ex.Errorcode, ex );
            }
        }

        return result;
    }

    /// <summary>
    /// Read next MP3 frame.
    /// </summary>
    /// <returns> MP3 frame header. </returns>
    private Header ReadNextFrame()
    {
        if ( _framesize == -1 )
        {
            NextFrame();
        }

        return _header;
    }

    /// <summary>
    /// Read next MP3 frame.
    /// </summary>
    private void NextFrame()
    {
        // entire frame is read by the header class.
        _header.ReadHeader( this, _crc );
    }

    /// <summary>
    /// Unreads the bytes read from the frame.
    /// </summary>
    /// <exception cref="BitstreamException"></exception>

    // REVIEW: add new error codes for this.
    public void UnreadFrame()
    {
        if ( ( _wordpointer == -1 ) && ( _bitindex == -1 ) && ( _framesize > 0 ) )
        {
            try
            {
                _source?.Unread( _frameBytes, 0, _framesize );
            }
            catch ( IOException )
            {
                throw new BitstreamException( STREAM_ERROR );
            }
        }
    }

    /// <summary>
    /// Close MP3 frame.
    /// </summary>
    public void CloseFrame()
    {
        _framesize   = -1;
        _wordpointer = -1;
        _bitindex    = -1;
    }

    /// <summary>
    /// Determines if the next 4 bytes of the stream represent a frame header.
    /// </summary>
    /// <param name="syncmode"></param>
    /// <returns></returns>
    public bool IsSyncCurrentPosition( int syncmode )
    {
        var read = ReadBytes( _syncbuf, 0, 4 );

        var headerstring = ( int )( ( _syncbuf[ 0 ] << 24 ) & 0xFF000000 )
                         | ( ( _syncbuf[ 1 ] << 16 ) & 0x00FF0000 )
                         | ( ( _syncbuf[ 2 ] << 8 ) & 0x0000FF00 )
                         | ( ( _syncbuf[ 3 ] << 0 ) & 0x000000FF );

        try
        {
            _source?.Unread( _syncbuf, 0, read );
        }
        catch ( IOException )
        {
            // ignored
        }

        var sync = false;

        switch ( read )
        {
            case 0:
                sync = true;

                break;

            case 4:
                sync = IsSyncMark( headerstring, syncmode, _syncword );

                break;
        }

        return sync;
    }

    // REVIEW: this class should provide inner classes to
    // parse the frame contents. Eventually, readBits will
    // be removed.
    public int ReadBits( int n )
    {
        return GetBits( n );
    }

    public int ReadCheckedBits( int n )
    {
        // REVIEW: implement CRC check.
        return GetBits( n );
    }

    /// <summary>
    /// Get next 32 bits from bitstream, which are stored in the headerstring.
    /// <paramref name="syncmode"/> allows Synchro flag ID. The returned value is
    /// False at the end of stream.
    /// </summary>
    public int SyncHeader( byte syncmode )
    {
        bool sync;

        // read additional 2 bytes
        var bytesRead = ReadBytes( _syncbuf, 0, 3 );

        if ( bytesRead != 3 )
        {
            throw new BitstreamException( STREAM_EOF );
        }

        var headerstring = ( ( _syncbuf[ 0 ] << 16 ) & 0x00FF0000 )
                         | ( ( _syncbuf[ 1 ] << 8 ) & 0x0000FF00 )
                         | ( ( _syncbuf[ 2 ] << 0 ) & 0x000000FF );

        do
        {
            headerstring <<= 8;

            if ( ReadBytes( _syncbuf, 3, 1 ) != 1 )
            {
                throw new BitstreamException( STREAM_EOF );
            }

            headerstring |= _syncbuf[ 3 ] & 0x000000FF;

            sync = IsSyncMark( headerstring, syncmode, _syncword );
        }
        while ( !sync );

        return headerstring;
    }

    public bool IsSyncMark( int headerstring, int syncmode, int word )
    {
        bool sync;

        if ( syncmode == INITIAL_SYNC )
        {
            sync = ( headerstring & 0xFFE00000 ) == 0xFFE00000; // SZD: MPEG 2.5
        }
        else
        {
            sync = ( ( headerstring & 0xFFF80C00 ) == word )
                && ( ( headerstring & 0x000000C0 ) == 0x000000C0 == _singleChMode );
        }

        // filter out invalid sample rate
        if ( sync )
        {
            sync = ( ( headerstring >>> 10 ) & 3 ) != 3;
        }

        // filter out invalid layer
        if ( sync )
        {
            sync = ( ( headerstring >>> 17 ) & 3 ) != 0;
        }

        // filter out invalid version
        if ( sync )
        {
            sync = ( ( headerstring >>> 19 ) & 3 ) != 1;
        }

        return sync;
    }

    /// <summary>
    /// Reads the data for the next frame. The frame is not parsed
    /// until parse frame is called.
    /// </summary>
    /// <param name="bytesize"></param>
    /// <returns></returns>
    public int ReadFrameData( int bytesize )
    {
        var numread = ReadFully( _frameBytes, 0, bytesize );

        _framesize   = bytesize;
        _wordpointer = -1;
        _bitindex    = -1;

        return numread;
    }

    /// <summary>
    /// Parses the data previously read with ReadFrameData().
    /// </summary>
    public void ParseFrame()
    {
        // Convert Bytes read to int
        var b        = 0;
        var byteread = _frameBytes;
        var bytesize = _framesize;

        for ( var k = 0; k < bytesize; k = k + 4 )
        {
            var b0 = byteread[ k ];

            byte b1 = 0;
            byte b2 = 0;
            byte b3 = 0;

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

            _framebuffer[ b++ ] = ( int )( ( b0 << 24 ) & 0xFF000000 )
                                | ( ( b1 << 16 ) & 0x00FF0000 )
                                | ( ( b2 << 8 ) & 0x0000FF00 )
                                | ( b3 & 0x000000FF );
        }

        _wordpointer = 0;
        _bitindex    = 0;
    }

    /// <summary>
    /// Read bits from buffer into the lower bits of an unsigned int. The
    /// LSB contains the latest read bit of the stream. (1 &lt;= number_of_bits &lt;= 16)
    /// </summary>
    /// <param name="numberOfBits"></param>
    /// <returns></returns>
    public int GetBits( int numberOfBits )
    {
        int returnvalue;
        var sum = _bitindex + numberOfBits;

        if ( _wordpointer < 0 )
        {
            _wordpointer = 0;
        }

        if ( sum <= 32 )
        {
            // all bits contained in *wordpointer
            returnvalue = ( _framebuffer[ _wordpointer ] >>> ( 32 - sum ) ) & _bitmask[ numberOfBits ];

            if ( ( _bitindex += numberOfBits ) == 32 )
            {
                _bitindex = 0;
                _wordpointer++;
            }

            return returnvalue;
        }

        var right = ( _framebuffer[ _wordpointer ] & 0x0000FFFF );

        _wordpointer++;

        var left = ( int )( _framebuffer[ _wordpointer ] & 0xFFFF0000 );

        returnvalue = ( int )( ( right << 16 ) & 0xFFFF0000 ) | ( ( left >>> 16 ) & 0x0000FFFF );
        returnvalue >>>= 48 - sum;
        returnvalue &= _bitmask[ numberOfBits ];

        _bitindex = sum - 32;

        return returnvalue;
    }

    /// <summary>
    /// Set the word we want to sync the header to. In Big-Endian byte order
    /// </summary>
    /// <param name="syncword"></param>
    public void SetSyncword( Int32 syncword )
    {
        _syncword     = ( Int32 )( syncword & 0xFFFFFF3F );
        _singleChMode = ( syncword & 0x000000C0 ) == 0x000000C0;
    }

    /// <summary>
    /// Reads the exact number of bytes from the source input stream into a byte array.
    /// </summary>
    /// <param name="b"> The byte array to read the specified number of bytes into. </param>
    /// <param name="offs"> The index in the array where the first byte read should be stored. </param>
    /// <param name="len"> the number of bytes to read. </param>
    /// <exception cref="BitstreamException">
    /// is thrown if the specified number of bytes could not be read from the stream.
    /// </exception>
    private int ReadFully( byte[] b, int offs, int len )
    {
        var nRead = 0;

        try
        {
            while ( len > 0 )
            {
                var bytesread = _source?.Read( b, offs, len );

                if ( bytesread == -1 )
                {
                    while ( len-- > 0 )
                    {
                        b[ offs++ ] = 0;
                    }

                    break;
                }

                nRead =  ( int )( nRead + bytesread )!;
                offs  += ( int )bytesread!;
                len   -= ( int )bytesread;
            }
        }
        catch ( IOException ex )
        {
            throw new BitstreamException( STREAM_ERROR, ex );
        }

        return nRead;
    }

    /// <summary>
    /// Simlar to ReadFully, but doesn't throw exception when EOF is reached.
    /// </summary>
    /// <param name="b"></param>
    /// <param name="offs"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    /// <exception cref="BitstreamException"></exception>
    private int ReadBytes( byte[] b, int offs, int len )
    {
        var totalBytesRead = 0;

        try
        {
            while ( len > 0 )
            {
                var bytesread = _source?.Read( b, offs, len );

                if ( bytesread == -1 )
                {
                    break;
                }

                totalBytesRead += ( int )bytesread!;
                offs           += ( int )bytesread;
                len            -= ( int )bytesread;
            }
        }
        catch ( IOException )
        {
            throw new BitstreamException( STREAM_ERROR );
        }

        return totalBytesRead;
    }
    
    // ------------------------------------------------------------------------
    
    /// <summary>
    /// Load ID3v2 frames.
    /// </summary>
    /// <param name="inStream"> MP3 InputStream. </param>
    private void LoadID3V2( PushbackStream inStream )
    {
        var size = -1;

        try
        {
            // Read ID3v2 header (10 bytes).
            inStream.Mark( 10 );

            size      = ReadID3V2Header( inStream );
            HeaderPos = size;
        }
        catch ( IOException )
        {
            // ignored
        }
        finally
        {
            try
            {
                // Unread ID3v2 header (10 bytes).
                inStream.Reset();
            }
            catch ( IOException )
            {
                // ignored
            }
        }

        // Load ID3v2 tags.
        try
        {
            if ( size > 0 )
            {
                _rawid3V2 = new byte[ size ];

                if ( inStream.Read( _rawid3V2, 0, _rawid3V2.Length ) > 0 )
                {
                    ParseID3V2Frames( _rawid3V2 );
                }
            }
        }
        catch ( IOException )
        {
            // ignored
        }
    }

    /// <summary>
    /// Parse ID3v2 tag header to find out size of ID3v2 frames.
    /// </summary>
    /// <param name="inStream"> MP3 InputStream </param>
    /// <returns> size of ID3v2 frames + header </returns>
    [SuppressMessage( "ReSharper", "MustUseReturnValue" )]
    private int ReadID3V2Header( PushbackStream inStream )
    {
        var id3Header = new byte[ 4 ];
        var size      = -10;

        inStream.Read( id3Header, 0, 3 );

        // Look for ID3v2
        if ( ( id3Header[ 0 ] == 'I' ) && ( id3Header[ 1 ] == 'D' ) && ( id3Header[ 2 ] == '3' ) )
        {
            inStream.Read( id3Header, 0, 3 );
            inStream.Read( id3Header, 0, 4 );

            size = ( id3Header[ 0 ] << 21 )
                 + ( id3Header[ 1 ] << 14 )
                 + ( id3Header[ 2 ] << 7 )
                 + id3Header[ 3 ];
        }

        return size + 10;
    }

    /// <summary>
    /// Return raw ID3v2 frames + header.
    /// </summary>
    /// <returns>
    /// ID3v2 InputStream or null if ID3v2 frames are not available.
    /// </returns>
    public InputStream? GetRawID3V2()
    {
        if ( _rawid3V2 == null )
        {
            return null;
        }
        else
        {
            return new ByteArrayInputStream( _rawid3V2 );
        }
    }

    private void ParseID3V2Frames( byte[]? bframes )
    {
        if ( bframes == null )
        {
            return;
        }

        if ( !"ID3".Equals( bframes.ToString()?.Substring( 0, 3 ) ) )
        {
            return;
        }

        var v2Version = ( bframes[ 3 ] & 0xFF );

        if ( ( v2Version < 2 ) || ( v2Version > 4 ) )
        {
            return;
        }

        try
        {
            float? replayGain     = 0f;
            float? replayGainPeak = 0f;
            int    size;

            for ( var i = 10; ( i < bframes.Length ) && ( bframes[ i ] > 0 ); i += size )
            {
                string? value;

                if ( ( v2Version == 3 ) || ( v2Version == 4 ) )
                {
                    // ID3v2.3 & ID3v2.4
                    var code = new string( bframes.ToString()?.Substring( i, 4 ) );

                    size = ( Int32 )( ( bframes[ i + 4 ] << 24 ) & 0xFF000000 )
                         | ( ( bframes[ i + 5 ] << 16 ) & 0x00FF0000 )
                         | ( ( bframes[ i + 6 ] << 8 ) & 0x0000FF00 )
                         | ( bframes[ i + 7 ] & 0x000000FF );

                    i += 10;

                    if ( code.Equals( "TXXX" ) )
                    {
                        value = ParseText( bframes, i, size, 1 );

                        var values = value.Split( "\0" );

                        if ( values.Length == 2 )
                        {
                            var name = values[ 0 ];

                            value = values[ 1 ];

                            if ( name.Equals( "replaygain_track_peak" ) )
                            {
                                replayGainPeak = float.Parse( value );

                                if ( replayGain != null )
                                {
                                    break;
                                }
                            }
                            else if ( name.Equals( "replaygain_track_gain" ) )
                            {
                                replayGain = float.Parse( value.Replace( " dB", "" ) ) + 3;

                                if ( replayGainPeak != null )
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // ID3v2.2
                    var scode = new string( bframes.ToString()?.Substring( i, 3 ) );

                    size = ( bframes[ i + 3 ] << 16 ) + ( bframes[ i + 4 ] << 8 ) + bframes[ i + 5 ];

                    i += 6;

                    if ( scode.Equals( "TXXX" ) )
                    {
                        value = ParseText( bframes, i, size, 1 );

                        var values = value.Split( "\0" );

                        if ( values.Length == 2 )
                        {
                            var name = values[ 0 ];
                            value = values[ 1 ];

                            if ( name.Equals( "replaygain_track_peak" ) )
                            {
                                replayGainPeak = float.Parse( value );

                                if ( replayGain != null )
                                {
                                    break;
                                }
                            }
                            else if ( name.Equals( "replaygain_track_gain" ) )
                            {
                                replayGain = float.Parse( value.Replace( " dB", "" ) ) + 3;

                                if ( replayGainPeak != null )
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if ( ( replayGain != null ) && ( replayGainPeak != null ) )
            {
                ReplayGainScale = ( float )Math.Pow( 10, ( double )( replayGain / 20f ) );

                // If scale * peak > 1 then reduce scale (preamp) to prevent clipping.
                ReplayGainScale = Math.Min( ( float )( 1 / replayGainPeak ), ReplayGainScale );
            }
        }
        catch ( GdxRuntimeException )
        {
            // ignored
        }
    }
}
