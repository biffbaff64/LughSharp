using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Assets.Loaders;

/// <summary>
/// AssetLoader to load TextureAtlas instances.
/// Passing a <see cref="TextureAtlasParameter"/> to
/// <see cref="AssetManager.Load(String, Type, IAssetLoaderParameters)"/>
/// allows to specify whether the atlas regions should be flipped on the
/// y-axis or not.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class TextureAtlasLoader : SynchronousAssetLoader< TextureAtlas, TextureAtlasLoader.TextureAtlasParameter >
{
    private TextureAtlasData? _data;

    public TextureAtlasLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="assetManager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override TextureAtlas Load( AssetManager assetManager,
                                       string fileName,
                                       FileInfo? file,
                                       TextureAtlasParameter parameter )
    {
        if ( _data == null )
        {
            throw new GdxRuntimeException( "TextureAtlasData object is NULL!" );
        }
        
        foreach ( TextureAtlasData.Page page in _data.Pages )
        {
            if ( page.textureFile != null )
            {
                var name = page.textureFile.FullName.Replace( "\\\\", "/" );

                var texture = assetManager.Get< Texture >( name );

                page.texture = texture;
            }
        }

        var atlas = new TextureAtlas( _data );
        _data = null;

        return atlas;
    }

    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                    FileInfo? atlasFile,
                                                    IAssetLoaderParameters? parameter )
    {
        if ( atlasFile == null )
        {
            throw new GdxRuntimeException( "Atlas File cannot be null!" );
        }
        
        DirectoryInfo? imgDir = atlasFile.Directory;

        _data = parameter != null
            ? new TextureAtlasData( atlasFile, imgDir, ( (TextureAtlasParameter)parameter ).FlipVertically )
            : new TextureAtlasData( atlasFile, imgDir, false );

        var dependencies = new List< AssetDescriptor >();

        foreach ( TextureAtlasData.Page page in _data.Pages )
        {
            var tparams = new TextureLoader.TextureParameter
            {
                Format     = page.Format,
                GenMipMaps = page.UseMipMaps,
                MinFilter  = page.MinFilter,
                MagFilter  = page.MagFilter
            };

            if ( page.textureFile != null )
            {
                dependencies.Add( new AssetDescriptor( page.textureFile, typeof(Texture), tparams ) );
            }
        }

        return dependencies;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    public class TextureAtlasParameter : AssetLoaderParameters
    {
        public bool FlipVertically { get; private set; }

        public TextureAtlasParameter()
        {
            this.FlipVertically = false;
        }

        public TextureAtlasParameter( bool flip )
        {
            this.FlipVertically = flip;
        }
    }
}