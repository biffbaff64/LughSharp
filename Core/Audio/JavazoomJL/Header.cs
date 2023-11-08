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

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// Class for extracting information from a frame header.
/// </summary>
[PublicAPI]
public class Header
{
    public static int[][] frequencies =
    {
        new[] { 22050, 24000, 16000, 1 },
        new[] { 44100, 48000, 32000, 1 },
        new[] { 11025, 12000, 8000, 1 }
    };

    // Constant for MPEG-2 LSF version
    public const int MPEG2_LSF  = 0;
    public const int MPEG25_LSF = 2;

    // Constant for MPEG-1 version
    public const int MPEG1 = 1;

    public const int STEREO               = 0;
    public const int JOINT_STEREO         = 1;
    public const int DUAL_CHANNEL         = 2;
    public const int SINGLE_CHANNEL       = 3;
    public const int FOURTYFOUR_POINT_ONE = 0;
    public const int FOURTYEIGHT          = 1;
    public const int THIRTYTWO            = 2;

    #region public properties

    public short   Checksum              { get; set; }
    public int     Framesize             { get; set; }
    public int     NSlots                { get; set; }
    public int     HModeExtension        { get; set; }
    public int     HeaderString          { get; set; } = -1;
    public int     HNumberOfSubbands     { get; set; }
    public int     HIntensityStereoBound { get; set; }
    public byte[]? HVbrToc               { get; set; }
    public Int32   HVbrScale             { get; set; }
    public bool    HVbr                  { get; set; }
    public bool    HCopyright            { get; set; }
    public bool    HOriginal             { get; set; }
    public int     HMode                 { get; set; }
    public int     HSampleFrequency      { get; set; }
    public int     HBitrateIndex         { get; set; }
    public int     HVersion              { get; set; }
    public int     HLayer                { get; set; }

    #endregion public properties

    #region private

    private int      _hProtectionBit;
    private int      _hPaddingBit;
    private double[] _hVbrTimePerFrame = { -1, 384, 1152, 1152 };
    private Int32    _hVbrFrames;
    private Int32    _hVbrBytes;
    private byte     _syncmode = Bitstream.INITIAL_SYNC;
    private Crc16?   _crc;

    #endregion

    // ------------------------------------------------------------------------

    /// <summary>
    /// Read a 32-bit header from the bitstream.
    /// </summary>
    private void ReadHeader( Bitstream stream, Crc16?[] crcp )
    {
        int headerString;
        var sync = false;

        do
        {
            headerString = stream.SyncHeader( _syncmode );
            HeaderString = headerString;

            if ( _syncmode == Bitstream.INITIAL_SYNC )
            {
                HVersion = ( headerString >>> 19 ) & 1;

                if ( ( ( headerString >>> 20 ) & 1 ) == 0 )
                {
                    if ( HVersion == MPEG2_LSF )
                    {
                        HVersion = MPEG25_LSF;
                    }
                    else
                    {
                        throw new BitstreamException( Bitstream.UNKNOWN_ERROR, null );
                    }
                }

                if ( ( HSampleFrequency = ( headerString >>> 10 ) & 3 ) == 3 )
                {
                    throw new BitstreamException( Bitstream.UNKNOWN_ERROR, null );
                }
            }

            HLayer          = ( 4 - ( headerString >>> 17 ) ) & 3;
            _hProtectionBit = ( headerString >>> 16 ) & 1;
            HBitrateIndex   = ( headerString >>> 12 ) & 0xF;
            _hPaddingBit    = ( headerString >>> 9 ) & 1;
            HMode           = ( headerString >>> 6 ) & 3;
            HModeExtension  = ( headerString >>> 4 ) & 3;

            if ( HMode == JOINT_STEREO )
            {
                HIntensityStereoBound = ( HModeExtension << 2 ) + 4;
            }
            else
            {
                // should never be used
                HIntensityStereoBound = 0;
            }

            if ( ( ( headerString >>> 3 ) & 1 ) == 1 )
            {
                HCopyright = true;
            }

            if ( ( ( headerString >>> 2 ) & 1 ) == 1 )
            {
                HOriginal = true;
            }

            // calculate number of subbands:
            if ( HLayer == 1 )
            {
                HNumberOfSubbands = 32;
            }
            else
            {
                var channelBitrate = HBitrateIndex;

                // calculate bitrate per channel:
                if ( HMode != SINGLE_CHANNEL )
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

                if ( ( channelBitrate == 1 ) || ( channelBitrate == 2 ) )
                {
                    HNumberOfSubbands = HSampleFrequency == THIRTYTWO ? 12 : 8;
                }
                else if ( ( HSampleFrequency == FOURTYEIGHT )
                       || channelBitrate is >= 3 and <= 5 )
                {
                    HNumberOfSubbands = 27;
                }
                else
                {
                    HNumberOfSubbands = 30;
                }
            }

            if ( HIntensityStereoBound > HNumberOfSubbands )
            {
                HIntensityStereoBound = HNumberOfSubbands;
            }

            // calculate Framesize and NSlots
            CalculateFramesize();

            // read framedata:
            int framesizeloaded = stream.ReadFrameData( Framesize );

            if ( ( Framesize >= 0 ) && ( framesizeloaded != Framesize ) )
            {
                throw new BitstreamException( Bitstream.INVALIDFRAME, null );
            }

            if ( stream.IsSyncCurrentPosition( _syncmode ) )
            {
                if ( _syncmode == Bitstream.INITIAL_SYNC )
                {
                    _syncmode = Bitstream.STRICT_SYNC;

                    stream.SetSyncword( headerString & 0xFFF80CC0 );
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

        if ( _hProtectionBit == 0 )
        {
            // frame contains a _crc Checksum
            Checksum = stream.GetBits( 16 );

            _crc ??= new Crc16();

            _crc.AddBits( headerString, 16 );

            crcp[ 0 ] = _crc;
        }
        else
        {
            crcp[ 0 ] = null;
        }

        if ( HSampleFrequency == FOURTYFOUR_POINT_ONE )
        {
//            if ( offset == null )
//            {
//                int max = max_number_of_frames( stream );
//                offset = new int[ max ];
//
//                for ( int i = 0; i < max; i++ )
//                {
//                    offset[ i ] = 0;
//                }
//            }
//
//            // Investigate more
//            int cf = stream.CurrentFrame();
//            int lf = stream.LastFrame();
//            
//            if ( ( cf > 0 ) && ( cf == lf ) )
//            {
//                offset[ cf ] = offset[ cf - 1 ] + _h_padding_bit;
//            }
//            else
//            {
//                offset[ 0 ] = _h_padding_bit;
//            }
        }
    }

    /// <summary>
    /// Parse frame to extract optionnal VBR frame.
    /// </summary>
    public void ParseVbr( byte[] firstframe )
    {
        // Trying Xing header.
        var   xing = "Xing";
        var   tmp  = new long[ 4 ];
        Int32 offset;

        // Compute "Xing" offset depending on MPEG version and channels.
        if ( HVersion == MPEG1 )
        {
            if ( HMode == SINGLE_CHANNEL )
            {
                offset = 21 - 4;
            }
            else
            {
                offset = 36 - 4;
            }
        }
        else if ( HMode == SINGLE_CHANNEL )
        {
            offset = 13 - 4;
        }
        else
        {
            offset = 21 - 4;
        }

        try
        {
            Array.Copy( firstframe, offset, tmp, 0, 4 );

            // Is "Xing" ?
            if ( xing.Equals( tmp ) )
            {
                // Yes.
                _hVbrFrames = -1;
                _hVbrBytes  = -1;
                HVbr        = true;
                HVbrScale   = -1;
                HVbrToc     = new byte[ 100 ];

                var length = 4;

                // Read flags.
                var flags = new byte[ 4 ];

                Array.Copy( firstframe, offset + length, flags, 0, flags.Length );

                length += flags.Length;

                // Read number of frames (if available).
                if ( ( flags[ 3 ] & ( 1 << 0 ) ) != 0 )
                {
                    Array.Copy( firstframe, offset + length, tmp, 0, tmp.Length );

                    _hVbrFrames = ( Int32 )( ( ( tmp[ 0 ] << 24 ) & 0xFF000000 )
                                           | ( ( tmp[ 1 ] << 16 ) & 0x00FF0000 )
                                           | ( ( tmp[ 2 ] << 8 ) & 0x0000FF00 )
                                           | ( tmp[ 3 ] & 0x000000FF ) );

                    length += 4;
                }

                // Read size (if available).
                if ( ( flags[ 3 ] & ( 1 << 1 ) ) != 0 )
                {
                    Array.Copy( firstframe, offset + length, tmp, 0, tmp.Length );

                    _hVbrBytes = ( Int32 )( ( ( tmp[ 0 ] << 24 ) & 0xFF000000 )
                                          | ( ( tmp[ 1 ] << 16 ) & 0x00FF0000 )
                                          | ( ( tmp[ 2 ] << 8 ) & 0x0000FF00 )
                                          | ( tmp[ 3 ] & 0x000000FF ) );

                    length += 4;
                }

                // Read TOC (if available).
                if ( ( flags[ 3 ] & ( 1 << 2 ) ) != 0 )
                {
                    Array.Copy( firstframe, offset + length, HVbrToc, 0, HVbrToc.Length );
                    length += HVbrToc.Length;
                }

                // Read scale (if available).
                if ( ( flags[ 3 ] & ( 1 << 3 ) ) != 0 )
                {
                    Array.Copy( firstframe, offset + length, tmp, 0, tmp.Length );

                    HVbrScale = ( Int32 )( ( ( tmp[ 0 ] << 24 ) & 0xFF000000 )
                                         | ( ( tmp[ 1 ] << 16 ) & 0x00FF0000 )
                                         | ( ( tmp[ 2 ] << 8 ) & 0x0000FF00 )
                                         | ( tmp[ 3 ] & 0x000000FF ) );
                }

                Console.WriteLine( $@"VBR:{xing} Frames:{_hVbrFrames} Size:{_hVbrBytes}" );
            }
        }
        catch ( IndexOutOfRangeException e )
        {
            throw new BitstreamException( "XingVBRHeader Corrupted", e );
        }

        // Trying VBRI header.
        var vbri = "VBRI";
        offset = 36 - 4;

        try
        {
            Array.Copy( firstframe, offset, tmp, 0, 4 );

            // Is "VBRI" ?
            if ( vbri.Equals( tmp ) )
            {
                // Yes.
                HVbr        = true;
                _hVbrFrames = -1;
                _hVbrBytes  = -1;
                HVbrScale   = -1;
                HVbrToc     = new byte[ 100 ];

                // Bytes.
                var length = 4 + 6;

                Array.Copy( firstframe, offset + length, tmp, 0, tmp.Length );

                _hVbrBytes = ( Int32 )( ( ( tmp[ 0 ] << 24 ) & 0xFF000000 )
                                      | ( ( tmp[ 1 ] << 16 ) & 0x00FF0000 )
                                      | ( ( tmp[ 2 ] << 8 ) & 0x0000FF00 )
                                      | ( tmp[ 3 ] & 0x000000FF ) );

                length += 4;

                // Frames.
                Array.Copy( firstframe, offset + length, tmp, 0, tmp.Length );

                _hVbrFrames = ( Int32 )( ( ( tmp[ 0 ] << 24 ) & 0xFF000000 )
                                       | ( ( tmp[ 1 ] << 16 ) & 0x00FF0000 )
                                       | ( ( tmp[ 2 ] << 8 ) & 0x0000FF00 )
                                       | ( tmp[ 3 ] & 0x000000FF ) );
            }
        }
        catch ( IndexOutOfRangeException e )
        {
            throw new BitstreamException( "VBRIVBRHeader Corrupted", e );
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int Frequency() => frequencies[ HVersion ][ HSampleFrequency ];

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public bool Checksums()
    {
        if ( _hProtectionBit == 0 )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns Checksum flag. Compares computed Checksum with stream Checksum.
    /// </summary>
    public bool checksum_ok()
    {
        return Checksum == _crc?.Checksum();
    }

    /// <summary>
    /// Returns Layer III Padding bit.
    /// </summary>
    public bool Padding()
    {
        if ( _hPaddingBit == 0 )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private static int[][][] _bitrates =
    {
        new[]
        {
            new[]
            {
                0 /* free format */, 32000, 48000, 56000, 64000, 80000, 96000,
                112000, 128000, 144000, 160000, 176000, 192000, 224000,
                256000, 0
            },
            new[]
            {
                0 /* free format */, 8000, 16000, 24000, 32000, 40000, 48000,
                56000, 64000, 80000, 96000, 112000, 128000, 144000,
                160000, 0
            },
            new[]
            {
                0 /* free format */, 8000, 16000, 24000, 32000, 40000, 48000,
                56000, 64000, 80000, 96000, 112000, 128000, 144000,
                160000, 0
            }
        },
        new[]
        {
            new[]
            {
                0 /* free format */, 32000, 64000, 96000, 128000, 160000, 192000,
                224000, 256000, 288000, 320000, 352000, 384000,
                416000, 448000, 0
            },
            new[]
            {
                0 /* free format */, 32000, 48000, 56000, 64000, 80000, 96000,
                112000, 128000, 160000, 192000, 224000, 256000, 320000,
                384000, 0
            },
            new[]
            {
                0 /* free format */, 32000, 40000, 48000, 56000, 64000, 80000,
                96000, 112000, 128000, 160000, 192000, 224000, 256000,
                320000, 0
            }
        },
        new[]
        {
            new[]
            {
                0 /* free format */, 32000, 48000, 56000, 64000, 80000, 96000,
                112000, 128000, 144000, 160000, 176000, 192000, 224000,
                256000, 0
            },
            new[]
            {
                0 /* free format */, 8000, 16000, 24000, 32000, 40000, 48000,
                56000, 64000, 80000, 96000, 112000, 128000, 144000,
                160000, 0
            },
            new[]
            {
                0 /* free format */, 8000, 16000, 24000, 32000, 40000, 48000,
                56000, 64000, 80000, 96000, 112000, 128000, 144000,
                160000, 0
            }
        }
    };

    /// <summary>
    /// Calculate Frame size. Calculates Framesize in bytes excluding header size.
    /// </summary>
    public int CalculateFramesize()
    {
        if ( HLayer == 1 )
        {
            Framesize = ( 12 * _bitrates[ HVersion ][ 0 ][ HBitrateIndex ] ) / frequencies[ HVersion ][ HSampleFrequency ];

            if ( _hPaddingBit != 0 )
            {
                Framesize++;
            }

            Framesize <<= 2; // one slot is 4 bytes long
            NSlots    =   0;
        }
        else
        {
            Framesize = ( 144 * _bitrates[ HVersion ][ HLayer - 1 ][ HBitrateIndex ] ) / frequencies[ HVersion ][ HSampleFrequency ];

            if ( ( HVersion == MPEG2_LSF ) || ( HVersion == MPEG25_LSF ) )
            {
                Framesize >>= 1; // SZD
            }

            if ( _hPaddingBit != 0 )
            {
                Framesize++;
            }

            // Layer III slots
            if ( HLayer == 3 )
            {
                if ( HVersion == MPEG1 )
                {
                    NSlots = Framesize
                           - ( HMode == SINGLE_CHANNEL ? 17 : 32 ) // side info size
                           - ( _hProtectionBit != 0 ? 0 : 2 )      // CRC size
                           - 4;                                    // header size
                }
                else
                {
                    NSlots = Framesize
                           - ( HMode == SINGLE_CHANNEL ? 9 : 17 ) // side info size
                           - ( _hProtectionBit != 0 ? 0 : 2 )     // CRC size
                           - 4;                                   // header size
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
    /// <param name="streamsize"></param>
    /// <returns> number of frames </returns>
    public int MaxNumberOfFrames( int streamsize )
    {
        if ( HVbr )
        {
            return _hVbrFrames;
        }
        else if ( ( ( Framesize + 4 ) - _hPaddingBit ) == 0 )
        {
            return 0;
        }
        else
        {
            return streamsize / ( ( Framesize + 4 ) - _hPaddingBit );
        }
    }

    /**
     * Returns the maximum number of frames in the stream.
     * @param streamsize
     * @return number of frames
     */
    public int MinNumberOfFrames( int streamsize ) // E.B
    {
        if ( HVbr )
        {
            return _hVbrFrames;
        }
        else if ( ( ( Framesize + 5 ) - _hPaddingBit ) == 0 )
        {
            return 0;
        }
        else
        {
            return streamsize / ( ( Framesize + 5 ) - _hPaddingBit );
        }
    }

    /// <summary>
    /// Returns milliseconds per frame
    /// </summary>
    public float MsPerFrame()
    {
        float[][] msPerFrameArray =
        {
            new[] { 8.707483f, 8.0f, 12.0f },
            new[] { 26.12245f, 24.0f, 36.0f },
            new[] { 26.12245f, 24.0f, 36.0f },
        };

        if ( HVbr )
        {
            var tpf = _hVbrTimePerFrame[ HLayer ] / Frequency();

            if ( ( HVersion == MPEG2_LSF ) || ( HVersion == MPEG25_LSF ) )
            {
                tpf /= 2;
            }

            return ( float )( tpf * 1000 );
        }
        else
        {
            return msPerFrameArray[ HLayer - 1 ][ HSampleFrequency ];
        }
    }

    /// <summary>
    /// Returns total ms.
    /// </summary>
    /// <param name="streamsize"></param>
    /// <returns> total milliseconds </returns>
    public float TotalMS( int streamsize ) => MaxNumberOfFrames( streamsize ) * MsPerFrame();

    /// <summary>
    /// Return Layer version.
    /// </summary>
    public string? LayerString()
    {
        switch ( HLayer )
        {
            case 1:
                return "I";

            case 2:
                return "II";

            case 3:
                return "III";
        }

        return null;
    }

    private static string[][][] _bitrateStr =
    {
        new[]
        {
            new[]
            {
                "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s",
                "128 kbit/s", "144 kbit/s", "160 kbit/s", "176 kbit/s",
                "192 kbit/s", "224 kbit/s", "256 kbit/s", "forbidden"
            },
            new[]
            {
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s",
                "32 kbit/s", "40 kbit/s", "48 kbit/s", "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s",
                "128 kbit/s", "144 kbit/s", "160 kbit/s", "forbidden"
            },
            new[]
            {
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s",
                "32 kbit/s", "40 kbit/s", "48 kbit/s", "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s",
                "128 kbit/s", "144 kbit/s", "160 kbit/s", "forbidden"
            }
        },
        new[]
        {
            new[]
            {
                "free format", "32 kbit/s", "64 kbit/s", "96 kbit/s",
                "128 kbit/s", "160 kbit/s", "192 kbit/s", "224 kbit/s",
                "256 kbit/s", "288 kbit/s", "320 kbit/s", "352 kbit/s",
                "384 kbit/s", "416 kbit/s", "448 kbit/s", "forbidden"
            },
            new[]
            {
                "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s",
                "128 kbit/s", "160 kbit/s", "192 kbit/s", "224 kbit/s",
                "256 kbit/s", "320 kbit/s", "384 kbit/s", "forbidden"
            },
            new[]
            {
                "free format", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                "56 kbit/s", "64 kbit/s", "80 kbit/s", "96 kbit/s",
                "112 kbit/s", "128 kbit/s", "160 kbit/s", "192 kbit/s",
                "224 kbit/s", "256 kbit/s", "320 kbit/s", "forbidden"
            }
        },
        new[]
        {
            new[]
            {
                "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s",
                "128 kbit/s", "144 kbit/s", "160 kbit/s", "176 kbit/s",
                "192 kbit/s", "224 kbit/s", "256 kbit/s", "forbidden"
            },
            new[]
            {
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s",
                "32 kbit/s", "40 kbit/s", "48 kbit/s", "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s",
                "128 kbit/s", "144 kbit/s", "160 kbit/s", "forbidden"
            },
            new[]
            {
                "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s",
                "32 kbit/s", "40 kbit/s", "48 kbit/s", "56 kbit/s",
                "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s",
                "128 kbit/s", "144 kbit/s", "160 kbit/s", "forbidden"
            }
        },
    };

    /// <summary>
    /// Return Bitrate.
    /// </summary>
    /// <returns> bitrate in bps </returns>
    public string BitrateString()
    {
        if ( HVbr )
        {
            return $"{Bitrate() / 1000} kb/s";
        }
        else
        {
            return _bitrateStr[ HVersion ][ HLayer - 1 ][ HBitrateIndex ];
        }
    }

    /// <summary>
    /// Return Bitrate.
    /// </summary>
    /// <returns> bitrate in bps and average bitrate for VBR header </returns>
    public int Bitrate()
    {
        if ( HVbr )
        {
            return ( int )( ( _hVbrBytes * 8 ) / ( MsPerFrame() * _hVbrFrames ) ) * 1000;
        }
        else
        {
            return _bitrates[ HVersion ][ HLayer - 1 ][ HBitrateIndex ];
        }
    }

    /// <summary>
    /// Return Instant Bitrate. Bitrate for VBR is not constant.
    /// </summary>
    /// <returns> bitrate in bps </returns>
    public int BitrateInstant() => _bitrates[ HVersion ][ HLayer - 1 ][ HBitrateIndex ];

    /// <summary>
    /// Returns Frequency string in kHz
    /// </summary>
    public string? SampleFrequencyString()
    {
        if ( HSampleFrequency == THIRTYTWO )
        {
            if ( HVersion == MPEG1 )
            {
                return "32 kHz";
            }
            else if ( HVersion == MPEG2_LSF )
            {
                return "16 kHz";
            }
            else
            {
                return "8 kHz";
            }
        }
        else if ( HSampleFrequency == FOURTYFOUR_POINT_ONE )
        {
            if ( HVersion == MPEG1 )
            {
                return "44.1 kHz";
            }
            else if ( HVersion == MPEG2_LSF )
            {
                return "22.05 kHz";
            }
            else
            {
                return "11.025 kHz";
            }
        }
        else if ( HSampleFrequency == FOURTYEIGHT )
        {
            if ( HVersion == MPEG1 )
            {
                return "48 kHz";
            }
            else if ( HVersion == MPEG2_LSF )
            {
                return "24 kHz";
            }
            else
            {
                return "12 kHz";
            }
        }

        return null;
    }

    public int GetSampleRate()
    {
        if ( HSampleFrequency == THIRTYTWO )
        {
            if ( HVersion == MPEG1 )
            {
                return 32000;
            }
            else if ( HVersion == MPEG2_LSF )
            {
                return 16000;
            }
            else
            {
                return 8000;
            }
        }
        else if ( HSampleFrequency == FOURTYFOUR_POINT_ONE )
        {
            if ( HVersion == MPEG1 )
            {
                return 44100;
            }
            else if ( HVersion == MPEG2_LSF )
            {
                return 22050;
            }
            else
            {
                return 11025;
            }
        }
        else if ( HSampleFrequency == FOURTYEIGHT )
        {
            if ( HVersion == MPEG1 )
            {
                return 48000;
            }
            else if ( HVersion == MPEG2_LSF )
            {
                return 24000;
            }
            else
            {
                return 12000;
            }
        }

        return 0;
    }

    /// <summary>
    /// Returns Mode.
    /// </summary>
    public string? ModeString()
    {
        switch ( HMode )
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
    /// Returns Version, MPEG-1 or MPEG-2 LSF or MPEG-2.5 LSF
    /// </summary>
    public string? VersionString()
    {
        switch ( HVersion )
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

    public override string ToString()
    {
        StringBuilder buffer = new( 200 );

        buffer.Append( "Layer " );
        buffer.Append( LayerString() );
        buffer.Append( " frame " );
        buffer.Append( ModeString() );
        buffer.Append( ' ' );
        buffer.Append( VersionString() );

        if ( !Checksums() )
        {
            buffer.Append( " no" );
        }

        buffer.Append( " checksums" );
        buffer.Append( ' ' );
        buffer.Append( SampleFrequencyString() );
        buffer.Append( ',' );
        buffer.Append( ' ' );
        buffer.Append( BitrateString() );

        return buffer.ToString();
    }
}
