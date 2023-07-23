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

using System.Diagnostics;

using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// A <see cref="IVertexData"/> implementation based on OpenGL vertex buffer objects.
/// If the OpenGL ES context was lost you can call <see cref="Invalidate()"/> to
/// recreate a new OpenGL vertex buffer object.
/// <para>
/// The data is bound via GLVertexAttribPointer() according to the attribute aliases
/// specified via <see cref="VertexAttributes"/> in the constructor. VertexBufferObjects
/// must be disposed via the <see cref="Dispose()"/> method when no longer needed.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class VertexBufferObjectSubData : IVertexData
{
    public VertexAttributes? Attributes { get; set; }
    public ByteBuffer        ByteBuffer { get; set; }

    private          FloatBuffer _buffer;
    private readonly bool        _isDirect;
    private readonly int         _usage;

    private bool _isStatic;
    private int  _bufferHandle;
    private bool _isDirty = false;
    private bool _isBound = false;

    /// <summary>
    /// Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <seealso cref="VertexAttributes"/>.  </param>
    public VertexBufferObjectSubData( bool isStatic, int numVertices, params VertexAttribute[] attributes )
        : this( isStatic, numVertices, new VertexAttributes( attributes ) )
    {
    }

    /// <summary>
    /// Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the {@link VertexAttributes}. </param>
    public VertexBufferObjectSubData( bool isStatic, int numVertices, VertexAttributes attributes )
    {
        this._isStatic  = isStatic;
        this.Attributes = attributes;

        ByteBuffer = BufferUtils.NewByteBuffer( this.Attributes.VertexSize * numVertices );
        _isDirect  = true;

        _usage  = isStatic ? IGL20.GL_Static_Draw : IGL20.GL_Dynamic_Draw;
        _buffer = ByteBuffer.AsFloatBuffer();

        _bufferHandle = CreateBufferObject();

        _buffer.Flip();
        ByteBuffer.Flip();
    }

    private int CreateBufferObject()
    {
        var result = Gdx.GL20.GLGenBuffer();

        Gdx.GL20.GLBindBuffer( IGL20.GL_Array_Buffer, result );
        Gdx.GL20.GLBufferData( IGL20.GL_Array_Buffer, ByteBuffer.Capacity, null!, _usage );
        Gdx.GL20.GLBindBuffer( IGL20.GL_Array_Buffer, 0 );

        return result;
    }

    private void BufferChanged()
    {
        if ( _isBound )
        {
            Gdx.GL20.GLBufferSubData( IGL20.GL_Array_Buffer, 0, ByteBuffer.Limit, ByteBuffer );
            _isDirty = false;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertexData stores </returns>
    public int NumVertices => ( _buffer.Limit * 4 ) / Attributes!.VertexSize;

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertedData can store </returns>
    public int NumMaxVertices => ByteBuffer.Capacity / Attributes!.VertexSize;

    /// <summary>
    /// Sets the vertices of this VertexData, discarding the old vertex data. The
    /// count must equal the number of floats per vertex times the number of vertices
    /// to be copied to this VertexData. The order of the vertex attributes must be
    /// the same as specified at construction time via <see cref="VertexAttributes"/>.
    /// <para>
    /// This can be called in between calls to bind and unbind. The vertex data will
    /// be updated instantly.
    /// </para>
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
            ByteBuffer.Limit    = ( _buffer.Limit << 2 );
        }

        BufferChanged();
    }

    /// <summary>
    /// Update (a portion of) the vertices. Does not resize the backing buffer. </summary>
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

            ByteBuffer.Position = ( targetOffset * 4 );
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
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="IVertexData.SetVertices"/>; Any modifications made to the Buffer
    /// *after* the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.  </returns>
    public FloatBuffer Buffer
    {
        get
        {
            _isDirty = true;

            return _buffer;
        }
        set => _buffer = value;
    }

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.</param>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_Array_Buffer, _bufferHandle );

        if ( _isDirty )
        {
            ByteBuffer.Limit = ( _buffer.Limit * 4 );
            Gdx.GL20.GLBufferData( IGL20.GL_Array_Buffer, ByteBuffer.Limit, ByteBuffer, _usage );
            _isDirty = false;
        }

        var numAttributes = Attributes?.Size ?? 0;

        if ( locations == null )
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                VertexAttribute attribute = Attributes!.Get( i );
                var             location  = shader.GetAttributeLocation( attribute.alias );

                if ( location < 0 ) continue;

                shader.EnableVertexAttribute( location );

                shader.SetVertexAttribute
                    (
                    location, attribute.numComponents,
                    attribute.type, attribute.normalized,
                    Attributes!.VertexSize, attribute.Offset
                    );
            }
        }
        else
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                VertexAttribute attribute = Attributes!.Get( i );
                var             location  = locations[ i ];

                if ( location < 0 ) continue;

                shader.EnableVertexAttribute( location );

                shader.SetVertexAttribute
                    (
                    location, attribute.numComponents,
                    attribute.type, attribute.normalized,
                    Attributes!.VertexSize, attribute.Offset
                    );
            }
        }

        _isBound = true;
    }

    /// <summary>
    /// Unbinds this VertexData.
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

                if ( location >= 0 ) shader.DisableVertexAttribute( location );
            }
        }

        Gdx.GL20.GLBindBuffer( IGL20.GL_Array_Buffer, 0 );
        _isBound = false;
    }

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = CreateBufferObject();
        _isDirty      = true;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_Array_Buffer, 0 );
        Gdx.GL20.GLDeleteBuffer( _bufferHandle );
        _bufferHandle = 0;
    }
}