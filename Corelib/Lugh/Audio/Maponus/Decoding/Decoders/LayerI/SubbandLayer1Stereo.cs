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


namespace Corelib.Lugh.Audio.Maponus.Decoding.Decoders.LayerI;

/// <summary>
/// public class for layer I subbands in stereo mode.
/// </summary>
[PublicAPI]
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
