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

public interface IBufferBuilderBase<TU> //where TU : GLFrameBuffer< GLTexture >
{
    int Width  { get; }
    int Height { get; }

    List< FrameBufferTextureAttachmentSpec > TextureAttachmentSpecs { get; }

    FrameBufferRenderBufferAttachmentSpec? StencilRenderBufferSpec            { get; set; }
    FrameBufferRenderBufferAttachmentSpec? DepthRenderBufferSpec              { get; set; }
    FrameBufferRenderBufferAttachmentSpec? PackedStencilDepthRenderBufferSpec { get; set; }

    bool HasStencilRenderBuffer            { get; set; }
    bool HasDepthRenderBuffer              { get; set; }
    bool HasPackedStencilDepthRenderBuffer { get; set; }

    GLFrameBufferBuilder<TU> AddColorTextureAttachment( int internalFormat, int format, int type );

    GLFrameBufferBuilder<TU> AddBasicColorTextureAttachment( Pixmap.Format format );

    GLFrameBufferBuilder<TU> AddFloatAttachment( int internalFormat,
                                                 int format,
                                                 int type,
                                                 bool gpuOnly );

    GLFrameBufferBuilder<TU> AddDepthTextureAttachment( int internalFormat, int type );

    GLFrameBufferBuilder<TU> AddStencilTextureAttachment( int internalFormat, int type );

    GLFrameBufferBuilder<TU> AddDepthRenderBuffer( int internalFormat );

    GLFrameBufferBuilder<TU> AddStencilRenderBuffer( int internalFormat );

    GLFrameBufferBuilder<TU> AddStencilDepthPackedRenderBuffer( int internalFormat );

    GLFrameBufferBuilder<TU> AddBasicDepthRenderBuffer();

    GLFrameBufferBuilder<TU> AddBasicStencilRenderBuffer();

    GLFrameBufferBuilder<TU> AddBasicStencilDepthPackedRenderBuffer();
}