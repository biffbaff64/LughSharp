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


using LibGDXSharp.Gdx.Assets;
using LibGDXSharp.Gdx.Assets.Loaders.Resolvers;
using LibGDXSharp.Gdx.Graphics;
using LibGDXSharp.Gdx.Graphics.G2D;
using LibGDXSharp.Gdx.Utils;
using LibGDXSharp.Gdx.Utils.Collections.Extensions;

using XmlReader = System.Xml.XmlReader;

namespace LibGDXSharp.Gdx.Maps.Tiled.Loaders.New;

public class TmxMapLoader : BaseTmxMapLoader< TmxMapLoader.LoaderParameters >
{
    /// <summary>
    ///     Creates a new TmxMapLoader using a <see cref="InternalFileHandleResolver" />.
    /// </summary>
    public TmxMapLoader() : base( new InternalFileHandleResolver() )
    {
    }

    /// <summary>
    ///     Creates loader
    /// </summary>
    /// <param name="resolver"></param>
    public TmxMapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    ///     Loads the <see cref="TiledMap" /> from the given file. The file is resolved via
    ///     the <see cref="IFileHandleResolver" /> set in the constructor of this class.
    ///     By default it will resolve to an internal file. The map will be loaded for a
    ///     y-up coordinate system.
    /// </summary>
    /// <param name="fileName"> the filename </param>
    /// <returns> the TiledMap </returns>
    public TiledMap Load( string fileName ) => Load( fileName, new LoaderParameters() );

    /// <summary>
    ///     Loads the <see cref="TiledMap" /> from the given file. The file is resolved
    ///     via the <see cref="IFileHandleResolver" /> set in the constructor of this class.
    ///     By default it will resolve to an internal file.
    /// </summary>
    /// <param name="fileName"> the filename </param>
    /// <param name="parameter"> specifies whether to use y-up, generate mip maps etc. </param>
    /// <returns> the TiledMap </returns>
    public TiledMap Load( string fileName, LoaderParameters parameter )
    {
        GdxRuntimeException.ThrowIfNull( xmlReader );

        FileInfo tmxFile = Resolve( fileName );

        xmlElement = xmlReader.Parse( tmxFile );

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

    public override void Load( AssetManager? manager,
                               string? fileName,
                               FileInfo? tmxFile,
                               LoaderParameters? parameter )
    {
        Debug.Assert( tmxFile != null );

        Map = LoadTiledMap( tmxFile,
                            parameter,
                            new IImageResolver.AssetManagerImageResolver( manager! ) );
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
        foreach ( XmlReader.Element tileset in xmlElement!.GetChildrenByName( "tileset" ) )
        {
            var source = tileset.GetAttribute( "source", null );

            if ( source != null )
            {
                FileInfo? tsxFile = GetRelativeFileHandle( tmxFile, source );

                XmlReader.Element? tset         = xmlReader!.Parse( tsxFile! );
                XmlReader.Element? imageElement = tset!.GetChildByName( "image" );

                if ( imageElement != null )
                {
                    var       imageSource = imageElement.GetAttribute( "source" );
                    FileInfo? image       = GetRelativeFileHandle( tsxFile, imageSource );

                    fileHandles.Add( image! );
                }
                else
                {
                    foreach ( XmlReader.Element tile in tset.GetChildrenByName( "tile" ) )
                    {
                        var       imageSource = tile.GetChildByName( "image" )?.GetAttribute( "source" );
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
                    var       imageSource = imageElement.GetAttribute( "source" );
                    FileInfo? image       = GetRelativeFileHandle( tmxFile, imageSource );
                    fileHandles.Add( image! );
                }
                else
                {
                    foreach ( XmlReader.Element tile in tileset.GetChildrenByName( "tile" ) )
                    {
                        var       imageSource = tile.GetChildByName( "image" )?.GetAttribute( "source" );
                        FileInfo? image       = GetRelativeFileHandle( tmxFile, imageSource );
                        fileHandles.Add( image! );
                    }
                }
            }
        }

        // ImageLayer descriptors
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach ( XmlReader.Element? imageLayer in xmlElement.GetChildrenByName( "imagelayer" ) )
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

    //TODO: Complete this documentation
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

            if ( texture == null )
            {
                throw new GdxRuntimeException( $"Tileset image not found: {image.Name}" );
            }

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

                    image = GetRelativeFileHandle( source != null ? GetRelativeFileHandle( tmxFile, source ) : tmxFile, imageSource );
                }

                TextureRegion? texture = imageResolver.GetImage( Path.GetFullPath( image?.Name! ) );
                var            tileId  = firstgid + tileElement.GetAttribute( "id" );

                AddStaticTiledMapTile( tileSet, texture, int.Parse( tileId ), offsetX, offsetY );
            }
        }
    }

    public class LoaderParameters : BaseTmxLoaderParameters
    {
        public LoaderParameters()
        {
        }
    }
}
