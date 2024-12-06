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


namespace Corelib.Lugh.Audio.Maponus.Decoding.Decoders.LayerII;

/// <summary>
/// public class for layer II subbands in joint stereo mode.
/// </summary>
[PublicAPI]
public class SubbandLayer2IntensityStereo : SubbandLayer2
{
    protected float channel2Scalefactor1;
    protected float channel2Scalefactor2;
    protected float channel2Scalefactor3;
    protected int   channel2Scfsi;

    public SubbandLayer2IntensityStereo( int subbandnumber )
        : base( subbandnumber )
    {
    }

    /// <summary>
    /// </summary>
    public override void ReadScaleFactorSelection( Bitstream stream, Crc16? crc )
    {
        if ( allocation != 0 )
        {
            scfsi         = stream.GetBitsFromBuffer( 2 );
            channel2Scfsi = stream.GetBitsFromBuffer( 2 );

            if ( crc != null )
            {
                crc.AddBits( scfsi, 2 );
                crc.AddBits( channel2Scfsi, 2 );
            }
        }
    }

    /// <summary>
    /// </summary>
    public override void ReadScaleFactor( Bitstream stream, Header? header )
    {
        if ( allocation != 0 )
        {
            base.ReadScaleFactor( stream, header );

            if ( channel2Scfsi is >= 0 and <= 3 )
            {
                channel2Scalefactor1 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
                channel2Scalefactor2 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
                channel2Scalefactor3 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
            }
        }

        if ( allocation != 0 )
        {
            base.ReadScaleFactor( stream, header );

            switch ( channel2Scfsi )
            {
                case 0:
                    channel2Scalefactor1 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
                    channel2Scalefactor2 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
                    channel2Scalefactor3 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];

                    break;

                case 1:
                    channel2Scalefactor1 =
                        channel2Scalefactor2 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];

                    channel2Scalefactor3 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];

                    break;

                case 2:
                    channel2Scalefactor1 =
                        channel2Scalefactor2 =
                            channel2Scalefactor3 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];

                    break;

                case 3:
                    channel2Scalefactor1 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];

                    channel2Scalefactor2 =
                        channel2Scalefactor3 = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];

                    break;
            }
        }
    }

    /// <summary>
    /// </summary>
    public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
    {
        if ( allocation != 0 )
        {
            var sample = samples[ samplenumber ];

            if ( groupingtable[ 0 ] == null )
            {
                sample = ( sample + d[ 0 ] ) * cFactor[ 0 ];
            }

            switch ( channels )
            {
                case OutputChannels.BOTH_CHANNELS:
                {
                    var sample2 = sample;

                    switch ( groupnumber )
                    {
                        case <= 4:
                            sample  *= scalefactor1;
                            sample2 *= channel2Scalefactor1;

                            break;

                        case <= 8:
                            sample  *= scalefactor2;
                            sample2 *= channel2Scalefactor2;

                            break;

                        default:
                            sample  *= scalefactor3;
                            sample2 *= channel2Scalefactor3;

                            break;
                    }

                    filter1?.AddSample( sample, subbandnumber );
                    filter2?.AddSample( sample2, subbandnumber );

                    break;
                }

                case OutputChannels.LEFT_CHANNEL:
                    sample *= groupnumber switch
                    {
                        <= 4  => scalefactor1,
                        <= 8  => scalefactor2,
                        var _ => scalefactor3,
                    };

                    filter1?.AddSample( sample, subbandnumber );

                    break;

                default:
                    sample *= groupnumber switch
                    {
                        <= 4  => channel2Scalefactor1,
                        <= 8  => channel2Scalefactor2,
                        var _ => channel2Scalefactor3,
                    };

                    filter1?.AddSample( sample, subbandnumber );

                    break;
            }
        }

        return ++samplenumber == 3;
    }
}
