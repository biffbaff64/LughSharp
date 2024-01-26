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

public class IndexBufferObject : IIndexData
{
    private readonly ShortBuffer _buffer;
    private readonly ByteBuffer  _byteBuffer;
    private readonly bool        _empty;
    private readonly bool        _ownsBuffer;
    private readonly int         _usage;
    private          int         _bufferHandle;
    private          bool        _isBound = false;
    private          bool        _isDirect;
    private          bool        _isDirty = true;

    public IndexBufferObject( int maxIndices )
        : this( true, maxIndices )
    {
    }

    public IndexBufferObject( bool isStatic, int maxIndices )
    {
        _empty = maxIndices == 0;

        if ( _empty )
        {
            maxIndices = 1;
        }

        _byteBuffer = BufferUtils.NewByteBuffer( maxIndices * 2 );
        _isDirect   = true;
        _buffer     = _byteBuffer.AsShortBuffer();
        _ownsBuffer = true;

        _buffer.Flip();
        _byteBuffer.Flip();

        _bufferHandle = Gdx.GL20.GLGenBuffer();
        _usage        = isStatic ? IGL20.GL_STATIC_DRAW : IGL20.GL_DYNAMIC_DRAW;
    }

    /// <inheritdoc />
    public int NumIndices => _empty ? 0 : _buffer.Limit;

    /// <inheritdoc />
    public int NumMaxIndices => _empty ? 0 : _buffer.Capacity;

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
            Gdx.GL20.GLBufferData(
                IGL20.GL_ELEMENT_ARRAY_BUFFER,
                _byteBuffer.Limit,
                _byteBuffer,
                _usage
                );

            _isDirty = false;
        }
    }

    /// <inheritdoc />
    public void SetIndices( ShortBuffer indices )
    {
        _isDirty = true;

        var pos = indices.Position;

        _buffer.Clear();
        _buffer.Put( indices );
        _buffer.Flip();

        indices.Position = pos;

        _byteBuffer.Position = 0;
        _byteBuffer.Limit    = _buffer.Limit << 1;

        if ( _isBound )
        {
            Gdx.GL20.GLBufferData(
                IGL20.GL_ELEMENT_ARRAY_BUFFER,
                _byteBuffer.Limit,
                _byteBuffer,
                _usage
                );

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
            Gdx.GL20.GLBufferData(
                IGL20.GL_ELEMENT_ARRAY_BUFFER,
                _byteBuffer.Limit,
                _byteBuffer,
                _usage
                );

            _isDirty = false;
        }
    }

    /// <inheritdoc />
    public ShortBuffer GetBuffer( bool forWriting )
    {
        _isDirty = forWriting;

        return _buffer;
    }

    /// <inheritdoc />
    public void Bind()
    {
        if ( _bufferHandle == 0 )
        {
            throw new GdxRuntimeException( "No buffer allocated!" );
        }

        Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, _bufferHandle );

        if ( _isDirty )
        {
            _byteBuffer.Limit = _buffer.Limit * 2;

            Gdx.GL20.GLBufferData( IGL20.GL_ELEMENT_ARRAY_BUFFER,
                                   _byteBuffer.Limit,
                                   _byteBuffer,
                                   _usage );

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
        _bufferHandle = Gdx.GL20.GLGenBuffer();
        _isDirty      = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, 0 );
        Gdx.GL20.GLDeleteBuffers( _bufferHandle );

        _bufferHandle = 0;

        if ( _ownsBuffer )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
        }
    }
}
