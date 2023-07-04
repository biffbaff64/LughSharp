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

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class FrameBufferTextureAttachmentSpec
{
    public int InternalFormat { get; }
    public int Format         { get; }
    public int Type           { get; }

    public bool IsFloat   { get; init; }
    public bool IsGpuOnly { get; init; }
    public bool IsDepth   { get; init; }
    public bool IsStencil { get; init; }

    public FrameBufferTextureAttachmentSpec( int internalformat, int format, int type )
    {
        this.InternalFormat = internalformat;
        this.Format         = format;
        this.Type           = type;
    }

    public bool IsColorTexture => ( !IsDepth && !IsStencil );
}