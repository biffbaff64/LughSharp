// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Graphics.GLUtils;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Graphics.FrameBuffers;

[PublicAPI]
public class FrameBufferCubemap : GLFrameBuffer< Cubemap >
{
    /// <summary>
    /// cubemap sides cache
    /// </summary>
    private static readonly Cubemap.CubemapSide[] _cubemapSides = Cubemap.CubemapSide.Values();

    /// <summary>
    /// the zero-based index of the active side
    /// </summary>
    private int _currentSide;

    public FrameBufferCubemap()
    {
    }

    /// <summary>
    /// Creates a GLFrameBuffer from the specifications provided by bufferBuilder
    /// </summary>
    /// <param name="bufferBuilder"></param>
    public FrameBufferCubemap( FrameBufferCubemapBuilder bufferBuilder )
        : base( bufferBuilder )
    {
    }

    /// <summary>
    /// Creates a new FrameBuffer having the given dimensions and potentially
    /// a depth and a stencil buffer attached.
    /// </summary>
    /// <param name="format">
    /// the format of the color buffer; according to the OpenGL ES 2.0 spec,
    /// only RGB565, RGBA4444 and RGB5_A1 are color-renderable
    /// </param>
    /// <param name="width"> the width of the cubemap in pixels </param>
    /// <param name="height"> the height of the cubemap in pixels </param>
    /// <param name="hasDepth"> whether to attach a depth buffer </param>
    /// <param name="hasStencil"> whether to attach a stencil buffer </param>
    /// <exception cref="GdxRuntimeException">
    /// Thrown if the FrameBuffer could not be created
    /// </exception>
    public FrameBufferCubemap( Pixmap.ColorFormat format,
                               int width,
                               int height,
                               bool hasDepth,
                               bool hasStencil = false )
    {
        BufferBuilder = new FrameBufferBuilder( width, height );

        FrameBufferCubemapBuilder frameBufferBuilder = new( width, height );

        frameBufferBuilder.AddBasicColorTextureAttachment( format );

        if ( hasDepth )
        {
            frameBufferBuilder.AddBasicDepthRenderBuffer();
        }

        if ( hasStencil )
        {
            frameBufferBuilder.AddBasicStencilRenderBuffer();
        }

        BufferBuilder = frameBufferBuilder;

        BuildBuffer();
    }

    protected override Cubemap CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        GLOnlyTextureData data = new( BufferBuilder.Width,
                                      BufferBuilder.Height,
                                      0,
                                      attachmentSpec.InternalFormat,
                                      attachmentSpec.Format,
                                      attachmentSpec.Type );

        Cubemap result = new( data, data, data, data, data, data );

        result.SetFilter( Texture.TextureFilter.Linear, Texture.TextureFilter.Linear );
        result.SetWrap( Texture.TextureWrap.ClampToEdge, Texture.TextureWrap.ClampToEdge );

        return result;
    }

    protected override void DisposeColorTexture( Cubemap colorTexture )
    {
        colorTexture.Dispose();
    }

    protected override void AttachFrameBufferColorTexture( Cubemap texture )
    {
        var glHandle = ( uint ) texture.GLTextureHandle;

        Cubemap.CubemapSide[] sides = Cubemap.CubemapSide.Values();

        foreach ( var side in sides )
        {
            GdxApi.Bindings.FramebufferTexture2D( IGL.GL_FRAMEBUFFER,
                                           IGL.GL_COLOR_ATTACHMENT0,
                                           side.GLTarget,
                                           glHandle,
                                           0 );
        }
    }

    /// <summary>
    /// Makes the frame buffer current so everything gets drawn to it,
    /// must be followed by call to either <see cref="NextSide()"/> or
    /// <see cref="BindSide(Cubemap.CubemapSide)"/> to activate the side
    /// to render onto.
    /// </summary>
    protected override void Bind()
    {
        _currentSide = -1;

        base.Bind();
    }

    /// Bind the next side of cubemap and return false if no more side.
    /// Should be called in between a call to
    /// <see cref="GLFrameBuffer{T}.Begin"/>
    /// and
    /// <see cref="GLFrameBuffer{T}.End()"/>
    /// to cycle to each side of the
    /// cubemap to render on.
    public bool NextSide()
    {
        if ( _currentSide > 5 )
        {
            throw new GdxRuntimeException( "No remaining sides." );
        }

        if ( _currentSide == 5 )
        {
            return false;
        }

        _currentSide++;

        BindSide( GetSide() );

        return true;
    }

    /// <summary>
    /// Bind the side, making it active to render on. Should be called
    /// in between a call to <see cref="GLFrameBuffer{T}.Begin()"/> and
    /// <see cref="GLFrameBuffer{T}.End()"/>.
    /// </summary>
    /// <param name="side"> The side to bind </param>
    protected void BindSide( Cubemap.CubemapSide? side )
    {
        ArgumentNullException.ThrowIfNull( side );

        GdxApi.Bindings.FramebufferTexture2D( IGL.GL_FRAMEBUFFER,
                                       IGL.GL_COLOR_ATTACHMENT0,
                                       side.GLTarget,
                                       ( uint ) GetColorBufferTexture().GLTextureHandle,
                                       0 );
    }

    /// <summary>
    /// Get the currently bound side.
    /// </summary>
    public Cubemap.CubemapSide? GetSide()
    {
        return _currentSide < 0 ? null : _cubemapSides[ _currentSide ];
    }
}
