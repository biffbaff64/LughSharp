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

//#pragma warning disable IDE0079 // Remove unnecessary suppression
//#pragma warning disable CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
//#pragma warning disable CS8603  // Possible null reference return.
//#pragma warning disable IDE0060 // Remove unused parameter.
//#pragma warning disable IDE1006 // Naming Styles.
//#pragma warning disable IDE0090 // Use 'new(...)'.
//#pragma warning disable CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type

// ============================================================================

using System.Numerics;

using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;

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

namespace Corelib.Lugh.Graphics.OpenGL;

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
    /// <summary>
    /// The null pointer, just like in C/C++.
    /// </summary>
    public readonly void* NULL = ( void* )0;

    // ========================================================================
    // ========================================================================

    /// <inheritdoc/>
    public (int major, int minor) GetOpenGLVersion()
    {
        var version = GetString( IGL.GL_VERSION );

        if ( version == null )
        {
            throw new GdxRuntimeException( "NULL GL Version returned!" );
        }

        return ( version[ 0 ], version[ 2 ] );
    }

    // ========================================================================
    // ========================================================================

    public void MessageCallback( int source,
                                 int type,
                                 uint id,
                                 int severity,
                                 int length,
                                 string message,
                                 void* userParam )
    {
        Logger.Error( $"GL CALLBACK: {( type == IGL.GL_DEBUG_TYPE_ERROR ? "** GL ERROR **" : "" )} type = {type}, severity = {severity}, message = {message}\n" );
    }

    // ========================================================================
    // ========================================================================

    /// <inheritdoc/>
    public void CullFace( GLenum mode )
    {
        GetDelegateForFunction< glCullFaceDelegate >( "glCullFace", out _glCullFace );
        
        _glCullFace( mode );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void FrontFace( GLenum mode )
    {
        GetDelegateForFunction< glFrontFaceDelegate >( "glFrontFace", out _glFrontFace );

        _glFrontFace( mode );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Hint( GLenum target, GLenum mode )
    {
        GetDelegateForFunction< glHintDelegate >( "glHint", out _glHint );

        _glHint( target, mode );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void LineWidth( GLfloat width )
    {
        GetDelegateForFunction< glLineWidthDelegate >( "glLineWidth", out _glLineWidth );

        _glLineWidth( width );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PointSize( GLfloat size )
    {
        GetDelegateForFunction< glPointSizeDelegate >( "glPointSize", out _glPointSize );

        _glPointSize( size );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PolygonMode( GLenum face, GLenum mode )
    {
        GetDelegateForFunction< glPolygonModeDelegate >( "glPolygonMode", out _glPolygonMode );

        _glPolygonMode( face, mode );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Scissor( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        GetDelegateForFunction< glScissorDelegate >( "glScissor", out _glScissor );
        
        _glScissor( x, y, width, height );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexParameterf( GLenum target, GLenum pname, GLfloat param )
    {
        GetDelegateForFunction< glTexParameterfDelegate >( "glTexParameterf", out _glTexParameterf );

        _glTexParameterf( target, pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexParameterfv( GLenum target, GLenum pname, GLfloat* parameters )
    {
        GetDelegateForFunction< glTexParameterfvDelegate >( "glTexParameterfv", out _glTexParameterfv );

        _glTexParameterfv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void TexParameterfv( GLenum target, GLenum pname, GLfloat[] parameters )
    {
        GetDelegateForFunction< glTexParameterfvDelegate >( "glTexParameterfv", out _glTexParameterfv );

        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glTexParameterfv( target, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexParameteri( GLenum target, GLenum pname, GLint param )
    {
        GetDelegateForFunction< glTexParameteriDelegate >( "glTexParameteri", out _glTexParameteri );

        _glTexParameteri( target, pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< glTexParameterivDelegate >( "glTexParameteriv", out _glTexParameteriv );

        _glTexParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void TexParameteriv( GLenum target, GLenum pname, GLint[] parameters )
    {
        GetDelegateForFunction< glTexParameterivDelegate >( "glTexParameteriv", out _glTexParameteriv );

        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glTexParameteriv( target, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexImage1D( GLenum target,
                            GLint level,
                            GLenum internalformat,
                            GLsizei width,
                            GLint border,
                            GLenum format,
                            GLenum type,
                            void* pixels )
    {
        GetDelegateForFunction< glTexImage1dDelegate >( "glTexImage1D", out _glTexImage1D );

        _glTexImage1D( target, level, internalformat, width, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void TexImage1D< T >( GLenum target,
                                 GLint level,
                                 GLenum internalformat,
                                 GLsizei width,
                                 GLint border,
                                 GLenum format,
                                 GLenum type,
                                 T[] pixels ) where T : unmanaged
    {
        GetDelegateForFunction< glTexImage1dDelegate >( "glTexImage1D", out _glTexImage1D );

        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexImage1D( target, level, internalformat, width, border, format, type, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexImage2D( GLenum target,
                            GLint level,
                            GLenum internalformat,
                            GLsizei width,
                            GLsizei height,
                            GLint border,
                            GLenum format,
                            GLenum type,
                            void* pixels )
    {
        GetDelegateForFunction< PFNGLTEXIMAGE2DPROC >( "glTexImage2D", out _glTexImage2D );

        _glTexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void TexImage2D< T >( GLenum target,
                                 GLint level,
                                 GLenum internalformat,
                                 GLsizei width,
                                 GLsizei height,
                                 GLint border,
                                 GLenum format,
                                 GLenum type,
                                 T[] pixels ) where T : unmanaged
    {
        GetDelegateForFunction< PFNGLTEXIMAGE2DPROC >( "glTexImage2D", out _glTexImage2D );

        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexImage2D( target, level, internalformat, width, height, border, format, type, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DrawBuffer( GLenum buf )
    {
        GetDelegateForFunction< PFNGLDRAWBUFFERPROC >( "glDrawBuffer", out _glDrawBuffer );

        _glDrawBuffer( buf );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Clear( GLbitfield mask )
    {
        GetDelegateForFunction< PFNGLCLEARPROC >( "glClear", out _glClear );

        _glClear( mask );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ClearColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        GetDelegateForFunction< PFNGLCLEARCOLORPROC >( "glClearColor", out _glClearColor );

        _glClearColor( red, green, blue, alpha );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ClearStencil( GLint s )
    {
        GetDelegateForFunction< PFNGLCLEARSTENCILPROC >( "glClearStencil", out _glClearStencil );

        _glClearStencil( s );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ClearDepth( GLdouble depth )
    {
        GetDelegateForFunction< PFNGLCLEARDEPTHPROC >( "glClearDepth", out _glClearDepth );

        _glClearDepth( depth );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void StencilMask( GLuint mask )
    {
        GetDelegateForFunction< PFNGLSTENCILMASKPROC >( "glStencilMask", out _glStencilMask );

        _glStencilMask( mask );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ColorMask( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha )
    {
        GetDelegateForFunction< PFNGLCOLORMASKPROC >( "glColorMask", out _glColorMask );

        _glColorMask( red, green, blue, alpha );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DepthMask( GLboolean flag )
    {
        GetDelegateForFunction< PFNGLDEPTHMASKPROC >( "glDepthMask", out _glDepthMask );

        _glDepthMask( flag );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Disable( GLenum cap )
    {
        GetDelegateForFunction< PFNGLDISABLEPROC >( "glDisable", out _glDisable );

        _glDisable( cap );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Enable( GLenum cap )
    {
        GetDelegateForFunction< PFNGLENABLEPROC >( "glEnable", out _glEnable );

        _glEnable( cap );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Finish()
    {
        GetDelegateForFunction< PFNGLFINISHPROC >( "glFinish", out _glFinish );

        _glFinish();
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Flush()
    {
        GetDelegateForFunction< PFNGLFLUSHPROC >( "glFlush", out _glFlush );

        _glFlush();
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BlendFunc( GLenum sfactor, GLenum dfactor )
    {
        GetDelegateForFunction< PFNGLBLENDFUNCPROC >( "glBlendFunc", out _glBlendFunc );

        _glBlendFunc( sfactor, dfactor );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void LogicOp( GLenum opcode )
    {
        GetDelegateForFunction< PFNGLLOGICOPPROC >( "glLogicOp", out _glLogicOp );

        _glLogicOp( opcode );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void StencilFunc( GLenum func, GLint @ref, GLuint mask )
    {
        GetDelegateForFunction< PFNGLSTENCILFUNCPROC >( "glStencilFunc", out _glStencilFunc );

        _glStencilFunc( func, @ref, mask );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void StencilOp( GLenum fail, GLenum zfail, GLenum zpass )
    {
        GetDelegateForFunction< PFNGLSTENCILOPPROC >( "glStencilOp", out _glStencilOp );

        _glStencilOp( fail, zfail, zpass );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DepthFunc( GLenum func )
    {
        GetDelegateForFunction< PFNGLDEPTHFUNCPROC >( "glDepthFunc", out _glDepthFunc );

        _glDepthFunc( func );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PixelStoref( GLenum pname, GLfloat param )
    {
        GetDelegateForFunction< PFNGLPIXELSTOREFPROC >( "glPixelStoref", out _glPixelStoref );

        _glPixelStoref( pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PixelStorei( GLenum pname, GLint param )
    {
        GetDelegateForFunction< PFNGLPIXELSTOREIPROC >( "glPixelStorei", out _glPixelStorei );

        _glPixelStorei( pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ReadBuffer( GLenum src )
    {
        GetDelegateForFunction< PFNGLREADBUFFERPROC >( "glReadBuffer", out _glReadBuffer );

        _glReadBuffer( src );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ReadPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels )
    {
        GetDelegateForFunction< PFNGLREADPIXELSPROC >( "glReadPixels", out _glReadPixels );

        _glReadPixels( x, y, width, height, format, type, pixels );
    }

    /// <inheritdoc/>
    public void ReadPixels< T >( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, ref T[] pixels ) where T : unmanaged
    {
        GetDelegateForFunction< PFNGLREADPIXELSPROC >( "glReadPixels", out _glReadPixels );

        fixed ( void* p = &pixels[ 0 ] )
        {
            _glReadPixels( x, y, width, height, format, type, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetBooleanv( GLenum pname, GLboolean* data )
    {
        GetDelegateForFunction< PFNGLGETBOOLEANVPROC >( "glGetBooleanv", out _glGetBooleanv );

        _glGetBooleanv( pname, data );
    }

    /// <inheritdoc/>
    public void GetBooleanv( GLenum pname, ref GLboolean[] data )
    {
        GetDelegateForFunction< PFNGLGETBOOLEANVPROC >( "glGetBooleanv", out _glGetBooleanv );

        fixed ( GLboolean* p = &data[ 0 ] )
        {
            _glGetBooleanv( pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetDoublev( GLenum pname, GLdouble* data )
    {
        GetDelegateForFunction< PFNGLGETDOUBLEVPROC >( "glGetDoublev", out _glGetDoublev );

        _glGetDoublev( pname, data );
    }

    /// <inheritdoc/>
    public void GetDoublev( GLenum pname, ref GLdouble[] data )
    {
        GetDelegateForFunction< PFNGLGETDOUBLEVPROC >( "glGetDoublev", out _glGetDoublev );

        fixed ( GLdouble* p = &data[ 0 ] )
        {
            _glGetDoublev( pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLenum GetError()
    {
        GetDelegateForFunction< PFNGLGETERRORPROC >( "glGetError", out _glGetError );

        return _glGetError();
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetFloatv( GLenum pname, float* data )
    {
        GetDelegateForFunction< PFNGLGETFLOATVPROC >( "glGetFloatv", out _glGetFloatv );

        _glGetFloatv( pname, data );
    }

    /// <inheritdoc/>
    public void GetFloatv( GLenum pname, ref GLfloat[] data )
    {
        GetDelegateForFunction< PFNGLGETFLOATVPROC >( "glGetFloatv", out _glGetFloatv );

        fixed ( GLfloat* p = &data[ 0 ] )
        {
            _glGetFloatv( pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetIntegerv( GLenum pname, GLint* data )
    {
        GetDelegateForFunction< PFNGLGETINTEGERVPROC >( "glGetIntegerv", out _glGetIntegerv );

        _glGetIntegerv( pname, data );
    }

    /// <inheritdoc/>
    public void GetIntegerv( GLenum pname, ref GLint[] data )
    {
        GetDelegateForFunction< PFNGLGETINTEGERVPROC >( "glGetIntegerv", out _glGetIntegerv );

        fixed ( GLint* p = &data[ 0 ] )
        {
            _glGetIntegerv( pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLubyte* GetString( GLenum name )
    {
//        GetDelegateForFunction< glGetStringDelegate >( "glGetString", out _glGetString );
        return GetDelegateFor< glGetStringDelegate >( "glGetString" )( name );

//        return _glGetString( name );
    }

    // ========================================================================

    /// <inheritdoc/>
    public string GetStringSafe( GLenum name )
    {
        GetDelegateForFunction< glGetStringDelegate >( "glGetString", out _glGetString );

        var p = _glGetString( name );

        if ( p == null )
        {
            return string.Empty;
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
    public void GetTexImage( GLenum target, GLint level, GLenum format, GLenum type, void* pixels )
    {
        GetDelegateForFunction< PFNGLGETTEXIMAGEPROC >( "glGetTexImage", out _glGetTexImage );

        _glGetTexImage( target, level, format, type, pixels );
    }

    /// <inheritdoc/>
    public void GetTexImage< T >( GLenum target, GLint level, GLenum format, GLenum type, ref T[] pixels ) where T : unmanaged
    {
        GetDelegateForFunction< PFNGLGETTEXIMAGEPROC >( "glGetTexImage", out _glGetTexImage );

        fixed ( void* p = &pixels[ 0 ] )
        {
            _glGetTexImage( target, level, format, type, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXPARAMETERFVPROC >( "glGetTexParameterfv", out _glGetTexParameterfv );

        _glGetTexParameterfv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetTexParameterfv( GLenum target, GLenum pname, ref GLfloat[] parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXPARAMETERFVPROC >( "glGetTexParameterfv", out _glGetTexParameterfv );

        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glGetTexParameterfv( target, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetTexParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXPARAMETERIVPROC >( "glGetTexParameteriv", out _glGetTexParameteriv );

        _glGetTexParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetTexParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXPARAMETERIVPROC >( "glGetTexParameteriv", out _glGetTexParameteriv );

        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetTexParameteriv( target, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, GLfloat* parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXLEVELPARAMETERFVPROC >( "glGetTexLevelParameterfv", out _glGetTexLevelParameterfv );

        _glGetTexLevelParameterfv( target, level, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, ref GLfloat[] parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXLEVELPARAMETERFVPROC >( "glGetTexLevelParameterfv", out _glGetTexLevelParameterfv );

        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glGetTexLevelParameterfv( target, level, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXLEVELPARAMETERIVPROC >( "glGetTexLevelParameteriv", out _glGetTexLevelParameteriv );

        _glGetTexLevelParameteriv( target, level, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, ref GLint[] parameters )
    {
        GetDelegateForFunction< PFNGLGETTEXLEVELPARAMETERIVPROC >( "glGetTexLevelParameteriv", out _glGetTexLevelParameteriv );

        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetTexLevelParameteriv( target, level, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsEnabled( GLenum cap )
    {
        GetDelegateForFunction< PFNGLISENABLEDPROC >( "glIsEnabled", out _glIsEnabled );

        return _glIsEnabled( cap );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DepthRange( GLdouble near, GLdouble far )
    {
        GetDelegateForFunction< PFNGLDEPTHRANGEPROC >( "glDepthRange", out _glDepthRange );

        _glDepthRange( near, far );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Viewport( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        GetDelegateForFunction< PFNGLVIEWPORTPROC >( "glViewport", out _glViewport );

        _glViewport( x, y, width, height );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DrawArrays( GLenum mode, GLint first, GLsizei count )
    {
        GetDelegateForFunction< PFNGLDRAWARRAYSPROC >( "glDrawArrays", out _glDrawArrays );

        _glDrawArrays( mode, first, count );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DrawElements( GLenum mode, GLsizei count, GLenum type, void* indices )
    {
        GetDelegateForFunction< PFNGLDRAWELEMENTSPROC >( "glDrawElements", out _glDrawElements );

        _glDrawElements( mode, count, type, indices );
    }

    /// <inheritdoc/>
    public void DrawElements< T >( GLenum mode, GLsizei count, GLenum type, T[] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        GetDelegateForFunction< PFNGLDRAWELEMENTSPROC >( "glDrawElements", out _glDrawElements );

        fixed ( T* p = &indices[ 0 ] )
        {
            _glDrawElements( mode, count, type, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PolygonOffset( GLfloat factor, GLfloat units )
    {
        GetDelegateForFunction< PFNGLPOLYGONOFFSETPROC >( "glPolygonOffset", out _glPolygonOffset );

        _glPolygonOffset( factor, units );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CopyTexImage1D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border )
    {
        GetDelegateForFunction< PFNGLCOPYTEXIMAGE1DPROC >( "glCopyTexImage1D", out _glCopyTexImage1D );

        _glCopyTexImage1D( target, level, internalformat, x, y, width, border );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CopyTexImage2D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border )
    {
        GetDelegateForFunction< PFNGLCOPYTEXIMAGE2DPROC >( "glCopyTexImage2D", out _glCopyTexImage2D );

        _glCopyTexImage2D( target, level, internalformat, x, y, width, height, border );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CopyTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width )
    {
        GetDelegateForFunction< PFNGLCOPYTEXSUBIMAGE1DPROC >( "glCopyTexSubImage1D", out _glCopyTexSubImage1D );

        _glCopyTexSubImage1D( target, level, xoffset, x, y, width );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CopyTexSubImage2D( GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        GetDelegateForFunction< PFNGLCOPYTEXSUBIMAGE2DPROC >( "glCopyTexSubImage2D", out _glCopyTexSubImage2D );

        _glCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels )
    {
        GetDelegateForFunction< PFNGLTEXSUBIMAGE1DPROC >( "glTexSubImage1D", out _glTexSubImage1D );

        _glTexSubImage1D( target, level, xoffset, width, format, type, pixels );
    }

    /// <inheritdoc/>
    public void TexSubImage1D< T >( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, T[] pixels )
        where T : unmanaged
    {
        GetDelegateForFunction< PFNGLTEXSUBIMAGE1DPROC >( "glTexSubImage1D", out _glTexSubImage1D );

        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexSubImage1D( target, level, xoffset, width, format, type, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexSubImage2D( GLenum target,
                               GLint level,
                               GLint xoffset,
                               GLint yoffset,
                               GLsizei width,
                               GLsizei height,
                               GLenum format,
                               GLenum type,
                               void* pixels )
    {
        GetDelegateForFunction< PFNGLTEXSUBIMAGE2DPROC >( "glTexSubImage2D", out _glTexSubImage2D );

        _glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
    }

    /// <inheritdoc/>
    public void TexSubImage2D< T >( GLenum target,
                                    GLint level,
                                    GLint xoffset,
                                    GLint yoffset,
                                    GLsizei width,
                                    GLsizei height,
                                    GLenum format,
                                    GLenum type,
                                    T[] pixels ) where T : unmanaged
    {
        GetDelegateForFunction< PFNGLTEXSUBIMAGE2DPROC >( "glTexSubImage2D", out _glTexSubImage2D );

        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BindTexture( GLenum target, GLuint texture )
    {
        GetDelegateForFunction< PFNGLBINDTEXTUREPROC >( "glBindTexture", out _glBindTexture );

        _glBindTexture( target, texture );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DeleteTextures( GLsizei n, GLuint* textures )
    {
        GetDelegateForFunction< PFNGLDELETETEXTURESPROC >( "glDeleteTextures", out _glDeleteTextures );

        _glDeleteTextures( n, textures );
    }

    /// <inheritdoc/>
    public void DeleteTextures( params GLuint[] textures )
    {
        GetDelegateForFunction< PFNGLDELETETEXTURESPROC >( "glDeleteTextures", out _glDeleteTextures );

        fixed ( void* p = &textures[ 0 ] )
        {
            _glDeleteTextures( textures.Length, ( GLuint* )p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GenTextures( GLsizei n, GLuint* textures )
    {
        GetDelegateForFunction< PFNGLGENTEXTURESPROC >( "glGenTextures", out _glGenTextures );

        _glGenTextures( n, textures );
    }

    /// <inheritdoc/>
    public GLuint[] GenTextures( GLsizei n )
    {
        GetDelegateForFunction< PFNGLGENTEXTURESPROC >( "glGenTextures", out _glGenTextures );

        var textures = new GLuint[ n ];

        fixed ( void* p = &textures[ 0 ] )
        {
            _glGenTextures( n, ( GLuint* )p );
        }

        return textures;
    }

    /// <inheritdoc/>
    public GLuint GenTexture()
    {
        return GenTextures( 1 )[ 0 ];
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsTexture( GLuint texture )
    {
        GetDelegateForFunction< PFNGLISTEXTUREPROC >( "glIsTexture", out _glIsTexture );

        return _glIsTexture( texture );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DrawRangeElements( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices )
    {
        GetDelegateForFunction< PFNGLDRAWRANGEELEMENTSPROC >( "glDrawRangeElements", out _glDrawRangeElements );

        _glDrawRangeElements( mode, start, end, count, type, indices );
    }

    /// <inheritdoc/>
    public void DrawRangeElements< T >( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        GetDelegateForFunction< PFNGLDRAWRANGEELEMENTSPROC >( "glDrawRangeElements", out _glDrawRangeElements );

        fixed ( T* pIndices = &indices[ 0 ] )
        {
            _glDrawRangeElements( mode, start, end, count, type, pIndices );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexImage3D( GLenum target,
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
        GetDelegateForFunction< PFNGLTEXIMAGE3DPROC >( "glTexImage3D", out _glTexImage3D );

        _glTexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void TexImage3D< T >( GLenum target,
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
        GetDelegateForFunction< PFNGLTEXIMAGE3DPROC >( "glTexImage3D", out _glTexImage3D );

        fixed ( T* pPixels = &pixels[ 0 ] )
        {
            _glTexImage3D( target, level, internalformat, width, height, depth, border, format, type, pPixels );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexSubImage3D( GLenum target,
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
        GetDelegateForFunction< PFNGLTEXSUBIMAGE3DPROC >( "glTexSubImage3D", out _glTexSubImage3D );

        _glTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
    }

    /// <inheritdoc/>
    public void TexSubImage3D< T >( GLenum target,
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
        GetDelegateForFunction< PFNGLTEXSUBIMAGE3DPROC >( "glTexSubImage3D", out _glTexSubImage3D );

        fixed ( T* pPixels = &pixels[ 0 ] )
        {
            _glTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pPixels );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CopyTexSubImage3D( GLenum target,
                                   GLint level,
                                   GLint xoffset,
                                   GLint yoffset,
                                   GLint zoffset,
                                   GLint x,
                                   GLint y,
                                   GLsizei width,
                                   GLsizei height )
    {
        GetDelegateForFunction< PFNGLCOPYTEXSUBIMAGE3DPROC >( "glCopyTexSubImage3D", out _glCopyTexSubImage3D );

        _glCopyTexSubImage3D( target, level, xoffset, yoffset, zoffset, x, y, width, height );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ActiveTexture( GLenum texture )
    {
        GetDelegateForFunction< PFNGLACTIVETEXTUREPROC >( "glActiveTexture", out _glActiveTexture );

        _glActiveTexture( texture );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SampleCoverage( GLfloat value, GLboolean invert )
    {
        GetDelegateForFunction< PFNGLSAMPLECOVERAGEPROC >( "glSampleCoverage", out _glSampleCoverage );

        _glSampleCoverage( value, invert );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CompressedTexImage3D( GLenum target,
                                      GLint level,
                                      GLenum internalformat,
                                      GLsizei width,
                                      GLsizei height,
                                      GLsizei depth,
                                      GLint border,
                                      GLsizei imageSize,
                                      void* data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXIMAGE3DPROC >( "glCompressedTexImage3D", out _glCompressedTexImage3D );

        _glCompressedTexImage3D( target, level, internalformat, width, height, depth, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void CompressedTexImage3D( GLenum target,
                                      GLint level,
                                      GLenum internalformat,
                                      GLsizei width,
                                      GLsizei height,
                                      GLsizei depth,
                                      GLint border,
                                      byte[] data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXIMAGE3DPROC >( "glCompressedTexImage3D", out _glCompressedTexImage3D );

        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage3D( target, level, internalformat, width, height, depth, border, data.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CompressedTexImage2D( GLenum target,
                                      GLint level,
                                      GLenum internalformat,
                                      GLsizei width,
                                      GLsizei height,
                                      GLint border,
                                      GLsizei imageSize,
                                      void* data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXIMAGE2DPROC >( "glCompressedTexImage2D", out _glCompressedTexImage2D );

        _glCompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void CompressedTexImage2D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLsizei height, GLint border, byte[] data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXIMAGE2DPROC >( "glCompressedTexImage2D", out _glCompressedTexImage2D );

        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage2D( target, level, internalformat, width, height, border, data.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLsizei imageSize, void* data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXIMAGE1DPROC >( "glCompressedTexImage1D", out _glCompressedTexImage1D );

        _glCompressedTexImage1D( target, level, internalformat, width, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void CompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, byte[] data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXIMAGE1DPROC >( "glCompressedTexImage1D", out _glCompressedTexImage1D );

        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage1D( target, level, internalformat, width, border, data.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CompressedTexSubImage3D( GLenum target,
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
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC >( "glCompressedTexSubImage3D", out _glCompressedTexSubImage3D );

        _glCompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void CompressedTexSubImage3D( GLenum target,
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
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC >( "glCompressedTexSubImage3D", out _glCompressedTexSubImage3D );

        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, data.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CompressedTexSubImage2D( GLenum target,
                                         GLint level,
                                         GLint xoffset,
                                         GLint yoffset,
                                         GLsizei width,
                                         GLsizei height,
                                         GLenum format,
                                         GLsizei imageSize,
                                         void* data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC >( "glCompressedTexSubImage2D", out _glCompressedTexSubImage2D );

        _glCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void CompressedTexSubImage2D( GLenum target,
                                         GLint level,
                                         GLint xoffset,
                                         GLint yoffset,
                                         GLsizei width,
                                         GLsizei height,
                                         GLenum format,
                                         byte[] data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC >( "glCompressedTexSubImage2D", out _glCompressedTexSubImage2D );

        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, data.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC >( "glCompressedTexSubImage1D", out _glCompressedTexSubImage1D );

        _glCompressedTexSubImage1D( target, level, xoffset, width, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void CompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, byte[] data )
    {
        GetDelegateForFunction< PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC >( "glCompressedTexSubImage1D", out _glCompressedTexSubImage1D );

        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage1D( target, level, xoffset, width, format, data.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetCompressedTexImage( GLenum target, GLint level, void* img )
    {
        GetDelegateForFunction< PFNGLGETCOMPRESSEDTEXIMAGEPROC >( "glGetCompressedTexImage", out _glGetCompressedTexImage );

        _glGetCompressedTexImage( target, level, img );
    }

    /// <inheritdoc/>
    public void GetCompressedTexImage( GLenum target, GLint level, ref byte[] img )
    {
        GetDelegateForFunction< PFNGLGETCOMPRESSEDTEXIMAGEPROC >( "glGetCompressedTexImage", out _glGetCompressedTexImage );

        fixed ( byte* p = &img[ 0 ] )
        {
            _glGetCompressedTexImage( target, level, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BlendFuncSeparate( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha )
    {
        GetDelegateForFunction< PFNGLBLENDFUNCSEPARATEPROC >( "glBlendFuncSeperate", out _glBlendFuncSeparate );

        _glBlendFuncSeparate( sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void MultiDrawArrays( GLenum mode, GLint* first, GLsizei* count, GLsizei drawcount )
    {
        GetDelegateForFunction< PFNGLMULTIDRAWARRAYSPROC >( "glMultiDrawArrays", out _glMultiDrawArrays );

        _glMultiDrawArrays( mode, first, count, drawcount );
    }

    /// <inheritdoc/>
    public void MultiDrawArrays( GLenum mode, GLint[] first, GLsizei[] count )
    {
        if ( first.Length != count.Length )
        {
            throw new ArgumentException( "first and count arrays must be of the same length" );
        }

        GetDelegateForFunction< PFNGLMULTIDRAWARRAYSPROC >( "glMultiDrawArrays", out _glMultiDrawArrays );

        fixed ( GLint* p1 = &first[ 0 ] )
        {
            fixed ( GLsizei* p2 = &count[ 0 ] )
            {
                _glMultiDrawArrays( mode, p1, p2, count.Length );
            }
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void MultiDrawElements( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount )
    {
        GetDelegateForFunction< PFNGLMULTIDRAWELEMENTSPROC >( "glMultiDrawElements", out _glMultiDrawElements );

        _glMultiDrawElements( mode, count, type, indices, drawcount );
    }

    /// <inheritdoc/>
    public void MultiDrawElements< T >( GLenum mode, GLsizei[] count, GLenum type, T[][] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        var indexPtrs = new void*[ indices.Length ];

        for ( var i = 0; i < indices.Length; i++ )
        {
            fixed ( void* p = &indices[ i ][ 0 ] )
            {
                indexPtrs[ i ] = p;
            }
        }

        GetDelegateForFunction< PFNGLMULTIDRAWELEMENTSPROC >( "glMultiDrawElements", out _glMultiDrawElements );

        fixed ( GLsizei* c = &count[ 0 ] )
        {
            fixed ( void** p = &indexPtrs[ 0 ] )
            {
                _glMultiDrawElements( mode, c, type, p, count.Length );
            }
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PointParameterf( GLenum pname, GLfloat param )
    {
        GetDelegateForFunction< PFNGLPOINTPARAMETERFPROC >( "glPointParameterf", out _glPointParameterf );

        _glPointParameterf( pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PointParameterfv( GLenum pname, GLfloat* parameters )
    {
        GetDelegateForFunction< PFNGLPOINTPARAMETERFVPROC >( "glPointParameterfv", out _glPointParameterfv );

        _glPointParameterfv( pname, parameters );
    }

    /// <inheritdoc/>
    public void PointParameterfv( GLenum pname, GLfloat[] parameters )
    {
        GetDelegateForFunction< PFNGLPOINTPARAMETERFVPROC >( "glPointParameterfv", out _glPointParameterfv );

        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glPointParameterfv( pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PointParameteri( GLenum pname, GLint param )
    {
        GetDelegateForFunction< PFNGLPOINTPARAMETERIPROC >( "glPointParameteri", out _glPointParameteri );

        _glPointParameteri( pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void PointParameteriv( GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< PFNGLPOINTPARAMETERIVPROC >( "glPointParameteriv", out _glPointParameteriv );

        _glPointParameteriv( pname, parameters );
    }

    /// <inheritdoc/>
    public void PointParameteriv( GLenum pname, GLint[] parameters )
    {
        GetDelegateForFunction< PFNGLPOINTPARAMETERIVPROC >( "glPointParameteriv", out _glPointParameteriv );

        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glPointParameteriv( pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BlendColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        GetDelegateForFunction< PFNGLBLENDCOLORPROC >( "glBlendColor", out _glBlendColor );

        _glBlendColor( red, green, blue, alpha );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BlendEquation( GLenum mode )
    {
        GetDelegateForFunction< PFNGLBLENDEQUATIONPROC >( "glBlendEquation", out _glBlendEquation );

        _glBlendEquation( mode );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GenQueries( GLsizei n, GLuint* ids )
    {
        GetDelegateForFunction< PFNGLGENQUERIESPROC >( "glGenQueries", out _glGenQueries );

        _glGenQueries( n, ids );
    }

    /// <inheritdoc/>
    public GLuint[] GenQueries( GLsizei n )
    {
        GetDelegateForFunction< PFNGLGENQUERIESPROC >( "glGenQueries", out _glGenQueries );

        var ret = new GLuint[ n ];

        fixed ( GLuint* p = &ret[ 0 ] )
        {
            _glGenQueries( n, p );
        }

        return ret;
    }

    /// <inheritdoc/>
    public GLuint GenQuery()
    {
        return GenQueries( 1 )[ 0 ];
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DeleteQueries( GLsizei n, GLuint* ids )
    {
        GetDelegateForFunction< PFNGLDELETEQUERIESPROC >( "glDeleteQueries", out _glDeleteQueries );

        _glDeleteQueries( n, ids );
    }

    /// <inheritdoc/>
    public void DeleteQueries( params GLuint[] ids )
    {
        GetDelegateForFunction< PFNGLDELETEQUERIESPROC >( "glDeleteQueries", out _glDeleteQueries );

        fixed ( GLuint* p = &ids[ 0 ] )
        {
            _glDeleteQueries( ids.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsQuery( GLuint id )
    {
        GetDelegateForFunction< PFNGLISQUERYPROC >( "glIsQuery", out _glIsQuery );

        return _glIsQuery( id );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BeginQuery( GLenum target, GLuint id )
    {
        GetDelegateForFunction< PFNGLBEGINQUERYPROC >( "glBeginQuery", out _glBeginQuery );

        _glBeginQuery( target, id );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void EndQuery( GLenum target )
    {
        GetDelegateForFunction< PFNGLENDQUERYPROC >( "glEndQuery", out _glEndQuery );

        _glEndQuery( target );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetQueryiv( GLenum target, GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< PFNGLGETQUERYIVPROC >( "glGetQueryiv", out _glGetQueryiv );

        _glGetQueryiv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetQueryiv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        GetDelegateForFunction< PFNGLGETQUERYIVPROC >( "glGetQueryiv", out _glGetQueryiv );

        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryiv( target, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetQueryObjectiv( GLuint id, GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< PFNGLGETQUERYOBJECTIVPROC >( "glGetQueryObjectiv", out _glGetQueryObjectiv );

        _glGetQueryObjectiv( id, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetQueryObjectiv( GLuint id, GLenum pname, ref GLint[] parameters )
    {
        GetDelegateForFunction< PFNGLGETQUERYOBJECTIVPROC >( "glGetQueryObjectiv", out _glGetQueryObjectiv );

        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryObjectiv( id, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetQueryObjectuiv( GLuint id, GLenum pname, GLuint* parameters )
    {
        GetDelegateForFunction< PFNGLGETQUERYOBJECTUIVPROC >( "glGetQueryObjectuiv", out _glGetQueryObjectuiv );

        _glGetQueryObjectuiv( id, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetQueryObjectuiv( GLuint id, GLenum pname, ref GLuint[] parameters )
    {
        GetDelegateForFunction< PFNGLGETQUERYOBJECTUIVPROC >( "glGetQueryObjectuiv", out _glGetQueryObjectuiv );

        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetQueryObjectuiv( id, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BindBuffer( GLenum target, GLuint buffer )
    {
        GetDelegateForFunction< PFNGLBINDBUFFERPROC >( "glBindBuffer", out _glBindBuffer );
        
        _glBindBuffer( target, buffer );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DeleteBuffers( GLsizei n, GLuint* buffers )
    {
        GetDelegateForFunction< PFNGLDELETEBUFFERSPROC >( "glDeleteBuffers", out _glDeleteBuffers );

        _glDeleteBuffers( n, buffers );
    }

    /// <inheritdoc/>
    public void DeleteBuffers( params GLuint[] buffers )
    {
        GetDelegateForFunction< PFNGLDELETEBUFFERSPROC >( "glDeleteBuffers", out _glDeleteBuffers );

        fixed ( GLuint* p = &buffers[ 0 ] )
        {
            _glDeleteBuffers( buffers.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GenBuffers( GLsizei n, GLuint* buffers )
    {
        GetDelegateForFunction< glGenBuffersDelegate >( "glGenBuffers", out _glGenBuffers );

        _glGenBuffers( n, buffers );
    }

    /// <inheritdoc/>
    public GLuint[] GenBuffers( GLsizei n )
    {
        GetDelegateForFunction< glGenBuffersDelegate >( "glGenBuffers", out _glGenBuffers );

        var ret = new GLuint[ n ];

        fixed ( GLuint* p = &ret[ 0 ] )
        {
            _glGenBuffers( n, p );
        }

        return ret;
    }

    /// <inheritdoc/>
    public GLuint GenBuffer()
    {
        return GenBuffers( 1 )[ 0 ];
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsBuffer( GLuint buffer )
    {
        GetDelegateForFunction< glIsBufferDelegate >( "glIsBuffer", out _glIsBuffer );

        return _glIsBuffer( buffer );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BufferData( GLenum target, GLsizeiptr size, void* data, GLenum usage )
    {
        GetDelegateForFunction< PFNGLBUFFERDATAPROC >( "glBufferData", out _glBufferData );
        
        _glBufferData( target, size, data, usage );
    }

    /// <inheritdoc/>
    public void BufferData< T >( GLenum target, T[] data, GLenum usage ) where T : unmanaged
    {
        GetDelegateForFunction< PFNGLBUFFERDATAPROC >( "glBufferData", out _glBufferData );

        fixed ( T* p = &data[ 0 ] )
        {
            _glBufferData( target, sizeof( T ) * data.Length, p, usage );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data )
    {
        GetDelegateForFunction< PFNGLBUFFERSUBDATAPROC >( "glBufferSubData", out _glBufferSubData );

        _glBufferSubData( target, offset, size, data );
    }

    /// <inheritdoc/>
    public void BufferSubData< T >( GLenum target, GLintptr offsetCount, T[] data ) where T : unmanaged
    {
        GetDelegateForFunction< PFNGLBUFFERSUBDATAPROC >( "glBufferSubData", out _glBufferSubData );

        fixed ( T* p = &data[ 0 ] )
        {
            _glBufferSubData( target, offsetCount * sizeof( T ), sizeof( T ) * data.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glGetBufferSubData( target, offset, size, data );
    }

    /// <inheritdoc/>
    public void GetBufferSubData< T >( GLenum target, GLintptr offsetCount, GLsizei count, ref T[] data ) where T : unmanaged
    {
        fixed ( T* p = &data[ 0 ] )
        {
            _glGetBufferSubData( target, offsetCount * sizeof( T ), sizeof( T ) * count, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void* MapBuffer( GLenum target, GLenum access )
    {
        return _glMapBuffer( target, access );
    }

    /// <inheritdoc/>
    public Span< T > MapBuffer< T >( GLenum target, GLenum access ) where T : unmanaged
    {
        GLint size;

        _glGetBufferParameteriv( target, IGL.GL_BUFFER_SIZE, &size );

        var ret = _glMapBuffer( target, access );

        return new Span< T >( ret, size / sizeof( T ) );
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean UnmapBuffer( GLenum target )
    {
        return _glUnmapBuffer( target );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetBufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetBufferParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetBufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetBufferParameteriv( target, pname, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetBufferPointerv( GLenum target, GLenum pname, void** parameters )
    {
        _glGetBufferPointerv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetBufferPointerv( GLenum target, GLenum pname, ref IntPtr[] parameters )
    {
        fixed ( IntPtr* p = &parameters[ 0 ] )
        {
            _glGetBufferPointerv( target, pname, ( void** )p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BlendEquationSeparate( GLenum modeRGB, GLenum modeAlpha )
    {
        _glBlendEquationSeparate( modeRGB, modeAlpha );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DrawBuffers( GLsizei n, GLenum* bufs )
    {
        _glDrawBuffers( n, bufs );
    }

    /// <inheritdoc/>
    public void DrawBuffers( params GLenum[] bufs )
    {
        fixed ( GLenum* pbufs = &bufs[ 0 ] )
        {
            _glDrawBuffers( bufs.Length, pbufs );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void StencilOpSeparate( GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass )
    {
        _glStencilOpSeparate( face, sfail, dpfail, dppass );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void StencilFuncSeparate( GLenum face, GLenum func, GLint @ref, GLuint mask )
    {
        _glStencilFuncSeparate( face, func, @ref, mask );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void StencilMaskSeparate( GLenum face, GLuint mask )
    {
        _glStencilMaskSeparate( face, mask );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void AttachShader( GLuint program, GLuint shader )
    {
        GetDelegateForFunction< PFNGLATTACHSHADERPROC >( "glAttachShader", out _glAttachShader );

        _glAttachShader( program, shader );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BindAttribLocation( GLuint program, GLuint index, GLchar* name )
    {
        _glBindAttribLocation( program, index, name );
    }

    /// <inheritdoc/>
    public void BindAttribLocation( GLuint program, GLuint index, string name )
    {
        var utf8 = Encoding.UTF8.GetBytes( name );

        fixed ( byte* putf8 = &utf8[ 0 ] )
        {
            _glBindAttribLocation( program, index, putf8 );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void CompileShader( GLuint shader )
    {
        GetDelegateForFunction< PFNGLCOMPILESHADERPROC >( "glCompileShader", out _glCompileShader );

        _glCompileShader( shader );
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLuint CreateProgram()
    {
        GetDelegateForFunction< PFNGLCREATEPROGRAMPROC >( "glCreateProgram", out _glCreateProgram );

        return _glCreateProgram();
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLuint CreateShader( GLenum type )
    {
        GetDelegateForFunction< PFNGLCREATESHADERPROC >( "glCreateShader", out _glCreateShader );
        
        return _glCreateShader( type );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DeleteProgram( GLuint program )
    {
        GetDelegateForFunction< PFNGLDELETEPROGRAMPROC >( "glDeleteProgram", out _glDeleteProgram );

        _glDeleteProgram( program );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DeleteShader( GLuint shader )
    {
        GetDelegateForFunction< PFNGLDELETESHADERPROC >( "glDeleteShader", out _glDeleteShader );

        _glDeleteShader( shader );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DetachShader( GLuint program, GLuint shader )
    {
        _glDetachShader( program, shader );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DisableVertexAttribArray( GLuint index )
    {
        _glDisableVertexAttribArray( index );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void EnableVertexAttribArray( GLuint index )
    {
        _glEnableVertexAttribArray( index );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetActiveAttrib( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name )
    {
        GetDelegateForFunction< PFNGLGETACTIVEATTRIBPROC >( "glGetActiveAttrib", out _glGetActiveAttrib );

        _glGetActiveAttrib( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc/>
    public string GetActiveAttrib( GLuint program, GLuint index, GLsizei bufSize, out GLint size, out GLenum type )
    {
        var     name = stackalloc GLchar[ bufSize ];
        GLsizei len;

        GetDelegateForFunction< PFNGLGETACTIVEATTRIBPROC >( "glGetActiveAttrib", out _glGetActiveAttrib );

        fixed ( GLenum* ptype = &type )
        {
            fixed ( GLint* psize = &size )
            {
                _glGetActiveAttrib( program, index, bufSize, &len, psize, ptype, name );
            }
        }

        return new string( ( sbyte* )name, 0, len, Encoding.UTF8 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetActiveUniform( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name )
    {
        _glGetActiveUniform( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc/>
    public string GetActiveUniform( GLuint program, GLuint index, GLsizei bufSize, out GLint size, out GLenum type )
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

    // ========================================================================

    /// <inheritdoc/>
    public void GetAttachedShaders( GLuint program, GLsizei maxCount, GLsizei* count, GLuint* shaders )
    {
        _glGetAttachedShaders( program, maxCount, count, shaders );
    }

    /// <inheritdoc/>
    public GLuint[] GetAttachedShaders( GLuint program, GLsizei maxCount )
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

    // ========================================================================

    /// <inheritdoc/>
    public GLint GetAttribLocation( GLuint program, GLchar* name )
    {
        return _glGetAttribLocation( program, name );
    }

    /// <inheritdoc/>
    public GLint GetAttribLocation( GLuint program, string name )
    {
        fixed ( GLchar* pname = Encoding.UTF8.GetBytes( name ) )
        {
            return _glGetAttribLocation( program, pname );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetProgramiv( GLuint program, GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< PFNGLGETPROGRAMIVPROC >( "glGetProgramiv", out _glGetProgramiv );

        _glGetProgramiv( program, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetProgramiv( GLuint program, GLenum pname, ref GLint[] parameters )
    {
        GetDelegateForFunction< PFNGLGETPROGRAMIVPROC >( "glGetProgramiv", out _glGetProgramiv );

        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetProgramiv( program, pname, pparams );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetProgramInfoLog( GLuint program, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        GetDelegateForFunction< PFNGLGETPROGRAMINFOLOGPROC >( "glGetProgramInfoLog", out _glGetProgramInfoLog );
        
        _glGetProgramInfoLog( program, bufSize, length, infoLog );
    }

    /// <inheritdoc/>
    public string GetProgramInfoLog( GLuint program, GLsizei bufSize )
    {
        var     infoLog = stackalloc GLchar[ bufSize ];
        GLsizei len;

        GetDelegateForFunction< PFNGLGETPROGRAMINFOLOGPROC >( "glGetProgramInfoLog", out _glGetProgramInfoLog );

        _glGetProgramInfoLog( program, bufSize, &len, infoLog );

        return new string( ( sbyte* )infoLog, 0, len, Encoding.UTF8 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetShaderiv( GLuint shader, GLenum pname, GLint* parameters )
    {
        GetDelegateForFunction< PFNGLGETSHADERIVPROC >( "glGetShaderiv", out _glGetShaderiv );

        _glGetShaderiv( shader, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetShaderiv( GLuint shader, GLenum pname, ref GLint[] parameters )
    {
        GetDelegateForFunction< PFNGLGETSHADERIVPROC >( "glGetShaderiv", out _glGetShaderiv );

        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetShaderiv( shader, pname, pparams );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetShaderInfoLog( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetShaderInfoLog( shader, bufSize, length, infoLog );
    }

    /// <inheritdoc/>
    public string GetShaderInfoLog( GLuint shader, GLsizei bufSize )
    {
        var     infoLog = stackalloc GLchar[ bufSize ];
        GLsizei len;
        
        GetDelegateForFunction< PFNGLGETSHADERINFOLOGPROC >( "glGetShaderInfoLog", out _glGetShaderInfoLog );
        
        _glGetShaderInfoLog( shader, bufSize, &len, infoLog );

        return new string( ( sbyte* )infoLog, 0, len, Encoding.UTF8 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetShaderSource( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* source )
    {
        GetDelegateForFunction< PFNGLGETSHADERSOURCEPROC >( "glGetShaderSource", out _glGetShaderSource );

        _glGetShaderSource( shader, bufSize, length, source );
    }

    /// <inheritdoc/>
    public string GetShaderSource( GLuint shader, GLsizei bufSize = 4096 )
    {
        var     source = stackalloc GLchar[ bufSize ];
        GLsizei len;

        GetDelegateForFunction< PFNGLGETSHADERSOURCEPROC >( "glGetShaderSource", out _glGetShaderSource );
        
        _glGetShaderSource( shader, bufSize, &len, source );

        return new string( ( sbyte* )source, 0, len, Encoding.UTF8 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLint GetUniformLocation( GLuint program, GLchar* name )
    {
        return _glGetUniformLocation( program, name );
    }

    /// <inheritdoc/>
    public GLint GetUniformLocation( GLuint program, string name )
    {
        fixed ( byte* pname = Encoding.UTF8.GetBytes( name ) )
        {
            return _glGetUniformLocation( program, pname );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetUniformfv( GLuint program, GLint location, GLfloat* parameters )
    {
        _glGetUniformfv( program, location, parameters );
    }

    /// <inheritdoc/>
    public void GetUniformfv( GLuint program, GLint location, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* pparams =
                   &parameters[ 0 ] )
        {
            _glGetUniformfv( program, location, pparams );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetUniformiv( GLuint program, GLint location, GLint* parameters )
    {
        _glGetUniformiv( program, location, parameters );
    }

    /// <inheritdoc/>
    public void GetUniformiv( GLuint program, GLint location, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetUniformiv( program, location, pparams );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetVertexAttribdv( GLuint index, GLenum pname, GLdouble* parameters )
    {
        _glGetVertexAttribdv( index, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetVertexAttribdv( GLuint index, GLenum pname, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribdv( index, pname, pparams );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetVertexAttribfv( GLuint index, GLenum pname, GLfloat* parameters )
    {
        _glGetVertexAttribfv( index, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetVertexAttribfv( GLuint index, GLenum pname, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribfv( index, pname, pparams );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetVertexAttribiv( GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetVertexAttribiv( index, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetVertexAttribiv( GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pparams = &parameters[ 0 ] )
        {
            _glGetVertexAttribiv( index, pname, pparams );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetVertexAttribPointerv( GLuint index, GLenum pname, void** pointer )
    {
        _glGetVertexAttribPointerv( index, pname, pointer );
    }

    /// <inheritdoc/>
    public void GetVertexAttribPointerv( GLuint index, GLenum pname, ref uint[] pointer )
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

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsProgram( GLuint program )
    {
        return _glIsProgram( program );
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsShader( GLuint shader )
    {
        return _glIsShader( shader );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void LinkProgram( GLuint program )
    {
        GetDelegateForFunction< PFNGLLINKPROGRAMPROC >( "glLinkProgram", out _glLinkProgram );

        _glLinkProgram( program );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ShaderSource( GLuint shader, GLsizei count, GLchar** str, GLint* length )
    {
        GetDelegateForFunction< PFNGLSHADERSOURCEPROC >( "glShaderSource", out _glShaderSource );

        _glShaderSource( shader, count, str, length );
    }

    /// <inheritdoc/>
    public void ShaderSource( GLuint shader, params string[] @string )
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

        GetDelegateForFunction< PFNGLSHADERSOURCEPROC >( "glShaderSource", out _glShaderSource );
        
        _glShaderSource( shader, count, pstring, length );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UseProgram( GLuint program )
    {
        GetDelegateForFunction< PFNGLUSEPROGRAMPROC >( "glUseProgram", out _glUseProgram );

        _glUseProgram( program );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform1f( GLint location, GLfloat v0 )
    {
        GetDelegateForFunction< PFNGLUNIFORM1FPROC >( "glUniform1f", out _glUniform1f );

        _glUniform1f( location, v0 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform2f( GLint location, GLfloat v0, GLfloat v1 )
    {
        GetDelegateForFunction< PFNGLUNIFORM2FPROC >( "glUniform2f", out _glUniform2f );

        _glUniform2f( location, v0, v1 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform3f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        GetDelegateForFunction< PFNGLUNIFORM3FPROC >( "glUniform3f", out _glUniform3f );

        _glUniform3f( location, v0, v1, v2 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform4f( GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        GetDelegateForFunction< PFNGLUNIFORM4FPROC >( "glUniform4f", out _glUniform4f );
        
        _glUniform4f( location, v0, v1, v2, v3 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform1i( GLint location, GLint v0 )
    {
        GetDelegateForFunction< PFNGLUNIFORM1IPROC >( "glUniform1i", out _glUniform1i );

        _glUniform1i( location, v0 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform2i( GLint location, GLint v0, GLint v1 )
    {
        _glUniform2i( location, v0, v1 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform3i( GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glUniform3i( location, v0, v1, v2 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform4i( GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glUniform4i( location, v0, v1, v2, v3 );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform1fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform1fv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform1fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform1fv( location, value.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform2fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform2fv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform2fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform2fv( location, value.Length / 2, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform3fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform3fv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform3fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform3fv( location, value.Length / 3, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform4fv( GLint location, GLsizei count, GLfloat* value )
    {
        _glUniform4fv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform4fv( GLint location, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniform4fv( location, value.Length / 4, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform1iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform1iv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform1iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform1iv( location, value.Length, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform2iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform2iv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform2iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform2iv( location, value.Length / 2, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform3iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform3iv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform3iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform3iv( location, value.Length / 3, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Uniform4iv( GLint location, GLsizei count, GLint* value )
    {
        _glUniform4iv( location, count, value );
    }

    /// <inheritdoc/>
    public void Uniform4iv( GLint location, params GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glUniform4iv( location, value.Length / 4, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2fv( location, value.Length / 4, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix3fv( location, value.Length / 9, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4fv( location, value.Length / 16, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean ValidateProgram( GLuint program )
    {
        return _glValidateProgram( program );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib1d( GLuint index, GLdouble x )
    {
        _glVertexAttrib1d( index, x );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib1dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib1dv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib1dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib1dv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib1f( GLuint index, GLfloat x )
    {
        _glVertexAttrib1f( index, x );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib1fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib1fv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib1fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib1fv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib1s( GLuint index, GLshort x )
    {
        _glVertexAttrib1s( index, x );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib1sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib1sv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib1sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib1sv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttrib2d( index, x, y );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib2dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib2dv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib2dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib2dv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib2f( GLuint index, GLfloat x, GLfloat y )
    {
        _glVertexAttrib2f( index, x, y );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib2fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib2fv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib2fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib2fv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib2s( GLuint index, GLshort x, GLshort y )
    {
        _glVertexAttrib2s( index, x, y );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib2sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib2sv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib2sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib2sv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib3d( GLuint index, GLdouble x, GLdouble y, GLdouble z )
    {
        _glVertexAttrib3d( index, x, y, z );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib3dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib3dv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib3dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib3dv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib3f( GLuint index, GLfloat x, GLfloat y, GLfloat z )
    {
        _glVertexAttrib3f( index, x, y, z );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib3fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib3fv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib3fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib3fv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib3s( GLuint index, GLshort x, GLshort y, GLshort z )
    {
        _glVertexAttrib3s( index, x, y, z );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib3sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib3sv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib3sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib3sv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4Nbv( GLuint index, GLbyte* v )
    {
        _glVertexAttrib4Nbv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4Nbv( GLuint index, params GLbyte[] v )
    {
        fixed ( GLbyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nbv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4Niv( GLuint index, GLint* v )
    {
        _glVertexAttrib4Niv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4Niv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttrib4Niv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4Nsv( GLuint index, GLshort* v )
    {
        _glVertexAttrib4Nsv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4Nsv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nsv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4Nub( GLuint index, GLubyte x, GLubyte y, GLubyte z, GLubyte w )
    {
        _glVertexAttrib4Nub( index, x, y, z, w );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4Nubv( GLuint index, GLubyte* v )
    {
        _glVertexAttrib4Nubv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4Nubv( GLuint index, params GLubyte[] v )
    {
        fixed ( GLubyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nubv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4Nuiv( GLuint index, GLuint* v )
    {
        _glVertexAttrib4Nuiv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4Nuiv( GLuint index, params GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nuiv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4Nusv( GLuint index, GLushort* v )
    {
        _glVertexAttrib4Nusv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4Nusv( GLuint index, params GLushort[] v )
    {
        fixed ( GLushort* p = &v[ 0 ] )
        {
            _glVertexAttrib4Nusv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4bv( GLuint index, GLbyte* v )
    {
        _glVertexAttrib4bv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4bv( GLuint index, params GLbyte[] v )
    {
        fixed ( GLbyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4bv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glVertexAttrib4d( index, x, y, z, w );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4dv( GLuint index, GLdouble* v )
    {
        _glVertexAttrib4dv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4dv( GLuint index, params GLdouble[] v )
    {
        fixed ( GLdouble* p = &v[ 0 ] )
        {
            _glVertexAttrib4dv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4f( GLuint index, GLfloat x, GLfloat y, GLfloat z, GLfloat w )
    {
        _glVertexAttrib4f( index, x, y, z, w );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4fv( GLuint index, GLfloat* v )
    {
        _glVertexAttrib4fv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4fv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* p = &v[ 0 ] )
        {
            _glVertexAttrib4fv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4iv( GLuint index, GLint* v )
    {
        _glVertexAttrib4iv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4iv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttrib4iv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4s( GLuint index, GLshort x, GLshort y, GLshort z, GLshort w )
    {
        _glVertexAttrib4s( index, x, y, z, w );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4sv( GLuint index, GLshort* v )
    {
        _glVertexAttrib4sv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4sv( GLuint index, params GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttrib4sv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4ubv( GLuint index, GLubyte* v )
    {
        _glVertexAttrib4ubv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4ubv( GLuint index, params GLubyte[] v )
    {
        fixed ( GLubyte* p = &v[ 0 ] )
        {
            _glVertexAttrib4ubv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4uiv( GLuint index, GLuint* v )
    {
        _glVertexAttrib4uiv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4uiv( GLuint index, params GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttrib4uiv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttrib4usv( GLuint index, GLushort* v )
    {
        _glVertexAttrib4usv( index, v );
    }

    /// <inheritdoc/>
    public void VertexAttrib4usv( GLuint index, params GLushort[] v )
    {
        fixed ( GLushort* p = &v[ 0 ] )
        {
            _glVertexAttrib4usv( index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribPointer( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, void* pointer )
    {
        _glVertexAttribPointer( index, size, type, normalized, stride, pointer );
    }

    /// <inheritdoc/>
    public void VertexAttribPointer( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, uint pointer )
    {
        _glVertexAttribPointer( index, size, type, normalized, stride, ( void* )pointer );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix2x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2x3fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix2x3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2x3fv( location, value.Length / 6, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix3x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3x2fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix3x2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix3x2fv( location, value.Length / 6, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix2x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix2x4fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix2x4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix2x4fv( location, value.Length / 8, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix4x2fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4x2fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix4x2fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4x2fv( location, value.Length / 8, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix3x4fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix3x4fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix3x4fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p = &value[ 0 ] )
        {
            _glUniformMatrix3x4fv( location, value.Length / 12, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void UniformMatrix4x3fv( GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glUniformMatrix4x3fv( location, count, transpose, value );
    }

    /// <inheritdoc/>
    public void UniformMatrix4x3fv( GLint location, GLboolean transpose, params GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glUniformMatrix4x3fv( location, value.Length / 12, transpose, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void ColorMaski( GLuint index, GLboolean r, GLboolean g, GLboolean b, GLboolean a )
    {
        _glColorMaski( index, r, g, b, a );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetBooleani_v( GLenum target, GLuint index, GLboolean* data )
    {
        _glGetBooleani_v( target, index, data );
    }

    /// <inheritdoc/>
    public void GetBooleani_v( GLenum target, GLuint index, ref GLboolean[] data )
    {
        fixed ( GLboolean* p = &data[ 0 ] )
        {
            _glGetBooleani_v( target, index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetIntegeri_v( GLenum target, GLuint index, GLint* data )
    {
        _glGetIntegeri_v( target, index, data );
    }

    /// <inheritdoc/>
    public void GetIntegeri_v( GLenum target, GLuint index, ref GLint[] data )
    {
        fixed ( GLint* p = &data[ 0 ] )
        {
            _glGetIntegeri_v( target, index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Enablei( GLenum target, GLuint index )
    {
        _glEnablei( target, index );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void Disablei( GLenum target, GLuint index )
    {
        _glDisablei( target, index );
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsEnabledi( GLenum target, GLuint index )
    {
        return _glIsEnabledi( target, index );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BeginTransformFeedback( GLenum primitiveMode )
    {
        _glBeginTransformFeedback( primitiveMode );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void EndTransformFeedback()
    {
        _glEndTransformFeedback();
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BindBufferRange( GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glBindBufferRange( target, index, buffer, offset, size );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BindBufferBase( GLenum target, GLuint index, GLuint buffer )
    {
        _glBindBufferBase( target, index, buffer );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void WaitSync( void* sync, GLbitfield flags, GLuint64 timeout )
    {
        _glWaitSync( sync, flags, timeout );
    }

    /// <inheritdoc/>
    public void WaitSyncSafe( IntPtr sync, GLbitfield flags, GLuint64 timeout )
    {
        _glWaitSync( sync.ToPointer(), flags, timeout );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetInteger64v( GLenum pname, GLint64* data )
    {
        _glGetInteger64v( pname, data );
    }

    /// <inheritdoc/>
    public void GetInteger64v( GLenum pname, ref GLint64[] data )
    {
        fixed ( GLint64* dp = &data[ 0 ] )
        {
            _glGetInteger64v( pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetSynciv( void* sync, GLenum pname, GLsizei bufSize, GLsizei* length, GLint* values )
    {
        _glGetSynciv( sync, pname, bufSize, length, values );
    }

    /// <inheritdoc/>
    public GLint[] GetSynciv( IntPtr sync, GLenum pname, GLsizei bufSize )
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

    // ========================================================================

    /// <inheritdoc/>
    public void GetInteger64i_v( GLenum target, GLuint index, GLint64* data )
    {
        _glGetInteger64i_v( target, index, data );
    }

    /// <inheritdoc/>
    public void GetInteger64i_v( GLenum target, GLuint index, ref GLint64[] data )
    {
        fixed ( GLint64* dp = &data[ 0 ] )
        {
            _glGetInteger64i_v( target, index, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetBufferParameteri64v( GLenum target, GLenum pname, GLint64* parameters )
    {
        _glGetBufferParameteri64v( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void GetBufferParameteri64v( GLenum target, GLenum pname, ref GLint64[] parameters )
    {
        fixed ( GLint64* dp = &parameters[ 0 ] )
        {
            _glGetBufferParameteri64v( target, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void FramebufferTexture( GLenum target, GLenum attachment, GLuint texture, GLint level )
    {
        _glFramebufferTexture( target, attachment, texture, level );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexImage2DMultisample( GLenum target,
                                       GLsizei samples,
                                       GLenum internalformat,
                                       GLsizei width,
                                       GLsizei height,
                                       GLboolean fixedsamplelocations )
    {
        _glTexImage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void TexImage3DMultisample( GLenum target,
                                       GLsizei samples,
                                       GLenum internalformat,
                                       GLsizei width,
                                       GLsizei height,
                                       GLsizei depth,
                                       GLboolean fixedsamplelocations )
    {
        _glTexImage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetMultisamplefv( GLenum pname, GLuint index, GLfloat* val )
    {
        _glGetMultisamplefv( pname, index, val );
    }

    /// <inheritdoc/>
    public void GetMultisamplefvSafe( GLenum pname, GLuint index, ref GLfloat[] val )
    {
        fixed ( GLfloat* dp = &val[ 0 ] )
        {
            _glGetMultisamplefv( pname, index, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SampleMaski( GLuint maskNumber, GLbitfield mask )
    {
        _glSampleMaski( maskNumber, mask );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BindFragDataLocationIndexed( GLuint program, GLuint colorNumber, GLuint index, GLchar* name )
    {
        _glBindFragDataLocationIndexed( program, colorNumber, index, name );
    }

    /// <inheritdoc/>
    public void BindFragDataLocationIndexed( GLuint program, GLuint colorNumber, GLuint index, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            _glBindFragDataLocationIndexed( program, colorNumber, index, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLint GetFragDataIndex( GLuint program, GLchar* name )
    {
        return _glGetFragDataIndex( program, name );
    }

    /// <inheritdoc/>
    public GLint GetFragDataIndex( GLuint program, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetFragDataIndex( program, p );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GenSamplers( GLsizei count, GLuint* samplers )
    {
        _glGenSamplers( count, samplers );
    }

    /// <inheritdoc/>
    public GLuint[] GenSamplers( GLsizei count )
    {
        var result = new GLuint[ count ];

        fixed ( GLuint* dp = &result[ 0 ] )
        {
            _glGenSamplers( count, dp );
        }

        return result;
    }

    /// <inheritdoc/>
    public GLuint GenSampler()
    {
        return GenSamplers( 1 )[ 0 ];
    }

    // ========================================================================

    /// <inheritdoc/>
    public void DeleteSamplers( GLsizei count, GLuint* samplers )
    {
        _glDeleteSamplers( count, samplers );
    }

    /// <inheritdoc/>
    public void DeleteSamplers( params GLuint[] samplers )
    {
        fixed ( GLuint* dp = &samplers[ 0 ] )
        {
            _glDeleteSamplers( samplers.Length, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public GLboolean IsSampler( GLuint sampler )
    {
        return _glIsSampler( sampler );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void BindSampler( GLuint unit, GLuint sampler )
    {
        _glBindSampler( unit, sampler );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SamplerParameteri( GLuint sampler, GLenum pname, GLint param )
    {
        _glSamplerParameteri( sampler, pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SamplerParameteriv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glSamplerParameteriv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void SamplerParameteriv( GLuint sampler, GLenum pname, GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glSamplerParameteriv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SamplerParameterf( GLuint sampler, GLenum pname, GLfloat param )
    {
        _glSamplerParameterf( sampler, pname, param );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param )
    {
        _glSamplerParameterfv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void SamplerParameterfv( GLuint sampler, GLenum pname, GLfloat[] param )
    {
        fixed ( GLfloat* dp = &param[ 0 ] )
        {
            _glSamplerParameterfv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glSamplerParameterIiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void SamplerParameterIiv( GLuint sampler, GLenum pname, GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glSamplerParameterIiv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void SamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param )
    {
        _glSamplerParameterIuiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void SamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* dp =
                   &param[ 0 ] )
        {
            _glSamplerParameterIuiv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetSamplerParameteriv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glGetSamplerParameteriv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void GetSamplerParameteriv( GLuint sampler, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameteriv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetSamplerParameterIiv( GLuint sampler, GLenum pname, GLint* param )
    {
        _glGetSamplerParameterIiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void GetSamplerParameterIiv( GLuint sampler, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameterIiv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetSamplerParameterfv( GLuint sampler, GLenum pname, GLfloat* param )
    {
        _glGetSamplerParameterfv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void GetSamplerParameterfv( GLuint sampler, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* dp =
                   &param[ 0 ] )
        {
            _glGetSamplerParameterfv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetSamplerParameterIuiv( GLuint sampler, GLenum pname, GLuint* param )
    {
        _glGetSamplerParameterIuiv( sampler, pname, param );
    }

    /// <inheritdoc/>
    public void GetSamplerParameterIuiv( GLuint sampler, GLenum pname, ref GLuint[] param )
    {
        fixed ( GLuint* dp = &param[ 0 ] )
        {
            _glGetSamplerParameterIuiv( sampler, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void QueryCounter( GLuint id, GLenum target )
    {
        _glQueryCounter( id, target );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetQueryObjecti64v( GLuint id, GLenum pname, GLint64* param )
    {
        _glGetQueryObjecti64v( id, pname, param );
    }

    /// <inheritdoc/>
    public void GetQueryObjecti64v( GLuint id, GLenum pname, ref GLint64[] param )
    {
        fixed ( GLint64* dp =
                   &param[ 0 ] )
        {
            _glGetQueryObjecti64v( id, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void GetQueryObjectui64v( GLuint id, GLenum pname, GLuint64* param )
    {
        _glGetQueryObjectui64v( id, pname, param );
    }

    /// <inheritdoc/>
    public void GetQueryObjectui64v( GLuint id, GLenum pname, ref GLuint64[] param )
    {
        fixed ( GLuint64* dp = &param[ 0 ] )
        {
            _glGetQueryObjectui64v( id, pname, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribDivisor( GLuint index, GLuint divisor )
    {
        _glVertexAttribDivisor( index, divisor );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP1ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP1ui( index, type, normalized, value );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP1uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP1uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void VertexAttribP1uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP1uiv( index, type, normalized, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP2ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP2ui( index, type, normalized, value );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP2uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP2uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void VertexAttribP2uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP2uiv( index, type, normalized, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP3ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP3ui( index, type, normalized, value );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP3uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP3uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void VertexAttribP3uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP3uiv( index, type, normalized, dp );
        }
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP4ui( GLuint index, GLenum type, GLboolean normalized, GLuint value )
    {
        _glVertexAttribP4ui( index, type, normalized, value );
    }

    // ========================================================================

    /// <inheritdoc/>
    public void VertexAttribP4uiv( GLuint index, GLenum type, GLboolean normalized, GLuint* value )
    {
        _glVertexAttribP4uiv( index, type, normalized, value );
    }

    /// <inheritdoc/>
    public void VertexAttribP4uiv( GLuint index, GLenum type, GLboolean normalized, GLuint[] value )
    {
        fixed ( GLuint* dp = &value[ 0 ] )
        {
            _glVertexAttribP4uiv( index, type, normalized, dp );
        }
    }

    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void MinSampleShading( GLfloat value )
    {
        _glMinSampleShading( value );
    }

    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="mode"></param>
    public void BlendEquationi( GLuint buf, GLenum mode )
    {
        _glBlendEquationi( buf, mode );
    }

    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="modeRGB"></param>
    /// <param name="modeAlpha"></param>
    public void BlendEquationSeparatei( GLuint buf, GLenum modeRGB, GLenum modeAlpha )
    {
        _glBlendEquationSeparatei( buf, modeRGB, modeAlpha );
    }

    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    public void BlendFunci( GLuint buf, GLenum src, GLenum dst )
    {
        _glBlendFunci( buf, src, dst );
    }

    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="srcRGB"></param>
    /// <param name="dstRGB"></param>
    /// <param name="srcAlpha"></param>
    /// <param name="dstAlpha"></param>
    public void BlendFuncSeparatei( GLuint buf, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha )
    {
        _glBlendFuncSeparatei( buf, srcRGB, dstRGB, srcAlpha, dstAlpha );
    }

    // ========================================================================

    public struct DrawArraysIndirectCommand
    {
        public GLuint Count         { get; set; }
        public GLuint InstanceCount { get; set; }
        public GLuint First         { get; set; }
        public GLuint BaseInstance  { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="indirect"></param>
    public void DrawArraysIndirect( GLenum mode, void* indirect )
    {
        _glDrawArraysIndirect( mode, indirect );
    }

    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="indirect"></param>
    public void DrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect )
    {
        _glDrawArraysIndirect( mode, &indirect );
    }

    // ========================================================================

    public struct DrawElementsIndirectCommand
    {
        public GLuint Count;
        public GLuint PrimCount;
        public GLuint FirstIndex;
        public GLuint BaseVertex;
        public GLuint BaseInstance;
    }

    public void DrawElementsIndirect( GLenum mode, GLenum type, void* indirect )
    {
        _glDrawElementsIndirect( mode, type, indirect );
    }

    public void DrawElementsIndirect( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect )
    {
        _glDrawElementsIndirect( mode, type, &indirect );
    }

    // ========================================================================

    public void Uniform1d( GLint location, GLdouble x )
    {
        _glUniform1d( location, x );
    }

    // ========================================================================

    public void Uniform2d( GLint location, GLdouble x, GLdouble y )
    {
        _glUniform2d( location, x, y );
    }

    // ========================================================================

    public void Uniform3d( GLint location, GLdouble x, GLdouble y, GLdouble z )
    {
        _glUniform3d( location, x, y, z );
    }

    // ========================================================================

    public void Uniform4d( GLint location, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glUniform4d( location, x, y, z, w );
    }

    // ========================================================================

    public void Uniform1dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform1dv( location, count, value );
    }

    public void Uniform1dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform1dv( location, value.Length, p );
        }
    }

    // ========================================================================

    public void Uniform2dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform2dv( location, count, value );
    }

    public void Uniform2dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform2dv( location, value.Length / 2, p );
        }
    }

    // ========================================================================

    public void Uniform3dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform3dv( location, count, value );
    }

    public void Uniform3dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform3dv( location, value.Length / 3, p );
        }
    }

    // ========================================================================

    public void Uniform4dv( GLint location, GLsizei count, GLdouble* value )
    {
        _glUniform4dv( location, count, value );
    }

    public void Uniform4dv( GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniform4dv( location, value.Length / 4, p );
        }
    }

    // ========================================================================

    public void UniformMatrix2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2dv( location, count, transpose, value );
    }

    public void UniformMatrix2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix2dv( location, value.Length / 4, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3dv( location, count, transpose, value );
    }

    public void UniformMatrix3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix3dv( location, value.Length / 9, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4dv( location, count, transpose, value );
    }

    public void UniformMatrix4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix4dv( location, value.Length / 16, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix2x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2x3dv( location, count, transpose, value );
    }

    public void UniformMatrix2x3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix2x3dv( location, value.Length / 6, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix2x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix2x4dv( location, count, transpose, value );
    }

    public void UniformMatrix2x4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix2x4dv( location, value.Length / 8, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix3x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3x2dv( location, count, transpose, value );
    }

    public void UniformMatrix3x2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix3x2dv( location, value.Length / 6, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix3x4dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix3x4dv( location, count, transpose, value );
    }

    public void UniformMatrix3x4dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix3x4dv( location, value.Length / 12, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix4x2dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4x2dv( location, count, transpose, value );
    }

    public void UniformMatrix4x2dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix4x2dv( location, value.Length / 8, transpose, p );
        }
    }

    // ========================================================================

    public void UniformMatrix4x3dv( GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glUniformMatrix4x3dv( location, count, transpose, value );
    }

    public void UniformMatrix4x3dv( GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* p = &value[ 0 ] )
        {
            _glUniformMatrix4x3dv( location, value.Length / 12, transpose, p );
        }
    }

    // ========================================================================

    public void GetUniformdv( GLuint program, GLint location, GLdouble* parameters )
    {
        _glGetUniformdv( program, location, parameters );
    }

    public void GetUniformdv( GLuint program, GLint location, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* p = &parameters[ 0 ] )
        {
            _glGetUniformdv( program, location, p );
        }
    }

    // ========================================================================

    public GLint GetSubroutineUniformLocation( GLuint program, GLenum shadertype, GLchar* name )
    {
        return _glGetSubroutineUniformLocation( program, shadertype, name );
    }

    public GLint GetSubroutineUniformLocation( GLuint program, GLenum shadertype, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetSubroutineUniformLocation( program, shadertype, p );
        }
    }

    // ========================================================================

    public GLuint GetSubroutineIndex( GLuint program, GLenum shadertype, GLchar* name )
    {
        return _glGetSubroutineIndex( program, shadertype, name );
    }

    public GLuint GetSubroutineIndex( GLuint program, GLenum shadertype, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &nameBytes[ 0 ] )
        {
            return _glGetSubroutineIndex( program, shadertype, p );
        }
    }

    // ========================================================================

    public void GetActiveSubroutineUniformiv( GLuint program, GLenum shadertype, GLuint index, GLenum pname, GLint* values )
    {
        _glGetActiveSubroutineUniformiv( program, shadertype, index, pname, values );
    }

    public void GetActiveSubroutineUniformiv( GLuint program, GLenum shadertype, GLuint index, GLenum pname, ref GLint[] values )
    {
        fixed ( GLint* p = &values[ 0 ] )
        {
            _glGetActiveSubroutineUniformiv( program, shadertype, index, pname, p );
        }
    }

    // ========================================================================

    public void GetActiveSubroutineUniformName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name )
    {
        _glGetActiveSubroutineUniformName( program, shadertype, index, bufsize, length, name );
    }

    public string GetActiveSubroutineUniformName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize )
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

    public void GetActiveSubroutineName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name )
    {
        _glGetActiveSubroutineName( program, shadertype, index, bufsize, length, name );
    }

    public string GetActiveSubroutineName( GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize )
    {
        var name = new GLchar[ bufsize ];

        fixed ( GLchar* p = &name[ 0 ] )
        {
            GLsizei length;

            _glGetActiveSubroutineName( program, shadertype, index, bufsize, &length, p );

            return new string( ( GLbyte* )p, 0, length, Encoding.UTF8 );
        }
    }

    // ========================================================================

    public void UniformSubroutinesuiv( GLenum shadertype, GLsizei count, GLuint* indices )
    {
        _glUniformSubroutinesuiv( shadertype, count, indices );
    }

    public void UniformSubroutinesuiv( GLenum shadertype, GLuint[] indices )
    {
        fixed ( GLuint* p = &indices[ 0 ] )
        {
            _glUniformSubroutinesuiv( shadertype, indices.Length, p );
        }
    }

    // ========================================================================

    public void GetUniformSubroutineuiv( GLenum shadertype, GLint location, GLuint* parameters )
    {
        _glGetUniformSubroutineuiv( shadertype, location, parameters );
    }

    public void GetUniformSubroutineuiv( GLenum shadertype, GLint location, ref GLuint[] parameters )
    {
        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetUniformSubroutineuiv( shadertype, location, p );
        }
    }

    // ========================================================================

    public void GetProgramStageiv( GLuint program, GLenum shadertype, GLenum pname, GLint* values )
    {
        _glGetProgramStageiv( program, shadertype, pname, values );
    }

    public void GetProgramStageiv( GLuint program, GLenum shadertype, GLenum pname, ref GLint[] values )
    {
        fixed ( GLint* p = &values[ 0 ] )
        {
            _glGetProgramStageiv( program, shadertype, pname, p );
        }
    }

    // ========================================================================

    public void PatchParameteri( GLenum pname, GLint value )
    {
        _glPatchParameteri( pname, value );
    }

    // ========================================================================

    public void PatchParameterfv( GLenum pname, GLfloat* values )
    {
        _glPatchParameterfv( pname, values );
    }

    public void PatchParameterfv( GLenum pname, GLfloat[] values )
    {
        fixed ( GLfloat* p = &values[ 0 ] )
        {
            _glPatchParameterfv( pname, p );
        }
    }

    // ========================================================================

    public void BindTransformFeedback( GLenum target, GLuint id )
    {
        _glBindTransformFeedback( target, id );
    }

    // ========================================================================

    public void DeleteTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glDeleteTransformFeedbacks( n, ids );
    }

    public void DeleteTransformFeedbacks( params GLuint[] ids )
    {
        fixed ( GLuint* p = &ids[ 0 ] )
        {
            _glDeleteTransformFeedbacks( ids.Length, p );
        }
    }

    // ========================================================================

    public void GenTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glGenTransformFeedbacks( n, ids );
    }

    public GLuint[] GenTransformFeedbacks( GLsizei n )
    {
        var r = new GLuint[ n ];

        fixed ( GLuint* p = &r[ 0 ] )
        {
            _glGenTransformFeedbacks( n, p );
        }

        return r;
    }

    public GLuint GenTransformFeedback()
    {
        return GenTransformFeedbacks( 1 )[ 0 ];
    }

    // ========================================================================

    public GLboolean IsTransformFeedback( GLuint id )
    {
        return _glIsTransformFeedback( id );
    }

    // ========================================================================

    public void PauseTransformFeedback()
    {
        _glPauseTransformFeedback();
    }

    // ========================================================================

    public void ResumeTransformFeedback()
    {
        _glResumeTransformFeedback();
    }

    // ========================================================================

    public void DrawTransformFeedback( GLenum mode, GLuint id )
    {
        _glDrawTransformFeedback( mode, id );
    }

    // ========================================================================

    public void DrawTransformFeedbackStream( GLenum mode, GLuint id, GLuint stream )
    {
        _glDrawTransformFeedbackStream( mode, id, stream );
    }

    // ========================================================================

    public void BeginQueryIndexed( GLenum target, GLuint index, GLuint id )
    {
        _glBeginQueryIndexed( target, index, id );
    }

    // ========================================================================

    public void EndQueryIndexed( GLenum target, GLuint index )
    {
        _glEndQueryIndexed( target, index );
    }

    // ========================================================================

    public void GetQueryIndexediv( GLenum target, GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetQueryIndexediv( target, index, pname, parameters );
    }

    public void GetQueryIndexediv( GLenum target, GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryIndexediv( target, index, pname, p );
        }
    }

    // ========================================================================

    public void ReleaseShaderCompiler()
    {
        _glReleaseShaderCompiler();
    }

    // ========================================================================

    public void ShaderBinary( GLsizei count, GLuint* shaders, GLenum binaryformat, void* binary, GLsizei length )
    {
        _glShaderBinary( count, shaders, binaryformat, binary, length );
    }

    public void ShaderBinary( GLuint[] shaders, GLenum binaryformat, byte[] binary )
    {
        var count = shaders.Length;

        fixed ( GLuint* pShaders = &shaders[ 0 ] )
        fixed ( byte* pBinary = &binary[ 0 ] )
        {
            _glShaderBinary( count, pShaders, binaryformat, pBinary, binary.Length );
        }
    }

    // ========================================================================

    public void GetShaderPrecisionFormat( GLenum shaderType, GLenum precisionType, GLint* range, GLint* precision )
    {
        _glGetShaderPrecisionFormat( shaderType, precisionType, range, precision );
    }

    public void GetShaderPrecisionFormat( GLenum shaderType, GLenum precisionType, ref GLint[] range, ref GLint precision )
    {
        range = new GLint[ 2 ];

        fixed ( GLint* pRange = &range[ 0 ] )
        fixed ( GLint* pPrecision = &precision )
        {
            _glGetShaderPrecisionFormat( shaderType, precisionType, pRange, pPrecision );
        }
    }

    // ========================================================================

    public void DepthRangef( GLfloat n, GLfloat f )
    {
        _glDepthRangef( n, f );
    }

    // ========================================================================

    public void ClearDepthf( GLfloat d )
    {
        _glClearDepthf( d );
    }

    // ========================================================================

    public void GetProgramBinary( GLuint program, GLsizei bufSize, GLsizei* length, GLenum* binaryFormat, void* binary )
    {
        _glGetProgramBinary( program, bufSize, length, binaryFormat, binary );
    }

    public byte[] GetProgramBinary( GLuint program, GLsizei bufSize, out GLenum binaryFormat )
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

    public void ProgramBinary( GLuint program, GLenum binaryFormat, void* binary, GLsizei length )
    {
        _glProgramBinary( program, binaryFormat, binary, length );
    }

    public void ProgramBinary( GLuint program, GLenum binaryFormat, byte[] binary )
    {
        fixed ( byte* pBinary = &binary[ 0 ] )
        {
            _glProgramBinary( program, binaryFormat, pBinary, binary.Length );
        }
    }

    // ========================================================================

    public void ProgramParameteri( GLuint program, GLenum pname, GLint value )
    {
        _glProgramParameteri( program, pname, value );
    }

    // ========================================================================

    public void UseProgramStages( GLuint pipeline, GLbitfield stages, GLuint program )
    {
        _glUseProgramStages( pipeline, stages, program );
    }

    // ========================================================================

    public void ActiveShaderProgram( GLuint pipeline, GLuint program )
    {
        _glActiveShaderProgram( pipeline, program );
    }

    // ========================================================================

    public GLuint CreateShaderProgramv( GLenum type, GLsizei count, GLchar** strings )
    {
        return _glCreateShaderProgramv( type, count, strings );
    }

    public GLuint CreateShaderProgramv( GLenum type, string[] strings )
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

    public void BindProgramPipeline( GLuint pipeline )
    {
        _glBindProgramPipeline( pipeline );
    }

    // ========================================================================

    public void DeleteProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glDeleteProgramPipelines( n, pipelines );
    }

    public void DeleteProgramPipelines( params GLuint[] pipelines )
    {
        fixed ( GLuint* pPipelines = &pipelines[ 0 ] )
        {
            _glDeleteProgramPipelines( pipelines.Length, pPipelines );
        }
    }

    // ========================================================================

    public void GenProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glGenProgramPipelines( n, pipelines );
    }

    public GLuint[] GenProgramPipelines( GLsizei n )
    {
        var pipelines = new GLuint[ n ];

        fixed ( GLuint* pPipelines = &pipelines[ 0 ] )
        {
            _glGenProgramPipelines( n, pPipelines );
        }

        return pipelines;
    }

    public GLuint GenProgramPipeline()
    {
        return GenProgramPipelines( 1 )[ 0 ];
    }

    // ========================================================================

    public GLboolean IsProgramPipeline( GLuint pipeline )
    {
        return _glIsProgramPipeline( pipeline );
    }

    // ========================================================================

    public void GetProgramPipelineiv( GLuint pipeline, GLenum pname, GLint* param )
    {
        _glGetProgramPipelineiv( pipeline, pname, param );
    }

    public void GetProgramPipelineiv( GLuint pipeline, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* pParam = &param[ 0 ] )
        {
            _glGetProgramPipelineiv( pipeline, pname, pParam );
        }
    }

    // ========================================================================

    public void ProgramUniform1i( GLuint program, GLint location, GLint v0 )
    {
        _glProgramUniform1i( program, location, v0 );
    }

    // ========================================================================

    public void ProgramUniform1iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform1iv( program, location, count, value );
    }

    public void ProgramUniform1iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue = &value[ 0 ] )
        {
            _glProgramUniform1iv( program, location, value.Length, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform1f( GLuint program, GLint location, GLfloat v0 )
    {
        _glProgramUniform1f( program, location, v0 );
    }

    // ========================================================================

    public void ProgramUniform1fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform1fv( program, location, count, value );
    }

    public void ProgramUniform1fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniform1fv( program, location, value.Length, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform1d( GLuint program, GLint location, GLdouble v0 )
    {
        _glProgramUniform1d( program, location, v0 );
    }

    // ========================================================================

    public void ProgramUniform1dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform1dv( program, location, count, value );
    }

    public void ProgramUniform1dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniform1dv( program, location, value.Length, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform1ui( GLuint program, GLint location, GLuint v0 )
    {
        _glProgramUniform1ui( program, location, v0 );
    }

    // ========================================================================

    public void ProgramUniform1uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform1uiv( program, location, count, value );
    }

    public void ProgramUniform1uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue = &value[ 0 ] )
        {
            _glProgramUniform1uiv( program, location, value.Length, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform2i( GLuint program, GLint location, GLint v0, GLint v1 )
    {
        _glProgramUniform2i( program, location, v0, v1 );
    }

    // ========================================================================

    public void ProgramUniform2iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform2iv( program, location, count, value );
    }

    public void ProgramUniform2iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue = &value[ 0 ] )
        {
            _glProgramUniform2iv( program, location, value.Length / 2, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform2f( GLuint program, GLint location, GLfloat v0, GLfloat v1 )
    {
        _glProgramUniform2f( program, location, v0, v1 );
    }

    // ========================================================================

    public void ProgramUniform2fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform2fv( program, location, count, value );
    }

    public void ProgramUniform2fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniform2fv( program, location, value.Length / 2, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform2d( GLuint program, GLint location, GLdouble v0, GLdouble v1 )
    {
        _glProgramUniform2d( program, location, v0, v1 );
    }

    // ========================================================================

    public void ProgramUniform2dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform2dv( program, location, count, value );
    }

    public void ProgramUniform2dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniform2dv( program, location, value.Length / 2, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform2ui( GLuint program, GLint location, GLuint v0, GLuint v1 )
    {
        _glProgramUniform2ui( program, location, v0, v1 );
    }

    // ========================================================================

    public void ProgramUniform2uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform2uiv( program, location, count, value );
    }

    public void ProgramUniform2uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform2uiv( program, location, value.Length / 2, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform3i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2 )
    {
        _glProgramUniform3i( program, location, v0, v1, v2 );
    }

    // ========================================================================

    public void ProgramUniform3iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform3iv( program, location, count, value );
    }

    public void ProgramUniform3iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue = &value[ 0 ] )
        {
            _glProgramUniform3iv( program, location, value.Length / 3, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform3f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )
    {
        _glProgramUniform3f( program, location, v0, v1, v2 );
    }

    // ========================================================================

    public void ProgramUniform3fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform3fv( program, location, count, value );
    }

    public void ProgramUniform3fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniform3fv( program, location, value.Length / 3, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform3d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2 )
    {
        _glProgramUniform3d( program, location, v0, v1, v2 );
    }

    // ========================================================================

    public void ProgramUniform3dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform3dv( program, location, count, value );
    }

    public void ProgramUniform3dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniform3dv( program, location, value.Length / 3, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform3ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2 )
    {
        _glProgramUniform3ui( program, location, v0, v1, v2 );
    }

    // ========================================================================

    public void ProgramUniform3uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform3uiv( program, location, count, value );
    }

    public void ProgramUniform3uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue = &value[ 0 ] )
        {
            _glProgramUniform3uiv( program, location, value.Length / 3, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform4i( GLuint program, GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )
    {
        _glProgramUniform4i( program, location, v0, v1, v2, v3 );
    }

    // ========================================================================

    public void ProgramUniform4iv( GLuint program, GLint location, GLsizei count, GLint* value )
    {
        _glProgramUniform4iv( program, location, count, value );
    }

    public void ProgramUniform4iv( GLuint program, GLint location, GLint[] value )
    {
        fixed ( GLint* pValue = &value[ 0 ] )
        {
            _glProgramUniform4iv( program, location, value.Length / 4, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform4f( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )
    {
        _glProgramUniform4f( program, location, v0, v1, v2, v3 );
    }

    // ========================================================================

    public void ProgramUniform4fv( GLuint program, GLint location, GLsizei count, GLfloat* value )
    {
        _glProgramUniform4fv( program, location, count, value );
    }

    public void ProgramUniform4fv( GLuint program, GLint location, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniform4fv( program, location, value.Length / 4, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform4d( GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2, GLdouble v3 )
    {
        _glProgramUniform4d( program, location, v0, v1, v2, v3 );
    }

    // ========================================================================

    public void ProgramUniform4dv( GLuint program, GLint location, GLsizei count, GLdouble* value )
    {
        _glProgramUniform4dv( program, location, count, value );
    }

    // ========================================================================

    public void ProgramUniform4dv( GLuint program, GLint location, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniform4dv( program, location, value.Length / 4, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniform4ui( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 )
    {
        _glProgramUniform4ui( program, location, v0, v1, v2, v3 );
    }

    // ========================================================================

    public void ProgramUniform4uiv( GLuint program, GLint location, GLsizei count, GLuint* value )
    {
        _glProgramUniform4uiv( program, location, count, value );
    }

    public void ProgramUniform4uiv( GLuint program, GLint location, GLuint[] value )
    {
        fixed ( GLuint* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniform4uiv( program, location, value.Length / 4, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2fv( program, location, value.Length / 4, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3fv( program, location, value.Length / 9, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix4fv( program, location, value.Length / 16, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix2dv( program, location, value.Length / 4, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue =
                   &value[ 0 ] )
        {
            _glProgramUniformMatrix3dv( program, location, value.Length / 9, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4dv( program, location, value.Length / 16, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix2x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x3fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix2x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix3x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x2fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix3x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2fv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix2x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix2x4fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix2x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix4x2fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x2fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix4x2fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2fv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix3x4fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix3x4fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix3x4fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix4x3fv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value )
    {
        _glProgramUniformMatrix4x3fv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix4x3fv( GLuint program, GLint location, GLboolean transpose, GLfloat[] value )
    {
        fixed ( GLfloat* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3fv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix2x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x3dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix2x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x3dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix3x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x2dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix3x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x2dv( program, location, value.Length / 6, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix2x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix2x4dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix2x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix2x4dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix4x2dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x2dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix4x2dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x2dv( program, location, value.Length / 8, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix3x4dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix3x4dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix3x4dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix3x4dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    // ========================================================================

    public void ProgramUniformMatrix4x3dv( GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value )
    {
        _glProgramUniformMatrix4x3dv( program, location, count, transpose, value );
    }

    public void ProgramUniformMatrix4x3dv( GLuint program, GLint location, GLboolean transpose, GLdouble[] value )
    {
        fixed ( GLdouble* pValue = &value[ 0 ] )
        {
            _glProgramUniformMatrix4x3dv( program, location, value.Length / 12, transpose, pValue );
        }
    }

    // ========================================================================

    public void ValidateProgramPipeline( GLuint pipeline )
    {
        _glValidateProgramPipeline( pipeline );
    }

    // ========================================================================

    public void GetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize, GLsizei* length, GLchar* infoLog )
    {
        _glGetProgramPipelineInfoLog( pipeline, bufSize, length, infoLog );
    }

    public string GetProgramPipelineInfoLog( GLuint pipeline, GLsizei bufSize )
    {
        var infoLog = new GLchar[ bufSize ];

        fixed ( GLchar* pInfoLog = &infoLog[ 0 ] )
        {
            GLsizei length;

            _glGetProgramPipelineInfoLog( pipeline, bufSize, &length, pInfoLog );

            return new string( ( sbyte* )pInfoLog, 0, length, Encoding.UTF8 );
        }
    }

    // ========================================================================

    public void VertexAttribL1d( GLuint index, GLdouble x )
    {
        _glVertexAttribL1d( index, x );
    }

    // ========================================================================

    public void VertexAttribL2d( GLuint index, GLdouble x, GLdouble y )
    {
        _glVertexAttribL2d( index, x, y );
    }

    // ========================================================================

    public void VertexAttribL3d( GLuint index, GLdouble x, GLdouble y, GLdouble z )
    {
        _glVertexAttribL3d( index, x, y, z );
    }

    // ========================================================================

    public void VertexAttribL4d( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w )
    {
        _glVertexAttribL4d( index, x, y, z, w );
    }

    // ========================================================================

    public void VertexAttribL1dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL1dv( index, v );
    }

    public void VertexAttribL1dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL1dv( index, pV );
        }
    }

    // ========================================================================

    public void VertexAttribL2dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL2dv( index, v );
    }

    public void VertexAttribL2dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL2dv( index, pV );
        }
    }

    // ========================================================================

    public void VertexAttribL3dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL3dv( index, v );
    }

    public void VertexAttribL3dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL3dv( index, pV );
        }
    }

    // ========================================================================

    public void VertexAttribL4dv( GLuint index, GLdouble* v )
    {
        _glVertexAttribL4dv( index, v );
    }

    public void VertexAttribL4dv( GLuint index, GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glVertexAttribL4dv( index, pV );
        }
    }

    // ========================================================================

    public void VertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, pointer );
    }

    public void VertexAttribLPointer( GLuint index, GLint size, GLenum type, GLsizei stride, GLsizei pointer )
    {
        _glVertexAttribLPointer( index, size, type, stride, ( void* )pointer );
    }

    // ========================================================================

    public void GetVertexAttribLdv( GLuint index, GLenum pname, GLdouble* parameters )
    {
        _glGetVertexAttribLdv( index, pname, parameters );
    }

    public void GetVertexAttribLdv( GLuint index, GLenum pname, ref GLdouble[] parameters )
    {
        fixed ( GLdouble* pP = &parameters[ 0 ] )
        {
            _glGetVertexAttribLdv( index, pname, pP );
        }
    }

    // ========================================================================

    public void ViewportArrayv( GLuint first, GLsizei count, GLfloat* v )
    {
        _glViewportArrayv( first, count, v );
    }

    public void ViewportArrayv( GLuint first, GLsizei count, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportArrayv( first, count, pV );
        }
    }

    // ========================================================================

    public void ViewportIndexedf( GLuint index, GLfloat x, GLfloat y, GLfloat w, GLfloat h )
    {
        _glViewportIndexedf( index, x, y, w, h );
    }

    // ========================================================================

    public void ViewportIndexedfv( GLuint index, GLfloat* v )
    {
        _glViewportIndexedfv( index, v );
    }

    public void ViewportIndexedfv( GLuint index, params GLfloat[] v )
    {
        fixed ( GLfloat* pV = &v[ 0 ] )
        {
            _glViewportIndexedfv( index, pV );
        }
    }

    // ========================================================================

    public void ScissorArrayv( GLuint first, GLsizei count, GLint* v )
    {
        _glScissorArrayv( first, count, v );
    }

    public void ScissorArrayv( GLuint first, GLsizei count, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorArrayv( first, count, pV );
        }
    }

    // ========================================================================

    public void ScissorIndexed( GLuint index, GLint left, GLint bottom, GLsizei width, GLsizei height )
    {
        _glScissorIndexed( index, left, bottom, width, height );
    }

    // ========================================================================

    public void ScissorIndexedv( GLuint index, GLint* v )
    {
        _glScissorIndexedv( index, v );
    }

    public void ScissorIndexedv( GLuint index, params GLint[] v )
    {
        fixed ( GLint* pV = &v[ 0 ] )
        {
            _glScissorIndexedv( index, pV );
        }
    }

    // ========================================================================

    public void DepthRangeArrayv( GLuint first, GLsizei count, GLdouble* v )
    {
        _glDepthRangeArrayv( first, count, v );
    }

    public void DepthRangeArrayv( GLuint first, GLsizei count, params GLdouble[] v )
    {
        fixed ( GLdouble* pV = &v[ 0 ] )
        {
            _glDepthRangeArrayv( first, count, pV );
        }
    }

    // ========================================================================

    public void DepthRangeIndexed( GLuint index, GLdouble n, GLdouble f )
    {
        _glDepthRangeIndexed( index, n, f );
    }

    // ========================================================================

    public void GetFloati_v( GLenum target, GLuint index, GLfloat* data )
    {
        _glGetFloati_v( target, index, data );
    }

    public void GetFloati_v( GLenum target, GLuint index, ref GLfloat[] data )
    {
        fixed ( GLfloat* pData = &data[ 0 ] )
        {
            _glGetFloati_v( target, index, pData );
        }
    }

    // ========================================================================

    public void GetDoublei_v( GLenum target, GLuint index, GLdouble* data )
    {
        _glGetDoublei_v( target, index, data );
    }

    public void GetDoublei_v( GLenum target, GLuint index, ref GLdouble[] data )
    {
        fixed ( GLdouble* pData = &data[ 0 ] )
        {
            _glGetDoublei_v( target, index, pData );
        }
    }

    // ========================================================================

    public void DrawArraysInstancedBaseInstance( GLenum mode, GLint first, GLsizei count, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawArraysInstancedBaseInstance( mode, first, count, instancecount, baseinstance );
    }

    // ========================================================================

    public void DrawElementsInstancedBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
    }

    public void DrawElementsInstancedBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLuint baseinstance ) where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseInstance( mode, count, type, p, instancecount, baseinstance );
        }
    }

    // ========================================================================

    public void DrawElementsInstancedBaseVertexBaseInstance( GLenum mode, GLsizei count, GLenum type, void* indices,
                                                             GLsizei instancecount, GLint basevertex, GLuint baseinstance )
    {
        _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
    }

    public void DrawElementsInstancedBaseVertexBaseInstance< T >( GLenum mode, GLsizei count, GLenum type, T[] indices,
                                                                  GLsizei instancecount, GLint basevertex, GLuint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseVertexBaseInstance( mode, count, type, p, instancecount, basevertex, baseinstance );
        }
    }

    // ========================================================================

    public void GetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, GLint* parameters )
    {
        _glGetInternalformativ( target, internalformat, pname, bufSize, parameters );
    }

    public void GetInternalformativ( GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetInternalformativ( target, internalformat, pname, bufSize, p );
        }
    }

    // ========================================================================

    public void GetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, GLint* parameters )
    {
        _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, parameters );
    }

    public void GetActiveAtomicCounterBufferiv( GLuint program, GLuint bufferIndex, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetActiveAtomicCounterBufferiv( program, bufferIndex, pname, p );
        }
    }

    // ========================================================================

    public void BindImageTexture( GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum access, GLenum format )
    {
        _glBindImageTexture( unit, texture, level, layered, layer, access, format );
    }

    // ========================================================================

    public void MemoryBarrier( GLbitfield barriers )
    {
        _glMemoryBarrier( barriers );
    }

    // ========================================================================

    public void TexStorage1D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width )
    {
        _glTexStorage1D( target, levels, internalformat, width );
    }

    // ========================================================================

    public void TexStorage2D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glTexStorage2D( target, levels, internalformat, width, height );
    }

    // ========================================================================

    public void TexStorage3D( GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glTexStorage3D( target, levels, internalformat, width, height, depth );
    }

    // ========================================================================

    public void DrawTransformFeedbackInstanced( GLenum mode, GLuint id, GLsizei instancecount )
    {
        _glDrawTransformFeedbackInstanced( mode, id, instancecount );
    }

    // ========================================================================

    public void DrawTransformFeedbackStreamInstanced( GLenum mode, GLuint id, GLuint stream, GLsizei instancecount )
    {
        _glDrawTransformFeedbackStreamInstanced( mode, id, stream, instancecount );
    }

    // ========================================================================

    public void GetPointerv( GLenum pname, void** parameters )
    {
        _glGetPointerv( pname, parameters );
    }

    public void GetPointerv( GLenum pname, ref IntPtr[] parameters )
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

    public void ClearBufferData( GLenum target, GLenum internalformat, GLenum format, GLenum type, void* data )
    {
        _glClearBufferData( target, internalformat, format, type, data );
    }

    public void ClearBufferData< T >( GLenum target, GLenum internalformat, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferData( target, internalformat, format, type, t );
        }
    }

    // ========================================================================

    public void ClearBufferSubData( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data )
    {
        _glClearBufferSubData( target, internalformat, offset, size, format, type, data );
    }

    public void ClearBufferSubData< T >( GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearBufferSubData( target, internalformat, offset, size, format, type, t );
        }
    }

    // ========================================================================

    public void DispatchCompute( GLuint numGroupsX, GLuint numGroupsY, GLuint numGroupsZ )
    {
        _glDispatchCompute( numGroupsX, numGroupsY, numGroupsZ );
    }

    // ========================================================================

    public struct DispatchIndirectCommand
    {
        public uint NumGroupsX;
        public uint NumGroupsY;
        public uint NumGroupsZ;
    }

    public void DispatchComputeIndirect( void* indirect )
    {
        _glDispatchComputeIndirect( indirect );
    }

    public void DispatchComputeIndirect( DispatchIndirectCommand indirect )
    {
        _glDispatchComputeIndirect( &indirect );
    }

    // ========================================================================

    public void CopyImageSubData( GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ,
                                  GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY,
                                  GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth )
    {
        _glCopyImageSubData( srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth );
    }

    // ========================================================================

    public void FramebufferParameteri( GLenum target, GLenum pname, GLint param )
    {
        _glFramebufferParameteri( target, pname, param );
    }

    // ========================================================================

    public void GetFramebufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetFramebufferParameteriv( target, pname, parameters );
    }

    public void GetFramebufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pParameters = &parameters[ 0 ] )
        {
            _glGetFramebufferParameteriv( target, pname, pParameters );
        }
    }

    // ========================================================================

    public void GetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, GLint64* parameters )
    {
        _glGetInternalformati64v( target, internalformat, pname, count, parameters );
    }

    public void GetInternalformati64v( GLenum target, GLenum internalformat, GLenum pname, GLsizei count, ref GLint64[] parameters )
    {
        fixed ( GLint64* pParams = &parameters[ 0 ] )
        {
            _glGetInternalformati64v( target, internalformat, pname, count, pParams );
        }
    }

    // ========================================================================

    public void InvalidateTexSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glInvalidateTexSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth );
    }

    // ========================================================================

    public void InvalidateTexImage( GLuint texture, GLint level )
    {
        _glInvalidateTexImage( texture, level );
    }

    // ========================================================================

    public void InvalidateBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr length )
    {
        _glInvalidateBufferSubData( buffer, offset, length );
    }

    // ========================================================================

    public void InvalidateBufferData( GLuint buffer )
    {
        _glInvalidateBufferData( buffer );
    }

    // ========================================================================

    public void InvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments )
    {
        _glInvalidateFramebuffer( target, numAttachments, attachments );
    }

    public void InvalidateFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments )
    {
        fixed ( GLenum* pAttachments = &attachments[ 0 ] )
        {
            _glInvalidateFramebuffer( target, numAttachments, pAttachments );
        }
    }

    // ========================================================================

    public void InvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glInvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
    }

    public void InvalidateSubFramebuffer( GLenum target, GLsizei numAttachments, GLenum[] attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        fixed ( GLenum* pAttachments = &attachments[ 0 ] )
        {
            _glInvalidateSubFramebuffer( target, numAttachments, pAttachments, x, y, width, height );
        }
    }

    // ========================================================================

    public void MultiDrawArraysIndirect( GLenum mode, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, indirect, drawcount, stride );
    }

    public void MultiDrawArraysIndirect( GLenum mode, DrawArraysIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirect( mode, ( void* )&indirect, drawcount, stride );
    }

    // ========================================================================

    public void MultiDrawElementsIndirect( GLenum mode, GLenum type, void* indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
    }

    public void MultiDrawElementsIndirect( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect, GLsizei drawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirect( mode, type, ( void* )&indirect, drawcount, stride );
    }

    // ========================================================================

    public void GetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, GLint* parameters )
    {
        _glGetProgramInterfaceiv( program, programInterface, pname, parameters );
    }

    public void GetProgramInterfaceiv( GLuint program, GLenum programInterface, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* pParameters = &parameters[ 0 ] )
        {
            _glGetProgramInterfaceiv( program, programInterface, pname, pParameters );
        }
    }

    // ========================================================================

    public GLuint GetProgramResourceIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceIndex( program, programInterface, name );
    }

    public GLuint GetProgramResourceIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* pName = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceIndex( program, programInterface, pName );
        }
    }

    // ========================================================================

    public void GetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei* length, GLchar* name )
    {
        _glGetProgramResourceName( program, programInterface, index, bufSize, length, name );
    }

    public string GetProgramResourceName( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize )
    {
        var name = new GLchar[ bufSize ];

        fixed ( GLchar* pName = &name[ 0 ] )
        {
            GLsizei length;

            _glGetProgramResourceName( program, programInterface, index, bufSize, &length, pName );

            return new string( ( sbyte* )pName, 0, length, Encoding.UTF8 );
        }
    }

    // ========================================================================

    public void GetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLsizei propCount,
                                      GLenum* props, GLsizei bufSize, GLsizei* length, GLint* parameters )
    {
        _glGetProgramResourceiv( program, programInterface, index, propCount, props, bufSize, length, parameters );
    }

    public void GetProgramResourceiv( GLuint program, GLenum programInterface, GLuint index, GLenum[] props, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( GLenum* pProps = &props[ 0 ] )
        fixed ( GLint* pParams = &parameters[ 0 ] )
        {
            GLsizei length;
            _glGetProgramResourceiv( program, programInterface, index, props.Length, pProps, bufSize, &length, pParams );
        }
    }

    // ========================================================================

    public GLint GetProgramResourceLocation( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocation( program, programInterface, name );
    }

    public GLint GetProgramResourceLocation( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* pName = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocation( program, programInterface, pName );
        }
    }

    // ========================================================================

    public GLint GetProgramResourceLocationIndex( GLuint program, GLenum programInterface, GLchar* name )
    {
        return _glGetProgramResourceLocationIndex( program, programInterface, name );
    }

    public GLint GetProgramResourceLocationIndex( GLuint program, GLenum programInterface, string name )
    {
        var nameBytes = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* pName = &nameBytes[ 0 ] )
        {
            return _glGetProgramResourceLocationIndex( program, programInterface, pName );
        }
    }

    // ========================================================================

    public void ShaderStorageBlockBinding( GLuint program, GLuint storageBlockIndex, GLuint storageBlockBinding )
    {
        _glShaderStorageBlockBinding( program, storageBlockIndex, storageBlockBinding );
    }

    // ========================================================================

    public void TexBufferRange( GLenum target, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTexBufferRange( target, internalformat, buffer, offset, size );
    }

    // ========================================================================

    public void TexStorage2DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations )
    {
        _glTexStorage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
    }

    // ========================================================================

    public void TexStorage3DMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width,
                                         GLsizei height, GLsizei depth, GLboolean fixedsamplelocations )
    {
        _glTexStorage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

    // ========================================================================

    public void TextureView( GLuint texture,
                             GLenum target,
                             GLuint origtexture,
                             GLenum internalformat, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers )
    {
        _glTextureView( texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers );
    }

    // ========================================================================

    public void BindVertexBuffer( GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )
    {
        _glBindVertexBuffer( bindingindex, buffer, offset, stride );
    }

    // ========================================================================

    public void VertexAttribFormat( GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset )
    {
        _glVertexAttribFormat( attribindex, size, type, normalized, relativeoffset );
    }

    // ========================================================================

    public void VertexAttribIFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribIFormat( attribindex, size, type, relativeoffset );
    }

    // ========================================================================

    public void VertexAttribLFormat( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexAttribLFormat( attribindex, size, type, relativeoffset );
    }

    // ========================================================================

    public void VertexAttribBinding( GLuint attribindex, GLuint bindingindex )
    {
        _glVertexAttribBinding( attribindex, bindingindex );
    }

    // ========================================================================

    public void VertexBindingDivisor( GLuint bindingindex, GLuint divisor )
    {
        _glVertexBindingDivisor( bindingindex, divisor );
    }

    // ========================================================================

    public void DebugMessageControl( GLenum source, GLenum type, GLenum severity, GLsizei count, GLuint* ids, GLboolean enabled )
    {
        _glDebugMessageControl( source, type, severity, count, ids, enabled );
    }

    public void DebugMessageControl( GLenum source, GLenum type, GLenum severity, GLuint[] ids, GLboolean enabled )
    {
        fixed ( GLuint* pIds = ids )
        {
            _glDebugMessageControl( source, type, severity, ids.Length, pIds, enabled );
        }
    }

    // ========================================================================

    public void DebugMessageInsert( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* buf )
    {
        _glDebugMessageInsert( source, type, id, severity, length, buf );
    }

    public void DebugMessageInsert( GLenum source, GLenum type, GLuint id, GLenum severity, string buf )
    {
        var bufBytes = Encoding.UTF8.GetBytes( buf );

        fixed ( GLchar* pBufBytes = bufBytes )
        {
            _glDebugMessageInsert( source, type, id, severity, bufBytes.Length, pBufBytes );
        }
    }

    // ========================================================================

    public void DebugMessageCallback( GLDEBUGPROC callback, void* userParam )
    {
        _glDebugMessageCallback( callback, userParam );
    }

    public void DebugMessageCallback( GLDEBUGPROCSAFE callback, void* userParam )
    {
        _glDebugMessageCallback( CallbackUnsafe, userParam );

        return;

        void CallbackUnsafe( GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* message, void* param )
        {
            var messageString = new string( ( sbyte* )message, 0, length, Encoding.UTF8 );
            callback( source, type, id, severity, messageString, param );
        }
    }

    // ========================================================================

    public GLuint GetDebugMessageLog( GLuint count,
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

    public GLuint GetDebugMessageLog( GLuint count,
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

        fixed ( GLenum* pSources = &sources[ 0 ] )
        fixed ( GLenum* pTypes = &types[ 0 ] )
        fixed ( GLuint* pIds = &ids[ 0 ] )
        fixed ( GLenum* pSeverities = &severities[ 0 ] )
        fixed ( GLsizei* pLengths = &lengths[ 0 ] )
        fixed ( GLchar* pMessageLogBytes = &messageLogBytes[ 0 ] )
        {
            var pstart = pMessageLogBytes;
            var ret    = _glGetDebugMessageLog( count, bufSize, pSources, pTypes, pIds, pSeverities, pLengths, pMessageLogBytes );

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

    // ========================================================================

    public void PushDebugGroup( GLenum source, GLuint id, GLsizei length, GLchar* message )
    {
        _glPushDebugGroup( source, id, length, message );
    }

    public void PushDebugGroup( GLenum source, GLuint id, string message )
    {
        var messageBytes = Encoding.UTF8.GetBytes( message );

        fixed ( GLchar* pMessageBytes = messageBytes )
        {
            _glPushDebugGroup( source, id, messageBytes.Length, pMessageBytes );
        }
    }

    // ========================================================================

    public void PopDebugGroup()
    {
        _glPopDebugGroup();
    }

    // ========================================================================

    public void ObjectLabel( GLenum identifier, GLuint name, GLsizei length, GLchar* label )
    {
        _glObjectLabel( identifier, name, length, label );
    }

    public void ObjectLabel( GLenum identifier, GLuint name, string label )
    {
        var labelBytes = Encoding.UTF8.GetBytes( label );

        fixed ( GLchar* pLabelBytes = labelBytes )
        {
            _glObjectLabel( identifier, name, labelBytes.Length, pLabelBytes );
        }
    }

    // ========================================================================

    public void GetObjectLabel( GLenum identifier, GLuint name, GLsizei bufSize, GLsizei* length, GLchar* label )
    {
        _glGetObjectLabel( identifier, name, bufSize, length, label );
    }

    public string GetObjectLabel( GLenum identifier, GLuint name, GLsizei bufSize )
    {
        var labelBytes = new GLchar[ bufSize ];

        fixed ( GLchar* pLabelBytes = labelBytes )
        {
            GLsizei length;

            _glGetObjectLabel( identifier, name, bufSize, &length, pLabelBytes );

            return new string( ( sbyte* )pLabelBytes, 0, length, Encoding.UTF8 );
        }
    }

    // ========================================================================

    public void ObjectPtrLabel( void* ptr, GLsizei length, GLchar* label )
    {
        _glObjectPtrLabel( ptr, length, label );
    }

    public void ObjectPtrLabel( IntPtr ptr, string label )
    {
        var labelBytes = Encoding.UTF8.GetBytes( label );

        fixed ( GLchar* pLabelBytes = labelBytes )
        {
            _glObjectPtrLabel( ptr.ToPointer(), labelBytes.Length, pLabelBytes );
        }
    }

    // ========================================================================

    public void GetObjectPtrLabel( void* ptr, GLsizei bufSize, GLsizei* length, GLchar* label )
    {
        _glGetObjectPtrLabel( ptr, bufSize, length, label );
    }

    public string GetObjectPtrLabel( IntPtr ptr, GLsizei bufSize )
    {
        var labelBytes = new GLchar[ bufSize ];

        fixed ( GLchar* pLabelBytes = labelBytes )
        {
            GLsizei length;

            _glGetObjectPtrLabel( ptr.ToPointer(), bufSize, &length, pLabelBytes );

            return new string( ( sbyte* )pLabelBytes, 0, length, Encoding.UTF8 );
        }
    }

    // ========================================================================

    public void BufferStorage( GLenum target, GLsizeiptr size, void* data, GLbitfield flags )
    {
        _glBufferStorage( target, size, data, flags );
    }

    public void BufferStorage< T >( GLenum target, T[] data, GLbitfield flags ) where T : unmanaged
    {
        fixed ( void* pData = &data[ 0 ] )
        {
            _glBufferStorage( target, data.Length * sizeof( T ), pData, flags );
        }
    }

    // ========================================================================

    public void ClearTexImage( GLuint texture, GLint level, GLenum format, GLenum type, void* data )
    {
        _glClearTexImage( texture, level, format, type, data );
    }

    public void ClearTexImage< T >( GLuint texture, GLint level, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearTexImage( texture, level, format, type, t );
        }
    }

    // ========================================================================

    public void ClearTexSubImage( GLuint texture, GLint level, GLint xOffset, GLint yOffset, GLint zOffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* data )
    {
        _glClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data );
    }

    public void ClearTexSubImage< T >( GLuint texture, GLint level, GLint xOffset, GLint yOffset, GLint zOffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, T[] data )
        where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, t );
        }
    }

    // ========================================================================

    public void BindBuffersBase( GLenum target, GLuint first, GLsizei count, GLuint* buffers )
    {
        _glBindBuffersBase( target, first, count, buffers );
    }

    public void BindBuffersBase( GLenum target, GLuint first, GLuint[] buffers )
    {
        fixed ( GLuint* pBuffers = &buffers[ 0 ] )
        {
            _glBindBuffersBase( target, first, buffers.Length, pBuffers );
        }
    }

    // ========================================================================

    public void BindBuffersRange( GLenum target, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizeiptr* sizes )
    {
        _glBindBuffersRange( target, first, count, buffers, offsets, sizes );
    }

    public void BindBuffersRange( GLenum target, GLuint first, GLuint[] buffers, GLintptr[] offsets, GLsizeiptr[] sizes )
    {
        fixed ( GLuint* pBuffers = &buffers[ 0 ] )
        {
            fixed ( GLintptr* pOffsets = &offsets[ 0 ] )
            {
                fixed ( GLsizeiptr* pSizes = &sizes[ 0 ] )
                {
                    _glBindBuffersRange( target, first, buffers.Length, pBuffers, pOffsets, pSizes );
                }
            }
        }
    }

    // ========================================================================

    public void BindTextures( GLuint first, GLsizei count, GLuint* textures )
    {
        _glBindTextures( first, count, textures );
    }

    public void BindTextures( GLuint first, GLuint[] textures )
    {
        fixed ( GLuint* pTextures = &textures[ 0 ] )
        {
            _glBindTextures( first, textures.Length, pTextures );
        }
    }

    // ========================================================================

    public void BindSamplers( GLuint first, GLsizei count, GLuint* samplers )
    {
        _glBindSamplers( first, count, samplers );
    }

    public void BindSamplers( GLuint first, GLuint[] samplers )
    {
        fixed ( GLuint* pSamplers = &samplers[ 0 ] )
        {
            _glBindSamplers( first, samplers.Length, pSamplers );
        }
    }

    // ========================================================================

    public void BindImageTextures( GLuint first, GLsizei count, GLuint* textures )
    {
        _glBindImageTextures( first, count, textures );
    }

    public void BindImageTextures( GLuint first, GLuint[] textures )
    {
        fixed ( GLuint* pTextures = &textures[ 0 ] )
        {
            _glBindImageTextures( first, textures.Length, pTextures );
        }
    }

    // ========================================================================

    public void BindVertexBuffers( GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides )
    {
        _glBindVertexBuffers( first, count, buffers, offsets, strides );
    }

    public void BindVertexBuffers( GLuint first, GLuint[] buffers, GLintptr[] offsets, GLsizei[] strides )
    {
        fixed ( GLuint* pBuffers = &buffers[ 0 ] )
        {
            fixed ( GLintptr* pOffsets = &offsets[ 0 ] )
            {
                fixed ( GLsizei* pStrides = &strides[ 0 ] )
                {
                    _glBindVertexBuffers( first, buffers.Length, pBuffers, pOffsets, pStrides );
                }
            }
        }
    }

    // ========================================================================

    public void ClipControl( GLenum origin, GLenum depth )
    {
        _glClipControl( origin, depth );
    }

    // ========================================================================

    public void CreateTransformFeedbacks( GLsizei n, GLuint* ids )
    {
        _glCreateTransformFeedbacks( n, ids );
    }

    public GLuint[] CreateTransformFeedbacks( GLsizei n )
    {
        var ids = new GLuint[ n ];

        fixed ( GLuint* pIds = &ids[ 0 ] )
        {
            _glCreateTransformFeedbacks( n, pIds );
        }

        return ids;
    }

    public GLuint CreateTransformFeedbacks()
    {
        return CreateTransformFeedbacks( 1 )[ 0 ];
    }

    // ========================================================================

    public void TransformFeedbackBufferBase( GLuint xfb, GLuint index, GLuint buffer )
    {
        _glTransformFeedbackBufferBase( xfb, index, buffer );
    }

    // ========================================================================

    public void TransformFeedbackBufferRange( GLuint xfb, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTransformFeedbackBufferRange( xfb, index, buffer, offset, size );
    }

    // ========================================================================

    public void GetTransformFeedbackiv( GLuint xfb, GLenum pname, GLint* param )
    {
        _glGetTransformFeedbackiv( xfb, pname, param );
    }

    public void GetTransformFeedbackiv( GLuint xfb, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* pParam = &param[ 0 ] )
        {
            _glGetTransformFeedbackiv( xfb, pname, pParam );
        }
    }

    // ========================================================================

    public void GetTransformFeedbacki_v( GLuint xfb, GLenum pname, GLuint index, GLint* param )
    {
        _glGetTransformFeedbacki_v( xfb, pname, index, param );
    }

    public void GetTransformFeedbacki_v( GLuint xfb, GLenum pname, GLuint index, ref GLint[] param )
    {
        fixed ( GLint* pParam = &param[ 0 ] )
        {
            _glGetTransformFeedbacki_v( xfb, pname, index, pParam );
        }
    }

    // ========================================================================

    public void GetTransformFeedbacki64_v( GLuint xfb, GLenum pname, GLuint index, GLint64* param )
    {
        _glGetTransformFeedbacki64_v( xfb, pname, index, param );
    }

    public void GetTransformFeedbacki64_v( GLuint xfb, GLenum pname, GLuint index, ref GLint64[] param )
    {
        fixed ( GLint64* pParam = &param[ 0 ] )
        {
            _glGetTransformFeedbacki64_v( xfb, pname, index, pParam );
        }
    }

    // ========================================================================

    public void CreateBuffers( GLsizei n, GLuint* buffers )
    {
        _glCreateBuffers( n, buffers );
    }

    public GLuint[] CreateBuffers( GLsizei n )
    {
        var buffers = new GLuint[ n ];

        fixed ( GLuint* pBuffers = &buffers[ 0 ] )
        {
            _glCreateBuffers( n, pBuffers );
        }

        return buffers;
    }

    public GLuint CreateBuffer()
    {
        return CreateBuffers( 1 )[ 0 ];
    }

    // ========================================================================

    public void NamedBufferStorage( GLuint buffer, GLsizeiptr size, void* data, GLbitfield flags )
    {
        _glNamedBufferStorage( buffer, size, data, flags );
    }

    public void NamedBufferStorage< T >( GLuint buffer, GLsizeiptr size, T[] data, GLbitfield flags ) where T : unmanaged
    {
        fixed ( T* pData = &data[ 0 ] )
        {
            _glNamedBufferStorage( buffer, size, pData, flags );
        }
    }

    // ========================================================================

    public void NamedBufferData( GLuint buffer, GLsizeiptr size, void* data, GLenum usage )
    {
        _glNamedBufferData( buffer, size, data, usage );
    }

    public void NamedBufferData< T >( GLuint buffer, GLsizeiptr size, T[] data, GLenum usage ) where T : unmanaged
    {
        fixed ( T* pData = &data[ 0 ] )
        {
            _glNamedBufferData( buffer, size, pData, usage );
        }
    }

    // ========================================================================

    public void NamedBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glNamedBufferSubData( buffer, offset, size, data );
    }

    public void NamedBufferSubData< T >( GLuint buffer, GLintptr offset, GLsizeiptr size, T[] data ) where T : unmanaged
    {
        fixed ( T* pData = &data[ 0 ] )
        {
            _glNamedBufferSubData( buffer, offset, size, pData );
        }
    }

    // ========================================================================

    public void CopyNamedBufferSubData( GLuint readBuffer, GLuint writeBuffer, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size )
    {
        _glCopyNamedBufferSubData( readBuffer, writeBuffer, readOffset, writeOffset, size );
    }

    // ========================================================================

    public void ClearNamedBufferData( GLuint buffer, GLenum internalformat, GLenum format, GLenum type, void* data )
    {
        _glClearNamedBufferData( buffer, internalformat, format, type, data );
    }

    public void ClearNamedBufferData< T >( GLuint buffer, GLenum internalformat, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearNamedBufferData( buffer, internalformat, format, type, t );
        }
    }

    // ========================================================================

    public void ClearNamedBufferSubData( GLuint buffer, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data )
    {
        _glClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, data );
    }

    public void ClearNamedBufferSubData< T >( GLuint buffer, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, T[] data ) where T : unmanaged
    {
        fixed ( T* t = &data[ 0 ] )
        {
            _glClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, t );
        }
    }

    // ========================================================================

    public void* MapNamedBuffer( GLuint buffer, GLenum access )
    {
        return _glMapNamedBuffer( buffer, access );
    }

    public System.Span< T > MapNamedBuffer< T >( GLuint buffer, GLenum access ) where T : unmanaged
    {
        var size = stackalloc GLint[ 1 ];
        _glGetNamedBufferParameteriv( buffer, IGL.GL_BUFFER_SIZE, size );
        void* ptr = _glMapNamedBuffer( buffer, access );

        return new System.Span< T >( ptr, *size / sizeof( T ) );
    }

    // ========================================================================

    public void* MapNamedBufferRange( GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access )
    {
        return _glMapNamedBufferRange( buffer, offset, length, access );
    }

    public System.Span< T > MapNamedBufferRange< T >( GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access ) where T : unmanaged
    {
        var ptr = _glMapNamedBufferRange( buffer, offset, length, access );

        return new System.Span< T >( ptr, ( int )length / sizeof( T ) );
    }

    // ========================================================================

    public GLboolean UnmapNamedBuffer( GLuint buffer )
    {
        return _glUnmapNamedBuffer( buffer );
    }

    // ========================================================================

    public void FlushMappedNamedBufferRange( GLuint buffer, GLintptr offset, GLsizeiptr length )
    {
        _glFlushMappedNamedBufferRange( buffer, offset, length );
    }

    // ========================================================================

    public void GetNamedBufferParameteriv( GLuint buffer, GLenum pname, GLint* parameters )
    {
        _glGetNamedBufferParameteriv( buffer, pname, parameters );
    }

    public void GetNamedBufferParameteriv( GLuint buffer, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* ptrParameters = &parameters[ 0 ] )
        {
            _glGetNamedBufferParameteriv( buffer, pname, ptrParameters );
        }
    }

    // ========================================================================

    public void GetNamedBufferParameteri64v( GLuint buffer, GLenum pname, GLint64* parameters )
    {
        _glGetNamedBufferParameteri64v( buffer, pname, parameters );
    }

    public void GetNamedBufferParameteri64v( GLuint buffer, GLenum pname, ref GLint64[] parameters )
    {
        fixed ( GLint64* ptrParameters = &parameters[ 0 ] )
        {
            _glGetNamedBufferParameteri64v( buffer, pname, ptrParameters );
        }
    }

    // ========================================================================

    public void GetNamedBufferPointerv( GLuint buffer, GLenum pname, void** parameters )
    {
        _glGetNamedBufferPointerv( buffer, pname, parameters );
    }

    public void GetNamedBufferPointerv( GLuint buffer, GLenum pname, ref IntPtr[] parameters )
    {
        var ptrParameters = new void*[ parameters.Length ];

        for ( var i = 0; i < parameters.Length; i++ )
        {
            ptrParameters[ i ] = ( void* )parameters[ i ];
        }

        fixed ( void** ptr = &ptrParameters[ 0 ] )
        {
            _glGetNamedBufferPointerv( buffer, pname, ptr );
        }

        for ( var i = 0; i < parameters.Length; i++ )
        {
            parameters[ i ] = ( IntPtr )ptrParameters[ i ];
        }
    }

    // ========================================================================

    public void GetNamedBufferSubData( GLuint buffer, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glGetNamedBufferSubData( buffer, offset, size, data );
    }

    public T[] GetNamedBufferSubData< T >( GLuint buffer, GLintptr offset, GLsizeiptr size ) where T : unmanaged
    {
        var data = new T[ size / sizeof( T ) ];

        fixed ( T* ptrData = &data[ 0 ] )
        {
            _glGetNamedBufferSubData( buffer, offset, size, ptrData );
        }

        return data;
    }

    // ========================================================================

    public void CreateFramebuffers( GLsizei n, GLuint* framebuffers )
    {
        _glCreateFramebuffers( n, framebuffers );
    }

    public GLuint[] CreateFramebuffers( GLsizei n )
    {
        var framebuffers = new GLuint[ n ];

        fixed ( GLuint* ptrFramebuffers = &framebuffers[ 0 ] )
        {
            _glCreateFramebuffers( n, ptrFramebuffers );
        }

        return framebuffers;
    }

    public GLuint CreateFramebuffer()
    {
        return CreateFramebuffers( 1 )[ 0 ];
    }

    // ========================================================================

    public void NamedFramebufferRenderbuffer( GLuint framebuffer, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer )
    {
        _glNamedFramebufferRenderbuffer( framebuffer, attachment, renderbuffertarget, renderbuffer );
    }

    // ========================================================================

    public void NamedFramebufferParameteri( GLuint framebuffer, GLenum pname, GLint param )
    {
        _glNamedFramebufferParameteri( framebuffer, pname, param );
    }

    // ========================================================================

    public void NamedFramebufferTexture( GLuint framebuffer, GLenum attachment, GLuint texture, GLint level )
    {
        _glNamedFramebufferTexture( framebuffer, attachment, texture, level );
    }

    // ========================================================================

    public void NamedFramebufferTextureLayer( GLuint framebuffer, GLenum attachment, GLuint texture, GLint level, GLint layer )
    {
        _glNamedFramebufferTextureLayer( framebuffer, attachment, texture, level, layer );
    }

    // ========================================================================

    public void NamedFramebufferDrawBuffer( GLuint framebuffer, GLenum buf )
    {
        _glNamedFramebufferDrawBuffer( framebuffer, buf );
    }

    // ========================================================================

    public void NamedFramebufferDrawBuffers( GLuint framebuffer, GLsizei n, GLenum* bufs )
    {
        _glNamedFramebufferDrawBuffers( framebuffer, n, bufs );
    }

    public void NamedFramebufferDrawBuffers( GLuint framebuffer, GLenum[] bufs )
    {
        fixed ( GLenum* ptrBufs = &bufs[ 0 ] )
        {
            _glNamedFramebufferDrawBuffers( framebuffer, bufs.Length, ptrBufs );
        }
    }

    // ========================================================================

    public void NamedFramebufferReadBuffer( GLuint framebuffer, GLenum src )
    {
        _glNamedFramebufferReadBuffer( framebuffer, src );
    }

    // ========================================================================

    public void InvalidateNamedFramebufferData( GLuint framebuffer, GLsizei numAttachments, GLenum* attachments )
    {
        _glInvalidateNamedFramebufferData( framebuffer, numAttachments, attachments );
    }

    public void InvalidateNamedFramebufferData( GLuint framebuffer, GLenum[] attachments )
    {
        fixed ( GLenum* ptrAttachments = &attachments[ 0 ] )
        {
            _glInvalidateNamedFramebufferData( framebuffer, attachments.Length, ptrAttachments );
        }
    }

    // ========================================================================

    public void InvalidateNamedFramebufferSubData( GLuint framebuffer, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glInvalidateNamedFramebufferSubData( framebuffer, numAttachments, attachments, x, y, width, height );
    }

    public void InvalidateNamedFramebufferSubData( GLuint framebuffer, GLenum[] attachments, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        fixed ( GLenum* ptrAttachments = &attachments[ 0 ] )
        {
            _glInvalidateNamedFramebufferSubData( framebuffer, attachments.Length, ptrAttachments, x, y, width, height );
        }
    }

    // ========================================================================

    public void ClearNamedFramebufferiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLint* value )
    {
        _glClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, value );
    }

    public void ClearNamedFramebufferiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLint[] value )
    {
        fixed ( GLint* ptrValue = &value[ 0 ] )
        {
            _glClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, ptrValue );
        }
    }

    // ========================================================================

    public void ClearNamedFramebufferuiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLuint* value )
    {
        _glClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, value );
    }

    public void ClearNamedFramebufferuiv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLuint[] value )
    {
        fixed ( GLuint* ptrValue = &value[ 0 ] )
        {
            _glClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, ptrValue );
        }
    }

    // ========================================================================

    public void ClearNamedFramebufferfv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLfloat* value )
    {
        _glClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, value );
    }

    public void ClearNamedFramebufferfv( GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLfloat[] value )
    {
        fixed ( GLfloat* ptrValue = &value[ 0 ] )
        {
            _glClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, ptrValue );
        }
    }

    // ========================================================================

    public void ClearNamedFramebufferfi( GLuint framebuffer, GLenum buffer, GLfloat depth, GLint stencil )
    {
        _glClearNamedFramebufferfi( framebuffer, buffer, depth, stencil );
    }

    // ========================================================================

    public void BlitNamedFramebuffer( GLuint readFramebuffer, GLuint drawFramebuffer, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1,
                                      GLbitfield mask, GLenum filter )
    {
        _glBlitNamedFramebuffer( readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
    }

    // ========================================================================

    public GLenum CheckNamedFramebufferStatus( GLuint framebuffer, GLenum target )
    {
        return _glCheckNamedFramebufferStatus( framebuffer, target );
    }

    // ========================================================================

    public void GetNamedFramebufferParameteriv( GLuint framebuffer, GLenum pname, GLint* param )
    {
        _glGetNamedFramebufferParameteriv( framebuffer, pname, param );
    }

    public void GetNamedFramebufferParameteriv( GLuint framebuffer, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glGetNamedFramebufferParameteriv( framebuffer, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetNamedFramebufferAttachmentParameteriv( GLuint framebuffer, GLenum attachment, GLenum pname, GLint* parameters )
    {
        _glGetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, parameters );
    }

    public void GetNamedFramebufferAttachmentParameteriv( GLuint framebuffer, GLenum attachment, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* ptrParams = &parameters[ 0 ] )
        {
            _glGetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, ptrParams );
        }
    }

    // ========================================================================

    public void CreateRenderbuffers( GLsizei n, GLuint* renderbuffers )
    {
        _glCreateRenderbuffers( n, renderbuffers );
    }

    public GLuint[] CreateRenderbuffers( GLsizei n )
    {
        var renderbuffers = new GLuint[ n ];

        fixed ( GLuint* ptrRenderbuffers = &renderbuffers[ 0 ] )
        {
            _glCreateRenderbuffers( n, ptrRenderbuffers );
        }

        return renderbuffers;
    }

    public GLuint CreateRenderbuffer()
    {
        return CreateRenderbuffers( 1 )[ 0 ];
    }

    // ========================================================================

    public void NamedRenderbufferStorage( GLuint renderbuffer, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glNamedRenderbufferStorage( renderbuffer, internalformat, width, height );
    }

    // ========================================================================

    public void NamedRenderbufferStorageMultisample( GLuint renderbuffer, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glNamedRenderbufferStorageMultisample( renderbuffer, samples, internalformat, width, height );
    }

    // ========================================================================

    public void GetNamedRenderbufferParameteriv( GLuint renderbuffer, GLenum pname, GLint* param )
    {
        _glGetNamedRenderbufferParameteriv( renderbuffer, pname, param );
    }

    public void GetNamedRenderbufferParameteriv( GLuint renderbuffer, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glGetNamedRenderbufferParameteriv( renderbuffer, pname, ptrParam );
        }
    }

    // ========================================================================

    public void CreateTextures( GLenum target, GLsizei n, GLuint* textures )
    {
        _glCreateTextures( target, n, textures );
    }

    public GLuint[] CreateTextures( GLenum target, GLsizei n )
    {
        var textures = new GLuint[ n ];

        fixed ( GLuint* ptrTextures = &textures[ 0 ] )
        {
            _glCreateTextures( target, n, ptrTextures );
        }

        return textures;
    }

    public GLuint CreateTexture( GLenum target )
    {
        return CreateTextures( target, 1 )[ 0 ];
    }

    // ========================================================================

    public void TextureBuffer( GLuint texture, GLenum internalformat, GLuint buffer )
    {
        _glTextureBuffer( texture, internalformat, buffer );
    }

    // ========================================================================

    public void TextureBufferRange( GLuint texture, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size )
    {
        _glTextureBufferRange( texture, internalformat, buffer, offset, size );
    }

    // ========================================================================

    public void TextureStorage1D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width )
    {
        _glTextureStorage1D( texture, levels, internalformat, width );
    }

    // ========================================================================

    public void TextureStorage2D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glTextureStorage2D( texture, levels, internalformat, width, height );
    }

    // ========================================================================

    public void TextureStorage3D( GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth )
    {
        _glTextureStorage3D( texture, levels, internalformat, width, height, depth );
    }

    // ========================================================================

    public void TextureStorage2DMultisample( GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations )
    {
        _glTextureStorage2DMultisample( texture, samples, internalformat, width, height, fixedsamplelocations );
    }

    // ========================================================================

    public void TextureStorage3DMultisample( GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedsamplelocations )
    {
        _glTextureStorage3DMultisample( texture, samples, internalformat, width, height, depth, fixedsamplelocations );
    }

    // ========================================================================

    public void TextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage1D( texture, level, xoffset, width, format, type, pixels );
    }

    public void TextureSubImage1D< T >( GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, T[] pixels ) where T : unmanaged
    {
        fixed ( T* ptrPixels = &pixels[ 0 ] )
        {
            _glTextureSubImage1D( texture, level, xoffset, width, format, type, ptrPixels );
        }
    }

    // ========================================================================

    public void TextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, pixels );
    }

    public void TextureSubImage2D< T >( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, T[] pixels ) where T : unmanaged
    {
        fixed ( T* ptrPixels = &pixels[ 0 ] )
        {
            _glTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, ptrPixels );
        }
    }

    // ========================================================================

    public void TextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* pixels )
    {
        _glTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
    }

    public void TextureSubImage3D< T >( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type,
                                        T[] pixels ) where T : unmanaged
    {
        fixed ( T* ptrPixels = &pixels[ 0 ] )
        {
            _glTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, ptrPixels );
        }
    }

    // ========================================================================

    public void CompressedTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width,
                                             GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, data );
    }

    public void CompressedTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLsizei width,
                                             GLenum format, GLsizei imageSize, byte[] data )
    {
        fixed ( byte* ptrData = &data[ 0 ] )
        {
            _glCompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, ptrData );
        }
    }

    // ========================================================================

    public void CompressedTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, data );
    }

    public void CompressedTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, byte[] data )
    {
        fixed ( byte* ptrData = &data[ 0 ] )
        {
            _glCompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, ptrData );
        }
    }

    // ========================================================================

    public void CompressedTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize,
                                             void* data )
    {
        _glCompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
    }

    public void CompressedTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize,
                                             byte[] data )
    {
        fixed ( byte* ptrData = &data[ 0 ] )
        {
            _glCompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, ptrData );
        }
    }

    // ========================================================================

    public void CopyTextureSubImage1D( GLuint texture, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width )
    {
        _glCopyTextureSubImage1D( texture, level, xoffset, x, y, width );
    }

    // ========================================================================

    public void CopyTextureSubImage2D( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTextureSubImage2D( texture, level, xoffset, yoffset, x, y, width, height );
    }

    // ========================================================================

    public void CopyTextureSubImage3D( GLuint texture, GLint level, GLint xoffset, GLint yoffset,
                                       GLint zoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, x, y, width, height );
    }

    // ========================================================================

    public void TextureParameterf( GLuint texture, GLenum pname, GLfloat param )
    {
        _glTextureParameterf( texture, pname, param );
    }

    // ========================================================================

    public void TextureParameterfv( GLuint texture, GLenum pname, GLfloat* param )
    {
        _glTextureParameterfv( texture, pname, param );
    }

    public void TextureParameterfv( GLuint texture, GLenum pname, GLfloat[] param )
    {
        fixed ( GLfloat* ptrParam = &param[ 0 ] )
        {
            _glTextureParameterfv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void TextureParameteri( GLuint texture, GLenum pname, GLint param )
    {
        _glTextureParameteri( texture, pname, param );
    }

    // ========================================================================

    public void TextureParameterIiv( GLuint texture, GLenum pname, GLint* param )
    {
        _glTextureParameterIiv( texture, pname, param );
    }

    public void TextureParameterIiv( GLuint texture, GLenum pname, GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glTextureParameterIiv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void TextureParameterIuiv( GLuint texture, GLenum pname, GLuint* param )
    {
        _glTextureParameterIuiv( texture, pname, param );
    }

    public void TextureParameterIuiv( GLuint texture, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* ptrParam = &param[ 0 ] )
        {
            _glTextureParameterIuiv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void TextureParameteriv( GLuint texture, GLenum pname, GLint* param )
    {
        _glTextureParameteriv( texture, pname, param );
    }

    public void TextureParameteriv( GLuint texture, GLenum pname, GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glTextureParameteriv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GenerateTextureMipmap( GLuint texture )
    {
        _glGenerateTextureMipmap( texture );
    }

    // ========================================================================

    public void BindTextureUnit( GLuint unit, GLuint texture )
    {
        _glBindTextureUnit( unit, texture );
    }

    // ========================================================================

    public void GetTextureImage( GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pixels )
    {
        _glGetTextureImage( texture, level, format, type, bufSize, pixels );
    }

    public T[] GetTextureImage< T >( GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize ) where T : unmanaged
    {
        var pixels = new T[ bufSize ];

        fixed ( T* ptrPixels = &pixels[ 0 ] )
        {
            _glGetTextureImage( texture, level, format, type, bufSize, ptrPixels );
        }

        return pixels;
    }

    // ========================================================================

    public void GetCompressedTextureImage( GLuint texture, GLint level, GLsizei bufSize, void* pixels )
    {
        _glGetCompressedTextureImage( texture, level, bufSize, pixels );
    }

    public byte[] GetCompressedTextureImage( GLuint texture, GLint level, GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];

        fixed ( byte* ptrPixels = &pixels[ 0 ] )
        {
            _glGetCompressedTextureImage( texture, level, bufSize, ptrPixels );
        }

        return pixels;
    }

    // ========================================================================

    public void GetTextureLevelParameterfv( GLuint texture, GLint level, GLenum pname, GLfloat* param )
    {
        _glGetTextureLevelParameterfv( texture, level, pname, param );
    }

    public void GetTextureLevelParameterfv( GLuint texture, GLint level, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* ptrParam = &param[ 0 ] )
        {
            _glGetTextureLevelParameterfv( texture, level, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetTextureLevelParameteriv( GLuint texture, GLint level, GLenum pname, GLint* param )
    {
        _glGetTextureLevelParameteriv( texture, level, pname, param );
    }

    public void GetTextureLevelParameteriv( GLuint texture, GLint level, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glGetTextureLevelParameteriv( texture, level, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetTextureParameterfv( GLuint texture, GLenum pname, GLfloat* param )
    {
        _glGetTextureParameterfv( texture, pname, param );
    }

    public void GetTextureParameterfv( GLuint texture, GLenum pname, ref GLfloat[] param )
    {
        fixed ( GLfloat* ptrParam = &param[ 0 ] )
        {
            _glGetTextureParameterfv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetTextureParameterIiv( GLuint texture, GLenum pname, GLint* param )
    {
        _glGetTextureParameterIiv( texture, pname, param );
    }

    public void GetTextureParameterIiv( GLuint texture, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glGetTextureParameterIiv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetTextureParameterIuiv( GLuint texture, GLenum pname, GLuint* param )
    {
        _glGetTextureParameterIuiv( texture, pname, param );
    }

    public void GetTextureParameterIuiv( GLuint texture, GLenum pname, ref GLuint[] param )
    {
        fixed ( GLuint* ptrParam = &param[ 0 ] )
        {
            _glGetTextureParameterIuiv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetTextureParameteriv( GLuint texture, GLenum pname, GLint* param )
    {
        _glGetTextureParameteriv( texture, pname, param );
    }

    public void GetTextureParameteriv( GLuint texture, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glGetTextureParameteriv( texture, pname, ptrParam );
        }
    }

    // ========================================================================

    public void CreateVertexArrays( GLsizei n, GLuint* arrays )
    {
        _glCreateVertexArrays( n, arrays );
    }

    public GLuint[] CreateVertexArrays( GLsizei n )
    {
        var arrays = new GLuint[ n ];

        fixed ( GLuint* ptrArrays = &arrays[ 0 ] )
        {
            _glCreateVertexArrays( n, ptrArrays );
        }

        return arrays;
    }

    public GLuint CreateVertexArray()
    {
        return CreateVertexArrays( 1 )[ 0 ];
    }

    // ========================================================================

    public void DisableVertexArrayAttrib( GLuint vaobj, GLuint index )
    {
        _glDisableVertexArrayAttrib( vaobj, index );
    }

    // ========================================================================

    public void EnableVertexArrayAttrib( GLuint vaobj, GLuint index )
    {
        _glEnableVertexArrayAttrib( vaobj, index );
    }

    // ========================================================================

    public void VertexArrayElementBuffer( GLuint vaobj, GLuint buffer )
    {
        _glVertexArrayElementBuffer( vaobj, buffer );
    }

    // ========================================================================

    public void VertexArrayVertexBuffer( GLuint vaobj, GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )
    {
        _glVertexArrayVertexBuffer( vaobj, bindingindex, buffer, offset, stride );
    }

    public void VertexArrayVertexBuffers( GLuint vaobj, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides )
    {
        _glVertexArrayVertexBuffers( vaobj, first, count, buffers, offsets, strides );
    }

    // ========================================================================

    public void VertexArrayVertexBuffers( GLuint vaobj, GLuint first, GLuint[] buffers, GLintptr[] offsets, GLsizei[] strides )
    {
        fixed ( GLuint* ptrBuffers = &buffers[ 0 ] )
        fixed ( GLintptr* ptrOffsets = &offsets[ 0 ] )
        fixed ( GLsizei* ptrStrides = &strides[ 0 ] )
        {
            _glVertexArrayVertexBuffers( vaobj, first, ( GLsizei )buffers.Length, ptrBuffers, ptrOffsets, ptrStrides );
        }
    }

    // ========================================================================

    public void VertexArrayAttribBinding( GLuint vaobj, GLuint attribindex, GLuint bindingindex )
    {
        _glVertexArrayAttribBinding( vaobj, attribindex, bindingindex );
    }

    // ========================================================================

    public void VertexArrayAttribFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset )
    {
        _glVertexArrayAttribFormat( vaobj, attribindex, size, type, normalized, relativeoffset );
    }

    // ========================================================================

    public void VertexArrayAttribIFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexArrayAttribIFormat( vaobj, attribindex, size, type, relativeoffset );
    }

    // ========================================================================

    public void VertexArrayAttribLFormat( GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )
    {
        _glVertexArrayAttribLFormat( vaobj, attribindex, size, type, relativeoffset );
    }

    // ========================================================================

    public void VertexArrayBindingDivisor( GLuint vaobj, GLuint bindingindex, GLuint divisor )
    {
        _glVertexArrayBindingDivisor( vaobj, bindingindex, divisor );
    }

    // ========================================================================

    public void GetVertexArrayiv( GLuint vaobj, GLenum pname, GLint* param )
    {
        _glGetVertexArrayiv( vaobj, pname, param );
    }

    public void GetVertexArrayiv( GLuint vaobj, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glGetVertexArrayiv( vaobj, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetVertexArrayIndexediv( GLuint vaobj, GLuint index, GLenum pname, GLint* param )
    {
        _glGetVertexArrayIndexediv( vaobj, index, pname, param );
    }

    public void GetVertexArrayIndexediv( GLuint vaobj, GLuint index, GLenum pname, ref GLint[] param )
    {
        fixed ( GLint* ptrParam = &param[ 0 ] )
        {
            _glGetVertexArrayIndexediv( vaobj, index, pname, ptrParam );
        }
    }

    // ========================================================================

    public void GetVertexArrayIndexed64iv( GLuint vaobj, GLuint index, GLenum pname, GLint64* param )
    {
        _glGetVertexArrayIndexed64iv( vaobj, index, pname, param );
    }

    public void GetVertexArrayIndexed64iv( GLuint vaobj, GLuint index, GLenum pname, ref GLint64[] param )
    {
        fixed ( GLint64* ptrParam = &param[ 0 ] )
        {
            _glGetVertexArrayIndexed64iv( vaobj, index, pname, ptrParam );
        }
    }

    // ========================================================================

    public void CreateSamplers( GLsizei n, GLuint* samplers )
    {
        _glCreateSamplers( n, samplers );
    }

    public GLuint[] CreateSamplers( GLsizei n )
    {
        var samplers = new GLuint[ n ];

        fixed ( GLuint* ptrSamplers = &samplers[ 0 ] )
        {
            _glCreateSamplers( n, ptrSamplers );
        }

        return samplers;
    }

    public GLuint CreateSamplers()
    {
        return CreateSamplers( 1 )[ 0 ];
    }

    // ========================================================================

    public void CreateProgramPipelines( GLsizei n, GLuint* pipelines )
    {
        _glCreateProgramPipelines( n, pipelines );
    }

    public GLuint[] CreateProgramPipelines( GLsizei n )
    {
        var pipelines = new GLuint[ n ];

        fixed ( GLuint* ptrPipelines = &pipelines[ 0 ] )
        {
            _glCreateProgramPipelines( n, ptrPipelines );
        }

        return pipelines;
    }

    public GLuint CreateProgramPipeline()
    {
        return CreateProgramPipelines( 1 )[ 0 ];
    }

    // ========================================================================

    public void CreateQueries( GLenum target, GLsizei n, GLuint* ids )
    {
        _glCreateQueries( target, n, ids );
    }

    // ========================================================================

    public GLuint[] CreateQueries( GLenum target, GLsizei n )
    {
        var ids = new GLuint[ n ];

        fixed ( GLuint* ptrIds = &ids[ 0 ] )
        {
            _glCreateQueries( target, n, ptrIds );
        }

        return ids;
    }

    // ========================================================================

    public GLuint CreateQuery( GLenum target )
    {
        return CreateQueries( target, 1 )[ 0 ];
    }

    // ========================================================================

    public void GetQueryBufferObjecti64v( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjecti64v( id, buffer, pname, offset );
    }

    // ========================================================================

    public void GetQueryBufferObjectiv( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjectiv( id, buffer, pname, offset );
    }

    // ========================================================================

    public void GetQueryBufferObjectui64v( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjectui64v( id, buffer, pname, offset );
    }

    // ========================================================================

    public void GetQueryBufferObjectuiv( GLuint id, GLuint buffer, GLenum pname, GLintptr offset )
    {
        _glGetQueryBufferObjectuiv( id, buffer, pname, offset );
    }

    // ========================================================================

    public void MemoryBarrierByRegion( GLbitfield barriers )
    {
        _glMemoryBarrierByRegion( barriers );
    }

    // ========================================================================

    public void GetTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset,
                                    GLint zoffset, GLsizei width, GLsizei height, GLsizei depth,
                                    GLenum format, GLenum type, GLsizei bufSize, void* pixels )
    {
        _glGetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels );
    }

    // ========================================================================

    public byte[] GetTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset,
                                      GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type,
                                      GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];

        fixed ( void* ptrPixels = &pixels[ 0 ] )
        {
            _glGetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width,
                                   height, depth, format, type, bufSize, ptrPixels );
        }

        return pixels;
    }

    // ========================================================================

    public void GetCompressedTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLsizei bufSize, void* pixels )
    {
        _glGetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels );
    }

    // ========================================================================

    public byte[] GetCompressedTextureSubImage( GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];

        fixed ( void* ptrPixels = &pixels[ 0 ] )
        {
            _glGetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, ptrPixels );
        }

        return pixels;
    }

    // ========================================================================

    public GLenum GetGraphicsResetStatus()
    {
        return _glGetGraphicsResetStatus();
    }

    // ========================================================================

    public void GetnCompressedTexImage( GLenum target, GLint lod, GLsizei bufSize, void* pixels )
    {
        _glGetnCompressedTexImage( target, lod, bufSize, pixels );
    }

    // ========================================================================

    public byte[] GetnCompressedTexImage( GLenum target, GLint lod, GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];

        fixed ( void* ptrPixels = &pixels[ 0 ] )
        {
            _glGetnCompressedTexImage( target, lod, bufSize, ptrPixels );
        }

        return pixels;
    }

    // ========================================================================

    public void GetnTexImage( GLenum target, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pixels )
    {
        _glGetnTexImage( target, level, format, type, bufSize, pixels );
    }

    // ========================================================================

    public byte[] GetnTexImage( GLenum target, GLint level, GLenum format, GLenum type, GLsizei bufSize )
    {
        var pixels = new byte[ bufSize ];

        fixed ( void* ptrPixels = &pixels[ 0 ] )
        {
            _glGetnTexImage( target, level, format, type, bufSize, ptrPixels );
        }

        return pixels;
    }

    // ========================================================================

    public void GetnUniformdv( GLuint program, GLint location, GLsizei bufSize, GLdouble* parameters )
    {
        _glGetnUniformdv( program, location, bufSize, parameters );
    }

    // ========================================================================

    public void GetnUniformdv( GLuint program, GLint location, GLsizei bufSize, ref GLdouble[] parameters )
    {
        fixed ( void* ptrParameters = &parameters[ 0 ] )
        {
            _glGetnUniformdv( program, location, bufSize, ( GLdouble* )ptrParameters );
        }
    }

    // ========================================================================

    public void GetnUniformfv( GLuint program, GLint location, GLsizei bufSize, GLfloat* parameters )
    {
        _glGetnUniformfv( program, location, bufSize, parameters );
    }

    // ========================================================================

    public void GetnUniformfv( GLuint program, GLint location, GLsizei bufSize, ref GLfloat[] parameters )
    {
        fixed ( void* ptrParameters = &parameters[ 0 ] )
        {
            _glGetnUniformfv( program, location, bufSize, ( GLfloat* )ptrParameters );
        }
    }

    // ========================================================================

    public void GetnUniformiv( GLuint program, GLint location, GLsizei bufSize, GLint* parameters )
    {
        _glGetnUniformiv( program, location, bufSize, parameters );
    }

    // ========================================================================

    public void GetnUniformiv( GLuint program, GLint location, GLsizei bufSize, ref GLint[] parameters )
    {
        fixed ( void* ptrParameters = &parameters[ 0 ] )
        {
            _glGetnUniformiv( program, location, bufSize, ( GLint* )ptrParameters );
        }
    }

    // ========================================================================

    public void GetnUniformuiv( GLuint program, GLint location, GLsizei bufSize, GLuint* parameters )
    {
        _glGetnUniformuiv( program, location, bufSize, parameters );
    }

    // ========================================================================

    public void GetnUniformuiv( GLuint program, GLint location, GLsizei bufSize, ref GLuint[] parameters )
    {
        fixed ( void* ptrParameters = &parameters[ 0 ] )
        {
            _glGetnUniformuiv( program, location, bufSize, ( GLuint* )ptrParameters );
        }
    }

    // ========================================================================

    public void ReadnPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, GLsizei bufSize, void* data )
    {
        _glReadnPixels( x, y, width, height, format, type, bufSize, data );
    }

    // ========================================================================

    public byte[] ReadnPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, GLsizei bufSize )
    {
        var data = new byte[ bufSize ];

        fixed ( void* ptrData = &data[ 0 ] )
        {
            _glReadnPixels( x, y, width, height, format, type, bufSize, ptrData );
        }

        return data;
    }

    // ========================================================================

    public void TextureBarrier()
    {
        _glTextureBarrier();
    }

    // ========================================================================

    public void SpecializeShader( GLuint shader, GLchar* pEntryPoint, GLuint numSpecializationConstants, GLuint* pConstantIndex, GLuint* pConstantValue )
    {
        _glSpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
    }

    // ========================================================================

    public void SpecializeShader( GLuint shader, string pEntryPoint, GLuint numSpecializationConstants, GLuint[] pConstantIndex, GLuint[] pConstantValue )
    {
        var pEntryPointBytes = Encoding.UTF8.GetBytes( pEntryPoint );

        fixed ( GLchar* ptrPEntryPoint = &pEntryPointBytes[ 0 ] )
        fixed ( GLuint* ptrPConstantIndex = &pConstantIndex[ 0 ] )
        fixed ( GLuint* ptrPConstantValue = &pConstantValue[ 0 ] )
        {
            _glSpecializeShader( shader, ptrPEntryPoint, numSpecializationConstants, ptrPConstantIndex, ptrPConstantValue );
        }
    }

    // ========================================================================

    public void MultiDrawArraysIndirectCount( GLenum mode, void* indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
    }

    // ========================================================================

    public void MultiDrawArraysIndirectCount( GLenum mode, DrawArraysIndirectCommand indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawArraysIndirectCount( mode, &indirect, drawcount, maxdrawcount, stride );
    }

    // ========================================================================

    public void MultiDrawElementsIndirectCount( GLenum mode, GLenum type, void* indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
    }

    // ========================================================================

    public void MultiDrawElementsIndirectCount( GLenum mode, GLenum type, DrawElementsIndirectCommand indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride )
    {
        _glMultiDrawElementsIndirectCount( mode, type, &indirect, drawcount, maxdrawcount, stride );
    }

    // ========================================================================

    /// <summary>
    /// Controls the parameters of polygon offset.
    /// </summary>
    /// <param name="factor">Specifies a scale factor that is used to create a variable depth offset for each polygon. The initial value is 0.</param>
    /// <param name="units">Is multiplied by an implementation-specific value to create a constant depth offset. The initial value is 0.</param>
    /// <param name="clamp">Specifies the maximum (or minimum) depth clamping value. The initial value is 0.</param>
    public void PolygonOffsetClamp( GLfloat factor, GLfloat units, GLfloat clamp )
    {
        _glPolygonOffsetClamp( factor, units, clamp );
    }

    // ========================================================================

    public void TransformFeedbackVaryings( GLuint program, GLsizei count, GLchar** varyings, GLenum bufferMode )
    {
        _glTransformFeedbackVaryings( program, count, varyings, bufferMode );
    }

    // ========================================================================

    public void TransformFeedbackVaryings( GLuint program, string[] varyings, GLenum bufferMode )
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

    // ========================================================================

    public void GetTransformFeedbackVarying( GLuint program,
                                             GLuint index,
                                             GLsizei bufSize,
                                             GLsizei* length,
                                             GLsizei* size,
                                             GLenum* type,
                                             GLchar* name )
    {
        _glGetTransformFeedbackVarying( program, index, bufSize, length, size, type, name );
    }

    // ========================================================================

    public string GetTransformFeedbackVarying( GLuint program, GLuint index, GLsizei bufSize, out GLsizei size, out GLenum type )
    {
        var name = new GLchar[ bufSize ];

        fixed ( GLsizei* pSize = &size )
        {
            fixed ( GLenum* pType = &type )
            {
                fixed ( GLchar* p = &name[ 0 ] )
                {
                    GLsizei length;

                    _glGetTransformFeedbackVarying( program, index, bufSize, &length, pSize, pType, p );

                    return new string( ( sbyte* )p, 0, length, Encoding.UTF8 );
                }
            }
        }
    }

    // ========================================================================

    public void ClampColor( GLenum target, GLenum clamp )
    {
        _glClampColor( target, clamp );
    }

    // ========================================================================

    public void BeginConditionalRender( GLuint id, GLenum mode )
    {
        _glBeginConditionalRender( id, mode );
    }

    // ========================================================================

    public void EndConditionalRender()
    {
        _glEndConditionalRender();
    }

    // ========================================================================

    public void VertexAttribIPointer( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer )
    {
        _glVertexAttribIPointer( index, size, type, stride, pointer );
    }

    // ========================================================================

    public void VertexAttribIPointer( GLuint index, GLint size, GLenum type, GLsizei stride, uint pointer )
    {
        _glVertexAttribIPointer( index, size, type, stride, ( void* )pointer );
    }

    // ========================================================================

    public void GetVertexAttribIiv( GLuint index, GLenum pname, GLint* parameters )
    {
        _glGetVertexAttribIiv( index, pname, parameters );
    }

    // ========================================================================

    public void GetVertexAttribIiv( GLuint index, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetVertexAttribIiv( index, pname, p );
        }
    }

    // ========================================================================

    public void GetVertexAttribIuiv( GLuint index, GLenum pname, GLuint* parameters )
    {
        _glGetVertexAttribIuiv( index, pname, parameters );
    }

    // ========================================================================

    public void GetVertexAttribIuiv( GLuint index, GLenum pname, ref GLuint[] parameters )
    {
        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetVertexAttribIuiv( index, pname, p );
        }
    }

    // ========================================================================

    public void VertexAttribI1i( GLuint index, GLint x )
    {
        _glVertexAttribI1i( index, x );
    }

    // ========================================================================

    public void VertexAttribI2i( GLuint index, GLint x, GLint y )
    {
        _glVertexAttribI2i( index, x, y );
    }

    // ========================================================================

    public void VertexAttribI3i( GLuint index, GLint x, GLint y, GLint z )
    {
        _glVertexAttribI3i( index, x, y, z );
    }

    // ========================================================================

    public void VertexAttribI4i( GLuint index, GLint x, GLint y, GLint z, GLint w )
    {
        _glVertexAttribI4i( index, x, y, z, w );
    }

    // ========================================================================

    public void VertexAttribI1ui( GLuint index, GLuint x )
    {
        _glVertexAttribI1ui( index, x );
    }

    // ========================================================================

    public void VertexAttribI2ui( GLuint index, GLuint x, GLuint y )
    {
        _glVertexAttribI2ui( index, x, y );
    }

    // ========================================================================

    public void VertexAttribI3ui( GLuint index, GLuint x, GLuint y, GLuint z )
    {
        _glVertexAttribI3ui( index, x, y, z );
    }

    // ========================================================================

    public void VertexAttribI4ui( GLuint index, GLuint x, GLuint y, GLuint z, GLuint w )
    {
        _glVertexAttribI4ui( index, x, y, z, w );
    }

    // ========================================================================

    public void VertexAttribI1iv( GLuint index, GLint* v )
    {
        _glVertexAttribI1iv( index, v );
    }

    public void VertexAttribI1iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI1iv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI2iv( GLuint index, GLint* v )
    {
        _glVertexAttribI2iv( index, v );
    }

    public void VertexAttribI2iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI2iv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI3iv( GLuint index, GLint* v )
    {
        _glVertexAttribI3iv( index, v );
    }

    public void VertexAttribI3iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI3iv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI4iv( GLuint index, GLint* v )
    {
        _glVertexAttribI4iv( index, v );
    }

    public void VertexAttribI4iv( GLuint index, GLint[] v )
    {
        fixed ( GLint* p = &v[ 0 ] )
        {
            _glVertexAttribI4iv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI1uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI1uiv( index, v );
    }

    public void VertexAttribI1uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI1uiv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI2uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI2uiv( index, v );
    }

    public void VertexAttribI2uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI2uiv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI3uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI3uiv( index, v );
    }

    public void VertexAttribI3uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI3uiv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI4uiv( GLuint index, GLuint* v )
    {
        _glVertexAttribI4uiv( index, v );
    }

    public void VertexAttribI4uiv( GLuint index, GLuint[] v )
    {
        fixed ( GLuint* p = &v[ 0 ] )
        {
            _glVertexAttribI4uiv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI4bv( GLuint index, GLbyte* v )
    {
        _glVertexAttribI4bv( index, v );
    }

    public void VertexAttribI4bv( GLuint index, GLbyte[] v )
    {
        fixed ( GLbyte* p = &v[ 0 ] )
        {
            _glVertexAttribI4bv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI4sv( GLuint index, GLshort* v )
    {
        _glVertexAttribI4sv( index, v );
    }

    public void VertexAttribI4sv( GLuint index, GLshort[] v )
    {
        fixed ( GLshort* p = &v[ 0 ] )
        {
            _glVertexAttribI4sv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI4ubv( GLuint index, GLubyte* v )
    {
        _glVertexAttribI4ubv( index, v );
    }

    public void VertexAttribI4ubv( GLuint index, GLubyte[] v )
    {
        fixed ( GLubyte* p = &v[ 0 ] )
        {
            _glVertexAttribI4ubv( index, p );
        }
    }

    // ========================================================================

    public void VertexAttribI4usv( GLuint index, GLushort* v )
    {
        _glVertexAttribI4usv( index, v );
    }

    public void VertexAttribI4usv( GLuint index, GLushort[] v )
    {
        fixed ( GLushort* p = &v[ 0 ] )
        {
            _glVertexAttribI4usv( index, p );
        }
    }

    // ========================================================================

    public void GetUniformuiv( GLuint program, GLint location, GLuint* parameters )
    {
        _glGetUniformuiv( program, location, parameters );
    }

    public void GetUniformuiv( GLuint program, GLint location, ref GLuint[] parameters )
    {
        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetUniformuiv( program, location, p );
        }
    }

    // ========================================================================

    public void BindFragDataLocation( GLuint program, GLuint color, GLchar* name )
    {
        _glBindFragDataLocation( program, color, name );
    }

    public void BindFragDataLocation( GLuint program, GLuint color, string name )
    {
        var narr = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &narr[ 0 ] )
        {
            _glBindFragDataLocation( program, color, p );
        }
    }

    // ========================================================================

    public GLint GetFragDataLocation( GLuint program, GLchar* name )
    {
        return _glGetFragDataLocation( program, name );
    }

    public GLint GetFragDataLocation( GLuint program, string name )
    {
        var narr = Encoding.UTF8.GetBytes( name );

        fixed ( GLchar* p = &narr[ 0 ] )
        {
            return _glGetFragDataLocation( program, p );
        }
    }

    // ========================================================================

    public void Uniform1ui( GLint location, GLuint v0 )
    {
        _glUniform1ui( location, v0 );
    }

    // ========================================================================

    public void Uniform2ui( GLint location, GLuint v0, GLuint v1 )
    {
        _glUniform2ui( location, v0, v1 );
    }

    // ========================================================================

    public void Uniform3ui( GLint location, GLuint v0, GLuint v1, GLuint v2 )
    {
        _glUniform3ui( location, v0, v1, v2 );
    }

    // ========================================================================

    public void Uniform4ui( GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 )
    {
        _glUniform4ui( location, v0, v1, v2, v3 );
    }

    // ========================================================================

    public void Uniform1uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform1uiv( location, count, value );
    }

    public void Uniform1uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform1uiv( location, value.Length, p );
        }
    }

    // ========================================================================

    public void Uniform2uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform2uiv( location, count, value );
    }

    public void Uniform2uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform2uiv( location, value.Length / 2, p );
        }
    }

    // ========================================================================

    public void Uniform3uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform3uiv( location, count, value );
    }

    public void Uniform3uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform3uiv( location, value.Length / 3, p );
        }
    }

    // ========================================================================

    public void Uniform4uiv( GLint location, GLsizei count, GLuint* value )
    {
        _glUniform4uiv( location, count, value );
    }

    public void Uniform4uiv( GLint location, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glUniform4uiv( location, value.Length / 4, p );
        }
    }

    // ========================================================================

    public void TexParameterIiv( GLenum target, GLenum pname, GLint* param )
    {
        _glTexParameterIiv( target, pname, param );
    }

    public void TexParameterIiv( GLenum target, GLenum pname, GLint[] param )
    {
        fixed ( GLint* p = &param[ 0 ] )
        {
            _glTexParameterIiv( target, pname, p );
        }
    }

    // ========================================================================

    public void TexParameterIuiv( GLenum target, GLenum pname, GLuint* param )
    {
        _glTexParameterIuiv( target, pname, param );
    }

    public void TexParameterIuiv( GLenum target, GLenum pname, GLuint[] param )
    {
        fixed ( GLuint* p = &param[ 0 ] )
        {
            _glTexParameterIuiv( target, pname, p );
        }
    }

    // ========================================================================

    public void GetTexParameterIiv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetTexParameterIiv( target, pname, parameters );
    }

    public void GetTexParameterIiv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetTexParameterIiv( target, pname, p );
        }
    }

    // ========================================================================

    public void GetTexParameterIuiv( GLenum target, GLenum pname, GLuint* parameters )
    {
        _glGetTexParameterIuiv( target, pname, parameters );
    }

    public void GetTexParameterIuiv( GLenum target, GLenum pname, ref GLuint[] parameters )
    {
        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetTexParameterIuiv( target, pname, p );
        }
    }

    // ========================================================================

    public void ClearBufferiv( GLenum buffer, GLint drawbuffer, GLint* value )
    {
        _glClearBufferiv( buffer, drawbuffer, value );
    }

    public void ClearBufferiv( GLenum buffer, GLint drawbuffer, GLint[] value )
    {
        fixed ( GLint* p = &value[ 0 ] )
        {
            _glClearBufferiv( buffer, drawbuffer, p );
        }
    }

    // ========================================================================

    public void ClearBufferuiv( GLenum buffer, GLint drawbuffer, GLuint* value )
    {
        _glClearBufferuiv( buffer, drawbuffer, value );
    }

    public void ClearBufferuiv( GLenum buffer, GLint drawbuffer, GLuint[] value )
    {
        fixed ( GLuint* p = &value[ 0 ] )
        {
            _glClearBufferuiv( buffer, drawbuffer, p );
        }
    }

    // ========================================================================

    public void ClearBufferfv( GLenum buffer, GLint drawbuffer, GLfloat* value )
    {
        _glClearBufferfv( buffer, drawbuffer, value );
    }

    public void ClearBufferfv( GLenum buffer, GLint drawbuffer, GLfloat[] value )
    {
        fixed ( GLfloat* p =
                   &value[ 0 ] )
        {
            _glClearBufferfv( buffer, drawbuffer, p );
        }
    }

    // ========================================================================

    public void ClearBufferfi( GLenum buffer, GLint drawbuffer, GLfloat depth, GLint stencil )
    {
        _glClearBufferfi( buffer, drawbuffer, depth, stencil );
    }

    // ========================================================================

    public GLubyte* GetStringi( GLenum name, GLuint index )
    {
        return _glGetStringi( name, index );
    }

    public string GetStringiSafe( GLenum name, GLuint index )
    {
        var ptr = _glGetStringi( name, index );

        if ( ptr == null )
        {
            return string.Empty;
        }

        var i = 0;

        while ( ptr[ i ] != 0 )
        {
            i++;
        }

        return new string( ( sbyte* )ptr, 0, i, Encoding.UTF8 );
    }

    // ========================================================================

    public GLboolean IsRenderbuffer( GLuint renderbuffer )
    {
        return _glIsRenderbuffer( renderbuffer );
    }

    // ========================================================================

    public void BindRenderbuffer( GLenum target, GLuint renderbuffer )
    {
        _glBindRenderbuffer( target, renderbuffer );
    }

    // ========================================================================

    public void DeleteRenderbuffers( GLsizei n, GLuint* renderbuffers )
    {
        _glDeleteRenderbuffers( n, renderbuffers );
    }

    public void DeleteRenderbuffers( params GLuint[] renderbuffers )
    {
        fixed ( GLuint* p = &renderbuffers[ 0 ] )
        {
            _glDeleteRenderbuffers( renderbuffers.Length, p );
        }
    }

    // ========================================================================

    public void GenRenderbuffers( GLsizei n, GLuint* renderbuffers )
    {
        _glGenRenderbuffers( n, renderbuffers );
    }

    public GLuint[] GenRenderbuffers( GLsizei n )
    {
        var renderbuffers = new GLuint[ n ];

        fixed ( GLuint* p = &renderbuffers[ 0 ] )
        {
            _glGenRenderbuffers( n, p );
        }

        return renderbuffers;
    }

    public GLuint GenRenderbuffer()
    {
        return GenRenderbuffers( 1 )[ 0 ];
    }

    // ========================================================================

    public void RenderbufferStorage( GLenum target, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glRenderbufferStorage( target, internalformat, width, height );
    }

    // ========================================================================

    public void GetRenderbufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetRenderbufferParameteriv( target, pname, parameters );
    }

    public void GetRenderbufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetRenderbufferParameteriv( target, pname, p );
        }
    }

    // ========================================================================

    public GLboolean IsFramebuffer( GLuint framebuffer )
    {
        return _glIsFramebuffer( framebuffer );
    }

    // ========================================================================

    public void BindFramebuffer( GLenum target, GLuint framebuffer )
    {
        _glBindFramebuffer( target, framebuffer );
    }

    // ========================================================================

    public void DeleteFramebuffers( GLsizei n, GLuint* framebuffers )
    {
        _glDeleteFramebuffers( n, framebuffers );
    }

    public void DeleteFramebuffers( params GLuint[] framebuffers )
    {
        fixed ( GLuint* p = &framebuffers[ 0 ] )
        {
            _glDeleteFramebuffers( framebuffers.Length, p );
        }
    }

    // ========================================================================

    public void GenFramebuffers( GLsizei n, GLuint* framebuffers )
    {
        _glGenFramebuffers( n, framebuffers );
    }

    public GLuint[] GenFramebuffers( GLsizei n )
    {
        var framebuffers = new GLuint[ n ];

        fixed ( GLuint* p = &framebuffers[ 0 ] )
        {
            _glGenFramebuffers( n, p );
        }

        return framebuffers;
    }

    public GLuint GenFramebuffer()
    {
        return GenFramebuffers( 1 )[ 0 ];
    }

    // ========================================================================

    public GLenum CheckFramebufferStatus( GLenum target )
    {
        return _glCheckFramebufferStatus( target );
    }

    // ========================================================================

    public void FramebufferTexture1D( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level )
    {
        _glFramebufferTexture1D( target, attachment, textarget, texture, level );
    }

    // ========================================================================

    public void FramebufferTexture2D( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level )
    {
        _glFramebufferTexture2D( target, attachment, textarget, texture, level );
    }

    // ========================================================================

    public void FramebufferTexture3D( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level, GLint zoffset )
    {
        _glFramebufferTexture3D( target, attachment, textarget, texture, level, zoffset );
    }

    // ========================================================================

    public void FramebufferRenderbuffer( GLenum target, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer )
    {
        _glFramebufferRenderbuffer( target, attachment, renderbuffertarget, renderbuffer );
    }

    // ========================================================================

    public void GetFramebufferAttachmentParameteriv( GLenum target, GLenum attachment, GLenum pname, GLint* parameters )
    {
        _glGetFramebufferAttachmentParameteriv( target, attachment, pname, parameters );
    }

    public void GetFramebufferAttachmentParameteriv( GLenum target, GLenum attachment, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetFramebufferAttachmentParameteriv( target, attachment, pname, p );
        }
    }

    // ========================================================================

    public void GenerateMipmap( GLenum target )
    {
        _glGenerateMipmap( target );
    }

    // ========================================================================

    public void BlitFramebuffer( GLint srcX0,
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

    // ========================================================================

    public void RenderbufferStorageMultisample( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height )
    {
        _glRenderbufferStorageMultisample( target, samples, internalformat, width, height );
    }

    // ========================================================================

    public void FramebufferTextureLayer( GLenum target, GLenum attachment, GLuint texture, GLint level, GLint layer )
    {
        _glFramebufferTextureLayer( target, attachment, texture, level, layer );
    }

    // ========================================================================

    public void* MapBufferRange( GLenum target, GLintptr offset, GLsizeiptr length, GLbitfield access )
    {
        return _glMapBufferRange( target, offset, length, access );
    }

    public Span< T > MapBufferRange< T >( GLenum target, GLintptr offset, GLsizeiptr length, GLbitfield access ) where T : unmanaged
    {
        void* ret = _glMapBufferRange( target, offset, length, access );

        return new Span< T >( ret, length );
    }

    // ========================================================================

    public void FlushMappedBufferRange( GLenum target, GLintptr offset, GLsizeiptr length )
    {
        _glFlushMappedBufferRange( target, offset, length );
    }

    // ========================================================================

    public void BindVertexArray( GLuint array )
    {
        _glBindVertexArray( array );
    }

    // ========================================================================

    public void DeleteVertexArrays( GLsizei n, GLuint* arrays )
    {
        _glDeleteVertexArrays( n, arrays );
    }

    public void DeleteVertexArrays( params GLuint[] arrays )
    {
        fixed ( GLuint* p = &arrays[ 0 ] )
        {
            _glDeleteVertexArrays( arrays.Length, p );
        }
    }

    // ========================================================================

    public void GenVertexArrays( GLsizei n, GLuint* arrays )
    {
        GetDelegateForFunction< PFNGLGENVERTEXARRAYSPROC >( "glGenVertexArrays", out _glGenVertexArrays );
        
        _glGenVertexArrays( n, arrays );
    }

    public GLuint[] GenVertexArrays( GLsizei n )
    {
        var arrays = new GLuint[ n ];

        fixed ( GLuint* p = &arrays[ 0 ] )
        {
            _glGenVertexArrays( n, p );
        }

        return arrays;
    }

    public GLuint GenVertexArray()
    {
        GLuint array = 0;

        _glGenVertexArrays( 1, &array );

        return array;
    }

    // ========================================================================

    public GLboolean IsVertexArray( GLuint array )
    {
        return _glIsVertexArray( array );
    }

    // ========================================================================

    public void DrawArraysInstanced( GLenum mode, GLint first, GLsizei count, GLsizei instancecount )
    {
        _glDrawArraysInstanced( mode, first, count, instancecount );
    }

    // ========================================================================

    public void DrawElementsInstanced( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount )
    {
        _glDrawElementsInstanced( mode, count, type, indices, instancecount );
    }

    public void DrawElementsInstanced< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( T* p = &indices[ 0 ] )
        {
            _glDrawElementsInstanced( mode, count, type, p, instancecount );
        }
    }

    // ========================================================================

    public void TexBuffer( GLenum target, GLenum internalformat, GLuint buffer )
    {
        _glTexBuffer( target, internalformat, buffer );
    }

    // ========================================================================

    public void PrimitiveRestartIndex( GLuint index )
    {
        _glPrimitiveRestartIndex( index );
    }

    // ========================================================================

    public void CopyBufferSubData( GLenum readTarget, GLenum writeTarget, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size )
    {
        _glCopyBufferSubData( readTarget, writeTarget, readOffset, writeOffset, size );
    }

    // ========================================================================

    public void GetUniformIndices( GLuint program, GLsizei uniformCount, GLchar** uniformNames, GLuint* uniformIndices )
    {
        _glGetUniformIndices( program, uniformCount, uniformNames, uniformIndices );
    }

    public GLuint[] GetUniformIndices( GLuint program, params string[] uniformNames )
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

    // ========================================================================

    public void GetActiveUniformsiv( GLuint program, GLsizei uniformCount, GLuint* uniformIndices, GLenum pname, GLint* parameters )
    {
        _glGetActiveUniformsiv( program, uniformCount, uniformIndices, pname, parameters );
    }

    public GLint[] GetActiveUniformsiv( GLuint program, GLenum pname, params GLuint[] uniformIndices )
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

    // ========================================================================

    public void GetActiveUniformName( GLuint program, GLuint uniformIndex, GLsizei bufSize, GLsizei* length, GLchar* uniformName )
    {
        _glGetActiveUniformName( program, uniformIndex, bufSize, length, uniformName );
    }

    // ========================================================================

    public string GetActiveUniformName( GLuint program, GLuint uniformIndex, GLsizei bufSize )
    {
        var     uniformName = stackalloc GLchar[ bufSize ];
        GLsizei length;
        _glGetActiveUniformName( program, uniformIndex, bufSize, &length, uniformName );

        return new string( ( sbyte* )uniformName, 0, length, Encoding.UTF8 );
    }

    // ========================================================================

    public GLuint GetUniformBlockIndex( GLuint program, GLchar* uniformBlockName )
    {
        return _glGetUniformBlockIndex( program, uniformBlockName );
    }

    // ========================================================================

    public GLuint GetUniformBlockIndex( GLuint program, string uniformBlockName )
    {
        var uniformBlockNameBytes = Encoding.UTF8.GetBytes( uniformBlockName );

        fixed ( GLchar* p = &uniformBlockNameBytes[ 0 ] )
        {
            return _glGetUniformBlockIndex( program, p );
        }
    }

    // ========================================================================

    public void GetActiveUniformBlockiv( GLuint program, GLuint uniformBlockIndex, GLenum pname, GLint* parameters )
    {
        _glGetActiveUniformBlockiv( program, uniformBlockIndex, pname, parameters );
    }

    public void GetActiveUniformBlockiv( GLuint program, GLuint uniformBlockIndex, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetActiveUniformBlockiv( program, uniformBlockIndex, pname, p );
        }
    }

    // ========================================================================

    public void GetActiveUniformBlockName( GLuint program, GLuint uniformBlockIndex, GLsizei bufSize, GLsizei* length, GLchar* uniformBlockName )
    {
        _glGetActiveUniformBlockName( program, uniformBlockIndex, bufSize, length, uniformBlockName );
    }

    // ========================================================================

    public string GetActiveUniformBlockName( GLuint program, GLuint uniformBlockIndex, GLsizei bufSize )
    {
        var uniformBlockName = stackalloc GLchar[ bufSize ];

        GLsizei length;

        _glGetActiveUniformBlockName( program, uniformBlockIndex, bufSize, &length, uniformBlockName );

        return new string( ( sbyte* )uniformBlockName, 0, length, Encoding.UTF8 );
    }

    // ========================================================================

    public void UniformBlockBinding( GLuint program, GLuint uniformBlockIndex, GLuint uniformBlockBinding )
    {
        _glUniformBlockBinding( program, uniformBlockIndex, uniformBlockBinding );
    }

    // ========================================================================

    public void DrawElementsBaseVertex( GLenum mode, GLsizei count, GLenum type, void* indices, GLint basevertex )
    {
        _glDrawElementsBaseVertex( mode, count, type, indices, basevertex );
    }

    public void DrawElementsBaseVertex< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLint basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsBaseVertex( mode, count, type, p, basevertex );
        }
    }

    // ========================================================================

    public void DrawRangeElementsBaseVertex( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices, GLint basevertex )
    {
        _glDrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
    }

    public void DrawRangeElementsBaseVertex< T >( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, T[] indices, GLint basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawRangeElementsBaseVertex( mode, start, end, count, type, p, basevertex );
        }
    }

    // ========================================================================

    public void DrawElementsInstancedBaseVertex( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLint basevertex )
    {
        _glDrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
    }

    public void DrawElementsInstancedBaseVertex< T >( GLenum mode, GLsizei count, GLenum type, T[] indices, GLsizei instancecount, GLint basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( void* p = &indices[ 0 ] )
        {
            _glDrawElementsInstancedBaseVertex( mode, count, type, p, instancecount, basevertex );
        }
    }

    // ========================================================================

    public void MultiDrawElementsBaseVertex( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount, GLint* basevertex )
    {
        _glMultiDrawElementsBaseVertex( mode, count, type, indices, drawcount, basevertex );
    }

    public void MultiDrawElementsBaseVertex< T >( GLenum mode, GLenum type, T[][] indices, GLint[] basevertex ) where T : unmanaged, IUnsignedNumber< T >
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

    // ========================================================================

    public void ProvokingVertex( GLenum mode ) => _glProvokingVertex( mode );

    // ========================================================================

    public void* FenceSync( GLenum condition, GLbitfield flags )
    {
        return _glFenceSync( condition, flags );
    }

    public IntPtr FenceSyncSafe( GLenum condition, GLbitfield flags )
    {
        return new IntPtr( _glFenceSync( condition, flags ) );
    }

    // ========================================================================

    public GLboolean IsSync( void* sync )
    {
        return _glIsSync( sync );
    }

    public GLboolean IsSyncSafe( IntPtr sync )
    {
        return _glIsSync( sync.ToPointer() );
    }

    // ========================================================================

    /// <inheritdoc />
    public void DeleteSync( void* sync )
    {
        _glDeleteSync( sync );
    }

    /// <inheritdoc />
    public void DeleteSyncSafe( IntPtr sync )
    {
        _glDeleteSync( sync.ToPointer() );
    }

    // ========================================================================

    /// <summary>
    /// Causes the client to block and wait for a sync object to become signaled.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose status to wait on.</param>
    /// <param name="flags">
    /// A bitfield controlling the command flushing behavior. <paramref name="flags"/> may be
    /// <see cref="IGL.GL_SYNC_FLUSH_COMMANDS_BIT"/> or zero.
    /// </param>
    /// <param name="timeout">
    /// The timeout, specified in nanoseconds, for which the implementation should wait for
    /// <paramref name="sync"/> to become signaled.
    /// </param>
    /// <returns>
    /// One of <see cref="IGL.GL_ALREADY_SIGNALED"/>, <see cref="IGL.GL_TIMEOUT_EXPIRED"/>,
    /// <see cref="IGL.GL_CONDITION_SATISFIED"/>, or <see cref="IGL.GL_WAIT_FAILED"/>.
    /// </returns>
    public GLenum ClientWaitSync( void* sync, GLbitfield flags, GLuint64 timeout )
    {
        return _glClientWaitSync( sync, flags, timeout );
    }

    /// <summary>
    /// Causes the client to block and wait for a sync object to become signaled.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose status to wait on.</param>
    /// <param name="flags">
    /// A bitfield controlling the command flushing behavior. <paramref name="flags"/> may be
    /// <see cref="IGL.GL_SYNC_FLUSH_COMMANDS_BIT"/> or zero.
    /// </param>
    /// <param name="timeout">
    /// The timeout, specified in nanoseconds, for which the implementation should wait for
    /// <paramref name="sync"/> to become signaled.
    /// </param>
    /// <returns>
    /// One of <see cref="IGL.GL_ALREADY_SIGNALED"/>, <see cref="IGL.GL_TIMEOUT_EXPIRED"/>,
    /// <see cref="IGL.GL_CONDITION_SATISFIED"/>, or <see cref="IGL.GL_WAIT_FAILED"/>.
    /// </returns>
    public GLenum ClientWaitSyncSafe( IntPtr sync, GLbitfield flags, GLuint64 timeout )
    {
        return _glClientWaitSync( sync.ToPointer(), flags, timeout );
    }

    // ========================================================================
}

//#pragma warning restore IDE0079 // Remove unnecessary suppression
//#pragma warning restore CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
//#pragma warning restore CS8603  // Possible null reference return.
//#pragma warning restore IDE0060 // Remove unused parameter.
//#pragma warning restore IDE1006 // Naming Styles.
//#pragma warning restore IDE0090 // Use 'new(...)'.
//#pragma warning restore CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type