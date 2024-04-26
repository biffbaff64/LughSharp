// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Audio.MP3Sharp.Decoding.Decoders.LayerI;

namespace LughSharp.LibCore.Audio.MP3Sharp.Decoding.Decoders;

/// <summary>
///     Implements decoding of MPEG Audio Layer I frames.
/// </summary>
[PublicAPI]
public class LayerIDecoder : IFrameDecoder
{
    protected readonly Crc16? crc = new();

    protected AudioBase?       buffer  = null!;
    protected SynthesisFilter? filter1 = null!;
    protected SynthesisFilter? filter2 = null!;
    protected Header?          header  = null!;
    protected int              mode;
    protected int              nuSubbands;
    protected Bitstream        stream   = null!;
    protected ASubband[]       subbands = null!;

    protected int whichChannels;

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

    // new Crc16[1] to enable CRC checking.

    public virtual void Create( Bitstream stream0,
                                Header header0,
                                SynthesisFilter? filtera,
                                SynthesisFilter? filterb,
                                AudioBase? buffer0,
                                int whichCh0 )
    {
        stream        = stream0;
        header        = header0;
        filter1       = filtera;
        filter2       = filterb;
        buffer        = buffer0;
        whichChannels = whichCh0;
    }

    protected virtual void CreateSubbands()
    {
        int i;

        switch ( mode )
        {
            case Header.SINGLE_CHANNEL:
            {
                for ( i = 0; i < nuSubbands; ++i )
                {
                    subbands[ i ] = new SubbandLayer1( i );
                }

                break;
            }

            case Header.JOINT_STEREO:
            {
                for ( i = 0; i < header?.IntensityStereoBound(); ++i )
                {
                    subbands[ i ] = new SubbandLayer1Stereo( i );
                }

                for ( ; i < nuSubbands; ++i )
                {
                    subbands[ i ] = new SubbandLayer1IntensityStereo( i );
                }

                break;
            }

            default:
            {
                for ( i = 0; i < nuSubbands; ++i )
                {
                    subbands[ i ] = new SubbandLayer1Stereo( i );
                }

                break;
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

                filter1?.CalculatePcSamples( buffer );

                if ( ( whichChannels == OutputChannels.BOTH_CHANNELS ) && ( hdrMode != Header.SINGLE_CHANNEL ) )
                {
                    filter2?.CalculatePcSamples( buffer );
                }
            }
            while ( !writeReady );
        }
        while ( !readReady );
    }
}