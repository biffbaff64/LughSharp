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

using Corelib.LibCore.Audio.Maponus.Support;

namespace Corelib.LibCore.Audio.Maponus.IO;

/// <summary>
/// public class allowing WaveFormat Access
/// </summary>
/// <remarks>
/// This class is marked as  to indicate to ReSharper that
/// methods and members etc may be called externally.
/// </remarks>
[PublicAPI]
public class WaveFile : RiffFile
{
    public const int MAX_WAVE_CHANNELS = 2;

    private readonly int             _numSamples;
    private readonly RiffChunkHeader _pcmData;
    private readonly WaveFormatChunk _waveFormat;

    private bool _justWriteLengthBytes;
    private long _pcmDataOffset; // offset of 'pc_data' in output file

    /// <summary>
    /// Creates a new WaveFile instance.
    /// </summary>
    public WaveFile()
    {
        _pcmData        = new RiffChunkHeader( this );
        _waveFormat     = new WaveFormatChunk( this );
        _pcmData.CkId   = FourCC( "data" );
        _pcmData.CkSize = 0;
        _numSamples     = 0;
    }

    /// <summary>
    /// Pass in either a FileName or a Stream.
    /// </summary>
    public virtual int OpenForWrite( string filename,
                                     Stream? stream,
                                     int samplingRate,
                                     short bitsPerSample,
                                     short numChannels )
    {
        // Verify parameters...
        if ( ( ( bitsPerSample != 8 ) && ( bitsPerSample != 16 ) ) || ( numChannels < 1 ) || ( numChannels > 2 ) )
        {
            return DDC_INVALID_CALL;
        }

        _waveFormat.Data.Config( samplingRate, bitsPerSample, numChannels );

        if ( stream != null )
        {
            Open( stream, RF_WRITE );
        }
        else
        {
            Open( filename, RF_WRITE );
        }

        sbyte[] theWave =
        [
            ( sbyte ) SupportClass.Identity( 'W' ),
            ( sbyte ) SupportClass.Identity( 'A' ),
            ( sbyte ) SupportClass.Identity( 'V' ),
            ( sbyte ) SupportClass.Identity( 'E' )
        ];

        var retcode = Write( theWave, 4 );

        if ( retcode == DDC_SUCCESS )
        {
            Write( _waveFormat.Header, 8 );
            Write( _waveFormat.Data.FormatTag, 2 );
            Write( _waveFormat.Data.NumChannels, 2 );
            Write( _waveFormat.Data.NumSamplesPerSec, 4 );
            Write( _waveFormat.Data.NumAvgBytesPerSec, 4 );
            Write( _waveFormat.Data.NumBlockAlign, 2 );

            retcode = Write( _waveFormat.Data.NumBitsPerSample, 2 );

            if ( retcode == DDC_SUCCESS )
            {
                _pcmDataOffset = CurrentFilePosition();
                retcode        = Write( _pcmData, 8 );
            }
        }

        return retcode;
    }

    /// <summary>
    /// Write 16-bit audio
    /// </summary>
    public virtual int WriteData( short[] data, int numData )
    {
        var extraBytes = numData * 2;
        _pcmData.CkSize += extraBytes;

        return Write( data, extraBytes );
    }

    public override int Close()
    {
        var rc = DDC_SUCCESS;

        if ( CurrentFileMode() == RF_WRITE )
        {
            rc = Backpatch( _pcmDataOffset, _pcmData, 8 );
        }

        if ( !_justWriteLengthBytes )
        {
            if ( rc == DDC_SUCCESS )
            {
                rc = base.Close();
            }
        }

        return rc;
    }

    public int Close( bool justWriteLengthBytes )
    {
        _justWriteLengthBytes = justWriteLengthBytes;

        var ret = Close();

        _justWriteLengthBytes = false;

        return ret;
    }

    // [Hz]
    public virtual int SamplingRate()
    {
        return _waveFormat.Data.NumSamplesPerSec;
    }

    public virtual short BitsPerSample()
    {
        return _waveFormat.Data.NumBitsPerSample;
    }

    public virtual short NumberOfChannels()
    {
        return _waveFormat.Data.NumChannels;
    }

    public virtual int NumSamples()
    {
        return _numSamples;
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
                             otherWave.NumberOfChannels() );
    }
}

[PublicAPI]
public class WaveFormatChunkData
{
    public WaveFormatChunkData( WaveFile enclosingInstance )
    {
        EnclosingInstance = enclosingInstance;

        FormatTag = 1; // PCM

        Config( 44100, 16, 1 );
    }

    public int      NumAvgBytesPerSec { get; set; }
    public short    NumBitsPerSample  { get; set; }
    public short    NumBlockAlign     { get; set; }
    public short    NumChannels       { get; set; } // Number of channels (mono=1, stereo=2)
    public int      NumSamplesPerSec  { get; set; } // Sampling rate [Hz]
    public short    FormatTag         { get; set; } // Format category (PCM=1)
    public WaveFile EnclosingInstance { get; set; }

    public void Config( int newSamplingRate, short newBitsPerSample, short newNumChannels )
    {
        NumSamplesPerSec  = newSamplingRate;
        NumChannels       = newNumChannels;
        NumBitsPerSample  = newBitsPerSample;
        NumAvgBytesPerSec = ( NumChannels * NumSamplesPerSec * NumBitsPerSample ) / 8;
        NumBlockAlign     = ( short ) ( ( NumChannels * NumBitsPerSample ) / 8 );
    }
}

[PublicAPI]
public class WaveFormatChunk
{
    public WaveFormatChunk( WaveFile enclosingInstance )
    {
        EnclosingInstance = enclosingInstance;
        Header            = new RiffFile.RiffChunkHeader( enclosingInstance );
        Data              = new WaveFormatChunkData( enclosingInstance );
        Header.CkId       = RiffFile.FourCC( "fmt " );
        Header.CkSize     = 16;
    }

    public WaveFormatChunkData      Data              { get; set; }
    public WaveFile                 EnclosingInstance { get; set; }
    public RiffFile.RiffChunkHeader Header            { get; set; }

    public virtual int VerifyValidity()
    {
        var ret = ( Header.CkId == RiffFile.FourCC( "fmt " ) )
               && Data.NumChannels is 1 or 2
               && ( Data.NumAvgBytesPerSec == ( ( Data.NumChannels * Data.NumSamplesPerSec * Data.NumBitsPerSample ) / 8 ) )
               && ( Data.NumBlockAlign == ( ( Data.NumChannels * Data.NumBitsPerSample ) / 8 ) );

        return ret ? 1 : 0;
    }
}

[PublicAPI]
public class WaveFileSample
{
    public WaveFileSample( WaveFile enclosingInstance )
    {
        EnclosingInstance = enclosingInstance;
        Chan              = new short[ WaveFile.MAX_WAVE_CHANNELS ];
    }

    public short[]  Chan              { get; set; }
    public WaveFile EnclosingInstance { get; set; }
}
