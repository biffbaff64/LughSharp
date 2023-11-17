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

    public SynthesisFilter( int channelNumber, float scalefactor, float[]? eq0 = null )
    {
    }

    public void Reset()
    {
    }

    public void InputSample( float scaledSample, int subbandnumber )
    {
    }

    public void InputSamples( float[] samples )
    {
    }

    public void ComputeNewV()
    {
    }

    /// <summary>
    /// Calculate 32 PCM samples and put them into the OutputBuffer object.
    /// </summary>
    public void CalculatePcmSamples( out OutputBuffer? buffer )
    {
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

    private void ComputePcmSamples0( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples1( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples2( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples3( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples4( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples5( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples6( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples7( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples8( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples9( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples10( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples11( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples12( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples13( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples14( out OutputBuffer? buffer )
    {
    }

    private void ComputePcmSamples15( out OutputBuffer? buffer )
    {
    }

    private readonly static double MyPI = 3.14159265358979323846;

    private readonly static float Cos164  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 64.0 ) ) );
    private readonly static float Cos364  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 3.0 / 64.0 ) ) );
    private readonly static float Cos564  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 5.0 / 64.0 ) ) );
    private readonly static float Cos764  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 7.0 / 64.0 ) ) );
    private readonly static float Cos964  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 9.0 / 64.0 ) ) );
    private readonly static float Cos1164 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 11.0 / 64.0 ) ) );
    private readonly static float Cos1364 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 13.0 / 64.0 ) ) );
    private readonly static float Cos1564 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 15.0 / 64.0 ) ) );
    private readonly static float Cos1764 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 17.0 / 64.0 ) ) );
    private readonly static float Cos1964 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 19.0 / 64.0 ) ) );
    private readonly static float Cos2164 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 21.0 / 64.0 ) ) );
    private readonly static float Cos2364 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 23.0 / 64.0 ) ) );
    private readonly static float Cos2564 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 25.0 / 64.0 ) ) );
    private readonly static float Cos2764 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 27.0 / 64.0 ) ) );
    private readonly static float Cos2964 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 29.0 / 64.0 ) ) );
    private readonly static float Cos3164 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 31.0 / 64.0 ) ) );
    private readonly static float Cos132  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 32.0 ) ) );
    private readonly static float Cos332  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 3.0 / 32.0 ) ) );
    private readonly static float Cos532  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 5.0 / 32.0 ) ) );
    private readonly static float Cos732  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 7.0 / 32.0 ) ) );
    private readonly static float Cos932  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 9.0 / 32.0 ) ) );
    private readonly static float Cos1132 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 11.0 / 32.0 ) ) );
    private readonly static float Cos1332 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 13.0 / 32.0 ) ) );
    private readonly static float Cos1532 = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 15.0 / 32.0 ) ) );
    private readonly static float Cos116  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 16.0 ) ) );
    private readonly static float Cos316  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 3.0 / 16.0 ) ) );
    private readonly static float Cos516  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 5.0 / 16.0 ) ) );
    private readonly static float Cos716  = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 7.0 / 16.0 ) ) );
    private readonly static float Cos18   = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 8.0 ) ) );
    private readonly static float Cos38   = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI * 3.0 / 8.0 ) ) );
    private readonly static float Cos14   = ( float )( 1.0 / ( 2.0 * Math.Cos( MyPI / 4.0 ) ) );

    // Note: These values are not in the same order
    // as in Annex 3-B.3 of the ISO/IEC DIS 11172-3
    // private float d[] = {0.000000000, -4.000442505};

    private static float[]? _d = null;

    // d[] split into subarrays of length 16. This provides for more faster
    // access by allowing a block of 16 to be addressed with constant offset.
    private static float[ , ]? _d16 = null;

    /// <summary>
    /// Loads the data for the d[] from the resource SFd.ser.
    /// </summary>
    /// <returns>the loaded values for d[]</returns>
    private float[] LoadD()
    {
        try
        {

        }
        catch ( IOException ex )
        {
            throw new ExceptionInitialiserError( ex );
        }
    }
}
