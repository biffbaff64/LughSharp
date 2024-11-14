// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

#pragma warning disable IDE0079    // Remove unnecessary suppression
#pragma warning disable CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning disable CS8603     // Possible null reference return.
#pragma warning disable IDE0060    // Remove unused parameter.
#pragma warning disable IDE1006    // Naming Styles.
#pragma warning disable IDE0090    // Use 'new(...)'.
#pragma warning disable CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning disable SYSLIB1054 // 

using GLenum = System.Int32;
using GLfloat = System.Single;
using GLint = System.Int32;
using GLsizei = System.Int32;
using GLbitfield = System.UInt32;
using GLdouble = System.Double;
using GLuint = System.UInt32;
using GLboolean = System.Boolean;
using GLubyte = System.Byte;
using GLsizeiptr = System.Int32;
using GLintptr = System.Int32;
using GLshort = System.Int16;
using GLbyte = System.SByte;
using GLushort = System.UInt16;
using GLchar = System.Byte;
using GLuint64 = System.UInt64;
using GLint64 = System.Int64;

namespace Corelib.LibCore.Graphics.OpenGL;

public unsafe partial class GLBindings : IGLBindings
{
// ========================================================================

    /// <inheritdoc/>
    public void glObjectLabel( GLenum identifier, GLuint name, GLsizei length, GLchar* label )
    {
        _glObjectLabel( identifier, name, length, label );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glObjectLabel( GLenum identifier, GLuint name, string label )
    {
        var labelBytes = Encoding.UTF8.GetBytes( label );

        fixed ( GLchar* p_labelBytes = labelBytes )
        {
            _glObjectLabel( identifier, name, labelBytes.Length, p_labelBytes );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetObjectLabel( GLenum identifier, GLuint name, GLsizei bufSize, GLsizei* length, GLchar* label )
    {
        _glGetObjectLabel( identifier, name, bufSize, length, label );
    }

// ========================================================================

    /// <inheritdoc/>
    public string glGetObjectLabel( GLenum identifier, GLuint name, GLsizei bufSize )
    {
        var labelBytes = new GLchar[ bufSize ];
        GLsizei  length;

        fixed ( GLchar* p_labelBytes = labelBytes )
        {
            _glGetObjectLabel( identifier, name, bufSize, &length, p_labelBytes );

            return new string( ( sbyte* )p_labelBytes, 0, length, Encoding.UTF8 );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glObjectPtrLabel( void* ptr, GLsizei length, GLchar* label )
    {
        _glObjectPtrLabel( ptr, length, label );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glObjectPtrLabel( IntPtr ptr, string label )
    {
        var labelBytes = Encoding.UTF8.GetBytes( label );

        fixed ( GLchar* p_labelBytes = labelBytes )
        {
            _glObjectPtrLabel( ptr.ToPointer(), labelBytes.Length, p_labelBytes );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetObjectPtrLabel( void* ptr, GLsizei bufSize, GLsizei* length, GLchar* label )
    {
        _glGetObjectPtrLabel( ptr, bufSize, length, label );
    }

// ========================================================================

    /// <inheritdoc/>
    public string glGetObjectPtrLabel( IntPtr ptr, GLsizei bufSize )
    {
        var labelBytes = new GLchar[ bufSize ];
        GLsizei  length;

        fixed ( GLchar* p_labelBytes = labelBytes )
        {
            _glGetObjectPtrLabel( ptr.ToPointer(), bufSize, &length, p_labelBytes );

            return new string( ( sbyte* )p_labelBytes, 0, length, Encoding.UTF8 );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBufferStorage( GLenum target, GLsizeiptr size, void* data, GLbitfield flags )
    {
        _glBufferStorage( target, size, data, flags );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBufferStorage< T >( GLenum target, T[] data, GLbitfield flags ) where T : unmanaged
    {
        fixed ( void* p_data = &data[ 0 ] )
        {
            _glBufferStorage( target, data.Length * sizeof( T ), p_data, flags );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearTexImage( GLuint texture, GLint level, GLenum format, GLenum type, void* data )
    {
        _glClearTexImage( texture, level, format, type, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearTexImage< T >( GLuint texture, GLint level, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearTexImage( texture, level, format, type, t );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearTexSubImage( GLuint texture, GLint level, GLint xOffset, GLint yOffset, GLint zOffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* data )
    {
        _glClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearTexSubImage< T >( GLuint texture, GLint level, GLint xOffset, GLint yOffset, GLint zOffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, T[] data )
        where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, t );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindBuffersBase( GLenum target, GLuint first, GLsizei count, GLuint* buffers )
    {
        _glBindBuffersBase( target, first, count, buffers );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindBuffersBase( GLenum target, GLuint first, GLuint[] buffers )
    {
        fixed ( GLuint* p_buffers = &buffers[ 0 ] )
        {
            _glBindBuffersBase( target, first, buffers.Length, p_buffers );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindBuffersRange( GLenum target, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizeiptr* sizes )
    {
        _glBindBuffersRange( target, first, count, buffers, offsets, sizes );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindBuffersRange( GLenum target, GLuint first, GLuint[] buffers, GLintptr[] offsets, GLsizeiptr[] sizes )
    {
        fixed ( GLuint* p_buffers = &buffers[ 0 ] )
        {
            fixed ( GLintptr* p_offsets = &offsets[ 0 ] )
            {
                fixed ( GLsizeiptr* p_sizes =
                           &sizes[ 0 ] )
                {
                    _glBindBuffersRange( target, first, buffers.Length, p_buffers, p_offsets, p_sizes );
                }
            }
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindTextures( GLuint first, GLsizei count, GLuint* textures )
    {
        _glBindTextures( first, count, textures );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindTextures( GLuint first, GLuint[] textures )
    {
        fixed ( GLuint* p_textures = &textures[ 0 ] )
        {
            _glBindTextures( first, textures.Length, p_textures );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindSamplers( GLuint first, GLsizei count, GLuint* samplers )
    {
        _glBindSamplers( first, count, samplers );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindSamplers( GLuint first, GLuint[] samplers )
    {
        fixed ( GLuint* p_samplers = &samplers[ 0 ] )
        {
            _glBindSamplers( first, samplers.Length, p_samplers );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindImageTextures( GLuint first, GLsizei count, GLuint* textures )
    {
        _glBindImageTextures( first, count, textures );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindImageTextures( GLuint first, GLuint[] textures )
    {
        fixed ( GLuint* p_textures = &textures[ 0 ] )
        {
            _glBindImageTextures( first, textures.Length, p_textures );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindVertexBuffers( GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides )
    {
        _glBindVertexBuffers( first, count, buffers, offsets, strides );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindVertexBuffers( GLuint first, GLuint[] buffers, GLintptr[] offsets, GLsizei[] strides )
    {
        fixed ( GLuint* p_buffers = &buffers[ 0 ] )
        {
            fixed ( GLintptr* p_offsets = &offsets[ 0 ] )
            {
                fixed ( GLsizei* p_strides = &strides[ 0 ] )
                {
                    _glBindVertexBuffers( first, buffers.Length, p_buffers, p_offsets, p_strides );
                }
            }
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClipControl( GLenum origin, GLenum depth )
    {
        _glClipControl( origin, depth );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glCreateTransformFeedbacks( n, ids );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateTransformFeedbacks()
    {
        return glCreateTransformFeedbacks( 1 )[ 0 ];
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTransformFeedbackBufferBase( GLuint xfb, GLuint index, GLuint buffer )
    {
        _glTransformFeedbackBufferBase( xfb, index, buffer );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTransformFeedbackBufferRange( GLuint xfb, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTransformFeedbackBufferRange( xfb, index, buffer, offset, size );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTransformFeedbackiv( GLuint xfb, GLenum pname, GLint* param )
    {
        _glGetTransformFeedbackiv( xfb, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTransformFeedbackiv( GLuint xfb, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* p_param = &param[ 0 ] )
        {
            _glGetTransformFeedbackiv( xfb, pname, p_param );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTransformFeedbacki_v( GLuint xfb, GLenum pname, GLuint index, GLint* param )
    {
        _glGetTransformFeedbacki_v( xfb, pname, index, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTransformFeedbacki_v( GLuint xfb, GLenum pname, GLuint index, ref GLint[] param )
    {
        fixed ( GLint* p_param = &param[ 0 ] )
        {
            _glGetTransformFeedbacki_v( xfb, pname, index, p_param );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTransformFeedbacki64_v( GLuint xfb, GLenum pname, GLuint index, GLint64* param )
    {
        _glGetTransformFeedbacki64_v( xfb, pname, index, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTransformFeedbacki64_v( GLuint xfb, GLenum pname, GLuint index, ref GLint64[] param )
    {
        fixed ( GLint64* p_param = &param[ 0 ] )
        {
            _glGetTransformFeedbacki64_v( xfb, pname, index, p_param );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateBuffers( GLsizei n, GLuint* buffers )
    {
        _glCreateBuffers( n, buffers );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint[] glCreateBuffers( GLsizei n )
    {
        var buffers = new GLuint[ n ];

        fixed ( GLuint* p_buffers = &buffers[ 0 ] )
        {
            _glCreateBuffers( n, p_buffers );
        }

        return buffers;
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateBuffer()
    {
        return glCreateBuffers( 1 )[ 0 ];
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedBufferStorage( GLuint buffer, GLsizeiptr size, void* data, GLbitfield flags )
    {
        _glNamedBufferStorage( buffer, size, data, flags );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedBufferStorage< T >( GLuint buffer, GLsizeiptr size, T[] data, GLbitfield flags ) where T : unmanaged
    {
        fixed ( T* p_data = &data[ 0 ] )
        {
            _glNamedBufferStorage( buffer, size, p_data, flags );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedBufferData( GLuint buffer, GLsizeiptr size, void* data, GLenum usage )
    {
        _glNamedBufferData( buffer, size, data, usage );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedBufferData< T >( GLuint buffer, GLsizeiptr size, T[] data, GLenum usage ) where T : unmanaged
    {
        fixed ( T* p_data = &data[ 0 ] )
        {
            _glNamedBufferData( buffer, size, p_data, usage );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glNamedBufferSubData( buffer, offset, size, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedBufferSubData< T >( GLuint buffer, GLintptr offset, GLsizeiptr size, T[] data ) where T : unmanaged
    {
        fixed ( T* p_data = &data[ 0 ] )
        {
            _glNamedBufferSubData( buffer, offset, size, p_data );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCopyNamedBufferSubData( GLuint readBuffer, GLuint writeBuffer, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size )
    {
        _glCopyNamedBufferSubData( readBuffer, writeBuffer, readOffset, writeOffset, size );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedBufferData( GLuint buffer, GLenum internalformat, GLenum format, GLenum type, void* data )
    {
        _glClearNamedBufferData( buffer, internalformat, format, type, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedBufferData< T >( GLuint buffer, GLenum internalformat, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearNamedBufferData( buffer, internalformat, format, type, t );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedBufferSubData( GLuint buffer, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data )
    {
        _glClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedBufferSubData< T >( GLuint buffer, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, t );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void* glMapNamedBuffer( GLuint buffer, GLenum access )
    {
        return _glMapNamedBuffer( buffer, access );
    }

// ========================================================================

    /// <inheritdoc/>
    public System.Span< T > glMapNamedBuffer< T >( GLuint buffer, GLenum access ) where T : unmanaged
    {
        var size = stackalloc GLint[ 1 ];
        _glGetNamedBufferParameteriv( buffer, GL_BUFFER_SIZE, size );
        void* ptr = _glMapNamedBuffer( buffer, access );

        return new System.Span< T >( ptr, *size / sizeof( T ) );
    }

// ========================================================================

    /// <inheritdoc/>
    public void* glMapNamedBufferRange( GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access )
    {
        return _glMapNamedBufferRange( buffer, offset, length, access );
    }

// ========================================================================

    /// <inheritdoc/>
    public System.Span< T > glMapNamedBufferRange< T >( GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access ) where T : unmanaged
    {
        void* ptr = _glMapNamedBufferRange( buffer, offset, length, access );

        return new System.Span< T >( ptr, ( int )length / sizeof( T ) );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLboolean glUnmapNamedBuffer( GLuint buffer )
    {
        return _glUnmapNamedBuffer( buffer );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glFlushMappedNamedBufferRange( GLuint buffer, GLintptr offset, GLsizeiptr length )
    {
        _glFlushMappedNamedBufferRange( buffer, offset, length );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedBufferParameteriv( GLuint buffer, GLenum pname, GLint* parameters )
    {
        _glGetNamedBufferParameteriv( buffer, pname, parameters );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedBufferParameteriv( GLuint buffer, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* ptr_parameters = &parameters[ 0 ] )
        {
            _glGetNamedBufferParameteriv( buffer, pname, ptr_parameters );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedBufferParameteri64v( GLuint buffer, GLenum pname, GLint64* parameters )
    {
        _glGetNamedBufferParameteri64v( buffer, pname, parameters );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedBufferParameteri64v( GLuint buffer, GLenum pname, ref GLint64[] parameters )
    {
        fixed ( GLint64* ptr_parameters = &parameters[ 0 ] )
        {
            _glGetNamedBufferParameteri64v( buffer, pname, ptr_parameters );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedBufferPointerv( GLuint buffer, GLenum pname, void** parameters )
    {
        _glGetNamedBufferPointerv( buffer, pname, parameters );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedBufferPointerv( GLuint buffer, GLenum pname, ref IntPtr[] parameters )
    {
        var ptr_parameters = new void*[ parameters.Length ];

        for ( var i = 0; i < parameters.Length; i++ )
        {
            ptr_parameters[ i ] = ( void* )parameters[ i ];
        }

        fixed ( void** ptr = &ptr_parameters[ 0 ] )
        {
            _glGetNamedBufferPointerv( buffer, pname, ptr );
        }

        for ( var i = 0; i < parameters.Length; i++ )
        {
            parameters[ i ] = ( IntPtr )ptr_parameters[ i ];
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glGetNamedBufferSubData( buffer, offset, size, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public T[] glGetNamedBufferSubData< T >( GLuint buffer, GLintptr offset, GLsizeiptr size ) where T : unmanaged
    {
        var data = new T[ size / sizeof( T ) ];

        fixed ( T* ptr_data = &data[ 0 ] )
        {
            _glGetNamedBufferSubData( buffer, offset, size, ptr_data );
        }

        return data;
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateFramebuffers( GLsizei n, GLuint* framebuffers )
    {
        _glCreateFramebuffers( n, framebuffers );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint[] glCreateFramebuffers( GLsizei n )
    {
        var framebuffers = new GLuint[ n ];

        fixed ( GLuint* ptr_framebuffers = &framebuffers[ 0 ] )
        {
            _glCreateFramebuffers( n, ptr_framebuffers );
        }

        return framebuffers;
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateFramebuffer()
    {
        return glCreateFramebuffers( 1 )[ 0 ];
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferRenderbuffer( GLuint framebuffer, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer )
    {
        _glNamedFramebufferRenderbuffer( framebuffer, attachment, renderbuffertarget, renderbuffer );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferParameteri( GLuint framebuffer, GLenum pname, GLint param )
    {
        _glNamedFramebufferParameteri( framebuffer, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferTexture( GLuint framebuffer, GLenum attachment, GLuint texture, GLint level )
    {
        _glNamedFramebufferTexture( framebuffer, attachment, texture, level );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferTextureLayer( GLuint framebuffer, GLenum attachment, GLuint texture, GLint level, GLint layer )
    {
        _glNamedFramebufferTextureLayer( framebuffer, attachment, texture, level, layer );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferDrawBuffer( GLuint framebuffer, GLenum buf )
    {
        _glNamedFramebufferDrawBuffer( framebuffer, buf );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferDrawBuffers( GLuint framebuffer, GLsizei n, GLenum* bufs )
    {
        _glNamedFramebufferDrawBuffers( framebuffer, n, bufs );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferDrawBuffers( GLuint framebuffer, GLenum[] bufs )
    {
        fixed ( GLenum* ptr_bufs = &bufs[ 0 ] )
        {
            _glNamedFramebufferDrawBuffers( framebuffer, bufs.Length, ptr_bufs );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedFramebufferReadBuffer( GLuint framebuffer, GLenum src )
    {
        _glNamedFramebufferReadBuffer( framebuffer, src );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferData( GLuint framebuffer, GLsizei numAttachments, GLenum* attachments )
    {
        _glInvalidateNamedFramebufferData( framebuffer, numAttachments, attachments );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferData( GLuint framebuffer, GLenum[] attachments )
    {
        fixed ( GLenum* ptr_attachments = &attachments[ 0 ] )
        {
            _glInvalidateNamedFramebufferData( framebuffer, attachments.Length, ptr_attachments );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferSubData( GLuint framebuffer, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glInvalidateNamedFramebufferSubData( framebuffer, numAttachments, attachments, x, y, width, height );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferSubData( GLuint framebuffer, GLenum[] attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        fixed ( GLenum* ptr_attachments = &attachments[ 0 ] )
        {
            _glInvalidateNamedFramebufferSubData( framebuffer, attachments.Length, ptr_attachments, x, y, width, height );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedFramebufferiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLint* value )
    {
        _glClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, value );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedFramebufferiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLint[] value )
    {
        fixed ( GLint* ptr_value =
                   &value[ 0 ] ) _glClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, ptr_value );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedFramebufferuiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLuint* value )
    {
        _glClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, value );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedFramebufferuiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLuint[] value )
    {
        fixed ( GLuint* ptr_value =
                   &value[ 0 ] ) _glClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, ptr_value );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedFramebufferfv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLfloat* value )
    {
        _glClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, value );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedFramebufferfv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLfloat[] value )
    {
        fixed ( GLfloat* ptr_value = &value[ 0 ] )
        {
            _glClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, ptr_value );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glClearNamedFramebufferfi( GLuint framebuffer, GLenum buffer, GLfloat depth, GLint stencil )
    {
        _glClearNamedFramebufferfi( framebuffer, buffer, depth, stencil );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBlitNamedFramebuffer( GLuint readFramebuffer, GLuint drawFramebuffer, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1,
                                        GLbitfield mask, GLenum filter )
    {
        _glBlitNamedFramebuffer( readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLenum glCheckNamedFramebufferStatus( GLuint framebuffer, GLenum target )
    {
        return _glCheckNamedFramebufferStatus( framebuffer, target );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedFramebufferParameteriv( GLuint framebuffer, GLenum pname, GLint* param )
    {
        _glGetNamedFramebufferParameteriv( framebuffer, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedFramebufferParameteriv( GLuint framebuffer, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param = &param[ 0 ] )
        {
            _glGetNamedFramebufferParameteriv( framebuffer, pname, ptr_param );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedFramebufferAttachmentParameteriv( GLuint framebuffer, GLenum attachment, GLenum pname, GLint* params_ )
    {
        _glGetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, params_ );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedFramebufferAttachmentParameteriv( GLuint framebuffer, GLenum attachment, GLenum pname, ref GLint[] params_ )
    {
        fixed ( GLint* ptr_params_ = &params_[ 0 ] )
        {
            _glGetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, ptr_params_ );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateRenderbuffers( GLsizei n, GLuint* renderbuffers )
    {
        _glCreateRenderbuffers( n, renderbuffers );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint[] glCreateRenderbuffers( GLsizei n )
    {
        var renderbuffers = new GLuint[ n ];

        fixed ( GLuint* ptr_renderbuffers = &renderbuffers[ 0 ] )
        {
            _glCreateRenderbuffers( n, ptr_renderbuffers );
        }

        return renderbuffers;
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateRenderbuffer()
    {
        return glCreateRenderbuffers( 1 )[ 0 ];
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedRenderbufferStorage( GLuint renderbuffer, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glNamedRenderbufferStorage( renderbuffer, internalformat, width, height );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glNamedRenderbufferStorageMultisample( GLuint renderbuffer, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glNamedRenderbufferStorageMultisample( renderbuffer, samples, internalformat, width, height );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedRenderbufferParameteriv( GLuint renderbuffer, GLenum pname, GLint* param )
    {
        _glGetNamedRenderbufferParameteriv( renderbuffer, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetNamedRenderbufferParameteriv( GLuint renderbuffer, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param = &param[ 0 ] )
        {
            _glGetNamedRenderbufferParameteriv( renderbuffer, pname, ptr_param );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateTextures( GLenum target, GLsizei n, GLuint* textures )
    {
        _glCreateTextures( target, n, textures );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint[] glCreateTextures( GLenum target, GLsizei n )
    {
        var textures = new GLuint[ n ];

        fixed ( GLuint* ptr_textures = &textures[ 0 ] )
        {
            _glCreateTextures( target, n, ptr_textures );
        }

        return textures;
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateTexture( GLenum target )
    {
        return glCreateTextures( target, 1 )[ 0 ];
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureBuffer( GLuint texture, GLenum internalformat, GLuint buffer )
    {
        _glTextureBuffer( texture, internalformat, buffer );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureBufferRange( GLuint texture, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTextureBufferRange( texture, internalformat, buffer, offset, size );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureStorage1D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width )
    {
        _glTextureStorage1D( texture, levels, internalformat, width );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureStorage2D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glTextureStorage2D( texture, levels, internalformat, width, height );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureStorage3D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glTextureStorage3D( texture, levels, internalformat, width, height, depth );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureStorage2DMultisample( GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations )
    {
        _glTextureStorage2DMultisample( texture, samples, internalformat, width, height, fixedsamplelocations );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureStorage3DMultisample( GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedsamplelocations )
    {
        _glTextureStorage3DMultisample( texture, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage1D( texture, level, xoffset, width, format, type, pixels );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureSubImage1D< T >( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, T[] pixels ) where T : unmanaged
    {
        fixed ( T* ptr_pixels = &pixels[ 0 ] )
        {
            _glTextureSubImage1D( texture, level, xoffset, width, format, type, ptr_pixels );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, pixels );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureSubImage2D< T >( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, T[] pixels ) where T : unmanaged
    {
        fixed ( T* ptr_pixels = &pixels[ 0 ] )
        {
            _glTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, ptr_pixels );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureSubImage3D< T >( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type,
                                          T[] pixels )
        where T : unmanaged
    {
        fixed ( T* ptr_pixels = &pixels[ 0 ] )
        {
            _glTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, ptr_pixels );
        }
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCompressedTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCompressedTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, byte[] data )
    {
        fixed ( byte* ptr_data
                   = &data[ 0 ] ) _glCompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, ptr_data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCompressedTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCompressedTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, byte[] data )
    {
        fixed ( byte* ptr_data
                   = &data[ 0 ] ) _glCompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, ptr_data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCompressedTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize,
                                               void* data )
    {
        _glCompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCompressedTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize,
                                               byte[] data )
    {
        fixed ( byte* ptr_data
                   = &data[ 0 ] ) _glCompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, ptr_data );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCopyTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width )
    {
        _glCopyTextureSubImage1D( texture, level, xoffset, x, y, width );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCopyTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTextureSubImage2D( texture, level, xoffset, yoffset, x, y, width, height );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCopyTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, x, y, width, height );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameterf( GLuint texture, GLenum pname, GLfloat param )
    {
        _glTextureParameterf( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameterfv( GLuint texture, GLenum pname, GLfloat* param )
    {
        _glTextureParameterfv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameterfv( GLuint texture, GLenum pname, GLfloat[] param )
    {
        fixed ( GLfloat* ptr_param = &param[ 0 ] ) _glTextureParameterfv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameteri( GLuint texture, GLenum pname, GLint param )
    {
        _glTextureParameteri( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameterIiv( GLuint texture, GLenum pname, GLint* param )
    {
        _glTextureParameterIiv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameterIiv( GLuint texture, GLenum pname, GLint[] param )
    {
        fixed ( GLint* ptr_param = &param[ 0 ] ) _glTextureParameterIiv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameterIuiv( GLuint texture, GLenum pname, GLuint* param )
    {
        _glTextureParameterIuiv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameterIuiv( GLuint texture, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* ptr_param =
                   &param[ 0 ] ) _glTextureParameterIuiv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameteriv( GLuint texture, GLenum pname, GLint* param )
    {
        _glTextureParameteriv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glTextureParameteriv( GLuint texture, GLenum pname, GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glTextureParameteriv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGenerateTextureMipmap( GLuint texture )
    {
        _glGenerateTextureMipmap( texture );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glBindTextureUnit( GLuint unit, GLuint texture )
    {
        _glBindTextureUnit( unit, texture );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureImage( GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pixels )
    {
        _glGetTextureImage( texture, level, format, type, bufSize, pixels );
    }

// ========================================================================

    /// <inheritdoc/>
    public T[] glGetTextureImage< T >( GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize ) where T : unmanaged
    {
        var pixels =
            new T[ bufSize ];
        fixed ( T* ptr_pixels = &pixels[ 0 ] ) _glGetTextureImage( texture, level, format, type, bufSize, ptr_pixels );

        return pixels;
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetCompressedTextureImage( GLuint texture, GLint level, GLsizei bufSize, void* pixels )
    {
        _glGetCompressedTextureImage( texture, level, bufSize, pixels );
    }

// ========================================================================

    /// <inheritdoc/>
    public byte[] glGetCompressedTextureImage( GLuint texture, GLint level, GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];
        fixed ( byte* ptr_pixels
                   = &pixels[ 0 ] ) _glGetCompressedTextureImage( texture, level, bufSize, ptr_pixels );

        return pixels;
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureLevelParameterfv( GLuint texture, GLint level, GLenum pname, GLfloat* param )
    {
        _glGetTextureLevelParameterfv( texture, level, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureLevelParameterfv( GLuint texture, GLint level, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* ptr_param =
                   &param[ 0 ] ) _glGetTextureLevelParameterfv( texture, level, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureLevelParameteriv( GLuint texture, GLint level, GLenum pname, GLint* param )
    {
        _glGetTextureLevelParameteriv( texture, level, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureLevelParameteriv( GLuint texture, GLint level, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetTextureLevelParameteriv( texture, level, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameterfv( GLuint texture, GLenum pname, GLfloat* param )
    {
        _glGetTextureParameterfv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameterfv( GLuint texture, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameterfv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameterIiv( GLuint texture, GLenum pname, GLint* param )
    {
        _glGetTextureParameterIiv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameterIiv( GLuint texture, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameterIiv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameterIuiv( GLuint texture, GLenum pname, GLuint* param )
    {
        _glGetTextureParameterIuiv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameterIuiv( GLuint texture, GLenum pname, ref GLuint[] param )
    {
        fixed ( GLuint* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameterIuiv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameteriv( GLuint texture, GLenum pname, GLint* param )
    {
        _glGetTextureParameteriv( texture, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetTextureParameteriv( GLuint texture, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameteriv( texture, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateVertexArrays( GLsizei n, GLuint* arrays )
    {
        _glCreateVertexArrays( n, arrays );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint[] glCreateVertexArrays( GLsizei n )
    {
        var arrays = new GLuint[ n ];
        fixed ( GLuint* ptr_arrays =
                   &arrays[ 0 ] ) _glCreateVertexArrays( n, ptr_arrays );

        return arrays;
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateVertexArray()
    {
        return glCreateVertexArrays( 1 )[ 0 ];
    }

// ========================================================================

    /// <inheritdoc/>
    public void glDisableVertexArrayAttrib( GLuint vaobj, GLuint index )
    {
        _glDisableVertexArrayAttrib( vaobj, index );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glEnableVertexArrayAttrib( GLuint vaobj, GLuint index )
    {
        _glEnableVertexArrayAttrib( vaobj, index );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayElementBuffer( GLuint vaobj, GLuint buffer )
    {
        _glVertexArrayElementBuffer( vaobj, buffer );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayVertexBuffer( GLuint vaobj, GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )
    {
        _glVertexArrayVertexBuffer( vaobj, bindingindex, buffer, offset, stride );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayVertexBuffers( GLuint vaobj, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides )
    {
        _glVertexArrayVertexBuffers( vaobj, first, count, buffers, offsets, strides );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayVertexBuffers( GLuint vaobj, GLuint first, GLuint[] buffers, GLintptr[] offsets, GLsizei[] strides )
    {
        fixed ( GLuint* ptr_buffers
                   = &buffers[ 0 ] )
        fixed ( GLintptr* ptr_offsets = &offsets[ 0 ] )
        fixed ( GLsizei* ptr_strides =
                   &strides[ 0 ] )
            _glVertexArrayVertexBuffers( vaobj, first, ( GLsizei )buffers.Length, ptr_buffers, ptr_offsets, ptr_strides );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayAttribBinding( GLuint vaobj, GLuint attribindex, GLuint bindingindex )
    {
        _glVertexArrayAttribBinding( vaobj, attribindex, bindingindex );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayAttribFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset )
    {
        _glVertexArrayAttribFormat( vaobj, attribindex, size, type, normalized, relativeoffset );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayAttribIFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexArrayAttribIFormat( vaobj, attribindex, size, type, relativeoffset );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayAttribLFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexArrayAttribLFormat( vaobj, attribindex, size, type, relativeoffset );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glVertexArrayBindingDivisor( GLuint vaobj, GLuint bindingindex, GLuint divisor )
    {
        _glVertexArrayBindingDivisor( vaobj, bindingindex, divisor );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetVertexArrayiv( GLuint vaobj, GLenum pname, GLint* param )
    {
        _glGetVertexArrayiv( vaobj, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetVertexArrayiv( GLuint vaobj, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetVertexArrayiv( vaobj, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetVertexArrayIndexediv( GLuint vaobj, GLuint index, GLenum pname, GLint* param )
    {
        _glGetVertexArrayIndexediv( vaobj, index, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetVertexArrayIndexediv( GLuint vaobj, GLuint index, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetVertexArrayIndexediv( vaobj, index, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetVertexArrayIndexed64iv( GLuint vaobj, GLuint index, GLenum pname, GLint64* param )
    {
        _glGetVertexArrayIndexed64iv( vaobj, index, pname, param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glGetVertexArrayIndexed64iv( GLuint vaobj, GLuint index, GLenum pname, ref GLint64[] param )
    {
        fixed ( GLint64* ptr_param =
                   &param[ 0 ] ) _glGetVertexArrayIndexed64iv( vaobj, index, pname, ptr_param );
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateSamplers( GLsizei n, GLuint* samplers )
    {
        _glCreateSamplers( n, samplers );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint[] glCreateSamplers( GLsizei n )
    {
        var samplers = new GLuint[ n ];
        fixed ( GLuint* ptr_samplers =
                   &samplers[ 0 ] ) _glCreateSamplers( n, ptr_samplers );

        return samplers;
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateSamplers()
    {
        return glCreateSamplers( 1 )[ 0 ];
    }

// ========================================================================

    /// <inheritdoc/>
    public void glCreateProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glCreateProgramPipelines( n, pipelines );
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint[] glCreateProgramPipelines( GLsizei n )
    {
        var pipelines = new GLuint[ n ];
        fixed ( GLuint* ptr_pipelines =
                   &pipelines[ 0 ] ) _glCreateProgramPipelines( n, ptr_pipelines );

        return pipelines;
    }

// ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateProgramPipeline()
    {
        return glCreateProgramPipelines( 1 )[ 0 ];
    }

}

#pragma warning restore IDE0079    // Remove unnecessary suppression
#pragma warning restore CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore CS8603     // Possible null reference return.
#pragma warning restore IDE0060    // Remove unused parameter.
#pragma warning restore IDE1006    // Naming Styles.
#pragma warning restore IDE0090    // Use 'new(...)'.
#pragma warning restore CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning restore SYSLIB1054 // 

