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


using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Utils.Buffers;

using Buffer = LughSharp.LibCore.Utils.Buffers.Buffer;

namespace LughSharp.Backends.DesktopGL.Graphics;

[PublicAPI]
public class DesktopGL30 : DesktopGL20, IGL30
{
    /// <inheritdoc />
    public void GLReadBuffer( int mode )
    {
    }

    /// <inheritdoc />
    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, Buffer indices )
    {
    }

    /// <inheritdoc />
    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, int offset )
    {
    }

    /// <inheritdoc />
    public void GLTexImage3D( int target, int level, int internalformat, int width, int height, int depth, int border, int format, int type, Buffer pixels )
    {
    }

    /// <inheritdoc />
    public void GLTexImage3D( int target, int level, int internalformat, int width, int height, int depth, int border, int format, int type, int offset )
    {
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void GLCopyTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
    }

    /// <inheritdoc />
    public void GLGenQueries( int n, int[] ids, int offset )
    {
    }

    /// <inheritdoc />
    public void GLGenQueries( int n, IntBuffer ids )
    {
    }

    /// <inheritdoc />
    public void GLDeleteQueries( int n, int[] ids, int offset )
    {
    }

    /// <inheritdoc />
    public void GLDeleteQueries( int n, IntBuffer ids )
    {
    }

    /// <inheritdoc />
    public bool GLIsQuery( int id )
    {
        return false;
    }

    /// <inheritdoc />
    public void GLBeginQuery( int target, int id )
    {
    }

    /// <inheritdoc />
    public void GLEndQuery( int target )
    {
    }

    /// <inheritdoc />
    public void GLGetQueryiv( int target, int pname, IntBuffer param )
    {
    }

    /// <inheritdoc />
    public void GLGetQueryObjectuiv( int id, int pname, IntBuffer param )
    {
    }

    /// <inheritdoc />
    public bool GLUnmapBuffer( int target )
    {
        return false;
    }

    /// <inheritdoc />
    public Buffer GLGetBufferPointerv( int target, int pname )
    {
        return null!;
    }

    /// <inheritdoc />
    public void GLDrawBuffers( int n, IntBuffer bufs )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix2X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix3X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix2X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix4X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix3X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix4X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLBlitFramebuffer( int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, int mask, int filter )
    {
    }

    /// <inheritdoc />
    public void GLRenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
    }

    /// <inheritdoc />
    public void GLFramebufferTextureLayer( int target, int attachment, int texture, int level, int layer )
    {
    }

    /// <inheritdoc />
    public Buffer GLMapBufferRange( int target, int offset, int length, int access )
    {
        return null!;
    }

    /// <inheritdoc />
    public void GLFlushMappedBufferRange( int target, int offset, int length )
    {
    }

    /// <inheritdoc />
    public void GLBindVertexArray( int array )
    {
    }

    /// <inheritdoc />
    public void GLDeleteVertexArrays( int n, int[] arrays, int offset )
    {
    }

    /// <inheritdoc />
    public void GLDeleteVertexArrays( int n, IntBuffer arrays )
    {
    }

    /// <inheritdoc />
    public void GLGenVertexArrays( int n, int[] arrays, int offset )
    {
    }

    /// <inheritdoc />
    public void GLGenVertexArrays( int n, IntBuffer arrays )
    {
    }

    /// <inheritdoc />
    public bool GLIsVertexArray( int array )
    {
        return false;
    }

    /// <inheritdoc />
    public void GLBeginTransformFeedback( int primitiveMode )
    {
    }

    /// <inheritdoc />
    public void GLEndTransformFeedback()
    {
    }

    /// <inheritdoc />
    public void GLBindBufferRange( int target, int index, int buffer, int offset, int size )
    {
    }

    /// <inheritdoc />
    public void GLBindBufferBase( int target, int index, int buffer )
    {
    }

    /// <inheritdoc />
    public void GLTransformFeedbackVaryings( int program, string[] varyings, int bufferMode )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttribIPointer( int index, int size, int type, int stride, int offset )
    {
    }

    /// <inheritdoc />
    public void GLGetVertexAttribIiv( int index, int pname, IntBuffer param )
    {
    }

    /// <inheritdoc />
    public void GLGetVertexAttribIuiv( int index, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttribI4I( int index, int x, int y, int z, int w )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttribI4Ui( int index, int x, int y, int z, int w )
    {
    }

    /// <inheritdoc />
    public void GLGetUniformuiv( int program, int location, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public int GLGetFragDataLocation( int program, string name )
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLUniform1Uiv( int location, int count, IntBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniform3Uiv( int location, int count, IntBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniform4Uiv( int location, int count, IntBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLClearBufferiv( int buffer, int drawbuffer, IntBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLClearBufferuiv( int buffer, int drawbuffer, IntBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLClearBufferfv( int buffer, int drawbuffer, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
    }

    /// <inheritdoc />
    public string GLGetStringi( int name, int index )
    {
        return string.Empty;
    }

    /// <inheritdoc />
    public void GLCopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
    }

    /// <inheritdoc />
    public void GLGetUniformIndices( int program, string[] uniformNames, IntBuffer uniformIndices )
    {
    }

    /// <inheritdoc />
    public void GLGetActiveUniformsiv( int program, int uniformCount, IntBuffer uniformIndices, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public int GLGetUniformBlockIndex( int program, string uniformBlockName )
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLGetActiveUniformBlockiv( int program, int uniformBlockIndex, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetActiveUniformBlockName( int program, int uniformBlockIndex, Buffer length, Buffer uniformBlockName )
    {
    }

    /// <inheritdoc />
    public string GLGetActiveUniformBlockName( int program, int uniformBlockIndex )
    {
        return string.Empty;
    }

    /// <inheritdoc />
    public void GLUniformBlockBinding( int program, int uniformBlockIndex, int uniformBlockBinding )
    {
    }

    /// <inheritdoc />
    public void GLDrawArraysInstanced( int mode, int first, int count, int instanceCount )
    {
    }

    /// <inheritdoc />
    public void GLDrawElementsInstanced( int mode, int count, int type, int indicesOffset, int instanceCount )
    {
    }

    /// <inheritdoc />
    public void GLGetInteger64V( int pname, LongBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetBufferParameteri64V( int target, int pname, LongBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGenSamplers( int count, int[] samplers, int offset )
    {
    }

    /// <inheritdoc />
    public void GLGenSamplers( int count, IntBuffer samplers )
    {
    }

    /// <inheritdoc />
    public void GLDeleteSamplers( int count, int[] samplers, int offset )
    {
    }

    /// <inheritdoc />
    public void GLDeleteSamplers( int count, IntBuffer samplers )
    {
    }

    /// <inheritdoc />
    public bool GLIsSampler( int sampler )
    {
        return false;
    }

    /// <inheritdoc />
    public void GLBindSampler( int unit, int sampler )
    {
    }

    /// <inheritdoc />
    public void GLSamplerParameteri( int sampler, int pname, int param )
    {
    }

    /// <inheritdoc />
    public void GLSamplerParameteriv( int sampler, int pname, IntBuffer param )
    {
    }

    /// <inheritdoc />
    public void GLSamplerParameterf( int sampler, int pname, float param )
    {
    }

    /// <inheritdoc />
    public void GLSamplerParameterfv( int sampler, int pname, FloatBuffer param )
    {
    }

    /// <inheritdoc />
    public void GLGetSamplerParameteriv( int sampler, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetSamplerParameterfv( int sampler, int pname, FloatBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttribDivisor( int index, int divisor )
    {
    }

    /// <inheritdoc />
    public void GLBindTransformFeedback( int target, int id )
    {
    }

    /// <inheritdoc />
    public void GLDeleteTransformFeedbacks( int n, int[] ids, int offset )
    {
    }

    /// <inheritdoc />
    public void GLDeleteTransformFeedbacks( int n, IntBuffer ids )
    {
    }

    /// <inheritdoc />
    public void GLGenTransformFeedbacks( int n, int[] ids, int offset )
    {
    }

    /// <inheritdoc />
    public void GLGenTransformFeedbacks( int n, IntBuffer ids )
    {
    }

    /// <inheritdoc />
    public bool GLIsTransformFeedback( int id )
    {
        return false;
    }

    /// <inheritdoc />
    public void GLPauseTransformFeedback()
    {
    }

    /// <inheritdoc />
    public void GLResumeTransformFeedback()
    {
    }

    /// <inheritdoc />
    public void GLProgramParameteri( int program, int pname, int value )
    {
    }

    /// <inheritdoc />
    public void GLInvalidateFramebuffer( int target, int numAttachments, IntBuffer attachments )
    {
    }

    /// <inheritdoc />
    public void GLInvalidateSubFramebuffer( int target, int numAttachments, IntBuffer attachments, int x, int y, int width, int height )
    {
    }
}
