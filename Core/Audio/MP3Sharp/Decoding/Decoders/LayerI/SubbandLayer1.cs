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

namespace LibGDXSharp.Audio.MP3Sharp;

/// <summary>
///     public class for layer I subbands in single channel mode. Used for single channel mode
///     and in derived class for intensity stereo mode
/// </summary>
public class SubbandLayer1 : ASubband
{
    // Factors and offsets for sample requantization
    public readonly static float[] TableFactor =
    {
        0.0f, ( 1.0f / 2.0f ) * ( 4.0f / 3.0f ), ( 1.0f / 4.0f ) * ( 8.0f / 7.0f ), ( 1.0f / 8.0f ) * ( 16.0f / 15.0f ),
        ( 1.0f / 16.0f ) * ( 32.0f / 31.0f ), ( 1.0f / 32.0f ) * ( 64.0f / 63.0f ), ( 1.0f / 64.0f ) * ( 128.0f / 127.0f ),
        ( 1.0f / 128.0f ) * ( 256.0f / 255.0f ), ( 1.0f / 256.0f ) * ( 512.0f / 511.0f ), ( 1.0f / 512.0f ) * ( 1024.0f / 1023.0f ),
        ( 1.0f / 1024.0f ) * ( 2048.0f / 2047.0f ), ( 1.0f / 2048.0f ) * ( 4096.0f / 4095.0f ), ( 1.0f / 4096.0f ) * ( 8192.0f / 8191.0f ),
        ( 1.0f / 8192.0f ) * ( 16384.0f / 16383.0f ), ( 1.0f / 16384.0f ) * ( 32768.0f / 32767.0f )
    };

    public readonly static float[] TableOffset =
    {
        0.0f, ( ( 1.0f / 2.0f ) - 1.0f ) * ( 4.0f / 3.0f ), ( ( 1.0f / 4.0f ) - 1.0f ) * ( 8.0f / 7.0f ),
        ( ( 1.0f / 8.0f ) - 1.0f ) * ( 16.0f / 15.0f ), ( ( 1.0f / 16.0f ) - 1.0f ) * ( 32.0f / 31.0f ),
        ( ( 1.0f / 32.0f ) - 1.0f ) * ( 64.0f / 63.0f ), ( ( 1.0f / 64.0f ) - 1.0f ) * ( 128.0f / 127.0f ),
        ( ( 1.0f / 128.0f ) - 1.0f ) * ( 256.0f / 255.0f ), ( ( 1.0f / 256.0f ) - 1.0f ) * ( 512.0f / 511.0f ),
        ( ( 1.0f / 512.0f ) - 1.0f ) * ( 1024.0f / 1023.0f ), ( ( 1.0f / 1024.0f ) - 1.0f ) * ( 2048.0f / 2047.0f ),
        ( ( 1.0f / 2048.0f ) - 1.0f ) * ( 4096.0f / 4095.0f ), ( ( 1.0f / 4096.0f ) - 1.0f ) * ( 8192.0f / 8191.0f ),
        ( ( 1.0f / 8192.0f ) - 1.0f ) * ( 16384.0f / 16383.0f ), ( ( 1.0f / 16384.0f ) - 1.0f ) * ( 32768.0f / 32767.0f )
    };

    protected readonly int subbandnumber;

    protected int   allocation;
    protected float factor;
    protected float offset;
    protected float sample;
    protected int   samplelength;
    protected int   samplenumber;
    protected float scalefactor;

    /// <summary>
    ///     Construtor.
    /// </summary>
    public SubbandLayer1( int subbandnumber )
    {
        this.subbandnumber = subbandnumber;
        samplenumber       = 0;
    }

    /// <summary>
    /// </summary>
    public override void ReadAllocation( Bitstream stream, Header? header, Crc16 crc )
    {
        if ( ( allocation = stream.GetBitsFromBuffer( 4 ) ) == 15 )
        {
        }

        // cerr << "WARNING: stream contains an illegal allocation!\n";
        // MPEG-stream is corrupted!
        crc.AddBits( allocation, 4 );

        if ( allocation != 0 )
        {
            samplelength = allocation + 1;
            factor       = TableFactor[ allocation ];
            offset       = TableOffset[ allocation ];
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
    }

    /// <summary>
    /// </summary>
    public override bool ReadSampleData( Bitstream stream )
    {
        if ( allocation != 0 )
        {
            sample = stream.GetBitsFromBuffer( samplelength );
        }

        if ( ++samplenumber == 12 )
        {
            samplenumber = 0;

            return true;
        }

        return false;
    }

    /// <summary>
    /// </summary>
    public override bool PutNextSample( int channels, SynthesisFilter? filter1, SynthesisFilter? filter2 )
    {
        if ( ( allocation != 0 ) && ( channels != OutputChannels.RIGHT_CHANNEL ) )
        {
            var scaledSample = ( ( sample * factor ) + offset ) * scalefactor;
            filter1?.AddSample( scaledSample, subbandnumber );
        }

        return true;
    }
}
