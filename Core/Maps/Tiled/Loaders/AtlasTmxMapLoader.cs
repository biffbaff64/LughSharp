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

using LibGDXSharp.G2D;
using LibGDXSharp.Maps;
using LibGDXSharp.Maps.Tiled;
using LibGDXSharp.Utils.Xml;

namespace LibGDXSharp.Maps.Tiled;

/// <summary>
/// A TiledMap Loader which loads tiles from a TextureAtlas instead of separate images.
/// It requires a map-level property called 'atlas' with its value being the relative
/// path to the TextureAtlas.
/// <p>
/// The atlas must have in it indexed regions named after the tilesets used in the map.
/// The indexes shall be local to the tileset (not the global id). Strip whitespace and
/// rotation should not be used when creating the atlas.
/// </p>
/// </summary>
public class AtlasTmxMapLoader : BaseTmxMapLoader< AtlasTmxMapLoader.AtlasTiledMapLoaderParameters >
{
    public sealed class AtlasTiledMapLoaderParameters
        : BaseTmxMapLoader< AtlasTiledMapLoaderParameters >.Parameters
    {
        public bool ForceTextureFilters { get; set; } = false;
    }

    protected interface IAtlasResolver : IImageResolver
    {
        public TextureAtlas GetAtlas();

        public class DirectAtlasResolver : IAtlasResolver
        {
            private TextureAtlas _atlas;

            public DirectAtlasResolver( TextureAtlas atlas )
            {
                this._atlas = atlas;
            }

            public TextureAtlas GetAtlas()
            {
                return _atlas;
            }

            public TextureRegion? GetImage( string name )
            {
                return _atlas.FindRegion( name );
            }
        }

        public class AssetManagerAtlasResolver : IAtlasResolver
        {
            private readonly AssetManager _assetManager;
            private readonly String       _atlasName;

            public AssetManagerAtlasResolver( AssetManager assetManager, String atlasName )
            {
                this._assetManager = assetManager;
                this._atlasName    = atlasName;
            }

            public TextureAtlas GetAtlas()
            {
                return _assetManager.Get< TextureAtlas >( _atlasName );
            }

            public TextureRegion? GetImage( string name )
            {
                return GetAtlas().FindRegion( name );
            }
        }
    }

    protected List< Texture > trackedTextures = new();
    protected IAtlasResolver  atlasResolver;

    public AtlasTmxMapLoader() : base( new InternalFileHandleResolver() )
    {
    }

    public AtlasTmxMapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    public TiledMap Load( string fileName )
    {
        return Load( fileName, new AtlasTmxMapLoader.AtlasTiledMapLoaderParameters() );
    }

    public TiledMap Load( string fileName, AtlasTmxMapLoader.AtlasTiledMapLoaderParameters parameter )
    {
        FileInfo tmxFile = Resolve( fileName );

        this.root = xml.Parse( tmxFile );

        FileInfo atlasFileHandle = GetAtlasFileHandle( tmxFile );

        var atlas = new TextureAtlas( atlasFileHandle );
        this.atlasResolver = new IAtlasResolver.DirectAtlasResolver( atlas );

        TiledMap map = LoadTiledMap( tmxFile, parameter, atlasResolver );
        
        map.SetOwnedResources( new List< TextureAtlas >( new [] { atlas } ) );
        SetTextureFilters( parameter.TextureMinFilter, parameter.TextureMagFilter );

        return map;
    }

    public void LoadAsync( AssetManager manager, string fileName, FileInfo tmxFile,
                           AtlasTmxMapLoader.AtlasTiledMapLoaderParameters parameter )
    {
        FileInfo atlasHandle = GetAtlasFileHandle( tmxFile );
        this.atlasResolver = new IAtlasResolver.AssetManagerAtlasResolver( manager, atlasHandle.Path() );

        this.map = loadTiledMap( tmxFile, parameter, atlasResolver );
    }

    @Override

    public TiledMap loadSync( AssetManager manager, String fileName, FileHandle file,
                              AtlasTmxMapLoader.AtlasTiledMapLoaderParameters parameter )
    {
        if ( parameter != null )
        {
            setTextureFilters( parameter.textureMinFilter, parameter.textureMagFilter );
        }

        return map;
    }

    @Override

    protected Array< AssetDescriptor > getDependencyAssetDescriptors( FileHandle tmxFile,
                                                                      TextureLoader.TextureParameter textureParameter )
    {
        Array< AssetDescriptor > descriptors = new Array< AssetDescriptor >();

        // Atlas dependencies
        final FileHandle atlasFileHandle = getAtlasFileHandle( tmxFile );

        if ( atlasFileHandle != null )
        {
            descriptors.add( new AssetDescriptor( atlasFileHandle, TextureAtlas.class));
        }

        return descriptors;
    }

    @Override

    protected void addStaticTiles( FileHandle tmxFile, ImageResolver imageResolver, TiledMapTileSet tileSet,
                                   XmlReader.Element element,
                                   Array< XmlReader.Element > tileElements, String name, int firstgid, int tilewidth,
                                   int tileheight,
                                   int spacing, int margin,
                                   String source, int offsetX, int offsetY, String imageSource, int imageWidth,
                                   int imageHeight, FileHandle image )
    {

        TextureAtlas atlas       = atlasResolver.getAtlas();
        String       regionsName = name;

        for ( Texture texture :
        atlas.getTextures()) {
            trackedTextures.add( texture );
        }

        MapProperties props = tileSet.getProperties();
        props.put( "imagesource", imageSource );
        props.put( "imagewidth", imageWidth );
        props.put( "imageheight", imageHeight );
        props.put( "tilewidth", tilewidth );
        props.put( "tileheight", tileheight );
        props.put( "margin", margin );
        props.put( "spacing", spacing );

        if ( imageSource != null && imageSource.length() > 0 )
        {
            int lastgid = firstgid + ( ( imageWidth / tilewidth ) * ( imageHeight / tileheight ) ) - 1;
            for ( AtlasRegion region :
            atlas.findRegions( regionsName )) {
                // Handle unused tileIds
                if ( region != null )
                {
                    int tileId = firstgid + region.index;

                    if ( tileId >= firstgid && tileId <= lastgid )
                    {
                        addStaticTiledMapTile( tileSet, region, tileId, offsetX, offsetY );
                    }
                }
            }
        }

        // Add tiles with individual image sources
        for ( XmlReader.Element tileElement :
        tileElements) {
            int          tileId = firstgid + tileElement.getIntAttribute( "id", 0 );
            TiledMapTile tile   = tileSet.getTile( tileId );

            if ( tile == null )
            {
                XmlReader.Element imageElement = tileElement.getChildByName( "image" );

                if ( imageElement != null )
                {
                    String regionName = imageElement.getAttribute( "source" );
                    regionName = regionName.substring( 0, regionName.lastIndexOf( '.' ) );
                    AtlasRegion region = atlas.findRegion( regionName );

                    if ( region == null )
                        throw new GdxRuntimeException( "Tileset atlasRegion not found: " + regionName );

                    addStaticTiledMapTile( tileSet, region, tileId, offsetX, offsetY );
                }
            }
        }
    }

    protected FileHandle getAtlasFileHandle( FileHandle tmxFile )
    {
        XmlReader.Element properties = root.getChildByName( "properties" );

        String atlasFilePath = null;

        if ( properties != null )
        {
            for ( XmlReader.Element property :
            properties.getChildrenByName( "property" )) {
                String name = property.getAttribute( "name" );

                if ( name.startsWith( "atlas" ) )
                {
                    atlasFilePath = property.getAttribute( "value" );

                    break;
                }
            }
        }

        if ( atlasFilePath == null )
        {
            throw new GdxRuntimeException( "The map is missing the 'atlas' property" );
        }
        else
        {
            FileInfo fileHandle = GetRelativeFileHandle( tmxFile, atlasFilePath );

            if ( !fileHandle.Exists() )
            {
                throw new GdxRuntimeException( "The 'atlas' file could not be found: '" + atlasFilePath + "'" );
            }

            return fileHandle;
        }
    }

    protected void SetTextureFilters( TextureFilter min, TextureFilter mag )
    {
        foreach ( Texture texture in trackedTextures)
        {
            texture.SetFilter( min, mag );
        }

        trackedTextures.Clear();
    }
}