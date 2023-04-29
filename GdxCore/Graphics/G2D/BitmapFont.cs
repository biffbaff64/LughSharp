using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.G2D
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public sealed class BitmapFont
    {
        private const string FontName       = "Resources/arial-15.fnt";
        private const int    Log2_Page_Size = 9;
        private const int    Page_Size      = 1 << Log2_Page_Size;
        private const int    Pages          = 0x10000 / Page_Size;

        public bool Flipped { get; set; }

        private readonly BitmapFontData        _data;
        private readonly List< TextureRegion > _regions;
        private readonly BitmapFontCache       _cache;

        private bool _integer;
        private bool _ownsTexture;

        /// <summary>
        /// Creates a BitmapFont using the default 15pt Arial font included in the library.
        /// This is convenient to easily display text without bothering without generating
        /// a bitmap font yourself.
        /// </summary>
        public BitmapFont()
            : this
                (
                 Gdx.Files.Internal( FontName ),
                 Gdx.Files.Internal( FontName ),
                 false,
                 true
                )
        {
        }

        /// <summary>
        /// Creates a BitmapFont using the default 15pt Arial font included in the
        /// libGDXSharp project.
        /// <para>
        /// This is convenient to easily display text without bothering without generating a
        /// bitmap font yourself.
        /// </para>
        /// </summary>
        /// <param name="flip">
        /// If true, the glyphs will be flipped for use with a perspective where 0,0 is
        /// the upper left corner.
        /// </param>
        public BitmapFont( bool flip )
            : this( Gdx.Files.Internal( FontName ), Gdx.Files.Internal( FontName ), flip )
        {
        }

        /// <summary>
        /// Creates a BitmapFont with the glyphs relative to the specified region.
        /// If the region is null, the glyph textures are loaded from the image file
        /// given in the font file. The Dispose() method will not dispose the region's
        /// texture in this case!
        /// </summary>
        /// <param name="fontFile"> the font definition file.</param>
        /// <param name="region">
        /// The texture region containing the glyphs. The glyphs must be relative to
        /// the lower left corner (ie, the region should not be flipped). If the region
        /// is null the glyph images are loaded from the image path in the font file.
        /// </param>
        /// <param name="flip">
        /// If true, the glyphs will be flipped for use with a perspective where 0,0
        /// is the upper left corner.
        /// </param>
        public BitmapFont( FileInfo fontFile, TextureRegion region, bool flip = false )
            : this( new BitmapFontData( fontFile, flip ), region, true )
        {
        }

        /// <summary>
        /// Creates a BitmapFont from a BMFont file. The image file name is read from
        /// the BMFont file and the image is loaded from the same directory.
        /// </summary>
        /// <param name="fontFile"> the font definition file.</param>
        /// <param name="flip">
        /// If true, the glyphs will be flipped for use with a perspective where 0,0
        /// is the upper left corner.
        /// </param>
        public BitmapFont( FileInfo fontFile, bool flip = false )
            : this( new BitmapFontData( fontFile, flip ), ( TextureRegion? )null, true )
        {
        }

        /// <summary>
        /// Creates a BitmapFont from a BMFont file, using the specified image for
        /// glyphs. Any image specified in the BMFont file is ignored.
        /// </summary>
        /// <param name="fontFile"> the font definition file.</param>
        /// <param name="imageFile"></param>
        /// <param name="flip">
        /// If true, the glyphs will be flipped for use with a perspective where
        /// 0,0 is the upper left corner.
        /// </param>
        /// <param name="integer"></param>
        public BitmapFont( FileInfo fontFile, FileInfo imageFile, bool flip, bool integer = true )
            : this
                (
                 new BitmapFontData( fontFile, flip ),
                 new TextureRegion( new Texture( imageFile, false ) ),
                 integer
                )
        {
            _ownsTexture = true;
        }

        /// <summary>
        /// Constructs a new BitmapFont from the given <see cref="BitmapFontData"/> and
        /// <see cref="TextureRegion"/>. If the TextureRegion is null, the image path(s)
        /// will be read from the BitmapFontData.
        /// <para>
        /// The dispose() method will not dispose the texture of the region(s) if the
        /// region is != null.
        /// </para>
        /// <para>
        /// Passing a single TextureRegion assumes that your font only needs a single
        /// texture page. If you need to support multiple pages, either let the Font read
        /// the images themselves (by specifying null as the TextureRegion), or by
        /// specifying each page manually with the TextureRegion[] constructor.
        /// </para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="region"></param>
        /// <param name="integer">
        /// If true, rendering positions will be at integer values to avoid filtering
        /// artifacts.
        /// </param>
        public BitmapFont( BitmapFontData data, TextureRegion? region, bool integer )
            : this( data, region != null ? ListExtensions.With( region ) : null, integer )
        {
        }

        /// <summary>
        /// Constructs a new BitmapFont from the given <see cref="BitmapFontData"/> and array
        /// of <see cref="TextureRegion"/>. If the TextureRegion is null or empty, the image
        /// path(s) will be read from the BitmapFontData. The dispose() method will not dispose
        /// the texture of the region(s) if the regions array is != null and not empty.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pageRegions"></param>
        /// <param name="integer">
        /// If true, rendering positions will be at integer values to avoid filtering artifacts.
        /// </param>
        public BitmapFont( BitmapFontData data, List< TextureRegion >? pageRegions, bool integer )
        {
            this.Flipped  = data.Flipped;
            this._data    = data;
            this._integer = integer;

            if ( ( pageRegions == null ) || ( pageRegions.Count == 0 ) )
            {
                if ( data.ImagePaths == null )
                {
                    throw new ArgumentException
                        ( "If no regions are specified, the font data must have an images path." );
                }

                // Load each path.
                var n = data.ImagePaths.Length;

                _regions = new List< TextureRegion >( capacity: n );

                for ( var i = 0; i < n; i++ )
                {
                    FileInfo file = data.FontFile == null
                        ? Gdx.Files.Internal( data.ImagePaths[ i ] )
                        : Gdx.Files.GetFileHandle( data.ImagePaths[ i ], data.FontFile.Type );

                    _regions.Add( new TextureRegion( new Texture( file, false ) ) );
                }

                _ownsTexture = true;
            }
            else
            {
                _regions     = pageRegions;
                _ownsTexture = false;
            }

            _cache = NewFontCache();

            Load( data );
        }

        public void Load( BitmapFontData data )
        {
            foreach ( var page in data.Glyphs )
            {
                if ( page == null ) continue;

                foreach ( Glyph? glyph in page )
                {
                    data.SetGlyphRegion( glyph, _regions[ glyph.Page ] );
                }
            }

            if ( data.MissingGlyph != null )
            {
                data.SetGlyphRegion
                    (
                     data.MissingGlyph,
                     _regions.Get( data.MissingGlyph.Page )
                    );
            }
        }

        /// <summary>
        /// Draws text at the specified position.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GlyphLayout Draw( IBatch batch, string str, float x, float y )
        {
            _cache.Clear();

            GlyphLayout layout = _cache.AddText( str, x, y );

            _cache.Draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public GlyphLayout Draw( Batch batch, CharSequence str, float x, float y, float targetWidth, int halign, bool wrap )
        {
            _cache.clear();
            GlyphLayout layout = _cache.addText( str, x, y, targetWidth, halign, wrap );
            _cache.draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public GlyphLayout Draw( Batch batch,
                                 CharSequence str,
                                 float x,
                                 float y,
                                 int start,
                                 int end,
                                 float targetWidth,
                                 int halign,
                                 bool wrap )
        {
            _cache.clear();
            GlyphLayout layout = _cache.addText( str, x, y, start, end, targetWidth, halign, wrap );
            _cache.draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public GlyphLayout Draw( Batch batch,
                                 CharSequence str,
                                 float x,
                                 float y,
                                 int start,
                                 int end,
                                 float targetWidth,
                                 int halign,
                                 bool wrap,
                                 string truncate )
        {
            _cache.clear();
            GlyphLayout layout = _cache.addText( str, x, y, start, end, targetWidth, halign, wrap, truncate );
            _cache.draw( batch );

            return layout;
        }

        /** Draws text at the specified position.
 * @see BitmapFontCache#addText(CharSequence, float, float, int, int, float, int, bool, string) */
        public void Draw( Batch batch, GlyphLayout layout, float x, float y )
        {
            _cache.clear();
            _cache.addText( layout, x, y );
            _cache.draw( batch );
        }

        /** Returns the color of text drawn with this font. */
        public Color GetColor()
        {
            return _cache.getColor();
        }

        /** A convenience method for setting the font color. The color can also be set by modifying {@link #getColor()}. */
        public void SetColor( Color color )
        {
            _cache.getColor().set( color );
        }

        /** A convenience method for setting the font color. The color can also be set by modifying {@link #getColor()}. */
        public void SetColor( float r, float g, float b, float a )
        {
            _cache.GetColor().set( r, g, b, a );
        }

        public float GetScaleX()
        {
            return _data.ScaleX;
        }

        public float GetScaleY()
        {
            return _data.ScaleY;
        }

        /** Returns the first texture region. This is included for backwards compatibility, and for convenience since most fonts only
 * use one texture page. For multi-page fonts, use {@link #getRegions()}.
 * @return the first texture region */
        public TextureRegion GetRegion()
        {
            return _regions.first();
        }

        /** Returns the array of TextureRegions that represents each texture page of glyphs.
 * @return the array of texture regions; modifying it may produce undesirable results */
        public List< TextureRegion > GetRegions()
        {
            return _regions;
        }

        /** Returns the texture page at the given index.
 * @return the texture page at the given index */
        public TextureRegion GetRegion( int index )
        {
            return _regions.get( index );
        }

        /** Returns the line height, which is the distance from one line of text to the next. */
        public float GetLineHeight()
        {
            return _data.LineHeight;
        }

        /** Returns the x-advance of the space character. */
        public float GetSpaceXadvance()
        {
            return _data.SpaceXadvance;
        }

        /** Returns the x-height, which is the distance from the top of most lowercase characters to the baseline. */
        public float GetXHeight()
        {
            return _data.XHeight;
        }

        /** Returns the cap height, which is the distance from the top of most uppercase characters to the baseline. Since the drawing
 * position is the cap height of the first line, the cap height can be used to get the location of the baseline. */
        public float GetCapHeight()
        {
            return _data.CapHeight;
        }

        /** Returns the ascent, which is the distance from the cap height to the top of the tallest glyph. */
        public float GetAscent()
        {
            return _data.Ascent;
        }

        /** Returns the descent, which is the distance from the bottom of the glyph that extends the lowest to the baseline. This
 * number is negative. */
        public float GetDescent()
        {
            return _data.Descent;
        }

        /** Disposes the texture used by this BitmapFont's region IF this BitmapFont created the texture. */
        public void Dispose()
        {
            if ( _ownsTexture )
            {
                for ( var i = 0; i < _regions.size; i++ )
                    _regions.get( i ).getTexture().dispose();
            }
        }

        /** Makes the specified glyphs fixed width. This can be useful to make the numbers in a font fixed width. Eg, when horizontally
 * centering a score or loading percentage text, it will not jump around as different numbers are shown. */
        public void SetFixedWidthGlyphs( CharSequence glyphs )
        {
            var data       = this._data;
            var maxAdvance = 0;

            for ( int index = 0, end = glyphs.length(); index < end; index++ )
            {
                Glyph g = data.getGlyph( glyphs.charAt( index ) );

                if ( ( g != null ) && ( g.xadvance > maxAdvance ) )
                {
                    maxAdvance = g.xadvance;
                }
            }

            for ( int index = 0, end = glyphs.length(); index < end; index++ )
            {
                Glyph? g = data.GetGlyph( glyphs.charAt( index ) );

                if ( g == null ) continue;

                g.xoffset    += ( maxAdvance - g.xadvance ) / 2;
                g.xadvance   =  maxAdvance;
                g.kerning    =  null;
                g.fixedWidth =  true;
            }
        }

        /** Specifies whether to use integer positions. Default is to use them so filtering doesn't kick in as badly. */
        public void SetUseIntegerPositions( bool integer )
        {
            this._integer = integer;
            _cache.SetUseIntegerPositions( integer );
        }

        /** Checks whether this font uses integer positions for drawing. */
        public bool UsesIntegerPositions()
        {
            return _integer;
        }

        /** For expert usage -- returns the BitmapFontCache used by this font, for rendering to a sprite batch. This can be used, for
 * example, to manipulate glyph colors within a specific index.
 * @return the bitmap font cache used by this font */
        public BitmapFontCache GetCache()
        {
            return _cache;
        }

        /** Gets the underlying {@link BitmapFontData} for this BitmapFont. */
        public BitmapFontData GetData()
        {
            return _data;
        }

        /** @return whether the texture is owned by the font, font disposes the texture itself if true */
        public bool OwnsTexture()
        {
            return _ownsTexture;
        }

        /** Sets whether the font owns the texture. In case it does, the font will also dispose of the texture when {@link #dispose()}
 * is called. Use with care!
 * @param ownsTexture whether the font owns the texture */
        public void SetOwnsTexture( bool ownsTexture )
        {
            this._ownsTexture = ownsTexture;
        }

        /** Creates a new BitmapFontCache for this font. Using this method allows the font to provide the BitmapFontCache
 * implementation to customize rendering.
 * <p>
 * Note this method is called by the BitmapFont constructors. If a subclass overrides this method, it will be called before the
 * subclass constructors. */
        public BitmapFontCache NewFontCache()
        {
            return new BitmapFontCache( this, _integer );
        }

        public string ToString()
        {
            return _data.Name != null ? _data.Name : base.ToString();
        }

        /** Represents a single character in a font page. */
        public sealed class Glyph
        {
            public int        id;
            public int        srcX;
            public int        srcY;
            public int        width;
            public int        height;
            public float      u;
            public float      v;
            public float      u2;
            public float      v2;
            public int        xoffset, yoffset;
            public int        xadvance;
            public byte[]?[]? kerning;
            public bool       fixedWidth;

            /// <summary>
            /// The index to the texture page that holds this glyph.
            /// </summary>
            public int Page { get; set; } = 0;

            public int GetKerning( char ch )
            {
                var page = kerning?[ ch >>> Log2_Page_Size ];

                return page != null ? page[ ch & ( Page_Size - 1 ) ] : 0;

            }

            public void SetKerning( int ch, int value )
            {
                kerning ??= new byte[ Pages ][];

                var page = kerning[ ch >>> Log2_Page_Size ];

                if ( page == null ) kerning[ ch >>> Log2_Page_Size ] = page = new byte[ Page_Size ];

                page[ ch & ( Page_Size - 1 ) ] = ( byte )value;
            }

            public new string ToString()
            {
                return id.ToString();
            }
        }

        private static int IndexOf( string text, char ch, int start )
        {
            int n = text.Length;

            for ( ; start < n; start++ )
            {
                if ( text[ start ] == ch )
                {
                    return start;
                }
            }

            return n;
        }

        /// <summary>
        /// Backing data for a <see cref="BitmapFont"/>.
        /// </summary>
        public sealed class BitmapFontData
        {
            // The name of the font, or null.
            public string? Name { get; private set; }

            // An array of the image paths, for multiple texture pages.
            public string[]? ImagePaths { get; set; }

            public FileInfo FontFile  { get; set; }
            public bool     Flipped   { get; set; }
            public float    PadTop    { get; set; }
            public float    PadRight  { get; set; }
            public float    PadBottom { get; set; }
            public float    PadLeft   { get; set; }

            /// <summary>
            /// The distance from one line of text to the next.
            /// </summary>
            public float LineHeight { get; set; }

            /// <summary>
            /// The distance from the top of most uppercase characters to the
            /// baseline. Since the drawing position is the cap height of the
            /// first line, the cap height can be used to get the location of
            /// the baseline. 
            /// </summary>
            public float CapHeight { get; set; } = 1;

            /// <summary>
            /// The distance from the cap height to the top of the tallest glyph.
            /// </summary>
            public float Ascent { get; set; }

            /// <summary>
            /// The distance from the bottom of the glyph that extends the lowest
            /// to the baseline. This number is negative.
            /// </summary>
            public float Descent { get; set; }

            /// <summary>
            /// The distance to move down when \n is encountered.
            /// </summary>
            public float Down { get; set; }

            /// <summary>
            /// Multiplier for the line height of blank lines. down * blankLineHeight is
            /// used as the distance to move down for a blank line. 
            /// </summary>
            public float BlankLineScale { get; set; } = 1;

            public float ScaleX        { get; set; } = 1;
            public float ScaleY        { get; set; } = 1;
            public bool  MarkupEnabled { get; set; }

            /// <summary>
            /// The amount to add to the glyph X position when drawing a cursor between
            /// glyphs. This field is not set by the BMFont file, it needs to be set
            /// manually depending on how the glyphs are rendered on the backing textures. 
            /// </summary>
            public float CursorX { get; set; }

            public Glyph?[]?[] Glyphs { get; set; } = new Glyph[ Pages ][];

            /// <summary>
            /// The glyph to display for characters not in the font. May be null.
            /// </summary>
            public Glyph? MissingGlyph { get; set; }

            /// <summary>
            /// The width of the space character.
            /// </summary>
            public float SpaceXadvance { get; set; }

            /// <summary>
            /// The x-height, which is the distance from the top of most lowercase
            /// characters to the baseline.
            /// </summary>
            public float XHeight { get; set; } = 1;

            /// <summary>
            /// Additional characters besides whitespace where text is wrapped.
            /// Eg, a hypen (-).
            /// </summary>
            public char[]? breakChars;

            public readonly char[] xChars =
            {
                'x', 'e', 'a', 'o', 'n', 's', 'r', 'c', 'u', 'm', 'v', 'w', 'z'
            };

            public readonly char[] capChars =
            {
                'M', 'N', 'B', 'D', 'C', 'E', 'F', 'K', 'A', 'G', 'H', 'I', 'J',
                'L', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            /// <summary>
            /// Creates an empty BitmapFontData for configuration before calling
            /// <see cref="Load(FileInfo, bool)"/>, to subclass, or to populate
            /// yourself, e.g. using stb-truetype or FreeType.
            /// </summary>
            public BitmapFontData()
            {
                this.FontFile = null!;
                this.Flipped  = false;
            }

            /// <summary>
            /// </summary>
            /// <param name="fontFile"></param>
            /// <param name="flip"></param>
            public BitmapFontData( FileInfo fontFile, bool flip )
            {
                this.FontFile = fontFile;
                this.Flipped  = flip;

                Load( fontFile, flip );
            }

            /// <summary>
            /// </summary>
            /// <param name="file"></param>
            /// <param name="flip"></param>
            /// <exception cref="InvalidOperationException"></exception>
            /// <exception cref="GdxRuntimeException"></exception>
            public void Load( FileInfo file, bool flip )
            {
                if ( ImagePaths != null )
                {
                    throw new InvalidOperationException( "Already loaded." );
                }

                Name = file.Name;

                BufferedReader reader = new BufferedReader( new InputStreamReader( file.Read() ), 512 );

                try
                {
                    string line = reader.readLine(); // info

                    if ( line == null ) throw new GdxRuntimeException( "File is empty." );

                    line = line.Substring( line.IndexOf( "padding=", StringComparison.Ordinal ) + 8 );

                    var padding
                        = line.Substring( 0, line.IndexOf( ' ' ) )
                            .Split( ",", 4 );

                    if ( padding.Length != 4 ) throw new GdxRuntimeException( "Invalid padding." );

                    PadTop    = int.Parse( padding[ 0 ] );
                    PadRight  = int.Parse( padding[ 1 ] );
                    PadBottom = int.Parse( padding[ 2 ] );
                    PadLeft   = int.Parse( padding[ 3 ] );

                    var padY = PadTop + PadBottom;

                    line = reader.readLine();

                    if ( line == null ) throw new GdxRuntimeException( "Missing common header." );

                    var common = line.Split( " ", 9 ); // At most we want the 6th element; i.e. "page=N"

                    // At least lineHeight and base are required.
                    if ( common.Length < 3 ) throw new GdxRuntimeException( "Invalid common header." );

                    if ( !common[ 1 ].StartsWith( "lineHeight=" ) ) throw new GdxRuntimeException( "Missing: lineHeight" );

                    LineHeight = int.Parse( common[ 1 ].Substring( 11 ) );

                    if ( !common[ 2 ].StartsWith( "base=" ) ) throw new GdxRuntimeException( "Missing: base" );

                    float baseLine = int.Parse( common[ 2 ].Substring( 5 ) );

                    var pageCount = 1;

                    if ( common is [_, _, _, _, _, not null, ..] && common[ 5 ].StartsWith( "pages=" ) )
                    {
                        try
                        {
                            pageCount = Math.Max( 1, int.Parse( common[ 5 ].Substring( 6 ) ) );
                        }
                        catch ( NumberFormatException ignored )
                        {
                            // Use one page.
                        }
                    }

                    ImagePaths = new string[ pageCount ];

                    // Read each page definition.
                    for ( var p = 0; p < pageCount; p++ )
                    {
                        // Read each "page" info line.
                        line = reader.readLine();

                        if ( line == null ) throw new GdxRuntimeException( "Missing additional page definitions." );

                        // Expect ID to mean "index".
                        Matcher matcher = Pattern.compile( ".*id=(\\d+)" ).matcher( line );

                        if ( matcher.find() )
                        {
                            string id = matcher.group( 1 );

                            try
                            {
                                var pageID = int.Parse( id );

                                if ( pageID != p )
                                    throw new GdxRuntimeException( "Page IDs must be indices starting at 0: " + id );
                            }
                            catch ( NumberFormatException ex )
                            {
                                throw new GdxRuntimeException( "Invalid page id: " + id, ex );
                            }
                        }

                        matcher = Pattern.compile( ".*file=\"?([^\"]+)\"?" ).matcher( line );

                        if ( !matcher.find() ) throw new GdxRuntimeException( "Missing: file" );
                        string fileName = matcher.group( 1 );

                        ImagePaths[ p ] = file.Parent().child( fileName ).path().replaceAll( "\\\\", "/" );
                    }

                    Descent = 0;

                    while ( true )
                    {
                        line = reader.readLine();

                        if ( line == null ) break;                   // EOF
                        if ( line.StartsWith( "kernings " ) ) break; // Starting kernings block.
                        if ( line.StartsWith( "metrics " ) ) break;  // Starting metrics block.

                        if ( !line.StartsWith( "char " ) ) continue;

                        var glyph = new Glyph();

                        var tokens = new StringTokenizer( line, " =" );

                        tokens.NextToken();
                        tokens.NextToken();

                        var ch = int.Parse( tokens.NextToken() );

                        if ( ch <= 0 )
                        {
                            MissingGlyph = glyph;
                        }
                        else if ( ch <= char.MaxValue )
                        {
                            SetGlyph( ch, glyph );
                        }
                        else
                        {
                            continue;
                        }

                        glyph.id = ch;
                        tokens.NextToken();
                        glyph.srcX = int.Parse( tokens.NextToken() );
                        tokens.NextToken();
                        glyph.srcY = int.Parse( tokens.NextToken() );
                        tokens.NextToken();
                        glyph.width = int.Parse( tokens.NextToken() );
                        tokens.NextToken();
                        glyph.height = int.Parse( tokens.NextToken() );
                        tokens.NextToken();
                        glyph.xoffset = int.Parse( tokens.NextToken() );
                        tokens.NextToken();

                        if ( flip )
                        {
                            glyph.yoffset = int.Parse( tokens.NextToken() );
                        }
                        else
                        {
                            glyph.yoffset = -( glyph.height + int.Parse( tokens.NextToken() ) );
                        }

                        tokens.NextToken();

                        glyph.xadvance = int.Parse( tokens.NextToken() );

                        // Check for page safely, it could be omitted or invalid.
                        if ( tokens.HasMoreTokens() ) tokens.NextToken();

                        if ( tokens.HasMoreTokens() )
                        {
                            try
                            {
                                glyph.Page = int.Parse( tokens.NextToken() );
                            }
                            catch ( NumberFormatException ignored )
                            {
                            }
                        }

                        if ( glyph is { width: > 0, height: > 0 } )
                        {
                            Descent = Math.Min( baseLine + glyph.yoffset, Descent );
                        }
                    }

                    Descent += PadBottom;

                    while ( true )
                    {
                        line = reader.readLine();

                        if ( line == null ) break;
                        if ( !line.StartsWith( "kerning " ) ) break;

                        var tokens = new StringTokenizer( line, " =" );

                        tokens.NextToken();
                        tokens.NextToken();

                        var first = int.Parse( tokens.NextToken() );

                        tokens.NextToken();

                        var second = int.Parse( tokens.NextToken() );

                        if ( ( first < 0 )
                             || ( first > CharHelper.Max_Value )
                             || ( second < 0 )
                             || ( second > CharHelper.Max_Value ) )
                        {
                            continue;
                        }

                        Glyph? glyph = GetGlyph( ( char )first );
                        tokens.NextToken();

                        var amount = int.Parse( tokens.NextToken() );

                        // Kernings may exist for glyph pairs not contained in the font.
                        glyph?.SetKerning( second, amount );
                    }

                    var hasMetricsOverride = false;

                    float overrideAscent        = 0;
                    float overrideDescent       = 0;
                    float overrideDown          = 0;
                    float overrideCapHeight     = 0;
                    float overrideLineHeight    = 0;
                    float overrideSpaceXAdvance = 0;
                    float overrideXHeight       = 0;

                    // Metrics override
                    if ( ( line != null ) && line.StartsWith( "metrics " ) )
                    {
                        hasMetricsOverride = true;

                        var tokens = new StringTokenizer( line, " =" );

                        tokens.NextToken();
                        tokens.NextToken();

                        overrideAscent = float.Parse( tokens.NextToken() );
                        tokens.NextToken();

                        overrideDescent = float.Parse( tokens.NextToken() );
                        tokens.NextToken();

                        overrideDown = float.Parse( tokens.NextToken() );
                        tokens.NextToken();

                        overrideCapHeight = float.Parse( tokens.NextToken() );
                        tokens.NextToken();

                        overrideLineHeight = float.Parse( tokens.NextToken() );
                        tokens.NextToken();

                        overrideSpaceXAdvance = float.Parse( tokens.NextToken() );
                        tokens.NextToken();

                        overrideXHeight = float.Parse( tokens.NextToken() );
                    }

                    Glyph? spaceGlyph = GetGlyph( ' ' );

                    if ( spaceGlyph == null )
                    {
                        spaceGlyph = new Glyph { id = ' ' };

                        Glyph? xadvanceGlyph = GetGlyph( 'l' );

                        if ( xadvanceGlyph == null )
                        {
                            xadvanceGlyph = GetFirstGlyph();
                        }

                        spaceGlyph.xadvance = xadvanceGlyph.xadvance;

                        SetGlyph( ' ', spaceGlyph );
                    }

                    if ( spaceGlyph.width == 0 )
                    {
                        spaceGlyph.width   = ( int )( PadLeft + spaceGlyph.xadvance + PadRight );
                        spaceGlyph.xoffset = ( int )-PadLeft;
                    }

                    SpaceXadvance = spaceGlyph.xadvance;

                    Glyph? xGlyph = null;

                    foreach ( var xChar in xChars )
                    {
                        xGlyph = GetGlyph( xChar );

                        if ( xGlyph != null ) break;
                    }

                    xGlyph ??= GetFirstGlyph();

                    XHeight = xGlyph.height - padY;

                    Glyph? capGlyph = null;

                    foreach ( var capChar in capChars )
                    {
                        capGlyph = GetGlyph( capChar );

                        if ( capGlyph != null ) break;
                    }

                    if ( capGlyph == null )
                    {
                        foreach ( var page in Glyphs )
                        {
                            if ( page == null ) continue;

                            foreach ( Glyph? glyph in page )
                            {
                                if ( ( glyph == null )
                                     || ( glyph.height == 0 )
                                     || ( glyph.width == 0 ) )
                                {
                                    continue;
                                }

                                CapHeight = Math.Max( CapHeight, glyph.height );
                            }
                        }
                    }
                    else
                    {
                        CapHeight = capGlyph.height;
                    }

                    CapHeight -= padY;

                    Ascent = baseLine - CapHeight;
                    Down   = -LineHeight;

                    if ( flip )
                    {
                        Ascent = -Ascent;
                        Down   = -Down;
                    }

                    if ( hasMetricsOverride )
                    {
                        this.Ascent        = overrideAscent;
                        this.Descent       = overrideDescent;
                        this.Down          = overrideDown;
                        this.CapHeight     = overrideCapHeight;
                        this.LineHeight    = overrideLineHeight;
                        this.SpaceXadvance = overrideSpaceXAdvance;
                        this.XHeight       = overrideXHeight;
                    }
                }
                catch ( Exception ex )
                {
                    throw new GdxRuntimeException( "Error loading font file: " + file, ex );
                }
                finally
                {
                    StreamUtils.CloseQuietly( reader );
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="glyph">
            /// A reference to the Glyph whose region is to be set.
            /// </param>
            /// <param name="region"></param>
            public void SetGlyphRegion( Glyph glyph, TextureRegion region )
            {
                var invTexWidth  = 1.0f / region.Texture.Width;
                var invTexHeight = 1.0f / region.Texture.Height;

                var u = region.U;
                var v = region.V;

                var offsetX = 0;
                var offsetY = 0;

                if ( region is TextureAtlas.AtlasRegion )
                {
                    // Compensate for whitespace stripped from left and top edges.
                    var atlasRegion = ( TextureAtlas.AtlasRegion )region;

                    offsetX = ( int )atlasRegion.OffsetX;
                    offsetY = ( int )( atlasRegion.OriginalHeight - atlasRegion.PackedHeight - atlasRegion.OffsetY );
                }

                var x  = glyph.srcX;
                var x2 = glyph.srcX + glyph.width;
                var y  = glyph.srcY;
                var y2 = glyph.srcY + glyph.height;

                // Shift glyph for left and top edge stripped whitespace.
                // Clip glyph for right and bottom edge stripped whitespace.
                // Note: if the font region has padding, whitespace stripping must not be used.
                if ( offsetX > 0 )
                {
                    x -= offsetX;

                    if ( x < 0 )
                    {
                        glyph.width   += x;
                        glyph.xoffset -= x;

                        x = 0;
                    }

                    x2 -= offsetX;

                    if ( x2 > region.RegionWidth )
                    {
                        glyph.width -= x2 - region.RegionWidth;

                        x2 = region.RegionWidth;
                    }
                }

                if ( offsetY > 0 )
                {
                    y -= offsetY;

                    if ( y < 0 )
                    {
                        glyph.height += y;

                        if ( glyph.height < 0 ) glyph.height = 0;

                        y = 0;
                    }

                    y2 -= offsetY;

                    if ( y2 > region.RegionHeight )
                    {
                        var amount = y2 - region.RegionHeight;

                        glyph.height  -= amount;
                        glyph.yoffset += amount;

                        y2 = region.RegionHeight;
                    }
                }

                glyph.u  = u + ( x * invTexWidth );
                glyph.u2 = u + ( x2 * invTexWidth );

                if ( Flipped )
                {
                    glyph.v  = v + ( y * invTexHeight );
                    glyph.v2 = v + ( y2 * invTexHeight );
                }
                else
                {
                    glyph.v2 = v + ( y * invTexHeight );
                    glyph.v  = v + ( y2 * invTexHeight );
                }
            }

            /// <summary>
            /// Sets the line height, which is the distance from one line of text to the next.
            /// </summary>
            public void SetLineHeight( float height )
            {
                LineHeight = height * ScaleY;
                Down       = Flipped ? LineHeight : -LineHeight;
            }

            /// <summary>
            /// </summary>
            /// <param name="ch"></param>
            /// <param name="glyph"></param>
            public void SetGlyph( int ch, Glyph glyph )
            {
                var page = Glyphs?[ ch / Page_Size ];

                if ( page == null )
                {
                    page = new Glyph[ Page_Size ];

                    this.Glyphs![ ch / Page_Size ] = page;
                }

                page[ ch & ( Page_Size - 1 ) ] = glyph;
            }

            /// <summary>
            /// </summary>
            /// <returns></returns>
            /// <exception cref="GdxRuntimeException"></exception>
            public Glyph GetFirstGlyph()
            {
                foreach ( var page in this.Glyphs )
                {
                    foreach ( Glyph? glyph in page )
                    {
                        if ( ( glyph == null ) || ( glyph.height == 0 ) || ( glyph.width == 0 ) ) continue;

                        return glyph;
                    }
                }

                throw new GdxRuntimeException( "No glyphs found." );
            }

            /// <summary>
            /// Returns true if the font has the glyph, or if the font has a <see cref="MissingGlyph"/>.
            /// </summary>
            public bool HasGlyph( char ch )
            {
                if ( MissingGlyph != null ) return true;

                return GetGlyph( ch ) != null;
            }

            /// <summary>
            /// Returns the glyph for the specified character, or null if no such
            /// glyph exists. Note that
            /// </summary>
            /// See also <see cref="GetGlyphs"/> should be be used to shape a string
            /// of characters into a list of glyphs. 
            public Glyph? GetGlyph( char ch ) => Glyphs[ ch / Page_Size ][ ch & ( Page_Size - 1 ) ];

            /// <summary>
            /// Using the specified string, populates the glyphs and positions of the
            /// specified glyph run.
            /// </summary>
            /// <param name="run"></param>
            /// <param name="str">
            /// Characters to convert to glyphs. Will not contain newline or color tags.
            /// May contain "[[" for an escaped left square bracket.
            /// </param>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <param name="lastGlyph">
            /// The glyph immediately before this run, or null if this is run is the
            /// first on a line of text.
            /// </param>
            public void GetGlyphs( GlyphLayout.GlyphRun run, string str, int start, int end, Glyph lastGlyph )
            {
                var max = end - start;

                if ( max == 0 ) return;

                var markupEnabled = this.MarkupEnabled;
                var scaleX        = this.ScaleX;

                var glyphs    = run.Glyphs;
                var xAdvances = run.XAdvances;

                // Guess at number of glyphs needed.
                glyphs.EnsureCapacity( max );
                run.XAdvances.EnsureCapacity( max + 1 );

                do
                {
                    var ch = str[ start++ ];

                    if ( ch == '\r' ) continue; // Ignore.
                    Glyph? glyph = GetGlyph( ch );

                    if ( glyph == null )
                    {
                        if ( MissingGlyph == null ) continue;
                        glyph = MissingGlyph;
                    }

                    glyphs.Add( glyph );

                    xAdvances.Add
                        (
                         lastGlyph == null // First glyph on line, adjust the position so it isn't drawn left of 0.
                             ? ( glyph.fixedWidth ? 0 : ( -glyph.xoffset * scaleX ) - PadLeft )
                             : ( lastGlyph.xadvance + lastGlyph.GetKerning( ch ) ) * scaleX
                        );

                    lastGlyph = glyph;

                    // "[[" is an escaped left square bracket, skip second character.
                    if ( markupEnabled
                         && ( ch == '[' )
                         && ( start < end )
                         && ( str[ start ] == '[' ) )
                    {
                        start++;
                    }
                }
                while ( start < end );

                if ( lastGlyph != null )
                {
                    var lastGlyphWidth = lastGlyph.fixedWidth
                        ? lastGlyph.xadvance * scaleX
                        : ( ( lastGlyph.width + lastGlyph.xoffset ) * scaleX ) - PadRight;

                    xAdvances.Add( lastGlyphWidth );
                }
            }

            /// <summary>
            /// Returns the first valid glyph index to use to wrap to the next line,
            /// starting at the specified start index and (typically) moving toward
            /// the beginning of the glyphs array.
            /// </summary>
            public int GetWrapIndex( List< Glyph > glyphList, int start )
            {
                var i  = start - 1;
                var ch = ( char )( glyphList[ i ] ).id;

                if ( IsWhitespace( ch ) ) return i;
                if ( IsBreakChar( ch ) ) i--;

                for ( ; i > 0; i-- )
                {
                    ch = ( char )( glyphList[ i ] ).id;

                    if ( IsWhitespace( ch ) || IsBreakChar( ch ) ) return i + 1;
                }

                return 0;
            }

            /// <summary>
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            public bool IsBreakChar( char c )
            {
                if ( breakChars == null )
                {
                    return false;
                }

                foreach ( var br in breakChars )
                {
                    if ( c == br )
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            public static bool IsWhitespace( char c )
            {
                switch ( c )
                {
                    case '\n':
                    case '\r':
                    case '\t':
                    case ' ':
                        return true;

                    default:
                        return false;
                }
            }

            /// <summary>
            /// Scales the font by the specified amounts on both axes
            /// <para>
            /// Note that smoother scaling can be achieved if the texture backing
            /// the BitmapFont is using <see cref="TextureFilter.Linear"/>.
            /// The default is Nearest, so use a BitmapFont constructor that takes
            /// a <see cref="TextureRegion"/>.
            /// </para>
            /// </summary>
            /// <exception cref="ArgumentException">if scaleX or scaleY is zero.</exception>
            public void SetScale( float scalex, float scaley )
            {
                if ( scalex == 0 )
                    throw new ArgumentException( "scaleX cannot be 0." );

                if ( scaley == 0 )
                    throw new ArgumentException( "scaleY cannot be 0." );

                var x = scalex / this.ScaleX;
                var y = scaley / this.ScaleY;

                this.ScaleX = scalex;
                this.ScaleY = scaley;

                LineHeight    *= y;
                SpaceXadvance *= x;
                XHeight       *= y;
                CapHeight     *= y;
                Ascent        *= y;
                Descent       *= y;
                Down          *= y;
                PadLeft       *= x;
                PadRight      *= x;
                PadTop        *= y;
                PadBottom     *= y;
            }

            /// <summary>
            /// Scales the font by the specified amount in both directions.
            /// </summary>
            /// See also <see cref="SetScale(float, float)"/>
            /// <exception cref="ArgumentException">if scaleX or scaleY is zero.</exception>
            public void SetScale( float scaleXy )
            {
                SetScale( scaleXy, scaleXy );
            }

            /// <summary>
            /// Sets the font's scale relative to the current scale.
            /// </summary>
            /// See also <see cref="SetScale(float, float)"/>
            /// <exception cref="ArgumentException">if the resulting scale is zero.</exception>
            public void Scale( float amount )
            {
                SetScale( ScaleX + amount, ScaleY + amount );
            }

            public override string? ToString()
            {
                return Name ?? base.ToString();
            }
        }

    }
}
