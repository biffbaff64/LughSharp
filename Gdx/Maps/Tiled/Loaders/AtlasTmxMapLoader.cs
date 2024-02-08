// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


using System.Xml;

using LibGDXSharp.Gdx.Assets;
using LibGDXSharp.Gdx.Assets.Loaders;
using LibGDXSharp.Gdx.Assets.Loaders.Resolvers;
using LibGDXSharp.Gdx.Graphics;
using LibGDXSharp.Gdx.Graphics.G2D;
using LibGDXSharp.Gdx.Utils;

using XmlReader = System.Xml.XmlReader;

namespace LibGDXSharp.Gdx.Maps.Tiled.Loaders;

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

    public AtlasTmxMapLoader()
        : base( new InternalFileHandleResolver() )
    {
    }

    public AtlasTmxMapLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    public TiledMap Load( string fileName ) => Load( fileName, new AtlasTiledMapLoaderParameters() );

    public TiledMap Load( string fileName, AtlasTiledMapLoaderParameters parameter )
    {
        FileInfo tmxFile = Resolve( fileName );

        xmlElement = xmlReader?.Parse( tmxFile );

        FileInfo atlasFileHandle = GetAtlasFileHandle( tmxFile );

        var atlas = new TextureAtlas( atlasFileHandle );

        atlasResolver = new IAtlasResolver.DirectAtlasResolver( atlas );

        TiledMap map = LoadTiledMap( tmxFile, parameter, atlasResolver );
        map.OwnedResources = new List< object >( new[] { atlas } );

        SetTextureFilters( parameter.TextureMinFilter, parameter.TextureMagFilter );

        return map;
    }

    public override void Load( AssetManager? manager,
                               string? fileName,
                               FileInfo? tmxFile,
                               AtlasTiledMapLoaderParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( manager );
        ArgumentNullException.ThrowIfNull( tmxFile );

        FileInfo atlasHandle = GetAtlasFileHandle( tmxFile );
        atlasResolver = new IAtlasResolver.AssetManagerAtlasResolver( manager, atlasHandle.Name );

        Map = LoadTiledMap( tmxFile, parameter, atlasResolver );
    }

    [PublicAPI]
    public new List< AssetDescriptor > GetDependencyAssetDescriptors( FileInfo tmxFile, TextureLoader.TextureLoaderParameters textureLoaderParameters )
    {
        var descriptors = new List< AssetDescriptor >
        {
            new( GetAtlasFileHandle( tmxFile ),
                 typeof( TextureAtlas ),
                 textureLoaderParameters )
        };

        return descriptors;
    }

    //TODO: Add documentation for these parameters
    protected override void AddStaticTiles( FileInfo tmxFile,
                                            IImageResolver imageResolver,
                                            TiledMapTileSet tileSet,
                                            XmlNode node,
                                            XmlNodeList? tileElements,
                                            string? name,
                                            int firstgid,
                                            int tilewidth,
                                            int tileheight,
                                            int spacing,
                                            int margin,
                                            string? source,
                                            int offsetX,
                                            int offsetY,
                                            string? imageSource,
                                            int imageWidth,
                                            int imageHeight,
                                            FileInfo? image )
    {
        TextureAtlas atlas = atlasResolver!.GetAtlas();

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

            foreach ( AtlasRegion? region in atlas.FindRegions( name! ) )
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

    protected FileInfo GetAtlasFileHandle( FileInfo tmxFile )
    {
        XmlReader.Element? properties = xmlElement?.GetChildByName( "properties" );

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

        if ( fileHandle!.Exists )
        {
            return fileHandle;
        }

        throw new GdxRuntimeException( $"The 'atlas' file could not be found: '{atlasFilePath}'" );
    }

    /// <summary>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="mag"></param>
    protected void SetTextureFilters( TextureFilter min, TextureFilter mag )
    {
        foreach ( Texture texture in trackedTextures )
        {
            texture.SetFilter( min, mag );
        }

        trackedTextures.Clear();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class AtlasTiledMapLoaderParameters : BaseTmxLoaderParameters
    {
        public bool ForceTextureFilters { get; set; } = false;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

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
