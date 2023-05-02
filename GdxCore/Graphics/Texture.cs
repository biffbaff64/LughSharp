namespace LibGDXSharp.Graphics
{
    public class Texture
    {
        public int Width  { get; set; }
        public int Height { get; set; }

        public Texture( ITextureData? potPixmap )
        {
        }

        public Texture( Pixmap? potPixmap )
        {
        }

        public Texture( FileInfo imageFile, bool b )
        {
        }

        public Texture( FileInfo? pageTextureFile, Pixmap.Format pageFormat, bool pageUseMipMaps )
        {
        }

        public void SetFilter( TextureFilter minFilter, TextureFilter magFilter )
        {
        }

        public void SetWrap( TextureWrap wrapU, TextureWrap wrapV )
        {
        }

        public void Load( ITextureData? loaderInfoData )
        {
        }

        public void Dispose()
        {
        }
    }
}
