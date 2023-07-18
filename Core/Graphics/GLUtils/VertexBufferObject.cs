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

using Buffer = LibGDXSharp.Utils.Buffers.Buffer;

namespace LibGDXSharp.Graphics.GLUtils;

public sealed class VertexBufferObject : IVertexData
{
//    private VertexAttributes _attributes;
    private FloatBuffer _buffer;
    private ByteBuffer?  _byteBuffer;
    private bool        _ownsBuffer;
    private int         _bufferHandle;
    private int         _usage;
    private bool        _isDirty = false;
    private bool        _isBound = false;

    public VertexBufferObject( bool isStatic, int maxVertices, VertexAttributes attributes )
    {
        throw new NotImplementedException();
    }

    /// <returns> the number of vertices this VertexData stores </returns>
    public int NumVertices { get; set; }

    /// <returns> the number of vertices this VertedData can store </returns>
    public int NumMaxVertices { get; set; }

    public int Usage
    {
        get => _usage;
        set
        {
            if ( _isBound ) throw new GdxRuntimeException( "Cannot change usage while VBO is bound" );

            _usage = value;
        }
    }

    /// <returns> the <see cref="VertexAttributes"/> as specified during construction. </returns>
    public VertexAttributes Attributes { get; set; }

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="SetVertices"/>; Any modifications made to the Buffer
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
        set => throw new GdxRuntimeException
            ( "DO NOT USE Buffer.Set: Call SetBuffer(Buffer, bool, VertexAtrtributes) instead." );
    }

    /// <summary>
    /// Low level method to reset the buffer and attributes to the specified values. Use with care!
    /// </summary>
    public void SetBuffer( Buffer data, bool ownsBuffer, VertexAttributes value )
    {
        if ( _isBound ) throw new GdxRuntimeException( "Cannot change attributes while VBO is bound" );

        if ( this._ownsBuffer && ( _byteBuffer != null ) )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
        }

        Attributes = value;
        
        if ( data is ByteBuffer buffer)
        {
            _byteBuffer = buffer;
        }
        else
        {
            throw new GdxRuntimeException( "Only ByteBuffer is currently supported" );
        }
        
        this._ownsBuffer = ownsBuffer;

        int l = _byteBuffer.Limit;
        
        _byteBuffer.Limit = _byteBuffer.Capacity;
        
        buffer = byteBuffer.asFloatBuffer();
        
        ( ( Buffer )byteBuffer ).limit( l );
        ( ( Buffer )buffer ).limit( l / 4 );
    }

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
    }

    /// <summary>
    /// Update (a portion of) the vertices. Does not resize the backing buffer. </summary>
    /// <param name="targetOffset"></param>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count )
    {
    }

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        IGL20 gl = Gdx.GL20;

        gl.GLBindBuffer( IGL20.GL_Array_Buffer, _bufferHandle );

        if ( _isDirty )
        {
            _byteBuffer.Limit = ( Buffer.Limit * 4 );
            gl.GLBufferData( IGL20.GL_Array_Buffer, _byteBuffer.Limit, _byteBuffer, Usage );
            _isDirty = false;
        }

        int numAttributes = Attributes.Size;

        for ( int i = 0; i < numAttributes; i++ )
        {
            VertexAttribute attribute = Attributes.Get( i );

            int location = ( locations == null )
                ? shader.GetAttributeLocation( attribute.alias )
                : locations[ i ];

            if ( location < 0 ) continue;

            shader.EnableVertexAttribute( location );

            shader.SetVertexAttribute
                (
                location, attribute.numComponents, attribute.type, attribute.normalized,
                Attributes.VertexSize, attribute.Offset
                );
        }

        _isBound = true;
    }

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Unbind( ShaderProgram shader, int[]? locations = null )
    {
        IGL20 gl            = Gdx.GL20;
        int   numAttributes = Attributes.Size;

        if ( locations == null )
        {
            for ( int i = 0; i < numAttributes; i++ )
            {
                shader.DisableVertexAttribute( Attributes.Get( i ).alias );
            }
        }
        else
        {
            for ( int i = 0; i < numAttributes; i++ )
            {
                int location = locations[ i ];

                if ( location >= 0 ) shader.DisableVertexAttribute( location );
            }
        }

        gl.GLBindBuffer( IGL20.GL_Array_Buffer, 0 );
        _isBound = false;
    }

    private void BufferChanged()
    {
        if ( _isBound )
        {
            Gdx.GL20.GLBufferData( IGL20.GL_Array_Buffer, _byteBuffer.Limit, _byteBuffer, Usage );
            _isDirty = false;
        }
    }

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = Gdx.GL20.GLGenBuffer();
        _isDirty      = true;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        IGL20 gl = Gdx.GL20;

        gl.GLBindBuffer( IGL20.GL_Array_Buffer, 0 );
        gl.GLDeleteBuffer( _bufferHandle );

        _bufferHandle = 0;

        if ( _ownsBuffer ) BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
    }
}