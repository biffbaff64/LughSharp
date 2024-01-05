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

using Buffer = LibGDXSharp.Files.Buffers.Buffer;

namespace LibGDXSharp.Graphics.GLUtils;

public class InstanceBufferObject : IInstanceData
{

    private FloatBuffer _buffer = null!;
    private int         _bufferHandle;
    private ByteBuffer? _byteBuffer;
    private bool        _isBound = false;
    private bool        _isDirty = false;
    private bool        _ownsBuffer;
    private int         _usage;

    public InstanceBufferObject( bool isStatic, int numVertices, params VertexAttribute[] attributes )
        : this( isStatic, numVertices, new VertexAttributes( attributes ) )
    {
    }

    public InstanceBufferObject( bool isStatic, int numVertices, VertexAttributes instanceAttributes )
    {
        if ( Gdx.GL30 == null )
        {
            throw new GdxRuntimeException
                ( "InstanceBufferObject requires a device running with GLES 3.0 compatibilty" );
        }

        _bufferHandle = Gdx.GL20.GLGenBuffer();

        ByteBuffer data = BufferUtils.NewByteBuffer( instanceAttributes.VertexSize * numVertices );

        data.Limit = 0;

        SetBuffer( data, true, instanceAttributes );

        Usage = isStatic ? IGL20.GL_STATIC_DRAW : IGL20.GL_DYNAMIC_DRAW;
    }

    /// <summary>
    ///     The GL enum used in the call to <see cref="IGL20.GLBufferData(int, int, Buffer, int)" />",
    ///     e.g. GL_STATIC_DRAW or GL_DYNAMIC_DRAW. It can only be called when the VBO is not bound.
    /// </summary>
    public int Usage
    {
        get => _usage;
        set
        {
            if ( _isBound )
            {
                throw new GdxRuntimeException( "Cannot change _usage while VBO is bound" );
            }

            _usage = value;
        }
    }

    public int NumInstances    => ( _buffer.Limit * 4 ) / Attributes.VertexSize;
    public int NumMaxInstances => _byteBuffer!.Capacity / Attributes.VertexSize;

    public VertexAttributes Attributes { get; set; } = null!;

    public FloatBuffer GetBuffer( bool forWriting = true )
    {
        _isDirty |= forWriting;

        return _buffer;
    }

    public void SetInstanceData( float[] data, int offset, int count )
    {
        Debug.Assert( _byteBuffer != null, "SetInstanceData(float[], int, int) fail: _byteBuffer is NULL" );

        _isDirty = true;

        BufferUtils.Copy( data, _byteBuffer, count, offset );

        _buffer.Position = 0;
        _buffer.Limit    = count;

        BufferChanged();
    }

    public void SetInstanceData( FloatBuffer data, int count )
    {
        Debug.Assert( _byteBuffer != null, "SetInstanceData(FloatBuffer, int) fail: _byteBuffer is NULL" );

        _isDirty = true;

        BufferUtils.Copy( data, _byteBuffer, count );

        _buffer.Position = 0;
        _buffer.Limit    = count;

        BufferChanged();
    }

    public void UpdateInstanceData( int targetOffset, float[] data, int sourceOffset, int count )
    {
        if ( _byteBuffer == null )
        {
            throw new GdxRuntimeException( "_byteBuffer cannot be null" );
        }

        _isDirty = true;

        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 4;

        BufferUtils.Copy( data, sourceOffset, count, _byteBuffer );

        _byteBuffer.Position = pos;
        _buffer.Position     = 0;

        BufferChanged();
    }

    public void UpdateInstanceData( int targetOffset, FloatBuffer data, int sourceOffset, int count )
    {
        GdxRuntimeException.ThrowIfNull( _byteBuffer );

        _isDirty = true;

        var pos = _byteBuffer.Position;

        _byteBuffer.Position = targetOffset * 4;
        data.Position        = sourceOffset * 4;

        BufferUtils.Copy( data, _byteBuffer, count );

        _byteBuffer.Position = pos;
        _buffer.Position     = 0;

        BufferChanged();
    }

    /// <summary>
    ///     Binds this InstanceBufferObject for rendering via
    ///     GLDrawArraysInstanced or GLDrawElementsInstanced
    /// </summary>
    public void Bind( ShaderProgram shader, int[]? locations = null )
    {
        Debug.Assert( _byteBuffer != null, "Bind(ShaderProgram, int[]) fail: _byteBuffer is NULL" );

        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, _bufferHandle );

        if ( _isDirty )
        {
            _byteBuffer.Limit = _buffer.Limit * 4;

            Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Limit, _byteBuffer, Usage );

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

                shader.SetVertexAttribute( location + unitOffset,
                                           attribute.numComponents,
                                           attribute.type,
                                           attribute.normalized,
                                           Attributes.VertexSize,
                                           attribute.Offset );

                Gdx.GL30?.GLVertexAttribDivisor( location + unitOffset, 1 );
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

                shader.SetVertexAttribute( location + unitOffset,
                                           attribute.numComponents,
                                           attribute.type,
                                           attribute.normalized,
                                           Attributes.VertexSize,
                                           attribute.Offset );

                Gdx.GL30?.GLVertexAttribDivisor( location + unitOffset, 1 );
            }
        }

        _isBound = true;
    }

    /// <summary>
    ///     Unbinds this InstanceBufferObject.
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
                shader.DisableVertexAttribute( location + unitOffset );
            }
        }

        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );
        _isBound = false;
    }

    /// <summary>
    ///     Invalidates the InstanceBufferObject so a new OpenGL _buffer handle
    ///     is created. Use this in case of a context loss.
    /// </summary>
    public void Invalidate()
    {
        _bufferHandle = Gdx.GL20.GLGenBuffer();
        _isDirty      = true;
    }

    /// <summary>
    ///     Disposes of all resources this InstanceBufferObject uses.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL20.GLBindBuffer( IGL20.GL_ARRAY_BUFFER, 0 );
        Gdx.GL20.GLDeleteBuffers( _bufferHandle );

        _bufferHandle = 0;

        if ( _ownsBuffer && ( _byteBuffer != null ) )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _byteBuffer );
        }
    }

    /// <summary>
    ///     Low level method to reset the _buffer and _attributes to
    ///     the specified values. Use with care!
    /// </summary>
    protected void SetBuffer( Buffer data, bool ownsBuffer, VertexAttributes value )
    {
        if ( _isBound )
        {
            throw new GdxRuntimeException( "Cannot change _attributes while VBO is bound" );
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
        _buffer           = _byteBuffer.AsFloatBuffer();
        _byteBuffer.Limit = lim;
        _buffer.Limit     = lim / 4;
    }

    private void BufferChanged()
    {
        if ( _byteBuffer == null )
        {
            throw new GdxRuntimeException( "NULL _byteBuffer not allowed" );
        }

        if ( _isBound )
        {
            Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Limit, null!, Usage );
            Gdx.GL20.GLBufferData( IGL20.GL_ARRAY_BUFFER, _byteBuffer.Limit, _byteBuffer, Usage );
            _isDirty = false;
        }
    }
}
