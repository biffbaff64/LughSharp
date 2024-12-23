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

namespace Corelib.Lugh.Graphics.OpenGL;

// ============================================================================

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

    private unsafe void DetermineOGL()
    {
        Logger.Checkpoint();

        if ( Glfw.GetCurrentContext() == null )
        {
            Logger.Debug( "NULL Current OpenGL Context!" );
        }

        GetDelegateForFunction< glGetStringDelegate >( "glGetString", out _glGetString );

        var version      = BytePointerToString.Convert( GetString( IGL.GL_VERSION ) );
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
                Logger.Debug( $"Error: {GdxApi.Bindings.GetError()}" );

                throw new GdxRuntimeException( "Unable to determine OpenGL Version" );
            }
        }

        Logger.Debug( $"Language Version: {majorVersion}.{minorVersion}" );
    }

    // ========================================================================
    // ========================================================================

    public void Import()
    {
//        DetermineOGL();
//
//        if ( _ogl10 || _ogl11 || _ogl12 || _ogl13 || _ogl14 || _ogl15
//             || _ogl20 || _ogl21
//             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glCullFace", out _glCullFace );
//            GetDelegateForFunction( "glFrontFace", out _glFrontFace );
//            GetDelegateForFunction( "glHint", out _glHint );
//            GetDelegateForFunction( "glLineWidth", out _glLineWidth );
//            GetDelegateForFunction( "glPointSize", out _glPointSize );
//            GetDelegateForFunction( "glPolygonMode", out _glPolygonMode );
//            GetDelegateForFunction( "glScissor", out _glScissor );
//            GetDelegateForFunction( "glTexParameterf", out _glTexParameterf );
//            GetDelegateForFunction( "glTexParameterfv", out _glTexParameterfv );
//            GetDelegateForFunction( "glTexParameteri", out _glTexParameteri );
//            GetDelegateForFunction( "glTexParameteriv", out _glTexParameteriv );
//            GetDelegateForFunction( "glTexImage1D", out _glTexImage1D );
//            GetDelegateForFunction( "glTexImage2D", out _glTexImage2D );
//            GetDelegateForFunction( "glDrawBuffer", out _glDrawBuffer );
//            GetDelegateForFunction( "glClear", out _glClear );
//            GetDelegateForFunction( "glClearColor", out _glClearColor );
//            GetDelegateForFunction( "glClearStencil", out _glClearStencil );
//            GetDelegateForFunction( "glClearDepth", out _glClearDepth );
//            GetDelegateForFunction( "glStencilMask", out _glStencilMask );
//            GetDelegateForFunction( "glColorMask", out _glColorMask );
//            GetDelegateForFunction( "glDepthMask", out _glDepthMask );
//            GetDelegateForFunction( "glDisable", out _glDisable );
//            GetDelegateForFunction( "glEnable", out _glEnable );
//            GetDelegateForFunction( "glFinish", out _glFinish );
//            GetDelegateForFunction( "glFlush", out _glFlush );
//            GetDelegateForFunction( "glBlendFunc", out _glBlendFunc );
//            GetDelegateForFunction( "glGetString", out _glGetString );
//            GetDelegateForFunction( "glLogicOp", out _glLogicOp );
//            GetDelegateForFunction( "glStencilFunc", out _glStencilFunc );
//            GetDelegateForFunction( "glStencilOp", out _glStencilOp );
//            GetDelegateForFunction( "glDepthFunc", out _glDepthFunc );
//            GetDelegateForFunction( "glPixelStoref", out _glPixelStoref );
//            GetDelegateForFunction( "glPixelStorei", out _glPixelStorei );
//            GetDelegateForFunction( "glReadBuffer", out _glReadBuffer );
//            GetDelegateForFunction( "glReadPixels", out _glReadPixels );
//            GetDelegateForFunction( "glGetBooleanv", out _glGetBooleanv );
//            GetDelegateForFunction( "glGetDoublev", out _glGetDoublev );
//            GetDelegateForFunction( "glGetError", out _glGetError );
//            GetDelegateForFunction( "glGetFloatv", out _glGetFloatv );
//            GetDelegateForFunction( "glGetIntegerv", out _glGetIntegerv );
//            GetDelegateForFunction( "glGetTexImage", out _glGetTexImage );
//            GetDelegateForFunction( "glGetTexParameterfv", out _glGetTexParameterfv );
//            GetDelegateForFunction( "glGetTexParameteriv", out _glGetTexParameteriv );
//            GetDelegateForFunction( "glGetTexLevelParameterfv", out _glGetTexLevelParameterfv );
//            GetDelegateForFunction( "glGetTexLevelParameteriv", out _glGetTexLevelParameteriv );
//            GetDelegateForFunction( "glIsEnabled", out _glIsEnabled );
//            GetDelegateForFunction( "glDepthRange", out _glDepthRange );
//            GetDelegateForFunction( "glViewport", out _glViewport );
//        }
//
//        if ( _ogl11 || _ogl12 || _ogl13 || _ogl14 || _ogl15
//             || _ogl20 || _ogl21
//             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glDrawArrays", out _glDrawArrays );
//            GetDelegateForFunction( "glDrawElements", out _glDrawElements );
//            GetDelegateForFunction( "glPolygonOffset", out _glPolygonOffset );
//            GetDelegateForFunction( "glCopyTexImage1D", out _glCopyTexImage1D );
//            GetDelegateForFunction( "glCopyTexImage2D", out _glCopyTexImage2D );
//            GetDelegateForFunction( "glCopyTexSubImage1D", out _glCopyTexSubImage1D );
//            GetDelegateForFunction( "glCopyTexSubImage2D", out _glCopyTexSubImage2D );
//            GetDelegateForFunction( "glTexSubImage1D", out _glTexSubImage1D );
//            GetDelegateForFunction( "glTexSubImage2D", out _glTexSubImage2D );
//            GetDelegateForFunction( "glBindTexture", out _glBindTexture );
//            GetDelegateForFunction( "glDeleteTextures", out _glDeleteTextures );
//            GetDelegateForFunction( "glGenTextures", out _glGenTextures );
//            GetDelegateForFunction( "glIsTexture", out _glIsTexture );
//        }
//
//        if ( _ogl12 || _ogl13 || _ogl14 || _ogl15
//             || _ogl20 || _ogl21
//             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glDrawRangeElements", out _glDrawRangeElements );
//            GetDelegateForFunction( "glTexImage3D", out _glTexImage3D );
//            GetDelegateForFunction( "glTexSubImage3D", out _glTexSubImage3D );
//            GetDelegateForFunction( "glCopyTexSubImage3D", out _glCopyTexSubImage3D );
//        }
//
//        if ( _ogl13 || _ogl14 || _ogl15
//             || _ogl20 || _ogl21
//             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glActiveTexture", out _glActiveTexture );
//            GetDelegateForFunction( "glSampleCoverage", out _glSampleCoverage );
//            GetDelegateForFunction( "glCompressedTexImage3D", out _glCompressedTexImage3D );
//            GetDelegateForFunction( "glCompressedTexImage2D", out _glCompressedTexImage2D );
//            GetDelegateForFunction( "glCompressedTexImage1D", out _glCompressedTexImage1D );
//            GetDelegateForFunction( "glCompressedTexSubImage3D", out _glCompressedTexSubImage3D );
//            GetDelegateForFunction( "glCompressedTexSubImage2D", out _glCompressedTexSubImage2D );
//            GetDelegateForFunction( "glCompressedTexSubImage1D", out _glCompressedTexSubImage1D );
//            GetDelegateForFunction( "glGetCompressedTexImage", out _glGetCompressedTexImage );
//        }
//
//        if ( _ogl14 || _ogl15
//                    || _ogl20 || _ogl21
//                    || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glBlendFuncSeparate", out _glBlendFuncSeparate );
//            GetDelegateForFunction( "glMultiDrawArrays", out _glMultiDrawArrays );
//            GetDelegateForFunction( "glMultiDrawElements", out _glMultiDrawElements );
//            GetDelegateForFunction( "glPointParameterf", out _glPointParameterf );
//            GetDelegateForFunction( "glPointParameterfv", out _glPointParameterfv );
//            GetDelegateForFunction( "glPointParameteri", out _glPointParameteri );
//            GetDelegateForFunction( "glPointParameteriv", out _glPointParameteriv );
//            GetDelegateForFunction( "glBlendColor", out _glBlendColor );
//            GetDelegateForFunction( "glBlendEquation", out _glBlendEquation );
//        }
//
//        if ( _ogl15
//             || _ogl20 || _ogl21
//             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glGenQueries", out _glGenQueries );
//            GetDelegateForFunction( "glDeleteQueries", out _glDeleteQueries );
//            GetDelegateForFunction( "glIsQuery", out _glIsQuery );
//            GetDelegateForFunction( "glBeginQuery", out _glBeginQuery );
//            GetDelegateForFunction( "glEndQuery", out _glEndQuery );
//            GetDelegateForFunction( "glGetQueryiv", out _glGetQueryiv );
//            GetDelegateForFunction( "glGetQueryObjectiv", out _glGetQueryObjectiv );
//            GetDelegateForFunction( "glGetQueryObjectuiv", out _glGetQueryObjectuiv );
//            GetDelegateForFunction( "glBindBuffer", out _glBindBuffer );
//            GetDelegateForFunction( "glDeleteBuffers", out _glDeleteBuffers );
//            GetDelegateForFunction( "glGenBuffers", out _glGenBuffers );
//            GetDelegateForFunction( "glIsBuffer", out _glIsBuffer );
//            GetDelegateForFunction( "glBufferData", out _glBufferData );
//            GetDelegateForFunction( "glBufferSubData", out _glBufferSubData );
//            GetDelegateForFunction( "glGetBufferSubData", out _glGetBufferSubData );
//            GetDelegateForFunction( "glMapBuffer", out _glMapBuffer );
//            GetDelegateForFunction( "glUnmapBuffer", out _glUnmapBuffer );
//            GetDelegateForFunction( "glGetBufferParameteriv", out _glGetBufferParameteriv );
//            GetDelegateForFunction( "glGetBufferPointerv", out _glGetBufferPointerv );
//        }
//
//        if ( _ogl20 || _ogl21
//                    || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glBlendEquationSeparate", out _glBlendEquationSeparate );
//            GetDelegateForFunction( "glDrawBuffers", out _glDrawBuffers );
//            GetDelegateForFunction( "glStencilOpSeparate", out _glStencilOpSeparate );
//            GetDelegateForFunction( "glStencilFuncSeparate", out _glStencilFuncSeparate );
//            GetDelegateForFunction( "glStencilMaskSeparate", out _glStencilMaskSeparate );
//            GetDelegateForFunction( "glAttachShader", out _glAttachShader );
//            GetDelegateForFunction( "glBindAttribLocation", out _glBindAttribLocation );
//            GetDelegateForFunction( "glCompileShader", out _glCompileShader );
//            GetDelegateForFunction( "glCreateProgram", out _glCreateProgram );
//            GetDelegateForFunction( "glCreateShader", out _glCreateShader );
//            GetDelegateForFunction( "glDeleteProgram", out _glDeleteProgram );
//            GetDelegateForFunction( "glDeleteShader", out _glDeleteShader );
//            GetDelegateForFunction( "glDetachShader", out _glDetachShader );
//            GetDelegateForFunction( "glDisableVertexAttribArray", out _glDisableVertexAttribArray );
//            GetDelegateForFunction( "glEnableVertexAttribArray", out _glEnableVertexAttribArray );
//            GetDelegateForFunction( "glGetActiveAttrib", out _glGetActiveAttrib );
//            GetDelegateForFunction( "glGetActiveUniform", out _glGetActiveUniform );
//            GetDelegateForFunction( "glGetAttachedShaders", out _glGetAttachedShaders );
//            GetDelegateForFunction( "glGetAttribLocation", out _glGetAttribLocation );
//            GetDelegateForFunction( "glGetProgramiv", out _glGetProgramiv );
//            GetDelegateForFunction( "glGetProgramInfoLog", out _glGetProgramInfoLog );
//            GetDelegateForFunction( "glGetShaderiv", out _glGetShaderiv );
//            GetDelegateForFunction( "glGetShaderInfoLog", out _glGetShaderInfoLog );
//            GetDelegateForFunction( "glGetShaderSource", out _glGetShaderSource );
//            GetDelegateForFunction( "glGetUniformLocation", out _glGetUniformLocation );
//            GetDelegateForFunction( "glGetUniformfv", out _glGetUniformfv );
//            GetDelegateForFunction( "glGetUniformiv", out _glGetUniformiv );
//            GetDelegateForFunction( "glGetVertexAttribdv", out _glGetVertexAttribdv );
//            GetDelegateForFunction( "glGetVertexAttribfv", out _glGetVertexAttribfv );
//            GetDelegateForFunction( "glGetVertexAttribiv", out _glGetVertexAttribiv );
//            GetDelegateForFunction( "glGetVertexAttribPointerv", out _glGetVertexAttribPointerv );
//            GetDelegateForFunction( "glIsProgram", out _glIsProgram );
//            GetDelegateForFunction( "glIsShader", out _glIsShader );
//            GetDelegateForFunction( "glLinkProgram", out _glLinkProgram );
//            GetDelegateForFunction( "glShaderSource", out _glShaderSource );
//            GetDelegateForFunction( "glUseProgram", out _glUseProgram );
//            GetDelegateForFunction( "glUniform1f", out _glUniform1f );
//            GetDelegateForFunction( "glUniform2f", out _glUniform2f );
//            GetDelegateForFunction( "glUniform3f", out _glUniform3f );
//            GetDelegateForFunction( "glUniform4f", out _glUniform4f );
//            GetDelegateForFunction( "glUniform1i", out _glUniform1i );
//            GetDelegateForFunction( "glUniform2i", out _glUniform2i );
//            GetDelegateForFunction( "glUniform3i", out _glUniform3i );
//            GetDelegateForFunction( "glUniform4i", out _glUniform4i );
//            GetDelegateForFunction( "glUniform1fv", out _glUniform1fv );
//            GetDelegateForFunction( "glUniform2fv", out _glUniform2fv );
//            GetDelegateForFunction( "glUniform3fv", out _glUniform3fv );
//            GetDelegateForFunction( "glUniform4fv", out _glUniform4fv );
//            GetDelegateForFunction( "glUniform1iv", out _glUniform1iv );
//            GetDelegateForFunction( "glUniform2iv", out _glUniform2iv );
//            GetDelegateForFunction( "glUniform3iv", out _glUniform3iv );
//            GetDelegateForFunction( "glUniform4iv", out _glUniform4iv );
//            GetDelegateForFunction( "glUniformMatrix2fv", out _glUniformMatrix2fv );
//            GetDelegateForFunction( "glUniformMatrix3fv", out _glUniformMatrix3fv );
//            GetDelegateForFunction( "glUniformMatrix4fv", out _glUniformMatrix4fv );
//            GetDelegateForFunction( "glValidateProgram", out _glValidateProgram );
//            GetDelegateForFunction( "glVertexAttrib1d", out _glVertexAttrib1d );
//            GetDelegateForFunction( "glVertexAttrib1dv", out _glVertexAttrib1dv );
//            GetDelegateForFunction( "glVertexAttrib1f", out _glVertexAttrib1f );
//            GetDelegateForFunction( "glVertexAttrib1fv", out _glVertexAttrib1fv );
//            GetDelegateForFunction( "glVertexAttrib1s", out _glVertexAttrib1s );
//            GetDelegateForFunction( "glVertexAttrib1sv", out _glVertexAttrib1sv );
//            GetDelegateForFunction( "glVertexAttrib2d", out _glVertexAttrib2d );
//            GetDelegateForFunction( "glVertexAttrib2dv", out _glVertexAttrib2dv );
//            GetDelegateForFunction( "glVertexAttrib2f", out _glVertexAttrib2f );
//            GetDelegateForFunction( "glVertexAttrib2fv", out _glVertexAttrib2fv );
//            GetDelegateForFunction( "glVertexAttrib2s", out _glVertexAttrib2s );
//            GetDelegateForFunction( "glVertexAttrib2sv", out _glVertexAttrib2sv );
//            GetDelegateForFunction( "glVertexAttrib3d", out _glVertexAttrib3d );
//            GetDelegateForFunction( "glVertexAttrib3dv", out _glVertexAttrib3dv );
//            GetDelegateForFunction( "glVertexAttrib3f", out _glVertexAttrib3f );
//            GetDelegateForFunction( "glVertexAttrib3fv", out _glVertexAttrib3fv );
//            GetDelegateForFunction( "glVertexAttrib3s", out _glVertexAttrib3s );
//            GetDelegateForFunction( "glVertexAttrib3sv", out _glVertexAttrib3sv );
//            GetDelegateForFunction( "glVertexAttrib4Nbv", out _glVertexAttrib4Nbv );
//            GetDelegateForFunction( "glVertexAttrib4Niv", out _glVertexAttrib4Niv );
//            GetDelegateForFunction( "glVertexAttrib4Nsv", out _glVertexAttrib4Nsv );
//            GetDelegateForFunction( "glVertexAttrib4Nub", out _glVertexAttrib4Nub );
//            GetDelegateForFunction( "glVertexAttrib4Nubv", out _glVertexAttrib4Nubv );
//            GetDelegateForFunction( "glVertexAttrib4Nuiv", out _glVertexAttrib4Nuiv );
//            GetDelegateForFunction( "glVertexAttrib4Nusv", out _glVertexAttrib4Nusv );
//            GetDelegateForFunction( "glVertexAttrib4bv", out _glVertexAttrib4bv );
//            GetDelegateForFunction( "glVertexAttrib4d", out _glVertexAttrib4d );
//            GetDelegateForFunction( "glVertexAttrib4dv", out _glVertexAttrib4dv );
//            GetDelegateForFunction( "glVertexAttrib4f", out _glVertexAttrib4f );
//            GetDelegateForFunction( "glVertexAttrib4fv", out _glVertexAttrib4fv );
//            GetDelegateForFunction( "glVertexAttrib4iv", out _glVertexAttrib4iv );
//            GetDelegateForFunction( "glVertexAttrib4s", out _glVertexAttrib4s );
//            GetDelegateForFunction( "glVertexAttrib4sv", out _glVertexAttrib4sv );
//            GetDelegateForFunction( "glVertexAttrib4ubv", out _glVertexAttrib4ubv );
//            GetDelegateForFunction( "glVertexAttrib4uiv", out _glVertexAttrib4uiv );
//            GetDelegateForFunction( "glVertexAttrib4usv", out _glVertexAttrib4usv );
//            GetDelegateForFunction( "glVertexAttribPointer", out _glVertexAttribPointer );
//        }
//
//        if ( _ogl21
//             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glUniformMatrix2x3fv", out _glUniformMatrix2x3fv );
//            GetDelegateForFunction( "glUniformMatrix3x2fv", out _glUniformMatrix3x2fv );
//            GetDelegateForFunction( "glUniformMatrix2x4fv", out _glUniformMatrix2x4fv );
//            GetDelegateForFunction( "glUniformMatrix4x2fv", out _glUniformMatrix4x2fv );
//            GetDelegateForFunction( "glUniformMatrix3x4fv", out _glUniformMatrix3x4fv );
//            GetDelegateForFunction( "glUniformMatrix4x3fv", out _glUniformMatrix4x3fv );
//        }
//
//        if ( _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glColorMaski", out _glColorMaski );
//            GetDelegateForFunction( "glGetBooleani_v", out _glGetBooleani_v );
//            GetDelegateForFunction( "glGetIntegeri_v", out _glGetIntegeri_v );
//            GetDelegateForFunction( "glEnablei", out _glEnablei );
//            GetDelegateForFunction( "glDisablei", out _glDisablei );
//            GetDelegateForFunction( "glIsEnabledi", out _glIsEnabledi );
//            GetDelegateForFunction( "glBeginTransformFeedback", out _glBeginTransformFeedback );
//            GetDelegateForFunction( "glEndTransformFeedback", out _glEndTransformFeedback );
//            GetDelegateForFunction( "glBindBufferRange", out _glBindBufferRange );
//            GetDelegateForFunction( "glBindBufferBase", out _glBindBufferBase );
//            GetDelegateForFunction( "glTransformFeedbackVaryings", out _glTransformFeedbackVaryings );
//            GetDelegateForFunction( "glGetTransformFeedbackVarying", out _glGetTransformFeedbackVarying );
//            GetDelegateForFunction( "glClampColor", out _glClampColor );
//            GetDelegateForFunction( "glBeginConditionalRender", out _glBeginConditionalRender );
//            GetDelegateForFunction( "glEndConditionalRender", out _glEndConditionalRender );
//            GetDelegateForFunction( "glVertexAttribIPointer", out _glVertexAttribIPointer );
//            GetDelegateForFunction( "glGetVertexAttribIiv", out _glGetVertexAttribIiv );
//            GetDelegateForFunction( "glGetVertexAttribIuiv", out _glGetVertexAttribIuiv );
//            GetDelegateForFunction( "glVertexAttribI1i", out _glVertexAttribI1i );
//            GetDelegateForFunction( "glVertexAttribI2i", out _glVertexAttribI2i );
//            GetDelegateForFunction( "glVertexAttribI3i", out _glVertexAttribI3i );
//            GetDelegateForFunction( "glVertexAttribI4i", out _glVertexAttribI4i );
//            GetDelegateForFunction( "glVertexAttribI1ui", out _glVertexAttribI1ui );
//            GetDelegateForFunction( "glVertexAttribI2ui", out _glVertexAttribI2ui );
//            GetDelegateForFunction( "glVertexAttribI3ui", out _glVertexAttribI3ui );
//            GetDelegateForFunction( "glVertexAttribI4ui", out _glVertexAttribI4ui );
//            GetDelegateForFunction( "glVertexAttribI1iv", out _glVertexAttribI1iv );
//            GetDelegateForFunction( "glVertexAttribI2iv", out _glVertexAttribI2iv );
//            GetDelegateForFunction( "glVertexAttribI3iv", out _glVertexAttribI3iv );
//            GetDelegateForFunction( "glVertexAttribI4iv", out _glVertexAttribI4iv );
//            GetDelegateForFunction( "glVertexAttribI1uiv", out _glVertexAttribI1uiv );
//            GetDelegateForFunction( "glVertexAttribI2uiv", out _glVertexAttribI2uiv );
//            GetDelegateForFunction( "glVertexAttribI3uiv", out _glVertexAttribI3uiv );
//            GetDelegateForFunction( "glVertexAttribI4uiv", out _glVertexAttribI4uiv );
//            GetDelegateForFunction( "glVertexAttribI4bv", out _glVertexAttribI4bv );
//            GetDelegateForFunction( "glVertexAttribI4sv", out _glVertexAttribI4sv );
//            GetDelegateForFunction( "glVertexAttribI4ubv", out _glVertexAttribI4ubv );
//            GetDelegateForFunction( "glVertexAttribI4usv", out _glVertexAttribI4usv );
//            GetDelegateForFunction( "glGetUniformuiv", out _glGetUniformuiv );
//            GetDelegateForFunction( "glBindFragDataLocation", out _glBindFragDataLocation );
//            GetDelegateForFunction( "glGetFragDataLocation", out _glGetFragDataLocation );
//            GetDelegateForFunction( "glUniform1ui", out _glUniform1ui );
//            GetDelegateForFunction( "glUniform2ui", out _glUniform2ui );
//            GetDelegateForFunction( "glUniform3ui", out _glUniform3ui );
//            GetDelegateForFunction( "glUniform4ui", out _glUniform4ui );
//            GetDelegateForFunction( "glUniform1uiv", out _glUniform1uiv );
//            GetDelegateForFunction( "glUniform2uiv", out _glUniform2uiv );
//            GetDelegateForFunction( "glUniform3uiv", out _glUniform3uiv );
//            GetDelegateForFunction( "glUniform4uiv", out _glUniform4uiv );
//            GetDelegateForFunction( "glTexParameterIiv", out _glTexParameterIiv );
//            GetDelegateForFunction( "glTexParameterIuiv", out _glTexParameterIuiv );
//            GetDelegateForFunction( "glGetTexParameterIiv", out _glGetTexParameterIiv );
//            GetDelegateForFunction( "glGetTexParameterIuiv", out _glGetTexParameterIuiv );
//            GetDelegateForFunction( "glClearBufferiv", out _glClearBufferiv );
//            GetDelegateForFunction( "glClearBufferuiv", out _glClearBufferuiv );
//            GetDelegateForFunction( "glClearBufferfv", out _glClearBufferfv );
//            GetDelegateForFunction( "glClearBufferfi", out _glClearBufferfi );
//            GetDelegateForFunction( "glGetStringi", out _glGetStringi );
//            GetDelegateForFunction( "glIsRenderbuffer", out _glIsRenderbuffer );
//            GetDelegateForFunction( "glBindRenderbuffer", out _glBindRenderbuffer );
//            GetDelegateForFunction( "glDeleteRenderbuffers", out _glDeleteRenderbuffers );
//            GetDelegateForFunction( "glGenRenderbuffers", out _glGenRenderbuffers );
//            GetDelegateForFunction( "glRenderbufferStorage", out _glRenderbufferStorage );
//            GetDelegateForFunction( "glGetRenderbufferParameteriv", out _glGetRenderbufferParameteriv );
//            GetDelegateForFunction( "glIsFramebuffer", out _glIsFramebuffer );
//            GetDelegateForFunction( "glBindFramebuffer", out _glBindFramebuffer );
//            GetDelegateForFunction( "glDeleteFramebuffers", out _glDeleteFramebuffers );
//            GetDelegateForFunction( "glGenFramebuffers", out _glGenFramebuffers );
//            GetDelegateForFunction( "glCheckFramebufferStatus", out _glCheckFramebufferStatus );
//            GetDelegateForFunction( "glFramebufferTexture1D", out _glFramebufferTexture1D );
//            GetDelegateForFunction( "glFramebufferTexture2D", out _glFramebufferTexture2D );
//            GetDelegateForFunction( "glFramebufferTexture3D", out _glFramebufferTexture3D );
//            GetDelegateForFunction( "glFramebufferRenderbuffer", out _glFramebufferRenderbuffer );
//            GetDelegateForFunction( "glGetFramebufferAttachmentParameteriv", out _glGetFramebufferAttachmentParameteriv );
//            GetDelegateForFunction( "glGenerateMipmap", out _glGenerateMipmap );
//            GetDelegateForFunction( "glBlitFramebuffer", out _glBlitFramebuffer );
//            GetDelegateForFunction( "glRenderbufferStorageMultisample", out _glRenderbufferStorageMultisample );
//            GetDelegateForFunction( "glFramebufferTextureLayer", out _glFramebufferTextureLayer );
//            GetDelegateForFunction( "glMapBufferRange", out _glMapBufferRange );
//            GetDelegateForFunction( "glFlushMappedBufferRange", out _glFlushMappedBufferRange );
//            GetDelegateForFunction( "glBindVertexArray", out _glBindVertexArray );
//            GetDelegateForFunction( "glDeleteVertexArrays", out _glDeleteVertexArrays );
//            GetDelegateForFunction( "glGenVertexArrays", out _glGenVertexArrays );
//            GetDelegateForFunction( "glIsVertexArray", out _glIsVertexArray );
//        }
//
//        if ( _ogl31 || _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glDrawArraysInstanced", out _glDrawArraysInstanced );
//            GetDelegateForFunction( "glDrawElementsInstanced", out _glDrawElementsInstanced );
//            GetDelegateForFunction( "glTexBuffer", out _glTexBuffer );
//            GetDelegateForFunction( "glPrimitiveRestartIndex", out _glPrimitiveRestartIndex );
//            GetDelegateForFunction( "glCopyBufferSubData", out _glCopyBufferSubData );
//            GetDelegateForFunction( "glGetUniformIndices", out _glGetUniformIndices );
//            GetDelegateForFunction( "glGetActiveUniformsiv", out _glGetActiveUniformsiv );
//            GetDelegateForFunction( "glGetActiveUniformName", out _glGetActiveUniformName );
//            GetDelegateForFunction( "glGetUniformBlockIndex", out _glGetUniformBlockIndex );
//            GetDelegateForFunction( "glGetActiveUniformBlockiv", out _glGetActiveUniformBlockiv );
//            GetDelegateForFunction( "glGetActiveUniformBlockName", out _glGetActiveUniformBlockName );
//            GetDelegateForFunction( "glUniformBlockBinding", out _glUniformBlockBinding );
//        }
//
//        if ( _ogl32 || _ogl33 || _ogl34
//             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glDrawElementsBaseVertex", out _glDrawElementsBaseVertex );
//            GetDelegateForFunction( "glDrawRangeElementsBaseVertex", out _glDrawRangeElementsBaseVertex );
//            GetDelegateForFunction( "glDrawElementsInstancedBaseVertex", out _glDrawElementsInstancedBaseVertex );
//            GetDelegateForFunction( "glMultiDrawElementsBaseVertex", out _glMultiDrawElementsBaseVertex );
//            GetDelegateForFunction( "glProvokingVertex", out _glProvokingVertex );
//            GetDelegateForFunction( "glFenceSync", out _glFenceSync );
//            GetDelegateForFunction( "glIsSync", out _glIsSync );
//            GetDelegateForFunction( "glDeleteSync", out _glDeleteSync );
//            GetDelegateForFunction( "glClientWaitSync", out _glClientWaitSync );
//            GetDelegateForFunction( "glWaitSync", out _glWaitSync );
//            GetDelegateForFunction( "glGetInteger64v", out _glGetInteger64v );
//            GetDelegateForFunction( "glGetSynciv", out _glGetSynciv );
//            GetDelegateForFunction( "glGetInteger64i_v", out _glGetInteger64i_v );
//            GetDelegateForFunction( "glGetBufferParameteri64v", out _glGetBufferParameteri64v );
//            GetDelegateForFunction( "glFramebufferTexture", out _glFramebufferTexture );
//            GetDelegateForFunction( "glTexImage2DMultisample", out _glTexImage2DMultisample );
//            GetDelegateForFunction( "glTexImage3DMultisample", out _glTexImage3DMultisample );
//            GetDelegateForFunction( "glGetMultisamplefv", out _glGetMultisamplefv );
//            GetDelegateForFunction( "glSampleMaski", out _glSampleMaski );
//        }
//
//        if ( _ogl33 || _ogl34
//                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glBindFragDataLocationIndexed", out _glBindFragDataLocationIndexed );
//            GetDelegateForFunction( "glGetFragDataIndex", out _glGetFragDataIndex );
//            GetDelegateForFunction( "glGenSamplers", out _glGenSamplers );
//            GetDelegateForFunction( "glDeleteSamplers", out _glDeleteSamplers );
//            GetDelegateForFunction( "glIsSampler", out _glIsSampler );
//            GetDelegateForFunction( "glBindSampler", out _glBindSampler );
//            GetDelegateForFunction( "glSamplerParameteri", out _glSamplerParameteri );
//            GetDelegateForFunction( "glSamplerParameteriv", out _glSamplerParameteriv );
//            GetDelegateForFunction( "glSamplerParameterf", out _glSamplerParameterf );
//            GetDelegateForFunction( "glSamplerParameterfv", out _glSamplerParameterfv );
//            GetDelegateForFunction( "glSamplerParameterIiv", out _glSamplerParameterIiv );
//            GetDelegateForFunction( "glSamplerParameterIuiv", out _glSamplerParameterIuiv );
//            GetDelegateForFunction( "glGetSamplerParameteriv", out _glGetSamplerParameteriv );
//            GetDelegateForFunction( "glGetSamplerParameterIiv", out _glGetSamplerParameterIiv );
//            GetDelegateForFunction( "glGetSamplerParameterfv", out _glGetSamplerParameterfv );
//            GetDelegateForFunction( "glGetSamplerParameterIuiv", out _glGetSamplerParameterIuiv );
//            GetDelegateForFunction( "glQueryCounter", out _glQueryCounter );
//            GetDelegateForFunction( "glGetQueryObjecti64v", out _glGetQueryObjecti64v );
//            GetDelegateForFunction( "glGetQueryObjectui64v", out _glGetQueryObjectui64v );
//            GetDelegateForFunction( "glVertexAttribDivisor", out _glVertexAttribDivisor );
//            GetDelegateForFunction( "glVertexAttribP1ui", out _glVertexAttribP1ui );
//            GetDelegateForFunction( "glVertexAttribP1uiv", out _glVertexAttribP1uiv );
//            GetDelegateForFunction( "glVertexAttribP2ui", out _glVertexAttribP2ui );
//            GetDelegateForFunction( "glVertexAttribP2uiv", out _glVertexAttribP2uiv );
//            GetDelegateForFunction( "glVertexAttribP3ui", out _glVertexAttribP3ui );
//            GetDelegateForFunction( "glVertexAttribP3uiv", out _glVertexAttribP3uiv );
//            GetDelegateForFunction( "glVertexAttribP4ui", out _glVertexAttribP4ui );
//            GetDelegateForFunction( "glVertexAttribP4uiv", out _glVertexAttribP4uiv );
//        }
//
//        if ( _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glMinSampleShading", out _glMinSampleShading );
//            GetDelegateForFunction( "glBlendEquationi", out _glBlendEquationi );
//            GetDelegateForFunction( "glBlendEquationSeparatei", out _glBlendEquationSeparatei );
//            GetDelegateForFunction( "glBlendFunci", out _glBlendFunci );
//            GetDelegateForFunction( "glBlendFuncSeparatei", out _glBlendFuncSeparatei );
//            GetDelegateForFunction( "glDrawArraysIndirect", out _glDrawArraysIndirect );
//            GetDelegateForFunction( "glDrawElementsIndirect", out _glDrawElementsIndirect );
//            GetDelegateForFunction( "glUniform1d", out _glUniform1d );
//            GetDelegateForFunction( "glUniform2d", out _glUniform2d );
//            GetDelegateForFunction( "glUniform3d", out _glUniform3d );
//            GetDelegateForFunction( "glUniform4d", out _glUniform4d );
//            GetDelegateForFunction( "glUniform1dv", out _glUniform1dv );
//            GetDelegateForFunction( "glUniform2dv", out _glUniform2dv );
//            GetDelegateForFunction( "glUniform3dv", out _glUniform3dv );
//            GetDelegateForFunction( "glUniform4dv", out _glUniform4dv );
//            GetDelegateForFunction( "glUniformMatrix2dv", out _glUniformMatrix2dv );
//            GetDelegateForFunction( "glUniformMatrix3dv", out _glUniformMatrix3dv );
//            GetDelegateForFunction( "glUniformMatrix4dv", out _glUniformMatrix4dv );
//            GetDelegateForFunction( "glUniformMatrix2x3dv", out _glUniformMatrix2x3dv );
//            GetDelegateForFunction( "glUniformMatrix2x4dv", out _glUniformMatrix2x4dv );
//            GetDelegateForFunction( "glUniformMatrix3x2dv", out _glUniformMatrix3x2dv );
//            GetDelegateForFunction( "glUniformMatrix3x4dv", out _glUniformMatrix3x4dv );
//            GetDelegateForFunction( "glUniformMatrix4x2dv", out _glUniformMatrix4x2dv );
//            GetDelegateForFunction( "glUniformMatrix4x3dv", out _glUniformMatrix4x3dv );
//            GetDelegateForFunction( "glGetUniformdv", out _glGetUniformdv );
//            GetDelegateForFunction( "glGetSubroutineUniformLocation", out _glGetSubroutineUniformLocation );
//            GetDelegateForFunction( "glGetSubroutineIndex", out _glGetSubroutineIndex );
//            GetDelegateForFunction( "glGetActiveSubroutineUniformiv", out _glGetActiveSubroutineUniformiv );
//            GetDelegateForFunction( "glGetActiveSubroutineUniformName", out _glGetActiveSubroutineUniformName );
//            GetDelegateForFunction( "glGetActiveSubroutineName", out _glGetActiveSubroutineName );
//            GetDelegateForFunction( "glUniformSubroutinesuiv", out _glUniformSubroutinesuiv );
//            GetDelegateForFunction( "glGetUniformSubroutineuiv", out _glGetUniformSubroutineuiv );
//            GetDelegateForFunction( "glGetProgramStageiv", out _glGetProgramStageiv );
//            GetDelegateForFunction( "glPatchParameteri", out _glPatchParameteri );
//            GetDelegateForFunction( "glPatchParameterfv", out _glPatchParameterfv );
//            GetDelegateForFunction( "glBindTransformFeedback", out _glBindTransformFeedback );
//            GetDelegateForFunction( "glDeleteTransformFeedbacks", out _glDeleteTransformFeedbacks );
//            GetDelegateForFunction( "glGenTransformFeedbacks", out _glGenTransformFeedbacks );
//            GetDelegateForFunction( "glIsTransformFeedback", out _glIsTransformFeedback );
//            GetDelegateForFunction( "glPauseTransformFeedback", out _glPauseTransformFeedback );
//            GetDelegateForFunction( "glResumeTransformFeedback", out _glResumeTransformFeedback );
//            GetDelegateForFunction( "glDrawTransformFeedback", out _glDrawTransformFeedback );
//            GetDelegateForFunction( "glDrawTransformFeedbackStream", out _glDrawTransformFeedbackStream );
//            GetDelegateForFunction( "glBeginQueryIndexed", out _glBeginQueryIndexed );
//            GetDelegateForFunction( "glEndQueryIndexed", out _glEndQueryIndexed );
//            GetDelegateForFunction( "glGetQueryIndexediv", out _glGetQueryIndexediv );
//        }
//
//        if ( _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glReleaseShaderCompiler", out _glReleaseShaderCompiler );
//            GetDelegateForFunction( "glShaderBinary", out _glShaderBinary );
//            GetDelegateForFunction( "glGetShaderPrecisionFormat", out _glGetShaderPrecisionFormat );
//            GetDelegateForFunction( "glDepthRangef", out _glDepthRangef );
//            GetDelegateForFunction( "glClearDepthf", out _glClearDepthf );
//            GetDelegateForFunction( "glGetProgramBinary", out _glGetProgramBinary );
//            GetDelegateForFunction( "glProgramBinary", out _glProgramBinary );
//            GetDelegateForFunction( "glProgramParameteri", out _glProgramParameteri );
//            GetDelegateForFunction( "glUseProgramStages", out _glUseProgramStages );
//            GetDelegateForFunction( "glActiveShaderProgram", out _glActiveShaderProgram );
//            GetDelegateForFunction( "glCreateShaderProgramv", out _glCreateShaderProgramv );
//            GetDelegateForFunction( "glBindProgramPipeline", out _glBindProgramPipeline );
//            GetDelegateForFunction( "glDeleteProgramPipelines", out _glDeleteProgramPipelines );
//            GetDelegateForFunction( "glGenProgramPipelines", out _glGenProgramPipelines );
//            GetDelegateForFunction( "glIsProgramPipeline", out _glIsProgramPipeline );
//            GetDelegateForFunction( "glGetProgramPipelineiv", out _glGetProgramPipelineiv );
//            GetDelegateForFunction( "glProgramUniform1i", out _glProgramUniform1i );
//            GetDelegateForFunction( "glProgramUniform1iv", out _glProgramUniform1iv );
//            GetDelegateForFunction( "glProgramUniform1f", out _glProgramUniform1f );
//            GetDelegateForFunction( "glProgramUniform1fv", out _glProgramUniform1fv );
//            GetDelegateForFunction( "glProgramUniform1d", out _glProgramUniform1d );
//            GetDelegateForFunction( "glProgramUniform1dv", out _glProgramUniform1dv );
//            GetDelegateForFunction( "glProgramUniform1ui", out _glProgramUniform1ui );
//            GetDelegateForFunction( "glProgramUniform1uiv", out _glProgramUniform1uiv );
//            GetDelegateForFunction( "glProgramUniform2i", out _glProgramUniform2i );
//            GetDelegateForFunction( "glProgramUniform2iv", out _glProgramUniform2iv );
//            GetDelegateForFunction( "glProgramUniform2f", out _glProgramUniform2f );
//            GetDelegateForFunction( "glProgramUniform2fv", out _glProgramUniform2fv );
//            GetDelegateForFunction( "glProgramUniform2d", out _glProgramUniform2d );
//            GetDelegateForFunction( "glProgramUniform2dv", out _glProgramUniform2dv );
//            GetDelegateForFunction( "glProgramUniform2ui", out _glProgramUniform2ui );
//            GetDelegateForFunction( "glProgramUniform2uiv", out _glProgramUniform2uiv );
//            GetDelegateForFunction( "glProgramUniform3i", out _glProgramUniform3i );
//            GetDelegateForFunction( "glProgramUniform3iv", out _glProgramUniform3iv );
//            GetDelegateForFunction( "glProgramUniform3f", out _glProgramUniform3f );
//            GetDelegateForFunction( "glProgramUniform3fv", out _glProgramUniform3fv );
//            GetDelegateForFunction( "glProgramUniform3d", out _glProgramUniform3d );
//            GetDelegateForFunction( "glProgramUniform3dv", out _glProgramUniform3dv );
//            GetDelegateForFunction( "glProgramUniform3ui", out _glProgramUniform3ui );
//            GetDelegateForFunction( "glProgramUniform3uiv", out _glProgramUniform3uiv );
//            GetDelegateForFunction( "glProgramUniform4i", out _glProgramUniform4i );
//            GetDelegateForFunction( "glProgramUniform4iv", out _glProgramUniform4iv );
//            GetDelegateForFunction( "glProgramUniform4f", out _glProgramUniform4f );
//            GetDelegateForFunction( "glProgramUniform4fv", out _glProgramUniform4fv );
//            GetDelegateForFunction( "glProgramUniform4d", out _glProgramUniform4d );
//            GetDelegateForFunction( "glProgramUniform4dv", out _glProgramUniform4dv );
//            GetDelegateForFunction( "glProgramUniform4ui", out _glProgramUniform4ui );
//            GetDelegateForFunction( "glProgramUniform4uiv", out _glProgramUniform4uiv );
//            GetDelegateForFunction( "glProgramUniformMatrix2fv", out _glProgramUniformMatrix2fv );
//            GetDelegateForFunction( "glProgramUniformMatrix3fv", out _glProgramUniformMatrix3fv );
//            GetDelegateForFunction( "glProgramUniformMatrix4fv", out _glProgramUniformMatrix4fv );
//            GetDelegateForFunction( "glProgramUniformMatrix2dv", out _glProgramUniformMatrix2dv );
//            GetDelegateForFunction( "glProgramUniformMatrix3dv", out _glProgramUniformMatrix3dv );
//            GetDelegateForFunction( "glProgramUniformMatrix4dv", out _glProgramUniformMatrix4dv );
//            GetDelegateForFunction( "glProgramUniformMatrix2x3fv", out _glProgramUniformMatrix2x3fv );
//            GetDelegateForFunction( "glProgramUniformMatrix3x2fv", out _glProgramUniformMatrix3x2fv );
//            GetDelegateForFunction( "glProgramUniformMatrix2x4fv", out _glProgramUniformMatrix2x4fv );
//            GetDelegateForFunction( "glProgramUniformMatrix4x2fv", out _glProgramUniformMatrix4x2fv );
//            GetDelegateForFunction( "glProgramUniformMatrix3x4fv", out _glProgramUniformMatrix3x4fv );
//            GetDelegateForFunction( "glProgramUniformMatrix4x3fv", out _glProgramUniformMatrix4x3fv );
//            GetDelegateForFunction( "glProgramUniformMatrix2x3dv", out _glProgramUniformMatrix2x3dv );
//            GetDelegateForFunction( "glProgramUniformMatrix3x2dv", out _glProgramUniformMatrix3x2dv );
//            GetDelegateForFunction( "glProgramUniformMatrix2x4dv", out _glProgramUniformMatrix2x4dv );
//            GetDelegateForFunction( "glProgramUniformMatrix4x2dv", out _glProgramUniformMatrix4x2dv );
//            GetDelegateForFunction( "glProgramUniformMatrix3x4dv", out _glProgramUniformMatrix3x4dv );
//            GetDelegateForFunction( "glProgramUniformMatrix4x3dv", out _glProgramUniformMatrix4x3dv );
//            GetDelegateForFunction( "glValidateProgramPipeline", out _glValidateProgramPipeline );
//            GetDelegateForFunction( "glGetProgramPipelineInfoLog", out _glGetProgramPipelineInfoLog );
//            GetDelegateForFunction( "glVertexAttribL1d", out _glVertexAttribL1d );
//            GetDelegateForFunction( "glVertexAttribL2d", out _glVertexAttribL2d );
//            GetDelegateForFunction( "glVertexAttribL3d", out _glVertexAttribL3d );
//            GetDelegateForFunction( "glVertexAttribL4d", out _glVertexAttribL4d );
//            GetDelegateForFunction( "glVertexAttribL1dv", out _glVertexAttribL1dv );
//            GetDelegateForFunction( "glVertexAttribL2dv", out _glVertexAttribL2dv );
//            GetDelegateForFunction( "glVertexAttribL3dv", out _glVertexAttribL3dv );
//            GetDelegateForFunction( "glVertexAttribL4dv", out _glVertexAttribL4dv );
//            GetDelegateForFunction( "glVertexAttribLPointer", out _glVertexAttribLPointer );
//            GetDelegateForFunction( "glGetVertexAttribLdv", out _glGetVertexAttribLdv );
//            GetDelegateForFunction( "glViewportArrayv", out _glViewportArrayv );
//            GetDelegateForFunction( "glViewportIndexedf", out _glViewportIndexedf );
//            GetDelegateForFunction( "glViewportIndexedfv", out _glViewportIndexedfv );
//            GetDelegateForFunction( "glScissorArrayv", out _glScissorArrayv );
//            GetDelegateForFunction( "glScissorIndexed", out _glScissorIndexed );
//            GetDelegateForFunction( "glScissorIndexedv", out _glScissorIndexedv );
//            GetDelegateForFunction( "glDepthRangeArrayv", out _glDepthRangeArrayv );
//            GetDelegateForFunction( "glDepthRangeIndexed", out _glDepthRangeIndexed );
//            GetDelegateForFunction( "glGetFloati_v", out _glGetFloati_v );
//            GetDelegateForFunction( "glGetDoublei_v", out _glGetDoublei_v );
//        }
//
//        if ( _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glDrawArraysInstancedBaseInstance", out _glDrawArraysInstancedBaseInstance );
//            GetDelegateForFunction( "glDrawElementsInstancedBaseInstance", out _glDrawElementsInstancedBaseInstance );
//            GetDelegateForFunction( "glDrawElementsInstancedBaseVertexBaseInstance", out _glDrawElementsInstancedBaseVertexBaseInstance );
//            GetDelegateForFunction( "glGetInternalformativ", out _glGetInternalformativ );
//            GetDelegateForFunction( "glGetActiveAtomicCounterBufferiv", out _glGetActiveAtomicCounterBufferiv );
//            GetDelegateForFunction( "glBindImageTexture", out _glBindImageTexture );
//            GetDelegateForFunction( "glMemoryBarrier", out _glMemoryBarrier );
//            GetDelegateForFunction( "glTexStorage1D", out _glTexStorage1D );
//            GetDelegateForFunction( "glTexStorage2D", out _glTexStorage2D );
//            GetDelegateForFunction( "glTexStorage3D", out _glTexStorage3D );
//            GetDelegateForFunction( "glDrawTransformFeedbackInstanced", out _glDrawTransformFeedbackInstanced );
//            GetDelegateForFunction( "glDrawTransformFeedbackStreamInstanced", out _glDrawTransformFeedbackStreamInstanced );
//        }
//
//        if ( _ogl43 || _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glGetPointerv", out _glGetPointerv );
//            GetDelegateForFunction( "glClearBufferData", out _glClearBufferData );
//            GetDelegateForFunction( "glClearBufferSubData", out _glClearBufferSubData );
//            GetDelegateForFunction( "glDispatchCompute", out _glDispatchCompute );
//            GetDelegateForFunction( "glDispatchComputeIndirect", out _glDispatchComputeIndirect );
//            GetDelegateForFunction( "glCopyImageSubData", out _glCopyImageSubData );
//            GetDelegateForFunction( "glFramebufferParameteri", out _glFramebufferParameteri );
//            GetDelegateForFunction( "glGetFramebufferParameteriv", out _glGetFramebufferParameteriv );
//            GetDelegateForFunction( "glGetInternalformati64v", out _glGetInternalformati64v );
//            GetDelegateForFunction( "glInvalidateTexSubImage", out _glInvalidateTexSubImage );
//            GetDelegateForFunction( "glInvalidateTexImage", out _glInvalidateTexImage );
//            GetDelegateForFunction( "glInvalidateBufferSubData", out _glInvalidateBufferSubData );
//            GetDelegateForFunction( "glInvalidateBufferData", out _glInvalidateBufferData );
//            GetDelegateForFunction( "glInvalidateFramebuffer", out _glInvalidateFramebuffer );
//            GetDelegateForFunction( "glInvalidateSubFramebuffer", out _glInvalidateSubFramebuffer );
//            GetDelegateForFunction( "glMultiDrawArraysIndirect", out _glMultiDrawArraysIndirect );
//            GetDelegateForFunction( "glMultiDrawElementsIndirect", out _glMultiDrawElementsIndirect );
//            GetDelegateForFunction( "glGetProgramInterfaceiv", out _glGetProgramInterfaceiv );
//            GetDelegateForFunction( "glGetProgramResourceIndex", out _glGetProgramResourceIndex );
//            GetDelegateForFunction( "glGetProgramResourceName", out _glGetProgramResourceName );
//            GetDelegateForFunction( "glGetProgramResourceiv", out _glGetProgramResourceiv );
//            GetDelegateForFunction( "glGetProgramResourceLocation", out _glGetProgramResourceLocation );
//            GetDelegateForFunction( "glGetProgramResourceLocationIndex", out _glGetProgramResourceLocationIndex );
//            GetDelegateForFunction( "glShaderStorageBlockBinding", out _glShaderStorageBlockBinding );
//            GetDelegateForFunction( "glTexBufferRange", out _glTexBufferRange );
//            GetDelegateForFunction( "glTexStorage2DMultisample", out _glTexStorage2DMultisample );
//            GetDelegateForFunction( "glTexStorage3DMultisample", out _glTexStorage3DMultisample );
//            GetDelegateForFunction( "glTextureView", out _glTextureView );
//            GetDelegateForFunction( "glBindVertexBuffer", out _glBindVertexBuffer );
//            GetDelegateForFunction( "glVertexAttribFormat", out _glVertexAttribFormat );
//            GetDelegateForFunction( "glVertexAttribIFormat", out _glVertexAttribIFormat );
//            GetDelegateForFunction( "glVertexAttribLFormat", out _glVertexAttribLFormat );
//            GetDelegateForFunction( "glVertexAttribBinding", out _glVertexAttribBinding );
//            GetDelegateForFunction( "glVertexBindingDivisor", out _glVertexBindingDivisor );
//            GetDelegateForFunction( "glDebugMessageControl", out _glDebugMessageControl );
//            GetDelegateForFunction( "glDebugMessageInsert", out _glDebugMessageInsert );
//            GetDelegateForFunction( "glDebugMessageCallback", out _glDebugMessageCallback );
//            GetDelegateForFunction( "glGetDebugMessageLog", out _glGetDebugMessageLog );
//            GetDelegateForFunction( "glPushDebugGroup", out _glPushDebugGroup );
//            GetDelegateForFunction( "glPopDebugGroup", out _glPopDebugGroup );
//            GetDelegateForFunction( "glObjectLabel", out _glObjectLabel );
//            GetDelegateForFunction( "glGetObjectLabel", out _glGetObjectLabel );
//            GetDelegateForFunction( "glObjectPtrLabel", out _glObjectPtrLabel );
//            GetDelegateForFunction( "glGetObjectPtrLabel", out _glGetObjectPtrLabel );
//        }
//
//        if ( _ogl44 || _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glBufferStorage", out _glBufferStorage );
//            GetDelegateForFunction( "glClearTexImage", out _glClearTexImage );
//            GetDelegateForFunction( "glClearTexSubImage", out _glClearTexSubImage );
//            GetDelegateForFunction( "glBindBuffersBase", out _glBindBuffersBase );
//            GetDelegateForFunction( "glBindBuffersRange", out _glBindBuffersRange );
//            GetDelegateForFunction( "glBindTextures", out _glBindTextures );
//            GetDelegateForFunction( "glBindSamplers", out _glBindSamplers );
//            GetDelegateForFunction( "glBindImageTextures", out _glBindImageTextures );
//            GetDelegateForFunction( "glBindVertexBuffers", out _glBindVertexBuffers );
//        }
//
//        if ( _ogl45 || _ogl46 )
//        {
//            GetDelegateForFunction( "glClipControl", out _glClipControl );
//            GetDelegateForFunction( "glCreateTransformFeedbacks", out _glCreateTransformFeedbacks );
//            GetDelegateForFunction( "glTransformFeedbackBufferBase", out _glTransformFeedbackBufferBase );
//            GetDelegateForFunction( "glTransformFeedbackBufferRange", out _glTransformFeedbackBufferRange );
//            GetDelegateForFunction( "glGetTransformFeedbackiv", out _glGetTransformFeedbackiv );
//            GetDelegateForFunction( "glGetTransformFeedbacki_v", out _glGetTransformFeedbacki_v );
//            GetDelegateForFunction( "glGetTransformFeedbacki64_v", out _glGetTransformFeedbacki64_v );
//
//            GetDelegateForFunction( "glCreateBuffers", out _glCreateBuffers );
//            GetDelegateForFunction( "glNamedBufferStorage", out _glNamedBufferStorage );
//            GetDelegateForFunction( "glNamedBufferData", out _glNamedBufferData );
//            GetDelegateForFunction( "glNamedBufferSubData", out _glNamedBufferSubData );
//            GetDelegateForFunction( "glCopyNamedBufferSubData", out _glCopyNamedBufferSubData );
//            GetDelegateForFunction( "glClearNamedBufferData", out _glClearNamedBufferData );
//            GetDelegateForFunction( "glClearNamedBufferSubData", out _glClearNamedBufferSubData );
//            GetDelegateForFunction( "glMapNamedBuffer", out _glMapNamedBuffer );
//            GetDelegateForFunction( "glMapNamedBufferRange", out _glMapNamedBufferRange );
//            GetDelegateForFunction( "glUnmapNamedBuffer", out _glUnmapNamedBuffer );
//            GetDelegateForFunction( "glFlushMappedNamedBufferRange", out _glFlushMappedNamedBufferRange );
//            GetDelegateForFunction( "glGetNamedBufferParameteriv", out _glGetNamedBufferParameteriv );
//            GetDelegateForFunction( "glGetNamedBufferParameteri64v", out _glGetNamedBufferParameteri64v );
//            GetDelegateForFunction( "glGetNamedBufferPointerv", out _glGetNamedBufferPointerv );
//            GetDelegateForFunction( "glGetNamedBufferSubData", out _glGetNamedBufferSubData );
//            GetDelegateForFunction( "glCreateFramebuffers", out _glCreateFramebuffers );
//            GetDelegateForFunction( "glNamedFramebufferRenderbuffer", out _glNamedFramebufferRenderbuffer );
//            GetDelegateForFunction( "glNamedFramebufferParameteri", out _glNamedFramebufferParameteri );
//            GetDelegateForFunction( "glNamedFramebufferTexture", out _glNamedFramebufferTexture );
//            GetDelegateForFunction( "glNamedFramebufferTextureLayer", out _glNamedFramebufferTextureLayer );
//            GetDelegateForFunction( "glNamedFramebufferDrawBuffer", out _glNamedFramebufferDrawBuffer );
//            GetDelegateForFunction( "glNamedFramebufferDrawBuffers", out _glNamedFramebufferDrawBuffers );
//            GetDelegateForFunction( "glNamedFramebufferReadBuffer", out _glNamedFramebufferReadBuffer );
//            GetDelegateForFunction( "glInvalidateNamedFramebufferData", out _glInvalidateNamedFramebufferData );
//            GetDelegateForFunction( "glInvalidateNamedFramebufferSubData", out _glInvalidateNamedFramebufferSubData );
//            GetDelegateForFunction( "glClearNamedFramebufferiv", out _glClearNamedFramebufferiv );
//            GetDelegateForFunction( "glClearNamedFramebufferuiv", out _glClearNamedFramebufferuiv );
//            GetDelegateForFunction( "glClearNamedFramebufferfv", out _glClearNamedFramebufferfv );
//            GetDelegateForFunction( "glClearNamedFramebufferfi", out _glClearNamedFramebufferfi );
//            GetDelegateForFunction( "glBlitNamedFramebuffer", out _glBlitNamedFramebuffer );
//            GetDelegateForFunction( "glCheckNamedFramebufferStatus", out _glCheckNamedFramebufferStatus );
//            GetDelegateForFunction( "glGetNamedFramebufferParameteriv", out _glGetNamedFramebufferParameteriv );
//            GetDelegateForFunction( "glGetNamedFramebufferAttachmentParameteriv", out _glGetNamedFramebufferAttachmentParameteriv );
//            GetDelegateForFunction( "glCreateRenderbuffers", out _glCreateRenderbuffers );
//            GetDelegateForFunction( "glNamedRenderbufferStorage", out _glNamedRenderbufferStorage );
//            GetDelegateForFunction( "glNamedRenderbufferStorageMultisample", out _glNamedRenderbufferStorageMultisample );
//            GetDelegateForFunction( "glGetNamedRenderbufferParameteriv", out _glGetNamedRenderbufferParameteriv );
//            GetDelegateForFunction( "glCreateTextures", out _glCreateTextures );
//            GetDelegateForFunction( "glTextureBuffer", out _glTextureBuffer );
//            GetDelegateForFunction( "glTextureBufferRange", out _glTextureBufferRange );
//            GetDelegateForFunction( "glTextureStorage1D", out _glTextureStorage1D );
//            GetDelegateForFunction( "glTextureStorage2D", out _glTextureStorage2D );
//            GetDelegateForFunction( "glTextureStorage3D", out _glTextureStorage3D );
//            GetDelegateForFunction( "glTextureStorage2DMultisample", out _glTextureStorage2DMultisample );
//            GetDelegateForFunction( "glTextureStorage3DMultisample", out _glTextureStorage3DMultisample );
//            GetDelegateForFunction( "glTextureSubImage1D", out _glTextureSubImage1D );
//            GetDelegateForFunction( "glTextureSubImage2D", out _glTextureSubImage2D );
//            GetDelegateForFunction( "glTextureSubImage3D", out _glTextureSubImage3D );
//            GetDelegateForFunction( "glCompressedTextureSubImage1D", out _glCompressedTextureSubImage1D );
//            GetDelegateForFunction( "glCompressedTextureSubImage2D", out _glCompressedTextureSubImage2D );
//            GetDelegateForFunction( "glCompressedTextureSubImage3D", out _glCompressedTextureSubImage3D );
//            GetDelegateForFunction( "glCopyTextureSubImage1D", out _glCopyTextureSubImage1D );
//            GetDelegateForFunction( "glCopyTextureSubImage2D", out _glCopyTextureSubImage2D );
//            GetDelegateForFunction( "glCopyTextureSubImage3D", out _glCopyTextureSubImage3D );
//            GetDelegateForFunction( "glTextureParameterf", out _glTextureParameterf );
//            GetDelegateForFunction( "glTextureParameterfv", out _glTextureParameterfv );
//            GetDelegateForFunction( "glTextureParameteri", out _glTextureParameteri );
//            GetDelegateForFunction( "glTextureParameterIiv", out _glTextureParameterIiv );
//            GetDelegateForFunction( "glTextureParameterIuiv", out _glTextureParameterIuiv );
//            GetDelegateForFunction( "glTextureParameteriv", out _glTextureParameteriv );
//            GetDelegateForFunction( "glGenerateTextureMipmap", out _glGenerateTextureMipmap );
//            GetDelegateForFunction( "glBindTextureUnit", out _glBindTextureUnit );
//            GetDelegateForFunction( "glGetTextureImage", out _glGetTextureImage );
//            GetDelegateForFunction( "glGetCompressedTextureImage", out _glGetCompressedTextureImage );
//            GetDelegateForFunction( "glGetTextureLevelParameterfv", out _glGetTextureLevelParameterfv );
//            GetDelegateForFunction( "glGetTextureLevelParameteriv", out _glGetTextureLevelParameteriv );
//            GetDelegateForFunction( "glGetTextureParameterfv", out _glGetTextureParameterfv );
//            GetDelegateForFunction( "glGetTextureParameterIiv", out _glGetTextureParameterIiv );
//            GetDelegateForFunction( "glGetTextureParameterIuiv", out _glGetTextureParameterIuiv );
//            GetDelegateForFunction( "glGetTextureParameteriv", out _glGetTextureParameteriv );
//            GetDelegateForFunction( "glCreateVertexArrays", out _glCreateVertexArrays );
//            GetDelegateForFunction( "glDisableVertexArrayAttrib", out _glDisableVertexArrayAttrib );
//            GetDelegateForFunction( "glEnableVertexArrayAttrib", out _glEnableVertexArrayAttrib );
//            GetDelegateForFunction( "glVertexArrayElementBuffer", out _glVertexArrayElementBuffer );
//            GetDelegateForFunction( "glVertexArrayVertexBuffer", out _glVertexArrayVertexBuffer );
//            GetDelegateForFunction( "glVertexArrayVertexBuffers", out _glVertexArrayVertexBuffers );
//            GetDelegateForFunction( "glVertexArrayAttribBinding", out _glVertexArrayAttribBinding );
//            GetDelegateForFunction( "glVertexArrayAttribFormat", out _glVertexArrayAttribFormat );
//            GetDelegateForFunction( "glVertexArrayAttribIFormat", out _glVertexArrayAttribIFormat );
//            GetDelegateForFunction( "glVertexArrayAttribLFormat", out _glVertexArrayAttribLFormat );
//            GetDelegateForFunction( "glVertexArrayBindingDivisor", out _glVertexArrayBindingDivisor );
//            GetDelegateForFunction( "glGetVertexArrayiv", out _glGetVertexArrayiv );
//            GetDelegateForFunction( "glGetVertexArrayIndexediv", out _glGetVertexArrayIndexediv );
//            GetDelegateForFunction( "glGetVertexArrayIndexed64iv", out _glGetVertexArrayIndexed64iv );
//            GetDelegateForFunction( "glCreateSamplers", out _glCreateSamplers );
//            GetDelegateForFunction( "glCreateProgramPipelines", out _glCreateProgramPipelines );
//            GetDelegateForFunction( "glCreateQueries", out _glCreateQueries );
//            GetDelegateForFunction( "glGetQueryBufferObjecti64v", out _glGetQueryBufferObjecti64v );
//            GetDelegateForFunction( "glGetQueryBufferObjectiv", out _glGetQueryBufferObjectiv );
//            GetDelegateForFunction( "glGetQueryBufferObjectui64v", out _glGetQueryBufferObjectui64v );
//            GetDelegateForFunction( "glGetQueryBufferObjectuiv", out _glGetQueryBufferObjectuiv );
//            GetDelegateForFunction( "glMemoryBarrierByRegion", out _glMemoryBarrierByRegion );
//            GetDelegateForFunction( "glGetTextureSubImage", out _glGetTextureSubImage );
//            GetDelegateForFunction( "glGetCompressedTextureSubImage", out _glGetCompressedTextureSubImage );
//            GetDelegateForFunction( "glGetGraphicsResetStatus", out _glGetGraphicsResetStatus );
//            GetDelegateForFunction( "glGetnCompressedTexImage", out _glGetnCompressedTexImage );
//            GetDelegateForFunction( "glGetnUniformfv", out _glGetnUniformfv );
//            GetDelegateForFunction( "glGetnUniformiv", out _glGetnUniformiv );
//            GetDelegateForFunction( "glGetnUniformuiv", out _glGetnUniformuiv );
//            GetDelegateForFunction( "glReadnPixels", out _glReadnPixels );
//            GetDelegateForFunction( "glTextureBarrier", out _glTextureBarrier );
//
////            _glGetnTexImage                             = Marshal.GetDelegateForFunctionPointer< PFNGLGETNTEXIMAGEPROC                                         >( "glGetnTexImage", out 
////            _glGetnUniformdv                            = Marshal.GetDelegateForFunctionPointer< PFNGLGETNUNIFORMDVPROC                                         >( "glGetnUniformdv", out 
//        }
//
//        if ( _ogl46 )
//        {
//            GetDelegateForFunction( "glSpecializeShader", out _glSpecializeShader );
//            GetDelegateForFunction( "glPolygonOffsetClamp", out _glPolygonOffsetClamp );
//
////            _glMultiDrawArraysIndirectCount   = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC                                         >( "glMultiDrawArraysIndirectCount", out 
////            _glMultiDrawElementsIndirectCount = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC                                         >( "glMultiDrawElementsIndirectCount", out 
//        }
    }

    // ========================================================================
    // ========================================================================

    // Import wglGetProcAddress
    [DllImport( "opengl32.dll", EntryPoint = "wglGetProcAddress", CallingConvention = CallingConvention.StdCall )]
    private static extern IntPtr wglGetProcAddress( string procname );

    // ========================================================================
    // ========================================================================

    public static bool GetDelegateForFunction< T >( string functionName, out T functionDelegate ) where T : Delegate
    {
        Logger.Debug( $"Loading OGL Function: {functionName}" );
        
        var functionPtr = wglGetProcAddress( functionName );

        if ( functionPtr == IntPtr.Zero )
        {
            functionPtr = Glfw.GetProcAddress( functionName );
        }

        if ( functionPtr != IntPtr.Zero )
        {
            try
            {
                functionDelegate = Marshal.GetDelegateForFunctionPointer< T >( functionPtr );

                Logger.Debug( $"OGL Function {functionName} loaded successfully" );
                
                return true;
            }
            catch ( Exception ex )
            {
                Logger.Debug( $"Error creating delegate for {functionName}: {ex.Message}" );

                functionDelegate = null!;

                return false;
            }
        }

        Logger.Debug( $"Failed to load {functionName}" );

        functionDelegate = null!;

        return false;
    }

    // ========================================================================
    // ========================================================================

    public static T GetDelegateFor< T >( string functionName ) where T : Delegate
    {
        Logger.Debug( $"Loading OGL Function: {functionName}" );
        
        var functionPtr = wglGetProcAddress( functionName );

        if ( functionPtr == IntPtr.Zero )
        {
            functionPtr = Glfw.GetProcAddress( functionName );
        }

        if ( functionPtr != IntPtr.Zero )
        {
            try
            {
                T functionDelegate = Marshal.GetDelegateForFunctionPointer< T >( functionPtr );

                Logger.Debug( $"OGL Function {functionName} loaded successfully" );
                
                return functionDelegate;
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( $"Error creating delegate for {functionName}: {ex.Message}" );
            }
        }

        throw new GdxRuntimeException( $"Failed to load {functionName}" );
    }
}

//#pragma warning restore IDE0079 // Remove unnecessary suppression
//#pragma warning restore CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
//#pragma warning restore CS8603  // Possible null reference return.
//#pragma warning restore IDE0060 // Remove unused parameter.
//#pragma warning restore IDE1006 // Naming Styles.
//#pragma warning restore IDE0090 // Use 'new(...)'.
//#pragma warning restore CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type