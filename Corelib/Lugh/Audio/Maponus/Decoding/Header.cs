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

using Corelib.Lugh.Audio.Maponus.Support;

namespace Corelib.Lugh.Audio.Maponus.Decoding;

/// <summary>
/// public class for extracting information from a frame header.
/// </summary>
[PublicAPI]
public class Header
{
    #region constants
    
    public const int MPEG2_LSF  = 0;
    public const int MPEG25_LSF = 2;
    public const int MPEG1      = 1;

    public const int STEREO               = 0;
    public const int JOINT_STEREO         = 1;
    public const int DUAL_CHANNEL         = 2;
    public const int SINGLE_CHANNEL       = 3;
    public const int FOURTYFOUR_POINT_ONE = 0;
    public const int FOURTYEIGHT          = 1;
    public const int THIRTYTWO            = 2;

    #endregion constants

    // ========================================================================

    public static readonly int[][] Frequencies =
    [
        [ 22050, 24000, 16000, 1 ],
        [ 44100, 48000, 32000, 1 ],
        [ 11025, 12000, 8000, 1 ],
    ];

    public static readonly int[][][] Bitrates =
    [
        [
            [
                0, 32000, 48000, 56000, 64000, 80000, 96000, 112000,
                128000, 144000, 160000, 176000, 192000, 224000, 256000, 0,
            ],
            [
                0, 8000, 16000, 24000, 32000, 40000, 48000, 56000,
                64000, 80000, 96000, 112000, 128000, 144000, 160000, 0,
            ],
            [
                0, 8000, 16000, 24000, 32000, 40000, 48000, 56000,
                64000, 80000, 96000, 112000, 128000, 144000, 160000, 0,
            ],
        ],
        [
            [
                0, 32000, 64000, 96000, 128000, 160000, 192000, 224000,
                256000, 288000, 320000, 352000, 384000, 416000, 448000, 0,
            ],
            [
                0, 32000, 48000, 56000, 64000, 80000, 96000, 112000,
                128000, 160000, 192000, 224000, 256000, 320000, 384000, 0,
            ],
            [
                0, 32000, 40000, 48000, 56000, 64000, 80000, 96000,
                112000, 128000, 160000, 192000, 224000, 256000, 320000, 0,
            ],
        ],
        [
            [
                0, 32000, 48000, 56000, 64000, 80000, 96000, 112000,
                128000, 144000, 160000, 176000, 192000, 224000, 256000, 0,
            ],
            [
                0, 8000, 16000, 24000, 32000, 40000, 48000, 56000,
                64000, 80000, 96000, 112000, 128000, 144000, 160000, 0,
            ],
            [
                0, 8000, 16000, 24000, 32000, 40000, 48000, 56000,
                64000, 80000, 96000, 112000, 128000, 144000, 160000, 0,
            ],
        ],
    ];

    public static readonly string[][][] BitrateStr =
    [
        [
            [
                "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s", "96 kbit/s",
                "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s", "176 kbit/s", "192 kbit/s", "224 kbit/s",
                "256 kbit/s", "forbidden",
            ],
            [
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                "forbidden",
            ],
            [
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                "forbidden",
            ],
        ],
        [
            [
                "free format", "32 kbit/s", "64 kbit/s", "96 kbit/s", "128 kbit/s", "160 kbit/s", "192 kbit/s",
                "224 kbit/s", "256 kbit/s", "288 kbit/s", "320 kbit/s", "352 kbit/s", "384 kbit/s", "416 kbit/s",
                "448 kbit/s", "forbidden",
            ],
            [
                "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s", "96 kbit/s",
                "112 kbit/s", "128 kbit/s", "160 kbit/s", "192 kbit/s", "224 kbit/s", "256 kbit/s", "320 kbit/s",
                "384 kbit/s", "forbidden",
            ],
            [
                "free format", "32 kbit/s", "40 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s",
                "96 kbit/s", "112 kbit/s", "128 kbit/s", "160 kbit/s", "192 kbit/s", "224 kbit/s", "256 kbit/s",
                "320 kbit/s", "forbidden",
            ],
        ],
        [
            [
                "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s", "96 kbit/s",
                "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s", "176 kbit/s", "192 kbit/s", "224 kbit/s",
                "256 kbit/s", "forbidden",
            ],
            [
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                "forbidden",
            ],
            [
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                "forbidden",
            ],
        ],
    ];

    // ========================================================================
    
    public int   Framesize { get; set; }
    public short Checksum  { get; set; }
    public int   NSlots    { get; set; }

    // ========================================================================
    
    private int    _bitrateIndex;
    private bool   _copyright;
    private Crc16? _crc;
    private int    _headerstring = -1;
    private int    _intensityStereoBound;
    private int    _layer;
    private int    _mode;
    private int    _modeExtension;
    private int    _numberOfSubbands;
    private bool   _original;
    private int    _paddingBit;
    private int    _protectionBit;
    private int    _sampleFrequency;
    private sbyte  _syncmode = Bitstream.INITIAL_SYNC;
    private int    _version;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Returns synchronized header.
    /// </summary>
    public virtual int SyncHeader => _headerstring;

    /// <summary>
    /// Read a 32-bit header from the bitstream.
    /// </summary>
    public void ReadHeader( Bitstream stream, Crc16[]? crcp )
    {
        int headerstring;

        var sync = false;

        do
        {
            headerstring  = stream.SyncHeader( _syncmode );
            _headerstring = headerstring;

            if ( _syncmode == Bitstream.INITIAL_SYNC )
            {
                _version = SupportClass.URShift( headerstring, 19 ) & 1;

                if ( ( SupportClass.URShift( headerstring, 20 ) & 1 ) == 0 )

                    // SZD: MPEG2.5 detection
                {
                    if ( _version == MPEG2_LSF )
                    {
                        _version = MPEG25_LSF;
                    }
                    else
                    {
                        throw new BitstreamException( BitstreamErrors.UNKNOWN_ERROR );
                    }
                }

                if ( ( _sampleFrequency = SupportClass.URShift( headerstring, 10 ) & 3 ) == 3 )
                {
                    throw new BitstreamException( BitstreamErrors.UNKNOWN_ERROR );
                }
            }

            _layer         = ( 4 - SupportClass.URShift( headerstring, 17 ) ) & 3;
            _protectionBit = SupportClass.URShift( headerstring, 16 ) & 1;
            _bitrateIndex  = SupportClass.URShift( headerstring, 12 ) & 0xF;
            _paddingBit    = SupportClass.URShift( headerstring, 9 ) & 1;
            _mode          = SupportClass.URShift( headerstring, 6 ) & 3;
            _modeExtension = SupportClass.URShift( headerstring, 4 ) & 3;

            if ( _mode == JOINT_STEREO )
            {
                _intensityStereoBound = ( _modeExtension << 2 ) + 4;
            }
            else
            {
                _intensityStereoBound = 0;
            }

            // should never be used
            _copyright |= ( SupportClass.URShift( headerstring, 3 ) & 1 ) == 1;
            _original  |= ( SupportClass.URShift( headerstring, 2 ) & 1 ) == 1;

            // calculate number of subbands:
            if ( _layer == 1 )
            {
                _numberOfSubbands = 32;
            }
            else
            {
                var channelBitrate = _bitrateIndex;

                // calculate bitrate per channel:
                if ( _mode != SINGLE_CHANNEL )
                {
                    if ( channelBitrate == 4 )
                    {
                        channelBitrate = 1;
                    }
                    else
                    {
                        channelBitrate -= 4;
                    }
                }

                if ( channelBitrate is 1 or 2 )
                {
                    _numberOfSubbands = _sampleFrequency == THIRTYTWO ? 12 : 8;
                }
                else if ( ( _sampleFrequency == FOURTYEIGHT ) || channelBitrate is >= 3 and <= 5 )
                {
                    _numberOfSubbands = 27;
                }
                else
                {
                    _numberOfSubbands = 30;
                }
            }

            if ( _intensityStereoBound > _numberOfSubbands )
            {
                _intensityStereoBound = _numberOfSubbands;
            }

            // calculate framesize and nSlots
            CalculateFrameSize();

            // read framedata:
            stream.Read_frame_data( Framesize );

            if ( stream.IsSyncCurrentPosition( _syncmode ) )
            {
                if ( _syncmode == Bitstream.INITIAL_SYNC )
                {
                    _syncmode = Bitstream.STRICT_SYNC;
                    stream.SetSyncWord( headerstring & unchecked( ( int ) 0xFFF80CC0 ) );
                }

                sync = true;
            }
            else
            {
                stream.UnreadFrame();
            }
        }
        while ( !sync );

        stream.ParseFrame();

        if ( _protectionBit == 0 )
        {
            // frame contains a crc checksum
            Checksum = ( short ) stream.GetBitsFromBuffer( 16 );

            _crc ??= new Crc16();
            _crc.AddBits( headerstring, 16 );

            crcp![ 0 ] = _crc;
        }
        else
        {
            crcp![ 0 ] = null!;
        }

        if ( _sampleFrequency == FOURTYFOUR_POINT_ONE )
        {
            /*
            if (offset == null)
            {
            int max = max_number_of_frames(stream);
            offset = new int[max];
            for(int i=0; i<max; i++) offset[i] = 0;
            }
            // Bizarre, y avait ici une acollade ouvrante
            int cf = stream.current_frame();
            int lf = stream.last_frame();
            if ((cf > 0) && (cf == lf))
            {
            offset[cf] = offset[cf-1] + h_padding_bit;
            }
            else
            {
            offset[0] = h_padding_bit;
            }
            */
        }
    }

    // Functions to query header contents:
    /// <summary>
    /// Returns version.
    /// </summary>
    public int Version()
    {
        return _version;
    }

    /// <summary>
    /// Returns Layer ID.
    /// </summary>
    public int Layer()
    {
        return _layer;
    }

    /// <summary>
    /// Returns bitrate index.
    /// </summary>
    public int bitrate_index()
    {
        return _bitrateIndex;
    }

    /// <summary>
    /// Returns Sample Frequency.
    /// </summary>
    public int GetSampleFrequency()
    {
        return _sampleFrequency;
    }

    /// <summary>
    /// Returns Frequency.
    /// </summary>
    public int Frequency()
    {
        return Frequencies[ _version ][ _sampleFrequency ];
    }

    /// <summary>
    /// Returns Mode.
    /// </summary>
    public int Mode()
    {
        return _mode;
    }

    /// <summary>
    /// Returns Protection bit.
    /// </summary>
    public bool IsProtection()
    {
        return _protectionBit == 0;
    }

    /// <summary>
    /// Returns Copyright.
    /// </summary>
    public bool IsCopyright()
    {
        return _copyright;
    }

    /// <summary>
    /// Returns Original.
    /// </summary>
    public bool IsOriginal()
    {
        return _original;
    }

    /// <summary>
    /// Returns Checksum flag.
    /// Compares computed checksum with stream checksum.
    /// </summary>
    public bool IsChecksumOk()
    {
        return Checksum == _crc?.Checksum();
    }

    /// <summary>
    /// Returns Layer III Padding bit.
    /// </summary>
    public bool IsPadding()
    {
        return _paddingBit != 0;
    }

    /// <summary>
    /// Returns Slots.
    /// </summary>
    public int Slots()
    {
        return NSlots;
    }

    /// <summary>
    /// Returns Mode Extension.
    /// </summary>
    public int ModeExtension()
    {
        return _modeExtension;
    }

    /// <summary>
    /// Calculate Frame size in bytes excluding header size.
    /// </summary>
    public int CalculateFrameSize()
    {
        if ( _layer == 1 )
        {
            Framesize = ( 12 * Bitrates[ _version ][ 0 ][ _bitrateIndex ] ) / Frequencies[ _version ][ _sampleFrequency ];

            if ( _paddingBit != 0 )
            {
                Framesize++;
            }

            Framesize <<= 2; // one slot is 4 bytes long
            NSlots    =   0;
        }
        else
        {
            Framesize = ( 144 * Bitrates[ _version ][ _layer - 1 ][ _bitrateIndex ] ) / Frequencies[ _version ][ _sampleFrequency ];

            if ( _version is MPEG2_LSF or MPEG25_LSF )
            {
                Framesize >>= 1;
            }

            // SZD
            if ( _paddingBit != 0 )
            {
                Framesize++;
            }

            // Layer III slots
            if ( _layer == 3 )
            {
                if ( _version == MPEG1 )
                {
                    NSlots = Framesize - ( _mode == SINGLE_CHANNEL ? 17 : 32 ) - ( _protectionBit != 0 ? 0 : 2 ) - 4; // header size
                }
                else
                {
                    // MPEG-2 LSF, SZD: MPEG-2.5 LSF
                    NSlots = Framesize - ( _mode == SINGLE_CHANNEL ? 9 : 17 ) - ( _protectionBit != 0 ? 0 : 2 ) - 4; // header size
                }
            }
            else
            {
                NSlots = 0;
            }
        }

        Framesize -= 4; // subtract header size

        return Framesize;
    }

    /// <summary>
    /// Returns the maximum number of frames in the stream.
    /// </summary>
    public int MaxNumberOfFrame( int streamsize )
    {
        if ( ( ( Framesize + 4 ) - _paddingBit ) == 0 )
        {
            return 0;
        }

        return streamsize / ( ( Framesize + 4 ) - _paddingBit );
    }

    /// <summary>
    /// Returns the maximum number of frames in the stream.
    /// </summary>
    public int MinNumberOfFrames( int streamsize )
    {
        if ( ( ( Framesize + 5 ) - _paddingBit ) == 0 )
        {
            return 0;
        }

        return streamsize / ( ( Framesize + 5 ) - _paddingBit );
    }

    /// <summary>
    /// Returns ms/frame.
    /// </summary>
    public float MsPerFrame()
    {
        float[][] msPerFrameArray =
        [
            [ 8.707483f, 8.0f, 12.0f ], [ 26.12245f, 24.0f, 36.0f ],
            [ 26.12245f, 24.0f, 36.0f ],
        ];

        return msPerFrameArray[ _layer - 1 ][ _sampleFrequency ];
    }

    /// <summary>
    /// Returns total ms.
    /// </summary>
    public float TotalMS( int streamsize )
    {
        return MaxNumberOfFrame( streamsize ) * MsPerFrame();
    }

    // functions which return header informations as strings:
    /// <summary>
    /// Return Layer version.
    /// </summary>
    public string? LayerAsString()
    {
        return _layer switch
        {
            1     => "I",
            2     => "II",
            3     => "III",
            var _ => null,
        };
    }

    /// <summary>
    /// Returns Bitrate.
    /// </summary>
    public string BitrateAsString()
    {
        return BitrateStr[ _version ][ _layer - 1 ][ _bitrateIndex ];
    }

    /// <summary>
    /// Returns Frequency
    /// </summary>
    public string? SampleFrequencyAsString()
    {
        switch ( _sampleFrequency )
        {
            case THIRTYTWO:
                if ( _version == MPEG1 )
                {
                    return "32 kHz";
                }

                return _version == MPEG2_LSF ? "16 kHz" : "8 kHz";

            case FOURTYFOUR_POINT_ONE:
                if ( _version == MPEG1 )
                {
                    return "44.1 kHz";
                }

                return _version == MPEG2_LSF ? "22.05 kHz" : "11.025 kHz";

            case FOURTYEIGHT:
                if ( _version == MPEG1 )
                {
                    return "48 kHz";
                }

                return _version == MPEG2_LSF ? "24 kHz" : "12 kHz";
        }

        return null;
    }

    /// <summary>
    /// Returns Mode.
    /// </summary>
    public string? ModeAsString()
    {
        switch ( _mode )
        {
            case STEREO:
                return "Stereo";

            case JOINT_STEREO:
                return "Joint stereo";

            case DUAL_CHANNEL:
                return "Dual channel";

            case SINGLE_CHANNEL:
                return "Single channel";
        }

        return null;
    }

    /// <summary>
    /// Returns Version.
    /// </summary>
    public string? VersionAsString()
    {
        switch ( _version )
        {
            case MPEG1:
                return "MPEG-1";

            case MPEG2_LSF:
                return "MPEG-2 LSF";

            case MPEG25_LSF:
                return "MPEG-2.5 LSF";
        }

        return null;
    }

    /// <summary>
    /// Returns the number of subbands in the current frame.
    /// </summary>
    public int NumberSubbands()
    {
        return _numberOfSubbands;
    }

    /// <summary>
    /// Returns Intensity Stereo.
    /// Layer II joint stereo only).
    /// Returns the number of subbands which are in stereo mode,
    /// subbands above that limit are in intensity stereo mode.
    /// </summary>
    public int IntensityStereoBound()
    {
        return _intensityStereoBound;
    }

    public override string ToString()
    {
        var buffer = new StringBuilder( 200 );

        buffer.Append( "Layer " );
        buffer.Append( LayerAsString() );
        buffer.Append( " frame " );
        buffer.Append( ModeAsString() );
        buffer.Append( ' ' );
        buffer.Append( VersionAsString() );

        if ( !IsProtection() )
        {
            buffer.Append( " no" );
        }

        buffer.Append( " checksums" );
        buffer.Append( ' ' );
        buffer.Append( SampleFrequencyAsString() );
        buffer.Append( ',' );
        buffer.Append( ' ' );
        buffer.Append( BitrateAsString() );

        return buffer.ToString();
    }
}
