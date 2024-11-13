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
// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateQueries( GLenum target, GLsizei n, GLuint* ids )
    {
        _glCreateQueries( target, n, ids );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glCreateQueries( GLenum target, GLsizei n )
    {
        var ids = new GLuint[ n ];
        fixed ( GLuint* ptr_ids =
                   &ids[ 0 ] ) _glCreateQueries( target, n, ptr_ids );

        return ids;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateQuery( GLenum target )
    {
        return glCreateQueries( target, 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryBufferObjecti64v( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjecti64v( id, buffer, pname, offset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryBufferObjectiv( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjectiv( id, buffer, pname, offset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryBufferObjectui64v( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjectui64v( id, buffer, pname, offset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryBufferObjectuiv( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjectuiv( id, buffer, pname, offset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMemoryBarrierByRegion( GLbitfield barriers )
    {
        _glMemoryBarrierByRegion( barriers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type,
                                      GLsizei bufSize,
                                      void* pixels )
    {
        _glGetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public byte[] glGetTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type,
                                        GLsizei bufSize )
    {
        var pixels
            = new byte[ bufSize ];
        fixed ( void* ptr_pixels =
                   &pixels[ 0 ] ) _glGetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, ptr_pixels );

        return pixels;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetCompressedTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLsizei bufSize, void* pixels )
    {
        _glGetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public byte[] glGetCompressedTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLsizei bufSize )
    {
        var pixels
            = new byte[ bufSize ];
        fixed ( void* ptr_pixels =
                   &pixels[ 0 ] ) _glGetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, ptr_pixels );

        return pixels;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLenum glGetGraphicsResetStatus()
    {
        return _glGetGraphicsResetStatus();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnCompressedTexImage( GLenum target, GLint lod, GLsizei bufSize, void* pixels )
    {
        _glGetnCompressedTexImage( target, lod, bufSize, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public byte[] glGetnCompressedTexImage( GLenum target, GLint lod, GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];
        fixed ( void* ptr_pixels =
                   &pixels[ 0 ] ) _glGetnCompressedTexImage( target, lod, bufSize, ptr_pixels );

        return pixels;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnTexImage( GLenum target, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pixels )
    {
        _glGetnTexImage( target, level, format, type, bufSize, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public byte[] glGetnTexImage( GLenum target, GLint level, GLenum format, GLenum type, GLsizei bufSize )
    {
        var pixels =
            new byte[ bufSize ];
        fixed ( void* ptr_pixels = &pixels[ 0 ] ) _glGetnTexImage( target, level, format, type, bufSize, ptr_pixels );

        return pixels;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformdv( GLuint program, GLint location, GLsizei bufSize, GLdouble* parameters )
    {
        _glGetnUniformdv( program, location, bufSize, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformdv( GLuint program, GLint location, GLsizei bufSize, ref GLdouble[] parameters )
    {
        fixed ( void* ptr_parameters =
                   &parameters[ 0 ] ) _glGetnUniformdv( program, location, bufSize, ( GLdouble* )ptr_parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformfv( GLuint program, GLint location, GLsizei bufSize, GLfloat* parameters )
    {
        _glGetnUniformfv( program, location, bufSize, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformfv( GLuint program, GLint location, GLsizei bufSize, ref GLfloat[] parameters )
    {
        fixed ( void* ptr_parameters =
                   &parameters[ 0 ] ) _glGetnUniformfv( program, location, bufSize, ( GLfloat* )ptr_parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformiv( GLuint program, GLint location, GLsizei bufSize, GLint* parameters )
    {
        _glGetnUniformiv( program, location, bufSize, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformiv( GLuint program, GLint location, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( void* ptr_parameters =
                   &parameters[ 0 ] ) _glGetnUniformiv( program, location, bufSize, ( GLint* )ptr_parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformuiv( GLuint program, GLint location, GLsizei bufSize, GLuint* parameters )
    {
        _glGetnUniformuiv( program, location, bufSize, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetnUniformuiv( GLuint program, GLint location, GLsizei bufSize, ref GLuint[] parameters )
    {
        fixed ( void* ptr_parameters = &parameters[ 0 ] )
        {
            _glGetnUniformuiv( program, location, bufSize, ( GLuint* )ptr_parameters );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glReadnPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, GLsizei bufSize, void* data )
    {
        _glReadnPixels( x, y, width, height, format, type, bufSize, data );
    }

    // ------------------------------------------------------------------------

    public byte[] glReadnPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, GLsizei bufSize )
    {
        var data = new byte[ bufSize ];

        fixed ( void* ptr_data = &data[ 0 ] )
        {
            _glReadnPixels( x, y, width, height, format, type, bufSize, ptr_data );
        }

        return data;
    }

    // ------------------------------------------------------------------------

    public void glTextureBarrier()
    {
        _glTextureBarrier();
    }

    // ------------------------------------------------------------------------

    public void glSpecializeShader( GLuint shader, GLchar* pEntryPoint, GLuint numSpecializationConstants, GLuint* pConstantIndex, GLuint* pConstantValue )
    {
        _glSpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
    }

    // ------------------------------------------------------------------------

    public void glSpecializeShader( GLuint shader, string pEntryPoint, GLuint numSpecializationConstants, GLuint[] pConstantIndex, GLuint[] pConstantValue )
    {
        var pEntryPointBytes = Encoding.UTF8.GetBytes( pEntryPoint );
        fixed ( GLchar* ptr_pEntryPoint = &pEntryPointBytes[ 0 ] )
        fixed ( GLuint* ptr_pConstantIndex = &pConstantIndex[ 0 ] )
        fixed ( GLuint* ptr_pConstantValue = &pConstantValue[ 0 ] )
        {
            _glSpecializeShader( shader, ptr_pEntryPoint, numSpecializationConstants, ptr_pConstantIndex, ptr_pConstantValue );
        }
    }

    // ------------------------------------------------------------------------

    public void glMultiDrawArraysIndirectCount( GLenum mode, void* indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
    }

    // ------------------------------------------------------------------------

    public void glMultiDrawArraysIndirectCount( GLenum mode, DrawArraysIndirectCommand indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirectCount( mode, &indirect, drawcount, maxdrawcount, stride );
    }

    // ------------------------------------------------------------------------

    public void glMultiDrawElementsIndirectCount( GLenum mode, GLenum type, void* indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
    }

    // ------------------------------------------------------------------------

    public void glMultiDrawElementsIndirectCount( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirectCount( mode, type, &indirect, drawcount, maxdrawcount, stride );
    }

    // ------------------------------------------------------------------------

    public void glPolygonOffsetClamp( GLfloat factor, GLfloat units, GLfloat clamp )
    {
        _glPolygonOffsetClamp( factor, units, clamp );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTransformFeedbackVaryings( GLuint program, GLsizei count, GLchar** varyings, GLenum bufferMode )
    {
        _glTransformFeedbackVaryings( program, count, varyings, bufferMode );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTransformFeedbackVaryings( GLuint program, string[] varyings, GLenum bufferMode )
    {
        var varyingsBytes = new GLchar[ varyings.Length ][];

        for ( var i = 0; i < varyings.Length; i++ )
        {
            varyingsBytes[ i ] = Encoding.UTF8.GetBytes( varyings[ i ] );
        }

        var varyingsPtrs = new GLchar*[ varyings.Length ];

        for ( var i = 0; i < varyings.Length; i++ )
        {
            fixed ( GLchar* p = &varyingsBytes[ i ][ 0 ] )
            {
                varyingsPtrs[ i ] = p;
            }
        }

        fixed ( GLchar** p = &varyingsPtrs[ 0 ] )
        {
            _glTransformFeedbackVaryings( program, varyings.Length, p, bufferMode );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTransformFeedbackVarying( GLuint program,
                                               GLuint index,
                                               GLsizei bufSize,
                                               GLsizei* length,
                                               GLsizei* size,
                                               GLenum* type,
                                               GLchar* name )
    {
        _glGetTransformFeedbackVarying( program, index, bufSize, length, size, type, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetTransformFeedbackVarying( GLuint program, GLuint index, GLsizei bufSize, out GLsizei size, out GLenum type )
    {
        var     name = new GLchar[ bufSize ];
        GLsizei length;

        fixed ( GLsizei* pSize = &size )
        {
            fixed ( GLenum* pType = &type )
            {
                fixed ( GLchar* p = &name[ 0 ] )
                {
                    _glGetTransformFeedbackVarying( program, index, bufSize, &length, pSize, pType, p );

                    return new string( ( sbyte* )p, 0, length, Encoding.UTF8 );
                }
            }
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClampColor( GLenum target, GLboolean clamp )
    {
        _glClampColor( target, clamp ? IGL.GL_TRUE : IGL.GL_FALSE );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBeginConditionalRender( GLuint id, GLenum mode )
    {
        _glBeginConditionalRender( id, mode );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEndConditionalRender()
    {
        _glEndConditionalRender();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribIPointer( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer )
    {
        _glVertexAttribIPointer( index, size, type, stride, pointer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribIPointer( GLuint index, GLint size, GLenum type, GLsizei stride, uint pointer )
    {
        _glVertexAttribIPointer( index, size, type, stride, ( void* )pointer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribIiv( GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetVertexAttribIiv( index, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribIiv( GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p =
                   &parameters[ 0 ] )
        {
            _glGetVertexAttribIiv( index, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribIuiv( GLuint index, GLenum pname, GLuint* parameters )
    {
        _glGetVertexAttribIuiv( index, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribIuiv( GLuint index, GLenum pname, ref GLuint[] parameters )
    {
        fixed ( GLuint* p =
                   &parameters[ 0 ] )
        {
            _glGetVertexAttribIuiv( index, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI1i( GLuint index, GLint x )
    {
        _glVertexAttribI1i( index, x );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI2i( GLuint index, GLint x, GLint y )
    {
        _glVertexAttribI2i( index, x, y );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI3i( GLuint index, GLint x, GLint y, GLint z )
    {
        _glVertexAttribI3i( index, x, y, z );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4i( GLuint index, GLint x, GLint y, GLint z, GLint w )
    {
        _glVertexAttribI4i( index, x, y, z, w );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI1ui( GLuint index, GLuint x )
    {
        _glVertexAttribI1ui( index, x );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI2ui( GLuint index, GLuint x, GLuint y )
    {
        _glVertexAttribI2ui( index, x, y );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI3ui( GLuint index, GLuint x, GLuint y, GLuint z )
    {
        _glVertexAttribI3ui( index, x, y, z );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4ui( GLuint index, GLuint x, GLuint y, GLuint z, GLuint w )
    {
        _glVertexAttribI4ui( index, x, y, z, w );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI1iv( GLuint index, GLint* v )
    {
        _glVertexAttribI1iv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI1iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI1iv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI2iv( GLuint index, GLint* v )
    {
        _glVertexAttribI2iv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI2iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI2iv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI3iv( GLuint index, GLint* v )
    {
        _glVertexAttribI3iv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI3iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI3iv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4iv( GLuint index, GLint* v )
    {
        _glVertexAttribI4iv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI4iv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI1uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI1uiv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI1uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI1uiv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI2uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI2uiv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI2uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI2uiv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI3uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI3uiv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI3uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI3uiv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI4uiv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI4uiv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4bv( GLuint index, GLbyte* v )
    {
        _glVertexAttribI4bv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4bv( GLuint index, GLbyte[] v )
    {
        fixed ( GLbyte* p = &v[ 0 ] )
        {
            _glVertexAttribI4bv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4sv( GLuint index, GLshort* v )
    {
        _glVertexAttribI4sv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4sv( GLuint index, GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttribI4sv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4ubv( GLuint index, GLubyte* v )
    {
        _glVertexAttribI4ubv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4ubv( GLuint index, GLubyte[] v )
    {
        fixed ( GLubyte* p = &v[ 0 ] )
        {
            _glVertexAttribI4ubv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4usv( GLuint index, GLushort* v )
    {
        _glVertexAttribI4usv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribI4usv( GLuint index, GLushort[] v )
    {
        fixed ( GLushort* p = &v[ 0 ] )
        {
            _glVertexAttribI4usv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformuiv( GLuint program, GLint location, GLuint* parameters )
    {
        _glGetUniformuiv( program, location, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformuiv( GLuint program, GLint location, ref GLuint[] parameters )
    {
        fixed ( GLuint* p =
                   &parameters[ 0 ] )
        {
            _glGetUniformuiv( program, location, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindFragDataLocation( GLuint program, GLuint color, GLchar* name )
    {
        _glBindFragDataLocation( program, color, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindFragDataLocation( GLuint program, GLuint color, string name )
    {
        var narr = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p =
                   &narr[ 0 ] )
        {
            _glBindFragDataLocation( program, color, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetFragDataLocation( GLuint program, GLchar* name )
    {
        return _glGetFragDataLocation( program, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetFragDataLocation( GLuint program, string name )
    {
        var narr = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p =
                   &narr[ 0 ] )
        {
            return _glGetFragDataLocation( program, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1ui( GLint location, GLuint v0 )
    {
        _glUniform1ui( location, v0 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2ui( GLint location, GLuint v0, GLuint v1 )
    {
        _glUniform2ui( location, v0, v1 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3ui( GLint location, GLuint v0, GLuint v1, GLuint v2 )
    {
        _glUniform3ui( location, v0, v1, v2 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4ui( GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 )
    {
        _glUniform4ui( location, v0, v1, v2, v3 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform1uiv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform1uiv( location, value.Length, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform2uiv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform2uiv( location, value.Length / 2, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform3uiv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform3uiv( location, value.Length / 3, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform4uiv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform4uiv( location, value.Length / 4, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameterIiv( GLenum target, GLenum pname, GLint* param )
    {
        _glTexParameterIiv( target, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameterIiv( GLenum target, GLenum pname, GLint[] param )
    {
        fixed ( GLint* p = &param[ 0 ] )
        {
            _glTexParameterIiv( target, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameterIuiv( GLenum target, GLenum pname, GLuint* param )
    {
        _glTexParameterIuiv( target, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameterIuiv( GLenum target, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* p =
                   &param[ 0 ] )
        {
            _glTexParameterIuiv( target, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexParameterIiv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetTexParameterIiv( target, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexParameterIiv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p =
                   &parameters[ 0 ] )
        {
            _glGetTexParameterIiv( target, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexParameterIuiv( GLenum target, GLenum pname, GLuint* parameters )
    {
        _glGetTexParameterIuiv( target, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexParameterIuiv( GLenum target, GLenum pname, ref GLuint[] parameters )
    {
        fixed ( GLuint* p =
                   &parameters[ 0 ] )
        {
            _glGetTexParameterIuiv( target, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferiv( GLenum buffer, GLint drawbuffer, GLint* value )
    {
        _glClearBufferiv( buffer, drawbuffer, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferiv( GLenum buffer, GLint drawbuffer, GLint[] value )
    {
        fixed ( GLint* p =
                   &value[ 0 ] )
        {
            _glClearBufferiv( buffer, drawbuffer, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferuiv( GLenum buffer, GLint drawbuffer, GLuint* value )
    {
        _glClearBufferuiv( buffer, drawbuffer, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferuiv( GLenum buffer, GLint drawbuffer, GLuint[] value )
    {
        fixed ( GLuint* p =
                   &value[ 0 ] )
        {
            _glClearBufferuiv( buffer, drawbuffer, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferfv( GLenum buffer, GLint drawbuffer, GLfloat* value )
    {
        _glClearBufferfv( buffer, drawbuffer, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferfv( GLenum buffer, GLint drawbuffer, GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glClearBufferfv( buffer, drawbuffer, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferfi( GLenum buffer, GLint drawbuffer, GLfloat depth, GLint stencil )
    {
        _glClearBufferfi( buffer, drawbuffer, depth, stencil );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLubyte* glGetStringi( GLenum name, GLuint index )
    {
        return _glGetStringi( name, index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetStringiSafe( GLenum name, GLuint index )
    {
        var ptr = _glGetStringi( name, index );

        if ( ptr == null )
        {
            return null;
        }

        var i = 0;

        while ( ptr[ i ] != 0 )
        {
            i++;
        }

        return new string( ( sbyte* )ptr, 0, i, Encoding.UTF8 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsRenderbuffer( GLuint renderbuffer )
    {
        return _glIsRenderbuffer( renderbuffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindRenderbuffer( GLenum target, GLuint renderbuffer )
    {
        _glBindRenderbuffer( target, renderbuffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteRenderbuffers( GLsizei n, GLuint* renderbuffers )
    {
        _glDeleteRenderbuffers( n, renderbuffers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteRenderbuffers( params GLuint[] renderbuffers )
    {
        fixed ( GLuint* p =
                   &renderbuffers[ 0 ] )
        {
            _glDeleteRenderbuffers( renderbuffers.Length, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenRenderbuffers( GLsizei n, GLuint* renderbuffers )
    {
        _glGenRenderbuffers( n, renderbuffers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glGenRenderbuffers( GLsizei n )
    {
        var renderbuffers = new GLuint[ n ];

        fixed ( GLuint* p =
                   &renderbuffers[ 0 ] )
        {
            _glGenRenderbuffers( n, p );
        }

        return renderbuffers;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGenRenderbuffer()
    {
        return glGenRenderbuffers( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glRenderbufferStorage( GLenum target, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glRenderbufferStorage( target, internalformat, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetRenderbufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetRenderbufferParameteriv( target, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetRenderbufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p =
                   &parameters[ 0 ] )
        {
            _glGetRenderbufferParameteriv( target, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsFramebuffer( GLuint framebuffer )
    {
        return _glIsFramebuffer( framebuffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindFramebuffer( GLenum target, GLuint framebuffer )
    {
        _glBindFramebuffer( target, framebuffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteFramebuffers( GLsizei n, GLuint* framebuffers )
    {
        _glDeleteFramebuffers( n, framebuffers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteFramebuffers( params GLuint[] framebuffers )
    {
        fixed ( GLuint* p =
                   &framebuffers[ 0 ] )
        {
            _glDeleteFramebuffers( framebuffers.Length, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenFramebuffers( GLsizei n, GLuint* framebuffers )
    {
        _glGenFramebuffers( n, framebuffers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glGenFramebuffers( GLsizei n )
    {
        var framebuffers = new GLuint[ n ];

        fixed ( GLuint* p =
                   &framebuffers[ 0 ] )
        {
            _glGenFramebuffers( n, p );
        }

        return framebuffers;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGenFramebuffer()
    {
        return glGenFramebuffers( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLenum glCheckFramebufferStatus( GLenum target )
    {
        return _glCheckFramebufferStatus( target );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFramebufferTexture1D( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level )
    {
        _glFramebufferTexture1D( target, attachment, textarget, texture, level );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFramebufferTexture2D( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level )
    {
        _glFramebufferTexture2D( target, attachment, textarget, texture, level );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFramebufferTexture3D( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level, GLint zoffset )
    {
        _glFramebufferTexture3D( target, attachment, textarget, texture, level, zoffset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFramebufferRenderbuffer( GLenum target, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer )
    {
        _glFramebufferRenderbuffer( target, attachment, renderbuffertarget, renderbuffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetFramebufferAttachmentParameteriv( GLenum target, GLenum attachment, GLenum pname, GLint* parameters )
    {
        _glGetFramebufferAttachmentParameteriv( target, attachment, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetFramebufferAttachmentParameteriv( GLenum target, GLenum attachment, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetFramebufferAttachmentParameteriv( target, attachment, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenerateMipmap( GLenum target )
    {
        _glGenerateMipmap( target );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlitFramebuffer( GLint srcX0,
                                   GLint srcY0,
                                   GLint srcX1,
                                   GLint srcY1,
                                   GLint dstX0,
                                   GLint dstY0,
                                   GLint dstX1,
                                   GLint dstY1,
                                   GLbitfield mask,
                                   GLenum filter )
    {
        _glBlitFramebuffer( srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glRenderbufferStorageMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glRenderbufferStorageMultisample( target, samples, internalformat, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFramebufferTextureLayer( GLenum target, GLenum attachment, GLuint texture, GLint level, GLint layer )
    {
        _glFramebufferTextureLayer( target, attachment, texture, level, layer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void* glMapBufferRange( GLenum target, GLintptr offset, GLsizeiptr length, GLbitfield access )
    {
        return _glMapBufferRange( target, offset, length, access );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public Span< T > glMapBufferRange< T >( GLenum target, GLintptr offset, GLsizeiptr length, GLbitfield access ) where T : unmanaged
    {
        void* ret = _glMapBufferRange( target, offset, length, access );

        return new Span< T >( ret, length );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFlushMappedBufferRange( GLenum target, GLintptr offset, GLsizeiptr length )
    {
        _glFlushMappedBufferRange( target, offset, length );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindVertexArray( GLuint array )
    {
        _glBindVertexArray( array );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteVertexArrays( GLsizei n, GLuint* arrays )
    {
        _glDeleteVertexArrays( n, arrays );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteVertexArrays( params GLuint[] arrays )
    {
        fixed ( GLuint* p = &arrays[ 0 ] )
        {
            _glDeleteVertexArrays( arrays.Length, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenVertexArrays( GLsizei n, GLuint* arrays )
    {
        _glGenVertexArrays( n, arrays );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glGenVertexArrays( GLsizei n )
    {
        var arrays = new GLuint[ n ];

        fixed ( GLuint* p = &arrays[ 0 ] )
        {
            _glGenVertexArrays( n, p );
        }

        return arrays;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGenVertexArray()
    {
        GLuint array = 0;
        _glGenVertexArrays( 1, &array );

        return array;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsVertexArray( GLuint array )
    {
        return _glIsVertexArray( array );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawArraysInstanced( GLenum mode, GLint first, GLsizei count, GLsizei instancecount )
    {
        _glDrawArraysInstanced( mode, first, count, instancecount );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstanced( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount )
    {
        _glDrawElementsInstanced( mode, count, type, indices, instancecount );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstanced< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( T* p = &indices[ 0 ] )
        {
            _glDrawElementsInstanced( mode, count, type, p, instancecount );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexBuffer( GLenum target, GLenum internalformat, GLuint buffer )
    {
        _glTexBuffer( target, internalformat, buffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPrimitiveRestartIndex( GLuint index )
    {
        _glPrimitiveRestartIndex( index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyBufferSubData( GLenum readTarget, GLenum writeTarget, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size )
    {
        _glCopyBufferSubData( readTarget, writeTarget, readOffset, writeOffset, size );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformIndices( GLuint program, GLsizei uniformCount, GLchar** uniformNames, GLuint* uniformIndices )
    {
        _glGetUniformIndices( program, uniformCount, uniformNames, uniformIndices );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glGetUniformIndices( GLuint program, params string[] uniformNames )
    {
        var uniformCount     = uniformNames.Length;
        var uniformNamesPtrs = new GLchar[ uniformCount ][];

        for ( var i = 0; i < uniformCount; i++ )
        {
            uniformNamesPtrs[ i ] = Encoding.UTF8.GetBytes( uniformNames[ i ] );
        }

        var pUniformNames = stackalloc GLchar*[ uniformCount ];

        for ( var i = 0; i < uniformCount; i++ )
        {
            fixed ( GLchar* p = &uniformNamesPtrs[ i ][ 0 ] )
            {
                pUniformNames[ i ] = p;
            }
        }

        var uniformIndices = new GLuint[ uniformCount ];

        fixed ( GLuint* p = &uniformIndices[ 0 ] )
        {
            _glGetUniformIndices( program, uniformCount, pUniformNames, p );
        }

        return uniformIndices;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveUniformsiv( GLuint program, GLsizei uniformCount, GLuint* uniformIndices, GLenum pname, GLint* parameters )
    {
        _glGetActiveUniformsiv( program, uniformCount, uniformIndices, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint[] glGetActiveUniformsiv( GLuint program, GLenum pname, params GLuint[] uniformIndices )
    {
        var uniformCount = uniformIndices.Length;
        var parameters   = new GLint[ uniformCount ];

        fixed ( GLuint* p = &uniformIndices[ 0 ] )
        {
            fixed ( GLint* pParameters = &parameters[ 0 ] )
            {
                _glGetActiveUniformsiv( program, uniformCount, p, pname, pParameters );
            }
        }

        return parameters;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveUniformName( GLuint program, GLuint uniformIndex, GLsizei bufSize, GLsizei* length, GLchar* uniformName )
    {
        _glGetActiveUniformName( program, uniformIndex, bufSize, length, uniformName );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetActiveUniformName( GLuint program, GLuint uniformIndex, GLsizei bufSize )
    {
        var     uniformName = stackalloc GLchar[ bufSize ];
        GLsizei length;
        _glGetActiveUniformName( program, uniformIndex, bufSize, &length, uniformName );

        return new string( ( sbyte* )uniformName, 0, length, Encoding.UTF8 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetUniformBlockIndex( GLuint program, GLchar* uniformBlockName )
    {
        return _glGetUniformBlockIndex( program, uniformBlockName );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetUniformBlockIndex( GLuint program, string uniformBlockName )
    {
        var uniformBlockNameBytes = Encoding.UTF8.GetBytes( uniformBlockName );

        fixed ( GLchar* p = &uniformBlockNameBytes[ 0 ] )
        {
            return _glGetUniformBlockIndex( program, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveUniformBlockiv( GLuint program, GLuint uniformBlockIndex, GLenum pname, GLint* parameters )
    {
        _glGetActiveUniformBlockiv( program, uniformBlockIndex, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveUniformBlockiv( GLuint program, GLuint uniformBlockIndex, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p =
                   &parameters[ 0 ] )
        {
            _glGetActiveUniformBlockiv( program, uniformBlockIndex, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveUniformBlockName( GLuint program, GLuint uniformBlockIndex, GLsizei bufSize, GLsizei* length, GLchar* uniformBlockName )
    {
        _glGetActiveUniformBlockName( program, uniformBlockIndex, bufSize, length, uniformBlockName );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetActiveUniformBlockName( GLuint program, GLuint uniformBlockIndex, GLsizei bufSize )
    {
        var     uniformBlockName = stackalloc GLchar[ bufSize ];
        GLsizei length;
        _glGetActiveUniformBlockName( program, uniformBlockIndex, bufSize, &length, uniformBlockName );

        return new string( ( sbyte* )uniformBlockName, 0, length, Encoding.UTF8 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformBlockBinding( GLuint program, GLuint uniformBlockIndex, GLuint uniformBlockBinding )
    {
        _glUniformBlockBinding( program, uniformBlockIndex, uniformBlockBinding );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsBaseVertex( GLenum mode, GLsizei count, GLenum type, void* indices, GLint basevertex )
    {
        _glDrawElementsBaseVertex( mode, count, type, indices, basevertex );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsBaseVertex< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLint basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p
                   = &indices[ 0 ] )
        {
            _glDrawElementsBaseVertex( mode, count, type, p, basevertex );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawRangeElementsBaseVertex( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices, GLint basevertex )
    {
        _glDrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawRangeElementsBaseVertex< T >( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, T[] indices, GLint basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p
                   = &indices[ 0 ] )
        {
            _glDrawRangeElementsBaseVertex( mode, start, end, count, type, p, basevertex );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstancedBaseVertex( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLint basevertex )
    {
        _glDrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstancedBaseVertex< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLint basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p
                   = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseVertex( mode, count, type, p, instancecount, basevertex );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawElementsBaseVertex( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount, GLint* basevertex )
    {
        _glMultiDrawElementsBaseVertex( mode, count, type, indices, drawcount, basevertex );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawElementsBaseVertex< T >( GLenum mode, GLenum type, T[][] indices, GLint[] basevertex ) where T : unmanaged, IUnsignedNumber< T >
    {
        if ( indices.Length != basevertex.Length )
        {
            throw new ArgumentException( "indices and basevertex must have the same length" );
        }

        var counts    = new GLsizei[ indices.Length ];
        var indexPtrs = new T*[ indices.Length ];

        for ( var i = 0; i < indices.Length; i++ )
        {
            counts[ i ] = indices[ i ].Length;

            fixed ( T* p = &indices[ i ][ 0 ] )
            {
                indexPtrs[ i ] = p;
            }
        }

        fixed ( GLsizei* cp = &counts[ 0 ] )
        {
            fixed ( GLint* bvp = &basevertex[ 0 ] )
            {
                fixed ( T** ip = &indexPtrs[ 0 ] )
                {
                    _glMultiDrawElementsBaseVertex( mode, cp, type, ( void** )ip, indices.Length, bvp );
                }
            }
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProvokingVertex( GLenum mode )
    {
        _glProvokingVertex( mode );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void* glFenceSync( GLenum condition, GLbitfield flags )
    {
        return _glFenceSync( condition, flags );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public IntPtr glFenceSyncSafe( GLenum condition, GLbitfield flags )
    {
        return new IntPtr( _glFenceSync( condition, flags ) );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsSync( void* sync )
    {
        return _glIsSync( sync );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsSyncSafe( IntPtr sync )
    {
        return _glIsSync( sync.ToPointer() );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteSync( void* sync )
    {
        _glDeleteSync( sync );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteSyncSafe( IntPtr sync )
    {
        _glDeleteSync( sync.ToPointer() );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLenum glClientWaitSync( void* sync, GLbitfield flags, GLuint64 timeout )
    {
        return _glClientWaitSync( sync, flags, timeout );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLenum glClientWaitSyncSafe( IntPtr sync, GLbitfield flags, GLuint64 timeout )
    {
        return _glClientWaitSync( sync.ToPointer(), flags, timeout );
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

