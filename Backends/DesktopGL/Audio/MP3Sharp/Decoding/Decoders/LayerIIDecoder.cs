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
/// Implements decoding of MPEG Audio Layer II frames.
/// </summary>
[PublicAPI]
public class LayerIIDecoder : LayerIDecoder
{
    protected override void CreateSubbands()
    {
        switch ( mode )
        {
            case Header.SINGLE_CHANNEL:
            {
                for ( var i = 0; i < nuSubbands; ++i )
                {
                    subbands[ i ] = new SubbandLayer2( i );
                }

                break;
            }

            case Header.JOINT_STEREO:
            {
                int i;
                
                for ( i = 0; i < header?.IntensityStereoBound(); ++i )
                {
                    subbands[ i ] = new SubbandLayer2Stereo( i );
                }

                for ( ; i < nuSubbands; ++i )
                {
                    subbands[ i ] = new SubbandLayer2IntensityStereo( i );
                }

                break;
            }

            default:
            {
                for ( var i = 0; i < nuSubbands; ++i )
                {
                    subbands[ i ] = new SubbandLayer2Stereo( i );
                }

                break;
            }
        }
    }

    protected override void ReadScaleFactorSelection()
    {
        for ( var i = 0; i < nuSubbands; ++i )
        {
            ( ( SubbandLayer2 )subbands[ i ] ).ReadScaleFactorSelection( stream, crc );
        }
    }
}
