namespace LibGDXSharp.Graphics
{
    public class Texture
    {
        public Texture( ITextureData? potPixmap )
        {
            throw new NotImplementedException();
        }

        public int Width  { get; set; }
        public int Height { get; set; }

        public void SetFilter( TextureFilter minFilter, TextureFilter magFilter )
        {
            throw new NotImplementedException();
        }

        public void SetWrap( TextureWrap wrapU, TextureWrap wrapV )
        {
            throw new NotImplementedException();
        }

        public void Load( ITextureData? loaderInfoData )
        {
            throw new NotImplementedException();
        }
    }
}
