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
///     public class for layer II subbands in stereo mode.
/// </summary>
public class SubbandLayer2Stereo : SubbandLayer2
{
    protected readonly float[] channel2C          = { 0 };
    protected readonly int[]   channel2Codelength = { 0 };
    protected readonly float[] channel2D          = { 0 };
    protected readonly float[] channel2Factor     = { 0 };
    protected readonly float[] channel2Samples;
    protected          int     channel2Allocation;
    protected          float   channel2Scalefactor1;
    protected          float   channel2Scalefactor2;
    protected          float   channel2Scalefactor3;
    protected          int     channel2Scfsi;

    public SubbandLayer2Stereo( int subbandnumber )
        : base( subbandnumber ) => channel2Samples = new float[ 3 ];

    /// <summary>
    /// </summary>
    public override void ReadAllocation( Bitstream stream, Header? header, Crc16? crc )
    {
        var length = GetAllocationLength( header );
        allocation         = stream.GetBitsFromBuffer( length );
        channel2Allocation = stream.GetBitsFromBuffer( length );

        if ( crc != null )
        {
            crc.AddBits( allocation, length );
            crc.AddBits( channel2Allocation, length );
        }
    }

    /// <summary>
    /// </summary>
    public override void ReadScaleFactorSelection( Bitstream stream, Crc16? crc )
    {
        if ( allocation != 0 )
        {
            scfsi = stream.GetBitsFromBuffer( 2 );
            crc?.AddBits( scfsi, 2 );
        }

        if ( channel2Allocation != 0 )
        {
            channel2Scfsi = stream.GetBitsFromBuffer( 2 );
            crc?.AddBits( channel2Scfsi, 2 );
        }
    }

    /// <summary>
    /// </summary>
    public override void ReadScaleFactor( Bitstream stream, Header? header )
    {
        base.ReadScaleFactor( stream, header );

        if ( channel2Allocation != 0 )
        {
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

            PrepareForSampleRead( header,
                                  channel2Allocation,
                                  1,
                                  channel2Factor,
                                  channel2Codelength,
                                  channel2C,
                                  channel2D );
        }
    }

    /// <summary>
    /// </summary>
    public override bool ReadSampleData( Bitstream stream )
    {
        var returnvalue = base.ReadSampleData( stream );

        if ( channel2Allocation != 0 )
        {
            if ( groupingtable[ 1 ] != null )
            {
                var samplecode = stream.GetBitsFromBuffer( channel2Codelength[ 0 ] );

                // create requantized samples:
                samplecode += samplecode << 1;
                /*
                float[] target = channel2_samples;
                float[] source = channel2_groupingtable[0];
                int tmp = 0;
                int temp = 0;
                target[tmp++] = source[samplecode + temp];
                temp++;
                target[tmp++] = source[samplecode + temp];
                temp++;
                target[tmp] = source[samplecode + temp];
                // memcpy (channel2_samples, channel2_groupingtable + samplecode, 3 * sizeof (real));
                */
                var target = channel2Samples;
                var source = groupingtable[ 1 ];
                var tmp    = 0;
                var temp   = samplecode;

                target[ tmp ] = source![ temp ];
                temp++;
                tmp++;
                target[ tmp ] = source[ temp ];
                temp++;
                tmp++;
                target[ tmp ] = source[ temp ];
            }
            else
            {
                channel2Samples[ 0 ] =
                    ( float )( ( stream.GetBitsFromBuffer( channel2Codelength[ 0 ] ) * channel2Factor[ 0 ] ) - 1.0 );

                channel2Samples[ 1 ] =
                    ( float )( ( stream.GetBitsFromBuffer( channel2Codelength[ 0 ] ) * channel2Factor[ 0 ] ) - 1.0 );

                channel2Samples[ 2 ] =
                    ( float )( ( stream.GetBitsFromBuffer( channel2Codelength[ 0 ] ) * channel2Factor[ 0 ] ) - 1.0 );
            }
        }

        return returnvalue;
    }

    /// <summary>
    /// </summary>
    public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
    {
        var returnvalue = base.PutNextSample( channels, filter1, filter2 );

        if ( ( channel2Allocation != 0 ) && ( channels != OutputChannels.LEFT_CHANNEL ) )
        {
            var sample = channel2Samples[ samplenumber - 1 ];

            if ( groupingtable[ 1 ] == null )
            {
                sample = ( sample + channel2D[ 0 ] ) * channel2C[ 0 ];
            }

            sample *= groupnumber switch
                      {
                          <= 4 => channel2Scalefactor1,
                          <= 8 => channel2Scalefactor2,
                          _    => channel2Scalefactor3
                      };

            if ( channels == OutputChannels.BOTH_CHANNELS )
            {
                filter2?.AddSample( sample, subbandnumber );
            }
            else
            {
                filter1?.AddSample( sample, subbandnumber );
            }
        }

        return returnvalue;
    }
}
