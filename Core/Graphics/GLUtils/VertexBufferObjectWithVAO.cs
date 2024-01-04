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

[PublicAPI]
public class VertexBufferObjectWithVAO : IVertexData
{
    private readonly static IntBuffer TmpHandle = BufferUtils.NewIntBuffer( 1 );

    public VertexAttributes Attributes { get; set; }

    private readonly ByteBuffer  _byteBuffer;
    private readonly bool        _ownsBuffer;
    private readonly int         _usage;
    private readonly List< int > _cachedLocations = new();

    private FloatBuffer _buffer;
    private int         _bufferHandle;
    private bool        _isStatic;
    private bool        _isDirty   = false;
    private bool        _isBound   = false;
    private int         _vaoHandle = -1;

    /// <summary>
    /// Constructs a new interleaved VertexBufferObjectWithVAO.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttribute"/>s. </param>
    public VertexBufferObjectWithVAO( bool isStatic, int numVertices, params VertexAttribute[] attributes )
        : this( isStatic, numVertices, new VertexAttributes( attributes ) )
    {
    }

    /// <summary>
    /// Constructs a new interleaved VertexBufferObjectWithVAO.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttributes"/>. </param>
    public VertexBufferObjectWithVAO( bool isStatic, int numVertices, VertexAttributes attributes )
    {
        this._isStatic  = isStatic;
        this.Attributes = attributes;

        _byteBuffer = BufferUtils.NewByteBuffer( this.Attributes.VertexSize * numVertices );
        _buffer     = _byteBuffer.AsFloatBuffer();
        _ownsBuffer = true;

        _buffer.Flip();
        _byteBuffer.Flip();

        _bufferHandle = Gdx.GL20.GLGenBuffer();
        _usage        = isStatic ? IGL20.GL_STATIC_DRAW : IGL20.GL_DYNAMIC_DRAW;

        CreateVAO();
    }

    public VertexBufferObjectWithVAO( bool isStatic, ByteBuffer unmanagedBuffer, VertexAttributes attributes )
    {
        this._isStatic  = isStatic;
        this.Attributes = attributes;

        _byteBuffer = unmanagedBuffer;
        _ownsBuffer = false;
        _buffer     = _byteBuffer.AsFloatBuffer();

        _buffer.Flip();
        _byteBuffer.Flip();

        _bufferHandle = Gdx.GL20.GLGenBuffer();
        _usage        = isStatic ? IGL20.GL_STATIC_DRAW : IGL20.GL_DYNAMIC_DRAW;

        CreateVAO();
    }

    private void BufferChanged()
    {
        if ( _isBound )
        {
            Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, _bufferHandle );
            Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Limit, _byteBuffer, _usage );
            _isDirty = false;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertexData stores </returns>
    public int NumVertices => ( _buffer.Limit * 4 ) / Attributes.VertexSize;

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertedData can store </returns>
    public int NumMaxVertices => _byteBuffer.Capacity / Attributes.VertexSize;

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

        BufferUtils.Copy( vertices, _byteBuffer, count, offset );

        _buffer.Position = 0;
        _buffer.Limit    = count;

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
        var pos = _byteBuffer.Position;

        _byteBuffer.Position = ( targetOffset * 4 );

        BufferUtils.Copy( vertices, sourceOffset, count, _byteBuffer );

        _byteBuffer.Position = pos;
        _buffer.Position     = 0;

        BufferChanged();
    }

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="IVertexData.SetVertices"/>; Any modifications made to the Buffer
    /// after* the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.  </returns>
    public FloatBuffer GetBuffer( bool forWriting )
    {
        _isDirty |= forWriting;

        return _buffer;
    }

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        Gdx.GL30?.GLBindVertexArray( _vaoHandle );

        BindAttributes( shader, locations );

        //if our data has changed upload it:
        BindData( Gdx.GL30 );

        _isBound = true;
    }

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Unbind( ShaderProgram? shader, int[]? locations = null )
    {
        Gdx.GL30?.GLBindVertexArray( 0 );
        _isBound = false;
    }

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = Gdx.GL30!.GLGenBuffer();

        CreateVAO();

        _isDirty = true;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL30?.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );
        Gdx.GL30?.GLDeleteBuffers( _bufferHandle );

        _bufferHandle = 0;

        if ( _ownsBuffer )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
        }

        DeleteVAO();
    }

    private void BindAttributes( ShaderProgram shader, int[]? locations )
    {
        var stillValid    = this._cachedLocations.Count != 0;
        var numAttributes = Attributes.Size;

        if ( stillValid )
        {
            if ( locations == null )
            {
                for ( var i = 0; stillValid && ( i < numAttributes ); i++ )
                {
                    VertexAttribute attribute = Attributes.Get( i );

                    var location = shader.GetAttributeLocation( attribute.alias );

                    stillValid = location == this._cachedLocations[ i ];
                }
            }
            else
            {
                stillValid = locations.Length == this._cachedLocations.Count;

                for ( var i = 0; stillValid && ( i < numAttributes ); i++ )
                {
                    stillValid = locations[ i ] == this._cachedLocations[ i ];
                }
            }
        }

        if ( !stillValid )
        {
            Gdx.GL.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, _bufferHandle );

            UnbindAttributes( shader );

            this._cachedLocations.Clear();

            for ( var i = 0; i < numAttributes; i++ )
            {
                VertexAttribute attribute = Attributes.Get( i );

                this._cachedLocations.Add( locations == null
                                               ? shader.GetAttributeLocation( attribute.alias )
                                               : locations[ i ] );

                var location = this._cachedLocations[ i ];

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
    }

    private void UnbindAttributes( ShaderProgram shaderProgram )
    {
        if ( _cachedLocations.Count == 0 )
        {
            return;
        }

        var numAttributes = Attributes.Size;

        for ( var i = 0; i < numAttributes; i++ )
        {
            var location = _cachedLocations[ i ];

            if ( location < 0 )
            {
                continue;
            }

            shaderProgram.DisableVertexAttribute( location );
        }
    }

    private void BindData( IGL20? gl )
    {
        if ( _isDirty )
        {
            gl?.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, _bufferHandle );

            _byteBuffer.Limit = _buffer.Limit * 4;

            gl?.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Limit, _byteBuffer, _usage );

            _isDirty = false;
        }
    }

    private void CreateVAO()
    {
        TmpHandle.Clear();

        Gdx.GL30?.GLGenVertexArrays( 1, TmpHandle );

        _vaoHandle = TmpHandle.Get();
    }

    private void DeleteVAO()
    {
        if ( _vaoHandle != -1 )
        {
            TmpHandle.Clear();
            TmpHandle.Put( _vaoHandle );

            TmpHandle.Flip();

            Gdx.GL30?.GLDeleteVertexArrays( 1, TmpHandle );

            _vaoHandle = -1;
        }
    }
}
