// ///////////////////////////////////////////////////////////////////////////////
// MIT License
// 
// Copyright (c) 2023 dcronqvist (Daniel Cronqvist <daniel@dcronqvist.se>)
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

#pragma warning disable IDE0079    // Remove unnecessary suppression
#pragma warning disable CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning disable CS8603     // Possible null reference return.
#pragma warning disable IDE0060    // Remove unused parameter.
#pragma warning disable IDE1006    // Naming Styles.
#pragma warning disable IDE0090    // Use 'new(...)'.
#pragma warning disable CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning disable SYSLIB1054 // 

// ============================================================================

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

// ============================================================================

namespace Corelib.LibCore.Graphics.OpenGL;

// ============================================================================
// ============================================================================

/// <summary>
/// Bindings for OpenGL, for core profile. Blazing fast, low level,
/// direct access to the OpenGL API for all versions of OpenGL, using the unmanaged
/// delegates feature in C# 9.0,
/// <para>
/// Also includes a few overloads of many functions to make them a bit more C# friendly
/// (e.g. passing arrays of bytes or floats instead of passing pointers to fixed memory
/// locations). Significant effort has been made to make sure that the overloads are as
/// efficient as possible, in terms of both performance and memory usage.
/// </para>
/// </summary>
[PublicAPI]
public unsafe partial class GLBindings : IGLBindings
{
    public const string LIBGL   = "opengl32.dll";
    public const string LIBGLFW = "glfw3.dll";

    /// <summary>
    /// The null pointer, just like in C/C++.
    /// </summary>
    public readonly void* NULL = ( void* )0;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Returns a Tuple holding the version of OpenGL being used, obtained by
    /// calling <see cref="glGetString"/> with the parameter IGL.GL_VERSION.
    /// </summary>
    public (int major, int minor) GetProjectOpenGLVersion()
    {
        var version = glGetString( IGL.GL_VERSION );

        if ( version == null )
        {
            Logger.Debug( "NULL GL Version returned!" );
        }

        return version == null
            ? ( GraphicsData.DEFAULT_GL_MAJOR, GraphicsData.DEFAULT_GL_MINOR )
            : ( version[ 0 ], version[ 2 ] );
    }

    // ========================================================================
    // ========================================================================

    /// <inheritdoc/>
    public void glCullFace( GLenum mode )
    {
        _glCullFace( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glCullFace", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCullFace( GLenum mode );

    // ========================================================================

    /// <inheritdoc/>
    public void glFrontFace( GLenum mode )
    {
        _glFrontFace( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glFrontFace", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFrontFace( GLenum mode );

    // ========================================================================

    /// <inheritdoc/>
    public void glHint( GLenum target, GLenum mode )
    {
        _glHint( target, mode );
    }

    [DllImport( LIBGL, EntryPoint = "glHint", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glHint( GLenum target, GLenum mode );

    // ========================================================================

    /// <inheritdoc/>
    public void glLineWidth( GLfloat width )
    {
        _glLineWidth( width );
    }

    [DllImport( LIBGL, EntryPoint = "glLineWidth", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLineWidth( GLfloat width );

    // ========================================================================

    /// <inheritdoc/>
    public void glPointSize( GLfloat size )
    {
        _glPointSize( size );
    }

    [DllImport( LIBGL, EntryPoint = "glPointSize", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointSize( GLfloat size );

    // ========================================================================

    /// <inheritdoc/>
    public void glPolygonMode( GLenum face, GLenum mode )
    {
        _glPolygonMode( face, mode );
    }

    [DllImport( LIBGL, EntryPoint = "glPolygonMode", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPolygonMode( GLenum face, GLenum mode );

    // ========================================================================

    /// <inheritdoc/>
    public void glScissor( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glScissor( x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glScissor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glScissor( GLint x, GLint y, GLsizei width, GLsizei height );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexParameterf( GLenum target, GLenum pname, GLfloat param )
    {
        _glTexParameterf( target, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameterf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameterf( GLenum target, GLenum pname, GLfloat param );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters )
    {
        _glTexParameterfv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glTexParameterfv( GLenum target, GLenum pname, GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glTexParameterfv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexParameteri( GLenum target, GLenum pname, GLint param )
    {
        _glTexParameteri( target, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameteri( GLenum target, GLenum pname, GLint param );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glTexParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glTexParameteriv( GLenum target, GLenum pname, GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glTexParameteriv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameteriv( GLenum target, GLenum pname, GLint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLenum format, GLenum type, void* pixels )
    {
        _glTexImage1D( target, level, internalformat, width, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexImage1D< T >( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLenum format, GLenum type, T[] pixels )
        where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexImage1D( target, level, internalformat, width, border, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLenum format, GLenum type, void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexImage2D( GLenum target,
                              GLint level,
                              GLenum internalformat,
                              GLsizei width,
                              GLsizei height,
                              GLint border,
                              GLenum format,
                              GLenum type,
                              void* pixels )
    {
        _glTexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexImage2D< T >( GLenum target,
                                   GLint level,
                                   GLenum internalformat,
                                   GLsizei width,
                                   GLsizei height,
                                   GLint border,
                                   GLenum format,
                                   GLenum type,
                                   T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexImage2D( target, level, internalformat, width, height, border, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage2D( GLenum target,
                                              GLint level,
                                              GLenum internalformat,
                                              GLsizei width,
                                              GLsizei height,
                                              GLint border,
                                              GLenum format,
                                              GLenum type,
                                              void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glDrawBuffer( GLenum buf )
    {
        _glDrawBuffer( buf );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawBuffer( GLenum buf );

    // ========================================================================

    /// <inheritdoc/>
    public void glClear( GLbitfield mask )
    {
        _glClear( mask );
    }

    [DllImport( LIBGL, EntryPoint = "glClear", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClear( GLbitfield mask );

    // ========================================================================

    /// <inheritdoc/>
    public void glClearColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        _glClearColor( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glClearColor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );

    // ========================================================================

    /// <inheritdoc/>
    public void glClearStencil( GLint s )
    {
        _glClearStencil( s );
    }

    [DllImport( LIBGL, EntryPoint = "glClearStencil", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearStencil( GLint s );

    // ========================================================================

    /// <inheritdoc/>
    public void glClearDepth( GLdouble depth )
    {
        _glClearDepth( depth );
    }

    [DllImport( LIBGL, EntryPoint = "glClearDepth", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearDepth( GLdouble depth );

    // ========================================================================

    /// <inheritdoc/>
    public void glStencilMask( GLuint mask )
    {
        _glStencilMask( mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilMask( GLuint mask );

    // ========================================================================

    /// <inheritdoc/>
    public void glColorMask( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha )
    {
        _glColorMask( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glColorMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glColorMask( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha );

    // ========================================================================

    /// <inheritdoc/>
    public void glDepthMask( GLboolean flag )
    {
        _glDepthMask( flag );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthMask( GLboolean flag );

    // ========================================================================

    /// <inheritdoc/>
    public void glDisable( GLenum cap )
    {
        _glDisable( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glDisable", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDisable( GLenum cap );

    // ========================================================================

    /// <inheritdoc/>
    public void glEnable( GLenum cap )
    {
        _glEnable( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glEnable", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEnable( GLenum cap );

    // ========================================================================

    /// <inheritdoc/>
    public void glFinish()
    {
        _glFinish();
    }

    [DllImport( LIBGL, EntryPoint = "glFinish", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFinish();

    // ========================================================================

    /// <inheritdoc/>
    public void glFlush()
    {
        _glFlush();
    }

    [DllImport( LIBGL, EntryPoint = "glFlush", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFlush();

    // ========================================================================

    /// <inheritdoc/>
    public void glBlendFunc( GLenum sfactor, GLenum dfactor )
    {
        _glBlendFunc( sfactor, dfactor );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFunc( GLenum sfactor, GLenum dfactor );

    // ========================================================================

    /// <inheritdoc/>
    public void glLogicOp( GLenum opcode )
    {
        _glLogicOp( opcode );
    }

    [DllImport( LIBGL, EntryPoint = "glLogicOp", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLogicOp( GLenum opcode );

    // ========================================================================

    /// <inheritdoc/>
    public void glStencilFunc( GLenum func, GLint @ref, GLuint mask )
    {
        _glStencilFunc( func, @ref, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilFunc( GLenum func, GLint @ref, GLuint mask );

    // ========================================================================

    /// <inheritdoc/>
    public void glStencilOp( GLenum fail, GLenum zfail, GLenum zpass )
    {
        _glStencilOp( fail, zfail, zpass );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilOp", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilOp( GLenum fail, GLenum zfail, GLenum zpass );

    // ========================================================================

    /// <inheritdoc/>
    public void glDepthFunc( GLenum func )
    {
        _glDepthFunc( func );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthFunc( GLenum func );

    // ========================================================================

    /// <inheritdoc/>
    public void glPixelStoref( GLenum pname, GLfloat param )
    {
        _glPixelStoref( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPixelStoref", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPixelStoref( GLenum pname, GLfloat param );

    // ========================================================================

    /// <inheritdoc/>
    public void glPixelStorei( GLenum pname, GLint param )
    {
        _glPixelStorei( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPixelStorei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPixelStorei( GLenum pname, GLint param );

    // ========================================================================

    /// <inheritdoc/>
    public void glReadBuffer( GLenum src )
    {
        _glReadBuffer( src );
    }

    [DllImport( LIBGL, EntryPoint = "glReadBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glReadBuffer( GLenum src );

    // ========================================================================

    /// <inheritdoc/>
    public void glReadPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels )
    {
        _glReadPixels( x, y, width, height, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glReadPixels< T >( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, ref T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glReadPixels( x, y, width, height, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glReadPixels", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glReadPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetBooleanv( GLenum pname, GLboolean* data )
    {
        _glGetBooleanv( pname, data );
    }

    /// <inheritdoc/>
    public void glGetBooleanv( GLenum pname, ref GLboolean[] data )
    {
        fixed ( GLboolean* p = &data[ 0 ] )
        {
            _glGetBooleanv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBooleanv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBooleanv( GLenum pname, GLboolean* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetDoublev( GLenum pname, GLdouble* data )
    {
        _glGetDoublev( pname, data );
    }

    /// <inheritdoc/>
    public void glGetDoublev( GLenum pname, ref GLdouble[] data )
    {
        fixed ( GLdouble* p = &data[ 0 ] )
        {
            _glGetDoublev( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetDoublev", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetDoublev( GLenum pname, GLdouble* data );

    // ========================================================================

    /// <inheritdoc/>
    public GLenum glGetError()
    {
        return _glGetError();
    }

    [DllImport( LIBGL, EntryPoint = "glGetError", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLenum _glGetError();

    // ========================================================================

    /// <inheritdoc/>
    public void glGetFloatv( GLenum pname, float* data )
    {
        _glGetFloatv( pname, data );
    }

    /// <inheritdoc/>
    public void glGetFloatv( GLenum pname, ref GLfloat[] data )
    {
        fixed ( GLfloat* p = &data[ 0 ] )
        {
            _glGetFloatv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetFloatv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetFloatv( GLenum pname, float* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetIntegerv( GLenum pname, GLint* data )
    {
        _glGetIntegerv( pname, data );
    }

    /// <inheritdoc/>
    public void glGetIntegerv( GLenum pname, ref GLint[] data )
    {
        fixed ( GLint* p = &data[ 0 ] )
        {
            _glGetIntegerv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetIntegerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetIntegerv( GLenum pname, GLint* data );

    // ========================================================================

    /// <inheritdoc/>
    public GLubyte* glGetString( GLenum name )
    {
        return _glGetString( name );
    }

    [DllImport( LIBGL, EntryPoint = "glGetString", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLubyte* _glGetString( GLenum name );

    // ========================================================================

    /// <inheritdoc/>
    public string glGetStringSafe( GLenum name )
    {
        var p = _glGetString( name );

        if ( p == null )
        {
            return null;
        }

        var i = 0;

        while ( p[ i ] != 0 )
        {
            i++;
        }

        return new string( ( sbyte* )p, 0, i, Encoding.UTF8 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void glGetTexImage( GLenum target, GLint level, GLenum format, GLenum type, void* pixels )
    {
        _glGetTexImage( target, level, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glGetTexImage< T >( GLenum target, GLint level, GLenum format, GLenum type, ref T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glGetTexImage( target, level, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetIntegerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexImage( GLenum target, GLint level, GLenum format, GLenum type, void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters )
    {
        _glGetTexParameterfv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexParameterfv( GLenum target, GLenum pname, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glGetTexParameterfv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetTexParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetTexParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetTexParameteriv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexParameteriv( GLenum target, GLenum pname, GLint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, GLfloat* parameters )
    {
        _glGetTexLevelParameterfv( target, level, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glGetTexLevelParameterfv( target, level, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexLevelParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, GLfloat* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, GLint* parameters )
    {
        _glGetTexLevelParameteriv( target, level, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetTexLevelParameteriv( target, level, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexLevelParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, GLint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsEnabled( GLenum cap )
    {
        return _glIsEnabled( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glIsEnabled", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsEnabled( GLenum cap );

    // ========================================================================

    /// <inheritdoc/>
    public void glDepthRange( GLdouble near, GLdouble far )
    {
        _glDepthRange( near, far );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthRange", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthRange( GLdouble near, GLdouble far );

    // ========================================================================

    /// <inheritdoc/>
    public void glViewport( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glViewport( x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glViewport", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glViewport( GLint x, GLint y, GLsizei width, GLsizei height );

    // ========================================================================

    /// <inheritdoc/>
    public void glDrawArrays( GLenum mode, GLint first, GLsizei count )
    {
        _glDrawArrays( mode, first, count );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawArrays", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawArrays( GLenum mode, GLint first, GLsizei count );

    // ========================================================================

    /// <inheritdoc/>
    public void glDrawElements( GLenum mode, GLsizei count, GLenum type, void* indices )
    {
        _glDrawElements( mode, count, type, indices );
    }

    /// <inheritdoc/>
    public void glDrawElements< T >( GLenum mode, GLsizei count, GLenum type, T[] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( T* p = &indices[ 0 ] )
        {
            _glDrawElements( mode, count, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawElements", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawElements( GLenum mode, GLsizei count, GLenum type, void* indices );

    // ========================================================================

    /// <inheritdoc/>
    public void glPolygonOffset( GLfloat factor, GLfloat units )
    {
        _glPolygonOffset( factor, units );
    }

    [DllImport( LIBGL, EntryPoint = "glPolygonOffset", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPolygonOffset( GLfloat factor, GLfloat units );

    // ========================================================================

    /// <inheritdoc/>
    public void glCopyTexImage1D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border )
    {
        _glCopyTexImage1D( target, level, internalformat, x, y, width, border );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexImage1D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border );

    // ========================================================================

    /// <inheritdoc/>
    public void glCopyTexImage2D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border )
    {
        _glCopyTexImage2D( target, level, internalformat, x, y, width, height, border );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexImage2D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border );

    // ========================================================================

    /// <inheritdoc/>
    public void glCopyTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width )
    {
        _glCopyTexSubImage1D( target, level, xoffset, x, y, width );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width );

    // ========================================================================

    /// <inheritdoc/>
    public void glCopyTexSubImage2D( GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage2D( GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels )
    {
        _glTexSubImage1D( target, level, xoffset, width, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexSubImage1D< T >( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, T[] pixels )
        where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexSubImage1D( target, level, xoffset, width, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexSubImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexSubImage2D( GLenum target,
                                 GLint level,
                                 GLint xoffset,
                                 GLint yoffset,
                                 GLsizei width,
                                 GLsizei height,
                                 GLenum format,
                                 GLenum type,
                                 void* pixels )
    {
        _glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexSubImage2D< T >( GLenum target,
                                      GLint level,
                                      GLint xoffset,
                                      GLint yoffset,
                                      GLsizei width,
                                      GLsizei height,
                                      GLenum format,
                                      GLenum type,
                                      T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexSubImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexSubImage2D( GLenum target,
                                                 GLint level,
                                                 GLint xoffset,
                                                 GLint yoffset,
                                                 GLsizei width,
                                                 GLsizei height,
                                                 GLenum format,
                                                 GLenum type,
                                                 void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glBindTexture( GLenum target, GLuint texture )
    {
        _glBindTexture( target, texture );
    }

    [DllImport( LIBGL, EntryPoint = "glBindTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindTexture( GLenum target, GLuint texture );

    // ========================================================================

    /// <inheritdoc/>
    public void glDeleteTextures( GLsizei n, GLuint* textures )
    {
        _glDeleteTextures( n, textures );
    }

    /// <inheritdoc/>
    public void glDeleteTextures( params GLuint[] textures )
    {
        fixed ( void* p = &textures[ 0 ] )
        {
            _glDeleteTextures( textures.Length, ( GLuint* )p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteTextures", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteTextures( GLsizei n, GLuint* textures );

    // ========================================================================

    /// <inheritdoc/>
    public void glGenTextures( GLsizei n, GLuint* textures )
    {
        _glGenTextures( n, textures );
    }

    /// <inheritdoc/>
    public GLuint[] glGenTextures( GLsizei n )
    {
        var textures = new GLuint[ n ];

        fixed ( void* p = &textures[ 0 ] )
        {
            _glGenTextures( n, ( GLuint* )p );
        }

        return textures;
    }

    /// <inheritdoc/>
    public GLuint glGenTexture()
    {
        return glGenTextures( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenTextures( GLsizei n, GLuint* textures );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsTexture( GLuint texture )
    {
        return _glIsTexture( texture );
    }

    [DllImport( LIBGL, EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsTexture( GLuint texture );

    // ========================================================================

    /// <inheritdoc/>
    public void glDrawRangeElements( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices )
    {
        _glDrawRangeElements( mode, start, end, count, type, indices );
    }

    /// <inheritdoc/>
    public void glDrawRangeElements< T >( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( T* p_indices = &indices[ 0 ] )
        {
            _glDrawRangeElements( mode, start, end, count, type, p_indices );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawRangeElements", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawRangeElements( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexImage3D( GLenum target,
                              GLint level,
                              GLint internalformat,
                              GLsizei width,
                              GLsizei height,
                              GLsizei depth,
                              GLint border,
                              GLenum format,
                              GLenum type,
                              void* pixels )
    {
        _glTexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexImage3D< T >( GLenum target,
                                   GLint level,
                                   GLint internalformat,
                                   GLsizei width,
                                   GLsizei height,
                                   GLsizei depth,
                                   GLint border,
                                   GLenum format,
                                   GLenum type,
                                   T[] pixels ) where T : unmanaged
    {
        fixed ( T* p_pixels = &pixels[ 0 ] )
        {
            _glTexImage3D( target, level, internalformat, width, height, depth, border, format, type, p_pixels );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage3D( GLenum target,
                                              GLint level,
                                              GLint internalformat,
                                              GLsizei width,
                                              GLsizei height,
                                              GLsizei depth,
                                              GLint border,
                                              GLenum format,
                                              GLenum type,
                                              void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexSubImage3D( GLenum target,
                                 GLint level,
                                 GLint xoffset,
                                 GLint yoffset,
                                 GLint zoffset,
                                 GLsizei width,
                                 GLsizei height,
                                 GLsizei depth,
                                 GLenum format,
                                 GLenum type,
                                 void* pixels )
    {
        _glTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexSubImage3D< T >( GLenum target,
                                      GLint level,
                                      GLint xoffset,
                                      GLint yoffset,
                                      GLint zoffset,
                                      GLsizei width,
                                      GLsizei height,
                                      GLsizei depth,
                                      GLenum format,
                                      GLenum type,
                                      T[] pixels ) where T : unmanaged
    {
        fixed ( T* p_pixels = &pixels[ 0 ] )
        {
            _glTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, p_pixels );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexSubImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexSubImage3D( GLenum target,
                                                 GLint level,
                                                 GLint xoffset,
                                                 GLint yoffset,
                                                 GLint zoffset,
                                                 GLsizei width,
                                                 GLsizei height,
                                                 GLsizei depth,
                                                 GLenum format,
                                                 GLenum type,
                                                 void* pixels );

    // ========================================================================

    /// <inheritdoc/>
    public void glCopyTexSubImage3D( GLenum target,
                                     GLint level,
                                     GLint xoffset,
                                     GLint yoffset,
                                     GLint zoffset,
                                     GLint x,
                                     GLint y,
                                     GLsizei width,
                                     GLsizei height )
    {
        _glCopyTexSubImage3D( target, level, xoffset, yoffset, zoffset, x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage3D( GLenum target,
                                                     GLint level,
                                                     GLint xoffset,
                                                     GLint yoffset,
                                                     GLint zoffset,
                                                     GLint x,
                                                     GLint y,
                                                     GLsizei width,
                                                     GLsizei height );

    // ========================================================================

    /// <inheritdoc/>
    public void glActiveTexture( GLenum texture )
    {
        _glActiveTexture( texture );
    }

    [DllImport( LIBGL, EntryPoint = "glActiveTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glActiveTexture( GLenum texture );

    // ========================================================================

    /// <inheritdoc/>
    public void glSampleCoverage( GLfloat value, GLboolean invert )
    {
        _glSampleCoverage( value, invert );
    }

    [DllImport( LIBGL, EntryPoint = "glSampleCoverage", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSampleCoverage( GLfloat value, GLboolean invert );

    // ========================================================================

    /// <inheritdoc/>
    public void glCompressedTexImage3D( GLenum target,
                                        GLint level,
                                        GLenum internalformat,
                                        GLsizei width,
                                        GLsizei height,
                                        GLsizei depth,
                                        GLint border,
                                        GLsizei imageSize,
                                        void* data )
    {
        _glCompressedTexImage3D( target, level, internalformat, width, height, depth, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexImage3D( GLenum target,
                                        GLint level,
                                        GLenum internalformat,
                                        GLsizei width,
                                        GLsizei height,
                                        GLsizei depth,
                                        GLint border,
                                        byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage3D( target, level, internalformat, width, height, depth, border, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexImage3D( GLenum target,
                                                        GLint level,
                                                        GLenum internalformat,
                                                        GLsizei width,
                                                        GLsizei height,
                                                        GLsizei depth,
                                                        GLint border,
                                                        GLsizei imageSize,
                                                        void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glCompressedTexImage2D( GLenum target,
                                        GLint level,
                                        GLenum internalformat,
                                        GLsizei width,
                                        GLsizei height,
                                        GLint border,
                                        GLsizei imageSize,
                                        void* data )
    {
        _glCompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexImage2D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLsizei height, GLint border, byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage2D( target, level, internalformat, width, height, border, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexImage2D( GLenum target,
                                                        GLint level,
                                                        GLenum internalformat,
                                                        GLsizei width,
                                                        GLsizei height,
                                                        GLint border,
                                                        GLsizei imageSize,
                                                        void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glCompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLsizei imageSize, void* data )
    {
        _glCompressedTexImage1D( target, level, internalformat, width, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage1D( target, level, internalformat, width, border, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLsizei imageSize, void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glCompressedTexSubImage3D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLint zoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLsizei depth,
                                           GLenum format,
                                           GLsizei imageSize,
                                           void* data )
    {
        _glCompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexSubImage3D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLint zoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLsizei depth,
                                           GLenum format,
                                           byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexSubImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexSubImage3D( GLenum target,
                                                           GLint level,
                                                           GLint xoffset,
                                                           GLint yoffset,
                                                           GLint zoffset,
                                                           GLsizei width,
                                                           GLsizei height,
                                                           GLsizei depth,
                                                           GLenum format,
                                                           GLsizei imageSize,
                                                           void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glCompressedTexSubImage2D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLenum format,
                                           GLsizei imageSize,
                                           void* data )
    {
        _glCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexSubImage2D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLenum format,
                                           byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexSubImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexSubImage2D( GLenum target,
                                                           GLint level,
                                                           GLint xoffset,
                                                           GLint yoffset,
                                                           GLsizei width,
                                                           GLsizei height,
                                                           GLenum format,
                                                           GLsizei imageSize,
                                                           void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glCompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTexSubImage1D( target, level, xoffset, width, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage1D( target, level, xoffset, width, format, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexSubImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetCompressedTexImage( GLenum target, GLint level, void* img )
    {
        _glGetCompressedTexImage( target, level, img );
    }

    /// <inheritdoc/>
    public void glGetCompressedTexImage( GLenum target, GLint level, ref byte[] img )
    {
        fixed ( byte* p = &img[ 0 ] )
        {
            _glGetCompressedTexImage( target, level, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetCompressedTexImage", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetCompressedTexImage( GLenum target, GLint level, void* img );

    // ========================================================================

    /// <inheritdoc/>
    public void glBlendFuncSeparate( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha )
    {
        _glBlendFuncSeparate( sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFuncSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFuncSeparate( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha );

    // ========================================================================

    /// <inheritdoc/>
    public void glMultiDrawArrays( GLenum mode, GLint* first, GLsizei* count, GLsizei drawcount )
    {
        _glMultiDrawArrays( mode, first, count, drawcount );
    }

    /// <inheritdoc/>
    public void glMultiDrawArrays( GLenum mode, GLint[] first, GLsizei[] count )
    {
        if ( first.Length != count.Length )
        {
            throw new ArgumentException( "first and count arrays must be of the same length" );
        }

        fixed ( GLint* p1 = &first[ 0 ] )
        {
            fixed ( GLsizei* p2 = &count[ 0 ] )
            {
                _glMultiDrawArrays( mode, p1, p2, count.Length );
            }
        }
    }

    [DllImport( LIBGL, EntryPoint = "glMultiDrawArrays", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMultiDrawArrays( GLenum mode, GLint* first, GLsizei* count, GLsizei drawcount );

    // ========================================================================

    /// <inheritdoc/>
    public void glMultiDrawElements( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount )
    {
        _glMultiDrawElements( mode, count, type, indices, drawcount );
    }

    /// <inheritdoc/>
    public void glMultiDrawElements< T >( GLenum mode, GLsizei[] count, GLenum type, T[][] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        var indexPtrs = new void*[ indices.Length ];

        for ( var i = 0; i < indices.Length; i++ )
        {
            fixed ( void* p = &indices[ i ][ 0 ] )
            {
                indexPtrs[ i ] = p;
            }
        }

        fixed ( GLsizei* c = &count[ 0 ] )
        {
            fixed ( void** p = &indexPtrs[ 0 ] )
            {
                _glMultiDrawElements( mode, c, type, p, count.Length );
            }
        }
    }

    [DllImport( LIBGL, EntryPoint = "glMultiDrawElements", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMultiDrawElements( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount );

    // ========================================================================

    /// <inheritdoc/>
    public void glPointParameterf( GLenum pname, GLfloat param )
    {
        _glPointParameterf( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameterf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameterf( GLenum pname, GLfloat param );

    // ========================================================================

    /// <inheritdoc/>
    public void glPointParameterfv( GLenum pname, GLfloat* parameters )
    {
        _glPointParameterfv( pname, parameters );
    }

    /// <inheritdoc/>
    public void glPointParameterfv( GLenum pname, GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glPointParameterfv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameterfv( GLenum pname, GLfloat* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glPointParameteri( GLenum pname, GLint param )
    {
        _glPointParameteri( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameteri( GLenum pname, GLint param );

    // ========================================================================

    /// <inheritdoc/>
    public void glPointParameteriv( GLenum pname, GLint* parameters )
    {
        _glPointParameteriv( pname, parameters );
    }

    /// <inheritdoc/>
    public void glPointParameteriv( GLenum pname, GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glPointParameteriv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameteriv( GLenum pname, GLint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glBlendColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        _glBlendColor( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlenColor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );

    // ========================================================================

    /// <inheritdoc/>
    public void glBlendEquation( GLenum mode )
    {
        _glBlendEquation( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendEquation", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquation( GLenum mode );

    // ========================================================================

    /// <inheritdoc/>
    public void glGenQueries( GLsizei n, GLuint* ids )
    {
        _glGenQueries( n, ids );
    }

    /// <inheritdoc/>
    public GLuint[] glGenQueries( GLsizei n )
    {
        var ret = new GLuint[ n ];

        fixed ( GLuint* p = &ret[ 0 ] )
        {
            _glGenQueries( n, p );
        }

        return ret;
    }

    /// <inheritdoc/>
    public GLuint glGenQuery()
    {
        return glGenQueries( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenQueries", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenQueries( GLsizei n, GLuint* ids );

    // ========================================================================

    /// <inheritdoc/>
    public void glDeleteQueries( GLsizei n, GLuint* ids )
    {
        _glDeleteQueries( n, ids );
    }

    /// <inheritdoc/>
    public void glDeleteQueries( params GLuint[] ids )
    {
        fixed ( GLuint* p = &ids[ 0 ] )
        {
            _glDeleteQueries( ids.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteQueries", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteQueries( GLsizei n, GLuint* ids );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsQuery( GLuint id )
    {
        return _glIsQuery( id );
    }

    [DllImport( LIBGL, EntryPoint = "glIsQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsQuery( GLuint id );

    // ========================================================================

    /// <inheritdoc/>
    public void glBeginQuery( GLenum target, GLuint id )
    {
        _glBeginQuery( target, id );
    }

    [DllImport( LIBGL, EntryPoint = "glBeginQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBeginQuery( GLenum target, GLuint id );

    // ========================================================================

    /// <inheritdoc/>
    public void glEndQuery( GLenum target )
    {
        _glEndQuery( target );
    }

    [DllImport( LIBGL, EntryPoint = "glEndQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEndQuery( GLenum target );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetQueryiv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetQueryiv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetQueryiv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryiv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryiv( GLenum target, GLenum pname, GLint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetQueryObjectiv( GLuint id, GLenum pname, GLint* parameters )
    {
        _glGetQueryObjectiv( id, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetQueryObjectiv( GLuint id, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryObjectiv( id, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryObjectiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryObjectiv( GLuint id, GLenum pname, GLint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetQueryObjectuiv( GLuint id, GLenum pname, GLuint* parameters )
    {
        _glGetQueryObjectuiv( id, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetQueryObjectuiv( GLuint id, GLenum pname, ref GLuint[] parameters )
    {
        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetQueryObjectuiv( id, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryObjectuiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryObjectuiv( GLuint id, GLenum pname, GLuint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glBindBuffer( GLenum target, GLuint buffer )
    {
        _glBindBuffer( target, buffer );
    }

    [DllImport( LIBGL, EntryPoint = "glBindBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindBuffer( GLenum target, GLuint buffer );

    // ========================================================================

    /// <inheritdoc/>
    public void glDeleteBuffers( GLsizei n, GLuint* buffers )
    {
        _glDeleteBuffers( n, buffers );
    }

    /// <inheritdoc/>
    public void glDeleteBuffers( params GLuint[] buffers )
    {
        fixed ( GLuint* p = &buffers[ 0 ] )
        {
            _glDeleteBuffers( buffers.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteBuffers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteBuffers( GLsizei n, GLuint* buffers );

    // ========================================================================

    /// <inheritdoc/>
    public void glGenBuffers( GLsizei n, GLuint* buffers )
    {
        _glGenBuffers( n, buffers );
    }

    /// <inheritdoc/>
    public GLuint[] glGenBuffers( GLsizei n )
    {
        var ret = new GLuint[ n ];

        fixed ( GLuint* p = &ret[ 0 ] )
        {
            _glGenBuffers( n, p );
        }

        return ret;
    }

    /// <inheritdoc/>
    public GLuint glGenBuffer()
    {
        return glGenBuffers( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenBuffers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenBuffers( GLsizei n, GLuint* buffers );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsBuffer( GLuint buffer )
    {
        return _glIsBuffer( buffer );
    }

    [DllImport( LIBGL, EntryPoint = "glIsBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsBuffer( GLuint buffer );

    // ========================================================================

    /// <inheritdoc/>
    public void glBufferData( GLenum target, GLsizeiptr size, void* data, GLenum usage )
    {
        _glBufferData( target, size, data, usage );
    }

    /// <inheritdoc/>
    public void glBufferData< T >( GLenum target, T[] data, GLenum usage ) where T : unmanaged
    {
        fixed ( T* p = &data[ 0 ] )
        {
            _glBufferData( target, sizeof( T ) * data.Length, p, usage );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glBufferData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBufferData( GLenum target, GLsizeiptr size, void* data, GLenum usage );

    // ========================================================================

    /// <inheritdoc/>
    public void glBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glBufferSubData( target, offset, size, data );
    }

    /// <inheritdoc/>
    public void glBufferSubData< T >( GLenum target, GLintptr offsetCount, T[] data ) where T : unmanaged
    {
        fixed ( T* p = &data[ 0 ] )
        {
            _glBufferSubData( target, offsetCount * sizeof( T ), sizeof( T ) * data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glBufferSubData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glGetBufferSubData( target, offset, size, data );
    }

    /// <inheritdoc/>
    public void glGetBufferSubData< T >( GLenum target, GLintptr offsetCount, GLsizei count, ref T[] data ) where T : unmanaged
    {
        fixed ( T* p = &data[ 0 ] )
        {
            _glGetBufferSubData( target, offsetCount * sizeof( T ), sizeof( T ) * count, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBufferSubData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data );

    // ========================================================================

    /// <inheritdoc/>
    public void* glMapBuffer( GLenum target, GLenum access )
    {
        return _glMapBuffer( target, access );
    }

    /// <inheritdoc/>
    public Span< T > glMapBuffer< T >( GLenum target, GLenum access ) where T : unmanaged
    {
        GLint size;

        _glGetBufferParameteriv( target, IGL.GL_BUFFER_SIZE, &size );

        var ret = _glMapBuffer( target, access );

        return new Span< T >( ret, size / sizeof( T ) );
    }

    [DllImport( LIBGL, EntryPoint = "glMapBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void* _glMapBuffer( GLenum target, GLenum access );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glUnmapBuffer( GLenum target )
    {
        return _glUnmapBuffer( target );
    }

    [DllImport( LIBGL, EntryPoint = "glUnmapBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glUnmapBuffer( GLenum target );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetBufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetBufferParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetBufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetBufferParameteriv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBufferParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBufferParameteriv( GLenum target, GLenum pname, GLint* parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetBufferPointerv( GLenum target, GLenum pname, void** parameters )
    {
        _glGetBufferPointerv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetBufferPointerv( GLenum target, GLenum pname, ref IntPtr[] parameters )
    {
        fixed ( IntPtr* p = &parameters[ 0 ] )
        {
            _glGetBufferPointerv( target, pname, ( void** )p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBufferPointerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBufferPointerv( GLenum target, GLenum pname, void** parameters );

    // ========================================================================

    /// <inheritdoc/>
    public void glBlendEquationSeparate( GLenum modeRGB, GLenum modeAlpha )
    {
        _glBlendEquationSeparate( modeRGB, modeAlpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendEquationSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquationSeparate( GLenum modeRGB, GLenum modeAlpha );

    // ========================================================================

    /// <inheritdoc/>
    public void glDrawBuffers( GLsizei n, GLenum* bufs )
    {
        _glDrawBuffers( n, bufs );
    }

    /// <inheritdoc/>
    public void glDrawBuffers( params GLenum[] bufs )
    {
        fixed ( GLenum* pbufs = &bufs[ 0 ] )
        {
            _glDrawBuffers( bufs.Length, pbufs );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawBuffers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawBuffers( GLsizei n, GLenum* bufs );

    // ========================================================================

    /// <inheritdoc/>
    public void glStencilOpSeparate( GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass )
    {
        _glStencilOpSeparate( face, sfail, dpfail, dppass );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilOpSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilOpSeparate( GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass );

    // ========================================================================

    /// <inheritdoc/>
    public void glStencilFuncSeparate( GLenum face, GLenum func, GLint @ref, GLuint mask )
    {
        _glStencilFuncSeparate( face, func, @ref, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilFuncSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilFuncSeparate( GLenum face, GLenum sfail, GLint @ref, GLuint mask );

    // ========================================================================

    /// <inheritdoc/>
    public void glStencilMaskSeparate( GLenum face, GLuint mask )
    {
        _glStencilMaskSeparate( face, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilMaskSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilMaskSeparate( GLenum face, GLuint mask );

    // ========================================================================

    /// <inheritdoc/>
    public void glAttachShader( GLuint program, GLuint shader )
    {
        _glAttachShader( program, shader );
    }

    [DllImport( LIBGL, EntryPoint = "glAttachShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glAttachShader( GLuint program, GLuint shader );

    // ========================================================================

    /// <inheritdoc/>
    public void glBindAttribLocation( GLuint program, GLuint index, GLchar* name )
    {
        _glBindAttribLocation( program, index, name );
    }

    /// <inheritdoc/>
    public void glBindAttribLocation( GLuint program, GLuint index, string name )
    {
        var utf8 = Encoding.UTF8.GetBytes( name );

        fixed ( byte* putf8 = &utf8[ 0 ] )
        {
            _glBindAttribLocation( program, index, putf8 );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glBindAttribLocation", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindAttribLocation( GLuint program, GLuint index, GLchar* name );

    // ========================================================================

    /// <inheritdoc/>
    public void glCompileShader( GLuint shader )
    {
        _glCompileShader( shader );
    }

    [DllImport( LIBGL, EntryPoint = "glCompileShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompileShader( GLuint shader );

    // ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateProgram()
    {
        return _glCreateProgram();
    }

    [DllImport( LIBGL, EntryPoint = "glCreateProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glCreateProgram();

    // ========================================================================

    /// <inheritdoc/>
    public GLuint glCreateShader( GLenum type )
    {
        return _glCreateShader( type );
    }

    [DllImport( LIBGL, EntryPoint = "glCreateShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glCreateShader( GLenum type );

    // ========================================================================

    /// <inheritdoc/>
    public void glDeleteProgram( GLuint program )
    {
        _glDeleteProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDeleteProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteProgram( GLuint program );

    // ========================================================================

    /// <inheritdoc/>
    public void glDeleteShader( GLuint shader )
    {
        _glDeleteShader( shader );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDeleteShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteShader( GLuint shader );

    // ========================================================================

    /// <inheritdoc/>
    public void glDetachShader( GLuint program, GLuint shader )
    {
        _glDetachShader( program, shader );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDetachShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDetachShader( GLuint program, GLuint shader );

    // ========================================================================

    /// <inheritdoc/>
    public void glDisableVertexAttribArray( GLuint index )
    {
        _glDisableVertexAttribArray( index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDisableVertexAttribArray", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDisableVertexAttribArray( GLuint index );

    // ========================================================================

    /// <inheritdoc/>
    public void glEnableVertexAttribArray( GLuint index )
    {
        _glEnableVertexAttribArray( index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glEnableVertexAttribArray", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEnableVertexAttribArray( GLuint index );

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsProgram( GLuint program )
    {
        return _glIsProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glIsProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsProgram( GLuint program );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsShader( GLuint shader )
    {
        return _glIsShader( shader );
    }
    
    [DllImport( LIBGL, EntryPoint = "glIsShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsShader( GLuint shader );

    // ========================================================================

    /// <inheritdoc/>
    public void glLinkProgram( GLuint program )
    {
        _glLinkProgram( program );
    }

    [DllImport( LIBGL, EntryPoint = "glLinkProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLinkProgram( GLuint program );
    
    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glUseProgram( GLuint program )
    {
        _glUseProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUseProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUseProgram( GLuint program );

    // ========================================================================

    /// <inheritdoc/>
    public void glUniform1f( GLint location, GLfloat v0 )
    {
        _glUniform1f( location, v0 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform1f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1f( GLint location, GLfloat v0 );

    // ========================================================================

    /// <inheritdoc/>
    public void glUniform2f( GLint location, GLfloat v0, GLfloat v1 )
    {
        _glUniform2f( location, v0, v1 );
    }

    [DllImport( LIBGL, EntryPoint = "glUniform2f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2f( GLint location, GLfloat v0, GLfloat v1 );
    
    // ========================================================================

    /// <inheritdoc/>
    public void glUniform3f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        _glUniform3f( location, v0, v1, v2 );
    }
    
    [DllImport(LIBGL, EntryPoint = "glUniform3f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2 );

    // ========================================================================

    /// <inheritdoc/>
    public void glUniform4f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        _glUniform4f( location, v0, v1, v2, v3 );
    }
    
    [DllImport(LIBGL, EntryPoint = "glUniform4f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 );

    // ========================================================================

    /// <inheritdoc/>
    public void glUniform1i( GLint location, GLint v0 )
    {
        _glUniform1i( location, v0 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform1i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1i( GLint location, GLint v0 );

    // ========================================================================

    /// <inheritdoc/>
    public void glUniform2i( GLint location, GLint v0, GLint v1 )
    {
        _glUniform2i( location, v0, v1 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform2i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2i( GLint location, GLint v0, GLint v1 );

    // ========================================================================

    /// <inheritdoc/>
    public void glUniform3i( GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glUniform3i( location, v0, v1, v2 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform3i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3i( GLint location, GLint v0, GLint v1, GLint v2 );

    // ========================================================================

    /// <inheritdoc/>
    public void glUniform4i( GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glUniform4i( location, v0, v1, v2, v3 );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUniform4i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4i( GLint location, GLint v0, GLint v1, GLint v2, GLint v3 );

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glValidateProgram( GLuint program )
    {
        return _glValidateProgram( program );
    }
    
    [DllImport( LIBGL, EntryPoint = "glValidateProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern bool _glValidateProgram( GLuint program );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib1d( GLuint index, GLdouble x )
    {
        _glVertexAttrib1d( index, x );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1d( GLuint index, GLdouble x );

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib1f( GLuint index, GLfloat x )
    {
        _glVertexAttrib1f( index, x );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1f( GLuint index, GLfloat x );

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib1s( GLuint index, GLshort x )
    {
        _glVertexAttrib1s( index, x );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib1s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib1s( GLuint index, GLshort x );

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttrib2d( index, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2d( GLuint index, GLdouble x, GLdouble y );

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib2f( GLuint index, GLfloat x, GLfloat y )
    {
        _glVertexAttrib2f( index, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2f( GLuint index, GLfloat x, GLfloat y );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2fv( GLuint index, GLfloat* v );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib2s( GLuint index, GLshort x, GLshort y )
    {
        _glVertexAttrib2s( index, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2s( GLuint index, GLshort x, GLshort y );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib2sv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib2sv( GLuint index, GLshort* v );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib3d( GLuint index, GLdouble x, GLdouble y, GLdouble z )
    {
        _glVertexAttrib3d( index, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3d( GLuint index, GLdouble x, GLdouble y, GLdouble z );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3dv( GLuint index, GLdouble* v );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib3f( GLuint index, GLfloat x, GLfloat y, GLfloat z )
    {
        _glVertexAttrib3f( index, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3f( GLuint index, GLfloat x, GLfloat y, GLfloat z );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3fv( GLuint index, GLfloat* v );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib3s( GLuint index, GLshort x, GLshort y, GLshort z )
    {
        _glVertexAttrib3s( index, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3s( GLuint index, GLshort x, GLshort y, GLshort z );
    
    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib3sv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib3sv( GLuint index, GLshort* v );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Nbv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Nbv( GLuint index, GLbyte* v );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Niv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Niv( GLuint index, GLint* v );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Nsv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Nsv( GLuint index, GLshort* v );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib4Nub( GLuint index, GLubyte x, GLubyte y, GLubyte z, GLubyte w )
    {
        _glVertexAttrib4Nub( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Nub", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Nub( GLuint index, GLubyte x, GLubyte y, GLubyte z, GLubyte w );
    
    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Nubv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Nubv( GLuint index, GLubyte* v );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Nuiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Nuiv( GLuint index, GLuint* v );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4Nusv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4Nusv( GLuint index, GLushort* v );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4bv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4bv( GLuint index, GLbyte* v );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glVertexAttrib4d( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w );

    // ========================================================================

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

    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4dv( GLuint index, GLdouble* v );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib4f( GLuint index, GLfloat x, GLfloat y, GLfloat z, GLfloat w )
    {
        _glVertexAttrib4f( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4f( GLuint index, GLfloat x, GLfloat y, GLfloat z, GLfloat w );

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttrib4s( GLuint index, GLshort x, GLshort y, GLshort z, GLshort w )
    {
        _glVertexAttrib4s( index, x, y, z, w );
    }
    
    [DllImport( LIBGL, EntryPoint = "glVertexAttrib4s", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttrib4s( GLuint index, GLshort x, GLshort y, GLshort z, GLshort w );

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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
    
    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glColorMaski( GLuint index, GLboolean r, GLboolean g, GLboolean b, GLboolean a )
    {
        _glColorMaski( index, r, g, b, a );
    }
    
    [DllImport( LIBGL, EntryPoint = "glColorMaski", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glColorMaski( GLuint index, GLboolean r, GLboolean g, GLboolean b, GLboolean a );

    // ========================================================================

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

    // ========================================================================

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

    // ========================================================================

    /// <inheritdoc/>
    public void glEnablei( GLenum target, GLuint index )
    {
        _glEnablei( target, index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glEnablei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEnablei( GLenum target, GLuint index );

    // ========================================================================

    /// <inheritdoc/>
    public void glDisablei( GLenum target, GLuint index )
    {
        _glDisablei( target, index );
    }
    
    [DllImport( LIBGL, EntryPoint = "glDisablei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDisablei( GLenum target, GLuint index );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsEnabledi( GLenum target, GLuint index )
    {
        return _glIsEnabledi( target, index );
    }

    [DllImport( LIBGL, EntryPoint = "glIsEnabledi", CallingConvention = CallingConvention.Cdecl)]
    private static extern GLboolean _glIsEnabledi( GLenum target, GLuint index );
    
    // ========================================================================

    /// <inheritdoc/>
    public void glBeginTransformFeedback( GLenum primitiveMode )
    {
        _glBeginTransformFeedback( primitiveMode );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBeginTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBeginTransformFeedback( GLenum primitiverMode );

    // ========================================================================

    /// <inheritdoc/>
    public void glEndTransformFeedback()
    {
        _glEndTransformFeedback();
    }
    
    [DllImport( LIBGL, EntryPoint = "glEndTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEndTransformFeedback();

    // ========================================================================

    /// <inheritdoc/>
    public void glBindBufferRange( GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glBindBufferRange( target, index, buffer, offset, size );
    }

    [DllImport( LIBGL, EntryPoint = "glBindBufferRange", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindBufferRange( GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size );

    // ========================================================================

    /// <inheritdoc/>
    public void glBindBufferBase( GLenum target, GLuint index, GLuint buffer )
    {
        _glBindBufferBase( target, index, buffer );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBindBufferBase", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindBufferBase( GLenum target, GLuint index, GLuint buffer );
    
    // ========================================================================
    
    /// <inheritdoc/>
    public void glWaitSync( void* sync, GLbitfield flags, GLuint64 timeout )
    {
        _glWaitSync( sync, flags, timeout );
    }

    /// <inheritdoc/>
    public void glWaitSyncSafe( IntPtr sync, GLbitfield flags, GLuint64 timeout )
    {
        _glWaitSync( sync.ToPointer(), flags, timeout );
    }

    [DllImport( LIBGL, EntryPoint = "glWaitSync", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glWaitSync( void* sync, GLbitfield flags, GLuint64 timeout );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetInteger64v( GLenum pname, GLint64* data )
    {
        _glGetInteger64v( pname, data );
    }

    /// <inheritdoc/>
    public void glGetInteger64v( GLenum pname, ref GLint64[] data )
    {
        fixed ( GLint64* dp = &data[ 0 ] )
        {
            _glGetInteger64v( pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetInteger64v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetInteger64v( GLenum pname, GLint64* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetSynciv( void* sync, GLenum pname, GLsizei bufSize, GLsizei* length, GLint* values )
    {
        _glGetSynciv( sync, pname, bufSize, length, values );
    }

    /// <inheritdoc/>
    public GLint[] glGetSynciv( IntPtr sync, GLenum pname, GLsizei bufSize )
    {
        var ret = new GLint[ bufSize ];

        fixed ( GLint* dp = &ret[ 0 ] )
        {
            GLsizei len;
            _glGetSynciv( sync.ToPointer(), pname, bufSize, &len, dp );
            Array.Resize( ref ret, len );
        }

        return ret;
    }

    [DllImport( LIBGL, EntryPoint = "glGetSynciv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetSynciv( void* sync, GLenum pname, GLsizei bufSize, GLsizei* length, GLint* values );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetInteger64i_v( GLenum target, GLuint index, GLint64* data )
    {
        _glGetInteger64i_v( target, index, data );
    }

    /// <inheritdoc/>
    public void glGetInteger64i_v( GLenum target, GLuint index, ref GLint64[] data )
    {
        fixed ( GLint64* dp = &data[ 0 ] )
        {
            _glGetInteger64i_v( target, index, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetInteger64i_v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetInteger64i_v( GLenum target, GLuint index, GLint64* data );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetBufferParameteri64v( GLenum target, GLenum pname, GLint64* parameters )
    {
        _glGetBufferParameteri64v( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetBufferParameteri64v( GLenum target, GLenum pname, ref GLint64[] parameters )
    {
        fixed ( GLint64* dp = &parameters[ 0 ] )
        {
            _glGetBufferParameteri64v( target, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBufferParameteri64v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBufferParameteri64v( GLenum target, GLenum pname, GLint64* parameters );
    
    // ========================================================================

    /// <inheritdoc/>
    public void glFramebufferTexture( GLenum target, GLenum attachment, GLuint texture, GLint level )
    {
        _glFramebufferTexture( target, attachment, texture, level );
    }

    [DllImport( LIBGL, EntryPoint = "glFramebufferTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFramebufferTexture( GLenum target, GLenum attachment, GLuint texture, GLint level );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexImage2DMultisample( GLenum target,
                                         GLsizei samples,
                                         GLenum internalformat,
                                         GLsizei width,
                                         GLsizei height,
                                         GLboolean fixedsamplelocations )
    {
        _glTexImage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage2DMultisample", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage2DMultisample( GLenum target,
                                                        GLsizei samples,
                                                        GLenum internalformat,
                                                        GLsizei width,
                                                        GLsizei height,
                                                        GLboolean fixedsamplelocations );

    // ========================================================================

    /// <inheritdoc/>
    public void glTexImage3DMultisample( GLenum target,
                                         GLsizei samples,
                                         GLenum internalformat,
                                         GLsizei width,
                                         GLsizei height,
                                         GLsizei depth,
                                         GLboolean fixedsamplelocations )
    {
        _glTexImage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage3DMultisample", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage3DMultisample( GLenum target,
                                                        GLsizei samples,
                                                        GLenum internalformat,
                                                        GLsizei width,
                                                        GLsizei height,
                                                        GLsizei depth,
                                                        GLboolean fixedsamplelocations );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetMultisamplefv( GLenum pname, GLuint index, GLfloat* val )
    {
        _glGetMultisamplefv( pname, index, val );
    }

    /// <inheritdoc/>
    public void glGetMultisamplefvSafe( GLenum pname, GLuint index, ref GLfloat[] val )
    {
        fixed ( GLfloat* dp = &val[ 0 ] )
        {
            _glGetMultisamplefv( pname, index, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetMultisamplefv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetMultisamplefv( GLenum pname, GLuint index, GLfloat* val );
    
    // ========================================================================

    /// <inheritdoc/>
    public void glSampleMaski( GLuint maskNumber, GLbitfield mask )
    {
        _glSampleMaski( maskNumber, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glSampleMaski", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSampleMaski( GLuint maskNumber, GLbitfield mask );

    // ========================================================================

    /// <inheritdoc/>
    public void glBindFragDataLocationIndexed( GLuint program, GLuint colorNumber, GLuint index, GLchar* name )
    {
        _glBindFragDataLocationIndexed( program, colorNumber, index, name );
    }

    /// <inheritdoc/>
    public void glBindFragDataLocationIndexed( GLuint program, GLuint colorNumber, GLuint index, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            _glBindFragDataLocationIndexed( program, colorNumber, index, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glBindFragDataLocationIndexed", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindFragDataLocationIndexed( GLuint program, GLuint colorNumber, GLuint index, GLchar* name );

    // ========================================================================

    /// <inheritdoc/>
    public GLint glGetFragDataIndex( GLuint program, GLchar* name )
    {
        return _glGetFragDataIndex( program, name );
    }

    /// <inheritdoc/>
    public GLint glGetFragDataIndex( GLuint program, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetFragDataIndex( program, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetFragDataIndex", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetFragDataIndex( GLuint program, GLchar* name );

    // ========================================================================

    /// <inheritdoc/>
    public void glGenSamplers( GLsizei count, GLuint* samplers )
    {
        _glGenSamplers( count, samplers );
    }

    /// <inheritdoc/>
    public GLuint[] glGenSamplers( GLsizei count )
    {
        var result = new GLuint[ count ];

        fixed ( GLuint* dp = &result[ 0 ] )
        {
            _glGenSamplers( count, dp );
        }

        return result;
    }

    /// <inheritdoc/>
    public GLuint glGenSampler()
    {
        return glGenSamplers( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenSamplers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenSamplers( GLsizei count, GLuint* samplers );

    // ========================================================================

    /// <inheritdoc/>
    public void glDeleteSamplers( GLsizei count, GLuint* samplers )
    {
        _glDeleteSamplers( count, samplers );
    }

    /// <inheritdoc/>
    public void glDeleteSamplers( params GLuint[] samplers )
    {
        fixed ( GLuint* dp = &samplers[ 0 ] )
        {
            _glDeleteSamplers( samplers.Length, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteSamplers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteSamplers( GLsizei count, GLuint* samplers );

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean glIsSampler( GLuint sampler )
    {
        return _glIsSampler( sampler );
    }

    [DllImport( LIBGL, EntryPoint = "glIsSampler", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsSampler( GLuint sampler );

    // ========================================================================

    /// <inheritdoc/>
    public void glBindSampler( GLuint unit, GLuint sampler )
    {
        _glBindSampler( unit, sampler );
    }

    [DllImport( LIBGL, EntryPoint = "glBindSampler", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindSampler( GLuint unit, GLuint sampler );

    // ========================================================================

    /// <inheritdoc/>
    public void glSamplerParameteri( GLuint sampler, GLenum pname, GLint param )
    {
        _glSamplerParameteri( sampler, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glSamplerParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSamplerParameteri( GLuint sampler, GLenum pname, GLint param );

    // ========================================================================

    /// <inheritdoc/>
    public void glSamplerParameteriv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glSamplerParameteriv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glSamplerParameteriv( GLuint sampler, GLenum pname, GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glSamplerParameteriv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glSamplerParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSamplerParameteriv( GLuint sampler, GLenum pname, GLint* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glSamplerParameterf( GLuint sampler, GLenum pname, GLfloat param )
    {
        _glSamplerParameterf( sampler, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glSamplerParameterf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSamplerParameterf( GLuint sampler, GLenum pname, GLfloat param );

    // ========================================================================

    /// <inheritdoc/>
    public void glSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param )
    {
        _glSamplerParameterfv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat[] param )
    {
        fixed ( GLfloat* dp = &param[ 0 ] )
        {
            _glSamplerParameterfv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glSamplerParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glSamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glSamplerParameterIiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glSamplerParameterIiv( GLuint sampler, GLenum pname, GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glSamplerParameterIiv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glSamplerParameterIiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param )
    {
        _glSamplerParameterIuiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* dp =
                   &param[ 0 ] )
        {
            _glSamplerParameterIuiv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glSamplerParameterIuiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetSamplerParameteriv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glGetSamplerParameteriv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glGetSamplerParameteriv( GLuint sampler, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameteriv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetSamplerParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetSamplerParameteriv( GLuint sampler, GLenum pname, GLint* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetSamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glGetSamplerParameterIiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glGetSamplerParameterIiv( GLuint sampler, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameterIiv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetSamplerParameterIiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetSamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param )
    {
        _glGetSamplerParameterfv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glGetSamplerParameterfv( GLuint sampler, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* dp =
                   &param[ 0 ] )
        {
            _glGetSamplerParameterfv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetSamplerParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param )
    {
        _glGetSamplerParameterIuiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void glGetSamplerParameterIuiv( GLuint sampler, GLenum pname, ref GLuint[] param )
    {
        fixed ( GLuint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameterIuiv( sampler, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetSamplerParameterIuiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glQueryCounter( GLuint id, GLenum target )
    {
        _glQueryCounter( id, target );
    }

    [DllImport( LIBGL, EntryPoint = "glQueryCounter", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glQueryCounter( GLuint id, GLenum target );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetQueryObjecti64v( GLuint id, GLenum pname, GLint64* param )
    {
        _glGetQueryObjecti64v( id, pname, param );
    }

    /// <inheritdoc/>
    public void glGetQueryObjecti64v( GLuint id, GLenum pname, ref GLint64[] param )
    {
        fixed ( GLint64* dp =
                   &param[ 0 ] )
        {
            _glGetQueryObjecti64v( id, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryObjecti64v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryObjecti64v( GLuint id, GLenum pname, GLint64* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glGetQueryObjectui64v( GLuint id, GLenum pname, GLuint64* param )
    {
        _glGetQueryObjectui64v( id, pname, param );
    }

    /// <inheritdoc/>
    public void glGetQueryObjectui64v( GLuint id, GLenum pname, ref GLuint64[] param )
    {
        fixed ( GLuint64* dp = &param[ 0 ] )
        {
            _glGetQueryObjectui64v( id, pname, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryObjectui64v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryObjectui64v( GLuint id, GLenum pname, GLuint64* param );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribDivisor( GLuint index, GLuint divisor )
    {
        _glVertexAttribDivisor( index, divisor );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribDivisor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribDivisor( GLuint index, GLuint divisor );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP1ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP1ui( index, type, normalized, value );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP1ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP1ui( GLuint index, GLenum type, GLboolean normalized, GLuint value );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP1uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP1uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void glVertexAttribP1uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP1uiv( index, type, normalized, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP1uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP1uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP2ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP2ui( index, type, normalized, value );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP2ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP2ui( GLuint index, GLenum type, GLboolean normalized, GLuint value );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP2uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP2uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void glVertexAttribP2uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP2uiv( index, type, normalized, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP2uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP2uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP3ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP3ui( index, type, normalized, value );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP3ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP3ui( GLuint index, GLenum type, GLboolean normalized, GLuint value );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP3uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP3uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void glVertexAttribP3uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP3uiv( index, type, normalized, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP3uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP3uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP4ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP4ui( index, type, normalized, value );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP4ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP4ui( GLuint index, GLenum type, GLboolean normalized, GLuint value );

    // ========================================================================

    /// <inheritdoc/>
    public void glVertexAttribP4uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP4uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void glVertexAttribP4uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp =
                   &value[ 0 ] )
        {
            _glVertexAttribP4uiv( index, type, normalized, dp );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribP4uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribP4uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value );

    // ========================================================================

    public void glMinSampleShading( GLfloat value )
    {
        _glMinSampleShading( value );
    }

    [DllImport( LIBGL, EntryPoint = "glMinSampleShading", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMinSampleShading( GLfloat value );

    // ========================================================================

    public void glBlendEquationi( GLuint buf, GLenum mode )
    {
        _glBlendEquationi( buf, mode );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendEquationi", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquationi( GLuint buf, GLenum mode );

    // ========================================================================

    public void glBlendEquationSeparatei( GLuint buf, GLenum modeRGB, GLenum modeAlpha )
    {
        _glBlendEquationSeparatei( buf, modeRGB, modeAlpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendEquationSeparatei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquationSeparatei( GLuint buf, GLenum modeRGB, GLenum modeAlpha );

    // ========================================================================

    public void glBlendFunci( GLuint buf, GLenum src, GLenum dst )
    {
        _glBlendFunci( buf, src, dst );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFunci", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFunci( GLuint buf, GLenum src, GLenum dst );

    // ========================================================================

    public void glBlendFuncSeparatei( GLuint buf, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha )
    {
        _glBlendFuncSeparatei( buf, srcRGB, dstRGB, srcAlpha, dstAlpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFuncSeparatei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFuncSeparatei( GLuint buf, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha );

    // ========================================================================

    public struct DrawArraysIndirectCommand
    {
        public GLuint count;
        public GLuint instanceCount;
        public GLuint first;
        public GLuint baseInstance;
    }

    public void glDrawArraysIndirect( GLenum mode, void* indirect )
    {
        _glDrawArraysIndirect( mode, indirect );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawArraysIndirect", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawArraysIndirect( GLenum mode, void* indirect );

    // ========================================================================

    public void glDrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect )
    {
        _glDrawArraysIndirect( mode, &indirect );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawArraysIndirect", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect );

    // ========================================================================

    public struct DrawElementsIndirectCommand
    {
        public GLuint count;
        public GLuint primCount;
        public GLuint firstIndex;
        public GLuint baseVertex;
        public GLuint baseInstance;
    }

    public void glDrawElementsIndirect( GLenum mode, GLenum type, void* indirect )
    {
        _glDrawElementsIndirect( mode, type, indirect );
    }

    public void glDrawElementsIndirect( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect )
    {
        _glDrawElementsIndirect( mode, type, &indirect );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawElementsIndirect", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawElementsIndirect( GLenum mode, GLenum type, void* indirect );
    
    // ========================================================================

    public void glUniform1d( GLint location, GLdouble x )
    {
        _glUniform1d( location, x );
    }

    [DllImport( LIBGL, EntryPoint = "glUniform1d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1d( GLint location, GLdouble x );

    // ========================================================================

    public void glUniform2d( GLint location, GLdouble x, GLdouble y )
    {
        _glUniform2d( location, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glUniform2d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2d( GLint location, GLdouble x, GLdouble y );

    // ========================================================================

    public void glUniform3d( GLint location, GLdouble x, GLdouble y, GLdouble z )
    {
        _glUniform3d( location, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glUniform3d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3d( GLint location, GLdouble x, GLdouble y, GLdouble z );

    // ========================================================================

    public void glUniform4d( GLint location, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glUniform4d( location, x, y, z, w );
    }

    [DllImport( LIBGL, EntryPoint = "glUniform4d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4d( GLint location, GLdouble x, GLdouble y, GLdouble z, GLdouble w );

    // ========================================================================

    public void glUniform1dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform1dv( location, count, value );
    }

    public void glUniform1dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform1dv( location, value.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniform1dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform1dv( GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glUniform2dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform2dv( location, count, value );
    }

    public void glUniform2dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform2dv( location, value.Length / 2, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniform2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform2dv( GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glUniform3dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform3dv( location, count, value );
    }

    public void glUniform3dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform3dv( location, value.Length / 3, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniform3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform3dv( GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glUniform4dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform4dv( location, count, value );
    }

    public void glUniform4dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform4dv( location, value.Length / 4, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniform4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniform4dv( GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glUniformMatrix2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2dv( location, count, transpose, value );
    }

    public void glUniformMatrix2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix2dv( location, value.Length / 4, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glUniformMatrix3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3dv( location, count, transpose, value );
    }

    public void glUniformMatrix3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix3dv( location, value.Length / 9, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glUniformMatrix4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4dv( location, count, transpose, value );
    }


    public void glUniformMatrix4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix4dv( location, value.Length / 16, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glUniformMatrix2x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2x3dv( location, count, transpose, value );
    }

    public void glUniformMatrix2x3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix2x3dv( location, value.Length / 6, transpose, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix2x3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix2x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================
    
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

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix2x4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix2x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

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

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix3x2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix3x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

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

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix3x4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix3x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

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

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix4x2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix4x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

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

    [DllImport( LIBGL, EntryPoint = "glUniformMatrix4x3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformMatrix4x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

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

    [DllImport( LIBGL, EntryPoint = "glGetUniformdv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetUniformdv( GLuint program, GLint location, GLdouble* parameters );

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

    [DllImport( LIBGL, EntryPoint = "glGetSubroutineUniformLocation", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetSubroutineUniformLocation( GLuint program, GLenum shadertype, GLchar* name );

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

    [DllImport( LIBGL, EntryPoint = "glGetSubroutineIndex", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glGetSubroutineIndex( GLuint program, GLenum shadertype, GLchar* name );

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

    [DllImport( LIBGL, EntryPoint = "glGetActiveSubroutineUniformiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetActiveSubroutineUniformiv( GLuint program, GLenum shadertype, GLuint index, GLenum pname, GLint* values );

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

    [DllImport( LIBGL, EntryPoint = "glGetActiveSubroutineUniformName", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetActiveSubroutineUniformName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name );

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

            return new string( ( GLbyte* )p, 0, length, Encoding.UTF8 );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetActiveSubroutineName", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetActiveSubroutineName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name );

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

    [DllImport( LIBGL, EntryPoint = "glUniformSubroutinesuiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUniformSubroutinesuiv( GLenum shadertype, GLsizei count, GLuint* indices );

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

    [DllImport( LIBGL, EntryPoint = "glGetUniformSubroutineuiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetUniformSubroutineuiv( GLenum shadertype, GLint location, GLuint* parameters );

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

    [DllImport( LIBGL, EntryPoint = "glGetProgramStageiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramStageiv( GLuint program, GLenum shadertype, GLenum pname, GLint* values );

    // ========================================================================

    public void glPatchParameteri( GLenum pname, GLint value )
    {
        _glPatchParameteri( pname, value );
    }

    [DllImport( LIBGL, EntryPoint = "glPatchParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPatchParameteri( GLenum pname, GLint value );

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

    [DllImport( LIBGL, EntryPoint = "glPatchParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPatchParameterfv( GLenum pname, GLfloat* values );

    // ========================================================================

    public void glBindTransformFeedback( GLenum target, GLuint id )
    {
        _glBindTransformFeedback( target, id );
    }

    [DllImport( LIBGL, EntryPoint = "glBindTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindTransformFeedback( GLenum target, GLuint id );

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

    [DllImport( LIBGL, EntryPoint = "glDeleteTransformFeedbacks", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteTransformFeedbacks( GLsizei n, GLuint* ids );

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

    [DllImport( LIBGL, EntryPoint = "glGenTransformFeedbacks", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenTransformFeedbacks( GLsizei n, GLuint* ids );

    // ========================================================================

    public GLboolean glIsTransformFeedback( GLuint id )
    {
        return _glIsTransformFeedback( id );
    }

    [DllImport( LIBGL, EntryPoint = "glIsTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsTransformFeedback( GLuint id );

    // ========================================================================

    public void glPauseTransformFeedback()
    {
        _glPauseTransformFeedback();
    }

    [DllImport( LIBGL, EntryPoint = "glPauseTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPauseTransformFeedback();

    // ========================================================================

    public void glResumeTransformFeedback()
    {
        _glResumeTransformFeedback();
    }

    [DllImport( LIBGL, EntryPoint = "glResumeTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glResumeTransformFeedback();

    // ========================================================================

    public void glDrawTransformFeedback( GLenum mode, GLuint id )
    {
        _glDrawTransformFeedback( mode, id );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawTransformFeedback", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawTransformFeedback( GLenum mode, GLuint id );

    // ========================================================================

    public void glDrawTransformFeedbackStream( GLenum mode, GLuint id, GLuint stream )
    {
        _glDrawTransformFeedbackStream( mode, id, stream );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawTransformFeedbackStream", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawTransformFeedbackStream( GLenum mode, GLuint id, GLuint stream );

    // ========================================================================

    public void glBeginQueryIndexed( GLenum target, GLuint index, GLuint id )
    {
        _glBeginQueryIndexed( target, index, id );
    }

    [DllImport( LIBGL, EntryPoint = "glBeginQueryIndexed", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBeginQueryIndexed( GLenum target, GLuint index, GLuint id );

    // ========================================================================

    public void glEndQueryIndexed( GLenum target, GLuint index )
    {
        _glEndQueryIndexed( target, index );
    }

    [DllImport( LIBGL, EntryPoint = "glEndQueryIndexed", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEndQueryIndexed( GLenum target, GLuint index );

    // ========================================================================

    public void glGetQueryIndexediv( GLenum target, GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetQueryIndexediv( target, index, pname, parameters );
    }

    public void glGetQueryIndexediv( GLenum target, GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryIndexediv( target, index, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryIndexediv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryIndexediv( GLenum target, GLuint index, GLenum pname, GLint* parameters );

    // ========================================================================

    public void glReleaseShaderCompiler()
    {
        _glReleaseShaderCompiler();
    }

    [DllImport( LIBGL, EntryPoint = "glReleaseShaderCompiler", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glReleaseShaderCompiler();

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

    [DllImport( LIBGL, EntryPoint = "glShaderBinary", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glShaderBinary( GLsizei count, GLuint* shaders, GLenum binaryformat, void* binary, GLsizei length );

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

    [DllImport( LIBGL, EntryPoint = "glGetShaderPrecisionFormat", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetShaderPrecisionFormat( GLenum shaderType, GLenum precisionType, GLint* range, GLint* precision );

    // ========================================================================

    public void glDepthRangef( GLfloat n, GLfloat f )
    {
        _glDepthRangef( n, f );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthRangef", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthRangef( GLfloat n, GLfloat f );

    // ========================================================================

    public void glClearDepthf( GLfloat d )
    {
        _glClearDepthf( d );
    }

    [DllImport( LIBGL, EntryPoint = "glClearDepthf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearDepthf( GLfloat d );

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

    [DllImport( LIBGL, EntryPoint = "glGetProgramBinary", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramBinary( GLuint program, GLsizei bufSize, GLsizei* length, GLenum* binaryFormat, void* binary );

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

    [DllImport( LIBGL, EntryPoint = "glProgramBinary", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramBinary( GLuint program, GLenum binaryFormat, void* binary, GLsizei length );

    // ========================================================================

    public void glProgramParameteri( GLuint program, GLenum pname, GLint value )
    {
        _glProgramParameteri( program, pname, value );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramParameteri( GLuint program, GLenum pname, GLint value );

    // ========================================================================

    public void glUseProgramStages( GLuint pipeline, GLbitfield stages, GLuint program )
    {
        _glUseProgramStages( pipeline, stages, program );
    }

    [DllImport( LIBGL, EntryPoint = "glUseProgramStages", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glUseProgramStages( GLuint pipeline, GLbitfield stages, GLuint program );

    // ========================================================================

    public void glActiveShaderProgram( GLuint pipeline, GLuint program )
    {
        _glActiveShaderProgram( pipeline, program );
    }

    [DllImport( LIBGL, EntryPoint = "glActiveShaderProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glActiveShaderProgram( GLuint pipeline, GLuint program );

    // ========================================================================

    public GLuint glCreateShaderProgramv( GLenum type, GLsizei count, GLchar** strings )
    {
        return _glCreateShaderProgramv( type, count, strings );
    }

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

    [DllImport( LIBGL, EntryPoint = "glCreateShaderProgramv", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glCreateShaderProgramv( GLenum type, GLsizei count, GLchar** strings );

    // ========================================================================

    public void glBindProgramPipeline( GLuint pipeline )
    {
        _glBindProgramPipeline( pipeline );
    }

    [DllImport( LIBGL, EntryPoint = "glBindProgramPipeline", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindProgramPipeline( GLuint pipeline );

    // ========================================================================

    public void glDeleteProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glDeleteProgramPipelines( n, pipelines );
    }

    public void glDeleteProgramPipelines( params GLuint[] pipelines )
    {
        fixed ( GLuint* pPipelines = &pipelines[ 0 ] )
        {
            _glDeleteProgramPipelines( pipelines.Length, pPipelines );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteProgramPipelines", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteProgramPipelines( GLsizei n, GLuint* pipelines );

    // ========================================================================

    public void glGenProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glGenProgramPipelines( n, pipelines );
    }

    public GLuint[] glGenProgramPipelines( GLsizei n )
    {
        var pipelines = new GLuint[ n ];

        fixed ( GLuint* pPipelines = &pipelines[ 0 ] )
        {
            _glGenProgramPipelines( n, pPipelines );
        }

        return pipelines;
    }

    public GLuint glGenProgramPipeline()
    {
        return glGenProgramPipelines( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenProgramPipelines", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenProgramPipelines( GLsizei n, GLuint* pipelines );

    // ========================================================================

    public GLboolean glIsProgramPipeline( GLuint pipeline )
    {
        return _glIsProgramPipeline( pipeline );
    }

    [DllImport( LIBGL, EntryPoint = "glIsProgramPipeline", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsProgramPipeline( GLuint pipeline );

    // ========================================================================

    public void glGetProgramPipelineiv( GLuint pipeline, GLenum pname, GLint* param )
    {
        _glGetProgramPipelineiv( pipeline, pname, param );
    }

    public void glGetProgramPipelineiv( GLuint pipeline, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* pParam = &param[ 0 ] )
        {
            _glGetProgramPipelineiv( pipeline, pname, pParam );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramPipelineiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramPipelineiv( GLuint pipeline, GLenum pname, GLint* param );

    // ========================================================================

    public void glProgramUniform1i( GLuint program, GLint location, GLint v0 )
    {
        _glProgramUniform1i( program, location, v0 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1i( GLuint program, GLint location, GLint v0 );

    // ========================================================================

    public void glProgramUniform1iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform1iv( program, location, count, value );
    }

    public void glProgramUniform1iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1iv( program, location, value.Length, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1iv( GLuint program, GLint location, GLsizei count, GLint* value );

    // ========================================================================

    public void glProgramUniform1f( GLuint program, GLint location, GLfloat v0 )
    {
        _glProgramUniform1f( program, location, v0 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1f( GLuint program, GLint location, GLfloat v0 );

    // ========================================================================

    public void glProgramUniform1fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform1fv( program, location, count, value );
    }

    public void glProgramUniform1fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1fv( program, location, value.Length, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1fv( GLuint program, GLint location, GLsizei count, GLfloat* value );

    // ========================================================================

    public void glProgramUniform1d( GLuint program, GLint location, GLdouble v0 )
    {
        _glProgramUniform1d( program, location, v0 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1d( GLuint program, GLint location, GLdouble v0 );

    // ========================================================================

    public void glProgramUniform1dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform1dv( program, location, count, value );
    }

    public void glProgramUniform1dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1dv( program, location, value.Length, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1dv( GLuint program, GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glProgramUniform1ui( GLuint program, GLint location, GLuint v0 )
    {
        _glProgramUniform1ui( program, location, v0 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1ui( GLuint program, GLint location, GLuint v0 );

    // ========================================================================

    public void glProgramUniform1uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform1uiv( program, location, count, value );
    }

    public void glProgramUniform1uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue = &value[ 0 ] )
        {
            _glProgramUniform1uiv( program, location, value.Length, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform1uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform1uiv( GLuint program, GLint location, GLsizei count, GLuint* value );

    // ========================================================================

    public void glProgramUniform2i( GLuint program, GLint location, GLint v0, GLint v1 )
    {
        _glProgramUniform2i( program, location, v0, v1 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2i( GLuint program, GLint location, GLint v0, GLint v1 );

    // ========================================================================

    public void glProgramUniform2iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform2iv( program, location, count, value );
    }

    public void glProgramUniform2iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue = &value[ 0 ] )
        {
            _glProgramUniform2iv( program, location, value.Length / 2, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2iv( GLuint program, GLint location, GLsizei count, GLint* value );

    // ========================================================================

    public void glProgramUniform2f( GLuint program, GLint location, GLfloat v0, GLfloat v1 )
    {
        _glProgramUniform2f( program, location, v0, v1 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2f( GLuint program, GLint location, GLfloat v0, GLfloat v1 );

    // ========================================================================

    public void glProgramUniform2fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform2fv( program, location, count, value );
    }

    public void glProgramUniform2fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniform2fv( program, location, value.Length / 2, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2fv( GLuint program, GLint location, GLsizei count, GLfloat* value );

    // ========================================================================

    public void glProgramUniform2d( GLuint program, GLint location, GLdouble v0, GLdouble v1 )
    {
        _glProgramUniform2d( program, location, v0, v1 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2d( GLuint program, GLint location, GLdouble v0, GLdouble v1 );

    // ========================================================================

    public void glProgramUniform2dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform2dv( program, location, count, value );
    }

    public void glProgramUniform2dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniform2dv( program, location, value.Length / 2, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2dv( GLuint program, GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glProgramUniform2ui( GLuint program, GLint location, GLuint v0, GLuint v1 )
    {
        _glProgramUniform2ui( program, location, v0, v1 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2ui( GLuint program, GLint location, GLuint v0, GLuint v1 );

    // ========================================================================

    public void glProgramUniform2uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform2uiv( program, location, count, value );
    }

    public void glProgramUniform2uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2uiv( program, location, value.Length / 2, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform2uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform2uiv( GLuint program, GLint location, GLsizei count, GLuint* value );

    // ========================================================================

    public void glProgramUniform3i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glProgramUniform3i( program, location, v0, v1, v2 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2 );

    // ========================================================================

    public void glProgramUniform3iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform3iv( program, location, count, value );
    }

    public void glProgramUniform3iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue = &value[ 0 ] )
        {
            _glProgramUniform3iv( program, location, value.Length / 3, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3iv( GLuint program, GLint location, GLsizei count, GLint* value );

    // ========================================================================

    public void glProgramUniform3f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        _glProgramUniform3f( program, location, v0, v1, v2 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2 );

    // ========================================================================

    public void glProgramUniform3fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform3fv( program, location, count, value );
    }

    public void glProgramUniform3fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniform3fv( program, location, value.Length / 3, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3fv( GLuint program, GLint location, GLsizei count, GLfloat* value );

    // ========================================================================

    public void glProgramUniform3d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2 )
    {
        _glProgramUniform3d( program, location, v0, v1, v2 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2 );

    // ========================================================================

    public void glProgramUniform3dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform3dv( program, location, count, value );
    }

    public void glProgramUniform3dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniform3dv( program, location, value.Length / 3, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3dv( GLuint program, GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glProgramUniform3ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2 )
    {
        _glProgramUniform3ui( program, location, v0, v1, v2 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2 );

    // ========================================================================

    public void glProgramUniform3uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform3uiv( program, location, count, value );
    }

    public void glProgramUniform3uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue = &value[ 0 ] )
        {
            _glProgramUniform3uiv( program, location, value.Length / 3, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform3uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform3uiv( GLuint program, GLint location, GLsizei count, GLuint* value );

    // ========================================================================

    public void glProgramUniform4i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glProgramUniform4i( program, location, v0, v1, v2, v3 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4i", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2, GLint v3 );

    // ========================================================================

    public void glProgramUniform4iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform4iv( program, location, count, value );
    }

    public void glProgramUniform4iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue = &value[ 0 ] )
        {
            _glProgramUniform4iv( program, location, value.Length / 4, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4iv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4iv( GLuint program, GLint location, GLsizei count, GLint* value );

    // ========================================================================

    public void glProgramUniform4f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        _glProgramUniform4f( program, location, v0, v1, v2, v3 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4f", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 );

    // ========================================================================

    public void glProgramUniform4fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform4fv( program, location, count, value );
    }

    public void glProgramUniform4fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniform4fv( program, location, value.Length / 4, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4fv( GLuint program, GLint location, GLsizei count, GLfloat* value );

    // ========================================================================

    public void glProgramUniform4d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2, GLdouble v3 )
    {
        _glProgramUniform4d( program, location, v0, v1, v2, v3 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2, GLdouble v3 );

    // ========================================================================

    public void glProgramUniform4dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform4dv( program, location, count, value );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4dv( GLuint program, GLint location, GLsizei count, GLdouble* value );

    // ========================================================================

    public void glProgramUniform4dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniform4dv( program, location, value.Length / 4, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4dv( GLuint program, GLint location, GLdouble[] value );

    // ========================================================================

    public void glProgramUniform4ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 )
    {
        _glProgramUniform4ui( program, location, v0, v1, v2, v3 );
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4ui", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 );

    // ========================================================================

    public void glProgramUniform4uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform4uiv( program, location, count, value );
    }

    public void glProgramUniform4uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4uiv( program, location, value.Length / 4, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniform4uiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniform4uiv( GLuint program, GLint location, GLsizei count, GLuint* value );

    // ========================================================================

    public void glProgramUniformMatrix2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2fv( program, location, value.Length / 4, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3fv( program, location, value.Length / 9, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix4fv( program, location, value.Length / 16, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2dv( program, location, value.Length / 4, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3dv( program, location, value.Length / 9, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4dv( program, location, value.Length / 16, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix2x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x3fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix2x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix2x3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix2x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix3x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x2fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix3x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix3x2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix3x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix2x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x4fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix2x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix2x4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix2x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix4x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x2fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix4x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix4x2fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix4x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix3x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x4fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix3x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix3x4fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix3x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix4x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x3fv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix4x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix4x3fv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix4x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value );

    // ========================================================================

    public void glProgramUniformMatrix2x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x3dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix2x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix2x3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix2x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix3x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x2dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix3x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix3x2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix3x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix2x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x4dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix2x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix2x4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix2x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix4x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x2dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix4x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix4x2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix4x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix3x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x4dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix3x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix3x4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix3x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glProgramUniformMatrix4x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x3dv( program, location, count, transpose, value );
    }

    public void glProgramUniformMatrix4x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glProgramUniformMatrix4x3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glProgramUniformMatrix4x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value );

    // ========================================================================

    public void glValidateProgramPipeline( GLuint pipeline )
    {
        _glValidateProgramPipeline( pipeline );
    }

    [DllImport( LIBGL, EntryPoint = "glValidateProgramPipeline", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glValidateProgramPipeline( GLuint pipeline );

    // ========================================================================

    public void glGetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetProgramPipelineInfoLog( pipeline, bufSize, length, infoLog );
    }

    public string glGetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize )
    {
        var infoLog = new GLchar[ bufSize ];

        fixed ( GLchar* pInfoLog = &infoLog[ 0 ] )
        {
            GLsizei length;

            _glGetProgramPipelineInfoLog( pipeline, bufSize, &length, pInfoLog );

            return new string( ( sbyte* )pInfoLog, 0, length, Encoding.UTF8 );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramPipelineInfoLog", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize, GLsizei* length, GLchar* infoLog );

    // ========================================================================

    public void glVertexAttribL1d( GLuint index, GLdouble x )
    {
        _glVertexAttribL1d( index, x );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL1d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL1d( GLuint index, GLdouble x );

    // ========================================================================

    public void glVertexAttribL2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttribL2d( index, x, y );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL2d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL2d( GLuint index, GLdouble x, GLdouble y );

    // ========================================================================

    public void glVertexAttribL3d( GLuint index, GLdouble x, GLdouble y, GLdouble z )
    {
        _glVertexAttribL3d( index, x, y, z );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL3d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL3d( GLuint index, GLdouble x, GLdouble y, GLdouble z );

    // ========================================================================

    public void glVertexAttribL4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glVertexAttribL4d( index, x, y, z, w );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL4d", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w );

    // ========================================================================

    public void glVertexAttribL1dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL1dv( index, v );
    }

    public void glVertexAttribL1dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL1dv( index, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL1dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL1dv( GLuint index, GLdouble* v );

    // ========================================================================

    public void glVertexAttribL2dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL2dv( index, v );
    }

    public void glVertexAttribL2dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL2dv( index, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL2dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL2dv( GLuint index, GLdouble* v );

    // ========================================================================

    public void glVertexAttribL3dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL3dv( index, v );
    }

    public void glVertexAttribL3dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL3dv( index, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL3dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL3dv( GLuint index, GLdouble* v );

    // ========================================================================

    public void glVertexAttribL4dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL4dv( index, v );
    }

    public void glVertexAttribL4dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL4dv( index, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribL4dv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribL4dv( GLuint index, GLdouble* v );

    // ========================================================================

    public void glVertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, pointer );
    }

    public void glVertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, GLsizei pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, ( void* )pointer );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribLPointer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer );

    // ========================================================================

    public void glGetVertexAttribLdv( GLuint index, GLenum pname, GLdouble* parameters )
    {
        _glGetVertexAttribLdv( index, pname, parameters );
    }

    public void glGetVertexAttribLdv( GLuint index, GLenum pname, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* pP = &parameters[ 0 ] )
        {
            _glGetVertexAttribLdv( index, pname, pP );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetVertexAttribLdv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetVertexAttribLdv( GLuint index, GLenum pname, GLdouble* parameters );

    // ========================================================================

    public void glViewportArrayv( GLuint first, GLsizei count, GLfloat* v )
    {
        _glViewportArrayv( first, count, v );
    }

    public void glViewportArrayv( GLuint first, GLsizei count, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportArrayv( first, count, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glViewportArrayv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glViewportArrayv( GLuint first, GLsizei count, GLfloat* v );

    // ========================================================================

    public void glViewportIndexedf( GLuint index, GLfloat x, GLfloat y, GLfloat w, GLfloat h )
    {
        _glViewportIndexedf( index, x, y, w, h );
    }

    [DllImport( LIBGL, EntryPoint = "glViewportIndexedf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glViewportIndexedf( GLuint index, GLfloat x, GLfloat y, GLfloat w, GLfloat h );

    // ========================================================================

    public void glViewportIndexedfv( GLuint index, GLfloat* v )
    {
        _glViewportIndexedfv( index, v );
    }

    public void glViewportIndexedfv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportIndexedfv( index, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glViewportIndexedfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glViewportIndexedfv( GLuint index, GLfloat* v );

    // ========================================================================

    public void glScissorArrayv( GLuint first, GLsizei count, GLint* v )
    {
        _glScissorArrayv( first, count, v );
    }

    public void glScissorArrayv( GLuint first, GLsizei count, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorArrayv( first, count, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glScissorArrayv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glScissorArrayv( GLuint first, GLsizei count, GLint* v );

    // ========================================================================

    public void glScissorIndexed( GLuint index, GLint left, GLint bottom, GLsizei width, GLsizei height )
    {
        _glScissorIndexed( index, left, bottom, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glScissorIndexed", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glScissorIndexed( GLuint index, GLint left, GLint bottom, GLsizei width, GLsizei height );

    // ========================================================================

    public void glScissorIndexedv( GLuint index, GLint* v )
    {
        _glScissorIndexedv( index, v );
    }

    public void glScissorIndexedv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorIndexedv( index, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glScissorIndexedv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glScissorIndexedv( GLuint index, GLint* v );

    // ========================================================================

    public void glDepthRangeArrayv( GLuint first, GLsizei count, GLdouble* v )
    {
        _glDepthRangeArrayv( first, count, v );
    }

    public void glDepthRangeArrayv( GLuint first, GLsizei count, params GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glDepthRangeArrayv( first, count, pV );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDepthRangeArrayv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthRangeArrayv( GLuint first, GLsizei count, GLdouble* v );

    // ========================================================================

    public void glDepthRangeIndexed( GLuint index, GLdouble n, GLdouble f )
    {
        _glDepthRangeIndexed( index, n, f );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthRangeIndexed", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthRangeIndexed( GLuint index, GLdouble n, GLdouble f );

    // ========================================================================

    public void glGetFloati_v( GLenum target, GLuint index, GLfloat* data )
    {
        _glGetFloati_v( target, index, data );
    }

    public void glGetFloati_v( GLenum target, GLuint index, ref GLfloat[] data )
    {
        fixed ( GLfloat* pData = &data[ 0 ] )
        {
            _glGetFloati_v( target, index, pData );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetFloati_v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetFloati_v( GLenum target, GLuint index, GLfloat* data );

    // ========================================================================

    public void glGetDoublei_v( GLenum target, GLuint index, GLdouble* data )
    {
        _glGetDoublei_v( target, index, data );
    }

    public void glGetDoublei_v( GLenum target, GLuint index, ref GLdouble[] data )
    {
        fixed ( GLdouble* pData = &data[ 0 ] )
        {
            _glGetDoublei_v( target, index, pData );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetDoublei_v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetDoublei_v( GLenum target, GLuint index, GLdouble* data );

    // ========================================================================

    public void glDrawArraysInstancedBaseInstance( GLenum mode, GLint first, GLsizei count, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawArraysInstancedBaseInstance( mode, first, count, instancecount, baseinstance );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawArraysInstancedBaseInstance", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawArraysInstancedBaseInstance( GLenum mode, GLint first, GLsizei count, GLsizei instancecount, GLuint baseinstance );

    // ========================================================================

    public void glDrawElementsInstancedBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
    }

    public void glDrawElementsInstancedBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLuint baseinstance ) where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseInstance( mode, count, type, p, instancecount, baseinstance );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawElementsInstancedBaseInstance", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawElementsInstancedBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLuint baseinstance );

    // ========================================================================

    public void glDrawElementsInstancedBaseVertexBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices,
                                                               GLsizei instancecount, GLint basevertex, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
    }

    public void glDrawElementsInstancedBaseVertexBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices,
                                                                    GLsizei instancecount, GLint basevertex, GLuint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, p, instancecount, basevertex, baseinstance );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawElementsInstancedBaseVertexBaseInstance", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawElementsInstancedBaseVertexBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices,
                                                                               GLsizei instancecount, GLint basevertex, GLuint baseinstance );

    // ========================================================================

    public void glGetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, GLint* parameters )
    {
        _glGetInternalformativ( target, internalformat, pname, bufSize, parameters );
    }

    public void glGetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetInternalformativ( target, internalformat, pname, bufSize, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetInternalformativ", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, GLint* parameters );

    // ========================================================================

    public void glGetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, GLint* parameters )
    {
        _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, parameters );
    }

    public void glGetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetActiveAtomicCounterBufferiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, GLint* parameters );

    // ========================================================================

    public void glBindImageTexture( GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum access, GLenum format )
    {
        _glBindImageTexture( unit, texture, level, layered, layer, access, format );
    }

    [DllImport( LIBGL, EntryPoint = "glBindImageTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindImageTexture( GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum access, GLenum format );

    // ========================================================================

    public void glMemoryBarrier( GLbitfield barriers )
    {
        _glMemoryBarrier( barriers );
    }

    [DllImport( LIBGL, EntryPoint = "glMemoryBarrier", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMemoryBarrier( GLbitfield barriers );

    // ========================================================================

    public void glTexStorage1D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width )
    {
        _glTexStorage1D( target, levels, internalformat, width );
    }

    [DllImport( LIBGL, EntryPoint = "glTexStorage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexStorage1D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width );

    // ========================================================================

    public void glTexStorage2D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glTexStorage2D( target, levels, internalformat, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glTexStorage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexStorage2D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height );

    // ========================================================================

    public void glTexStorage3D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glTexStorage3D( target, levels, internalformat, width, height, depth );
    }

    [DllImport( LIBGL, EntryPoint = "glTexStorage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexStorage3D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth );

    // ========================================================================

    public void glDrawTransformFeedbackInstanced( GLenum mode, GLuint id, GLsizei instancecount )
    {
        _glDrawTransformFeedbackInstanced( mode, id, instancecount );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawTransformFeedbackInstanced", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawTransformFeedbackInstanced( GLenum mode, GLuint id, GLsizei instancecount );

    // ========================================================================

    public void glDrawTransformFeedbackStreamInstanced( GLenum mode, GLuint id, GLuint stream, GLsizei instancecount )
    {
        _glDrawTransformFeedbackStreamInstanced( mode, id, stream, instancecount );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawTransformFeedbackStreamInstanced", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawTransformFeedbackStreamInstanced( GLenum mode, GLuint id, GLuint stream, GLsizei instancecount );

    // ========================================================================

    public void glGetPointerv( GLenum pname, void** parameters )
    {
        _glGetPointerv( pname, parameters );
    }

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

    [DllImport( LIBGL, EntryPoint = "glGetPointerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetPointerv( GLenum pname, void** parameters );

    // ========================================================================

    public void glClearBufferData( GLenum target, GLenum internalformat, GLenum format, GLenum type, void* data )
    {
        _glClearBufferData( target, internalformat, format, type, data );
    }

    public void glClearBufferData< T >( GLenum target, GLenum internalformat, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferData( target, internalformat, format, type, t );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glClearBufferData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearBufferData( GLenum target, GLenum internalformat, GLenum format, GLenum type, void* data );

    // ========================================================================

    public void glClearBufferSubData( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data )
    {
        _glClearBufferSubData( target, internalformat, offset, size, format, type, data );
    }

    public void glClearBufferSubData< T >( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferSubData( target, internalformat, offset, size, format, type, t );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glClearBufferSubData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearBufferSubData( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data );

    // ========================================================================

    public void glDispatchCompute( GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z )
    {
        _glDispatchCompute( num_groups_x, num_groups_y, num_groups_z );
    }

    [DllImport( LIBGL, EntryPoint = "glDispatchCompute", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDispatchCompute( GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z );

    // ========================================================================

    public struct DispatchIndirectCommand
    {
        public uint num_groups_x;
        public uint num_groups_y;
        public uint num_groups_z;
    }

    public void glDispatchComputeIndirect( void* indirect )
    {
        _glDispatchComputeIndirect( indirect );
    }

    public void glDispatchComputeIndirect( DispatchIndirectCommand indirect )
    {
        _glDispatchComputeIndirect( &indirect );
    }

    [DllImport( LIBGL, EntryPoint = "glDispatchComputeIndirect", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDispatchComputeIndirect( void* indirect );

    // ========================================================================

    public void glCopyImageSubData( GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ,
                                    GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY,
                                    GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth )
    {
        _glCopyImageSubData( srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyImageSubData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyImageSubData( GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ,
                                                    GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY,
                                                    GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth );

    // ========================================================================

    public void glFramebufferParameteri( GLenum target, GLenum pname, GLint param )
    {
        _glFramebufferParameteri( target, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glFramebufferParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFramebufferParameteri( GLenum target, GLenum pname, GLint param );

    // ========================================================================

    public void glGetFramebufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetFramebufferParameteriv( target, pname, parameters );
    }

    public void glGetFramebufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p_parameters = &parameters[ 0 ] )
        {
            _glGetFramebufferParameteriv( target, pname, p_parameters );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetFramebufferParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetFramebufferParameteriv( GLenum target, GLenum pname, GLint* parameters );

    // ========================================================================

    public void glGetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, GLint64* parameters )
    {
        _glGetInternalformati64v( target, internalformat, pname, count, parameters );
    }

    public void glGetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, ref GLint64[] parameters )
    {
        fixed ( GLint64* p_params = &parameters[ 0 ] )
        {
            _glGetInternalformati64v( target, internalformat, pname, count, p_params );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetInternalformati64v", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, GLint64* parameters );

    // ========================================================================

    public void glInvalidateTexSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glInvalidateTexSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth );
    }

    [DllImport( LIBGL, EntryPoint = "glInvalidateTexSubImage", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glInvalidateTexSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset,
                                                         GLint zoffset, GLsizei width, GLsizei height, GLsizei depth );

    // ========================================================================

    public void glInvalidateTexImage( GLuint texture, GLint level )
    {
        _glInvalidateTexImage( texture, level );
    }

    [DllImport( LIBGL, EntryPoint = "glInvalidateTexImage", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glInvalidateTexImage( GLuint texture, GLint level );

    // ========================================================================

    public void glInvalidateBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr length )
    {
        _glInvalidateBufferSubData( buffer, offset, length );
    }

    [DllImport( LIBGL, EntryPoint = "glInvalidateBufferSubData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glInvalidateBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr length );

    // ========================================================================

    public void glInvalidateBufferData( GLuint buffer )
    {
        _glInvalidateBufferData( buffer );
    }

    [DllImport( LIBGL, EntryPoint = "glInvalidateBufferData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glInvalidateBufferData( GLuint buffer );

    // ========================================================================

    public void glInvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments )
    {
        _glInvalidateFramebuffer( target, numAttachments, attachments );
    }

    public void glInvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments )
    {
        fixed ( GLenum* p_attachments = &attachments[ 0 ] )
        {
            _glInvalidateFramebuffer( target, numAttachments, p_attachments );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glInvalidateFramebuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glInvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments );

    // ========================================================================

    public void glInvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glInvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
    }

    public void glInvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        fixed ( GLenum* p_attachments = &attachments[ 0 ] )
        {
            _glInvalidateSubFramebuffer( target, numAttachments, p_attachments, x, y, width, height );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glInvalidateSubFramebuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glInvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height );

    // ========================================================================

    public void glMultiDrawArraysIndirect( GLenum mode, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, indirect, drawcount, stride );
    }

    public void glMultiDrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, ( void* )&indirect, drawcount, stride );
    }

    [DllImport( LIBGL, EntryPoint = "glMultiDrawArraysIndirect", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMultiDrawArraysIndirect( GLenum mode, void* indirect, GLsizei drawcount, GLsizei stride );

    // ========================================================================

    public void glMultiDrawElementsIndirect( GLenum mode, GLenum type, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
    }

    public void glMultiDrawElementsIndirect( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, ( void* )&indirect, drawcount, stride );
    }

    [DllImport( LIBGL, EntryPoint = "glMultiDrawElementsIndirect", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMultiDrawElementsIndirect( GLenum mode, GLenum type, void* indirect, GLsizei drawcount, GLsizei stride );

    // ========================================================================

    public void glGetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, GLint* parameters )
    {
        _glGetProgramInterfaceiv( program, programInterface, pname, parameters );
    }

    public void glGetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p_parameters = &parameters[ 0 ] )
        {
            _glGetProgramInterfaceiv( program, programInterface, pname, p_parameters );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramInterfaceiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, GLint* parameters );

    // ========================================================================

    public GLuint glGetProgramResourceIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceIndex( program, programInterface, name );
    }

    public GLuint glGetProgramResourceIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceIndex( program, programInterface, p_name );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramResourceIndex", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glGetProgramResourceIndex( GLuint program, GLenum programInterface, GLchar* name );

    // ========================================================================

    public void glGetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei* length, GLchar* name )
    {
        _glGetProgramResourceName( program, programInterface, index, bufSize, length, name );
    }

    public string glGetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize )
    {
        var     name = new GLchar[ bufSize ];

        fixed ( GLchar* p_name = &name[ 0 ] )
        {
            GLsizei length;
            
            _glGetProgramResourceName( program, programInterface, index, bufSize, &length, p_name );

            return new string( ( sbyte* )p_name, 0, length, Encoding.UTF8 );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramResourceName", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei* length, GLchar* name );

    // ========================================================================

    public void glGetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLsizei propCount,
                                        GLenum* props, GLsizei bufSize, GLsizei* length, GLint* parameters )
    {
        _glGetProgramResourceiv( program, programInterface, index, propCount, props, bufSize, length, parameters );
    }

    public void glGetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLenum[] props, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLenum* p_props = &props[ 0 ] )
        fixed ( GLint* p_params = &parameters[ 0 ] )
        {
            GLsizei length;
            _glGetProgramResourceiv( program, programInterface, index, props.Length, p_props, bufSize, &length, p_params );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramResourceiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLsizei propCount,
                                                        GLenum* props, GLsizei bufSize, GLsizei* length, GLint* parameters );

    // ========================================================================

    public GLint glGetProgramResourceLocation( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocation( program, programInterface, name );
    }

    public GLint glGetProgramResourceLocation( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocation( program, programInterface, p_name );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramResourceLocation", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetProgramResourceLocation( GLuint program, GLenum programInterface, GLchar* name );

    // ========================================================================

    public GLint glGetProgramResourceLocationIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocationIndex( program, programInterface, name );
    }

    public GLint glGetProgramResourceLocationIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocationIndex( program, programInterface, p_name );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetProgramResourceLocationIndex", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLint _glGetProgramResourceLocationIndex( GLuint program, GLenum programInterface, GLchar* name );

    // ========================================================================

    public void glShaderStorageBlockBinding( GLuint program, GLuint storageBlockIndex, GLuint storageBlockBinding )
    {
        _glShaderStorageBlockBinding( program, storageBlockIndex, storageBlockBinding );
    }

    [DllImport( LIBGL, EntryPoint = "glShaderStorageBlockBinding", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glShaderStorageBlockBinding( GLuint program, GLuint storageBlockIndex, GLuint storageBlockBinding );

    // ========================================================================

    public void glTexBufferRange( GLenum target, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTexBufferRange( target, internalformat, buffer, offset, size );
    }

    [DllImport( LIBGL, EntryPoint = "glTexBufferRange", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexBufferRange( GLenum target, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size );

    // ========================================================================

    public void glTexStorage2DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations )
    {
        _glTexStorage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
    }

    [DllImport( LIBGL, EntryPoint = "glTexStorage2DMultisample", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexStorage2DMultisample( GLenum target, GLsizei samples, GLenum internalformat,
                                                           GLsizei width, GLsizei height, GLboolean fixedsamplelocations );

    // ========================================================================

    public void glTexStorage3DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width,
                                           GLsizei height, GLsizei depth, GLboolean fixedsamplelocations )
    {
        _glTexStorage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

    [DllImport( LIBGL, EntryPoint = "glTexStorage3DMultisample", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexStorage3DMultisample( GLenum target,
                                                           GLsizei samples,
                                                           GLenum internalformat,
                                                           GLsizei width,
                                                           GLsizei height,
                                                           GLsizei depth,
                                                           GLboolean fixedsamplelocations );

    // ========================================================================

    public void glTextureView( GLuint texture,
                               GLenum target,
                               GLuint origtexture,
                               GLenum internalformat, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers )
    {
        _glTextureView( texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers );
    }

    [DllImport( LIBGL, EntryPoint = "glTextureView", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTextureView( GLuint texture, GLenum target, GLuint origtexture, GLenum internalformat, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers );

    // ========================================================================

    public void glBindVertexBuffer( GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )
    {
        _glBindVertexBuffer( bindingindex, buffer, offset, stride );
    }

    [DllImport( LIBGL, EntryPoint = "glBindVertexBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindVertexBuffer( GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride );

    // ========================================================================

    public void glVertexAttribFormat( GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset )
    {
        _glVertexAttribFormat( attribindex, size, type, normalized, relativeoffset );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribFormat", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribFormat( GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset );

    // ========================================================================

    public void glVertexAttribIFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribIFormat( attribindex, size, type, relativeoffset );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribIFormat", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribIFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset );

    // ========================================================================

    public void glVertexAttribLFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribLFormat( attribindex, size, type, relativeoffset );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribLFormat", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribLFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset );

    // ========================================================================

    public void glVertexAttribBinding( GLuint attribindex, GLuint bindingindex )
    {
        _glVertexAttribBinding( attribindex, bindingindex );
    }

    [DllImport( LIBGL, EntryPoint = "glVertexAttribBinding", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glVertexAttribBinding( GLuint attribindex, GLuint bindingindex );

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

    [DllImport( LIBGL, EntryPoint = "glDebugMessageControl", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDebugMessageControl( GLenum source, GLenum type, GLenum severity, GLsizei count, GLuint* ids, GLboolean enabled );

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
            var pstart = p_messageLogBytes;
            var ret    = _glGetDebugMessageLog( count, bufSize, p_sources, p_types, p_ids, p_severities, p_lengths, p_messageLogBytes );

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
    
    // ========================================================================
}

#pragma warning restore IDE0079    // Remove unnecessary suppression
#pragma warning restore CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore CS8603     // Possible null reference return.
#pragma warning restore IDE0060    // Remove unused parameter.
#pragma warning restore IDE1006    // Naming Styles.
#pragma warning restore IDE0090    // Use 'new(...)'.
#pragma warning restore CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning restore SYSLIB1054 // 
