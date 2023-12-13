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

using System;
using System.IO;

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;


/// <summary>
/// A PushbackStream is a stream that can "push back" or "unread" data. This is useful in situations where it is convenient for a
/// fragment of code to read an indefinite number of data bytes that are delimited by a particular byte value; after reading the
/// terminating byte, the code fragment can "unread" it, so that the next read operation on the input stream will reread the byte
/// that was pushed back.
/// </summary>
public class PushbackStream {
    private readonly int                _BackBufferSize;
    private readonly CircularByteBuffer _CircularByteBuffer;
    private readonly Stream             _Stream;
    private readonly byte[]             _TemporaryBuffer;
    private          int                _NumForwardBytesInBuffer;

    public PushbackStream(Stream s, int backBufferSize) {
        _Stream             = s;
        _BackBufferSize     = backBufferSize;
        _TemporaryBuffer    = new byte[_BackBufferSize];
        _CircularByteBuffer = new CircularByteBuffer(_BackBufferSize);
    }

    public int Read(sbyte[] readBuffer, int offset, int length) {
        // Read 
        int  index         = 0;
        bool canReadStream = true;
        while (index < length && canReadStream) {
            if (_NumForwardBytesInBuffer > 0) {
                // from memory
                _NumForwardBytesInBuffer--;
                readBuffer[offset + index] = (sbyte)_CircularByteBuffer[_NumForwardBytesInBuffer];
                index++;
            }
            else {
                // from stream
                int countBytesToRead = length - index > _TemporaryBuffer.Length ? _TemporaryBuffer.Length : length - index;
                int countBytesRead   = _Stream.Read(_TemporaryBuffer, 0, countBytesToRead);
                canReadStream = countBytesRead >= countBytesToRead;
                for (int i = 0; i < countBytesRead; i++) {
                    _CircularByteBuffer.Push(_TemporaryBuffer[i]);
                    readBuffer[offset + index + i] = (sbyte)_TemporaryBuffer[i];
                }
                index += countBytesRead;
            }
        }
        return index;
    }

    public void UnRead(int length) {
        _NumForwardBytesInBuffer += length;
        if (_NumForwardBytesInBuffer > _BackBufferSize) {
            throw new Exception("The backstream cannot unread the requested number of bytes.");
        }
    }

    public void Close() {
        _Stream.Close();
    }
}