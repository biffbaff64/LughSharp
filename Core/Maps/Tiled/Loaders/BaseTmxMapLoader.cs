using System.Runtime.Serialization;

using LibGDXSharp.G2D;
using LibGDXSharp.Maps.Objects;
using LibGDXSharp.Maps.Tiled.Objects;
using LibGDXSharp.Maps.Tiled.Tiles;
using LibGDXSharp.Maths;

using XmlReader = LibGDXSharp.Utils.Xml.XmlReader;

namespace LibGDXSharp.Maps.Tiled;

public abstract class BaseTmxMapLoader<TP>
    : AsynchronousAssetLoader< TiledMap, TP > where TP : BaseTmxMapLoader< TP >.Parameters
{
    public class Parameters : AssetLoaderParameters
    {
        // generate mipmaps?
        internal bool GenerateMipMaps { get; set; } = false;

        // The TextureFilter to use for minification
        internal TextureFilter TextureMinFilter { get; set; } = TextureFilter.Nearest;

        // The TextureFilter to use for magnification
        internal TextureFilter TextureMagFilter { get; set; } = TextureFilter.Nearest;

        // Whether to convert the objects' pixel position and size to the equivalent in tile space.
        internal bool ConvertObjectToTileSpace { get; set; } = false;

        // Whether to flip all Y coordinates so that Y positive is up.
        // All LibGDX renderers require flipped Y coordinates, and thus flipY set to true.
        // This parameter is included for non-rendering related purposes of TMX files,
        // or custom renderers.
        internal bool FlipY { get; set; } = true;
    }

    protected const uint Flag_Flip_Horizontally = 0x80000000;
    protected const uint Flag_Flip_Vertically   = 0x40000000;
    protected const uint Flag_Flip_Diagonally   = 0x20000000;
    protected const uint Mask_Clear             = 0xE0000000;

    protected readonly XmlReader          xml = new();
    protected          XmlReader.Element? root;

    protected bool convertObjectToTileSpace;
    protected bool flipY = true;

    protected int mapTileWidth;
    protected int mapTileHeight;
    protected int mapWidthInPixels;
    protected int mapHeightInPixels;

    protected TiledMap Map { get; set; } = null!;

    protected BaseTmxMapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    public List< AssetDescriptor >? GetDependencies( string fileName, FileInfo tmxFile, TP? parameter )
    {
        this.root = xml.Parse( tmxFile );

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

        if ( root == null )
            throw new GdxRuntimeException( "Root cannot be null!" );

        var mapOrientation = root.GetAttribute( "orientation", null );

        var mapWidth   = root.GetIntAttribute( "width", 0 );
        var mapHeight  = root.GetIntAttribute( "height", 0 );
        var tileWidth  = root.GetIntAttribute( "tilewidth", 0 );
        var tileHeight = root.GetIntAttribute( "tileheight", 0 );

        var hexSideLength      = root.GetIntAttribute( "hexsidelength", 0 );
        var staggerAxis        = root.GetAttribute( "staggeraxis", null );
        var staggerIndex       = root.GetAttribute( "staggerindex", null );
        var mapBackgroundColor = root.GetAttribute( "backgroundcolor", null );

        MapProperties mapProperties = Map.Properties;

        if ( mapOrientation != null )
        {
            mapProperties.Put( "orientation", mapOrientation );
        }

        mapProperties.Put( "width", mapWidth );
        mapProperties.Put( "height", mapHeight );
        mapProperties.Put( "tilewidth", tileWidth );
        mapProperties.Put( "tileheight", tileHeight );
        mapProperties.Put( "hexsidelength", hexSideLength );

        if ( staggerAxis != null )
        {
            mapProperties.Put( "staggeraxis", staggerAxis );
        }

        if ( staggerIndex != null )
        {
            mapProperties.Put( "staggerindex", staggerIndex );
        }

        if ( mapBackgroundColor != null )
        {
            mapProperties.Put( "backgroundcolor", mapBackgroundColor );
        }

        this.mapTileWidth      = tileWidth;
        this.mapTileHeight     = tileHeight;
        this.mapWidthInPixels  = mapWidth * tileWidth;
        this.mapHeightInPixels = mapHeight * tileHeight;

        if ( mapOrientation != null )
        {
            if ( "staggered".Equals( mapOrientation ) )
            {
                if ( mapHeight > 1 )
                {
                    this.mapWidthInPixels  += tileWidth / 2;
                    this.mapHeightInPixels = ( mapHeightInPixels / 2 ) + ( tileHeight / 2 );
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

        for ( int i = 0, j = root.GetChildCount(); i < j; i++ )
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
        var name = element.Name;

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
        if ( element.GetName().equals( "group" ) )
        {
            var groupLayer = new MapGroupLayer();

            LoadBasicLayerInfo( groupLayer, element );

            XmlReader.Element properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( groupLayer.GetProperties(), properties );
            }

            for ( int i = 0, j = element.GetChildCount(); i < j; i++ )
            {
                XmlReader.Element child = element.GetChild( i );

                LoadLayer( map, groupLayer.GetLayers(), child, tmxFile, imageResolver );
            }

            foreach ( MapLayer layer in groupLayer.GetLayers() )
            {
                layer.SetParent( groupLayer );
            }

            parentLayers.Add( groupLayer );
        }
    }

    protected void LoadTileLayer( TiledMap map, MapLayers parentLayers, Utils.Xml.XmlReader.Element element )
    {
        if ( element.GetName().equals( "layer" ) )
        {
            var width      = element.GetIntAttribute( "width", 0 );
            var height     = element.GetIntAttribute( "height", 0 );
            var tileWidth  = map.GetProperties().get< int >( "tilewidth" );
            var tileHeight = map.GetProperties().get< int >( "tileheight" );
            var layer      = new TiledMapTileLayer( width, height, tileWidth, tileHeight );

            LoadBasicLayerInfo( layer, element );

            var ids      = GetTileIds( element, width, height );
            var tilesets = map.GetTileSets();

            for ( int y = 0; y < height; y++ )
            {
                for ( int x = 0; x < width; x++ )
                {
                    int  id               = ids[ y * width + x ];
                    bool flipHorizontally = ( ( id & Flag_Flip_Horizontally ) != 0 );
                    bool flipVertically   = ( ( id & Flag_Flip_Vertically ) != 0 );
                    bool flipDiagonally   = ( ( id & Flag_Flip_Diagonally ) != 0 );

                    ITiledMapTile tile = tilesets.getTile( id & ~Mask_Clear );

                    if ( tile != null )
                    {
                        TiledMapTileLayer.Cell cell = CreateTileLayerCell( flipHorizontally, flipVertically, flipDiagonally );
                        cell.setTile( tile );
                        layer.SetCell( x, flipY ? height - 1 - y : y, cell );
                    }
                }
            }

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.GetProperties(), properties );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadObjectGroup( TiledMap map, MapLayers parentLayers, Utils.Xml.XmlReader.Element element )
    {
        if ( element.GetName().equals( "objectgroup" ) )
        {
            var layer = new MapLayer();

            LoadBasicLayerInfo( layer, element );

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.GetProperties(), properties );
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
        if ( element.GetName().equals( "imagelayer" ) )
        {
            float x = 0;
            float y = 0;

            if ( element.HasAttribute( "offsetx" ) )
            {
                x = float.Parse( element.GetAttribute( "offsetx", "0" ) );
            }
            else
            {
                x = float.Parse( element.GetAttribute( "x", "0" ) );
            }

            if ( element.HasAttribute( "offsety" ) )
            {
                y = float.Parse( element.GetAttribute( "offsety", "0" ) );
            }
            else
            {
                y = float.Parse( element.GetAttribute( "y", "0" ) );
            }

            if ( flipY ) y = mapHeightInPixels - y;

            TextureRegion? texture = null;

            XmlReader.Element? image = element.GetChildByName( "image" );

            if ( image != null )
            {
                var source = image.getAttribute( "source" );
                FileInfo handle = GetRelativeFileHandle( tmxFile, source );

                texture =  imageResolver.GetImage( handle.Path() );
                y       -= texture.RegionHeight;
            }

            var layer = new TiledMapImageLayer( texture, x, y );

            LoadBasicLayerInfo( layer, element );

            XmlReader.Element? properties = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( layer.GetProperties(), properties );
            }

            parentLayers.Add( layer );
        }
    }

    protected void LoadBasicLayerInfo( MapLayer layer, XmlReader.Element element )
    {
        var name    = element.GetAttribute( "name", null );

        var opacity = float.Parse( element.GetAttribute( "opacity", "1.0" )
                                   ?? throw new NullReferenceException() );
        
        var visible = element.GetIntAttribute( "visible", 1 ) == 1;
        var offsetX = element.GetFloatAttribute( "offsetx", 0 );
        var offsetY = element.GetFloatAttribute( "offsety", 0 );

        layer.Name    = name;
        layer.Opacity = opacity;
        layer.Visible = visible;
        layer.OffsetX = offsetX;
        layer.OffsetY = offsetY;
    }

    protected void LoadObject( TiledMap map, MapLayer layer, Utils.Xml.XmlReader.Element element )
    {
        LoadObject( map, layer.Objects, element, mapHeightInPixels );
    }

    protected void LoadObject( TiledMap map, ITiledMapTile tile, Utils.Xml.XmlReader.Element element )
    {
        LoadObject( map, tile.GetObjects(), element, tile.TextureRegion.RegionHeight );
    }

    protected void LoadObject( TiledMap map, MapObjects objects, XmlReader.Element element, float heightInPixels )
    {
        if ( element.Name.Equals( "object" ) )
        {
            MapObject mapObject = null;

            float scaleX = convertObjectToTileSpace ? 1.0f / mapTileWidth : 1.0f;
            float scaleY = convertObjectToTileSpace ? 1.0f / mapTileHeight : 1.0f;

            float x = element.getFloatAttribute( "x", 0 ) * scaleX;

            float y = ( flipY ? ( heightInPixels - element.getFloatAttribute( "y", 0 ) )
                          : element.getFloatAttribute( "y", 0 ) ) * scaleY;

            float width  = element.getFloatAttribute( "width", 0 ) * scaleX;
            float height = element.getFloatAttribute( "height", 0 ) * scaleY;

            if ( element.getChildCount() > 0 )
            {
                Utils.Xml.XmlReader.Element child = null;

                if ( ( child = element.getChildByName( "polygon" ) ) != null )
                {
                    string[] points   = child.getAttribute( "points" ).split( " " );
                    float[]  vertices = new float[ points.length * 2 ];

                    for ( int i = 0; i < points.length; i++ )
                    {
                        string[] point = points[ i ].split( "," );
                        vertices[ i * 2 ]     = Float.parseFloat( point[ 0 ] ) * scaleX;
                        vertices[ i * 2 + 1 ] = Float.parseFloat( point[ 1 ] ) * scaleY * ( flipY ? -1 : 1 );
                    }

                    Polygon polygon = new Polygon( vertices );
                    polygon.setPosition( x, y );
                    mapObject = new PolygonMapObject( polygon );
                }
                else if ( ( child = element.getChildByName( "polyline" ) ) != null )
                {
                    string[] points   = child.getAttribute( "points" ).split( " " );
                    float[]  vertices = new float[ points.length * 2 ];

                    for ( int i = 0; i < points.length; i++ )
                    {
                        string[] point = points[ i ].split( "," );
                        vertices[ i * 2 ]     = Float.parseFloat( point[ 0 ] ) * scaleX;
                        vertices[ i * 2 + 1 ] = Float.parseFloat( point[ 1 ] ) * scaleY * ( flipY ? -1 : 1 );
                    }

                    Polyline polyline = new Polyline( vertices );
                    polyline.setPosition( x, y );
                    mapObject = new PolylineMapObject( polyline );
                }
                else if ( ( child = element.getChildByName( "ellipse" ) ) != null )
                {
                    mapObject = new EllipseMapObject( x, flipY ? y - height : y, width, height );
                }
            }

            if ( mapObject == null )
            {
                string gid = null;

                if ( ( gid = element.getAttribute( "gid", null ) ) != null )
                {
                    int  id               = ( int )Long.parseLong( gid );
                    bool flipHorizontally = ( ( id & Flag_Flip_Horizontally ) != 0 );
                    bool flipVertically   = ( ( id & Flag_Flip_Vertically ) != 0 );

                    TiledMapTile tile = map.getTileSets().getTile( id & ~Mask_Clear );

                    TiledMapTileMapObject tiledMapTileMapObject = new TiledMapTileMapObject
                        ( tile, flipHorizontally, flipVertically );

                    TextureRegion textureRegion = tiledMapTileMapObject.getTextureRegion();
                    tiledMapTileMapObject.getProperties().put( "gid", id );
                    tiledMapTileMapObject.setX( x );
                    tiledMapTileMapObject.setY( flipY ? y : y - height );
                    float objectWidth  = element.getFloatAttribute( "width", textureRegion.getRegionWidth() );
                    float objectHeight = element.getFloatAttribute( "height", textureRegion.getRegionHeight() );
                    tiledMapTileMapObject.setScaleX( scaleX * ( objectWidth / textureRegion.getRegionWidth() ) );
                    tiledMapTileMapObject.setScaleY( scaleY * ( objectHeight / textureRegion.getRegionHeight() ) );
                    tiledMapTileMapObject.setRotation( element.getFloatAttribute( "rotation", 0 ) );
                    mapObject = tiledMapTileMapObject;
                }
                else
                {
                    mapObject = new RectangleMapObject( x, flipY ? y - height : y, width, height );
                }
            }

            mapObject.setName( element.getAttribute( "name", null ) );
            string rotation = element.getAttribute( "rotation", null );

            if ( rotation != null )
            {
                mapObject.getProperties().put( "rotation", Float.parseFloat( rotation ) );
            }

            string type = element.getAttribute( "type", null );

            if ( type != null )
            {
                mapObject.getProperties().put( "type", type );
            }

            int id = element.getIntAttribute( "id", 0 );

            if ( id != 0 )
            {
                mapObject.getProperties().put( "id", id );
            }

            mapObject.getProperties().put( "x", x );

            if ( mapObject instanceof tiledMapTileMapObject) {
                mapObject.getProperties().put( "y", y );
            } else {
                mapObject.getProperties().put( "y", ( flipY ? y - height : y ) );
            }

            mapObject.getProperties().put( "width", width );
            mapObject.getProperties().put( "height", height );
            mapObject.setVisible( element.getIntAttribute( "visible", 1 ) == 1 );
            
            XmlReader.Element properties = element.getChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( object.getProperties(), properties );
            }

            mapObject.add( object );
        }
    }

    protected void LoadProperties( MapProperties properties, Utils.Xml.XmlReader.Element element )
    {
        if ( element == null ) return;

        if ( element.getName().equals( "properties" ) )
        {
            for ( Utils.Xml.XmlReader.Element property :
            element.getChildrenByName( "property" )) {
                string name  = property.getAttribute( "name", null );
                string value = property.getAttribute( "value", null );
                string type  = property.getAttribute( "type", null );

                if ( value == null )
                {
                    value = property.getText();
                }

                Object castValue = CastProperty( name, value, type );
                properties.put( name, castValue );
            }
        }
    }

    protected Object CastProperty( string name, string value, string type )
    {
        if ( type == null )
        {
            return value;
        }
        else if ( type.equals( "int" ) )
        {
            return Integer.valueOf( value );
        }
        else if ( type.equals( "float" ) )
        {
            return Float.valueOf( value );
        }
        else if ( type.equals( "bool" ) )
        {
            return bool.valueOf( value );
        }
        else if ( type.equals( "color" ) )
        {
            // Tiled uses the format #AARRGGBB
            string opaqueColor = value.substring( 3 );
            string alpha       = value.substring( 1, 3 );

            return Color.valueOf( opaqueColor + alpha );
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

    protected TiledMapTileLayer.Cell CreateTileLayerCell( bool flipHorizontally, bool flipVertically, bool flipDiagonally )
    {
        var cell = new TiledMapTileLayer.Cell();

        if ( flipDiagonally )
        {
            if ( flipHorizontally && flipVertically )
            {
                cell.setFlipHorizontally( true );
                cell.setRotation( TiledMapTileLayer.Cell.ROTATE_270 );
            }
            else if ( flipHorizontally )
            {
                cell.setRotation( TiledMapTileLayer.Cell.ROTATE_270 );
            }
            else if ( flipVertically )
            {
                cell.setRotation( Cell.ROTATE_90 );
            }
            else
            {
                cell.setFlipVertically( true );
                cell.setRotation( TiledMapTileLayer.Cell.ROTATE_270 );
            }
        }
        else
        {
            cell.setFlipHorizontally( flipHorizontally );
            cell.setFlipVertically( flipVertically );
        }

        return cell;
    }

    public int[] GetTileIds( Utils.Xml.XmlReader.Element element, int width, int height )
    {
        Utils.Xml.XmlReader.Element data     = element.GetChildByName( "data" );
        string                      encoding = data.GetAttribute( "encoding", null );

        if ( encoding == null )
        {
            // no 'encoding' attribute means that the encoding is XML
            throw new GdxRuntimeException( "Unsupported encoding (XML) for TMX Layer Data" );
        }

        var ids = new int[ width * height ];

        if ( encoding.Equals( "csv" ) )
        {
            var array = data.Text?.Split( "," );

            for ( int i = 0; i < array.length; i++ )
            {
                ids[ i ] = ( int )Long.parseLong( array[ i ].trim() );
            }
        }
        else
        {
            if ( true )
                if ( encoding.equals( "base64" ) )
                {
                    InputStream is  = null;

                    try
                    {
                        var compression = data.GetAttribute( "compression", null );
                        var bytes       = System.Convert.FromBase64String( data.ToString() );

                        if ( compression == null )
                            is = new ByteArrayInputStream( bytes );
                        else if ( compression.equals( "gzip" ) )
                            is = new BufferedInputStream
                            ( new GZIPInputStream( new ByteArrayInputStream( bytes ), bytes.length ) );

                        else if ( compression.equals( "zlib" ) )
                            is = new BufferedInputStream( new InflaterInputStream( new ByteArrayInputStream( bytes ) ) );
                        else

                        throw new GdxRuntimeException( "Unrecognised compression (" + compression + ") for TMX Layer Data" );

                        byte[] temp = new byte[ 4 ];

                        for ( int y = 0; y < height; y++ )
                        {
                            for ( int x = 0; x < width; x++ )
                            {
                                int read = is.read( temp );

                                while ( read < temp.length )
                                {
                                    int curr = is.read( temp, read, temp.length - read );

                                    if ( curr == -1 ) break;
                                    read += curr;
                                }

                                if ( read != temp.length )
                                    throw new GdxRuntimeException( "Error Reading TMX Layer Data: Premature end of tile data" );

                                ids[ y * width + x ] = UnsignedByteToInt( temp[ 0 ] )
                                                       | UnsignedByteToInt( temp[ 1 ] ) << 8
                                                       | UnsignedByteToInt( temp[ 2 ] ) << 16
                                                       | UnsignedByteToInt( temp[ 3 ] ) << 24;
                            }
                        }
                    }
                    catch ( IOException e )
                    {
                        throw new GdxRuntimeException( "Error Reading TMX Layer Data - IOException: " + e.getMessage() );
                    }
                    finally
                    {
                        StreamUtils.closeQuietly(  is);
                    }
                }
                else
                {
                    // any other value of 'encoding' is one we're not aware of, probably a feature of a future version of Tiled
                    // or another editor
                    throw new GdxRuntimeException( "Unrecognised encoding (" + encoding + ") for TMX Layer Data" );
                }
        }

        return ids;
    }

    protected static int UnsignedByteToInt( byte b )
    {
        return b & 0xFF;
    }

    protected static FileInfo GetRelativeFileHandle( FileInfo file, string path )
    {
        StringTokenizer tokenizer = new StringTokenizer( path, "\\/" );
        FileInfo        result    = file.parent();

        while ( tokenizer.hasMoreElements() )
        {
            string token = tokenizer.nextToken();

            if ( token.equals( ".." ) )
                result = result.parent();
            else
            {
                result = result.child( token );
            }
        }

        return result;
    }

    protected void LoadTileSet( XmlReader.Element element, FileInfo tmxFile, IImageResolver imageResolver )
    {
        if ( element.GetName().equals( "tileset" ) )
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
            var offsetX = 0;
            var offsetY = 0;

            if ( offset != null )
            {
                offsetX = offset.GetIntAttribute( "x", 0 );
                offsetY = offset.GetIntAttribute( "y", 0 );
            }

            var tileSet = new TiledMapTileSet();

            tileSet.Name = name;

            var tileSetProperties = tileSet.GetProperties();
            XmlReader.Element? properties        = element.GetChildByName( "properties" );

            if ( properties != null )
            {
                LoadProperties( tileSetProperties, properties );
            }

            tileSetProperties.Put( "firstgid", firstgid );

            // Tiles
            List< XmlReader.Element > tileElements = element.GetChildrenByName( "tile" );

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

            List< AnimatedTiledMapTile > animatedTiles = new List< AnimatedTiledMapTile >();

            foreach ( XmlReader.Element tileElement in tileElements )
            {
                var localtid = tileElement.GetIntAttribute( "id", 0 );
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
                tileSet.PutTile( animatedTile.GetId(), animatedTile );
            }

            Map.GetTileSets().addTileSet( tileSet );
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
                                            string source,
                                            int offsetX,
                                            int offsetY,
                                            string imageSource,
                                            int imageWidth,
                                            int imageHeight,
                                            FileInfo image );

    protected void AddTileProperties( ITiledMapTile tile, XmlReader.Element tileElement )
    {
        string? terrain = tileElement.GetAttribute( "terrain", null );

        if ( terrain != null )
        {
            tile.GetProperties().Put( "terrain", terrain );
        }

        string? probability = tileElement.GetAttribute( "probability", null );

        if ( probability != null )
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
        XmlReader.Element? objectgroupElement = tileElement.GetChildByName( "objectgroup" );

        if ( objectgroupElement != null )
        {
            foreach ( XmlReader.Element objectElement in objectgroupElement.GetChildrenByName( "object" ) )
            {
                LoadObject( Map, tile, objectElement );
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
            var staticTiles = new List< StaticTiledMapTile >();
            var intervals   = new List< int >();

            foreach ( XmlReader.Element frameElement in animationElement.GetChildrenByName( "frame" ) )
            {
                staticTiles.Add
                    (
                    ( StaticTiledMapTile )tileSet.GetTile
                        ( firstgid + frameElement.GetIntAttribute( "tileid" ) )!
                    );

                intervals.Add( frameElement.GetIntAttribute( "duration" ) );
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
                                          TextureRegion textureRegion,
                                          int tileId,
                                          float offsetX,
                                          float offsetY )
    {
        ITiledMapTile tile = new StaticTiledMapTile( textureRegion );

        tile.ID      = tileId;
        tile.OffsetX = offsetX;
        tile.OffsetY = flipY ? -offsetY : offsetY;

        tileSet.PutTile( tileId, tile );
    }
}
