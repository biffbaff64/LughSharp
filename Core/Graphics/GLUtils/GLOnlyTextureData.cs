using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// A <see cref="ITextureData"/> implementation which should be used to create
/// gl only textures. This TextureData fits perfectly for <see cref="FrameBuffer"/>s.
/// The data is not managed.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class GLOnlyTextureData : ITextureData
{
    public int  Width          { get; set; } = 0;
    public int  Height         { get; set; } = 0;
    public bool IsPrepared     { get; set; } = false;
    public int  MipLevel       { get; set; } = 0;
    public int  InternalFormat { get; set; }
    public int  Format         { get; set; }
    public int  Type           { get; set; }

    public GLOnlyTextureData( int width,
                              int height,
                              int mipMapLevel,
                              int internalFormat,
                              int format,
                              int type )
    {
        this.Width          = width;
        this.Height         = height;
        this.MipLevel       = mipMapLevel;
        this.InternalFormat = internalFormat;
        this.Format         = format;
        this.Type           = type;
    }

    public new ITextureData.TextureDataType GetType()
    {
        return ITextureData.TextureDataType.Custom;
    }

    public void Prepare()
    {
        if ( IsPrepared )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }
        
        IsPrepared = true;
    }

    public void ConsumeCustomData( int target )
    {
        Gdx.GL.GLTexImage2D( target, MipLevel, InternalFormat, Width, Height, 0, Format, Type, null! );
    }

    public Pixmap ConsumePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    public bool DisposePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    public int GetWidth()
    {
        return Width;
    }

    public int GetHeight()
    {
        return Height;
    }

    public Pixmap.Format GetFormat()
    {
        return Pixmap.Format.RGBA8888;
    }

    public bool UseMipMaps()
    {
        return false;
    }

    public bool IsManaged()
    {
        return false;
    }
}