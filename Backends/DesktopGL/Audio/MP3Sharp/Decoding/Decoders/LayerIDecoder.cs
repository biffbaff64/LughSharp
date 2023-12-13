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
/// Implements decoding of MPEG Audio Layer I frames.
/// </summary>
[PublicAPI]
public class LayerIDecoder : IFrameDecoder
{
    protected readonly Crc16? crc = new();

    protected ABuffer?         buffer   = null!;
    protected SynthesisFilter? filter1  = null!;
    protected SynthesisFilter? filter2  = null!;
    protected Header?          header   = null!;
    protected Bitstream        stream   = null!;
    protected ASubband[]       subbands = null!;
    protected int              mode;
    protected int              nuSubbands;

    protected int whichChannels;

    // new Crc16[1] to enable CRC checking.

    public virtual void Create( Bitstream stream0,
                                Header header0,
                                SynthesisFilter? filtera,
                                SynthesisFilter? filterb,
                                ABuffer? buffer0,
                                int whichCh0 )
    {
        stream        = stream0;
        header        = header0;
        filter1       = filtera;
        filter2       = filterb;
        buffer        = buffer0;
        whichChannels = whichCh0;
    }

    public virtual void DecodeFrame()
    {
        nuSubbands = header!.NumberSubbands();
        subbands   = new ASubband[ 32 ];
        mode       = header.Mode();

        CreateSubbands();

        ReadAllocation();
        ReadScaleFactorSelection();

        if ( ( crc != null ) || header.IsChecksumOk() )
        {
            ReadScaleFactors();

            ReadSampleData();
        }
    }

    protected virtual void CreateSubbands()
    {
        int i;

        if ( mode == Header.SINGLE_CHANNEL )
        {
            for ( i = 0; i < nuSubbands; ++i )
            {
                subbands[ i ] = new SubbandLayer1( i );
            }
        }
        else if ( mode == Header.JOINT_STEREO )
        {
            for ( i = 0; i < header?.IntensityStereoBound(); ++i )
            {
                subbands[ i ] = new SubbandLayer1Stereo( i );
            }

            for ( ; i < nuSubbands; ++i )
            {
                subbands[ i ] = new SubbandLayer1IntensityStereo( i );
            }
        }
        else
        {
            for ( i = 0; i < nuSubbands; ++i )
            {
                subbands[ i ] = new SubbandLayer1Stereo( i );
            }
        }
    }

    protected virtual void ReadAllocation()
    {
        // start to read audio data:
        for ( var i = 0; i < nuSubbands; ++i )
        {
            subbands[ i ].ReadAllocation( stream, header, crc! );
        }
    }

    protected virtual void ReadScaleFactorSelection()
    {
        // scale factor selection not present for layer I. 
    }

    protected virtual void ReadScaleFactors()
    {
        for ( var i = 0; i < nuSubbands; ++i )
        {
            subbands[ i ].ReadScaleFactor( stream, header );
        }
    }

    protected virtual void ReadSampleData()
    {
        var readReady  = false;
        var writeReady = false;
        var hdrMode    = header?.Mode();

        do
        {
            int i;

            for ( i = 0; i < nuSubbands; ++i )
            {
                readReady = subbands[ i ].ReadSampleData( stream );
            }

            do
            {
                for ( i = 0; i < nuSubbands; ++i )
                {
                    writeReady = subbands[ i ].PutNextSample( whichChannels, filter1, filter2 );
                }

                filter1?.calculate_pc_samples( buffer );

                if ( ( whichChannels == OutputChannels.BOTH_CHANNELS ) && ( hdrMode != Header.SINGLE_CHANNEL ) )
                {
                    filter2?.calculate_pc_samples( buffer );
                }
            }
            while ( !writeReady );
        }
        while ( !readReady );
    }
}
