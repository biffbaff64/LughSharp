// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Graphics.G2D;

using XmlReader = LibGDXSharp.Utils.Xml.XmlReader;

namespace LibGDXSharp.Maps.Tiled;

/// <summary>
///     A TiledMap Loader which loads tiles from a TextureAtlas instead of separate images.
///     It requires a map-level property called 'atlas' with its value being the relative
///     path to the TextureAtlas.
///     <p>
///         The atlas must have in it indexed regions named after the tilesets used in the map.
///         The indexes shall be local to the tileset (not the global id). Strip whitespace and
///         rotation should not be used when creating the atlas.
///     </p>
/// </summary>
public class AtlasTmxMapLoader : BaseTmxMapLoader< AtlasTmxMapLoader.AtlasTiledMapLoaderParameters >
{
    protected readonly List< Texture > trackedTextures = new();
    protected          IAtlasResolver? atlasResolver;

    public AtlasTmxMapLoader() : base( new InternalFileHandleResolver() )
    {
    }

    public AtlasTmxMapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    public TiledMap Load( string fileName ) => Load( fileName, new AtlasTiledMapLoaderParameters() );

    public TiledMap Load( string fileName, AtlasTiledMapLoaderParameters parameter )
    {
        FileInfo tmxFile = Resolve( fileName );

        root = xml?.Parse( tmxFile );

        FileInfo? atlasFileHandle = GetAtlasFileHandle( tmxFile );

        var atlas = new TextureAtlas( atlasFileHandle! );

        atlasResolver = new IAtlasResolver.DirectAtlasResolver( atlas );

        TiledMap map = LoadTiledMap( tmxFile, parameter, atlasResolver );
        map.OwnedResources = new List< object >( new[] { atlas } );

        SetTextureFilters( parameter.TextureMinFilter, parameter.TextureMagFilter );

        return map;
    }

    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? tmxFile,
                                    AssetLoaderParameters parameter )
    {
        ArgumentNullException.ThrowIfNull( manager );
        ArgumentNullException.ThrowIfNull( tmxFile );

        FileInfo? atlasHandle = GetAtlasFileHandle( tmxFile );
        atlasResolver = new IAtlasResolver.AssetManagerAtlasResolver( manager, atlasHandle!.Name );

        Map = LoadTiledMap( tmxFile, ( AtlasTiledMapLoaderParameters )parameter, atlasResolver );
    }

    public override TiledMap LoadSync( AssetManager? manager,
                                       string? fileName,
                                       FileInfo? file,
                                       AssetLoaderParameters? parameter )
    {
        if ( parameter != null )
        {
            SetTextureFilters( ( ( AtlasTiledMapLoaderParameters )parameter ).TextureMinFilter,
                               ( ( AtlasTiledMapLoaderParameters )parameter ).TextureMagFilter );
        }

        return Map;
    }

    protected new List< AssetDescriptor > GetDependencyAssetDescriptors( FileInfo tmxFile,
                                                                         TextureLoader.TextureParameter textureParameter )
    {
        var descriptors = new List< AssetDescriptor >();

        // Atlas dependencies
        FileInfo? atlasFileHandle = GetAtlasFileHandle( tmxFile );

        if ( atlasFileHandle != null )
        {
            descriptors.Add( new AssetDescriptor( atlasFileHandle, typeof( TextureAtlas ), textureParameter ) );
        }

        return descriptors;
    }

    protected override void AddStaticTiles( FileInfo tmxFile,
                                            IImageResolver imageResolver,
                                            TiledMapTileSet tileSet,
                                            XmlReader.Element element,
                                            List< XmlReader.Element > tileElements,
                                            string? name,
                                            int firstgid,
                                            int tilewidth,
                                            int tileheight,
                                            int spacing,
                                            int margin,
                                            string? source,
                                            int offsetX,
                                            int offsetY,
                                            string imageSource,
                                            int imageWidth,
                                            int imageHeight,
                                            FileInfo? image )
    {
        TextureAtlas atlas       = atlasResolver!.GetAtlas();
        var          regionsName = name;

        foreach ( Texture texture in atlas.Textures )
        {
            trackedTextures.Add( texture );
        }

        MapProperties props = tileSet.Properties;

        props.Put( "imagesource", imageSource );
        props.Put( "imagewidth", imageWidth );
        props.Put( "imageheight", imageHeight );
        props.Put( "tilewidth", tilewidth );
        props.Put( "tileheight", tileheight );
        props.Put( "margin", margin );
        props.Put( "spacing", spacing );

        if ( imageSource is { Length: > 0 } )
        {
            var lastgid = ( firstgid + ( ( imageWidth / tilewidth ) * ( imageHeight / tileheight ) ) ) - 1;

            foreach ( AtlasRegion? region in atlas.FindRegions( regionsName! ) )
            {
                // Handle unused tileIds
                if ( region != null )
                {
                    var tileId = firstgid + region.Index;

                    if ( ( tileId >= firstgid ) && ( tileId <= lastgid ) )
                    {
                        AddStaticTiledMapTile( tileSet, region, tileId, offsetX, offsetY );
                    }
                }
            }
        }

        // Add tiles with individual image sources
        foreach ( XmlReader.Element tileElement in tileElements )
        {
            var            tileId = firstgid + tileElement.GetIntAttribute( "id", 0 );
            ITiledMapTile? tile   = tileSet.GetTile( tileId );

            if ( tile == null )
            {
                XmlReader.Element? imageElement = tileElement.GetChildByName( "image" );

                if ( imageElement != null )
                {
                    var regionName = imageElement.GetAttribute( "source" );

                    regionName = regionName.Substring( 0, regionName.LastIndexOf( '.' ) );

                    AtlasRegion? region = atlas.FindRegion( regionName );

                    if ( region == null )
                    {
                        throw new GdxRuntimeException( $"Tileset atlasRegion not found: {regionName}" );
                    }

                    AddStaticTiledMapTile( tileSet, region, tileId, offsetX, offsetY );
                }
            }
        }
    }

    protected FileInfo? GetAtlasFileHandle( FileInfo tmxFile )
    {
        XmlReader.Element? properties = root?.GetChildByName( "properties" );

        string? atlasFilePath = null;

        if ( properties != null )
        {
            foreach ( XmlReader.Element property in properties.GetChildrenByName( "property" ) )
            {
                var name = property.GetAttribute( "name" );

                if ( name.StartsWith( "atlas" ) )
                {
                    atlasFilePath = property.GetAttribute( "value" );

                    break;
                }
            }
        }

        if ( atlasFilePath == null )
        {
            throw new GdxRuntimeException( "The map is missing the 'atlas' property" );
        }

        FileInfo? fileHandle = GetRelativeFileHandle( tmxFile, atlasFilePath );

        if ( fileHandle is { Exists: true } )
        {
            return fileHandle;
        }

        throw new GdxRuntimeException( $"The 'atlas' file could not be found: '{atlasFilePath}'" );
    }

    protected void SetTextureFilters( TextureFilter min, TextureFilter mag )
    {
        foreach ( Texture texture in trackedTextures )
        {
            texture.SetFilter( min, mag );
        }

        trackedTextures.Clear();
    }

    /// <summary>
    ///     Returns the assets this asset requires to be loaded first.
    ///     This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters parameter ) => null!;

    public class AtlasTiledMapLoaderParameters
        : Parameters
    {
        public bool ForceTextureFilters { get; set; } = false;
    }


    protected interface IAtlasResolver : IImageResolver
    {
        public TextureAtlas GetAtlas();

        public class DirectAtlasResolver : IAtlasResolver
        {
            private readonly TextureAtlas _atlas;

            public DirectAtlasResolver( TextureAtlas atlas ) => _atlas = atlas;

            public TextureAtlas GetAtlas() => _atlas;

            public TextureRegion? GetImage( string name ) => _atlas.FindRegion( name );
        }

        public class AssetManagerAtlasResolver : IAtlasResolver
        {
            private readonly AssetManager _assetManager;
            private readonly string       _atlasName;

            public AssetManagerAtlasResolver( AssetManager assetManager, string atlasName )
            {
                _assetManager = assetManager;
                _atlasName    = atlasName;
            }

            public TextureAtlas GetAtlas() => _assetManager.Get< TextureAtlas >( _atlasName );

            public TextureRegion? GetImage( string name ) => GetAtlas().FindRegion( name );
        }
    }
}
