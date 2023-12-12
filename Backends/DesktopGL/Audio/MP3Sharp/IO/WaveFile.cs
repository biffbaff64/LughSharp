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

using MP3Sharp.Support;

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;

/// <summary>
/// public class allowing WaveFormat Access
/// </summary>
public class WaveFile : RiffFile
{
    public const     int             MAX_WAVE_CHANNELS = 2;
    private readonly int             _NumSamples;
    private readonly RiffChunkHeader _PcmData;
    private readonly WaveFormatChunk _WaveFormat;
    private          bool            _JustWriteLengthBytes;
    private          long            _PcmDataOffset; // offset of 'pc_data' in output file

    /// <summary>
    /// Creates a new WaveFile instance.
    /// </summary>
    public WaveFile()
    {
        _PcmData        = new RiffChunkHeader( this );
        _WaveFormat     = new WaveFormatChunk( this );
        _PcmData.CkId   = FourCC( "data" );
        _PcmData.CkSize = 0;
        _NumSamples     = 0;
    }

    /// <summary>
    /// Pass in either a FileName or a Stream.
    /// </summary>
    public virtual int OpenForWrite( string filename,
                                     Stream stream,
                                     int samplingRate,
                                     short bitsPerSample,
                                     short numChannels )
    {
        // Verify parameters...
        if ( ( bitsPerSample != 8 && bitsPerSample != 16 ) || numChannels < 1 || numChannels > 2 )
        {
            return DDC_INVALID_CALL;
        }

        _WaveFormat.Data.Config( samplingRate, bitsPerSample, numChannels );

        int retcode = DDC_SUCCESS;

        if ( stream != null )
            Open( stream, RF_WRITE );
        else
            Open( filename, RF_WRITE );

        if ( retcode == DDC_SUCCESS )
        {
            sbyte[] theWave =
            {
                ( sbyte )SupportClass.Identity( 'W' ), ( sbyte )SupportClass.Identity( 'A' ),
                ( sbyte )SupportClass.Identity( 'V' ), ( sbyte )SupportClass.Identity( 'E' )
            };

            retcode = Write( theWave, 4 );

            if ( retcode == DDC_SUCCESS )
            {
                // Ecriture de wave_format
                retcode = Write( _WaveFormat.Header, 8 );
                retcode = Write( _WaveFormat.Data.FormatTag, 2 );
                retcode = Write( _WaveFormat.Data.NumChannels, 2 );
                retcode = Write( _WaveFormat.Data.NumSamplesPerSec, 4 );
                retcode = Write( _WaveFormat.Data.NumAvgBytesPerSec, 4 );
                retcode = Write( _WaveFormat.Data.NumBlockAlign, 2 );
                retcode = Write( _WaveFormat.Data.NumBitsPerSample, 2 );

                if ( retcode == DDC_SUCCESS )
                {
                    _PcmDataOffset = CurrentFilePosition();
                    retcode        = Write( _PcmData, 8 );
                }
            }
        }

        return retcode;
    }

    /// <summary>
    /// Write 16-bit audio
    /// </summary>
    public virtual int WriteData( short[] data, int numData )
    {
        int extraBytes = numData * 2;
        _PcmData.CkSize += extraBytes;

        return Write( data, extraBytes );
    }

    public override int Close()
    {
        int rc = DDC_SUCCESS;

        if ( Fmode == RF_WRITE )
            rc = Backpatch( _PcmDataOffset, _PcmData, 8 );

        if ( !_JustWriteLengthBytes )
        {
            if ( rc == DDC_SUCCESS )
                rc = base.Close();
        }

        return rc;
    }

    public int Close( bool justWriteLengthBytes )
    {
        _JustWriteLengthBytes = justWriteLengthBytes;
        int ret = Close();
        _JustWriteLengthBytes = false;

        return ret;
    }

    // [Hz]
    public virtual int SamplingRate()
    {
        return _WaveFormat.Data.NumSamplesPerSec;
    }

    public virtual short BitsPerSample()
    {
        return _WaveFormat.Data.NumBitsPerSample;
    }

    public virtual short NumChannels()
    {
        return _WaveFormat.Data.NumChannels;
    }

    public virtual int NumSamples()
    {
        return _NumSamples;
    }

    /// <summary>
    /// Open for write using another wave file's parameters...
    /// </summary>
    public virtual int OpenForWrite( string filename, WaveFile otherWave )
    {
        return OpenForWrite( filename,
                             null,
                             otherWave.SamplingRate(),
                             otherWave.BitsPerSample(),
                             otherWave.NumChannels() );
    }

    public sealed class WaveFormatChunkData
    {
        private WaveFile _EnclosingInstance;
        public  int      NumAvgBytesPerSec;
        public  short    NumBitsPerSample;
        public  short    NumBlockAlign;
        public  short    NumChannels;      // Number of channels (mono=1, stereo=2)
        public  int      NumSamplesPerSec; // Sampling rate [Hz]
        public  short    FormatTag;        // Format category (PCM=1)

        public WaveFormatChunkData( WaveFile enclosingInstance )
        {
            InitBlock( enclosingInstance );
            FormatTag = 1; // PCM
            Config( 44100, 16, 1 );
        }

        public WaveFile EnclosingInstance => _EnclosingInstance;

        private void InitBlock( WaveFile enclosingInstance )
        {
            _EnclosingInstance = enclosingInstance;
        }

        public void Config( int newSamplingRate, short newBitsPerSample, short newNumChannels )
        {
            NumSamplesPerSec  = newSamplingRate;
            NumChannels       = newNumChannels;
            NumBitsPerSample  = newBitsPerSample;
            NumAvgBytesPerSec = ( NumChannels * NumSamplesPerSec * NumBitsPerSample ) / 8;
            NumBlockAlign     = ( short )( ( NumChannels * NumBitsPerSample ) / 8 );
        }
    }

    public class WaveFormatChunk
    {
        public  WaveFormatChunkData Data;
        private WaveFile            _EnclosingInstance;
        public  RiffChunkHeader     Header;

        public WaveFormatChunk( WaveFile enclosingInstance )
        {
            InitBlock( enclosingInstance );
            Header        = new RiffChunkHeader( enclosingInstance );
            Data          = new WaveFormatChunkData( enclosingInstance );
            Header.CkId   = FourCC( "fmt " );
            Header.CkSize = 16;
        }

        public WaveFile EnclosingInstance => _EnclosingInstance;

        private void InitBlock( WaveFile enclosingInstance )
        {
            _EnclosingInstance = enclosingInstance;
        }

        public virtual int VerifyValidity()
        {
            bool ret = Header.CkId == FourCC( "fmt " )
                    && ( Data.NumChannels == 1 || Data.NumChannels == 2 )
                    && Data.NumAvgBytesPerSec == ( Data.NumChannels * Data.NumSamplesPerSec * Data.NumBitsPerSample ) / 8
                    && Data.NumBlockAlign == ( Data.NumChannels * Data.NumBitsPerSample ) / 8;

            return ret ? 1 : 0;
        }
    }

    public class WaveFileSample
    {
        public  short[]  Chan;
        private WaveFile _EnclosingInstance;

        public WaveFileSample( WaveFile enclosingInstance )
        {
            InitBlock( enclosingInstance );
            Chan = new short[ MAX_WAVE_CHANNELS ];
        }

        public WaveFile EnclosingInstance => _EnclosingInstance;

        private void InitBlock( WaveFile enclosingInstance )
        {
            _EnclosingInstance = enclosingInstance;
        }
    }
}
