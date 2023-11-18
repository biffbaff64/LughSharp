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

using LibGDXSharp.Core.Files;

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// A class for the synthesis filter bank. This class does a fast downsampling
/// from 32, 44.1 or 48 kHz to 8 kHz, if ULAW is defined. Frequencies above 4 kHz
/// are removed by ignoring higher subbands.
/// </summary>
[PublicAPI]
public class SynthesisFilter
{
    private float[] _v1;
    private float[] _v2;
    private float[] _actualV;        // v1 or v2
    private int     _actualWritepos; // 0-15
    private float[] _samples;        // 32 new subband samples
    private int     _channel;
    private float   _scalefactor;

    /// <summary>
    /// The _scalefactor scales the calculated float pcm samples to short values
    /// (raw pcm samples are in [-1.0, 1.0], if no violations occur).
    /// </summary>
    public SynthesisFilter( int channelNumber, float factor, float[]? eq0 = null )
    {
        if ( _d == null )
        {
            _d   = LoadD();
            _d16 = SplitArray( _d, 16 );
        }

        _actualV        = null!;
        _actualWritepos = 0;

        _v1          = new float[ 512 ];
        _v2          = new float[ 512 ];
        _samples     = new float[ 32 ];
        _channel     = channelNumber;
        _scalefactor = factor;

        Reset();
    }

    /// <summary>
    /// Reset the Sysnthesis filter.
    /// </summary>
    public void Reset()
    {
        Array.Fill( _v1, 0.0f );
        Array.Fill( _v2, 0.0f );
        Array.Fill( _samples, 0.0f );

        _actualV        = _v1;
        _actualWritepos = 15;
    }

    public void InputSample( float sample, int subbandnumber )
    {
        _samples[ subbandnumber ] = sample;
    }

    public void InputSamples( float[] samples )
    {
        for ( var i = 31; i >= 0; i-- )
        {
            _samples[ i ] = samples[ i ];
        }
    }

    public void ComputeNewV()
    {
        var s0  = _samples[ 0 ];
        var s1  = _samples[ 1 ];
        var s2  = _samples[ 2 ];
        var s3  = _samples[ 3 ];
        var s4  = _samples[ 4 ];
        var s5  = _samples[ 5 ];
        var s6  = _samples[ 6 ];
        var s7  = _samples[ 7 ];
        var s8  = _samples[ 8 ];
        var s9  = _samples[ 9 ];
        var s10 = _samples[ 10 ];
        var s11 = _samples[ 11 ];
        var s12 = _samples[ 12 ];
        var s13 = _samples[ 13 ];
        var s14 = _samples[ 14 ];
        var s15 = _samples[ 15 ];
        var s16 = _samples[ 16 ];
        var s17 = _samples[ 17 ];
        var s18 = _samples[ 18 ];
        var s19 = _samples[ 19 ];
        var s20 = _samples[ 20 ];
        var s21 = _samples[ 21 ];
        var s22 = _samples[ 22 ];
        var s23 = _samples[ 23 ];
        var s24 = _samples[ 24 ];
        var s25 = _samples[ 25 ];
        var s26 = _samples[ 26 ];
        var s27 = _samples[ 27 ];
        var s28 = _samples[ 28 ];
        var s29 = _samples[ 29 ];
        var s30 = _samples[ 30 ];
        var s31 = _samples[ 31 ];

        var p0  = s0 + s31;
        var p1  = s1 + s30;
        var p2  = s2 + s29;
        var p3  = s3 + s28;
        var p4  = s4 + s27;
        var p5  = s5 + s26;
        var p6  = s6 + s25;
        var p7  = s7 + s24;
        var p8  = s8 + s23;
        var p9  = s9 + s22;
        var p10 = s10 + s21;
        var p11 = s11 + s20;
        var p12 = s12 + s19;
        var p13 = s13 + s18;
        var p14 = s14 + s17;
        var p15 = s15 + s16;

        var pp0  = p0 + p15;
        var pp1  = p1 + p14;
        var pp2  = p2 + p13;
        var pp3  = p3 + p12;
        var pp4  = p4 + p11;
        var pp5  = p5 + p10;
        var pp6  = p6 + p9;
        var pp7  = p7 + p8;
        var pp8  = ( p0 - p15 ) * Cos132;
        var pp9  = ( p1 - p14 ) * Cos332;
        var pp10 = ( p2 - p13 ) * Cos532;
        var pp11 = ( p3 - p12 ) * Cos732;
        var pp12 = ( p4 - p11 ) * Cos932;
        var pp13 = ( p5 - p10 ) * Cos132;
        var pp14 = ( p6 - p9 ) * Cos1332;
        var pp15 = ( p7 - p8 ) * Cos1532;

        p0  = pp0 + pp7;
        p1  = pp1 + pp6;
        p2  = pp2 + pp5;
        p3  = pp3 + pp4;
        p4  = ( pp0 - pp7 ) * Cos116;
        p5  = ( pp1 - pp6 ) * Cos316;
        p6  = ( pp2 - pp5 ) * Cos516;
        p7  = ( pp3 - pp4 ) * Cos716;
        p8  = pp8 + pp15;
        p9  = pp9 + pp14;
        p10 = pp10 + pp13;
        p11 = pp11 + pp12;
        p12 = ( pp8 - pp15 ) * Cos116;
        p13 = ( pp9 - pp14 ) * Cos316;
        p14 = ( pp10 - pp13 ) * Cos516;
        p15 = ( pp11 - pp12 ) * Cos716;

        pp0  = p0 + p3;
        pp1  = p1 + p2;
        pp2  = ( p0 - p3 ) * Cos18;
        pp3  = ( p1 - p2 ) * Cos38;
        pp4  = p4 + p7;
        pp5  = p5 + p6;
        pp6  = ( p4 - p7 ) * Cos18;
        pp7  = ( p5 - p6 ) * Cos38;
        pp8  = p8 + p11;
        pp9  = p9 + p10;
        pp10 = ( p8 - p11 ) * Cos18;
        pp11 = ( p9 - p10 ) * Cos38;
        pp12 = p12 + p15;
        pp13 = p13 + p14;
        pp14 = ( p12 - p15 ) * Cos18;
        pp15 = ( p13 - p14 ) * Cos38;

        p0  = pp0 + pp1;
        p1  = ( pp0 - pp1 ) * Cos14;
        p2  = pp2 + pp3;
        p3  = ( pp2 - pp3 ) * Cos14;
        p4  = pp4 + pp5;
        p5  = ( pp4 - pp5 ) * Cos14;
        p6  = pp6 + pp7;
        p7  = ( pp6 - pp7 ) * Cos14;
        p8  = pp8 + pp9;
        p9  = ( pp8 - pp9 ) * Cos14;
        p10 = pp10 + pp11;
        p11 = ( pp10 - pp11 ) * Cos14;
        p12 = pp12 + pp13;
        p13 = ( pp12 - pp13 ) * Cos14;
        p14 = pp14 + pp15;
        p15 = ( pp14 - pp15 ) * Cos14;

        float newV1,  newV2,  newV3,  newV4,  newV8,  newV9;
        float newV10, newV11, newV12, newV13, newV14, newV15;

        // this is pretty insane coding
        float tmp1;

        var newV19 /* 36-17 */ = -( newV4 = ( newV12 = p7 ) + p5 ) - p6;
        var newV27 /* 44-17 */ = -p6 - p7 - p4;
        var newV6              = ( newV10 = ( newV14 = p15 ) + p11 ) + p13;
        var newV17 /* 34-17 */ = -( newV2 = p15 + p13 + p9 ) - p14;
        var newV21 /* 38-17 */ = ( tmp1 = -p14 - p15 - p10 - p11 ) - p13;
        var newV29 /* 46-17 */ = -p14 - p15 - p12 - p8;
        var newV25 /* 42-17 */ = tmp1 - p12;
        var newV31 /* 48-17 */ = -p0;
        var newV0              = p1;
        var newV23 /* 40-17 */ = -( newV8 = p3 ) - p2;

        p0  = ( s0 - s31 ) * Cos164;
        p1  = ( s1 - s30 ) * Cos364;
        p2  = ( s2 - s29 ) * Cos564;
        p3  = ( s3 - s28 ) * Cos764;
        p4  = ( s4 - s27 ) * Cos964;
        p5  = ( s5 - s26 ) * Cos1164;
        p6  = ( s6 - s25 ) * Cos1364;
        p7  = ( s7 - s24 ) * Cos1564;
        p8  = ( s8 - s23 ) * Cos1764;
        p9  = ( s9 - s22 ) * Cos1964;
        p10 = ( s10 - s21 ) * Cos2164;
        p11 = ( s11 - s20 ) * Cos2364;
        p12 = ( s12 - s19 ) * Cos2564;
        p13 = ( s13 - s18 ) * Cos2764;
        p14 = ( s14 - s17 ) * Cos2964;
        p15 = ( s15 - s16 ) * Cos3164;

        pp0  = p0 + p15;
        pp1  = p1 + p14;
        pp2  = p2 + p13;
        pp3  = p3 + p12;
        pp4  = p4 + p11;
        pp5  = p5 + p10;
        pp6  = p6 + p9;
        pp7  = p7 + p8;
        pp8  = ( p0 - p15 ) * Cos132;
        pp9  = ( p1 - p14 ) * Cos332;
        pp10 = ( p2 - p13 ) * Cos532;
        pp11 = ( p3 - p12 ) * Cos732;
        pp12 = ( p4 - p11 ) * Cos932;
        pp13 = ( p5 - p10 ) * Cos1132;
        pp14 = ( p6 - p9 ) * Cos1332;
        pp15 = ( p7 - p8 ) * Cos1532;

        p0  = pp0 + pp7;
        p1  = pp1 + pp6;
        p2  = pp2 + pp5;
        p3  = pp3 + pp4;
        p4  = ( pp0 - pp7 ) * Cos116;
        p5  = ( pp1 - pp6 ) * Cos316;
        p6  = ( pp2 - pp5 ) * Cos516;
        p7  = ( pp3 - pp4 ) * Cos716;
        p8  = pp8 + pp15;
        p9  = pp9 + pp14;
        p10 = pp10 + pp13;
        p11 = pp11 + pp12;
        p12 = ( pp8 - pp15 ) * Cos116;
        p13 = ( pp9 - pp14 ) * Cos316;
        p14 = ( pp10 - pp13 ) * Cos516;
        p15 = ( pp11 - pp12 ) * Cos716;

        pp0  = p0 + p3;
        pp1  = p1 + p2;
        pp2  = ( p0 - p3 ) * Cos18;
        pp3  = ( p1 - p2 ) * Cos38;
        pp4  = p4 + p7;
        pp5  = p5 + p6;
        pp6  = ( p4 - p7 ) * Cos18;
        pp7  = ( p5 - p6 ) * Cos38;
        pp8  = p8 + p11;
        pp9  = p9 + p10;
        pp10 = ( p8 - p11 ) * Cos18;
        pp11 = ( p9 - p10 ) * Cos38;
        pp12 = p12 + p15;
        pp13 = p13 + p14;
        pp14 = ( p12 - p15 ) * Cos18;
        pp15 = ( p13 - p14 ) * Cos38;

        p0  = pp0 + pp1;
        p1  = ( pp0 - pp1 ) * Cos14;
        p2  = pp2 + pp3;
        p3  = ( pp2 - pp3 ) * Cos14;
        p4  = pp4 + pp5;
        p5  = ( pp4 - pp5 ) * Cos14;
        p6  = pp6 + pp7;
        p7  = ( pp6 - pp7 ) * Cos14;
        p8  = pp8 + pp9;
        p9  = ( pp8 - pp9 ) * Cos14;
        p10 = pp10 + pp11;
        p11 = ( pp10 - pp11 ) * Cos14;
        p12 = pp12 + pp13;
        p13 = ( pp12 - pp13 ) * Cos14;
        p14 = pp14 + pp15;
        p15 = ( pp14 - pp15 ) * Cos14;

        // manually doing something that a compiler should handle sucks
        // coding like this is hard to read
        float tmp2;

        var newV5              = ( newV11 = ( newV13 = ( newV15 = p15 ) + p7 ) + p11 ) + p5 + p13;
        var newV7              = ( newV9 = p15 + p11 + p3 ) + p13;
        var newV16 /* 33-17 */ = -( newV1 = ( tmp1 = p13 + p15 + p9 ) + p1 ) - p14;
        var newV18 /* 35-17 */ = -( newV3 = tmp1 + p5 + p7 ) - p6 - p14;

        var newV22 /* 39-17 */ = ( tmp1 = -p10 - p11 - p14 - p15 ) - p13 - p2 - p3;
        var newV20 /* 37-17 */ = tmp1 - p13 - p5 - p6 - p7;
        var newV24 /* 41-17 */ = tmp1 - p12 - p2 - p3;
        var newV26 /* 43-17 */ = tmp1 - p12 - ( tmp2 = p4 + p6 + p7 );
        var newV30 /* 47-17 */ = ( tmp1 = -p8 - p12 - p14 - p15 ) - p0;
        var newV28 /* 45-17 */ = tmp1 - tmp2;

        // insert V[0-15] (== new_v[0-15]) into actual v:
        // float[] x2 = _actualV + actual_write_pos;
        var dest = _actualV;
        var pos  = _actualWritepos;

        dest[ 0 + pos ]   = newV0;
        dest[ 16 + pos ]  = newV1;
        dest[ 32 + pos ]  = newV2;
        dest[ 48 + pos ]  = newV3;
        dest[ 64 + pos ]  = newV4;
        dest[ 80 + pos ]  = newV5;
        dest[ 96 + pos ]  = newV6;
        dest[ 112 + pos ] = newV7;
        dest[ 128 + pos ] = newV8;
        dest[ 144 + pos ] = newV9;
        dest[ 160 + pos ] = newV10;
        dest[ 176 + pos ] = newV11;
        dest[ 192 + pos ] = newV12;
        dest[ 208 + pos ] = newV13;
        dest[ 224 + pos ] = newV14;
        dest[ 240 + pos ] = newV15;

        // V[16] is always 0.0:
        dest[ 256 + pos ] = 0.0f;

        // insert V[17-31] (== -new_v[15-1]) into actual v:
        dest[ 272 + pos ] = -newV15;
        dest[ 288 + pos ] = -newV14;
        dest[ 304 + pos ] = -newV13;
        dest[ 320 + pos ] = -newV12;
        dest[ 336 + pos ] = -newV11;
        dest[ 352 + pos ] = -newV10;
        dest[ 368 + pos ] = -newV9;
        dest[ 384 + pos ] = -newV8;
        dest[ 400 + pos ] = -newV7;
        dest[ 416 + pos ] = -newV6;
        dest[ 432 + pos ] = -newV5;
        dest[ 448 + pos ] = -newV4;
        dest[ 464 + pos ] = -newV3;
        dest[ 480 + pos ] = -newV2;
        dest[ 496 + pos ] = -newV1;

        // insert V[32] (== -new_v[0]) into other v:
        dest = ( _actualV == _v1 ) ? _v2 : _v1;

        dest[ 0 + pos ] = -newV0;

        // insert V[33-48] (== new_v[16-31]) into other v:
        dest[ 16 + pos ]  = newV16;
        dest[ 32 + pos ]  = newV17;
        dest[ 48 + pos ]  = newV18;
        dest[ 64 + pos ]  = newV19;
        dest[ 80 + pos ]  = newV20;
        dest[ 96 + pos ]  = newV21;
        dest[ 112 + pos ] = newV22;
        dest[ 128 + pos ] = newV23;
        dest[ 144 + pos ] = newV24;
        dest[ 160 + pos ] = newV25;
        dest[ 176 + pos ] = newV26;
        dest[ 192 + pos ] = newV27;
        dest[ 208 + pos ] = newV28;
        dest[ 224 + pos ] = newV29;
        dest[ 240 + pos ] = newV30;
        dest[ 256 + pos ] = newV31;

        // insert V[49-63] (== new_v[30-16]) into other v:
        dest[ 272 + pos ] = newV30;
        dest[ 288 + pos ] = newV29;
        dest[ 304 + pos ] = newV28;
        dest[ 320 + pos ] = newV27;
        dest[ 336 + pos ] = newV26;
        dest[ 352 + pos ] = newV25;
        dest[ 368 + pos ] = newV24;
        dest[ 384 + pos ] = newV23;
        dest[ 400 + pos ] = newV22;
        dest[ 416 + pos ] = newV21;
        dest[ 432 + pos ] = newV20;
        dest[ 448 + pos ] = newV19;
        dest[ 464 + pos ] = newV18;
        dest[ 480 + pos ] = newV17;
        dest[ 496 + pos ] = newV16;
    }

    /// <summary>
    /// Calculate 32 PCM samples and put them into the OutputBuffer object.
    /// </summary>
    public void CalculatePcmSamples( out OutputBuffer? buffer )
    {
        ComputeNewV();
        ComputePcmSamples( out buffer );

        _actualWritepos = ( _actualWritepos + 1 ) & 0xf;
        _actualV        = _actualV == _v1 ? _v2 : _v1;

        // this may not be necessary. The Layer III decoder always
        // outputs 32 subband samples, but I haven't checked layer I & II.
        Array.Fill( _samples, 0.0f );
    }

    public void ComputePcmSamples( out OutputBuffer? buffer )
    {
        switch ( _actualWritepos )
        {
            case 0:
                ComputePcmSamples0( out buffer );

                break;

            case 1:
                ComputePcmSamples1( out buffer );

                break;

            case 2:
                ComputePcmSamples2( out buffer );

                break;

            case 3:
                ComputePcmSamples3( out buffer );

                break;

            case 4:
                ComputePcmSamples4( out buffer );

                break;

            case 5:
                ComputePcmSamples5( out buffer );

                break;

            case 6:
                ComputePcmSamples6( out buffer );

                break;

            case 7:
                ComputePcmSamples7( out buffer );

                break;

            case 8:
                ComputePcmSamples8( out buffer );

                break;

            case 9:
                ComputePcmSamples9( out buffer );

                break;

            case 10:
                ComputePcmSamples10( out buffer );

                break;

            case 11:
                ComputePcmSamples11( out buffer );

                break;

            case 12:
                ComputePcmSamples12( out buffer );

                break;

            case 13:
                ComputePcmSamples13( out buffer );

                break;

            case 14:
                ComputePcmSamples14( out buffer );

                break;

            case 15:
                ComputePcmSamples15( out buffer );

                break;

            default:
                buffer = null;

                break;
        }

        if ( buffer != null )
        {
            buffer.AppendSamples( _channel, _tmpOut );
        }
    }

    private float[] _tmpOut = new float[ 32 ];

    private void ComputePcmSamples0( ref OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 0 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;
            
            dvp += 16;
        }
    }

    private void ComputePcmSamples1( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 1 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples2( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 2 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples3( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 3 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples4( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 4 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples5( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 5 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples6( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 6 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples7( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 7 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples8( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 8 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples9( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 9 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples10( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 10 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples11( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 11 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples12( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 12 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples13( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 13 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples14( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 14 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 15 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] = pcmSample;

            dvp += 16;
        }
    }

    private void ComputePcmSamples15( out OutputBuffer buffer )
    {
        var vp     = _actualV;
        var tmpOut = _tmpOut;
        var dvp    = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcmSample = ( ( ( vp[ 15 + dvp ] * _d16[ i, 0 ] )
                              + ( vp[ 14 + dvp ] * _d16[ i, 1 ] )
                              + ( vp[ 13 + dvp ] * _d16[ i, 2 ] )
                              + ( vp[ 12 + dvp ] * _d16[ i, 3 ] )
                              + ( vp[ 11 + dvp ] * _d16[ i, 4 ] )
                              + ( vp[ 10 + dvp ] * _d16[ i, 5 ] )
                              + ( vp[ 9 + dvp ] * _d16[ i, 6 ] )
                              + ( vp[ 8 + dvp ] * _d16[ i, 7 ] )
                              + ( vp[ 7 + dvp ] * _d16[ i, 8 ] )
                              + ( vp[ 6 + dvp ] * _d16[ i, 9 ] )
                              + ( vp[ 5 + dvp ] * _d16[ i, 10 ] )
                              + ( vp[ 4 + dvp ] * _d16[ i, 11 ] )
                              + ( vp[ 3 + dvp ] * _d16[ i, 12 ] )
                              + ( vp[ 2 + dvp ] * _d16[ i, 13 ] )
                              + ( vp[ 1 + dvp ] * _d16[ i, 14 ] )
                              + ( vp[ 0 + dvp ] * _d16[ i, 15 ] ) )
                            * _scalefactor );

            tmpOut[ i ] =  pcmSample;
            dvp         += 16;
        }
    }

    private readonly static double MyPI = 3.14159265358979323846;

    private readonly static float Cos164  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 64.0 ) ) );
    private readonly static float Cos364  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 3.0 ) / 64.0 ) ) );
    private readonly static float Cos564  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 5.0 ) / 64.0 ) ) );
    private readonly static float Cos764  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 7.0 ) / 64.0 ) ) );
    private readonly static float Cos964  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 9.0 ) / 64.0 ) ) );
    private readonly static float Cos1164 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 11.0 ) / 64.0 ) ) );
    private readonly static float Cos1364 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 13.0 ) / 64.0 ) ) );
    private readonly static float Cos1564 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 15.0 ) / 64.0 ) ) );
    private readonly static float Cos1764 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 17.0 ) / 64.0 ) ) );
    private readonly static float Cos1964 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 19.0 ) / 64.0 ) ) );
    private readonly static float Cos2164 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 21.0 ) / 64.0 ) ) );
    private readonly static float Cos2364 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 23.0 ) / 64.0 ) ) );
    private readonly static float Cos2564 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 25.0 ) / 64.0 ) ) );
    private readonly static float Cos2764 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 27.0 ) / 64.0 ) ) );
    private readonly static float Cos2964 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 29.0 ) / 64.0 ) ) );
    private readonly static float Cos3164 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 31.0 ) / 64.0 ) ) );
    private readonly static float Cos132  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 32.0 ) ) );
    private readonly static float Cos332  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 3.0 ) / 32.0 ) ) );
    private readonly static float Cos532  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 5.0 ) / 32.0 ) ) );
    private readonly static float Cos732  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 7.0 ) / 32.0 ) ) );
    private readonly static float Cos932  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 9.0 ) / 32.0 ) ) );
    private readonly static float Cos1132 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 11.0 ) / 32.0 ) ) );
    private readonly static float Cos1332 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 13.0 ) / 32.0 ) ) );
    private readonly static float Cos1532 = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 15.0 ) / 32.0 ) ) );
    private readonly static float Cos116  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 16.0 ) ) );
    private readonly static float Cos316  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 3.0 ) / 16.0 ) ) );
    private readonly static float Cos516  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 5.0 ) / 16.0 ) ) );
    private readonly static float Cos716  = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 7.0 ) / 16.0 ) ) );
    private readonly static float Cos18   = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 8.0 ) ) );
    private readonly static float Cos38   = ( float )( 1.0 / ( 2.0 * Math.Cos( ( MyPI * 3.0 ) / 8.0 ) ) );
    private readonly static float Cos14   = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 4.0 ) ) );

    // Note: These values are not in the same order
    // as in Annex 3-B.3 of the ISO/IEC DIS 11172-3
    // private float d[] = {0.000000000, -4.000442505};

    private static float[]? _d = null;

    // d[] split into subarrays of length 16. This provides for more faster
    // access by allowing a block of 16 to be addressed with constant offset.
    private static float[ , ] _d16 = null!;

    /// <summary>
    /// Loads the data for the d[] from the resource SFd.ser.
    /// </summary>
    /// <returns>the loaded values for d[]</returns>
    private float[] LoadD()
    {
        try
        {
            var o = DeserializeArray( GetResourceAsStream( "/sfd.ser" ), typeof( float ), 512 );

            return ( float[] )o;
        }
        catch ( IOException ex )
        {
            throw new ExceptionInitialiserError( ex );
        }
    }

    /**
     * Deserializes an array from a given <code>InputStream</code>.
     *
     * @param in The <code>InputStream</code> to deserialize an object from.
     *
     * @param elemType The class denoting the type of the array elements.
     * @param length The expected length of the array, or -1 if any length is expected.
     */
    private Object DeserializeArray( InputStream inStream, Type elemType, int length )
    {
        ArgumentNullException.ThrowIfNull( inStream );
        ArgumentNullException.ThrowIfNull( elemType );

        if ( length < -1 )
        {
            throw new ArgumentException( $@"length should not be < -1 [Actual value: {length}" );
        }

        var obj = Deserialize( inStream );

        Type cls = obj.GetType();

        if ( !cls.IsArray )
        {
            throw new InvalidTypeException( "object is not an array" );
        }

        Type arrayElemType = cls.GetComponentType();

        if ( arrayElemType != elemType )
        {
            throw new InvalidTypeException( "unexpected array component type" );
        }

        if ( length != -1 )
        {
            var arrayLength = Array.GetLength( obj );

            if ( arrayLength != length )
            {
                throw new InvalidTypeException( "array length mismatch" );
            }
        }

        return obj;
    }

    public object Deserialize( InputStream inStream )
    {
        ArgumentNullException.ThrowIfNull( inStream );

        ObjectInputStream objIn = new ObjectInputStream( inStream );

        Object obj;

        try
        {
            obj = objIn.ReadObject();
        }
        catch ( ClassNotFoundException ex )
        {
            throw new InvalidClassException( ex.toString() );
        }

        return obj;
    }

    /// <summary>
    /// Converts a 1D array into a number of smaller arrays. This is used to
    /// achieve offset + constant indexing into an array. Each sub-array
    /// represents a block of values of the original array.
    /// </summary>
    /// <param name="array"> The array to split up into blocks. </param>
    /// <param name="blockSize">
    /// The size of the blocks to split the array into. This must be an exact
    /// divisor of the length of the array, or some data will be lost from
    /// the main array.
    /// </param>
    /// <returns>
    /// An array of arrays in which each element in the returned array will
    /// be of length <tt>blockSize</tt>.
    /// </returns>
    private float[ , ] SplitArray( float[] array, int blockSize )
    {
        var size  = array.Length / blockSize;
        var split = new float[ size ][];

        for ( var i = 0; i < size; i++ )
        {
            split[ i ] = SubArray( array, i * blockSize, blockSize );
        }

        return split;
    }

    /// <summary>
    /// Returns a subarray of an existing array.
    /// </summary>
    /// <param name="array"> The array to retrieve a subarra from. </param>
    /// <param name="offs">
    /// The offset in the array that corresponds to the first index of the subarray.
    /// </param>
    /// <param name="len"> The number of indeces in the subarray. </param>
    /// <returns> The subarray, which may be of length 0. </returns>
    private float[] SubArray( float[] array, int offs, int len )
    {
        if ( ( offs + len ) > array.Length )
        {
            len = array.Length - offs;
        }

        len = Math.Max( len, 0 );

        var subarray = new float[ len ];

        for ( var i = 0; i < len; i++ )
        {
            subarray[ i ] = array[ offs + i ];
        }

        return subarray;
    }
}
