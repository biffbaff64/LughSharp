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

    private unsafe void SetOGL()
    {
        Logger.Checkpoint();

        if ( Glfw.GetCurrentContext() == null )
        {
            Logger.Debug( "NULL Current OpenGL Context!" );
        }

        LoadOpenGLFunction< glGetStringDelegate >( "glGetString", out _glGetString );

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
        SetOGL();

        if ( _ogl10 || _ogl11 || _ogl12 || _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glCullFace", out _glCullFace );
            LoadOpenGLFunction( "glFrontFace", out _glFrontFace );
            LoadOpenGLFunction( "glHint", out _glHint );
            LoadOpenGLFunction( "glLineWidth", out _glLineWidth );
            LoadOpenGLFunction( "glPointSize", out _glPointSize );
            LoadOpenGLFunction( "glPolygonMode", out _glPolygonMode );
            LoadOpenGLFunction( "glScissor", out _glScissor );
            LoadOpenGLFunction( "glTexParameterf", out _glTexParameterf );
            LoadOpenGLFunction( "glTexParameterfv", out _glTexParameterfv );
            LoadOpenGLFunction( "glTexParameteri", out _glTexParameteri );
            LoadOpenGLFunction( "glTexParameteriv", out _glTexParameteriv );
            LoadOpenGLFunction( "glTexImage1D", out _glTexImage1D );
            LoadOpenGLFunction( "glTexImage2D", out _glTexImage2D );
            LoadOpenGLFunction( "glDrawBuffer", out _glDrawBuffer );
            LoadOpenGLFunction( "glClear", out _glClear );
            LoadOpenGLFunction( "glClearColor", out _glClearColor );
            LoadOpenGLFunction( "glClearStencil", out _glClearStencil );
            LoadOpenGLFunction( "glClearDepth", out _glClearDepth );
            LoadOpenGLFunction( "glStencilMask", out _glStencilMask );
            LoadOpenGLFunction( "glColorMask", out _glColorMask );
            LoadOpenGLFunction( "glDepthMask", out _glDepthMask );
            LoadOpenGLFunction( "glDisable", out _glDisable );
            LoadOpenGLFunction( "glEnable", out _glEnable );
            LoadOpenGLFunction( "glFinish", out _glFinish );
            LoadOpenGLFunction( "glFlush", out _glFlush );
            LoadOpenGLFunction( "glBlendFunc", out _glBlendFunc );
            LoadOpenGLFunction( "glGetString", out _glGetString );
            LoadOpenGLFunction( "glLogicOp", out _glLogicOp );
            LoadOpenGLFunction( "glStencilFunc", out _glStencilFunc );
            LoadOpenGLFunction( "glStencilOp", out _glStencilOp );
            LoadOpenGLFunction( "glDepthFunc", out _glDepthFunc );
            LoadOpenGLFunction( "glPixelStoref", out _glPixelStoref );
            LoadOpenGLFunction( "glPixelStorei", out _glPixelStorei );
            LoadOpenGLFunction( "glReadBuffer", out _glReadBuffer );
            LoadOpenGLFunction( "glReadPixels", out _glReadPixels );
            LoadOpenGLFunction( "glGetBooleanv", out _glGetBooleanv );
            LoadOpenGLFunction( "glGetDoublev", out _glGetDoublev );
            LoadOpenGLFunction( "glGetError", out _glGetError );
            LoadOpenGLFunction( "glGetFloatv", out _glGetFloatv );
            LoadOpenGLFunction( "glGetIntegerv", out _glGetIntegerv );
            LoadOpenGLFunction( "glGetTexImage", out _glGetTexImage );
            LoadOpenGLFunction( "glGetTexParameterfv", out _glGetTexParameterfv );
            LoadOpenGLFunction( "glGetTexParameteriv", out _glGetTexParameteriv );
            LoadOpenGLFunction( "glGetTexLevelParameterfv", out _glGetTexLevelParameterfv );
            LoadOpenGLFunction( "glGetTexLevelParameteriv", out _glGetTexLevelParameteriv );
            LoadOpenGLFunction( "glIsEnabled", out _glIsEnabled );
            LoadOpenGLFunction( "glDepthRange", out _glDepthRange );
            LoadOpenGLFunction( "glViewport", out _glViewport );
        }

        if ( _ogl11 || _ogl12 || _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glDrawArrays", out _glDrawArrays );
            LoadOpenGLFunction( "glDrawElements", out _glDrawElements );
            LoadOpenGLFunction( "glPolygonOffset", out _glPolygonOffset );
            LoadOpenGLFunction( "glCopyTexImage1D", out _glCopyTexImage1D );
            LoadOpenGLFunction( "glCopyTexImage2D", out _glCopyTexImage2D );
            LoadOpenGLFunction( "glCopyTexSubImage1D", out _glCopyTexSubImage1D );
            LoadOpenGLFunction( "glCopyTexSubImage2D", out _glCopyTexSubImage2D );
            LoadOpenGLFunction( "glTexSubImage1D", out _glTexSubImage1D );
            LoadOpenGLFunction( "glTexSubImage2D", out _glTexSubImage2D );
            LoadOpenGLFunction( "glBindTexture", out _glBindTexture );
            LoadOpenGLFunction( "glDeleteTextures", out _glDeleteTextures );
            LoadOpenGLFunction( "glGenTextures", out _glGenTextures );
            LoadOpenGLFunction( "glIsTexture", out _glIsTexture );
        }

        if ( _ogl12 || _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glDrawRangeElements", out _glDrawRangeElements );
            LoadOpenGLFunction( "glTexImage3D", out _glTexImage3D );
            LoadOpenGLFunction( "glTexSubImage3D", out _glTexSubImage3D );
            LoadOpenGLFunction( "glCopyTexSubImage3D", out _glCopyTexSubImage3D );
        }

        if ( _ogl13 || _ogl14 || _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glActiveTexture", out _glActiveTexture );
            LoadOpenGLFunction( "glSampleCoverage", out _glSampleCoverage );
            LoadOpenGLFunction( "glCompressedTexImage3D", out _glCompressedTexImage3D );
            LoadOpenGLFunction( "glCompressedTexImage2D", out _glCompressedTexImage2D );
            LoadOpenGLFunction( "glCompressedTexImage1D", out _glCompressedTexImage1D );
            LoadOpenGLFunction( "glCompressedTexSubImage3D", out _glCompressedTexSubImage3D );
            LoadOpenGLFunction( "glCompressedTexSubImage2D", out _glCompressedTexSubImage2D );
            LoadOpenGLFunction( "glCompressedTexSubImage1D", out _glCompressedTexSubImage1D );
            LoadOpenGLFunction( "glGetCompressedTexImage", out _glGetCompressedTexImage );
        }

        if ( _ogl14 || _ogl15
                    || _ogl20 || _ogl21
                    || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glBlendFuncSeparate", out _glBlendFuncSeparate );
            LoadOpenGLFunction( "glMultiDrawArrays", out _glMultiDrawArrays );
            LoadOpenGLFunction( "glMultiDrawElements", out _glMultiDrawElements );
            LoadOpenGLFunction( "glPointParameterf", out _glPointParameterf );
            LoadOpenGLFunction( "glPointParameterfv", out _glPointParameterfv );
            LoadOpenGLFunction( "glPointParameteri", out _glPointParameteri );
            LoadOpenGLFunction( "glPointParameteriv", out _glPointParameteriv );
            LoadOpenGLFunction( "glBlendColor", out _glBlendColor );
            LoadOpenGLFunction( "glBlendEquation", out _glBlendEquation );
        }

        if ( _ogl15
             || _ogl20 || _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glGenQueries", out _glGenQueries );
            LoadOpenGLFunction( "glDeleteQueries", out _glDeleteQueries );
            LoadOpenGLFunction( "glIsQuery", out _glIsQuery );
            LoadOpenGLFunction( "glBeginQuery", out _glBeginQuery );
            LoadOpenGLFunction( "glEndQuery", out _glEndQuery );
            LoadOpenGLFunction( "glGetQueryiv", out _glGetQueryiv );
            LoadOpenGLFunction( "glGetQueryObjectiv", out _glGetQueryObjectiv );
            LoadOpenGLFunction( "glGetQueryObjectuiv", out _glGetQueryObjectuiv );
            LoadOpenGLFunction( "glBindBuffer", out _glBindBuffer );
            LoadOpenGLFunction( "glDeleteBuffers", out _glDeleteBuffers );
            LoadOpenGLFunction( "glGenBuffers", out _glGenBuffers );
            LoadOpenGLFunction( "glIsBuffer", out _glIsBuffer );
            LoadOpenGLFunction( "glBufferData", out _glBufferData );
            LoadOpenGLFunction( "glBufferSubData", out _glBufferSubData );
            LoadOpenGLFunction( "glGetBufferSubData", out _glGetBufferSubData );
            LoadOpenGLFunction( "glMapBuffer", out _glMapBuffer );
            LoadOpenGLFunction( "glUnmapBuffer", out _glUnmapBuffer );
            LoadOpenGLFunction( "glGetBufferParameteriv", out _glGetBufferParameteriv );
            LoadOpenGLFunction( "glGetBufferPointerv", out _glGetBufferPointerv );
        }

        if ( _ogl20 || _ogl21
                    || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glBlendEquationSeparate", out _glBlendEquationSeparate );
            LoadOpenGLFunction( "glDrawBuffers", out _glDrawBuffers );
            LoadOpenGLFunction( "glStencilOpSeparate", out _glStencilOpSeparate );
            LoadOpenGLFunction( "glStencilFuncSeparate", out _glStencilFuncSeparate );
            LoadOpenGLFunction( "glStencilMaskSeparate", out _glStencilMaskSeparate );
            LoadOpenGLFunction( "glAttachShader", out _glAttachShader );
            LoadOpenGLFunction( "glBindAttribLocation", out _glBindAttribLocation );
            LoadOpenGLFunction( "glCompileShader", out _glCompileShader );
            LoadOpenGLFunction( "glCreateProgram", out _glCreateProgram );
            LoadOpenGLFunction( "glCreateShader", out _glCreateShader );
            LoadOpenGLFunction( "glDeleteProgram", out _glDeleteProgram );
            LoadOpenGLFunction( "glDeleteShader", out _glDeleteShader );
            LoadOpenGLFunction( "glDetachShader", out _glDetachShader );
            LoadOpenGLFunction( "glDisableVertexAttribArray", out _glDisableVertexAttribArray );
            LoadOpenGLFunction( "glEnableVertexAttribArray", out _glEnableVertexAttribArray );
            LoadOpenGLFunction( "glGetActiveAttrib", out _glGetActiveAttrib );
            LoadOpenGLFunction( "glGetActiveUniform", out _glGetActiveUniform );
            LoadOpenGLFunction( "glGetAttachedShaders", out _glGetAttachedShaders );
            LoadOpenGLFunction( "glGetAttribLocation", out _glGetAttribLocation );
            LoadOpenGLFunction( "glGetProgramiv", out _glGetProgramiv );
            LoadOpenGLFunction( "glGetProgramInfoLog", out _glGetProgramInfoLog );
            LoadOpenGLFunction( "glGetShaderiv", out _glGetShaderiv );
            LoadOpenGLFunction( "glGetShaderInfoLog", out _glGetShaderInfoLog );
            LoadOpenGLFunction( "glGetShaderSource", out _glGetShaderSource );
            LoadOpenGLFunction( "glGetUniformLocation", out _glGetUniformLocation );
            LoadOpenGLFunction( "glGetUniformfv", out _glGetUniformfv );
            LoadOpenGLFunction( "glGetUniformiv", out _glGetUniformiv );
            LoadOpenGLFunction( "glGetVertexAttribdv", out _glGetVertexAttribdv );
            LoadOpenGLFunction( "glGetVertexAttribfv", out _glGetVertexAttribfv );
            LoadOpenGLFunction( "glGetVertexAttribiv", out _glGetVertexAttribiv );
            LoadOpenGLFunction( "glGetVertexAttribPointerv", out _glGetVertexAttribPointerv );
            LoadOpenGLFunction( "glIsProgram", out _glIsProgram );
            LoadOpenGLFunction( "glIsShader", out _glIsShader );
            LoadOpenGLFunction( "glLinkProgram", out _glLinkProgram );
            LoadOpenGLFunction( "glShaderSource", out _glShaderSource );
            LoadOpenGLFunction( "glUseProgram", out _glUseProgram );
            LoadOpenGLFunction( "glUniform1f", out _glUniform1f );
            LoadOpenGLFunction( "glUniform2f", out _glUniform2f );
            LoadOpenGLFunction( "glUniform3f", out _glUniform3f );
            LoadOpenGLFunction( "glUniform4f", out _glUniform4f );
            LoadOpenGLFunction( "glUniform1i", out _glUniform1i );
            LoadOpenGLFunction( "glUniform2i", out _glUniform2i );
            LoadOpenGLFunction( "glUniform3i", out _glUniform3i );
            LoadOpenGLFunction( "glUniform4i", out _glUniform4i );
            LoadOpenGLFunction( "glUniform1fv", out _glUniform1fv );
            LoadOpenGLFunction( "glUniform2fv", out _glUniform2fv );
            LoadOpenGLFunction( "glUniform3fv", out _glUniform3fv );
            LoadOpenGLFunction( "glUniform4fv", out _glUniform4fv );
            LoadOpenGLFunction( "glUniform1iv", out _glUniform1iv );
            LoadOpenGLFunction( "glUniform2iv", out _glUniform2iv );
            LoadOpenGLFunction( "glUniform3iv", out _glUniform3iv );
            LoadOpenGLFunction( "glUniform4iv", out _glUniform4iv );
            LoadOpenGLFunction( "glUniformMatrix2fv", out _glUniformMatrix2fv );
            LoadOpenGLFunction( "glUniformMatrix3fv", out _glUniformMatrix3fv );
            LoadOpenGLFunction( "glUniformMatrix4fv", out _glUniformMatrix4fv );
            LoadOpenGLFunction( "glValidateProgram", out _glValidateProgram );
            LoadOpenGLFunction( "glVertexAttrib1d", out _glVertexAttrib1d );
            LoadOpenGLFunction( "glVertexAttrib1dv", out _glVertexAttrib1dv );
            LoadOpenGLFunction( "glVertexAttrib1f", out _glVertexAttrib1f );
            LoadOpenGLFunction( "glVertexAttrib1fv", out _glVertexAttrib1fv );
            LoadOpenGLFunction( "glVertexAttrib1s", out _glVertexAttrib1s );
            LoadOpenGLFunction( "glVertexAttrib1sv", out _glVertexAttrib1sv );
            LoadOpenGLFunction( "glVertexAttrib2d", out _glVertexAttrib2d );
            LoadOpenGLFunction( "glVertexAttrib2dv", out _glVertexAttrib2dv );
            LoadOpenGLFunction( "glVertexAttrib2f", out _glVertexAttrib2f );
            LoadOpenGLFunction( "glVertexAttrib2fv", out _glVertexAttrib2fv );
            LoadOpenGLFunction( "glVertexAttrib2s", out _glVertexAttrib2s );
            LoadOpenGLFunction( "glVertexAttrib2sv", out _glVertexAttrib2sv );
            LoadOpenGLFunction( "glVertexAttrib3d", out _glVertexAttrib3d );
            LoadOpenGLFunction( "glVertexAttrib3dv", out _glVertexAttrib3dv );
            LoadOpenGLFunction( "glVertexAttrib3f", out _glVertexAttrib3f );
            LoadOpenGLFunction( "glVertexAttrib3fv", out _glVertexAttrib3fv );
            LoadOpenGLFunction( "glVertexAttrib3s", out _glVertexAttrib3s );
            LoadOpenGLFunction( "glVertexAttrib3sv", out _glVertexAttrib3sv );
            LoadOpenGLFunction( "glVertexAttrib4Nbv", out _glVertexAttrib4Nbv );
            LoadOpenGLFunction( "glVertexAttrib4Niv", out _glVertexAttrib4Niv );
            LoadOpenGLFunction( "glVertexAttrib4Nsv", out _glVertexAttrib4Nsv );
            LoadOpenGLFunction( "glVertexAttrib4Nub", out _glVertexAttrib4Nub );
            LoadOpenGLFunction( "glVertexAttrib4Nubv", out _glVertexAttrib4Nubv );
            LoadOpenGLFunction( "glVertexAttrib4Nuiv", out _glVertexAttrib4Nuiv );
            LoadOpenGLFunction( "glVertexAttrib4Nusv", out _glVertexAttrib4Nusv );
            LoadOpenGLFunction( "glVertexAttrib4bv", out _glVertexAttrib4bv );
            LoadOpenGLFunction( "glVertexAttrib4d", out _glVertexAttrib4d );
            LoadOpenGLFunction( "glVertexAttrib4dv", out _glVertexAttrib4dv );
            LoadOpenGLFunction( "glVertexAttrib4f", out _glVertexAttrib4f );
            LoadOpenGLFunction( "glVertexAttrib4fv", out _glVertexAttrib4fv );
            LoadOpenGLFunction( "glVertexAttrib4iv", out _glVertexAttrib4iv );
            LoadOpenGLFunction( "glVertexAttrib4s", out _glVertexAttrib4s );
            LoadOpenGLFunction( "glVertexAttrib4sv", out _glVertexAttrib4sv );
            LoadOpenGLFunction( "glVertexAttrib4ubv", out _glVertexAttrib4ubv );
            LoadOpenGLFunction( "glVertexAttrib4uiv", out _glVertexAttrib4uiv );
            LoadOpenGLFunction( "glVertexAttrib4usv", out _glVertexAttrib4usv );
            LoadOpenGLFunction( "glVertexAttribPointer", out _glVertexAttribPointer );
        }

        if ( _ogl21
             || _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glUniformMatrix2x3fv", out _glUniformMatrix2x3fv );
            LoadOpenGLFunction( "glUniformMatrix3x2fv", out _glUniformMatrix3x2fv );
            LoadOpenGLFunction( "glUniformMatrix2x4fv", out _glUniformMatrix2x4fv );
            LoadOpenGLFunction( "glUniformMatrix4x2fv", out _glUniformMatrix4x2fv );
            LoadOpenGLFunction( "glUniformMatrix3x4fv", out _glUniformMatrix3x4fv );
            LoadOpenGLFunction( "glUniformMatrix4x3fv", out _glUniformMatrix4x3fv );
        }

        if ( _ogl30 || _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glColorMaski", out _glColorMaski );
            LoadOpenGLFunction( "glGetBooleani_v", out _glGetBooleani_v );
            LoadOpenGLFunction( "glGetIntegeri_v", out _glGetIntegeri_v );
            LoadOpenGLFunction( "glEnablei", out _glEnablei );
            LoadOpenGLFunction( "glDisablei", out _glDisablei );
            LoadOpenGLFunction( "glIsEnabledi", out _glIsEnabledi );
            LoadOpenGLFunction( "glBeginTransformFeedback", out _glBeginTransformFeedback );
            LoadOpenGLFunction( "glEndTransformFeedback", out _glEndTransformFeedback );
            LoadOpenGLFunction( "glBindBufferRange", out _glBindBufferRange );
            LoadOpenGLFunction( "glBindBufferBase", out _glBindBufferBase );
            LoadOpenGLFunction( "glTransformFeedbackVaryings", out _glTransformFeedbackVaryings );
            LoadOpenGLFunction( "glGetTransformFeedbackVarying", out _glGetTransformFeedbackVarying );
            LoadOpenGLFunction( "glClampColor", out _glClampColor );
            LoadOpenGLFunction( "glBeginConditionalRender", out _glBeginConditionalRender );
            LoadOpenGLFunction( "glEndConditionalRender", out _glEndConditionalRender );
            LoadOpenGLFunction( "glVertexAttribIPointer", out _glVertexAttribIPointer );
            LoadOpenGLFunction( "glGetVertexAttribIiv", out _glGetVertexAttribIiv );
            LoadOpenGLFunction( "glGetVertexAttribIuiv", out _glGetVertexAttribIuiv );
            LoadOpenGLFunction( "glVertexAttribI1i", out _glVertexAttribI1i );
            LoadOpenGLFunction( "glVertexAttribI2i", out _glVertexAttribI2i );
            LoadOpenGLFunction( "glVertexAttribI3i", out _glVertexAttribI3i );
            LoadOpenGLFunction( "glVertexAttribI4i", out _glVertexAttribI4i );
            LoadOpenGLFunction( "glVertexAttribI1ui", out _glVertexAttribI1ui );
            LoadOpenGLFunction( "glVertexAttribI2ui", out _glVertexAttribI2ui );
            LoadOpenGLFunction( "glVertexAttribI3ui", out _glVertexAttribI3ui );
            LoadOpenGLFunction( "glVertexAttribI4ui", out _glVertexAttribI4ui );
            LoadOpenGLFunction( "glVertexAttribI1iv", out _glVertexAttribI1iv );
            LoadOpenGLFunction( "glVertexAttribI2iv", out _glVertexAttribI2iv );
            LoadOpenGLFunction( "glVertexAttribI3iv", out _glVertexAttribI3iv );
            LoadOpenGLFunction( "glVertexAttribI4iv", out _glVertexAttribI4iv );
            LoadOpenGLFunction( "glVertexAttribI1uiv", out _glVertexAttribI1uiv );
            LoadOpenGLFunction( "glVertexAttribI2uiv", out _glVertexAttribI2uiv );
            LoadOpenGLFunction( "glVertexAttribI3uiv", out _glVertexAttribI3uiv );
            LoadOpenGLFunction( "glVertexAttribI4uiv", out _glVertexAttribI4uiv );
            LoadOpenGLFunction( "glVertexAttribI4bv", out _glVertexAttribI4bv );
            LoadOpenGLFunction( "glVertexAttribI4sv", out _glVertexAttribI4sv );
            LoadOpenGLFunction( "glVertexAttribI4ubv", out _glVertexAttribI4ubv );
            LoadOpenGLFunction( "glVertexAttribI4usv", out _glVertexAttribI4usv );
            LoadOpenGLFunction( "glGetUniformuiv", out _glGetUniformuiv );
            LoadOpenGLFunction( "glBindFragDataLocation", out _glBindFragDataLocation );
            LoadOpenGLFunction( "glGetFragDataLocation", out _glGetFragDataLocation );
            LoadOpenGLFunction( "glUniform1ui", out _glUniform1ui );
            LoadOpenGLFunction( "glUniform2ui", out _glUniform2ui );
            LoadOpenGLFunction( "glUniform3ui", out _glUniform3ui );
            LoadOpenGLFunction( "glUniform4ui", out _glUniform4ui );
            LoadOpenGLFunction( "glUniform1uiv", out _glUniform1uiv );
            LoadOpenGLFunction( "glUniform2uiv", out _glUniform2uiv );
            LoadOpenGLFunction( "glUniform3uiv", out _glUniform3uiv );
            LoadOpenGLFunction( "glUniform4uiv", out _glUniform4uiv );
            LoadOpenGLFunction( "glTexParameterIiv", out _glTexParameterIiv );
            LoadOpenGLFunction( "glTexParameterIuiv", out _glTexParameterIuiv );
            LoadOpenGLFunction( "glGetTexParameterIiv", out _glGetTexParameterIiv );
            LoadOpenGLFunction( "glGetTexParameterIuiv", out _glGetTexParameterIuiv );
            LoadOpenGLFunction( "glClearBufferiv", out _glClearBufferiv );
            LoadOpenGLFunction( "glClearBufferuiv", out _glClearBufferuiv );
            LoadOpenGLFunction( "glClearBufferfv", out _glClearBufferfv );
            LoadOpenGLFunction( "glClearBufferfi", out _glClearBufferfi );
            LoadOpenGLFunction( "glGetStringi", out _glGetStringi );
            LoadOpenGLFunction( "glIsRenderbuffer", out _glIsRenderbuffer );
            LoadOpenGLFunction( "glBindRenderbuffer", out _glBindRenderbuffer );
            LoadOpenGLFunction( "glDeleteRenderbuffers", out _glDeleteRenderbuffers );
            LoadOpenGLFunction( "glGenRenderbuffers", out _glGenRenderbuffers );
            LoadOpenGLFunction( "glRenderbufferStorage", out _glRenderbufferStorage );
            LoadOpenGLFunction( "glGetRenderbufferParameteriv", out _glGetRenderbufferParameteriv );
            LoadOpenGLFunction( "glIsFramebuffer", out _glIsFramebuffer );
            LoadOpenGLFunction( "glBindFramebuffer", out _glBindFramebuffer );
            LoadOpenGLFunction( "glDeleteFramebuffers", out _glDeleteFramebuffers );
            LoadOpenGLFunction( "glGenFramebuffers", out _glGenFramebuffers );
            LoadOpenGLFunction( "glCheckFramebufferStatus", out _glCheckFramebufferStatus );
            LoadOpenGLFunction( "glFramebufferTexture1D", out _glFramebufferTexture1D );
            LoadOpenGLFunction( "glFramebufferTexture2D", out _glFramebufferTexture2D );
            LoadOpenGLFunction( "glFramebufferTexture3D", out _glFramebufferTexture3D );
            LoadOpenGLFunction( "glFramebufferRenderbuffer", out _glFramebufferRenderbuffer );
            LoadOpenGLFunction( "glGetFramebufferAttachmentParameteriv", out _glGetFramebufferAttachmentParameteriv );
            LoadOpenGLFunction( "glGenerateMipmap", out _glGenerateMipmap );
            LoadOpenGLFunction( "glBlitFramebuffer", out _glBlitFramebuffer );
            LoadOpenGLFunction( "glRenderbufferStorageMultisample", out _glRenderbufferStorageMultisample );
            LoadOpenGLFunction( "glFramebufferTextureLayer", out _glFramebufferTextureLayer );
            LoadOpenGLFunction( "glMapBufferRange", out _glMapBufferRange );
            LoadOpenGLFunction( "glFlushMappedBufferRange", out _glFlushMappedBufferRange );
            LoadOpenGLFunction( "glBindVertexArray", out _glBindVertexArray );
            LoadOpenGLFunction( "glDeleteVertexArrays", out _glDeleteVertexArrays );
            LoadOpenGLFunction( "glGenVertexArrays", out _glGenVertexArrays );
            LoadOpenGLFunction( "glIsVertexArray", out _glIsVertexArray );
        }

        if ( _ogl31 || _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glDrawArraysInstanced", out _glDrawArraysInstanced );
            LoadOpenGLFunction( "glDrawElementsInstanced", out _glDrawElementsInstanced );
            LoadOpenGLFunction( "glTexBuffer", out _glTexBuffer );
            LoadOpenGLFunction( "glPrimitiveRestartIndex", out _glPrimitiveRestartIndex );
            LoadOpenGLFunction( "glCopyBufferSubData", out _glCopyBufferSubData );
            LoadOpenGLFunction( "glGetUniformIndices", out _glGetUniformIndices );
            LoadOpenGLFunction( "glGetActiveUniformsiv", out _glGetActiveUniformsiv );
            LoadOpenGLFunction( "glGetActiveUniformName", out _glGetActiveUniformName );
            LoadOpenGLFunction( "glGetUniformBlockIndex", out _glGetUniformBlockIndex );
            LoadOpenGLFunction( "glGetActiveUniformBlockiv", out _glGetActiveUniformBlockiv );
            LoadOpenGLFunction( "glGetActiveUniformBlockName", out _glGetActiveUniformBlockName );
            LoadOpenGLFunction( "glUniformBlockBinding", out _glUniformBlockBinding );
        }

        if ( _ogl32 || _ogl33 || _ogl34
             || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glDrawElementsBaseVertex", out _glDrawElementsBaseVertex );
            LoadOpenGLFunction( "glDrawRangeElementsBaseVertex", out _glDrawRangeElementsBaseVertex );
            LoadOpenGLFunction( "glDrawElementsInstancedBaseVertex", out _glDrawElementsInstancedBaseVertex );
            LoadOpenGLFunction( "glMultiDrawElementsBaseVertex", out _glMultiDrawElementsBaseVertex );
            LoadOpenGLFunction( "glProvokingVertex", out _glProvokingVertex );
            LoadOpenGLFunction( "glFenceSync", out _glFenceSync );
            LoadOpenGLFunction( "glIsSync", out _glIsSync );
            LoadOpenGLFunction( "glDeleteSync", out _glDeleteSync );
            LoadOpenGLFunction( "glClientWaitSync", out _glClientWaitSync );
            LoadOpenGLFunction( "glWaitSync", out _glWaitSync );
            LoadOpenGLFunction( "glGetInteger64v", out _glGetInteger64v );
            LoadOpenGLFunction( "glGetSynciv", out _glGetSynciv );
            LoadOpenGLFunction( "glGetInteger64i_v", out _glGetInteger64i_v );
            LoadOpenGLFunction( "glGetBufferParameteri64v", out _glGetBufferParameteri64v );
            LoadOpenGLFunction( "glFramebufferTexture", out _glFramebufferTexture );
            LoadOpenGLFunction( "glTexImage2DMultisample", out _glTexImage2DMultisample );
            LoadOpenGLFunction( "glTexImage3DMultisample", out _glTexImage3DMultisample );
            LoadOpenGLFunction( "glGetMultisamplefv", out _glGetMultisamplefv );
            LoadOpenGLFunction( "glSampleMaski", out _glSampleMaski );
        }

        if ( _ogl33 || _ogl34
                    || _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glBindFragDataLocationIndexed", out _glBindFragDataLocationIndexed );
            LoadOpenGLFunction( "glGetFragDataIndex", out _glGetFragDataIndex );
            LoadOpenGLFunction( "glGenSamplers", out _glGenSamplers );
            LoadOpenGLFunction( "glDeleteSamplers", out _glDeleteSamplers );
            LoadOpenGLFunction( "glIsSampler", out _glIsSampler );
            LoadOpenGLFunction( "glBindSampler", out _glBindSampler );
            LoadOpenGLFunction( "glSamplerParameteri", out _glSamplerParameteri );
            LoadOpenGLFunction( "glSamplerParameteriv", out _glSamplerParameteriv );
            LoadOpenGLFunction( "glSamplerParameterf", out _glSamplerParameterf );
            LoadOpenGLFunction( "glSamplerParameterfv", out _glSamplerParameterfv );
            LoadOpenGLFunction( "glSamplerParameterIiv", out _glSamplerParameterIiv );
            LoadOpenGLFunction( "glSamplerParameterIuiv", out _glSamplerParameterIuiv );
            LoadOpenGLFunction( "glGetSamplerParameteriv", out _glGetSamplerParameteriv );
            LoadOpenGLFunction( "glGetSamplerParameterIiv", out _glGetSamplerParameterIiv );
            LoadOpenGLFunction( "glGetSamplerParameterfv", out _glGetSamplerParameterfv );
            LoadOpenGLFunction( "glGetSamplerParameterIuiv", out _glGetSamplerParameterIuiv );
            LoadOpenGLFunction( "glQueryCounter", out _glQueryCounter );
            LoadOpenGLFunction( "glGetQueryObjecti64v", out _glGetQueryObjecti64v );
            LoadOpenGLFunction( "glGetQueryObjectui64v", out _glGetQueryObjectui64v );
            LoadOpenGLFunction( "glVertexAttribDivisor", out _glVertexAttribDivisor );
            LoadOpenGLFunction( "glVertexAttribP1ui", out _glVertexAttribP1ui );
            LoadOpenGLFunction( "glVertexAttribP1uiv", out _glVertexAttribP1uiv );
            LoadOpenGLFunction( "glVertexAttribP2ui", out _glVertexAttribP2ui );
            LoadOpenGLFunction( "glVertexAttribP2uiv", out _glVertexAttribP2uiv );
            LoadOpenGLFunction( "glVertexAttribP3ui", out _glVertexAttribP3ui );
            LoadOpenGLFunction( "glVertexAttribP3uiv", out _glVertexAttribP3uiv );
            LoadOpenGLFunction( "glVertexAttribP4ui", out _glVertexAttribP4ui );
            LoadOpenGLFunction( "glVertexAttribP4uiv", out _glVertexAttribP4uiv );
        }

        if ( _ogl40 || _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glMinSampleShading", out _glMinSampleShading );
            LoadOpenGLFunction( "glBlendEquationi", out _glBlendEquationi );
            LoadOpenGLFunction( "glBlendEquationSeparatei", out _glBlendEquationSeparatei );
            LoadOpenGLFunction( "glBlendFunci", out _glBlendFunci );
            LoadOpenGLFunction( "glBlendFuncSeparatei", out _glBlendFuncSeparatei );
            LoadOpenGLFunction( "glDrawArraysIndirect", out _glDrawArraysIndirect );
            LoadOpenGLFunction( "glDrawElementsIndirect", out _glDrawElementsIndirect );
            LoadOpenGLFunction( "glUniform1d", out _glUniform1d );
            LoadOpenGLFunction( "glUniform2d", out _glUniform2d );
            LoadOpenGLFunction( "glUniform3d", out _glUniform3d );
            LoadOpenGLFunction( "glUniform4d", out _glUniform4d );
            LoadOpenGLFunction( "glUniform1dv", out _glUniform1dv );
            LoadOpenGLFunction( "glUniform2dv", out _glUniform2dv );
            LoadOpenGLFunction( "glUniform3dv", out _glUniform3dv );
            LoadOpenGLFunction( "glUniform4dv", out _glUniform4dv );
            LoadOpenGLFunction( "glUniformMatrix2dv", out _glUniformMatrix2dv );
            LoadOpenGLFunction( "glUniformMatrix3dv", out _glUniformMatrix3dv );
            LoadOpenGLFunction( "glUniformMatrix4dv", out _glUniformMatrix4dv );
            LoadOpenGLFunction( "glUniformMatrix2x3dv", out _glUniformMatrix2x3dv );
            LoadOpenGLFunction( "glUniformMatrix2x4dv", out _glUniformMatrix2x4dv );
            LoadOpenGLFunction( "glUniformMatrix3x2dv", out _glUniformMatrix3x2dv );
            LoadOpenGLFunction( "glUniformMatrix3x4dv", out _glUniformMatrix3x4dv );
            LoadOpenGLFunction( "glUniformMatrix4x2dv", out _glUniformMatrix4x2dv );
            LoadOpenGLFunction( "glUniformMatrix4x3dv", out _glUniformMatrix4x3dv );
            LoadOpenGLFunction( "glGetUniformdv", out _glGetUniformdv );
            LoadOpenGLFunction( "glGetSubroutineUniformLocation", out _glGetSubroutineUniformLocation );
            LoadOpenGLFunction( "glGetSubroutineIndex", out _glGetSubroutineIndex );
            LoadOpenGLFunction( "glGetActiveSubroutineUniformiv", out _glGetActiveSubroutineUniformiv );
            LoadOpenGLFunction( "glGetActiveSubroutineUniformName", out _glGetActiveSubroutineUniformName );
            LoadOpenGLFunction( "glGetActiveSubroutineName", out _glGetActiveSubroutineName );
            LoadOpenGLFunction( "glUniformSubroutinesuiv", out _glUniformSubroutinesuiv );
            LoadOpenGLFunction( "glGetUniformSubroutineuiv", out _glGetUniformSubroutineuiv );
            LoadOpenGLFunction( "glGetProgramStageiv", out _glGetProgramStageiv );
            LoadOpenGLFunction( "glPatchParameteri", out _glPatchParameteri );
            LoadOpenGLFunction( "glPatchParameterfv", out _glPatchParameterfv );
            LoadOpenGLFunction( "glBindTransformFeedback", out _glBindTransformFeedback );
            LoadOpenGLFunction( "glDeleteTransformFeedbacks", out _glDeleteTransformFeedbacks );
            LoadOpenGLFunction( "glGenTransformFeedbacks", out _glGenTransformFeedbacks );
            LoadOpenGLFunction( "glIsTransformFeedback", out _glIsTransformFeedback );
            LoadOpenGLFunction( "glPauseTransformFeedback", out _glPauseTransformFeedback );
            LoadOpenGLFunction( "glResumeTransformFeedback", out _glResumeTransformFeedback );
            LoadOpenGLFunction( "glDrawTransformFeedback", out _glDrawTransformFeedback );
            LoadOpenGLFunction( "glDrawTransformFeedbackStream", out _glDrawTransformFeedbackStream );
            LoadOpenGLFunction( "glBeginQueryIndexed", out _glBeginQueryIndexed );
            LoadOpenGLFunction( "glEndQueryIndexed", out _glEndQueryIndexed );
            LoadOpenGLFunction( "glGetQueryIndexediv", out _glGetQueryIndexediv );
        }

        if ( _ogl41 || _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glReleaseShaderCompiler", out _glReleaseShaderCompiler );
            LoadOpenGLFunction( "glShaderBinary", out _glShaderBinary );
            LoadOpenGLFunction( "glGetShaderPrecisionFormat", out _glGetShaderPrecisionFormat );
            LoadOpenGLFunction( "glDepthRangef", out _glDepthRangef );
            LoadOpenGLFunction( "glClearDepthf", out _glClearDepthf );
            LoadOpenGLFunction( "glGetProgramBinary", out _glGetProgramBinary );
            LoadOpenGLFunction( "glProgramBinary", out _glProgramBinary );
            LoadOpenGLFunction( "glProgramParameteri", out _glProgramParameteri );
            LoadOpenGLFunction( "glUseProgramStages", out _glUseProgramStages );
            LoadOpenGLFunction( "glActiveShaderProgram", out _glActiveShaderProgram );
            LoadOpenGLFunction( "glCreateShaderProgramv", out _glCreateShaderProgramv );
            LoadOpenGLFunction( "glBindProgramPipeline", out _glBindProgramPipeline );
            LoadOpenGLFunction( "glDeleteProgramPipelines", out _glDeleteProgramPipelines );
            LoadOpenGLFunction( "glGenProgramPipelines", out _glGenProgramPipelines );
            LoadOpenGLFunction( "glIsProgramPipeline", out _glIsProgramPipeline );
            LoadOpenGLFunction( "glGetProgramPipelineiv", out _glGetProgramPipelineiv );
            LoadOpenGLFunction( "glProgramUniform1i", out _glProgramUniform1i );
            LoadOpenGLFunction( "glProgramUniform1iv", out _glProgramUniform1iv );
            LoadOpenGLFunction( "glProgramUniform1f", out _glProgramUniform1f );
            LoadOpenGLFunction( "glProgramUniform1fv", out _glProgramUniform1fv );
            LoadOpenGLFunction( "glProgramUniform1d", out _glProgramUniform1d );
            LoadOpenGLFunction( "glProgramUniform1dv", out _glProgramUniform1dv );
            LoadOpenGLFunction( "glProgramUniform1ui", out _glProgramUniform1ui );
            LoadOpenGLFunction( "glProgramUniform1uiv", out _glProgramUniform1uiv );
            LoadOpenGLFunction( "glProgramUniform2i", out _glProgramUniform2i );
            LoadOpenGLFunction( "glProgramUniform2iv", out _glProgramUniform2iv );
            LoadOpenGLFunction( "glProgramUniform2f", out _glProgramUniform2f );
            LoadOpenGLFunction( "glProgramUniform2fv", out _glProgramUniform2fv );
            LoadOpenGLFunction( "glProgramUniform2d", out _glProgramUniform2d );
            LoadOpenGLFunction( "glProgramUniform2dv", out _glProgramUniform2dv );
            LoadOpenGLFunction( "glProgramUniform2ui", out _glProgramUniform2ui );
            LoadOpenGLFunction( "glProgramUniform2uiv", out _glProgramUniform2uiv );
            LoadOpenGLFunction( "glProgramUniform3i", out _glProgramUniform3i );
            LoadOpenGLFunction( "glProgramUniform3iv", out _glProgramUniform3iv );
            LoadOpenGLFunction( "glProgramUniform3f", out _glProgramUniform3f );
            LoadOpenGLFunction( "glProgramUniform3fv", out _glProgramUniform3fv );
            LoadOpenGLFunction( "glProgramUniform3d", out _glProgramUniform3d );
            LoadOpenGLFunction( "glProgramUniform3dv", out _glProgramUniform3dv );
            LoadOpenGLFunction( "glProgramUniform3ui", out _glProgramUniform3ui );
            LoadOpenGLFunction( "glProgramUniform3uiv", out _glProgramUniform3uiv );
            LoadOpenGLFunction( "glProgramUniform4i", out _glProgramUniform4i );
            LoadOpenGLFunction( "glProgramUniform4iv", out _glProgramUniform4iv );
            LoadOpenGLFunction( "glProgramUniform4f", out _glProgramUniform4f );
            LoadOpenGLFunction( "glProgramUniform4fv", out _glProgramUniform4fv );
            LoadOpenGLFunction( "glProgramUniform4d", out _glProgramUniform4d );
            LoadOpenGLFunction( "glProgramUniform4dv", out _glProgramUniform4dv );
            LoadOpenGLFunction( "glProgramUniform4ui", out _glProgramUniform4ui );
            LoadOpenGLFunction( "glProgramUniform4uiv", out _glProgramUniform4uiv );
            LoadOpenGLFunction( "glProgramUniformMatrix2fv", out _glProgramUniformMatrix2fv );
            LoadOpenGLFunction( "glProgramUniformMatrix3fv", out _glProgramUniformMatrix3fv );
            LoadOpenGLFunction( "glProgramUniformMatrix4fv", out _glProgramUniformMatrix4fv );
            LoadOpenGLFunction( "glProgramUniformMatrix2dv", out _glProgramUniformMatrix2dv );
            LoadOpenGLFunction( "glProgramUniformMatrix3dv", out _glProgramUniformMatrix3dv );
            LoadOpenGLFunction( "glProgramUniformMatrix4dv", out _glProgramUniformMatrix4dv );
            LoadOpenGLFunction( "glProgramUniformMatrix2x3fv", out _glProgramUniformMatrix2x3fv );
            LoadOpenGLFunction( "glProgramUniformMatrix3x2fv", out _glProgramUniformMatrix3x2fv );
            LoadOpenGLFunction( "glProgramUniformMatrix2x4fv", out _glProgramUniformMatrix2x4fv );
            LoadOpenGLFunction( "glProgramUniformMatrix4x2fv", out _glProgramUniformMatrix4x2fv );
            LoadOpenGLFunction( "glProgramUniformMatrix3x4fv", out _glProgramUniformMatrix3x4fv );
            LoadOpenGLFunction( "glProgramUniformMatrix4x3fv", out _glProgramUniformMatrix4x3fv );
            LoadOpenGLFunction( "glProgramUniformMatrix2x3dv", out _glProgramUniformMatrix2x3dv );
            LoadOpenGLFunction( "glProgramUniformMatrix3x2dv", out _glProgramUniformMatrix3x2dv );
            LoadOpenGLFunction( "glProgramUniformMatrix2x4dv", out _glProgramUniformMatrix2x4dv );
            LoadOpenGLFunction( "glProgramUniformMatrix4x2dv", out _glProgramUniformMatrix4x2dv );
            LoadOpenGLFunction( "glProgramUniformMatrix3x4dv", out _glProgramUniformMatrix3x4dv );
            LoadOpenGLFunction( "glProgramUniformMatrix4x3dv", out _glProgramUniformMatrix4x3dv );
            LoadOpenGLFunction( "glValidateProgramPipeline", out _glValidateProgramPipeline );
            LoadOpenGLFunction( "glGetProgramPipelineInfoLog", out _glGetProgramPipelineInfoLog );
            LoadOpenGLFunction( "glVertexAttribL1d", out _glVertexAttribL1d );
            LoadOpenGLFunction( "glVertexAttribL2d", out _glVertexAttribL2d );
            LoadOpenGLFunction( "glVertexAttribL3d", out _glVertexAttribL3d );
            LoadOpenGLFunction( "glVertexAttribL4d", out _glVertexAttribL4d );
            LoadOpenGLFunction( "glVertexAttribL1dv", out _glVertexAttribL1dv );
            LoadOpenGLFunction( "glVertexAttribL2dv", out _glVertexAttribL2dv );
            LoadOpenGLFunction( "glVertexAttribL3dv", out _glVertexAttribL3dv );
            LoadOpenGLFunction( "glVertexAttribL4dv", out _glVertexAttribL4dv );
            LoadOpenGLFunction( "glVertexAttribLPointer", out _glVertexAttribLPointer );
            LoadOpenGLFunction( "glGetVertexAttribLdv", out _glGetVertexAttribLdv );
            LoadOpenGLFunction( "glViewportArrayv", out _glViewportArrayv );
            LoadOpenGLFunction( "glViewportIndexedf", out _glViewportIndexedf );
            LoadOpenGLFunction( "glViewportIndexedfv", out _glViewportIndexedfv );
            LoadOpenGLFunction( "glScissorArrayv", out _glScissorArrayv );
            LoadOpenGLFunction( "glScissorIndexed", out _glScissorIndexed );
            LoadOpenGLFunction( "glScissorIndexedv", out _glScissorIndexedv );
            LoadOpenGLFunction( "glDepthRangeArrayv", out _glDepthRangeArrayv );
            LoadOpenGLFunction( "glDepthRangeIndexed", out _glDepthRangeIndexed );
            LoadOpenGLFunction( "glGetFloati_v", out _glGetFloati_v );
            LoadOpenGLFunction( "glGetDoublei_v", out _glGetDoublei_v );
        }

        if ( _ogl42 || _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glDrawArraysInstancedBaseInstance", out _glDrawArraysInstancedBaseInstance );
            LoadOpenGLFunction( "glDrawElementsInstancedBaseInstance", out _glDrawElementsInstancedBaseInstance );
            LoadOpenGLFunction( "glDrawElementsInstancedBaseVertexBaseInstance", out _glDrawElementsInstancedBaseVertexBaseInstance );
            LoadOpenGLFunction( "glGetInternalformativ", out _glGetInternalformativ );
            LoadOpenGLFunction( "glGetActiveAtomicCounterBufferiv", out _glGetActiveAtomicCounterBufferiv );
            LoadOpenGLFunction( "glBindImageTexture", out _glBindImageTexture );
            LoadOpenGLFunction( "glMemoryBarrier", out _glMemoryBarrier );
            LoadOpenGLFunction( "glTexStorage1D", out _glTexStorage1D );
            LoadOpenGLFunction( "glTexStorage2D", out _glTexStorage2D );
            LoadOpenGLFunction( "glTexStorage3D", out _glTexStorage3D );
            LoadOpenGLFunction( "glDrawTransformFeedbackInstanced", out _glDrawTransformFeedbackInstanced );
            LoadOpenGLFunction( "glDrawTransformFeedbackStreamInstanced", out _glDrawTransformFeedbackStreamInstanced );
        }

        if ( _ogl43 || _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glGetPointerv", out _glGetPointerv );
            LoadOpenGLFunction( "glClearBufferData", out _glClearBufferData );
            LoadOpenGLFunction( "glClearBufferSubData", out _glClearBufferSubData );
            LoadOpenGLFunction( "glDispatchCompute", out _glDispatchCompute );
            LoadOpenGLFunction( "glDispatchComputeIndirect", out _glDispatchComputeIndirect );
            LoadOpenGLFunction( "glCopyImageSubData", out _glCopyImageSubData );
            LoadOpenGLFunction( "glFramebufferParameteri", out _glFramebufferParameteri );
            LoadOpenGLFunction( "glGetFramebufferParameteriv", out _glGetFramebufferParameteriv );
            LoadOpenGLFunction( "glGetInternalformati64v", out _glGetInternalformati64v );
            LoadOpenGLFunction( "glInvalidateTexSubImage", out _glInvalidateTexSubImage );
            LoadOpenGLFunction( "glInvalidateTexImage", out _glInvalidateTexImage );
            LoadOpenGLFunction( "glInvalidateBufferSubData", out _glInvalidateBufferSubData );
            LoadOpenGLFunction( "glInvalidateBufferData", out _glInvalidateBufferData );
            LoadOpenGLFunction( "glInvalidateFramebuffer", out _glInvalidateFramebuffer );
            LoadOpenGLFunction( "glInvalidateSubFramebuffer", out _glInvalidateSubFramebuffer );
            LoadOpenGLFunction( "glMultiDrawArraysIndirect", out _glMultiDrawArraysIndirect );
            LoadOpenGLFunction( "glMultiDrawElementsIndirect", out _glMultiDrawElementsIndirect );
            LoadOpenGLFunction( "glGetProgramInterfaceiv", out _glGetProgramInterfaceiv );
            LoadOpenGLFunction( "glGetProgramResourceIndex", out _glGetProgramResourceIndex );
            LoadOpenGLFunction( "glGetProgramResourceName", out _glGetProgramResourceName );
            LoadOpenGLFunction( "glGetProgramResourceiv", out _glGetProgramResourceiv );
            LoadOpenGLFunction( "glGetProgramResourceLocation", out _glGetProgramResourceLocation );
            LoadOpenGLFunction( "glGetProgramResourceLocationIndex", out _glGetProgramResourceLocationIndex );
            LoadOpenGLFunction( "glShaderStorageBlockBinding", out _glShaderStorageBlockBinding );
            LoadOpenGLFunction( "glTexBufferRange", out _glTexBufferRange );
            LoadOpenGLFunction( "glTexStorage2DMultisample", out _glTexStorage2DMultisample );
            LoadOpenGLFunction( "glTexStorage3DMultisample", out _glTexStorage3DMultisample );
            LoadOpenGLFunction( "glTextureView", out _glTextureView );
            LoadOpenGLFunction( "glBindVertexBuffer", out _glBindVertexBuffer );
            LoadOpenGLFunction( "glVertexAttribFormat", out _glVertexAttribFormat );
            LoadOpenGLFunction( "glVertexAttribIFormat", out _glVertexAttribIFormat );
            LoadOpenGLFunction( "glVertexAttribLFormat", out _glVertexAttribLFormat );
            LoadOpenGLFunction( "glVertexAttribBinding", out _glVertexAttribBinding );
            LoadOpenGLFunction( "glVertexBindingDivisor", out _glVertexBindingDivisor );
            LoadOpenGLFunction( "glDebugMessageControl", out _glDebugMessageControl );
            LoadOpenGLFunction( "glDebugMessageInsert", out _glDebugMessageInsert );
            LoadOpenGLFunction( "glDebugMessageCallback", out _glDebugMessageCallback );
            LoadOpenGLFunction( "glGetDebugMessageLog", out _glGetDebugMessageLog );
            LoadOpenGLFunction( "glPushDebugGroup", out _glPushDebugGroup );
            LoadOpenGLFunction( "glPopDebugGroup", out _glPopDebugGroup );
            LoadOpenGLFunction( "glObjectLabel", out _glObjectLabel );
            LoadOpenGLFunction( "glGetObjectLabel", out _glGetObjectLabel );
            LoadOpenGLFunction( "glObjectPtrLabel", out _glObjectPtrLabel );
            LoadOpenGLFunction( "glGetObjectPtrLabel", out _glGetObjectPtrLabel );
        }

        if ( _ogl44 || _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glBufferStorage", out _glBufferStorage );
            LoadOpenGLFunction( "glClearTexImage", out _glClearTexImage );
            LoadOpenGLFunction( "glClearTexSubImage", out _glClearTexSubImage );
            LoadOpenGLFunction( "glBindBuffersBase", out _glBindBuffersBase );
            LoadOpenGLFunction( "glBindBuffersRange", out _glBindBuffersRange );
            LoadOpenGLFunction( "glBindTextures", out _glBindTextures );
            LoadOpenGLFunction( "glBindSamplers", out _glBindSamplers );
            LoadOpenGLFunction( "glBindImageTextures", out _glBindImageTextures );
            LoadOpenGLFunction( "glBindVertexBuffers", out _glBindVertexBuffers );
        }

        if ( _ogl45 || _ogl46 )
        {
            LoadOpenGLFunction( "glClipControl", out _glClipControl );
            LoadOpenGLFunction( "glCreateTransformFeedbacks", out _glCreateTransformFeedbacks );
            LoadOpenGLFunction( "glTransformFeedbackBufferBase", out _glTransformFeedbackBufferBase );
            LoadOpenGLFunction( "glTransformFeedbackBufferRange", out _glTransformFeedbackBufferRange );
            LoadOpenGLFunction( "glGetTransformFeedbackiv", out _glGetTransformFeedbackiv );
            LoadOpenGLFunction( "glGetTransformFeedbacki_v", out _glGetTransformFeedbacki_v );
            LoadOpenGLFunction( "glGetTransformFeedbacki64_v", out _glGetTransformFeedbacki64_v );

            LoadOpenGLFunction( "glCreateBuffers", out _glCreateBuffers );
            LoadOpenGLFunction( "glNamedBufferStorage", out _glNamedBufferStorage );
            LoadOpenGLFunction( "glNamedBufferData", out _glNamedBufferData );
            LoadOpenGLFunction( "glNamedBufferSubData", out _glNamedBufferSubData );
            LoadOpenGLFunction( "glCopyNamedBufferSubData", out _glCopyNamedBufferSubData );
            LoadOpenGLFunction( "glClearNamedBufferData", out _glClearNamedBufferData );
            LoadOpenGLFunction( "glClearNamedBufferSubData", out _glClearNamedBufferSubData );
            LoadOpenGLFunction( "glMapNamedBuffer", out _glMapNamedBuffer );
            LoadOpenGLFunction( "glMapNamedBufferRange", out _glMapNamedBufferRange );
            LoadOpenGLFunction( "glUnmapNamedBuffer", out _glUnmapNamedBuffer );
            LoadOpenGLFunction( "glFlushMappedNamedBufferRange", out _glFlushMappedNamedBufferRange );
            LoadOpenGLFunction( "glGetNamedBufferParameteriv", out _glGetNamedBufferParameteriv );
            LoadOpenGLFunction( "glGetNamedBufferParameteri64v", out _glGetNamedBufferParameteri64v );
            LoadOpenGLFunction( "glGetNamedBufferPointerv", out _glGetNamedBufferPointerv );
            LoadOpenGLFunction( "glGetNamedBufferSubData", out _glGetNamedBufferSubData );
            LoadOpenGLFunction( "glCreateFramebuffers", out _glCreateFramebuffers );
            LoadOpenGLFunction( "glNamedFramebufferRenderbuffer", out _glNamedFramebufferRenderbuffer );
            LoadOpenGLFunction( "glNamedFramebufferParameteri", out _glNamedFramebufferParameteri );
            LoadOpenGLFunction( "glNamedFramebufferTexture", out _glNamedFramebufferTexture );
            LoadOpenGLFunction( "glNamedFramebufferTextureLayer", out _glNamedFramebufferTextureLayer );
            LoadOpenGLFunction( "glNamedFramebufferDrawBuffer", out _glNamedFramebufferDrawBuffer );
            LoadOpenGLFunction( "glNamedFramebufferDrawBuffers", out _glNamedFramebufferDrawBuffers );
            LoadOpenGLFunction( "glNamedFramebufferReadBuffer", out _glNamedFramebufferReadBuffer );
            LoadOpenGLFunction( "glInvalidateNamedFramebufferData", out _glInvalidateNamedFramebufferData );
            LoadOpenGLFunction( "glInvalidateNamedFramebufferSubData", out _glInvalidateNamedFramebufferSubData );
            LoadOpenGLFunction( "glClearNamedFramebufferiv", out _glClearNamedFramebufferiv );
            LoadOpenGLFunction( "glClearNamedFramebufferuiv", out _glClearNamedFramebufferuiv );
            LoadOpenGLFunction( "glClearNamedFramebufferfv", out _glClearNamedFramebufferfv );
            LoadOpenGLFunction( "glClearNamedFramebufferfi", out _glClearNamedFramebufferfi );
            LoadOpenGLFunction( "glBlitNamedFramebuffer", out _glBlitNamedFramebuffer );
            LoadOpenGLFunction( "glCheckNamedFramebufferStatus", out _glCheckNamedFramebufferStatus );
            LoadOpenGLFunction( "glGetNamedFramebufferParameteriv", out _glGetNamedFramebufferParameteriv );
            LoadOpenGLFunction( "glGetNamedFramebufferAttachmentParameteriv", out _glGetNamedFramebufferAttachmentParameteriv );
            LoadOpenGLFunction( "glCreateRenderbuffers", out _glCreateRenderbuffers );
            LoadOpenGLFunction( "glNamedRenderbufferStorage", out _glNamedRenderbufferStorage );
            LoadOpenGLFunction( "glNamedRenderbufferStorageMultisample", out _glNamedRenderbufferStorageMultisample );
            LoadOpenGLFunction( "glGetNamedRenderbufferParameteriv", out _glGetNamedRenderbufferParameteriv );
            LoadOpenGLFunction( "glCreateTextures", out _glCreateTextures );
            LoadOpenGLFunction( "glTextureBuffer", out _glTextureBuffer );
            LoadOpenGLFunction( "glTextureBufferRange", out _glTextureBufferRange );
            LoadOpenGLFunction( "glTextureStorage1D", out _glTextureStorage1D );
            LoadOpenGLFunction( "glTextureStorage2D", out _glTextureStorage2D );
            LoadOpenGLFunction( "glTextureStorage3D", out _glTextureStorage3D );
            LoadOpenGLFunction( "glTextureStorage2DMultisample", out _glTextureStorage2DMultisample );
            LoadOpenGLFunction( "glTextureStorage3DMultisample", out _glTextureStorage3DMultisample );
            LoadOpenGLFunction( "glTextureSubImage1D", out _glTextureSubImage1D );
            LoadOpenGLFunction( "glTextureSubImage2D", out _glTextureSubImage2D );
            LoadOpenGLFunction( "glTextureSubImage3D", out _glTextureSubImage3D );
            LoadOpenGLFunction( "glCompressedTextureSubImage1D", out _glCompressedTextureSubImage1D );
            LoadOpenGLFunction( "glCompressedTextureSubImage2D", out _glCompressedTextureSubImage2D );
            LoadOpenGLFunction( "glCompressedTextureSubImage3D", out _glCompressedTextureSubImage3D );
            LoadOpenGLFunction( "glCopyTextureSubImage1D", out _glCopyTextureSubImage1D );
            LoadOpenGLFunction( "glCopyTextureSubImage2D", out _glCopyTextureSubImage2D );
            LoadOpenGLFunction( "glCopyTextureSubImage3D", out _glCopyTextureSubImage3D );
            LoadOpenGLFunction( "glTextureParameterf", out _glTextureParameterf );
            LoadOpenGLFunction( "glTextureParameterfv", out _glTextureParameterfv );
            LoadOpenGLFunction( "glTextureParameteri", out _glTextureParameteri );
            LoadOpenGLFunction( "glTextureParameterIiv", out _glTextureParameterIiv );
            LoadOpenGLFunction( "glTextureParameterIuiv", out _glTextureParameterIuiv );
            LoadOpenGLFunction( "glTextureParameteriv", out _glTextureParameteriv );
            LoadOpenGLFunction( "glGenerateTextureMipmap", out _glGenerateTextureMipmap );
            LoadOpenGLFunction( "glBindTextureUnit", out _glBindTextureUnit );
            LoadOpenGLFunction( "glGetTextureImage", out _glGetTextureImage );
            LoadOpenGLFunction( "glGetCompressedTextureImage", out _glGetCompressedTextureImage );
            LoadOpenGLFunction( "glGetTextureLevelParameterfv", out _glGetTextureLevelParameterfv );
            LoadOpenGLFunction( "glGetTextureLevelParameteriv", out _glGetTextureLevelParameteriv );
            LoadOpenGLFunction( "glGetTextureParameterfv", out _glGetTextureParameterfv );
            LoadOpenGLFunction( "glGetTextureParameterIiv", out _glGetTextureParameterIiv );
            LoadOpenGLFunction( "glGetTextureParameterIuiv", out _glGetTextureParameterIuiv );
            LoadOpenGLFunction( "glGetTextureParameteriv", out _glGetTextureParameteriv );
            LoadOpenGLFunction( "glCreateVertexArrays", out _glCreateVertexArrays );
            LoadOpenGLFunction( "glDisableVertexArrayAttrib", out _glDisableVertexArrayAttrib );
            LoadOpenGLFunction( "glEnableVertexArrayAttrib", out _glEnableVertexArrayAttrib );
            LoadOpenGLFunction( "glVertexArrayElementBuffer", out _glVertexArrayElementBuffer );
            LoadOpenGLFunction( "glVertexArrayVertexBuffer", out _glVertexArrayVertexBuffer );
            LoadOpenGLFunction( "glVertexArrayVertexBuffers", out _glVertexArrayVertexBuffers );
            LoadOpenGLFunction( "glVertexArrayAttribBinding", out _glVertexArrayAttribBinding );
            LoadOpenGLFunction( "glVertexArrayAttribFormat", out _glVertexArrayAttribFormat );
            LoadOpenGLFunction( "glVertexArrayAttribIFormat", out _glVertexArrayAttribIFormat );
            LoadOpenGLFunction( "glVertexArrayAttribLFormat", out _glVertexArrayAttribLFormat );
            LoadOpenGLFunction( "glVertexArrayBindingDivisor", out _glVertexArrayBindingDivisor );
            LoadOpenGLFunction( "glGetVertexArrayiv", out _glGetVertexArrayiv );
            LoadOpenGLFunction( "glGetVertexArrayIndexediv", out _glGetVertexArrayIndexediv );
            LoadOpenGLFunction( "glGetVertexArrayIndexed64iv", out _glGetVertexArrayIndexed64iv );
            LoadOpenGLFunction( "glCreateSamplers", out _glCreateSamplers );
            LoadOpenGLFunction( "glCreateProgramPipelines", out _glCreateProgramPipelines );
            LoadOpenGLFunction( "glCreateQueries", out _glCreateQueries );
            LoadOpenGLFunction( "glGetQueryBufferObjecti64v", out _glGetQueryBufferObjecti64v );
            LoadOpenGLFunction( "glGetQueryBufferObjectiv", out _glGetQueryBufferObjectiv );
            LoadOpenGLFunction( "glGetQueryBufferObjectui64v", out _glGetQueryBufferObjectui64v );
            LoadOpenGLFunction( "glGetQueryBufferObjectuiv", out _glGetQueryBufferObjectuiv );
            LoadOpenGLFunction( "glMemoryBarrierByRegion", out _glMemoryBarrierByRegion );
            LoadOpenGLFunction( "glGetTextureSubImage", out _glGetTextureSubImage );
            LoadOpenGLFunction( "glGetCompressedTextureSubImage", out _glGetCompressedTextureSubImage );
            LoadOpenGLFunction( "glGetGraphicsResetStatus", out _glGetGraphicsResetStatus );
            LoadOpenGLFunction( "glGetnCompressedTexImage", out _glGetnCompressedTexImage );
            LoadOpenGLFunction( "glGetnUniformfv", out _glGetnUniformfv );
            LoadOpenGLFunction( "glGetnUniformiv", out _glGetnUniformiv );
            LoadOpenGLFunction( "glGetnUniformuiv", out _glGetnUniformuiv );
            LoadOpenGLFunction( "glReadnPixels", out _glReadnPixels );
            LoadOpenGLFunction( "glTextureBarrier", out _glTextureBarrier );

//            _glGetnTexImage                             = Marshal.GetDelegateForFunctionPointer< PFNGLGETNTEXIMAGEPROC                                         >( "glGetnTexImage", out 
//            _glGetnUniformdv                            = Marshal.GetDelegateForFunctionPointer< PFNGLGETNUNIFORMDVPROC                                         >( "glGetnUniformdv", out 
        }

        if ( _ogl46 )
        {
            LoadOpenGLFunction( "glSpecializeShader", out _glSpecializeShader );
            LoadOpenGLFunction( "glPolygonOffsetClamp", out _glPolygonOffsetClamp );

//            _glMultiDrawArraysIndirectCount   = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC                                         >( "glMultiDrawArraysIndirectCount", out 
//            _glMultiDrawElementsIndirectCount = Marshal.GetDelegateForFunctionPointer< PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC                                         >( "glMultiDrawElementsIndirectCount", out 
        }
    }

    // ========================================================================
    // ========================================================================

    // Import wglGetProcAddress
    [DllImport( "opengl32.dll", EntryPoint = "wglGetProcAddress", CallingConvention = CallingConvention.StdCall )]
    private static extern IntPtr wglGetProcAddress( string procname );

    // ========================================================================
    // ========================================================================

    private static readonly Dictionary< string, Delegate > _loadedFunctions = new();

    public static bool LoadOpenGLFunction< T >( string functionName, out T functionDelegate ) where T : Delegate
    {
        Logger.Debug( $"Loading OGL Function: {functionName}" );
        
        if ( _loadedFunctions.TryGetValue( functionName, out var existingDelegate ) )
        {
            functionDelegate = ( T )existingDelegate; // Cast to the requested delegate type

            return true; // Already loaded
        }

        var functionPtr = wglGetProcAddress( functionName );

        if ( functionPtr == IntPtr.Zero )
        {
            functionPtr = Glfw.GetProcAddress( functionName );
        }

        if ( functionPtr != IntPtr.Zero )
        {
            try
            {
                var del = Marshal.GetDelegateForFunctionPointer< T >( functionPtr );

                _loadedFunctions.Add( functionName, del ); // Cache the loaded function

                functionDelegate = del;

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
}

//#pragma warning restore IDE0079 // Remove unnecessary suppression
//#pragma warning restore CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
//#pragma warning restore CS8603  // Possible null reference return.
//#pragma warning restore IDE0060 // Remove unused parameter.
//#pragma warning restore IDE1006 // Naming Styles.
//#pragma warning restore IDE0090 // Use 'new(...)'.
//#pragma warning restore CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type