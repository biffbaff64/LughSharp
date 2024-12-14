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
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.GLUtils;

/// <summary>
/// Modification of the <see cref="VertexBufferObjectSubData"/> class.
/// Sets the glVertexAttribDivisor for every <see cref="VertexAttribute"/>
/// automatically.
/// </summary>
[PublicAPI]
public class InstanceBufferObjectSubData : IInstanceData
{
    public int              BufferHandle { get; set; }
    public VertexAttributes Attributes   { get; set; }

    private readonly FloatBuffer _buffer;
    private readonly ByteBuffer  _byteBuffer;
    private readonly bool        _isDirect;

    private readonly int  _usage;
    private          bool _isBound = false;
    private          bool _isDirty = false;
    private          bool _isStatic;

    // ========================================================================
    
    /// <summary>
    /// Constructs a new interleaved InstanceBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numInstances"> the maximum number of vertices. </param>
    /// <param name="instanceAttributes"> the <see cref="VertexAttributes"/>". </param>
    public InstanceBufferObjectSubData( bool isStatic,
                                        int numInstances,
                                        params VertexAttribute[] instanceAttributes )
        : this( isStatic, numInstances, new VertexAttributes( instanceAttributes ) )
    {
    }

    /// <summary>
    /// Constructs a new interleaved InstanceBufferObject.
    /// </summary>
    /// <param name="isStatic"> whether the vertex data is static. </param>
    /// <param name="numInstances"> the maximum number of vertices. </param>
    /// <param name="instanceAttributes"> the <see cref="VertexAttributes"/>". </param>
    public InstanceBufferObjectSubData( bool isStatic, int numInstances, VertexAttributes instanceAttributes )
    {
        _isStatic   = isStatic;
        Attributes  = instanceAttributes;
        _byteBuffer = BufferUtils.NewByteBuffer( Attributes.VertexSize * numInstances, false );
        _isDirect   = true;

        _usage       = isStatic ? IGL.GL_STATIC_DRAW : IGL.GL_DYNAMIC_DRAW;
        _buffer      = _byteBuffer.AsFloatBuffer();
        BufferHandle = CreateBufferObject();

        _buffer.Flip();
        _byteBuffer.Flip();
    }

    /// <summary>
    /// Returns the number of instances in this buffer.
    /// </summary>
    public int NumInstances => ( _buffer.Limit * 4 ) / Attributes.VertexSize;

    /// <summary>
    /// Returns the max number of instances in this buffer.
    /// </summary>
    public int NumMaxInstances => _byteBuffer.Capacity / Attributes.VertexSize;

    public FloatBuffer GetBuffer( bool forWriting )
    {
        _isDirty |= forWriting;

        return _buffer;
    }

    public void SetInstanceData( float[] data, int offset, int count )
    {
        _isDirty = true;

        if ( _isDirect )
        {
            BufferUtils.Copy( data, _byteBuffer, count, offset );

            _buffer.Position = 0;
            _buffer.Limit    = count;
        }
        else
        {
            _buffer.Clear();

            _buffer.Put( data, offset, count );

            _buffer.Flip();
            _byteBuffer.Position = 0;
            _byteBuffer.Limit    = _buffer.Limit << 2;
        }

        BufferChanged();
    }

    public void SetInstanceData( FloatBuffer data, int count )
    {
        _isDirty = true;

        if ( _isDirect )
        {
            BufferUtils.Copy( data, _byteBuffer, count );

            _buffer.Position = 0;
            _buffer.Limit    = count;
        }
        else
        {
            _buffer.Clear();

            _buffer.Put( data );

            _buffer.Flip();
            _byteBuffer.Position = 0;
            _byteBuffer.Limit    = _buffer.Limit << 2;
        }

        BufferChanged();
    }

    public void UpdateInstanceData( int targetOffset, float[] data, int sourceOffset, int count )
    {
        _isDirty = true;

        if ( _isDirect )
        {
            var pos = _byteBuffer.Position;

            _byteBuffer.Position = targetOffset * 4;

            BufferUtils.Copy( data, sourceOffset, count, _byteBuffer );

            _byteBuffer.Position = pos;
        }
        else
        {
            throw new GdxRuntimeException( "Buffer must be allocated direct." ); // Should never happen
        }

        BufferChanged();
    }

    public void UpdateInstanceData( int targetOffset, FloatBuffer data, int sourceOffset, int count )
    {
        _isDirty = true;

        if ( _isDirect )
        {
            var pos = _byteBuffer.Position;

            _byteBuffer.Position = targetOffset * 4;
            data.Position        = sourceOffset * 4;

            BufferUtils.Copy( data, _byteBuffer, count );

            _byteBuffer.Position = pos;
        }
        else
        {
            throw new GdxRuntimeException( "Buffer must be allocated direct." ); // Should never happen
        }

        BufferChanged();
    }

    /// <summary>
    /// Binds this InstanceBufferObject for rendering via glDrawArraysInstanced
    /// or glDrawElementsInstanced.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"></param>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        Gdx.GL.BindBuffer( IGL.GL_ARRAY_BUFFER, ( uint ) BufferHandle );

        if ( _isDirty )
        {
            unsafe
            {
                fixed ( void* ptr = &_byteBuffer.BackingArray()[ 0 ] )
                {
                    _byteBuffer.Limit = _buffer.Limit * 4;
                    Gdx.GL.BufferData( IGL.GL_ARRAY_BUFFER, _byteBuffer.Limit, ptr, _usage );
                    _isDirty = false;
                }
            }
        }

        var numAttributes = Attributes.Size;

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

                var unitOffset = +attribute.Unit;
                shader.EnableVertexAttribute( location + unitOffset );

                shader.SetVertexAttribute( location + unitOffset,
                                           attribute.NumComponents,
                                           attribute.Type,
                                           attribute.Normalized,
                                           Attributes.VertexSize,
                                           attribute.Offset );

                Gdx.GL.VertexAttribDivisor( ( uint ) ( location + unitOffset ), 1 );
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

                var unitOffset = +attribute.Unit;
                shader.EnableVertexAttribute( location + unitOffset );

                shader.SetVertexAttribute( location + unitOffset,
                                           attribute.NumComponents,
                                           attribute.Type,
                                           attribute.Normalized,
                                           Attributes.VertexSize,
                                           attribute.Offset );

                Gdx.GL.VertexAttribDivisor( ( uint ) ( location + unitOffset ), 1 );
            }
        }

        _isBound = true;
    }

    /// <summary>
    /// Unbinds this InstanceBufferObject.
    /// </summary>
    public void Unbind( ShaderProgram shader, int[]? locations = null )
    {
        var numAttributes = Attributes.Size;

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

                var unitOffset = +attribute.Unit;

                shader.DisableVertexAttribute( location + unitOffset );
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

                var unitOffset = +attribute.Unit;

                shader.EnableVertexAttribute( location + unitOffset );
            }
        }

        Gdx.GL.BindBuffer( IGL.GL_ARRAY_BUFFER, 0 );
        _isBound = false;
    }

    /// <summary>
    /// Invalidates the InstanceBufferObject so a new OpenGL buffer handle is
    /// created. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        BufferHandle = CreateBufferObject();
        _isDirty     = true;
    }

    /// <summary>
    /// Disposes of all resources this InstanceBufferObject uses.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL.BindBuffer( IGL.GL_ARRAY_BUFFER, 0 );
        Gdx.GL.DeleteBuffers( ( uint ) BufferHandle );
        BufferHandle = 0;
    }

    private unsafe int CreateBufferObject()
    {
        var result = Gdx.GL.GenBuffer();

        Gdx.GL.BindBuffer( IGL.GL_ARRAY_BUFFER, result );
        Gdx.GL.BufferData( IGL.GL_ARRAY_BUFFER, _byteBuffer.Capacity, null!, _usage );
        Gdx.GL.BindBuffer( IGL.GL_ARRAY_BUFFER, 0 );

        return ( int ) result;
    }

    private unsafe void BufferChanged()
    {
        if ( _isBound )
        {
            fixed ( void* ptr = &_byteBuffer.BackingArray()[ 0 ] )
            {
                Gdx.GL.BufferData( IGL.GL_ARRAY_BUFFER, _byteBuffer.Limit, null!, _usage );
                Gdx.GL.BufferSubData( IGL.GL_ARRAY_BUFFER, 0, _byteBuffer.Limit, ptr );
                _isDirty = false;
            }
        }
    }
}
