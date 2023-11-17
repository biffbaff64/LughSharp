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

namespace LibGDXSharp.Core.Audio.JavazoomJL;

[PublicAPI]
public class LayerIIIDecoder : IFrameDecoder
{
    private const int SSLIMIT = 18;
    private const int SBLIMIT = 32;

    public int[]? ScalefacBuffer { get; set; }

    private readonly double _d43 = 4.0 / 3.0;

    private int   _counter      = 0;
    private int   _checkSumHuff = 0;
    private int[] _is1D;

    private float[ ,, ] _ro;
    private float[ ,, ] _lr;
    private float[]     _out1D;
    private float[ , ]  _prevblck;
    private float[ , ]  _k;
    private int[]       _nonzero;
    private int         _whichChannels;

    private Bitstream?       _stream;
    private Header?          _header;
    private SynthesisFilter? _filter1;
    private SynthesisFilter? _filter2;
    private OutputBuffer?    _buffer;
    private BitReserve?      _br;
    private IIISideInfoT?    _si;

    private Temporary2[] _iiiScalefactor;
    private Temporary2[] _scalefac;

    private int _maxGr;
    private int _frameStart;
    private int _part2Start;
    private int _channels;
    private int _firstChannel;
    private int _lastChannel;
    private int _sfreq;

    public LayerIIIDecoder( Bitstream stream0,
                            Header header0,
                            SynthesisFilter filtera,
                            SynthesisFilter filterb,
                            OutputBuffer buffer0,
                            int which_ch0 )
    {
        HuffCodeTab.InitHuff();

        _is1D     = new int[ ( SBLIMIT * SSLIMIT ) + 4 ];
        _ro       = new float[ 2, SBLIMIT, SSLIMIT ];
        _lr       = new float[ 2, SBLIMIT, SSLIMIT ];
        _out1D    = new float[ SBLIMIT * SSLIMIT ];
        _prevblck = new float[ 2, SBLIMIT * SSLIMIT ];
        _k        = new float[ 2, SBLIMIT * SSLIMIT ];
        _nonzero  = new int[ 2 ];

        _iiiScalefactor      = new Temporary2[ 2 ];
        _iiiScalefactor[ 0 ] = new Temporary2();
        _iiiScalefactor[ 1 ] = new Temporary2();
        _scalefac            = _iiiScalefactor;

        // L3TABLE INIT

        _sfBandIndex = new SBI[ 9 ]; // MPEG2.5 +3 indices

        int[] l0 = { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        int[] s0 = { 0, 4, 8, 12, 18, 24, 32, 42, 56, 74, 100, 132, 174, 192 };
        int[] l1 = { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 114, 136, 162, 194, 232, 278, 330, 394, 464, 540, 576 };
        int[] s1 = { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 136, 180, 192 };
        int[] l2 = { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        int[] s2 = { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192 };

        int[] l3 = { 0, 4, 8, 12, 16, 20, 24, 30, 36, 44, 52, 62, 74, 90, 110, 134, 162, 196, 238, 288, 342, 418, 576 };
        int[] s3 = { 0, 4, 8, 12, 16, 22, 30, 40, 52, 66, 84, 106, 136, 192 };
        int[] l4 = { 0, 4, 8, 12, 16, 20, 24, 30, 36, 42, 50, 60, 72, 88, 106, 128, 156, 190, 230, 276, 330, 384, 576 };
        int[] s4 = { 0, 4, 8, 12, 16, 22, 28, 38, 50, 64, 80, 100, 126, 192 };
        int[] l5 = { 0, 4, 8, 12, 16, 20, 24, 30, 36, 44, 54, 66, 82, 102, 126, 156, 194, 240, 296, 364, 448, 550, 576 };
        int[] s5 = { 0, 4, 8, 12, 16, 22, 30, 42, 58, 78, 104, 138, 180, 192 };

        // MPEG2.5
        int[] l6 = { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        int[] s6 = { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192 };
        int[] l7 = { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        int[] s7 = { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192 };
        int[] l8 = { 0, 12, 24, 36, 48, 60, 72, 88, 108, 132, 160, 192, 232, 280, 336, 400, 476, 566, 568, 570, 572, 574, 576 };
        int[] s8 = { 0, 8, 16, 24, 36, 52, 72, 96, 124, 160, 162, 164, 166, 192 };

        _sfBandIndex[ 0 ] = new SBI( l0, s0 );
        _sfBandIndex[ 1 ] = new SBI( l1, s1 );
        _sfBandIndex[ 2 ] = new SBI( l2, s2 );

        _sfBandIndex[ 3 ] = new SBI( l3, s3 );
        _sfBandIndex[ 4 ] = new SBI( l4, s4 );
        _sfBandIndex[ 5 ] = new SBI( l5, s5 );

        // MPEG2.5
        _sfBandIndex[ 6 ] = new SBI( l6, s6 );
        _sfBandIndex[ 7 ] = new SBI( l7, s7 );
        _sfBandIndex[ 8 ] = new SBI( l8, s8 );

        // END OF L3TABLE INIT

        if ( _reorderTable == null )
        {
            // generate LUT
            _reorderTable = new int[ 9 ][];

            for ( var i = 0; i < 9; i++ )
            {
                _reorderTable[ i ] = Reorder( _sfBandIndex[ i ].s );
            }
        }

        int[] ll0 = { 0, 6, 11, 16, 21 };
        int[] ss0 = { 0, 6, 12 };

        sftable        = new Sftable( ll0, ss0 );
        ScalefacBuffer = new int[ 54 ];

        _stream        = stream0;
        _header        = header0;
        _filter1       = filtera;
        _filter2       = filterb;
        _buffer        = buffer0;
        _whichChannels = which_ch0;

        _frameStart = 0;
        _channels   = _header.HMode == Header.SINGLE_CHANNEL ? 1 : 2;
        _maxGr      = _header.HVersion == Header.MPEG1 ? 2 : 1;

        _sfreq = _header.HSampleFrequency
               + ( _header.HVersion == Header.MPEG1
                     ? 3
                     : _header.HVersion == Header.MPEG25_LSF
                         ? 6
                         : 0 ); // SZD

        if ( _channels == 2 )
        {
            switch ( _whichChannels )
            {
                case OutputChannels.LEFT_CHANNEL:
                case OutputChannels.DOWNMIX_CHANNELS:
                    _firstChannel = _lastChannel = 0;

                    break;

                case OutputChannels.RIGHT_CHANNEL:
                    _firstChannel = _lastChannel = 1;

                    break;

                case OutputChannels.BOTH_CHANNELS:
                default:
                    _firstChannel = 0;
                    _lastChannel  = 1;

                    break;
            }
        }
        else
        {
            _firstChannel = _lastChannel = 0;
        }

        for ( var ch = 0; ch < 2; ch++ )
        {
            for ( var j = 0; j < 576; j++ )
            {
                _prevblck[ ch, j ] = 0.0f;
            }
        }

        _nonzero[ 0 ] = _nonzero[ 1 ] = 576;

        _br = new BitReserve();
        _si = new IIISideInfoT();
    }

    /// <summary>
    /// Notify decoder that a seek is being made.
    /// </summary>
    public void seek_notify()
    {
        _frameStart = 0;

        for ( var ch = 0; ch < 2; ch++ )
        {
            for ( var j = 0; j < 576; j++ )
            {
                _prevblck[ ch, j ] = 0.0f;
            }
        }

        _br = new BitReserve();
    }

    public void DecodeFrame()
    {
        Decode();
    }

    // subband samples are buffered and passed to the
    // SynthesisFilter in one go.
    private float[] _samples1 = new float[ 32 ];
    private float[] _samples2 = new float[ 32 ];

    /// <summary>
    /// Decode one frame, filling the buffer with the output samples.
    /// </summary>
    public void Decode()
    {
        int flushMain;

        var nSlots = _header.NSlots;

        GetSideInfo();

        for ( var i = 0; i < nSlots; i++ )
        {
            _br.HPutbuf( _stream.GetBits( 8 ) );
        }

        var mainDataEnd = _br.HSstell() >>> 3; // of previous frame

        if ( ( flushMain = _br.HSstell() & 7 ) != 0 )
        {
            _br.HGetBits( 8 - flushMain );
            mainDataEnd++;
        }

        var bytesToDiscard = _frameStart - mainDataEnd - _si.mainDataBegin;

        _frameStart += nSlots;

        if ( bytesToDiscard < 0 )
        {
            return;
        }

        if ( mainDataEnd > 4096 )
        {
            _frameStart -= 4096;
            _br.RewindNbytes( 4096 );
        }

        for ( ; bytesToDiscard > 0; bytesToDiscard-- )
        {
            _br.HGetBits( 8 );
        }

        for ( var gr = 0; gr < _maxGr; gr++ )
        {
            for ( var ch = 0; ch < _channels; ch++ )
            {
                _part2Start = _br.HSstell();

                if ( _header.HVersion == Header.MPEG1 )
                {
                    GetScaleFactors( ch, gr );
                }
                else
                {
                    // MPEG-2 LSF, SZD: MPEG-2.5 LSF
                    GetLsfScaleFactors( ch, gr );
                }

                HuffmanDecode( ch, gr );

                // System.out.println("CheckSum HuffMan = " + _checkSumHuff);
                dequantize_sample( _ro[ ch ], ch, gr );
            }

            Stereo( gr );

            if ( ( _whichChannels == OutputChannels.DOWNMIX_CHANNELS ) && ( _channels > 1 ) )
            {
                DoDownmix();
            }

            for ( var ch = _firstChannel; ch <= _lastChannel; ch++ )
            {
                Reorder( _lr[ ch ], ch, gr );
                Antialias( ch, gr );

                Hybrid( ch, gr );

                for ( var sb18 = 18; sb18 < 576; sb18 += 36 )
                {
                    // Frequency inversion
                    for ( var ss = 1; ss < SSLIMIT; ss += 2 )
                    {
                        _out1D[ sb18 + ss ] = -_out1D[ sb18 + ss ];
                    }
                }

                int sb;

                if ( ( ch == 0 ) || ( _whichChannels == OutputChannels.RIGHT_CHANNEL ) )
                {
                    for ( var ss = 0; ss < SSLIMIT; ss++ )
                    {
                        // Polyphase synthesis
                        sb = 0;

                        for ( var sb18 = 0; sb18 < 576; sb18 += 18 )
                        {
                            _samples1[ sb ] = _out1D[ sb18 + ss ];

                            // filter1.input_sample(_out1D[sb18+ss], sb);
                            sb++;
                        }

                        _filter1.InputSamples( _samples1 );
                        _filter1.CalculatePcmSamples( _buffer );
                    }
                }
                else
                {
                    for ( var ss = 0; ss < SSLIMIT; ss++ )
                    {
                        // Polyphase synthesis
                        sb = 0;

                        for ( var sb18 = 0; sb18 < 576; sb18 += 18 )
                        {
                            _samples2[ sb ] = _out1D[ sb18 + ss ];

                            // filter2.input_sample(_out1D[sb18+ss], sb);
                            sb++;
                        }

                        _filter2.InputSamples( _samples2 );
                        _filter2.CalculatePcmSamples( _buffer );
                    }
                }
            }
        }

        _counter++;
    }

    /// <summary>
    /// Reads the side info from the _stream, assuming the entire. frame has
    /// been read already.
    /// <para>
    /// Mono : 136 bits (= 17 bytes) Stereo : 256 bits (= 32 bytes)
    /// </para>
    /// </summary>
    private bool GetSideInfo()
    {
        int ch;

        if ( _header.HVersion == Header.MPEG1 )
        {

            _si.mainDataBegin = _stream.GetBits( 9 );

            if ( _channels == 1 )
            {
                _si.privateBits = _stream.GetBits( 5 );
            }
            else
            {
                _si.privateBits = _stream.GetBits( 3 );
            }

            for ( ch = 0; ch < _channels; ch++ )
            {
                _si.ch[ ch ].scfsi[ 0 ] = _stream.GetBits( 1 );
                _si.ch[ ch ].scfsi[ 1 ] = _stream.GetBits( 1 );
                _si.ch[ ch ].scfsi[ 2 ] = _stream.GetBits( 1 );
                _si.ch[ ch ].scfsi[ 3 ] = _stream.GetBits( 1 );
            }

            for ( var gr = 0; gr < 2; gr++ )
            {
                for ( ch = 0; ch < _channels; ch++ )
                {
                    _si.ch[ ch ].gr[ gr ].part23Length        = _stream.GetBits( 12 );
                    _si.ch[ ch ].gr[ gr ].bigValues           = _stream.GetBits( 9 );
                    _si.ch[ ch ].gr[ gr ].globalGain          = _stream.GetBits( 8 );
                    _si.ch[ ch ].gr[ gr ].scalefacCompress    = _stream.GetBits( 4 );
                    _si.ch[ ch ].gr[ gr ].windowSwitchingFlag = _stream.GetBits( 1 );

                    if ( _si.ch[ ch ].gr[ gr ].windowSwitchingFlag != 0 )
                    {
                        _si.ch[ ch ].gr[ gr ].blockType      = _stream.GetBits( 2 );
                        _si.ch[ ch ].gr[ gr ].mixedBlockFlag = _stream.GetBits( 1 );

                        _si.ch[ ch ].gr[ gr ].tableSelect[ 0 ] = _stream.GetBits( 5 );
                        _si.ch[ ch ].gr[ gr ].tableSelect[ 1 ] = _stream.GetBits( 5 );

                        _si.ch[ ch ].gr[ gr ].subblockGain[ 0 ] = _stream.GetBits( 3 );
                        _si.ch[ ch ].gr[ gr ].subblockGain[ 1 ] = _stream.GetBits( 3 );
                        _si.ch[ ch ].gr[ gr ].subblockGain[ 2 ] = _stream.GetBits( 3 );

                        // Set region_count parameters since they are implicit in this case.

                        if ( _si.ch[ ch ].gr[ gr ].blockType == 0 )

                            // Side info bad: blockType == 0 in split block
                        {
                            return false;
                        }
                        else if ( _si.ch[ ch ].gr[ gr ].blockType == 2 && _si.ch[ ch ].gr[ gr ].mixedBlockFlag == 0 )
                        {
                            _si.ch[ ch ].gr[ gr ].region0Count = 8;
                        }
                        else
                        {
                            _si.ch[ ch ].gr[ gr ].region0Count = 7;
                        }

                        _si.ch[ ch ].gr[ gr ].region1Count = 20 - _si.ch[ ch ].gr[ gr ].region0Count;
                    }
                    else
                    {
                        _si.ch[ ch ].gr[ gr ].tableSelect[ 0 ] = _stream.GetBits( 5 );
                        _si.ch[ ch ].gr[ gr ].tableSelect[ 1 ] = _stream.GetBits( 5 );
                        _si.ch[ ch ].gr[ gr ].tableSelect[ 2 ] = _stream.GetBits( 5 );
                        _si.ch[ ch ].gr[ gr ].region0Count     = _stream.GetBits( 4 );
                        _si.ch[ ch ].gr[ gr ].region1Count     = _stream.GetBits( 3 );
                        _si.ch[ ch ].gr[ gr ].blockType        = 0;
                    }

                    _si.ch[ ch ].gr[ gr ].preflag           = _stream.GetBits( 1 );
                    _si.ch[ ch ].gr[ gr ].scalefacScale     = _stream.GetBits( 1 );
                    _si.ch[ ch ].gr[ gr ].count1TableSelect = _stream.GetBits( 1 );
                }
            }

        }
        else
        {
            // MPEG-2 LSF, SZD: MPEG-2.5 LSF

            _si.mainDataBegin = _stream.GetBits( 8 );

            if ( _channels == 1 )
            {
                _si.privateBits = _stream.GetBits( 1 );
            }
            else
            {
                _si.privateBits = _stream.GetBits( 2 );
            }

            for ( ch = 0; ch < _channels; ch++ )
            {

                _si.ch[ ch ].gr[ 0 ].part23Length        = _stream.GetBits( 12 );
                _si.ch[ ch ].gr[ 0 ].bigValues           = _stream.GetBits( 9 );
                _si.ch[ ch ].gr[ 0 ].globalGain          = _stream.GetBits( 8 );
                _si.ch[ ch ].gr[ 0 ].scalefacCompress    = _stream.GetBits( 9 );
                _si.ch[ ch ].gr[ 0 ].windowSwitchingFlag = _stream.GetBits( 1 );

                if ( _si.ch[ ch ].gr[ 0 ].windowSwitchingFlag != 0 )
                {

                    _si.ch[ ch ].gr[ 0 ].blockType        = _stream.GetBits( 2 );
                    _si.ch[ ch ].gr[ 0 ].mixedBlockFlag   = _stream.GetBits( 1 );
                    _si.ch[ ch ].gr[ 0 ].tableSelect[ 0 ] = _stream.GetBits( 5 );
                    _si.ch[ ch ].gr[ 0 ].tableSelect[ 1 ] = _stream.GetBits( 5 );

                    _si.ch[ ch ].gr[ 0 ].subblockGain[ 0 ] = _stream.GetBits( 3 );
                    _si.ch[ ch ].gr[ 0 ].subblockGain[ 1 ] = _stream.GetBits( 3 );
                    _si.ch[ ch ].gr[ 0 ].subblockGain[ 2 ] = _stream.GetBits( 3 );

                    // Set region_count parameters since they are implicit in this case.

                    if ( _si.ch[ ch ].gr[ 0 ].blockType == 0 )

                        // Side info bad: blockType == 0 in split block
                    {
                        return false;
                    }
                    else if ( _si.ch[ ch ].gr[ 0 ].blockType == 2 && _si.ch[ ch ].gr[ 0 ].mixedBlockFlag == 0 )
                    {
                        _si.ch[ ch ].gr[ 0 ].region0Count = 8;
                    }
                    else
                    {
                        _si.ch[ ch ].gr[ 0 ].region0Count = 7;
                        _si.ch[ ch ].gr[ 0 ].region1Count = 20 - _si.ch[ ch ].gr[ 0 ].region0Count;
                    }

                }
                else
                {
                    _si.ch[ ch ].gr[ 0 ].tableSelect[ 0 ] = _stream.GetBits( 5 );
                    _si.ch[ ch ].gr[ 0 ].tableSelect[ 1 ] = _stream.GetBits( 5 );
                    _si.ch[ ch ].gr[ 0 ].tableSelect[ 2 ] = _stream.GetBits( 5 );
                    _si.ch[ ch ].gr[ 0 ].region0Count     = _stream.GetBits( 4 );
                    _si.ch[ ch ].gr[ 0 ].region1Count     = _stream.GetBits( 3 );
                    _si.ch[ ch ].gr[ 0 ].blockType        = 0;
                }

                _si.ch[ ch ].gr[ 0 ].scalefacScale     = _stream.GetBits( 1 );
                _si.ch[ ch ].gr[ 0 ].count1TableSelect = _stream.GetBits( 1 );
            }
        }

        return true;
    }

    private void GetScaleFactors( int ch, int gr )
    {
        GrInfoS grInfo    = _si.ch[ ch ].gr[ gr ];
        var     scaleComp = grInfo.scalefacCompress;
        var     length0   = _slen[ 0, scaleComp ];
        var     length1   = _slen[ 1, scaleComp ];

        if ( ( grInfo.windowSwitchingFlag != 0 ) && ( grInfo.blockType == 2 ) )
        {
            if ( grInfo.mixedBlockFlag != 0 )
            {
                // MIXED
                for ( var sfb = 0; sfb < 8; sfb++ )
                {
                    _scalefac[ ch ].l[ sfb ] = _br.HGetBits( _slen[ 0, grInfo.scalefacCompress ] );
                }

                int window;

                for ( var sfb = 3; sfb < 6; sfb++ )
                {
                    for ( window = 0; window < 3; window++ )
                    {
                        _scalefac[ ch ].s[ window, sfb ] = _br.HGetBits( _slen[ 0, grInfo.scalefacCompress ] );
                    }
                }

                for ( var sfb = 6; sfb < 12; sfb++ )
                {
                    for ( window = 0; window < 3; window++ )
                    {
                        _scalefac[ ch ].s[ window, sfb ] = _br.HGetBits( _slen[ 1, grInfo.scalefacCompress ] );
                    }
                }

                window = 0;

                for ( var sfb = 12; window < 3; window++ )
                {
                    _scalefac[ ch ].s[ window, sfb ] = 0;
                }
            }
            else
            {
                _scalefac[ ch ].s[ 0, 0 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 1, 0 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 2, 0 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 0, 1 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 1, 1 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 2, 1 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 0, 2 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 1, 2 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 2, 2 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 0, 3 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 1, 3 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 2, 3 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 0, 4 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 1, 4 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 2, 4 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 0, 5 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 1, 5 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 2, 5 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].s[ 0, 6 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 1, 6 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 2, 6 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 0, 7 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 1, 7 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 2, 7 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 0, 8 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 1, 8 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 2, 8 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 0, 9 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 1, 9 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 2, 9 ]  = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 0, 10 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 1, 10 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 2, 10 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 0, 11 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 1, 11 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 2, 11 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].s[ 0, 12 ] = 0;
                _scalefac[ ch ].s[ 1, 12 ] = 0;
                _scalefac[ ch ].s[ 2, 12 ] = 0;
            }
        }
        else
        {
            // LONG types 0,1,3

            if ( ( _si?.ch[ ch ].scfsi[ 0 ] == 0 ) || ( gr == 0 ) )
            {
                _scalefac[ ch ].l[ 0 ] = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 1 ] = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 2 ] = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 3 ] = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 4 ] = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 5 ] = _br.HGetBits( length0 );
            }

            if ( ( _si.ch[ ch ].scfsi[ 1 ] == 0 ) || ( gr == 0 ) )
            {
                _scalefac[ ch ].l[ 6 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 7 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 8 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 9 ]  = _br.HGetBits( length0 );
                _scalefac[ ch ].l[ 10 ] = _br.HGetBits( length0 );
            }

            if ( ( _si.ch[ ch ].scfsi[ 2 ] == 0 ) || ( gr == 0 ) )
            {
                _scalefac[ ch ].l[ 11 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 12 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 13 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 14 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 15 ] = _br.HGetBits( length1 );
            }

            if ( ( _si.ch[ ch ].scfsi[ 3 ] == 0 ) || ( gr == 0 ) )
            {
                _scalefac[ ch ].l[ 16 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 17 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 18 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 19 ] = _br.HGetBits( length1 );
                _scalefac[ ch ].l[ 20 ] = _br.HGetBits( length1 );
            }

            _scalefac[ ch ].l[ 21 ] = 0;
            _scalefac[ ch ].l[ 22 ] = 0;
        }
    }

    private int[] _newSlen = new int[ 4 ];

    private void GetLsfScaleData( int ch, int gr )
    {
        var modeExt = _header.HModeExtension;
        int blocktypenumber;
        var blocknumber = 0;

        GrInfoS grInfo = _si.ch[ ch ].gr[ gr ];

        var scalefacComp = grInfo.scalefacCompress;

        if ( grInfo.blockType == 2 )
        {
            if ( grInfo.mixedBlockFlag == 0 )
            {
                blocktypenumber = 1;
            }
            else if ( grInfo.mixedBlockFlag == 1 )
            {
                blocktypenumber = 2;
            }
            else
            {
                blocktypenumber = 0;
            }
        }
        else
        {
            blocktypenumber = 0;
        }

        if ( !( ( ( modeExt == 1 ) || ( modeExt == 3 ) ) && ( ch == 1 ) ) )
        {
            if ( scalefacComp < 400 )
            {
                _newSlen[ 0 ]                 = ( scalefacComp >>> 4 ) / 5;
                _newSlen[ 1 ]                 = ( scalefacComp >>> 4 ) % 5;
                _newSlen[ 2 ]                 = ( scalefacComp & 0xF ) >>> 2;
                _newSlen[ 3 ]                 = scalefacComp & 3;
                _si.ch[ ch ].gr[ gr ].preflag = 0;
                blocknumber                   = 0;

            }
            else if ( scalefacComp < 500 )
            {
                _newSlen[ 0 ]                 = ( scalefacComp - 400 >>> 2 ) / 5;
                _newSlen[ 1 ]                 = ( scalefacComp - 400 >>> 2 ) % 5;
                _newSlen[ 2 ]                 = scalefacComp - 400 & 3;
                _newSlen[ 3 ]                 = 0;
                _si.ch[ ch ].gr[ gr ].preflag = 0;
                blocknumber                   = 1;
            }
            else if ( scalefacComp < 512 )
            {
                _newSlen[ 0 ]                 = ( scalefacComp - 500 ) / 3;
                _newSlen[ 1 ]                 = ( scalefacComp - 500 ) % 3;
                _newSlen[ 2 ]                 = 0;
                _newSlen[ 3 ]                 = 0;
                _si.ch[ ch ].gr[ gr ].preflag = 1;
                blocknumber                   = 2;
            }
        }

        if ( ( ( modeExt == 1 ) || ( modeExt == 3 ) ) && ( ch == 1 ) )
        {
            var intScalefacComp = scalefacComp >>> 1;

            if ( intScalefacComp < 180 )
            {
                _newSlen[ 0 ]                 = intScalefacComp / 36;
                _newSlen[ 1 ]                 = intScalefacComp % 36 / 6;
                _newSlen[ 2 ]                 = intScalefacComp % 36 % 6;
                _newSlen[ 3 ]                 = 0;
                _si.ch[ ch ].gr[ gr ].preflag = 0;
                blocknumber                   = 3;
            }
            else if ( intScalefacComp < 244 )
            {
                _newSlen[ 0 ]                 = ( intScalefacComp - 180 & 0x3F ) >>> 4;
                _newSlen[ 1 ]                 = ( intScalefacComp - 180 & 0xF ) >>> 2;
                _newSlen[ 2 ]                 = intScalefacComp - 180 & 3;
                _newSlen[ 3 ]                 = 0;
                _si.ch[ ch ].gr[ gr ].preflag = 0;
                blocknumber                   = 4;
            }
            else if ( intScalefacComp < 255 )
            {
                _newSlen[ 0 ]                 = ( intScalefacComp - 244 ) / 3;
                _newSlen[ 1 ]                 = ( intScalefacComp - 244 ) % 3;
                _newSlen[ 2 ]                 = 0;
                _newSlen[ 3 ]                 = 0;
                _si.ch[ ch ].gr[ gr ].preflag = 0;
                blocknumber                   = 5;
            }
        }

        //TODO: why 45, not 54?
        for ( var x = 0; x < 45; x++ )
        {
            ScalefacBuffer[ x ] = 0;
        }

        var m = 0;

        for ( var i = 0; i < 4; i++ )
        {
            for ( var j = 0; j < nrOfSfbBlock[ blocknumber, blocktypenumber, i ]; j++ )
            {
                ScalefacBuffer[ m ] = _newSlen[ i ] == 0 ? 0 : _br.HGetBits( _newSlen[ i ] );
                m++;
            }
        }
    }

    private void GetLsfScaleFactors( int ch, int gr )
    {
        var     m = 0;
        int     sfb;
        GrInfoS grInfo = _si.ch[ ch ].gr[ gr ];

        GetLsfScaleData( ch, gr );

        if ( grInfo.windowSwitchingFlag != 0 && grInfo.blockType == 2 )
        {
            int window;

            if ( grInfo.mixedBlockFlag != 0 )
            {
                for ( sfb = 0; sfb < 8; sfb++ )
                {
                    _scalefac[ ch ].l[ sfb ] = ScalefacBuffer[ m ];
                    m++;
                }

                for ( sfb = 3; sfb < 12; sfb++ )
                {
                    for ( window = 0; window < 3; window++ )
                    {
                        _scalefac[ ch ].s[ window, sfb ] = ScalefacBuffer[ m ];
                        m++;
                    }
                }

                for ( window = 0; window < 3; window++ )
                {
                    _scalefac[ ch ].s[ window, 12 ] = 0;
                }
            }
            else
            {
                for ( sfb = 0; sfb < 12; sfb++ )
                {
                    for ( window = 0; window < 3; window++ )
                    {
                        _scalefac[ ch ].s[ window, sfb ] = ScalefacBuffer[ m ];
                        m++;
                    }
                }

                for ( window = 0; window < 3; window++ )
                {
                    _scalefac[ ch ].s[ window, 12 ] = 0;
                }
            }
        }
        else
        {
            // LONG types 0,1,3

            for ( sfb = 0; sfb < 21; sfb++ )
            {
                _scalefac[ ch ].l[ sfb ] = ScalefacBuffer[ m ];
                m++;
            }

            _scalefac[ ch ].l[ 21 ] = 0;
            _scalefac[ ch ].l[ 22 ] = 0;
        }
    }

    private int[] _x = { 0 };
    private int[] _y = { 0 };
    private int[] _v = { 0 };
    private int[] _w = { 0 };

    private void HuffmanDecode( int ch, int gr )
    {
        _x[ 0 ] = 0;
        _y[ 0 ] = 0;
        _v[ 0 ] = 0;
        _w[ 0 ] = 0;

        var part23End = _part2Start + _si.ch[ ch ].gr[ gr ].part23Length;
        int region1Start;
        int region2Start;
        int index;

        int buf;
        int buf1;

        HuffCodeTab h;

        // Find region boundary for short block case
        if ( ( _si.ch[ ch ].gr[ gr ].windowSwitchingFlag != 0 )
          && ( _si.ch[ ch ].gr[ gr ].blockType == 2 ) )
        {
            // Region2.
            // MS: Extrahandling for 8KHZ
            region1Start = _sfreq == 8 ? 72 : 36; // sfb[9/3]*3=36 or in case 8KHZ = 72
            region2Start = 576;                   // No Region2 for short block case
        }
        else
        {
            // Find region boundary for long block case

            buf  = _si.ch[ ch ].gr[ gr ].region0Count + 1;
            buf1 = buf + _si.ch[ ch ].gr[ gr ].region1Count + 1;

            if ( buf1 > _sfBandIndex[ _sfreq ].l.Length - 1 )
            {
                buf1 = _sfBandIndex[ _sfreq ].l.Length - 1;
            }

            region1Start = _sfBandIndex[ _sfreq ].l[ buf ];
            region2Start = _sfBandIndex[ _sfreq ].l[ buf1 ]; /* MI */
        }

        index = 0;

        // Read bigvalues area
        for ( var i = 0; i < _si.ch[ ch ].gr[ gr ].bigValues << 1; i += 2 )
        {
            if ( i < region1Start )
            {
                h = HuffCodeTab.ht[ _si.ch[ ch ].gr[ gr ].tableSelect[ 0 ] ];
            }
            else if ( i < region2Start )
            {
                h = HuffCodeTab.ht[ _si.ch[ ch ].gr[ gr ].tableSelect[ 1 ] ];
            }
            else
            {
                h = HuffCodeTab.ht[ _si.ch[ ch ].gr[ gr ].tableSelect[ 2 ] ];
            }

            HuffCodeTab.HuffmanDecoder( h, _x, _y, _v, _w, _br );

            // if (index >= is_1d.Length)
            // System.out.println("i0="+i+"/"+(_si.ch[ch].gr[gr].bigValues<<1)+" Index="+index+" is_1d="+is_1d.Length);

            _is1D[ index++ ] = _x[ 0 ];
            _is1D[ index++ ] = _y[ 0 ];

            _checkSumHuff = _checkSumHuff + _x[ 0 ] + _y[ 0 ];

            // System.out.println("x = "+x[0]+" y = "+y[0]);
        }

        // Read count1 area
        h       = HuffCodeTab.ht[ _si.ch[ ch ].gr[ gr ].count1TableSelect + 32 ];
        var numBits = _br.HSstell();

        while ( numBits < part23End && index < 576 )
        {
            HuffCodeTab.HuffmanDecoder( h, _x, _y, _v, _w, _br );

            _is1D[ index++ ] = _v[ 0 ];
            _is1D[ index++ ] = _w[ 0 ];
            _is1D[ index++ ] = _x[ 0 ];
            _is1D[ index++ ] = _y[ 0 ];

            _checkSumHuff = _checkSumHuff + _v[ 0 ] + _w[ 0 ] + _x[ 0 ] + _y[ 0 ];

            numBits = _br.HSstell();
        }

        if ( numBits > part23End )
        {
            _br.RewindNbits( numBits - part23End );
            index -= 4;
        }

        numBits = _br.HSstell();

        // Dismiss stuffing bits
        if ( numBits < part23End )
        {
            _br.HGetBits( part23End - numBits );
        }

        // Zero out rest

        if ( index < 576 )
        {
            _nonzero[ ch ] = index;
        }
        else
        {
            _nonzero[ ch ] = 576;
        }

        if ( index < 0 )
        {
            index = 0;
        }

        // may not be necessary
        for ( ; index < 576; index++ )
        {
            _is1D[ index ] = 0;
        }
    }

    private void IstereoKValues( int ispos, int io_type, int i )
    {
        if ( ispos == 0 )
        {
            _k[ 0, i ] = 1.0f;
            _k[ 1, i ] = 1.0f;
        }
        else if ( ( ispos & 1 ) != 0 )
        {
            _k[ 0, i ] = io[ io_type, ispos + 1 >>> 1 ];
            _k[ 1, i ] = 1.0f;
        }
        else
        {
            _k[ 0, i ] = 1.0f;
            _k[ 1, i ] = io[ io_type, ispos >>> 1 ];
        }
    }

    private void dequantize_sample( float[ , ] xr, int ch, int gr )
    {
        GrInfoS grInfo = _si.ch[ ch ].gr[ gr ];
        var     cb     = 0;
        int     nextCbBoundary;
        var     cbBegin = 0;
        var     cbWidth = 0;
        var     index   = 0;
        int     j;
        float   gGain;
        var     xr1D = xr;

        // choose correct scalefactor band per block type, initalize boundary

        if ( grInfo.windowSwitchingFlag != 0 && grInfo.blockType == 2 )
        {
            if ( grInfo.mixedBlockFlag != 0 )
            {
                nextCbBoundary = _sfBandIndex[ _sfreq ].l[ 1 ]; // LONG blocks: 0,1,3
            }
            else
            {
                cbWidth        = _sfBandIndex[ _sfreq ].s[ 1 ];
                nextCbBoundary = ( cbWidth << 2 ) - cbWidth;
                cbBegin        = 0;
            }
        }
        else
        {
            nextCbBoundary = _sfBandIndex[ _sfreq ].l[ 1 ]; // LONG blocks: 0,1,3
        }

        // Compute overall (global) scaling.
        gGain = ( float )Math.Pow( 2.0, ( 0.25 * ( grInfo.globalGain - 210.0 ) ) );

        for ( j = 0; j < _nonzero[ ch ]; j++ )
        {
            // Modif E.B 02/22/99
            var reste   = j % SSLIMIT;
            var quotien = ( ( j - reste ) / SSLIMIT );

            if ( is_1d[ j ] == 0 )
            {
                xr1D[ quotien ][ reste ] = 0.0f;
            }
            else
            {
                int abv = is_1d[ j ];

                if ( abv < t_43.Length )
                {
                    if ( is_1d[ j ] > 0 )
                    {
                        xr1D[ quotien ][ reste ] = gGain * t_43[ abv ];
                    }
                    else if ( -abv < t_43.Length )
                    {
                        xr1D[ quotien ][ reste ] = -gGain * t_43[ -abv ];
                    }
                    else
                    {
                        xr1D[ quotien ][ reste ] = -gGain * ( float )Math.pow( -abv, d43 );
                    }
                }
                else if ( is_1d[ j ] > 0 )
                {
                    xr1D[ quotien, reste ] = gGain * ( float )Math.pow( abv, d43 );
                }
                else
                {
                    xr1D[ quotien, reste ] = -gGain * ( float )Math.pow( -abv, d43 );
                }
            }
        }

        // apply formula per block type
        for ( j = 0; j < _nonzero[ ch ]; j++ )
        {
            var reste   = j % SSLIMIT;
            var quotien = ( ( j - reste ) / SSLIMIT );

            if ( index == nextCbBoundary )
            {
                if ( grInfo.windowSwitchingFlag != 0 && grInfo.blockType == 2 )
                {
                    if ( grInfo.mixedBlockFlag != 0 )
                    {
                        if ( index == _sfBandIndex[ _sfreq ].l[ 8 ] )
                        {
                            nextCbBoundary = _sfBandIndex[ _sfreq ].s[ 4 ];
                            nextCbBoundary = ( nextCbBoundary << 2 ) - nextCbBoundary;
                            cb             = 3;
                            cbWidth        = _sfBandIndex[ _sfreq ].s[ 4 ] - _sfBandIndex[ _sfreq ].s[ 3 ];

                            cbBegin = _sfBandIndex[ _sfreq ].s[ 3 ];
                            cbBegin = ( cbBegin << 2 ) - cbBegin;
                        }
                        else if ( index < _sfBandIndex[ _sfreq ].l[ 8 ] )
                        {
                            nextCbBoundary = _sfBandIndex[ _sfreq ].l[ ++cb + 1 ];
                        }
                        else
                        {
                            nextCbBoundary = _sfBandIndex[ _sfreq ].s[ ++cb + 1 ];
                            nextCbBoundary = ( nextCbBoundary << 2 ) - nextCbBoundary;

                            cbBegin = _sfBandIndex[ _sfreq ].s[ cb ];
                            cbWidth = _sfBandIndex[ _sfreq ].s[ cb + 1 ] - cbBegin;
                            cbBegin = ( cbBegin << 2 ) - cbBegin;
                        }
                    }
                    else
                    {
                        nextCbBoundary = _sfBandIndex[ _sfreq ].s[ ++cb + 1 ];
                        nextCbBoundary = ( nextCbBoundary << 2 ) - nextCbBoundary;

                        cbBegin = _sfBandIndex[ _sfreq ].s[ cb ];
                        cbWidth = _sfBandIndex[ _sfreq ].s[ cb + 1 ] - cbBegin;
                        cbBegin = ( cbBegin << 2 ) - cbBegin;
                    }
                }
                else
                {
                    nextCbBoundary = _sfBandIndex[ _sfreq ].l[ ++cb + 1 ];
                }
            }

            // Do long/short dependent scaling operations

            if ( ( grInfo.windowSwitchingFlag != 0 )
              && ( ( grInfo.blockType == 2 )
                && ( ( grInfo.mixedBlockFlag == 0 ) || ( grInfo.blockType == 2 ) )
                && ( grInfo.mixedBlockFlag != 0 )
                && ( j >= 36 ) ) )
            {
                var tIndex = ( index - cbBegin ) / cbWidth;
                var idx    = _scalefac[ ch ].s[ tIndex, cb ] << grInfo.scalefacScale;

                idx += grInfo.subblockGain[ tIndex ] << 2;

                xr1D[ quotien, reste ] *= twoToNegativeHalfPow[ idx ];
            }
            else
            {
                // LONG block types 0,1,3 & 1st 2 subbands of switched blocks
                var idx = _scalefac[ ch ].l[ cb ];

                if ( grInfo.preflag != 0 )
                {
                    idx += pretab[ cb ];
                }

                idx                    =  idx << grInfo.scalefacScale;
                xr1D[ quotien, reste ] *= twoToNegativeHalfPow[ idx ];
            }

            index++;
        }

        for ( j = _nonzero[ ch ]; j < 576; j++ )
        {
            var reste   = j % SSLIMIT;
            var quotien = ( ( j - reste ) / SSLIMIT );

            if ( reste < 0 )
            {
                reste = 0;
            }

            if ( quotien < 0 )
            {
                quotien = 0;
            }

            xr1D[ quotien, reste ] = 0.0f;
        }
    }

    private void Reorder( float[ , ] xr, int ch, int gr )
    {
        GrInfoS grInfo = _si.ch[ ch ].gr[ gr ];
        int     index;

        var xr1D = xr;

        if ( ( grInfo.windowSwitchingFlag != 0 ) && ( grInfo.blockType == 2 ) )
        {
            for ( index = 0; index < 576; index++ )
            {
                _out1D[ index ] = 0.0f;
            }

            if ( grInfo.mixedBlockFlag != 0 )
            {
                // no reorder for low 2 subbands
                for ( index = 0; index < 36; index++ )
                {
                    var reste   = index % SSLIMIT;
                    var quotien = ( ( index - reste ) / SSLIMIT );

                    _out1D[ index ] = xr1D[ quotien, reste ];
                }

                // reordering for rest switched short
                for ( var sfb = 3; sfb < 13; sfb++ )
                {
                    var sfbStart = _sfBandIndex[ _sfreq ].s[ sfb ];
                    var sfbLines = _sfBandIndex[ _sfreq ].s[ sfb + 1 ] - sfbStart;

                    var sfbStart3 = ( sfbStart << 2 ) - sfbStart;

                    for ( int freq = 0, freq3 = 0; freq < sfbLines; freq++, freq3 += 3 )
                    {
                        var srcLine = sfbStart3 + freq;
                        var desLine = sfbStart3 + freq3;

                        var reste   = srcLine % SSLIMIT;
                        var quotien = ( ( srcLine - reste ) / SSLIMIT );

                        _out1D[ desLine ] =  xr1D[ quotien, reste ];
                        srcLine           += sfbLines;
                        desLine++;

                        reste   = srcLine % SSLIMIT;
                        quotien = ( ( srcLine - reste ) / SSLIMIT );

                        _out1D[ desLine ] =  xr1D[ quotien, reste ];
                        srcLine           += sfbLines;
                        desLine++;

                        reste   = srcLine % SSLIMIT;
                        quotien = ( ( srcLine - reste ) / SSLIMIT );

                        _out1D[ desLine ] = xr1D[ quotien, reste ];
                    }
                }
            }
            else
            {
                for ( index = 0; index < 576; index++ )
                {
                    var j       = _reorderTable?[ _sfreq ][ index ];
                    var reste   = j % SSLIMIT;
                    var quotien = ( ( j - reste ) / SSLIMIT );

                    _out1D[ index ] = xr1D[ ( int )quotien!, ( int )reste! ];
                }
            }
        }
        else
        {
            for ( index = 0; index < 576; index++ )
            {
                var reste   = index % SSLIMIT;
                var quotien = ( ( index - reste ) / SSLIMIT );

                _out1D[ index ] = xr1D[ quotien, reste ];
            }
        }
    }

    private int[]   _isPos   = new int[ 576 ];
    private float[] _isRatio = new float[ 576 ];

    private void Stereo( int gr )
    {
        int sb, ss;

        if ( _channels == 1 )
        {
            for ( sb = 0; sb < SBLIMIT; sb++ )
            {
                for ( ss = 0; ss < SSLIMIT; ss += 3 )
                {
                    _lr[ 0, sb, ss ]     = _ro[ 0, sb, ss ];
                    _lr[ 0, sb, ss + 1 ] = _ro[ 0, sb, ss + 1 ];
                    _lr[ 0, sb, ss + 2 ] = _ro[ 0, sb, ss + 2 ];
                }
            }
        }
        else
        {

            GrInfoS grInfo  = _si!.ch[ 0 ].gr[ gr ];
            var     modeExt = _header!.HModeExtension;
            int     i;

            var msStereo = ( _header.HMode == Header.JOINT_STEREO ) && ( ( modeExt & 0x2 ) != 0 );
            var iStereo  = ( _header.HMode == Header.JOINT_STEREO ) && ( ( modeExt & 0x1 ) != 0 );

            var lsf = ( _header.HVersion == Header.MPEG2_LSF )
                   || ( _header.HVersion == Header.MPEG25_LSF );

            var ioType = grInfo.scalefacCompress & 1;

            for ( i = 0; i < 576; i++ )
            {
                _isPos[ i ] = 7;

                _isRatio[ i ] = 0.0f;
            }

            if ( iStereo )
            {
                int sfb;

                if ( ( grInfo.windowSwitchingFlag != 0 ) && ( grInfo.blockType == 2 ) )
                {
                    int temp;

                    if ( grInfo.mixedBlockFlag != 0 )
                    {
                        var maxSfb = 0;

                        for ( var j = 0; j < 3; j++ )
                        {
                            var sfbcnt = 2;

                            for ( sfb = 12; sfb >= 3; sfb-- )
                            {
                                i = _sfBandIndex[ _sfreq ].s[ sfb ];
                                var lines = _sfBandIndex[ _sfreq ].s[ sfb + 1 ] - i;
                                i = ( ( ( i << 2 ) - i ) + ( ( j + 1 ) * lines ) ) - 1;

                                while ( lines > 0 )
                                {
                                    if ( _ro[ 1, i / 18, i % 18 ] != 0.0f )
                                    {
                                        sfbcnt = sfb;
                                        sfb    = -10;
                                        lines  = -10;
                                    }

                                    lines--;
                                    i--;
                                }
                            }

                            sfb = sfbcnt + 1;

                            if ( sfb > maxSfb )
                            {
                                maxSfb = sfb;
                            }

                            while ( sfb < 12 )
                            {
                                temp = _sfBandIndex[ _sfreq ].s[ sfb ];
                                sb   = _sfBandIndex[ _sfreq ].s[ sfb + 1 ] - temp;
                                i    = ( ( temp << 2 ) - temp ) + ( j * sb );

                                for ( ; sb > 0; sb-- )
                                {
                                    _isPos[ i ] = _scalefac[ 1 ].s[ j, sfb ];

                                    if ( _isPos[ i ] != 7 )
                                    {
                                        if ( lsf )
                                        {
                                            IstereoKValues( _isPos[ i ], ioType, i );
                                        }
                                        else
                                        {
                                            _isRatio[ i ] = tan12[ _isPos[ i ] ];
                                        }
                                    }

                                    i++;
                                }

                                sfb++;
                            }

                            sfb = _sfBandIndex[ _sfreq ].s[ 10 ];
                            sb  = _sfBandIndex[ _sfreq ].s[ 11 ] - sfb;

                            sfb  = ( ( sfb << 2 ) - sfb ) + ( j * sb );
                            temp = _sfBandIndex[ _sfreq ].s[ 11 ];
                            sb   = _sfBandIndex[ _sfreq ].s[ 12 ] - temp;
                            i    = ( ( temp << 2 ) - temp ) + ( j * sb );

                            for ( ; sb > 0; sb-- )
                            {
                                _isPos[ i ] = _isPos[ sfb ];

                                if ( lsf )
                                {
                                    _k[ 0, i ] = _k[ 0, sfb ];
                                    _k[ 1, i ] = _k[ 1, sfb ];
                                }
                                else
                                {
                                    _isRatio[ i ] = _isRatio[ sfb ];
                                }

                                i++;
                            }
                        }

                        if ( maxSfb <= 3 )
                        {
                            i  = 2;
                            ss = 17;
                            sb = -1;

                            while ( i >= 0 )
                            {
                                if ( _ro[ 1, i, ss ] != 0.0f )
                                {
                                    sb = ( i << 4 ) + ( i << 1 ) + ss;
                                    i  = -1;
                                }
                                else
                                {
                                    ss--;

                                    if ( ss < 0 )
                                    {
                                        i--;
                                        ss = 17;
                                    }
                                }
                            }

                            i = 0;

                            while ( _sfBandIndex[ _sfreq ].l[ i ] <= sb )
                            {
                                i++;
                            }

                            sfb = i;
                            i   = _sfBandIndex[ _sfreq ].l[ i ];

                            for ( ; sfb < 8; sfb++ )
                            {
                                sb = _sfBandIndex[ _sfreq ].l[ sfb + 1 ] - _sfBandIndex[ _sfreq ].l[ sfb ];

                                for ( ; sb > 0; sb-- )
                                {
                                    _isPos[ i ] = _scalefac[ 1 ].l[ sfb ];

                                    if ( _isPos[ i ] != 7 )
                                    {
                                        if ( lsf )
                                        {
                                            IstereoKValues( _isPos[ i ], ioType, i );
                                        }
                                        else
                                        {
                                            _isRatio[ i ] = tan12[ _isPos[ i ] ];
                                        }
                                    }

                                    i++;
                                }
                            }
                        }
                    }
                    else
                    {
                        for ( var j = 0; j < 3; j++ )
                        {
                            var sfbcnt = -1;

                            for ( sfb = 12; sfb >= 0; sfb-- )
                            {
                                temp = _sfBandIndex[ _sfreq ].s[ sfb ];
                                var lines = _sfBandIndex[ _sfreq ].s[ sfb + 1 ] - temp;
                                i = ( ( ( temp << 2 ) - temp ) + ( ( j + 1 ) * lines ) ) - 1;

                                while ( lines > 0 )
                                {
                                    if ( _ro[ 1, i / 18, i % 18 ] != 0.0f )
                                    {
                                        sfbcnt = sfb;
                                        sfb    = -10;
                                        lines  = -10;
                                    }

                                    lines--;
                                    i--;
                                }
                            }

                            sfb = sfbcnt + 1;

                            while ( sfb < 12 )
                            {
                                temp = _sfBandIndex[ _sfreq ].s[ sfb ];
                                sb   = _sfBandIndex[ _sfreq ].s[ sfb + 1 ] - temp;
                                i    = ( ( temp << 2 ) - temp ) + ( j * sb );

                                for ( ; sb > 0; sb-- )
                                {
                                    _isPos[ i ] = _scalefac[ 1 ].s[ j, sfb ];

                                    if ( _isPos[ i ] != 7 )
                                    {
                                        if ( lsf )
                                        {
                                            IstereoKValues( _isPos[ i ], ioType, i );
                                        }
                                        else
                                        {
                                            _isRatio[ i ] = tan12[ _isPos[ i ] ];
                                        }
                                    }

                                    i++;
                                }

                                sfb++;
                            }

                            temp = _sfBandIndex[ _sfreq ].s[ 10 ];
                            var temp2 = _sfBandIndex[ _sfreq ].s[ 11 ];
                            sb  = temp2 - temp;
                            sfb = ( ( temp << 2 ) - temp ) + ( j * sb );
                            sb  = _sfBandIndex[ _sfreq ].s[ 12 ] - temp2;
                            i   = ( ( temp2 << 2 ) - temp2 ) + ( j * sb );

                            for ( ; sb > 0; sb-- )
                            {
                                _isPos[ i ] = _isPos[ sfb ];

                                if ( lsf )
                                {
                                    _k[ 0, i ] = _k[ 0, sfb ];
                                    _k[ 1, i ] = _k[ 1, sfb ];
                                }
                                else
                                {
                                    _isRatio[ i ] = _isRatio[ sfb ];
                                }

                                i++;
                            }
                        }
                    }
                }
                else
                {
                    i  = 31;
                    ss = 17;
                    sb = 0;

                    while ( i >= 0 )
                    {
                        if ( _ro[ 1, i, ss ] != 0.0f )
                        {
                            sb = ( i << 4 ) + ( i << 1 ) + ss;
                            i  = -1;
                        }
                        else
                        {
                            ss--;

                            if ( ss < 0 )
                            {
                                i--;
                                ss = 17;
                            }
                        }
                    }

                    i = 0;

                    while ( _sfBandIndex[ _sfreq ].l[ i ] <= sb )
                    {
                        i++;
                    }

                    sfb = i;
                    i   = _sfBandIndex[ _sfreq ].l[ i ];

                    for ( ; sfb < 21; sfb++ )
                    {
                        sb = _sfBandIndex[ _sfreq ].l[ sfb + 1 ] - _sfBandIndex[ _sfreq ].l[ sfb ];

                        for ( ; sb > 0; sb-- )
                        {
                            _isPos[ i ] = _scalefac[ 1 ].l[ sfb ];

                            if ( _isPos[ i ] != 7 )
                            {
                                if ( lsf )
                                {
                                    IstereoKValues( _isPos[ i ], ioType, i );
                                }
                                else
                                {
                                    _isRatio[ i ] = tan12[ _isPos[ i ] ];
                                }
                            }

                            i++;
                        }
                    }

                    sfb = _sfBandIndex[ _sfreq ].l[ 20 ];

                    for ( sb = 576 - _sfBandIndex[ _sfreq ].l[ 21 ]; ( sb > 0 ) && ( i < 576 ); sb-- )
                    {
                        _isPos[ i ] = _isPos[ sfb ]; // error here : i >=576

                        if ( lsf )
                        {
                            _k[ 0, i ] = _k[ 0, sfb ];
                            _k[ 1, i ] = _k[ 1, sfb ];
                        }
                        else
                        {
                            _isRatio[ i ] = _isRatio[ sfb ];
                        }

                        i++;
                    }
                }
            }

            i = 0;

            for ( sb = 0; sb < SBLIMIT; sb++ )
            {
                for ( ss = 0; ss < SSLIMIT; ss++ )
                {
                    if ( _isPos[ i ] == 7 )
                    {
                        if ( msStereo )
                        {
                            _lr[ 0, sb, ss ] = ( _ro[ 0, sb, ss ] + _ro[ 1, sb, ss ] ) * 0.707106781f;
                            _lr[ 1, sb, ss ] = ( _ro[ 0, sb, ss ] - _ro[ 1, sb, ss ] ) * 0.707106781f;
                        }
                        else
                        {
                            _lr[ 0, sb, ss ] = _ro[ 0, sb, ss ];
                            _lr[ 1, sb, ss ] = _ro[ 1, sb, ss ];
                        }
                    }
                    else if ( iStereo )
                    {
                        if ( lsf )
                        {
                            _lr[ 0, sb, ss ] = _ro[ 0, sb, ss ] * _k[ 0, i ];
                            _lr[ 1, sb, ss ] = _ro[ 0, sb, ss ] * _k[ 1, i ];
                        }
                        else
                        {
                            _lr[ 1, sb, ss ] = _ro[ 0, sb, ss ] / ( 1 + _isRatio[ i ] );
                            _lr[ 0, sb, ss ] = _lr[ 1, sb, ss ] * _isRatio[ i ];
                        }
                    }

                    i++;
                }
            }
        }
    }

    private void Antialias( int ch, int gr )
    {
        int      sb18;
        int      sb18Lim;
        GrInfoS? grInfo = _si?.ch[ ch ].gr[ gr ];

        Debug.Assert( grInfo != null );

        // 31 alias-reduction operations between each pair of sub-bands
        // with 8 butterflies between each pair

        if ( ( grInfo.windowSwitchingFlag != 0 )
          && grInfo is { blockType: 2, mixedBlockFlag: 0 } )
        {
            return;
        }

        if ( ( grInfo.windowSwitchingFlag != 0 )
          && ( grInfo.mixedBlockFlag != 0 )
          && ( grInfo.blockType == 2 ) )
        {
            sb18Lim = 18;
        }
        else
        {
            sb18Lim = 558;
        }

        for ( sb18 = 0; sb18 < sb18Lim; sb18 += 18 )
        {
            for ( var ss = 0; ss < 8; ss++ )
            {
                var srcIdx1 = ( sb18 + 17 ) - ss;
                var srcIdx2 = sb18 + 18 + ss;
                var bu      = _out1D[ srcIdx1 ];
                var bd      = _out1D[ srcIdx2 ];

                _out1D[ srcIdx1 ] = ( bu * _cs[ ss ] ) - ( bd * _ca[ ss ] );
                _out1D[ srcIdx2 ] = ( bd * _cs[ ss ] ) + ( bu * _ca[ ss ] );
            }
        }
    }

    private float[] _tsOutCopy = new float[ 18 ];
    private float[] _rawout    = new float[ 36 ];

    private void Hybrid( int ch, int gr )
    {
        int      sb18;
        GrInfoS? grInfo = _si?.ch[ ch ].gr[ gr ];

        for ( sb18 = 0; sb18 < 576; sb18 += 18 )
        {
            var bt = ( grInfo?.windowSwitchingFlag != 0 )
                  && ( grInfo?.mixedBlockFlag != 0 )
                  && ( sb18 < 36 )
                ? 0
                : grInfo?.blockType;

            var tsOut = _out1D;

            for ( var cc = 0; cc < 18; cc++ )
            {
                _tsOutCopy[ cc ] = tsOut[ cc + sb18 ];
            }

            InvMdct( _tsOutCopy, _rawout, ( int )bt! );

            for ( var cc = 0; cc < 18; cc++ )
            {
                tsOut[ cc + sb18 ] = _tsOutCopy[ cc ];
            }

            var prvblk = _prevblck;

            tsOut[ 0 + sb18 ]      = _rawout[ 0 ] + prvblk[ ch, sb18 + 0 ];
            prvblk[ ch, sb18 + 0 ] = _rawout[ 18 ];

            tsOut[ 1 + sb18 ]      = _rawout[ 1 ] + prvblk[ ch, sb18 + 1 ];
            prvblk[ ch, sb18 + 1 ] = _rawout[ 19 ];

            tsOut[ 2 + sb18 ]      = _rawout[ 2 ] + prvblk[ ch, sb18 + 2 ];
            prvblk[ ch, sb18 + 2 ] = _rawout[ 20 ];

            tsOut[ 3 + sb18 ]      = _rawout[ 3 ] + prvblk[ ch, sb18 + 3 ];
            prvblk[ ch, sb18 + 3 ] = _rawout[ 21 ];

            tsOut[ 4 + sb18 ]      = _rawout[ 4 ] + prvblk[ ch, sb18 + 4 ];
            prvblk[ ch, sb18 + 4 ] = _rawout[ 22 ];

            tsOut[ 5 + sb18 ]      = _rawout[ 5 ] + prvblk[ ch, sb18 + 5 ];
            prvblk[ ch, sb18 + 5 ] = _rawout[ 23 ];

            tsOut[ 6 + sb18 ]      = _rawout[ 6 ] + prvblk[ ch, sb18 + 6 ];
            prvblk[ ch, sb18 + 6 ] = _rawout[ 24 ];

            tsOut[ 7 + sb18 ]      = _rawout[ 7 ] + prvblk[ ch, sb18 + 7 ];
            prvblk[ ch, sb18 + 7 ] = _rawout[ 25 ];

            tsOut[ 8 + sb18 ]      = _rawout[ 8 ] + prvblk[ ch, sb18 + 8 ];
            prvblk[ ch, sb18 + 8 ] = _rawout[ 26 ];

            tsOut[ 9 + sb18 ]      = _rawout[ 9 ] + prvblk[ ch, sb18 + 9 ];
            prvblk[ ch, sb18 + 9 ] = _rawout[ 27 ];

            tsOut[ 10 + sb18 ]      = _rawout[ 10 ] + prvblk[ ch, sb18 + 10 ];
            prvblk[ ch, sb18 + 10 ] = _rawout[ 28 ];

            tsOut[ 11 + sb18 ]      = _rawout[ 11 ] + prvblk[ ch, sb18 + 11 ];
            prvblk[ ch, sb18 + 11 ] = _rawout[ 29 ];

            tsOut[ 12 + sb18 ]      = _rawout[ 12 ] + prvblk[ ch, sb18 + 12 ];
            prvblk[ ch, sb18 + 12 ] = _rawout[ 30 ];

            tsOut[ 13 + sb18 ]      = _rawout[ 13 ] + prvblk[ ch, sb18 + 13 ];
            prvblk[ ch, sb18 + 13 ] = _rawout[ 31 ];

            tsOut[ 14 + sb18 ]      = _rawout[ 14 ] + prvblk[ ch, sb18 + 14 ];
            prvblk[ ch, sb18 + 14 ] = _rawout[ 32 ];

            tsOut[ 15 + sb18 ]      = _rawout[ 15 ] + prvblk[ ch, sb18 + 15 ];
            prvblk[ ch, sb18 + 15 ] = _rawout[ 33 ];

            tsOut[ 16 + sb18 ]      = _rawout[ 16 ] + prvblk[ ch, sb18 + 16 ];
            prvblk[ ch, sb18 + 16 ] = _rawout[ 34 ];

            tsOut[ 17 + sb18 ]      = _rawout[ 17 ] + prvblk[ ch, sb18 + 17 ];
            prvblk[ ch, sb18 + 17 ] = _rawout[ 35 ];
        }
    }

    private void DoDownmix()
    {
        for ( var sb = 0; sb < SSLIMIT; sb++ )
        {
            for ( var ss = 0; ss < SSLIMIT; ss += 3 )
            {
                _lr[ 0, sb, ss ]     = ( _lr[ 0, sb, ss ] + _lr[ 1, sb, ss ] ) * 0.5f;
                _lr[ 0, sb, ss + 1 ] = ( _lr[ 0, sb, ss + 1 ] + _lr[ 1, sb, ss + 1 ] ) * 0.5f;
                _lr[ 0, sb, ss + 2 ] = ( _lr[ 0, sb, ss + 2 ] + _lr[ 1, sb, ss + 2 ] ) * 0.5f;
            }
        }
    }

    public void InvMdct( float[] inArray, float[] outArray, int blockType )
    {
        float tmpf0, tmpf1, tmpf2, tmpf3,  tmpf4, tmpf5, tmpf6;
        float tmpf7, tmpf8, tmpf9, tmpf10, tmpf11;

        if ( blockType == 2 )
        {
            outArray[ 0 ]  = 0.0f;
            outArray[ 1 ]  = 0.0f;
            outArray[ 2 ]  = 0.0f;
            outArray[ 3 ]  = 0.0f;
            outArray[ 4 ]  = 0.0f;
            outArray[ 5 ]  = 0.0f;
            outArray[ 6 ]  = 0.0f;
            outArray[ 7 ]  = 0.0f;
            outArray[ 8 ]  = 0.0f;
            outArray[ 9 ]  = 0.0f;
            outArray[ 10 ] = 0.0f;
            outArray[ 11 ] = 0.0f;
            outArray[ 12 ] = 0.0f;
            outArray[ 13 ] = 0.0f;
            outArray[ 14 ] = 0.0f;
            outArray[ 15 ] = 0.0f;
            outArray[ 16 ] = 0.0f;
            outArray[ 17 ] = 0.0f;
            outArray[ 18 ] = 0.0f;
            outArray[ 19 ] = 0.0f;
            outArray[ 20 ] = 0.0f;
            outArray[ 21 ] = 0.0f;
            outArray[ 22 ] = 0.0f;
            outArray[ 23 ] = 0.0f;
            outArray[ 24 ] = 0.0f;
            outArray[ 25 ] = 0.0f;
            outArray[ 26 ] = 0.0f;
            outArray[ 27 ] = 0.0f;
            outArray[ 28 ] = 0.0f;
            outArray[ 29 ] = 0.0f;
            outArray[ 30 ] = 0.0f;
            outArray[ 31 ] = 0.0f;
            outArray[ 32 ] = 0.0f;
            outArray[ 33 ] = 0.0f;
            outArray[ 34 ] = 0.0f;
            outArray[ 35 ] = 0.0f;

            var sixI = 0;

            for ( var i = 0; i < 3; i++ )
            {
                // 12 point IMDCT
                // Begin 12 point IDCT
                // Input aliasing for 12 pt IDCT
                inArray[ 15 + i ] += inArray[ 12 + i ];
                inArray[ 12 + i ] += inArray[ 9 + i ];
                inArray[ 9 + i ]  += inArray[ 6 + i ];
                inArray[ 6 + i ]  += inArray[ 3 + i ];
                inArray[ 3 + i ]  += inArray[ 0 + i ];

                // Input aliasing on odd indices (for 6 point IDCT)
                inArray[ 15 + i ] += inArray[ 9 + i ];
                inArray[ 9 + i ]  += inArray[ 3 + i ];

                // 3 point IDCT on even indices
                var pp2 = inArray[ 12 + i ] * 0.500000000f;
                var pp1 = inArray[ 6 + i ] * 0.866025403f;
                var sum = inArray[ 0 + i ] + pp2;

                tmpf1 = inArray[ 0 + i ] - inArray[ 12 + i ];
                tmpf0 = sum + pp1;
                tmpf2 = sum - pp1;

                // End 3 point IDCT on even indices
                // 3 point IDCT on odd indices (for 6 point IDCT)
                pp2   = inArray[ 15 + i ] * 0.500000000f;
                pp1   = inArray[ 9 + i ] * 0.866025403f;
                sum   = inArray[ 3 + i ] + pp2;
                tmpf4 = inArray[ 3 + i ] - inArray[ 15 + i ];
                tmpf5 = sum + pp1;
                tmpf3 = sum - pp1;

                // End 3 point IDCT on odd indices
                // Twiddle factors on odd indices (for 6 point IDCT)
                tmpf3 *= 1.931851653f;
                tmpf4 *= 0.707106781f;
                tmpf5 *= 0.517638090f;

                // Output butterflies on 2 3 point IDCT's (for 6 point IDCT)
                var save = tmpf0;
                tmpf0 += tmpf5;
                tmpf5 =  save - tmpf5;
                save  =  tmpf1;
                tmpf1 += tmpf4;
                tmpf4 =  save - tmpf4;
                save  =  tmpf2;
                tmpf2 += tmpf3;
                tmpf3 =  save - tmpf3;

                // End 6 point IDCT
                // Twiddle factors on indices (for 12 point IDCT)
                tmpf0 *= 0.504314480f;
                tmpf1 *= 0.541196100f;
                tmpf2 *= 0.630236207f;
                tmpf3 *= 0.821339815f;
                tmpf4 *= 1.306562965f;
                tmpf5 *= 3.830648788f;

                // End 12 point IDCT

                // Shift to 12 point modified IDCT, multiply by window type 2
                tmpf8  = -tmpf0 * 0.793353340f;
                tmpf9  = -tmpf0 * 0.608761429f;
                tmpf7  = -tmpf1 * 0.923879532f;
                tmpf10 = -tmpf1 * 0.382683432f;
                tmpf6  = -tmpf2 * 0.991444861f;
                tmpf11 = -tmpf2 * 0.130526192f;

                tmpf0 = tmpf3;
                tmpf1 = tmpf4 * 0.382683432f;
                tmpf2 = tmpf5 * 0.608761429f;

                tmpf3 = -tmpf5 * 0.793353340f;
                tmpf4 = -tmpf4 * 0.923879532f;
                tmpf5 = -tmpf0 * 0.991444861f;

                tmpf0 *= 0.130526192f;

                outArray[ sixI + 6 ]  += tmpf0;
                outArray[ sixI + 7 ]  += tmpf1;
                outArray[ sixI + 8 ]  += tmpf2;
                outArray[ sixI + 9 ]  += tmpf3;
                outArray[ sixI + 10 ] += tmpf4;
                outArray[ sixI + 11 ] += tmpf5;
                outArray[ sixI + 12 ] += tmpf6;
                outArray[ sixI + 13 ] += tmpf7;
                outArray[ sixI + 14 ] += tmpf8;
                outArray[ sixI + 15 ] += tmpf9;
                outArray[ sixI + 16 ] += tmpf10;
                outArray[ sixI + 17 ] += tmpf11;

                sixI += 6;
            }
        }
        else
        {
            // 36 point IDCT
            // input aliasing for 36 point IDCT
            inArray[ 17 ] += inArray[ 16 ];
            inArray[ 16 ] += inArray[ 15 ];
            inArray[ 15 ] += inArray[ 14 ];
            inArray[ 14 ] += inArray[ 13 ];
            inArray[ 13 ] += inArray[ 12 ];
            inArray[ 12 ] += inArray[ 11 ];
            inArray[ 11 ] += inArray[ 10 ];
            inArray[ 10 ] += inArray[ 9 ];
            inArray[ 9 ]  += inArray[ 8 ];
            inArray[ 8 ]  += inArray[ 7 ];
            inArray[ 7 ]  += inArray[ 6 ];
            inArray[ 6 ]  += inArray[ 5 ];
            inArray[ 5 ]  += inArray[ 4 ];
            inArray[ 4 ]  += inArray[ 3 ];
            inArray[ 3 ]  += inArray[ 2 ];
            inArray[ 2 ]  += inArray[ 1 ];
            inArray[ 1 ]  += inArray[ 0 ];

            // 18 point IDCT for odd indices
            // input aliasing for 18 point IDCT
            inArray[ 17 ] += inArray[ 15 ];
            inArray[ 15 ] += inArray[ 13 ];
            inArray[ 13 ] += inArray[ 11 ];
            inArray[ 11 ] += inArray[ 9 ];
            inArray[ 9 ]  += inArray[ 7 ];
            inArray[ 7 ]  += inArray[ 5 ];
            inArray[ 5 ]  += inArray[ 3 ];
            inArray[ 3 ]  += inArray[ 1 ];

            // ----------------------------------------------------------------
            // Fast 9 Point Inverse Discrete Cosine Transform
            //
            // By Francois-Raymond Boyer
            // mailto:boyerf@iro.umontreal.ca
            // http://www.iro.umontreal.ca/~boyerf
            //
            // The code has been optimized for Intel processors
            // (takes a lot of time to convert float to and from iternal FPU representation)
            //
            // It is a simple "factorization" of the IDCT matrix.
            // ----------------------------------------------------------------

            // 9 point IDCT on even indices

            // 5 points on odd indices (not realy an IDCT)
            var i00 = inArray[ 0 ] + inArray[ 0 ];

            var iip12 = i00 + inArray[ 12 ];

            var tmp0 = iip12
                     + ( inArray[ 4 ] * 1.8793852415718f )
                     + ( inArray[ 8 ] * 1.532088886238f )
                     + ( inArray[ 16 ] * 0.34729635533386f );

            var tmp1 = ( i00 + inArray[ 4 ] )
                     - inArray[ 8 ]
                     - inArray[ 12 ]
                     - inArray[ 12 ]
                     - inArray[ 16 ];

            var tmp2 = ( iip12 - ( inArray[ 4 ] * 0.34729635533386f ) - ( inArray[ 8 ] * 1.8793852415718f ) ) + ( inArray[ 16 ] * 1.532088886238f );

            var tmp3 = ( ( iip12 - ( inArray[ 4 ] * 1.532088886238f ) ) + ( inArray[ 8 ] * 0.34729635533386f ) ) - ( inArray[ 16 ] * 1.8793852415718f );

            var tmp4 = ( ( ( inArray[ 0 ] - inArray[ 4 ] ) + inArray[ 8 ] ) - inArray[ 12 ] ) + inArray[ 16 ];

            // 4 points on even indices
            var i66 = inArray[ 6 ] * 1.732050808f; // Sqrt[3]

            var tmp5 = ( inArray[ 2 ] * 1.9696155060244f )
                     + i66
                     + ( inArray[ 10 ] * 1.2855752193731f )
                     + ( inArray[ 14 ] * 0.68404028665134f );

            var tmp6 = ( inArray[ 2 ] - inArray[ 10 ] - inArray[ 14 ] ) * 1.732050808f;

            var tmp7 = ( ( inArray[ 2 ] * 1.2855752193731f )
                       - i66
                       - ( inArray[ 10 ] * 0.68404028665134f ) )
                     + ( inArray[ 14 ] * 1.9696155060244f );

            var tmp8 = ( ( ( inArray[ 2 ] * 0.68404028665134f ) - i66 )
                       + ( inArray[ 10 ] * 1.9696155060244f ) )
                     - ( inArray[ 14 ] * 1.2855752193731f );

            // 9 point IDCT on odd indices
            // 5 points on odd indices (not realy an IDCT)
            var i0    = inArray[ 0 + 1 ] + inArray[ 0 + 1 ];
            var i0P12 = i0 + inArray[ 12 + 1 ];

            var tmp9 = i0P12
                     + ( inArray[ 4 + 1 ] * 1.8793852415718f )
                     + ( inArray[ 8 + 1 ] * 1.532088886238f )
                     + ( inArray[ 16 + 1 ] * 0.34729635533386f );

            var tmp10 = ( i0 + inArray[ 4 + 1 ] )
                      - inArray[ 8 + 1 ]
                      - inArray[ 12 + 1 ]
                      - inArray[ 12 + 1 ]
                      - inArray[ 16 + 1 ];

            var tmp11 = ( i0P12
                        - ( inArray[ 4 + 1 ] * 0.34729635533386f )
                        - ( inArray[ 8 + 1 ] * 1.8793852415718f ) )
                      + ( inArray[ 16 + 1 ] * 1.532088886238f );

            var tmp12 = ( ( i0P12 - ( inArray[ 4 + 1 ] * 1.532088886238f ) )
                        + ( inArray[ 8 + 1 ] * 0.34729635533386f ) )
                      - ( inArray[ 16 + 1 ] * 1.8793852415718f );

            var tmp13 = ( ( ( ( inArray[ 0 + 1 ] - inArray[ 4 + 1 ] )
                            + inArray[ 8 + 1 ] )
                          - inArray[ 12 + 1 ] )
                        + inArray[ 16 + 1 ] )
                      * 0.707106781f;

            // 4 points on even indices
            var i6 = inArray[ 6 + 1 ] * 1.732050808f; // Sqrt[3]

            var tmp14 = ( inArray[ 2 + 1 ] * 1.9696155060244f )
                      + i6
                      + ( inArray[ 10 + 1 ] * 1.2855752193731f )
                      + ( inArray[ 14 + 1 ] * 0.68404028665134f );

            var tmp15 = ( inArray[ 2 + 1 ] - inArray[ 10 + 1 ] - inArray[ 14 + 1 ] ) * 1.732050808f;

            var tmp16 = ( ( inArray[ 2 + 1 ] * 1.2855752193731f )
                        - i6
                        - ( inArray[ 10 + 1 ] * 0.68404028665134f ) )
                      + ( inArray[ 14 + 1 ] * 1.9696155060244f );

            var tmp17 = ( ( ( inArray[ 2 + 1 ] * 0.68404028665134f ) - i6 )
                        + ( inArray[ 10 + 1 ] * 1.9696155060244f ) )
                      - ( inArray[ 14 + 1 ] * 1.2855752193731f );

            // Twiddle factors on odd indices
            // and
            // Butterflies on 9 point IDCT's
            // and
            // twiddle factors for 36 point IDCT

            var e = tmp0 + tmp5;
            var o = ( tmp9 + tmp14 ) * 0.501909918f;
            tmpf0 = e + o;

            var tmpf17 = e - o;
            e     = tmp1 + tmp6;
            o     = ( tmp10 + tmp15 ) * 0.517638090f;
            tmpf1 = e + o;

            var tmpf16 = e - o;
            e     = tmp2 + tmp7;
            o     = ( tmp11 + tmp16 ) * 0.551688959f;
            tmpf2 = e + o;

            var tmpf15 = e - o;
            e     = tmp3 + tmp8;
            o     = ( tmp12 + tmp17 ) * 0.610387294f;
            tmpf3 = e + o;

            var tmpf14 = e - o;
            tmpf4 = tmp4 + tmp13;

            var tmpf13 = tmp4 - tmp13;
            e     = tmp3 - tmp8;
            o     = ( tmp12 - tmp17 ) * 0.871723397f;
            tmpf5 = e + o;

            var tmpf12 = e - o;
            e      = tmp2 - tmp7;
            o      = ( tmp11 - tmp16 ) * 1.183100792f;
            tmpf6  = e + o;
            tmpf11 = e - o;
            e      = tmp1 - tmp6;
            o      = ( tmp10 - tmp15 ) * 1.931851653f;
            tmpf7  = e + o;
            tmpf10 = e - o;
            e      = tmp0 - tmp5;
            o      = ( tmp9 - tmp14 ) * 5.736856623f;
            tmpf8  = e + o;
            tmpf9  = e - o;

            // end 36 point IDCT */
            // shift to modified IDCT

            outArray[ 0 ]  = -tmpf9 * Win[ blockType, 0 ];
            outArray[ 1 ]  = -tmpf10 * Win[ blockType, 1 ];
            outArray[ 2 ]  = -tmpf11 * Win[ blockType, 2 ];
            outArray[ 3 ]  = -tmpf12 * Win[ blockType, 3 ];
            outArray[ 4 ]  = -tmpf13 * Win[ blockType, 4 ];
            outArray[ 5 ]  = -tmpf14 * Win[ blockType, 5 ];
            outArray[ 6 ]  = -tmpf15 * Win[ blockType, 6 ];
            outArray[ 7 ]  = -tmpf16 * Win[ blockType, 7 ];
            outArray[ 8 ]  = -tmpf17 * Win[ blockType, 8 ];
            outArray[ 9 ]  = tmpf17 * Win[ blockType, 9 ];
            outArray[ 10 ] = tmpf16 * Win[ blockType, 10 ];
            outArray[ 11 ] = tmpf15 * Win[ blockType, 11 ];
            outArray[ 12 ] = tmpf14 * Win[ blockType, 12 ];
            outArray[ 13 ] = tmpf13 * Win[ blockType, 13 ];
            outArray[ 14 ] = tmpf12 * Win[ blockType, 14 ];
            outArray[ 15 ] = tmpf11 * Win[ blockType, 15 ];
            outArray[ 16 ] = tmpf10 * Win[ blockType, 16 ];
            outArray[ 17 ] = tmpf9 * Win[ blockType, 17 ];
            outArray[ 18 ] = tmpf8 * Win[ blockType, 18 ];
            outArray[ 19 ] = tmpf7 * Win[ blockType, 19 ];
            outArray[ 20 ] = tmpf6 * Win[ blockType, 20 ];
            outArray[ 21 ] = tmpf5 * Win[ blockType, 21 ];
            outArray[ 22 ] = tmpf4 * Win[ blockType, 22 ];
            outArray[ 23 ] = tmpf3 * Win[ blockType, 23 ];
            outArray[ 24 ] = tmpf2 * Win[ blockType, 24 ];
            outArray[ 25 ] = tmpf1 * Win[ blockType, 25 ];
            outArray[ 26 ] = tmpf0 * Win[ blockType, 26 ];
            outArray[ 27 ] = tmpf0 * Win[ blockType, 27 ];
            outArray[ 28 ] = tmpf1 * Win[ blockType, 28 ];
            outArray[ 29 ] = tmpf2 * Win[ blockType, 29 ];
            outArray[ 30 ] = tmpf3 * Win[ blockType, 30 ];
            outArray[ 31 ] = tmpf4 * Win[ blockType, 31 ];
            outArray[ 32 ] = tmpf5 * Win[ blockType, 32 ];
            outArray[ 33 ] = tmpf6 * Win[ blockType, 33 ];
            outArray[ 34 ] = tmpf7 * Win[ blockType, 34 ];
            outArray[ 35 ] = tmpf8 * Win[ blockType, 35 ];
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class SBI
    {
        public int[] l;
        public int[] s;

        public SBI()
        {
            l = new int[ 23 ];
            s = new int[ 14 ];
        }

        public SBI( int[] thel, int[] thes )
        {
            l = thel;
            s = thes;
        }
    }

    [PublicAPI]
    public record GrInfoS
    {
        public int   part23Length        = 0;
        public int   bigValues           = 0;
        public int   globalGain          = 0;
        public int   scalefacCompress    = 0;
        public int   windowSwitchingFlag = 0;
        public int   blockType           = 0;
        public int   mixedBlockFlag      = 0;
        public int[] tableSelect         = new int[ 3 ];
        public int[] subblockGain        = new int[ 3 ];
        public int   region0Count        = 0;
        public int   region1Count        = 0;
        public int   preflag             = 0;
        public int   scalefacScale       = 0;
        public int   count1TableSelect   = 0;
    }

    [PublicAPI]
    public record Temporary
    {
        public int[]     scfsi;
        public GrInfoS[] gr;

        public Temporary()
        {
            scfsi   = new int[ 4 ];
            gr      = new GrInfoS[ 2 ];
            gr[ 0 ] = new GrInfoS();
            gr[ 1 ] = new GrInfoS();
        }
    }

    [PublicAPI]
    public record IIISideInfoT
    {
        public int         mainDataBegin = 0;
        public int         privateBits   = 0;
        public Temporary[] ch;

        public IIISideInfoT()
        {
            ch      = new Temporary[ 2 ];
            ch[ 0 ] = new Temporary();
            ch[ 1 ] = new Temporary();
        }
    }

    [PublicAPI]
    public record Temporary2
    {
        public int[]    l = new int[ 23 ];    // [cb]
        public int[ , ] s = new int[ 3, 13 ]; // [window][cb]
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private readonly int[ , ] _slen =
    {
        { 0, 0, 0, 0, 3, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4 },
        { 0, 1, 2, 3, 0, 1, 2, 3, 1, 2, 3, 1, 2, 3, 2, 3 },
    };

    public readonly int[] pretab =
    {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 3, 3, 3, 2, 0
    };

    private LayerIIIDecoder.SBI[] _sfBandIndex; // Init in the constructor.

    public readonly float[] twoToNegativeHalfPow =
    {
        1.0000000000E+00f, 7.0710678119E-01f, 5.0000000000E-01f, 3.5355339059E-01f,
        2.5000000000E-01f, 1.7677669530E-01f, 1.2500000000E-01f, 8.8388347648E-02f,
        6.2500000000E-02f, 4.4194173824E-02f, 3.1250000000E-02f, 2.2097086912E-02f,
        1.5625000000E-02f, 1.1048543456E-02f, 7.8125000000E-03f, 5.5242717280E-03f,
        3.9062500000E-03f, 2.7621358640E-03f, 1.9531250000E-03f, 1.3810679320E-03f,
        9.7656250000E-04f, 6.9053396600E-04f, 4.8828125000E-04f, 3.4526698300E-04f,
        2.4414062500E-04f, 1.7263349150E-04f, 1.2207031250E-04f, 8.6316745750E-05f,
        6.1035156250E-05f, 4.3158372875E-05f, 3.0517578125E-05f, 2.1579186438E-05f,
        1.5258789062E-05f, 1.0789593219E-05f, 7.6293945312E-06f, 5.3947966094E-06f,
        3.8146972656E-06f, 2.6973983047E-06f, 1.9073486328E-06f, 1.3486991523E-06f,
        9.5367431641E-07f, 6.7434957617E-07f, 4.7683715820E-07f, 3.3717478809E-07f,
        2.3841857910E-07f, 1.6858739404E-07f, 1.1920928955E-07f, 8.4293697022E-08f,
        5.9604644775E-08f, 4.2146848511E-08f, 2.9802322388E-08f, 2.1073424255E-08f,
        1.4901161194E-08f, 1.0536712128E-08f, 7.4505805969E-09f, 5.2683560639E-09f,
        3.7252902985E-09f, 2.6341780319E-09f, 1.8626451492E-09f, 1.3170890160E-09f,
        9.3132257462E-10f, 6.5854450798E-10f, 4.6566128731E-10f, 3.2927225399E-10f
    };

    private readonly float[] _t43 = CreateT43();

    private static float[] CreateT43()
    {
        var t43 = new float[ 8192 ];
        var d43 = 4.0 / 3.0;

        for ( var i = 0; i < 8192; i++ )
        {
            t43[ i ] = ( float )Math.Pow( i, d43 );
        }

        return t43;
    }

    public readonly float[ , ] io =
    {
        {
            1.0000000000E+00f, 8.4089641526E-01f, 7.0710678119E-01f, 5.9460355751E-01f,
            5.0000000001E-01f, 4.2044820763E-01f, 3.5355339060E-01f, 2.9730177876E-01f,
            2.5000000001E-01f, 2.1022410382E-01f, 1.7677669530E-01f, 1.4865088938E-01f,
            1.2500000000E-01f, 1.0511205191E-01f, 8.8388347652E-02f, 7.4325444691E-02f,
            6.2500000003E-02f, 5.2556025956E-02f, 4.4194173826E-02f, 3.7162722346E-02f,
            3.1250000002E-02f, 2.6278012978E-02f, 2.2097086913E-02f, 1.8581361173E-02f,
            1.5625000001E-02f, 1.3139006489E-02f, 1.1048543457E-02f, 9.2906805866E-03f,
            7.8125000006E-03f, 6.5695032447E-03f, 5.5242717285E-03f, 4.6453402934E-03f
        },
        {
            1.0000000000E+00f, 7.0710678119E-01f, 5.0000000000E-01f, 3.5355339060E-01f,
            2.5000000000E-01f, 1.7677669530E-01f, 1.2500000000E-01f, 8.8388347650E-02f,
            6.2500000001E-02f, 4.4194173825E-02f, 3.1250000001E-02f, 2.2097086913E-02f,
            1.5625000000E-02f, 1.1048543456E-02f, 7.8125000002E-03f, 5.5242717282E-03f,
            3.9062500001E-03f, 2.7621358641E-03f, 1.9531250001E-03f, 1.3810679321E-03f,
            9.7656250004E-04f, 6.9053396603E-04f, 4.8828125002E-04f, 3.4526698302E-04f,
            2.4414062501E-04f, 1.7263349151E-04f, 1.2207031251E-04f, 8.6316745755E-05f,
            6.1035156254E-05f, 4.3158372878E-05f, 3.0517578127E-05f, 2.1579186439E-05f
        }
    };

    public readonly float[] tan12 =
    {
        0.0f,
        0.26794919f, 0.57735027f, 1.0f, 1.73205081f, 3.73205081f,
        9.9999999e10f, -3.73205081f, -1.73205081f, -1.0f, -0.57735027f,
        -0.26794919f, 0.0f, 0.26794919f, 0.57735027f, 1.0f
    };

    private static int[][]? _reorderTable;

    /// <summary>
    /// Loads the data for the reorder
    /// </summary>
    private static int[] Reorder( int[] scalefac_band )
    {
        var j  = 0;
        var ix = new int[ 576 ];

        for ( var sfb = 0; sfb < 13; sfb++ )
        {
            var start = scalefac_band[ sfb ];
            var end   = scalefac_band[ sfb + 1 ];

            for ( var window = 0; window < 3; window++ )
            {
                for ( var i = start; i < end; i++ )
                {
                    ix[ ( 3 * i ) + window ] = j++;
                }
            }
        }

        return ix;
    }

    private static float[] _cs =
    {
        0.857492925712f, 0.881741997318f, 0.949628649103f, 0.983314592492f,
        0.995517816065f, 0.999160558175f, 0.999899195243f, 0.999993155067f
    };

    private static float[] _ca =
    {
        -0.5144957554270f, -0.4717319685650f, -0.3133774542040f, -0.1819131996110f,
        -0.0945741925262f, -0.0409655828852f, -0.0141985685725f, -0.00369997467375f
    };

    public readonly static float[ , ] Win =
    {
        {
            -1.6141214951E-02f, -5.3603178919E-02f, -1.0070713296E-01f, -1.6280817573E-01f,
            -4.9999999679E-01f, -3.8388735032E-01f, -6.2061144372E-01f, -1.1659756083E+00f,
            -3.8720752656E+00f, -4.2256286556E+00f, -1.5195289984E+00f, -9.7416483388E-01f,
            -7.3744074053E-01f, -1.2071067773E+00f, -5.1636156596E-01f, -4.5426052317E-01f,
            -4.0715656898E-01f, -3.6969460527E-01f, -3.3876269197E-01f, -3.1242222492E-01f,
            -2.8939587111E-01f, -2.6880081906E-01f, -5.0000000266E-01f, -2.3251417468E-01f,
            -2.1596714708E-01f, -2.0004979098E-01f, -1.8449493497E-01f, -1.6905846094E-01f,
            -1.5350360518E-01f, -1.3758624925E-01f, -1.2103922149E-01f, -2.0710679058E-01f,
            -8.4752577594E-02f, -6.4157525656E-02f, -4.1131172614E-02f, -1.4790705759E-02f
        },
        {
            -1.6141214951E-02f, -5.3603178919E-02f, -1.0070713296E-01f, -1.6280817573E-01f,
            -4.9999999679E-01f, -3.8388735032E-01f, -6.2061144372E-01f, -1.1659756083E+00f,
            -3.8720752656E+00f, -4.2256286556E+00f, -1.5195289984E+00f, -9.7416483388E-01f,
            -7.3744074053E-01f, -1.2071067773E+00f, -5.1636156596E-01f, -4.5426052317E-01f,
            -4.0715656898E-01f, -3.6969460527E-01f, -3.3908542600E-01f, -3.1511810350E-01f,
            -2.9642226150E-01f, -2.8184548650E-01f, -5.4119610000E-01f, -2.6213228100E-01f,
            -2.5387916537E-01f, -2.3296291359E-01f, -1.9852728987E-01f, -1.5233534808E-01f,
            -9.6496400054E-02f, -3.3423828516E-02f, 0.0000000000E+00f, 0.0000000000E+00f,
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f
        },
        {
            -4.8300800645E-02f, -1.5715656932E-01f, -2.8325045177E-01f, -4.2953747763E-01f,
            -1.2071067795E+00f, -8.2426483178E-01f, -1.1451749106E+00f, -1.7695290101E+00f,
            -4.5470225061E+00f, -3.4890531002E+00f, -7.3296292804E-01f, -1.5076514758E-01f,
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f,
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f,
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f,
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f,
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f,
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f
        },
        {
            0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f, 0.0000000000E+00f,
            0.0000000000E+00f, 0.0000000000E+00f, -1.5076513660E-01f, -7.3296291107E-01f,
            -3.4890530566E+00f, -4.5470224727E+00f, -1.7695290031E+00f, -1.1451749092E+00f,
            -8.3137738100E-01f, -1.3065629650E+00f, -5.4142014250E-01f, -4.6528974900E-01f,
            -4.1066990750E-01f, -3.7004680800E-01f, -3.3876269197E-01f, -3.1242222492E-01f,
            -2.8939587111E-01f, -2.6880081906E-01f, -5.0000000266E-01f, -2.3251417468E-01f,
            -2.1596714708E-01f, -2.0004979098E-01f, -1.8449493497E-01f, -1.6905846094E-01f,
            -1.5350360518E-01f, -1.3758624925E-01f, -1.2103922149E-01f, -2.0710679058E-01f,
            -8.4752577594E-02f, -6.4157525656E-02f, -4.1131172614E-02f, -1.4790705759E-02f
        }
    };

    [PublicAPI]
    public class Sftable
    {
        public int[] l;
        public int[] s;

        public Sftable()
        {
            l = new int[ 5 ];
            s = new int[ 3 ];
        }

        public Sftable( int[] thel, int[] thes )
        {
            l = thel;
            s = thes;
        }
    }

    public Sftable sftable;

    public readonly int[ ,, ] nrOfSfbBlock =
    {
        {
            { 6, 5, 5, 5 },
            { 9, 9, 9, 9 },
            { 6, 9, 9, 9 }
        },
        {
            { 6, 5, 7, 3 },
            { 9, 9, 12, 6 },
            { 6, 9, 12, 6 }
        },
        {
            { 11, 10, 0, 0 },
            { 18, 18, 0, 0 },
            { 15, 18, 0, 0 }
        },
        {
            { 7, 7, 7, 0 },
            { 12, 12, 12, 0 },
            { 6, 15, 12, 0 }
        },
        {
            { 6, 6, 6, 3 },
            { 12, 9, 9, 6 },
            { 6, 12, 9, 6 }
        },
        {
            { 8, 8, 5, 0 },
            { 15, 12, 9, 0 },
            { 6, 18, 9, 0 }
        }
    };
}
