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

using System.Text;

namespace LibGDXSharp.Graphics.GLUtils;

public interface IGLFrameBufferBase
{
    /// <summary>
    /// Makes the frame buffer current so everything gets drawn to it.
    /// </summary>
    void Bind();

    /// <summary>
    /// Sets viewport to the dimensions of framebuffer.
    /// Called by <see cref="GLFrameBuffer{T}.Begin"/>.
    /// </summary>
    void SetFrameBufferViewport();

    /// <summary>
    /// Binds the frame buffer and sets the viewport accordingly,
    /// so everything gets drawn to it.
    /// </summary>
    void Begin();

    /// <summary>
    /// Unbinds the framebuffer, all drawing will be performed to the
    /// normal framebuffer from here on.
    /// </summary>
    void End();

    /// <summary>
    /// Unbinds the framebuffer and sets viewport sizes, all drawing will be
    /// performed to the normal framebuffer from here on.
    /// </summary>
    /// <param name="x"> the x-axis position of the viewport in pixels </param>
    /// <param name="y"> the y-asis position of the viewport in pixels </param>
    /// <param name="width"> the width of the viewport in pixels </param>
    /// <param name="height"> the height of the viewport in pixels  </param>
    void End( int x, int y, int width, int height );

    /// <summary>
    /// Invalidates all frame buffers. This can be used when the OpenGL context is
    /// lost to rebuild all managed frame buffers. This assumes that the texture
    /// attached to this buffer has already been rebuild! Use with care. 
    /// </summary>
    void InvalidateAllFrameBuffers( IApplication app );

    void          ClearAllFrameBuffers( IApplication app );
    StringBuilder GetManagedStatus( in StringBuilder builder );

    void Build();
    
    /// <summary>
    /// Releases all resources associated with the FrameBuffer.
    /// </summary>
    void Dispose();
}