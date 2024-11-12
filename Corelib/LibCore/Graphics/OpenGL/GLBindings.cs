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

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning disable CS8603  // Possible null reference return.
#pragma warning disable IDE0060 // Remove unused parameter.
#pragma warning disable IDE1006 // Naming Styles.
#pragma warning disable IDE0090 // Use 'new(...)'.
#pragma warning disable CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type

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

// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------

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
[SuppressMessage( "Interoperability", "SYSLIB1054:Use \'LibraryImportAttribute\' instead of \'DllImportAttribute\' to generate P/Invoke marshalling code at compile time" )]
public unsafe class GLBindings : IGLBindings
{
    public const string LIBGL   = "opengl32.dll";
    public const string LIBGLFW = "glfw3.dll";

    /// <summary>
    /// The null pointer, just like in C/C++.
    /// </summary>
    public readonly void* NULL = ( void* )0;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCullFace( GLenum mode )
    {
        _glCullFace( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glCullFace", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCullFace( GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFrontFace( GLenum mode )
    {
        _glFrontFace( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glFrontFace", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFrontFace( GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glHint( GLenum target, GLenum mode )
    {
        _glHint( target, mode );
    }

    [DllImport( LIBGL, EntryPoint = "glHint", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glHint( GLenum target, GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glLineWidth( GLfloat width )
    {
        _glLineWidth( width );
    }

    [DllImport( LIBGL, EntryPoint = "glLineWidth", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLineWidth( GLfloat width );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPointSize( GLfloat size )
    {
        _glPointSize( size );
    }

    [DllImport( LIBGL, EntryPoint = "glPointSize", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointSize( GLfloat size );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPolygonMode( GLenum face, GLenum mode )
    {
        _glPolygonMode( face, mode );
    }

    [DllImport( LIBGL, EntryPoint = "glPolygonMode", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPolygonMode( GLenum face, GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glScissor( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glScissor( x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glScissor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glScissor( GLint x, GLint y, GLsizei width, GLsizei height );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameterf( GLenum target, GLenum pname, GLfloat param )
    {
        _glTexParameterf( target, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameterf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameterf( GLenum target, GLenum pname, GLfloat param );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameteri( GLenum target, GLenum pname, GLint param )
    {
        _glTexParameteri( target, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameteri( GLenum target, GLenum pname, GLint param );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawBuffer( GLenum buf )
    {
        _glDrawBuffer( buf );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawBuffer( GLenum buf );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClear( GLbitfield mask )
    {
        _glClear( mask );
    }

    [DllImport( LIBGL, EntryPoint = "glClear", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClear( GLbitfield mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        _glClearColor( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glClearColor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearStencil( GLint s )
    {
        _glClearStencil( s );
    }

    [DllImport( LIBGL, EntryPoint = "glClearStencil", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearStencil( GLint s );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearDepth( GLdouble depth )
    {
        _glClearDepth( depth );
    }

    [DllImport( LIBGL, EntryPoint = "glClearDepth", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearDepth( GLdouble depth );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilMask( GLuint mask )
    {
        _glStencilMask( mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilMask( GLuint mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glColorMask( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha )
    {
        _glColorMask( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glColorMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glColorMask( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthMask( GLboolean flag )
    {
        _glDepthMask( flag );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthMask( GLboolean flag );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDisable( GLenum cap )
    {
        _glDisable( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glDisable", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDisable( GLenum cap );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEnable( GLenum cap )
    {
        _glEnable( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glEnable", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEnable( GLenum cap );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFinish()
    {
        _glFinish();
    }

    [DllImport( LIBGL, EntryPoint = "glFinish", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFinish();

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFlush()
    {
        _glFlush();
    }

    [DllImport( LIBGL, EntryPoint = "glFlush", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFlush();

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendFunc( GLenum sfactor, GLenum dfactor )
    {
        _glBlendFunc( sfactor, dfactor );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFunc( GLenum sfactor, GLenum dfactor );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glLogicOp( GLenum opcode )
    {
        _glLogicOp( opcode );
    }

    [DllImport( LIBGL, EntryPoint = "glLogicOp", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLogicOp( GLenum opcode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilFunc( GLenum func, GLint @ref, GLuint mask )
    {
        _glStencilFunc( func, @ref, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilFunc( GLenum func, GLint @ref, GLuint mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilOp( GLenum fail, GLenum zfail, GLenum zpass )
    {
        _glStencilOp( fail, zfail, zpass );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilOp", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilOp( GLenum fail, GLenum zfail, GLenum zpass );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthFunc( GLenum func )
    {
        _glDepthFunc( func );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthFunc( GLenum func );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPixelStoref( GLenum pname, GLfloat param )
    {
        _glPixelStoref( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPixelStoref", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPixelStoref( GLenum pname, GLfloat param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPixelStorei( GLenum pname, GLint param )
    {
        _glPixelStorei( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPixelStorei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPixelStorei( GLenum pname, GLint param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glReadBuffer( GLenum src )
    {
        _glReadBuffer( src );
    }

    [DllImport( LIBGL, EntryPoint = "glReadBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glReadBuffer( GLenum src );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLenum glGetError()
    {
        return _glGetError();
    }

    [DllImport( LIBGL, EntryPoint = "glGetError", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLenum _glGetError();

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLubyte* glGetString( GLenum name )
    {
        return _glGetString( name );
    }

    [DllImport( LIBGL, EntryPoint = "glGetString", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLubyte* _glGetString( GLenum name );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsEnabled( GLenum cap )
    {
        return _glIsEnabled( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glIsEnabled", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsEnabled( GLenum cap );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthRange( GLdouble near, GLdouble far )
    {
        _glDepthRange( near, far );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthRange", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthRange( GLdouble near, GLdouble far );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glViewport( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glViewport( x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glViewport", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glViewport( GLint x, GLint y, GLsizei width, GLsizei height );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawArrays( GLenum mode, GLint first, GLsizei count )
    {
        _glDrawArrays( mode, first, count );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawArrays", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawArrays( GLenum mode, GLint first, GLsizei count );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPolygonOffset( GLfloat factor, GLfloat units )
    {
        _glPolygonOffset( factor, units );
    }

    [DllImport( LIBGL, EntryPoint = "glPolygonOffset", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPolygonOffset( GLfloat factor, GLfloat units );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexImage1D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border )
    {
        _glCopyTexImage1D( target, level, internalformat, x, y, width, border );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexImage1D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexImage2D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border )
    {
        _glCopyTexImage2D( target, level, internalformat, x, y, width, height, border );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexImage2D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width )
    {
        _glCopyTexSubImage1D( target, level, xoffset, x, y, width );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexSubImage2D( GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage2D( GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindTexture( GLenum target, GLuint texture )
    {
        _glBindTexture( target, texture );
    }

    [DllImport( LIBGL, EntryPoint = "glBindTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindTexture( GLenum target, GLuint texture );
    
    // ------------------------------------------------------------------------

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
    
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsTexture( GLuint texture )
    {
        return _glIsTexture( texture );
    }

    [DllImport( LIBGL, EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsTexture( GLuint texture );
    
    // ------------------------------------------------------------------------

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
    
    // ------------------------------------------------------------------------

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
        
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glActiveTexture( GLenum texture )
    {
        _glActiveTexture( texture );
    }
    
    [DllImport( LIBGL, EntryPoint = "glActiveTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glActiveTexture( GLenum texture );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSampleCoverage( GLfloat value, GLboolean invert )
    {
        _glSampleCoverage( value, invert );
    }

    [DllImport( LIBGL, EntryPoint = "glSampleCoverage", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSampleCoverage( GLfloat value, GLboolean invert );
    
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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
    
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendFuncSeparate( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha )
    {
        _glBlendFuncSeparate( sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFuncSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFuncSeparate( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha );
    
    // ------------------------------------------------------------------------

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
    
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------
    
    /// <inheritdoc/>
    public void glPointParameterf( GLenum pname, GLfloat param )
    {
        _glPointParameterf( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameterf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameterf( GLenum pname, GLfloat param );

    // ------------------------------------------------------------------------

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
    
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPointParameteri( GLenum pname, GLint param )
    {
        _glPointParameteri( pname, param );
    }
    
    [DllImport( LIBGL, EntryPoint = "glPointParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameteri( GLenum pname, GLint param );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        _glBlendColor( red, green, blue, alpha );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBlenColor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendEquation( GLenum mode )
    {
        _glBlendEquation( mode );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBlendEquation", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquation( GLenum mode );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsQuery( GLuint id )
    {
        return _glIsQuery( id );
    }
    
    [DllImport( LIBGL, EntryPoint = "glIsQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsQuery( GLuint id );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBeginQuery( GLenum target, GLuint id )
    {
        _glBeginQuery( target, id );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBeginQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBeginQuery( GLenum target, GLuint id );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEndQuery( GLenum target )
    {
        _glEndQuery( target );
    }
    
    [DllImport( LIBGL, EntryPoint = "glEndQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEndQuery( GLenum target );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBuffer( GLenum target, GLuint buffer )
    {
        _glBindBuffer( target, buffer );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBindBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindBuffer( GLenum target, GLuint buffer );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsBuffer( GLuint buffer )
    {
        return _glIsBuffer( buffer );
    }
    
    [DllImport( LIBGL, EntryPoint = "glIsBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsBuffer( GLuint buffer );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glUnmapBuffer( GLenum target )
    {
        return _glUnmapBuffer( target );
    }
    
    [DllImport( LIBGL, EntryPoint = "glUnmapBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glUnmapBuffer( GLenum target );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendEquationSeparate( GLenum modeRGB, GLenum modeAlpha )
    {
        _glBlendEquationSeparate( modeRGB, modeAlpha );
    }
    
    [DllImport( LIBGL, EntryPoint = "glBlendEquationSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquationSeparate( GLenum modeRGB, GLenum modeAlpha );

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilOpSeparate( GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass )
    {
        _glStencilOpSeparate( face, sfail, dpfail, dppass );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilFuncSeparate( GLenum face, GLenum func, GLint @ref, GLuint mask )
    {
        _glStencilFuncSeparate( face, func, @ref, mask );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilMaskSeparate( GLenum face, GLuint mask )
    {
        _glStencilMaskSeparate( face, mask );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glAttachShader( GLuint program, GLuint shader )
    {
        _glAttachShader( program, shader );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindAttribLocation( GLuint program, GLuint index, GLchar* name )
    {
        _glBindAttribLocation( program, index, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindAttribLocation( GLuint program, GLuint index, string name )
    {
        var utf8 = Encoding.UTF8.GetBytes( name );

        fixed ( byte* putf8 = &utf8[ 0 ] )
        {
            _glBindAttribLocation( program, index, putf8 );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompileShader( GLuint shader )
    {
        _glCompileShader( shader );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateProgram()
    {
        return _glCreateProgram();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateShader( GLenum type )
    {
        return _glCreateShader( type );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteProgram( GLuint program )
    {
        _glDeleteProgram( program );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteShader( GLuint shader )
    {
        _glDeleteShader( shader );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDetachShader( GLuint program, GLuint shader )
    {
        _glDetachShader( program, shader );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDisableVertexAttribArray( GLuint index )
    {
        _glDisableVertexAttribArray( index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEnableVertexAttribArray( GLuint index )
    {
        _glEnableVertexAttribArray( index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveAttrib( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name )
    {
        _glGetActiveAttrib( program, index, bufSize, length, size, type, name );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveUniform( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name )
    {
        _glGetActiveUniform( program, index, bufSize, length, size, type, name );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetAttachedShaders( GLuint program, GLsizei maxCount, GLsizei* count, GLuint* shaders )
    {
        _glGetAttachedShaders( program, maxCount, count, shaders );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetAttribLocation( GLuint program, GLchar* name )
    {
        return _glGetAttribLocation( program, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetAttribLocation( GLuint program, string name )
    {
        fixed ( GLchar* pname = Encoding.UTF8.GetBytes( name ) )
        {
            return _glGetAttribLocation( program, pname );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramiv( GLuint program, GLenum pname, GLint* parameters )
    {
        _glGetProgramiv( program, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramiv( GLuint program, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetProgramiv( program, pname, pparams );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramInfoLog( GLuint program, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetProgramInfoLog( program, bufSize, length, infoLog );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetProgramInfoLog( GLuint program, GLsizei bufSize )
    {
        var     infoLog = stackalloc GLchar[ bufSize ];
        GLsizei len;
        _glGetProgramInfoLog( program, bufSize, &len, infoLog );

        return new string( ( sbyte* )infoLog, 0, len, Encoding.UTF8 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderiv( GLuint shader, GLenum pname, GLint* parameters )
    {
        _glGetShaderiv( shader, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderiv( GLuint shader, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetShaderiv( shader, pname, pparams );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderInfoLog( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetShaderInfoLog( shader, bufSize, length, infoLog );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetShaderInfoLog( GLuint shader, GLsizei bufSize )
    {
        var     infoLog = stackalloc GLchar[ bufSize ];
        GLsizei len;
        _glGetShaderInfoLog( shader, bufSize, &len, infoLog );

        return new string( ( sbyte* )infoLog, 0, len, Encoding.UTF8 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderSource( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* source )
    {
        _glGetShaderSource( shader, bufSize, length, source );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetShaderSource( GLuint shader, GLsizei bufSize = 4096 )
    {
        var     source = stackalloc GLchar[ bufSize ];
        GLsizei len;
        _glGetShaderSource( shader, bufSize, &len, source );

        return new string( ( sbyte* )source, 0, len, Encoding.UTF8 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetUniformLocation( GLuint program, GLchar* name )
    {
        return _glGetUniformLocation( program, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetUniformLocation( GLuint program, string name )
    {
        fixed ( byte* pname = Encoding.UTF8.GetBytes( name ) )
        {
            return _glGetUniformLocation( program, pname );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformfv( GLuint program, GLint location, GLfloat* parameters )
    {
        _glGetUniformfv( program, location, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformfv( GLuint program, GLint location, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* pparams =
                   &parameters[ 0 ] )
        {
            _glGetUniformfv( program, location, pparams );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformiv( GLuint program, GLint location, GLint* parameters )
    {
        _glGetUniformiv( program, location, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformiv( GLuint program, GLint location, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetUniformiv( program, location, pparams );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribdv( GLuint index, GLenum pname, GLdouble* parameters )
    {
        _glGetVertexAttribdv( index, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribdv( GLuint index, GLenum pname, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribdv( index, pname, pparams );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribfv( GLuint index, GLenum pname, GLfloat* parameters )
    {
        _glGetVertexAttribfv( index, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribfv( GLuint index, GLenum pname, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribfv( index, pname, pparams );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribiv( GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetVertexAttribiv( index, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribiv( GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribiv( index, pname, pparams );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribPointerv( GLuint index, GLenum pname, void** pointer )
    {
        _glGetVertexAttribPointerv( index, pname, pointer );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsProgram( GLuint program )
    {
        return _glIsProgram( program );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsShader( GLuint shader )
    {
        return _glIsShader( shader );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glLinkProgram( GLuint program )
    {
        _glLinkProgram( program );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glShaderSource( GLuint shader, GLsizei count, GLchar** @string, GLint* length )
    {
        _glShaderSource( shader, count, @string, length );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUseProgram( GLuint program )
    {
        _glUseProgram( program );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1f( GLint location, GLfloat v0 )
    {
        _glUniform1f( location, v0 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2f( GLint location, GLfloat v0, GLfloat v1 )
    {
        _glUniform2f( location, v0, v1 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        _glUniform3f( location, v0, v1, v2 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        _glUniform4f( location, v0, v1, v2, v3 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1i( GLint location, GLint v0 )
    {
        _glUniform1i( location, v0 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2i( GLint location, GLint v0, GLint v1 )
    {
        _glUniform2i( location, v0, v1 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3i( GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glUniform3i( location, v0, v1, v2 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4i( GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glUniform4i( location, v0, v1, v2, v3 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform1fv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform1fv( location, value.Length, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform2fv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform2fv( location, value.Length / 2, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform3fv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform3fv( location, value.Length / 3, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform4fv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform4fv( location, value.Length / 4, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform1iv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform1iv( location, value.Length, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform2iv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform2iv( location, value.Length / 2, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform3iv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform3iv( location, value.Length / 3, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform4iv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform4iv( location, value.Length / 4, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2fv( location, value.Length / 4, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix3fv( location, value.Length / 9, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4fv( location, value.Length / 16, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glValidateProgram( GLuint program )
    {
        return _glValidateProgram( program );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1d( GLuint index, GLdouble x )
    {
        _glVertexAttrib1d( index, x );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib1dv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib1dv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1f( GLuint index, GLfloat x )
    {
        _glVertexAttrib1f( index, x );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib1fv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib1fv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1s( GLuint index, GLshort x )
    {
        _glVertexAttrib1s( index, x );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib1sv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib1sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib1sv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttrib2d( index, x, y );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib2dv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib2dv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2f( GLuint index, GLfloat x, GLfloat y )
    {
        _glVertexAttrib2f( index, x, y );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib2fv( index, v );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib2sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib2sv( index, v );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib3dv( index, v );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib3fv( index, v );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib3sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib3sv( index, v );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4Nubv( GLuint index, GLubyte* v )
    {
        _glVertexAttrib4Nubv( index, v );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib4dv( index, v );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib4fv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib4fv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4iv( GLuint index, GLint* v )
    {
        _glVertexAttrib4iv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4iv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttrib4iv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4s( GLuint index, GLshort x, GLshort y, GLshort z, GLshort w )
    {
        _glVertexAttrib4s( index, x, y, z, w );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib4sv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib4sv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4ubv( GLuint index, GLubyte* v )
    {
        _glVertexAttrib4ubv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4ubv( GLuint index, params GLubyte[] v )
    {
        fixed ( GLubyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4ubv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4uiv( GLuint index, GLuint* v )
    {
        _glVertexAttrib4uiv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4uiv( GLuint index, params GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttrib4uiv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4usv( GLuint index, GLushort* v )
    {
        _glVertexAttrib4usv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttrib4usv( GLuint index, params GLushort[] v )
    {
        fixed ( GLushort* p = &v[ 0 ] )
        {
            _glVertexAttrib4usv( index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribPointer( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, void* pointer )
    {
        _glVertexAttribPointer( index, size, type, normalized, stride, pointer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribPointer( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, uint pointer )
    {
        _glVertexAttribPointer( index, size, type, normalized, stride, ( void* )pointer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2x3fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2x3fv( location, value.Length / 6, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3x2fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix3x2fv( location, value.Length / 6, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2x4fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2x4fv( location, value.Length / 8, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4x2fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4x2fv( location, value.Length / 8, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3x4fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniformMatrix3x4fv( location, value.Length / 12, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4x3fv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4x3fv( location, value.Length / 12, transpose, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glColorMaski( GLuint index, GLboolean r, GLboolean g, GLboolean b, GLboolean a )
    {
        _glColorMaski( index, r, g, b, a );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBooleani_v( GLenum target, GLuint index, GLboolean* data )
    {
        _glGetBooleani_v( target, index, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBooleani_v( GLenum target, GLuint index, ref GLboolean[] data )
    {
        fixed ( GLboolean* p = &data[ 0 ] )
        {
            _glGetBooleani_v( target, index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetIntegeri_v( GLenum target, GLuint index, GLint* data )
    {
        _glGetIntegeri_v( target, index, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetIntegeri_v( GLenum target, GLuint index, ref GLint[] data )
    {
        fixed ( GLint* p = &data[ 0 ] )
        {
            _glGetIntegeri_v( target, index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEnablei( GLenum target, GLuint index )
    {
        _glEnablei( target, index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDisablei( GLenum target, GLuint index )
    {
        _glDisablei( target, index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsEnabledi( GLenum target, GLuint index )
    {
        return _glIsEnabledi( target, index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBeginTransformFeedback( GLenum primitiveMode )
    {
        _glBeginTransformFeedback( primitiveMode );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEndTransformFeedback()
    {
        _glEndTransformFeedback();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBufferRange( GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glBindBufferRange( target, index, buffer, offset, size );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBufferBase( GLenum target, GLuint index, GLuint buffer )
    {
        _glBindBufferBase( target, index, buffer );
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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glWaitSync( void* sync, GLbitfield flags, GLuint64 timeout )
    {
        _glWaitSync( sync, flags, timeout );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glWaitSyncSafe( IntPtr sync, GLbitfield flags, GLuint64 timeout )
    {
        _glWaitSync( sync.ToPointer(), flags, timeout );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInteger64v( GLenum pname, GLint64* data )
    {
        _glGetInteger64v( pname, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInteger64v( GLenum pname, ref GLint64[] data )
    {
        fixed ( GLint64* dp = &data[ 0 ] )
        {
            _glGetInteger64v( pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSynciv( void* sync, GLenum pname, GLsizei bufSize, GLsizei* length, GLint* values )
    {
        _glGetSynciv( sync, pname, bufSize, length, values );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInteger64i_v( GLenum target, GLuint index, GLint64* data )
    {
        _glGetInteger64i_v( target, index, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInteger64i_v( GLenum target, GLuint index, ref GLint64[] data )
    {
        fixed ( GLint64* dp = &data[ 0 ] )
        {
            _glGetInteger64i_v( target, index, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBufferParameteri64v( GLenum target, GLenum pname, GLint64* parameters )
    {
        _glGetBufferParameteri64v( target, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBufferParameteri64v( GLenum target, GLenum pname, ref GLint64[] parameters )
    {
        fixed ( GLint64* dp = &parameters[ 0 ] )
        {
            _glGetBufferParameteri64v( target, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFramebufferTexture( GLenum target, GLenum attachment, GLuint texture, GLint level )
    {
        _glFramebufferTexture( target, attachment, texture, level );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetMultisamplefv( GLenum pname, GLuint index, GLfloat* val )
    {
        _glGetMultisamplefv( pname, index, val );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetMultisamplefvSafe( GLenum pname, GLuint index, ref GLfloat[] val )
    {
        fixed ( GLfloat* dp = &val[ 0 ] )
        {
            _glGetMultisamplefv( pname, index, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSampleMaski( GLuint maskNumber, GLbitfield mask )
    {
        _glSampleMaski( maskNumber, mask );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindFragDataLocationIndexed( GLuint program, GLuint colorNumber, GLuint index, GLchar* name )
    {
        _glBindFragDataLocationIndexed( program, colorNumber, index, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindFragDataLocationIndexed( GLuint program, GLuint colorNumber, GLuint index, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            _glBindFragDataLocationIndexed( program, colorNumber, index, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetFragDataIndex( GLuint program, GLchar* name )
    {
        return _glGetFragDataIndex( program, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetFragDataIndex( GLuint program, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetFragDataIndex( program, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenSamplers( GLsizei count, GLuint* samplers )
    {
        _glGenSamplers( count, samplers );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGenSampler()
    {
        return glGenSamplers( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteSamplers( GLsizei count, GLuint* samplers )
    {
        _glDeleteSamplers( count, samplers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteSamplers( params GLuint[] samplers )
    {
        fixed ( GLuint* dp = &samplers[ 0 ] )
        {
            _glDeleteSamplers( samplers.Length, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsSampler( GLuint sampler )
    {
        return _glIsSampler( sampler );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindSampler( GLuint unit, GLuint sampler )
    {
        _glBindSampler( unit, sampler );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameteri( GLuint sampler, GLenum pname, GLint param )
    {
        _glSamplerParameteri( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameteriv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glSamplerParameteriv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameteriv( GLuint sampler, GLenum pname, GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glSamplerParameteriv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameterf( GLuint sampler, GLenum pname, GLfloat param )
    {
        _glSamplerParameterf( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param )
    {
        _glSamplerParameterfv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat[] param )
    {
        fixed ( GLfloat* dp = &param[ 0 ] )
        {
            _glSamplerParameterfv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glSamplerParameterIiv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameterIiv( GLuint sampler, GLenum pname, GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glSamplerParameterIiv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param )
    {
        _glSamplerParameterIuiv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* dp =
                   &param[ 0 ] )
        {
            _glSamplerParameterIuiv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameteriv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glGetSamplerParameteriv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameteriv( GLuint sampler, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameteriv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glGetSamplerParameterIiv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameterIiv( GLuint sampler, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameterIiv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param )
    {
        _glGetSamplerParameterfv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameterfv( GLuint sampler, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* dp =
                   &param[ 0 ] )
        {
            _glGetSamplerParameterfv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param )
    {
        _glGetSamplerParameterIuiv( sampler, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetSamplerParameterIuiv( GLuint sampler, GLenum pname, ref GLuint[] param )
    {
        fixed ( GLuint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameterIuiv( sampler, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glQueryCounter( GLuint id, GLenum target )
    {
        _glQueryCounter( id, target );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryObjecti64v( GLuint id, GLenum pname, GLint64* param )
    {
        _glGetQueryObjecti64v( id, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryObjecti64v( GLuint id, GLenum pname, ref GLint64[] param )
    {
        fixed ( GLint64* dp =
                   &param[ 0 ] )
        {
            _glGetQueryObjecti64v( id, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryObjectui64v( GLuint id, GLenum pname, GLuint64* param )
    {
        _glGetQueryObjectui64v( id, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryObjectui64v( GLuint id, GLenum pname, ref GLuint64[] param )
    {
        fixed ( GLuint64* dp = &param[ 0 ] )
        {
            _glGetQueryObjectui64v( id, pname, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribDivisor( GLuint index, GLuint divisor )
    {
        _glVertexAttribDivisor( index, divisor );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP1ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP1ui( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP1uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP1uiv( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP1uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP1uiv( index, type, normalized, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP2ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP2ui( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP2uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP2uiv( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP2uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP2uiv( index, type, normalized, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP3ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP3ui( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP3uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP3uiv( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP3uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP3uiv( index, type, normalized, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP4ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP4ui( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP4uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP4uiv( index, type, normalized, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribP4uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp =
                   &value[ 0 ] )
        {
            _glVertexAttribP4uiv( index, type, normalized, dp );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMinSampleShading( GLfloat value )
    {
        _glMinSampleShading( value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendEquationi( GLuint buf, GLenum mode )
    {
        _glBlendEquationi( buf, mode );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendEquationSeparatei( GLuint buf, GLenum modeRGB, GLenum modeAlpha )
    {
        _glBlendEquationSeparatei( buf, modeRGB, modeAlpha );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendFunci( GLuint buf, GLenum src, GLenum dst )
    {
        _glBlendFunci( buf, src, dst );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendFuncSeparatei( GLuint buf, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha )
    {
        _glBlendFuncSeparatei( buf, srcRGB, dstRGB, srcAlpha, dstAlpha );
    }

// ------------------------------------------------------------------------

    public struct DrawArraysIndirectCommand
    {
        public GLuint count;
        public GLuint primCount;
        public GLuint first;
        public GLuint reservedMustBeZero;
    }

    public struct DrawArraysIndirectCommand
    {
        public GLuint count;
        public GLuint instanceCount;
        public GLuint first;
        public GLuint baseInstance;
    }

    /// <inheritdoc/>
    public void glDrawArraysIndirect( GLenum mode, void* indirect )
    {
        _glDrawArraysIndirect( mode, indirect );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect )
    {
        _glDrawArraysIndirect( mode, &indirect );
    }

// ------------------------------------------------------------------------

//    public struct DrawElementsIndirectCommand
//    {
//        public GLuint count;
//        public GLuint primCount;
//        public GLuint firstIndex;
//        public GLuint baseVertex;
//        public GLuint reservedMustBeZero;
//    }

    public struct DrawElementsIndirectCommand
    {
        public GLuint count;
        public GLuint primCount;
        public GLuint firstIndex;
        public GLuint baseVertex;
        public GLuint baseInstance;
    }

    /// <inheritdoc/>
    public void glDrawElementsIndirect( GLenum mode, GLenum type, void* indirect )
    {
        _glDrawElementsIndirect( mode, type, indirect );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsIndirect( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect )
    {
        _glDrawElementsIndirect( mode, type, &indirect );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1d( GLint location, GLdouble x )
    {
        _glUniform1d( location, x );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2d( GLint location, GLdouble x, GLdouble y )
    {
        _glUniform2d( location, x, y );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3d( GLint location, GLdouble x, GLdouble y, GLdouble z )
    {
        _glUniform3d( location, x, y, z );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4d( GLint location, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glUniform4d( location, x, y, z, w );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform1dv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform1dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] ) _glUniform1dv( location, value.Length, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform2dv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform2dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] ) _glUniform2dv( location, value.Length / 2, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform3dv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform3dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] ) _glUniform3dv( location, value.Length / 3, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform4dv( location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniform4dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] ) _glUniform4dv( location, value.Length / 4, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix2dv( location, value.Length / 4, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix3dv( location, value.Length / 9, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix4dv( location, value.Length / 16, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2x3dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix2x3dv( location, value.Length / 6, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2x4dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix2x4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix2x4dv( location, value.Length / 8, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3x2dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix3x2dv( location, value.Length / 6, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3x4dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix3x4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix3x4dv( location, value.Length / 12, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4x2dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix4x2dv( location, value.Length / 8, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4x3dv( location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformMatrix4x3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p =
                   &value[ 0 ] ) _glUniformMatrix4x3dv( location, value.Length / 12, transpose, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformdv( GLuint program, GLint location, GLdouble* parameters )
    {
        _glGetUniformdv( program, location, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformdv( GLuint program, GLint location, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* p =
                   &parameters[ 0 ] ) _glGetUniformdv( program, location, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetSubroutineUniformLocation( GLuint program, GLenum shadertype, GLchar* name )
    {
        return _glGetSubroutineUniformLocation( program, shadertype, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetSubroutineUniformLocation( GLuint program, GLenum shadertype, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetSubroutineUniformLocation( program, shadertype, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetSubroutineIndex( GLuint program, GLenum shadertype, GLchar* name )
    {
        return _glGetSubroutineIndex( program, shadertype, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetSubroutineIndex( GLuint program, GLenum shadertype, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetSubroutineIndex( program, shadertype, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveSubroutineUniformiv( GLuint program, GLenum shadertype, GLuint index, GLenum pname, GLint* values )
    {
        _glGetActiveSubroutineUniformiv( program, shadertype, index, pname, values );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveSubroutineUniformiv( GLuint program, GLenum shadertype, GLuint index, GLenum pname, ref GLint[] values )
    {
        fixed ( GLint* p =
                   &values[ 0 ] ) _glGetActiveSubroutineUniformiv( program, shadertype, index, pname, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveSubroutineUniformName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name )
    {
        _glGetActiveSubroutineUniformName( program, shadertype, index, bufsize, length, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetActiveSubroutineUniformName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize )
    {
        var name = new GLchar[ bufsize ];
        GLsizei  length;

        fixed ( GLchar* p = &name[ 0 ] )
        {
            _glGetActiveSubroutineUniformName( program, shadertype, index, bufsize, &length, p );

            return new string( ( sbyte* )p, 0, length, Encoding.UTF8 );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveSubroutineName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name )
    {
        _glGetActiveSubroutineName( program, shadertype, index, bufsize, length, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetActiveSubroutineName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize )
    {
        var name = new GLchar[ bufsize ];
        GLsizei  length;

        fixed ( GLchar* p = &name[ 0 ] )
        {
            _glGetActiveSubroutineName( program, shadertype, index, bufsize, &length, p );

            return new string( ( sbyte* )p, 0, length, Encoding.UTF8 );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformSubroutinesuiv( GLenum shadertype, GLsizei count, GLuint* indices )
    {
        _glUniformSubroutinesuiv( shadertype, count, indices );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUniformSubroutinesuiv( GLenum shadertype, GLuint[] indices )
    {
        fixed ( GLuint* p =
                   &indices[ 0 ] ) _glUniformSubroutinesuiv( shadertype, indices.Length, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformSubroutineuiv( GLenum shadertype, GLint location, GLuint* parameters )
    {
        _glGetUniformSubroutineuiv( shadertype, location, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetUniformSubroutineuiv( GLenum shadertype, GLint location, ref GLuint[] parameters )
    {
        fixed ( GLuint* p =
                   &parameters[ 0 ] ) _glGetUniformSubroutineuiv( shadertype, location, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramStageiv( GLuint program, GLenum shadertype, GLenum pname, GLint* values )
    {
        _glGetProgramStageiv( program, shadertype, pname, values );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramStageiv( GLuint program, GLenum shadertype, GLenum pname, ref GLint[] values )
    {
        fixed ( GLint* p =
                   &values[ 0 ] ) _glGetProgramStageiv( program, shadertype, pname, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPatchParameteri( GLenum pname, GLint value )
    {
        _glPatchParameteri( pname, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPatchParameterfv( GLenum pname, GLfloat* values )
    {
        _glPatchParameterfv( pname, values );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPatchParameterfv( GLenum pname, GLfloat[] values )
    {
        fixed ( GLfloat* p = &values[ 0 ] ) _glPatchParameterfv( pname, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindTransformFeedback( GLenum target, GLuint id )
    {
        _glBindTransformFeedback( target, id );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glDeleteTransformFeedbacks( n, ids );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteTransformFeedbacks( params GLuint[] ids )
    {
        fixed ( GLuint* p = &ids[ 0 ] ) _glDeleteTransformFeedbacks( ids.Length, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glGenTransformFeedbacks( n, ids );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glGenTransformFeedbacks( GLsizei n )
    {
        var r = new GLuint[ n ];
        fixed ( GLuint* p = &r[ 0 ] ) _glGenTransformFeedbacks( n, p );

        return r;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGenTransformFeedback()
    {
        return glGenTransformFeedbacks( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsTransformFeedback( GLuint id )
    {
        return _glIsTransformFeedback( id );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPauseTransformFeedback()
    {
        _glPauseTransformFeedback();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glResumeTransformFeedback()
    {
        _glResumeTransformFeedback();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawTransformFeedback( GLenum mode, GLuint id )
    {
        _glDrawTransformFeedback( mode, id );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawTransformFeedbackStream( GLenum mode, GLuint id, GLuint stream )
    {
        _glDrawTransformFeedbackStream( mode, id, stream );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBeginQueryIndexed( GLenum target, GLuint index, GLuint id )
    {
        _glBeginQueryIndexed( target, index, id );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEndQueryIndexed( GLenum target, GLuint index )
    {
        _glEndQueryIndexed( target, index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryIndexediv( GLenum target, GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetQueryIndexediv( target, index, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryIndexediv( GLenum target, GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p =
                   &parameters[ 0 ] ) _glGetQueryIndexediv( target, index, pname, p );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glReleaseShaderCompiler()
    {
        _glReleaseShaderCompiler();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glShaderBinary( GLsizei count, GLuint* shaders, GLenum binaryformat, void* binary, GLsizei length )
    {
        _glShaderBinary( count, shaders, binaryformat, binary, length );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glShaderBinary( GLuint[] shaders, GLenum binaryformat, byte[] binary )
    {
        var count = shaders.Length;

        fixed ( GLuint* pShaders = &shaders[ 0 ] )
        fixed ( byte* pBinary = &binary[ 0 ] )
        {
            _glShaderBinary( count, pShaders, binaryformat, pBinary, binary.Length );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderPrecisionFormat( GLenum shaderType, GLenum precisionType, GLint* range, GLint* precision )
    {
        _glGetShaderPrecisionFormat( shaderType, precisionType, range, precision );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetShaderPrecisionFormat( GLenum shaderType, GLenum precisionType, ref GLint[] range, ref GLint precision )
    {
        range = new GLint[ 2 ];

        fixed ( GLint* pRange = &range[ 0 ] )
        fixed ( GLint* pPrecision = &precision )
        {
            _glGetShaderPrecisionFormat( shaderType, precisionType, pRange, pPrecision );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthRangef( GLfloat n, GLfloat f )
    {
        _glDepthRangef( n, f );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearDepthf( GLfloat d )
    {
        _glClearDepthf( d );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramBinary( GLuint program, GLsizei bufSize, GLsizei* length, GLenum* binaryFormat, void* binary )
    {
        _glGetProgramBinary( program, bufSize, length, binaryFormat, binary );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public byte[] glGetProgramBinary( GLuint program, GLsizei bufSize, out GLenum binaryFormat )
    {
        var  binary = new byte[ bufSize ];
        GLsizei length;

        fixed ( byte* pBinary = &binary[ 0 ] )
        fixed ( GLenum* pBinaryFormat = &binaryFormat )
        {
            _glGetProgramBinary( program, bufSize, &length, pBinaryFormat, pBinary );
        }

        Array.Resize( ref binary, length );

        return binary;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramBinary( GLuint program, GLenum binaryFormat, void* binary, GLsizei length )
    {
        _glProgramBinary( program, binaryFormat, binary, length );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramBinary( GLuint program, GLenum binaryFormat, byte[] binary )
    {
        fixed ( byte* pBinary = &binary[ 0 ] )
        {
            _glProgramBinary( program, binaryFormat, pBinary, binary.Length );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramParameteri( GLuint program, GLenum pname, GLint value )
    {
        _glProgramParameteri( program, pname, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glUseProgramStages( GLuint pipeline, GLbitfield stages, GLuint program )
    {
        _glUseProgramStages( pipeline, stages, program );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glActiveShaderProgram( GLuint pipeline, GLuint program )
    {
        _glActiveShaderProgram( pipeline, program );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateShaderProgramv( GLenum type, GLsizei count, GLchar** strings )
    {
        return _glCreateShaderProgramv( type, count, strings );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindProgramPipeline( GLuint pipeline )
    {
        _glBindProgramPipeline( pipeline );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glDeleteProgramPipelines( n, pipelines );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteProgramPipelines( params GLuint[] pipelines )
    {
        fixed ( GLuint* pPipelines =
                   &pipelines[ 0 ] )
        {
            _glDeleteProgramPipelines( pipelines.Length, pPipelines );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glGenProgramPipelines( n, pipelines );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGenProgramPipeline()
    {
        return glGenProgramPipelines( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsProgramPipeline( GLuint pipeline )
    {
        return _glIsProgramPipeline( pipeline );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramPipelineiv( GLuint pipeline, GLenum pname, GLint* param )
    {
        _glGetProgramPipelineiv( pipeline, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramPipelineiv( GLuint pipeline, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* pParam =
                   &param[ 0 ] )
        {
            _glGetProgramPipelineiv( pipeline, pname, pParam );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1i( GLuint program, GLint location, GLint v0 )
    {
        _glProgramUniform1i( program, location, v0 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform1iv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1iv( program, location, value.Length, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1f( GLuint program, GLint location, GLfloat v0 )
    {
        _glProgramUniform1f( program, location, v0 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform1fv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1fv( program, location, value.Length, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1d( GLuint program, GLint location, GLdouble v0 )
    {
        _glProgramUniform1d( program, location, v0 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform1dv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1dv( program, location, value.Length, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1ui( GLuint program, GLint location, GLuint v0 )
    {
        _glProgramUniform1ui( program, location, v0 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform1uiv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform1uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform1uiv( program, location, value.Length, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2i( GLuint program, GLint location, GLint v0, GLint v1 )
    {
        _glProgramUniform2i( program, location, v0, v1 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform2iv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2iv( program, location, value.Length / 2, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2f( GLuint program, GLint location, GLfloat v0, GLfloat v1 )
    {
        _glProgramUniform2f( program, location, v0, v1 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform2fv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2fv( program, location, value.Length / 2, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2d( GLuint program, GLint location, GLdouble v0, GLdouble v1 )
    {
        _glProgramUniform2d( program, location, v0, v1 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform2dv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2dv( program, location, value.Length / 2, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2ui( GLuint program, GLint location, GLuint v0, GLuint v1 )
    {
        _glProgramUniform2ui( program, location, v0, v1 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform2uiv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform2uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2uiv( program, location, value.Length / 2, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glProgramUniform3i( program, location, v0, v1, v2 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform3iv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3iv( program, location, value.Length / 3, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        _glProgramUniform3f( program, location, v0, v1, v2 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform3fv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3fv( program, location, value.Length / 3, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2 )
    {
        _glProgramUniform3d( program, location, v0, v1, v2 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform3dv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3dv( program, location, value.Length / 3, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2 )
    {
        _glProgramUniform3ui( program, location, v0, v1, v2 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform3uiv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform3uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform3uiv( program, location, value.Length / 3, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glProgramUniform4i( program, location, v0, v1, v2, v3 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform4iv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4iv( program, location, value.Length / 4, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        _glProgramUniform4f( program, location, v0, v1, v2, v3 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform4fv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4fv( program, location, value.Length / 4, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2, GLdouble v3 )
    {
        _glProgramUniform4d( program, location, v0, v1, v2, v3 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform4dv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4dv( program, location, value.Length / 4, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 )
    {
        _glProgramUniform4ui( program, location, v0, v1, v2, v3 );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform4uiv( program, location, count, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniform4uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4uiv( program, location, value.Length / 4, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2fv( program, location, value.Length / 4, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3fv( program, location, value.Length / 9, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix4fv( program, location, value.Length / 16, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2dv( program, location, value.Length / 4, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3dv( program, location, value.Length / 9, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4dv( program, location, value.Length / 16, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x3fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x2fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x4fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x2fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x4fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x3fv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x3dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x2dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x4dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix2x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x2dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x4dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix3x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x3dv( program, location, count, transpose, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glProgramUniformMatrix4x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glValidateProgramPipeline( GLuint pipeline )
    {
        _glValidateProgramPipeline( pipeline );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetProgramPipelineInfoLog( pipeline, bufSize, length, infoLog );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize )
    {
        var infoLog = new GLchar[ bufSize ];
        GLsizei  length;

        fixed ( GLchar* pInfoLog = &infoLog[ 0 ] )
        {
            _glGetProgramPipelineInfoLog( pipeline, bufSize, &length, pInfoLog );

            return new string( ( sbyte* )pInfoLog, 0, length, Encoding.UTF8 );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL1d( GLuint index, GLdouble x )
    {
        _glVertexAttribL1d( index, x );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttribL2d( index, x, y );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL3d( GLuint index, GLdouble x, GLdouble y, GLdouble z )
    {
        _glVertexAttribL3d( index, x, y, z );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glVertexAttribL4d( index, x, y, z, w );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL1dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL1dv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL1dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL1dv( index, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL2dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL2dv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL2dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL2dv( index, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL3dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL3dv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL3dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL3dv( index, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL4dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL4dv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribL4dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL4dv( index, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, pointer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, GLsizei pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, ( void* )pointer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribLdv( GLuint index, GLenum pname, GLdouble* parameters )
    {
        _glGetVertexAttribLdv( index, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexAttribLdv( GLuint index, GLenum pname, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* pP = &parameters[ 0 ] )
        {
            _glGetVertexAttribLdv( index, pname, pP );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glViewportArrayv( GLuint first, GLsizei count, GLfloat* v )
    {
        _glViewportArrayv( first, count, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glViewportArrayv( GLuint first, GLsizei count, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportArrayv( first, count, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glViewportIndexedf( GLuint index, GLfloat x, GLfloat y, GLfloat w, GLfloat h )
    {
        _glViewportIndexedf( index, x, y, w, h );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glViewportIndexedfv( GLuint index, GLfloat* v )
    {
        _glViewportIndexedfv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glViewportIndexedfv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportIndexedfv( index, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glScissorArrayv( GLuint first, GLsizei count, GLint* v )
    {
        _glScissorArrayv( first, count, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glScissorArrayv( GLuint first, GLsizei count, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorArrayv( first, count, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glScissorIndexed( GLuint index, GLint left, GLint bottom, GLsizei width, GLsizei height )
    {
        _glScissorIndexed( index, left, bottom, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glScissorIndexedv( GLuint index, GLint* v )
    {
        _glScissorIndexedv( index, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glScissorIndexedv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorIndexedv( index, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthRangeArrayv( GLuint first, GLsizei count, GLdouble* v )
    {
        _glDepthRangeArrayv( first, count, v );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthRangeArrayv( GLuint first, GLsizei count, params GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glDepthRangeArrayv( first, count, pV );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthRangeIndexed( GLuint index, GLdouble n, GLdouble f )
    {
        _glDepthRangeIndexed( index, n, f );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetFloati_v( GLenum target, GLuint index, GLfloat* data )
    {
        _glGetFloati_v( target, index, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetFloati_v( GLenum target, GLuint index, ref GLfloat[] data )
    {
        fixed ( GLfloat* pData = &data[ 0 ] )
        {
            _glGetFloati_v( target, index, pData );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetDoublei_v( GLenum target, GLuint index, GLdouble* data )
    {
        _glGetDoublei_v( target, index, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetDoublei_v( GLenum target, GLuint index, ref GLdouble[] data )
    {
        fixed ( GLdouble* pData = &data[ 0 ] )
        {
            _glGetDoublei_v( target, index, pData );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawArraysInstancedBaseInstance( GLenum mode, GLint first, GLsizei count, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawArraysInstancedBaseInstance( mode, first, count, instancecount, baseinstance );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstancedBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstancedBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLuint baseinstance ) where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseInstance( mode, count, type, p, instancecount, baseinstance );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstancedBaseVertexBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLint basevertex, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElementsInstancedBaseVertexBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLint basevertex, GLuint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, p, instancecount, basevertex, baseinstance );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, GLint* parameters )
    {
        _glGetInternalformativ( target, internalformat, pname, bufSize, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetInternalformativ( target, internalformat, pname, bufSize, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, GLint* parameters )
    {
        _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, p );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindImageTexture( GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum access, GLenum format )
    {
        _glBindImageTexture( unit, texture, level, layered, layer, access, format );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMemoryBarrier( GLbitfield barriers )
    {
        _glMemoryBarrier( barriers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexStorage1D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width )
    {
        _glTexStorage1D( target, levels, internalformat, width );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexStorage2D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glTexStorage2D( target, levels, internalformat, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexStorage3D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glTexStorage3D( target, levels, internalformat, width, height, depth );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawTransformFeedbackInstanced( GLenum mode, GLuint id, GLsizei instancecount )
    {
        _glDrawTransformFeedbackInstanced( mode, id, instancecount );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawTransformFeedbackStreamInstanced( GLenum mode, GLuint id, GLuint stream, GLsizei instancecount )
    {
        _glDrawTransformFeedbackStreamInstanced( mode, id, stream, instancecount );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetPointerv( GLenum pname, void** parameters )
    {
        _glGetPointerv( pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetPointerv( GLenum pname, ref IntPtr[] parameters )
    {
        var length = parameters.Length;

        var  ptr = parameters[ 0 ].ToPointer();
        var p   = &ptr;
        _glGetPointerv( pname, p );

        for ( var i = 0; i < length; i++ )
        {
            parameters[ i ] = new IntPtr( *p );
            p++;
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferData( GLenum target, GLenum internalformat, GLenum format, GLenum type, void* data )
    {
        _glClearBufferData( target, internalformat, format, type, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferData< T >( GLenum target, GLenum internalformat, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferData( target, internalformat, format, type, t );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferSubData( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data )
    {
        _glClearBufferSubData( target, internalformat, offset, size, format, type, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearBufferSubData< T >( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferSubData( target, internalformat, offset, size, format, type, t );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDispatchCompute( GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z )
    {
        _glDispatchCompute( num_groups_x, num_groups_y, num_groups_z );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDispatchComputeIndirect( void* indirect )
    {
        _glDispatchComputeIndirect( indirect );
    }

// ------------------------------------------------------------------------

    public struct DispatchIndirectCommand
    {
        public uint num_groups_x;
        public uint num_groups_y;
        public uint num_groups_z;
    }

    /// <inheritdoc/>
    public void glDispatchComputeIndirect( DispatchIndirectCommand indirect )
    {
        _glDispatchComputeIndirect( &indirect );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyImageSubData( GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ, GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY,
                                    GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth )
    {
        _glCopyImageSubData( srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFramebufferParameteri( GLenum target, GLenum pname, GLint param )
    {
        _glFramebufferParameteri( target, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetFramebufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetFramebufferParameteriv( target, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetFramebufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p_parameters = &parameters[ 0 ] )
        {
            _glGetFramebufferParameteriv( target, pname, p_parameters );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, GLint64* parameters )
    {
        _glGetInternalformati64v( target, internalformat, pname, count, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, ref GLint64[] parameters )
    {
        fixed ( GLint64* p_params = &parameters[ 0 ] )
        {
            _glGetInternalformati64v( target, internalformat, pname, count, p_params );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateTexSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glInvalidateTexSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateTexImage( GLuint texture, GLint level )
    {
        _glInvalidateTexImage( texture, level );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr length )
    {
        _glInvalidateBufferSubData( buffer, offset, length );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateBufferData( GLuint buffer )
    {
        _glInvalidateBufferData( buffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments )
    {
        _glInvalidateFramebuffer( target, numAttachments, attachments );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments )
    {
        fixed ( GLenum* p_attachments = &attachments[ 0 ] )
        {
            _glInvalidateFramebuffer( target, numAttachments, p_attachments );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glInvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        fixed ( GLenum* p_attachments = &attachments[ 0 ] )
        {
            _glInvalidateSubFramebuffer( target, numAttachments, p_attachments, x, y, width, height );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawArraysIndirect( GLenum mode, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, indirect, drawcount, stride );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, ( void* )&indirect, drawcount, stride );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawElementsIndirect( GLenum mode, GLenum type, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawElementsIndirect( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, ( void* )&indirect, drawcount, stride );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, GLint* parameters )
    {
        _glGetProgramInterfaceiv( program, programInterface, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p_parameters = &parameters[ 0 ] )
        {
            _glGetProgramInterfaceiv( program, programInterface, pname, p_parameters );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetProgramResourceIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceIndex( program, programInterface, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetProgramResourceIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceIndex( program, programInterface, p_name );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei* length, GLchar* name )
    {
        _glGetProgramResourceName( program, programInterface, index, bufSize, length, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize )
    {
        var name = new GLchar[ bufSize ];
        GLsizei  length;

        fixed ( GLchar* p_name = &name[ 0 ] )
        {
            _glGetProgramResourceName( program, programInterface, index, bufSize, &length, p_name );

            return new string( ( sbyte* )p_name, 0, length, Encoding.UTF8 );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLsizei propCount, GLenum* props, GLsizei bufSize, GLsizei* length, GLint* parameters )
    {
        _glGetProgramResourceiv( program, programInterface, index, propCount, props, bufSize, length, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLenum[] props, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLenum* p_props = &props[ 0 ] )
        fixed ( GLint* p_params = &parameters[ 0 ] )
        {
            GLsizei length;
            _glGetProgramResourceiv( program, programInterface, index, props.Length, p_props, bufSize, &length, p_params );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetProgramResourceLocation( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocation( program, programInterface, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetProgramResourceLocation( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocation( program, programInterface, p_name );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetProgramResourceLocationIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocationIndex( program, programInterface, name );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLint glGetProgramResourceLocationIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p_name = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocationIndex( program, programInterface, p_name );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glShaderStorageBlockBinding( GLuint program, GLuint storageBlockIndex, GLuint storageBlockBinding )
    {
        _glShaderStorageBlockBinding( program, storageBlockIndex, storageBlockBinding );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexBufferRange( GLenum target, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTexBufferRange( target, internalformat, buffer, offset, size );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexStorage2DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations )
    {
        _glTexStorage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexStorage3DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedsamplelocations )
    {
        _glTexStorage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureView( GLuint texture, GLenum target, GLuint origtexture, GLenum internalformat, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers )
    {
        _glTextureView( texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindVertexBuffer( GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )
    {
        _glBindVertexBuffer( bindingindex, buffer, offset, stride );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribFormat( GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset )
    {
        _glVertexAttribFormat( attribindex, size, type, normalized, relativeoffset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribIFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribIFormat( attribindex, size, type, relativeoffset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribLFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribLFormat( attribindex, size, type, relativeoffset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexAttribBinding( GLuint attribindex, GLuint bindingindex )
    {
        _glVertexAttribBinding( attribindex, bindingindex );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexBindingDivisor( GLuint bindingindex, GLuint divisor )
    {
        _glVertexBindingDivisor( bindingindex, divisor );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDebugMessageControl( GLenum source, GLenum type, GLenum severity, GLsizei count, GLuint* ids, GLboolean enabled )
    {
        _glDebugMessageControl( source, type, severity, count, ids, enabled );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDebugMessageControl( GLenum source, GLenum type, GLenum severity, GLuint[] ids, GLboolean enabled )
    {
        fixed ( GLuint* p_ids = ids )
        {
            _glDebugMessageControl( source, type, severity, ids.Length, p_ids, enabled );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDebugMessageInsert( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* buf )
    {
        _glDebugMessageInsert( source, type, id, severity, length, buf );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDebugMessageInsert( GLenum source, GLenum type, GLuint id, GLenum severity, string buf )
    {
        var bufBytes = Encoding.UTF8.GetBytes( buf );

        fixed ( GLchar* p_bufBytes = bufBytes )
        {
            _glDebugMessageInsert( source, type, id, severity, bufBytes.Length, p_bufBytes );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDebugMessageCallback( GLDEBUGPROC callback, void* userParam )
    {
        _glDebugMessageCallback( callback, userParam );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDebugMessageCallback( GLDEBUGPROCSAFE callback, void* userParam )
    {
        GLDEBUGPROC callbackUnsafe = ( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* message, void* userParam ) =>
        {
            var messageString = new string( ( sbyte* )message, 0, length, Encoding.UTF8 );
            callback( source, type, id, severity, messageString, userParam );
        };

        _glDebugMessageCallback( callbackUnsafe, userParam );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetDebugMessageLog( GLuint count, GLsizei bufsize, GLenum* sources, GLenum* types, GLuint* ids, GLenum* severities, GLsizei* lengths, GLchar* messageLog )
    {
        return _glGetDebugMessageLog( count, bufsize, sources, types, ids, severities, lengths, messageLog );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glGetDebugMessageLog( GLuint count, GLsizei bufSize, out GLenum[] sources, out GLenum[] types, out GLuint[] ids, out GLenum[] severities, out string[] messageLog )
    {
        sources    = new GLenum[ count ];
        types      = new GLenum[ count ];
        ids        = new GLuint[ count ];
        severities = new GLenum[ count ];

        var  messageLogBytes = new GLchar[ bufSize ];
        var lengths         = new GLsizei[ count ];

        fixed ( GLenum* p_sources = &sources[ 0 ] )
        fixed ( GLenum* p_types = &types[ 0 ] )
        fixed ( GLuint* p_ids = &ids[ 0 ] )
        fixed ( GLenum* p_severities = &severities[ 0 ] )
        fixed ( GLsizei* p_lengths = &lengths[ 0 ] )
        fixed ( GLchar* p_messageLogBytes = &messageLogBytes[ 0 ] )
        {
            var pstart = p_messageLogBytes;
            GLuint  ret    = _glGetDebugMessageLog( count, bufSize, p_sources, p_types, p_ids, p_severities, p_lengths, p_messageLogBytes );
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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPushDebugGroup( GLenum source, GLuint id, GLsizei length, GLchar* message )
    {
        _glPushDebugGroup( source, id, length, message );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPushDebugGroup( GLenum source, GLuint id, string message )
    {
        var messageBytes = Encoding.UTF8.GetBytes( message );

        fixed ( GLchar* p_messageBytes = messageBytes )
        {
            _glPushDebugGroup( source, id, messageBytes.Length, p_messageBytes );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPopDebugGroup()
    {
        _glPopDebugGroup();
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glObjectLabel( GLenum identifier, GLuint name, GLsizei length, GLchar* label )
    {
        _glObjectLabel( identifier, name, length, label );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glObjectLabel( GLenum identifier, GLuint name, string label )
    {
        var labelBytes = Encoding.UTF8.GetBytes( label );

        fixed ( GLchar* p_labelBytes = labelBytes )
        {
            _glObjectLabel( identifier, name, labelBytes.Length, p_labelBytes );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetObjectLabel( GLenum identifier, GLuint name, GLsizei bufSize, GLsizei* length, GLchar* label )
    {
        _glGetObjectLabel( identifier, name, bufSize, length, label );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glObjectPtrLabel( void* ptr, GLsizei length, GLchar* label )
    {
        _glObjectPtrLabel( ptr, length, label );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glObjectPtrLabel( IntPtr ptr, string label )
    {
        var labelBytes = Encoding.UTF8.GetBytes( label );

        fixed ( GLchar* p_labelBytes = labelBytes )
        {
            _glObjectPtrLabel( ptr.ToPointer(), labelBytes.Length, p_labelBytes );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetObjectPtrLabel( void* ptr, GLsizei bufSize, GLsizei* length, GLchar* label )
    {
        _glGetObjectPtrLabel( ptr, bufSize, length, label );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBufferStorage( GLenum target, GLsizeiptr size, void* data, GLbitfield flags )
    {
        _glBufferStorage( target, size, data, flags );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBufferStorage< T >( GLenum target, T[] data, GLbitfield flags ) where T : unmanaged
    {
        fixed ( void* p_data = &data[ 0 ] )
        {
            _glBufferStorage( target, data.Length * sizeof( T ), p_data, flags );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearTexImage( GLuint texture, GLint level, GLenum format, GLenum type, void* data )
    {
        _glClearTexImage( texture, level, format, type, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearTexImage< T >( GLuint texture, GLint level, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearTexImage( texture, level, format, type, t );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearTexSubImage( GLuint texture, GLint level, GLint xOffset, GLint yOffset, GLint zOffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* data )
    {
        _glClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearTexSubImage< T >( GLuint texture, GLint level, GLint xOffset, GLint yOffset, GLint zOffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, T[] data )
        where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, t );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBuffersBase( GLenum target, GLuint first, GLsizei count, GLuint* buffers )
    {
        _glBindBuffersBase( target, first, count, buffers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBuffersBase( GLenum target, GLuint first, GLuint[] buffers )
    {
        fixed ( GLuint* p_buffers = &buffers[ 0 ] )
        {
            _glBindBuffersBase( target, first, buffers.Length, p_buffers );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBuffersRange( GLenum target, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizeiptr* sizes )
    {
        _glBindBuffersRange( target, first, count, buffers, offsets, sizes );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindTextures( GLuint first, GLsizei count, GLuint* textures )
    {
        _glBindTextures( first, count, textures );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindTextures( GLuint first, GLuint[] textures )
    {
        fixed ( GLuint* p_textures = &textures[ 0 ] )
        {
            _glBindTextures( first, textures.Length, p_textures );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindSamplers( GLuint first, GLsizei count, GLuint* samplers )
    {
        _glBindSamplers( first, count, samplers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindSamplers( GLuint first, GLuint[] samplers )
    {
        fixed ( GLuint* p_samplers = &samplers[ 0 ] )
        {
            _glBindSamplers( first, samplers.Length, p_samplers );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindImageTextures( GLuint first, GLsizei count, GLuint* textures )
    {
        _glBindImageTextures( first, count, textures );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindImageTextures( GLuint first, GLuint[] textures )
    {
        fixed ( GLuint* p_textures = &textures[ 0 ] )
        {
            _glBindImageTextures( first, textures.Length, p_textures );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindVertexBuffers( GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides )
    {
        _glBindVertexBuffers( first, count, buffers, offsets, strides );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClipControl( GLenum origin, GLenum depth )
    {
        _glClipControl( origin, depth );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glCreateTransformFeedbacks( n, ids );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateTransformFeedbacks()
    {
        return glCreateTransformFeedbacks( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTransformFeedbackBufferBase( GLuint xfb, GLuint index, GLuint buffer )
    {
        _glTransformFeedbackBufferBase( xfb, index, buffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTransformFeedbackBufferRange( GLuint xfb, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTransformFeedbackBufferRange( xfb, index, buffer, offset, size );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTransformFeedbackiv( GLuint xfb, GLenum pname, GLint* param )
    {
        _glGetTransformFeedbackiv( xfb, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTransformFeedbackiv( GLuint xfb, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* p_param = &param[ 0 ] )
        {
            _glGetTransformFeedbackiv( xfb, pname, p_param );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTransformFeedbacki_v( GLuint xfb, GLenum pname, GLuint index, GLint* param )
    {
        _glGetTransformFeedbacki_v( xfb, pname, index, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTransformFeedbacki_v( GLuint xfb, GLenum pname, GLuint index, ref GLint[] param )
    {
        fixed ( GLint* p_param = &param[ 0 ] )
        {
            _glGetTransformFeedbacki_v( xfb, pname, index, p_param );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTransformFeedbacki64_v( GLuint xfb, GLenum pname, GLuint index, GLint64* param )
    {
        _glGetTransformFeedbacki64_v( xfb, pname, index, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTransformFeedbacki64_v( GLuint xfb, GLenum pname, GLuint index, ref GLint64[] param )
    {
        fixed ( GLint64* p_param = &param[ 0 ] )
        {
            _glGetTransformFeedbacki64_v( xfb, pname, index, p_param );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateBuffers( GLsizei n, GLuint* buffers )
    {
        _glCreateBuffers( n, buffers );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateBuffer()
    {
        return glCreateBuffers( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedBufferStorage( GLuint buffer, GLsizeiptr size, void* data, GLbitfield flags )
    {
        _glNamedBufferStorage( buffer, size, data, flags );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedBufferStorage< T >( GLuint buffer, GLsizeiptr size, T[] data, GLbitfield flags ) where T : unmanaged
    {
        fixed ( T* p_data = &data[ 0 ] )
        {
            _glNamedBufferStorage( buffer, size, p_data, flags );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedBufferData( GLuint buffer, GLsizeiptr size, void* data, GLenum usage )
    {
        _glNamedBufferData( buffer, size, data, usage );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedBufferData< T >( GLuint buffer, GLsizeiptr size, T[] data, GLenum usage ) where T : unmanaged
    {
        fixed ( T* p_data = &data[ 0 ] )
        {
            _glNamedBufferData( buffer, size, p_data, usage );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glNamedBufferSubData( buffer, offset, size, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedBufferSubData< T >( GLuint buffer, GLintptr offset, GLsizeiptr size, T[] data ) where T : unmanaged
    {
        fixed ( T* p_data = &data[ 0 ] )
        {
            _glNamedBufferSubData( buffer, offset, size, p_data );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyNamedBufferSubData( GLuint readBuffer, GLuint writeBuffer, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size )
    {
        _glCopyNamedBufferSubData( readBuffer, writeBuffer, readOffset, writeOffset, size );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedBufferData( GLuint buffer, GLenum internalformat, GLenum format, GLenum type, void* data )
    {
        _glClearNamedBufferData( buffer, internalformat, format, type, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedBufferData< T >( GLuint buffer, GLenum internalformat, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearNamedBufferData( buffer, internalformat, format, type, t );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedBufferSubData( GLuint buffer, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data )
    {
        _glClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedBufferSubData< T >( GLuint buffer, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, t );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void* glMapNamedBuffer( GLuint buffer, GLenum access )
    {
        return _glMapNamedBuffer( buffer, access );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public System.Span< T > glMapNamedBuffer< T >( GLuint buffer, GLenum access ) where T : unmanaged
    {
        var size = stackalloc GLint[ 1 ];
        _glGetNamedBufferParameteriv( buffer, GL_BUFFER_SIZE, size );
        void* ptr = _glMapNamedBuffer( buffer, access );

        return new System.Span< T >( ptr, *size / sizeof( T ) );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void* glMapNamedBufferRange( GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access )
    {
        return _glMapNamedBufferRange( buffer, offset, length, access );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public System.Span< T > glMapNamedBufferRange< T >( GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access ) where T : unmanaged
    {
        void* ptr = _glMapNamedBufferRange( buffer, offset, length, access );

        return new System.Span< T >( ptr, ( int )length / sizeof( T ) );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glUnmapNamedBuffer( GLuint buffer )
    {
        return _glUnmapNamedBuffer( buffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFlushMappedNamedBufferRange( GLuint buffer, GLintptr offset, GLsizeiptr length )
    {
        _glFlushMappedNamedBufferRange( buffer, offset, length );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedBufferParameteriv( GLuint buffer, GLenum pname, GLint* parameters )
    {
        _glGetNamedBufferParameteriv( buffer, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedBufferParameteriv( GLuint buffer, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* ptr_parameters = &parameters[ 0 ] )
        {
            _glGetNamedBufferParameteriv( buffer, pname, ptr_parameters );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedBufferParameteri64v( GLuint buffer, GLenum pname, GLint64* parameters )
    {
        _glGetNamedBufferParameteri64v( buffer, pname, parameters );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedBufferParameteri64v( GLuint buffer, GLenum pname, ref GLint64[] parameters )
    {
        fixed ( GLint64* ptr_parameters = &parameters[ 0 ] )
        {
            _glGetNamedBufferParameteri64v( buffer, pname, ptr_parameters );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedBufferPointerv( GLuint buffer, GLenum pname, void** parameters )
    {
        _glGetNamedBufferPointerv( buffer, pname, parameters );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glGetNamedBufferSubData( buffer, offset, size, data );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateFramebuffers( GLsizei n, GLuint* framebuffers )
    {
        _glCreateFramebuffers( n, framebuffers );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateFramebuffer()
    {
        return glCreateFramebuffers( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferRenderbuffer( GLuint framebuffer, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer )
    {
        _glNamedFramebufferRenderbuffer( framebuffer, attachment, renderbuffertarget, renderbuffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferParameteri( GLuint framebuffer, GLenum pname, GLint param )
    {
        _glNamedFramebufferParameteri( framebuffer, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferTexture( GLuint framebuffer, GLenum attachment, GLuint texture, GLint level )
    {
        _glNamedFramebufferTexture( framebuffer, attachment, texture, level );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferTextureLayer( GLuint framebuffer, GLenum attachment, GLuint texture, GLint level, GLint layer )
    {
        _glNamedFramebufferTextureLayer( framebuffer, attachment, texture, level, layer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferDrawBuffer( GLuint framebuffer, GLenum buf )
    {
        _glNamedFramebufferDrawBuffer( framebuffer, buf );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferDrawBuffers( GLuint framebuffer, GLsizei n, GLenum* bufs )
    {
        _glNamedFramebufferDrawBuffers( framebuffer, n, bufs );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferDrawBuffers( GLuint framebuffer, GLenum[] bufs )
    {
        fixed ( GLenum* ptr_bufs = &bufs[ 0 ] )
        {
            _glNamedFramebufferDrawBuffers( framebuffer, bufs.Length, ptr_bufs );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedFramebufferReadBuffer( GLuint framebuffer, GLenum src )
    {
        _glNamedFramebufferReadBuffer( framebuffer, src );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferData( GLuint framebuffer, GLsizei numAttachments, GLenum* attachments )
    {
        _glInvalidateNamedFramebufferData( framebuffer, numAttachments, attachments );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferData( GLuint framebuffer, GLenum[] attachments )
    {
        fixed ( GLenum* ptr_attachments = &attachments[ 0 ] )
        {
            _glInvalidateNamedFramebufferData( framebuffer, attachments.Length, ptr_attachments );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferSubData( GLuint framebuffer, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glInvalidateNamedFramebufferSubData( framebuffer, numAttachments, attachments, x, y, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glInvalidateNamedFramebufferSubData( GLuint framebuffer, GLenum[] attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        fixed ( GLenum* ptr_attachments = &attachments[ 0 ] )
        {
            _glInvalidateNamedFramebufferSubData( framebuffer, attachments.Length, ptr_attachments, x, y, width, height );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedFramebufferiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLint* value )
    {
        _glClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedFramebufferiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLint[] value )
    {
        fixed ( GLint* ptr_value =
                   &value[ 0 ] ) _glClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, ptr_value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedFramebufferuiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLuint* value )
    {
        _glClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedFramebufferuiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLuint[] value )
    {
        fixed ( GLuint* ptr_value =
                   &value[ 0 ] ) _glClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, ptr_value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedFramebufferfv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLfloat* value )
    {
        _glClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, value );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedFramebufferfv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLfloat[] value )
    {
        fixed ( GLfloat* ptr_value = &value[ 0 ] )
        {
            _glClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, ptr_value );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearNamedFramebufferfi( GLuint framebuffer, GLenum buffer, GLfloat depth, GLint stencil )
    {
        _glClearNamedFramebufferfi( framebuffer, buffer, depth, stencil );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlitNamedFramebuffer( GLuint readFramebuffer, GLuint drawFramebuffer, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1,
                                        GLbitfield mask, GLenum filter )
    {
        _glBlitNamedFramebuffer( readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLenum glCheckNamedFramebufferStatus( GLuint framebuffer, GLenum target )
    {
        return _glCheckNamedFramebufferStatus( framebuffer, target );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedFramebufferParameteriv( GLuint framebuffer, GLenum pname, GLint* param )
    {
        _glGetNamedFramebufferParameteriv( framebuffer, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedFramebufferParameteriv( GLuint framebuffer, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param = &param[ 0 ] )
        {
            _glGetNamedFramebufferParameteriv( framebuffer, pname, ptr_param );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedFramebufferAttachmentParameteriv( GLuint framebuffer, GLenum attachment, GLenum pname, GLint* params_ )
    {
        _glGetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, params_ );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedFramebufferAttachmentParameteriv( GLuint framebuffer, GLenum attachment, GLenum pname, ref GLint[] params_ )
    {
        fixed ( GLint* ptr_params_ = &params_[ 0 ] )
        {
            _glGetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, ptr_params_ );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateRenderbuffers( GLsizei n, GLuint* renderbuffers )
    {
        _glCreateRenderbuffers( n, renderbuffers );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateRenderbuffer()
    {
        return glCreateRenderbuffers( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedRenderbufferStorage( GLuint renderbuffer, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glNamedRenderbufferStorage( renderbuffer, internalformat, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glNamedRenderbufferStorageMultisample( GLuint renderbuffer, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glNamedRenderbufferStorageMultisample( renderbuffer, samples, internalformat, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedRenderbufferParameteriv( GLuint renderbuffer, GLenum pname, GLint* param )
    {
        _glGetNamedRenderbufferParameteriv( renderbuffer, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetNamedRenderbufferParameteriv( GLuint renderbuffer, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param = &param[ 0 ] )
        {
            _glGetNamedRenderbufferParameteriv( renderbuffer, pname, ptr_param );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateTextures( GLenum target, GLsizei n, GLuint* textures )
    {
        _glCreateTextures( target, n, textures );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateTexture( GLenum target )
    {
        return glCreateTextures( target, 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureBuffer( GLuint texture, GLenum internalformat, GLuint buffer )
    {
        _glTextureBuffer( texture, internalformat, buffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureBufferRange( GLuint texture, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTextureBufferRange( texture, internalformat, buffer, offset, size );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureStorage1D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width )
    {
        _glTextureStorage1D( texture, levels, internalformat, width );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureStorage2D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glTextureStorage2D( texture, levels, internalformat, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureStorage3D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glTextureStorage3D( texture, levels, internalformat, width, height, depth );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureStorage2DMultisample( GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations )
    {
        _glTextureStorage2DMultisample( texture, samples, internalformat, width, height, fixedsamplelocations );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureStorage3DMultisample( GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedsamplelocations )
    {
        _glTextureStorage3DMultisample( texture, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage1D( texture, level, xoffset, width, format, type, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureSubImage1D< T >( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, T[] pixels ) where T : unmanaged
    {
        fixed ( T* ptr_pixels = &pixels[ 0 ] )
        {
            _glTextureSubImage1D( texture, level, xoffset, width, format, type, ptr_pixels );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureSubImage2D< T >( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, T[] pixels ) where T : unmanaged
    {
        fixed ( T* ptr_pixels = &pixels[ 0 ] )
        {
            _glTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, ptr_pixels );
        }
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, byte[] data )
    {
        fixed ( byte* ptr_data
                   = &data[ 0 ] ) _glCompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, ptr_data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, byte[] data )
    {
        fixed ( byte* ptr_data
                   = &data[ 0 ] ) _glCompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, ptr_data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize,
                                               void* data )
    {
        _glCompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize,
                                               byte[] data )
    {
        fixed ( byte* ptr_data
                   = &data[ 0 ] ) _glCompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, ptr_data );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width )
    {
        _glCopyTextureSubImage1D( texture, level, xoffset, x, y, width );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTextureSubImage2D( texture, level, xoffset, yoffset, x, y, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, x, y, width, height );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameterf( GLuint texture, GLenum pname, GLfloat param )
    {
        _glTextureParameterf( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameterfv( GLuint texture, GLenum pname, GLfloat* param )
    {
        _glTextureParameterfv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameterfv( GLuint texture, GLenum pname, GLfloat[] param )
    {
        fixed ( GLfloat* ptr_param = &param[ 0 ] ) _glTextureParameterfv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameteri( GLuint texture, GLenum pname, GLint param )
    {
        _glTextureParameteri( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameterIiv( GLuint texture, GLenum pname, GLint* param )
    {
        _glTextureParameterIiv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameterIiv( GLuint texture, GLenum pname, GLint[] param )
    {
        fixed ( GLint* ptr_param = &param[ 0 ] ) _glTextureParameterIiv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameterIuiv( GLuint texture, GLenum pname, GLuint* param )
    {
        _glTextureParameterIuiv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameterIuiv( GLuint texture, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* ptr_param =
                   &param[ 0 ] ) _glTextureParameterIuiv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameteriv( GLuint texture, GLenum pname, GLint* param )
    {
        _glTextureParameteriv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTextureParameteriv( GLuint texture, GLenum pname, GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glTextureParameteriv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenerateTextureMipmap( GLuint texture )
    {
        _glGenerateTextureMipmap( texture );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindTextureUnit( GLuint unit, GLuint texture )
    {
        _glBindTextureUnit( unit, texture );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureImage( GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pixels )
    {
        _glGetTextureImage( texture, level, format, type, bufSize, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public T[] glGetTextureImage< T >( GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize ) where T : unmanaged
    {
        var pixels =
            new T[ bufSize ];
        fixed ( T* ptr_pixels = &pixels[ 0 ] ) _glGetTextureImage( texture, level, format, type, bufSize, ptr_pixels );

        return pixels;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetCompressedTextureImage( GLuint texture, GLint level, GLsizei bufSize, void* pixels )
    {
        _glGetCompressedTextureImage( texture, level, bufSize, pixels );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public byte[] glGetCompressedTextureImage( GLuint texture, GLint level, GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];
        fixed ( byte* ptr_pixels
                   = &pixels[ 0 ] ) _glGetCompressedTextureImage( texture, level, bufSize, ptr_pixels );

        return pixels;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureLevelParameterfv( GLuint texture, GLint level, GLenum pname, GLfloat* param )
    {
        _glGetTextureLevelParameterfv( texture, level, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureLevelParameterfv( GLuint texture, GLint level, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* ptr_param =
                   &param[ 0 ] ) _glGetTextureLevelParameterfv( texture, level, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureLevelParameteriv( GLuint texture, GLint level, GLenum pname, GLint* param )
    {
        _glGetTextureLevelParameteriv( texture, level, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureLevelParameteriv( GLuint texture, GLint level, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetTextureLevelParameteriv( texture, level, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameterfv( GLuint texture, GLenum pname, GLfloat* param )
    {
        _glGetTextureParameterfv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameterfv( GLuint texture, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameterfv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameterIiv( GLuint texture, GLenum pname, GLint* param )
    {
        _glGetTextureParameterIiv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameterIiv( GLuint texture, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameterIiv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameterIuiv( GLuint texture, GLenum pname, GLuint* param )
    {
        _glGetTextureParameterIuiv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameterIuiv( GLuint texture, GLenum pname, ref GLuint[] param )
    {
        fixed ( GLuint* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameterIuiv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameteriv( GLuint texture, GLenum pname, GLint* param )
    {
        _glGetTextureParameteriv( texture, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTextureParameteriv( GLuint texture, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetTextureParameteriv( texture, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateVertexArrays( GLsizei n, GLuint* arrays )
    {
        _glCreateVertexArrays( n, arrays );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glCreateVertexArrays( GLsizei n )
    {
        var arrays = new GLuint[ n ];
        fixed ( GLuint* ptr_arrays =
                   &arrays[ 0 ] ) _glCreateVertexArrays( n, ptr_arrays );

        return arrays;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateVertexArray()
    {
        return glCreateVertexArrays( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDisableVertexArrayAttrib( GLuint vaobj, GLuint index )
    {
        _glDisableVertexArrayAttrib( vaobj, index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEnableVertexArrayAttrib( GLuint vaobj, GLuint index )
    {
        _glEnableVertexArrayAttrib( vaobj, index );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayElementBuffer( GLuint vaobj, GLuint buffer )
    {
        _glVertexArrayElementBuffer( vaobj, buffer );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayVertexBuffer( GLuint vaobj, GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )
    {
        _glVertexArrayVertexBuffer( vaobj, bindingindex, buffer, offset, stride );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayVertexBuffers( GLuint vaobj, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides )
    {
        _glVertexArrayVertexBuffers( vaobj, first, count, buffers, offsets, strides );
    }

// ------------------------------------------------------------------------

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

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayAttribBinding( GLuint vaobj, GLuint attribindex, GLuint bindingindex )
    {
        _glVertexArrayAttribBinding( vaobj, attribindex, bindingindex );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayAttribFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset )
    {
        _glVertexArrayAttribFormat( vaobj, attribindex, size, type, normalized, relativeoffset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayAttribIFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexArrayAttribIFormat( vaobj, attribindex, size, type, relativeoffset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayAttribLFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexArrayAttribLFormat( vaobj, attribindex, size, type, relativeoffset );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glVertexArrayBindingDivisor( GLuint vaobj, GLuint bindingindex, GLuint divisor )
    {
        _glVertexArrayBindingDivisor( vaobj, bindingindex, divisor );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexArrayiv( GLuint vaobj, GLenum pname, GLint* param )
    {
        _glGetVertexArrayiv( vaobj, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexArrayiv( GLuint vaobj, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetVertexArrayiv( vaobj, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexArrayIndexediv( GLuint vaobj, GLuint index, GLenum pname, GLint* param )
    {
        _glGetVertexArrayIndexediv( vaobj, index, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexArrayIndexediv( GLuint vaobj, GLuint index, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptr_param =
                   &param[ 0 ] ) _glGetVertexArrayIndexediv( vaobj, index, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexArrayIndexed64iv( GLuint vaobj, GLuint index, GLenum pname, GLint64* param )
    {
        _glGetVertexArrayIndexed64iv( vaobj, index, pname, param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetVertexArrayIndexed64iv( GLuint vaobj, GLuint index, GLenum pname, ref GLint64[] param )
    {
        fixed ( GLint64* ptr_param =
                   &param[ 0 ] ) _glGetVertexArrayIndexed64iv( vaobj, index, pname, ptr_param );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateSamplers( GLsizei n, GLuint* samplers )
    {
        _glCreateSamplers( n, samplers );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glCreateSamplers( GLsizei n )
    {
        var samplers = new GLuint[ n ];
        fixed ( GLuint* ptr_samplers =
                   &samplers[ 0 ] ) _glCreateSamplers( n, ptr_samplers );

        return samplers;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateSamplers()
    {
        return glCreateSamplers( 1 )[ 0 ];
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCreateProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glCreateProgramPipelines( n, pipelines );
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint[] glCreateProgramPipelines( GLsizei n )
    {
        var pipelines = new GLuint[ n ];
        fixed ( GLuint* ptr_pipelines =
                   &pipelines[ 0 ] ) _glCreateProgramPipelines( n, ptr_pipelines );

        return pipelines;
    }

// ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateProgramPipeline()
    {
        return glCreateProgramPipelines( 1 )[ 0 ];
    }

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
}

#pragma warning restore IDE0079 // Remove unnecessary suppression
#pragma warning restore CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore CS8603  // Possible null reference return.
#pragma warning restore IDE0060 // Remove unused parameter.
#pragma warning restore IDE1006 // Naming Styles.
#pragma warning restore IDE0090 // Use 'new(...)'.
#pragma warning restore CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type