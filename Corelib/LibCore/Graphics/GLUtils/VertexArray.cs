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

using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Buffers;

namespace Corelib.LibCore.Graphics.GLUtils;

[PublicAPI]
public class VertexArray : IVertexData
{
    private readonly FloatBuffer _buffer;
    private readonly ByteBuffer  _byteBuffer;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Constructs a new interleaved VertexArray
    /// </summary>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttribute"/>s  </param>
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
        Attributes  = attributes;
        _byteBuffer = BufferUtils.NewByteBuffer( Attributes.VertexSize * numVertices );
        _buffer     = _byteBuffer.AsFloatBuffer();

        _buffer.Flip();
        _byteBuffer.Flip();
    }

    /// <summary>
    /// Returns the number of vertices this VertexData stores.
    /// </summary>
    public int NumVertices => ( _buffer.Limit * 4 ) / Attributes.VertexSize;

    /// <summary>
    /// Returns the number of vertices this VertedData can store.
    /// </summary>
    public int NumMaxVertices => _byteBuffer.Capacity / Attributes.VertexSize;

    /// <summary>
    /// Returns the <see cref="VertexAttributes"/> as specified during construction.
    /// </summary>
    public VertexAttributes Attributes { get; set; }

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="IVertexData.SetVertices"/>; Any modifications made to the Buffer
    /// after* the call to bind will not automatically be uploaded.
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
    /// Update (a portion of) the vertices. Does not resize the backing buffer.
    /// </summary>
    /// <param name="targetOffset"></param>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count )
    {
        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 4;

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

        _byteBuffer.Limit = _buffer.Limit * 4;

        if ( locations == null )
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                var attribute = Attributes.Get( i );
                var location  = shader.GetAttributeLocation( attribute.Alias );

                if ( location < 0 )
                {
                    continue;
                }

                shader.EnableVertexAttribute( location );

                if ( attribute.Type == IGL.GL_FLOAT )
                {
                    _buffer.Position = attribute.Offset / 4;

                    shader.SetVertexAttribute( location,
                                               attribute.NumComponents,
                                               attribute.Type,
                                               attribute.Normalized,
                                               Attributes.VertexSize,
                                               _buffer );
                }
                else
                {
                    _byteBuffer.Position = attribute.Offset;

                    shader.SetVertexAttribute( location,
                                               attribute.NumComponents,
                                               attribute.Type,
                                               attribute.Normalized,
                                               Attributes.VertexSize,
                                               _byteBuffer );
                }
            }
        }
        else
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                var attribute = Attributes.Get( i );
                var location  = locations[ i ];

                if ( location < 0 )
                {
                    continue;
                }

                shader.EnableVertexAttribute( location );

                if ( attribute.Type == IGL.GL_FLOAT )
                {
                    _buffer.Position = attribute.Offset / 4;

                    shader.SetVertexAttribute( location,
                                               attribute.NumComponents,
                                               attribute.Type,
                                               attribute.Normalized,
                                               Attributes.VertexSize,
                                               _buffer );
                }
                else
                {
                    _byteBuffer.Position = attribute.Offset;

                    shader.SetVertexAttribute( location,
                                               attribute.NumComponents,
                                               attribute.Type,
                                               attribute.Normalized,
                                               Attributes.VertexSize,
                                               _byteBuffer );
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
                shader?.DisableVertexAttribute( Attributes.Get( i ).Alias );
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
