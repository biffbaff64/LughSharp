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
/// Implements decoding of MPEG Audio Layer I frames.
/// </summary>
[PublicAPI]
public class LayerIDecoder : IFrameDecoder
{
    protected readonly Crc16? CRC = new();

    protected AudioBase?       Buffer  = null!;
    protected SynthesisFilter? Filter1 = null!;
    protected SynthesisFilter? Filter2 = null!;
    protected Header?          Header  = null!;
    protected int              Mode;
    protected int              NuSubbands;
    protected Bitstream        Stream   = null!;
    protected ASubband[]       Subbands = null!;
    protected int              WhichChannels;

    public virtual void DecodeFrame()
    {
        NuSubbands = Header!.NumberSubbands();
        Subbands   = new ASubband[ 32 ];
        Mode       = Header.Mode();

        CreateSubbands();

        ReadAllocation();
        ReadScaleFactorSelection();

        if ( ( CRC != null ) || Header.IsChecksumOk() )
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
        Stream        = stream0;
        Header        = header0;
        Filter1       = filtera;
        Filter2       = filterb;
        Buffer        = buffer0;
        WhichChannels = whichCh0;
    }

    protected virtual void CreateSubbands()
    {
        int i;

        switch ( Mode )
        {
            case Header.SINGLE_CHANNEL:
            {
                for ( i = 0; i < NuSubbands; ++i )
                {
                    Subbands[ i ] = new SubbandLayer1( i );
                }

                break;
            }

            case Header.JOINT_STEREO:
            {
                for ( i = 0; i < Header?.IntensityStereoBound(); ++i )
                {
                    Subbands[ i ] = new SubbandLayer1Stereo( i );
                }

                for ( ; i < NuSubbands; ++i )
                {
                    Subbands[ i ] = new SubbandLayer1IntensityStereo( i );
                }

                break;
            }

            default:
            {
                for ( i = 0; i < NuSubbands; ++i )
                {
                    Subbands[ i ] = new SubbandLayer1Stereo( i );
                }

                break;
            }
        }
    }

    protected virtual void ReadAllocation()
    {
        // start to read audio data:
        for ( var i = 0; i < NuSubbands; ++i )
        {
            Subbands[ i ].ReadAllocation( Stream, Header, CRC! );
        }
    }

    protected virtual void ReadScaleFactorSelection()
    {
        // scale factor selection not present for layer I. 
    }

    protected virtual void ReadScaleFactors()
    {
        for ( var i = 0; i < NuSubbands; ++i )
        {
            Subbands[ i ].ReadScaleFactor( Stream, Header );
        }
    }

    protected virtual void ReadSampleData()
    {
        var readReady  = false;
        var writeReady = false;
        var hdrMode    = Header?.Mode();

        do
        {
            int i;

            for ( i = 0; i < NuSubbands; ++i )
            {
                readReady = Subbands[ i ].ReadSampleData( Stream );
            }

            do
            {
                for ( i = 0; i < NuSubbands; ++i )
                {
                    writeReady = Subbands[ i ].PutNextSample( WhichChannels, Filter1, Filter2 );
                }

                Filter1?.CalculatePcSamples( Buffer );

                if ( ( WhichChannels == OutputChannels.BOTH_CHANNELS ) && ( hdrMode != Header.SINGLE_CHANNEL ) )
                {
                    Filter2?.CalculatePcSamples( Buffer );
                }
            }
            while ( !writeReady );
        }
        while ( !readReady );
    }
}
