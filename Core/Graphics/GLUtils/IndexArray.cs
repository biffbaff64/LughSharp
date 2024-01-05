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

using LibGDXSharp.Files.Buffers;

namespace LibGDXSharp.Graphics.GLUtils;

public class IndexArray : IIndexData, IDisposable
{

    // used to work around bug: https://android-review.googlesource.com/#/c/73175/
    private readonly bool        _empty;
    private          ShortBuffer _buffer;
    private          ByteBuffer  _byteBuffer;

    /// <summary>
    ///     Creates a new IndexArray to be used with vertex arrays.
    /// </summary>
    /// <param name="maxIndices"> the maximum number of indices this buffer can hold  </param>
    public IndexArray( int maxIndices )
    {
        _empty = maxIndices == 0;

        if ( _empty )
        {
            maxIndices = 1; // avoid allocating a zero-sized buffer because of a bug in Android's ART < Android 5.0
        }

        _byteBuffer = BufferUtils.NewByteBuffer( maxIndices * 2 );

        _buffer = _byteBuffer.AsShortBuffer();
        _buffer.Flip();

        _byteBuffer.Flip();
    }

    /// <summary>
    ///     Returns the number of indices currently stored in this buffer.
    /// </summary>
    public int NumIndices => _empty ? 0 : _buffer.Limit;

    /// <summary>
    ///     Returns the maximum number of indices this IndexArray can store.
    /// </summary>
    public int NumMaxIndices => _empty ? 0 : _buffer.Capacity;

    /// <summary>
    ///     Sets the indices of this IndexArray, discarding the old indices.
    ///     The count must equal the number of indices to be copied to
    ///     this IndexArray.
    ///     <para>
    ///         This can be called in between calls to <see cref="Bind()" /> and
    ///         <see cref="Unbind()" />. The index data will be updated instantly.
    ///     </para>
    /// </summary>
    /// <param name="indices"> the vertex data </param>
    /// <param name="offset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of shorts to copy  </param>
    public void SetIndices( short[] indices, int offset, int count )
    {
        _buffer.Clear();
        _buffer.Put( indices, offset, count );
        _buffer.Flip();

        _byteBuffer.Position = 0;
        _byteBuffer.Limit    = count << 1;
    }

    public void SetIndices( ShortBuffer indices )
    {
        var pos = indices.Position;

        _buffer.Clear();
        _buffer.Limit = indices.Remaining();

        _buffer.Put( indices );
        _buffer.Flip();

        indices.Position = pos;

        _byteBuffer.Position = 0;
        _byteBuffer.Limit    = _buffer.Limit << 1;
    }

    public void UpdateIndices( int targetOffset, short[] indices, int offset, int count )
    {
        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 2;

        BufferUtils.Copy( indices, offset, _byteBuffer, count );

        _byteBuffer.Position = pos;
    }

    /// <summary>
    ///     Returns the underlying ShortBuffer. If you modify the buffer contents
    ///     they wil be uploaded on the call to <see cref="Bind()" />.
    ///     If you need immediate uploading use <see cref="SetIndices(short[], int, int)" />.
    /// </summary>
    /// <returns> the underlying short buffer. </returns>
    public ShortBuffer GetBuffer( bool forWriting ) => _buffer;

    /// <summary>
    ///     Binds this IndexArray for rendering with glDrawElements.
    ///     Default method is empty.
    /// </summary>
    public virtual void Bind()
    {
    }

    /// <summary>
    ///     Unbinds this IndexArray.
    ///     Default method is empty.
    /// </summary>
    public virtual void Unbind()
    {
    }

    /// <summary>
    ///     Invalidates the IndexArray so a new OpenGL buffer handle is
    ///     created. Use this in case of a context loss.
    ///     Default method is empty.
    /// </summary>
    public virtual void Invalidate()
    {
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing,
    ///     releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _byteBuffer = null!;
            _buffer     = null!;
        }
    }
}
