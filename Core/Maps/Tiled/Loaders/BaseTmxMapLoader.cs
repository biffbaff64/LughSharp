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

using System.IO.Compression;
using System.Runtime.Serialization;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using LibGDXSharp.G2D;
using LibGDXSharp.Maps.Objects;
using LibGDXSharp.Maps.Tiled.Objects;
using LibGDXSharp.Maps.Tiled.Tiles;
using LibGDXSharp.Maths;
using LibGDXSharp.Utils.Xml;

namespace LibGDXSharp.Maps.Tiled;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class BaseTmxMapLoader<TP>
    : AsynchronousAssetLoader< TiledMap, TP > where TP : BaseTmxMapLoader< TP >.Parameters
{
    public class Parameters : AssetLoaderParameters
    {
        // generate mipmaps?
        public bool GenerateMipMaps { get; set; } = false;

        // The TextureFilter to use for minification
        public TextureFilter TextureMinFilter { get; set; } = TextureFilter.Nearest;

        // The TextureFilter to use for magnification
        public TextureFilter TextureMagFilter { get; set; } = TextureFilter.Nearest;

        // Whether to convert the objects' pixel position and size to the equivalent in tile space.
        public bool ConvertObjectToTileSpace { get; set; } = false;

        // Whether to flip all Y coordinates so that Y positive is up.
        // All LibGDX renderers require flipped Y coordinates, and thus flipY set to true.
        // This parameter is included for non-rendering related purposes of TMX files,
        // or custom renderers.
        public bool FlipY { get; set; } = true;
    }

    protected const uint FLAG_FLIP_HORIZONTALLY = 0x80000000;
    protected const uint FLAG_FLIP_VERTICALLY   = 0x40000000;
    protected const uint FLAG_FLIP_DIAGONALLY   = 0x20000000;
    protected const uint MASK_CLEAR             = 0xE0000000;

    protected readonly XmlReader         xml = new();
    protected          XmlReader.Element root;

    protected bool convertObjectToTileSpace;
    protected bool flipY = true;

    public    int      MapTileWidth      { get; set; }
    public    int      MapTileHeight     { get; set; }
    public    int      MapWidthInPixels  { get; set; }
    public    int      MapHeightInPixels { get; set; }
    protected TiledMap Map               { get; set; } = null!;

    protected BaseTmxMapLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
        root = default!;
    }

    public List< AssetDescriptor >? GetDependencies( string fileName, FileInfo tmxFile, TP? parameter )
    {
        var textureParameter = new TextureLoader.TextureParameter();

        if ( parameter != null )
        {
            textureParameter.GenMipMaps = parameter.GenerateMipMaps;
            textureParameter.MinFilter  = parameter.TextureMinFilter;
            textureParameter.MagFilter  = parameter.TextureMagFilter;
        }

        return GetDependencyAssetDescriptors( tmxFile, textureParameter );
    }

    protected List< AssetDescriptor >? GetDependencyAssetDescriptors( FileInfo tmxFile,
                                                                      TextureLoader.TextureParameter textureParameter )
    {
        return default;
    }

    /// <summary>
    /// Loads the map data, given the XML root element
    /// </summary>
    /// <param name="tmxFile">The Filehandle of the tmx file </param>
    /// <param name="parameter"></param>
    /// <param name="imageResolver"></param>
    /// <returns>The <see cref="TiledMap"/>.</returns>
    protected TiledMap LoadTiledMap( FileInfo tmxFile, TP? parameter, IImageResolver imageResolver )
    {
        this.Map = new TiledMap();

        if ( parameter != null )
        {
            this.convertObjectToTileSpace = parameter.ConvertObjectToTileSpace;
            this.flipY                    = parameter.FlipY;
        }
        else
        {
            this.convertObjectToTileSpace = false;
            this.flipY                    = true;
        }

        if ( root == null ) throw new GdxRuntimeException( "Root cannot be null!" );

        var mapOrientation = root.GetAttribute( "orientation", null );

        var mapWidth      = int.Parse( root.GetAttribute( "width" ) );
        var mapHeight     = int.Parse( root.GetAttribute( "height" ) );
        var tileWidth     = int.Parse( root.GetAttribute( "tilewidth" ) );
        var tileHeight    = int.Parse( root.GetAttribute( "tileheight" ) );
        var hexSideLength = int.Parse( root.GetAttribute( "hexsidelength" ) );

        var staggerAxis        = root.GetAttribute( "staggeraxis", null );
        var staggerIndex       = root.GetAttribute( "staggerindex", null );
        var mapBackgroundColor = root.GetAttribute( "backgroundcolor", null );

        MapProperties mapProperties = Map.Properties;

        if ( mapOrientation != string.Empty )
        {
            mapProperties.Put( "orientation", mapOrientation );
        }

        mapProperties.Put( "width", mapWidth );
        mapProperties.Put( "height", mapHeight );
        mapProperties.Put( "tilewidth", tileWidth );
        mapProperties.Put( "tileheight", tileHeight );
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

        this.MapTileWidth      = tileWidth;
        this.MapTileHeight     = tileHeight;
        this.MapWidthInPixels  = mapWidth * tileWidth;
        this.MapHeightInPixels = mapHeight * tileHeight;

        if ( mapOrientation != string.Empty )
        {
            if ( "staggered".Equals( mapOrientation ) )
            {
                if ( mapHeight > 1 )
                {
                    this.MapWidthInPixels  += tileWidth / 2;
                    this.MapHeightInPixels =  ( MapHeightInPixels / 2 ) + ( tileHeight / 2 );
                }
            }
        }

        XmlReader.Element? properties = root.GetChildByName( "properties" );

        if ( properties != null )
        {
            LoadProperties( Map.Properties, properties );
        }

        List< XmlReader.Element > tilesets = root.GetChildrenByName( "tileset" );

        foreach ( XmlReader.Element element in tilesets )
        {
            LoadTileSet( element, tmxFile, imageResolver );
            root.RemoveChild( element );
        }

        for ( int i = 0, j = root.ChildCount; i < j; i++ )
        {
            XmlReader.Element element = root.GetChild( i );

            LoadLayer( Map, Map.Layers, element, tmxFile, imageResolver );
        }

        return Map;
    }

    protected void LoadLayer( TiledMap map,
                              MapLayers parentLayers,
                              XmlReader.Element element,
                              FileInfo tmxFile,
                              IImageResolver imageResolver )
    {
        ArgumentNullException.ThrowIfNull( element );

        var name = element.Name;

        if ( name == null ) throw new GdxRuntimeException( $"{name} cannot be null!" );

        if ( name.Equals( "group" ) )
        {
            LoadLayerGroup( map, parentLayers, element, tmxFile, imageResolver );
        }
        else if ( name.Equals( "layer" ) )
        {
            LoadTileLayer( map, parentLayers, element );
        }
        else if ( name.Equals( "objectgroup" ) )
        {
            LoadObjectGroup( map, parentLayers, element );
        }
        else if ( name.Equals( "imagelayer" ) )
        {
            LoadImageLayer( map, parentLayers, element, tmxFile, imageResolver );
        }
    }

    protected void LoadLayerGroup( TiledMap map,
                                   MapLayers parentLayers,
                                   XmlReader.Element element,
                                   FileInfo tmxFile,
                                   IImageResolver imageResolver )
    {
        ArgumentNullException.ThrowIfNull( element );

        if ( element.Name!.Equals( "group" ) )
        {
            var groupLayer = new MapGroupLayer();

            LoadBasicLayerInfo( groupLayer, element );

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( groupLayer.Properties, properties );
            }

            for ( int i = 0, j = element.ChildCount; i < j; i++ )
            {
                XmlReader.Element child = element.GetChild( i );

                LoadLayer( map, groupLayer.Layers, child, tmxFile, imageResolver );
            }

            foreach ( MapLayer layer in groupLayer.Layers )
            {
                layer.Parent = groupLayer;
            }

            parentLayers.Add( groupLayer );
        }
    }

    protected void LoadTileLayer( TiledMap map, MapLayers parentLayers, XmlReader.Element element )
    {
        if ( element.Name == null ) throw new ArgumentException( "element cannot by null!" );

        if ( element.Name.Equals( "layer" ) )
        {
            var width      = element.GetIntAttribute( "width", 0 );
            var height     = element.GetIntAttribute( "height", 0 );
            var tileWidth  = map.Properties.Get< int >( "tilewidth" );
            var tileHeight = map.Properties.Get< int >( "tileheight" );
            var layer      = new TiledMapTileLayer( width, height, tileWidth, tileHeight );

            LoadBasicLayerInfo( layer, element );

            var ids      = GetTileIDs( element, width, height );
            var tilesets = map.Tilesets;

            for ( var y = 0; y < height; y++ )
            {
                for ( var x = 0; x < width; x++ )
                {
                    var id               = ids[ ( y * width ) + x ];
                    var flipHorizontally = ( ( id & FLAG_FLIP_HORIZONTALLY ) != 0 );
                    var flipVertically   = ( ( id & FLAG_FLIP_VERTICALLY ) != 0 );
                    var flipDiagonally   = ( ( id & FLAG_FLIP_DIAGONALLY ) != 0 );

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

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadObjectGroup( TiledMap map, MapLayers parentLayers, XmlReader.Element element )
    {
        if ( element.Name == null ) throw new ArgumentException( "element cannot by null!" );

        if ( element.Name.Equals( "objectgroup" ) )
        {
            var layer = new MapLayer();

            LoadBasicLayerInfo( layer, element );

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            foreach ( XmlReader.Element objectElement in element.GetChildrenByName( "object" ) )
            {
                LoadObject( map, layer, objectElement );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadImageLayer( TiledMap map,
                                   MapLayers parentLayers,
                                   XmlReader.Element element,
                                   FileInfo tmxFile,
                                   IImageResolver imageResolver )
    {
        if ( element.Name == null ) throw new ArgumentException( "element cannot by null!" );

        if ( element.Name.Equals( "imagelayer" ) )
        {
            var x = float.Parse
                (
                element.HasAttribute( "offsetx" )
                    ? element.GetAttribute( "offsetx", "0" )!
                    : element.GetAttribute( "x", "0" )!
                );

            var y = float.Parse
                (
                element.HasAttribute( "offsety" )
                    ? element.GetAttribute( "offsety", "0" )!
                    : element.GetAttribute( "y", "0" )!
                );

            if ( flipY ) y = MapHeightInPixels - y;

            TextureRegion? texture = null;

            XmlReader.Element? image = element.GetChildByName( "image" );

            if ( image != null )
            {
                var      source = image.GetAttribute( "source" );
                FileInfo handle = GetRelativeFileHandle( tmxFile, source );

                texture = imageResolver.GetImage( handle.FullName );

                if ( texture == null ) throw new GdxRuntimeException( "Image Texture cannot be null!" );

                y -= texture.RegionHeight;
            }

            var layer = new TiledMapImageLayer( texture, x, y );

            LoadBasicLayerInfo( layer, element );

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.Properties, properties );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadBasicLayerInfo( MapLayer layer, XmlReader.Element element )
    {
        var name = element.GetAttribute( "name", null );

        var opacity = float.Parse
            (
            element.GetAttribute( "opacity", "1.0" )
            ?? throw new NullReferenceException()
            );

        var visible = element.GetIntAttribute( "visible", 1 ) == 1;
        var offsetX = element.GetFloatAttribute( "offsetx", 0 );
        var offsetY = element.GetFloatAttribute( "offsety", 0 );

        layer.Name    = name;
        layer.Opacity = opacity;
        layer.Visible = visible;
        layer.OffsetX = offsetX;
        layer.OffsetY = offsetY;
    }

    protected void LoadObject( TiledMap map, MapLayer layer, XmlReader.Element element )
    {
        LoadObject( map, layer.Objects, element, MapHeightInPixels );
    }

    protected void LoadObject( TiledMap map, ITiledMapTile tile, XmlReader.Element element )
    {
        LoadObject( map, tile.GetObjects(), element, tile.TextureRegion.RegionHeight );
    }

    protected void LoadObject( TiledMap map, MapObjects objects, XmlReader.Element element, float heightInPixels )
    {
        if ( element.Name == null ) throw new ArgumentException( "element cannot by null!" );

        if ( element.Name.Equals( "object" ) )
        {
            MapObject? mapObject = null;

            var scaleX = convertObjectToTileSpace ? 1.0f / MapTileWidth : 1.0f;
            var scaleY = convertObjectToTileSpace ? 1.0f / MapTileHeight : 1.0f;

            var x = element.GetFloatAttribute( "x", 0 ) * scaleX;

            var y = ( flipY
                        ? ( heightInPixels - element.GetFloatAttribute( "y", 0 ) )
                        : element.GetFloatAttribute( "y", 0 ) )
                    * scaleY;

            var width  = element.GetFloatAttribute( "width", 0 ) * scaleX;
            var height = element.GetFloatAttribute( "height", 0 ) * scaleY;

            if ( element.ChildCount > 0 )
            {
                XmlReader.Element? child;

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
                        string[] point = points[ i ].Split( "," );
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

                    var flipHorizontally = ( ( id & FLAG_FLIP_HORIZONTALLY ) != 0 );
                    var flipVertically   = ( ( id & FLAG_FLIP_VERTICALLY ) != 0 );

                    ITiledMapTile? tile = map.Tilesets.GetTile( ( int )( id & ~MASK_CLEAR ) );

                    var tiledMapTileMapObject = new TiledMapTileMapObject( tile!, flipHorizontally, flipVertically );

                    TextureRegion textureRegion = tiledMapTileMapObject.TextureRegion!;

                    tiledMapTileMapObject.Properties.Put( "gid", id );
                    tiledMapTileMapObject.X = x;
                    tiledMapTileMapObject.Y = ( flipY ? y : y - height );

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
                mapObject.Properties.Put( "y", ( flipY ? y - height : y ) );
            }

            mapObject.Properties.Put( "width", width );
            mapObject.Properties.Put( "height", height );
            mapObject.Visible = ( element.GetIntAttribute( "visible", 1 ) == 1 );

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( mapObject.Properties, properties );
            }

            objects.Add( mapObject );
        }
    }

    protected void LoadProperties( MapProperties properties, XmlReader.Element? element )
    {
        if ( ( element == null ) || ( element.Name == null ) ) return;

        if ( element.Name.Equals( "properties" ) )
        {
            foreach ( XmlReader.Element property in element.GetChildrenByName( "property" ) )
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
        if ( type == null )
        {
            return value;
        }
        else if ( type.Equals( "int" ) )
        {
            return int.Parse( value! );
        }
        else if ( type.Equals( "float" ) )
        {
            return float.Parse( value! );
        }
        else if ( type.Equals( "bool" ) )
        {
            return bool.Parse( value! );
        }
        else if ( type.Equals( "color" ) || type.Equals( "colour" ) )
        {
            // Tiled uses the format #AARRGGBB
            var opaqueColor = value?.Substring( 3 );
            var alpha       = value?.Substring( 1, 3 );

            return Color.ValueOf( opaqueColor + alpha );
        }
        else
        {
            throw new GdxRuntimeException
                (
                "Wrong type given for property "
                + name
                + ", given : "
                + type
                + ", supported : string, bool, int, float, color"
                );
        }
    }

    protected TiledMapTileLayer.Cell CreateTileLayerCell( bool flipHorizontally,
                                                          bool flipVertically,
                                                          bool flipDiagonally )
    {
        var cell = new TiledMapTileLayer.Cell();

        if ( flipDiagonally )
        {
            if ( flipHorizontally && flipVertically )
            {
                cell.SetFlipHorizontally( true );
                cell.SetRotation( TiledMapTileLayer.Cell.ROTATE270 );
            }
            else if ( flipHorizontally )
            {
                cell.SetRotation( TiledMapTileLayer.Cell.ROTATE270 );
            }
            else if ( flipVertically )
            {
                cell.SetRotation( TiledMapTileLayer.Cell.ROTATE90 );
            }
            else
            {
                cell.SetFlipVertically( true );
                cell.SetRotation( TiledMapTileLayer.Cell.ROTATE270 );
            }
        }
        else
        {
            cell.SetFlipHorizontally( flipHorizontally );
            cell.SetFlipVertically( flipVertically );
        }

        return cell;
    }

    public int[] GetTileIDs( Utils.Xml.XmlReader.Element element, int width, int height )
    {
        XmlReader.Element? data = element.GetChildByName( "data" );

        if ( data == null ) throw new GdxRuntimeException( "data is missing" );

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
                if ( encoding.Equals( "base64" ) )
                {
                    Stream inputStream = null!;

                    try
                    {
                        var compression = data.GetAttribute( "compression", null );
                        var bytes       = System.Convert.FromBase64String( data.ToString() );

                        if ( compression == null )
                        {
                            inputStream = new MemoryStream( bytes );
                        }
                        else if ( compression.Equals( "gzip" ) )
                        {
                            inputStream = new BufferedStream
                                ( new GZipStream( new MemoryStream( bytes ), CompressionMode.Decompress ) );
                        }
                        else if ( compression.Equals( "zlib" ) )
                        {
                            inputStream = new BufferedStream
                                ( new InflaterInputStream( new MemoryStream( bytes ) ) );
                        }
                        else
                        {
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

                                    if ( curr == -1 ) break;
                                    read += curr;
                                }

                                if ( read != temp.Length )
                                {
                                    throw new GdxRuntimeException
                                        ( "Error Reading TMX Layer Data: Premature end of tile data" );
                                }

                                ids[ ( y * width ) + x ] = UnsignedByteToInt( temp[ 0 ] )
                                                           | ( UnsignedByteToInt( temp[ 1 ] ) << 8 )
                                                           | ( UnsignedByteToInt( temp[ 2 ] ) << 16 )
                                                           | ( UnsignedByteToInt( temp[ 3 ] ) << 24 );
                            }
                        }
                    }
                    catch ( IOException e )
                    {
                        throw new GdxRuntimeException( "Error Reading TMX Layer Data - IOException: " + e.Message );
                    }
                    finally
                    {
                        if ( inputStream != null )
                        {
                            inputStream.Close();
                        }
                    }
                }
                else
                {
                    // any other value of 'encoding' is one we're not aware of, probably
                    // a feature of a future version of Tiled or another editor
                    throw new GdxRuntimeException( "Unrecognised encoding (" + encoding + ") for TMX Layer Data" );
                }
        }

        return ids;
    }

    protected static int UnsignedByteToInt( byte b )
    {
        return b & 0xFF;
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

    protected void LoadTileSet( XmlReader.Element element, FileInfo tmxFile, IImageResolver imageResolver )
    {
        if ( element.Name!.Equals( "tileset" ) )
        {
            var firstgid    = element.GetIntAttribute( "firstgid", 1 );
            var imageSource = "";
            var imageWidth  = 0;
            var imageHeight = 0;

            FileInfo? image = null;

            var source = element.GetAttribute( "source", null );

            if ( source != null )
            {
                FileInfo tsx = GetRelativeFileHandle( tmxFile, source );

                try
                {
                    element = xml.Parse( tsx );

                    XmlReader.Element? imageElement = element.GetChildByName( "image" );

                    if ( imageElement != null )
                    {
                        imageSource = imageElement.GetAttribute( "source" );
                        imageWidth  = imageElement.GetIntAttribute( "width", 0 );
                        imageHeight = imageElement.GetIntAttribute( "height", 0 );
                        image       = GetRelativeFileHandle( tsx, imageSource );
                    }
                }
                catch ( SerializationException e )
                {
                    throw new GdxRuntimeException( "Error parsing external tileset." );
                }
            }
            else
            {
                XmlReader.Element? imageElement = element.GetChildByName( "image" );

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

            XmlReader.Element? offset  = element.GetChildByName( "tileoffset" );
            var                offsetX = 0;
            var                offsetY = 0;

            if ( offset != null )
            {
                offsetX = offset.GetIntAttribute( "x", 0 );
                offsetY = offset.GetIntAttribute( "y", 0 );
            }

            var tileSet = new TiledMapTileSet
            {
                Name = name
            };

            MapProperties      tileSetProperties = tileSet.Properties;
            XmlReader.Element? properties        = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( tileSetProperties, properties );
            }

            tileSetProperties.Put( "firstgid", firstgid );

            // Tiles
            List< XmlReader.Element > tileElements = element.GetChildrenByName( "tile" );

            //TODO: IMPROVE THIS FORMATTING
            AddStaticTiles
                (
                tmxFile,
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
                image
                );

            var animatedTiles = new List< AnimatedTiledMapTile >();

            foreach ( XmlReader.Element tileElement in tileElements )
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
                                            FileInfo? image );

    protected void AddTileProperties( ITiledMapTile tile, XmlReader.Element tileElement )
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

        XmlReader.Element? properties = tileElement.GetChildByName( "properties" );

        if ( properties != null )
        {
            LoadProperties( tile.GetProperties(), properties );
        }
    }

    protected void AddTileObjectGroup( ITiledMapTile tile, XmlReader.Element tileElement )
    {
        if ( tileElement.HasAttribute( "objectgroup" ) )
        {
            XmlReader.Element? objectgroupElement = tileElement.GetChildByName( "objectgroup" );

            if ( objectgroupElement != null )
            {
                List< XmlReader.Element > children = objectgroupElement.GetChildrenByName( "object" );

                foreach ( XmlReader.Element objectElement in children )
                {
                    LoadObject( Map, tile, objectElement );
                }
            }
        }
    }

    protected AnimatedTiledMapTile? CreateAnimatedTile( TiledMapTileSet tileSet,
                                                        ITiledMapTile tile,
                                                        XmlReader.Element tileElement,
                                                        int firstgid )
    {
        XmlReader.Element? animationElement = tileElement.GetChildByName( "animation" );

        if ( animationElement != null )
        {
            var                       staticTiles = new List< StaticTiledMapTile >();
            var                       intervals   = new List< int >();
            List< XmlReader.Element > frames      = animationElement.GetChildrenByName( "frame" );

            foreach ( XmlReader.Element frameElement in frames )
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
}