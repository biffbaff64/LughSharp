// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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
/// public class for layer I subbands in joint stereo mode.
/// </summary>
[PublicAPI]
public class SubbandLayer1IntensityStereo : SubbandLayer1
{
    protected float channel2Scalefactor;

    public SubbandLayer1IntensityStereo( int subbandnumber )
        : base( subbandnumber )
    {
    }

    /// <summary>
    /// </summary>
    public override void ReadScaleFactor( Bitstream stream, Header? header )
    {
        if ( allocation != 0 )
        {
            scalefactor         = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
            channel2Scalefactor = ScaleFactors[ stream.GetBitsFromBuffer( 6 ) ];
        }
    }

    /// <summary>
    /// </summary>
    public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
    {
        if ( allocation != 0 )
        {
            sample = ( sample * factor ) + offset; // requantization

            switch ( channels )
            {
                case OutputChannels.BOTH_CHANNELS:
                {
                    float sample1 = sample * scalefactor, sample2 = sample * channel2Scalefactor;
                    filter1?.AddSample( sample1, subbandnumber );
                    filter2?.AddSample( sample2, subbandnumber );

                    break;
                }

                case OutputChannels.LEFT_CHANNEL:
                {
                    var sample1 = sample * scalefactor;
                    filter1?.AddSample( sample1, subbandnumber );

                    break;
                }

                default:
                {
                    var sample2 = sample * channel2Scalefactor;
                    filter1?.AddSample( sample2, subbandnumber );

                    break;
                }
            }
        }

        return true;
    }
}
