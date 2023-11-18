// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// Implements decoding of MPEG Audio Layer I frames.
/// </summary>
[PublicAPI]
public class LayerIDecoder : IFrameDecoder
{
    protected Bitstream?       stream;
    protected Header?          header;
    protected SynthesisFilter? filter1;
    protected SynthesisFilter? filter2;
    protected OutputBuffer?    buffer;
    protected int              whichChannels;
    protected int              mode;

    protected int        numSubbands;
    protected Subband[]? subbands;
    protected Crc16?     crc = new(); // = new Crc16[1] to enable CRC checking.

    public LayerIDecoder()
    {
    }

    /// <inheritdoc/>
    public void DecodeFrame()
    {
        GdxRuntimeException.ThrowIfNull( header );

        numSubbands = header.HNumberOfSubbands;
        subbands    = new Subband[ 32 ];
        mode        = header.HMode;

        CreateSubbands();

        ReadAllocation();
        ReadScaleFactorSelection();

        if ( ( crc != null ) || header.ChecksumOk() )
        {
            ReadScaleFactors();
            ReadSampleData();
        }
    }

    public virtual void Create( Bitstream stream0,
                                Header header0,
                                SynthesisFilter? filterA,
                                SynthesisFilter? filterB,
                                OutputBuffer? output,
                                int channel )
    {
        this.stream        = stream0;
        this.header        = header0;
        this.filter1       = filterA;
        this.filter2       = filterB;
        this.buffer        = output;
        this.whichChannels = channel;
    }

    protected virtual void CreateSubbands()
    {
        int i;

        if ( mode == Header.SINGLE_CHANNEL )
        {
            for ( i = 0; i < numSubbands; ++i )
            {
                subbands![ i ] = new SubbandLayer1( i );
            }
        }
        else if ( mode == Header.JOINT_STEREO )
        {
            for ( i = 0; i < header?.HIntensityStereoBound; ++i )
            {
                subbands![ i ] = new SubbandLayer1Stereo( i );
            }

            for ( ; i < numSubbands; ++i )
            {
                subbands![ i ] = new SubbandLayer1IntensityStereo( i );
            }
        }
        else
        {
            for ( i = 0; i < numSubbands; ++i )
            {
                subbands![ i ] = new SubbandLayer1Stereo( i );
            }
        }
    }

    protected virtual void ReadAllocation()
    {
        for ( var i = 0; i < numSubbands; ++i )
        {
            subbands![ i ].ReadAllocation( stream, header, crc );
        }
    }

    protected virtual void ReadScaleFactorSelection()
    {
        // scale factor selection not present for layer I.
    }

    protected virtual void ReadScaleFactors()
    {
        for ( var i = 0; i < numSubbands; ++i )
        {
            subbands?[ i ].ReadScaleFactor( stream, header );
        }
    }

    protected virtual void ReadSampleData()
    {
        var readReady  = false;
        var writeReady = false;
        var hMode      = header?.HMode;

        do
        {
            for ( var i = 0; i < numSubbands; ++i )
            {
                readReady = subbands![ i ].ReadSampleData( stream );
            }

            do
            {
                for ( var i = 0; i < numSubbands; ++i )
                {
                    writeReady = subbands![ i ].PutNextSample( whichChannels, filter1, filter2 );
                }

                filter1?.CalculatePcmSamples( out buffer );

                if ( ( whichChannels == OutputChannels.BOTH_CHANNELS ) && ( hMode != Header.SINGLE_CHANNEL ) )
                {
                    filter2?.CalculatePcmSamples( out buffer );
                }
            }
            while ( !writeReady );
        }
        while ( !readReady );
    }

    // Scalefactors for layer I and II, Annex 3-B.1 in ISO/IEC DIS 11172:
    protected readonly static float[] ScaleFactors =
    {
        2.00000000000000f, 1.58740105196820f, 1.25992104989487f, 1.00000000000000f,
        0.79370052598410f, 0.62996052494744f, 0.50000000000000f, 0.39685026299205f,
        0.31498026247372f, 0.25000000000000f, 0.19842513149602f, 0.15749013123686f,
        0.12500000000000f, 0.09921256574801f, 0.07874506561843f, 0.06250000000000f,
        0.04960628287401f, 0.03937253280921f, 0.03125000000000f, 0.02480314143700f,
        0.01968626640461f, 0.01562500000000f, 0.01240157071850f, 0.00984313320230f,
        0.00781250000000f, 0.00620078535925f, 0.00492156660115f, 0.00390625000000f,
        0.00310039267963f, 0.00246078330058f, 0.00195312500000f, 0.00155019633981f,
        0.00123039165029f, 0.00097656250000f, 0.00077509816991f, 0.00061519582514f,
        0.00048828125000f, 0.00038754908495f, 0.00030759791257f, 0.00024414062500f,
        0.00019377454248f, 0.00015379895629f, 0.00012207031250f, 0.00009688727124f,
        0.00007689947814f, 0.00006103515625f, 0.00004844363562f, 0.00003844973907f,
        0.00003051757813f, 0.00002422181781f, 0.00001922486954f, 0.00001525878906f,
        0.00001211090890f, 0.00000961243477f, 0.00000762939453f, 0.00000605545445f,
        0.00000480621738f, 0.00000381469727f, 0.00000302772723f, 0.00000240310869f,
        0.00000190734863f, 0.00000151386361f, 0.00000120155435f, 0.00000000000000f
        /*
         * illegal scalefactor
         */
    };

    /// <summary>
    /// Abstract base class for subband classes of layer I and II.
    /// </summary>
    [PublicAPI]
    public abstract class Subband
    {
        public abstract void ReadAllocation( Bitstream? stream, Header? header, Crc16? crc );

        public abstract void ReadScaleFactor( Bitstream? stream, Header? header );

        public abstract bool ReadSampleData( Bitstream? stream );

        public abstract bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 );
    }

    /// <summary>
    /// Class for layer I subbands in single channel mode. Used for single
    /// channel mode and in derived class for intensity stereo mode
    /// </summary>
    [PublicAPI]
    public class SubbandLayer1 : Subband
    {
        // Factors and offsets for sample requantization
        public readonly static float[] TableFactor =
        {
            0.0f,
            ( ( 1.0f / 2.0f ) * 4.0f ) / 3.0f, ( ( 1.0f / 4.0f ) * 8.0f ) / 7.0f,
            ( ( 1.0f / 8.0f ) * 16.0f ) / 15.0f, ( ( 1.0f / 16.0f ) * 32.0f ) / 31.0f,
            ( ( 1.0f / 32.0f ) * 64.0f ) / 63.0f, ( ( 1.0f / 64.0f ) * 128.0f ) / 127.0f,
            ( ( 1.0f / 128.0f ) * 256.0f ) / 255.0f, ( ( 1.0f / 256.0f ) * 512.0f ) / 511.0f,
            ( ( 1.0f / 512.0f ) * 1024.0f ) / 1023.0f, ( ( 1.0f / 1024.0f ) * 2048.0f ) / 2047.0f,
            ( ( 1.0f / 2048.0f ) * 4096.0f ) / 4095.0f, ( ( 1.0f / 4096.0f ) * 8192.0f ) / 8191.0f,
            ( ( 1.0f / 8192.0f ) * 16384.0f ) / 16383.0f, ( ( 1.0f / 16384.0f ) * 32768.0f ) / 32767.0f
        };

        public readonly static float[] TableOffset =
        {
            0.0f,
            ( ( ( 1.0f / 2.0f ) - 1.0f ) * 4.0f ) / 3.0f, ( ( ( 1.0f / 4.0f ) - 1.0f ) * 8.0f ) / 7.0f,
            ( ( ( 1.0f / 8.0f ) - 1.0f ) * 16.0f ) / 15.0f, ( ( ( 1.0f / 16.0f ) - 1.0f ) * 32.0f ) / 31.0f,
            ( ( ( 1.0f / 32.0f ) - 1.0f ) * 64.0f ) / 63.0f, ( ( ( 1.0f / 64.0f ) - 1.0f ) * 128.0f ) / 127.0f,
            ( ( ( 1.0f / 128.0f ) - 1.0f ) * 256.0f ) / 255.0f, ( ( ( 1.0f / 256.0f ) - 1.0f ) * 512.0f ) / 511.0f,
            ( ( ( 1.0f / 512.0f ) - 1.0f ) * 1024.0f ) / 1023.0f, ( ( ( 1.0f / 1024.0f ) - 1.0f ) * 2048.0f ) / 2047.0f,
            ( ( ( 1.0f / 2048.0f ) - 1.0f ) * 4096.0f ) / 4095.0f, ( ( ( 1.0f / 4096.0f ) - 1.0f ) * 8192.0f ) / 8191.0f,
            ( ( ( 1.0f / 8192.0f ) - 1.0f ) * 16384.0f ) / 16383.0f, ( ( ( 1.0f / 16384.0f ) - 1.0f ) * 32768.0f ) / 32767.0f
        };

        public int   subbandNumber;
        public int   samplenumber;
        public int   allocation;
        public float scalefactor;
        public int   samplelength;
        public float sample;
        public float factor;
        public float offset;

        public SubbandLayer1( int subbandNumber )
        {
            this.subbandNumber = subbandNumber;
            samplenumber       = 0;
        }

        public override void ReadAllocation( Bitstream? stream, Header? header, Crc16? crc )
        {
            ArgumentNullException.ThrowIfNull( stream );
            
            if ( ( allocation = stream.GetBits( 4 ) ) == 15 )
            {
                throw new DecoderException( Mp3Decoder.ILLEGAL_SUBBAND_ALLOCATION );
            }

            if ( crc != null )
            {
                crc.AddBits( allocation, 4 );
            }

            if ( allocation != 0 )
            {
                samplelength = allocation + 1;
                factor       = TableFactor[ allocation ];
                offset       = TableOffset[ allocation ];
            }
        }

        public override void ReadScaleFactor( Bitstream? stream, Header? header )
        {
            if ( allocation != 0 )
            {
                Debug.Assert( stream != null );

                scalefactor = ScaleFactors[ stream.GetBits( 6 ) ];
            }
        }

        public override bool ReadSampleData( Bitstream? stream )
        {
            ArgumentNullException.ThrowIfNull( stream );

            if ( allocation != 0 )
            {
                sample = stream.GetBits( samplelength );
            }

            if ( ++samplenumber == 12 )
            {
                samplenumber = 0;

                return true;
            }

            return false;
        }

        public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
        {
            if ( ( allocation != 0 ) && ( channels != OutputChannels.RIGHT_CHANNEL ) )
            {
                var scaledSample = ( ( sample * factor ) + offset ) * scalefactor;

                filter1?.InputSample( scaledSample, subbandNumber );
            }

            return true;
        }
    };

    /// <summary>
    /// Class for layer I subbands in joint stereo mode.
    /// </summary>
    [PublicAPI]
    public class SubbandLayer1IntensityStereo : SubbandLayer1
    {
        protected float channel2ScaleFactor;

        public SubbandLayer1IntensityStereo( int subbandNumber )
            : base( subbandNumber )
        {
        }

        public override void ReadScaleFactor( Bitstream? stream, Header? header )
        {
            if ( allocation != 0 )
            {
                Debug.Assert( stream != null );

                scalefactor         = ScaleFactors[ stream.GetBits( 6 ) ];
                channel2ScaleFactor = ScaleFactors[ stream.GetBits( 6 ) ];
            }
        }

        public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
        {
            if ( allocation != 0 )
            {
                sample = ( sample * factor ) + offset; // requantization

                if ( channels == OutputChannels.BOTH_CHANNELS )
                {
                    float sample1 = sample * scalefactor, sample2 = sample * channel2ScaleFactor;
                    filter1?.InputSample( sample1, subbandNumber );
                    filter2?.InputSample( sample2, subbandNumber );
                }
                else if ( channels == OutputChannels.LEFT_CHANNEL )
                {
                    var sample1 = sample * scalefactor;
                    filter1?.InputSample( sample1, subbandNumber );
                }
                else
                {
                    var sample2 = sample * channel2ScaleFactor;
                    filter1?.InputSample( sample2, subbandNumber );
                }
            }

            return true;
        }
    };

    /// <summary>
    /// Class for layer I subbands in stereo mode.
    /// </summary>
    [PublicAPI]
    public class SubbandLayer1Stereo : SubbandLayer1
    {
        protected int   channel2Allocation;
        protected float channel2ScaleFactor;
        protected int   channel2SampleLength;
        protected float channel2Sample;
        protected float channel2Factor;
        protected float channel2Offset;

        public SubbandLayer1Stereo( int subbandnumber )
            : base( subbandnumber )
        {
        }

        public override void ReadAllocation( Bitstream? stream, Header? header, Crc16? crc )
        {
            ArgumentNullException.ThrowIfNull( stream );
            
            allocation         = stream.GetBits( 4 );
            channel2Allocation = stream.GetBits( 4 );

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
                channel2SampleLength = channel2Allocation + 1;
                channel2Factor       = TableFactor[ channel2Allocation ];
                channel2Offset       = TableOffset[ channel2Allocation ];
            }
        }

        public override void ReadScaleFactor( Bitstream? stream, Header? header )
        {
            Debug.Assert( stream != null );

            if ( allocation != 0 )
            {
                scalefactor = ScaleFactors[ stream.GetBits( 6 ) ];
            }

            if ( channel2Allocation != 0 )
            {
                channel2ScaleFactor = ScaleFactors[ stream.GetBits( 6 ) ];
            }
        }

        public override bool ReadSampleData( Bitstream? stream )
        {
            ArgumentNullException.ThrowIfNull( stream );

            var returnvalue = base.ReadSampleData( stream );

            if ( channel2Allocation != 0 )
            {
                channel2Sample = stream.GetBits( channel2SampleLength );
            }

            return returnvalue;
        }

        public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
        {
            base.PutNextSample( channels, filter1, filter2 );

            if ( ( channel2Allocation != 0 ) && ( channels != OutputChannels.LEFT_CHANNEL ) )
            {
                var sample2 = ( ( channel2Sample * channel2Factor ) + channel2Offset ) * channel2ScaleFactor;

                if ( channels == OutputChannels.BOTH_CHANNELS )
                {
                    filter2?.InputSample( sample2, subbandNumber );
                }
                else
                {
                    filter1?.InputSample( sample2, subbandNumber );
                }
            }

            return true;
        }
    };
}
