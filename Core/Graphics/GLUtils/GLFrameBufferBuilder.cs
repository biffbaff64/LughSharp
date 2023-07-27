// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Graphics.GLUtils;

public abstract class GLFrameBufferBuilder<TU> where TU : GLFrameBuffer< GLTexture >
{
    public int Width  { get; }
    public int Height { get; }

    public List< FrameBufferTextureAttachmentSpec > TextureAttachmentSpecs { get; } = new();

    public FrameBufferRenderBufferAttachmentSpec? StencilRenderBufferSpec            { get; set; }
    public FrameBufferRenderBufferAttachmentSpec? DepthRenderBufferSpec              { get; set; }
    public FrameBufferRenderBufferAttachmentSpec? PackedStencilDepthRenderBufferSpec { get; set; }

    public bool HasStencilRenderBuffer            { get; set; }
    public bool HasDepthRenderBuffer              { get; set; }
    public bool HasPackedStencilDepthRenderBuffer { get; set; }

    // ReSharper disable once PublicConstructorInAbstractClass
    public GLFrameBufferBuilder( int width, int height )
    {
        this.Width  = width;
        this.Height = height;
    }

    public GLFrameBufferBuilder<TU> AddColorTextureAttachment( int internalFormat, int format, int type )
    {
        TextureAttachmentSpecs.Add( new FrameBufferTextureAttachmentSpec( internalFormat, format, type ) );

        return this;
    }

    public GLFrameBufferBuilder<TU> AddBasicColorTextureAttachment( Pixmap.Format format )
    {
        var glFormat = PixmapFormat.ToGLFormat( format );
        var glType   = PixmapFormat.ToGLType( format );

        return AddColorTextureAttachment( glFormat, glFormat, glType );
    }

    public GLFrameBufferBuilder<TU> AddFloatAttachment( int internalFormat,
                                                        int format,
                                                        int type,
                                                        bool gpuOnly )
    {
        var spec = new FrameBufferTextureAttachmentSpec( internalFormat, format, type )
        {
            IsFloat   = true,
            IsGpuOnly = gpuOnly
        };

        TextureAttachmentSpecs.Add( spec );

        return this;
    }

    public GLFrameBufferBuilder<TU> AddDepthTextureAttachment( int internalFormat, int type )
    {
        var spec = new FrameBufferTextureAttachmentSpec( internalFormat, IGL20.GL_Depth_Component, type )
        {
            IsDepth = true
        };

        TextureAttachmentSpecs.Add( spec );

        return this;
    }

    public GLFrameBufferBuilder<TU> AddStencilTextureAttachment( int internalFormat, int type )
    {
        var spec = new FrameBufferTextureAttachmentSpec( internalFormat, IGL20.GL_Stencil_Attachment, type )
        {
            IsStencil = true
        };

        TextureAttachmentSpecs.Add( spec );

        return this;
    }

    public GLFrameBufferBuilder<TU> AddDepthRenderBuffer( int internalFormat )
    {
        DepthRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
        HasDepthRenderBuffer  = true;

        return this;
    }

    public GLFrameBufferBuilder<TU> AddStencilRenderBuffer( int internalFormat )
    {
        StencilRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
        HasStencilRenderBuffer  = true;

        return this;
    }

    public GLFrameBufferBuilder<TU> AddStencilDepthPackedRenderBuffer( int internalFormat )
    {
        PackedStencilDepthRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
        HasPackedStencilDepthRenderBuffer  = true;

        return this;
    }

    public GLFrameBufferBuilder<TU> AddBasicDepthRenderBuffer()
    {
        return AddDepthRenderBuffer( IGL20.GL_Depth_Component16 );
    }

    public GLFrameBufferBuilder<TU> AddBasicStencilRenderBuffer()
    {
        return AddStencilRenderBuffer( IGL20.GL_Stencil_Index8 );
    }

    public GLFrameBufferBuilder<TU> AddBasicStencilDepthPackedRenderBuffer()
    {
        return AddStencilDepthPackedRenderBuffer( IGL30.GL_Depth24_Stencil8 );
    }

    public abstract object Build();
}