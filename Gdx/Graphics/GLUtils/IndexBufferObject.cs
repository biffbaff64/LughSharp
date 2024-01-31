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


using LibGDXSharp.Gdx.Utils;
using LibGDXSharp.Gdx.Utils.Buffers;

namespace LibGDXSharp.Gdx.Graphics.GLUtils;

public class IndexBufferObject : IIndexData
{
    private readonly ShortBuffer _buffer;
    private readonly ByteBuffer  _byteBuffer;
    private readonly bool        _empty;
    private readonly bool        _ownsBuffer;
    private readonly int         _usage;
    private          uint        _bufferHandle;
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

        _bufferHandle = Core.Gdx.GL20.GLGenBuffer();
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
            Core.Gdx.GL20.GLBufferData(
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
            Core.Gdx.GL20.GLBufferData(
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
            Core.Gdx.GL20.GLBufferData(
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

        Core.Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, _bufferHandle );

        if ( _isDirty )
        {
            _byteBuffer.Limit = _buffer.Limit * 2;

            Core.Gdx.GL20.GLBufferData( IGL20.GL_ELEMENT_ARRAY_BUFFER,
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
        Core.Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, 0 );
        _isBound = false;
    }

    /// <inheritdoc />
    public void Invalidate()
    {
        _bufferHandle = Core.Gdx.GL20.GLGenBuffer();
        _isDirty      = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Core.Gdx.GL20.GLBindBuffer( IGL20.GL_ELEMENT_ARRAY_BUFFER, 0 );
        Core.Gdx.GL20.GLDeleteBuffers( _bufferHandle );

        _bufferHandle = 0;

        if ( _ownsBuffer )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
        }
    }
}
