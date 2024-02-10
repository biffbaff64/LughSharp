// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Utils.Buffers;

using Buffer = LibGDXSharp.Files.Buffers.Buffer;

namespace LibGDXSharp.Backends.DesktopGL.Graphics;

public class DesktopGL30 : DesktopGL20, IGL30
{
    public void GLReadBuffer( int mode )
    {
    }

    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, int offset )
    {
    }

    public void GLTexImage3D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int depth,
                              int border,
                              int format,
                              int type,
                              int offset )
    {
    }

    public void GLTexSubImage3D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int zoffset,
                                 int width,
                                 int height,
                                 int depth,
                                 int format,
                                 int type,
                                 int offset )
    {
    }

    public void GLCopyTexSubImage3D( int target,
                                     int level,
                                     int xoffset,
                                     int yoffset,
                                     int zoffset,
                                     int x,
                                     int y,
                                     int width,
                                     int height )
    {
    }

    public void GLGenQueries( int n, int[] ids, int offset )
    {
    }

    public void GLGenQueries( int n, IntBuffer ids )
    {
    }

    public void GLDeleteQueries( int n, int[] ids, int offset )
    {
    }

    public void GLDeleteQueries( int n, IntBuffer ids )
    {
    }

    public bool GLIsQuery( int id ) => false;

    public void GLBeginQuery( int target, int id )
    {
    }

    public void GLEndQuery( int target )
    {
    }

    public void GLGetQueryiv( int target, int pname, IntBuffer param )
    {
    }

    public void GLGetQueryObjectuiv( int id, int pname, IntBuffer param )
    {
    }

    public bool GLUnmapBuffer( int target ) => false;

    public void GLDrawBuffers( int n, IntBuffer bufs )
    {
    }

    public void GLUniformMatrix2X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix3X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix2X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix4X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix3X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix4X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLBlitFramebuffer( int srcX0,
                                   int srcY0,
                                   int srcX1,
                                   int srcY1,
                                   int dstX0,
                                   int dstY0,
                                   int dstX1,
                                   int dstY1,
                                   int mask,
                                   int filter )
    {
    }

    public void GLRenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
    }

    public void GLFramebufferTextureLayer( int target, int attachment, int texture, int level, int layer )
    {
    }

    public void GLFlushMappedBufferRange( int target, int offset, int length )
    {
    }

    public void GLBindVertexArray( int array )
    {
    }

    public void GLDeleteVertexArrays( int n, int[] arrays, int offset )
    {
    }

    public void GLDeleteVertexArrays( int n, IntBuffer arrays )
    {
    }

    public void GLGenVertexArrays( int n, int[] arrays, int offset )
    {
    }

    public void GLGenVertexArrays( int n, IntBuffer arrays )
    {
    }

    public bool GLIsVertexArray( int array ) => false;

    public void GLBeginTransformFeedback( int primitiveMode )
    {
    }

    public void GLEndTransformFeedback()
    {
    }

    public void GLBindBufferRange( int target, int index, int buffer, int offset, int size )
    {
    }

    public void GLBindBufferBase( int target, int index, int buffer )
    {
    }

    public void GLTransformFeedbackVaryings( int program, string[] varyings, int bufferMode )
    {
    }

    public void GLVertexAttribIPointer( int index, int size, int type, int stride, int offset )
    {
    }

    public void GLGetVertexAttribIiv( int index, int pname, IntBuffer param )
    {
    }

    public void GLGetVertexAttribIuiv( int index, int pname, IntBuffer parameters )
    {
    }

    public void GLVertexAttribI4I( int index, int x, int y, int z, int w )
    {
    }

    public void GLVertexAttribI4Ui( int index, int x, int y, int z, int w )
    {
    }

    public void GLGetUniformuiv( int program, int location, IntBuffer parameters )
    {
    }

    public int GLGetFragDataLocation( int program, string name ) => 0;

    public void GLUniform1Uiv( int location, int count, IntBuffer value )
    {
    }

    public void GLUniform3Uiv( int location, int count, IntBuffer value )
    {
    }

    public void GLUniform4Uiv( int location, int count, IntBuffer value )
    {
    }

    public void GLClearBufferiv( int buffer, int drawbuffer, IntBuffer value )
    {
    }

    public void GLClearBufferuiv( int buffer, int drawbuffer, IntBuffer value )
    {
    }

    public void GLClearBufferfv( int buffer, int drawbuffer, FloatBuffer value )
    {
    }

    public void GLClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
    }

    public string GLGetStringi( int name, int index ) => null;

    public void GLCopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
    }

    public void GLGetUniformIndices( int program, string[] uniformNames, IntBuffer uniformIndices )
    {
    }

    public void GLGetActiveUniformsiv( int program, int uniformCount, IntBuffer uniformIndices, int pname, IntBuffer parameters )
    {
    }

    public int GLGetUniformBlockIndex( int program, string uniformBlockName ) => 0;

    public void GLGetActiveUniformBlockiv( int program, int uniformBlockIndex, int pname, IntBuffer parameters )
    {
    }

    public string GLGetActiveUniformBlockName( int program, int uniformBlockIndex ) => null;

    public void GLUniformBlockBinding( int program, int uniformBlockIndex, int uniformBlockBinding )
    {
    }

    public void GLDrawArraysInstanced( int mode, int first, int count, int instanceCount )
    {
    }

    public void GLDrawElementsInstanced( int mode, int count, int type, int indicesOffset, int instanceCount )
    {
    }

    public void GLGetInteger64V( int pname, LongBuffer parameters )
    {
    }

    public void GLGetBufferParameteri64V( int target, int pname, LongBuffer parameters )
    {
    }

    public void GLGenSamplers( int count, int[] samplers, int offset )
    {
    }

    public void GLGenSamplers( int count, IntBuffer samplers )
    {
    }

    public void GLDeleteSamplers( int count, int[] samplers, int offset )
    {
    }

    public void GLDeleteSamplers( int count, IntBuffer samplers )
    {
    }

    public bool GLIsSampler( int sampler ) => false;

    public void GLBindSampler( int unit, int sampler )
    {
    }

    public void GLSamplerParameteri( int sampler, int pname, int param )
    {
    }

    public void GLSamplerParameteriv( int sampler, int pname, IntBuffer param )
    {
    }

    public void GLSamplerParameterf( int sampler, int pname, float param )
    {
    }

    public void GLSamplerParameterfv( int sampler, int pname, FloatBuffer param )
    {
    }

    public void GLGetSamplerParameteriv( int sampler, int pname, IntBuffer parameters )
    {
    }

    public void GLGetSamplerParameterfv( int sampler, int pname, FloatBuffer parameters )
    {
    }

    public void GLVertexAttribDivisor( int index, int divisor )
    {
    }

    public void GLBindTransformFeedback( int target, int id )
    {
    }

    public void GLDeleteTransformFeedbacks( int n, int[] ids, int offset )
    {
    }

    public void GLDeleteTransformFeedbacks( int n, IntBuffer ids )
    {
    }

    public void GLGenTransformFeedbacks( int n, int[] ids, int offset )
    {
    }

    public void GLGenTransformFeedbacks( int n, IntBuffer ids )
    {
    }

    public bool GLIsTransformFeedback( int id ) => false;

    public void GLPauseTransformFeedback()
    {
    }

    public void GLResumeTransformFeedback()
    {
    }

    public void GLProgramParameteri( int program, int pname, int value )
    {
    }

    public void GLInvalidateFramebuffer( int target, int numAttachments, IntBuffer attachments )
    {
    }

    public void GLInvalidateSubFramebuffer( int target,
                                            int numAttachments,
                                            IntBuffer attachments,
                                            int x,
                                            int y,
                                            int width,
                                            int height )
    {
    }

    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, Buffer indices )
    {
    }

    public void GLTexImage3D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int depth,
                              int border,
                              int format,
                              int type,
                              Buffer pixels )
    {
    }

    public void GLTexSubImage3D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int zoffset,
                                 int width,
                                 int height,
                                 int depth,
                                 int format,
                                 int type,
                                 Buffer pixels )
    {
    }

    public Buffer GLGetBufferPointerv( int target, int pname ) => null;

    public Buffer GLMapBufferRange( int target, int offset, int length, int access ) => null;

    public void GLGetActiveUniformBlockName( int program, int uniformBlockIndex, Buffer length, Buffer uniformBlockName )
    {
    }
}
