// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Audio.MP3Sharp;

/// <summary>
///     public class for layer I subbands in stereo mode.
/// </summary>
public class SubbandLayer1Stereo : SubbandLayer1
{
    protected int   channel2Allocation;
    protected float channel2Factor;
    protected float channel2Offset;
    protected float channel2Sample;
    protected int   channel2Samplelength;
    protected float channel2Scalefactor;

    public SubbandLayer1Stereo( int subbandnumber )
        : base( subbandnumber )
    {
    }

    /// <summary>
    /// </summary>
    public override void ReadAllocation( Bitstream stream, Header? header, Crc16? crc )
    {
        allocation = stream.GetBitsFromBuffer( 4 );

        if ( allocation > 14 )
        {
            return;
        }

        channel2Allocation = stream.GetBitsFromBuffer( 4 );

        if ( crc != null )
        {
            crc.AddBits( allocation, 4 );
            crc.AddBits( channel2Allocation, 4 );
        }

        if ( allocation != 0 )
        {
            samplelength = allocation + 1;
            factor       = TableFactor[ allocation ];
            offset       = TableOffset[ allocation ];
        }

        if ( channel2Allocation != 0 )
        {
            channel2Samplelength = channel2Allocation + 1;
            channel2Factor       = TableFactor[ channel2Allocation ];
            channel2Offset       = TableOffset[ channel2Allocation ];
        }
    }

    /// <summary>
    /// </summary>
    public override void ReadScaleFactor( Bitstream stream, Header? header )
    {
        if ( allocation != 0 )
        {
            scalefactor = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
        }

        if ( channel2Allocation != 0 )
        {
            channel2Scalefactor = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
        }
    }

    /// <summary>
    /// </summary>
    public override bool ReadSampleData( Bitstream stream )
    {
        var returnvalue = base.ReadSampleData( stream );

        if ( channel2Allocation != 0 )
        {
            channel2Sample = stream.GetBitsFromBuffer( channel2Samplelength );
        }

        return returnvalue;
    }

    /// <summary>
    /// </summary>
    public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
    {
        base.PutNextSample( channels, filter1, filter2 );

        if ( ( channel2Allocation != 0 ) && ( channels != OutputChannels.LEFT_CHANNEL ) )
        {
            var sample2 = ( ( channel2Sample * channel2Factor ) + channel2Offset ) * channel2Scalefactor;

            if ( channels == OutputChannels.BOTH_CHANNELS )
            {
                filter2?.AddSample( sample2, subbandnumber );
            }
            else
            {
                filter1?.AddSample( sample2, subbandnumber );
            }
        }

        return true;
    }
}
