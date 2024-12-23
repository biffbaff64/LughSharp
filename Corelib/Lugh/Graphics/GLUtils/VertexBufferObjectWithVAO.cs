// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Buffers;

namespace Corelib.Lugh.Graphics.GLUtils;

/// <summary>
/// 
/// </summary>
[PublicAPI]
public class VertexBufferObjectWithVAO : IVertexData
{
    public VertexAttributes Attributes { get; set; }

    private static readonly IntBuffer _tmpHandle = BufferUtils.NewIntBuffer( 1 );

    private readonly FloatBuffer _buffer;

    private readonly ByteBuffer  _byteBuffer;
    private readonly List< int > _cachedLocations = new();
    private readonly bool        _ownsBuffer;
    private readonly int         _usage;

    private int  _bufferHandle;
    private bool _isBound = false;
    private bool _isDirty = false;
    private bool _isStatic;
    private int  _vaoHandle = -1;

    // ========================================================================
    // ========================================================================

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
        _isStatic  = isStatic;
        Attributes = attributes;

        _byteBuffer = BufferUtils.NewByteBuffer( Attributes.VertexSize * numVertices, false );
        _buffer     = _byteBuffer.AsFloatBuffer();
        _ownsBuffer = true;

        _buffer.Flip();
        _byteBuffer.Flip();

        _bufferHandle = ( int )GdxApi.Bindings.GenBuffer();
        _usage        = isStatic ? IGL.GL_STATIC_DRAW : IGL.GL_DYNAMIC_DRAW;

        CreateVAO();
    }

    public VertexBufferObjectWithVAO( bool isStatic, ByteBuffer unmanagedBuffer, VertexAttributes attributes )
    {
        _isStatic  = isStatic;
        Attributes = attributes;

        _byteBuffer = unmanagedBuffer;
        _ownsBuffer = false;
        _buffer     = _byteBuffer.AsFloatBuffer();

        _buffer.Flip();
        _byteBuffer.Flip();

        _bufferHandle = ( int )GdxApi.Bindings.GenBuffer();

        _usage = isStatic ? IGL.GL_STATIC_DRAW : IGL.GL_DYNAMIC_DRAW;

        CreateVAO();
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
    /// Update (a portion of) the vertices. Does not resize the backing buffer.
    /// </summary>
    /// <param name="targetOffset"></param>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count )
    {
        _isDirty = true;
        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 4;

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
        GdxApi.Bindings.BindVertexArray( ( uint )_vaoHandle );

        BindAttributes( shader, locations );

        // if our data has changed upload it:
        BindData();

        _isBound = true;
    }

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Unbind( ShaderProgram? shader, int[]? locations = null )
    {
        GdxApi.Bindings.BindVertexArray( 0 );
        _isBound = false;
    }

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = ( int )GdxApi.Bindings.GenBuffer();

        CreateVAO();

        _isDirty = true;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        GdxApi.Bindings.BindBuffer( IGL.GL_ARRAY_BUFFER, 0 );
        GdxApi.Bindings.DeleteBuffers( ( uint )_bufferHandle );

        _bufferHandle = 0;

        if ( _ownsBuffer )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
        }

        DeleteVAO();
    }

    private unsafe void BufferChanged()
    {
        if ( _isBound )
        {
            GdxApi.Bindings.BindBuffer( IGL.GL_ARRAY_BUFFER, ( uint )_bufferHandle );

            fixed ( void* ptr = &_byteBuffer.BackingArray()[ 0 ] )
            {
                GdxApi.Bindings.BufferData( IGL.GL_ARRAY_BUFFER, _byteBuffer.Limit, ptr, _usage );
            }

            _isDirty = false;
        }
    }

    private void BindAttributes( ShaderProgram shader, int[]? locations )
    {
        var stillValid    = _cachedLocations.Count != 0;
        var numAttributes = Attributes.Size;

        if ( stillValid )
        {
            if ( locations == null )
            {
                for ( var i = 0; stillValid && ( i < numAttributes ); i++ )
                {
                    var attribute = Attributes.Get( i );

                    var location = shader.GetAttributeLocation( attribute.Alias );

                    stillValid = location == _cachedLocations[ i ];
                }
            }
            else
            {
                stillValid = locations.Length == _cachedLocations.Count;

                for ( var i = 0; stillValid && ( i < numAttributes ); i++ )
                {
                    stillValid = locations[ i ] == _cachedLocations[ i ];
                }
            }
        }

        if ( !stillValid )
        {
            GdxApi.Bindings.BindBuffer( IGL.GL_ARRAY_BUFFER, ( uint )_bufferHandle );

            UnbindAttributes( shader );

            _cachedLocations.Clear();

            for ( var i = 0; i < numAttributes; i++ )
            {
                var attribute = Attributes.Get( i );

                _cachedLocations.Add( locations == null
                                          ? shader.GetAttributeLocation( attribute.Alias )
                                          : locations[ i ] );

                var location = _cachedLocations[ i ];

                if ( location < 0 )
                {
                    continue;
                }

                shader.EnableVertexAttribute( location );

                shader.SetVertexAttribute( location,
                                           attribute.NumComponents,
                                           attribute.Type,
                                           attribute.Normalized,
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

    private unsafe void BindData()
    {
        if ( _isDirty )
        {
            GdxApi.Bindings.BindBuffer( IGL.GL_ARRAY_BUFFER, ( uint )_bufferHandle );

            _byteBuffer.Limit = _buffer.Limit * 4;

            fixed ( void* ptr = &_byteBuffer.BackingArray()[ 0 ] )
            {
                GdxApi.Bindings.BufferData( IGL.GL_ARRAY_BUFFER, _byteBuffer.Limit, ptr, _usage );
            }

            _isDirty = false;
        }
    }

    private unsafe void CreateVAO()
    {
        _tmpHandle.Clear();

        fixed ( int* intptr = &_tmpHandle.BackingArray()[ 0 ] )
        {
            GdxApi.Bindings.GenVertexArrays( 1, ( uint* )intptr );
        }

        _vaoHandle = _tmpHandle.Get();
    }

    private unsafe void DeleteVAO()
    {
        if ( _vaoHandle != -1 )
        {
            _tmpHandle.Clear();
            _tmpHandle.Put( _vaoHandle );
            _tmpHandle.Flip();

            fixed ( int* intptr = &_tmpHandle.BackingArray()[ 0 ] )
            {
                GdxApi.Bindings.DeleteVertexArrays( 1, ( uint* )intptr );
            }

            _vaoHandle = -1;
        }
    }
}