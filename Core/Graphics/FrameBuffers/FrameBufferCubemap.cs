// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Graphics.FrameBuffers;

[PublicAPI]
public class FrameBufferCubemap : GLFrameBuffer< Cubemap >
{
    /// <summary>
    /// the zero-based index of the active side 
    /// </summary>
    private int _currentSide;

    /// <summary>
    /// cubemap sides cache
    /// </summary>
    private readonly static Cubemap.CubemapSide[] CubemapSides = Cubemap.CubemapSide.Values();

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
    public FrameBufferCubemap( Pixmap.Format format, int width,
                               int height, bool hasDepth, bool hasStencil = false )
    {
        this.BufferBuilder = new FrameBufferBuilder( width, height );
        
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

        this.BufferBuilder = frameBufferBuilder;

        Build();
    }

    protected override Cubemap CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        GLOnlyTextureData data = new
                (
                BufferBuilder.Width, BufferBuilder.Height, 0,
                attachmentSpec.InternalFormat,
                attachmentSpec.Format,
                attachmentSpec.Type
                );

        Cubemap result = new( data, data, data, data, data, data );

        result.SetFilter( TextureFilter.Linear, TextureFilter.Linear );
        result.SetWrap( TextureWrap.ClampToEdge, TextureWrap.ClampToEdge );

        return result;
    }

    protected override void DisposeColorTexture( Cubemap colorTexture )
    {
        colorTexture.Dispose();
    }

    protected override void AttachFrameBufferColorTexture( Cubemap texture )
    {
        var glHandle = texture.GetTextureObjectHandle();

        Cubemap.CubemapSide[] sides = Cubemap.CubemapSide.Values();

        foreach ( Cubemap.CubemapSide side in sides )
        {
            Gdx.GL20.GLFramebufferTexture2D
                (
                IGL20.GL_FRAMEBUFFER,
                IGL20.GL_COLOR_ATTACHMENT0,
                side.GLEnum,
                glHandle, 0
                );
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
    /// Should be called in between a call to <see cref="GLFrameBuffer{T}.Begin"/>
    /// and <see cref="GLFrameBuffer{T}.End()"/> to cycle to each side of the
    /// cubemap to render on.
    public bool NextSide()
    {
        if ( _currentSide > 5 )
        {
            throw new GdxRuntimeException( "No remaining sides." );
        }
        else if ( _currentSide == 5 )
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
        
        Gdx.GL20.GLFramebufferTexture2D
                (
                IGL20.GL_FRAMEBUFFER, IGL20.GL_COLOR_ATTACHMENT0, side.GLEnum,
                GetColorBufferTexture().GetTextureObjectHandle(), 0
                );
    }

    /// <summary>
    /// Get the currently bound side.
    /// </summary>
    public Cubemap.CubemapSide? GetSide()
    {
        return _currentSide < 0 ? null : CubemapSides[ _currentSide ];
    }
}