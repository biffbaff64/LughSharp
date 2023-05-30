using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class GLFrameBuffer<T> where T : GLTexture
{
    public const int GL_Depth24_Stencil8_OES = 0x88F0;

    // the frame buffers
    public Dictionary< IApplication, List< GLFrameBuffer< T > >? >? Buffers { get; set; } = new();

    // the color buffer texture
    public List< T > TextureAttachments { get; set; } = new();

    // the default framebuffer handle, a.k.a screen.
    public int DefaultFramebufferHandle { get; set; }

    /** true if we have polled for the default handle already. */
    public bool DefaultFramebufferHandleInitialized { get; set; } = false;

    public int  FramebufferHandle              { get; set; }
    public int  DepthbufferHandle              { get; set; }
    public int  StencilbufferHandle            { get; set; }
    public int  DepthStencilPackedBufferHandle { get; set; }
    public bool HasDepthStencilPackedBuffer    { get; set; }
    public bool HasMultipleTexturesPresent     { get; set; }

    protected FrameBufferBuilder BufferBuilder { get; init; }

    protected GLFrameBuffer()
    {
        BufferBuilder = null!;
    }

    /// <summary>
    /// Creates a GLFrameBuffer from the specifications provided by bufferBuilder.
    /// </summary>
    protected GLFrameBuffer( FrameBufferBuilder bufferBuilder )
    {
        this.BufferBuilder = bufferBuilder;

        Build();
    }

    /// <summary>
    /// Convenience method to return the first Texture attachment present in the fbo.
    /// </summary>
    public T GetColorBufferTexture()
    {
        return TextureAttachments.First();
    }

    /// <summary>
    /// Override this method in a derived class to set up the
    /// backing texture as you like.
    /// </summary>
    protected abstract T CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec );

    /// <summary>
    /// Override this method in a derived class to dispose the
    /// backing texture as you like.
    /// </summary>
    protected abstract void DisposeColorTexture( T colorTexture );

    /// <summary>
    /// Override this method in a derived class to attach the backing
    /// texture to the GL framebuffer object.
    /// </summary>
    protected abstract void AttachFrameBufferColorTexture( T texture );

    protected void Build()
    {
        CheckValidBuilder();

        // iOS uses a different framebuffer handle! (not necessarily 0)
        if ( !DefaultFramebufferHandleInitialized )
        {
            DefaultFramebufferHandleInitialized = true;

            if ( Gdx.App.AppType == IApplication.ApplicationType.IOS )
            {
                IntBuffer intbuf = ByteBuffer.AllocateDirect( ( 16 * sizeof( int ) ) / 8 ).Order
                    ( ByteOrder.NativeOrder ).AsIntBuffer();

                Gdx.GL20.GLGetIntegerv( IGL20.GL_Framebuffer_Binding, intbuf );
                DefaultFramebufferHandle = intbuf.Get( 0 );
            }
            else
            {
                DefaultFramebufferHandle = 0;
            }
        }

        FramebufferHandle = Gdx.GL20.GLGenFramebuffer();
        Gdx.GL20.GLBindFramebuffer( IGL20.GL_Framebuffer, FramebufferHandle );

        var width  = BufferBuilder.Width;
        var height = BufferBuilder.Height;

        if ( BufferBuilder.HasDepthRenderBuffer )
        {
            DepthbufferHandle = Gdx.GL20.GLGenRenderbuffer();
            Gdx.GL20.GLBindRenderbuffer( IGL20.GL_Renderbuffer, DepthbufferHandle );

            Gdx.GL20.GLRenderbufferStorage
                ( IGL20.GL_Renderbuffer, BufferBuilder.DepthRenderBufferSpec!.InternalFormat, width, height );
        }

        if ( BufferBuilder.HasStencilRenderBuffer )
        {
            StencilbufferHandle = Gdx.GL20.GLGenRenderbuffer();
            Gdx.GL20.GLBindRenderbuffer( IGL20.GL_Renderbuffer, StencilbufferHandle );

            Gdx.GL20.GLRenderbufferStorage
                ( IGL20.GL_Renderbuffer, BufferBuilder.StencilRenderBufferSpec!.InternalFormat, width, height );
        }

        if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
        {
            DepthStencilPackedBufferHandle = Gdx.GL20.GLGenRenderbuffer();
            Gdx.GL20.GLBindRenderbuffer( IGL20.GL_Renderbuffer, DepthStencilPackedBufferHandle );

            Gdx.GL20.GLRenderbufferStorage
                (
                 IGL20.GL_Renderbuffer,
                 BufferBuilder.PackedStencilDepthRenderBufferSpec!.InternalFormat,
                 width,
                 height
                );
        }

        HasMultipleTexturesPresent = BufferBuilder.TextureAttachmentSpecs.Count > 1;

        var colorTextureCounter = 0;

        if ( HasMultipleTexturesPresent )
        {
            foreach ( FrameBufferTextureAttachmentSpec attachmentSpec in BufferBuilder.TextureAttachmentSpecs )
            {
                T texture = CreateTexture( attachmentSpec );
                TextureAttachments.Add( texture );

                if ( attachmentSpec.IsColorTexture )
                {
                    Gdx.GL20.GLFramebufferTexture2D
                        (
                         IGL20.GL_Framebuffer,
                         IGL20.GL_Color_Attachment0 + colorTextureCounter,
                         IGL20.GL_Texture_2D,
                         texture.GetTextureObjectHandle(),
                         0
                        );

                    colorTextureCounter++;
                }
                else if ( attachmentSpec.IsDepth )
                {
                    Gdx.GL20.GLFramebufferTexture2D
                        (
                         IGL20.GL_Framebuffer,
                         IGL20.GL_Depth_Attachment,
                         IGL20.GL_Texture_2D,
                         texture.GetTextureObjectHandle(),
                         0
                        );
                }
                else if ( attachmentSpec.IsStencil )
                {
                    Gdx.GL20.GLFramebufferTexture2D
                        (
                         IGL20.GL_Framebuffer,
                         IGL20.GL_Stencil_Attachment,
                         IGL20.GL_Texture_2D,
                         texture.GetTextureObjectHandle(),
                         0
                        );
                }
            }
        }
        else
        {
            T texture = CreateTexture( BufferBuilder.TextureAttachmentSpecs.First() );
            TextureAttachments.Add( texture );

            Gdx.GL20.GLBindTexture( texture.GLTarget, texture.GetTextureObjectHandle() );
        }

        if ( HasMultipleTexturesPresent )
        {
            IntBuffer buffer = BufferUtils.NewIntBuffer( colorTextureCounter );

            for ( var i = 0; i < colorTextureCounter; i++ )
            {
                buffer.Put( IGL20.GL_Color_Attachment0 + i );
            }

            buffer.Position = 0;

            Gdx.GL30.GLDrawBuffers( colorTextureCounter, buffer );
        }
        else
        {
            AttachFrameBufferColorTexture( TextureAttachments.First() );
        }

        if ( BufferBuilder.HasDepthRenderBuffer )
        {
            Gdx.GL20.GLFramebufferRenderbuffer
                ( IGL20.GL_Framebuffer, IGL20.GL_Depth_Attachment, IGL20.GL_Renderbuffer, DepthbufferHandle );
        }

        if ( BufferBuilder.HasStencilRenderBuffer )
        {
            Gdx.GL20.GLFramebufferRenderbuffer
                ( IGL20.GL_Framebuffer, IGL20.GL_Stencil_Attachment, IGL20.GL_Renderbuffer, StencilbufferHandle );
        }

        if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
        {
            Gdx.GL20.GLFramebufferRenderbuffer
                (
                 IGL20.GL_Framebuffer,
                 IGL30.GL_Depth_Stencil_Attachment,
                 IGL20.GL_Renderbuffer,
                 DepthStencilPackedBufferHandle
                );
        }

        Gdx.GL20.GLBindRenderbuffer( IGL20.GL_Renderbuffer, 0 );

        foreach ( T texture in TextureAttachments )
        {
            Gdx.GL20.GLBindTexture( texture.GLTarget, 0 );
        }

        var result = Gdx.GL20.GLCheckFramebufferStatus( IGL20.GL_Framebuffer );

        if ( ( result == IGL20.GL_Framebuffer_Unsupported )
             && BufferBuilder is { HasDepthRenderBuffer: true, HasStencilRenderBuffer: true }
             && ( Gdx.Graphics.SupportsExtension( "GL_OES_packed_depth_stencil" )
                  || Gdx.Graphics.SupportsExtension( "GL_EXT_packed_depth_stencil" ) ) )
        {
            if ( BufferBuilder.HasDepthRenderBuffer )
            {
                Gdx.GL20.GLDeleteRenderbuffer( DepthbufferHandle );
                DepthbufferHandle = 0;
            }

            if ( BufferBuilder.HasStencilRenderBuffer )
            {
                Gdx.GL20.GLDeleteRenderbuffer( StencilbufferHandle );

                StencilbufferHandle = 0;
            }

            if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
            {
                Gdx.GL20.GLDeleteRenderbuffer( DepthStencilPackedBufferHandle );
                DepthStencilPackedBufferHandle = 0;
            }

            DepthStencilPackedBufferHandle = Gdx.GL20.GLGenRenderbuffer();
            HasDepthStencilPackedBuffer    = true;

            Gdx.GL20.GLBindRenderbuffer( IGL20.GL_Renderbuffer, DepthStencilPackedBufferHandle );
            Gdx.GL20.GLRenderbufferStorage( IGL20.GL_Renderbuffer, GL_Depth24_Stencil8_OES, width, height );
            Gdx.GL20.GLBindRenderbuffer( IGL20.GL_Renderbuffer, 0 );

            Gdx.GL20.GLFramebufferRenderbuffer
                (
                 IGL20.GL_Framebuffer,
                 IGL20.GL_Depth_Attachment,
                 IGL20.GL_Renderbuffer,
                 DepthStencilPackedBufferHandle
                );

            Gdx.GL20.GLFramebufferRenderbuffer
                (
                 IGL20.GL_Framebuffer,
                 IGL20.GL_Stencil_Attachment,
                 IGL20.GL_Renderbuffer,
                 DepthStencilPackedBufferHandle
                );

            result = Gdx.GL20.GLCheckFramebufferStatus( IGL20.GL_Framebuffer );
        }

        Gdx.GL20.GLBindFramebuffer( IGL20.GL_Framebuffer, DefaultFramebufferHandle );

        if ( result != IGL20.GL_Framebuffer_Complete )
        {
            foreach ( T texture in TextureAttachments )
            {
                DisposeColorTexture( texture );
            }

            if ( HasDepthStencilPackedBuffer )
            {
                Gdx.GL20.GLDeleteBuffer( DepthStencilPackedBufferHandle );
            }
            else
            {
                if ( BufferBuilder.HasDepthRenderBuffer )
                {
                    Gdx.GL20.GLDeleteRenderbuffer( DepthbufferHandle );
                }

                if ( BufferBuilder.HasStencilRenderBuffer )
                {
                    Gdx.GL20.GLDeleteRenderbuffer( StencilbufferHandle );
                }
            }

            Gdx.GL20.GLDeleteFramebuffer( FramebufferHandle );

            if ( result == IGL20.GL_Framebuffer_Incomplete_Attachment )
                throw new IllegalStateException( "Frame buffer couldn't be constructed: incomplete attachment" );

            if ( result == IGL20.GL_Framebuffer_Incomplete_Dimensions )
                throw new IllegalStateException( "Frame buffer couldn't be constructed: incomplete dimensions" );

            if ( result == IGL20.GL_Framebuffer_Incomplete_Missing_Attachment )
                throw new IllegalStateException( "Frame buffer couldn't be constructed: missing attachment" );

            if ( result == IGL20.GL_Framebuffer_Unsupported )
                throw new IllegalStateException
                    ( "Frame buffer couldn't be constructed: unsupported combination of formats" );

            throw new IllegalStateException( "Frame buffer couldn't be constructed: unknown error " + result );
        }

        AddManagedFrameBuffer( Gdx.App, this );
    }

    private void CheckValidBuilder()
    {
        var runningGL30 = Gdx.Graphics.IsGL30Available();

        if ( !runningGL30 )
        {
            if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
            {
                throw new GdxRuntimeException( "Packed Stencil/Render render buffers are not available on GLES 2.0" );
            }

            if ( BufferBuilder.TextureAttachmentSpecs.Count > 1 )
            {
                throw new GdxRuntimeException( "Multiple render targets not available on GLES 2.0" );
            }

            foreach ( FrameBufferTextureAttachmentSpec spec in BufferBuilder.TextureAttachmentSpecs )
            {
                if ( spec.IsDepth )
                {
                    throw new GdxRuntimeException( "Depth texture FrameBuffer Attachment not available on GLES 2.0" );
                }

                if ( spec.IsStencil )
                {
                    throw new GdxRuntimeException( "Stencil texture FrameBuffer Attachment not available on GLES 2.0" );
                }

                if ( spec.IsFloat )
                {
                    if ( !Gdx.Graphics.SupportsExtension( "OES_texture_float" ) )
                    {
                        throw new GdxRuntimeException
                            ( "Float texture FrameBuffer Attachment not available on GLES 2.0" );
                    }
                }
            }
        }
    }

    /// <summary>
    /// Releases all resources associated with the FrameBuffer.
    /// </summary>
    public void Dispose()
    {
        foreach ( T texture in TextureAttachments )
        {
            DisposeColorTexture( texture );
        }

        if ( HasDepthStencilPackedBuffer )
        {
            Gdx.GL20.GLDeleteRenderbuffer( DepthStencilPackedBufferHandle );
        }
        else
        {
            if ( BufferBuilder.HasDepthRenderBuffer )
            {
                Gdx.GL20.GLDeleteRenderbuffer( DepthbufferHandle );
            }

            if ( BufferBuilder.HasStencilRenderBuffer )
            {
                Gdx.GL20.GLDeleteRenderbuffer( StencilbufferHandle );
            }
        }

        Gdx.GL20.GLDeleteFramebuffer( FramebufferHandle );

        if ( Buffers?[ Gdx.App ] != null )
        {
            Buffers[ Gdx.App ]?.Remove( this );
        }
    }

    /// <summary>
    /// Makes the frame buffer current so everything gets drawn to it.
    /// </summary>
    public void Bind()
    {
        Gdx.GL20.GLBindFramebuffer( IGL20.GL_Framebuffer, FramebufferHandle );
    }

    /// <summary>
    /// Unbinds the framebuffer, all drawing will be performed to the
    /// normal framebuffer from here on.
    /// </summary>
    public void Unbind()
    {
        Gdx.GL20.GLBindFramebuffer( IGL20.GL_Framebuffer, DefaultFramebufferHandle );
    }

    /// <summary>
    /// Binds the frame buffer and sets the viewport accordingly,
    /// so everything gets drawn to it.
    /// </summary>
    public void Begin()
    {
        Bind();
        SetFrameBufferViewport();
    }

    /// <summary>
    /// Sets viewport to the dimensions of framebuffer.
    /// Called by <see cref="Begin()"/>.
    /// </summary>
    public void SetFrameBufferViewport()
    {
        Gdx.GL20.GLViewport( 0, 0, BufferBuilder.Width, BufferBuilder.Height );
    }

    /// <summary>
    /// Unbinds the framebuffer, all drawing will be performed to the
    /// normal framebuffer from here on.
    /// </summary>
    public void End()
    {
        End( 0, 0, Gdx.Graphics.GetBackBufferWidth(), Gdx.Graphics.GetBackBufferHeight() );
    }

    /// <summary>
    /// Unbinds the framebuffer and sets viewport sizes, all drawing will be
    /// performed to the normal framebuffer from here on.
    /// </summary>
    /// <param name="x"> the x-axis position of the viewport in pixels </param>
    /// <param name="y"> the y-asis position of the viewport in pixels </param>
    /// <param name="width"> the width of the viewport in pixels </param>
    /// <param name="height"> the height of the viewport in pixels  </param>
    public void End( int x, int y, int width, int height )
    {
        Unbind();
        Gdx.GL20.GLViewport( x, y, width, height );
    }

    private void AddManagedFrameBuffer( IApplication app, GLFrameBuffer< T > frameBuffer )
    {
        List< GLFrameBuffer< T > > managedResources = Buffers?[ app ] ?? new List< GLFrameBuffer< T > >();

        managedResources.Add( frameBuffer );
        Buffers?.Put( app, managedResources );
    }

    /// <summary>
    /// Invalidates all frame buffers. This can be used when the OpenGL context is
    /// lost to rebuild all managed frame buffers. This assumes that the texture
    /// attached to this buffer has already been rebuild! Use with care. 
    /// </summary>
    public void InvalidateAllFrameBuffers( IApplication app )
    {
        ArgumentNullException.ThrowIfNull( app );

        if ( Buffers?[ app ] == null )
        {
            return;
        }

        for ( int i = 0; i < Buffers?[ app ]?.Count; i++ )
        {
            Buffers?[ app ]?[ i ].Build();
        }
    }

    public void ClearAllFrameBuffers( IApplication app )
    {
        Buffers?.Remove( app );
    }

    public StringBuilder GetManagedStatus( in StringBuilder builder )
    {
        builder.Append( "Managed buffers/app: { " );

        foreach ( IApplication app in Buffers!.Keys )
        {
            builder.Append( Buffers[ app ]?.Count );
            builder.Append( ' ' );
        }

        builder.Append( '}' );

        return builder;
    }

    public string ManagedStatus => GetManagedStatus( new StringBuilder() ).ToString();
}