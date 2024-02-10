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

using LibGDXSharp.LibCore.Assets;
using LibGDXSharp.LibCore.Assets.Loaders.Resolvers;
using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Graphics.G2D;
using LibGDXSharp.LibCore.Utils;
using LibGDXSharp.LibCore.Utils.Collections.Extensions;

using XmlReader = System.Xml.XmlReader;

namespace LibGDXSharp.LibCore.Maps.Tiled.Loaders;

[PublicAPI]
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
        FileInfo tmxFile = Resolve( fileName );

        // ----------------------------------------

        xmlDocument.LoadXml( tmxFile.Name );
        xmlRootNode = xmlDocument.SelectSingleNode( "map" );

        // ----------------------------------------

        var              textures     = new Dictionary< string, Texture >();
        List< FileInfo > textureFiles = GetDependencyFileHandles( tmxFile );

        foreach ( FileInfo textureFile in textureFiles )
        {
            var texture = new Texture( textureFile, parameter.GenerateMipMaps );

            texture.SetFilter( parameter.TextureMinFilter, parameter.TextureMagFilter );
            textures.Put( Path.GetFullPath( textureFile.Name ), texture );
        }

        // ----------------------------------------
        // Load the TiledMap

        TiledMap map = LoadTiledMap( tmxFile, parameter, new IImageResolver.DirectImageResolver( textures ) );

        map.OwnedResources = new List< object >( textures.Values.ToList() );

        return map;
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="tmxFile"></param>
    /// <param name="parameter"></param>
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

    /// <summary>
    /// </summary>
    /// <param name="tmxFile"></param>
    /// <param name="textureParameter"></param>
    /// <returns></returns>
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

    /// <summary>
    /// </summary>
    /// <param name="tmxFile"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    protected List< FileInfo > GetDependencyFileHandles( FileInfo tmxFile )
    {
        var fileHandles = new List< FileInfo >();

        // TileSet descriptors
        XmlNodeList? tilesetNodeList;

        if ( ( tilesetNodeList = xmlDocument.SelectNodes( "tileset" ) ) == null )
        {
            throw new GdxRuntimeException( "Error: Map does not contain tileset nodes." );
        }

        foreach ( XmlNode? tileset in tilesetNodeList )
        {
            var source = tileset?.Attributes?[ "source" ]?.Value;

            if ( source != null )
            {
                FileInfo? tsxFile = GetRelativeFileHandle( tmxFile, source );

                XmlNode? tset         = xmlDocument.SelectSingleNode( "tileset" );
                XmlNode? imageElement = tset?.SelectSingleNode( "image" );

                if ( imageElement != null )
                {
                    var       imageSource = imageElement.Attributes?[ "source" ]?.Value;
                    FileInfo? image       = GetRelativeFileHandle( tsxFile, imageSource );

                    fileHandles.Add( image! );
                }
                else
                {
                    if ( tset == null )
                    {
                        throw new GdxRuntimeException();
                    }

                    XmlNodeList? tileNodes = tset.SelectNodes( "tile" );

                    if ( tileNodes != null )
                    {
                        foreach ( XmlNode tile in tileNodes )
                        {
                            var       imageSource = tile.SelectSingleNode( "image" )?.Attributes?[ "source" ]?.Value;
                            FileInfo? image       = GetRelativeFileHandle( tsxFile, imageSource );

                            fileHandles.Add( image! );
                        }
                    }
                }
            }
            else
            {
                XmlNode? imageElement = tileset?.SelectSingleNode( "image" );

                if ( imageElement != null )
                {
                    var       imageSource = imageElement.Attributes?[ "source" ]?.Value;
                    FileInfo? image       = GetRelativeFileHandle( tmxFile, imageSource );

                    if ( image != null )
                    {
                        fileHandles.Add( image );
                    }
                }
                else
                {
                    XmlNodeList? tileList = tileset?.SelectNodes( "tile" );

                    if ( tileList != null )
                    {
                        foreach ( XmlNode? tile in tileList )
                        {
                            var imageSource = tile?.SelectSingleNode( "image" )?.Attributes?[ "source" ]?.Value;

                            FileInfo? image = GetRelativeFileHandle( tmxFile, imageSource );
                            fileHandles.Add( image! );
                        }
                    }
                }
            }
        }

        // ImageLayer descriptors
        XmlNodeList? imageLayerList = xmlDocument.SelectNodes( "imagelayer" );

        if ( imageLayerList != null )
        {
            foreach ( XmlNode? imageLayer in imageLayerList )
            {
                XmlNode? image  = imageLayer?.SelectSingleNode( "image" );
                var      source = image?.Attributes?[ "source" ]?.Value;

                if ( source != null )
                {
                    FileInfo? handle = GetRelativeFileHandle( tmxFile, source );
                    fileHandles.Add( handle! );
                }
            }
        }

        return fileHandles;
    }

    /// <inheritdoc/>
    protected override void AddStaticTiles( FileInfo tmxFile,
                                            IImageResolver imageResolver,
                                            TiledMapTileSet tileset,
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
        MapProperties props = tileset.Properties;

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

                    AddStaticTiledMapTile( tileset, tileRegion, tileId, offsetX, offsetY );
                }
            }
        }
        else
        {
            if ( tileElements == null )
            {
                throw new GdxRuntimeException( "Error: Tile Elements List is null!" );
            }

            // Every tile has its own image source
            foreach ( XmlNode? tileElement in tileElements )
            {
                XmlNode? imageElement = tileElement?.SelectSingleNode( "image" );

                if ( imageElement != null )
                {
                    imageSource = imageElement.Attributes?[ "source" ]?.Value;

                    image = GetRelativeFileHandle( source != null
                                                       ? GetRelativeFileHandle( tmxFile, source )
                                                       : tmxFile,
                                                   imageSource );
                }

                TextureRegion? texture = imageResolver.GetImage( Path.GetFullPath( image?.Name! ) );
                var            tileId  = firstgid + tileElement?.Attributes?[ "id" ]?.Value;

                AddStaticTiledMapTile( tileset, texture, int.Parse( tileId ), offsetX, offsetY );
            }
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class LoaderParameters : BaseTmxLoaderParameters
    {
        public LoaderParameters()
        {
        }
    }
}
