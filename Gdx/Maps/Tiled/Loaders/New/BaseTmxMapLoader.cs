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


using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using LibGDXSharp.Gdx.Assets;
using LibGDXSharp.Gdx.Assets.Loaders;
using LibGDXSharp.Gdx.Assets.Loaders.Resolvers;
using LibGDXSharp.Gdx.Graphics;
using LibGDXSharp.Gdx.Graphics.G2D;
using LibGDXSharp.Gdx.Maps.Objects;
using LibGDXSharp.Gdx.Maps.Tiled.Objects;
using LibGDXSharp.Gdx.Maps.Tiled.Tiles;
using LibGDXSharp.Gdx.Maths;
using LibGDXSharp.Gdx.Utils;

using XmlReader = System.Xml.XmlReader;

namespace LibGDXSharp.Gdx.Maps.Tiled.Loaders.New;

[PublicAPI]
public abstract class BaseTmxMapLoader<TP>
    : AsynchronousAssetLoader< TiledMap, TP > where TP : BaseTmxMapLoader< TP >.BaseTmxLoaderParameters
{
    public int      MapTileWidth      { get; set; }
    public int      MapTileHeight     { get; set; }
    public int      MapWidthInPixels  { get; set; }
    public int      MapHeightInPixels { get; set; }
    public TiledMap Map               { get; set; } = null!;

    // ------------------------------------------------------------------------

    protected const uint FLAG_FLIP_HORIZONTALLY = 0x80000000;
    protected const uint FLAG_FLIP_VERTICALLY   = 0x40000000;
    protected const uint FLAG_FLIP_DIAGONALLY   = 0x20000000;
    protected const uint MASK_CLEAR             = 0xE0000000;

    protected bool convertObjectToTileSpace;
    protected bool flipY = true;

    protected XmlDocument xmlDocument;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    protected BaseTmxMapLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="tmxFile"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public List< AssetDescriptor >? GetDependencies( string fileName, FileInfo tmxFile, TP? parameter )
    {
        var textureParameter = new TextureLoader.TextureLoaderParameters();

        if ( parameter != null )
        {
            textureParameter.GenMipMaps = parameter.GenerateMipMaps;
            textureParameter.MinFilter  = parameter.TextureMinFilter;
            textureParameter.MagFilter  = parameter.TextureMagFilter;
        }

        return GetDependencyAssetDescriptors( tmxFile, textureParameter );
    }

    /// <summary>
    /// </summary>
    /// <param name="tmxFile"></param>
    /// <param name="textureLoaderParameters"></param>
    /// <returns></returns>
    protected List< AssetDescriptor >? GetDependencyAssetDescriptors( FileInfo tmxFile,
                                                                      TextureLoader.TextureLoaderParameters textureLoaderParameters )
        => default( List< AssetDescriptor >? );

    /// <summary>
    ///     Loads the map data, given the XML root element.
    /// </summary>
    /// <param name="tmxFile">The Filehandle of the tmx file </param>
    /// <param name="parameter"></param>
    /// <param name="imageResolver"></param>
    /// <returns>The <see cref="TiledMap" />.</returns>
    protected TiledMap LoadTiledMap( FileInfo tmxFile, TP? parameter, IImageResolver imageResolver )
    {
        // ----------------------------

        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml( tmxFile.Name );

        // ----------------------------

        // Extract the main Map node. Everything else is a child of this node.
        XmlNode? mapNode = xmlDocument.SelectSingleNode( "map" );

        if ( mapNode == null )
        {
            throw new GdxRuntimeException( "TMXFile does not contain a 'map' node!" );
        }

        if ( mapNode.Attributes == null )
        {
            throw new GdxRuntimeException( "Tmx Map has no attributes!" );
        }

        XmlNodeList?  propertiesList      = mapNode.SelectNodes( "properties/property" );
        XmlNodeList?  mapLayersList       = mapNode.SelectNodes( "layer" );
        XmlNodeList?  imageLayerList      = mapNode.SelectNodes( "imagelayer" );
        XmlNodeList?  nodesObjectGroup    = mapNode.SelectNodes( "objectgroup" );
        XmlNodeList?  tilesetList         = mapNode.SelectNodes( "tileset" );
        XmlNodeList?  nodesGroup          = mapNode.SelectNodes( "group" );
        XmlAttribute? attrParallaxOriginX = mapNode.Attributes?[ "parallaxoriginx" ];
        XmlAttribute? attrParallaxOriginY = mapNode.Attributes?[ "parallaxoriginy" ];

        // ----------------------------

        Map = new TiledMap();

        if ( parameter != null )
        {
            convertObjectToTileSpace = parameter.ConvertObjectToTileSpace;
            flipY                    = parameter.FlipY;
        }
        else
        {
            convertObjectToTileSpace = false;
            flipY                    = true;
        }

        // ----------------------------
        
        //@formatter:off
        var mapVersion         = mapNode.Attributes?[ "version"         ]?.Value;
        var tiledVersion       = mapNode.Attributes?[ "tiledversion"    ]?.Value;
        var mapOrientation     = mapNode.Attributes?[ "orientation"     ]?.Value;
        var renderOrder        = mapNode.Attributes?[ "renderorder"     ]?.Value;
        var mapWidth           = int.Parse( mapNode.Attributes?[ "width"           ]?.Value! );
        var mapHeight          = int.Parse( mapNode.Attributes?[ "height"          ]?.Value! );
        var tileWidth          = int.Parse( mapNode.Attributes?[ "tilewidth"       ]?.Value! );
        var tileHeight         = int.Parse( mapNode.Attributes?[ "tileheight"      ]?.Value! );
        var isInfinite         = mapNode.Attributes?[ "infinite"        ]?.Value == "1";
        var nextLayerID        = mapNode.Attributes?[ "nextlayerid"     ]?.Value;
        var nextObjectID       = mapNode.Attributes?[ "nextobjectid"    ]?.Value;
        var hexSideLength      = mapNode.Attributes?[ "hexsidelength"   ]?.Value;
        var staggerAxis        = mapNode.Attributes?[ "staggeraxis"     ]?.Value;
        var staggerIndex       = mapNode.Attributes?[ "staggerindex"    ]?.Value;
        var mapBackgroundColor = mapNode.Attributes?[ "backgroundcolor" ]?.Value;
        //@formatter:on

        // ----------------------------
        // Create the MapProperties object

        MapProperties mapProperties = Map.Properties;

        mapProperties.Put( "version", mapVersion );
        mapProperties.Put( "tiledversion", tiledVersion );

        if ( mapOrientation != string.Empty )
        {
            mapProperties.Put( "orientation", mapOrientation );
        }

        mapProperties.Put( "renderorder", renderOrder );
        mapProperties.Put( "width", mapWidth );
        mapProperties.Put( "height", mapHeight );
        mapProperties.Put( "tilewidth", tileWidth );
        mapProperties.Put( "tileheight", tileHeight );
        mapProperties.Put( "infinite", isInfinite );
        mapProperties.Put( "nextLayerID", nextLayerID );
        mapProperties.Put( "nextObjectID", nextObjectID );
        mapProperties.Put( "hexsidelength", hexSideLength );

        if ( staggerAxis != string.Empty )
        {
            mapProperties.Put( "staggeraxis", staggerAxis );
        }

        if ( staggerIndex != string.Empty )
        {
            mapProperties.Put( "staggerindex", staggerIndex );
        }

        if ( mapBackgroundColor != string.Empty )
        {
            mapProperties.Put( "backgroundcolor", mapBackgroundColor );
        }

        MapTileWidth      = tileWidth;
        MapTileHeight     = tileHeight;
        MapWidthInPixels  = mapWidth * tileWidth;
        MapHeightInPixels = mapHeight * tileHeight;

        if ( mapOrientation != string.Empty )
        {
            if ( "staggered".Equals( mapOrientation ) )
            {
                if ( mapHeight > 1 )
                {
                    MapWidthInPixels  += tileWidth / 2;
                    MapHeightInPixels =  ( MapHeightInPixels / 2 ) + ( tileHeight / 2 );
                }
            }
        }

        // ----------------------------

        // At this point we have...
        // The map loaded from the TmxFile.
        // The map and tile dimensions set up.
        // Lists of tilesets and layers created.

        // ----------------------------
//        XmlElement p = xmlReader.XmlElement ? properties = xmlElement.GetChildByName( "properties" );

//        if ( properties != null )
//        {
//            LoadProperties( Map.Properties, properties );
//        }

        // ----------------------------
        // Load all tilesets used in this map

        if ( tilesetList != null )
        {
            foreach ( XmlNode tset in tilesetList )
            {
                LoadTileSet( tset, tmxFile, imageResolver );
            }
        }

        // ----------------------------
        // Load all layers used in this map

        if ( mapLayersList != null )
        {
            foreach ( XmlNode layer in mapLayersList )
            {
                LoadLayer( Map, Map.Layers, layer, tmxFile, imageResolver );
            }
        }

        // ----------------------------

        return Map;
    }

    protected void LoadLayer( TiledMap map,
                              MapLayers parentLayers,
                              XmlNode node,
                              FileInfo tmxFile,
                              IImageResolver imageResolver )
    {
        ArgumentNullException.ThrowIfNull( node );
        ArgumentNullException.ThrowIfNull( node.Name );

        switch ( node )
        {
            case { Name: "imagelayer" }:
                LoadImageLayer( map, parentLayers, node, tmxFile, imageResolver );

                break;

            case { Name: "group" }:
                LoadLayerGroup( map, parentLayers, node, tmxFile, imageResolver );

                break;

            case { Name: "layer" }:
                LoadTileLayer( map, parentLayers, node );

                break;

            case { Name: "objectgroup" }:
                LoadObjectGroup( map, parentLayers, node );

                break;
        }
    }

    protected void LoadLayerGroup( TiledMap map,
                                   MapLayers parentLayers,
                                   XmlNode element,
                                   FileInfo tmxFile,
                                   IImageResolver imageResolver )
    {
        ArgumentNullException.ThrowIfNull( element );

        if ( element.Name.Equals( "group" ) )
        {
            var groupLayer = new MapGroupLayer();

            LoadBasicLayerInfo( groupLayer, element );

            XmlElement? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( groupLayer.Properties, properties );
            }

            for ( int i = 0, j = element.ChildCount; i < j; i++ )
            {
                XmlElement child = element.GetChild( i );

                LoadLayer( map, groupLayer.Layers, child, tmxFile, imageResolver );
            }

            foreach ( MapLayer layer in groupLayer.Layers )
            {
                layer.Parent = groupLayer;
            }

            parentLayers.Add( groupLayer );
        }
    }

    protected void LoadTileLayer( TiledMap map, MapLayers parentLayers, XmlNode element )
    {
        if ( element.Name == null )
        {
            throw new ArgumentException( "element.Name cannot by null!" );
        }

        if ( element.Name.Equals( "layer" ) )
        {
            var width      = element.GetIntAttribute( "width", 0 );
            var height     = element.GetIntAttribute( "height", 0 );
            var tileWidth  = map.Properties.Get< int >( "tilewidth" );
            var tileHeight = map.Properties.Get< int >( "tileheight" );
            var layer      = new TiledMapTileLayer( width, height, tileWidth, tileHeight );

            LoadBasicLayerInfo( layer, element );

            var              ids      = GetTileIDs( element, width, height );
            TiledMapTileSets tilesets = map.Tilesets;

            for ( var y = 0; y < height; y++ )
            {
                for ( var x = 0; x < width; x++ )
                {
                    var id               = ids[ ( y * width ) + x ];
                    var flipHorizontally = ( id & FLAG_FLIP_HORIZONTALLY ) != 0;
                    var flipVertically   = ( id & FLAG_FLIP_VERTICALLY ) != 0;
                    var flipDiagonally   = ( id & FLAG_FLIP_DIAGONALLY ) != 0;

                    ITiledMapTile? tile = tilesets.GetTile( ( int )( id & ~MASK_CLEAR ) );

                    if ( tile != null )
                    {
                        TiledMapTileLayer.Cell cell =
                            CreateTileLayerCell( flipHorizontally, flipVertically, flipDiagonally );

                        cell.SetTile( tile );
                        layer.SetCell( x, flipY ? height - 1 - y : y, cell );
                    }
                }
            }

            XmlElement? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadObjectGroup( TiledMap map, MapLayers parentLayers, XmlNode element )
    {
        if ( element.Name == null )
        {
            throw new ArgumentException( "element cannot by null!" );
        }

        if ( element.Name.Equals( "objectgroup" ) )
        {
            var layer = new MapLayer();

            LoadBasicLayerInfo( layer, element );

            XmlElement? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            foreach ( XmlElement objectElement in element.GetChildrenByName( "object" ) )
            {
                LoadObject( map, layer, objectElement );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadImageLayer( TiledMap map,
                                   MapLayers parentLayers,
                                   XmlNode element,
                                   FileInfo tmxFile,
                                   IImageResolver imageResolver )
    {
        if ( element.Name == null )
        {
            throw new ArgumentException( "element cannot by null!" );
        }

        if ( element.Name.Equals( "imagelayer" ) )
        {
            var x = float.Parse( element.HasAttribute( "offsetx" )
                                     ? element.GetAttribute( "offsetx", "0" )!
                                     : element.GetAttribute( "x", "0" )! );

            var y = float.Parse( element.HasAttribute( "offsety" )
                                     ? element.GetAttribute( "offsety", "0" )!
                                     : element.GetAttribute( "y", "0" )! );

            if ( flipY )
            {
                y = MapHeightInPixels - y;
            }

            TextureRegion? texture = null;

            XmlNode? image = element.GetChildByName( "image" );

            if ( image != null )
            {
                var       source = image.GetAttribute( "source" );
                FileInfo? handle = GetRelativeFileHandle( tmxFile, source );

                texture = imageResolver.GetImage( handle!.FullName );

                if ( texture == null )
                {
                    throw new GdxRuntimeException( "Image Texture cannot be null!" );
                }

                y -= texture.RegionHeight;
            }

            var layer = new TiledMapImageLayer( texture, x, y );

            LoadBasicLayerInfo( layer, element );

            XmlNode? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadBasicLayerInfo( MapLayer layer, XmlNode element )
    {
        var name        = element.Attributes?[ "name" ]?.Value;
        var opacityText = element.Attributes?[ "opacity " ]?.Value ?? "1.0";

        layer.Name    = name;
        layer.Opacity = float.Parse( opacityText );
        layer.Visible = element.Attributes?[ "visible" ]?.Value == "1";
        layer.OffsetX = int.Parse( element.Attributes?[ "offsetx" ]?.Value ?? "0" );
        layer.OffsetY = int.Parse( element.Attributes?[ "offsety" ]?.Value ?? "0" );
    }

    protected void LoadObject( TiledMap map, MapLayer layer, XmlNode element )
        => LoadObject( map, layer.Objects, element, MapHeightInPixels );

    protected void LoadObject( TiledMap map, ITiledMapTile tile, XmlNode element )
        => LoadObject( map, tile.GetObjects(), element, tile.TextureRegion.RegionHeight );

    protected void LoadObject( TiledMap map, MapObjects objects, XmlNode element, float heightInPixels )
    {
        if ( element.Name == null )
        {
            throw new ArgumentException( "element cannot by null!" );
        }

        if ( element.Name.Equals( "object" ) )
        {
            MapObject? mapObject = null;

            var scaleX = convertObjectToTileSpace ? 1.0f / MapTileWidth : 1.0f;
            var scaleY = convertObjectToTileSpace ? 1.0f / MapTileHeight : 1.0f;

            var x = element.GetFloatAttribute( "x", 0 ) * scaleX;

            var y = ( flipY
                        ? heightInPixels - element.GetFloatAttribute( "y", 0 )
                        : element.GetFloatAttribute( "y", 0 ) )
                  * scaleY;

            var width  = element.GetFloatAttribute( "width", 0 ) * scaleX;
            var height = element.GetFloatAttribute( "height", 0 ) * scaleY;

            if ( element.ChildCount > 0 )
            {
                XmlNode? child;

                if ( ( child = element.GetChildByName( "polygon" ) ) != null )
                {
                    var points   = child.GetAttribute( "points" ).Split( " " );
                    var vertices = new float[ points.Length * 2 ];

                    for ( var i = 0; i < points.Length; i++ )
                    {
                        var point = points[ i ].Split( "," );
                        vertices[ i * 2 ]         = float.Parse( point[ 0 ] ) * scaleX;
                        vertices[ ( i * 2 ) + 1 ] = float.Parse( point[ 1 ] ) * scaleY * ( flipY ? -1 : 1 );
                    }

                    var polygon = new Polygon( vertices );
                    polygon.SetPosition( x, y );
                    mapObject = new PolygonMapObject( polygon );
                }
                else if ( ( child = element.GetChildByName( "polyline" ) ) != null )
                {
                    var points   = child.GetAttribute( "points" ).Split( " " );
                    var vertices = new float[ points.Length * 2 ];

                    for ( var i = 0; i < points.Length; i++ )
                    {
                        var point = points[ i ].Split( "," );
                        vertices[ i * 2 ]         = float.Parse( point[ 0 ] ) * scaleX;
                        vertices[ ( i * 2 ) + 1 ] = float.Parse( point[ 1 ] ) * scaleY * ( flipY ? -1 : 1 );
                    }

                    var polyline = new Polyline( vertices );
                    polyline.SetPosition( x, y );
                    mapObject = new PolylineMapObject( polyline );
                }
                else if ( element.GetChildByName( "ellipse" ) != null )
                {
                    mapObject = new EllipseMapObject( x, flipY ? y - height : y, width, height );
                }
            }

            int id;

            if ( mapObject == null )
            {
                string? gid;

                if ( ( gid = element.GetAttribute( "gid", null ) ) != null )
                {
                    id = ( int )long.Parse( gid );

                    var flipHorizontally = ( id & FLAG_FLIP_HORIZONTALLY ) != 0;
                    var flipVertically   = ( id & FLAG_FLIP_VERTICALLY ) != 0;

                    ITiledMapTile? tile = map.Tilesets.GetTile( ( int )( id & ~MASK_CLEAR ) );

                    var tiledMapTileMapObject = new TiledMapTileMapObject( tile!, flipHorizontally, flipVertically );

                    TextureRegion textureRegion = tiledMapTileMapObject.TextureRegion!;

                    tiledMapTileMapObject.Properties.Put( "gid", id );
                    tiledMapTileMapObject.X = x;
                    tiledMapTileMapObject.Y = flipY ? y : y - height;

                    var objectWidth  = element.GetFloatAttribute( "width", textureRegion.RegionWidth );
                    var objectHeight = element.GetFloatAttribute( "height", textureRegion.RegionHeight );

                    tiledMapTileMapObject.ScaleX   = scaleX * ( objectWidth / textureRegion.RegionWidth );
                    tiledMapTileMapObject.ScaleY   = scaleY * ( objectHeight / textureRegion.RegionHeight );
                    tiledMapTileMapObject.Rotation = element.GetFloatAttribute( "rotation", 0 );

                    mapObject = tiledMapTileMapObject;
                }
                else
                {
                    mapObject = new RectangleMapObject( x, flipY ? y - height : y, width, height );
                }
            }

            mapObject.Name = element.GetAttribute( "name", null )!;
            var rotation = element.GetAttribute( "rotation", null );

            if ( rotation != null )
            {
                mapObject.Properties.Put( "rotation", float.Parse( rotation ) );
            }

            var type = element.GetAttribute( "type", null );

            if ( type != null )
            {
                mapObject.Properties.Put( "type", type );
            }

            id = element.GetIntAttribute( "id", 0 );

            if ( id != 0 )
            {
                mapObject.Properties.Put( "id", id );
            }

            mapObject.Properties.Put( "x", x );

            if ( mapObject is TiledMapTileMapObject )
            {
                mapObject.Properties.Put( "y", y );
            }
            else
            {
                mapObject.Properties.Put( "y", flipY ? y - height : y );
            }

            mapObject.Properties.Put( "width", width );
            mapObject.Properties.Put( "height", height );
            mapObject.Visible = element.GetIntAttribute( "visible", 1 ) == 1;

            XmlNode? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( mapObject.Properties, properties );
            }

            objects.Add( mapObject );
        }
    }

    protected void LoadProperties( MapProperties properties, XmlNode? element )
    {
        if ( element is not { Name: not null } )
        {
            return;
        }

        if ( element.Name.Equals( "properties" ) )
        {
            foreach ( XmlNode property in element.GetChildrenByName( "property" ) )
            {
                var name  = property.GetAttribute( "name", null );
                var value = property.GetAttribute( "value", null );
                var type  = property.GetAttribute( "type", null );

                value ??= property.Text;

                var castValue = CastProperty( name, value, type );
                properties.Put( name!, castValue );
            }
        }
    }

    protected object? CastProperty( string? name, string? value, string? type )
    {
        switch ( type )
        {
            case null:
                return value;

            case "int":
                return int.Parse( value! );

            case "float":
                return float.Parse( value! );

            case "bool":
                return bool.Parse( value! );

            case "color":
            case "colour":
            {
                // Tiled uses the format #AARRGGBB
                var opaqueColor = value?.Substring( 3 );
                var alpha       = value?.Substring( 1, 3 );

                return Color.ValueOf( opaqueColor + alpha );
            }

            default:
                throw new GdxRuntimeException
                    ( $"Wrong type given for property {name}, given : {type}, "
                    + $"supported : string, bool, int, float, color" );
        }
    }

    protected TiledMapTileLayer.Cell CreateTileLayerCell( bool flipHorizontally,
                                                          bool flipVertically,
                                                          bool flipDiagonally )
    {
        var cell = new TiledMapTileLayer.Cell();

        if ( flipDiagonally )
        {
            switch ( flipHorizontally )
            {
                case true when flipVertically:
                    cell.SetFlipHorizontally( true );
                    cell.SetRotation( TiledMapTileLayer.Cell.ROTATE270 );

                    break;

                case true:
                    cell.SetRotation( TiledMapTileLayer.Cell.ROTATE270 );

                    break;

                default:
                {
                    if ( flipVertically )
                    {
                        cell.SetRotation( TiledMapTileLayer.Cell.ROTATE90 );
                    }
                    else
                    {
                        cell.SetFlipVertically( true );
                        cell.SetRotation( TiledMapTileLayer.Cell.ROTATE270 );
                    }

                    break;
                }
            }
        }
        else
        {
            cell.SetFlipHorizontally( flipHorizontally );
            cell.SetFlipVertically( flipVertically );
        }

        return cell;
    }

    public int[] GetTileIDs( XmlNode element, int width, int height )
    {
        XmlNode? data = element.GetChildByName( "data" );

        if ( data == null )
        {
            throw new GdxRuntimeException( "data is missing" );
        }

        var encoding = data.GetAttribute( "encoding", null );

        if ( encoding == null )
        {
            // no 'encoding' attribute means that the encoding is XML
            throw new GdxRuntimeException( "Unsupported encoding (XML) for TMX Layer Data" );
        }

        var ids = new int[ width * height ];

        if ( encoding.Equals( "csv" ) )
        {
            var array = data.Text?.Split( "," );

            for ( var i = 0; i < array?.Length; i++ )
            {
                ids[ i ] = ( int )long.Parse( array[ i ].Trim() );
            }
        }
        else
        {
            if ( true )
            {
                if ( encoding.Equals( "base64" ) )
                {
                    Stream? inputStream = null;

                    try
                    {
                        var compression = data.GetAttribute( "compression", null );
                        var bytes       = Convert.FromBase64String( data.ToString() );

                        switch ( compression )
                        {
                            case null:
                                inputStream = new MemoryStream( bytes );

                                break;

                            case "gzip":
                                inputStream = new BufferedStream
                                    ( new GZipStream( new MemoryStream( bytes ), CompressionMode.Decompress ) );

                                break;

                            case "zlib":
                                inputStream = new BufferedStream
                                    ( new InflaterInputStream( new MemoryStream( bytes ) ) );

                                break;

                            default:
                                throw new GdxRuntimeException
                                    ( "Unrecognised compression (" + compression + ") for TMX Layer Data" );
                        }

                        var temp = new byte[ 4 ];

                        for ( var y = 0; y < height; y++ )
                        {
                            for ( var x = 0; x < width; x++ )
                            {
                                var read = inputStream.Read( temp );

                                while ( read < temp.Length )
                                {
                                    var curr = inputStream.Read( temp, read, temp.Length - read );

                                    if ( curr == -1 )
                                    {
                                        break;
                                    }

                                    read += curr;
                                }

                                if ( read != temp.Length )
                                {
                                    throw new GdxRuntimeException
                                        ( "Error Reading TMX Layer Data: Premature end of tile data" );
                                }

                                ids[ ( y * width ) + x ] = MathUtils.UnsignedByteToInt( temp[ 0 ] )
                                                         | ( MathUtils.UnsignedByteToInt( temp[ 1 ] ) << 8 )
                                                         | ( MathUtils.UnsignedByteToInt( temp[ 2 ] ) << 16 )
                                                         | ( MathUtils.UnsignedByteToInt( temp[ 3 ] ) << 24 );
                            }
                        }
                    }
                    catch ( IOException e )
                    {
                        throw new GdxRuntimeException( "Error Reading TMX Layer Data - IOException: " + e.Message );
                    }
                    finally
                    {
                        inputStream?.Close();
                    }
                }
                else
                {
                    // any other value of 'encoding' is one we're not aware of, probably
                    // a feature of a future version of Tiled or another editor
                    throw new GdxRuntimeException( "Unrecognised encoding (" + encoding + ") for TMX Layer Data" );
                }
            }
        }

        return ids;
    }

    protected static FileInfo? GetRelativeFileHandle( FileInfo? file, string? path )
    {
        if ( file is null || path is null )
        {
            return null;
        }

        var uri1         = new Uri( file.FullName );
        var uri2         = new Uri( path );
        var relativePath = uri1.MakeRelativeUri( uri2 ).ToString();

        return new FileInfo( relativePath );
    }

    protected void LoadTileSet( XmlNode element, FileInfo tmxFile, IImageResolver imageResolver )
    {
        if ( ( bool )element.Name?.Equals( "tileset" ) )
        {
            var firstgid    = element.GetIntAttribute( "firstgid", 1 );
            var imageSource = "";
            var imageWidth  = 0;
            var imageHeight = 0;

            FileInfo? image = null;

            var source = element.GetAttribute( "source", null );

            if ( source != null )
            {
                FileInfo? tsx = GetRelativeFileHandle( tmxFile, source );

                try
                {
                    element = xmlReader?.Parse( tsx )!;

                    XmlNode? imageElement = element.GetChildByName( "image" );

                    if ( imageElement != null )
                    {
                        imageSource = imageElement.GetAttribute( "source" );
                        imageWidth  = imageElement.GetIntAttribute( "width", 0 );
                        imageHeight = imageElement.GetIntAttribute( "height", 0 );
                        image       = GetRelativeFileHandle( tsx, imageSource );
                    }
                }
                catch ( SerializationException )
                {
                    throw new GdxRuntimeException( "Error parsing external tileset." );
                }
            }
            else
            {
                XmlNode? imageElement = element.GetChildByName( "image" );

                if ( imageElement != null )
                {
                    imageSource = imageElement.GetAttribute( "source" );
                    imageWidth  = imageElement.GetIntAttribute( "width", 0 );
                    imageHeight = imageElement.GetIntAttribute( "height", 0 );
                    image       = GetRelativeFileHandle( tmxFile, imageSource );
                }
            }

            var name       = element.Get( "name", null );
            var tilewidth  = element.GetIntAttribute( "tilewidth", 0 );
            var tileheight = element.GetIntAttribute( "tileheight", 0 );
            var spacing    = element.GetIntAttribute( "spacing", 0 );
            var margin     = element.GetIntAttribute( "margin", 0 );

            XmlNode? offset  = element.GetChildByName( "tileoffset" );
            var      offsetX = 0;
            var      offsetY = 0;

            if ( offset != null )
            {
                offsetX = offset.GetIntAttribute( "x", 0 );
                offsetY = offset.GetIntAttribute( "y", 0 );
            }

            var tileSet = new TiledMapTileSet
            {
                Name = name
            };

            MapProperties tileSetProperties = tileSet.Properties;
            XmlNode?      properties        = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( tileSetProperties, properties );
            }

            tileSetProperties.Put( "firstgid", firstgid );

            // Tiles
            List< XmlNode > tileElements = element.GetChildrenByName( "tile" );

            //TODO: IMPROVE THIS FORMATTING
            AddStaticTiles( tmxFile,
                            imageResolver,
                            tileSet,
                            element,
                            tileElements,
                            name,
                            firstgid,
                            tilewidth,
                            tileheight,
                            spacing,
                            margin,
                            source,
                            offsetX,
                            offsetY,
                            imageSource,
                            imageWidth,
                            imageHeight,
                            image );

            var animatedTiles = new List< AnimatedTiledMapTile >();

            foreach ( XmlNode tileElement in tileElements )
            {
                var            localtid = tileElement.GetIntAttribute( "id", 0 );
                ITiledMapTile? tile     = tileSet.GetTile( firstgid + localtid );

                if ( tile != null )
                {
                    AnimatedTiledMapTile? animatedTile = CreateAnimatedTile( tileSet, tile, tileElement, firstgid );

                    if ( animatedTile != null )
                    {
                        animatedTiles.Add( animatedTile );
                        tile = animatedTile;
                    }

                    AddTileProperties( tile, tileElement );
                    AddTileObjectGroup( tile, tileElement );
                }
            }

            // replace original static tiles by animated tiles
            foreach ( AnimatedTiledMapTile animatedTile in animatedTiles )
            {
                tileSet.PutTile( animatedTile.ID, animatedTile );
            }

            Map.Tilesets.AddTileSet( tileSet );
        }
    }

    protected abstract void AddStaticTiles( FileInfo tmxFile,
                                            IImageResolver imageResolver,
                                            TiledMapTileSet tileset,
                                            XmlNode element,
                                            List< XmlNode > tileElements,
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
                                            FileInfo? image );

    protected void AddTileProperties( ITiledMapTile tile, XmlNode tileElement )
    {
        string? terrain;

        if ( ( terrain = tileElement.GetAttribute( "terrain", null ) ) != string.Empty )
        {
            tile.GetProperties().Put( "terrain", terrain );
        }

        string? probability;

        if ( ( probability = tileElement.GetAttribute( "probability", null ) ) != string.Empty )
        {
            tile.GetProperties().Put( "probability", probability );
        }

        XmlNode? properties = tileElement.GetChildByName( "properties" );

        if ( properties != null )
        {
            LoadProperties( tile.GetProperties(), properties );
        }
    }

    protected void AddTileObjectGroup( ITiledMapTile tile, XmlNode tileElement )
    {
        if ( tileElement.HasAttribute( "objectgroup" ) )
        {
            XmlNode? objectgroupElement = tileElement.GetChildByName( "objectgroup" );

            if ( objectgroupElement != null )
            {
                List< XmlNode > children = objectgroupElement.GetChildrenByName( "object" );

                foreach ( XmlNode objectElement in children )
                {
                    LoadObject( Map, tile, objectElement );
                }
            }
        }
    }

    protected AnimatedTiledMapTile? CreateAnimatedTile( TiledMapTileSet tileSet,
                                                        ITiledMapTile tile,
                                                        XmlNode tileElement,
                                                        int firstgid )
    {
        XmlNode? animationElement = tileElement.GetChildByName( "animation" );

        if ( animationElement != null )
        {
            var             staticTiles = new List< StaticTiledMapTile >();
            var             intervals   = new List< int >();
            List< XmlNode > frames      = animationElement.GetChildrenByName( "frame" );

            foreach ( XmlNode frameElement in frames )
            {
                staticTiles.Add
                    (
                    ( StaticTiledMapTile )tileSet.GetTile
                        ( firstgid + int.Parse( frameElement.GetAttribute( "tileid" ) ) )!
                    );

                intervals.Add( int.Parse( frameElement.GetAttribute( "duration" ) ) );
            }

            var animatedTile = new AnimatedTiledMapTile( intervals, staticTiles )
            {
                ID = tile.ID
            };

            return animatedTile;
        }

        return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="tileSet"></param>
    /// <param name="textureRegion"></param>
    /// <param name="tileId"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    protected void AddStaticTiledMapTile( TiledMapTileSet tileSet,
                                          TextureRegion? textureRegion,
                                          int tileId,
                                          float offsetX,
                                          float offsetY )
    {
        ArgumentNullException.ThrowIfNull( tileSet );
        ArgumentNullException.ThrowIfNull( textureRegion );

        ITiledMapTile tile = new StaticTiledMapTile( textureRegion );

        tile.ID      = tileId;
        tile.OffsetX = offsetX;
        tile.OffsetY = flipY ? -offsetY : offsetY;

        tileSet.PutTile( tileId, tile );
    }

    public class BaseTmxLoaderParameters : AssetLoaderParameters
    {
        // generate mipmaps?
        public bool GenerateMipMaps { get; set; } = false;

        // The TextureFilter to use for minification
        public TextureFilter TextureMinFilter { get; set; } = TextureFilter.Nearest;

        // The TextureFilter to use for magnification
        public TextureFilter TextureMagFilter { get; set; } = TextureFilter.Nearest;

        /// <summary>
        ///     Whether to convert the objects' pixel position and size to the equivalent in tile space.
        /// </summary>
        public bool ConvertObjectToTileSpace { get; set; } = false;

        /// <summary>
        ///     Whether to flip all Y coordinates so that Y positive is up. All LibGDX renderers
        ///     require flipped Y coordinates, and thus flipY set to true. This parameter is included
        ///     for non-rendering related purposes of TMX files, or custom renderers.
        /// </summary>
        public bool FlipY { get; set; } = true;
    }
}
