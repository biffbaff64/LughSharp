// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.FrameBuffers;

[PublicAPI]
public class GLFrameBufferBuilder< TU >( int width, int height )
    where TU : GLFrameBuffer< GLTexture >
{
    public int Width  { get; } = width;
    public int Height { get; } = height;

    public List< FrameBufferTextureAttachmentSpec > TextureAttachmentSpecs { get; } = new();

    public FrameBufferRenderBufferAttachmentSpec? StencilRenderBufferSpec            { get; set; }
    public FrameBufferRenderBufferAttachmentSpec? DepthRenderBufferSpec              { get; set; }
    public FrameBufferRenderBufferAttachmentSpec? PackedStencilDepthRenderBufferSpec { get; set; }

    public bool HasStencilRenderBuffer            { get; set; }
    public bool HasDepthRenderBuffer              { get; set; }
    public bool HasPackedStencilDepthRenderBuffer { get; set; }

    public GLFrameBufferBuilder< TU > AddColorTextureAttachment( int internalFormat, int format, int type )
    {
        TextureAttachmentSpecs.Add( new FrameBufferTextureAttachmentSpec( internalFormat, format, type ) );

        return this;
    }

    public GLFrameBufferBuilder< TU > AddBasicColorTextureAttachment( Pixmap.ColorFormat format )
    {
        var glFormat = PixmapFormat.ToGLFormat( format );
        var glType   = PixmapFormat.ToGLType( format );

        return AddColorTextureAttachment( glFormat, glFormat, glType );
    }

    public GLFrameBufferBuilder< TU > AddFloatAttachment( int internalFormat,
                                                          int format,
                                                          int type,
                                                          bool gpuOnly )
    {
        var spec = new FrameBufferTextureAttachmentSpec( internalFormat, format, type )
        {
            IsFloat   = true,
            IsGpuOnly = gpuOnly,
        };

        TextureAttachmentSpecs.Add( spec );

        return this;
    }

    public GLFrameBufferBuilder< TU > AddDepthTextureAttachment( int internalFormat, int type )
    {
        var spec = new FrameBufferTextureAttachmentSpec( internalFormat, IGL.GL_DEPTH_COMPONENT, type )
        {
            IsDepth = true,
        };

        TextureAttachmentSpecs.Add( spec );

        return this;
    }

    public GLFrameBufferBuilder< TU > AddStencilTextureAttachment( int internalFormat, int type )
    {
        var spec = new FrameBufferTextureAttachmentSpec( internalFormat, IGL.GL_STENCIL_ATTACHMENT, type )
        {
            IsStencil = true,
        };

        TextureAttachmentSpecs.Add( spec );

        return this;
    }

    public GLFrameBufferBuilder< TU > AddDepthRenderBuffer( int internalFormat )
    {
        DepthRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
        HasDepthRenderBuffer  = true;

        return this;
    }

    public GLFrameBufferBuilder< TU > AddStencilRenderBuffer( int internalFormat )
    {
        StencilRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
        HasStencilRenderBuffer  = true;

        return this;
    }

    public GLFrameBufferBuilder< TU > AddStencilDepthPackedRenderBuffer( int internalFormat )
    {
        PackedStencilDepthRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
        HasPackedStencilDepthRenderBuffer  = true;

        return this;
    }

    public GLFrameBufferBuilder< TU > AddBasicDepthRenderBuffer()
    {
        return AddDepthRenderBuffer( IGL.GL_DEPTH_COMPONENT16 );
    }

    public GLFrameBufferBuilder< TU > AddBasicStencilRenderBuffer()
    {
        return AddStencilRenderBuffer( IGL.GL_STENCIL_INDEX8 );
    }

    public GLFrameBufferBuilder< TU > AddBasicStencilDepthPackedRenderBuffer()
    {
        return AddStencilDepthPackedRenderBuffer( IGL.GL_DEPTH24_STENCIL8 );
    }

    public virtual object Build()
    {
        throw new GdxRuntimeException( "This method must be overriden by derived class(es)" );
    }
}
