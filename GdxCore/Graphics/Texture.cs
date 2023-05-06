namespace LibGDXSharp.Graphics;

public class Texture : GLTexture
{
    public int Width  { get; set; }
    public int Height { get; set; }

    private AssetManager                                _assetManager;
    private Dictionary< IApplication, List< Texture > > _managedTextures = new();

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