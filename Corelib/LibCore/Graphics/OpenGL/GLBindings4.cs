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
    public void glUniformMatrix2x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2x4dv( location, count, transpose, value );
    }

    public void glUniformMatrix2x4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix2x4dv( location, value.Length / 8, transpose, p );
        }
    }

    // ========================================================================

    public void glUniformMatrix3x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3x2dv( location, count, transpose, value );
    }

    public void glUniformMatrix3x2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix3x2dv( location, value.Length / 6, transpose, p );
        }
    }

    // ========================================================================

    public void glUniformMatrix3x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3x4dv( location, count, transpose, value );
    }

    public void glUniformMatrix3x4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix3x4dv( location, value.Length / 12, transpose, p );
        }
    }

    // ========================================================================

    public void glUniformMatrix4x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4x2dv( location, count, transpose, value );
    }

    public void glUniformMatrix4x2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix4x2dv( location, value.Length / 8, transpose, p );
        }
    }

    // ========================================================================

    public void glUniformMatrix4x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4x3dv( location, count, transpose, value );
    }

    public void glUniformMatrix4x3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix4x3dv( location, value.Length / 12, transpose, p );
        }
    }

    // ========================================================================

    public void glGetUniformdv( GLuint program, GLint location, GLdouble* parameters )
    {
        _glGetUniformdv( program, location, parameters );
    }

    public void glGetUniformdv( GLuint program, GLint location, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* p = &parameters[ 0 ] )
        {
            _glGetUniformdv( program, location, p );
        }
    }

    // ========================================================================

    public GLint glGetSubroutineUniformLocation( GLuint program, GLenum shadertype, GLchar* name )
    {
        return _glGetSubroutineUniformLocation( program, shadertype, name );
    }

    public GLint glGetSubroutineUniformLocation( GLuint program, GLenum shadertype, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetSubroutineUniformLocation( program, shadertype, p );
        }
    }

    // ========================================================================

    public GLuint glGetSubroutineIndex( GLuint program, GLenum shadertype, GLchar* name )
    {
        return _glGetSubroutineIndex( program, shadertype, name );
    }

    public GLuint glGetSubroutineIndex( GLuint program, GLenum shadertype, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetSubroutineIndex( program, shadertype, p );
        }
    }

    // ========================================================================

    public void glGetActiveSubroutineUniformiv( GLuint program, GLenum shadertype, GLuint index, GLenum pname, GLint* values )
    {
        _glGetActiveSubroutineUniformiv( program, shadertype, index, pname, values );
    }

    public void glGetActiveSubroutineUniformiv( GLuint program, GLenum shadertype, GLuint index, GLenum pname, ref GLint[] values )
    {
        fixed ( GLint* p = &values[ 0 ] )
        {
            _glGetActiveSubroutineUniformiv( program, shadertype, index, pname, p );
        }
    }

    // ========================================================================

    public void glGetActiveSubroutineUniformName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name )
    {
        _glGetActiveSubroutineUniformName( program, shadertype, index, bufsize, length, name );
    }

    public string glGetActiveSubroutineUniformName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize )
    {
        var name = new GLchar[ bufsize ];

        fixed ( GLchar* p = &name[ 0 ] )
        {
            GLsizei length;

            _glGetActiveSubroutineUniformName( program, shadertype, index, bufsize, &length, p );

            return new string( ( sbyte* )p, 0, length, Encoding.UTF8 );
        }
    }

    // ========================================================================

    public void glGetActiveSubroutineName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name )
    {
        _glGetActiveSubroutineName( program, shadertype, index, bufsize, length, name );
    }

    public string glGetActiveSubroutineName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize )
    {
        var name = new GLchar[ bufsize ];

        fixed ( GLchar* p = &name[ 0 ] )
        {
            GLsizei length;

            _glGetActiveSubroutineName( program, shadertype, index, bufsize, &length, p );

            return new string( ( sbyte* )p, 0, length, Encoding.UTF8 );
        }
    }

    // ========================================================================

    public void glUniformSubroutinesuiv( GLenum shadertype, GLsizei count, GLuint* indices )
    {
        _glUniformSubroutinesuiv( shadertype, count, indices );
    }

    public void glUniformSubroutinesuiv( GLenum shadertype, GLuint[] indices )
    {
        fixed ( GLuint* p = &indices[ 0 ] )
        {
            _glUniformSubroutinesuiv( shadertype, indices.Length, p );
        }
    }

    // ========================================================================

    public void glGetUniformSubroutineuiv( GLenum shadertype, GLint location, GLuint* parameters )
    {
        _glGetUniformSubroutineuiv( shadertype, location, parameters );
    }

    public void glGetUniformSubroutineuiv( GLenum shadertype, GLint location, ref GLuint[] parameters )
    {
        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetUniformSubroutineuiv( shadertype, location, p );
        }
    }

    // ========================================================================

    public void glGetProgramStageiv( GLuint program, GLenum shadertype, GLenum pname, GLint* values )
    {
        _glGetProgramStageiv( program, shadertype, pname, values );
    }

    public void glGetProgramStageiv( GLuint program, GLenum shadertype, GLenum pname, ref GLint[] values )
    {
        fixed ( GLint* p = &values[ 0 ] )
        {
            _glGetProgramStageiv( program, shadertype, pname, p );
        }
    }

    // ========================================================================

    public void glPatchParameteri( GLenum pname, GLint value )
    {
        _glPatchParameteri( pname, value );
    }

    // ========================================================================

    public void glPatchParameterfv( GLenum pname, GLfloat* values )
    {
        _glPatchParameterfv( pname, values );
    }

    public void glPatchParameterfv( GLenum pname, GLfloat[] values )
    {
        fixed ( GLfloat* p = &values[ 0 ] )
        {
            _glPatchParameterfv( pname, p );
        }
    }

    // ========================================================================

    public void glBindTransformFeedback( GLenum target, GLuint id )
    {
        _glBindTransformFeedback( target, id );
    }

    // ========================================================================

    public void glDeleteTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glDeleteTransformFeedbacks( n, ids );
    }

    public void glDeleteTransformFeedbacks( params GLuint[] ids )
    {
        fixed ( GLuint* p = &ids[ 0 ] )
        {
            _glDeleteTransformFeedbacks( ids.Length, p );
        }
    }

    // ========================================================================

    public void glGenTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glGenTransformFeedbacks( n, ids );
    }

    public GLuint[] glGenTransformFeedbacks( GLsizei n )
    {
        var r = new GLuint[ n ];

        fixed ( GLuint* p = &r[ 0 ] )
        {
            _glGenTransformFeedbacks( n, p );
        }

        return r;
    }

    public GLuint glGenTransformFeedback()
    {
        return glGenTransformFeedbacks( 1 )[ 0 ];
    }

    // ========================================================================

    public GLboolean glIsTransformFeedback( GLuint id )
    {
        return _glIsTransformFeedback( id );
    }

    // ========================================================================

    public void glPauseTransformFeedback()
    {
        _glPauseTransformFeedback();
    }

    // ========================================================================

    public void glResumeTransformFeedback()
    {
        _glResumeTransformFeedback();
    }

    // ========================================================================

    public void glDrawTransformFeedback( GLenum mode, GLuint id )
    {
        _glDrawTransformFeedback( mode, id );
    }

    // ========================================================================

    public void glDrawTransformFeedbackStream( GLenum mode, GLuint id, GLuint stream )
    {
        _glDrawTransformFeedbackStream( mode, id, stream );
    }

    // ========================================================================

    public void glBeginQueryIndexed( GLenum target, GLuint index, GLuint id )
    {
        _glBeginQueryIndexed( target, index, id );
    }

    // ========================================================================

    public void glEndQueryIndexed( GLenum target, GLuint index )
    {
        _glEndQueryIndexed( target, index );
    }

    // ========================================================================

    public void glGetQueryIndexediv( GLenum target, GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetQueryIndexediv( target, index, pname, parameters );
    }

    public void glGetQueryIndexediv( GLenum target, GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p =
                   &parameters[ 0 ] )
        {
            _glGetQueryIndexediv( target, index, pname, p );
        }
    }

    // ========================================================================

    public void glReleaseShaderCompiler()
    {
        _glReleaseShaderCompiler();
    }

    // ========================================================================

    public void glShaderBinary( GLsizei count, GLuint* shaders, GLenum binaryformat, void* binary, GLsizei length )
    {
        _glShaderBinary( count, shaders, binaryformat, binary, length );
    }

    public void glShaderBinary( GLuint[] shaders, GLenum binaryformat, byte[] binary )
    {
        var count = shaders.Length;

        fixed ( GLuint* pShaders = &shaders[ 0 ] )
        fixed ( byte* pBinary = &binary[ 0 ] )
        {
            _glShaderBinary( count, pShaders, binaryformat, pBinary, binary.Length );
        }
    }

    // ========================================================================

    public void glGetShaderPrecisionFormat( GLenum shaderType, GLenum precisionType, GLint* range, GLint* precision )
    {
        _glGetShaderPrecisionFormat( shaderType, precisionType, range, precision );
    }

    public void glGetShaderPrecisionFormat( GLenum shaderType, GLenum precisionType, ref GLint[] range, ref GLint precision )
    {
        range = new GLint[ 2 ];

        fixed ( GLint* pRange = &range[ 0 ] )
        fixed ( GLint* pPrecision = &precision )
        {
            _glGetShaderPrecisionFormat( shaderType, precisionType, pRange, pPrecision );
        }
    }

    // ========================================================================

    public void glDepthRangef( GLfloat n, GLfloat f )
    {
        _glDepthRangef( n, f );
    }

    // ========================================================================

    public void glClearDepthf( GLfloat d )
    {
        _glClearDepthf( d );
    }

    // ========================================================================

    public void glGetProgramBinary( GLuint program, GLsizei bufSize, GLsizei* length, GLenum* binaryFormat, void* binary )
    {
        _glGetProgramBinary( program, bufSize, length, binaryFormat, binary );
    }

    public byte[] glGetProgramBinary( GLuint program, GLsizei bufSize, out GLenum binaryFormat )
    {
        var     binary = new byte[ bufSize ];
        GLsizei length;

        fixed ( byte* pBinary = &binary[ 0 ] )
        fixed ( GLenum* pBinaryFormat = &binaryFormat )
        {
            _glGetProgramBinary( program, bufSize, &length, pBinaryFormat, pBinary );
        }

        Array.Resize( ref binary, length );

        return binary;
    }

    // ========================================================================

    public void glProgramBinary( GLuint program, GLenum binaryFormat, void* binary, GLsizei length )
    {
        _glProgramBinary( program, binaryFormat, binary, length );
    }

    public void glProgramBinary( GLuint program, GLenum binaryFormat, byte[] binary )
    {
        fixed ( byte* pBinary = &binary[ 0 ] )
        {
            _glProgramBinary( program, binaryFormat, pBinary, binary.Length );
        }
    }

    // ========================================================================

    public void glProgramParameteri( GLuint program, GLenum pname, GLint value )
    {
        _glProgramParameteri( program, pname, value );
    }

// ========================================================================

    public void glUseProgramStages( GLuint pipeline, GLbitfield stages, GLuint program )
    {
        _glUseProgramStages( pipeline, stages, program );
    }

// ========================================================================

    public void glActiveShaderProgram( GLuint pipeline, GLuint program )
    {
        _glActiveShaderProgram( pipeline, program );
    }

// ========================================================================

    public GLuint glCreateShaderProgramv( GLenum type, GLsizei count, GLchar** strings )
    {
        return _glCreateShaderProgramv( type, count, strings );
    }

// ========================================================================

    public GLuint glCreateShaderProgramv( GLenum type, string[] strings )
    {
        var stringsBytes = new GLchar[ strings.Length ][];

        for ( var i = 0; i < strings.Length; i++ )
        {
            stringsBytes[ i ] = Encoding.UTF8.GetBytes( strings[ i ] );
        }

        var stringsPtrs = new GLchar*[ strings.Length ];

        for ( var i = 0; i < strings.Length; i++ )
        {
            fixed ( GLchar* pString = &stringsBytes[ i ][ 0 ] )
            {
                stringsPtrs[ i ] = pString;
            }
        }

        fixed ( GLchar** pStrings = &stringsPtrs[ 0 ] )
        {
            return _glCreateShaderProgramv( type, strings.Length, pStrings );
        }
    }

// ========================================================================

    public void glBindProgramPipeline( GLuint pipeline )
    {
        _glBindProgramPipeline( pipeline );
    }

// ========================================================================

    public void glDeleteProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glDeleteProgramPipelines( n, pipelines );
    }

// ========================================================================

    public void glDeleteProgramPipelines( params GLuint[] pipelines )
    {
        fixed ( GLuint* pPipelines =
                   &pipelines[ 0 ] )
        {
            _glDeleteProgramPipelines( pipelines.Length, pPipelines );
        }
    }

// ========================================================================

    public void glGenProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glGenProgramPipelines( n, pipelines );
    }

// ========================================================================

    public GLuint glGenProgramPipeline()
    {
        return glGenProgramPipelines( 1 )[ 0 ];
    }

// ========================================================================

    public GLboolean glIsProgramPipeline( GLuint pipeline )
    {
        return _glIsProgramPipeline( pipeline );
    }

// ========================================================================

    public void glGetProgramPipelineiv( GLuint pipeline, GLenum pname, GLint* param )
    {
        _glGetProgramPipelineiv( pipeline, pname, param );
    }

// ========================================================================

    public void glGetProgramPipelineiv( GLuint pipeline, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* pParam =
                   &param[ 0 ] )
        {
            _glGetProgramPipelineiv( pipeline, pname, pParam );
        }
    }

// ========================================================================

    public void glProgramUniform1i( GLuint program, GLint location, GLint v0 )
    {
        _glProgramUniform1i( program, location, v0 );
    }

// ========================================================================

    public void glProgramUniform1iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform1iv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform1iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1iv( program, location, value.Length, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform1f( GLuint program, GLint location, GLfloat v0 )
    {
        _glProgramUniform1f( program, location, v0 );
    }

// ========================================================================

    public void glProgramUniform1fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform1fv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform1fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1fv( program, location, value.Length, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform1d( GLuint program, GLint location, GLdouble v0 )
    {
        _glProgramUniform1d( program, location, v0 );
    }

// ========================================================================

    public void glProgramUniform1dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform1dv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform1dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1dv( program, location, value.Length, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform1ui( GLuint program, GLint location, GLuint v0 )
    {
        _glProgramUniform1ui( program, location, v0 );
    }

// ========================================================================

    public void glProgramUniform1uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform1uiv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform1uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1uiv( program, location, value.Length, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform2i( GLuint program, GLint location, GLint v0, GLint v1 )
    {
        _glProgramUniform2i( program, location, v0, v1 );
    }

// ========================================================================

    public void glProgramUniform2iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform2iv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform2iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2iv( program, location, value.Length / 2, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform2f( GLuint program, GLint location, GLfloat v0, GLfloat v1 )
    {
        _glProgramUniform2f( program, location, v0, v1 );
    }

// ========================================================================

    public void glProgramUniform2fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform2fv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform2fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2fv( program, location, value.Length / 2, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform2d( GLuint program, GLint location, GLdouble v0, GLdouble v1 )
    {
        _glProgramUniform2d( program, location, v0, v1 );
    }

// ========================================================================

    public void glProgramUniform2dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform2dv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform2dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2dv( program, location, value.Length / 2, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform2ui( GLuint program, GLint location, GLuint v0, GLuint v1 )
    {
        _glProgramUniform2ui( program, location, v0, v1 );
    }

// ========================================================================

    public void glProgramUniform2uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform2uiv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform2uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2uiv( program, location, value.Length / 2, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform3i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glProgramUniform3i( program, location, v0, v1, v2 );
    }

// ========================================================================

    public void glProgramUniform3iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform3iv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform3iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3iv( program, location, value.Length / 3, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform3f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        _glProgramUniform3f( program, location, v0, v1, v2 );
    }

// ========================================================================

    public void glProgramUniform3fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform3fv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform3fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3fv( program, location, value.Length / 3, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform3d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2 )
    {
        _glProgramUniform3d( program, location, v0, v1, v2 );
    }

// ========================================================================

    public void glProgramUniform3dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform3dv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform3dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3dv( program, location, value.Length / 3, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform3ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2 )
    {
        _glProgramUniform3ui( program, location, v0, v1, v2 );
    }

// ========================================================================

    public void glProgramUniform3uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform3uiv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform3uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3uiv( program, location, value.Length / 3, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform4i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glProgramUniform4i( program, location, v0, v1, v2, v3 );
    }

// ========================================================================

    public void glProgramUniform4iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform4iv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform4iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4iv( program, location, value.Length / 4, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform4f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        _glProgramUniform4f( program, location, v0, v1, v2, v3 );
    }

// ========================================================================

    public void glProgramUniform4fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform4fv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform4fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4fv( program, location, value.Length / 4, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform4d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2, GLdouble v3 )
    {
        _glProgramUniform4d( program, location, v0, v1, v2, v3 );
    }

// ========================================================================

    public void glProgramUniform4dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform4dv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform4dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4dv( program, location, value.Length / 4, pValue );
        }
    }

// ========================================================================

    public void glProgramUniform4ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 )
    {
        _glProgramUniform4ui( program, location, v0, v1, v2, v3 );
    }

// ========================================================================

    public void glProgramUniform4uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform4uiv( program, location, count, value );
    }

// ========================================================================

    public void glProgramUniform4uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4uiv( program, location, value.Length / 4, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2fv( program, location, value.Length / 4, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3fv( program, location, value.Length / 9, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix4fv( program, location, value.Length / 16, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2dv( program, location, value.Length / 4, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3dv( program, location, value.Length / 9, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4dv( program, location, value.Length / 16, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix2x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x3fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix2x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix3x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x2fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix3x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix2x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x4fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix2x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix4x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x2fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix4x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix3x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x4fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix3x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix4x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x3fv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix4x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix2x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x3dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix2x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix3x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x2dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix3x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix2x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x4dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix2x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix4x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x2dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix4x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix3x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x4dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix3x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ========================================================================

    public void glProgramUniformMatrix4x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x3dv( program, location, count, transpose, value );
    }

// ========================================================================

    public void glProgramUniformMatrix4x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ========================================================================

    public void glValidateProgramPipeline( GLuint pipeline )
    {
        _glValidateProgramPipeline( pipeline );
    }

// ========================================================================

    public void glGetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetProgramPipelineInfoLog( pipeline, bufSize, length, infoLog );
    }

// ========================================================================

    public string glGetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize )
    {
        var     infoLog = new GLchar[ bufSize ];
        GLsizei length;

        fixed ( GLchar* pInfoLog = &infoLog[ 0 ] )
        {
            _glGetProgramPipelineInfoLog( pipeline, bufSize, &length, pInfoLog );

            return new string( ( sbyte* )pInfoLog, 0, length, Encoding.UTF8 );
        }
    }

// ========================================================================

    public void glVertexAttribL1d( GLuint index, GLdouble x )
    {
        _glVertexAttribL1d( index, x );
    }

// ========================================================================

    public void glVertexAttribL2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttribL2d( index, x, y );
    }

// ========================================================================

    public void glVertexAttribL3d( GLuint index, GLdouble x, GLdouble y, GLdouble z )
    {
        _glVertexAttribL3d( index, x, y, z );
    }

// ========================================================================

    public void glVertexAttribL4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glVertexAttribL4d( index, x, y, z, w );
    }

// ========================================================================

    public void glVertexAttribL1dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL1dv( index, v );
    }

// ========================================================================

    public void glVertexAttribL1dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL1dv( index, pV );
        }
    }

// ========================================================================

    public void glVertexAttribL2dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL2dv( index, v );
    }

// ========================================================================

    public void glVertexAttribL2dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL2dv( index, pV );
        }
    }

// ========================================================================

    public void glVertexAttribL3dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL3dv( index, v );
    }

// ========================================================================

    public void glVertexAttribL3dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL3dv( index, pV );
        }
    }

// ========================================================================

    public void glVertexAttribL4dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL4dv( index, v );
    }

// ========================================================================

    public void glVertexAttribL4dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL4dv( index, pV );
        }
    }

// ========================================================================

    public void glVertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, pointer );
    }

// ========================================================================

    public void glVertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, GLsizei pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, ( void* )pointer );
    }

// ========================================================================

    public void glGetVertexAttribLdv( GLuint index, GLenum pname, GLdouble* parameters )
    {
        _glGetVertexAttribLdv( index, pname, parameters );
    }

// ========================================================================

    public void glGetVertexAttribLdv( GLuint index, GLenum pname, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* pP = &parameters[ 0 ] )
        {
            _glGetVertexAttribLdv( index, pname, pP );
        }
    }

// ========================================================================

    public void glViewportArrayv( GLuint first, GLsizei count, GLfloat* v )
    {
        _glViewportArrayv( first, count, v );
    }

// ========================================================================

    public void glViewportArrayv( GLuint first, GLsizei count, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportArrayv( first, count, pV );
        }
    }

// ========================================================================

    public void glViewportIndexedf( GLuint index, GLfloat x, GLfloat y, GLfloat w, GLfloat h )
    {
        _glViewportIndexedf( index, x, y, w, h );
    }

// ========================================================================

    public void glViewportIndexedfv( GLuint index, GLfloat* v )
    {
        _glViewportIndexedfv( index, v );
    }

// ========================================================================

    public void glViewportIndexedfv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportIndexedfv( index, pV );
        }
    }

// ========================================================================

    public void glScissorArrayv( GLuint first, GLsizei count, GLint* v )
    {
        _glScissorArrayv( first, count, v );
    }

// ========================================================================

    public void glScissorArrayv( GLuint first, GLsizei count, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorArrayv( first, count, pV );
        }
    }

// ========================================================================

    public void glScissorIndexed( GLuint index, GLint left, GLint bottom, GLsizei width, GLsizei height )
    {
        _glScissorIndexed( index, left, bottom, width, height );
    }

// ========================================================================

    public void glScissorIndexedv( GLuint index, GLint* v )
    {
        _glScissorIndexedv( index, v );
    }

// ========================================================================

    public void glScissorIndexedv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorIndexedv( index, pV );
        }
    }

// ========================================================================

    public void glDepthRangeArrayv( GLuint first, GLsizei count, GLdouble* v )
    {
        _glDepthRangeArrayv( first, count, v );
    }

// ========================================================================

    public void glDepthRangeArrayv( GLuint first, GLsizei count, params GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glDepthRangeArrayv( first, count, pV );
        }
    }

// ========================================================================

    public void glDepthRangeIndexed( GLuint index, GLdouble n, GLdouble f )
    {
        _glDepthRangeIndexed( index, n, f );
    }

// ========================================================================

    public void glGetFloati_v( GLenum target, GLuint index, GLfloat* data )
    {
        _glGetFloati_v( target, index, data );
    }

// ========================================================================

    public void glGetFloati_v( GLenum target, GLuint index, ref GLfloat[] data )
    {
        fixed ( GLfloat* pData = &data[ 0 ] )
        {
            _glGetFloati_v( target, index, pData );
        }
    }

// ========================================================================

    public void glGetDoublei_v( GLenum target, GLuint index, GLdouble* data )
    {
        _glGetDoublei_v( target, index, data );
    }

// ========================================================================

    public void glGetDoublei_v( GLenum target, GLuint index, ref GLdouble[] data )
    {
        fixed ( GLdouble* pData = &data[ 0 ] )
        {
            _glGetDoublei_v( target, index, pData );
        }
    }

// ========================================================================

    public void glDrawArraysInstancedBaseInstance( GLenum mode, GLint first, GLsizei count, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawArraysInstancedBaseInstance( mode, first, count, instancecount, baseinstance );
    }

// ========================================================================

    public void glDrawElementsInstancedBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
    }

// ========================================================================

    public void glDrawElementsInstancedBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLuint baseinstance ) where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseInstance( mode, count, type, p, instancecount, baseinstance );
        }
    }

// ========================================================================

    public void glDrawElementsInstancedBaseVertexBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLint basevertex, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
    }

// ========================================================================

    public void glDrawElementsInstancedBaseVertexBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLint basevertex, GLuint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, p, instancecount, basevertex, baseinstance );
        }
    }

// ========================================================================

    public void glGetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, GLint* parameters )
    {
        _glGetInternalformativ( target, internalformat, pname, bufSize, parameters );
    }

// ========================================================================

    public void glGetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetInternalformativ( target, internalformat, pname, bufSize, p );
        }
    }

// ========================================================================

    public void glGetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, GLint* parameters )
    {
        _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, parameters );
    }

// ========================================================================

    public void glGetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, p );
        }
    }

// ========================================================================

    public void glBindImageTexture( GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum access, GLenum format )
    {
        _glBindImageTexture( unit, texture, level, layered, layer, access, format );
    }

// ========================================================================

    public void glMemoryBarrier( GLbitfield barriers )
    {
        _glMemoryBarrier( barriers );
    }

// ========================================================================

    public void glTexStorage1D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width )
    {
        _glTexStorage1D( target, levels, internalformat, width );
    }

// ========================================================================

    public void glTexStorage2D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glTexStorage2D( target, levels, internalformat, width, height );
    }

// ========================================================================

    public void glTexStorage3D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glTexStorage3D( target, levels, internalformat, width, height, depth );
    }

// ========================================================================

    public void glDrawTransformFeedbackInstanced( GLenum mode, GLuint id, GLsizei instancecount )
    {
        _glDrawTransformFeedbackInstanced( mode, id, instancecount );
    }

// ========================================================================

    public void glDrawTransformFeedbackStreamInstanced( GLenum mode, GLuint id, GLuint stream, GLsizei instancecount )
    {
        _glDrawTransformFeedbackStreamInstanced( mode, id, stream, instancecount );
    }

// ========================================================================

    public void glGetPointerv( GLenum pname, void** parameters )
    {
        _glGetPointerv( pname, parameters );
    }

// ========================================================================

    public void glGetPointerv( GLenum pname, ref IntPtr[] parameters )
    {
        var length = parameters.Length;

        var ptr = parameters[ 0 ].ToPointer();
        var p   = &ptr;
        _glGetPointerv( pname, p );

        for ( var i = 0; i < length; i++ )
        {
            parameters[ i ] = new IntPtr( *p );
            p++;
        }
    }

// ========================================================================

    public void glClearBufferData( GLenum target, GLenum internalformat, GLenum format, GLenum type, void* data )
    {
        _glClearBufferData( target, internalformat, format, type, data );
    }

// ========================================================================

    public void glClearBufferData< T >( GLenum target, GLenum internalformat, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferData( target, internalformat, format, type, t );
        }
    }

// ========================================================================

    public void glClearBufferSubData( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data )
    {
        _glClearBufferSubData( target, internalformat, offset, size, format, type, data );
    }

// ========================================================================

    public void glClearBufferSubData< T >( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferSubData( target, internalformat, offset, size, format, type, t );
        }
    }

// ========================================================================

    public void glDispatchCompute( GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z )
    {
        _glDispatchCompute( num_groups_x, num_groups_y, num_groups_z );
    }

// ========================================================================

    public void glDispatchComputeIndirect( void* indirect )
    {
        _glDispatchComputeIndirect( indirect );
    }

// ========================================================================

    public struct DispatchIndirectCommand
    {
        public uint num_groups_x;
        public uint num_groups_y;
        public uint num_groups_z;
    }

    public void glDispatchComputeIndirect( DispatchIndirectCommand indirect )
    {
        _glDispatchComputeIndirect( &indirect );
    }

// ========================================================================

    public void glCopyImageSubData( GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ, GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY,
                                    GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth )
    {
        _glCopyImageSubData( srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth );
    }

// ========================================================================

    public void glFramebufferParameteri( GLenum target, GLenum pname, GLint param )
    {
        _glFramebufferParameteri( target, pname, param );
    }

// ========================================================================

    public void glGetFramebufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetFramebufferParameteriv( target, pname, parameters );
    }

// ========================================================================

    public void glGetFramebufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p_parameters = &parameters[ 0 ] )
        {
            _glGetFramebufferParameteriv( target, pname, p_parameters );
        }
    }

// ========================================================================

    public void glGetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, GLint64* parameters )
    {
        _glGetInternalformati64v( target, internalformat, pname, count, parameters );
    }

// ========================================================================

    public void glGetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, ref GLint64[] parameters )
    {
        fixed ( GLint64* p_params = &parameters[ 0 ] )
        {
            _glGetInternalformati64v( target, internalformat, pname, count, p_params );
        }
    }

// ========================================================================

    public void glInvalidateTexSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glInvalidateTexSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth );
    }

// ========================================================================

    public void glInvalidateTexImage( GLuint texture, GLint level )
    {
        _glInvalidateTexImage( texture, level );
    }

// ========================================================================

    public void glInvalidateBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr length )
    {
        _glInvalidateBufferSubData( buffer, offset, length );
    }

// ========================================================================

    public void glInvalidateBufferData( GLuint buffer )
    {
        _glInvalidateBufferData( buffer );
    }

// ========================================================================

    public void glInvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments )
    {
        _glInvalidateFramebuffer( target, numAttachments, attachments );
    }

// ========================================================================

    public void glInvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments )
    {
        fixed ( GLenum* p_attachments = &attachments[ 0 ] )
        {
            _glInvalidateFramebuffer( target, numAttachments, p_attachments );
        }
    }

// ========================================================================

    public void glInvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glInvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
    }

// ========================================================================

    public void glInvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        fixed ( GLenum* p_attachments = &attachments[ 0 ] )
        {
            _glInvalidateSubFramebuffer( target, numAttachments, p_attachments, x, y, width, height );
        }
    }

// ========================================================================

    public void glMultiDrawArraysIndirect( GLenum mode, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, indirect, drawcount, stride );
    }

// ========================================================================

    public void glMultiDrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, ( void* )&indirect, drawcount, stride );
    }

// ========================================================================

    public void glMultiDrawElementsIndirect( GLenum mode, GLenum type, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
    }

// ========================================================================

    public void glMultiDrawElementsIndirect( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, ( void* )&indirect, drawcount, stride );
    }

// ========================================================================

    public void glGetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, GLint* parameters )
    {
        _glGetProgramInterfaceiv( program, programInterface, pname, parameters );
    }

// ========================================================================

    public void glGetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p_parameters = &parameters[ 0 ] )
        {
            _glGetProgramInterfaceiv( program, programInterface, pname, p_parameters );
        }
    }

// ========================================================================

    public GLuint glGetProgramResourceIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceIndex( program, programInterface, name );
    }

// ========================================================================

    public GLuint glGetProgramResourceIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceIndex( program, programInterface, p_name );
        }
    }

// ========================================================================

    public void glGetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei* length, GLchar* name )
    {
        _glGetProgramResourceName( program, programInterface, index, bufSize, length, name );
    }

// ========================================================================

    public string glGetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize )
    {
        var     name = new GLchar[ bufSize ];
        GLsizei length;

        fixed ( GLchar* p_name = &name[ 0 ] )
        {
            _glGetProgramResourceName( program, programInterface, index, bufSize, &length, p_name );

            return new string( ( sbyte* )p_name, 0, length, Encoding.UTF8 );
        }
    }

// ========================================================================

    public void glGetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLsizei propCount, GLenum* props, GLsizei bufSize, GLsizei* length, GLint* parameters )
    {
        _glGetProgramResourceiv( program, programInterface, index, propCount, props, bufSize, length, parameters );
    }

// ========================================================================

    public void glGetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLenum[] props, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLenum* p_props = &props[ 0 ] )
        fixed ( GLint* p_params = &parameters[ 0 ] )
        {
            GLsizei length;
            _glGetProgramResourceiv( program, programInterface, index, props.Length, p_props, bufSize, &length, p_params );
        }
    }

// ========================================================================

    public GLint glGetProgramResourceLocation( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocation( program, programInterface, name );
    }

// ========================================================================

    public GLint glGetProgramResourceLocation( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocation( program, programInterface, p_name );
        }
    }

// ========================================================================

    public GLint glGetProgramResourceLocationIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocationIndex( program, programInterface, name );
    }

// ========================================================================

    public GLint glGetProgramResourceLocationIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocationIndex( program, programInterface, p_name );
        }
    }

// ========================================================================

    public void glShaderStorageBlockBinding( GLuint program, GLuint storageBlockIndex, GLuint storageBlockBinding )
    {
        _glShaderStorageBlockBinding( program, storageBlockIndex, storageBlockBinding );
    }

// ========================================================================

    public void glTexBufferRange( GLenum target, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTexBufferRange( target, internalformat, buffer, offset, size );
    }

// ========================================================================

    public void glTexStorage2DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations )
    {
        _glTexStorage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
    }

// ========================================================================

    public void glTexStorage3DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedsamplelocations )
    {
        _glTexStorage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

// ========================================================================

    public void glTextureView( GLuint texture, GLenum target, GLuint origtexture, GLenum internalformat, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers )
    {
        _glTextureView( texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers );
    }

// ========================================================================

    public void glBindVertexBuffer( GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )
    {
        _glBindVertexBuffer( bindingindex, buffer, offset, stride );
    }

// ========================================================================

    public void glVertexAttribFormat( GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset )
    {
        _glVertexAttribFormat( attribindex, size, type, normalized, relativeoffset );
    }

// ========================================================================

    public void glVertexAttribIFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribIFormat( attribindex, size, type, relativeoffset );
    }

// ========================================================================

    public void glVertexAttribLFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribLFormat( attribindex, size, type, relativeoffset );
    }

// ========================================================================

    public void glVertexAttribBinding( GLuint attribindex, GLuint bindingindex )
    {
        _glVertexAttribBinding( attribindex, bindingindex );
    }

    // ========================================================================

    public void glVertexBindingDivisor( GLuint bindingindex, GLuint divisor )
    {
        _glVertexBindingDivisor( bindingindex, divisor );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexBindingDivisor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexBindingDivisor( GLuint bindingindex, GLuint divisor );

    // ========================================================================

    public void glDebugMessageControl( GLenum source, GLenum type, GLenum severity, GLsizei count, GLuint* ids, GLboolean enabled )
    {
        _glDebugMessageControl( source, type, severity, count, ids, enabled );
    }

    public void glDebugMessageControl( GLenum source, GLenum type, GLenum severity, GLuint[] ids, GLboolean enabled )
    {
        fixed ( GLuint* p_ids = ids )
        {
            _glDebugMessageControl( source, type, severity, ids.Length, p_ids, enabled );
        }
    }

    // ========================================================================

    public void glDebugMessageInsert( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* buf )
    {
        _glDebugMessageInsert( source, type, id, severity, length, buf );
    }

    public void glDebugMessageInsert( GLenum source, GLenum type, GLuint id, GLenum severity, string buf )
    {
        var bufBytes = Encoding.UTF8.GetBytes( buf );

        fixed ( GLchar* p_bufBytes = bufBytes )
        {
            _glDebugMessageInsert( source, type, id, severity, bufBytes.Length, p_bufBytes );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDebugMessageInsert", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDebugMessageInsert( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* buf );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLDEBUGPROC( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* message, void* userParam );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLDEBUGPROCSAFE( GLenum source, GLenum type, GLuint id, GLenum severity, string message, void* userParam );

    public void glDebugMessageCallback( GLDEBUGPROC callback, void* userParam )
    {
        _glDebugMessageCallback( callback, userParam );
    }

    public void glDebugMessageCallback( GLDEBUGPROCSAFE callback, void* userParam )
    {
        _glDebugMessageCallback( CallbackUnsafe, userParam );

        return;

        void CallbackUnsafe( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* message, void* param )
        {
            var messageString = new string( ( sbyte* )message, 0, length, Encoding.UTF8 );
            callback( source, type, id, severity, messageString, param );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDebugMessageCallback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDebugMessageCallback( GLDEBUGPROC callback, void* userParam );

    // ========================================================================

    public GLuint glGetDebugMessageLog( GLuint count,
                                        GLsizei bufsize,
                                        GLenum* sources,
                                        GLenum* types,
                                        GLuint* ids,
                                        GLenum* severities,
                                        GLsizei* lengths,
                                        GLchar* messageLog )
    {
        return _glGetDebugMessageLog( count, bufsize, sources, types, ids, severities, lengths, messageLog );
    }

    public GLuint glGetDebugMessageLog( GLuint count,
                                        GLsizei bufSize,
                                        out GLenum[] sources,
                                        out GLenum[] types,
                                        out GLuint[] ids,
                                        out GLenum[] severities,
                                        out string[] messageLog )
    {
        sources    = new GLenum[ count ];
        types      = new GLenum[ count ];
        ids        = new GLuint[ count ];
        severities = new GLenum[ count ];

        var messageLogBytes = new GLchar[ bufSize ];
        var lengths         = new GLsizei[ count ];

        fixed ( GLenum* p_sources = &sources[ 0 ] )
        fixed ( GLenum* p_types = &types[ 0 ] )
        fixed ( GLuint* p_ids = &ids[ 0 ] )
        fixed ( GLenum* p_severities = &severities[ 0 ] )
        fixed ( GLsizei* p_lengths = &lengths[ 0 ] )
        fixed ( GLchar* p_messageLogBytes = &messageLogBytes[ 0 ] )
        {
            var    pstart = p_messageLogBytes;
            GLuint ret    = _glGetDebugMessageLog( count, bufSize, p_sources, p_types, p_ids, p_severities, p_lengths, p_messageLogBytes );
            messageLog = new string[ count ];

            for ( var i = 0; i < count; i++ )
            {
                messageLog[ i ] =  new string( ( sbyte* )pstart, 0, lengths[ i ], Encoding.UTF8 );
                pstart          += lengths[ i ];
            }

            Array.Resize( ref sources, ( int )ret );
            Array.Resize( ref types, ( int )ret );
            Array.Resize( ref ids, ( int )ret );
            Array.Resize( ref severities, ( int )ret );
            Array.Resize( ref messageLog, ( int )ret );

            return ret;
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetDebugMessageLog", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glGetDebugMessageLog( GLuint count,
                                                        GLsizei bufsize,
                                                        GLenum* sources,
                                                        GLenum* types,
                                                        GLuint* ids,
                                                        GLenum* severities,
                                                        GLsizei* lengths,
                                                        GLchar* messageLog );

    // ========================================================================

    public void glPushDebugGroup( GLenum source, GLuint id, GLsizei length, GLchar* message )
    {
        _glPushDebugGroup( source, id, length, message );
    }

    public void glPushDebugGroup( GLenum source, GLuint id, string message )
    {
        var messageBytes = Encoding.UTF8.GetBytes( message );

        fixed ( GLchar* p_messageBytes = messageBytes )
        {
            _glPushDebugGroup( source, id, messageBytes.Length, p_messageBytes );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glPushDebugGroup", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPushDebugGroup( GLenum source, GLuint id, GLsizei length, GLchar* message );

    // ========================================================================

    public void glPopDebugGroup()
    {
        _glPopDebugGroup();
    }

    [DllImport( LIBGL, EntryPoint = "glPopDebugGroup", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPopDebugGroup();
}

#pragma warning restore IDE0079    // Remove unnecessary suppression
#pragma warning restore CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore CS8603     // Possible null reference return.
#pragma warning restore IDE0060    // Remove unused parameter.
#pragma warning restore IDE1006    // Naming Styles.
#pragma warning restore IDE0090    // Use 'new(...)'.
#pragma warning restore CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning restore SYSLIB1054 // 