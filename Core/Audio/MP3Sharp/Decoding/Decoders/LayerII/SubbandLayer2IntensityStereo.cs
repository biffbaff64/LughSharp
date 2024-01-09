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

using LibGDXSharp.Audio.MP3Sharp.Decoding;

namespace LibGDXSharp.Audio.MP3Sharp.Decoders.LayerII;

/// <summary>
///     public class for layer II subbands in joint stereo mode.
/// </summary>
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
                                  <= 4 => scalefactor1,
                                  <= 8 => scalefactor2,
                                  _    => scalefactor3
                              };

                    filter1?.AddSample( sample, subbandnumber );

                    break;

                default:
                    sample *= groupnumber switch
                              {
                                  <= 4 => channel2Scalefactor1,
                                  <= 8 => channel2Scalefactor2,
                                  _    => channel2Scalefactor3
                              };

                    filter1?.AddSample( sample, subbandnumber );

                    break;
            }
        }

        return ++samplenumber == 3;
    }
}
