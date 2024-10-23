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

using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Audio.Maponus.Decoding;

/// <summary>
/// A class for the synthesis filter bank. This class does a fast downsampling from 32,
/// 44.1 or 48 kHz to 8 kHz, if ULAW is defined. Frequencies above 4 kHz are removed by
/// ignoring higher subbands.
/// </summary>
[PublicAPI]
public class SynthesisFilter
{
    private const double MY_PI = 3.14159265358979323846;

    private readonly int     _channel;
    private readonly float[] _samples; // 32 new subband samples
    private readonly float   _scalefactor;
    private readonly float[] _v1;
    private readonly float[] _v2;

    private float[]  _actualV = null!; // v1 or v2
    private int      _actualWritePos;  // 0-15
    private float[]? _eq;
    private float[]  _tmpOut = null!;

    // Note: These values are not in the same order as in Annex 3-B.3 of the ISO/IEC DIS 11172-3 
    private static readonly float _cos164  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( MY_PI / 64.0 ) ) );
    private static readonly float _cos364  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 3.0 ) / 64.0 ) ) );
    private static readonly float _cos564  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 5.0 ) / 64.0 ) ) );
    private static readonly float _cos764  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 7.0 ) / 64.0 ) ) );
    private static readonly float _cos964  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 9.0 ) / 64.0 ) ) );
    private static readonly float _cos1164 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 11.0 ) / 64.0 ) ) );
    private static readonly float _cos1364 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 13.0 ) / 64.0 ) ) );
    private static readonly float _cos1564 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 15.0 ) / 64.0 ) ) );
    private static readonly float _cos1764 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 17.0 ) / 64.0 ) ) );
    private static readonly float _cos1964 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 19.0 ) / 64.0 ) ) );
    private static readonly float _cos2164 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 21.0 ) / 64.0 ) ) );
    private static readonly float _cos2364 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 23.0 ) / 64.0 ) ) );
    private static readonly float _cos2564 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 25.0 ) / 64.0 ) ) );
    private static readonly float _cos2764 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 27.0 ) / 64.0 ) ) );
    private static readonly float _cos2964 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 29.0 ) / 64.0 ) ) );
    private static readonly float _cos3164 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 31.0 ) / 64.0 ) ) );
    private static readonly float _cos132  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( MY_PI / 32.0 ) ) );
    private static readonly float _cos332  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 3.0 ) / 32.0 ) ) );
    private static readonly float _cos532  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 5.0 ) / 32.0 ) ) );
    private static readonly float _cos732  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 7.0 ) / 32.0 ) ) );
    private static readonly float _cos932  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 9.0 ) / 32.0 ) ) );
    private static readonly float _cos1132 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 11.0 ) / 32.0 ) ) );
    private static readonly float _cos1332 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 13.0 ) / 32.0 ) ) );
    private static readonly float _cos1532 = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 15.0 ) / 32.0 ) ) );
    private static readonly float _cos116  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( MY_PI / 16.0 ) ) );
    private static readonly float _cos316  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 3.0 ) / 16.0 ) ) );
    private static readonly float _cos516  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 5.0 ) / 16.0 ) ) );
    private static readonly float _cos716  = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 7.0 ) / 16.0 ) ) );
    private static readonly float _cos18   = ( float ) ( 1.0 / ( 2.0 * Math.Cos( MY_PI / 8.0 ) ) );
    private static readonly float _cos38   = ( float ) ( 1.0 / ( 2.0 * Math.Cos( ( MY_PI * 3.0 ) / 8.0 ) ) );
    private static readonly float _cos14   = ( float ) ( 1.0 / ( 2.0 * Math.Cos( MY_PI / 4.0 ) ) );

    private static float[]? _d;

    /// <summary>
    /// d[] split into subarrays of length 16. This provides for more faster
    /// access by allowing a block of 16 to be addressed with constant offset.
    /// </summary>
    private static float[][]? _d16;

    /// <summary>
    /// The original data for d[]. This data (was) loaded from a file
    /// to reduce the overall package size and to improve performance. 
    /// </summary>
    private static readonly float[] _dData =
    [
        0.000000000f, -0.000442505f, 0.003250122f, -0.007003784f, 0.031082153f, -0.078628540f,
        0.100311279f, -0.572036743f, 1.144989014f, 0.572036743f, 0.100311279f, 0.078628540f,
        0.031082153f, 0.007003784f, 0.003250122f, 0.000442505f, -0.000015259f, -0.000473022f,
        0.003326416f, -0.007919312f, 0.030517578f, -0.084182739f, 0.090927124f, -0.600219727f,
        1.144287109f, 0.543823242f, 0.108856201f, 0.073059082f, 0.031478882f, 0.006118774f,
        0.003173828f, 0.000396729f, -0.000015259f, -0.000534058f, 0.003387451f, -0.008865356f,
        0.029785156f, -0.089706421f, 0.080688477f, -0.628295898f, 1.142211914f, 0.515609741f,
        0.116577148f, 0.067520142f, 0.031738281f, 0.005294800f, 0.003082275f, 0.000366211f,
        -0.000015259f, -0.000579834f, 0.003433228f, -0.009841919f, 0.028884888f, -0.095169067f,
        0.069595337f, -0.656219482f, 1.138763428f, 0.487472534f, 0.123474121f, 0.061996460f,
        0.031845093f, 0.004486084f, 0.002990723f, 0.000320435f, -0.000015259f, -0.000625610f,
        0.003463745f, -0.010848999f, 0.027801514f, -0.100540161f, 0.057617188f, -0.683914185f,
        1.133926392f, 0.459472656f, 0.129577637f, 0.056533813f, 0.031814575f, 0.003723145f,
        0.002899170f, 0.000289917f, -0.000015259f, -0.000686646f, 0.003479004f, -0.011886597f,
        0.026535034f, -0.105819702f, 0.044784546f, -0.711318970f, 1.127746582f, 0.431655884f,
        0.134887695f, 0.051132202f, 0.031661987f, 0.003005981f, 0.002792358f, 0.000259399f,
        -0.000015259f, -0.000747681f, 0.003479004f, -0.012939453f, 0.025085449f, -0.110946655f,
        0.031082153f, -0.738372803f, 1.120223999f, 0.404083252f, 0.139450073f, 0.045837402f,
        0.031387329f, 0.002334595f, 0.002685547f, 0.000244141f, -0.000030518f, -0.000808716f,
        0.003463745f, -0.014022827f, 0.023422241f, -0.115921021f, 0.016510010f, -0.765029907f,
        1.111373901f, 0.376800537f, 0.143264771f, 0.040634155f, 0.031005859f, 0.001693726f,
        0.002578735f, 0.000213623f, -0.000030518f, -0.000885010f, 0.003417969f, -0.015121460f,
        0.021575928f, -0.120697021f, 0.001068115f, -0.791213989f, 1.101211548f, 0.349868774f,
        0.146362305f, 0.035552979f, 0.030532837f, 0.001098633f, 0.002456665f, 0.000198364f,
        -0.000030518f, -0.000961304f, 0.003372192f, -0.016235352f, 0.019531250f, -0.125259399f,
        -0.015228271f, -0.816864014f, 1.089782715f, 0.323318481f, 0.148773193f, 0.030609131f,
        0.029937744f, 0.000549316f, 0.002349854f, 0.000167847f, -0.000030518f, -0.001037598f,
        0.003280640f, -0.017349243f, 0.017257690f, -0.129562378f, -0.032379150f, -0.841949463f,
        1.077117920f, 0.297210693f, 0.150497437f, 0.025817871f, 0.029281616f, 0.000030518f,
        0.002243042f, 0.000152588f, -0.000045776f, -0.001113892f, 0.003173828f, -0.018463135f,
        0.014801025f, -0.133590698f, -0.050354004f, -0.866363525f, 1.063217163f, 0.271591187f,
        0.151596069f, 0.021179199f, 0.028533936f, -0.000442505f, 0.002120972f, 0.000137329f,
        -0.000045776f, -0.001205444f, 0.003051758f, -0.019577026f, 0.012115479f, -0.137298584f,
        -0.069168091f, -0.890090942f, 1.048156738f, 0.246505737f, 0.152069092f, 0.016708374f,
        0.027725220f, -0.000869751f, 0.002014160f, 0.000122070f, -0.000061035f, -0.001296997f,
        0.002883911f, -0.020690918f, 0.009231567f, -0.140670776f, -0.088775635f, -0.913055420f,
        1.031936646f, 0.221984863f, 0.151962280f, 0.012420654f, 0.026840210f, -0.001266479f,
        0.001907349f, 0.000106812f, -0.000061035f, -0.001388550f, 0.002700806f, -0.021789551f,
        0.006134033f, -0.143676758f, -0.109161377f, -0.935195923f, 1.014617920f, 0.198059082f,
        0.151306152f, 0.008316040f, 0.025909424f, -0.001617432f, 0.001785278f, 0.000106812f,
        -0.000076294f, -0.001480103f, 0.002487183f, -0.022857666f, 0.002822876f, -0.146255493f,
        -0.130310059f, -0.956481934f, 0.996246338f, 0.174789429f, 0.150115967f, 0.004394531f,
        0.024932861f, -0.001937866f, 0.001693726f, 0.000091553f, -0.000076294f, -0.001586914f,
        0.002227783f, -0.023910522f, -0.000686646f, -0.148422241f, -0.152206421f, -0.976852417f,
        0.976852417f, 0.152206421f, 0.148422241f, 0.000686646f, 0.023910522f, -0.002227783f,
        0.001586914f, 0.000076294f, -0.000091553f, -0.001693726f, 0.001937866f, -0.024932861f,
        -0.004394531f, -0.150115967f, -0.174789429f, -0.996246338f, 0.956481934f, 0.130310059f,
        0.146255493f, -0.002822876f, 0.022857666f, -0.002487183f, 0.001480103f, 0.000076294f,
        -0.000106812f, -0.001785278f, 0.001617432f, -0.025909424f, -0.008316040f, -0.151306152f,
        -0.198059082f, -1.014617920f, 0.935195923f, 0.109161377f, 0.143676758f, -0.006134033f,
        0.021789551f, -0.002700806f, 0.001388550f, 0.000061035f, -0.000106812f, -0.001907349f,
        0.001266479f, -0.026840210f, -0.012420654f, -0.151962280f, -0.221984863f, -1.031936646f,
        0.913055420f, 0.088775635f, 0.140670776f, -0.009231567f, 0.020690918f, -0.002883911f,
        0.001296997f, 0.000061035f, -0.000122070f, -0.002014160f, 0.000869751f, -0.027725220f,
        -0.016708374f, -0.152069092f, -0.246505737f, -1.048156738f, 0.890090942f, 0.069168091f,
        0.137298584f, -0.012115479f, 0.019577026f, -0.003051758f, 0.001205444f, 0.000045776f,
        -0.000137329f, -0.002120972f, 0.000442505f, -0.028533936f, -0.021179199f, -0.151596069f,
        -0.271591187f, -1.063217163f, 0.866363525f, 0.050354004f, 0.133590698f, -0.014801025f,
        0.018463135f, -0.003173828f, 0.001113892f, 0.000045776f, -0.000152588f, -0.002243042f,
        -0.000030518f, -0.029281616f, -0.025817871f, -0.150497437f, -0.297210693f, -1.077117920f,
        0.841949463f, 0.032379150f, 0.129562378f, -0.017257690f, 0.017349243f, -0.003280640f,
        0.001037598f, 0.000030518f, -0.000167847f, -0.002349854f, -0.000549316f, -0.029937744f,
        -0.030609131f, -0.148773193f, -0.323318481f, -1.089782715f, 0.816864014f, 0.015228271f,
        0.125259399f, -0.019531250f, 0.016235352f, -0.003372192f, 0.000961304f, 0.000030518f,
        -0.000198364f, -0.002456665f, -0.001098633f, -0.030532837f, -0.035552979f, -0.146362305f,
        -0.349868774f, -1.101211548f, 0.791213989f, -0.001068115f, 0.120697021f, -0.021575928f,
        0.015121460f, -0.003417969f, 0.000885010f, 0.000030518f, -0.000213623f, -0.002578735f,
        -0.001693726f, -0.031005859f, -0.040634155f, -0.143264771f, -0.376800537f, -1.111373901f,
        0.765029907f, -0.016510010f, 0.115921021f, -0.023422241f, 0.014022827f, -0.003463745f,
        0.000808716f, 0.000030518f, -0.000244141f, -0.002685547f, -0.002334595f, -0.031387329f,
        -0.045837402f, -0.139450073f, -0.404083252f, -1.120223999f, 0.738372803f, -0.031082153f,
        0.110946655f, -0.025085449f, 0.012939453f, -0.003479004f, 0.000747681f, 0.000015259f,
        -0.000259399f, -0.002792358f, -0.003005981f, -0.031661987f, -0.051132202f, -0.134887695f,
        -0.431655884f, -1.127746582f, 0.711318970f, -0.044784546f, 0.105819702f, -0.026535034f,
        0.011886597f, -0.003479004f, 0.000686646f, 0.000015259f, -0.000289917f, -0.002899170f,
        -0.003723145f, -0.031814575f, -0.056533813f, -0.129577637f, -0.459472656f, -1.133926392f,
        0.683914185f, -0.057617188f, 0.100540161f, -0.027801514f, 0.010848999f, -0.003463745f,
        0.000625610f, 0.000015259f, -0.000320435f, -0.002990723f, -0.004486084f, -0.031845093f,
        -0.061996460f, -0.123474121f, -0.487472534f, -1.138763428f, 0.656219482f, -0.069595337f,
        0.095169067f, -0.028884888f, 0.009841919f, -0.003433228f, 0.000579834f, 0.000015259f,
        -0.000366211f, -0.003082275f, -0.005294800f, -0.031738281f, -0.067520142f, -0.116577148f,
        -0.515609741f, -1.142211914f, 0.628295898f, -0.080688477f, 0.089706421f, -0.029785156f,
        0.008865356f, -0.003387451f, 0.000534058f, 0.000015259f, -0.000396729f, -0.003173828f,
        -0.006118774f, -0.031478882f, -0.073059082f, -0.108856201f, -0.543823242f, -1.144287109f,
        0.600219727f, -0.090927124f, 0.084182739f, -0.030517578f, 0.007919312f, -0.003326416f,
        0.000473022f, 0.000015259f,
    ];

    /// <summary>
    /// Contructor.
    /// The scalefactor scales the calculated float pcm samples to short values
    /// (raw pcm samples are in [-1.0, 1.0], if no violations occur).
    /// </summary>
    public SynthesisFilter( int channelnumber, float factor, float[]? eq0 )
    {
        InitBlock();

        if ( _d == null )
        {
            _d   = _dData; // load_d();
            _d16 = SplitArray( _d, 16 );
        }

        _v1          = new float[ 512 ];
        _v2          = new float[ 512 ];
        _samples     = new float[ 32 ];
        _channel     = channelnumber;
        _scalefactor = factor;
        Eq           = _eq;

        Reset();
    }

    /// <summary>
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public float[]? Eq
    {
        set
        {
            _eq = value;

            if ( _eq == null )
            {
                _eq = new float[ 32 ];

                for ( var i = 0; i < 32; i++ )
                {
                    _eq[ i ] = 1.0f;
                }
            }

            if ( _eq.Length < 32 )
            {
                throw new GdxRuntimeException( "eq0" );
            }
        }
    }

    /// <summary>
    /// </summary>
    private void InitBlock()
    {
        _tmpOut = new float[ 32 ];
    }

    /// <summary>
    /// Reset the synthesis filter.
    /// </summary>
    public void Reset()
    {
        // initialize v1[] and v2[]:
        for ( var p = 0; p < 512; p++ )
        {
            _v1[ p ] = _v2[ p ] = 0.0f;
        }

        // initialize samples[]:
        for ( var p2 = 0; p2 < 32; p2++ )
        {
            _samples[ p2 ] = 0.0f;
        }

        _actualV        = _v1;
        _actualWritePos = 15;
    }

    /// <summary>
    /// </summary>
    /// <param name="sample"></param>
    /// <param name="subbandnumber"></param>
    public void AddSample( float sample, int subbandnumber )
    {
        if ( _eq != null )
        {
            _samples[ subbandnumber ] = _eq[ subbandnumber ] * sample;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="s"></param>
    public void AddSamples( float[] s )
    {
        if ( _eq != null )
        {
            for ( var i = 31; i >= 0; i-- )
            {
                _samples[ i ] = s[ i ] * _eq[ i ];
            }
        }
    }

    /// <summary>
    /// Compute new values via a fast cosine transform.
    /// </summary>
    private void ComputeNewValues()
    {
        float newV1,  newV2,  newV3,  newV4,  newV8,  newV9;
        float newV10, newV11, newV12, newV13, newV14, newV15;

        var s = _samples;

        var s0  = s[ 0 ];
        var s1  = s[ 1 ];
        var s2  = s[ 2 ];
        var s3  = s[ 3 ];
        var s4  = s[ 4 ];
        var s5  = s[ 5 ];
        var s6  = s[ 6 ];
        var s7  = s[ 7 ];
        var s8  = s[ 8 ];
        var s9  = s[ 9 ];
        var s10 = s[ 10 ];
        var s11 = s[ 11 ];
        var s12 = s[ 12 ];
        var s13 = s[ 13 ];
        var s14 = s[ 14 ];
        var s15 = s[ 15 ];
        var s16 = s[ 16 ];
        var s17 = s[ 17 ];
        var s18 = s[ 18 ];
        var s19 = s[ 19 ];
        var s20 = s[ 20 ];
        var s21 = s[ 21 ];
        var s22 = s[ 22 ];
        var s23 = s[ 23 ];
        var s24 = s[ 24 ];
        var s25 = s[ 25 ];
        var s26 = s[ 26 ];
        var s27 = s[ 27 ];
        var s28 = s[ 28 ];
        var s29 = s[ 29 ];
        var s30 = s[ 30 ];
        var s31 = s[ 31 ];

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
        var pp8  = ( p0 - p15 ) * _cos132;
        var pp9  = ( p1 - p14 ) * _cos332;
        var pp10 = ( p2 - p13 ) * _cos532;
        var pp11 = ( p3 - p12 ) * _cos732;
        var pp12 = ( p4 - p11 ) * _cos932;
        var pp13 = ( p5 - p10 ) * _cos1132;
        var pp14 = ( p6 - p9 ) * _cos1332;
        var pp15 = ( p7 - p8 ) * _cos1532;

        p0  = pp0 + pp7;
        p1  = pp1 + pp6;
        p2  = pp2 + pp5;
        p3  = pp3 + pp4;
        p4  = ( pp0 - pp7 ) * _cos116;
        p5  = ( pp1 - pp6 ) * _cos316;
        p6  = ( pp2 - pp5 ) * _cos516;
        p7  = ( pp3 - pp4 ) * _cos716;
        p8  = pp8 + pp15;
        p9  = pp9 + pp14;
        p10 = pp10 + pp13;
        p11 = pp11 + pp12;
        p12 = ( pp8 - pp15 ) * _cos116;
        p13 = ( pp9 - pp14 ) * _cos316;
        p14 = ( pp10 - pp13 ) * _cos516;
        p15 = ( pp11 - pp12 ) * _cos716;

        pp0  = p0 + p3;
        pp1  = p1 + p2;
        pp2  = ( p0 - p3 ) * _cos18;
        pp3  = ( p1 - p2 ) * _cos38;
        pp4  = p4 + p7;
        pp5  = p5 + p6;
        pp6  = ( p4 - p7 ) * _cos18;
        pp7  = ( p5 - p6 ) * _cos38;
        pp8  = p8 + p11;
        pp9  = p9 + p10;
        pp10 = ( p8 - p11 ) * _cos18;
        pp11 = ( p9 - p10 ) * _cos38;
        pp12 = p12 + p15;
        pp13 = p13 + p14;
        pp14 = ( p12 - p15 ) * _cos18;
        pp15 = ( p13 - p14 ) * _cos38;

        p0  = pp0 + pp1;
        p1  = ( pp0 - pp1 ) * _cos14;
        p2  = pp2 + pp3;
        p3  = ( pp2 - pp3 ) * _cos14;
        p4  = pp4 + pp5;
        p5  = ( pp4 - pp5 ) * _cos14;
        p6  = pp6 + pp7;
        p7  = ( pp6 - pp7 ) * _cos14;
        p8  = pp8 + pp9;
        p9  = ( pp8 - pp9 ) * _cos14;
        p10 = pp10 + pp11;
        p11 = ( pp10 - pp11 ) * _cos14;
        p12 = pp12 + pp13;
        p13 = ( pp12 - pp13 ) * _cos14;
        p14 = pp14 + pp15;
        p15 = ( pp14 - pp15 ) * _cos14;

        // this is pretty insane coding
        float tmp1;

        var newV19 = -( newV4 = ( newV12 = p7 ) + p5 ) - p6;
        var newV27 = -p6 - p7 - p4;
        var newV6  = ( newV10 = ( newV14 = p15 ) + p11 ) + p13;
        var newV17 = -( newV2 = p15 + p13 + p9 ) - p14;
        var newV21 = ( tmp1 = -p14 - p15 - p10 - p11 ) - p13;
        var newV29 = -p14 - p15 - p12 - p8;
        var newV25 = tmp1 - p12;
        var newV31 = -p0;

        var newV0 = p1;

        var newV23 = -( newV8 = p3 ) - p2;

        p0  = ( s0 - s31 ) * _cos164;
        p1  = ( s1 - s30 ) * _cos364;
        p2  = ( s2 - s29 ) * _cos564;
        p3  = ( s3 - s28 ) * _cos764;
        p4  = ( s4 - s27 ) * _cos964;
        p5  = ( s5 - s26 ) * _cos1164;
        p6  = ( s6 - s25 ) * _cos1364;
        p7  = ( s7 - s24 ) * _cos1564;
        p8  = ( s8 - s23 ) * _cos1764;
        p9  = ( s9 - s22 ) * _cos1964;
        p10 = ( s10 - s21 ) * _cos2164;
        p11 = ( s11 - s20 ) * _cos2364;
        p12 = ( s12 - s19 ) * _cos2564;
        p13 = ( s13 - s18 ) * _cos2764;
        p14 = ( s14 - s17 ) * _cos2964;
        p15 = ( s15 - s16 ) * _cos3164;

        pp0  = p0 + p15;
        pp1  = p1 + p14;
        pp2  = p2 + p13;
        pp3  = p3 + p12;
        pp4  = p4 + p11;
        pp5  = p5 + p10;
        pp6  = p6 + p9;
        pp7  = p7 + p8;
        pp8  = ( p0 - p15 ) * _cos132;
        pp9  = ( p1 - p14 ) * _cos332;
        pp10 = ( p2 - p13 ) * _cos532;
        pp11 = ( p3 - p12 ) * _cos732;
        pp12 = ( p4 - p11 ) * _cos932;
        pp13 = ( p5 - p10 ) * _cos1132;
        pp14 = ( p6 - p9 ) * _cos1332;
        pp15 = ( p7 - p8 ) * _cos1532;

        p0  = pp0 + pp7;
        p1  = pp1 + pp6;
        p2  = pp2 + pp5;
        p3  = pp3 + pp4;
        p4  = ( pp0 - pp7 ) * _cos116;
        p5  = ( pp1 - pp6 ) * _cos316;
        p6  = ( pp2 - pp5 ) * _cos516;
        p7  = ( pp3 - pp4 ) * _cos716;
        p8  = pp8 + pp15;
        p9  = pp9 + pp14;
        p10 = pp10 + pp13;
        p11 = pp11 + pp12;
        p12 = ( pp8 - pp15 ) * _cos116;
        p13 = ( pp9 - pp14 ) * _cos316;
        p14 = ( pp10 - pp13 ) * _cos516;
        p15 = ( pp11 - pp12 ) * _cos716;

        pp0  = p0 + p3;
        pp1  = p1 + p2;
        pp2  = ( p0 - p3 ) * _cos18;
        pp3  = ( p1 - p2 ) * _cos38;
        pp4  = p4 + p7;
        pp5  = p5 + p6;
        pp6  = ( p4 - p7 ) * _cos18;
        pp7  = ( p5 - p6 ) * _cos38;
        pp8  = p8 + p11;
        pp9  = p9 + p10;
        pp10 = ( p8 - p11 ) * _cos18;
        pp11 = ( p9 - p10 ) * _cos38;
        pp12 = p12 + p15;
        pp13 = p13 + p14;
        pp14 = ( p12 - p15 ) * _cos18;
        pp15 = ( p13 - p14 ) * _cos38;

        p0  = pp0 + pp1;
        p1  = ( pp0 - pp1 ) * _cos14;
        p2  = pp2 + pp3;
        p3  = ( pp2 - pp3 ) * _cos14;
        p4  = pp4 + pp5;
        p5  = ( pp4 - pp5 ) * _cos14;
        p6  = pp6 + pp7;
        p7  = ( pp6 - pp7 ) * _cos14;
        p8  = pp8 + pp9;
        p9  = ( pp8 - pp9 ) * _cos14;
        p10 = pp10 + pp11;
        p11 = ( pp10 - pp11 ) * _cos14;
        p12 = pp12 + pp13;
        p13 = ( pp12 - pp13 ) * _cos14;
        p14 = pp14 + pp15;
        p15 = ( pp14 - pp15 ) * _cos14;

        // manually doing something that a compiler should handle sucks
        // coding like this is hard to read
        float tmp2;
        var   newV5  = ( newV11 = ( newV13 = ( newV15 = p15 ) + p7 ) + p11 ) + p5 + p13;
        var   newV7  = ( newV9 = p15 + p11 + p3 ) + p13;
        var   newV16 = -( newV1 = ( tmp1 = p13 + p15 + p9 ) + p1 ) - p14;
        var   newV18 = -( newV3 = tmp1 + p5 + p7 ) - p6 - p14;

        var newV22 = ( tmp1 = -p10 - p11 - p14 - p15 ) - p13 - p2 - p3;
        var newV20 = tmp1 - p13 - p5 - p6 - p7;
        var newV24 = tmp1 - p12 - p2 - p3;
        var newV26 = tmp1 - p12 - ( tmp2 = p4 + p6 + p7 );
        var newV30 = ( tmp1 = -p8 - p12 - p14 - p15 ) - p0;
        var newV28 = tmp1 - tmp2;

        // insert V[0-15] (== new_v[0-15]) into actual v:
        // float[] x2 = actual_v + actual_write_pos;
        var dest = _actualV;

        var pos = _actualWritePos;

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
        dest = _actualV == _v1 ? _v2 : _v1;

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
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSamples0( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 0 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSamples1( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 1 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSamples2( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 2 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSamples3( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 3 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample4( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 4 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample5( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 5 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample6( AudioBase buffer )
    {
        var dvp = 0;

        // fat chance of having this loop unroll
        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 6 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample7( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 7 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample8( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 8 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample9( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 9 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample10( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 10 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample11( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 11 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample12( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 12 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample13( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 13 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSample14( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 14 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 15 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSamples15( AudioBase buffer )
    {
        var dvp = 0;

        for ( var i = 0; i < 32; i++ )
        {
            var pcSample = ( ( _actualV[ 15 + dvp ] * _d16![ i ][ 0 ] )
                           + ( _actualV[ 14 + dvp ] * _d16[ i ][ 1 ] )
                           + ( _actualV[ 13 + dvp ] * _d16[ i ][ 2 ] )
                           + ( _actualV[ 12 + dvp ] * _d16[ i ][ 3 ] )
                           + ( _actualV[ 11 + dvp ] * _d16[ i ][ 4 ] )
                           + ( _actualV[ 10 + dvp ] * _d16[ i ][ 5 ] )
                           + ( _actualV[ 9 + dvp ] * _d16[ i ][ 6 ] )
                           + ( _actualV[ 8 + dvp ] * _d16[ i ][ 7 ] )
                           + ( _actualV[ 7 + dvp ] * _d16[ i ][ 8 ] )
                           + ( _actualV[ 6 + dvp ] * _d16[ i ][ 9 ] )
                           + ( _actualV[ 5 + dvp ] * _d16[ i ][ 10 ] )
                           + ( _actualV[ 4 + dvp ] * _d16[ i ][ 11 ] )
                           + ( _actualV[ 3 + dvp ] * _d16[ i ][ 12 ] )
                           + ( _actualV[ 2 + dvp ] * _d16[ i ][ 13 ] )
                           + ( _actualV[ 1 + dvp ] * _d16[ i ][ 14 ] )
                           + ( _actualV[ 0 + dvp ] * _d16[ i ][ 15 ] ) )
                         * _scalefactor;

            _tmpOut[ i ] =  pcSample;
            dvp          += 16;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    private void ComputePcSamples( AudioBase buffer )
    {
        switch ( _actualWritePos )
        {
            case 0:
                ComputePcSamples0( buffer );

                break;

            case 1:
                ComputePcSamples1( buffer );

                break;

            case 2:
                ComputePcSamples2( buffer );

                break;

            case 3:
                ComputePcSamples3( buffer );

                break;

            case 4:
                ComputePcSample4( buffer );

                break;

            case 5:
                ComputePcSample5( buffer );

                break;

            case 6:
                ComputePcSample6( buffer );

                break;

            case 7:
                ComputePcSample7( buffer );

                break;

            case 8:
                ComputePcSample8( buffer );

                break;

            case 9:
                ComputePcSample9( buffer );

                break;

            case 10:
                ComputePcSample10( buffer );

                break;

            case 11:
                ComputePcSample11( buffer );

                break;

            case 12:
                ComputePcSample12( buffer );

                break;

            case 13:
                ComputePcSample13( buffer );

                break;

            case 14:
                ComputePcSample14( buffer );

                break;

            case 15:
                ComputePcSamples15( buffer );

                break;
        }

        buffer.AppendSamples( _channel, _tmpOut );
    }

    /// <summary>
    /// Calculate 32 PCM samples and put the into the Obuffer-object.
    /// </summary>
    public void CalculatePcSamples( AudioBase? buffer )
    {
        ArgumentNullException.ThrowIfNull( buffer );

        ComputeNewValues();
        ComputePcSamples( buffer );

        _actualWritePos = ( _actualWritePos + 1 ) & 0xf;
        _actualV        = _actualV == _v1 ? _v2 : _v1;

        // initialize samples[]:
        //for (register float *floatp = samples + 32; floatp > samples; )
        // *--floatp = 0.0f;  

        // TODO: this may not be necessary. The Layer III decoder always
        // outputs 32 subband samples, but I haven't checked layer I & II.
        for ( var p = 0; p < 32; p++ )
        {
            _samples[ p ] = 0.0f;
        }
    }

    /// <summary>
    /// Converts a 1D array into a number of smaller arrays. This is used achieve offset
    /// + constant indexing into an array. Each sub-array represents a block of values of
    /// the original array.
    /// </summary>
    /// <param name="array"> The array to split up into blocks. </param>
    /// <param name="blockSize">
    /// The size of the blocks to split the array into. This must be an exact divisor of
    /// the length of the array, or some data will be lost from the main array.
    /// </param>
    /// <returns>
    /// An array of arrays in which each element in the returned array will be of length blockSize.
    /// </returns>
    private static float[][] SplitArray( float[] array, int blockSize )
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
    /// </summary>
    /// <param name="array"></param>
    /// <param name="offs"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    private static float[] SubArray( float[] array, int offs, int len )
    {
        if ( ( offs + len ) > array.Length )
        {
            len = array.Length - offs;
        }

        if ( len < 0 )
        {
            len = 0;
        }

        var subarray = new float[ len ];

        for ( var i = 0; i < len; i++ )
        {
            subarray[ i ] = array[ offs + i ];
        }

        return subarray;
    }
}
