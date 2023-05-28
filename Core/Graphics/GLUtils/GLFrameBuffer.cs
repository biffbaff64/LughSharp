namespace LibGDXSharp.Graphics.GLUtils;

public class GLFrameBuffer<T>
{
    public GLFrameBufferBuilder< T > bufferBuilder;

    // ========================================================================
    // Companion Classes
    // ========================================================================

    protected internal class FrameBufferTextureAttachmentSpec
    {
        internal int  InternalFormat;
        internal int  Format;
        internal int  Type;
        internal bool IsFloat;
        internal bool IsGpuOnly;
        internal bool IsDepth;
        internal bool IsStencil;

        public FrameBufferTextureAttachmentSpec( int internalformat, int format, int type )
        {
            this.InternalFormat = internalformat;
            this.Format         = format;
            this.Type           = type;
        }

        public bool ColorTexture => ( !IsDepth && !IsStencil );
    }

    protected internal class FrameBufferRenderBufferAttachmentSpec
    {
        internal int InternalFormat;

        public FrameBufferRenderBufferAttachmentSpec( int internalFormat )
        {
            this.InternalFormat = internalFormat;
        }
    }

    public abstract class GLFrameBufferBuilder<U>
    {
        protected int Width, Height;

        protected List< FrameBufferTextureAttachmentSpec > TextureAttachmentSpecs = new();

        protected FrameBufferRenderBufferAttachmentSpec StencilRenderBufferSpec;
        protected FrameBufferRenderBufferAttachmentSpec DepthRenderBufferSpec;
        protected FrameBufferRenderBufferAttachmentSpec PackedStencilDepthRenderBufferSpec;

        protected bool HasStencilRenderBuffer;
        protected bool HasDepthRenderBuffer;
        protected bool HasPackedStencilDepthRenderBuffer;

        public GLFrameBufferBuilder( int width, int height )
        {
            this.Width  = width;
            this.Height = height;
        }

        public GLFrameBufferBuilder< U > AddColorTextureAttachment( int internalFormat, int format, int type )
        {
            TextureAttachmentSpecs.Add( new FrameBufferTextureAttachmentSpec( internalFormat, format, type ) );

            return this;
        }

        public GLFrameBufferBuilder< U > AddBasicColorTextureAttachment( Pixmap.Format format )
        {
            var glFormat = PixmapFormat.ToGLFormat( format );
            var glType   = PixmapFormat.ToGLType( format );

            return AddColorTextureAttachment( glFormat, glFormat, glType );
        }

        public GLFrameBufferBuilder< U > AddFloatAttachment( int internalFormat,
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

        public GLFrameBufferBuilder< U > AddDepthTextureAttachment( int internalFormat, int type )
        {
            var spec = new FrameBufferTextureAttachmentSpec( internalFormat, IGL30.GL_Depth_Component, type )
            {
                IsDepth = true
            };

            TextureAttachmentSpecs.Add( spec );

            return this;
        }

        public GLFrameBufferBuilder< U > AddStencilTextureAttachment( int internalFormat, int type )
        {
            var spec = new FrameBufferTextureAttachmentSpec
                ( internalFormat, IGL30.GL_Stencil_Attachment, type )
                {
                    IsStencil = true
                };

            TextureAttachmentSpecs.Add( spec );

            return this;
        }

        public GLFrameBufferBuilder< U > AddDepthRenderBuffer( int internalFormat )
        {
            DepthRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
            HasDepthRenderBuffer  = true;

            return this;
        }

        public GLFrameBufferBuilder< U > AddStencilRenderBuffer( int internalFormat )
        {
            StencilRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
            HasStencilRenderBuffer  = true;

            return this;
        }

        public GLFrameBufferBuilder< U > AddStencilDepthPackedRenderBuffer( int internalFormat )
        {
            PackedStencilDepthRenderBufferSpec = new FrameBufferRenderBufferAttachmentSpec( internalFormat );
            HasPackedStencilDepthRenderBuffer  = true;

            return this;
        }

        public GLFrameBufferBuilder< U > AddBasicDepthRenderBuffer()
        {
            return AddDepthRenderBuffer( IGL20.GL_Depth_Component16 );
        }

        public GLFrameBufferBuilder< U > AddBasicStencilRenderBuffer()
        {
            return AddStencilRenderBuffer( IGL20.GL_Stencil_Index8 );
        }

        public GLFrameBufferBuilder< U > AddBasicStencilDepthPackedRenderBuffer()
        {
            return AddStencilDepthPackedRenderBuffer( IGL30.GL_Depth24_Stencil8 );
        }

        public abstract U Build();
    }

    public sealed class FrameBufferBuilder
        : GLFrameBufferBuilder< FrameBuffer >
    {
        public FrameBufferBuilder( int width, int height )
            : base( width, height )
        {
        }

        public override FrameBuffer Build()
        {
            return new FrameBuffer( this );
        }
    }

    public class FloatFrameBufferBuilder : GLFrameBufferBuilder< FloatFrameBuffer >
    {
        public FloatFrameBufferBuilder( int width, int height )
            : base( width, height )
        {
        }

        public override FloatFrameBuffer Build()
        {
            return new FloatFrameBuffer( this );
        }
    }

    public class FrameBufferCubemapBuilder : GLFrameBufferBuilder< FrameBufferCubemap >
    {
        public FrameBufferCubemapBuilder( int width, int height )
            : base( width, height )
        {
        }

        public override FrameBufferCubemap Build()
        {
            return new FrameBufferCubemap( this );
        }
    }
}