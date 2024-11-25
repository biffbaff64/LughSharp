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

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning disable CS8603  // Possible null reference return.
#pragma warning disable IDE0060 // Remove unused parameter.
#pragma warning disable IDE1006 // Naming Styles.
#pragma warning disable IDE0090 // Use 'new(...)'.
#pragma warning disable CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type

// ============================================================================

namespace Corelib.LibCore.Graphics.OpenGL;

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

public unsafe partial class NewGLBindings
{
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCULLFACEPROC( GLenum mode );
    private PFNGLCULLFACEPROC _glCullFace;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFRONTFACEPROC( GLenum mode );
    private PFNGLFRONTFACEPROC _glFrontFace;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLHINTPROC( GLenum target, GLenum mode );
    private PFNGLHINTPROC _glHint;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLLINEWIDTHPROC( GLfloat width );
    private PFNGLLINEWIDTHPROC _glLineWidth;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPOINTSIZEPROC( GLfloat size );
    private PFNGLPOINTSIZEPROC _glPointSize;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPOLYGONMODEPROC( GLenum face, GLenum mode );
    private PFNGLPOLYGONMODEPROC _glPolygonMode;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSCISSORPROC( GLint x, GLint y, GLsizei width, GLsizei height );
    private PFNGLSCISSORPROC _glScissor;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXPARAMETERFPROC( GLenum target, GLenum pname, GLfloat param );
    private PFNGLTEXPARAMETERFPROC _glTexParameterf;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXPARAMETERFVPROC( GLenum target, GLenum pname, GLfloat* @params );
    private PFNGLTEXPARAMETERFVPROC _glTexParameterfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXPARAMETERIPROC( GLenum target, GLenum pname, GLint param );
    private PFNGLTEXPARAMETERIPROC _glTexParameteri;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXPARAMETERIVPROC( GLenum target, GLenum pname, GLint* @params );
    private PFNGLTEXPARAMETERIVPROC _glTexParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXIMAGE1DPROC( GLenum target,
                                               GLint level,
                                               GLenum internalformat,
                                               GLsizei width,
                                               GLint border,
                                               GLenum format,
                                               GLenum type,
                                               void* pixels );
    private PFNGLTEXIMAGE1DPROC _glTexImage1D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXIMAGE2DPROC( GLenum target,
                                               GLint level,
                                               GLenum internalformat,
                                               GLsizei width,
                                               GLsizei height,
                                               GLint border,
                                               GLenum format,
                                               GLenum type,
                                               void* pixels );
    private PFNGLTEXIMAGE2DPROC _glTexImage2D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWBUFFERPROC( GLenum buf );
    private PFNGLDRAWBUFFERPROC _glDrawBuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARPROC( GLbitfield mask );
    private PFNGLCLEARPROC _glClear;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARCOLORPROC( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );
    private PFNGLCLEARCOLORPROC _glClearColor;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARSTENCILPROC( GLint s );
    private PFNGLCLEARSTENCILPROC _glClearStencil;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARDEPTHPROC( GLdouble depth );
    private PFNGLCLEARDEPTHPROC _glClearDepth;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSTENCILMASKPROC( GLuint mask );
    private PFNGLSTENCILMASKPROC _glStencilMask;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOLORMASKPROC( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha );
    private PFNGLCOLORMASKPROC _glColorMask;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDEPTHMASKPROC( GLboolean flag );
    private PFNGLDEPTHMASKPROC _glDepthMask;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDISABLEPROC( GLenum cap );
    private PFNGLDISABLEPROC _glDisable;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLENABLEPROC( GLenum cap );
    private PFNGLENABLEPROC _glEnable;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFINISHPROC();
    private PFNGLFINISHPROC _glFinish;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFLUSHPROC();
    private PFNGLFLUSHPROC _glFlush;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBLENDFUNCPROC( GLenum sfactor, GLenum dfactor );
    private PFNGLBLENDFUNCPROC _glBlendFunc;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLLOGICOPPROC( GLenum opcode );
    private PFNGLLOGICOPPROC _glLogicOp;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSTENCILFUNCPROC( GLenum func, GLint @ref, GLuint mask );
    private PFNGLSTENCILFUNCPROC _glStencilFunc;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSTENCILOPPROC( GLenum fail, GLenum zfail, GLenum zpass );
    private PFNGLSTENCILOPPROC _glStencilOp;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDEPTHFUNCPROC( GLenum func );
    private PFNGLDEPTHFUNCPROC _glDepthFunc;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPIXELSTOREFPROC( GLenum pname, GLfloat param );
    private PFNGLPIXELSTOREFPROC _glPixelStoref;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPIXELSTOREIPROC( GLenum pname, GLint param );
    private PFNGLPIXELSTOREIPROC _glPixelStorei;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLREADBUFFERPROC( GLenum src );
    private PFNGLREADBUFFERPROC _glReadBuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLREADPIXELSPROC( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels );
    private PFNGLREADPIXELSPROC _glReadPixels;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETBOOLEANVPROC( GLenum pname, GLboolean* data );
    private PFNGLGETBOOLEANVPROC _glGetBooleanv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETDOUBLEVPROC( GLenum pname, GLdouble* data );
    private PFNGLGETDOUBLEVPROC _glGetDoublev;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLenum PFNGLGETERRORPROC();
    private PFNGLGETERRORPROC _glGetError;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETFLOATVPROC( GLenum pname, GLfloat* data );
    private PFNGLGETFLOATVPROC _glGetFloatv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETINTEGERVPROC( GLenum pname, GLint* data );
    private PFNGLGETINTEGERVPROC _glGetIntegerv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLubyte* PFNGLGETSTRINGPROC( GLenum name );
    private PFNGLGETSTRINGPROC _glGetString;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTEXIMAGEPROC( GLenum target, GLint level, GLenum format, GLenum type, void* pixels );
    private PFNGLGETTEXIMAGEPROC _glGetTexImage;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTEXPARAMETERFVPROC( GLenum target, GLenum pname, GLfloat* @params );
    private PFNGLGETTEXPARAMETERFVPROC _glGetTexParameterfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTEXPARAMETERIVPROC( GLenum target, GLenum pname, GLint* @params );
    private PFNGLGETTEXPARAMETERIVPROC _glGetTexParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTEXLEVELPARAMETERFVPROC( GLenum target, GLint level, GLenum pname, GLfloat* @params );
    private PFNGLGETTEXLEVELPARAMETERFVPROC _glGetTexLevelParameterfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTEXLEVELPARAMETERIVPROC( GLenum target, GLint level, GLenum pname, GLint* @params );
    private PFNGLGETTEXLEVELPARAMETERIVPROC _glGetTexLevelParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISENABLEDPROC( GLenum cap );
    private PFNGLISENABLEDPROC _glIsEnabled;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDEPTHRANGEPROC( GLdouble near, GLdouble far );
    private PFNGLDEPTHRANGEPROC _glDepthRange;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVIEWPORTPROC( GLint x, GLint y, GLsizei width, GLsizei height );
    private PFNGLVIEWPORTPROC _glViewport;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWARRAYSPROC( GLenum mode, GLint first, GLsizei count );
    private PFNGLDRAWARRAYSPROC _glDrawArrays;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWELEMENTSPROC( GLenum mode, GLsizei count, GLenum type, void* indices );
    private PFNGLDRAWELEMENTSPROC _glDrawElements;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPOLYGONOFFSETPROC( GLfloat factor, GLfloat units );
    private PFNGLPOLYGONOFFSETPROC _glPolygonOffset;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOPYTEXIMAGE1DPROC( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border );
    private PFNGLCOPYTEXIMAGE1DPROC _glCopyTexImage1D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOPYTEXIMAGE2DPROC( GLenum target,
                                                   GLint level,
                                                   GLenum internalformat,
                                                   GLint x,
                                                   GLint y,
                                                   GLsizei width,
                                                   GLsizei height,
                                                   GLint border );
    private PFNGLCOPYTEXIMAGE2DPROC _glCopyTexImage2D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOPYTEXSUBIMAGE1DPROC( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width );
    private PFNGLCOPYTEXSUBIMAGE1DPROC _glCopyTexSubImage1D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOPYTEXSUBIMAGE2DPROC( GLenum target,
                                                      GLint level,
                                                      GLint xoffset,
                                                      GLint yoffset,
                                                      GLint x,
                                                      GLint y,
                                                      GLsizei width,
                                                      GLsizei height );
    private PFNGLCOPYTEXSUBIMAGE2DPROC _glCopyTexSubImage2D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXSUBIMAGE1DPROC( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels );
    private PFNGLTEXSUBIMAGE1DPROC _glTexSubImage1D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXSUBIMAGE2DPROC( GLenum target,
                                                  GLint level,
                                                  GLint xoffset,
                                                  GLint yoffset,
                                                  GLsizei width,
                                                  GLsizei height,
                                                  GLenum format,
                                                  GLenum type,
                                                  void* pixels );
    private PFNGLTEXSUBIMAGE2DPROC _glTexSubImage2D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDTEXTUREPROC( GLenum target, GLuint texture );
    private PFNGLBINDTEXTUREPROC _glBindTexture;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETETEXTURESPROC( GLsizei n, GLuint* textures );
    private PFNGLDELETETEXTURESPROC _glDeleteTextures;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENTEXTURESPROC( GLsizei n, GLuint* textures );
    private PFNGLGENTEXTURESPROC _glGenTextures;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISTEXTUREPROC( GLuint texture );
    private PFNGLISTEXTUREPROC _glIsTexture;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWRANGEELEMENTSPROC( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices );
    private PFNGLDRAWRANGEELEMENTSPROC _glDrawRangeElements;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXIMAGE3DPROC( GLenum target,
                                               GLint level,
                                               GLint internalformat,
                                               GLsizei width,
                                               GLsizei height,
                                               GLsizei depth,
                                               GLint border,
                                               GLenum format,
                                               GLenum type,
                                               void* pixels );
    private PFNGLTEXIMAGE3DPROC _glTexImage3D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXSUBIMAGE3DPROC( GLenum target,
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
    private PFNGLTEXSUBIMAGE3DPROC _glTexSubImage3D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOPYTEXSUBIMAGE3DPROC( GLenum target,
                                                      GLint level,
                                                      GLint xoffset,
                                                      GLint yoffset,
                                                      GLint zoffset,
                                                      GLint x,
                                                      GLint y,
                                                      GLsizei width,
                                                      GLsizei height );
    private PFNGLCOPYTEXSUBIMAGE3DPROC _glCopyTexSubImage3D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLACTIVETEXTUREPROC( GLenum texture );
    private PFNGLACTIVETEXTUREPROC _glActiveTexture;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLECOVERAGEPROC( GLfloat value, GLboolean invert );
    private PFNGLSAMPLECOVERAGEPROC _glSampleCoverage;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE3DPROC( GLenum target,
                                                         GLint level,
                                                         GLenum internalformat,
                                                         GLsizei width,
                                                         GLsizei height,
                                                         GLsizei depth,
                                                         GLint border,
                                                         GLsizei imageSize,
                                                         void* data );
    private PFNGLCOMPRESSEDTEXIMAGE3DPROC _glCompressedTexImage3D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE2DPROC( GLenum target,
                                                         GLint level,
                                                         GLenum internalformat,
                                                         GLsizei width,
                                                         GLsizei height,
                                                         GLint border,
                                                         GLsizei imageSize,
                                                         void* data );
    private PFNGLCOMPRESSEDTEXIMAGE2DPROC _glCompressedTexImage2D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE1DPROC( GLenum target,
                                                         GLint level,
                                                         GLenum internalformat,
                                                         GLsizei width,
                                                         GLint border,
                                                         GLsizei imageSize,
                                                         void* data );
    private PFNGLCOMPRESSEDTEXIMAGE1DPROC _glCompressedTexImage1D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC( GLenum target,
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
    private PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC _glCompressedTexSubImage3D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC( GLenum target,
                                                            GLint level,
                                                            GLint xoffset,
                                                            GLint yoffset,
                                                            GLsizei width,
                                                            GLsizei height,
                                                            GLenum format,
                                                            GLsizei imageSize,
                                                            void* data );
    private PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC _glCompressedTexSubImage2D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC( GLenum target,
                                                            GLint level,
                                                            GLint xoffset,
                                                            GLsizei width,
                                                            GLenum format,
                                                            GLsizei imageSize,
                                                            void* data );
    private PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC _glCompressedTexSubImage1D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETCOMPRESSEDTEXIMAGEPROC( GLenum target, GLint level, void* img );
    private PFNGLGETCOMPRESSEDTEXIMAGEPROC _glGetCompressedTexImage;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBLENDFUNCSEPARATEPROC( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha );
    private PFNGLBLENDFUNCSEPARATEPROC _glBlendFuncSeparate;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLMULTIDRAWARRAYSPROC( GLenum mode, GLint* first, GLsizei* count, GLsizei drawcount );
    private PFNGLMULTIDRAWARRAYSPROC _glMultiDrawArrays;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLMULTIDRAWELEMENTSPROC( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount );
    private PFNGLMULTIDRAWELEMENTSPROC _glMultiDrawElements;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPOINTPARAMETERFPROC( GLenum pname, GLfloat param );
    private PFNGLPOINTPARAMETERFPROC _glPointParameterf;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPOINTPARAMETERFVPROC( GLenum pname, GLfloat* @params );
    private PFNGLPOINTPARAMETERFVPROC _glPointParameterfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPOINTPARAMETERIPROC( GLenum pname, GLint param );
    private PFNGLPOINTPARAMETERIPROC _glPointParameteri;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPOINTPARAMETERIVPROC( GLenum pname, GLint* @params );
    private PFNGLPOINTPARAMETERIVPROC _glPointParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBLENDCOLORPROC( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );
    private PFNGLBLENDCOLORPROC _glBlendColor;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBLENDEQUATIONPROC( GLenum mode );
    private PFNGLBLENDEQUATIONPROC _glBlendEquation;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENQUERIESPROC( GLsizei n, GLuint* ids );
    private PFNGLGENQUERIESPROC _glGenQueries;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETEQUERIESPROC( GLsizei n, GLuint* ids );
    private PFNGLDELETEQUERIESPROC _glDeleteQueries;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISQUERYPROC( GLuint id );
    private PFNGLISQUERYPROC _glIsQuery;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBEGINQUERYPROC( GLenum target, GLuint id );
    private PFNGLBEGINQUERYPROC _glBeginQuery;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLENDQUERYPROC( GLenum target );
    private PFNGLENDQUERYPROC _glEndQuery;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETQUERYIVPROC( GLenum target, GLenum pname, GLint* @params );
    private PFNGLGETQUERYIVPROC _glGetQueryiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETQUERYOBJECTIVPROC( GLuint id, GLenum pname, GLint* @params );
    private PFNGLGETQUERYOBJECTIVPROC _glGetQueryObjectiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETQUERYOBJECTUIVPROC( GLuint id, GLenum pname, GLuint* @params );
    private PFNGLGETQUERYOBJECTUIVPROC _glGetQueryObjectuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDBUFFERPROC( GLenum target, GLuint buffer );
    private PFNGLBINDBUFFERPROC _glBindBuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETEBUFFERSPROC( GLsizei n, GLuint* buffers );
    private PFNGLDELETEBUFFERSPROC _glDeleteBuffers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENBUFFERSPROC( GLsizei n, GLuint* buffers );
    private PFNGLGENBUFFERSPROC _glGenBuffers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISBUFFERPROC( GLuint buffer );
    private PFNGLISBUFFERPROC _glIsBuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBUFFERDATAPROC( GLenum target, GLsizeiptr size, void* data, GLenum usage );
    private PFNGLBUFFERDATAPROC _glBufferData;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBUFFERSUBDATAPROC( GLenum target, GLintptr offset, GLsizeiptr size, void* data );
    private PFNGLBUFFERSUBDATAPROC _glBufferSubData;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETBUFFERSUBDATAPROC( GLenum target, GLintptr offset, GLsizeiptr size, void* data );
    private PFNGLGETBUFFERSUBDATAPROC _glGetBufferSubData;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void* PFNGLMAPBUFFERPROC( GLenum target, GLenum access );
    private PFNGLMAPBUFFERPROC _glMapBuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLUNMAPBUFFERPROC( GLenum target );
    private PFNGLUNMAPBUFFERPROC _glUnmapBuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETBUFFERPARAMETERIVPROC( GLenum target, GLenum pname, GLint* @params );
    private PFNGLGETBUFFERPARAMETERIVPROC _glGetBufferParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETBUFFERPOINTERVPROC( GLenum target, GLenum pname, void** @params );
    private PFNGLGETBUFFERPOINTERVPROC _glGetBufferPointerv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBLENDEQUATIONSEPARATEPROC( GLenum modeRGB, GLenum modeAlpha );
    private PFNGLBLENDEQUATIONSEPARATEPROC _glBlendEquationSeparate;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWBUFFERSPROC( GLsizei n, GLenum* bufs );
    private PFNGLDRAWBUFFERSPROC _glDrawBuffers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSTENCILOPSEPARATEPROC( GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass );
    private PFNGLSTENCILOPSEPARATEPROC _glStencilOpSeparate;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSTENCILFUNCSEPARATEPROC( GLenum face, GLenum func, GLint @ref, GLuint mask );
    private PFNGLSTENCILFUNCSEPARATEPROC _glStencilFuncSeparate;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSTENCILMASKSEPARATEPROC( GLenum face, GLuint mask );
    private PFNGLSTENCILMASKSEPARATEPROC _glStencilMaskSeparate;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLATTACHSHADERPROC( GLuint program, GLuint shader );
    private PFNGLATTACHSHADERPROC _glAttachShader;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDATTRIBLOCATIONPROC( GLuint program, GLuint index, GLchar* name );
    private PFNGLBINDATTRIBLOCATIONPROC _glBindAttribLocation;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOMPILESHADERPROC( GLuint shader );
    private PFNGLCOMPILESHADERPROC _glCompileShader;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLuint PFNGLCREATEPROGRAMPROC();
    private PFNGLCREATEPROGRAMPROC _glCreateProgram;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLuint PFNGLCREATESHADERPROC( GLenum type );
    private PFNGLCREATESHADERPROC _glCreateShader;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETEPROGRAMPROC( GLuint program );
    private PFNGLDELETEPROGRAMPROC _glDeleteProgram;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETESHADERPROC( GLuint shader );
    private PFNGLDELETESHADERPROC _glDeleteShader;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDETACHSHADERPROC( GLuint program, GLuint shader );
    private PFNGLDETACHSHADERPROC _glDetachShader;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDISABLEVERTEXATTRIBARRAYPROC( GLuint index );
    private PFNGLDISABLEVERTEXATTRIBARRAYPROC _glDisableVertexAttribArray;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLENABLEVERTEXATTRIBARRAYPROC( GLuint index );
    private PFNGLENABLEVERTEXATTRIBARRAYPROC _glEnableVertexAttribArray;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETACTIVEATTRIBPROC( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name );
    private PFNGLGETACTIVEATTRIBPROC _glGetActiveAttrib;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETACTIVEUNIFORMPROC( GLuint program, GLuint index, GLsizei bufSize, GLsizei* length, GLint* size, GLenum* type, GLchar* name );
    private PFNGLGETACTIVEUNIFORMPROC _glGetActiveUniform;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETATTACHEDSHADERSPROC( GLuint program, GLsizei maxCount, GLsizei* count, GLuint* shaders );
    private PFNGLGETATTACHEDSHADERSPROC _glGetAttachedShaders;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLint PFNGLGETATTRIBLOCATIONPROC( GLuint program, GLchar* name );
    private PFNGLGETATTRIBLOCATIONPROC _glGetAttribLocation;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETPROGRAMIVPROC( GLuint program, GLenum pname, GLint* @params );
    private PFNGLGETPROGRAMIVPROC _glGetProgramiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETPROGRAMINFOLOGPROC( GLuint program, GLsizei bufSize, GLsizei* length, GLchar* infoLog );
    private PFNGLGETPROGRAMINFOLOGPROC _glGetProgramInfoLog;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSHADERIVPROC( GLuint shader, GLenum pname, GLint* @params );
    private PFNGLGETSHADERIVPROC _glGetShaderiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSHADERINFOLOGPROC( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* infoLog );
    private PFNGLGETSHADERINFOLOGPROC _glGetShaderInfoLog;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSHADERSOURCEPROC( GLuint shader, GLsizei bufSize, GLsizei* length, GLchar* source );
    private PFNGLGETSHADERSOURCEPROC _glGetShaderSource;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLint PFNGLGETUNIFORMLOCATIONPROC( GLuint program, GLchar* name );
    private PFNGLGETUNIFORMLOCATIONPROC _glGetUniformLocation;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETUNIFORMFVPROC( GLuint program, GLint location, GLfloat* @params );
    private PFNGLGETUNIFORMFVPROC _glGetUniformfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETUNIFORMIVPROC( GLuint program, GLint location, GLint* @params );
    private PFNGLGETUNIFORMIVPROC _glGetUniformiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETVERTEXATTRIBDVPROC( GLuint index, GLenum pname, GLdouble* @params );
    private PFNGLGETVERTEXATTRIBDVPROC _glGetVertexAttribdv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETVERTEXATTRIBFVPROC( GLuint index, GLenum pname, GLfloat* @params );
    private PFNGLGETVERTEXATTRIBFVPROC _glGetVertexAttribfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETVERTEXATTRIBIVPROC( GLuint index, GLenum pname, GLint* @params );
    private PFNGLGETVERTEXATTRIBIVPROC _glGetVertexAttribiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETVERTEXATTRIBPOINTERVPROC( GLuint index, GLenum pname, void** pointer );
    private PFNGLGETVERTEXATTRIBPOINTERVPROC _glGetVertexAttribPointerv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISPROGRAMPROC( GLuint program );
    private PFNGLISPROGRAMPROC _glIsProgram;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISSHADERPROC( GLuint shader );
    private PFNGLISSHADERPROC _glIsShader;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLLINKPROGRAMPROC( GLuint program );
    private PFNGLLINKPROGRAMPROC _glLinkProgram;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSHADERSOURCEPROC( GLuint shader, GLsizei count, GLchar** @string, GLint* length );
    private PFNGLSHADERSOURCEPROC _glShaderSource;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUSEPROGRAMPROC( GLuint program );
    private PFNGLUSEPROGRAMPROC _glUseProgram;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM1FPROC( GLint location, GLfloat v0 );
    private PFNGLUNIFORM1FPROC _glUniform1f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM2FPROC( GLint location, GLfloat v0, GLfloat v1 );
    private PFNGLUNIFORM2FPROC _glUniform2f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM3FPROC( GLint location, GLfloat v0, GLfloat v1, GLfloat v2 );
    private PFNGLUNIFORM3FPROC _glUniform3f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM4FPROC( GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 );
    private PFNGLUNIFORM4FPROC _glUniform4f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM1IPROC( GLint location, GLint v0 );
    private PFNGLUNIFORM1IPROC _glUniform1i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM2IPROC( GLint location, GLint v0, GLint v1 );
    private PFNGLUNIFORM2IPROC _glUniform2i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM3IPROC( GLint location, GLint v0, GLint v1, GLint v2 );
    private PFNGLUNIFORM3IPROC _glUniform3i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM4IPROC( GLint location, GLint v0, GLint v1, GLint v2, GLint v3 );
    private PFNGLUNIFORM4IPROC _glUniform4i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM1FVPROC( GLint location, GLsizei count, GLfloat* value );
    private PFNGLUNIFORM1FVPROC _glUniform1fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM2FVPROC( GLint location, GLsizei count, GLfloat* value );
    private PFNGLUNIFORM2FVPROC _glUniform2fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM3FVPROC( GLint location, GLsizei count, GLfloat* value );
    private PFNGLUNIFORM3FVPROC _glUniform3fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM4FVPROC( GLint location, GLsizei count, GLfloat* value );
    private PFNGLUNIFORM4FVPROC _glUniform4fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM1IVPROC( GLint location, GLsizei count, GLint* value );
    private PFNGLUNIFORM1IVPROC _glUniform1iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM2IVPROC( GLint location, GLsizei count, GLint* value );
    private PFNGLUNIFORM2IVPROC _glUniform2iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM3IVPROC( GLint location, GLsizei count, GLint* value );
    private PFNGLUNIFORM3IVPROC _glUniform3iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM4IVPROC( GLint location, GLsizei count, GLint* value );
    private PFNGLUNIFORM4IVPROC _glUniform4iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX2FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX2FVPROC _glUniformMatrix2fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX3FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX3FVPROC _glUniformMatrix3fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX4FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX4FVPROC _glUniformMatrix4fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLVALIDATEPROGRAMPROC( GLuint program );
    private PFNGLVALIDATEPROGRAMPROC _glValidateProgram;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB1DPROC( GLuint index, GLdouble x );
    private PFNGLVERTEXATTRIB1DPROC _glVertexAttrib1d;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB1DVPROC( GLuint index, GLdouble* v );
    private PFNGLVERTEXATTRIB1DVPROC _glVertexAttrib1dv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB1FPROC( GLuint index, GLfloat x );
    private PFNGLVERTEXATTRIB1FPROC _glVertexAttrib1f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB1FVPROC( GLuint index, GLfloat* v );
    private PFNGLVERTEXATTRIB1FVPROC _glVertexAttrib1fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB1SPROC( GLuint index, GLshort x );
    private PFNGLVERTEXATTRIB1SPROC _glVertexAttrib1s;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB1SVPROC( GLuint index, GLshort* v );
    private PFNGLVERTEXATTRIB1SVPROC _glVertexAttrib1sv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB2DPROC( GLuint index, GLdouble x, GLdouble y );
    private PFNGLVERTEXATTRIB2DPROC _glVertexAttrib2d;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB2DVPROC( GLuint index, GLdouble* v );
    private PFNGLVERTEXATTRIB2DVPROC _glVertexAttrib2dv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB2FPROC( GLuint index, GLfloat x, GLfloat y );
    private PFNGLVERTEXATTRIB2FPROC _glVertexAttrib2f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB2FVPROC( GLuint index, GLfloat* v );
    private PFNGLVERTEXATTRIB2FVPROC _glVertexAttrib2fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB2SPROC( GLuint index, GLshort x, GLshort y );
    private PFNGLVERTEXATTRIB2SPROC _glVertexAttrib2s;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB2SVPROC( GLuint index, GLshort* v );
    private PFNGLVERTEXATTRIB2SVPROC _glVertexAttrib2sv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB3DPROC( GLuint index, GLdouble x, GLdouble y, GLdouble z );
    private PFNGLVERTEXATTRIB3DPROC _glVertexAttrib3d;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB3DVPROC( GLuint index, GLdouble* v );
    private PFNGLVERTEXATTRIB3DVPROC _glVertexAttrib3dv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB3FPROC( GLuint index, GLfloat x, GLfloat y, GLfloat z );
    private PFNGLVERTEXATTRIB3FPROC _glVertexAttrib3f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB3FVPROC( GLuint index, GLfloat* v );
    private PFNGLVERTEXATTRIB3FVPROC _glVertexAttrib3fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB3SPROC( GLuint index, GLshort x, GLshort y, GLshort z );
    private PFNGLVERTEXATTRIB3SPROC _glVertexAttrib3s;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB3SVPROC( GLuint index, GLshort* v );
    private PFNGLVERTEXATTRIB3SVPROC _glVertexAttrib3sv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4NBVPROC( GLuint index, GLbyte* v );
    private PFNGLVERTEXATTRIB4NBVPROC _glVertexAttrib4Nbv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4NIVPROC( GLuint index, GLint* v );
    private PFNGLVERTEXATTRIB4NIVPROC _glVertexAttrib4Niv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4NSVPROC( GLuint index, GLshort* v );
    private PFNGLVERTEXATTRIB4NSVPROC _glVertexAttrib4Nsv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4NUBPROC( GLuint index, GLubyte x, GLubyte y, GLubyte z, GLubyte w );
    private PFNGLVERTEXATTRIB4NUBPROC _glVertexAttrib4Nub;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4NUBVPROC( GLuint index, GLubyte* v );
    private PFNGLVERTEXATTRIB4NUBVPROC _glVertexAttrib4Nubv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4NUIVPROC( GLuint index, GLuint* v );
    private PFNGLVERTEXATTRIB4NUIVPROC _glVertexAttrib4Nuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4NUSVPROC( GLuint index, GLushort* v );
    private PFNGLVERTEXATTRIB4NUSVPROC _glVertexAttrib4Nusv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4BVPROC( GLuint index, GLbyte* v );
    private PFNGLVERTEXATTRIB4BVPROC _glVertexAttrib4bv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4DPROC( GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w );
    private PFNGLVERTEXATTRIB4DPROC _glVertexAttrib4d;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4DVPROC( GLuint index, GLdouble* v );
    private PFNGLVERTEXATTRIB4DVPROC _glVertexAttrib4dv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4FPROC( GLuint index, GLfloat x, GLfloat y, GLfloat z, GLfloat w );
    private PFNGLVERTEXATTRIB4FPROC _glVertexAttrib4f;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4FVPROC( GLuint index, GLfloat* v );
    private PFNGLVERTEXATTRIB4FVPROC _glVertexAttrib4fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4IVPROC( GLuint index, GLint* v );
    private PFNGLVERTEXATTRIB4IVPROC _glVertexAttrib4iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4SPROC( GLuint index, GLshort x, GLshort y, GLshort z, GLshort w );
    private PFNGLVERTEXATTRIB4SPROC _glVertexAttrib4s;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4SVPROC( GLuint index, GLshort* v );
    private PFNGLVERTEXATTRIB4SVPROC _glVertexAttrib4sv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4UBVPROC( GLuint index, GLubyte* v );
    private PFNGLVERTEXATTRIB4UBVPROC _glVertexAttrib4ubv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4UIVPROC( GLuint index, GLuint* v );
    private PFNGLVERTEXATTRIB4UIVPROC _glVertexAttrib4uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIB4USVPROC( GLuint index, GLushort* v );
    private PFNGLVERTEXATTRIB4USVPROC _glVertexAttrib4usv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBPOINTERPROC( GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, void* pointer );
    private PFNGLVERTEXATTRIBPOINTERPROC _glVertexAttribPointer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX2X3FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX2X3FVPROC _glUniformMatrix2x3fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX3X2FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX3X2FVPROC _glUniformMatrix3x2fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX2X4FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX2X4FVPROC _glUniformMatrix2x4fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX4X2FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX4X2FVPROC _glUniformMatrix4x2fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX3X4FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX3X4FVPROC _glUniformMatrix3x4fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMMATRIX4X3FVPROC( GLint location, GLsizei count, GLboolean transpose, GLfloat* value );
    private PFNGLUNIFORMMATRIX4X3FVPROC _glUniformMatrix4x3fv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOLORMASKIPROC( GLuint index, GLboolean r, GLboolean g, GLboolean b, GLboolean a );
    private PFNGLCOLORMASKIPROC _glColorMaski;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETBOOLEANI_VPROC( GLenum target, GLuint index, GLboolean* data );
    private PFNGLGETBOOLEANI_VPROC _glGetBooleani_v;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETINTEGERI_VPROC( GLenum target, GLuint index, GLint* data );
    private PFNGLGETINTEGERI_VPROC _glGetIntegeri_v;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLENABLEIPROC( GLenum target, GLuint index );
    private PFNGLENABLEIPROC _glEnablei;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDISABLEIPROC( GLenum target, GLuint index );
    private PFNGLDISABLEIPROC _glDisablei;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISENABLEDIPROC( GLenum target, GLuint index );
    private PFNGLISENABLEDIPROC _glIsEnabledi;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBEGINTRANSFORMFEEDBACKPROC( GLenum primitiveMode );
    private PFNGLBEGINTRANSFORMFEEDBACKPROC _glBeginTransformFeedback;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLENDTRANSFORMFEEDBACKPROC();
    private PFNGLENDTRANSFORMFEEDBACKPROC _glEndTransformFeedback;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDBUFFERRANGEPROC( GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size );
    private PFNGLBINDBUFFERRANGEPROC _glBindBufferRange;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDBUFFERBASEPROC( GLenum target, GLuint index, GLuint buffer );
    private PFNGLBINDBUFFERBASEPROC _glBindBufferBase;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTRANSFORMFEEDBACKVARYINGSPROC( GLuint program, GLsizei count, GLchar** varyings, GLenum bufferMode );
    private PFNGLTRANSFORMFEEDBACKVARYINGSPROC _glTransformFeedbackVaryings;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTRANSFORMFEEDBACKVARYINGPROC( GLuint program,
                                                                GLuint index,
                                                                GLsizei bufSize,
                                                                GLsizei* length,
                                                                GLsizei* size,
                                                                GLenum* type,
                                                                GLchar* name );
    private PFNGLGETTRANSFORMFEEDBACKVARYINGPROC _glGetTransformFeedbackVarying;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLAMPCOLORPROC( GLenum target, GLenum clamp );
    private PFNGLCLAMPCOLORPROC _glClampColor;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBEGINCONDITIONALRENDERPROC( GLuint id, GLenum mode );
    private PFNGLBEGINCONDITIONALRENDERPROC _glBeginConditionalRender;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLENDCONDITIONALRENDERPROC();
    private PFNGLENDCONDITIONALRENDERPROC _glEndConditionalRender;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBIPOINTERPROC( GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer );
    private PFNGLVERTEXATTRIBIPOINTERPROC _glVertexAttribIPointer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETVERTEXATTRIBIIVPROC( GLuint index, GLenum pname, GLint* parameters );
    private PFNGLGETVERTEXATTRIBIIVPROC _glGetVertexAttribIiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETVERTEXATTRIBIUIVPROC( GLuint index, GLenum pname, GLuint* parameters );
    private PFNGLGETVERTEXATTRIBIUIVPROC _glGetVertexAttribIuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI1IPROC( GLuint index, GLint x );
    private PFNGLVERTEXATTRIBI1IPROC _glVertexAttribI1i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI2IPROC( GLuint index, GLint x, GLint y );
    private PFNGLVERTEXATTRIBI2IPROC _glVertexAttribI2i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI3IPROC( GLuint index, GLint x, GLint y, GLint z );
    private PFNGLVERTEXATTRIBI3IPROC _glVertexAttribI3i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4IPROC( GLuint index, GLint x, GLint y, GLint z, GLint w );
    private PFNGLVERTEXATTRIBI4IPROC _glVertexAttribI4i;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI1UIPROC( GLuint index, GLuint x );
    private PFNGLVERTEXATTRIBI1UIPROC _glVertexAttribI1ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI2UIPROC( GLuint index, GLuint x, GLuint y );
    private PFNGLVERTEXATTRIBI2UIPROC _glVertexAttribI2ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI3UIPROC( GLuint index, GLuint x, GLuint y, GLuint z );
    private PFNGLVERTEXATTRIBI3UIPROC _glVertexAttribI3ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4UIPROC( GLuint index, GLuint x, GLuint y, GLuint z, GLuint w );
    private PFNGLVERTEXATTRIBI4UIPROC _glVertexAttribI4ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI1IVPROC( GLuint index, GLint* v );
    private PFNGLVERTEXATTRIBI1IVPROC _glVertexAttribI1iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI2IVPROC( GLuint index, GLint* v );
    private PFNGLVERTEXATTRIBI2IVPROC _glVertexAttribI2iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI3IVPROC( GLuint index, GLint* v );
    private PFNGLVERTEXATTRIBI3IVPROC _glVertexAttribI3iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4IVPROC( GLuint index, GLint* v );
    private PFNGLVERTEXATTRIBI4IVPROC _glVertexAttribI4iv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI1UIVPROC( GLuint index, GLuint* v );
    private PFNGLVERTEXATTRIBI1UIVPROC _glVertexAttribI1uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI2UIVPROC( GLuint index, GLuint* v );
    private PFNGLVERTEXATTRIBI2UIVPROC _glVertexAttribI2uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI3UIVPROC( GLuint index, GLuint* v );
    private PFNGLVERTEXATTRIBI3UIVPROC _glVertexAttribI3uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4UIVPROC( GLuint index, GLuint* v );
    private PFNGLVERTEXATTRIBI4UIVPROC _glVertexAttribI4uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4BVPROC( GLuint index, GLbyte* v );
    private PFNGLVERTEXATTRIBI4BVPROC _glVertexAttribI4bv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4SVPROC( GLuint index, GLshort* v );
    private PFNGLVERTEXATTRIBI4SVPROC _glVertexAttribI4sv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4UBVPROC( GLuint index, GLubyte* v );
    private PFNGLVERTEXATTRIBI4UBVPROC _glVertexAttribI4ubv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBI4USVPROC( GLuint index, GLushort* v );
    private PFNGLVERTEXATTRIBI4USVPROC _glVertexAttribI4usv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETUNIFORMUIVPROC( GLuint program, GLint location, GLuint* @params );
    private PFNGLGETUNIFORMUIVPROC _glGetUniformuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDFRAGDATALOCATIONPROC( GLuint program, GLuint color, GLchar* name );
    private PFNGLBINDFRAGDATALOCATIONPROC _glBindFragDataLocation;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLint PFNGLGETFRAGDATALOCATIONPROC( GLuint program, GLchar* name );
    private PFNGLGETFRAGDATALOCATIONPROC _glGetFragDataLocation;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM1UIPROC( GLint location, GLuint v0 );
    private PFNGLUNIFORM1UIPROC _glUniform1ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM2UIPROC( GLint location, GLuint v0, GLuint v1 );
    private PFNGLUNIFORM2UIPROC _glUniform2ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM3UIPROC( GLint location, GLuint v0, GLuint v1, GLuint v2 );
    private PFNGLUNIFORM3UIPROC _glUniform3ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM4UIPROC( GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 );
    private PFNGLUNIFORM4UIPROC _glUniform4ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM1UIVPROC( GLint location, GLsizei count, GLuint* value );
    private PFNGLUNIFORM1UIVPROC _glUniform1uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM2UIVPROC( GLint location, GLsizei count, GLuint* value );
    private PFNGLUNIFORM2UIVPROC _glUniform2uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM3UIVPROC( GLint location, GLsizei count, GLuint* value );
    private PFNGLUNIFORM3UIVPROC _glUniform3uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORM4UIVPROC( GLint location, GLsizei count, GLuint* value );
    private PFNGLUNIFORM4UIVPROC _glUniform4uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXPARAMETERIIVPROC( GLenum target, GLenum pname, GLint* param );
    private PFNGLTEXPARAMETERIIVPROC _glTexParameterIiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXPARAMETERIUIVPROC( GLenum target, GLenum pname, GLuint* param );
    private PFNGLTEXPARAMETERIUIVPROC _glTexParameterIuiv;
    
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTEXPARAMETERIIVPROC( GLenum target, GLenum pname, GLint* parameters );
    private PFNGLGETTEXPARAMETERIIVPROC _glGetTexParameterIiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETTEXPARAMETERIUIVPROC( GLenum target, GLenum pname, GLuint* parameters );
    private PFNGLGETTEXPARAMETERIUIVPROC _glGetTexParameterIuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARBUFFERIVPROC( GLenum buffer, GLint drawbuffer, GLint* value );
    private PFNGLCLEARBUFFERIVPROC _glClearBufferiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARBUFFERUIVPROC( GLenum buffer, GLint drawbuffer, GLuint* value );
    private PFNGLCLEARBUFFERUIVPROC _glClearBufferuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARBUFFERFVPROC( GLenum buffer, GLint drawbuffer, GLfloat* value );
    private PFNGLCLEARBUFFERFVPROC _glClearBufferfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCLEARBUFFERFIPROC( GLenum buffer, GLint drawbuffer, GLfloat depth, GLint stencil );
    private PFNGLCLEARBUFFERFIPROC _glClearBufferfi;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLubyte* PFNGLGETSTRINGIPROC( GLenum name, GLuint index );
    private PFNGLGETSTRINGIPROC _glGetStringi;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISRENDERBUFFERPROC( GLuint renderbuffer );
    private PFNGLISRENDERBUFFERPROC _glIsRenderbuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDRENDERBUFFERPROC( GLenum target, GLuint renderbuffer );
    private PFNGLBINDRENDERBUFFERPROC _glBindRenderbuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETERENDERBUFFERSPROC( GLsizei n, GLuint* renderbuffers );
    private PFNGLDELETERENDERBUFFERSPROC _glDeleteRenderbuffers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENRENDERBUFFERSPROC( GLsizei n, GLuint* renderbuffers );
    private PFNGLGENRENDERBUFFERSPROC _glGenRenderbuffers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLRENDERBUFFERSTORAGEPROC( GLenum target, GLenum internalformat, GLsizei width, GLsizei height );
    private PFNGLRENDERBUFFERSTORAGEPROC _glRenderbufferStorage;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETRENDERBUFFERPARAMETERIVPROC( GLenum target, GLenum pname, GLint* parameters );
    private PFNGLGETRENDERBUFFERPARAMETERIVPROC _glGetRenderbufferParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISFRAMEBUFFERPROC( GLuint framebuffer );
    private PFNGLISFRAMEBUFFERPROC _glIsFramebuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDFRAMEBUFFERPROC( GLenum target, GLuint framebuffer );
    private PFNGLBINDFRAMEBUFFERPROC _glBindFramebuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETEFRAMEBUFFERSPROC( GLsizei n, GLuint* framebuffers );
    private PFNGLDELETEFRAMEBUFFERSPROC _glDeleteFramebuffers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENFRAMEBUFFERSPROC( GLsizei n, GLuint* framebuffers );
    private PFNGLGENFRAMEBUFFERSPROC _glGenFramebuffers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLenum PFNGLCHECKFRAMEBUFFERSTATUSPROC( GLenum target );
    private PFNGLCHECKFRAMEBUFFERSTATUSPROC _glCheckFramebufferStatus;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFRAMEBUFFERTEXTURE1DPROC( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level );
    private PFNGLFRAMEBUFFERTEXTURE1DPROC _glFramebufferTexture1D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFRAMEBUFFERTEXTURE2DPROC( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level );
    private PFNGLFRAMEBUFFERTEXTURE2DPROC _glFramebufferTexture2D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFRAMEBUFFERTEXTURE3DPROC( GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level, GLint zoffset );
    private PFNGLFRAMEBUFFERTEXTURE3DPROC _glFramebufferTexture3D;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFRAMEBUFFERRENDERBUFFERPROC( GLenum target, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer );
    private PFNGLFRAMEBUFFERRENDERBUFFERPROC _glFramebufferRenderbuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC( GLenum target, GLenum attachment, GLenum pname, GLint* parameters );
    private PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC _glGetFramebufferAttachmentParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENERATEMIPMAPPROC( GLenum target );
    private PFNGLGENERATEMIPMAPPROC _glGenerateMipmap;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBLITFRAMEBUFFERPROC( GLint srcX0,
                                                    GLint srcY0,
                                                    GLint srcX1,
                                                    GLint srcY1,
                                                    GLint dstX0,
                                                    GLint dstY0,
                                                    GLint dstX1,
                                                    GLint dstY1,
                                                    GLbitfield mask,
                                                    GLenum filter );
    private PFNGLBLITFRAMEBUFFERPROC _glBlitFramebuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height );
    private PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC _glRenderbufferStorageMultisample;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFRAMEBUFFERTEXTURELAYERPROC( GLenum target, GLenum attachment, GLuint texture, GLint level, GLint layer );
    private PFNGLFRAMEBUFFERTEXTURELAYERPROC _glFramebufferTextureLayer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void* PFNGLMAPBUFFERRANGEPROC( GLenum target, GLintptr offset, GLsizeiptr length, GLbitfield access );
    private PFNGLMAPBUFFERRANGEPROC _glMapBufferRange;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFLUSHMAPPEDBUFFERRANGEPROC( GLenum target, GLintptr offset, GLsizeiptr length );
    private PFNGLFLUSHMAPPEDBUFFERRANGEPROC _glFlushMappedBufferRange;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDVERTEXARRAYPROC( GLuint array );
    private PFNGLBINDVERTEXARRAYPROC _glBindVertexArray;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETEVERTEXARRAYSPROC( GLsizei n, GLuint* arrays );
    private PFNGLDELETEVERTEXARRAYSPROC _glDeleteVertexArrays;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENVERTEXARRAYSPROC( GLsizei n, GLuint* arrays );
    private PFNGLGENVERTEXARRAYSPROC _glGenVertexArrays;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISVERTEXARRAYPROC( GLuint array );
    private PFNGLISVERTEXARRAYPROC _glIsVertexArray;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWARRAYSINSTANCEDPROC( GLenum mode, GLint first, GLsizei count, GLsizei instancecount );
    private PFNGLDRAWARRAYSINSTANCEDPROC _glDrawArraysInstanced;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDPROC( GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount );
    private PFNGLDRAWELEMENTSINSTANCEDPROC _glDrawElementsInstanced;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXBUFFERPROC( GLenum target, GLenum internalformat, GLuint buffer );
    private PFNGLTEXBUFFERPROC _glTexBuffer;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPRIMITIVERESTARTINDEXPROC( GLuint index );
    private PFNGLPRIMITIVERESTARTINDEXPROC _glPrimitiveRestartIndex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLCOPYBUFFERSUBDATAPROC( GLenum readTarget, GLenum writeTarget, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size );
    private PFNGLCOPYBUFFERSUBDATAPROC _glCopyBufferSubData;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETUNIFORMINDICESPROC( GLuint program, GLsizei uniformCount, GLchar** uniformNames, GLuint* uniformIndices );
    private PFNGLGETUNIFORMINDICESPROC _glGetUniformIndices;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETACTIVEUNIFORMSIVPROC( GLuint program, GLsizei uniformCount, GLuint* uniformIndices, GLenum pname, GLint* parameters );
    private PFNGLGETACTIVEUNIFORMSIVPROC _glGetActiveUniformsiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETACTIVEUNIFORMNAMEPROC( GLuint program, GLuint uniformIndex, GLsizei bufSize, GLsizei* length, GLchar* uniformName );
    private PFNGLGETACTIVEUNIFORMNAMEPROC _glGetActiveUniformName;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLuint PFNGLGETUNIFORMBLOCKINDEXPROC( GLuint program, GLchar* uniformBlockName );
    private PFNGLGETUNIFORMBLOCKINDEXPROC _glGetUniformBlockIndex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETACTIVEUNIFORMBLOCKIVPROC( GLuint program, GLuint uniformBlockIndex, GLenum pname, GLint* parameters );
    private PFNGLGETACTIVEUNIFORMBLOCKIVPROC _glGetActiveUniformBlockiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC( GLuint program,
                                                              GLuint uniformBlockIndex,
                                                              GLsizei bufSize,
                                                              GLsizei* length,
                                                              GLchar* uniformBlockName );
    private PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC _glGetActiveUniformBlockName;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLUNIFORMBLOCKBINDINGPROC( GLuint program, GLuint uniformBlockIndex, GLuint uniformBlockBinding );
    private PFNGLUNIFORMBLOCKBINDINGPROC _glUniformBlockBinding;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWELEMENTSBASEVERTEXPROC( GLenum mode, GLsizei count, GLenum type, void* indices, GLint basevertex );
    private PFNGLDRAWELEMENTSBASEVERTEXPROC _glDrawElementsBaseVertex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC( GLenum mode,
                                                                GLuint start,
                                                                GLuint end,
                                                                GLsizei count,
                                                                GLenum type,
                                                                void* indices,
                                                                GLint basevertex );
    private PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC _glDrawRangeElementsBaseVertex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC( GLenum mode,
                                                                    GLsizei count,
                                                                    GLenum type,
                                                                    void* indices,
                                                                    GLsizei instancecount,
                                                                    GLint basevertex );
    private PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC _glDrawElementsInstancedBaseVertex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC( GLenum mode,
                                                                GLsizei* count,
                                                                GLenum type,
                                                                void** indices,
                                                                GLsizei drawcount,
                                                                GLint* basevertex );
    private PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC _glMultiDrawElementsBaseVertex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLPROVOKINGVERTEXPROC( GLenum mode );
    private PFNGLPROVOKINGVERTEXPROC _glProvokingVertex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void* PFNGLFENCESYNCPROC( GLenum condition, GLbitfield flags );
    private PFNGLFENCESYNCPROC _glFenceSync;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISSYNCPROC( void* sync );
    private PFNGLISSYNCPROC _glIsSync;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETESYNCPROC( void* sync );
    private PFNGLDELETESYNCPROC _glDeleteSync;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLenum PFNGLCLIENTWAITSYNCPROC( void* sync, GLbitfield flags, GLuint64 timeout );
    private PFNGLCLIENTWAITSYNCPROC _glClientWaitSync;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLWAITSYNCPROC( void* sync, GLbitfield flags, GLuint64 timeout );
    private PFNGLWAITSYNCPROC _glWaitSync;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETINTEGER64VPROC( GLenum pname, GLint64* data );
    private PFNGLGETINTEGER64VPROC _glGetInteger64v;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSYNCIVPROC( void* sync, GLenum pname, GLsizei bufSize, GLsizei* length, GLint* values );
    private PFNGLGETSYNCIVPROC _glGetSynciv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETINTEGER64I_VPROC( GLenum target, GLuint index, GLint64* data );
    private PFNGLGETINTEGER64I_VPROC _glGetInteger64i_v;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETBUFFERPARAMETERI64VPROC( GLenum target, GLenum pname, GLint64* parameters );
    private PFNGLGETBUFFERPARAMETERI64VPROC _glGetBufferParameteri64v;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLFRAMEBUFFERTEXTUREPROC( GLenum target, GLenum attachment, GLuint texture, GLint level );
    private PFNGLFRAMEBUFFERTEXTUREPROC _glFramebufferTexture;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXIMAGE2DMULTISAMPLEPROC( GLenum target,
                                                          GLsizei samples,
                                                          GLenum internalformat,
                                                          GLsizei width,
                                                          GLsizei height,
                                                          GLboolean fixedsamplelocations );
    private PFNGLTEXIMAGE2DMULTISAMPLEPROC _glTexImage2DMultisample;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLTEXIMAGE3DMULTISAMPLEPROC( GLenum target,
                                                          GLsizei samples,
                                                          GLenum internalformat,
                                                          GLsizei width,
                                                          GLsizei height,
                                                          GLsizei depth,
                                                          GLboolean fixedsamplelocations );
    private PFNGLTEXIMAGE3DMULTISAMPLEPROC _glTexImage3DMultisample;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETMULTISAMPLEFVPROC( GLenum pname, GLuint index, GLfloat* val );
    private PFNGLGETMULTISAMPLEFVPROC _glGetMultisamplefv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLEMASKIPROC( GLuint maskNumber, GLbitfield mask );
    private PFNGLSAMPLEMASKIPROC _glSampleMaski;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDFRAGDATALOCATIONINDEXEDPROC( GLuint program, GLuint colorNumber, GLuint index, GLchar* name );
    private PFNGLBINDFRAGDATALOCATIONINDEXEDPROC _glBindFragDataLocationIndexed;
    
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLint PFNGLGETFRAGDATAINDEXPROC( GLuint program, GLchar* name );
    private PFNGLGETFRAGDATAINDEXPROC _glGetFragDataIndex;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGENSAMPLERSPROC( GLsizei count, GLuint* samplers );
    private PFNGLGENSAMPLERSPROC _glGenSamplers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLDELETESAMPLERSPROC( GLsizei count, GLuint* samplers );
    private PFNGLDELETESAMPLERSPROC _glDeleteSamplers;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GLboolean PFNGLISSAMPLERPROC( GLuint sampler );
    private PFNGLISSAMPLERPROC _glIsSampler;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLBINDSAMPLERPROC( GLuint unit, GLuint sampler );
    private PFNGLBINDSAMPLERPROC _glBindSampler;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLERPARAMETERIPROC( GLuint sampler, GLenum pname, GLint param );
    private PFNGLSAMPLERPARAMETERIPROC _glSamplerParameteri;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLERPARAMETERIVPROC( GLuint sampler, GLenum pname, GLint* param );
    private PFNGLSAMPLERPARAMETERIVPROC _glSamplerParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLERPARAMETERFPROC( GLuint sampler, GLenum pname, GLfloat param );
    private PFNGLSAMPLERPARAMETERFPROC _glSamplerParameterf;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLERPARAMETERFVPROC( GLuint sampler, GLenum pname, GLfloat* param );
    private PFNGLSAMPLERPARAMETERFVPROC _glSamplerParameterfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLERPARAMETERIIVPROC( GLuint sampler, GLenum pname, GLint* param );
    private PFNGLSAMPLERPARAMETERIIVPROC _glSamplerParameterIiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLSAMPLERPARAMETERIUIVPROC( GLuint sampler, GLenum pname, GLuint* param );
    private PFNGLSAMPLERPARAMETERIUIVPROC _glSamplerParameterIuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSAMPLERPARAMETERIVPROC( GLuint sampler, GLenum pname, GLint* param );
    private PFNGLGETSAMPLERPARAMETERIVPROC _glGetSamplerParameteriv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSAMPLERPARAMETERIIVPROC( GLuint sampler, GLenum pname, GLint* param );
    private PFNGLGETSAMPLERPARAMETERIIVPROC _glGetSamplerParameterIiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSAMPLERPARAMETERFVPROC( GLuint sampler, GLenum pname, GLfloat* param );
    private PFNGLGETSAMPLERPARAMETERFVPROC _glGetSamplerParameterfv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETSAMPLERPARAMETERIUIVPROC( GLuint sampler, GLenum pname, GLuint* param );
    private PFNGLGETSAMPLERPARAMETERIUIVPROC _glGetSamplerParameterIuiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLQUERYCOUNTERPROC( GLuint id, GLenum target );
    private PFNGLQUERYCOUNTERPROC _glQueryCounter;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETQUERYOBJECTI64VPROC( GLuint id, GLenum pname, GLint64* param );
    private PFNGLGETQUERYOBJECTI64VPROC _glGetQueryObjecti64v;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLGETQUERYOBJECTUI64VPROC( GLuint id, GLenum pname, GLuint64* param );
    private PFNGLGETQUERYOBJECTUI64VPROC _glGetQueryObjectui64v;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBDIVISORPROC( GLuint index, GLuint divisor );
    private PFNGLVERTEXATTRIBDIVISORPROC _glVertexAttribDivisor;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP1UIPROC( GLuint index, GLenum type, GLboolean normalized, GLuint value );
    private PFNGLVERTEXATTRIBP1UIPROC _glVertexAttribP1ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP1UIVPROC( GLuint index, GLenum type, GLboolean normalized, GLuint* value );
    private PFNGLVERTEXATTRIBP1UIVPROC _glVertexAttribP1uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP2UIPROC( GLuint index, GLenum type, GLboolean normalized, GLuint value );
    private PFNGLVERTEXATTRIBP2UIPROC _glVertexAttribP2ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP2UIVPROC( GLuint index, GLenum type, GLboolean normalized, GLuint* value );
    private PFNGLVERTEXATTRIBP2UIVPROC _glVertexAttribP2uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP3UIPROC( GLuint index, GLenum type, GLboolean normalized, GLuint value );
    private PFNGLVERTEXATTRIBP3UIPROC _glVertexAttribP3ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP3UIVPROC( GLuint index, GLenum type, GLboolean normalized, GLuint* value );
    private PFNGLVERTEXATTRIBP3UIVPROC _glVertexAttribP3uiv;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP4UIPROC( GLuint index, GLenum type, GLboolean normalized, GLuint value );
    private PFNGLVERTEXATTRIBP4UIPROC _glVertexAttribP4ui;

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void PFNGLVERTEXATTRIBP4UIVPROC( GLuint index, GLenum type, GLboolean normalized, GLuint* value );
    private PFNGLVERTEXATTRIBP4UIVPROC _glVertexAttribP4uiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMINSAMPLESHADINGPROC(GLfloat value);
    private PFNGLMINSAMPLESHADINGPROC _glMinSampleShading;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONIPROC(GLuint buf, GLenum mode);
    private PFNGLBLENDEQUATIONIPROC _glBlendEquationi;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONSEPARATEIPROC(GLuint buf, GLenum modeRGB, GLenum modeAlpha);
    private PFNGLBLENDEQUATIONSEPARATEIPROC _glBlendEquationSeparatei;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCIPROC(GLuint buf, GLenum src, GLenum dst);
    private PFNGLBLENDFUNCIPROC _glBlendFunci;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCSEPARATEIPROC(GLuint buf, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha);
    private PFNGLBLENDFUNCSEPARATEIPROC _glBlendFuncSeparatei;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINDIRECTPROC(GLenum mode, void* indirect);
    private PFNGLDRAWARRAYSINDIRECTPROC _glDrawArraysIndirect;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINDIRECTPROC(GLenum mode, GLenum type, void* indirect);
    private PFNGLDRAWELEMENTSINDIRECTPROC _glDrawElementsIndirect;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1DPROC(GLint location, GLdouble x);
    private PFNGLUNIFORM1DPROC _glUniform1d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2DPROC(GLint location, GLdouble x, GLdouble y);
    private PFNGLUNIFORM2DPROC _glUniform2d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3DPROC(GLint location, GLdouble x, GLdouble y, GLdouble z);
    private PFNGLUNIFORM3DPROC _glUniform3d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4DPROC(GLint location, GLdouble x, GLdouble y, GLdouble z, GLdouble w);
    private PFNGLUNIFORM4DPROC _glUniform4d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1DVPROC(GLint location, GLsizei count, GLdouble* value);
    private PFNGLUNIFORM1DVPROC _glUniform1dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2DVPROC(GLint location, GLsizei count, GLdouble* value);
    private PFNGLUNIFORM2DVPROC _glUniform2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3DVPROC(GLint location, GLsizei count, GLdouble* value);
    private PFNGLUNIFORM3DVPROC _glUniform3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4DVPROC(GLint location, GLsizei count, GLdouble* value);
    private PFNGLUNIFORM4DVPROC _glUniform4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX2DVPROC _glUniformMatrix2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX3DVPROC _glUniformMatrix3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX4DVPROC _glUniformMatrix4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X3DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX2X3DVPROC _glUniformMatrix2x3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X4DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX2X4DVPROC _glUniformMatrix2x4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X2DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX3X2DVPROC _glUniformMatrix3x2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X4DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX3X4DVPROC _glUniformMatrix3x4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X2DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX4X2DVPROC _glUniformMatrix4x2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X3DVPROC(GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLUNIFORMMATRIX4X3DVPROC _glUniformMatrix4x3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMDVPROC(GLuint program, GLint location, GLdouble* parameters);
    private PFNGLGETUNIFORMDVPROC _glGetUniformdv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLint PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC(GLuint program, GLenum shadertype, GLchar* name);
    private PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC _glGetSubroutineUniformLocation;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLuint PFNGLGETSUBROUTINEINDEXPROC(GLuint program, GLenum shadertype, GLchar* name);
    private PFNGLGETSUBROUTINEINDEXPROC _glGetSubroutineIndex;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC(GLuint program, GLenum shadertype, GLuint index, GLenum pname, GLint* values);
    private PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC _glGetActiveSubroutineUniformiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC(GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name);
    private PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC _glGetActiveSubroutineUniformName;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINENAMEPROC(GLuint program, GLenum shadertype, GLuint index, GLsizei bufsize, GLsizei* length, GLchar* name);
    private PFNGLGETACTIVESUBROUTINENAMEPROC _glGetActiveSubroutineName;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMSUBROUTINESUIVPROC(GLenum shadertype, GLsizei count, GLuint* indices);
    private PFNGLUNIFORMSUBROUTINESUIVPROC _glUniformSubroutinesuiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMSUBROUTINEUIVPROC(GLenum shadertype, GLint location, GLuint* @params);
    private PFNGLGETUNIFORMSUBROUTINEUIVPROC _glGetUniformSubroutineuiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMSTAGEIVPROC(GLuint program, GLenum shadertype, GLenum pname, GLint* values);
    private PFNGLGETPROGRAMSTAGEIVPROC _glGetProgramStageiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPATCHPARAMETERIPROC(GLenum pname, GLint value);
    private PFNGLPATCHPARAMETERIPROC _glPatchParameteri;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPATCHPARAMETERFVPROC(GLenum pname, GLfloat* values);
    private PFNGLPATCHPARAMETERFVPROC _glPatchParameterfv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTRANSFORMFEEDBACKPROC(GLenum target, GLuint id);
    private PFNGLBINDTRANSFORMFEEDBACKPROC _glBindTransformFeedback;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETETRANSFORMFEEDBACKSPROC(GLsizei n, GLuint* ids);
    private PFNGLDELETETRANSFORMFEEDBACKSPROC _glDeleteTransformFeedbacks;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENTRANSFORMFEEDBACKSPROC(GLsizei n, GLuint* ids);
    private PFNGLGENTRANSFORMFEEDBACKSPROC _glGenTransformFeedbacks;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLboolean PFNGLISTRANSFORMFEEDBACKPROC(GLuint id);
    private PFNGLISTRANSFORMFEEDBACKPROC _glIsTransformFeedback;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPAUSETRANSFORMFEEDBACKPROC();
    private PFNGLPAUSETRANSFORMFEEDBACKPROC _glPauseTransformFeedback;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRESUMETRANSFORMFEEDBACKPROC();
    private PFNGLRESUMETRANSFORMFEEDBACKPROC _glResumeTransformFeedback;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKPROC(GLenum mode, GLuint id);
    private PFNGLDRAWTRANSFORMFEEDBACKPROC _glDrawTransformFeedback;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC(GLenum mode, GLuint id, GLuint stream);
    private PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC _glDrawTransformFeedbackStream;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINQUERYINDEXEDPROC(GLenum target, GLuint index, GLuint id);
    private PFNGLBEGINQUERYINDEXEDPROC _glBeginQueryIndexed;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDQUERYINDEXEDPROC(GLenum target, GLuint index);
    private PFNGLENDQUERYINDEXEDPROC _glEndQueryIndexed;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYINDEXEDIVPROC(GLenum target, GLuint index, GLenum pname, GLint* parameters);
    private PFNGLGETQUERYINDEXEDIVPROC _glGetQueryIndexediv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRELEASESHADERCOMPILERPROC();
    private PFNGLRELEASESHADERCOMPILERPROC _glReleaseShaderCompiler;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERBINARYPROC(GLsizei count, GLuint* shaders, GLenum binaryformat, void* binary, GLsizei length);
    private PFNGLSHADERBINARYPROC _glShaderBinary;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERPRECISIONFORMATPROC(GLenum shadertype, GLenum precisiontype, GLint* range, GLint* precision);
    private PFNGLGETSHADERPRECISIONFORMATPROC _glGetShaderPrecisionFormat;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEFPROC(GLfloat n, GLfloat f);
    private PFNGLDEPTHRANGEFPROC _glDepthRangef;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARDEPTHFPROC(GLfloat d);
    private PFNGLCLEARDEPTHFPROC _glClearDepthf;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMBINARYPROC(GLuint program, GLsizei bufSize, GLsizei* length, GLenum* binaryFormat, void* binary);
    private PFNGLGETPROGRAMBINARYPROC _glGetProgramBinary;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMBINARYPROC(GLuint program, GLenum binaryFormat, void* binary, GLsizei length);
    private PFNGLPROGRAMBINARYPROC _glProgramBinary;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMPARAMETERIPROC(GLuint program, GLenum pname, GLint value);
    private PFNGLPROGRAMPARAMETERIPROC _glProgramParameteri;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUSEPROGRAMSTAGESPROC(GLuint pipeline, GLbitfield stages, GLuint program);
    private PFNGLUSEPROGRAMSTAGESPROC _glUseProgramStages;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLACTIVESHADERPROGRAMPROC(GLuint pipeline, GLuint program);
    private PFNGLACTIVESHADERPROGRAMPROC _glActiveShaderProgram;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLuint PFNGLCREATESHADERPROGRAMVPROC(GLenum type, GLsizei count, GLchar** strings);
    private PFNGLCREATESHADERPROGRAMVPROC _glCreateShaderProgramv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDPROGRAMPIPELINEPROC(GLuint pipeline);
    private PFNGLBINDPROGRAMPIPELINEPROC _glBindProgramPipeline;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEPROGRAMPIPELINESPROC(GLsizei n, GLuint* pipelines);
    private PFNGLDELETEPROGRAMPIPELINESPROC _glDeleteProgramPipelines;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENPROGRAMPIPELINESPROC(GLsizei n, GLuint* pipelines);
    private PFNGLGENPROGRAMPIPELINESPROC _glGenProgramPipelines;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLboolean PFNGLISPROGRAMPIPELINEPROC(GLuint pipeline);
    private PFNGLISPROGRAMPIPELINEPROC _glIsProgramPipeline;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMPIPELINEIVPROC(GLuint pipeline, GLenum pname, GLint* param);
    private PFNGLGETPROGRAMPIPELINEIVPROC _glGetProgramPipelineiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1IPROC(GLuint program, GLint location, GLint v0);
    private PFNGLPROGRAMUNIFORM1IPROC _glProgramUniform1i;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1IVPROC(GLuint program, GLint location, GLsizei count, GLint* value);
    private PFNGLPROGRAMUNIFORM1IVPROC _glProgramUniform1iv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1FPROC(GLuint program, GLint location, GLfloat v0);
    private PFNGLPROGRAMUNIFORM1FPROC _glProgramUniform1f;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1FVPROC(GLuint program, GLint location, GLsizei count, GLfloat* value);
    private PFNGLPROGRAMUNIFORM1FVPROC _glProgramUniform1fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1DPROC(GLuint program, GLint location, GLdouble v0);
    private PFNGLPROGRAMUNIFORM1DPROC _glProgramUniform1d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1DVPROC(GLuint program, GLint location, GLsizei count, GLdouble* value);
    private PFNGLPROGRAMUNIFORM1DVPROC _glProgramUniform1dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1UIPROC(GLuint program, GLint location, GLuint v0);
    private PFNGLPROGRAMUNIFORM1UIPROC _glProgramUniform1ui;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1UIVPROC(GLuint program, GLint location, GLsizei count, GLuint* value);
    private PFNGLPROGRAMUNIFORM1UIVPROC _glProgramUniform1uiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2IPROC(GLuint program, GLint location, GLint v0, GLint v1);
    private PFNGLPROGRAMUNIFORM2IPROC _glProgramUniform2i;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2IVPROC(GLuint program, GLint location, GLsizei count, GLint* value);
    private PFNGLPROGRAMUNIFORM2IVPROC _glProgramUniform2iv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2FPROC(GLuint program, GLint location, GLfloat v0, GLfloat v1);
    private PFNGLPROGRAMUNIFORM2FPROC _glProgramUniform2f;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2FVPROC(GLuint program, GLint location, GLsizei count, GLfloat* value);
    private PFNGLPROGRAMUNIFORM2FVPROC _glProgramUniform2fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2DPROC(GLuint program, GLint location, GLdouble v0, GLdouble v1);
    private PFNGLPROGRAMUNIFORM2DPROC _glProgramUniform2d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2DVPROC(GLuint program, GLint location, GLsizei count, GLdouble* value);
    private PFNGLPROGRAMUNIFORM2DVPROC _glProgramUniform2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2UIPROC(GLuint program, GLint location, GLuint v0, GLuint v1);
    private PFNGLPROGRAMUNIFORM2UIPROC _glProgramUniform2ui;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2UIVPROC(GLuint program, GLint location, GLsizei count, GLuint* value);
    private PFNGLPROGRAMUNIFORM2UIVPROC _glProgramUniform2uiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3IPROC(GLuint program, GLint location, GLint v0, GLint v1, GLint v2);
    private PFNGLPROGRAMUNIFORM3IPROC _glProgramUniform3i;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3IVPROC(GLuint program, GLint location, GLsizei count, GLint* value);
    private PFNGLPROGRAMUNIFORM3IVPROC _glProgramUniform3iv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3FPROC(GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2);
    private PFNGLPROGRAMUNIFORM3FPROC _glProgramUniform3f;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3FVPROC(GLuint program, GLint location, GLsizei count, GLfloat* value);
    private PFNGLPROGRAMUNIFORM3FVPROC _glProgramUniform3fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3DPROC(GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2);
    private PFNGLPROGRAMUNIFORM3DPROC _glProgramUniform3d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3DVPROC(GLuint program, GLint location, GLsizei count, GLdouble* value);
    private PFNGLPROGRAMUNIFORM3DVPROC _glProgramUniform3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3UIPROC(GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2);
    private PFNGLPROGRAMUNIFORM3UIPROC _glProgramUniform3ui;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3UIVPROC(GLuint program, GLint location, GLsizei count, GLuint* value);
    private PFNGLPROGRAMUNIFORM3UIVPROC _glProgramUniform3uiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4IPROC(GLuint program, GLint location, GLint v0, GLint v1, GLint v2, GLint v3);
    private PFNGLPROGRAMUNIFORM4IPROC _glProgramUniform4i;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4IVPROC(GLuint program, GLint location, GLsizei count, GLint* value);
    private PFNGLPROGRAMUNIFORM4IVPROC _glProgramUniform4iv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4FPROC(GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3);
    private PFNGLPROGRAMUNIFORM4FPROC _glProgramUniform4f;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4FVPROC(GLuint program, GLint location, GLsizei count, GLfloat* value);
    private PFNGLPROGRAMUNIFORM4FVPROC _glProgramUniform4fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4DPROC(GLuint program, GLint location, GLdouble v0, GLdouble v1, GLdouble v2, GLdouble v3);
    private PFNGLPROGRAMUNIFORM4DPROC _glProgramUniform4d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4DVPROC(GLuint program, GLint location, GLsizei count, GLdouble* value);
    private PFNGLPROGRAMUNIFORM4DVPROC _glProgramUniform4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4UIPROC(GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3);
    private PFNGLPROGRAMUNIFORM4UIPROC _glProgramUniform4ui;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4UIVPROC(GLuint program, GLint location, GLsizei count, GLuint* value);
    private PFNGLPROGRAMUNIFORM4UIVPROC _glProgramUniform4uiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX2FVPROC _glProgramUniformMatrix2fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX3FVPROC _glProgramUniformMatrix3fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX4FVPROC _glProgramUniformMatrix4fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX2DVPROC _glProgramUniformMatrix2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX3DVPROC _glProgramUniformMatrix3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX4DVPROC _glProgramUniformMatrix4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC _glProgramUniformMatrix2x3fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC _glProgramUniformMatrix3x2fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC _glProgramUniformMatrix2x4fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC _glProgramUniformMatrix4x2fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC _glProgramUniformMatrix3x4fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLfloat* value);
    private PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC _glProgramUniformMatrix4x3fv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC _glProgramUniformMatrix2x3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC _glProgramUniformMatrix3x2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC _glProgramUniformMatrix2x4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC _glProgramUniformMatrix4x2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC _glProgramUniformMatrix3x4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC(GLuint program, GLint location, GLsizei count, GLboolean transpose, GLdouble* value);
    private PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC _glProgramUniformMatrix4x3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVALIDATEPROGRAMPIPELINEPROC(GLuint pipeline);
    private PFNGLVALIDATEPROGRAMPIPELINEPROC _glValidateProgramPipeline;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMPIPELINEINFOLOGPROC(GLuint pipeline, GLsizei bufSize, GLsizei* length, GLchar* infoLog);
    private PFNGLGETPROGRAMPIPELINEINFOLOGPROC _glGetProgramPipelineInfoLog;
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL1DPROC(GLuint index, GLdouble x);
    private PFNGLVERTEXATTRIBL1DPROC _glVertexAttribL1d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL2DPROC(GLuint index, GLdouble x, GLdouble y);
    private PFNGLVERTEXATTRIBL2DPROC _glVertexAttribL2d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL3DPROC(GLuint index, GLdouble x, GLdouble y, GLdouble z);
    private PFNGLVERTEXATTRIBL3DPROC _glVertexAttribL3d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL4DPROC(GLuint index, GLdouble x, GLdouble y, GLdouble z, GLdouble w);
    private PFNGLVERTEXATTRIBL4DPROC _glVertexAttribL4d;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL1DVPROC(GLuint index, GLdouble* v);
    private PFNGLVERTEXATTRIBL1DVPROC _glVertexAttribL1dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL2DVPROC(GLuint index, GLdouble* v);
    private PFNGLVERTEXATTRIBL2DVPROC _glVertexAttribL2dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL3DVPROC(GLuint index, GLdouble* v);
    private PFNGLVERTEXATTRIBL3DVPROC _glVertexAttribL3dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL4DVPROC(GLuint index, GLdouble* v);
    private PFNGLVERTEXATTRIBL4DVPROC _glVertexAttribL4dv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBLPOINTERPROC(GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer);
    private PFNGLVERTEXATTRIBLPOINTERPROC _glVertexAttribLPointer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBLDVPROC(GLuint index, GLenum pname, GLdouble* parameters);
    private PFNGLGETVERTEXATTRIBLDVPROC _glGetVertexAttribLdv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTARRAYVPROC(GLuint first, GLsizei count, GLfloat* v);
    private PFNGLVIEWPORTARRAYVPROC _glViewportArrayv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTINDEXEDFPROC(GLuint index, GLfloat x, GLfloat y, GLfloat w, GLfloat h);
    private PFNGLVIEWPORTINDEXEDFPROC _glViewportIndexedf;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTINDEXEDFVPROC(GLuint index, GLfloat* v);
    private PFNGLVIEWPORTINDEXEDFVPROC _glViewportIndexedfv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORARRAYVPROC(GLuint first, GLsizei count, GLint* v);
    private PFNGLSCISSORARRAYVPROC _glScissorArrayv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORINDEXEDPROC(GLuint index, GLint left, GLint bottom, GLsizei width, GLsizei height);
    private PFNGLSCISSORINDEXEDPROC _glScissorIndexed;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORINDEXEDVPROC(GLuint index, GLint* v);
    private PFNGLSCISSORINDEXEDVPROC _glScissorIndexedv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEARRAYVPROC(GLuint first, GLsizei count, GLdouble* v);
    private PFNGLDEPTHRANGEARRAYVPROC _glDepthRangeArrayv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEINDEXEDPROC(GLuint index, GLdouble n, GLdouble f);
    private PFNGLDEPTHRANGEINDEXEDPROC _glDepthRangeIndexed;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFLOATI_VPROC(GLenum target, GLuint index, GLfloat* data);
    private PFNGLGETFLOATI_VPROC _glGetFloati_v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETDOUBLEI_VPROC(GLenum target, GLuint index, GLdouble* data);
    private PFNGLGETDOUBLEI_VPROC _glGetDoublei_v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC(GLenum mode, GLint first, GLsizei count, GLsizei instancecount, GLuint baseinstance);
    private PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC _glDrawArraysInstancedBaseInstance;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC(GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLuint baseinstance);
    private PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC _glDrawElementsInstancedBaseInstance;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC(GLenum mode, GLsizei count, GLenum type, void* indices, GLsizei instancecount, GLint basevertex, GLuint baseinstance);
    private PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC _glDrawElementsInstancedBaseVertexBaseInstance;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTERNALFORMATIVPROC(GLenum target, GLenum internalformat, GLenum pname, GLsizei bufSize, GLint* @params);
    private PFNGLGETINTERNALFORMATIVPROC _glGetInternalformativ;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC(GLuint program, GLuint bufferIndex, GLenum pname, GLint* @params);
    private PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC _glGetActiveAtomicCounterBufferiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDIMAGETEXTUREPROC(GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum access, GLenum format);
    private PFNGLBINDIMAGETEXTUREPROC _glBindImageTexture;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMEMORYBARRIERPROC(GLbitfield barriers);
    private PFNGLMEMORYBARRIERPROC _glMemoryBarrier;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE1DPROC(GLenum target, GLsizei levels, GLenum internalformat, GLsizei width);
    private PFNGLTEXSTORAGE1DPROC _glTexStorage1D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE2DPROC(GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height);
    private PFNGLTEXSTORAGE2DPROC _glTexStorage2D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE3DPROC(GLenum target, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth);
    private PFNGLTEXSTORAGE3DPROC _glTexStorage3D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC(GLenum mode, GLuint id, GLsizei instancecount);
    private PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC _glDrawTransformFeedbackInstanced;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC(GLenum mode, GLuint id, GLuint stream, GLsizei instancecount);
    private PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC _glDrawTransformFeedbackStreamInstanced;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPOINTERVPROC(GLenum pname, void** @params);
    private PFNGLGETPOINTERVPROC _glGetPointerv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERDATAPROC(GLenum target, GLenum internalformat, GLenum format, GLenum type, void* data);
    private PFNGLCLEARBUFFERDATAPROC _glClearBufferData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERSUBDATAPROC(GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data);
    private PFNGLCLEARBUFFERSUBDATAPROC _glClearBufferSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISPATCHCOMPUTEPROC(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z);
    private PFNGLDISPATCHCOMPUTEPROC _glDispatchCompute;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISPATCHCOMPUTEINDIRECTPROC(void* indirect);
    private PFNGLDISPATCHCOMPUTEINDIRECTPROC _glDispatchComputeIndirect;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYIMAGESUBDATAPROC(GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ, GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY, GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth);
    private PFNGLCOPYIMAGESUBDATAPROC _glCopyImageSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERPARAMETERIPROC(GLenum target, GLenum pname, GLint param);
    private PFNGLFRAMEBUFFERPARAMETERIPROC _glFramebufferParameteri;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFRAMEBUFFERPARAMETERIVPROC(GLenum target, GLenum pname, GLint* parameters);
    private PFNGLGETFRAMEBUFFERPARAMETERIVPROC _glGetFramebufferParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTERNALFORMATI64VPROC(GLenum target, GLenum internalformat, GLenum pname, GLsizei count, GLint64* @params);
    private PFNGLGETINTERNALFORMATI64VPROC _glGetInternalformati64v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATETEXSUBIMAGEPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth);
    private PFNGLINVALIDATETEXSUBIMAGEPROC _glInvalidateTexSubImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATETEXIMAGEPROC(GLuint texture, GLint level);
    private PFNGLINVALIDATETEXIMAGEPROC _glInvalidateTexImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEBUFFERSUBDATAPROC(GLuint buffer, GLintptr offset, GLsizeiptr length);
    private PFNGLINVALIDATEBUFFERSUBDATAPROC _glInvalidateBufferSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEBUFFERDATAPROC(GLuint buffer);
    private PFNGLINVALIDATEBUFFERDATAPROC _glInvalidateBufferData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEFRAMEBUFFERPROC(GLenum target, GLsizei numAttachments, GLenum* attachments);
    private PFNGLINVALIDATEFRAMEBUFFERPROC _glInvalidateFramebuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATESUBFRAMEBUFFERPROC(GLenum target, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height);
    private PFNGLINVALIDATESUBFRAMEBUFFERPROC _glInvalidateSubFramebuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSINDIRECTPROC(GLenum mode, void* indirect, GLsizei drawcount, GLsizei stride);
    private PFNGLMULTIDRAWARRAYSINDIRECTPROC _glMultiDrawArraysIndirect;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSINDIRECTPROC(GLenum mode, GLenum type, void* indirect, GLsizei drawcount, GLsizei stride);
    private PFNGLMULTIDRAWELEMENTSINDIRECTPROC _glMultiDrawElementsIndirect;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMINTERFACEIVPROC(GLuint program, GLenum programInterface, GLenum pname, GLint* parameters);
    private PFNGLGETPROGRAMINTERFACEIVPROC _glGetProgramInterfaceiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLuint PFNGLGETPROGRAMRESOURCEINDEXPROC(GLuint program, GLenum programInterface, GLchar* name);
    private PFNGLGETPROGRAMRESOURCEINDEXPROC _glGetProgramResourceIndex;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMRESOURCENAMEPROC(GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei* length, GLchar* name);
    private PFNGLGETPROGRAMRESOURCENAMEPROC _glGetProgramResourceName;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMRESOURCEIVPROC(GLuint program, GLenum programInterface, GLuint index, GLsizei propCount, GLenum* props, GLsizei bufSize, GLsizei* length, GLint* @params);
    private PFNGLGETPROGRAMRESOURCEIVPROC _glGetProgramResourceiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLint PFNGLGETPROGRAMRESOURCELOCATIONPROC(GLuint program, GLenum programInterface, GLchar* name);
    private PFNGLGETPROGRAMRESOURCELOCATIONPROC _glGetProgramResourceLocation;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLint PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC(GLuint program, GLenum programInterface, GLchar* name);
    private PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC _glGetProgramResourceLocationIndex;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETOBJECTPTRLABELPROC(void* ptr, GLsizei bufSize, GLsizei* length, GLchar* label);
    private PFNGLGETOBJECTPTRLABELPROC _glGetObjectPtrLabel;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBUFFERSTORAGEPROC(GLenum target, GLsizeiptr size, void* data, GLbitfield flags);
    private PFNGLBUFFERSTORAGEPROC _glBufferStorage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARTEXIMAGEPROC(GLuint texture, GLint level, GLenum format, GLenum type, void* data);
    private PFNGLCLEARTEXIMAGEPROC _glClearTexImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARTEXSUBIMAGEPROC(GLuint texture, GLint level, GLint xOffset, GLint yOffset, GLint zOffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* data);
    private PFNGLCLEARTEXSUBIMAGEPROC _glClearTexSubImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERSBASEPROC(GLenum target, GLuint first, GLsizei count, GLuint* buffers);
    private PFNGLBINDBUFFERSBASEPROC _glBindBuffersBase;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERSRANGEPROC(GLenum target, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizeiptr* sizes);
    private PFNGLBINDBUFFERSRANGEPROC _glBindBuffersRange;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTURESPROC(GLuint first, GLsizei count, GLuint* textures);
    private PFNGLBINDTEXTURESPROC _glBindTextures;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDSAMPLERSPROC(GLuint first, GLsizei count, GLuint* samplers);
    private PFNGLBINDSAMPLERSPROC _glBindSamplers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDIMAGETEXTURESPROC(GLuint first, GLsizei count, GLuint* textures);
    private PFNGLBINDIMAGETEXTURESPROC _glBindImageTextures;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXBUFFERSPROC(GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides);
    private PFNGLBINDVERTEXBUFFERSPROC _glBindVertexBuffers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLIPCONTROLPROC(GLenum origin, GLenum depth);
    private PFNGLCLIPCONTROLPROC _glClipControl;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATETRANSFORMFEEDBACKSPROC(GLsizei n, GLuint* ids);
    private PFNGLCREATETRANSFORMFEEDBACKSPROC _glCreateTransformFeedbacks;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC(GLuint xfb, GLuint index, GLuint buffer);
    private PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC _glTransformFeedbackBufferBase;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC(GLuint xfb, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size);
    private PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC _glTransformFeedbackBufferRange;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKIVPROC(GLuint xfb, GLenum pname, GLint* param);
    private PFNGLGETTRANSFORMFEEDBACKIVPROC _glGetTransformFeedbackiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKI_VPROC(GLuint xfb, GLenum pname, GLuint index, GLint* param);
    private PFNGLGETTRANSFORMFEEDBACKI_VPROC _glGetTransformFeedbacki_v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKI64_VPROC(GLuint xfb, GLenum pname, GLuint index, GLint64* param);
    private PFNGLGETTRANSFORMFEEDBACKI64_VPROC _glGetTransformFeedbacki64_v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEBUFFERSPROC(GLsizei n, GLuint* buffers);
    private PFNGLCREATEBUFFERSPROC _glCreateBuffers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERSTORAGEPROC(GLuint buffer, GLsizeiptr size, void* data, GLbitfield flags);
    private PFNGLNAMEDBUFFERSTORAGEPROC _glNamedBufferStorage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERDATAPROC(GLuint buffer, GLsizeiptr size, void* data, GLenum usage);
    private PFNGLNAMEDBUFFERDATAPROC _glNamedBufferData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERSUBDATAPROC(GLuint buffer, GLintptr offset, GLsizeiptr size, void* data);
    private PFNGLNAMEDBUFFERSUBDATAPROC _glNamedBufferSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYNAMEDBUFFERSUBDATAPROC(GLuint readBuffer, GLuint writeBuffer, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size);
    private PFNGLCOPYNAMEDBUFFERSUBDATAPROC _glCopyNamedBufferSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDBUFFERDATAPROC(GLuint buffer, GLenum internalformat, GLenum format, GLenum type, void* data);
    private PFNGLCLEARNAMEDBUFFERDATAPROC _glClearNamedBufferData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDBUFFERSUBDATAPROC(GLuint buffer, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* data);
    private PFNGLCLEARNAMEDBUFFERSUBDATAPROC _glClearNamedBufferSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void* PFNGLMAPNAMEDBUFFERPROC(GLuint buffer, GLenum access);
    private PFNGLMAPNAMEDBUFFERPROC _glMapNamedBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void* PFNGLMAPNAMEDBUFFERRANGEPROC(GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access);
    private PFNGLMAPNAMEDBUFFERRANGEPROC _glMapNamedBufferRange;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLboolean PFNGLUNMAPNAMEDBUFFERPROC(GLuint buffer);
    private PFNGLUNMAPNAMEDBUFFERPROC _glUnmapNamedBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC(GLuint buffer, GLintptr offset, GLsizeiptr length);
    private PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC _glFlushMappedNamedBufferRange;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPARAMETERIVPROC(GLuint buffer, GLenum pname, GLint* parameters);
    private PFNGLGETNAMEDBUFFERPARAMETERIVPROC _glGetNamedBufferParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPARAMETERI64VPROC(GLuint buffer, GLenum pname, GLint64* parameters);
    private PFNGLGETNAMEDBUFFERPARAMETERI64VPROC _glGetNamedBufferParameteri64v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPOINTERVPROC(GLuint buffer, GLenum pname, void** parameters);
    private PFNGLGETNAMEDBUFFERPOINTERVPROC _glGetNamedBufferPointerv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERSUBDATAPROC(GLuint buffer, GLintptr offset, GLsizeiptr size, void* data);
    private PFNGLGETNAMEDBUFFERSUBDATAPROC _glGetNamedBufferSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEFRAMEBUFFERSPROC(GLsizei n, GLuint* framebuffers);
    private PFNGLCREATEFRAMEBUFFERSPROC _glCreateFramebuffers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC(GLuint framebuffer, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer);
    private PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC _glNamedFramebufferRenderbuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC(GLuint framebuffer, GLenum pname, GLint param);
    private PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC _glNamedFramebufferParameteri;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERTEXTUREPROC(GLuint framebuffer, GLenum attachment, GLuint texture, GLint level);
    private PFNGLNAMEDFRAMEBUFFERTEXTUREPROC _glNamedFramebufferTexture;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC(GLuint framebuffer, GLenum attachment, GLuint texture, GLint level, GLint layer);
    private PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC _glNamedFramebufferTextureLayer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC(GLuint framebuffer, GLenum buf);
    private PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC _glNamedFramebufferDrawBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC(GLuint framebuffer, GLsizei n, GLenum* bufs);
    private PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC _glNamedFramebufferDrawBuffers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC(GLuint framebuffer, GLenum src);
    private PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC _glNamedFramebufferReadBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC(GLuint framebuffer, GLsizei numAttachments, GLenum* attachments);
    private PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC _glInvalidateNamedFramebufferData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC(GLuint framebuffer, GLsizei numAttachments, GLenum* attachments, GLint x, GLint y, GLsizei width, GLsizei height);
    private PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC _glInvalidateNamedFramebufferSubData;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERIVPROC(GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLint* value);
    private PFNGLCLEARNAMEDFRAMEBUFFERIVPROC _glClearNamedFramebufferiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC(GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLuint* value);
    private PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC _glClearNamedFramebufferuiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERFVPROC(GLuint framebuffer, GLenum buffer, GLint drawbuffer, GLfloat* value);
    private PFNGLCLEARNAMEDFRAMEBUFFERFVPROC _glClearNamedFramebufferfv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERFIPROC(GLuint framebuffer, GLenum buffer, GLfloat depth, GLint stencil);
    private PFNGLCLEARNAMEDFRAMEBUFFERFIPROC _glClearNamedFramebufferfi;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLITNAMEDFRAMEBUFFERPROC(GLuint readFramebuffer, GLuint drawFramebuffer, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GLbitfield mask, GLenum filter);
    private PFNGLBLITNAMEDFRAMEBUFFERPROC _glBlitNamedFramebuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLenum PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC(GLuint framebuffer, GLenum target);
    private PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC _glCheckNamedFramebufferStatus;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC(GLuint framebuffer, GLenum pname, GLint* param);
    private PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC _glGetNamedFramebufferParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC(GLuint framebuffer, GLenum attachment, GLenum pname, GLint* params_);
    private PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC _glGetNamedFramebufferAttachmentParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATERENDERBUFFERSPROC(GLsizei n, GLuint* renderbuffers);
    private PFNGLCREATERENDERBUFFERSPROC _glCreateRenderbuffers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDRENDERBUFFERSTORAGEPROC(GLuint renderbuffer, GLenum internalformat, GLsizei width, GLsizei height);
    private PFNGLNAMEDRENDERBUFFERSTORAGEPROC _glNamedRenderbufferStorage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC(GLuint renderbuffer, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height);
    private PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC _glNamedRenderbufferStorageMultisample;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC(GLuint renderbuffer, GLenum pname, GLint* param);
    private PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC _glGetNamedRenderbufferParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATETEXTURESPROC(GLenum target, GLsizei n, GLuint* textures);
    private PFNGLCREATETEXTURESPROC _glCreateTextures;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBUFFERPROC(GLuint texture, GLenum internalformat, GLuint buffer);
    private PFNGLTEXTUREBUFFERPROC _glTextureBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBUFFERRANGEPROC(GLuint texture, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size);
    private PFNGLTEXTUREBUFFERRANGEPROC _glTextureBufferRange;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE1DPROC(GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width);
    private PFNGLTEXTURESTORAGE1DPROC _glTextureStorage1D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE2DPROC(GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height);
    private PFNGLTEXTURESTORAGE2DPROC _glTextureStorage2D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE3DPROC(GLuint texture, GLsizei levels, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth);
    private PFNGLTEXTURESTORAGE3DPROC _glTextureStorage3D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC(GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations);
    private PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC _glTextureStorage2DMultisample;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC(GLuint texture, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedsamplelocations);
    private PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC _glTextureStorage3DMultisample;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE1DPROC(GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels);
    private PFNGLTEXTURESUBIMAGE1DPROC _glTextureSubImage1D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE2DPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels);
    private PFNGLTEXTURESUBIMAGE2DPROC _glTextureSubImage2D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE3DPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* pixels);
    private PFNGLTEXTURESUBIMAGE3DPROC _glTextureSubImage3D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC(GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data);
    private PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC _glCompressedTextureSubImage1D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, void* data);
    private PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC _glCompressedTextureSubImage2D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize, void* data);
    private PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC _glCompressedTextureSubImage3D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE1DPROC(GLuint texture, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width);
    private PFNGLCOPYTEXTURESUBIMAGE1DPROC _glCopyTextureSubImage1D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE2DPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height);
    private PFNGLCOPYTEXTURESUBIMAGE2DPROC _glCopyTextureSubImage2D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE3DPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLint x, GLint y, GLsizei width, GLsizei height);
    private PFNGLCOPYTEXTURESUBIMAGE3DPROC _glCopyTextureSubImage3D;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERFPROC(GLuint texture, GLenum pname, GLfloat param);
    private PFNGLTEXTUREPARAMETERFPROC _glTextureParameterf;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERFVPROC(GLuint texture, GLenum pname, GLfloat* param);
    private PFNGLTEXTUREPARAMETERFVPROC _glTextureParameterfv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIPROC(GLuint texture, GLenum pname, GLint param);
    private PFNGLTEXTUREPARAMETERIPROC _glTextureParameteri;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIIVPROC(GLuint texture, GLenum pname, GLint* param);
    private PFNGLTEXTUREPARAMETERIIVPROC _glTextureParameterIiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIUIVPROC(GLuint texture, GLenum pname, GLuint* param);
    private PFNGLTEXTUREPARAMETERIUIVPROC _glTextureParameterIuiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIVPROC(GLuint texture, GLenum pname, GLint* param);
    private PFNGLTEXTUREPARAMETERIVPROC _glTextureParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENERATETEXTUREMIPMAPPROC(GLuint texture);
    private PFNGLGENERATETEXTUREMIPMAPPROC _glGenerateTextureMipmap;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTUREUNITPROC(GLuint unit, GLuint texture);
    private PFNGLBINDTEXTUREUNITPROC _glBindTextureUnit;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREIMAGEPROC(GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pixels);
    private PFNGLGETTEXTUREIMAGEPROC _glGetTextureImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC(GLuint texture, GLint level, GLsizei bufSize, void* pixels);
    private PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC _glGetCompressedTextureImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURELEVELPARAMETERFVPROC(GLuint texture, GLint level, GLenum pname, GLfloat* param);
    private PFNGLGETTEXTURELEVELPARAMETERFVPROC _glGetTextureLevelParameterfv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURELEVELPARAMETERIVPROC(GLuint texture, GLint level, GLenum pname, GLint* param);
    private PFNGLGETTEXTURELEVELPARAMETERIVPROC _glGetTextureLevelParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERFVPROC(GLuint texture, GLenum pname, GLfloat* param);
    private PFNGLGETTEXTUREPARAMETERFVPROC _glGetTextureParameterfv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIIVPROC(GLuint texture, GLenum pname, GLint* param);
    private PFNGLGETTEXTUREPARAMETERIIVPROC _glGetTextureParameterIiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIUIVPROC(GLuint texture, GLenum pname, GLuint* param);
    private PFNGLGETTEXTUREPARAMETERIUIVPROC _glGetTextureParameterIuiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIVPROC(GLuint texture, GLenum pname, GLint* param);
    private PFNGLGETTEXTUREPARAMETERIVPROC _glGetTextureParameteriv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEVERTEXARRAYSPROC(GLsizei n, GLuint* arrays);
    private PFNGLCREATEVERTEXARRAYSPROC _glCreateVertexArrays;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEVERTEXARRAYATTRIBPROC(GLuint vaobj, GLuint index);
    private PFNGLDISABLEVERTEXARRAYATTRIBPROC _glDisableVertexArrayAttrib;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEVERTEXARRAYATTRIBPROC(GLuint vaobj, GLuint index);
    private PFNGLENABLEVERTEXARRAYATTRIBPROC _glEnableVertexArrayAttrib;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYELEMENTBUFFERPROC(GLuint vaobj, GLuint buffer);
    private PFNGLVERTEXARRAYELEMENTBUFFERPROC _glVertexArrayElementBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYVERTEXBUFFERPROC(GLuint vaobj, GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride);
    private PFNGLVERTEXARRAYVERTEXBUFFERPROC _glVertexArrayVertexBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYVERTEXBUFFERSPROC(GLuint vaobj, GLuint first, GLsizei count, GLuint* buffers, GLintptr* offsets, GLsizei* strides);
    private PFNGLVERTEXARRAYVERTEXBUFFERSPROC _glVertexArrayVertexBuffers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBBINDINGPROC(GLuint vaobj, GLuint attribindex, GLuint bindingindex);
    private PFNGLVERTEXARRAYATTRIBBINDINGPROC _glVertexArrayAttribBinding;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBFORMATPROC(GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset);
    private PFNGLVERTEXARRAYATTRIBFORMATPROC _glVertexArrayAttribFormat;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBIFORMATPROC(GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset);
    private PFNGLVERTEXARRAYATTRIBIFORMATPROC _glVertexArrayAttribIFormat;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBLFORMATPROC(GLuint vaobj, GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset);
    private PFNGLVERTEXARRAYATTRIBLFORMATPROC _glVertexArrayAttribLFormat;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYBINDINGDIVISORPROC(GLuint vaobj, GLuint bindingindex, GLuint divisor);
    private PFNGLVERTEXARRAYBINDINGDIVISORPROC _glVertexArrayBindingDivisor;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYIVPROC(GLuint vaobj, GLenum pname, GLint* param);
    private PFNGLGETVERTEXARRAYIVPROC _glGetVertexArrayiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYINDEXEDIVPROC(GLuint vaobj, GLuint index, GLenum pname, GLint* param);
    private PFNGLGETVERTEXARRAYINDEXEDIVPROC _glGetVertexArrayIndexediv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYINDEXED64IVPROC(GLuint vaobj, GLuint index, GLenum pname, GLint64* param);
    private PFNGLGETVERTEXARRAYINDEXED64IVPROC _glGetVertexArrayIndexed64iv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATESAMPLERSPROC(GLsizei n, GLuint* samplers);
    private PFNGLCREATESAMPLERSPROC _glCreateSamplers;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEPROGRAMPIPELINESPROC(GLsizei n, GLuint* pipelines);
    private PFNGLCREATEPROGRAMPIPELINESPROC _glCreateProgramPipelines;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEQUERIESPROC(GLenum target, GLsizei n, GLuint* ids);
    private PFNGLCREATEQUERIESPROC _glCreateQueries;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTI64VPROC(GLuint id, GLuint buffer, GLenum pname, GLintptr offset);
    private PFNGLGETQUERYBUFFEROBJECTI64VPROC _glGetQueryBufferObjecti64v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTIVPROC(GLuint id, GLuint buffer, GLenum pname, GLintptr offset);
    private PFNGLGETQUERYBUFFEROBJECTIVPROC _glGetQueryBufferObjectiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTUI64VPROC(GLuint id, GLuint buffer, GLenum pname, GLintptr offset);
    private PFNGLGETQUERYBUFFEROBJECTUI64VPROC _glGetQueryBufferObjectui64v;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTUIVPROC(GLuint id, GLuint buffer, GLenum pname, GLintptr offset);
    private PFNGLGETQUERYBUFFEROBJECTUIVPROC _glGetQueryBufferObjectuiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMEMORYBARRIERBYREGIONPROC(GLbitfield barriers);
    private PFNGLMEMORYBARRIERBYREGIONPROC _glMemoryBarrierByRegion;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURESUBIMAGEPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, GLsizei bufSize, void* pixels);
    private PFNGLGETTEXTURESUBIMAGEPROC _glGetTextureSubImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLsizei bufSize, void* pixels);
    private PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC _glGetCompressedTextureSubImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLenum PFNGLGETGRAPHICSRESETSTATUSPROC();
    private PFNGLGETGRAPHICSRESETSTATUSPROC _glGetGraphicsResetStatus;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNCOMPRESSEDTEXIMAGEPROC(GLenum target, GLint lod, GLsizei bufSize, void* pixels);
    private PFNGLGETNCOMPRESSEDTEXIMAGEPROC _glGetnCompressedTexImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNTEXIMAGEPROC(GLenum target, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pixels);
    private PFNGLGETNTEXIMAGEPROC _glGetnTexImage;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMDVPROC(GLuint program, GLint location, GLsizei bufSize, GLdouble* parameters);
    private PFNGLGETNUNIFORMDVPROC _glGetnUniformdv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMFVPROC(GLuint program, GLint location, GLsizei bufSize, GLfloat* parameters);
    private PFNGLGETNUNIFORMFVPROC _glGetnUniformfv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMIVPROC(GLuint program, GLint location, GLsizei bufSize, GLint* parameters);
    private PFNGLGETNUNIFORMIVPROC _glGetnUniformiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMUIVPROC(GLuint program, GLint location, GLsizei bufSize, GLuint* parameters);
    private PFNGLGETNUNIFORMUIVPROC _glGetnUniformuiv;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLREADNPIXELSPROC(GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, GLsizei bufSize, void* data);
    private PFNGLREADNPIXELSPROC _glReadnPixels;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBARRIERPROC();
    private PFNGLTEXTUREBARRIERPROC _glTextureBarrier;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSPECIALIZESHADERPROC(GLuint shader, GLchar* pEntryPoint, GLuint numSpecializationConstants, GLuint* pConstantIndex, GLuint* pConstantValue);
    private PFNGLSPECIALIZESHADERPROC _glSpecializeShader;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC(GLenum mode, void* indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride);
    private PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC _glMultiDrawArraysIndirectCount;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC(GLenum mode, GLenum type, void* indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride);
    private PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC _glMultiDrawElementsIndirectCount;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOLYGONOFFSETCLAMPPROC(GLfloat factor, GLfloat units, GLfloat clamp);
    private PFNGLPOLYGONOFFSETCLAMPPROC _glPolygonOffsetClamp;
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERSTORAGEBLOCKBINDINGPROC(GLuint program, GLuint storageBlockIndex, GLuint storageBlockBinding);
    private PFNGLSHADERSTORAGEBLOCKBINDINGPROC _glShaderStorageBlockBinding;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXBUFFERRANGEPROC(GLenum target, GLenum internalformat, GLuint buffer, GLintptr offset, GLsizeiptr size);
    private PFNGLTEXBUFFERRANGEPROC _glTexBufferRange;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE2DMULTISAMPLEPROC(GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLboolean fixedsamplelocations);
    private PFNGLTEXSTORAGE2DMULTISAMPLEPROC _glTexStorage2DMultisample;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE3DMULTISAMPLEPROC(GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedsamplelocations);
    private PFNGLTEXSTORAGE3DMULTISAMPLEPROC _glTexStorage3DMultisample;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREVIEWPROC(GLuint texture, GLenum target, GLuint origtexture, GLenum internalformat, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers);
    private PFNGLTEXTUREVIEWPROC _glTextureView;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXBUFFERPROC(GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride);
    private PFNGLBINDVERTEXBUFFERPROC _glBindVertexBuffer;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBFORMATPROC(GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint relativeoffset);
    private PFNGLVERTEXATTRIBFORMATPROC _glVertexAttribFormat;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBIFORMATPROC(GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset);
    private PFNGLVERTEXATTRIBIFORMATPROC _glVertexAttribIFormat;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBLFORMATPROC(GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset);
    private PFNGLVERTEXATTRIBLFORMATPROC _glVertexAttribLFormat;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBBINDINGPROC(GLuint attribindex, GLuint bindingindex);
    private PFNGLVERTEXATTRIBBINDINGPROC _glVertexAttribBinding;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXBINDINGDIVISORPROC(GLuint bindingindex, GLuint divisor);
    private PFNGLVERTEXBINDINGDIVISORPROC _glVertexBindingDivisor;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGECONTROLPROC(GLenum source, GLenum type, GLenum severity, GLsizei count, GLuint* ids, GLboolean enabled);
    private PFNGLDEBUGMESSAGECONTROLPROC _glDebugMessageControl;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGEINSERTPROC(GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* buf);
    private PFNGLDEBUGMESSAGEINSERTPROC _glDebugMessageInsert;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLDEBUGPROC(GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, GLchar* message, void* userParam);

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLDEBUGPROCSAFE( GLenum source, GLenum type, GLuint id, GLenum severity, string message, void* userParam );
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGECALLBACKPROC(GLDEBUGPROC callback, void* userParam);
    private PFNGLDEBUGMESSAGECALLBACKPROC _glDebugMessageCallback;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate GLuint PFNGLGETDEBUGMESSAGELOGPROC(GLuint count, GLsizei bufsize, GLenum* sources, GLenum* types, GLuint* ids, GLenum* severities, GLsizei* lengths, GLchar* messageLog);
    private PFNGLGETDEBUGMESSAGELOGPROC _glGetDebugMessageLog;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPUSHDEBUGGROUPPROC(GLenum source, GLuint id, GLsizei length, GLchar* message);
    private PFNGLPUSHDEBUGGROUPPROC _glPushDebugGroup;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOPDEBUGGROUPPROC();
    private PFNGLPOPDEBUGGROUPPROC _glPopDebugGroup;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLOBJECTLABELPROC(GLenum identifier, GLuint name, GLsizei length, GLchar* label);
    private PFNGLOBJECTLABELPROC _glObjectLabel;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETOBJECTLABELPROC(GLenum identifier, GLuint name, GLsizei bufSize, GLsizei* length, GLchar* label);
    private PFNGLGETOBJECTLABELPROC _glGetObjectLabel;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLOBJECTPTRLABELPROC(void* ptr, GLsizei length, GLchar* label);
    private PFNGLOBJECTPTRLABELPROC _glObjectPtrLabel;
}

#pragma warning restore IDE0079 // Remove unnecessary suppression
#pragma warning restore CS8618  // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore CS8603  // Possible null reference return.
#pragma warning restore IDE0060 // Remove unused parameter.
#pragma warning restore IDE1006 // Naming Styles.
#pragma warning restore IDE0090 // Use 'new(...)'.
#pragma warning restore CS8500  // This takes the address of, gets the size of, or declares a pointer to a managed type
