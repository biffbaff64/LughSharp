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

using Corelib.LibCore.Utils.Buffers;

namespace Corelib.LibCore.Graphics.GLUtils;

[PublicAPI]
public class IndexArray : IIndexData
{
    private readonly bool        _empty;
    private          ShortBuffer _buffer;
    private          ByteBuffer  _byteBuffer;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    /// <summary>
    /// Creates a new IndexArray to be used with vertex arrays.
    /// </summary>
    /// <param name="maxIndices"> the maximum number of indices this buffer can hold  </param>
    public IndexArray( int maxIndices )
    {
        _empty = maxIndices == 0;

        if ( _empty )
        {
            maxIndices = 1; 
        }

        _byteBuffer = BufferUtils.NewByteBuffer( maxIndices * 2, false );

        _buffer = _byteBuffer.AsShortBuffer();
        _buffer.Flip();

        _byteBuffer.Flip();
    }

    /// <summary>
    /// Returns the number of indices currently stored in this buffer.
    /// </summary>
    public int NumIndices => _empty ? 0 : _buffer.Limit;

    /// <summary>
    /// Returns the maximum number of indices this IndexArray can store.
    /// </summary>
    public int NumMaxIndices => _empty ? 0 : _buffer.Capacity;

    /// <summary>
    /// Sets the indices of this IndexArray, discarding the old indices. The count must
    /// equal the number of indices to be copied to this IndexArray.
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
        _buffer.Clear();
        _buffer.Put( indices, offset, count );
        _buffer.Flip();

        _byteBuffer.Position = 0;
        _byteBuffer.Limit    = count << 1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="indices"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetOffset"></param>
    /// <param name="indices"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    public void UpdateIndices( int targetOffset, short[] indices, int offset, int count )
    {
        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 2;

        BufferUtils.Copy( indices, offset, _byteBuffer, count );

        _byteBuffer.Position = pos;
    }

    /// <summary>
    /// Returns the underlying ShortBuffer. If you modify the buffer contents
    /// they wil be uploaded on the call to <see cref="Bind()"/>.
    /// If you need immediate uploading use <see cref="SetIndices(short[], int, int)"/>.
    /// </summary>
    /// <returns> the underlying short buffer. </returns>
    public virtual ShortBuffer GetBuffer( bool forWriting )
    {
        return _buffer;
    }

    /// <summary>
    /// Binds this IndexArray for rendering with glDrawElements. Default method is empty.
    /// </summary>
    public virtual void Bind()
    {
    }

    /// <summary>
    /// Unbinds this IndexArray. Default method is empty.
    /// </summary>
    public virtual void Unbind()
    {
    }

    /// <summary>
    /// Invalidates the IndexArray so a new OpenGL buffer handle is created.
    /// Use this in case of a context loss. Default method is empty.
    /// </summary>
    public virtual void Invalidate()
    {
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
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
