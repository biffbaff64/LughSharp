using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.UI;

namespace LibGDXSharp.Assets.Loaders;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class BitmapFontParameter : AssetLoaderParameters
{
    /// <summary>
    /// Flips the font vertically if <tt>true</tt>.
    /// Defaults to <tt>false</tt>.
    /// </summary>
    internal bool Flip { get; set; } = false;

    /// <summary>
    /// Generates mipmaps for the font if <tt>true</tt>.
    /// Defaults to <tt>false</tt>.
    /// </summary>
    internal bool GenMipMaps { get; set; } = false;

    /// <summary>
    /// The <see cref="TextureFilter"/> to use when scaling down the <see cref="BitmapFont"/>.
    /// Defaults to <see cref="TextureFilter.Nearest"/>.
    /// </summary>
    internal TextureFilter MinFilter { get; set; } = TextureFilter.Nearest;

    /// <summary>
    /// The <see cref="TextureFilter"/> to use when scaling up the <see cref="BitmapFont"/>.
    /// Defaults to <see cref="TextureFilter.Nearest"/>.
    /// </summary>
    internal TextureFilter MagFilter { get; set; } = TextureFilter.Nearest;

    /// <summary>
    /// optional <see cref="BitmapFont.BitmapFontData"/> to be used instead of
    /// loading the <see cref="Texture"/> directly. Use this if your font is
    /// embedded in a <see cref="Skin"/>.
    /// </summary>
    internal BitmapFont.BitmapFontData? BitmapFontData { get; set; } = null;

    /// <summary>
    /// The name of the <see cref="TextureAtlas"/> to load the <see cref="BitmapFont"/>.
    /// if null, will look for a separate image.
    /// </summary>
    internal string? AtlasName { get; set; } = null;
}

/// <summary>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class BitmapFontLoader : AsynchronousAssetLoader, IDisposable
{
    private BitmapFont.BitmapFontData? _data;

    /// <summary>
    /// </summary>
    /// <param name="resolver"></param>
    public BitmapFontLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first.
    /// This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             IAssetLoaderParameters parameter )
    {
        if ( file == null ) throw new NullReferenceException();

        var deps = new List< AssetDescriptor >();

        if ( ( ( BitmapFontParameter )parameter ).BitmapFontData != null )
        {
            _data = ( ( BitmapFontParameter )parameter ).BitmapFontData;

            return deps;
        }

        _data = new BitmapFont.BitmapFontData
            (
             file,
             ( ( BitmapFontParameter )parameter! ).Flip
            );

        if ( ( ( BitmapFontParameter )parameter ).AtlasName != null )
        {
            deps.Add
                (
                 new AssetDescriptor
                     (
                      ( ( BitmapFontParameter )parameter ).AtlasName,
                      typeof(TextureAtlas),
                      ( ( BitmapFontParameter )parameter )
                     )
                );
        }
        else
        {
            for ( var i = 0; i < _data.ImagePaths?.Length; i++ )
            {
                var path = _data.ImagePaths[ i ];

                FileInfo resolved = Resolve( path );

                var textureParams = new TextureLoader.TextureParameter();

                if ( parameter != null )
                {
                    textureParams.GenMipMaps = ( ( BitmapFontParameter )parameter ).GenMipMaps;
                    textureParams.MinFilter  = ( ( BitmapFontParameter )parameter ).MinFilter;
                    textureParams.MagFilter  = ( ( BitmapFontParameter )parameter ).MagFilter;
                }

                var descriptor = new AssetDescriptor( resolved, typeof(Texture), textureParams );
                deps.Add( descriptor );
            }
        }

        return deps;
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    IAssetLoaderParameters parameter )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public override BitmapFont LoadSync( AssetManager? manager,
                                         string? fileName,
                                         FileInfo? file,
                                         IAssetLoaderParameters parameter )
    {
        if ( file == null ) throw new GdxRuntimeException( "LoadSync: 'file' cannot be null!" );
        
        if ( ( ( BitmapFontParameter )parameter ).AtlasName != null )
        {
            var atlas = manager?.Get< TextureAtlas >( ( ( BitmapFontParameter )parameter ).AtlasName! );

            var name = Path.GetFileNameWithoutExtension( _data?.ImagePaths?[ 0 ] );

            TextureRegion? region = atlas?.FindRegion( name );

            if ( region == null )
            {
                throw new GdxRuntimeException
                    (
                     $"Could not find font region {name} in atlas "
                     + $"{( ( BitmapFontParameter )parameter ).AtlasName}"
                    );
            }

            return new BitmapFont( file, region );
        }
        else
        {
            var n    = ( int )_data?.ImagePaths?.Length!;
            var regs = new List< TextureRegion >( capacity: n );

            for ( var i = 0; i < n; i++ )
            {
                regs.Add( new TextureRegion( manager?.Get<Texture>( _data.ImagePaths[ i ], typeof(Texture) )! ) );
            }

            return new BitmapFont( _data, regs, true );
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }
}