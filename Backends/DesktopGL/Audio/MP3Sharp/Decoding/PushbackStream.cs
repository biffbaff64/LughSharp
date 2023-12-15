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

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;

/// <summary>
/// A PushbackStream is a stream that can "push back" or "unread" data. This is useful in
/// situations where it is convenient for a fragment of code to read an indefinite number
/// of data bytes that are delimited by a particular byte value; after reading the terminating
/// byte, the code fragment can "unread" it, so that the next read operation on the input stream
/// will reread the byte that was pushed back.
/// </summary>
[PublicAPI]
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
        var  index         = 0;
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
            throw new System.Exception( "The backstream cannot unread the requested number of bytes." );
        }
    }

    public void Close()
    {
        _stream.Close();
    }
}
