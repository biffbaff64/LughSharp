using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class VertexAttributes
{
    public static class Usage
    {
        public const int Position           = 1;
        public const int ColorUnpacked      = 2;
        public const int ColorPacked        = 4;
        public const int Normal             = 8;
        public const int TextureCoordinates = 16;
        public const int Generic            = 32;
        public const int BoneWeight         = 64;
        public const int Tangent            = 128;
        public const int BiNormal           = 256;
    }

}
