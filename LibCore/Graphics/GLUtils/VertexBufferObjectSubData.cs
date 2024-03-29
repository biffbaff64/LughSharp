﻿// ///////////////////////////////////////////////////////////////////////////////
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


using LughSharp.LibCore.Utils.Buffers;

namespace LughSharp.LibCore.Graphics.GLUtils;

/// <summary>
///     A <see cref="IVertexData" /> implementation based on OpenGL vertex buffer objects.
///     If the OpenGL ES context was lost you can call <see cref="Invalidate()" /> to
///     recreate a new OpenGL vertex buffer object.
///     <para>
///         The data is bound via GLVertexAttribPointer() according to the attribute aliases
///         specified via <see cref="VertexAttributes" /> in the constructor. VertexBufferObjects
///         must be disposed via the <see cref="Dispose()" /> method when no longer needed.
///     </para>
/// </summary>
public class VertexBufferObjectSubData : IVertexData
{
    private readonly FloatBuffer _buffer;
    private readonly bool        _isDirect;
    private readonly int         _usage;

    private int  _bufferHandle;
    private bool _isBound  = false;
    private bool _isDirty  = false;
    private bool _isStatic = false;

    /// <summary>
    ///     Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <seealso cref="VertexAttributes" />.  </param>
    public VertexBufferObjectSubData( bool isStatic, int numVertices, params VertexAttribute[] attributes )
        : this( isStatic, numVertices, new VertexAttributes( attributes ) )
    {
    }

    /// <summary>
    ///     Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the {@link VertexAttributes}. </param>
    public VertexBufferObjectSubData( bool isStatic, int numVertices, VertexAttributes attributes )
    {
        _isStatic  = isStatic;
        Attributes = attributes;

        ByteBuffer = BufferUtils.NewByteBuffer( Attributes.VertexSize * numVertices );
        _isDirect  = true;

        _usage  = isStatic ? IGL20.GL_STATIC_DRAW : IGL20.GL_DYNAMIC_DRAW;
        _buffer = ByteBuffer.AsFloatBuffer();

        _bufferHandle = CreateBufferObject();

        _buffer.Flip();
        ByteBuffer.Flip();
    }

    public ByteBuffer       ByteBuffer { get; set; }
    public VertexAttributes Attributes { get; set; }

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertexData stores </returns>
    public int NumVertices => ( _buffer.Limit * 4 ) / Attributes.VertexSize;

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertedData can store </returns>
    public int NumMaxVertices => ByteBuffer.Capacity / Attributes.VertexSize;

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
        _isDirty = true;

        if ( _isDirect )
        {
            BufferUtils.Copy( vertices, ByteBuffer, count, offset );

            _buffer.Position = 0;
            _buffer.Limit    = count;
        }
        else
        {
            _buffer.Clear();
            _buffer.Put( vertices, offset, count );

            _buffer.Flip();
            ByteBuffer.Position = 0;
            ByteBuffer.Limit    = _buffer.Limit << 2;
        }

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
        _isDirty = true;

        if ( _isDirect )
        {
            var pos = ByteBuffer.Position;

            ByteBuffer.Position = targetOffset * 4;
            BufferUtils.Copy( vertices, sourceOffset, count, ByteBuffer );
            ByteBuffer.Position = pos;
        }
        else
        {
            throw new GdxRuntimeException( "Buffer must be allocated direct." ); // Should never happen
        }

        BufferChanged();
    }

    /// <summary>
    ///     Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    ///     contents to be uploaded on the next call to bind. If you need immediate
    ///     uploading use <see cref="IVertexData.SetVertices" />; Any modifications made
    ///     to the Buffer after the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.  </returns>
    public FloatBuffer GetBuffer( bool forWriting )
    {
        _isDirty |= forWriting;

        return _buffer;
    }

    /// <summary>
    ///     Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.</param>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, _bufferHandle );

        if ( _isDirty )
        {
            ByteBuffer.Limit = _buffer.Limit * 4;
            Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, ByteBuffer.Limit, ByteBuffer, _usage );
            _isDirty = false;
        }

        var numAttributes = Attributes.Size;

        if ( locations == null )
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                VertexAttribute attribute = Attributes.Get( i );
                var             location  = shader.GetAttributeLocation( attribute.alias );

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
        }
        else
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                VertexAttribute attribute = Attributes.Get( i );
                var             location  = locations[ i ];

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
        }

        _isBound = true;
    }

    /// <summary>
    ///     Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.</param>
    public void Unbind( ShaderProgram shader, int[]? locations = null )
    {
        Debug.Assert( Attributes != null, "Attributes != null" );

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

        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );
        _isBound = false;
    }

    /// <summary>
    ///     Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = CreateBufferObject();
        _isDirty      = true;
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing,
    ///     or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );
        Gdx.GL20.GLDeleteBuffer( _bufferHandle );
        _bufferHandle = 0;
    }

    private int CreateBufferObject()
    {
        var result = Gdx.GL20.GLGenBuffer();

        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, result );
        Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, ByteBuffer.Capacity, null!, _usage );
        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );

        return result;
    }

    private void BufferChanged()
    {
        if ( _isBound )
        {
            Gdx.GL20.GLBufferSubData( IGL20.GL_ARRAY_BUFFER, 0, ByteBuffer.Limit, ByteBuffer );
            _isDirty = false;
        }
    }
}
