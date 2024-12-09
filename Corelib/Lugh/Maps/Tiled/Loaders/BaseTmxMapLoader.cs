// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Assets;
using Corelib.Lugh.Assets.Loaders;
using Corelib.Lugh.Assets.Loaders.Resolvers;
using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Maps.Objects;
using Corelib.Lugh.Maps.Tiled.Objects;
using Corelib.Lugh.Maps.Tiled.Tiles;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils.Exceptions;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Corelib.Lugh.Maps.Tiled.Loaders;

[PublicAPI]
public abstract class BaseTmxMapLoader< TP >( IFileHandleResolver resolver )
    : AsynchronousAssetLoader( resolver ) where TP : BaseTmxMapLoader< TP >.BaseTmxLoaderParameters
{
    public int      MapTileWidth      { get; set; }
    public int      MapTileHeight     { get; set; }
    public int      MapWidthInPixels  { get; set; }
    public int      MapHeightInPixels { get; set; }
    public TiledMap Map               { get; set; } = null!;

    // ========================================================================
    // ========================================================================

    protected const uint FLAG_FLIP_HORIZONTALLY = 0x80000000;
    protected const uint FLAG_FLIP_VERTICALLY   = 0x40000000;
    protected const uint FLAG_FLIP_DIAGONALLY   = 0x20000000;
    protected const uint MASK_CLEAR             = 0xE0000000;

    private XmlNodeList? _groupList;
    private XmlNodeList? _imageLayerList;
    private XmlNodeList? _mapLayersList;
    private XmlNodeList? _objectGroupList;
    private XmlNodeList? _tilesetList;

    // ========================================================================
    // ========================================================================

    protected bool ConvertObjectToTileSpace;
    protected bool FlipY = true;

    protected XmlDocument XmlDocument = new();
    protected XmlNode?    XmlRootNode;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Loads the map data, given the XML root element.
    /// </summary>
    /// <param name="tmxFile">The Filehandle of the tmx file </param>
    /// <param name="parameter"></param>
    /// <param name="imageResolver"></param>
    /// <returns>The <see cref="TiledMap"/>.</returns>
    protected TiledMap LoadTiledMap( FileInfo tmxFile, TP? parameter, IImageResolver imageResolver )
    {
        // ====================================================================

        if ( !XmlDocument.HasChildNodes )
        {
            XmlDocument.LoadXml( tmxFile.Name );
        }

        // ====================================================================

        // Extract the main Map node. Everything else is a child of this node.
        XmlRootNode = XmlDocument.SelectSingleNode( "map" );

        if ( XmlRootNode == null )
        {
            throw new GdxRuntimeException( "TMXFile does not contain a 'map' node!" );
        }

        if ( XmlRootNode.Attributes == null )
        {
            throw new GdxRuntimeException( "Tmx Map has no attributes!" );
        }

        _tilesetList     = XmlRootNode.SelectNodes( "tileset" );
        _mapLayersList   = XmlRootNode.SelectNodes( "layer" );
        _imageLayerList  = XmlRootNode.SelectNodes( "imagelayer" );
        _objectGroupList = XmlRootNode.SelectNodes( "objectgroup" );
        _groupList       = XmlRootNode.SelectNodes( "group" );

        // ----------------------------

        Map = new TiledMap();

        if ( parameter != null )
        {
            ConvertObjectToTileSpace = parameter.ConvertObjectToTileSpace;
            FlipY                    = parameter.FlipY;
        }
        else
        {
            ConvertObjectToTileSpace = false;
            FlipY                    = true;
        }

        var mapData = new MapData( XmlRootNode );

        // Fetch the existing MapProperties object from the newly created map,
        // and add the required data to it.

        var mapProperties = Map.Properties;

        mapProperties.Put( "version", mapData.MapVersion );
        mapProperties.Put( "tiledversion", mapData.TiledVersion );

        if ( mapData.MapOrientation != string.Empty )
        {
            mapProperties.Put( "orientation", mapData.MapOrientation );
        }

        //@formatter:off
        mapProperties.Put( "renderorder",   mapData.RenderOrder );
        mapProperties.Put( "width",         mapData.MapWidth );
        mapProperties.Put( "height",        mapData.MapHeight );
        mapProperties.Put( "tilewidth",     mapData.TileWidth );
        mapProperties.Put( "tileheight",    mapData.TileHeight );
        mapProperties.Put( "infinite",      mapData.IsInfinite );
        mapProperties.Put( "nextLayerID",   mapData.NextLayerID );
        mapProperties.Put( "nextObjectID",  mapData.NextObjectID );
        mapProperties.Put( "hexsidelength", mapData.HexSideLength );
        //@formatter:on

        //TODO: Check the TiledMap documentation to see if these have defaults.

        if ( mapData.StaggerAxis != string.Empty )
        {
            mapProperties.Put( "staggeraxis", mapData.StaggerAxis );
        }

        if ( mapData.StaggerIndex != string.Empty )
        {
            mapProperties.Put( "staggerindex", mapData.StaggerIndex );
        }

        if ( mapData.MapBackgroundColor != string.Empty )
        {
            mapProperties.Put( "backgroundcolor", mapData.MapBackgroundColor );
        }

        MapTileWidth      = mapData.TileWidth;
        MapTileHeight     = mapData.TileHeight;
        MapWidthInPixels  = mapData.MapWidth * mapData.TileWidth;
        MapHeightInPixels = mapData.MapHeight * mapData.TileHeight;

        if ( mapData.MapOrientation != string.Empty )
        {
            if ( "staggered".Equals( mapData.MapOrientation ) )
            {
                if ( mapData.MapHeight > 1 )
                {
                    MapWidthInPixels  += mapData.TileWidth / 2;
                    MapHeightInPixels =  ( MapHeightInPixels / 2 ) + ( mapData.TileHeight / 2 );
                }
            }
        }

        // ----------------------------
        // Load all tilesets used in this map

        if ( _tilesetList != null )
        {
            foreach ( XmlNode tset in _tilesetList )
            {
                LoadTileSet( tset, tmxFile, imageResolver );
            }
        }

        // ----------------------------
        // Load all layers used in this map

        if ( _mapLayersList != null )
        {
            foreach ( XmlNode layer in _mapLayersList )
            {
                LoadTileLayer( Map, Map.Layers, layer );
            }
        }

        if ( _imageLayerList != null )
        {
            foreach ( XmlNode layer in _imageLayerList )
            {
                LoadImageLayer( Map, Map.Layers, layer, tmxFile, imageResolver );
            }
        }

        if ( _objectGroupList != null )
        {
            foreach ( XmlNode layer in _objectGroupList )
            {
                LoadObjectGroup( Map, Map.Layers, layer );
            }
        }

        if ( _groupList != null )
        {
            foreach ( XmlNode layer in _groupList )
            {
                LoadLayerGroup( Map, Map.Layers, layer, tmxFile, imageResolver );
            }
        }

        return Map;
    }

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

    public virtual List< AssetDescriptor >? GetDependencyAssetDescriptors( FileInfo tmxFile,
                                                                           TextureLoader.TextureLoaderParameters textureLoaderParameters )
    {
        return default( List< AssetDescriptor >? );
    }

    // ========================================================================
    // ========================================================================
    // Load map components - layers, tilesets, groups etc

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="parentLayers"> The actual layer group belonging to the map. </param>
    /// <param name="node"> The xml node being processed. </param>
    /// <param name="tmxFile"></param>
    /// <param name="imageResolver"></param>
    protected void LoadLayerGroup( TiledMap map,
                                   MapLayers parentLayers,
                                   XmlNode node,
                                   FileInfo tmxFile,
                                   IImageResolver imageResolver )
    {
        ArgumentNullException.ThrowIfNull( node );

        if ( node.Name.Equals( "group" ) )
        {
            var groupLayer = new MapGroupLayer();

            LoadBasicLayerInfo( groupLayer, node );

            XmlNode? properties;

            if ( ( properties = node.SelectSingleNode( "properties" ) ) != null )
            {
                LoadProperties( groupLayer.Properties, properties );
            }

            for ( int i = 0, j = node.ChildNodes.Count; i < j; i++ )
            {
                var child = node.ChildNodes[ i ];

                if ( child != null )
                {
                    switch ( child )
                    {
                        case { Name: "layer" }:
                            LoadTileLayer( map, groupLayer.Layers, child );

                            break;

                        case { Name: "imagelayer" }:
                            LoadImageLayer( map, groupLayer.Layers, child, tmxFile, imageResolver );

                            break;

                        case { Name: "group" }:
                            LoadLayerGroup( map, groupLayer.Layers, child, tmxFile, imageResolver );

                            break;

                        case { Name: "objectgroup" }:
                            LoadObjectGroup( map, groupLayer.Layers, child );

                            break;
                    }
                }
            }

            foreach ( var layer in groupLayer.Layers )
            {
                layer.Parent = groupLayer;
            }

            parentLayers.Add( groupLayer );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="parentLayers"> The actual layer group belonging to the map. </param>
    /// <param name="node"> The xml node being processed. </param>
    /// <exception cref="ArgumentException"></exception>
    protected void LoadTileLayer( TiledMap map, MapLayers parentLayers, XmlNode node )
    {
        if ( node.Name == null )
        {
            throw new ArgumentException( "node.Name cannot by null!" );
        }

        if ( node.Name.Equals( "layer" ) )
        {
            var width  = NumberUtils.ParseInt( node.Attributes?[ "width" ]?.Value ) ?? 0;
            var height = NumberUtils.ParseInt( node.Attributes?[ "height" ]?.Value ) ?? 0;

            var tileWidth  = map.Properties.Get< int >( "tilewidth" );
            var tileHeight = map.Properties.Get< int >( "tileheight" );

            var layer = new TiledMapTileLayer( width, height, tileWidth, tileHeight );

            LoadBasicLayerInfo( layer, node );

            var ids = GetTileIDs( node, width, height );

            var tilesets = map.Tilesets;

            for ( var y = 0; y < height; y++ )
            {
                for ( var x = 0; x < width; x++ )
                {
                    var id               = ids[ ( y * width ) + x ];
                    var flipHorizontally = ( id & FLAG_FLIP_HORIZONTALLY ) != 0;
                    var flipVertically   = ( id & FLAG_FLIP_VERTICALLY ) != 0;
                    var flipDiagonally   = ( id & FLAG_FLIP_DIAGONALLY ) != 0;

                    var tile = tilesets.GetTile( ( int ) ( id & ~MASK_CLEAR ) );

                    if ( tile != null )
                    {
                        var cell = CreateTileLayerCell( flipHorizontally, flipVertically, flipDiagonally );

                        cell.SetTile( tile );
                        layer.SetCell( x, FlipY ? height - 1 - y : y, cell );
                    }
                }
            }

            var properties = node.SelectSingleNode( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            parentLayers.Add( layer );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="parentLayers"> The actual layer group belonging to the map. </param>
    /// <param name="node"></param>
    /// <exception cref="ArgumentException"></exception>
    protected void LoadObjectGroup( TiledMap map, MapLayers parentLayers, XmlNode node )
    {
        if ( node.Name == null )
        {
            throw new ArgumentException( "element cannot by null!" );
        }

        if ( node.Name.Equals( "objectgroup" ) )
        {
            var layer = new MapLayer();

            LoadBasicLayerInfo( layer, node );

            XmlNode? properties;

            if ( ( properties = node.SelectSingleNode( "properties" ) ) != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            XmlNode? objectNode;

            if ( ( objectNode = node.SelectSingleNode( "object" ) ) != null )
            {
                foreach ( XmlNode objectElement in objectNode )
                {
                    LoadObject( map, layer, objectElement );
                }
            }

            parentLayers.Add( layer );
        }
    }

    /// <summary>
    /// From the official Tiled website:
    /// "Image layers provide a way to quickly include a single image as foreground
    /// or background of your map. They currently have limited functionality and you
    /// may consider adding the image as a Tileset instead and place it as a Tile
    /// Object. This way, you gain the ability to freely scale and rotate the image."
    /// See https://doc.mapeditor.org/en/stable/manual/layers/
    /// </summary>
    /// <param name="map"> The parent <see cref="TiledMap"/>. </param>
    /// <param name="parentLayers"> The actual layer group belonging to the map. </param>
    /// <param name="node"> The xml node being processed. </param>
    /// <param name="tmxFile"> The parent TMX map file. </param>
    /// <param name="imageResolver"> The asset loader interface. </param>
    protected void LoadImageLayer( TiledMap map,
                                   MapLayers parentLayers,
                                   XmlNode node,
                                   FileInfo tmxFile,
                                   IImageResolver imageResolver )
    {
        if ( node.Name == null )
        {
            throw new ArgumentException( "element cannot by null!" );
        }

        if ( node.Name.Equals( "imagelayer" ) )
        {
            var x = float.Parse( node.SelectSingleNode( "offsetx" )?.Value
                              ?? node.SelectSingleNode( "x" )?.Value
                              ?? "0" );

            var y = float.Parse( node.SelectSingleNode( "offsety" )?.Value
                              ?? node.SelectSingleNode( "y" )?.Value
                              ?? "0" );

            if ( FlipY )
            {
                y = MapHeightInPixels - y;
            }

            TextureRegion? texture = null;

            var image = node.SelectSingleNode( "image" );

            if ( image != null )
            {
                var source = image.SelectSingleNode( "source" )?.Value;
                var handle = GetRelativeFileHandle( tmxFile, source );

                texture = imageResolver.GetImage( handle!.FullName );

                if ( texture == null )
                {
                    throw new GdxRuntimeException( "Image Texture cannot be null!" );
                }

                y -= texture.RegionHeight;
            }

            var imageLayer = new TiledMapImageLayer( texture, x, y );

            LoadBasicLayerInfo( imageLayer, node );

            var properties = node.SelectSingleNode( "properties" );

            if ( properties != null )
            {
                LoadProperties( imageLayer.Properties, properties );
            }

            parentLayers.Add( imageLayer );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="node"></param>
    protected void LoadBasicLayerInfo( MapLayer layer, XmlNode node )
    {
        var name        = node.Attributes?[ "name" ]?.Value;
        var opacityText = node.Attributes?[ "opacity " ]?.Value ?? "1.0";

        layer.Name    = name;
        layer.Opacity = float.Parse( opacityText );
        layer.Visible = node.Attributes?[ "visible" ]?.Value == "1";
        layer.OffsetX = NumberUtils.ParseInt( node.Attributes?[ "offsetx" ]?.Value ) ?? 0;
        layer.OffsetY = NumberUtils.ParseInt( node.Attributes?[ "offsety" ]?.Value ) ?? 0;
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="layer"></param>
    /// <param name="node"></param>
    protected void LoadObject( TiledMap map, MapLayer layer, XmlNode node )
    {
        LoadObject( map, layer.Objects, node, MapHeightInPixels );
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="tile"></param>
    /// <param name="node"></param>
    protected void LoadObject( TiledMap map, ITiledMapTile tile, XmlNode node )
    {
        LoadObject( map, tile.MapObjects, node, tile.TextureRegion.RegionHeight );
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="objects"></param>
    /// <param name="node"></param>
    /// <param name="heightInPixels"></param>
    /// <exception cref="ArgumentException"></exception>
    protected void LoadObject( TiledMap map, MapObjects objects, XmlNode? node, float heightInPixels )
    {
        if ( node?.Name == null )
        {
            return;
        }

        if ( node.Name.Equals( "object" ) )
        {
            MapObject? mapObject = null;

            var scaleX = ConvertObjectToTileSpace ? 1.0f / MapTileWidth : 1.0f;
            var scaleY = ConvertObjectToTileSpace ? 1.0f / MapTileHeight : 1.0f;

            var x = ( NumberUtils.ParseFloat( node.Attributes?[ "x" ]?.Value ) * scaleX ) ?? 0;

            var parsedY = NumberUtils.ParseFloat( node.Attributes?[ "y" ]?.Value ) ?? 0;
            var y       = ( FlipY ? heightInPixels - parsedY : parsedY ) * scaleY;

            var width  = ( NumberUtils.ParseFloat( node.Attributes?[ "width" ]?.Value ) * scaleX ) ?? 0;
            var height = ( NumberUtils.ParseFloat( node.Attributes?[ "height" ]?.Value ) * scaleY ) ?? 0;

            if ( node.ChildNodes.Count > 0 )
            {
                XmlNode? child;

                if ( ( child = node.SelectSingleNode( "polygon" ) ) != null )
                {
                    var points   = child.Attributes?[ "points" ]?.Value.Split( " " );
                    var vertices = new float[ points!.Length * 2 ];

                    for ( var i = 0; i < points.Length; i++ )
                    {
                        var point = points[ i ].Split( "," );
                        vertices[ i * 2 ]         = float.Parse( point[ 0 ] ) * scaleX;
                        vertices[ ( i * 2 ) + 1 ] = float.Parse( point[ 1 ] ) * scaleY * ( FlipY ? -1 : 1 );
                    }

                    var polygon = new Polygon( vertices );
                    polygon.SetPosition( x, y );
                    mapObject = new PolygonMapObject( polygon );
                }
                else if ( ( child = node.SelectSingleNode( "polyline" ) ) != null )
                {
                    var points   = child.Attributes?[ "points" ]?.Value.Split( " " );
                    var vertices = new float[ points!.Length * 2 ];

                    for ( var i = 0; i < points.Length; i++ )
                    {
                        var point = points[ i ].Split( "," );
                        vertices[ i * 2 ]         = float.Parse( point[ 0 ] ) * scaleX;
                        vertices[ ( i * 2 ) + 1 ] = float.Parse( point[ 1 ] ) * scaleY * ( FlipY ? -1 : 1 );
                    }

                    var polyline = new Polyline( vertices );
                    polyline.SetPosition( x, y );
                    mapObject = new PolylineMapObject( polyline );
                }
                else if ( node.SelectSingleNode( "ellipse" ) != null )
                {
                    mapObject = new EllipseMapObject( x, FlipY ? y - height : y, width, height );
                }
            }

            int id;

            if ( mapObject == null )
            {
                string? gid;

                if ( ( gid = node.Attributes?[ "gid" ]?.Value ) != null )
                {
                    id = ( int ) long.Parse( gid );

                    var flipHorizontally = ( id & FLAG_FLIP_HORIZONTALLY ) != 0;
                    var flipVertically   = ( id & FLAG_FLIP_VERTICALLY ) != 0;

                    var tile = map.Tilesets.GetTile( ( int ) ( id & ~MASK_CLEAR ) );

                    var tiledMapTileMapObject = new TiledMapTileMapObject( tile!, flipHorizontally, flipVertically );

                    var textureRegion = tiledMapTileMapObject.TextureRegion!;

                    tiledMapTileMapObject.Properties.Put( "gid", id );
                    tiledMapTileMapObject.X = x;
                    tiledMapTileMapObject.Y = FlipY ? y : y - height;

                    var objectWidth  = NumberUtils.ParseFloat( node.Attributes?[ "width " ]?.Value ) ?? textureRegion.RegionWidth;
                    var objectHeight = NumberUtils.ParseFloat( node.Attributes?[ "height " ]?.Value ) ?? textureRegion.RegionHeight;

                    tiledMapTileMapObject.ScaleX   = scaleX * ( objectWidth / textureRegion.RegionWidth );
                    tiledMapTileMapObject.ScaleY   = scaleY * ( objectHeight / textureRegion.RegionHeight );
                    tiledMapTileMapObject.Rotation = NumberUtils.ParseFloat( node.Attributes?[ "rotation" ]?.Value ) ?? 0;

                    mapObject = tiledMapTileMapObject;
                }
                else
                {
                    mapObject = new RectangleMapObject( x, FlipY ? y - height : y, width, height );
                }
            }

            mapObject.Name = node.Attributes?[ "name" ]?.Value!;

            var rotation = node.Attributes?[ "rotation" ]?.Value;

            if ( rotation != null )
            {
                mapObject.Properties.Put( "rotation", float.Parse( rotation ) );
            }

            var type = node.Attributes?[ "type" ]?.Value;

            if ( type != null )
            {
                mapObject.Properties.Put( "type", type );
            }

            id = NumberUtils.ParseInt( node.Attributes?[ "id" ]?.Value ) ?? 0;

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
                mapObject.Properties.Put( "y", FlipY ? y - height : y );
            }

            mapObject.Properties.Put( "width", width );
            mapObject.Properties.Put( "height", height );
            mapObject.IsVisible = ( NumberUtils.ParseInt( node.Attributes?[ "visible" ]?.Value ) ?? 1 ) == 1;

            var properties = node.SelectSingleNode( "properties" );

            if ( properties != null )
            {
                LoadProperties( mapObject.Properties, properties );
            }

            objects.Add( mapObject );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="node"></param>
    protected void LoadProperties( MapProperties properties, XmlNode? node )
    {
        if ( node?.Name == null )
        {
            return;
        }

        if ( node.Name.Equals( "properties" ) )
        {
            var props = node.SelectNodes( "property" );

            if ( props != null )
            {
                foreach ( XmlNode property in props )
                {
                    var name  = property.Attributes?[ "name" ]?.Value;
                    var value = property.Attributes?[ "value" ]?.Value;
                    var type  = property.Attributes?[ "type" ]?.Value;

                    value ??= property.InnerText;

                    var castValue = CastProperty( name, value, type );
                    properties.Put( name!, castValue );
                }
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
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
                throw new GdxRuntimeException( $"Wrong type given for property {name}, given : {type}, "
                                             + $"supported : string, bool, int, float, color ( or colour )" );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="flipHorizontally"></param>
    /// <param name="flipVertically"></param>
    /// <param name="flipDiagonally"></param>
    /// <returns></returns>
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

    /// <summary>
    /// </summary>
    /// <param name="node"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public int[] GetTileIDs( XmlNode node, int width, int height )
    {
        var data = node.SelectSingleNode( "data" );

        if ( data == null )
        {
            throw new GdxRuntimeException( "data is missing" );
        }

        var encoding = data.Attributes?[ "encoding" ]?.Value;

        if ( encoding == null )
        {
            // no 'encoding' attribute means that the encoding is XML
            throw new GdxRuntimeException( "Unsupported encoding (XML) for TMX Layer Data" );
        }

        var ids = new int[ width * height ];

        if ( encoding.Equals( "csv" ) )
        {
            var array = data.InnerText.Split( "," );

            for ( var i = 0; i < array.Length; i++ )
            {
                ids[ i ] = ( int ) long.Parse( array[ i ].Trim() );
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
                        var compression = data.Attributes?[ "compression" ]?.Value;
                        var bytes       = Convert.FromBase64String( data.ToString() ?? "" );

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
                                inputStream = new BufferedStream( new InflaterInputStream( new MemoryStream( bytes ) ) );

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

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="path"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Loads a Tileset as described in <paramref name="tilesetNode"/>.
    /// The Node is laid ouit as follows:-
    /// <code>
    /// &lt;tileset firstgid="x" source="filename.tsx"/&gt;
    /// </code>
    /// where 'x' is the id of the first tile in the tileset.
    /// The width and height dimensions of the image used for the tiles are held in the TSX file, as are
    /// the tile width/height, number of columns in the tile image, and total tile count.
    /// </summary>
    /// <param name="tilesetNode"> The node referencing the TSX tileset file. </param>
    /// <param name="tmxFile"> The current TMX file being processed. </param>
    /// <param name="imageResolver"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    protected void LoadTileSet( XmlNode tilesetNode, FileInfo tmxFile, IImageResolver imageResolver )
    {
        if ( tilesetNode.Name.Equals( "tileset" ) )
        {
            var firstgid    = NumberUtils.ParseInt( tilesetNode.Attributes?[ "firstgid" ]?.Value ) ?? 1;
            var imageSource = "";
            var imageWidth  = 0;
            var imageHeight = 0;

            FileInfo? image = null;

            // Fetch the .tsx file which contains the image name etc.
            var source = tilesetNode.Attributes?[ "source" ]?.Value;

            if ( source != null )
            {
                var tsx = new XmlDocument();
                tsx.LoadXml( source );

                try
                {
                    tilesetNode = tsx.SelectSingleNode( "tileset" )!;

                    var imageElement = tilesetNode.SelectSingleNode( "image" );

                    if ( imageElement != null )
                    {
                        // Image filename
                        imageSource = imageElement.Attributes?[ "source" ]?.Value;

                        // Image width, in pixels
                        imageWidth = NumberUtils.ParseInt( imageElement.Attributes?[ "width" ]?.Value ) ?? 0;

                        // Image height, in pixels
                        imageHeight = NumberUtils.ParseInt( imageElement.Attributes?[ "height" ]?.Value ) ?? 0;

                        if ( imageSource is null )
                        {
                            throw new GdxRuntimeException( $"Error: Image source for tileset {tmxFile.Name} is null!" );
                        }

                        image = GetRelativeFileHandle( new FileInfo( imageSource ), imageSource );
                    }
                }
                catch ( SerializationException )
                {
                    throw new GdxRuntimeException( "Error parsing external tileset." );
                }
            }
            else
            {
                var imageElement = tilesetNode.SelectSingleNode( "image" );

                if ( imageElement != null )
                {
                    // Image filename
                    imageSource = imageElement.Attributes?[ "source" ]?.Value;

                    // Image width, in pixels
                    imageWidth = NumberUtils.ParseInt( imageElement.Attributes?[ "width" ]?.Value ) ?? 0;

                    // Image height, in pixels
                    imageHeight = NumberUtils.ParseInt( imageElement.Attributes?[ "height" ]?.Value ) ?? 0;

                    image = GetRelativeFileHandle( tmxFile, imageSource );
                }
            }

            var name       = tilesetNode.Attributes?[ "name" ]?.Value ?? string.Empty;
            var tilewidth  = NumberUtils.ParseInt( tilesetNode.Attributes?[ "tilewidth" ]?.Value ) ?? 0;
            var tileheight = NumberUtils.ParseInt( tilesetNode.Attributes?[ "tileheight" ]?.Value ) ?? 0;
            var spacing    = NumberUtils.ParseInt( tilesetNode.Attributes?[ "spacing" ]?.Value ) ?? 0;
            var margin     = NumberUtils.ParseInt( tilesetNode.Attributes?[ "margin" ]?.Value ) ?? 0;

            var offset  = tilesetNode.SelectSingleNode( "tileoffset" );
            var offsetX = 0;
            var offsetY = 0;

            if ( offset != null )
            {
                offsetX = NumberUtils.ParseInt( offset.Attributes?[ "x" ]?.Value ) ?? 0;
                offsetY = NumberUtils.ParseInt( offset.Attributes?[ "y" ]?.Value ) ?? 0;
            }

            var tileSet = new TiledMapTileSet
            {
                Name = name,
            };

            var tileSetProperties = tileSet.Properties;
            var properties        = tilesetNode.SelectSingleNode( "properties" );

            if ( properties != null )
            {
                LoadProperties( tileSetProperties, properties );
            }

            tileSetProperties.Put( "firstgid", firstgid );

            // Tiles
            var tileElements = tilesetNode.SelectNodes( "tile" );

            if ( tileElements == null )
            {
                throw new GdxRuntimeException( "Error: No tile elements found!" );
            }

            //TODO: IMPROVE THIS FORMATTING
            AddStaticTiles( tmxFile,
                            imageResolver,
                            tileSet,
                            tilesetNode,
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
                var localtid = NumberUtils.ParseInt( tileElement.Attributes?[ "id" ]?.Value ) ?? 0;
                var tile     = tileSet.GetTile( firstgid + localtid );

                if ( tile != null )
                {
                    var animatedTile = CreateAnimatedTile( tileSet, tile, tileElement, firstgid );

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
            foreach ( var animatedTile in animatedTiles )
            {
                tileSet.PutTile( animatedTile.ID, animatedTile );
            }

            Map.Tilesets.AddTileSet( tileSet );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="tmxFile"></param>
    /// <param name="imageResolver"></param>
    /// <param name="tileset"></param>
    /// <param name="node"> The XmlNode holding these tiles. </param>
    /// <param name="tileElements"></param>
    /// <param name="name"></param>
    /// <param name="firstgid"> The ID of the first tile to be added. </param>
    /// <param name="tilewidth"> Width of a tile in pixels. </param>
    /// <param name="tileheight"> Height of a tile in pixels. </param>
    /// <param name="spacing"> The spacing, in pixels, between tiles. </param>
    /// <param name="margin"></param>
    /// <param name="source"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    /// <param name="imageSource"> Filepath of the source image. </param>
    /// <param name="imageWidth"> Width of the source image in pixels. </param>
    /// <param name="imageHeight"> Height of the source image in pixels. </param>
    /// <param name="image"></param>
    protected abstract void AddStaticTiles( FileInfo tmxFile,
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
                                            FileInfo? image );

    /// <summary>
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="node"></param>
    protected void AddTileProperties( ITiledMapTile tile, XmlNode node )
    {
        string? terrain;

        if ( ( terrain = node.Attributes![ "terrain" ]!.Value ) != string.Empty )
        {
            tile.Properties.Put( "terrain", terrain );
        }

        string? probability;

        if ( ( probability = node.Attributes![ "probability" ]!.Value ) != string.Empty )
        {
            tile.Properties.Put( "probability", probability );
        }

        var properties = node.SelectSingleNode( "properties" );

        if ( properties != null )
        {
            LoadProperties( tile.Properties, properties );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="node"></param>
    protected void AddTileObjectGroup( ITiledMapTile tile, XmlNode node )
    {
        if ( node.SelectSingleNode( "objectgroup" ) != null )
        {
            var objectgroupElement = node.SelectSingleNode( "objectgroup" );

            var children = objectgroupElement?.SelectNodes( "object" );

            if ( children != null )
            {
                foreach ( XmlNode objectElement in children )
                {
                    LoadObject( Map, tile, objectElement );
                }
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="tileSet"></param>
    /// <param name="tile"></param>
    /// <param name="node"></param>
    /// <param name="firstgid"></param>
    /// <returns></returns>
    protected AnimatedTiledMapTile? CreateAnimatedTile( TiledMapTileSet tileSet,
                                                        ITiledMapTile tile,
                                                        XmlNode node,
                                                        int firstgid )
    {
        var animationElement = node.SelectSingleNode( "animation" );

        if ( animationElement == null )
        {
            return null;
        }

        var staticTiles = new List< StaticTiledMapTile? >();
        var intervals   = new List< int >();

        XmlNodeList? frames;

        if ( ( frames = animationElement.SelectNodes( "frame" ) ) == null )
        {
            return null;
        }

        foreach ( XmlNode frameElement in frames )
        {
            staticTiles.Add( ( StaticTiledMapTile ) tileSet
                                .GetTile( firstgid + int.Parse( frameElement.Attributes![ "tileid" ]!.Value ) )! );

            intervals.Add( int.Parse( frameElement.Attributes[ "duration" ]!.Value ) );
        }

        var animatedTile = new AnimatedTiledMapTile( intervals, staticTiles! )
        {
            ID = tile.ID,
        };

        return animatedTile;
    }

    /// <summary>
    /// Add a standard, non-animating, static tile to the map.
    /// </summary>
    /// <param name="tileSet"></param>
    /// <param name="textureRegion"> The tile image </param>
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
        tile.OffsetY = FlipY ? -offsetY : offsetY;

        tileSet.PutTile( tileId, tile );
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class BaseTmxLoaderParameters : AssetLoaderParameters
    {
        // generate mipmaps?
        public bool GenerateMipMaps { get; set; } = false;

        // The TextureFilter to use for minification
        public Texture.TextureFilter TextureMinFilter { get; set; } = Texture.TextureFilter.Nearest;

        // The TextureFilter to use for magnification
        public Texture.TextureFilter TextureMagFilter { get; set; } = Texture.TextureFilter.Nearest;

        /// <summary>
        /// Whether to convert the objects' pixel position and size to the equivalent in tile space.
        /// </summary>
        public bool ConvertObjectToTileSpace { get; set; } = false;

        /// <summary>
        /// Whether to flip all Y coordinates so that Y positive is up. All LibGDX renderers
        /// require flipped Y coordinates, and thus flipY set to true. This parameter is included
        /// for non-rendering related purposes of TMX files, or custom renderers.
        /// </summary>
        public bool FlipY { get; set; } = true;
    }
}

// ========================================================================
// ========================================================================

internal class MapData
{
    internal MapData( XmlNode? node )
    {
        if ( node == null )
        {
            return;
        }

        //@formatter:off
        MapVersion         = node.Attributes?[ "version" ]?.Value;
        TiledVersion       = node.Attributes?[ "tiledversion" ]?.Value;
        MapOrientation     = node.Attributes?[ "orientation" ]?.Value;
        RenderOrder        = node.Attributes?[ "renderorder" ]?.Value;
        NextLayerID        = node.Attributes?[ "nextlayerid" ]?.Value;
        NextObjectID       = node.Attributes?[ "nextobjectid" ]?.Value;
        HexSideLength      = node.Attributes?[ "hexsidelength" ]?.Value;
        StaggerAxis        = node.Attributes?[ "staggeraxis" ]?.Value;
        StaggerIndex       = node.Attributes?[ "staggerindex" ]?.Value;
        MapBackgroundColor = node.Attributes?[ "backgroundcolor" ]?.Value;
        MapWidth           = int.Parse( node.Attributes?[ "width" ]?.Value! );
        MapHeight          = int.Parse( node.Attributes?[ "height" ]?.Value! );
        TileWidth          = int.Parse( node.Attributes?[ "tilewidth" ]?.Value! );
        TileHeight         = int.Parse( node.Attributes?[ "tileheight" ]?.Value! );
        IsInfinite         = node.Attributes?[ "infinite" ]?.Value == "1";
        //@formatter:on
    }

    internal string? MapVersion         { get; set; }
    internal string? TiledVersion       { get; set; }
    internal string? MapOrientation     { get; set; }
    internal string? RenderOrder        { get; set; }
    internal string? NextLayerID        { get; set; }
    internal string? NextObjectID       { get; set; }
    internal string? HexSideLength      { get; set; }
    internal string? StaggerAxis        { get; set; }
    internal string? StaggerIndex       { get; set; }
    internal string? MapBackgroundColor { get; set; }

    internal int MapWidth   { get; set; }
    internal int MapHeight  { get; set; }
    internal int TileWidth  { get; set; }
    internal int TileHeight { get; set; }

    internal bool IsInfinite { get; set; }
}
