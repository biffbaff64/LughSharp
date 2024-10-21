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


namespace Corelib.LibCore.Audio.Maponus.Decoding.Decoders.LayerII;

/// <summary>
/// public class for layer II subbands in stereo mode.
/// </summary>
[PublicAPI]
public class SubbandLayer2Stereo : SubbandLayer2
{
    protected readonly float[] channel2C          = [ 0 ];
    protected readonly int[]   channel2Codelength = [ 0 ];
    protected readonly float[] channel2D          = [ 0 ];
    protected readonly float[] channel2Factor     = [ 0 ];
    protected readonly float[] channel2Samples;
    protected          int     channel2Allocation;
    protected          float   channel2Scalefactor1;
    protected          float   channel2Scalefactor2;
    protected          float   channel2Scalefactor3;
    protected          int     channel2Scfsi;

    public SubbandLayer2Stereo( int subbandnumber )
        : base( subbandnumber )
    {
        channel2Samples = new float[ 3 ];
    }

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
                    ( float ) ( ( stream.GetBitsFromBuffer( channel2Codelength[ 0 ] ) * channel2Factor[ 0 ] ) - 1.0 );

                channel2Samples[ 1 ] =
                    ( float ) ( ( stream.GetBitsFromBuffer( channel2Codelength[ 0 ] ) * channel2Factor[ 0 ] ) - 1.0 );

                channel2Samples[ 2 ] =
                    ( float ) ( ( stream.GetBitsFromBuffer( channel2Codelength[ 0 ] ) * channel2Factor[ 0 ] ) - 1.0 );
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
                <= 4  => channel2Scalefactor1,
                <= 8  => channel2Scalefactor2,
                var _ => channel2Scalefactor3
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
