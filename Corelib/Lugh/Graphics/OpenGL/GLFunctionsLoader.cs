// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
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

//#pragma warning disable IDE0079 // Remove unnecessary suppression
//#pragma warning disable CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
//#pragma warning disable CS8603  // Possible null reference return.
//#pragma warning disable IDE0060 // Remove unused parameter.
//#pragma warning disable IDE1006 // Naming Styles.
//#pragma warning disable IDE0090 // Use 'new(...)'.
//#pragma warning disable CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type

// ============================================================================

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

public partial class GLBindings
{
    private bool _ogl10 = false;
    private bool _ogl11 = false;
    private bool _ogl12 = false;
    private bool _ogl13 = false;
    private bool _ogl14 = false;
    private bool _ogl15 = false;
    private bool _ogl20 = false;
    private bool _ogl21 = false;
    private bool _ogl30 = false;
    private bool _ogl31 = false;
    private bool _ogl32 = false;
    private bool _ogl33 = false;
    private bool _ogl34 = false;
    private bool _ogl40 = false;
    private bool _ogl41 = false;
    private bool _ogl42 = false;
    private bool _ogl43 = false;
    private bool _ogl44 = false;
    private bool _ogl45 = false;
    private bool _ogl46 = false;

    // ========================================================================
    // ========================================================================

    private unsafe void GetLanguageLevel()
    {
        var version = BytePointerToString.Convert( GetString( IGL.GL_VERSION ) );

        var majorVersion = ( int )char.GetNumericValue( version[ 0 ] );
        var minorVersion = ( int )char.GetNumericValue( version[ 2 ] );

        Logger.Debug( $"Major version: {majorVersion}, Minor version: {minorVersion}" );

        switch ( majorVersion )
        {
            case 1:
            {
                if ( minorVersion >= 0 ) _ogl10 = true;
                if ( minorVersion >= 1 ) _ogl11 = true;
                if ( minorVersion >= 2 ) _ogl12 = true;
                if ( minorVersion >= 3 ) _ogl13 = true;
                if ( minorVersion >= 4 ) _ogl14 = true;
                if ( minorVersion >= 5 ) _ogl15 = true;

                break;
            }

            case 2:
            {
                if ( minorVersion >= 0 ) _ogl20 = true;
                if ( minorVersion >= 1 ) _ogl21 = true;

                break;
            }

            case 3:
            {
                if ( minorVersion >= 0 ) _ogl30 = true;
                if ( minorVersion >= 1 ) _ogl31 = true;
                if ( minorVersion >= 2 ) _ogl32 = true;
                if ( minorVersion >= 3 ) _ogl33 = true;
                if ( minorVersion >= 4 ) _ogl34 = true;

                break;
            }

            case 4:
            {
                if ( minorVersion >= 0 ) _ogl40 = true;
                if ( minorVersion >= 1 ) _ogl41 = true;
                if ( minorVersion >= 2 ) _ogl42 = true;
                if ( minorVersion >= 3 ) _ogl43 = true;
                if ( minorVersion >= 4 ) _ogl44 = true;
                if ( minorVersion >= 5 ) _ogl45 = true;
                if ( minorVersion >= 6 ) _ogl46 = true;

                break;
            }

            default:
            {
                Logger.Debug( $"Error: {Gdx.GL.GetError()}" );

                throw new GdxRuntimeException( "Unable to determine OpenGL Version" );
            }
        }

        Logger.Debug( $"Language Version: {majorVersion}.{minorVersion}" );

        return;

        static GLubyte* GetString( GLenum name )
        {
            return _glGetString( name );
        }

        [DllImport( "opengl32.dll",
                    EntryPoint = "glGetString",
                    CallingConvention = CallingConvention.Cdecl,
                    CharSet = CharSet.Ansi )]
        static extern GLubyte* _glGetString( GLenum name );
    }

    // ========================================================================
    // ========================================================================

    public void Import()
    {
        Import( Glfw.GetProcAddress );
    }

    // ========================================================================
    // ========================================================================

    public void Import( IGLBindings.GetProcAddressHandler loader )
    {
        GetLanguageLevel();

        if ( _ogl10 || _ogl11 || _ogl12 || _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glCullFace               = Marshal.GetDelegateForFunctionPointer< PFNGLCULLFACEPROC >( loader.Invoke( "glCullFace" ) );
            _glFrontFace              = Marshal.GetDelegateForFunctionPointer< PFNGLFRONTFACEPROC >( loader.Invoke( "glFrontFace" ) );
            _glHint                   = Marshal.GetDelegateForFunctionPointer< PFNGLHINTPROC >( loader.Invoke( "glHint" ) );
            _glLineWidth              = Marshal.GetDelegateForFunctionPointer< PFNGLLINEWIDTHPROC >( loader.Invoke( "glLineWidth" ) );
            _glPointSize              = Marshal.GetDelegateForFunctionPointer< PFNGLPOINTSIZEPROC >( loader.Invoke( "glPointSize" ) );
            _glPolygonMode            = Marshal.GetDelegateForFunctionPointer< PFNGLPOLYGONMODEPROC >( loader.Invoke( "glPolygonMode" ) );
            _glScissor                = Marshal.GetDelegateForFunctionPointer< PFNGLSCISSORPROC >( loader.Invoke( "glScissor" ) );
            _glTexParameterf          = Marshal.GetDelegateForFunctionPointer< PFNGLTEXPARAMETERFPROC >( loader.Invoke( "glTexParameterf" ) );
            _glTexParameterfv         = Marshal.GetDelegateForFunctionPointer< PFNGLTEXPARAMETERFVPROC >( loader.Invoke( "glTexParameterfv" ) );
            _glTexParameteri          = Marshal.GetDelegateForFunctionPointer< PFNGLTEXPARAMETERIPROC >( loader.Invoke( "glTexParameteri" ) );
            _glTexParameteriv         = Marshal.GetDelegateForFunctionPointer< PFNGLTEXPARAMETERIVPROC >( loader.Invoke( "glTexParameteriv" ) );
            _glTexImage1D             = Marshal.GetDelegateForFunctionPointer< PFNGLTEXIMAGE1DPROC >( loader.Invoke( "glTexImage1D" ) );
            _glTexImage2D             = Marshal.GetDelegateForFunctionPointer< PFNGLTEXIMAGE2DPROC >( loader.Invoke( "glTexImage2D" ) );
            _glDrawBuffer             = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWBUFFERPROC >( loader.Invoke( "glDrawBuffer" ) );
            _glClear                  = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARPROC >( loader.Invoke( "glClear" ) );
            _glClearColor             = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARCOLORPROC >( loader.Invoke( "glClearColor" ) );
            _glClearStencil           = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARSTENCILPROC >( loader.Invoke( "glClearStencil" ) );
            _glClearDepth             = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARDEPTHPROC >( loader.Invoke( "glClearDepth" ) );
            _glStencilMask            = Marshal.GetDelegateForFunctionPointer< PFNGLSTENCILMASKPROC >( loader.Invoke( "glStencilMask" ) );
            _glColorMask              = Marshal.GetDelegateForFunctionPointer< PFNGLCOLORMASKPROC >( loader.Invoke( "glColorMask" ) );
            _glDepthMask              = Marshal.GetDelegateForFunctionPointer< PFNGLDEPTHMASKPROC >( loader.Invoke( "glDepthMask" ) );
            _glDisable                = Marshal.GetDelegateForFunctionPointer< PFNGLDISABLEPROC >( loader.Invoke( "glDisable" ) );
            _glEnable                 = Marshal.GetDelegateForFunctionPointer< PFNGLENABLEPROC >( loader.Invoke( "glEnable" ) );
            _glFinish                 = Marshal.GetDelegateForFunctionPointer< PFNGLFINISHPROC >( loader.Invoke( "glFinish" ) );
            _glFlush                  = Marshal.GetDelegateForFunctionPointer< PFNGLFLUSHPROC >( loader.Invoke( "glFlush" ) );
            _glBlendFunc              = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDFUNCPROC >( loader.Invoke( "glBlendFunc" ) );
            _glLogicOp                = Marshal.GetDelegateForFunctionPointer< PFNGLLOGICOPPROC >( loader.Invoke( "glLogicOp" ) );
            _glStencilFunc            = Marshal.GetDelegateForFunctionPointer< PFNGLSTENCILFUNCPROC >( loader.Invoke( "glStencilFunc" ) );
            _glStencilOp              = Marshal.GetDelegateForFunctionPointer< PFNGLSTENCILOPPROC >( loader.Invoke( "glStencilOp" ) );
            _glDepthFunc              = Marshal.GetDelegateForFunctionPointer< PFNGLDEPTHFUNCPROC >( loader.Invoke( "glDepthFunc" ) );
            _glPixelStoref            = Marshal.GetDelegateForFunctionPointer< PFNGLPIXELSTOREFPROC >( loader.Invoke( "glPixelStoref" ) );
            _glPixelStorei            = Marshal.GetDelegateForFunctionPointer< PFNGLPIXELSTOREIPROC >( loader.Invoke( "glPixelStorei" ) );
            _glReadBuffer             = Marshal.GetDelegateForFunctionPointer< PFNGLREADBUFFERPROC >( loader.Invoke( "glReadBuffer" ) );
            _glReadPixels             = Marshal.GetDelegateForFunctionPointer< PFNGLREADPIXELSPROC >( loader.Invoke( "glReadPixels" ) );
            _glGetBooleanv            = Marshal.GetDelegateForFunctionPointer< PFNGLGETBOOLEANVPROC >( loader.Invoke( "glGetBooleanv" ) );
            _glGetDoublev             = Marshal.GetDelegateForFunctionPointer< PFNGLGETDOUBLEVPROC >( loader.Invoke( "glGetDoublev" ) );
            _glGetError               = Marshal.GetDelegateForFunctionPointer< PFNGLGETERRORPROC >( loader.Invoke( "glGetError" ) );
            _glGetFloatv              = Marshal.GetDelegateForFunctionPointer< PFNGLGETFLOATVPROC >( loader.Invoke( "glGetFloatv" ) );
            _glGetIntegerv            = Marshal.GetDelegateForFunctionPointer< PFNGLGETINTEGERVPROC >( loader.Invoke( "glGetIntegerv" ) );
            _glGetString              = Marshal.GetDelegateForFunctionPointer< PFNGLGETSTRINGPROC >( loader.Invoke( "glGetString" ) );
            _glGetTexImage            = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXIMAGEPROC >( loader.Invoke( "glGetTexImage" ) );
            _glGetTexParameterfv      = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXPARAMETERFVPROC >( loader.Invoke( "glGetTexParameterfv" ) );
            _glGetTexParameteriv      = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXPARAMETERIVPROC >( loader.Invoke( "glGetTexParameteriv" ) );
            _glGetTexLevelParameterfv = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXLEVELPARAMETERFVPROC >( loader.Invoke( "glGetTexLevelParameterfv" ) );
            _glGetTexLevelParameteriv = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXLEVELPARAMETERIVPROC >( loader.Invoke( "glGetTexLevelParameteriv" ) );
            _glIsEnabled              = Marshal.GetDelegateForFunctionPointer< PFNGLISENABLEDPROC >( loader.Invoke( "glIsEnabled" ) );
            _glDepthRange             = Marshal.GetDelegateForFunctionPointer< PFNGLDEPTHRANGEPROC >( loader.Invoke( "glDepthRange" ) );
            _glViewport               = Marshal.GetDelegateForFunctionPointer< PFNGLVIEWPORTPROC >( loader.Invoke( "glViewport" ) );
        }

        if ( _ogl11 || _ogl12 || _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glDrawArrays        = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWARRAYSPROC >( loader.Invoke( "glDrawArrays" ) );
            _glDrawElements      = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWELEMENTSPROC >( loader.Invoke( "glDrawElements" ) );
            _glPolygonOffset     = Marshal.GetDelegateForFunctionPointer< PFNGLPOLYGONOFFSETPROC >( loader.Invoke( "glPolygonOffset" ) );
            _glCopyTexImage1D    = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXIMAGE1DPROC >( loader.Invoke( "glCopyTexImage1D" ) );
            _glCopyTexImage2D    = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXIMAGE2DPROC >( loader.Invoke( "glCopyTexImage2D" ) );
            _glCopyTexSubImage1D = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXSUBIMAGE1DPROC >( loader.Invoke( "glCopyTexSubImage1D" ) );
            _glCopyTexSubImage2D = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXSUBIMAGE2DPROC >( loader.Invoke( "glCopyTexSubImage2D" ) );
            _glTexSubImage1D     = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSUBIMAGE1DPROC >( loader.Invoke( "glTexSubImage1D" ) );
            _glTexSubImage2D     = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSUBIMAGE2DPROC >( loader.Invoke( "glTexSubImage2D" ) );
            _glBindTexture       = Marshal.GetDelegateForFunctionPointer< PFNGLBINDTEXTUREPROC >( loader.Invoke( "glBindTexture" ) );
            _glDeleteTextures    = Marshal.GetDelegateForFunctionPointer< PFNGLDELETETEXTURESPROC >( loader.Invoke( "glDeleteTextures" ) );
            _glGenTextures       = Marshal.GetDelegateForFunctionPointer< PFNGLGENTEXTURESPROC >( loader.Invoke( "glGenTextures" ) );
            _glIsTexture         = Marshal.GetDelegateForFunctionPointer< PFNGLISTEXTUREPROC >( loader.Invoke( "glIsTexture" ) );
        }

        if ( _ogl12 || _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glDrawRangeElements = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWRANGEELEMENTSPROC >( loader.Invoke( "glDrawRangeElements" ) );
            _glTexImage3D        = Marshal.GetDelegateForFunctionPointer< PFNGLTEXIMAGE3DPROC >( loader.Invoke( "glTexImage3D" ) );
            _glTexSubImage3D     = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSUBIMAGE3DPROC >( loader.Invoke( "glTexSubImage3D" ) );
            _glCopyTexSubImage3D = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXSUBIMAGE3DPROC >( loader.Invoke( "glCopyTexSubImage3D" ) );
        }

        if ( _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glActiveTexture           = Marshal.GetDelegateForFunctionPointer< PFNGLACTIVETEXTUREPROC >( loader.Invoke( "glActiveTexture" ) );
            _glSampleCoverage          = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLECOVERAGEPROC >( loader.Invoke( "glSampleCoverage" ) );
            _glCompressedTexImage3D    = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXIMAGE3DPROC >( loader.Invoke( "glCompressedTexImage3D" ) );
            _glCompressedTexImage2D    = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXIMAGE2DPROC >( loader.Invoke( "glCompressedTexImage2D" ) );
            _glCompressedTexImage1D    = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXIMAGE1DPROC >( loader.Invoke( "glCompressedTexImage1D" ) );
            _glCompressedTexSubImage3D = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC >( loader.Invoke( "glCompressedTexSubImage3D" ) );
            _glCompressedTexSubImage2D = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC >( loader.Invoke( "glCompressedTexSubImage2D" ) );
            _glCompressedTexSubImage1D = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC >( loader.Invoke( "glCompressedTexSubImage1D" ) );
            _glGetCompressedTexImage   = Marshal.GetDelegateForFunctionPointer< PFNGLGETCOMPRESSEDTEXIMAGEPROC >( loader.Invoke( "glGetCompressedTexImage" ) );
        }

        if ( _ogl14 || _ogl15
                    || _ogl20 || _ogl21
                    || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glBlendFuncSeparate = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDFUNCSEPARATEPROC >( loader.Invoke( "glBlendFuncSeparate" ) );
            _glMultiDrawArrays   = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWARRAYSPROC >( loader.Invoke( "glMultiDrawArrays" ) );
            _glMultiDrawElements = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWELEMENTSPROC >( loader.Invoke( "glMultiDrawElements" ) );
            _glPointParameterf   = Marshal.GetDelegateForFunctionPointer< PFNGLPOINTPARAMETERFPROC >( loader.Invoke( "glPointParameterf" ) );
            _glPointParameterfv  = Marshal.GetDelegateForFunctionPointer< PFNGLPOINTPARAMETERFVPROC >( loader.Invoke( "glPointParameterfv" ) );
            _glPointParameteri   = Marshal.GetDelegateForFunctionPointer< PFNGLPOINTPARAMETERIPROC >( loader.Invoke( "glPointParameteri" ) );
            _glPointParameteriv  = Marshal.GetDelegateForFunctionPointer< PFNGLPOINTPARAMETERIVPROC >( loader.Invoke( "glPointParameteriv" ) );
            _glBlendColor        = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDCOLORPROC >( loader.Invoke( "glBlendColor" ) );
            _glBlendEquation     = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDEQUATIONPROC >( loader.Invoke( "glBlendEquation" ) );
        }

        if ( _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glGenQueries           = Marshal.GetDelegateForFunctionPointer< PFNGLGENQUERIESPROC >( loader.Invoke( "glGenQueries" ) );
            _glDeleteQueries        = Marshal.GetDelegateForFunctionPointer< PFNGLDELETEQUERIESPROC >( loader.Invoke( "glDeleteQueries" ) );
            _glIsQuery              = Marshal.GetDelegateForFunctionPointer< PFNGLISQUERYPROC >( loader.Invoke( "glIsQuery" ) );
            _glBeginQuery           = Marshal.GetDelegateForFunctionPointer< PFNGLBEGINQUERYPROC >( loader.Invoke( "glBeginQuery" ) );
            _glEndQuery             = Marshal.GetDelegateForFunctionPointer< PFNGLENDQUERYPROC >( loader.Invoke( "glEndQuery" ) );
            _glGetQueryiv           = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYIVPROC >( loader.Invoke( "glGetQueryiv" ) );
            _glGetQueryObjectiv     = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYOBJECTIVPROC >( loader.Invoke( "glGetQueryObjectiv" ) );
            _glGetQueryObjectuiv    = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYOBJECTUIVPROC >( loader.Invoke( "glGetQueryObjectuiv" ) );
            _glBindBuffer           = Marshal.GetDelegateForFunctionPointer< PFNGLBINDBUFFERPROC >( loader.Invoke( "glBindBuffer" ) );
            _glDeleteBuffers        = Marshal.GetDelegateForFunctionPointer< PFNGLDELETEBUFFERSPROC >( loader.Invoke( "glDeleteBuffers" ) );
            _glGenBuffers           = Marshal.GetDelegateForFunctionPointer< PFNGLGENBUFFERSPROC >( loader.Invoke( "glGenBuffers" ) );
            _glIsBuffer             = Marshal.GetDelegateForFunctionPointer< PFNGLISBUFFERPROC >( loader.Invoke( "glIsBuffer" ) );
            _glBufferData           = Marshal.GetDelegateForFunctionPointer< PFNGLBUFFERDATAPROC >( loader.Invoke( "glBufferData" ) );
            _glBufferSubData        = Marshal.GetDelegateForFunctionPointer< PFNGLBUFFERSUBDATAPROC >( loader.Invoke( "glBufferSubData" ) );
            _glGetBufferSubData     = Marshal.GetDelegateForFunctionPointer< PFNGLGETBUFFERSUBDATAPROC >( loader.Invoke( "glGetBufferSubData" ) );
            _glMapBuffer            = Marshal.GetDelegateForFunctionPointer< PFNGLMAPBUFFERPROC >( loader.Invoke( "glMapBuffer" ) );
            _glUnmapBuffer          = Marshal.GetDelegateForFunctionPointer< PFNGLUNMAPBUFFERPROC >( loader.Invoke( "glUnmapBuffer" ) );
            _glGetBufferParameteriv = Marshal.GetDelegateForFunctionPointer< PFNGLGETBUFFERPARAMETERIVPROC >( loader.Invoke( "glGetBufferParameteriv" ) );
            _glGetBufferPointerv    = Marshal.GetDelegateForFunctionPointer< PFNGLGETBUFFERPOINTERVPROC >( loader.Invoke( "glGetBufferPointerv" ) );
        }

        if ( _ogl20 || _ogl21
                    || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glBlendEquationSeparate    = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDEQUATIONSEPARATEPROC >( loader.Invoke( "glBlendEquationSeparate" ) );
            _glDrawBuffers              = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWBUFFERSPROC >( loader.Invoke( "glDrawBuffers" ) );
            _glStencilOpSeparate        = Marshal.GetDelegateForFunctionPointer< PFNGLSTENCILOPSEPARATEPROC >( loader.Invoke( "glStencilOpSeparate" ) );
            _glStencilFuncSeparate      = Marshal.GetDelegateForFunctionPointer< PFNGLSTENCILFUNCSEPARATEPROC >( loader.Invoke( "glStencilFuncSeparate" ) );
            _glStencilMaskSeparate      = Marshal.GetDelegateForFunctionPointer< PFNGLSTENCILMASKSEPARATEPROC >( loader.Invoke( "glStencilMaskSeparate" ) );
            _glAttachShader             = Marshal.GetDelegateForFunctionPointer< PFNGLATTACHSHADERPROC >( loader.Invoke( "glAttachShader" ) );
            _glBindAttribLocation       = Marshal.GetDelegateForFunctionPointer< PFNGLBINDATTRIBLOCATIONPROC >( loader.Invoke( "glBindAttribLocation" ) );
            _glCompileShader            = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPILESHADERPROC >( loader.Invoke( "glCompileShader" ) );
            _glCreateProgram            = Marshal.GetDelegateForFunctionPointer< PFNGLCREATEPROGRAMPROC >( loader.Invoke( "glCreateProgram" ) );
            _glCreateShader             = Marshal.GetDelegateForFunctionPointer< PFNGLCREATESHADERPROC >( loader.Invoke( "glCreateShader" ) );
            _glDeleteProgram            = Marshal.GetDelegateForFunctionPointer< PFNGLDELETEPROGRAMPROC >( loader.Invoke( "glDeleteProgram" ) );
            _glDeleteShader             = Marshal.GetDelegateForFunctionPointer< PFNGLDELETESHADERPROC >( loader.Invoke( "glDeleteShader" ) );
            _glDetachShader             = Marshal.GetDelegateForFunctionPointer< PFNGLDETACHSHADERPROC >( loader.Invoke( "glDetachShader" ) );
            _glDisableVertexAttribArray = Marshal.GetDelegateForFunctionPointer< PFNGLDISABLEVERTEXATTRIBARRAYPROC >( loader.Invoke( "glDisableVertexAttribArray" ) );
            _glEnableVertexAttribArray  = Marshal.GetDelegateForFunctionPointer< PFNGLENABLEVERTEXATTRIBARRAYPROC >( loader.Invoke( "glEnableVertexAttribArray" ) );
            _glGetActiveAttrib          = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVEATTRIBPROC >( loader.Invoke( "glGetActiveAttrib" ) );
            _glGetActiveUniform         = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVEUNIFORMPROC >( loader.Invoke( "glGetActiveUniform" ) );
            _glGetAttachedShaders       = Marshal.GetDelegateForFunctionPointer< PFNGLGETATTACHEDSHADERSPROC >( loader.Invoke( "glGetAttachedShaders" ) );
            _glGetAttribLocation        = Marshal.GetDelegateForFunctionPointer< PFNGLGETATTRIBLOCATIONPROC >( loader.Invoke( "glGetAttribLocation" ) );
            _glGetProgramiv             = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMIVPROC >( loader.Invoke( "glGetProgramiv" ) );
            _glGetProgramInfoLog        = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMINFOLOGPROC >( loader.Invoke( "glGetProgramInfoLog" ) );
            _glGetShaderiv              = Marshal.GetDelegateForFunctionPointer< PFNGLGETSHADERIVPROC >( loader.Invoke( "glGetShaderiv" ) );
            _glGetShaderInfoLog         = Marshal.GetDelegateForFunctionPointer< PFNGLGETSHADERINFOLOGPROC >( loader.Invoke( "glGetShaderInfoLog" ) );
            _glGetShaderSource          = Marshal.GetDelegateForFunctionPointer< PFNGLGETSHADERSOURCEPROC >( loader.Invoke( "glGetShaderSource" ) );
            _glGetUniformLocation       = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMLOCATIONPROC >( loader.Invoke( "glGetUniformLocation" ) );
            _glGetUniformfv             = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMFVPROC >( loader.Invoke( "glGetUniformfv" ) );
            _glGetUniformiv             = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMIVPROC >( loader.Invoke( "glGetUniformiv" ) );
            _glGetVertexAttribdv        = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXATTRIBDVPROC >( loader.Invoke( "glGetVertexAttribdv" ) );
            _glGetVertexAttribfv        = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXATTRIBFVPROC >( loader.Invoke( "glGetVertexAttribfv" ) );
            _glGetVertexAttribiv        = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXATTRIBIVPROC >( loader.Invoke( "glGetVertexAttribiv" ) );
            _glGetVertexAttribPointerv  = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXATTRIBPOINTERVPROC >( loader.Invoke( "glGetVertexAttribPointerv" ) );
            _glIsProgram                = Marshal.GetDelegateForFunctionPointer< PFNGLISPROGRAMPROC >( loader.Invoke( "glIsProgram" ) );
            _glIsShader                 = Marshal.GetDelegateForFunctionPointer< PFNGLISSHADERPROC >( loader.Invoke( "glIsShader" ) );
            _glLinkProgram              = Marshal.GetDelegateForFunctionPointer< PFNGLLINKPROGRAMPROC >( loader.Invoke( "glLinkProgram" ) );
            _glShaderSource             = Marshal.GetDelegateForFunctionPointer< PFNGLSHADERSOURCEPROC >( loader.Invoke( "glShaderSource" ) );
            _glUseProgram               = Marshal.GetDelegateForFunctionPointer< PFNGLUSEPROGRAMPROC >( loader.Invoke( "glUseProgram" ) );
            _glUniform1f                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1FPROC >( loader.Invoke( "glUniform1f" ) );
            _glUniform2f                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2FPROC >( loader.Invoke( "glUniform2f" ) );
            _glUniform3f                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3FPROC >( loader.Invoke( "glUniform3f" ) );
            _glUniform4f                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4FPROC >( loader.Invoke( "glUniform4f" ) );
            _glUniform1i                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1IPROC >( loader.Invoke( "glUniform1i" ) );
            _glUniform2i                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2IPROC >( loader.Invoke( "glUniform2i" ) );
            _glUniform3i                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3IPROC >( loader.Invoke( "glUniform3i" ) );
            _glUniform4i                = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4IPROC >( loader.Invoke( "glUniform4i" ) );
            _glUniform1fv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1FVPROC >( loader.Invoke( "glUniform1fv" ) );
            _glUniform2fv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2FVPROC >( loader.Invoke( "glUniform2fv" ) );
            _glUniform3fv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3FVPROC >( loader.Invoke( "glUniform3fv" ) );
            _glUniform4fv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4FVPROC >( loader.Invoke( "glUniform4fv" ) );
            _glUniform1iv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1IVPROC >( loader.Invoke( "glUniform1iv" ) );
            _glUniform2iv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2IVPROC >( loader.Invoke( "glUniform2iv" ) );
            _glUniform3iv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3IVPROC >( loader.Invoke( "glUniform3iv" ) );
            _glUniform4iv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4IVPROC >( loader.Invoke( "glUniform4iv" ) );
            _glUniformMatrix2fv         = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX2FVPROC >( loader.Invoke( "glUniformMatrix2fv" ) );
            _glUniformMatrix3fv         = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX3FVPROC >( loader.Invoke( "glUniformMatrix3fv" ) );
            _glUniformMatrix4fv         = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX4FVPROC >( loader.Invoke( "glUniformMatrix4fv" ) );
            _glValidateProgram          = Marshal.GetDelegateForFunctionPointer< PFNGLVALIDATEPROGRAMPROC >( loader.Invoke( "glValidateProgram" ) );
            _glVertexAttrib1d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB1DPROC >( loader.Invoke( "glVertexAttrib1d" ) );
            _glVertexAttrib1dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB1DVPROC >( loader.Invoke( "glVertexAttrib1dv" ) );
            _glVertexAttrib1f           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB1FPROC >( loader.Invoke( "glVertexAttrib1f" ) );
            _glVertexAttrib1fv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB1FVPROC >( loader.Invoke( "glVertexAttrib1fv" ) );
            _glVertexAttrib1s           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB1SPROC >( loader.Invoke( "glVertexAttrib1s" ) );
            _glVertexAttrib1sv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB1SVPROC >( loader.Invoke( "glVertexAttrib1sv" ) );
            _glVertexAttrib2d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB2DPROC >( loader.Invoke( "glVertexAttrib2d" ) );
            _glVertexAttrib2dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB2DVPROC >( loader.Invoke( "glVertexAttrib2dv" ) );
            _glVertexAttrib2f           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB2FPROC >( loader.Invoke( "glVertexAttrib2f" ) );
            _glVertexAttrib2fv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB2FVPROC >( loader.Invoke( "glVertexAttrib2fv" ) );
            _glVertexAttrib2s           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB2SPROC >( loader.Invoke( "glVertexAttrib2s" ) );
            _glVertexAttrib2sv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB2SVPROC >( loader.Invoke( "glVertexAttrib2sv" ) );
            _glVertexAttrib3d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB3DPROC >( loader.Invoke( "glVertexAttrib3d" ) );
            _glVertexAttrib3dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB3DVPROC >( loader.Invoke( "glVertexAttrib3dv" ) );
            _glVertexAttrib3f           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB3FPROC >( loader.Invoke( "glVertexAttrib3f" ) );
            _glVertexAttrib3fv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB3FVPROC >( loader.Invoke( "glVertexAttrib3fv" ) );
            _glVertexAttrib3s           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB3SPROC >( loader.Invoke( "glVertexAttrib3s" ) );
            _glVertexAttrib3sv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB3SVPROC >( loader.Invoke( "glVertexAttrib3sv" ) );
            _glVertexAttrib4Nbv         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4NBVPROC >( loader.Invoke( "glVertexAttrib4Nbv" ) );
            _glVertexAttrib4Niv         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4NIVPROC >( loader.Invoke( "glVertexAttrib4Niv" ) );
            _glVertexAttrib4Nsv         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4NSVPROC >( loader.Invoke( "glVertexAttrib4Nsv" ) );
            _glVertexAttrib4Nub         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4NUBPROC >( loader.Invoke( "glVertexAttrib4Nub" ) );
            _glVertexAttrib4Nubv        = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4NUBVPROC >( loader.Invoke( "glVertexAttrib4Nubv" ) );
            _glVertexAttrib4Nuiv        = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4NUIVPROC >( loader.Invoke( "glVertexAttrib4Nuiv" ) );
            _glVertexAttrib4Nusv        = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4NUSVPROC >( loader.Invoke( "glVertexAttrib4Nusv" ) );
            _glVertexAttrib4bv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4BVPROC >( loader.Invoke( "glVertexAttrib4bv" ) );
            _glVertexAttrib4d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4DPROC >( loader.Invoke( "glVertexAttrib4d" ) );
            _glVertexAttrib4dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4DVPROC >( loader.Invoke( "glVertexAttrib4dv" ) );
            _glVertexAttrib4f           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4FPROC >( loader.Invoke( "glVertexAttrib4f" ) );
            _glVertexAttrib4fv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4FVPROC >( loader.Invoke( "glVertexAttrib4fv" ) );
            _glVertexAttrib4iv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4IVPROC >( loader.Invoke( "glVertexAttrib4iv" ) );
            _glVertexAttrib4s           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4SPROC >( loader.Invoke( "glVertexAttrib4s" ) );
            _glVertexAttrib4sv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4SVPROC >( loader.Invoke( "glVertexAttrib4sv" ) );
            _glVertexAttrib4ubv         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4UBVPROC >( loader.Invoke( "glVertexAttrib4ubv" ) );
            _glVertexAttrib4uiv         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4UIVPROC >( loader.Invoke( "glVertexAttrib4uiv" ) );
            _glVertexAttrib4usv         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIB4USVPROC >( loader.Invoke( "glVertexAttrib4usv" ) );
            _glVertexAttribPointer      = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBPOINTERPROC >( loader.Invoke( "glVertexAttribPointer" ) );
        }

        if ( _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glUniformMatrix2x3fv = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX2X3FVPROC >( loader.Invoke( "glUniformMatrix2x3fv" ) );
            _glUniformMatrix3x2fv = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX3X2FVPROC >( loader.Invoke( "glUniformMatrix3x2fv" ) );
            _glUniformMatrix2x4fv = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX2X4FVPROC >( loader.Invoke( "glUniformMatrix2x4fv" ) );
            _glUniformMatrix4x2fv = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX4X2FVPROC >( loader.Invoke( "glUniformMatrix4x2fv" ) );
            _glUniformMatrix3x4fv = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX3X4FVPROC >( loader.Invoke( "glUniformMatrix3x4fv" ) );
            _glUniformMatrix4x3fv = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX4X3FVPROC >( loader.Invoke( "glUniformMatrix4x3fv" ) );
        }

        if ( _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glColorMaski                          = Marshal.GetDelegateForFunctionPointer< PFNGLCOLORMASKIPROC >( loader.Invoke( "glColorMaski" ) );
            _glGetBooleani_v                       = Marshal.GetDelegateForFunctionPointer< PFNGLGETBOOLEANI_VPROC >( loader.Invoke( "glGetBooleani_v" ) );
            _glGetIntegeri_v                       = Marshal.GetDelegateForFunctionPointer< PFNGLGETINTEGERI_VPROC >( loader.Invoke( "glGetIntegeri_v" ) );
            _glEnablei                             = Marshal.GetDelegateForFunctionPointer< PFNGLENABLEIPROC >( loader.Invoke( "glEnablei" ) );
            _glDisablei                            = Marshal.GetDelegateForFunctionPointer< PFNGLDISABLEIPROC >( loader.Invoke( "glDisablei" ) );
            _glIsEnabledi                          = Marshal.GetDelegateForFunctionPointer< PFNGLISENABLEDIPROC >( loader.Invoke( "glIsEnabledi" ) );
            _glBeginTransformFeedback              = Marshal.GetDelegateForFunctionPointer< PFNGLBEGINTRANSFORMFEEDBACKPROC >( loader.Invoke( "glBeginTransformFeedback" ) );
            _glEndTransformFeedback                = Marshal.GetDelegateForFunctionPointer< PFNGLENDTRANSFORMFEEDBACKPROC >( loader.Invoke( "glEndTransformFeedback" ) );
            _glBindBufferRange                     = Marshal.GetDelegateForFunctionPointer< PFNGLBINDBUFFERRANGEPROC >( loader.Invoke( "glBindBufferRange" ) );
            _glBindBufferBase                      = Marshal.GetDelegateForFunctionPointer< PFNGLBINDBUFFERBASEPROC >( loader.Invoke( "glBindBufferBase" ) );
            _glTransformFeedbackVaryings           = Marshal.GetDelegateForFunctionPointer< PFNGLTRANSFORMFEEDBACKVARYINGSPROC >( loader.Invoke( "glTransformFeedbackVaryings" ) );
            _glGetTransformFeedbackVarying         = Marshal.GetDelegateForFunctionPointer< PFNGLGETTRANSFORMFEEDBACKVARYINGPROC >( loader.Invoke( "glGetTransformFeedbackVarying" ) );
            _glClampColor                          = Marshal.GetDelegateForFunctionPointer< PFNGLCLAMPCOLORPROC >( loader.Invoke( "glClampColor" ) );
            _glBeginConditionalRender              = Marshal.GetDelegateForFunctionPointer< PFNGLBEGINCONDITIONALRENDERPROC >( loader.Invoke( "glBeginConditionalRender" ) );
            _glEndConditionalRender                = Marshal.GetDelegateForFunctionPointer< PFNGLENDCONDITIONALRENDERPROC >( loader.Invoke( "glEndConditionalRender" ) );
            _glVertexAttribIPointer                = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBIPOINTERPROC >( loader.Invoke( "glVertexAttribIPointer" ) );
            _glGetVertexAttribIiv                  = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXATTRIBIIVPROC >( loader.Invoke( "glGetVertexAttribIiv" ) );
            _glGetVertexAttribIuiv                 = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXATTRIBIUIVPROC >( loader.Invoke( "glGetVertexAttribIuiv" ) );
            _glVertexAttribI1i                     = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI1IPROC >( loader.Invoke( "glVertexAttribI1i" ) );
            _glVertexAttribI2i                     = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI2IPROC >( loader.Invoke( "glVertexAttribI2i" ) );
            _glVertexAttribI3i                     = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI3IPROC >( loader.Invoke( "glVertexAttribI3i" ) );
            _glVertexAttribI4i                     = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4IPROC >( loader.Invoke( "glVertexAttribI4i" ) );
            _glVertexAttribI1ui                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI1UIPROC >( loader.Invoke( "glVertexAttribI1ui" ) );
            _glVertexAttribI2ui                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI2UIPROC >( loader.Invoke( "glVertexAttribI2ui" ) );
            _glVertexAttribI3ui                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI3UIPROC >( loader.Invoke( "glVertexAttribI3ui" ) );
            _glVertexAttribI4ui                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4UIPROC >( loader.Invoke( "glVertexAttribI4ui" ) );
            _glVertexAttribI1iv                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI1IVPROC >( loader.Invoke( "glVertexAttribI1iv" ) );
            _glVertexAttribI2iv                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI2IVPROC >( loader.Invoke( "glVertexAttribI2iv" ) );
            _glVertexAttribI3iv                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI3IVPROC >( loader.Invoke( "glVertexAttribI3iv" ) );
            _glVertexAttribI4iv                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4IVPROC >( loader.Invoke( "glVertexAttribI4iv" ) );
            _glVertexAttribI1uiv                   = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI1UIVPROC >( loader.Invoke( "glVertexAttribI1uiv" ) );
            _glVertexAttribI2uiv                   = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI2UIVPROC >( loader.Invoke( "glVertexAttribI2uiv" ) );
            _glVertexAttribI3uiv                   = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI3UIVPROC >( loader.Invoke( "glVertexAttribI3uiv" ) );
            _glVertexAttribI4uiv                   = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4UIVPROC >( loader.Invoke( "glVertexAttribI4uiv" ) );
            _glVertexAttribI4bv                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4BVPROC >( loader.Invoke( "glVertexAttribI4bv" ) );
            _glVertexAttribI4sv                    = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4SVPROC >( loader.Invoke( "glVertexAttribI4sv" ) );
            _glVertexAttribI4ubv                   = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4UBVPROC >( loader.Invoke( "glVertexAttribI4ubv" ) );
            _glVertexAttribI4usv                   = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBI4USVPROC >( loader.Invoke( "glVertexAttribI4usv" ) );
            _glGetUniformuiv                       = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMUIVPROC >( loader.Invoke( "glGetUniformuiv" ) );
            _glBindFragDataLocation                = Marshal.GetDelegateForFunctionPointer< PFNGLBINDFRAGDATALOCATIONPROC >( loader.Invoke( "glBindFragDataLocation" ) );
            _glGetFragDataLocation                 = Marshal.GetDelegateForFunctionPointer< PFNGLGETFRAGDATALOCATIONPROC >( loader.Invoke( "glGetFragDataLocation" ) );
            _glUniform1ui                          = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1UIPROC >( loader.Invoke( "glUniform1ui" ) );
            _glUniform2ui                          = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2UIPROC >( loader.Invoke( "glUniform2ui" ) );
            _glUniform3ui                          = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3UIPROC >( loader.Invoke( "glUniform3ui" ) );
            _glUniform4ui                          = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4UIPROC >( loader.Invoke( "glUniform4ui" ) );
            _glUniform1uiv                         = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1UIVPROC >( loader.Invoke( "glUniform1uiv" ) );
            _glUniform2uiv                         = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2UIVPROC >( loader.Invoke( "glUniform2uiv" ) );
            _glUniform3uiv                         = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3UIVPROC >( loader.Invoke( "glUniform3uiv" ) );
            _glUniform4uiv                         = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4UIVPROC >( loader.Invoke( "glUniform4uiv" ) );
            _glTexParameterIiv                     = Marshal.GetDelegateForFunctionPointer< PFNGLTEXPARAMETERIIVPROC >( loader.Invoke( "glTexParameterIiv" ) );
            _glTexParameterIuiv                    = Marshal.GetDelegateForFunctionPointer< PFNGLTEXPARAMETERIUIVPROC >( loader.Invoke( "glTexParameterIuiv" ) );
            _glGetTexParameterIiv                  = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXPARAMETERIIVPROC >( loader.Invoke( "glGetTexParameterIiv" ) );
            _glGetTexParameterIuiv                 = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXPARAMETERIUIVPROC >( loader.Invoke( "glGetTexParameterIuiv" ) );
            _glClearBufferiv                       = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARBUFFERIVPROC >( loader.Invoke( "glClearBufferiv" ) );
            _glClearBufferuiv                      = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARBUFFERUIVPROC >( loader.Invoke( "glClearBufferuiv" ) );
            _glClearBufferfv                       = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARBUFFERFVPROC >( loader.Invoke( "glClearBufferfv" ) );
            _glClearBufferfi                       = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARBUFFERFIPROC >( loader.Invoke( "glClearBufferfi" ) );
            _glGetStringi                          = Marshal.GetDelegateForFunctionPointer< PFNGLGETSTRINGIPROC >( loader.Invoke( "glGetStringi" ) );
            _glIsRenderbuffer                      = Marshal.GetDelegateForFunctionPointer< PFNGLISRENDERBUFFERPROC >( loader.Invoke( "glIsRenderbuffer" ) );
            _glBindRenderbuffer                    = Marshal.GetDelegateForFunctionPointer< PFNGLBINDRENDERBUFFERPROC >( loader.Invoke( "glBindRenderbuffer" ) );
            _glDeleteRenderbuffers                 = Marshal.GetDelegateForFunctionPointer< PFNGLDELETERENDERBUFFERSPROC >( loader.Invoke( "glDeleteRenderbuffers" ) );
            _glGenRenderbuffers                    = Marshal.GetDelegateForFunctionPointer< PFNGLGENRENDERBUFFERSPROC >( loader.Invoke( "glGenRenderbuffers" ) );
            _glRenderbufferStorage                 = Marshal.GetDelegateForFunctionPointer< PFNGLRENDERBUFFERSTORAGEPROC >( loader.Invoke( "glRenderbufferStorage" ) );
            _glGetRenderbufferParameteriv          = Marshal.GetDelegateForFunctionPointer< PFNGLGETRENDERBUFFERPARAMETERIVPROC >( loader.Invoke( "glGetRenderbufferParameteriv" ) );
            _glIsFramebuffer                       = Marshal.GetDelegateForFunctionPointer< PFNGLISFRAMEBUFFERPROC >( loader.Invoke( "glIsFramebuffer" ) );
            _glBindFramebuffer                     = Marshal.GetDelegateForFunctionPointer< PFNGLBINDFRAMEBUFFERPROC >( loader.Invoke( "glBindFramebuffer" ) );
            _glDeleteFramebuffers                  = Marshal.GetDelegateForFunctionPointer< PFNGLDELETEFRAMEBUFFERSPROC >( loader.Invoke( "glDeleteFramebuffers" ) );
            _glGenFramebuffers                     = Marshal.GetDelegateForFunctionPointer< PFNGLGENFRAMEBUFFERSPROC >( loader.Invoke( "glGenFramebuffers" ) );
            _glCheckFramebufferStatus              = Marshal.GetDelegateForFunctionPointer< PFNGLCHECKFRAMEBUFFERSTATUSPROC >( loader.Invoke( "glCheckFramebufferStatus" ) );
            _glFramebufferTexture1D                = Marshal.GetDelegateForFunctionPointer< PFNGLFRAMEBUFFERTEXTURE1DPROC >( loader.Invoke( "glFramebufferTexture1D" ) );
            _glFramebufferTexture2D                = Marshal.GetDelegateForFunctionPointer< PFNGLFRAMEBUFFERTEXTURE2DPROC >( loader.Invoke( "glFramebufferTexture2D" ) );
            _glFramebufferTexture3D                = Marshal.GetDelegateForFunctionPointer< PFNGLFRAMEBUFFERTEXTURE3DPROC >( loader.Invoke( "glFramebufferTexture3D" ) );
            _glFramebufferRenderbuffer             = Marshal.GetDelegateForFunctionPointer< PFNGLFRAMEBUFFERRENDERBUFFERPROC >( loader.Invoke( "glFramebufferRenderbuffer" ) );
            _glGetFramebufferAttachmentParameteriv = Marshal.GetDelegateForFunctionPointer< PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC >( loader.Invoke( "glGetFramebufferAttachmentParameteriv" ) );
            _glGenerateMipmap                      = Marshal.GetDelegateForFunctionPointer< PFNGLGENERATEMIPMAPPROC >( loader.Invoke( "glGenerateMipmap" ) );
            _glBlitFramebuffer                     = Marshal.GetDelegateForFunctionPointer< PFNGLBLITFRAMEBUFFERPROC >( loader.Invoke( "glBlitFramebuffer" ) );
            _glRenderbufferStorageMultisample      = Marshal.GetDelegateForFunctionPointer< PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC >( loader.Invoke( "glRenderbufferStorageMultisample" ) );
            _glFramebufferTextureLayer             = Marshal.GetDelegateForFunctionPointer< PFNGLFRAMEBUFFERTEXTURELAYERPROC >( loader.Invoke( "glFramebufferTextureLayer" ) );
            _glMapBufferRange                      = Marshal.GetDelegateForFunctionPointer< PFNGLMAPBUFFERRANGEPROC >( loader.Invoke( "glMapBufferRange" ) );
            _glFlushMappedBufferRange              = Marshal.GetDelegateForFunctionPointer< PFNGLFLUSHMAPPEDBUFFERRANGEPROC >( loader.Invoke( "glFlushMappedBufferRange" ) );
            _glBindVertexArray                     = Marshal.GetDelegateForFunctionPointer< PFNGLBINDVERTEXARRAYPROC >( loader.Invoke( "glBindVertexArray" ) );
            _glDeleteVertexArrays                  = Marshal.GetDelegateForFunctionPointer< PFNGLDELETEVERTEXARRAYSPROC >( loader.Invoke( "glDeleteVertexArrays" ) );
            _glGenVertexArrays                     = Marshal.GetDelegateForFunctionPointer< PFNGLGENVERTEXARRAYSPROC >( loader.Invoke( "glGenVertexArrays" ) );
            _glIsVertexArray                       = Marshal.GetDelegateForFunctionPointer< PFNGLISVERTEXARRAYPROC >( loader.Invoke( "glIsVertexArray" ) );
        }

        if ( _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glDrawArraysInstanced       = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWARRAYSINSTANCEDPROC >( loader.Invoke( "glDrawArraysInstanced" ) );
            _glDrawElementsInstanced     = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWELEMENTSINSTANCEDPROC >( loader.Invoke( "glDrawElementsInstanced" ) );
            _glTexBuffer                 = Marshal.GetDelegateForFunctionPointer< PFNGLTEXBUFFERPROC >( loader.Invoke( "glTexBuffer" ) );
            _glPrimitiveRestartIndex     = Marshal.GetDelegateForFunctionPointer< PFNGLPRIMITIVERESTARTINDEXPROC >( loader.Invoke( "glPrimitiveRestartIndex" ) );
            _glCopyBufferSubData         = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYBUFFERSUBDATAPROC >( loader.Invoke( "glCopyBufferSubData" ) );
            _glGetUniformIndices         = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMINDICESPROC >( loader.Invoke( "glGetUniformIndices" ) );
            _glGetActiveUniformsiv       = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVEUNIFORMSIVPROC >( loader.Invoke( "glGetActiveUniformsiv" ) );
            _glGetActiveUniformName      = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVEUNIFORMNAMEPROC >( loader.Invoke( "glGetActiveUniformName" ) );
            _glGetUniformBlockIndex      = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMBLOCKINDEXPROC >( loader.Invoke( "glGetUniformBlockIndex" ) );
            _glGetActiveUniformBlockiv   = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVEUNIFORMBLOCKIVPROC >( loader.Invoke( "glGetActiveUniformBlockiv" ) );
            _glGetActiveUniformBlockName = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC >( loader.Invoke( "glGetActiveUniformBlockName" ) );
            _glUniformBlockBinding       = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMBLOCKBINDINGPROC >( loader.Invoke( "glUniformBlockBinding" ) );
        }

        if ( _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glDrawElementsBaseVertex          = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWELEMENTSBASEVERTEXPROC >( loader.Invoke( "glDrawElementsBaseVertex" ) );
            _glDrawRangeElementsBaseVertex     = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC >( loader.Invoke( "glDrawRangeElementsBaseVertex" ) );
            _glDrawElementsInstancedBaseVertex = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC >( loader.Invoke( "glDrawElementsInstancedBaseVertex" ) );
            _glMultiDrawElementsBaseVertex     = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC >( loader.Invoke( "glMultiDrawElementsBaseVertex" ) );
            _glProvokingVertex                 = Marshal.GetDelegateForFunctionPointer< PFNGLPROVOKINGVERTEXPROC >( loader.Invoke( "glProvokingVertex" ) );
            _glFenceSync                       = Marshal.GetDelegateForFunctionPointer< PFNGLFENCESYNCPROC >( loader.Invoke( "glFenceSync" ) );
            _glIsSync                          = Marshal.GetDelegateForFunctionPointer< PFNGLISSYNCPROC >( loader.Invoke( "glIsSync" ) );
            _glDeleteSync                      = Marshal.GetDelegateForFunctionPointer< PFNGLDELETESYNCPROC >( loader.Invoke( "glDeleteSync" ) );
            _glClientWaitSync                  = Marshal.GetDelegateForFunctionPointer< PFNGLCLIENTWAITSYNCPROC >( loader.Invoke( "glClientWaitSync" ) );
            _glWaitSync                        = Marshal.GetDelegateForFunctionPointer< PFNGLWAITSYNCPROC >( loader.Invoke( "glWaitSync" ) );
            _glGetInteger64v                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETINTEGER64VPROC >( loader.Invoke( "glGetInteger64v" ) );
            _glGetSynciv                       = Marshal.GetDelegateForFunctionPointer< PFNGLGETSYNCIVPROC >( loader.Invoke( "glGetSynciv" ) );
            _glGetInteger64i_v                 = Marshal.GetDelegateForFunctionPointer< PFNGLGETINTEGER64I_VPROC >( loader.Invoke( "glGetInteger64i_v" ) );
            _glGetBufferParameteri64v          = Marshal.GetDelegateForFunctionPointer< PFNGLGETBUFFERPARAMETERI64VPROC >( loader.Invoke( "glGetBufferParameteri64v" ) );
            _glFramebufferTexture              = Marshal.GetDelegateForFunctionPointer< PFNGLFRAMEBUFFERTEXTUREPROC >( loader.Invoke( "glFramebufferTexture" ) );
            _glTexImage2DMultisample           = Marshal.GetDelegateForFunctionPointer< PFNGLTEXIMAGE2DMULTISAMPLEPROC >( loader.Invoke( "glTexImage2DMultisample" ) );
            _glTexImage3DMultisample           = Marshal.GetDelegateForFunctionPointer< PFNGLTEXIMAGE3DMULTISAMPLEPROC >( loader.Invoke( "glTexImage3DMultisample" ) );
            _glGetMultisamplefv                = Marshal.GetDelegateForFunctionPointer< PFNGLGETMULTISAMPLEFVPROC >( loader.Invoke( "glGetMultisamplefv" ) );
            _glSampleMaski                     = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLEMASKIPROC >( loader.Invoke( "glSampleMaski" ) );
        }

        if ( _ogl33 || _ogl34
                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glBindFragDataLocationIndexed = Marshal.GetDelegateForFunctionPointer< PFNGLBINDFRAGDATALOCATIONINDEXEDPROC >( loader.Invoke( "glBindFragDataLocationIndexed" ) );
            _glGetFragDataIndex            = Marshal.GetDelegateForFunctionPointer< PFNGLGETFRAGDATAINDEXPROC >( loader.Invoke( "glGetFragDataIndex" ) );
            _glGenSamplers                 = Marshal.GetDelegateForFunctionPointer< PFNGLGENSAMPLERSPROC >( loader.Invoke( "glGenSamplers" ) );
            _glDeleteSamplers              = Marshal.GetDelegateForFunctionPointer< PFNGLDELETESAMPLERSPROC >( loader.Invoke( "glDeleteSamplers" ) );
            _glIsSampler                   = Marshal.GetDelegateForFunctionPointer< PFNGLISSAMPLERPROC >( loader.Invoke( "glIsSampler" ) );
            _glBindSampler                 = Marshal.GetDelegateForFunctionPointer< PFNGLBINDSAMPLERPROC >( loader.Invoke( "glBindSampler" ) );
            _glSamplerParameteri           = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLERPARAMETERIPROC >( loader.Invoke( "glSamplerParameteri" ) );
            _glSamplerParameteriv          = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLERPARAMETERIVPROC >( loader.Invoke( "glSamplerParameteriv" ) );
            _glSamplerParameterf           = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLERPARAMETERFPROC >( loader.Invoke( "glSamplerParameterf" ) );
            _glSamplerParameterfv          = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLERPARAMETERFVPROC >( loader.Invoke( "glSamplerParameterfv" ) );
            _glSamplerParameterIiv         = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLERPARAMETERIIVPROC >( loader.Invoke( "glSamplerParameterIiv" ) );
            _glSamplerParameterIuiv        = Marshal.GetDelegateForFunctionPointer< PFNGLSAMPLERPARAMETERIUIVPROC >( loader.Invoke( "glSamplerParameterIuiv" ) );
            _glGetSamplerParameteriv       = Marshal.GetDelegateForFunctionPointer< PFNGLGETSAMPLERPARAMETERIVPROC >( loader.Invoke( "glGetSamplerParameteriv" ) );
            _glGetSamplerParameterIiv      = Marshal.GetDelegateForFunctionPointer< PFNGLGETSAMPLERPARAMETERIIVPROC >( loader.Invoke( "glGetSamplerParameterIiv" ) );
            _glGetSamplerParameterfv       = Marshal.GetDelegateForFunctionPointer< PFNGLGETSAMPLERPARAMETERFVPROC >( loader.Invoke( "glGetSamplerParameterfv" ) );
            _glGetSamplerParameterIuiv     = Marshal.GetDelegateForFunctionPointer< PFNGLGETSAMPLERPARAMETERIUIVPROC >( loader.Invoke( "glGetSamplerParameterIuiv" ) );
            _glQueryCounter                = Marshal.GetDelegateForFunctionPointer< PFNGLQUERYCOUNTERPROC >( loader.Invoke( "glQueryCounter" ) );
            _glGetQueryObjecti64v          = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYOBJECTI64VPROC >( loader.Invoke( "glGetQueryObjecti64v" ) );
            _glGetQueryObjectui64v         = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYOBJECTUI64VPROC >( loader.Invoke( "glGetQueryObjectui64v" ) );
            _glVertexAttribDivisor         = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBDIVISORPROC >( loader.Invoke( "glVertexAttribDivisor" ) );
            _glVertexAttribP1ui            = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP1UIPROC >( loader.Invoke( "glVertexAttribP1ui" ) );
            _glVertexAttribP1uiv           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP1UIVPROC >( loader.Invoke( "glVertexAttribP1uiv" ) );
            _glVertexAttribP2ui            = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP2UIPROC >( loader.Invoke( "glVertexAttribP2ui" ) );
            _glVertexAttribP2uiv           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP2UIVPROC >( loader.Invoke( "glVertexAttribP2uiv" ) );
            _glVertexAttribP3ui            = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP3UIPROC >( loader.Invoke( "glVertexAttribP3ui" ) );
            _glVertexAttribP3uiv           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP3UIVPROC >( loader.Invoke( "glVertexAttribP3uiv" ) );
            _glVertexAttribP4ui            = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP4UIPROC >( loader.Invoke( "glVertexAttribP4ui" ) );
            _glVertexAttribP4uiv           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBP4UIVPROC >( loader.Invoke( "glVertexAttribP4uiv" ) );
        }

        if ( _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glMinSampleShading               = Marshal.GetDelegateForFunctionPointer< PFNGLMINSAMPLESHADINGPROC >( loader.Invoke( "glMinSampleShading" ) );
            _glBlendEquationi                 = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDEQUATIONIPROC >( loader.Invoke( "glBlendEquationi" ) );
            _glBlendEquationSeparatei         = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDEQUATIONSEPARATEIPROC >( loader.Invoke( "glBlendEquationSeparatei" ) );
            _glBlendFunci                     = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDFUNCIPROC >( loader.Invoke( "glBlendFunci" ) );
            _glBlendFuncSeparatei             = Marshal.GetDelegateForFunctionPointer< PFNGLBLENDFUNCSEPARATEIPROC >( loader.Invoke( "glBlendFuncSeparatei" ) );
            _glDrawArraysIndirect             = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWARRAYSINDIRECTPROC >( loader.Invoke( "glDrawArraysIndirect" ) );
            _glDrawElementsIndirect           = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWELEMENTSINDIRECTPROC >( loader.Invoke( "glDrawElementsIndirect" ) );
            _glUniform1d                      = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1DPROC >( loader.Invoke( "glUniform1d" ) );
            _glUniform2d                      = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2DPROC >( loader.Invoke( "glUniform2d" ) );
            _glUniform3d                      = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3DPROC >( loader.Invoke( "glUniform3d" ) );
            _glUniform4d                      = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4DPROC >( loader.Invoke( "glUniform4d" ) );
            _glUniform1dv                     = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM1DVPROC >( loader.Invoke( "glUniform1dv" ) );
            _glUniform2dv                     = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM2DVPROC >( loader.Invoke( "glUniform2dv" ) );
            _glUniform3dv                     = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM3DVPROC >( loader.Invoke( "glUniform3dv" ) );
            _glUniform4dv                     = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORM4DVPROC >( loader.Invoke( "glUniform4dv" ) );
            _glUniformMatrix2dv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX2DVPROC >( loader.Invoke( "glUniformMatrix2dv" ) );
            _glUniformMatrix3dv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX3DVPROC >( loader.Invoke( "glUniformMatrix3dv" ) );
            _glUniformMatrix4dv               = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX4DVPROC >( loader.Invoke( "glUniformMatrix4dv" ) );
            _glUniformMatrix2x3dv             = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX2X3DVPROC >( loader.Invoke( "glUniformMatrix2x3dv" ) );
            _glUniformMatrix2x4dv             = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX2X4DVPROC >( loader.Invoke( "glUniformMatrix2x4dv" ) );
            _glUniformMatrix3x2dv             = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX3X2DVPROC >( loader.Invoke( "glUniformMatrix3x2dv" ) );
            _glUniformMatrix3x4dv             = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX3X4DVPROC >( loader.Invoke( "glUniformMatrix3x4dv" ) );
            _glUniformMatrix4x2dv             = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX4X2DVPROC >( loader.Invoke( "glUniformMatrix4x2dv" ) );
            _glUniformMatrix4x3dv             = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMMATRIX4X3DVPROC >( loader.Invoke( "glUniformMatrix4x3dv" ) );
            _glGetUniformdv                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMDVPROC >( loader.Invoke( "glGetUniformdv" ) );
            _glGetSubroutineUniformLocation   = Marshal.GetDelegateForFunctionPointer< PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC >( loader.Invoke( "glGetSubroutineUniformLocation" ) );
            _glGetSubroutineIndex             = Marshal.GetDelegateForFunctionPointer< PFNGLGETSUBROUTINEINDEXPROC >( loader.Invoke( "glGetSubroutineIndex" ) );
            _glGetActiveSubroutineUniformiv   = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC >( loader.Invoke( "glGetActiveSubroutineUniformiv" ) );
            _glGetActiveSubroutineUniformName = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC >( loader.Invoke( "glGetActiveSubroutineUniformName" ) );
            _glGetActiveSubroutineName        = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVESUBROUTINENAMEPROC >( loader.Invoke( "glGetActiveSubroutineName" ) );
            _glUniformSubroutinesuiv          = Marshal.GetDelegateForFunctionPointer< PFNGLUNIFORMSUBROUTINESUIVPROC >( loader.Invoke( "glUniformSubroutinesuiv" ) );
            _glGetUniformSubroutineuiv        = Marshal.GetDelegateForFunctionPointer< PFNGLGETUNIFORMSUBROUTINEUIVPROC >( loader.Invoke( "glGetUniformSubroutineuiv" ) );
            _glGetProgramStageiv              = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMSTAGEIVPROC >( loader.Invoke( "glGetProgramStageiv" ) );
            _glPatchParameteri                = Marshal.GetDelegateForFunctionPointer< PFNGLPATCHPARAMETERIPROC >( loader.Invoke( "glPatchParameteri" ) );
            _glPatchParameterfv               = Marshal.GetDelegateForFunctionPointer< PFNGLPATCHPARAMETERFVPROC >( loader.Invoke( "glPatchParameterfv" ) );
            _glBindTransformFeedback          = Marshal.GetDelegateForFunctionPointer< PFNGLBINDTRANSFORMFEEDBACKPROC >( loader.Invoke( "glBindTransformFeedback" ) );
            _glDeleteTransformFeedbacks       = Marshal.GetDelegateForFunctionPointer< PFNGLDELETETRANSFORMFEEDBACKSPROC >( loader.Invoke( "glDeleteTransformFeedbacks" ) );
            _glGenTransformFeedbacks          = Marshal.GetDelegateForFunctionPointer< PFNGLGENTRANSFORMFEEDBACKSPROC >( loader.Invoke( "glGenTransformFeedbacks" ) );
            _glIsTransformFeedback            = Marshal.GetDelegateForFunctionPointer< PFNGLISTRANSFORMFEEDBACKPROC >( loader.Invoke( "glIsTransformFeedback" ) );
            _glPauseTransformFeedback         = Marshal.GetDelegateForFunctionPointer< PFNGLPAUSETRANSFORMFEEDBACKPROC >( loader.Invoke( "glPauseTransformFeedback" ) );
            _glResumeTransformFeedback        = Marshal.GetDelegateForFunctionPointer< PFNGLRESUMETRANSFORMFEEDBACKPROC >( loader.Invoke( "glResumeTransformFeedback" ) );
            _glDrawTransformFeedback          = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWTRANSFORMFEEDBACKPROC >( loader.Invoke( "glDrawTransformFeedback" ) );
            _glDrawTransformFeedbackStream    = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC >( loader.Invoke( "glDrawTransformFeedbackStream" ) );
            _glBeginQueryIndexed              = Marshal.GetDelegateForFunctionPointer< PFNGLBEGINQUERYINDEXEDPROC >( loader.Invoke( "glBeginQueryIndexed" ) );
            _glEndQueryIndexed                = Marshal.GetDelegateForFunctionPointer< PFNGLENDQUERYINDEXEDPROC >( loader.Invoke( "glEndQueryIndexed" ) );
            _glGetQueryIndexediv              = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYINDEXEDIVPROC >( loader.Invoke( "glGetQueryIndexediv" ) );
        }

        if ( _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glReleaseShaderCompiler     = Marshal.GetDelegateForFunctionPointer< PFNGLRELEASESHADERCOMPILERPROC >( loader.Invoke( "glReleaseShaderCompiler" ) );
            _glShaderBinary              = Marshal.GetDelegateForFunctionPointer< PFNGLSHADERBINARYPROC >( loader.Invoke( "glShaderBinary" ) );
            _glGetShaderPrecisionFormat  = Marshal.GetDelegateForFunctionPointer< PFNGLGETSHADERPRECISIONFORMATPROC >( loader.Invoke( "glGetShaderPrecisionFormat" ) );
            _glDepthRangef               = Marshal.GetDelegateForFunctionPointer< PFNGLDEPTHRANGEFPROC >( loader.Invoke( "glDepthRangef" ) );
            _glClearDepthf               = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARDEPTHFPROC >( loader.Invoke( "glClearDepthf" ) );
            _glGetProgramBinary          = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMBINARYPROC >( loader.Invoke( "glGetProgramBinary" ) );
            _glProgramBinary             = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMBINARYPROC >( loader.Invoke( "glProgramBinary" ) );
            _glProgramParameteri         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMPARAMETERIPROC >( loader.Invoke( "glProgramParameteri" ) );
            _glUseProgramStages          = Marshal.GetDelegateForFunctionPointer< PFNGLUSEPROGRAMSTAGESPROC >( loader.Invoke( "glUseProgramStages" ) );
            _glActiveShaderProgram       = Marshal.GetDelegateForFunctionPointer< PFNGLACTIVESHADERPROGRAMPROC >( loader.Invoke( "glActiveShaderProgram" ) );
            _glCreateShaderProgramv      = Marshal.GetDelegateForFunctionPointer< PFNGLCREATESHADERPROGRAMVPROC >( loader.Invoke( "glCreateShaderProgramv" ) );
            _glBindProgramPipeline       = Marshal.GetDelegateForFunctionPointer< PFNGLBINDPROGRAMPIPELINEPROC >( loader.Invoke( "glBindProgramPipeline" ) );
            _glDeleteProgramPipelines    = Marshal.GetDelegateForFunctionPointer< PFNGLDELETEPROGRAMPIPELINESPROC >( loader.Invoke( "glDeleteProgramPipelines" ) );
            _glGenProgramPipelines       = Marshal.GetDelegateForFunctionPointer< PFNGLGENPROGRAMPIPELINESPROC >( loader.Invoke( "glGenProgramPipelines" ) );
            _glIsProgramPipeline         = Marshal.GetDelegateForFunctionPointer< PFNGLISPROGRAMPIPELINEPROC >( loader.Invoke( "glIsProgramPipeline" ) );
            _glGetProgramPipelineiv      = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMPIPELINEIVPROC >( loader.Invoke( "glGetProgramPipelineiv" ) );
            _glProgramUniform1i          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1IPROC >( loader.Invoke( "glProgramUniform1i" ) );
            _glProgramUniform1iv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1IVPROC >( loader.Invoke( "glProgramUniform1iv" ) );
            _glProgramUniform1f          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1FPROC >( loader.Invoke( "glProgramUniform1f" ) );
            _glProgramUniform1fv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1FVPROC >( loader.Invoke( "glProgramUniform1fv" ) );
            _glProgramUniform1d          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1DPROC >( loader.Invoke( "glProgramUniform1d" ) );
            _glProgramUniform1dv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1DVPROC >( loader.Invoke( "glProgramUniform1dv" ) );
            _glProgramUniform1ui         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1UIPROC >( loader.Invoke( "glProgramUniform1ui" ) );
            _glProgramUniform1uiv        = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM1UIVPROC >( loader.Invoke( "glProgramUniform1uiv" ) );
            _glProgramUniform2i          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2IPROC >( loader.Invoke( "glProgramUniform2i" ) );
            _glProgramUniform2iv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2IVPROC >( loader.Invoke( "glProgramUniform2iv" ) );
            _glProgramUniform2f          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2FPROC >( loader.Invoke( "glProgramUniform2f" ) );
            _glProgramUniform2fv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2FVPROC >( loader.Invoke( "glProgramUniform2fv" ) );
            _glProgramUniform2d          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2DPROC >( loader.Invoke( "glProgramUniform2d" ) );
            _glProgramUniform2dv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2DVPROC >( loader.Invoke( "glProgramUniform2dv" ) );
            _glProgramUniform2ui         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2UIPROC >( loader.Invoke( "glProgramUniform2ui" ) );
            _glProgramUniform2uiv        = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM2UIVPROC >( loader.Invoke( "glProgramUniform2uiv" ) );
            _glProgramUniform3i          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3IPROC >( loader.Invoke( "glProgramUniform3i" ) );
            _glProgramUniform3iv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3IVPROC >( loader.Invoke( "glProgramUniform3iv" ) );
            _glProgramUniform3f          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3FPROC >( loader.Invoke( "glProgramUniform3f" ) );
            _glProgramUniform3fv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3FVPROC >( loader.Invoke( "glProgramUniform3fv" ) );
            _glProgramUniform3d          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3DPROC >( loader.Invoke( "glProgramUniform3d" ) );
            _glProgramUniform3dv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3DVPROC >( loader.Invoke( "glProgramUniform3dv" ) );
            _glProgramUniform3ui         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3UIPROC >( loader.Invoke( "glProgramUniform3ui" ) );
            _glProgramUniform3uiv        = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM3UIVPROC >( loader.Invoke( "glProgramUniform3uiv" ) );
            _glProgramUniform4i          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4IPROC >( loader.Invoke( "glProgramUniform4i" ) );
            _glProgramUniform4iv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4IVPROC >( loader.Invoke( "glProgramUniform4iv" ) );
            _glProgramUniform4f          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4FPROC >( loader.Invoke( "glProgramUniform4f" ) );
            _glProgramUniform4fv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4FVPROC >( loader.Invoke( "glProgramUniform4fv" ) );
            _glProgramUniform4d          = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4DPROC >( loader.Invoke( "glProgramUniform4d" ) );
            _glProgramUniform4dv         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4DVPROC >( loader.Invoke( "glProgramUniform4dv" ) );
            _glProgramUniform4ui         = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4UIPROC >( loader.Invoke( "glProgramUniform4ui" ) );
            _glProgramUniform4uiv        = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORM4UIVPROC >( loader.Invoke( "glProgramUniform4uiv" ) );
            _glProgramUniformMatrix2fv   = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX2FVPROC >( loader.Invoke( "glProgramUniformMatrix2fv" ) );
            _glProgramUniformMatrix3fv   = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX3FVPROC >( loader.Invoke( "glProgramUniformMatrix3fv" ) );
            _glProgramUniformMatrix4fv   = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX4FVPROC >( loader.Invoke( "glProgramUniformMatrix4fv" ) );
            _glProgramUniformMatrix2dv   = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX2DVPROC >( loader.Invoke( "glProgramUniformMatrix2dv" ) );
            _glProgramUniformMatrix3dv   = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX3DVPROC >( loader.Invoke( "glProgramUniformMatrix3dv" ) );
            _glProgramUniformMatrix4dv   = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX4DVPROC >( loader.Invoke( "glProgramUniformMatrix4dv" ) );
            _glProgramUniformMatrix2x3fv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC >( loader.Invoke( "glProgramUniformMatrix2x3fv" ) );
            _glProgramUniformMatrix3x2fv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC >( loader.Invoke( "glProgramUniformMatrix3x2fv" ) );
            _glProgramUniformMatrix2x4fv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC >( loader.Invoke( "glProgramUniformMatrix2x4fv" ) );
            _glProgramUniformMatrix4x2fv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC >( loader.Invoke( "glProgramUniformMatrix4x2fv" ) );
            _glProgramUniformMatrix3x4fv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC >( loader.Invoke( "glProgramUniformMatrix3x4fv" ) );
            _glProgramUniformMatrix4x3fv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC >( loader.Invoke( "glProgramUniformMatrix4x3fv" ) );
            _glProgramUniformMatrix2x3dv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC >( loader.Invoke( "glProgramUniformMatrix2x3dv" ) );
            _glProgramUniformMatrix3x2dv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC >( loader.Invoke( "glProgramUniformMatrix3x2dv" ) );
            _glProgramUniformMatrix2x4dv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC >( loader.Invoke( "glProgramUniformMatrix2x4dv" ) );
            _glProgramUniformMatrix4x2dv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC >( loader.Invoke( "glProgramUniformMatrix4x2dv" ) );
            _glProgramUniformMatrix3x4dv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC >( loader.Invoke( "glProgramUniformMatrix3x4dv" ) );
            _glProgramUniformMatrix4x3dv = Marshal.GetDelegateForFunctionPointer< PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC >( loader.Invoke( "glProgramUniformMatrix4x3dv" ) );
            _glValidateProgramPipeline   = Marshal.GetDelegateForFunctionPointer< PFNGLVALIDATEPROGRAMPIPELINEPROC >( loader.Invoke( "glValidateProgramPipeline" ) );
            _glGetProgramPipelineInfoLog = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMPIPELINEINFOLOGPROC >( loader.Invoke( "glGetProgramPipelineInfoLog" ) );
            _glVertexAttribL1d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL1DPROC >( loader.Invoke( "glVertexAttribL1d" ) );
            _glVertexAttribL2d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL2DPROC >( loader.Invoke( "glVertexAttribL2d" ) );
            _glVertexAttribL3d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL3DPROC >( loader.Invoke( "glVertexAttribL3d" ) );
            _glVertexAttribL4d           = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL4DPROC >( loader.Invoke( "glVertexAttribL4d" ) );
            _glVertexAttribL1dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL1DVPROC >( loader.Invoke( "glVertexAttribL1dv" ) );
            _glVertexAttribL2dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL2DVPROC >( loader.Invoke( "glVertexAttribL2dv" ) );
            _glVertexAttribL3dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL3DVPROC >( loader.Invoke( "glVertexAttribL3dv" ) );
            _glVertexAttribL4dv          = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBL4DVPROC >( loader.Invoke( "glVertexAttribL4dv" ) );
            _glVertexAttribLPointer      = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBLPOINTERPROC >( loader.Invoke( "glVertexAttribLPointer" ) );
            _glGetVertexAttribLdv        = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXATTRIBLDVPROC >( loader.Invoke( "glGetVertexAttribLdv" ) );
            _glViewportArrayv            = Marshal.GetDelegateForFunctionPointer< PFNGLVIEWPORTARRAYVPROC >( loader.Invoke( "glViewportArrayv" ) );
            _glViewportIndexedf          = Marshal.GetDelegateForFunctionPointer< PFNGLVIEWPORTINDEXEDFPROC >( loader.Invoke( "glViewportIndexedf" ) );
            _glViewportIndexedfv         = Marshal.GetDelegateForFunctionPointer< PFNGLVIEWPORTINDEXEDFVPROC >( loader.Invoke( "glViewportIndexedfv" ) );
            _glScissorArrayv             = Marshal.GetDelegateForFunctionPointer< PFNGLSCISSORARRAYVPROC >( loader.Invoke( "glScissorArrayv" ) );
            _glScissorIndexed            = Marshal.GetDelegateForFunctionPointer< PFNGLSCISSORINDEXEDPROC >( loader.Invoke( "glScissorIndexed" ) );
            _glScissorIndexedv           = Marshal.GetDelegateForFunctionPointer< PFNGLSCISSORINDEXEDVPROC >( loader.Invoke( "glScissorIndexedv" ) );
            _glDepthRangeArrayv          = Marshal.GetDelegateForFunctionPointer< PFNGLDEPTHRANGEARRAYVPROC >( loader.Invoke( "glDepthRangeArrayv" ) );
            _glDepthRangeIndexed         = Marshal.GetDelegateForFunctionPointer< PFNGLDEPTHRANGEINDEXEDPROC >( loader.Invoke( "glDepthRangeIndexed" ) );
            _glGetFloati_v               = Marshal.GetDelegateForFunctionPointer< PFNGLGETFLOATI_VPROC >( loader.Invoke( "glGetFloati_v" ) );
            _glGetDoublei_v              = Marshal.GetDelegateForFunctionPointer< PFNGLGETDOUBLEI_VPROC >( loader.Invoke( "glGetDoublei_v" ) );
        }

        if ( _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glDrawArraysInstancedBaseInstance             = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC >( loader.Invoke( "glDrawArraysInstancedBaseInstance" ) );
            _glDrawElementsInstancedBaseInstance           = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC >( loader.Invoke( "glDrawElementsInstancedBaseInstance" ) );
            _glDrawElementsInstancedBaseVertexBaseInstance = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC >( loader.Invoke( "glDrawElementsInstancedBaseVertexBaseInstance" ) );
            _glGetInternalformativ                         = Marshal.GetDelegateForFunctionPointer< PFNGLGETINTERNALFORMATIVPROC >( loader.Invoke( "glGetInternalformativ" ) );
            _glGetActiveAtomicCounterBufferiv              = Marshal.GetDelegateForFunctionPointer< PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC >( loader.Invoke( "glGetActiveAtomicCounterBufferiv" ) );
            _glBindImageTexture                            = Marshal.GetDelegateForFunctionPointer< PFNGLBINDIMAGETEXTUREPROC >( loader.Invoke( "glBindImageTexture" ) );
            _glMemoryBarrier                               = Marshal.GetDelegateForFunctionPointer< PFNGLMEMORYBARRIERPROC >( loader.Invoke( "glMemoryBarrier" ) );
            _glTexStorage1D                                = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSTORAGE1DPROC >( loader.Invoke( "glTexStorage1D" ) );
            _glTexStorage2D                                = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSTORAGE2DPROC >( loader.Invoke( "glTexStorage2D" ) );
            _glTexStorage3D                                = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSTORAGE3DPROC >( loader.Invoke( "glTexStorage3D" ) );
            _glDrawTransformFeedbackInstanced              = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC >( loader.Invoke( "glDrawTransformFeedbackInstanced" ) );
            _glDrawTransformFeedbackStreamInstanced        = Marshal.GetDelegateForFunctionPointer< PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC >( loader.Invoke( "glDrawTransformFeedbackStreamInstanced" ) );
        }

        if ( _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            _glGetPointerv                     = Marshal.GetDelegateForFunctionPointer< PFNGLGETPOINTERVPROC >( loader.Invoke( "glGetPointerv" ) );
            _glClearBufferData                 = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARBUFFERDATAPROC >( loader.Invoke( "glClearBufferData" ) );
            _glClearBufferSubData              = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARBUFFERSUBDATAPROC >( loader.Invoke( "glClearBufferSubData" ) );
            _glDispatchCompute                 = Marshal.GetDelegateForFunctionPointer< PFNGLDISPATCHCOMPUTEPROC >( loader.Invoke( "glDispatchCompute" ) );
            _glDispatchComputeIndirect         = Marshal.GetDelegateForFunctionPointer< PFNGLDISPATCHCOMPUTEINDIRECTPROC >( loader.Invoke( "glDispatchComputeIndirect" ) );
            _glCopyImageSubData                = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYIMAGESUBDATAPROC >( loader.Invoke( "glCopyImageSubData" ) );
            _glFramebufferParameteri           = Marshal.GetDelegateForFunctionPointer< PFNGLFRAMEBUFFERPARAMETERIPROC >( loader.Invoke( "glFramebufferParameteri" ) );
            _glGetFramebufferParameteriv       = Marshal.GetDelegateForFunctionPointer< PFNGLGETFRAMEBUFFERPARAMETERIVPROC >( loader.Invoke( "glGetFramebufferParameteriv" ) );
            _glGetInternalformati64v           = Marshal.GetDelegateForFunctionPointer< PFNGLGETINTERNALFORMATI64VPROC >( loader.Invoke( "glGetInternalformati64v" ) );
            _glInvalidateTexSubImage           = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATETEXSUBIMAGEPROC >( loader.Invoke( "glInvalidateTexSubImage" ) );
            _glInvalidateTexImage              = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATETEXIMAGEPROC >( loader.Invoke( "glInvalidateTexImage" ) );
            _glInvalidateBufferSubData         = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATEBUFFERSUBDATAPROC >( loader.Invoke( "glInvalidateBufferSubData" ) );
            _glInvalidateBufferData            = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATEBUFFERDATAPROC >( loader.Invoke( "glInvalidateBufferData" ) );
            _glInvalidateFramebuffer           = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATEFRAMEBUFFERPROC >( loader.Invoke( "glInvalidateFramebuffer" ) );
            _glInvalidateSubFramebuffer        = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATESUBFRAMEBUFFERPROC >( loader.Invoke( "glInvalidateSubFramebuffer" ) );
            _glMultiDrawArraysIndirect         = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWARRAYSINDIRECTPROC >( loader.Invoke( "glMultiDrawArraysIndirect" ) );
            _glMultiDrawElementsIndirect       = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWELEMENTSINDIRECTPROC >( loader.Invoke( "glMultiDrawElementsIndirect" ) );
            _glGetProgramInterfaceiv           = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMINTERFACEIVPROC >( loader.Invoke( "glGetProgramInterfaceiv" ) );
            _glGetProgramResourceIndex         = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMRESOURCEINDEXPROC >( loader.Invoke( "glGetProgramResourceIndex" ) );
            _glGetProgramResourceName          = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMRESOURCENAMEPROC >( loader.Invoke( "glGetProgramResourceName" ) );
            _glGetProgramResourceiv            = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMRESOURCEIVPROC >( loader.Invoke( "glGetProgramResourceiv" ) );
            _glGetProgramResourceLocation      = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMRESOURCELOCATIONPROC >( loader.Invoke( "glGetProgramResourceLocation" ) );
            _glGetProgramResourceLocationIndex = Marshal.GetDelegateForFunctionPointer< PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC >( loader.Invoke( "glGetProgramResourceLocationIndex" ) );
            _glShaderStorageBlockBinding       = Marshal.GetDelegateForFunctionPointer< PFNGLSHADERSTORAGEBLOCKBINDINGPROC >( loader.Invoke( "glShaderStorageBlockBinding" ) );
            _glTexBufferRange                  = Marshal.GetDelegateForFunctionPointer< PFNGLTEXBUFFERRANGEPROC >( loader.Invoke( "glTexBufferRange" ) );
            _glTexStorage2DMultisample         = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSTORAGE2DMULTISAMPLEPROC >( loader.Invoke( "glTexStorage2DMultisample" ) );
            _glTexStorage3DMultisample         = Marshal.GetDelegateForFunctionPointer< PFNGLTEXSTORAGE3DMULTISAMPLEPROC >( loader.Invoke( "glTexStorage3DMultisample" ) );
            _glTextureView                     = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREVIEWPROC >( loader.Invoke( "glTextureView" ) );
            _glBindVertexBuffer                = Marshal.GetDelegateForFunctionPointer< PFNGLBINDVERTEXBUFFERPROC >( loader.Invoke( "glBindVertexBuffer" ) );
            _glVertexAttribFormat              = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBFORMATPROC >( loader.Invoke( "glVertexAttribFormat" ) );
            _glVertexAttribIFormat             = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBIFORMATPROC >( loader.Invoke( "glVertexAttribIFormat" ) );
            _glVertexAttribLFormat             = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBLFORMATPROC >( loader.Invoke( "glVertexAttribLFormat" ) );
            _glVertexAttribBinding             = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXATTRIBBINDINGPROC >( loader.Invoke( "glVertexAttribBinding" ) );
            _glVertexBindingDivisor            = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXBINDINGDIVISORPROC >( loader.Invoke( "glVertexBindingDivisor" ) );
            _glDebugMessageControl             = Marshal.GetDelegateForFunctionPointer< PFNGLDEBUGMESSAGECONTROLPROC >( loader.Invoke( "glDebugMessageControl" ) );
            _glDebugMessageInsert              = Marshal.GetDelegateForFunctionPointer< PFNGLDEBUGMESSAGEINSERTPROC >( loader.Invoke( "glDebugMessageInsert" ) );
            _glDebugMessageCallback            = Marshal.GetDelegateForFunctionPointer< PFNGLDEBUGMESSAGECALLBACKPROC >( loader.Invoke( "glDebugMessageCallback" ) );
            _glGetDebugMessageLog              = Marshal.GetDelegateForFunctionPointer< PFNGLGETDEBUGMESSAGELOGPROC >( loader.Invoke( "glGetDebugMessageLog" ) );
            _glPushDebugGroup                  = Marshal.GetDelegateForFunctionPointer< PFNGLPUSHDEBUGGROUPPROC >( loader.Invoke( "glPushDebugGroup" ) );
            _glPopDebugGroup                   = Marshal.GetDelegateForFunctionPointer< PFNGLPOPDEBUGGROUPPROC >( loader.Invoke( "glPopDebugGroup" ) );
            _glObjectLabel                     = Marshal.GetDelegateForFunctionPointer< PFNGLOBJECTLABELPROC >( loader.Invoke( "glObjectLabel" ) );
            _glGetObjectLabel                  = Marshal.GetDelegateForFunctionPointer< PFNGLGETOBJECTLABELPROC >( loader.Invoke( "glGetObjectLabel" ) );
            _glObjectPtrLabel                  = Marshal.GetDelegateForFunctionPointer< PFNGLOBJECTPTRLABELPROC >( loader.Invoke( "glObjectPtrLabel" ) );
            _glGetObjectPtrLabel               = Marshal.GetDelegateForFunctionPointer< PFNGLGETOBJECTPTRLABELPROC >( loader.Invoke( "glGetObjectPtrLabel" ) );
        }

        if ( _ogl44 || _ogl45 || _ogl46 )
        {
            _glBufferStorage     = Marshal.GetDelegateForFunctionPointer< PFNGLBUFFERSTORAGEPROC >( loader.Invoke( "glBufferStorage" ) );
            _glClearTexImage     = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARTEXIMAGEPROC >( loader.Invoke( "glClearTexImage" ) );
            _glClearTexSubImage  = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARTEXSUBIMAGEPROC >( loader.Invoke( "glClearTexSubImage" ) );
            _glBindBuffersBase   = Marshal.GetDelegateForFunctionPointer< PFNGLBINDBUFFERSBASEPROC >( loader.Invoke( "glBindBuffersBase" ) );
            _glBindBuffersRange  = Marshal.GetDelegateForFunctionPointer< PFNGLBINDBUFFERSRANGEPROC >( loader.Invoke( "glBindBuffersRange" ) );
            _glBindTextures      = Marshal.GetDelegateForFunctionPointer< PFNGLBINDTEXTURESPROC >( loader.Invoke( "glBindTextures" ) );
            _glBindSamplers      = Marshal.GetDelegateForFunctionPointer< PFNGLBINDSAMPLERSPROC >( loader.Invoke( "glBindSamplers" ) );
            _glBindImageTextures = Marshal.GetDelegateForFunctionPointer< PFNGLBINDIMAGETEXTURESPROC >( loader.Invoke( "glBindImageTextures" ) );
            _glBindVertexBuffers = Marshal.GetDelegateForFunctionPointer< PFNGLBINDVERTEXBUFFERSPROC >( loader.Invoke( "glBindVertexBuffers" ) );
        }

        if ( _ogl45 || _ogl46 )
        {
            _glClipControl                              = Marshal.GetDelegateForFunctionPointer< PFNGLCLIPCONTROLPROC >( loader.Invoke( "glClipControl" ) );
            _glCreateTransformFeedbacks                 = Marshal.GetDelegateForFunctionPointer< PFNGLCREATETRANSFORMFEEDBACKSPROC >( loader.Invoke( "glCreateTransformFeedbacks" ) );
            _glTransformFeedbackBufferBase              = Marshal.GetDelegateForFunctionPointer< PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC >( loader.Invoke( "glTransformFeedbackBufferBase" ) );
            _glTransformFeedbackBufferRange             = Marshal.GetDelegateForFunctionPointer< PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC >( loader.Invoke( "glTransformFeedbackBufferRange" ) );
            _glGetTransformFeedbackiv                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETTRANSFORMFEEDBACKIVPROC >( loader.Invoke( "glGetTransformFeedbackiv" ) );
            _glGetTransformFeedbacki_v                  = Marshal.GetDelegateForFunctionPointer< PFNGLGETTRANSFORMFEEDBACKI_VPROC >( loader.Invoke( "glGetTransformFeedbacki_v" ) );
            _glGetTransformFeedbacki64_v                = Marshal.GetDelegateForFunctionPointer< PFNGLGETTRANSFORMFEEDBACKI64_VPROC >( loader.Invoke( "glGetTransformFeedbacki64_v" ) );
            _glCreateBuffers                            = Marshal.GetDelegateForFunctionPointer< PFNGLCREATEBUFFERSPROC >( loader.Invoke( "glCreateBuffers" ) );
            _glNamedBufferStorage                       = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDBUFFERSTORAGEPROC >( loader.Invoke( "glNamedBufferStorage" ) );
            _glNamedBufferData                          = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDBUFFERDATAPROC >( loader.Invoke( "glNamedBufferData" ) );
            _glNamedBufferSubData                       = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDBUFFERSUBDATAPROC >( loader.Invoke( "glNamedBufferSubData" ) );
            _glCopyNamedBufferSubData                   = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYNAMEDBUFFERSUBDATAPROC >( loader.Invoke( "glCopyNamedBufferSubData" ) );
            _glClearNamedBufferData                     = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARNAMEDBUFFERDATAPROC >( loader.Invoke( "glClearNamedBufferData" ) );
            _glClearNamedBufferSubData                  = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARNAMEDBUFFERSUBDATAPROC >( loader.Invoke( "glClearNamedBufferSubData" ) );
            _glMapNamedBuffer                           = Marshal.GetDelegateForFunctionPointer< PFNGLMAPNAMEDBUFFERPROC >( loader.Invoke( "glMapNamedBuffer" ) );
            _glMapNamedBufferRange                      = Marshal.GetDelegateForFunctionPointer< PFNGLMAPNAMEDBUFFERRANGEPROC >( loader.Invoke( "glMapNamedBufferRange" ) );
            _glUnmapNamedBuffer                         = Marshal.GetDelegateForFunctionPointer< PFNGLUNMAPNAMEDBUFFERPROC >( loader.Invoke( "glUnmapNamedBuffer" ) );
            _glFlushMappedNamedBufferRange              = Marshal.GetDelegateForFunctionPointer< PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC >( loader.Invoke( "glFlushMappedNamedBufferRange" ) );
            _glGetNamedBufferParameteriv                = Marshal.GetDelegateForFunctionPointer< PFNGLGETNAMEDBUFFERPARAMETERIVPROC >( loader.Invoke( "glGetNamedBufferParameteriv" ) );
            _glGetNamedBufferParameteri64v              = Marshal.GetDelegateForFunctionPointer< PFNGLGETNAMEDBUFFERPARAMETERI64VPROC >( loader.Invoke( "glGetNamedBufferParameteri64v" ) );
            _glGetNamedBufferPointerv                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETNAMEDBUFFERPOINTERVPROC >( loader.Invoke( "glGetNamedBufferPointerv" ) );
            _glGetNamedBufferSubData                    = Marshal.GetDelegateForFunctionPointer< PFNGLGETNAMEDBUFFERSUBDATAPROC >( loader.Invoke( "glGetNamedBufferSubData" ) );
            _glCreateFramebuffers                       = Marshal.GetDelegateForFunctionPointer< PFNGLCREATEFRAMEBUFFERSPROC >( loader.Invoke( "glCreateFramebuffers" ) );
            _glNamedFramebufferRenderbuffer             = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC >( loader.Invoke( "glNamedFramebufferRenderbuffer" ) );
            _glNamedFramebufferParameteri               = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC >( loader.Invoke( "glNamedFramebufferParameteri" ) );
            _glNamedFramebufferTexture                  = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDFRAMEBUFFERTEXTUREPROC >( loader.Invoke( "glNamedFramebufferTexture" ) );
            _glNamedFramebufferTextureLayer             = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC >( loader.Invoke( "glNamedFramebufferTextureLayer" ) );
            _glNamedFramebufferDrawBuffer               = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC >( loader.Invoke( "glNamedFramebufferDrawBuffer" ) );
            _glNamedFramebufferDrawBuffers              = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC >( loader.Invoke( "glNamedFramebufferDrawBuffers" ) );
            _glNamedFramebufferReadBuffer               = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC >( loader.Invoke( "glNamedFramebufferReadBuffer" ) );
            _glInvalidateNamedFramebufferData           = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC >( loader.Invoke( "glInvalidateNamedFramebufferData" ) );
            _glInvalidateNamedFramebufferSubData        = Marshal.GetDelegateForFunctionPointer< PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC >( loader.Invoke( "glInvalidateNamedFramebufferSubData" ) );
            _glClearNamedFramebufferiv                  = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARNAMEDFRAMEBUFFERIVPROC >( loader.Invoke( "glClearNamedFramebufferiv" ) );
            _glClearNamedFramebufferuiv                 = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC >( loader.Invoke( "glClearNamedFramebufferuiv" ) );
            _glClearNamedFramebufferfv                  = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARNAMEDFRAMEBUFFERFVPROC >( loader.Invoke( "glClearNamedFramebufferfv" ) );
            _glClearNamedFramebufferfi                  = Marshal.GetDelegateForFunctionPointer< PFNGLCLEARNAMEDFRAMEBUFFERFIPROC >( loader.Invoke( "glClearNamedFramebufferfi" ) );
            _glBlitNamedFramebuffer                     = Marshal.GetDelegateForFunctionPointer< PFNGLBLITNAMEDFRAMEBUFFERPROC >( loader.Invoke( "glBlitNamedFramebuffer" ) );
            _glCheckNamedFramebufferStatus              = Marshal.GetDelegateForFunctionPointer< PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC >( loader.Invoke( "glCheckNamedFramebufferStatus" ) );
            _glGetNamedFramebufferParameteriv           = Marshal.GetDelegateForFunctionPointer< PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC >( loader.Invoke( "glGetNamedFramebufferParameteriv" ) );
            _glGetNamedFramebufferAttachmentParameteriv = Marshal.GetDelegateForFunctionPointer< PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC >( loader.Invoke( "glGetNamedFramebufferAttachmentParameteriv" ) );
            _glCreateRenderbuffers                      = Marshal.GetDelegateForFunctionPointer< PFNGLCREATERENDERBUFFERSPROC >( loader.Invoke( "glCreateRenderbuffers" ) );
            _glNamedRenderbufferStorage                 = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDRENDERBUFFERSTORAGEPROC >( loader.Invoke( "glNamedRenderbufferStorage" ) );
            _glNamedRenderbufferStorageMultisample      = Marshal.GetDelegateForFunctionPointer< PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC >( loader.Invoke( "glNamedRenderbufferStorageMultisample" ) );
            _glGetNamedRenderbufferParameteriv          = Marshal.GetDelegateForFunctionPointer< PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC >( loader.Invoke( "glGetNamedRenderbufferParameteriv" ) );
            _glCreateTextures                           = Marshal.GetDelegateForFunctionPointer< PFNGLCREATETEXTURESPROC >( loader.Invoke( "glCreateTextures" ) );
            _glTextureBuffer                            = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREBUFFERPROC >( loader.Invoke( "glTextureBuffer" ) );
            _glTextureBufferRange                       = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREBUFFERRANGEPROC >( loader.Invoke( "glTextureBufferRange" ) );
            _glTextureStorage1D                         = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESTORAGE1DPROC >( loader.Invoke( "glTextureStorage1D" ) );
            _glTextureStorage2D                         = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESTORAGE2DPROC >( loader.Invoke( "glTextureStorage2D" ) );
            _glTextureStorage3D                         = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESTORAGE3DPROC >( loader.Invoke( "glTextureStorage3D" ) );
            _glTextureStorage2DMultisample              = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC >( loader.Invoke( "glTextureStorage2DMultisample" ) );
            _glTextureStorage3DMultisample              = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC >( loader.Invoke( "glTextureStorage3DMultisample" ) );
            _glTextureSubImage1D                        = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESUBIMAGE1DPROC >( loader.Invoke( "glTextureSubImage1D" ) );
            _glTextureSubImage2D                        = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESUBIMAGE2DPROC >( loader.Invoke( "glTextureSubImage2D" ) );
            _glTextureSubImage3D                        = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTURESUBIMAGE3DPROC >( loader.Invoke( "glTextureSubImage3D" ) );
            _glCompressedTextureSubImage1D              = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC >( loader.Invoke( "glCompressedTextureSubImage1D" ) );
            _glCompressedTextureSubImage2D              = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC >( loader.Invoke( "glCompressedTextureSubImage2D" ) );
            _glCompressedTextureSubImage3D              = Marshal.GetDelegateForFunctionPointer< PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC >( loader.Invoke( "glCompressedTextureSubImage3D" ) );
            _glCopyTextureSubImage1D                    = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXTURESUBIMAGE1DPROC >( loader.Invoke( "glCopyTextureSubImage1D" ) );
            _glCopyTextureSubImage2D                    = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXTURESUBIMAGE2DPROC >( loader.Invoke( "glCopyTextureSubImage2D" ) );
            _glCopyTextureSubImage3D                    = Marshal.GetDelegateForFunctionPointer< PFNGLCOPYTEXTURESUBIMAGE3DPROC >( loader.Invoke( "glCopyTextureSubImage3D" ) );
            _glTextureParameterf                        = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREPARAMETERFPROC >( loader.Invoke( "glTextureParameterf" ) );
            _glTextureParameterfv                       = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREPARAMETERFVPROC >( loader.Invoke( "glTextureParameterfv" ) );
            _glTextureParameteri                        = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREPARAMETERIPROC >( loader.Invoke( "glTextureParameteri" ) );
            _glTextureParameterIiv                      = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREPARAMETERIIVPROC >( loader.Invoke( "glTextureParameterIiv" ) );
            _glTextureParameterIuiv                     = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREPARAMETERIUIVPROC >( loader.Invoke( "glTextureParameterIuiv" ) );
            _glTextureParameteriv                       = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREPARAMETERIVPROC >( loader.Invoke( "glTextureParameteriv" ) );
            _glGenerateTextureMipmap                    = Marshal.GetDelegateForFunctionPointer< PFNGLGENERATETEXTUREMIPMAPPROC >( loader.Invoke( "glGenerateTextureMipmap" ) );
            _glBindTextureUnit                          = Marshal.GetDelegateForFunctionPointer< PFNGLBINDTEXTUREUNITPROC >( loader.Invoke( "glBindTextureUnit" ) );
            _glGetTextureImage                          = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTUREIMAGEPROC >( loader.Invoke( "glGetTextureImage" ) );
            _glGetCompressedTextureImage                = Marshal.GetDelegateForFunctionPointer< PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC >( loader.Invoke( "glGetCompressedTextureImage" ) );
            _glGetTextureLevelParameterfv               = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTURELEVELPARAMETERFVPROC >( loader.Invoke( "glGetTextureLevelParameterfv" ) );
            _glGetTextureLevelParameteriv               = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTURELEVELPARAMETERIVPROC >( loader.Invoke( "glGetTextureLevelParameteriv" ) );
            _glGetTextureParameterfv                    = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTUREPARAMETERFVPROC >( loader.Invoke( "glGetTextureParameterfv" ) );
            _glGetTextureParameterIiv                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTUREPARAMETERIIVPROC >( loader.Invoke( "glGetTextureParameterIiv" ) );
            _glGetTextureParameterIuiv                  = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTUREPARAMETERIUIVPROC >( loader.Invoke( "glGetTextureParameterIuiv" ) );
            _glGetTextureParameteriv                    = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTUREPARAMETERIVPROC >( loader.Invoke( "glGetTextureParameteriv" ) );
            _glCreateVertexArrays                       = Marshal.GetDelegateForFunctionPointer< PFNGLCREATEVERTEXARRAYSPROC >( loader.Invoke( "glCreateVertexArrays" ) );
            _glDisableVertexArrayAttrib                 = Marshal.GetDelegateForFunctionPointer< PFNGLDISABLEVERTEXARRAYATTRIBPROC >( loader.Invoke( "glDisableVertexArrayAttrib" ) );
            _glEnableVertexArrayAttrib                  = Marshal.GetDelegateForFunctionPointer< PFNGLENABLEVERTEXARRAYATTRIBPROC >( loader.Invoke( "glEnableVertexArrayAttrib" ) );
            _glVertexArrayElementBuffer                 = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYELEMENTBUFFERPROC >( loader.Invoke( "glVertexArrayElementBuffer" ) );
            _glVertexArrayVertexBuffer                  = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYVERTEXBUFFERPROC >( loader.Invoke( "glVertexArrayVertexBuffer" ) );
            _glVertexArrayVertexBuffers                 = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYVERTEXBUFFERSPROC >( loader.Invoke( "glVertexArrayVertexBuffers" ) );
            _glVertexArrayAttribBinding                 = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYATTRIBBINDINGPROC >( loader.Invoke( "glVertexArrayAttribBinding" ) );
            _glVertexArrayAttribFormat                  = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYATTRIBFORMATPROC >( loader.Invoke( "glVertexArrayAttribFormat" ) );
            _glVertexArrayAttribIFormat                 = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYATTRIBIFORMATPROC >( loader.Invoke( "glVertexArrayAttribIFormat" ) );
            _glVertexArrayAttribLFormat                 = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYATTRIBLFORMATPROC >( loader.Invoke( "glVertexArrayAttribLFormat" ) );
            _glVertexArrayBindingDivisor                = Marshal.GetDelegateForFunctionPointer< PFNGLVERTEXARRAYBINDINGDIVISORPROC >( loader.Invoke( "glVertexArrayBindingDivisor" ) );
            _glGetVertexArrayiv                         = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXARRAYIVPROC >( loader.Invoke( "glGetVertexArrayiv" ) );
            _glGetVertexArrayIndexediv                  = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXARRAYINDEXEDIVPROC >( loader.Invoke( "glGetVertexArrayIndexediv" ) );
            _glGetVertexArrayIndexed64iv                = Marshal.GetDelegateForFunctionPointer< PFNGLGETVERTEXARRAYINDEXED64IVPROC >( loader.Invoke( "glGetVertexArrayIndexed64iv" ) );
            _glCreateSamplers                           = Marshal.GetDelegateForFunctionPointer< PFNGLCREATESAMPLERSPROC >( loader.Invoke( "glCreateSamplers" ) );
            _glCreateProgramPipelines                   = Marshal.GetDelegateForFunctionPointer< PFNGLCREATEPROGRAMPIPELINESPROC >( loader.Invoke( "glCreateProgramPipelines" ) );
            _glCreateQueries                            = Marshal.GetDelegateForFunctionPointer< PFNGLCREATEQUERIESPROC >( loader.Invoke( "glCreateQueries" ) );
            _glGetQueryBufferObjecti64v                 = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYBUFFEROBJECTI64VPROC >( loader.Invoke( "glGetQueryBufferObjecti64v" ) );
            _glGetQueryBufferObjectiv                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYBUFFEROBJECTIVPROC >( loader.Invoke( "glGetQueryBufferObjectiv" ) );
            _glGetQueryBufferObjectui64v                = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYBUFFEROBJECTUI64VPROC >( loader.Invoke( "glGetQueryBufferObjectui64v" ) );
            _glGetQueryBufferObjectuiv                  = Marshal.GetDelegateForFunctionPointer< PFNGLGETQUERYBUFFEROBJECTUIVPROC >( loader.Invoke( "glGetQueryBufferObjectuiv" ) );
            _glMemoryBarrierByRegion                    = Marshal.GetDelegateForFunctionPointer< PFNGLMEMORYBARRIERBYREGIONPROC >( loader.Invoke( "glMemoryBarrierByRegion" ) );
            _glGetTextureSubImage                       = Marshal.GetDelegateForFunctionPointer< PFNGLGETTEXTURESUBIMAGEPROC >( loader.Invoke( "glGetTextureSubImage" ) );
            _glGetCompressedTextureSubImage             = Marshal.GetDelegateForFunctionPointer< PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC >( loader.Invoke( "glGetCompressedTextureSubImage" ) );
            _glGetGraphicsResetStatus                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETGRAPHICSRESETSTATUSPROC >( loader.Invoke( "glGetGraphicsResetStatus" ) );
            _glGetnCompressedTexImage                   = Marshal.GetDelegateForFunctionPointer< PFNGLGETNCOMPRESSEDTEXIMAGEPROC >( loader.Invoke( "glGetnCompressedTexImage" ) );
            _glGetnUniformfv                            = Marshal.GetDelegateForFunctionPointer< PFNGLGETNUNIFORMFVPROC >( loader.Invoke( "glGetnUniformfv" ) );
            _glGetnUniformiv                            = Marshal.GetDelegateForFunctionPointer< PFNGLGETNUNIFORMIVPROC >( loader.Invoke( "glGetnUniformiv" ) );
            _glGetnUniformuiv                           = Marshal.GetDelegateForFunctionPointer< PFNGLGETNUNIFORMUIVPROC >( loader.Invoke( "glGetnUniformuiv" ) );
            _glReadnPixels                              = Marshal.GetDelegateForFunctionPointer< PFNGLREADNPIXELSPROC >( loader.Invoke( "glReadnPixels" ) );
            _glTextureBarrier                           = Marshal.GetDelegateForFunctionPointer< PFNGLTEXTUREBARRIERPROC >( loader.Invoke( "glTextureBarrier" ) );

//            _glGetnTexImage                             = Marshal.GetDelegateForFunctionPointer< PFNGLGETNTEXIMAGEPROC >( loader.Invoke( "glGetnTexImage" ) );
//            _glGetnUniformdv                            = Marshal.GetDelegateForFunctionPointer< PFNGLGETNUNIFORMDVPROC >( loader.Invoke( "glGetnUniformdv" ) );
        }

        if ( _ogl46 )
        {
            _glSpecializeShader   = Marshal.GetDelegateForFunctionPointer< PFNGLSPECIALIZESHADERPROC >( loader.Invoke( "glSpecializeShader" ) );
            _glPolygonOffsetClamp = Marshal.GetDelegateForFunctionPointer< PFNGLPOLYGONOFFSETCLAMPPROC >( loader.Invoke( "glPolygonOffsetClamp" ) );

//            _glMultiDrawArraysIndirectCount   = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC >( loader.Invoke( "glMultiDrawArraysIndirectCount" ) );
//            _glMultiDrawElementsIndirectCount = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC >( loader.Invoke( "glMultiDrawElementsIndirectCount" ) );
        }
    }
}

//#pragma warning restore IDE0079 // Remove unnecessary suppression
//#pragma warning restore CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
//#pragma warning restore CS8603  // Possible null reference return.
//#pragma warning restore IDE0060 // Remove unused parameter.
//#pragma warning restore IDE1006 // Naming Styles.
//#pragma warning restore IDE0090 // Use 'new(...)'.
//#pragma warning restore CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type
