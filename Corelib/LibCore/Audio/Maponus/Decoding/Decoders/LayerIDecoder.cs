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

using Corelib.LibCore.Audio.Maponus.Decoding.Decoders.LayerI;

namespace Corelib.LibCore.Audio.Maponus.Decoding.Decoders;

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

    // ========================================================================

    /// <summary>
    /// Decodes the current audio frame.
    /// </summary>
    public virtual void DecodeFrame()
    {
        if ( Header == null )
        {
            throw new InvalidOperationException( "Header is not initialized." );
        }

        NuSubbands = Header.NumberSubbands();
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

    /// <summary>
    /// Initializes the decoder with the required parameters.
    /// </summary>
    /// <param name="stream0"> Bitstream to decode. </param>
    /// <param name="header0"> Header of the frame. </param>
    /// <param name="filtera"> First synthesis filter. </param>
    /// <param name="filterb"> Second synthesis filter. </param>
    /// <param name="buffer0"> Audio buffer. </param>
    /// <param name="whichCh0"> Channel configuration. </param>
    public virtual void Create( Bitstream stream0,
                                Header header0,
                                SynthesisFilter? filtera,
                                SynthesisFilter? filterb,
                                AudioBase? buffer0,
                                int whichCh0 )
    {
        Stream        = stream0 ?? throw new ArgumentNullException( nameof( stream0 ) );
        Header        = header0 ?? throw new ArgumentNullException( nameof( header0 ) );
        Filter1       = filtera;
        Filter2       = filterb;
        Buffer        = buffer0;
        WhichChannels = whichCh0;
    }

    /// <summary>
    /// Creates subband instances based on the header mode.
    /// </summary>
    protected virtual void CreateSubbands()
    {
        var intensityStereoBound = Header?.IntensityStereoBound() ?? 0;

        for ( var i = 0; i < NuSubbands; ++i )
        {
            if ( ( Mode == Header.JOINT_STEREO ) && ( i >= intensityStereoBound ) )
            {
                Subbands[ i ] = new SubbandLayer1IntensityStereo( i );
            }
            else if ( ( Mode == Header.SINGLE_CHANNEL ) || ( Mode == Header.JOINT_STEREO ) )
            {
                Subbands[ i ] = new SubbandLayer1Stereo( i );
            }
            else
            {
                Subbands[ i ] = new SubbandLayer1( i );
            }
        }
    }

    /// <summary>
    /// Reads allocation information for each subband.
    /// </summary>
    protected virtual void ReadAllocation()
    {
        for ( var i = 0; i < NuSubbands; ++i )
        {
            Subbands[ i ].ReadAllocation( Stream, Header, CRC! );
        }
    }

    /// <summary>
    /// Placeholder for Layer I; scale factor selection not present.
    /// </summary>
    protected virtual void ReadScaleFactorSelection()
    {
        // Scale factor selection not present for Layer I.
    }

    /// <summary>
    /// Reads scale factors for each subband.
    /// </summary>
    protected virtual void ReadScaleFactors()
    {
        for ( var i = 0; i < NuSubbands; ++i )
        {
            Subbands[ i ].ReadScaleFactor( Stream, Header );
        }
    }

    /// <summary>
    /// Reads and processes sample data for each subband.
    /// </summary>
    protected virtual void ReadSampleData()
    {
        bool readReady;
        var  hdrMode = Header?.Mode() ?? 0;

        do
        {
            readReady = ReadSubbandSampleData();

            bool writeReady;

            do
            {
                writeReady = WriteSubbandSamples();
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

    /// <summary>
    /// Reads sample data for each subband from the bitstream.
    /// </summary>
    /// <returns>True if all subbands have read their sample data, false otherwise.</returns>
    private bool ReadSubbandSampleData()
    {
        var readReady = false;

        for ( var i = 0; i < NuSubbands; ++i )
        {
            readReady = Subbands[ i ].ReadSampleData( Stream );
        }

        return readReady;
    }

    /// <summary>
    /// Writes the next sample for each subband to the synthesis filters.
    /// </summary>
    /// <returns>True if all subbands have written their samples, false otherwise.</returns>
    private bool WriteSubbandSamples()
    {
        var writeReady = false;

        for ( var i = 0; i < NuSubbands; ++i )
        {
            writeReady = Subbands[ i ].PutNextSample( WhichChannels, Filter1, Filter2 );
        }

        return writeReady;
    }
}
