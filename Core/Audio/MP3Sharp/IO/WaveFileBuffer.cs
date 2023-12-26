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
/// Implements an Obuffer by writing the data to a file in RIFF WAVE format.
/// </summary>
[PublicAPI]
public class WaveFileBuffer : ABuffer
{
    private readonly short[]  _buffer;
    private readonly short[]  _bufferp;
    private readonly int      _channels;
    private readonly WaveFile _outWave;

    public WaveFileBuffer( int numberOfChannels, int freq, string fileName )
    {
        ArgumentNullException.ThrowIfNull( fileName );

        _buffer   = new short[ OBUFFERSIZE ];
        _bufferp  = new short[ MAXCHANNELS ];
        _channels = numberOfChannels;

        for ( var i = 0; i < numberOfChannels; ++i )
        {
            _bufferp[ i ] = ( short )i;
        }

        _outWave = new WaveFile();
        _outWave.OpenForWrite( fileName, null, freq, 16, ( short )_channels );
    }

    public WaveFileBuffer( int numberOfChannels, int freq, Stream stream )
    {
        _buffer   = new short[ OBUFFERSIZE ];
        _bufferp  = new short[ MAXCHANNELS ];
        _channels = numberOfChannels;

        for ( var i = 0; i < numberOfChannels; ++i )
        {
            _bufferp[ i ] = ( short )i;
        }

        _outWave = new WaveFile();
        _outWave.OpenForWrite( null!, stream, freq, 16, ( short )_channels );
    }

    /// <summary>
    /// Takes a 16 Bit PCM sample.
    /// </summary>
    protected override void Append( int channel, short valueRenamed )
    {
        _buffer[ _bufferp[ channel ] ] = valueRenamed;
        _bufferp[ channel ]            = ( short )( _bufferp[ channel ] + _channels );
    }

    public override void WriteBuffer( int val )
    {
        _outWave.WriteData( _buffer, _bufferp[ 0 ] );

        for ( var i = 0; i < _channels; ++i )
        {
            _bufferp[ i ] = ( short )i;
        }
    }

    public void Close( bool justWriteLengthBytes )
    {
        _outWave.Close( justWriteLengthBytes );
    }

    public override void Close()
    {
        _outWave.Close();
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
