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

namespace LibGDXSharp.Graphics.GLUtils;

public class FrameBufferCubemap : GLFrameBuffer<Cubemap>
{
    public FrameBufferCubemap( object frameBufferCubemapBuilder )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Override this method in a derived class to set up the
    /// backing texture as you like.
    /// </summary>
    public override Cubemap CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec ) => null;

    /// <summary>
    /// Override this method in a derived class to dispose the
    /// backing texture as you like.
    /// </summary>
    public override void DisposeColorTexture( Cubemap colorTexture )
    {
    }

    /// <summary>
    /// Override this method in a derived class to attach the backing
    /// texture to the GL framebuffer object.
    /// </summary>
    public override void AttachFrameBufferColorTexture( Cubemap texture )
    {
    }
}