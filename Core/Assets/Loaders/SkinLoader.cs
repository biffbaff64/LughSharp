using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.UI;

namespace LibGDXSharp.Assets.Loaders;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class SkinLoader : AsynchronousAssetLoader
{
    public SkinLoader( IFileHandleResolver resolver ) : base( resolver )
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
                                                             IAssetLoaderParameters? parameter )
    {
        List< AssetDescriptor > deps = new();

        if ( ( parameter == null ) || ( ( ( SkinParameter )parameter ).textureAtlasPath == null ) )
        {
            var path = Path.ChangeExtension( file?.FullName, ".atlas" );

            deps.Add( new AssetDescriptor( path, typeof(TextureAtlas), new SkinParameter() ) );
        }
        else if ( ( ( SkinParameter )parameter ).textureAtlasPath != null )
        {
            deps.Add
                (
                 new AssetDescriptor
                     (
                      ( ( SkinParameter )parameter ).textureAtlasPath,
                      typeof(TextureAtlas),
                      parameter
                     )
                );
        }

        return deps;
    }

    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    IAssetLoaderParameters parameter )
    {
    }

    public override object LoadSync( AssetManager? manager,
                                     string? fileName,
                                     FileInfo? file,
                                     IAssetLoaderParameters? parameter )
    {
        var textureAtlasPath = Path.ChangeExtension( file?.FullName, ".atlas" );

        Dictionary< string, object >? resources = null;

        if ( parameter != null )
        {
            if ( ( ( SkinParameter )parameter ).textureAtlasPath != null )
            {
                textureAtlasPath = ( ( SkinParameter )parameter ).textureAtlasPath;
            }

            if ( ( ( SkinParameter )parameter! ).resources != null )
            {
                resources = ( ( SkinParameter )parameter ).resources;
            }
        }

        if ( manager == null )
        {
            throw new GdxRuntimeException( "manager cannot be NULL!" );
        }
        
        var  atlas = manager.Get< TextureAtlas >( textureAtlasPath!, typeof(TextureAtlas) );
        Skin skin  = NewSkin( atlas );

        if ( resources != null )
        {
            foreach ( KeyValuePair< string, object > entry in resources )
            {
                skin.Add( entry.Key, entry.Value );
            }
        }

        skin.Load( file );

        return skin;
    }

    private Skin NewSkin( TextureAtlas atlas )
    {
        return new Skin( atlas );
    }
    
    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    public sealed class SkinParameter : AssetLoaderParameters
    {
        public readonly string?                       textureAtlasPath;
        public readonly Dictionary< string, object >? resources;

        public SkinParameter() : this( null, null )
        {
        }

        public SkinParameter( Dictionary< string, object > resources )
            : this( null, resources )
        {
        }

        public SkinParameter( string? textureAtlasPath, Dictionary< string, object >? resources = null )
        {
            this.textureAtlasPath = textureAtlasPath;
            this.resources        = resources;
        }
    }
}

