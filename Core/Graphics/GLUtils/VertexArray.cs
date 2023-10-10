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

[PublicAPI]
public class VertexArray : IVertexData
{
    private readonly FloatBuffer _buffer;
    private readonly ByteBuffer  _byteBuffer;

    /// <summary>
    /// Constructs a new interleaved VertexArray
    /// </summary>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <seealso cref="VertexAttribute"/>s  </param>
    public VertexArray( int numVertices, params VertexAttribute[] attributes )
        : this( numVertices, new VertexAttributes( attributes ) )
    {
    }

    /// <summary>
    /// Constructs a new interleaved VertexArray
    /// </summary>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttributes"/> </param>
    public VertexArray( int numVertices, VertexAttributes attributes )
    {
        this.Attributes  = attributes;
        this._byteBuffer = BufferUtils.NewUnsafeByteBuffer( this.Attributes.VertexSize * numVertices );
        this._buffer     = _byteBuffer.AsFloatBuffer();

        _buffer.Flip();
        _byteBuffer.Flip();
    }

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertexData stores </returns>
    public int NumVertices => ( this._buffer.Limit * 4 ) / Attributes.VertexSize;

    /// <summary>
    /// </summary>
    /// <returns> the number of vertices this VertedData can store </returns>
    public int NumMaxVertices => _byteBuffer.Capacity / Attributes.VertexSize;

    /// <summary>
    /// </summary>
    /// <returns> the <see cref="VertexAttributes"/> as specified during construction. </returns>
    public VertexAttributes Attributes { get; set; }

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="IVertexData.SetVertices"/>; Any modifications made to the Buffer
    /// *after* the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.</returns>
    public FloatBuffer GetBuffer( bool forWriting )
    {
        return _buffer;
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
        BufferUtils.Copy( vertices, _byteBuffer, count, offset );

        _buffer.Position = 0;
        _buffer.Limit    = count;
    }

    /// <summary>
    /// Update (a portion of) the vertices. Does not resize the backing buffer. </summary>
    /// <param name="targetOffset"></param>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count )
    {
        var pos = _byteBuffer.Position;

        _byteBuffer.Position = ( targetOffset * 4 );

        BufferUtils.Copy( vertices, sourceOffset, count, _byteBuffer );

        _byteBuffer.Position = pos;
    }

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.</param>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        var numAttributes = Attributes.Size;

        _byteBuffer.Limit = ( _buffer.Limit * 4 );

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

                if ( attribute.type == IGL20.GL_FLOAT )
                {
                    _buffer.Position = ( attribute.Offset / 4 );

                    shader.SetVertexAttribute
                        (
                         location, attribute.numComponents, attribute.type, attribute.normalized,
                         Attributes.VertexSize, _buffer
                        );
                }
                else
                {
                    _byteBuffer.Position = attribute.Offset;

                    shader.SetVertexAttribute
                        (
                         location, attribute.numComponents, attribute.type, attribute.normalized,
                         Attributes.VertexSize, _byteBuffer
                        );
                }
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

                if ( attribute.type == IGL20.GL_FLOAT )
                {
                    _buffer.Position = ( attribute.Offset / 4 );

                    shader.SetVertexAttribute
                        (
                         location, attribute.numComponents, attribute.type, attribute.normalized,
                         Attributes.VertexSize, _buffer
                        );
                }
                else
                {
                    _byteBuffer.Position = attribute.Offset;

                    shader.SetVertexAttribute
                        (
                         location, attribute.numComponents, attribute.type, attribute.normalized,
                         Attributes.VertexSize, _byteBuffer
                        );
                }
            }
        }
    }

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.</param>
    public void Unbind( ShaderProgram? shader, int[]? locations = null )
    {
        var numAttributes = Attributes.Size;

        if ( locations == null )
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                shader?.DisableVertexAttribute( Attributes.Get( i ).alias );
            }
        }
        else
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                var location = locations[ i ];

                if ( location >= 0 )
                {
                    shader?.DisableVertexAttribute( location );
                }
            }
        }
    }

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
    }
}
