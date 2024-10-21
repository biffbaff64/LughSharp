// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Assets;
using Corelib.LibCore.Assets.Loaders;
using Corelib.LibCore.Assets.Loaders.Resolvers;
using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Maps.Tiled.Loaders;

/// <summary>
/// A TiledMap Loader which loads tiles from a TextureAtlas instead of separate images.
/// It requires a map-level property called 'atlas' with its value being the relative
/// path to the TextureAtlas.
/// <para>
/// The atlas must have in it indexed regions named after the tilesets used in the map.
/// The indexes shall be local to the tileset (not the global id). Strip whitespace and
/// rotation should not be used when creating the atlas.
/// </para>
/// </summary>
[PublicAPI]
public class AtlasTmxMapLoader( IFileHandleResolver resolver )
    : BaseTmxMapLoader< AtlasTmxMapLoader.AtlasTiledMapLoaderParameters >( resolver )
{
    protected readonly List< Texture > TrackedTextures = [ ];
    protected          IAtlasResolver? AtlasResolver;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    public AtlasTmxMapLoader()
        : this( new InternalFileHandleResolver() )
    {
    }

    public TiledMap Load( string fileName )
    {
        return Load( fileName, new AtlasTiledMapLoaderParameters() );
    }

    public TiledMap Load( string fileName, AtlasTiledMapLoaderParameters parameter )
    {
        var tmxFile = Resolve( fileName );

        // ----------------------------------------

        XmlDocument.LoadXml( tmxFile.Name );
        XmlRootNode = XmlDocument.SelectSingleNode( "map" );

        // ----------------------------------------

        var atlasFileHandle = GetAtlasFileHandle( tmxFile );
        var atlas           = new TextureAtlas( atlasFileHandle );

        AtlasResolver = new IAtlasResolver.DirectAtlasResolver( atlas );

        var map = LoadTiledMap( tmxFile, parameter, AtlasResolver );

        map.OwnedResources = [ ..new[] { atlas } ];

        SetTextureFilters( parameter.TextureMinFilter, parameter.TextureMagFilter );

        return map;
    }

    /// <inheritdoc />
    public override void LoadAsync< TP >( AssetManager? manager,
                                    FileInfo? tmxFile,
                                    TP? parameter ) where TP : class
    {
        ArgumentNullException.ThrowIfNull( manager );
        ArgumentNullException.ThrowIfNull( tmxFile );

        var atlasHandle = GetAtlasFileHandle( tmxFile );

        AtlasResolver = new IAtlasResolver.AssetManagerAtlasResolver( manager, atlasHandle.Name );

        Map = LoadTiledMap( tmxFile, parameter as AtlasTiledMapLoaderParameters, AtlasResolver );
    }

    /// <inheritdoc />
    public override TiledMap LoadSync< TP >( AssetManager manager,
                                             FileInfo file,
                                             TP? parameter ) where TP : class
    {
        if ( parameter is AtlasTiledMapLoaderParameters p )
        {
            SetTextureFilters( p.TextureMinFilter, p.TextureMagFilter );
        }

        return Map;
    }

    /// <inheritdoc />
    public override List< AssetDescriptor > GetDependencies< TP >( string filename,
                                                                   FileInfo file,
                                                                   TP? p ) where TP : class
    {
        return null!;
    }

    /// <inheritdoc />
    public override List< AssetDescriptor > GetDependencyAssetDescriptors( FileInfo tmxFile,
                                                                           TextureLoader.TextureLoaderParameters textureLoaderParameters )
    {
        var descriptors = new List< AssetDescriptor >
        {
            new( GetAtlasFileHandle( tmxFile ), typeof( TextureAtlas ), textureLoaderParameters )
        };

        return descriptors;
    }

    /// <inheritdoc />
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
        var atlas = AtlasResolver?.GetAtlas();

        foreach ( var texture in atlas!.Textures )
        {
            TrackedTextures.Add( texture );
        }

        var props = tileSet.Properties;

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

            // Handle unused tileIds
            foreach ( var region in atlas.FindRegions( name! ) )
            {
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

        if ( tileElements != null )
        {
            // Add tiles with individual image sources
            foreach ( XmlNode? tileElement in tileElements )
            {
                var tileId = ( firstgid + NumberUtils.ParseInt( tileElement?.Attributes?[ "id" ]?.Value ) ) ?? 0;
                var tile   = tileSet.GetTile( tileId );

                if ( tile == null )
                {
                    var imageElement = tileElement?.SelectSingleNode( "image" );

                    if ( imageElement != null )
                    {
                        var regionName = imageElement.Attributes?[ "source" ]?.Value;

                        regionName = regionName?.Substring( 0, regionName.LastIndexOf( '.' ) );

                        var region = atlas.FindRegion( regionName );

                        if ( region == null )
                        {
                            throw new GdxRuntimeException( $"Tileset atlasRegion not found: {regionName}" );
                        }

                        AddStaticTiledMapTile( tileSet, region, tileId, offsetX, offsetY );
                    }
                }
            }
        }
    }
    
    protected FileInfo GetAtlasFileHandle( FileInfo tmxFile )
    {
        var properties = XmlDocument.SelectSingleNode( "properties" );

        if ( properties == null )
        {
            throw new GdxRuntimeException( "The map is missing a properties node." );
        }

        string? atlasFilePath = null;

        var propertyList = properties.SelectNodes( "property" );

        if ( propertyList != null )
        {
            foreach ( XmlNode? property in propertyList )
            {
                if ( property != null )
                {
                    var name = property.Attributes?[ "name" ]?.Value;

                    if ( name!.StartsWith( "atlas" ) )
                    {
                        atlasFilePath = property.Attributes?[ "value" ]?.Value;

                        break;
                    }
                }
            }
        }

        if ( atlasFilePath == null )
        {
            throw new GdxRuntimeException( "The map is missing the 'atlas' property" );
        }

        var fileHandle = GetRelativeFileHandle( tmxFile, atlasFilePath );

        if ( fileHandle!.Exists )
        {
            return fileHandle;
        }

        throw new GdxRuntimeException( $"The 'atlas' file could not be found: '{atlasFilePath}'" );
    }

    protected void SetTextureFilters( TextureFilter min, TextureFilter mag )
    {
        foreach ( var texture in TrackedTextures )
        {
            texture.SetFilter( min, mag );
        }

        TrackedTextures.Clear();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class AtlasTiledMapLoaderParameters : BaseTmxLoaderParameters
    {
        public bool ForceTextureFilters { get; set; } = false;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    protected interface IAtlasResolver : IImageResolver
    {
        public TextureAtlas? GetAtlas();

        public class DirectAtlasResolver( TextureAtlas atlas )
            : IAtlasResolver
        {
            public TextureAtlas GetAtlas()
            {
                return atlas;
            }

            public TextureRegion? GetImage( string name )
            {
                return atlas.FindRegion( name );
            }
        }

        public class AssetManagerAtlasResolver( AssetManager assetManager, string atlasName )
            : IAtlasResolver
        {
            public TextureAtlas? GetAtlas()
            {
                return assetManager.Get( atlasName ) as TextureAtlas;
            }

            public TextureRegion? GetImage( string name )
            {
                return GetAtlas()?.FindRegion( name );
            }
        }
    }
}
