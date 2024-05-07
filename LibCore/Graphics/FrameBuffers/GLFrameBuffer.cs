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
using LughSharp.LibCore.Utils.Collections.Extensions;
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Graphics.FrameBuffers;

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
public class GLFrameBuffer< T > : IDisposable where T : GLTexture
{
    public const int GL_DEPTH24_STENCIL8_OES = 0x88F0;

    // ------------------------------------------------------------------------

    public int  FramebufferHandle              { get; set; }
    public int  DepthbufferHandle              { get; set; }
    public int  StencilbufferHandle            { get; set; }
    public int  DepthStencilPackedBufferHandle { get; set; }
    public bool HasDepthStencilPackedBuffer    { get; set; }
    public bool HasMultipleTexturesPresent     { get; set; }

    // the frame buffers
    public Dictionary< IApplication, List< GLFrameBuffer< T > >? >? Buffers { get; set; } = new();

    // the color buffer texture
    public List< T > TextureAttachments { get; set; } = new();

    // the default framebuffer handle, a.k.a screen.
    public int DefaultFramebufferHandle { get; set; }

    // true if we have polled for the default handle already.
    public bool DefaultFramebufferHandleInitialized { get; set; }

    protected GLFrameBufferBuilder< GLFrameBuffer< GLTexture > > BufferBuilder { get; set; }

    // ------------------------------------------------------------------------

    protected GLFrameBuffer()
    {
        BufferBuilder = null!;
    }

    /// <summary>
    ///     Creates a GLFrameBuffer from the specifications provided
    ///     by bufferBuilder.
    /// </summary>
    protected GLFrameBuffer( GLFrameBufferBuilder< GLFrameBuffer< GLTexture > > bufferBuilder )
    {
        BufferBuilder = bufferBuilder;

        Build();
    }

    public string ManagedStatus => GetManagedStatus( new StringBuilder() ).ToString();

    /// <summary>
    ///     Override this method in a derived class to set up the
    ///     backing texture as you like.
    /// </summary>
    protected virtual T CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        throw new GdxRuntimeException( "This method must be overriden by derived class(es)" );
    }

    /// <summary>
    ///     Override this method in a derived class to dispose the
    ///     backing texture as you like.
    /// </summary>
    protected virtual void DisposeColorTexture( T colorTexture )
    {
        throw new GdxRuntimeException( "This method must be overriden by derived class(es)" );
    }

    /// <summary>
    ///     Override this method in a derived class to attach the backing
    ///     texture to the GL framebuffer object.
    /// </summary>
    protected virtual void AttachFrameBufferColorTexture( T texture )
    {
        throw new GdxRuntimeException( "This method must be overriden by derived class(es)" );
    }

    /// <summary>
    ///     Convenience method to return the first Texture attachment present in the fbo.
    /// </summary>
    public T GetColorBufferTexture()
    {
        return TextureAttachments.First();
    }

    public void Build()
    {
        CheckValidBuilder();

        if ( !DefaultFramebufferHandleInitialized )
        {
            DefaultFramebufferHandleInitialized = true;

            if ( Gdx.App.AppType == IApplication.ApplicationType.IOS )
            {
                var intbuf = ByteBuffer.AllocateDirect
                    ( ( 16 * sizeof( int ) ) / 8 ).Order( ByteOrder.NativeOrder ).AsIntBuffer();

                unsafe
                {
                    fixed ( int* ptr = &intbuf.BackingArray()[ 0 ] )
                    {
                        Gdx.GL.glGetIntegerv( IGL.GL_FRAMEBUFFER_BINDING, ptr );
                    }
                }

                DefaultFramebufferHandle = intbuf.Get( 0 );
            }
            else
            {
                DefaultFramebufferHandle = 0;
            }
        }

        FramebufferHandle = ( int ) Gdx.GL.glGenFramebuffer();

        Gdx.GL.glBindFramebuffer( IGL.GL_FRAMEBUFFER, ( uint ) FramebufferHandle );

        var width  = BufferBuilder.Width;
        var height = BufferBuilder.Height;

        if ( BufferBuilder.HasDepthRenderBuffer )
        {
            //TODO: uint -> int -> uint ????
            DepthbufferHandle = ( int ) Gdx.GL.glGenRenderbuffer();
            Gdx.GL.glBindRenderbuffer( IGL.GL_RENDERBUFFER, ( uint ) DepthbufferHandle );

            if ( BufferBuilder.DepthRenderBufferSpec != null )
            {
                Gdx.GL.glRenderbufferStorage( IGL.GL_RENDERBUFFER,
                                              BufferBuilder.DepthRenderBufferSpec.InternalFormat,
                                              width,
                                              height );
            }
        }

        if ( BufferBuilder.HasStencilRenderBuffer )
        {
            StencilbufferHandle = ( int ) Gdx.GL.glGenRenderbuffer();
            Gdx.GL.glBindRenderbuffer( IGL.GL_RENDERBUFFER, ( uint ) StencilbufferHandle );

            if ( BufferBuilder.StencilRenderBufferSpec != null )
            {
                Gdx.GL.glRenderbufferStorage( IGL.GL_RENDERBUFFER,
                                              BufferBuilder.StencilRenderBufferSpec.InternalFormat,
                                              width,
                                              height );
            }
        }

        if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
        {
            DepthStencilPackedBufferHandle = ( int ) Gdx.GL.glGenRenderbuffer();
            Gdx.GL.glBindRenderbuffer( IGL.GL_RENDERBUFFER, ( uint ) DepthStencilPackedBufferHandle );

            if ( BufferBuilder.PackedStencilDepthRenderBufferSpec != null )
            {
                Gdx.GL.glRenderbufferStorage( IGL.GL_RENDERBUFFER,
                                              BufferBuilder.PackedStencilDepthRenderBufferSpec.InternalFormat,
                                              width,
                                              height );
            }
        }

        HasMultipleTexturesPresent = BufferBuilder.TextureAttachmentSpecs.Count > 1;

        var colorTextureCounter = 0;

        if ( HasMultipleTexturesPresent )
        {
            foreach ( var attachmentSpec in BufferBuilder.TextureAttachmentSpecs )
            {
                var texture = CreateTexture( attachmentSpec );
                TextureAttachments.Add( texture );

                var tempHandle = ( uint ) texture.GLTextureHandle;

                if ( attachmentSpec.IsColorTexture )
                {
                    Gdx.GL.glFramebufferTexture2D( IGL.GL_FRAMEBUFFER,
                                                   IGL.GL_COLOR_ATTACHMENT0 + colorTextureCounter,
                                                   IGL.GL_TEXTURE_2D,
                                                   tempHandle,
                                                   0 );

                    colorTextureCounter++;
                }
                else if ( attachmentSpec.IsDepth )
                {
                    Gdx.GL.glFramebufferTexture2D( IGL.GL_FRAMEBUFFER,
                                                   IGL.GL_DEPTH_ATTACHMENT,
                                                   IGL.GL_TEXTURE_2D,
                                                   tempHandle,
                                                   0 );
                }
                else if ( attachmentSpec.IsStencil )
                {
                    Gdx.GL.glFramebufferTexture2D( IGL.GL_FRAMEBUFFER,
                                                   IGL.GL_STENCIL_ATTACHMENT,
                                                   IGL.GL_TEXTURE_2D,
                                                   tempHandle,
                                                   0 );
                }
            }
        }
        else
        {
            var texture = CreateTexture( BufferBuilder.TextureAttachmentSpecs.First() );
            TextureAttachments.Add( texture );

            Gdx.GL.glBindTexture( texture.GLTarget, ( uint ) texture.GLTextureHandle );
        }

        if ( HasMultipleTexturesPresent )
        {
            var buffer = BufferUtils.NewIntBuffer( colorTextureCounter );

            for ( var i = 0; i < colorTextureCounter; i++ )
            {
                buffer.Put( IGL.GL_COLOR_ATTACHMENT0 + i );
            }

            buffer.Position = 0;

            unsafe
            {
                fixed ( int* ptr = &buffer.BackingArray()[ 0 ] )
                {
                    Gdx.GL.glDrawBuffers( colorTextureCounter, ptr );
                }
            }
        }
        else
        {
            AttachFrameBufferColorTexture( TextureAttachments.First() );
        }

        if ( BufferBuilder.HasDepthRenderBuffer )
        {
            Gdx.GL.glFramebufferRenderbuffer( IGL.GL_FRAMEBUFFER,
                                              IGL.GL_DEPTH_ATTACHMENT,
                                              IGL.GL_RENDERBUFFER,
                                              ( uint ) DepthbufferHandle );
        }

        if ( BufferBuilder.HasStencilRenderBuffer )
        {
            Gdx.GL.glFramebufferRenderbuffer( IGL.GL_FRAMEBUFFER,
                                              IGL.GL_STENCIL_ATTACHMENT,
                                              IGL.GL_RENDERBUFFER,
                                              ( uint ) StencilbufferHandle );
        }

        if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
        {
            Gdx.GL.glFramebufferRenderbuffer( IGL.GL_FRAMEBUFFER,
                                              IGL.GL_DEPTH_STENCIL_ATTACHMENT,
                                              IGL.GL_RENDERBUFFER,
                                              ( uint ) DepthStencilPackedBufferHandle );
        }

        Gdx.GL.glBindRenderbuffer( IGL.GL_RENDERBUFFER, 0 );

        foreach ( var texture in TextureAttachments )
        {
            Gdx.GL.glBindTexture( texture.GLTarget, 0 );
        }

        var result = Gdx.GL.glCheckFramebufferStatus( IGL.GL_FRAMEBUFFER );

        if ( ( result == IGL.GL_FRAMEBUFFER_UNSUPPORTED )
          && BufferBuilder is { HasDepthRenderBuffer: true, HasStencilRenderBuffer: true }
          && ( Gdx.Graphics.SupportsExtension( "GL_OES_packed_depth_stencil" )
            || Gdx.Graphics.SupportsExtension( "GL_EXT_packed_depth_stencil" ) ) )
        {
            if ( BufferBuilder.HasDepthRenderBuffer )
            {
                Gdx.GL.glDeleteRenderbuffers( ( uint ) DepthbufferHandle );
                DepthbufferHandle = 0;
            }

            if ( BufferBuilder.HasStencilRenderBuffer )
            {
                Gdx.GL.glDeleteRenderbuffers( ( uint ) StencilbufferHandle );
                StencilbufferHandle = 0;
            }

            if ( BufferBuilder.HasPackedStencilDepthRenderBuffer )
            {
                Gdx.GL.glDeleteRenderbuffers( ( uint ) DepthStencilPackedBufferHandle );
                DepthStencilPackedBufferHandle = 0;
            }

            DepthStencilPackedBufferHandle = ( int ) Gdx.GL.glGenRenderbuffer();
            HasDepthStencilPackedBuffer    = true;

            Gdx.GL.glBindRenderbuffer( IGL.GL_RENDERBUFFER, ( uint ) DepthStencilPackedBufferHandle );
            Gdx.GL.glRenderbufferStorage( IGL.GL_RENDERBUFFER, GL_DEPTH24_STENCIL8_OES, width, height );
            Gdx.GL.glBindRenderbuffer( IGL.GL_RENDERBUFFER, 0 );

            Gdx.GL.glFramebufferRenderbuffer( IGL.GL_FRAMEBUFFER,
                                              IGL.GL_DEPTH_ATTACHMENT,
                                              IGL.GL_RENDERBUFFER,
                                              ( uint ) DepthStencilPackedBufferHandle );

            Gdx.GL.glFramebufferRenderbuffer( IGL.GL_FRAMEBUFFER,
                                              IGL.GL_STENCIL_ATTACHMENT,
                                              IGL.GL_RENDERBUFFER,
                                              ( uint ) DepthStencilPackedBufferHandle );

            result = Gdx.GL.glCheckFramebufferStatus( IGL.GL_FRAMEBUFFER );
        }

        Gdx.GL.glBindFramebuffer( IGL.GL_FRAMEBUFFER, ( uint ) DefaultFramebufferHandle );

        if ( result != IGL.GL_FRAMEBUFFER_COMPLETE )
        {
            foreach ( var texture in TextureAttachments )
            {
                DisposeColorTexture( texture );
            }

            if ( HasDepthStencilPackedBuffer )
            {
                Gdx.GL.glDeleteBuffers( ( uint ) DepthStencilPackedBufferHandle );
            }
            else
            {
                if ( BufferBuilder.HasDepthRenderBuffer )
                {
                    Gdx.GL.glDeleteRenderbuffers( ( uint ) DepthbufferHandle );
                }

                if ( BufferBuilder.HasStencilRenderBuffer )
                {
                    Gdx.GL.glDeleteRenderbuffers( ( uint ) StencilbufferHandle );
                }
            }

            Gdx.GL.glDeleteFramebuffers( ( uint ) FramebufferHandle );

            if ( result == IGL.GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT )
            {
                throw new GdxRuntimeException( "Frame buffer couldn't be constructed: incomplete attachment" );
            }

            if ( result == IGL.GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS )
            {
                throw new GdxRuntimeException( "Frame buffer couldn't be constructed: incomplete dimensions" );
            }

            if ( result == IGL.GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT )
            {
                throw new GdxRuntimeException( "Frame buffer couldn't be constructed: missing attachment" );
            }

            if ( result == IGL.GL_FRAMEBUFFER_UNSUPPORTED )
            {
                throw new GdxRuntimeException
                    ( "Frame buffer couldn't be constructed: unsupported combination of formats" );
            }

            throw new GdxRuntimeException( "Frame buffer couldn't be constructed: unknown error " + result );
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
                throw new GdxRuntimeException
                    ( "Packed Stencil/Render render buffers are not available on GLES 2.0" );
            }

            if ( BufferBuilder.TextureAttachmentSpecs.Count > 1 )
            {
                throw new GdxRuntimeException
                    ( "Multiple render targets not available on GLES 2.0" );
            }

            foreach ( var spec in BufferBuilder.TextureAttachmentSpecs )
            {
                if ( spec.IsDepth )
                {
                    throw new GdxRuntimeException
                        ( "Depth texture FrameBuffer Attachment not available on GLES 2.0" );
                }

                if ( spec.IsStencil )
                {
                    throw new GdxRuntimeException
                        ( "Stencil texture FrameBuffer Attachment not available on GLES 2.0" );
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
    ///     Makes the frame buffer current so everything gets drawn to it.
    /// </summary>
    protected virtual void Bind()
    {
        Gdx.GL.glBindFramebuffer( IGL.GL_FRAMEBUFFER, ( uint ) FramebufferHandle );
    }

    /// <summary>
    ///     Unbinds the framebuffer, all drawing will be performed to the
    ///     normal framebuffer from here on.
    /// </summary>
    public void Unbind()
    {
        Gdx.GL.glBindFramebuffer( IGL.GL_FRAMEBUFFER, ( uint ) DefaultFramebufferHandle );
    }

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
    public void SetFrameBufferViewport()
    {
        Gdx.GL.glViewport( 0, 0, BufferBuilder.Width, BufferBuilder.Height );
    }

    /// <summary>
    ///     Unbinds the framebuffer, all drawing will be performed to the
    ///     normal framebuffer from here on.
    /// </summary>
    public virtual void End()
    {
        End( 0, 0, Gdx.Graphics.BackBufferWidth, Gdx.Graphics.BackBufferHeight );
    }

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
        Gdx.GL.glViewport( x, y, width, height );
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

    public void ClearAllFrameBuffers( IApplication app )
    {
        Buffers?.Remove( app );
    }

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
                foreach ( var app in Buffers.Keys )
                {
                    builder.Append( Buffers[ app ]?.Count );
                    builder.Append( ' ' );
                }
            }
        }

        builder.Append( '}' );

        return builder;
    }

    /// <summary>
    ///     Releases all resources associated with the FrameBuffer.
    /// </summary>
    public void Dispose()
    {
        foreach ( var texture in TextureAttachments )
        {
            DisposeColorTexture( texture );
        }

        if ( HasDepthStencilPackedBuffer )
        {
            Gdx.GL.glDeleteRenderbuffers( ( uint ) DepthStencilPackedBufferHandle );
        }
        else
        {
            if ( BufferBuilder.HasDepthRenderBuffer )
            {
                Gdx.GL.glDeleteRenderbuffers( ( uint ) DepthbufferHandle );
            }

            if ( BufferBuilder.HasStencilRenderBuffer )
            {
                Gdx.GL.glDeleteRenderbuffers( ( uint ) StencilbufferHandle );
            }
        }

        Gdx.GL.glDeleteFramebuffers( ( uint ) FramebufferHandle );

        if ( Buffers?[ Gdx.App ] != null )
        {
            Buffers[ Gdx.App ]?.Remove( this );
        }
    }
}