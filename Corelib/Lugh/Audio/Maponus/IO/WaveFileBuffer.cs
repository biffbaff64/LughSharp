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

using Corelib.Lugh.Audio.Maponus.Decoding;

namespace Corelib.Lugh.Audio.Maponus.IO;

/// <summary>
/// Implements an Obuffer by writing the data to a file in RIFF WAVE format.
/// </summary>
[PublicAPI]
public class WaveFileBuffer : AudioBase
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
            _bufferp[ i ] = ( short ) i;
        }

        _outWave = new WaveFile();
        _outWave.OpenForWrite( fileName, null, freq, 16, ( short ) _channels );
    }

    public WaveFileBuffer( int numberOfChannels, int freq, Stream stream )
    {
        _buffer   = new short[ OBUFFERSIZE ];
        _bufferp  = new short[ MAXCHANNELS ];
        _channels = numberOfChannels;

        for ( var i = 0; i < numberOfChannels; ++i )
        {
            _bufferp[ i ] = ( short ) i;
        }

        _outWave = new WaveFile();
        _outWave.OpenForWrite( null!, stream, freq, 16, ( short ) _channels );
    }

    /// <summary>
    /// Takes a 16 Bit PCM sample.
    /// </summary>
    protected override void Append( int channel, short valueRenamed )
    {
        _buffer[ _bufferp[ channel ] ] = valueRenamed;
        _bufferp[ channel ]            = ( short ) ( _bufferp[ channel ] + _channels );
    }

    public override void WriteBuffer( int val )
    {
        _outWave.WriteData( _buffer, _bufferp[ 0 ] );

        for ( var i = 0; i < _channels; ++i )
        {
            _bufferp[ i ] = ( short ) i;
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
    /// </summary>
    public override void ClearBuffer()
    {
    }

    /// <summary>
    /// </summary>
    public override void SetStopFlag()
    {
    }
}
