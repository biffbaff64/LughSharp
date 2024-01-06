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

/// <summary>
///     This is a <see cref="FrameBuffer" /> variant backed by a float texture.
/// </summary>
public class FloatFrameBuffer : FrameBuffer
{
    public FloatFrameBuffer()
    {
    }

    /// <summary>
    ///     Creates a GLFrameBuffer from the specifications provided by bufferBuilder
    /// </summary>
    /// <param name="bufferBuilder"></param>
    public FloatFrameBuffer( GLFrameBufferBuilder< GLFrameBuffer< GLTexture > > bufferBuilder )
        : base( bufferBuilder )
    {
    }

    /// <summary>
    ///     Creates a new FrameBuffer with a float backing texture, having the given dimensions
    ///     and potentially a depth buffer attached.
    /// </summary>
    /// <param name="width"> the width of the framebuffer in pixels </param>
    /// <param name="height"> the height of the framebuffer in pixels </param>
    /// <param name="hasDepth"> whether to attach a depth buffer </param>
    /// <exception cref="GdxRuntimeException"> in case the FrameBuffer could not be created  </exception>
    public FloatFrameBuffer( int width, int height, bool hasDepth )
    {
        var bufferBuilder = new FrameBufferBuilder( width, height );

        bufferBuilder.AddFloatAttachment( IGL30.GL_RGBA32_F, IGL20.GL_RGBA, IGL20.GL_FLOAT, false );

        if ( hasDepth )
        {
            bufferBuilder.AddBasicDepthRenderBuffer();
        }

        BufferBuilder = bufferBuilder;

        Build();
    }

    protected override Texture CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        var data = new FloatTextureData( BufferBuilder.Width,
                                         BufferBuilder.Height,
                                         attachmentSpec.InternalFormat,
                                         attachmentSpec.Format,
                                         attachmentSpec.Type,
                                         attachmentSpec.IsGpuOnly );

        var result = new Texture( data );

        if ( Gdx.App.AppType == IApplication.ApplicationType.Desktop )
        {
            result.SetFilter( TextureFilter.Linear, TextureFilter.Linear );
        }
        else
        {
            // no filtering for float textures in OpenGL ES
            result.SetFilter( TextureFilter.Nearest, TextureFilter.Nearest );
        }

        result.SetWrap( TextureWrap.ClampToEdge, TextureWrap.ClampToEdge );

        return result;
    }
}
