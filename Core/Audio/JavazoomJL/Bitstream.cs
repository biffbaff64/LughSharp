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

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// The <code>Bistream</code> class is responsible for parsing an MPEG audio bitstream.
/// <para>
/// <b>REVIEW:</b> much of the parsing currently occurs in the various decoders.
/// This should be moved into this class and associated inner classes.
/// </para>
/// </summary>
[PublicAPI]
public class Bitstream
{
    // Synchronization control constant for the initial synchronization to the start of a frame.
    public const byte INITIAL_SYNC = 0;

    // Synchronization control constant for non-initial frame synchronizations.
    public const byte STRICT_SYNC = 1;

    // max. 1730 bytes per frame: 144 * 384kbit/s / 32000 Hz + 2 Bytes CRC
    // Maximum size of the frame buffer.
    public const int BUFFER_INT_SIZE = 433;

    // The first bitstream error code. See the {@link DecoderErrors DecoderErrors} interface for other bitstream error codes.
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

    public int   HeaderPos       { get; set; } = 0;
    public float ReplayGainScale { get; set; }

    // The frame buffer that holds the data for the current frame.
    private int[] _framebuffer = new int[ BUFFER_INT_SIZE ];

    // Number of valid bytes in the frame buffer.
    private int _framesize;

    // The bytes read from the stream.
    private byte[] _frameBytes = new byte[ BUFFER_INT_SIZE * 4 ];

    // Index into framebuffer where the next bits are retrieved.
    private int _wordpointer;

    // Number (0-31, from MSB to LSB) of next bit for get_bits()
    private int _bitindex;

    private int[] _bitmask =
    {
        0, // dummy
        0x00000001, 0x00000003, 0x00000007, 0x0000000F, 0x0000001F,
        0x0000003F, 0x0000007F, 0x000000FF, 0x000001FF, 0x000003FF,
        0x000007FF, 0x00000FFF, 0x00001FFF, 0x00003FFF, 0x00007FFF,
        0x0000FFFF, 0x0001FFFF
    };

    private PushbackInputStream? _source;

    private Header  _header   = new();
    private byte[]  _syncbuf  = new byte[ 4 ];
    private Crc16[] _crc      = new Crc16[ 1 ];
    private byte[]? _rawid3V2 = null;
    private bool    _firstframe;
    private int     _syncword;
    private bool    _singleChMode;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Construct a IBitstream that reads data from a given InputStream.  
    /// </summary>
    /// <param name="inStream"> The InputStream to read from. </param>
    public Bitstream( Stream inStream )
    {
        ArgumentNullException.ThrowIfNull( inStream );

        inStream = new BufferedStream( inStream );

        LoadID3V2( inStream );

        _firstframe = true;

        _source = new PushbackInputStream( inStream, BUFFER_INT_SIZE * 4 );

        CloseFrame();
    }


    /**
     * Load ID3v2 frames.
     * @param in MP3 InputStream.
     * @author JavaZOOM
     */
    private void LoadID3V2( Stream inStream )
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
        }
        finally
        {
            try
            {
                // Unread ID3v2 header (10 bytes).
                inStream.Reset();
            }
            catch ( IOException e )
            {
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
        catch ( IOException e )
        {
        }
    }

    /**
     * Parse ID3v2 tag header to find out size of ID3v2 frames.
     * @param in MP3 InputStream
     * @return size of ID3v2 frames + header
     * @throws IOException
     * @author JavaZOOM
     */
    [SuppressMessage( "ReSharper", "MustUseReturnValue" )]
    private int ReadID3V2Header( Stream inStream )
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
    public Stream? GetRawID3V2()
    {
        if ( _rawid3V2 == null )
        {
            return null;
        }
        else
        {
            ByteArrayInputStream bain = new ByteArrayInputStream( _rawid3V2 );

            return bain;
        }
    }

    private void ParseID3V2Frames( byte[] bframes )
    {
        if ( bframes == null )
        {
            return;
        }

        if ( !"ID3".Equals( new string( bframes, 0, 3 ) ) )
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
            float replayGain;
            float replayGainPeak;
            int   size;

            for ( var i = 10; ( i < bframes.Length ) && ( bframes[ i ] > 0 ); i += size )
            {
                string? value;

                if ( ( v2Version == 3 ) || ( v2Version == 4 ) )
                {
                    // ID3v2.3 & ID3v2.4
                    var code = new string( bframes, i, 4 );

                    size = ( Int32 )( ( ( bframes[ i + 4 ] << 24 ) & 0xFF000000 )
                                    | ( ( bframes[ i + 5 ] << 16 ) & 0x00FF0000 )
                                    | ( ( bframes[ i + 6 ] << 8 ) & 0x0000FF00 )
                                    | ( bframes[ i + 7 ] & 0x000000FF ) );

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
                    var scode = new string( bframes, i, 3 );

                    size = 0x00000000 + ( bframes[ i + 3 ] << 16 ) + ( bframes[ i + 4 ] << 8 ) + bframes[ i + 5 ];

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
                ReplayGainScale = ( float )Math.Pow( 10, replayGain / 20f );

                // If scale * peak > 1 then reduce scale (preamp) to prevent clipping.
                ReplayGainScale = Math.Min( 1 / replayGainPeak, ReplayGainScale );
            }
        }
        catch ( GdxRuntimeException ignored )
        {
        }
    }

    private string ParseText( byte[] bframes, int offset, int size, int skip )
    {
        var value = string.Empty;

        try
        {
            string[] encTypes =
            {
                "ISO-8859-1",
                "UTF16",
                "UTF-16BE",
                "UTF-8"
            };

            value = new string( bframes, offset + skip, size - skip, encTypes[ bframes[ offset ] ] );
        }
        catch ( UnsupportedEncodingException e )
        {
        }

        return value;
    }

    /**
     * Close the Bitstream.
     * @throws BitstreamException
     */
    public void close()
    {
        try
        {
            source.close();
        }
        catch
            ( IOException ex )
        {
            throw newBitstreamException( STREAM_ERROR, ex );
        }

    }

    /**
     * Reads and parses the next frame from the input source.
     * @return the Header describing details of the frame read, or null if the end of the stream has been reached.
     */
    public Header readFrame()
    {
        Header result = null;

        try
        {
            result = readNextFrame();

            // E.B, Parse VBR (if any) first frame.
            if ( firstframe == true )
            {
                result.parseVBR( frame_bytes );
                firstframe = false;
            }
        }
        catch ( BitstreamException ex )
        {
            if ( ex.getErrorCode() == INVALIDFRAME )

                // Try to skip this frame.
                // System.out.println("INVALIDFRAME");
            {
                try
                {
                    CloseFrame();
                    result = readNextFrame();
                }
                catch ( BitstreamException e )
                {
                    if ( e.getErrorCode() != STREAM_EOF ) // wrap original exception so stack trace is maintained.
                    {
                        throw newBitstreamException( e.getErrorCode(), e );
                    }
                }
            }
            else if ( ex.getErrorCode() != STREAM_EOF ) // wrap original exception so stack trace is maintained.
            {
                throw newBitstreamException( ex.getErrorCode(), ex );
            }
        }

        return result;
    }

    /**
     * Read next MP3 frame.
     * @return MP3 frame header.
     * @throws BitstreamException
     */
    private Header readNextFrame()
    {
        if ( framesize == -1 )
        {
            nextFrame();
        }

        return header;
    }

    /**
     * Read next MP3 frame.
     * @throws BitstreamException
     */
    private void nextFrame()
    {
        // entire frame is read by the header class.
        header.read_header( this, crc );
    }

    /**
     * Unreads the bytes read from the frame.
     * @throws BitstreamException
     */

// REVIEW: add new error codes for this.
    public void unreadFrame()
    {
        if ( wordpointer == -1 && bitindex == -1 && framesize > 0 )
        {
            try
            {
                source.unread( frame_bytes, 0, framesize );
            }
            catch ( IOException ex )
            {
                throw newBitstreamException( STREAM_ERROR );
            }
        }
    }

    /**
     * Close MP3 frame.
     */
    public void CloseFrame()
    {
        _framesize   = -1;
        _wordpointer = -1;
        _bitindex    = -1;
    }

    /**
     * Determines if the next 4 bytes of the stream represent a frame header.
     */
    public bool IsSyncCurrentPosition( int syncmode )
    {
        int read = readBytes( _syncbuf, 0, 4 );

        int headerstring = _syncbuf[ 0 ] << 24 & 0xFF000000
                         | _syncbuf[ 1 ] << 16 & 0x00FF0000
                         | _syncbuf[ 2 ] << 8 & 0x0000FF00
                         | _syncbuf[ 3 ] << 0 & 0x000000FF;

        try
        {
            _source.Unread( _syncbuf, 0, read );
        }
        catch ( IOException ex )
        {
        }

        bool sync = false;

        switch ( read )
        {
            case 0:
                sync = true;

                break;

            case 4:
                sync = isSyncMark( headerstring, syncmode, _syncword );

                break;
        }

        return sync;
    }

// REVIEW: this class should provide inner classes to
// parse the frame contents. Eventually, readBits will
// be removed.
    public int readBits( int n )
    {
        return get_bits( n );
    }

    public int readCheckedBits( int n )
    {
        // REVIEW: implement CRC check.
        return get_bits( n );
    }

    protected BitstreamException newBitstreamException( int errorcode )
    {
        return new BitstreamException( errorcode, null );
    }

    protected BitstreamException newBitstreamException( int errorcode, Throwable throwable )
    {
        return new BitstreamException( errorcode, throwable );
    }

    /**
     * Get next 32 bits from bitstream. They are stored in the headerstring. syncmod allows Synchro flag ID The returned value is
     * False at the end of stream.
     */
    int syncHeader( byte syncmode )
    {
        bool sync;
        int  headerstring;

        // read additional 2 bytes
        int bytesRead = readBytes( syncbuf, 0, 3 );

        if ( bytesRead != 3 )
        {
            throw newBitstreamException( STREAM_EOF, null );
        }

        headerstring = syncbuf[ 0 ] << 16 & 0x00FF0000 | syncbuf[ 1 ] << 8 & 0x0000FF00 | syncbuf[ 2 ] << 0 & 0x000000FF;

        do
        {
            headerstring <<= 8;

            if ( readBytes( syncbuf, 3, 1 ) != 1 )
            {
                throw newBitstreamException( STREAM_EOF, null );
            }

            headerstring |= syncbuf[ 3 ] & 0x000000FF;

            sync = isSyncMark( headerstring, syncmode, syncword );
        }
        while ( !sync );

        // current_frame_number++;
        // if (last_frame_number < current_frame_number) last_frame_number = current_frame_number;

        return headerstring;
    }

    public bool isSyncMark( int headerstring, int syncmode, int word )
    {
        bool sync = false;

        if ( syncmode == INITIAL_SYNC ) // sync = ((headerstring & 0xFFF00000) == 0xFFF00000);
        {
            sync = ( headerstring & 0xFFE00000 ) == 0xFFE00000; // SZD: MPEG 2.5
        }
        else
        {
            sync = ( headerstring & 0xFFF80C00 ) == word && ( headerstring & 0x000000C0 ) == 0x000000C0 == single_ch_mode;
        }

        // filter out invalid sample rate
        if ( sync )
        {
            sync = ( headerstring >>> 10 & 3 ) != 3;
        }

        // filter out invalid layer
        if ( sync )
        {
            sync = ( headerstring >>> 17 & 3 ) != 0;
        }

        // filter out invalid version
        if ( sync )
        {
            sync = ( headerstring >>> 19 & 3 ) != 1;
        }

        return sync;
    }

    /**
     * Reads the data for the next frame. The frame is not parsed until parse frame is called.
     */
    int read_frame_data( int bytesize )
    {
        int numread = 0;
        numread     = readFully( frame_bytes, 0, bytesize );
        framesize   = bytesize;
        wordpointer = -1;
        bitindex    = -1;

        return numread;
    }

    /**
     * Parses the data previously read with read_frame_data().
     */
    void parse_frame()
    {
        // Convert Bytes read to int
        int    b        = 0;
        byte[] byteread = frame_bytes;
        int    bytesize = framesize;

        // Check ID3v1 TAG (True only if last frame).
        // for (int t=0;t<(byteread.Length)-2;t++)
        // {
        // if ((byteread[t]=='T') && (byteread[t+1]=='A') && (byteread[t+2]=='G'))
        // {
        // System.out.println("ID3v1 detected at offset "+t);
        // throw newBitstreamException(INVALIDFRAME, null);
        // }
        // }

        for ( int k = 0; k < bytesize; k = k + 4 )
        {
            byte b0 = 0;
            byte b1 = 0;
            byte b2 = 0;
            byte b3 = 0;
            b0 = byteread[ k ];

            if ( k + 1 < bytesize )
            {
                b1 = byteread[ k + 1 ];
            }

            if ( k + 2 < bytesize )
            {
                b2 = byteread[ k + 2 ];
            }

            if ( k + 3 < bytesize )
            {
                b3 = byteread[ k + 3 ];
            }

            framebuffer[ b++ ] = b0 << 24 & 0xFF000000 | b1 << 16 & 0x00FF0000 | b2 << 8 & 0x0000FF00 | b3 & 0x000000FF;
        }

        wordpointer = 0;
        bitindex    = 0;
    }

    /**
     * Read bits from buffer into the lower bits of an unsigned int. The LSB contains the latest read bit of the stream. (1 <=
     * number_of_bits <= 16)
     */
    public int get_bits( int number_of_bits )
    {
        int returnvalue = 0;
        int sum         = bitindex + number_of_bits;

        // E.B
        // There is a problem here, wordpointer could be -1 ?!
        if ( wordpointer < 0 )
        {
            wordpointer = 0;
        }

        // E.B : End.

        if ( sum <= 32 )
        {
            // all bits contained in *wordpointer
            returnvalue = framebuffer[ wordpointer ] >>> 32 - sum & bitmask[ number_of_bits ];

            // returnvalue = (wordpointer[0] >> (32 - sum)) & bitmask[number_of_bits];
            if ( ( bitindex += number_of_bits ) == 32 )
            {
                bitindex = 0;
                wordpointer++; // added by me!
            }

            return returnvalue;
        }

        // E.B : Check that ?
        // ((short[])&returnvalue)[0] = ((short[])wordpointer + 1)[0];
        // wordpointer++; // Added by me!
        // ((short[])&returnvalue + 1)[0] = ((short[])wordpointer)[0];
        int Right = framebuffer[ wordpointer ] & 0x0000FFFF;

        wordpointer++;

        int Left = framebuffer[ wordpointer ] & 0xFFFF0000;

        returnvalue = Right << 16 & 0xFFFF0000 | Left >>> 16 & 0x0000FFFF;
        returnvalue >>>= 48 - sum; // returnvalue >>= 16 - (number_of_bits - (32 - bitindex))
        returnvalue &= bitmask[ number_of_bits ];

        bitindex = sum - 32;

        return returnvalue;
    }

    /**
     * Set the word we want to sync the header to. In Big-Endian byte order
     */
    void set_syncword( int syncword0 )
    {
        syncword       = syncword0 & 0xFFFFFF3F;
        single_ch_mode = ( syncword0 & 0x000000C0 ) == 0x000000C0;
    }

    /**
     * Reads the exact number of bytes from the source input stream into a byte array.
     *
     * @param b The byte array to read the specified number of bytes into.
     * @param offs The index in the array where the first byte read should be stored.
     * @param len the number of bytes to read.
     *
     * @exception BitstreamException is thrown if the specified number of bytes could not be read from the stream.
     */
    private int readFully( byte[] b, int offs, int len )
    {
        int nRead = 0;

        try
        {
            while ( len > 0 )
            {
                int bytesread = source.read( b, offs, len );

                if ( bytesread == -1 )
                {
                    while ( len-- > 0 )
                    {
                        b[ offs++ ] = 0;
                    }

                    break;

                    // throw newBitstreamException(UNEXPECTED_EOF, new EOFException());
                }

                nRead =  nRead + bytesread;
                offs  += bytesread;
                len   -= bytesread;
            }
        }
        catch ( IOException ex )
        {
            throw newBitstreamException( STREAM_ERROR, ex );
        }

        return nRead;
    }

    /**
     * Simlar to readFully, but doesn't throw exception when EOF is reached.
     */
    private int readBytes( byte[] b, int offs, int len )
    {
        int totalBytesRead = 0;

        try
        {
            while ( len > 0 )
            {
                int bytesread = source.read( b, offs, len );

                if ( bytesread == -1 )
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
            throw newBitstreamException( STREAM_ERROR, ex );
        }

        return totalBytesRead;
    }
}
