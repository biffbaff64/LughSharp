using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class PixmapTextureData : ITextureData
{
    public Pixmap        Pixmap        { get; set; }
    public Pixmap.Format Format        { get; set; }
    public bool          UseMipMaps    { get; set; }
    public bool          DisposePixmap { get; set; }
    public bool          Managed       { get; set; }

    public PixmapTextureData( Pixmap pixmap,
                              Pixmap.Format? format,
                              bool useMipMaps,
                              bool disposePixmap,
                              bool managed = false )
    {
        this.Pixmap        = pixmap;
        this.Format        = format ?? pixmap.PixFormat;
        this.UseMipMaps    = useMipMaps;
        this.DisposePixmap = disposePixmap;
        this.Managed       = managed;
    }

    public Pixmap ConsumePixmap() => Pixmap;

    public int GetWidth() => Pixmap.Width;

    public int GetHeight() => Pixmap.Height;

    public bool IsPrepared() => true;

    public new ITextureData.TextureDataType GetType()
    {
        return ITextureData.TextureDataType.Pixmap;
    }

    /// <returns>
    /// whether the caller of <see cref="ITextureData.ConsumePixmap"/> should dispose the
    /// Pixmap returned by <see cref="ITextureData.ConsumePixmap"/>
    /// </returns>
    bool ITextureData.DisposePixmap() => DisposePixmap;

    /// <returns> the <see cref="Pixmap.Format"/> of the pixel data </returns>
    public Pixmap.Format GetFormat() => Format;

    /// <returns> whether to generate mipmaps or not. </returns>
    bool ITextureData.UseMipMaps() => UseMipMaps;

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged() => Managed;

    public void ConsumeCustomData( int target )
    {
        throw new GdxRuntimeException( "This TextureData implementation does not upload data itself" );
    }

    public void Prepare()
    {
        throw new GdxRuntimeException
            ( "prepare() must not be called on a PixmapTextureData instance as it is already prepared." );
    }
}
