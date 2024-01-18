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

/// <summary>
///     IndexBufferObject wraps OpenGL's index buffer functionality to be
///     used in conjunction with VBOs.
///     <para>
///         You can also use this to store indices for vertex arrays. Do not call
///         <see cref="Bind()" />" or <see cref="Unbind()" />" in this case but rather
///         use <see cref="GetBuffer(bool)" />" to use the buffer directly with
///         GLDrawElements. You must also create the IndexBufferObject with the second
///         constructor and specify isDirect as true as glDrawElements in conjunction
///         with vertex arrays needs direct buffers.
///     </para>
///     <para>
///         VertexBufferObjects must be disposed via the <see cref="Dispose()" />" method
///         when no longer needed.
///     </para>
/// </summary>
public class IndexBufferObjectSubData : IIndexData
{
    private readonly ShortBuffer _buffer;
    private readonly ByteBuffer  _byteBuffer;
    private readonly int         _usage;
    private          int         _bufferHandle;
    private          bool        _isBound = false;
    private          bool        _isDirect;
    private          bool        _isDirty = true;

    /// <summary>
    ///     Creates a new IndexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the index buffer is static </param>
    /// <param name="maxIndices"> the maximum number of indices this buffer can hold </param>
    public IndexBufferObjectSubData( bool isStatic, int maxIndices )
    {
        _byteBuffer = BufferUtils.NewByteBuffer( maxIndices * 2 );
        _isDirect   = true;

        _usage  = isStatic ? IGL20.GL_STATIC_DRAW : IGL20.GL_DYNAMIC_DRAW;
        _buffer = _byteBuffer.AsShortBuffer();

        _buffer.Flip();
        _byteBuffer.Flip();

        _bufferHandle = CreateBufferObject();
    }

    /// <summary>
    ///     Creates a new IndexBufferObject to be used with vertex arrays.
    /// </summary>
    /// <param name="maxIndices"> the maximum number of indices this buffer can hold </param>
    public IndexBufferObjectSubData( int maxIndices )
    {
        _byteBuffer = BufferUtils.NewByteBuffer( maxIndices * 2 );
        _isDirect   = true;

        _usage  = IGL20.GL_STATIC_DRAW;
        _buffer = _byteBuffer.AsShortBuffer();

        _buffer.Flip();
        _byteBuffer.Flip();

        _bufferHandle = CreateBufferObject();
    }

    /// <inheritdoc />
    public int NumIndices => _buffer.Limit;

    /// <inheritdoc />
    public int NumMaxIndices => _buffer.Capacity;

    /// <inheritdoc />
    public void SetIndices( short[] indices, int offset, int count )
    {
        _isDirty = true;

        _buffer.Clear();
        _buffer.Put( indices, offset, count );
        _buffer.Flip();

        _byteBuffer.Position = 0;
        _byteBuffer.Limit    = count << 1;

        if ( _isBound )
        {
            Gdx.GL20.GLBufferSubData( IGL20.GL_ELEMENT_ARRAY_BUFFER,
                                      0,
                                      _byteBuffer.Limit,
                                      _byteBuffer );

            _isDirty = false;
        }
    }

    /// <inheritdoc />
    public void SetIndices( ShortBuffer indices )
    {
        var pos = indices.Position;

        _isDirty = true;

        _buffer.Clear();
        _buffer.Put( indices );
        _buffer.Flip();

        indices.Position = pos;

        _byteBuffer.Position = 0;
        _byteBuffer.Limit    = _buffer.Limit << 1;

        if ( _isBound )
        {
            Gdx.GL20.GLBufferSubData( IGL20.GL_ELEMENT_ARRAY_BUFFER,
                                      0,
                                      _byteBuffer.Limit,
                                      _byteBuffer );

            _isDirty = false;
        }
    }

    /// <inheritdoc />
    public void UpdateIndices( int targetOffset, short[] indices, int offset, int count )
    {
        _isDirty = true;

        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 2;

        BufferUtils.Copy( indices, offset, _byteBuffer, count );

        _byteBuffer.Position = pos;
        _buffer.Position     = 0;

        if ( _isBound )
        {
            Gdx.GL20.GLBufferSubData( IGL20.GL_ELEMENT_ARRAY_BUFFER,
                                      0,
                                      _byteBuffer.Limit,
                                      _byteBuffer );

            _isDirty = false;
        }
    }

    /// <inheritdoc />
    public ShortBuffer GetBuffer( bool forWriting )
    {
        _isDirty |= forWriting;

        return _buffer;
    }

    /// <inheritdoc />
    public void Bind()
    {
        if ( _bufferHandle == 0 )
        {
            throw new GdxRuntimeException
                ( "IndexBufferObject cannot be used after it has been disposed." );
        }

        Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, _bufferHandle );

        if ( _isDirty )
        {
            _byteBuffer.Limit = _buffer.Limit * 2;

            Gdx.GL20.GLBufferSubData( IGL20.GL_ELEMENT_ARRAY_BUFFER,
                                      0,
                                      _byteBuffer.Limit,
                                      _byteBuffer );

            _isDirty = false;
        }

        _isBound = true;
    }

    /// <inheritdoc />
    public void Unbind()
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, 0 );
        _isBound = false;
    }

    /// <inheritdoc />
    public void Invalidate()
    {
        _bufferHandle = CreateBufferObject();
        _isDirty      = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, 0 );
        Gdx.GL20.GLDeleteBuffers( _bufferHandle );

        _bufferHandle = 0;
    }

    private int CreateBufferObject()
    {
        var result = Gdx.GL20.GLGenBuffer();

        Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, result );
        Gdx.GL20.GLBufferData( IGL20.GL_ELEMENT_ARRAY_BUFFER, _byteBuffer.Capacity, null!, _usage );
        Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, 0 );

        return result;
    }
}
