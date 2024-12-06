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

using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Buffers;
using Corelib.Lugh.Utils.Exceptions;

using Buffer = Corelib.Lugh.Utils.Buffers.Buffer;

namespace Corelib.Lugh.Graphics.GLUtils;

/// <summary>
/// A <see cref="IVertexData"/> implementation based on OpenGL vertex buffer objects.
/// If the OpenGL context was lost you can call <see cref="Invalidate()"/> to recreate
/// a new OpenGL vertex buffer object.
/// <para>
/// The data is bound via <tt>GLBindings.glVertexAttribPointer</tt> according to the
/// attribute aliases specified via <see cref="VertexAttributes"/> in the constructor.
/// </para>
/// <para>
/// VertexBufferObjects must be disposed via the <see cref="Dispose()"/> method when
/// no longer needed
/// </para>
/// </summary>
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

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttribute"/>s.  </param>
    public VertexBufferObject( bool isStatic, int numVertices, params VertexAttribute[] attributes )
        : this( isStatic, numVertices, new VertexAttributes( attributes ) )
    {
    }

    /// <summary>
    /// Constructs a new interleaved VertexBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numVertices"> the maximum number of vertices </param>
    /// <param name="attributes"> the <see cref="VertexAttributes"/>.  </param>
    public VertexBufferObject( bool isStatic, int numVertices, VertexAttributes attributes )
    {
        _buffer    = default( FloatBuffer? )!;
        Attributes = default( VertexAttributes )!;

        _bufferHandle = ( int ) Gdx.GL.glGenBuffer();

        var data = BufferUtils.NewByteBuffer( attributes.VertexSize * numVertices, false );

        data.Limit = 0;

        SetBuffer( data, true, attributes );
        Usage = isStatic ? IGL.GL_STATIC_DRAW : IGL.GL_DYNAMIC_DRAW;
    }

    /// <summary>
    /// Constructs a new interleaved VertexBufferObject with the specified usage, data,
    /// ownership flag, and vertex attributes. It generates a buffer handle using OpenGL,
    /// sets the buffer data and attributes, and assigns the usage parameter.
    /// </summary>
    /// <param name="usage">
    /// Specifies the expected usage pattern of the data store (e.g., GL_STATIC_DRAW, GL_DYNAMIC_DRAW).
    /// </param>
    /// <param name="data">The byte buffer containing the vertex data.</param>
    /// <param name="ownsBuffer">Indicates whether this object should take ownership of the buffer.</param>
    /// <param name="attributes">The vertex attributes that define the structure of the vertex data.</param>
    public VertexBufferObject( int usage, ByteBuffer data, bool ownsBuffer, VertexAttributes attributes )
    {
        // Initialize the _buffer and Attributes fields to default values.
        _buffer    = default( FloatBuffer? )!;
        Attributes = default( VertexAttributes )!;

        // Generate a new buffer handle using OpenGL and assign it to _bufferHandle.
        _bufferHandle = ( int ) Gdx.GL.glGenBuffer();

        // Set the buffer data, ownership flag, and attributes using the provided parameters.
        SetBuffer( data, ownsBuffer, attributes );

        // Assign the usage parameter to the Usage property.
        Usage = usage;
    }

    /// <summary>
    /// The usage pattern for the vertex buffer object, which hints to the GPU how the
    /// data will be used (e.g., GL_STATIC_DRAW for data that doesn't change often).
    /// </summary>
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
    /// Returns the number of vertices this VertexData stores.
    /// </summary>
    public int NumVertices { get; set; }

    /// <summary>
    /// Returns the number of vertices this VertedData can store.
    /// </summary>
    public int NumMaxVertices { get; set; }

    /// <summary>
    /// Returns the <see cref="VertexAttributes"/> as specified during construction.
    /// </summary>
    public VertexAttributes Attributes { get; set; }

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="SetVertices"/>; Any modifications made to the Buffer
    /// after* the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.  </returns>
    public FloatBuffer GetBuffer( bool forWriting )
    {
        _isDirty |= forWriting;

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
    /// Update (a portion of) the vertices. Does not resize the backing buffer.
    /// </summary>
    /// <param name="targetOffset">The offset in the target buffer where the update begins.</param>
    /// <param name="vertices">The vertex data to be copied.</param>
    /// <param name="sourceOffset">The offset in the source array where copying starts.</param>
    /// <param name="count">The number of floats to copy.</param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count )
    {
        // Check if the byte buffer is null and log an error if it is.
        if ( _byteBuffer == null )
        {
            Logger.Error( "_byteBuffer is NULL!" );

            return;
        }

        // Mark the buffer as dirty, indicating it has been modified.
        _isDirty = true;

        // Save the current position of the byte buffer.
        var pos = _byteBuffer.Position;

        // Set the position of the byte buffer to the target offset, converted to bytes.
        _byteBuffer.Position = targetOffset * 4;

        // Copy the vertex data from the source array to the byte buffer.
        BufferUtils.Copy( vertices, sourceOffset, count, _byteBuffer );

        // Restore the byte buffer's position to its original value.
        _byteBuffer.Position = pos;

        // Reset the main buffer's position to the beginning.
        _buffer.Position = 0;

        // Signal that the buffer has changed.
        BufferChanged();
    }

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"> The <see cref="ShaderProgram"/> to use. </param>
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
                               ? shader.GetAttributeLocation( attribute.Alias )
                               : locations[ i ];

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

        _isBound = true;
    }

    /// <summary>
    /// Unbinds the vertex buffer object from the shader, disabling the vertex attributes.
    /// </summary>
    /// <param name="shader">The shader program currently in use.</param>
    /// <param name="locations">
    /// An optional array of attribute locations to be disabled. If null, attribute
    /// aliases from the vertex attributes are used.
    /// </param>
    public void Unbind( ShaderProgram shader, int[]? locations = null )
    {
        // Get the number of attributes in the vertex attributes.
        var numAttributes = Attributes.Size;

        // If no specific locations are provided, disable attributes using their aliases.
        if ( locations == null )
        {
            for ( var i = 0; i < numAttributes; i++ )
            {
                // Disable the vertex attribute for the alias of each attribute.
                shader.DisableVertexAttribute( Attributes.Get( i ).Alias );
            }
        }
        else
        {
            // If specific locations are provided, disable attributes based on the locations array.
            for ( var i = 0; i < numAttributes; i++ )
            {
                var location = locations[ i ];

                // Disable the vertex attribute at the given location if it is valid (>= 0).
                if ( location >= 0 )
                {
                    shader.DisableVertexAttribute( location );
                }
            }
        }

        // Unbind the buffer from the GL_ARRAY_BUFFER target.
        Gdx.GL.glBindBuffer( IGL.GL_ARRAY_BUFFER, 0 );

        // Mark the buffer as unbound.
        _isBound = false;
    }

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = ( int ) Gdx.GL.glGenBuffer();
        _isDirty      = true;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
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
    /// Low level method to reset the buffer and attributes to the specified values.
    /// Use with care!
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

    /// <summary>
    /// Handles any additional logic required when the buffer is updated.
    /// </summary>
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
