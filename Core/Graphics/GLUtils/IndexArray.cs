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

using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.Graphics.GLUtils;

[PublicAPI]
public class IndexArray : IIndexData
{
    public readonly ByteBuffer  byteBuffer;

    // used to work around bug: https://android-review.googlesource.com/#/c/73175/
    private readonly bool _empty;

    /// <summary>
    /// Creates a new IndexArray to be used with vertex arrays.
    /// </summary>
    /// <param name="maxIndices"> the maximum number of indices this buffer can hold  </param>
    public IndexArray( int maxIndices )
    {
        _empty = ( maxIndices == 0 );

        if ( _empty )
        {
            maxIndices = 1; // avoid allocating a zero-sized buffer because of a bug in Android's ART < Android 5.0
        }

        byteBuffer = BufferUtils.NewUnsafeByteBuffer( maxIndices * 2 );

        Buffer     = byteBuffer.AsShortBuffer();
        Buffer.Flip();
        
        byteBuffer.Flip();
    }

    /// <summary>
    /// Returns the number of indices currently stored in this buffer.
    /// </summary>
    public int NumIndices => _empty ? 0 : Buffer.Limit;

    /// <summary>
    /// Returns the maximum number of indices this IndexArray can store.
    /// </summary>
    public int NumMaxIndices => _empty ? 0 : Buffer.Capacity;

    /// <summary>
    /// Sets the indices of this IndexArray, discarding the old indices.
    /// The count must equal the number of indices to be copied to
    /// this IndexArray.
    /// <para>
    /// This can be called in between calls to <see cref="Bind()"/> and
    /// <see cref="Unbind()"/>. The index data will be updated instantly.
    /// </para>
    /// </summary>
    /// <param name="indices"> the vertex data </param>
    /// <param name="offset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of shorts to copy  </param>
    public void SetIndices( short[] indices, int offset, int count )
    {
        Buffer.Clear();
        Buffer.Put( indices, offset, count );
        Buffer.Flip();

        byteBuffer.Position = 0;
        byteBuffer.Limit = ( count << 1 );
    }

    public void SetIndices( ShortBuffer indices )
    {
        var pos = indices.Position;
        
        Buffer.Clear();
        Buffer.Limit = indices.Remaining();
        
        Buffer.Put( indices );
        Buffer.Flip();
        
        indices.Position = pos;
        
        byteBuffer.Position = 0;
        byteBuffer.Limit = ( Buffer.Limit << 1 );
    }

    public void UpdateIndices( int targetOffset, short[] indices, int offset, int count )
    {
        int pos = byteBuffer.Position;

        byteBuffer.Position = ( targetOffset * 2 );
        
        BufferUtils.Copy( indices, offset, byteBuffer, count );
        
        byteBuffer.Position = pos;
    }

    /// <summary>
    /// Returns the underlying ShortBuffer. If you modify the buffer contents
    /// they wil be uploaded on the call to <see cref="Bind()"/>.
    /// If you need immediate uploading use <see cref="SetIndices(short[], int, int)"/>.
    /// </summary>
    /// <returns> the underlying short buffer. </returns>
    public ShortBuffer Buffer { get; set; }

    /// <summary>
    /// Binds this IndexArray for rendering with glDrawElements.
    /// </summary>
    public void Bind()
    {
    }

    /// <summary>
    /// Unbinds this IndexArray.
    /// </summary>
    public void Unbind()
    {
    }

    /// <summary>
    /// Invalidates the IndexArray so a new OpenGL buffer handle is
    /// created. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
    }

    /// <summary>
    /// Disposes this IndexArray and all its associated OpenGL resources.
    /// </summary>
    public void Dispose()
    {
        BufferUtils.DisposeUnsafeByteBuffer( byteBuffer );
    }
}

