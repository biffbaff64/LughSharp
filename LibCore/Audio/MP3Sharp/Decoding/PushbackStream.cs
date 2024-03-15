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


using Exception = System.Exception;

namespace LughSharp.LibCore.Audio.MP3Sharp.Decoding;

/// <summary>
///     A PushbackStream is a stream that can "push back" or "unread" data. This is useful in
///     situations where it is convenient for a fragment of code to read an indefinite number
///     of data bytes that are delimited by a particular byte value; after reading the terminating
///     byte, the code fragment can "unread" it, so that the next read operation on the input stream
///     will reread the byte that was pushed back.
/// </summary>
public class PushbackStream
{
    private readonly int                _backBufferSize;
    private readonly CircularByteBuffer _circularByteBuffer;
    private readonly Stream             _stream;
    private readonly byte[]             _temporaryBuffer;
    private          int                _numForwardBytesInBuffer;

    public PushbackStream( Stream s, int backBufferSize )
    {
        _stream             = s;
        _backBufferSize     = backBufferSize;
        _temporaryBuffer    = new byte[ _backBufferSize ];
        _circularByteBuffer = new CircularByteBuffer( _backBufferSize );
    }

    public int Read( sbyte[] readBuffer, int offset, int length )
    {
        var index         = 0;
        var canReadStream = true;

        while ( ( index < length ) && canReadStream )
        {
            if ( _numForwardBytesInBuffer > 0 )
            {
                // from memory
                _numForwardBytesInBuffer--;
                readBuffer[ offset + index ] = ( sbyte )_circularByteBuffer[ _numForwardBytesInBuffer ];
                index++;
            }
            else
            {
                // from stream
                var countBytesToRead = ( length - index ) > _temporaryBuffer.Length ? _temporaryBuffer.Length : length - index;
                var countBytesRead   = _stream.Read( _temporaryBuffer, 0, countBytesToRead );
                canReadStream = countBytesRead >= countBytesToRead;

                for ( var i = 0; i < countBytesRead; i++ )
                {
                    _circularByteBuffer.Push( _temporaryBuffer[ i ] );
                    readBuffer[ offset + index + i ] = ( sbyte )_temporaryBuffer[ i ];
                }

                index += countBytesRead;
            }
        }

        return index;
    }

    public void UnRead( int length )
    {
        _numForwardBytesInBuffer += length;

        if ( _numForwardBytesInBuffer > _backBufferSize )
        {
            throw new Exception( "The backstream cannot unread the requested number of bytes." );
        }
    }

    public void Close()
    {
        _stream.Close();
    }
}
