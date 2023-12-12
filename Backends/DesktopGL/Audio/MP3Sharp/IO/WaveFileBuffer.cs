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

using MP3Sharp.Decoding;

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;

/// <summary> Implements an Obuffer by writing the data to a file in RIFF WAVE format.</summary>
public class WaveFileBuffer : ABuffer
{
    private readonly short[]  _Buffer;
    private readonly short[]  _Bufferp;
    private readonly int      _Channels;
    private readonly WaveFile _OutWave;

    public WaveFileBuffer( int numberOfChannels, int freq, string fileName )
    {
        if ( fileName == null )
            throw new NullReferenceException( "FileName" );

        _Buffer   = new short[ OBUFFERSIZE ];
        _Bufferp  = new short[ MAXCHANNELS ];
        _Channels = numberOfChannels;

        for ( int i = 0; i < numberOfChannels; ++i )
            _Bufferp[ i ] = ( short )i;

        _OutWave = new WaveFile();

        int rc = _OutWave.OpenForWrite( fileName, null, freq, 16, ( short )_Channels );
    }

    public WaveFileBuffer( int numberOfChannels, int freq, Stream stream )
    {
        _Buffer   = new short[ OBUFFERSIZE ];
        _Bufferp  = new short[ MAXCHANNELS ];
        _Channels = numberOfChannels;

        for ( int i = 0; i < numberOfChannels; ++i )
            _Bufferp[ i ] = ( short )i;

        _OutWave = new WaveFile();

        int rc = _OutWave.OpenForWrite( null, stream, freq, 16, ( short )_Channels );
    }

    /// <summary>
    /// Takes a 16 Bit PCM sample.
    /// </summary>
    protected override void Append( int channel, short valueRenamed )
    {
        _Buffer[ _Bufferp[ channel ] ] = valueRenamed;
        _Bufferp[ channel ]            = ( short )( _Bufferp[ channel ] + _Channels );
    }

    public override void WriteBuffer( int val )
    {
        int rc = _OutWave.WriteData( _Buffer, _Bufferp[ 0 ] );

        for ( int i = 0; i < _Channels; ++i )
            _Bufferp[ i ] = ( short )i;
    }

    public void Close( bool justWriteLengthBytes )
    {
        _OutWave.Close( justWriteLengthBytes );
    }

    public override void Close()
    {
        _OutWave.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void ClearBuffer()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public override void SetStopFlag()
    {
    }
}
