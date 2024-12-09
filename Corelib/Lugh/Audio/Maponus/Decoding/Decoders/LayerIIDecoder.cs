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

using Corelib.Lugh.Audio.Maponus.Decoding.Decoders.LayerII;

namespace Corelib.Lugh.Audio.Maponus.Decoding.Decoders;

/// <summary>
/// Implements decoding of MPEG Audio Layer II frames.
/// </summary>
[PublicAPI]
public class LayerIIDecoder : LayerIDecoder
{
    /// <summary>
    /// Creates subband instances based on the header mode for Layer II.
    /// </summary>
    protected override void CreateSubbands()
    {
        int i;
        
        switch ( Mode )
        {
            case Header.SINGLE_CHANNEL:
                for ( i = 0; i < NuSubbands; ++i )
                {
                    Subbands[ i ] = new SubbandLayer2( i );
                }

                break;

            case Header.JOINT_STEREO:
                for ( i = 0; i < Header?.IntensityStereoBound(); ++i )
                {
                    Subbands[ i ] = new SubbandLayer2Stereo( i );
                }

                for ( ; i < NuSubbands; ++i )
                {
                    Subbands[ i ] = new SubbandLayer2IntensityStereo( i );
                }

                break;

            default:
                for ( i = 0; i < NuSubbands; ++i )
                {
                    Subbands[ i ] = new SubbandLayer2Stereo( i );
                }

                break;
        }
    }

    /// <summary>
    /// Reads the scale factor selection for each subband.
    /// </summary>
    protected override void ReadScaleFactorSelection()
    {
        for ( var i = 0; i < NuSubbands; ++i )
        {
            ( ( SubbandLayer2 ) Subbands[ i ] ).ReadScaleFactorSelection( Stream, CRC );
        }
    }
}
