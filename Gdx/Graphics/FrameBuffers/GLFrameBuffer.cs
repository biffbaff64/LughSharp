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


using System.Text;

using LibGDXSharp.Gdx.Core;
using LibGDXSharp.Gdx.Utils;
using LibGDXSharp.Gdx.Utils.Buffers;
using LibGDXSharp.Gdx.Utils.Collections.Extensions;

namespace LibGDXSharp.Gdx.Graphics.FrameBuffers;

/// <summary>
///     Encapsulates OpenGL ES 2.0 frame buffer objects. This is a simple helper
///     class which should cover most FBO uses. It will automatically create a
///     gltexture for the color attachment and a renderbuffer for the depth buffer.
///     You can get a hold of the gltexture by <see cref="GLFrameBuffer{T}.GetColorBufferTexture()" />.
///     This class will only work with OpenGL ES 2.0.
///     <para>
///         FrameBuffers are managed. In case of an OpenGL context loss, which only
///         happens on Android when a user switches to another application or receives
///         an incoming call, the framebuffer will be automatically recreated.
///     </para>
///     <para>
///         A FrameBuffer must be disposed if it is no longer needed
///     </para>
/// </summary>
/// <typeparam name="T">
///     Types which derive from GLTexture, such as Texture, Cubemap, TextureArray.
/// </typeparam>
[PublicAPI]
public class GLFrameBuffer<T> : IDisposable where T : GLTexture
{
    public const int GL_DEPTH24_STENCIL8_OES = 0x88F0;

    protected GLFrameBuffer() => BufferBuilder = null!;

    /// <summary>
    ///     Creates a GLFrameBuffer from the specifications provided
    ///     by bufferBuilder.
    /// </summary>
    protected GLFrameBuffer( GLFrameBufferBuilder< GLFrameBuffer< GLTexture > > bufferBuilder )
    {
        BufferBuilder = bufferBuilder;

        Build();
    }

    // the frame buffers
    public Dictionary< IApplication, List< GLFrameBuffer< T > >? >? Buffers { get; set; } = new();

    // the color buffer texture
    public List< T > TextureAttachments { get; set; } = new();

    // the default framebuffer handle, a.k.a screen.
    public int DefaultFramebufferHandle { get; set; }

    // true if we have polled for the default handle already.
    public bool DefaultFramebufferHandleInitialized { get; set; } = false;

    public int  FramebufferHandle              { get; set; }
    public int  DepthbufferHandle              { get; set; }
    public int  StencilbufferHandle            { get; set; }
    public int  DepthStencilPackedBufferHandle { get; set; }
    public bool HasDepthStencilPackedBuffer    { get; set; }
    public bool HasMultipleTexturesPresent     { get; set; }

    public string ManagedStatus => GetManagedStatus( new StringBuilder() ).ToString();

    protected GLFrameBufferBuilder< GLFrameBuffer< GLTexture > > BufferBuilder { get; set; }

    /// <summary>
    ///     Releases all resources associated with the FrameBuffer.
    /// </summary>
    public void Dispose()
    {
        foreach ( T texture in TextureAttachments )
        {
            DisposeColorTexture( texture );
        }

        if ( HasDepthStencilPackedBuffer )
        {
            Core.Gdx.GL20.GLDeleteRenderbuffer( DepthStencilPackedBufferHandle );
        }
        else
        {
            if ( BufferBuilder.HasDepthRenderBuffer )
            {
                Core.Gdx.GL20.GLDeleteRenderbuffer( DepthbufferHandle );
            }

            if ( BufferBuilder.HasStencilRenderBuffer )
            {
                Core.Gdx.GL20.GLDeleteRenderbuffer( StencilbufferHandle );
            }
        }

        Core.Gdx.GL20.GLDeleteFramebuffer( FramebufferHandle );

        if ( Buffers != null )
        {
            if ( Buffers[ Core.Gdx.App ] != null )
            {
                Buffers[ Core.Gdx.App ]?.Remove( this );
            }
        }
    }

    /// <summary>
    ///     Override this method in a derived class to set up the
    ///     backing texture as you like.
    /// </summary>
    protected virtual T CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
        => throw new GdxRuntimeException( "This method must be overriden by derived class(es)" );

    /// <summary>
    ///     Override this method in a derived class to dispose the
    ///     backing texture as you like.
    /// </summary>
    protected virtual void DisposeColorTexture( T colorTexture ) => throw new GdxRuntimeException( "This method must be overriden by derived class(es)" );

    /// <summary>
    ///     Override this method in a derived class to attach the backing
    ///     texture to the GL framebuffer object.
    /// </summary>
    protected virtual void AttachFrameBufferColorTexture( T texture ) => throw new GdxRuntimeException( "This method must be overriden by derived class(es)" );

    /// <summary>
    ///     Convenience method to return the first Texture attachment present in the fbo.
    /// </summary>
    public T GetColorBufferTexture() => TextureAttachments.First();

    public void Build()
    {
        CheckValidBuilder();

        // iOS uses a different framebuffer handle! (not necessarily 0)
        if ( !DefaultFramebufferHandleInitialized )
        {
            DefaultFramebufferHandleInitialized = true;

            if ( Core.Gdx.App.AppType == IApplication.ApplicationType.IOS )
            {
                IntBuffer intbuf = ByteBuffer.AllocateDirect
                    ( ( 16 * sizeof( int ) ) / 8 ).Order( ByteOrder.NativeOrder ).AsIntBuffer();

                Core.Gdx.GL20.GLGetIntegerv( IGL20.GL_FRAMEBUFFER_BINDING, intbuf );
                DefaultFramebufferHandle = intbuf.Get( 0 );
            }
            else
            {
                DefaultFramebufferHandle = 0;
            }
        }

        FramebufferHandle = Core.Gdx.GL20.GLGenFramebuffer();
        Core.Gdx.GL20.GLBindFramebuffer( IGL20.GL_FRAMEBUFFER, FramebufferHandle );

        var width  = BufferBuilder.Width;
        var height = BufferBuilder.Height;

        if ( BufferBuilder.HasDepthRenderBuffer )
        {
            DepthbufferHandle = Core.Gdx.GL20.GLGenRenderbuffer();
            Core.Gdx.GL20.GLBindRenderbuffer( IGL20.GL_RENDERBUFFER, DepthbufferHandle );

            if ( BufferBuilder.DepthRenderBufferSpec != null )
            {
                Core.Gdx.GL20.GLRenderbufferStorage( IGL20.GL_RENDERBUFFER, BufferBuilder.DepthRenderBufferSpec.InternalFormat, width, height );
            }
        }

        if ( BufferBuilder.HasStencilRenderBuffer )
        {
            StencilbufferHandle = Core.Gdx.GL20.GLGenRenderbuffer();
            Core.Gdx.GL20.GLBindRenderbuffer( IGL20.GL_RENDERBUFFER, StencilbufferHandle );

            if ( BufferBuilder.StencilRenderBufferSpec != null )
            {
                Core.Gdx.GL20.GLRenderbufferStorage( IGL20.GL_RENDERBUFFER, BufferBuilder.StencilRenderBufferSpec.InternalFormat, width, height );
            }
        }

        if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
        {
            DepthStencilPackedBufferHandle = Core.Gdx.GL20.GLGenRenderbuffer();
            Core.Gdx.GL20.GLBindRenderbuffer( IGL20.GL_RENDERBUFFER, DepthStencilPackedBufferHandle );

            if ( BufferBuilder.PackedStencilDepthRenderBufferSpec != null )
            {
                Core.Gdx.GL20.GLRenderbufferStorage( IGL20.GL_RENDERBUFFER,
                                                     BufferBuilder.PackedStencilDepthRenderBufferSpec.InternalFormat,
                                                     width,
                                                     height );
            }
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
                    Core.Gdx.GL20.GLFramebufferTexture2D( IGL20.GL_FRAMEBUFFER,
                                                          IGL20.GL_COLOR_ATTACHMENT0 + colorTextureCounter,
                                                          IGL20.GL_TEXTURE_2D,
                                                          texture.GetTextureObjectHandle(),
                                                          0 );

                    colorTextureCounter++;
                }
                else if ( attachmentSpec.IsDepth )
                {
                    Core.Gdx.GL20.GLFramebufferTexture2D( IGL20.GL_FRAMEBUFFER,
                                                          IGL20.GL_DEPTH_ATTACHMENT,
                                                          IGL20.GL_TEXTURE_2D,
                                                          texture.GetTextureObjectHandle(),
                                                          0 );
                }
                else if ( attachmentSpec.IsStencil )
                {
                    Core.Gdx.GL20.GLFramebufferTexture2D( IGL20.GL_FRAMEBUFFER,
                                                          IGL20.GL_STENCIL_ATTACHMENT,
                                                          IGL20.GL_TEXTURE_2D,
                                                          texture.GetTextureObjectHandle(),
                                                          0 );
                }
            }
        }
        else
        {
            T texture = CreateTexture( BufferBuilder.TextureAttachmentSpecs.First() );
            TextureAttachments.Add( texture );

            Core.Gdx.GL20.GLBindTexture( texture.GLTarget, texture.GetTextureObjectHandle() );
        }

        if ( HasMultipleTexturesPresent )
        {
            IntBuffer buffer = BufferUtils.NewIntBuffer( colorTextureCounter );

            for ( var i = 0; i < colorTextureCounter; i++ )
            {
                buffer.Put( IGL20.GL_COLOR_ATTACHMENT0 + i );
            }

            buffer.Position = 0;

            Core.Gdx.GL30?.GLDrawBuffers( colorTextureCounter, buffer );
        }
        else
        {
            AttachFrameBufferColorTexture( TextureAttachments.First() );
        }

        if ( BufferBuilder.HasDepthRenderBuffer )
        {
            Core.Gdx.GL20.GLFramebufferRenderbuffer( IGL20.GL_FRAMEBUFFER,
                                                     IGL20.GL_DEPTH_ATTACHMENT,
                                                     IGL20.GL_RENDERBUFFER,
                                                     DepthbufferHandle );
        }

        if ( BufferBuilder.HasStencilRenderBuffer )
        {
            Core.Gdx.GL20.GLFramebufferRenderbuffer( IGL20.GL_FRAMEBUFFER, IGL20.GL_STENCIL_ATTACHMENT, IGL20.GL_RENDERBUFFER, StencilbufferHandle );
        }

        if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
        {
            Core.Gdx.GL20.GLFramebufferRenderbuffer( IGL20.GL_FRAMEBUFFER,
                                                     IGL30.GL_DEPTH_STENCIL_ATTACHMENT,
                                                     IGL20.GL_RENDERBUFFER,
                                                     DepthStencilPackedBufferHandle );
        }

        Core.Gdx.GL20.GLBindRenderbuffer( IGL20.GL_RENDERBUFFER, 0 );

        foreach ( T texture in TextureAttachments )
        {
            Core.Gdx.GL20.GLBindTexture( texture.GLTarget, 0 );
        }

        var result = Core.Gdx.GL20.GLCheckFramebufferStatus( IGL20.GL_FRAMEBUFFER );

        if ( ( result == IGL20.GL_FRAMEBUFFER_UNSUPPORTED )
          && BufferBuilder is { HasDepthRenderBuffer: true, HasStencilRenderBuffer: true }
          && ( Core.Gdx.Graphics.SupportsExtension( "GL_OES_packed_depth_stencil" )
            || Core.Gdx.Graphics.SupportsExtension( "GL_EXT_packed_depth_stencil" ) ) )
        {
            if ( BufferBuilder.HasDepthRenderBuffer )
            {
                Core.Gdx.GL20.GLDeleteRenderbuffer( DepthbufferHandle );
                DepthbufferHandle = 0;
            }

            if ( BufferBuilder.HasStencilRenderBuffer )
            {
                Core.Gdx.GL20.GLDeleteRenderbuffer( StencilbufferHandle );

                StencilbufferHandle = 0;
            }

            if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
            {
                Core.Gdx.GL20.GLDeleteRenderbuffer( DepthStencilPackedBufferHandle );
                DepthStencilPackedBufferHandle = 0;
            }

            DepthStencilPackedBufferHandle = Core.Gdx.GL20.GLGenRenderbuffer();
            HasDepthStencilPackedBuffer    = true;

            Core.Gdx.GL20.GLBindRenderbuffer( IGL20.GL_RENDERBUFFER, DepthStencilPackedBufferHandle );
            Core.Gdx.GL20.GLRenderbufferStorage( IGL20.GL_RENDERBUFFER, GL_DEPTH24_STENCIL8_OES, width, height );
            Core.Gdx.GL20.GLBindRenderbuffer( IGL20.GL_RENDERBUFFER, 0 );

            Core.Gdx.GL20.GLFramebufferRenderbuffer(
                IGL20.GL_FRAMEBUFFER,
                IGL20.GL_DEPTH_ATTACHMENT,
                IGL20.GL_RENDERBUFFER,
                DepthStencilPackedBufferHandle
                );

            Core.Gdx.GL20.GLFramebufferRenderbuffer(
                IGL20.GL_FRAMEBUFFER,
                IGL20.GL_STENCIL_ATTACHMENT,
                IGL20.GL_RENDERBUFFER,
                DepthStencilPackedBufferHandle
                );

            result = Core.Gdx.GL20.GLCheckFramebufferStatus( IGL20.GL_FRAMEBUFFER );
        }

        Core.Gdx.GL20.GLBindFramebuffer( IGL20.GL_FRAMEBUFFER, DefaultFramebufferHandle );

        if ( result != IGL20.GL_FRAMEBUFFER_COMPLETE )
        {
            foreach ( T texture in TextureAttachments )
            {
                DisposeColorTexture( texture );
            }

            if ( HasDepthStencilPackedBuffer )
            {
                Core.Gdx.GL20.GLDeleteBuffers( ( uint )DepthStencilPackedBufferHandle );
            }
            else
            {
                if ( BufferBuilder.HasDepthRenderBuffer )
                {
                    Core.Gdx.GL20.GLDeleteRenderbuffer( DepthbufferHandle );
                }

                if ( BufferBuilder.HasStencilRenderBuffer )
                {
                    Core.Gdx.GL20.GLDeleteRenderbuffer( StencilbufferHandle );
                }
            }

            Core.Gdx.GL20.GLDeleteFramebuffer( FramebufferHandle );

            if ( result == IGL20.GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT )
            {
                throw new GdxRuntimeException( "Frame buffer couldn't be constructed: incomplete attachment" );
            }

            if ( result == IGL20.GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS )
            {
                throw new GdxRuntimeException( "Frame buffer couldn't be constructed: incomplete dimensions" );
            }

            if ( result == IGL20.GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT )
            {
                throw new GdxRuntimeException( "Frame buffer couldn't be constructed: missing attachment" );
            }

            if ( result == IGL20.GL_FRAMEBUFFER_UNSUPPORTED )
            {
                throw new GdxRuntimeException
                    ( "Frame buffer couldn't be constructed: unsupported combination of formats" );
            }

            throw new GdxRuntimeException( "Frame buffer couldn't be constructed: unknown error " + result );
        }

        AddManagedFrameBuffer( Core.Gdx.App, this );
    }

    private void CheckValidBuilder()
    {
        var runningGL30 = Core.Gdx.Graphics.IsGL30Available();

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
                    if ( !Core.Gdx.Graphics.SupportsExtension( "OES_texture_float" ) )
                    {
                        throw new GdxRuntimeException( "Float texture FrameBuffer Attachment not available on GLES 2.0" );
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Makes the frame buffer current so everything gets drawn to it.
    /// </summary>
    protected virtual void Bind() => Core.Gdx.GL20.GLBindFramebuffer( IGL20.GL_FRAMEBUFFER, FramebufferHandle );

    /// <summary>
    ///     Unbinds the framebuffer, all drawing will be performed to the
    ///     normal framebuffer from here on.
    /// </summary>
    public void Unbind() => Core.Gdx.GL20.GLBindFramebuffer( IGL20.GL_FRAMEBUFFER, DefaultFramebufferHandle );

    /// <summary>
    ///     Binds the frame buffer and sets the viewport accordingly,
    ///     so everything gets drawn to it.
    /// </summary>
    public virtual void Begin()
    {
        Bind();
        SetFrameBufferViewport();
    }

    /// <summary>
    ///     Sets viewport to the dimensions of framebuffer.
    ///     Called by <see cref="Begin()" />.
    /// </summary>
    public void SetFrameBufferViewport() => Core.Gdx.GL20.GLViewport( 0, 0, BufferBuilder.Width, BufferBuilder.Height );

    /// <summary>
    ///     Unbinds the framebuffer, all drawing will be performed to the
    ///     normal framebuffer from here on.
    /// </summary>
    public virtual void End() => End( 0, 0, Core.Gdx.Graphics.BackBufferWidth, Core.Gdx.Graphics.BackBufferHeight );

    /// <summary>
    ///     Unbinds the framebuffer and sets viewport sizes, all drawing will be
    ///     performed to the normal framebuffer from here on.
    /// </summary>
    /// <param name="x"> the x-axis position of the viewport in pixels </param>
    /// <param name="y"> the y-asis position of the viewport in pixels </param>
    /// <param name="width"> the width of the viewport in pixels </param>
    /// <param name="height"> the height of the viewport in pixels  </param>
    public virtual void End( int x, int y, int width, int height )
    {
        Unbind();
        Core.Gdx.GL20.GLViewport( x, y, width, height );
    }

    /// <summary>
    /// </summary>
    /// <param name="app"></param>
    /// <param name="frameBuffer"></param>
    private void AddManagedFrameBuffer( IApplication app, GLFrameBuffer< T > frameBuffer )
    {
        if ( Buffers == null )
        {
            throw new GdxRuntimeException( "Buffers is NULL!" );
        }

        List< GLFrameBuffer< T > > managedResources = Buffers[ app ] ?? new List< GLFrameBuffer< T > >();

        managedResources.Add( frameBuffer );
        Buffers.Put( app, managedResources );
    }

    /// <summary>
    ///     Invalidates all frame buffers. This can be used when the OpenGL context is
    ///     lost to rebuild all managed frame buffers. This assumes that the texture
    ///     attached to this buffer has already been rebuild! Use with care.
    /// </summary>
    public void InvalidateAllFrameBuffers( IApplication app )
    {
        ArgumentNullException.ThrowIfNull( app );

        if ( Buffers?[ app ] == null )
        {
            return;
        }

        for ( var i = 0; i < Buffers[ app ]?.Count; i++ )
        {
            Buffers[ app ]?[ i ].Build();
        }
    }

    public void ClearAllFrameBuffers( IApplication app ) => Buffers?.Remove( app );

    public StringBuilder GetManagedStatus( in StringBuilder builder )
    {
        builder.Append( "Managed buffers/app: { " );

        if ( Buffers == null )
        {
            builder.Append( "null" );
        }
        else
        {
            if ( Buffers?.Keys != null )
            {
                foreach ( IApplication app in Buffers.Keys )
                {
                    builder.Append( Buffers[ app ]?.Count );
                    builder.Append( ' ' );
                }
            }
        }

        builder.Append( '}' );

        return builder;
    }
}
