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

// ----------------------------------------------------------------------------

using Corelib.LibCore.Utils;

using System.Numerics;

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

// ----------------------------------------------------------------------------

namespace Corelib.LibCore.Graphics.OpenGL;

public unsafe partial class GLBindings : IGLBindings
{
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteProgram( GLuint program )
    {
        _glDeleteProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDeleteProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteProgram( GLuint program );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteShader( GLuint shader )
    {
        _glDeleteShader( shader );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDeleteShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteShader( GLuint shader );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDetachShader( GLuint program, GLuint shader )
    {
        _glDetachShader( program, shader );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDetachShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDetachShader( GLuint program, GLuint shader );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDisableVertexAttribArray( GLuint index )
    {
        _glDisableVertexAttribArray( index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDisableVertexAttribArray", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDisableVertexAttribArray( GLuint index );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEnableVertexAttribArray( GLuint index )
    {
        _glEnableVertexAttribArray( index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glEnableVertexAttribArray", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEnableVertexAttribArray( GLuint index );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveAttrib( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name )
    {
        _glGetActiveAttrib( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc/>
    public string glGetActiveAttrib( GLuint program, GLuint index, GLsizei bufSize, out GLint size, out GLenum type )
    {
        var     name = stackalloc GLchar[ bufSize ];
        GLsizei len;

        fixed ( GLenum* ptype = &type )
        {
            fixed ( GLint* psize = &size )
            {
                _glGetActiveAttrib( program, index, bufSize, &len, psize, ptype, name );
            }
        }

        return new string( ( sbyte* )name, 0, len, Encoding.UTF8 );
    }

    [DllImport( LIBGL, EntryPoint = "glGetActiveAttrib", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetActiveAttrib( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveUniform( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name )
    {
        _glGetActiveUniform( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc/>
    public string glGetActiveUniform( GLuint program, GLuint index, GLsizei bufSize, out GLint size, out GLenum type )
    {
        var     name = stackalloc GLchar[ bufSize ];
        GLsizei len;

        fixed ( GLenum* ptype = &type )
        {
            fixed ( GLint* psize = &size )
            {
                _glGetActiveUniform( program, index, bufSize, &len, psize, ptype, name );
            }
        }

        return new string( ( sbyte* )name, 0, len, Encoding.UTF8 );
    }

    [DllImport( LIBGL, EntryPoint = "glGetActiveUniform", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetActiveUniform( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetAttachedShaders( GLuint program, GLsizei maxCount, GLsizei* count, GLuint* shaders )
    {
        _glGetAttachedShaders( program, maxCount, count, shaders );
    }

    /// <inheritdoc/>
    public GLuint[] glGetAttachedShaders( GLuint program, GLsizei maxCount )
    {
        var     shaders = new GLuint[ maxCount ];
        GLsizei count;

        fixed ( GLuint* pshaders = &shaders[ 0 ] )
        {
            _glGetAttachedShaders( program, maxCount, &count, pshaders );
        }

        Array.Resize( ref shaders, count );

        return shaders;
    }

    [DllImport( LIBGL, EntryPoint = "glGetAttachedShaders", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint[] _glGetAttachedShaders( GLuint program, GLsizei maxCount, GLsizei* count, GLuint* shaders );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetAttribLocation( GLuint program, GLchar* name )
    {
        return _glGetAttribLocation( program, name );
    }

    /// <inheritdoc/>
    public GLint glGetAttribLocation( GLuint program, string name )
    {
        fixed ( GLchar* pname = Encoding.UTF8.GetBytes( name ) )
        {
            return _glGetAttribLocation( program, pname );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetAttribLocation", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetAttribLocation( GLuint program, GLchar* name );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramiv( GLuint program, GLenum pname, GLint* parameters )
    {
        _glGetProgramiv( program, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetProgramiv( GLuint program, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetProgramiv( program, pname, pparams );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetProgramiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramiv( GLuint program, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramInfoLog( GLuint program, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetProgramInfoLog( program, bufSize, length, infoLog );
    }

    /// <inheritdoc/>
    public string glGetProgramInfoLog( GLuint program, GLsizei bufSize )
    {
        var     infoLog = stackalloc GLchar[ bufSize ];
        GLsizei len;
        _glGetProgramInfoLog( program, bufSize, &len, infoLog );

        return new string( ( sbyte* )infoLog, 0, len, Encoding.UTF8 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetProgramInfoLog", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramInfoLog( GLuint program, GLsizei bufSize, GLsizei* length, GLchar* infoLog );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderiv( GLuint shader, GLenum pname, GLint* parameters )
    {
        _glGetShaderiv( shader, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetShaderiv( GLuint shader, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetShaderiv( shader, pname, pparams );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetShaderiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetShaderiv( GLuint shader, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderInfoLog( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetShaderInfoLog( shader, bufSize, length, infoLog );
    }

    /// <inheritdoc/>
    public string glGetShaderInfoLog( GLuint shader, GLsizei bufSize )
    {
        var     infoLog = stackalloc GLchar[ bufSize ];
        GLsizei len;
        _glGetShaderInfoLog( shader, bufSize, &len, infoLog );

        return new string( ( sbyte* )infoLog, 0, len, Encoding.UTF8 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetShaderInfoLog", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetShaderInfoLog( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* infoLog );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderSource( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* source )
    {
        _glGetShaderSource( shader, bufSize, length, source );
    }

    /// <inheritdoc/>
    public string glGetShaderSource( GLuint shader, GLsizei bufSize = 4096 )
    {
        var     source = stackalloc GLchar[ bufSize ];
        GLsizei len;
        _glGetShaderSource( shader, bufSize, &len, source );

        return new string( ( sbyte* )source, 0, len, Encoding.UTF8 );
    }

    [DllImport( LIBGL, EntryPoint = "glGetShaderSource", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetShaderSource( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* source );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetUniformLocation( GLuint program, GLchar* name )
    {
        return _glGetUniformLocation( program, name );
    }

    /// <inheritdoc/>
    public GLint glGetUniformLocation( GLuint program, string name )
    {
        fixed ( byte* pname = Encoding.UTF8.GetBytes( name ) )
        {
            return _glGetUniformLocation( program, pname );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetUniformLocation", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetUniformLocation( GLuint program, GLchar* name );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformfv( GLuint program, GLint location, GLfloat* parameters )
    {
        _glGetUniformfv( program, location, parameters );
    }

    /// <inheritdoc/>
    public void glGetUniformfv( GLuint program, GLint location, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* pparams =
                   &parameters[ 0 ] )
        {
            _glGetUniformfv( program, location, pparams );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetUniformfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetUniformfv( GLuint program, GLint location, GLfloat* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformiv( GLuint program, GLint location, GLint* parameters )
    {
        _glGetUniformiv( program, location, parameters );
    }

    /// <inheritdoc/>
    public void glGetUniformiv( GLuint program, GLint location, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetUniformiv( program, location, pparams );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetUniformiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetUniformiv( GLuint program, GLint location, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribdv( GLuint index, GLenum pname, GLdouble* parameters )
    {
        _glGetVertexAttribdv( index, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetVertexAttribdv( GLuint index, GLenum pname, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribdv( index, pname, pparams );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetVertexAttribdv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetVertexAttribdv( GLuint index, GLenum pname, GLdouble* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribfv( GLuint index, GLenum pname, GLfloat* parameters )
    {
        _glGetVertexAttribfv( index, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetVertexAttribfv( GLuint index, GLenum pname, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribfv( index, pname, pparams );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetVertexAttribfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetVertexAttribfv( GLuint index, GLenum pname, GLfloat* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribiv( GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetVertexAttribiv( index, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetVertexAttribiv( GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribiv( index, pname, pparams );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetVertexAttribiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetVertexAttribiv( GLuint index, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribPointerv( GLuint index, GLenum pname, void** pointer )
    {
        _glGetVertexAttribPointerv( index, pname, pointer );
    }

    /// <inheritdoc/>
    public void glGetVertexAttribPointerv( GLuint index, GLenum pname, ref uint[] pointer )
    {
        var ptr = new void*[ pointer.Length ];

        fixed ( void** p = &ptr[ 0 ] )
        {
            _glGetVertexAttribPointerv( index, pname, p );
        }

        for ( var i = 0; i < pointer.Length; i++ )
        {
            pointer[ i ] = ( uint )ptr[ i ];
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetVertexAttribPointerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetVertexAttribPointerv( GLuint index, GLenum pname, void** pointer );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsProgram( GLuint program )
    {
        return _glIsProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glIsProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsProgram( GLuint program );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsShader( GLuint shader )
    {
        return _glIsShader( shader );
    }
    
    [DllImport( LIBGL, EntryPoint = "glIsShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsShader( GLuint shader );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glLinkProgram( GLuint program )
    {
        _glLinkProgram( program );
    }

    [DllImport( LIBGL, EntryPoint = "glLinkProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLinkProgram( GLuint program );
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glShaderSource( GLuint shader, GLsizei count, GLchar** str, GLint* length )
    {
        _glShaderSource( shader, count, str, length );
    }

    /// <inheritdoc/>
    public void glShaderSource( GLuint shader, params string[] @string )
    {
        var count   = @string.Length;
        var strings = new GLchar[ count ][];
        var lengths = new GLint[ count ];

        for ( var i = 0; i < count; i++ )
        {
            strings[ i ] = Encoding.UTF8.GetBytes( @string[ i ] );
            lengths[ i ] = @string[ i ].Length;
        }

        var pstring = stackalloc GLchar*[ count ];
        var length  = stackalloc GLint[ count ];

        for ( var i = 0; i < count; i++ )
        {
            fixed ( GLchar* p = &strings[ i ][ 0 ] )
            {
                pstring[ i ] = p;
            }

            length[ i ] = lengths[ i ];
        }

        _glShaderSource( shader, count, pstring, length );
    }

    [DllImport( LIBGL, EntryPoint = "glShaderSource", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glShaderSource( GLuint shader, GLsizei count, GLchar** str, GLint* length );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUseProgram( GLuint program )
    {
        _glUseProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUseProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUseProgram( GLuint program );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1f( GLint location, GLfloat v0 )
    {
        _glUniform1f( location, v0 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform1f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1f( GLint location, GLfloat v0 );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2f( GLint location, GLfloat v0, GLfloat v1 )
    {
        _glUniform2f( location, v0, v1 );
    }

    [DllImport( LIBGL, EntryPoint = "glUniform2f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2f( GLint location, GLfloat v0, GLfloat v1 );
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        _glUniform3f( location, v0, v1, v2 );
    }
    
    [DllImport(LIBGL, EntryPoint = "glUniform3f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2 );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        _glUniform4f( location, v0, v1, v2, v3 );
    }
    
    [DllImport(LIBGL, EntryPoint = "glUniform4f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1i( GLint location, GLint v0 )
    {
        _glUniform1i( location, v0 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform1i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1i( GLint location, GLint v0 );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2i( GLint location, GLint v0, GLint v1 )
    {
        _glUniform2i( location, v0, v1 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform2i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2i( GLint location, GLint v0, GLint v1 );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3i( GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glUniform3i( location, v0, v1, v2 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform3i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3i( GLint location, GLint v0, GLint v1, GLint v2 );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4i( GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glUniform4i( location, v0, v1, v2, v3 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform4i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4i( GLint location, GLint v0, GLint v1, GLint v2, GLint v3 );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform1fv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform1fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform1fv( location, value.Length, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform1fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1fv( GLint location, GLsizei count, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform2fv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform2fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform2fv( location, value.Length / 2, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2fv( GLint location, GLsizei count, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform3fv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform3fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform3fv( location, value.Length / 3, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3fv( GLint location, GLsizei count, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform4fv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform4fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform4fv( location, value.Length / 4, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4fv( GLint location, GLsizei count, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform1iv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform1iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform1iv( location, value.Length, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform1iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1iv( GLint location, GLsizei count, GLint* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform2iv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform2iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform2iv( location, value.Length / 2, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform2iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2iv( GLint location, GLsizei count, GLint* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform3iv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform3iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform3iv( location, value.Length / 3, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform3iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3iv( GLint location, GLsizei count, GLint* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform4iv( location, count, value );
    }

    /// <inheritdoc/>
    public void glUniform4iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform4iv( location, value.Length / 4, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform4iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4iv( GLint location, GLsizei count, GLint* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2fv( location, value.Length / 4, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix3fv( location, value.Length / 9, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4fv( location, value.Length / 16, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glValidateProgram( GLuint program )
    {
        return _glValidateProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glValidateProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern bool _glValidateProgram( GLuint program );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1d( GLuint index, GLdouble x )
    {
        _glVertexAttrib1d( index, x );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1d( GLuint index, GLdouble x );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib1dv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib1dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib1dv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1dv( GLuint index, GLdouble* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1f( GLuint index, GLfloat x )
    {
        _glVertexAttrib1f( index, x );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1f( GLuint index, GLfloat x );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib1fv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib1fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib1fv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1fv( GLuint index, GLfloat* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1s( GLuint index, GLshort x )
    {
        _glVertexAttrib1s( index, x );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1s( GLuint index, GLshort x );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib1sv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib1sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib1sv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1sv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1sv( GLuint index, GLshort* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttrib2d( index, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2d( GLuint index, GLdouble x, GLdouble y );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib2dv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib2dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib2dv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2dv( GLuint index, GLdouble* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2f( GLuint index, GLfloat x, GLfloat y )
    {
        _glVertexAttrib2f( index, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2f( GLuint index, GLfloat x, GLfloat y );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib2fv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib2fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib2fv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2s( GLuint index, GLshort x, GLshort y )
    {
        _glVertexAttrib2s( index, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2s( GLuint index, GLshort x, GLshort y );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib2sv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib2sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib2sv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3d( GLuint index, GLdouble x, GLdouble y, GLdouble z )
    {
        _glVertexAttrib3d( index, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3d( GLuint index, GLdouble x, GLdouble y, GLdouble z );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib3dv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib3dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib3dv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3f( GLuint index, GLfloat x, GLfloat y, GLfloat z )
    {
        _glVertexAttrib3f( index, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3f( GLuint index, GLfloat x, GLfloat y, GLfloat z );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib3fv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib3fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib3fv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3s( GLuint index, GLshort x, GLshort y, GLshort z )
    {
        _glVertexAttrib3s( index, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3s( GLuint index, GLshort x, GLshort y, GLshort z );
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib3sv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib3sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib3sv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Nbv( GLuint index, GLbyte* v )
    {
        _glVertexAttrib4Nbv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4Nbv( GLuint index, params GLbyte[] v )
    {
        fixed ( GLbyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nbv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Niv( GLuint index, GLint* v )
    {
        _glVertexAttrib4Niv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4Niv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttrib4Niv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Nsv( GLuint index, GLshort* v )
    {
        _glVertexAttrib4Nsv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4Nsv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nsv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Nub( GLuint index, GLubyte x, GLubyte y, GLubyte z, GLubyte w )
    {
        _glVertexAttrib4Nub( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Nub", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Nub( GLuint index, GLubyte x, GLubyte y, GLubyte z, GLubyte w );
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Nubv( GLuint index, GLubyte* v )
    {
        _glVertexAttrib4Nubv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4Nubv( GLuint index, params GLubyte[] v )
    {
        fixed ( GLubyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nubv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Nuiv( GLuint index, GLuint* v )
    {
        _glVertexAttrib4Nuiv( index, v );
    }


    /// <inheritdoc/>
    public void glVertexAttrib4Nuiv( GLuint index, params GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nuiv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Nusv( GLuint index, GLushort* v )
    {
        _glVertexAttrib4Nusv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4Nusv( GLuint index, params GLushort[] v )
    {
        fixed ( GLushort* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nusv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4bv( GLuint index, GLbyte* v )
    {
        _glVertexAttrib4bv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4bv( GLuint index, params GLbyte[] v )
    {
        fixed ( GLbyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4bv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glVertexAttrib4d( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib4dv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib4dv( index, p );
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4f( GLuint index, GLfloat x, GLfloat y, GLfloat z, GLfloat w )
    {
        _glVertexAttrib4f( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4f( GLuint index, GLfloat x, GLfloat y, GLfloat z, GLfloat w );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib4fv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib4fv( index, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4fv( GLuint index, GLfloat* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4iv( GLuint index, GLint* v )
    {
        _glVertexAttrib4iv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4iv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttrib4iv( index, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4iv( GLuint index, GLint* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4s( GLuint index, GLshort x, GLshort y, GLshort z, GLshort w )
    {
        _glVertexAttrib4s( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4s( GLuint index, GLshort x, GLshort y, GLshort z, GLshort w );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib4sv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib4sv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4sv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4sv( GLuint index, GLshort* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4ubv( GLuint index, GLubyte* v )
    {
        _glVertexAttrib4ubv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4ubv( GLuint index, params GLubyte[] v )
    {
        fixed ( GLubyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4ubv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4ubv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4ubv( GLuint index, GLubyte* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4uiv( GLuint index, GLuint* v )
    {
        _glVertexAttrib4uiv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4uiv( GLuint index, params GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttrib4uiv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4uiv( GLuint index, GLuint* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4usv( GLuint index, GLushort* v )
    {
        _glVertexAttrib4usv( index, v );
    }

    /// <inheritdoc/>
    public void glVertexAttrib4usv( GLuint index, params GLushort[] v )
    {
        fixed ( GLushort* p = &v[ 0 ] )
        {
            _glVertexAttrib4usv( index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4usv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4usv( GLuint index, GLushort* v );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribPointer( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, void* pointer )
    {
        _glVertexAttribPointer( index, size, type, normalized, stride, pointer );
    }

    /// <inheritdoc/>
    public void glVertexAttribPointer( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, uint pointer )
    {
        _glVertexAttribPointer( index, size, type, normalized, stride, ( void* )pointer );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribPointer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribPointer( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, void* pointer );
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2x3fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix2x3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2x3fv( location, value.Length / 6, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix2x3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix2x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3x2fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix3x2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix3x2fv( location, value.Length / 6, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix3x2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix3x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2x4fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix2x4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2x4fv( location, value.Length / 8, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix2x4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix2x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4x2fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix4x2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4x2fv( location, value.Length / 8, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix4x2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix4x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3x4fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix3x4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniformMatrix3x4fv( location, value.Length / 12, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix3x4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix3x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4x3fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void glUniformMatrix4x3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4x3fv( location, value.Length / 12, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix4x3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix4x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glColorMaski( GLuint index, GLboolean r, GLboolean g, GLboolean b, GLboolean a )
    {
        _glColorMaski( index, r, g, b, a );
    }
    
    [DllImport( LIBGL, EntryPoint = "glColorMaski", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glColorMaski( GLuint index, GLboolean r, GLboolean g, GLboolean b, GLboolean a );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBooleani_v( GLenum target, GLuint index, GLboolean* data )
    {
        _glGetBooleani_v( target, index, data );
    }

    /// <inheritdoc/>
    public void glGetBooleani_v( GLenum target, GLuint index, ref GLboolean[] data )
    {
        fixed ( GLboolean* p = &data[ 0 ] )
        {
            _glGetBooleani_v( target, index, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetBooleani_v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBooleani_v( GLenum target, GLuint index, GLboolean* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetIntegeri_v( GLenum target, GLuint index, GLint* data )
    {
        _glGetIntegeri_v( target, index, data );
    }

    /// <inheritdoc/>
    public void glGetIntegeri_v( GLenum target, GLuint index, ref GLint[] data )
    {
        fixed ( GLint* p = &data[ 0 ] )
        {
            _glGetIntegeri_v( target, index, p );
        }
    }
    
    [DllImport( LIBGL, EntryPoint = "glGetIntegeri_v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetIntegeri_v( GLenum target, GLuint index, GLint* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEnablei( GLenum target, GLuint index )
    {
        _glEnablei( target, index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glEnablei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEnablei( GLenum target, GLuint index );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDisablei( GLenum target, GLuint index )
    {
        _glDisablei( target, index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDisablei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDisablei( GLenum target, GLuint index );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsEnabledi( GLenum target, GLuint index )
    {
        return _glIsEnabledi( target, index );
    }

    [DllImport( LIBGL, EntryPoint = "glIsEnabledi", CallingConvention = CallingConvention.Cdecl)]
    private static extern GLboolean _glIsEnabledi( GLenum target, GLuint index );
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBeginTransformFeedback( GLenum primitiveMode )
    {
        _glBeginTransformFeedback( primitiveMode );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBeginTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBeginTransformFeedback( GLenum primitiverMode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEndTransformFeedback()
    {
        _glEndTransformFeedback();
    }
    
    [DllImport( LIBGL, EntryPoint = "glEndTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEndTransformFeedback();

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBufferRange( GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glBindBufferRange( target, index, buffer, offset, size );
    }

    [DllImport( LIBGL, EntryPoint = "glBindBufferRange", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindBufferRange( GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBufferBase( GLenum target, GLuint index, GLuint buffer )
    {
        _glBindBufferBase( target, index, buffer );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBindBufferBase", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindBufferBase( GLenum target, GLuint index, GLuint buffer );
    
    // ------------------------------------------------------------------------
}

#pragma warning restore IDE0079    // Remove unnecessary suppression
#pragma warning restore CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore CS8603     // Possible null reference return.
#pragma warning restore IDE0060    // Remove unused parameter.
#pragma warning restore IDE1006    // Naming Styles.
#pragma warning restore IDE0090    // Use 'new(...)'.
#pragma warning restore CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning restore SYSLIB1054 // 


