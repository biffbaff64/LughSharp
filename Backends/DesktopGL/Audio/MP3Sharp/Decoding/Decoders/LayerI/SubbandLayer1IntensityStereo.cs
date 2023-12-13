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

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;

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
    /// 
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
    /// 
    /// </summary>
    public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
    {
        if ( allocation != 0 )
        {
            sample = ( sample * factor ) + offset; // requantization

            if ( channels == OutputChannels.BOTH_CHANNELS )
            {
                float sample1 = sample * scalefactor, sample2 = sample * channel2Scalefactor;
                filter1?.AddSample( sample1, subbandnumber );
                filter2?.AddSample( sample2, subbandnumber );
            }
            else if ( channels == OutputChannels.LEFT_CHANNEL )
            {
                var sample1 = sample * scalefactor;
                filter1?.AddSample( sample1, subbandnumber );
            }
            else
            {
                var sample2 = sample * channel2Scalefactor;
                filter1?.AddSample( sample2, subbandnumber );
            }
        }

        return true;
    }
}
