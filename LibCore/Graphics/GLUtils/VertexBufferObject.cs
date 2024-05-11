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

using LughSharp.LibCore.Utils.Exceptions;
using Buffer = LughSharp.LibCore.Utils.Buffers.Buffer;

namespace LughSharp.LibCore.Graphics.GLUtils;

[PublicAPI]
public class VertexBufferObject : IVertexData
{
    private FloatBuffer _buffer;
    private int         _bufferHandle;
    private ByteBuffer? _byteBuffer;
    private bool        _isBound = false;
    private bool        _isDirty = false;
    private bool        _ownsBuffer;
    private int         _usage;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    /// <summary>
    ///     Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttribute" />s.  </param>
    public VertexBufferObject( bool isStatic, int numVertices, params VertexAttribute[] attributes )
        : this( isStatic, numVertices, new VertexAttributes( attributes ) )
    {
    }

    /// <summary>
    ///     Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttributes" />.  </param>
    public VertexBufferObject( bool isStatic, int numVertices, VertexAttributes attributes )
    {
        _buffer    = default( FloatBuffer? )!;
        Attributes = default( VertexAttributes )!;

        _bufferHandle = ( int ) Gdx.GL.glGenBuffer();

        var data = BufferUtils.NewByteBuffer( attributes.VertexSize * numVertices );

        data.Limit = 0;

        SetBuffer( data, true, attributes );
        Usage = isStatic ? IGL.GL_STATIC_DRAW : IGL.GL_DYNAMIC_DRAW;
    }

    /// <summary>
    ///     Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="usage"></param>
    /// <param name="data"></param>
    /// <param name="ownsBuffer"></param>
    /// <param name="attributes"> the <see cref="VertexAttributes" />.  </param>
    public VertexBufferObject( int usage, ByteBuffer data, bool ownsBuffer, VertexAttributes attributes )
    {
        _buffer    = default( FloatBuffer? )!;
        Attributes = default( VertexAttributes )!;

        _bufferHandle = ( int ) Gdx.GL.glGenBuffer();

        SetBuffer( data, ownsBuffer, attributes );
        Usage = usage;
    }

    public int Usage
    {
        get => _usage;
        init
        {
            if ( _isBound )
            {
                throw new GdxRuntimeException( "Cannot change usage while VBO is bound" );
            }

            _usage = value;
        }
    }

    /// <summary>
    ///     Returns the number of vertices this VertexData stores.
    /// </summary>
    public int NumVertices { get; set; }

    /// <summary>
    ///     Returns the number of vertices this VertedData can store.
    /// </summary>
    public int NumMaxVertices { get; set; }

    /// <summary>
    ///     Returns the <see cref="VertexAttributes" /> as specified during construction.
    /// </summary>
    public VertexAttributes Attributes { get; set; }

    /// <summary>
    ///     Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    ///     contents to be uploaded on the next call to bind. If you need immediate
    ///     uploading use <see cref="SetVertices" />; Any modifications made to the Buffer
    ///     after* the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.  </returns>
    public FloatBuffer GetBuffer( bool forWriting )
    {
        _isDirty |= forWriting;

        return _buffer;
    }

    /// <summary>
    ///     Sets the vertices of this VertexData, discarding the old vertex data. The
    ///     count must equal the number of floats per vertex times the number of vertices
    ///     to be copied to this VertexData. The order of the vertex attributes must be
    ///     the same as specified at construction time via <see cref="VertexAttributes" />.
    ///     <para>
    ///         This can be called in between calls to bind and unbind. The vertex data will
    ///         be updated instantly.
    ///     </para>
    /// </summary>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="offset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void SetVertices( float[] vertices, int offset, int count )
    {
        if ( _byteBuffer == null )
        {
            throw new GdxRuntimeException( "Byte buffer cannot be null!" );
        }

        if ( _buffer == null )
        {
            throw new GdxRuntimeException( "Buffer cannot be null!" );
        }

        _isDirty = true;

        BufferUtils.Copy( vertices, _byteBuffer, count, offset );

        _buffer.Position = 0;
        _buffer.Limit    = count;

        BufferChanged();
    }

    /// <summary>
    ///     Update (a portion of) the vertices. Does not resize the backing buffer.
    /// </summary>
    /// <param name="targetOffset"></param>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count )
    {
        if ( _byteBuffer == null )
        {
            Logger.Error( "_byteBuffer is NULL!" );

            return;
        }

        _isDirty = true;

        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 4;

        BufferUtils.Copy( vertices, sourceOffset, count, _byteBuffer );

        _byteBuffer.Position = pos;
        _buffer.Position     = 0;

        BufferChanged();
    }

    /// <summary>
    ///     Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        Gdx.GL.glBindBuffer( IGL.GL_ARRAY_BUFFER, ( uint ) _bufferHandle );

        if ( _isDirty )
        {
            if ( ( _byteBuffer == null ) || ( _buffer == null ) )
            {
                throw new NullReferenceException();
            }

            unsafe
            {
                _byteBuffer.Limit = _buffer.Limit * 4;

                fixed ( void* ptr = &_byteBuffer.BackingArray()[ 0 ] )
                {
                    Gdx.GL.glBufferData( IGL.GL_ARRAY_BUFFER, _byteBuffer.Limit, ptr, Usage );
                }

                _isDirty = false;
            }
        }

        var numAttributes = Attributes.Size;

        for ( var i = 0; i < numAttributes; i++ )
        {
            var attribute = Attributes.Get( i );

            var location = locations == null
                               ? shader.GetAttributeLocation( attribute.alias )
                               : locations[ i ];

            if ( location < 0 )
            {
                continue;
            }

            shader.EnableVertexAttribute( location );

            shader.SetVertexAttribute( location,
                                       attribute.numComponents,
                                       attribute.type,
                                       attribute.normalized,
                                       Attributes.VertexSize,
                                       attribute.Offset );
        }

        _isBound = true;
    }

    /// <summary>
    ///     Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Unbind( ShaderProgram shader, int[]? locations = null )
    {
        var numAttributes = Attributes.Size;

        if ( locations == null )
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                shader.DisableVertexAttribute( Attributes.Get( i ).alias );
            }
        }
        else
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                var location = locations[ i ];

                if ( location >= 0 )
                {
                    shader.DisableVertexAttribute( location );
                }
            }
        }

        Gdx.GL.glBindBuffer( IGL.GL_ARRAY_BUFFER, 0 );
        _isBound = false;
    }

    /// <summary>
    ///     Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = ( int ) Gdx.GL.glGenBuffer();
        _isDirty      = true;
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing,
    ///     or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL.glBindBuffer( IGL.GL_ARRAY_BUFFER, 0 );
        Gdx.GL.glDeleteBuffers( ( uint ) _bufferHandle );

        _bufferHandle = 0;

        if ( _ownsBuffer )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer! );
        }
    }

    /// <summary>
    ///     Low level method to reset the buffer and attributes to the specified values. Use with care!
    /// </summary>
    public void SetBuffer( Buffer data, bool ownsBuffer, VertexAttributes value )
    {
        if ( _isBound )
        {
            throw new GdxRuntimeException( "Cannot change attributes while VBO is bound" );
        }

        if ( _ownsBuffer && ( _byteBuffer != null ) )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
        }

        Attributes = value;

        if ( data is ByteBuffer buffer )
        {
            _byteBuffer = buffer;
        }
        else
        {
            throw new GdxRuntimeException( "Only ByteBuffer is currently supported" );
        }

        _ownsBuffer = ownsBuffer;

        var lim = _byteBuffer.Limit;

        _byteBuffer.Limit = _byteBuffer.Capacity;

        _buffer = _byteBuffer.AsFloatBuffer();

        _byteBuffer.Limit = lim;
        _buffer.Limit     = lim / 4;
    }

    private unsafe void BufferChanged()
    {
        if ( _isBound )
        {
            fixed ( void* ptr = &_byteBuffer!.BackingArray()[ 0 ] )
            {
                Gdx.GL.glBufferData( IGL.GL_ARRAY_BUFFER, _byteBuffer!.Limit, ptr, Usage );
            }

            _isDirty = false;
        }
    }
}