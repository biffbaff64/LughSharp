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

using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Graphics.G2D;
using LibGDXSharp.Utils.Xml;

namespace LibGDXSharp.Maps.Tiled;

[PublicAPI]
public class TmxMapLoader : BaseTmxMapLoader< TmxMapLoader.Parameters >
{
    public new class Parameters
        : BaseTmxMapLoader< TmxMapLoader.Parameters >.Parameters
    {
    }

    /// <summary>
    /// Creates a new TmxMapLoader using a <see cref="InternalFileHandleResolver"/>.
    /// </summary>
    public TmxMapLoader() : base( new InternalFileHandleResolver() )
    {
    }

    /// <summary>
    /// Creates loader
    /// </summary>
    /// <param name="resolver"></param>
    public TmxMapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    /// Loads the <see cref="TiledMap"/> from the given file. The file is resolved via
    /// the <see cref="IFileHandleResolver"/> set in the constructor of this class.
    /// By default it will resolve to an internal file. The map will be loaded for a
    /// y-up coordinate system.
    /// </summary>
    /// <param name="fileName"> the filename </param>
    /// <returns> the TiledMap </returns>
    public TiledMap Load( string fileName )
    {
        return Load( fileName, new TmxMapLoader.Parameters() );
    }

    /// <summary>
    /// Loads the <see cref="TiledMap"/> from the given file. The file is resolved
    /// via the <see cref="IFileHandleResolver"/> set in the constructor of this class.
    /// By default it will resolve to an internal file.
    /// </summary>
    /// <param name="fileName"> the filename </param>
    /// <param name="parameter"> specifies whether to use y-up, generate mip maps etc. </param>
    /// <returns> the TiledMap </returns>
    public TiledMap Load( string fileName, TmxMapLoader.Parameters parameter )
    {
        FileInfo tmxFile = Resolve( fileName );

        this.root = xml.Parse( tmxFile );

        var textures = new Dictionary< string, Texture >();

        List< FileInfo > textureFiles = GetDependencyFileHandles( tmxFile );

        foreach ( FileInfo textureFile in textureFiles )
        {
            var texture = new Texture( textureFile, parameter.GenerateMipMaps );

            texture.SetFilter( parameter.TextureMinFilter, parameter.TextureMagFilter );
            textures.Put( Path.GetFullPath( textureFile.Name ), texture );
        }

        TiledMap map = LoadTiledMap( tmxFile, parameter, new IImageResolver.DirectImageResolver( textures ) );

        map.OwnedResources = new List< object >( textures.Values.ToList() );

        return map;
    }

    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? tmxFile,
                                    AssetLoaderParameters parameter )
    {
        this.Map = LoadTiledMap
            (
            tmxFile!,
            ( ( Parameters )parameter ),
            new IImageResolver.AssetManagerImageResolver( manager! )
            );
    }

    public override TiledMap LoadSync( AssetManager? manager,
                                       string? fileName,
                                       FileInfo? file,
                                       AssetLoaderParameters parameter )
    {
        return Map;
    }

    protected List< AssetDescriptor > GetDependencyAssetDescriptors( FileInfo tmxFile,
                                                                     AssetLoaderParameters textureParameter )
    {
        var descriptors = new List< AssetDescriptor >();

        List< FileInfo > fileHandles = GetDependencyFileHandles( tmxFile );

        foreach ( FileInfo handle in fileHandles )
        {
            descriptors.Add( new AssetDescriptor( handle, typeof( Texture ), textureParameter ) );
        }

        return descriptors;
    }

    protected List< FileInfo > GetDependencyFileHandles( FileInfo tmxFile )
    {
        var fileHandles = new List< FileInfo >();

        // TileSet descriptors
        foreach ( XmlReader.Element tileset in root.GetChildrenByName( "tileset" ) )
        {
            var source = tileset.GetAttribute( "source", null );

            if ( source != null )
            {
                FileInfo? tsxFile = GetRelativeFileHandle( tmxFile, source );

                XmlReader.Element  tset         = xml.Parse( tsxFile! );
                XmlReader.Element? imageElement = tset.GetChildByName( "image" );

                if ( imageElement != null )
                {
                    var      imageSource = imageElement.GetAttribute( "source" );
                    FileInfo? image       = GetRelativeFileHandle( tsxFile, imageSource );

                    fileHandles.Add( image! );
                }
                else
                {
                    foreach ( XmlReader.Element tile in tset.GetChildrenByName( "tile" ) )
                    {
                        var      imageSource = tile.GetChildByName( "image" )?.GetAttribute( "source" );
                        FileInfo? image       = GetRelativeFileHandle( tsxFile, imageSource );

                        fileHandles.Add( image! );
                    }
                }
            }
            else
            {
                XmlReader.Element? imageElement = tileset.GetChildByName( "image" );

                if ( imageElement != null )
                {
                    var      imageSource = imageElement.GetAttribute( "source" );
                    FileInfo? image       = GetRelativeFileHandle( tmxFile, imageSource );
                    fileHandles.Add( image! );
                }
                else
                {
                    foreach ( XmlReader.Element tile in tileset.GetChildrenByName( "tile" ) )
                    {
                        var      imageSource = tile.GetChildByName( "image" )?.GetAttribute( "source" );
                        FileInfo? image       = GetRelativeFileHandle( tmxFile, imageSource );
                        fileHandles.Add( image! );
                    }
                }
            }
        }

        // ImageLayer descriptors
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach ( XmlReader.Element? imageLayer in root.GetChildrenByName( "imagelayer" ) )
        {
            XmlReader.Element? image  = imageLayer.GetChildByName( "image" );
            var                source = image?.GetAttribute( "source", null );

            if ( source != null )
            {
                FileInfo? handle = GetRelativeFileHandle( tmxFile, source );
                fileHandles.Add( handle! );
            }
        }

        return fileHandles;
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first.
    /// This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName, FileInfo? file,
                                                             AssetLoaderParameters parameter )
    {
        return null!;
    }

    /// <summary>
    /// </summary>
    /// <param name="tmxFile"></param>
    /// <param name="imageResolver"></param>
    /// <param name="tileSet"></param>
    /// <param name="element"></param>
    /// <param name="tileElements"></param>
    /// <param name="name"></param>
    /// <param name="firstgid"></param>
    /// <param name="tilewidth"></param>
    /// <param name="tileheight"></param>
    /// <param name="spacing"></param>
    /// <param name="margin"></param>
    /// <param name="source"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    /// <param name="imageSource"></param>
    /// <param name="imageWidth"></param>
    /// <param name="imageHeight"></param>
    /// <param name="image"></param>
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

        MapProperties props = tileSet.Properties;

        if ( image != null )
        {
            // One image for the whole tileSet
            TextureRegion? texture = imageResolver.GetImage( Path.GetFullPath( image.Name ) );

            if ( texture == null ) throw new GdxRuntimeException( $"Tileset image not found: {image.Name}" );

            props.Put( "imagesource", imageSource );
            props.Put( "imagewidth", imageWidth );
            props.Put( "imageheight", imageHeight );
            props.Put( "tilewidth", tilewidth );
            props.Put( "tileheight", tileheight );
            props.Put( "margin", margin );
            props.Put( "spacing", spacing );

            var stopWidth  = texture.RegionWidth - tilewidth;
            var stopHeight = texture.RegionHeight - tileheight;

            var id = firstgid;

            for ( var y = margin; y <= stopHeight; y += tileheight + spacing )
            {
                for ( var x = margin; x <= stopWidth; x += tilewidth + spacing )
                {
                    var tileRegion = new TextureRegion( texture, x, y, tilewidth, tileheight );
                    var tileId     = id++;

                    AddStaticTiledMapTile( tileSet, tileRegion, tileId, offsetX, offsetY );
                }
            }
        }
        else
        {
            // Every tile has its own image source
            foreach ( XmlReader.Element tileElement in tileElements )
            {
                XmlReader.Element? imageElement = tileElement.GetChildByName( "image" );

                if ( imageElement != null )
                {
                    imageSource = imageElement.GetAttribute( "source" );

                    image = GetRelativeFileHandle
                        ( source != null ? GetRelativeFileHandle( tmxFile, source ) : tmxFile, imageSource );
                }

                TextureRegion? texture = imageResolver.GetImage( Path.GetFullPath( image?.Name! ) );
                var            tileId  = firstgid + tileElement.GetAttribute( "id" );

                AddStaticTiledMapTile( tileSet, texture, int.Parse( tileId ), offsetX, offsetY );
            }
        }
    }
}