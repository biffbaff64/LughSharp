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

/// <summary>
/// Modification of the <see cref="VertexBufferObjectSubData"/> class.
/// Sets the glVertexAttribDivisor for every <see cref="VertexAttribute"/>
/// automatically.
/// </summary>
[PublicAPI]
public class InstanceBufferObjectSubData : IInstanceData
{
    public VertexAttributes Attributes { get; set; }
    
    private FloatBuffer      _buffer;
    private ByteBuffer       _byteBuffer;

    private int  _bufferHandle;
    private int  _usage;
    private bool _isDirect;
    private bool _isStatic;
    private bool _isDirty = false;
    private bool _isBound = false;

    /**
     * Constructs a new interleaved InstanceBufferObject.
     *
     * @param isStatic           whether the vertex data is static.
     * @param numInstances       the maximum number of vertices
     * @param instanceAttributes the {@link VertexAttributes}.
     */
    public InstanceBufferObjectSubData( bool isStatic, int numInstances, params VertexAttribute[] instanceAttributes )
        : this( isStatic, numInstances, new VertexAttributes( instanceAttributes ) )
    {
    }

    /**
     * Constructs a new interleaved InstanceBufferObject.
     *
     * @param isStatic           whether the vertex data is static.
     * @param numInstances       the maximum number of vertices
     * @param instanceAttributes the {@link VertexAttribute}s.
     */
    public InstanceBufferObjectSubData( bool isStatic, int numInstances, VertexAttributes instanceAttributes )
    {
        this._isStatic   = isStatic;
        this.Attributes = instanceAttributes;
        _byteBuffer      = BufferUtils.NewByteBuffer( this.Attributes.VertexSize * numInstances );
        _isDirect        = true;

        _usage        = isStatic ? IGL20.GL_STATIC_DRAW : IGL20.GL_DYNAMIC_DRAW;
        _buffer       = _byteBuffer.AsFloatBuffer();
        _bufferHandle = CreateBufferObject();
        
        _buffer.Flip();
        _byteBuffer.Flip();
    }

    private int CreateBufferObject()
    {
        int result = Gdx.GL20.GLGenBuffer();
        
        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, result );
        Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Capacity, null!, _usage );
        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );

        return result;
    }

    /**
     * Effectively returns {@link #getNumInstances()}.
     *
     * @return number of instances in this buffer
     */
    public int NumInstances => ( _buffer.Limit * 4 ) / Attributes.VertexSize;

    /**
     * Effectively returns {@link #getNumMaxInstances()}.
     *
     * @return maximum number of instances in this buffer
     */
    public int NumMaxInstances => _byteBuffer.Capacity / Attributes.VertexSize;

    public FloatBuffer GetBuffer( bool forWriting )
    {
        _isDirty |= forWriting;

        return _buffer;
    }

    private void BufferChanged()
    {
        if ( _isBound )
        {
            Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Limit, null!, _usage );
            Gdx.GL20.GLBufferSubData( IGL20.GL_ARRAY_BUFFER, 0, _byteBuffer.Limit, _byteBuffer );
            _isDirty = false;
        }
    }

    public void SetInstanceData( float[] data, int offset, int count )
    {
        _isDirty = true;

        if ( _isDirect )
        {
            BufferUtils.Copy( data, _byteBuffer, count, offset );

            _buffer.Position = 0;
            _buffer.Limit = count;
        }
        else
        {
            _buffer.Clear();
            
            _buffer.Put( data, offset, count );
            
            _buffer.Flip();
            _byteBuffer.Position = 0;
            _byteBuffer.Limit = ( _buffer.Limit << 2 );
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
            _buffer.Limit = count;
        }
        else
        {
            _buffer.Clear();
            
            _buffer.Put( data );
            
            _buffer.Flip();
            _byteBuffer.Position = 0;
            _byteBuffer.Limit = ( _buffer.Limit << 2 );
        }

        BufferChanged();
    }

    public void UpdateInstanceData( int targetOffset, float[] data, int sourceOffset, int count )
    {
        _isDirty = true;

        if ( _isDirect )
        {
            var pos = _byteBuffer.Position;

            _byteBuffer.Position = ( targetOffset * 4 );

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

            _byteBuffer.Position = ( targetOffset * 4 );
            data.Position        = ( sourceOffset * 4 );

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
        IGL20 gl = Gdx.GL20;

        gl.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, _bufferHandle );

        if ( _isDirty )
        {
            _byteBuffer.Limit = ( _buffer.Limit * 4 );
            gl.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Limit, _byteBuffer, _usage );
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

                var unitOffset = +attribute.unit;
                shader.EnableVertexAttribute( location + unitOffset );

                shader.SetVertexAttribute
                    (
                     location + unitOffset,
                     attribute.numComponents,
                     attribute.type,
                     attribute.normalized,
                     Attributes.VertexSize,
                     attribute.Offset
                    );

                Gdx.GL30.GLVertexAttribDivisor( location + unitOffset, 1 );
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

                var unitOffset = +attribute.unit;
                shader.EnableVertexAttribute( location + unitOffset );

                shader.SetVertexAttribute
                    (
                     location + unitOffset,
                     attribute.numComponents,
                     attribute.type,
                     attribute.normalized,
                     Attributes.VertexSize,
                     attribute.Offset
                    );

                Gdx.GL30.GLVertexAttribDivisor( location + unitOffset, 1 );
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
                VertexAttribute attribute = Attributes.Get( i );
                var             location  = shader.GetAttributeLocation( attribute.alias );

                if ( location < 0 )
                {
                    continue;
                }

                var unitOffset = +attribute.unit;

                shader.DisableVertexAttribute( location + unitOffset );
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

                var unitOffset = +attribute.unit;

                shader.EnableVertexAttribute( location + unitOffset );
            }
        }

        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );
        _isBound = false;
    }

    /// <summary>
    /// Invalidates the InstanceBufferObject so a new OpenGL buffer handle is
    /// created. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = CreateBufferObject();
        _isDirty      = true;
    }

    /// <summary>
    /// Disposes of all resources this InstanceBufferObject uses.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );
        Gdx.GL20.GLDeleteBuffer( _bufferHandle );
        _bufferHandle = 0;
    }

    public int GetBufferHandle()
    {
        return _bufferHandle;
    }
}
