using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public class Gdx2DPixmap
    {
        public const int Gdx2D_Format_Alpha           = 1;
        public const int Gdx2D_Format_Luminance_Alpha = 2;
        public const int Gdx2D_Format_RGB888          = 3;
        public const int Gdx2D_Format_RGBA8888        = 4;
        public const int Gdx2D_Format_RGB565          = 5;
        public const int Gdx2D_Format_RGBA4444        = 6;

        public const int Gdx2D_Scale_Nearest = 0;
        public const int Gdx2D_Scale_Linear  = 1;

        public const int Gdx2D_Blend_None     = 0;
        public const int Gdx2D_Blend_Src_Over = 1;
        
        public Gdx2DPixmap( int width, int height, int gdx2DPixmapFormat )
        {
        }
        
        public Gdx2DPixmap( byte[] encodedData, int offset, int len, int requestedFormat )
        {
        }
    }
}
