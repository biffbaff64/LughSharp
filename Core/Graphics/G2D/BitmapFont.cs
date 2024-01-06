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

using System.Text.RegularExpressions;

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Graphics.G2D;

public class BitmapFont
{
    private const    string          REGEX_PATTERN  = ".*id=(\\d+)";
    private const    string          FONT_NAME      = "Resources/arial-15.fnt";
    private const    int             LOG2_PAGE_SIZE = 9;
    private const    int             PAGE_SIZE      = 1 << LOG2_PAGE_SIZE;
    private const    int             PAGES          = 0x10000 / PAGE_SIZE;
    private readonly BitmapFontCache _cache;

    private readonly BitmapFontData _data;

    private readonly FileType              _fileType;
    private readonly List< TextureRegion > _regions;
    private          bool                  _integer;

    /// <summary>
    ///     Creates a BitmapFont using the default 15pt Arial font included in the library.
    ///     This is convenient to easily display text without bothering without generating
    ///     a bitmap font yourself.
    /// </summary>
    public BitmapFont()
        : this(
            Gdx.Files.Internal( FONT_NAME ),
            Gdx.Files.Internal( FONT_NAME ),
            false
            ) => _fileType = FileType.Internal;

    /// <summary>
    ///     Creates a BitmapFont using the default 15pt Arial font included in the
    ///     libGDXSharp project.
    ///     <para>
    ///         This is convenient to easily display text without bothering without generating a
    ///         bitmap font yourself.
    ///     </para>
    /// </summary>
    /// <param name="flip">
    ///     If true, the glyphs will be flipped for use with a perspective where 0,0 is
    ///     the upper left corner.
    /// </param>
    public BitmapFont( bool flip )
        : this( Gdx.Files.Internal( FONT_NAME ), Gdx.Files.Internal( FONT_NAME ), flip ) => _fileType = FileType.Internal;

    /// <summary>
    ///     Creates a BitmapFont with the glyphs relative to the specified region.
    ///     If the region is null, the glyph textures are loaded from the image file
    ///     given in the font file. The Dispose() method will not dispose the region's
    ///     texture in this case!
    /// </summary>
    /// <param name="fontFile"> the font definition file.</param>
    /// <param name="region">
    ///     The texture region containing the glyphs. The glyphs must be relative to
    ///     the lower left corner (ie, the region should not be flipped). If the region
    ///     is null the glyph images are loaded from the image path in the font file.
    /// </param>
    /// <param name="flip">
    ///     If true, the glyphs will be flipped for use with a perspective where 0,0
    ///     is the upper left corner.
    /// </param>
    public BitmapFont( FileInfo fontFile, TextureRegion region, bool flip = false )
        : this( new BitmapFontData( fontFile, flip ), region, true ) => _fileType = FileType.Local;

    /// <summary>
    ///     Creates a BitmapFont from a BMFont file. The image file name is read from
    ///     the BMFont file and the image is loaded from the same directory.
    /// </summary>
    /// <param name="fontFile"> the font definition file.</param>
    /// <param name="flip">
    ///     If true, the glyphs will be flipped for use with a perspective where 0,0
    ///     is the upper left corner.
    /// </param>
    public BitmapFont( FileInfo fontFile, bool flip = false )
        : this( new BitmapFontData( fontFile, flip ), ( TextureRegion? )null, true ) => _fileType = FileType.Local;

    /// <summary>
    ///     Creates a BitmapFont from a BMFont file, using the specified image for
    ///     glyphs. Any image specified in the BMFont file is ignored.
    /// </summary>
    /// <param name="fontFile"> the font definition file.</param>
    /// <param name="imageFile"></param>
    /// <param name="flip">
    ///     If true, the glyphs will be flipped for use with a perspective where
    ///     0,0 is the upper left corner.
    /// </param>
    /// <param name="integer"></param>
    public BitmapFont( FileInfo fontFile, FileInfo imageFile, bool flip, bool integer = true )
        : this(
            new BitmapFontData( fontFile, flip ),
            new TextureRegion( new Texture( imageFile, false ) ),
            integer
            )
    {
        OwnsTexture = true;
        _fileType   = FileType.Local;
    }

    /// <summary>
    ///     Constructs a new BitmapFont from the given <see cref="BitmapFontData" /> and
    ///     <see cref="TextureRegion" />. If the TextureRegion is null, the image path(s)
    ///     will be read from the BitmapFontData.
    ///     <para>
    ///         The dispose() method will not dispose the texture of the region(s) if the
    ///         region is != null.
    ///     </para>
    ///     <para>
    ///         Passing a single TextureRegion assumes that your font only needs a single
    ///         texture page. If you need to support multiple pages, either let the Font read
    ///         the images themselves (by specifying null as the TextureRegion), or by
    ///         specifying each page manually with the TextureRegion[] constructor.
    ///     </para>
    /// </summary>
    /// <param name="data"></param>
    /// <param name="region"></param>
    /// <param name="integer">
    ///     If true, rendering positions will be at integer values to avoid filtering
    ///     artifacts.
    /// </param>
    public BitmapFont( BitmapFontData data, TextureRegion? region, bool integer )
        : this( data, region != null ? ListExtensions.With( region ) : null, integer ) => _fileType = FileType.Local;

    /// <summary>
    ///     Constructs a new BitmapFont from the given <see cref="BitmapFontData" /> and array
    ///     of <see cref="TextureRegion" />. If the TextureRegion is null or empty, the image
    ///     path(s) will be read from the BitmapFontData. The dispose() method will not dispose
    ///     the texture of the region(s) if the regions array is != null and not empty.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pageRegions"></param>
    /// <param name="integer">
    ///     If true, rendering positions will be at integer values to avoid filtering artifacts.
    /// </param>
    public BitmapFont( BitmapFontData data, List< TextureRegion >? pageRegions, bool integer )
    {
        Flipped             = data.Flipped;
        _data               = data;
        UseIntegerPositions = integer;
        _fileType           = FileType.Local;

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
                    : Gdx.Files.GetFileHandle( data.ImagePaths[ i ], _fileType );

                _regions.Add( new TextureRegion( new Texture( file, false ) ) );
            }

            OwnsTexture = true;
        }
        else
        {
            _regions    = pageRegions;
            OwnsTexture = false;
        }

        _cache = NewFontCache();

        Load( data );
    }

    public bool Flipped     { get; set; }
    public bool OwnsTexture { get; set; }

    /// <summary>
    ///     Specifies whether to use integer positions.
    ///     Default is to use them so filtering doesn't kick in as badly.
    /// </summary>
    public bool UseIntegerPositions
    {
        get => _integer;
        set
        {
            _integer                   = value;
            _cache.UseIntegerPositions = value;
        }
    }

    protected virtual void Load( BitmapFontData data )
    {
        foreach ( Glyph?[]? page in data.Glyphs )
        {
            if ( page == null )
            {
                continue;
            }

            foreach ( Glyph? glyph in page )
            {
                if ( glyph != null )
                {
                    data.SetGlyphRegion( glyph, _regions[ glyph.Page ] );
                }
            }
        }

        if ( data.MissingGlyph != null )
        {
            data.MissingGlyph = data.SetGlyphRegion(
                data.MissingGlyph,
                _regions[ data.MissingGlyph.Page ]
                );
        }
    }

    /// <summary>
    ///     Draws text at the specified position.
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

    /// <summary>
    ///     Draws text at the specified position.
    /// </summary>
    public GlyphLayout Draw( IBatch batch, string str, float x, float y, int targetWidth, int halign, bool wrap )
    {
        _cache.Clear();

        GlyphLayout layout = _cache.AddText( str, x, y, targetWidth, halign, wrap );

        _cache.Draw( batch );

        return layout;
    }

    /// <summary>
    ///     Draws text at the specified position.
    /// </summary>
    public GlyphLayout Draw( IBatch batch,
                             string str,
                             float x,
                             float y,
                             int start,
                             int end,
                             float targetWidth,
                             int halign,
                             bool wrap )
    {
        _cache.Clear();

        GlyphLayout layout = _cache.AddText(
            str,
            x,
            y,
            start,
            end,
            targetWidth,
            halign,
            wrap
            );

        _cache.Draw( batch );

        return layout;
    }

    /// <summary>
    ///     Draws text at the specified position.
    /// </summary>
    public GlyphLayout Draw( IBatch batch,
                             string str,
                             float x,
                             float y,
                             int start,
                             int end,
                             float targetWidth,
                             int halign,
                             bool wrap,
                             string truncate )
    {
        _cache.Clear();

        GlyphLayout layout = _cache.AddText( str, x, y, start, end, targetWidth, halign, wrap, truncate );

        _cache.Draw( batch );

        return layout;
    }

    /// <summary>
    ///     Draws text at the specified position.
    /// </summary>
    public void Draw( IBatch batch, GlyphLayout layout, float x, float y )
    {
        _cache.Clear();
        _cache.AddText( layout, x, y );
        _cache.Draw( batch );
    }

    /// <summary>
    ///     Returns the color of text drawn with this font.
    /// </summary>
    public Color GetColor() => _cache.GetColor();

    /// <summary>
    ///     A convenience method for setting the font color.
    /// </summary>
    public void SetColor( Color color ) => _cache.GetColor().Set( color );

    /// <summary>
    ///     A convenience method for setting the font color.
    /// </summary>
    public void SetColor( float r, float g, float b, float a ) => _cache.GetColor().Set( r, g, b, a );

    public float GetScaleX() => _data.ScaleX;

    public float GetScaleY() => _data.ScaleY;

    /// <summary>
    ///     Returns the first texture region. This is included for backwards
    ///     compatibility, and for convenience since most fonts only use one
    ///     texture page.
    ///     <para>
    ///         For multi-page fonts, use <see cref="GetRegions()" />.
    ///     </para>
    /// </summary>
    /// <returns>the first texture region</returns>
    public TextureRegion GetRegion() => _regions.First();

    /// <summary>
    ///     Returns the array of TextureRegions that represents each texture page of glyphs.
    /// </summary>
    /// <returns>
    ///     the array of texture regions; modifying it may produce undesirable results
    /// </returns>
    public List< TextureRegion > GetRegions() => _regions;

    /// <summary>
    ///     Returns the texture page at the given index.
    /// </summary>
    public TextureRegion GetRegion( int index ) => _regions[ index ];

    /// <summary>
    ///     Returns the line height, which is the distance from one line of text to the next.
    /// </summary>
    public float GetLineHeight() => _data.LineHeight;

    /**
     * Returns the x-advance of the space character.
     */
    public float GetSpaceXadvance() => _data.SpaceXadvance;

    /**
     * Returns the x-height, which is the distance from the top of most lowercase characters to the baseline.
     */
    public float GetXHeight() => _data.XHeight;

    /// <summary>
    ///     Returns the cap height, which is the distance from the top of most uppercase
    ///     characters to the baseline. Since the drawing position is the cap height of
    ///     the first line, the cap height can be used to get the location of the baseline.
    /// </summary>
    public float GetCapHeight() => _data.CapHeight;

    /// <summary>
    ///     Returns the ascent, which is the distance from the cap height to the top of
    ///     the tallest glyph.
    /// </summary>
    public float GetAscent() => _data.Ascent;

    /// <summary>
    ///     Returns the descent, which is the distance from the bottom of the glyph that
    ///     extends the lowest to the baseline. This number is negative.
    /// </summary>
    public float GetDescent() => _data.Descent;

    /// <summary>
    ///     Disposes the texture used by this BitmapFont's region IF this BitmapFont
    ///     created the texture.
    /// </summary>
    public void Dispose()
    {
        if ( OwnsTexture )
        {
            foreach ( TextureRegion t in _regions )
            {
                t.Texture.Dispose();
            }
        }
    }

    /// <summary>
    ///     Makes the specified glyphs fixed width. This can be useful to make the numbers
    ///     in a font fixed width. Eg, when horizontally centering a score or loading
    ///     percentage text, it will not jump around as different numbers are shown.
    /// </summary>
    public void SetFixedWidthGlyphs( string glyphs )
    {
        BitmapFontData data       = _data;
        var            maxAdvance = 0;

        for ( int index = 0, end = glyphs.Length; index < end; index++ )
        {
            Glyph? g = data.GetGlyph( glyphs[ index ] );

            if ( ( g != null ) && ( g.xadvance > maxAdvance ) )
            {
                maxAdvance = g.xadvance;
            }
        }

        for ( int index = 0, end = glyphs.Length; index < end; index++ )
        {
            Glyph? g = data.GetGlyph( glyphs[ index ] );

            if ( g == null )
            {
                continue;
            }

            g.xoffset    += ( maxAdvance - g.xadvance ) / 2;
            g.xadvance   =  maxAdvance;
            g.kerning    =  null;
            g.fixedWidth =  true;
        }
    }

    /// <summary>
    ///     For expert usage -- returns the BitmapFontCache used by this font, for rendering
    ///     to a sprite batch. This can be used, for example, to manipulate glyph colors
    ///     within a specific index.
    /// </summary>
    /// <returns> the bitmap font cache used by this font  </returns>
    public BitmapFontCache GetCache() => _cache;

    /// <summary>
    ///     Gets the underlying <see cref="BitmapFontData" /> for this BitmapFont.
    /// </summary>
    public BitmapFontData GetData() => _data;

    /// <summary>
    ///     Creates a new BitmapFontCache for this font. Using this method allows the
    ///     font to provide the BitmapFontCache implementation to customize rendering.
    /// </summary>
    /// <para>
    ///     Note this method is called by the BitmapFont constructors. If a subclass
    ///     overrides this method, it will be called before the subclass constructors.
    /// </para>
    public BitmapFontCache NewFontCache() => new( this, UseIntegerPositions );

    public new string? ToString() => _data.Name ?? base.ToString();

    private static int IndexOf( string text, char ch, int start )
    {
        var n = text.Length;

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
    ///     Represents a single character in a font page.
    /// </summary>
    public class Glyph
    {
        internal bool       fixedWidth;
        internal int        height;
        internal int        id;
        internal byte[]?[]? kerning;
        internal int        srcX;
        internal int        srcY;
        internal float      u;
        internal float      u2;
        internal float      v;
        internal float      v2;
        internal int        width;
        internal int        xadvance;
        internal int        xoffset;
        internal int        yoffset;

        /// <summary>
        ///     The index to the texture page that holds this glyph.
        /// </summary>
        internal int Page { get; set; } = 0;

        internal int GetKerning( char ch )
        {
            var page = kerning?[ ch >>> LOG2_PAGE_SIZE ];

            return page != null ? page[ ch & ( PAGE_SIZE - 1 ) ] : 0;
        }

        internal void SetKerning( int ch, int value )
        {
            kerning ??= new byte[ PAGES ][];

            var page = kerning[ ch >>> LOG2_PAGE_SIZE ];

            if ( page == null )
            {
                kerning[ ch >>> LOG2_PAGE_SIZE ] = page = new byte[ PAGE_SIZE ];
            }

            page[ ch & ( PAGE_SIZE - 1 ) ] = ( byte )value;
        }

        public new string ToString() => id.ToString();
    }

    /// <summary>
    ///     Backing data for a <see cref="BitmapFont" />.
    /// </summary>
    public class BitmapFontData
    {

        internal readonly char[] capChars =
        {
            'M', 'N', 'B', 'D', 'C', 'E', 'F', 'K', 'A', 'G', 'H', 'I', 'J',
            'L', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        internal readonly char[] xChars =
        {
            'x', 'e', 'a', 'o', 'n', 's', 'r', 'c', 'u', 'm', 'v', 'w', 'z'
        };

        /// <summary>
        ///     Additional characters besides whitespace where text is wrapped.
        ///     Eg, a hypen (-).
        /// </summary>

        // ReSharper disable once UnassignedField.Global
        internal char[]? breakChars;

        /// <summary>
        ///     Creates an empty BitmapFontData for configuration before calling
        ///     <see cref="Load(FileInfo, bool)" />, to subclass, or to populate
        ///     yourself, e.g. using stb-truetype or FreeType.
        /// </summary>
        public BitmapFontData()
        {
            FontFile = null!;
            Flipped  = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="fontFile"></param>
        /// <param name="flip"></param>
        public BitmapFontData( FileInfo fontFile, bool flip )
        {
            FontFile = fontFile;
            Flipped  = flip;

            Load( fontFile, flip );
        }

        // The name of the font, or null.
        internal string? Name { get; private set; }

        // An array of the image paths, for multiple texture pages.
        internal string[]? ImagePaths { get; private set; }

        internal FileInfo FontFile  { get; set; }
        internal bool     Flipped   { get; set; }
        internal float    PadTop    { get; set; }
        internal float    PadRight  { get; set; }
        internal float    PadBottom { get; set; }
        internal float    PadLeft   { get; set; }

        /// <summary>
        ///     The distance from one line of text to the next.
        /// </summary>
        internal float LineHeight { get; private set; }

        /// <summary>
        ///     The distance from the top of most uppercase characters to the
        ///     baseline. Since the drawing position is the cap height of the
        ///     first line, the cap height can be used to get the location of
        ///     the baseline.
        /// </summary>
        internal float CapHeight { get; private set; } = 1;

        /// <summary>
        ///     The distance from the cap height to the top of the tallest glyph.
        /// </summary>
        internal float Ascent { get; private set; }

        /// <summary>
        ///     The distance from the bottom of the glyph that extends the lowest
        ///     to the baseline. This number is negative.
        /// </summary>
        internal float Descent { get; private set; }

        /// <summary>
        ///     The distance to move down when \n is encountered.
        /// </summary>
        internal float Down { get; set; }

        /// <summary>
        ///     Multiplier for the line height of blank lines. down * blankLineHeight is
        ///     used as the distance to move down for a blank line.
        /// </summary>
        internal float BlankLineScale { get; set; } = 1;

        internal float ScaleX        { get; private set; } = 1;
        internal float ScaleY        { get; private set; } = 1;
        internal bool  MarkupEnabled { get; set; }

        /// <summary>
        ///     The amount to add to the glyph X position when drawing a cursor between
        ///     glyphs. This field is not set by the BMFont file, it needs to be set
        ///     manually depending on how the glyphs are rendered on the backing textures.
        /// </summary>
        internal float CursorX { get; set; }

        internal Glyph?[]?[] Glyphs { get; set; } = new Glyph[ PAGES ][];

        /// <summary>
        ///     The glyph to display for characters not in the font. May be null.
        /// </summary>
        internal Glyph? MissingGlyph { get; set; }

        /// <summary>
        ///     The width of the space character.
        /// </summary>
        internal float SpaceXadvance { get; set; }

        /// <summary>
        ///     The x-height, which is the distance from the top of most lowercase
        ///     characters to the baseline.
        /// </summary>
        internal float XHeight { get; set; } = 1;

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

            var reader = new StreamReader( file.FullName );

            try
            {
                var line = reader.ReadLine(); // info

                if ( line == null )
                {
                    throw new GdxRuntimeException( "File is empty." );
                }

                line = line.Substring( line.IndexOf( "padding=", StringComparison.Ordinal ) + 8 );

                var padding = line.Substring( 0, line.IndexOf( ' ' ) ).Split( ",", 4 );

                if ( padding.Length != 4 )
                {
                    throw new GdxRuntimeException( "Invalid padding." );
                }

                PadTop    = int.Parse( padding[ 0 ] );
                PadRight  = int.Parse( padding[ 1 ] );
                PadBottom = int.Parse( padding[ 2 ] );
                PadLeft   = int.Parse( padding[ 3 ] );

                var padY = PadTop + PadBottom;

                line = reader.ReadLine();

                if ( line == null )
                {
                    throw new GdxRuntimeException( "Missing common header." );
                }

                var common = line.Split( " ", 9 ); // At most we want the 6th element; i.e. "page=N"

                // At least lineHeight and base are required.
                if ( common.Length < 3 )
                {
                    throw new GdxRuntimeException( "Invalid common header." );
                }

                if ( !common[ 1 ].StartsWith( "lineHeight=" ) )
                {
                    throw new GdxRuntimeException( "Missing: lineHeight" );
                }

                LineHeight = int.Parse( common[ 1 ].Substring( 11 ) );

                if ( !common[ 2 ].StartsWith( "base=" ) )
                {
                    throw new GdxRuntimeException( "Missing: base" );
                }

                float baseLine = int.Parse( common[ 2 ].Substring( 5 ) );

                var pageCount = 1;

                if ( common is [ var _, var _, var _, var _, var _, not null, .. ]
                  && common[ 5 ].StartsWith( "pages=" ) )
                {
                    try
                    {
                        pageCount = Math.Max( 1, int.Parse( common[ 5 ].Substring( 6 ) ) );
                    }
                    catch ( NumberFormatException ignored )
                    {
                        // Use one page.
                        Gdx.App.Log( "BitmapFont", "IGNORED NumberFormatException." + ignored.Message );
                    }
                }

                ImagePaths = new string[ pageCount ];

                // Read each page definition.
                for ( var p = 0; p < pageCount; p++ )
                {
                    // Read each "page" info line.
                    line = reader.ReadLine();

                    if ( line == null )
                    {
                        throw new GdxRuntimeException( "Missing additional page definitions." );
                    }

                    // Expect ID to mean "index".
                    var rx = new Regex( ".*id=(\\d+)" );

                    MatchCollection matches = rx.Matches( line );

                    if ( matches.Count > 0 )
                    {
                        Match id = matches[ 0 ];

                        try
                        {
                            var pageID = int.Parse( id.Value );

                            if ( pageID != p )
                            {
                                throw new GdxRuntimeException( "Page IDs must be indices starting at 0: " + id );
                            }
                        }
                        catch ( NumberFormatException ex )
                        {
                            throw new GdxRuntimeException( "Invalid page id: " + id, ex );
                        }
                    }

                    rx = new Regex( ".*file=\"?([^\"]+)\"?" );

                    matches = rx.Matches( line );

                    if ( matches.Count <= 0 )
                    {
                        throw new GdxRuntimeException( "Missing: file" );
                    }

//                    matcher = Pattern.Compile( ".*file=\"?([^\"]+)\"?" ).Matcher( line );

//                    if ( !matcher.Find() ) throw new GdxRuntimeException( "Missing: file" );

//                    ImagePaths[ p ] = matcher.Group( 1 )!.Replace( "\\\\", "/" );
                }

                Descent = 0;

                while ( true )
                {
                    line = reader.ReadLine();

                    if ( line == null )
                    {
                        break; // EOF
                    }

                    if ( line.StartsWith( "kernings " ) )
                    {
                        break; // Starting kernings block.
                    }

                    if ( line.StartsWith( "metrics " ) )
                    {
                        break; // Starting metrics block.
                    }

                    if ( !line.StartsWith( "char " ) )
                    {
                        continue;
                    }

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
                    if ( tokens.HasMoreTokens() )
                    {
                        tokens.NextToken();
                    }

                    if ( tokens.HasMoreTokens() )
                    {
                        try
                        {
                            glyph.Page = int.Parse( tokens.NextToken() );
                        }
                        catch ( NumberFormatException ignored )
                        {
                            Gdx.App.Log( "BitmapFont", "IGNORED NumberFormatException." + ignored.Message );
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
                    line = reader.ReadLine();

                    if ( line == null )
                    {
                        break;
                    }

                    if ( !line.StartsWith( "kerning " ) )
                    {
                        break;
                    }

                    var tokens = new StringTokenizer( line, " =" );

                    tokens.NextToken();
                    tokens.NextToken();

                    var first = int.Parse( tokens.NextToken() );

                    tokens.NextToken();

                    var second = int.Parse( tokens.NextToken() );

                    if ( ( first < 0 )
                      || ( first > Character.MAX_VALUE )
                      || ( second < 0 )
                      || ( second > Character.MAX_VALUE ) )
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

                    Glyph xadvanceGlyph = GetGlyph( 'l' ) ?? GetFirstGlyph();

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

                    if ( xGlyph != null )
                    {
                        break;
                    }
                }

                xGlyph ??= GetFirstGlyph();

                XHeight = xGlyph.height - padY;

                Glyph? capGlyph = null;

                foreach ( var capChar in capChars )
                {
                    capGlyph = GetGlyph( capChar );

                    if ( capGlyph != null )
                    {
                        break;
                    }
                }

                if ( capGlyph == null )
                {
                    foreach ( Glyph?[]? page in Glyphs )
                    {
                        if ( page == null )
                        {
                            continue;
                        }

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
                    Ascent        = overrideAscent;
                    Descent       = overrideDescent;
                    Down          = overrideDown;
                    CapHeight     = overrideCapHeight;
                    LineHeight    = overrideLineHeight;
                    SpaceXadvance = overrideSpaceXAdvance;
                    XHeight       = overrideXHeight;
                }
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( "Error loading font file: " + file, ex );
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="glyph">
        ///     A reference to the Glyph whose region is to be set.
        /// </param>
        /// <param name="region"></param>
        /// <remarks>This method is a candidate for reworking using 'ref'</remarks>
        public Glyph SetGlyphRegion( Glyph glyph, TextureRegion region )
        {
            var invTexWidth  = 1.0f / region.Texture.Width;
            var invTexHeight = 1.0f / region.Texture.Height;

            var u = region.U;
            var v = region.V;

            var offsetX = 0;
            var offsetY = 0;

            if ( region is AtlasRegion atlasRegion )
            {
                // Compensate for whitespace stripped from left and top edges.
                offsetX = ( int )atlasRegion.OffsetX;

                offsetY = ( int )( atlasRegion.OriginalHeight
                                 - atlasRegion.PackedHeight
                                 - atlasRegion.OffsetY );
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

                    if ( glyph.height < 0 )
                    {
                        glyph.height = 0;
                    }

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

            return glyph;
        }

        /// <summary>
        ///     Sets the line height, which is the distance from one line of text to the next.
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
            Glyph?[]? page = Glyphs[ ch / PAGE_SIZE ];

            if ( page == null )
            {
                page = new Glyph[ PAGE_SIZE ];

                Glyphs[ ch / PAGE_SIZE ] = page;
            }

            page[ ch & ( PAGE_SIZE - 1 ) ] = glyph;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public Glyph GetFirstGlyph()
        {
            foreach ( Glyph?[]? page in Glyphs )
            {
                if ( page != null )
                {
                    foreach ( Glyph? glyph in page )
                    {
                        if ( ( glyph == null ) || ( glyph.height == 0 ) || ( glyph.width == 0 ) )
                        {
                            continue;
                        }

                        return glyph;
                    }
                }
            }

            throw new GdxRuntimeException( "No glyphs found." );
        }

        /// <summary>
        ///     Returns true if the font has the glyph, or if the font has a <see cref="MissingGlyph" />.
        /// </summary>
        public bool HasGlyph( char ch )
        {
            if ( MissingGlyph != null )
            {
                return true;
            }

            return GetGlyph( ch ) != null;
        }

        /// <summary>
        ///     Returns the glyph for the specified character, or null if no such
        ///     glyph exists. Note that
        /// </summary>
        /// See also
        /// <see cref="GetGlyphs" />
        /// should be be used to shape a string
        /// of characters into a list of glyphs.
        public Glyph? GetGlyph( char ch ) => Glyphs[ ch / PAGE_SIZE ]?[ ch & ( PAGE_SIZE - 1 ) ];

        /// <summary>
        ///     Using the specified string, populates the glyphs and positions of the
        ///     specified glyph run.
        /// </summary>
        /// <param name="run"></param>
        /// <param name="str">
        ///     Characters to convert to glyphs. Will not contain newline or color tags.
        ///     May contain "[[" for an escaped left square bracket.
        /// </param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="lastGlyph">
        ///     The glyph immediately before this run, or null if this is run is the
        ///     first on a line of text.
        /// </param>
        public void GetGlyphs( GlyphLayout.GlyphRun? run, string str, int start, int end, Glyph? lastGlyph )
        {
            var max = end - start;

            if ( max == 0 )
            {
                return;
            }

            var markupEnabled = MarkupEnabled;
            var scaleX        = ScaleX;

            List< Glyph > glyphs    = run.Glyphs;
            List< float > xAdvances = run.XAdvances;

            // Guess at number of glyphs needed.
            glyphs.EnsureCapacity( max );
            run.XAdvances.EnsureCapacity( max + 1 );

            do
            {
                var ch = str[ start++ ];

                if ( ch == '\r' )
                {
                    continue; // Ignore.
                }

                Glyph? glyph = GetGlyph( ch );

                if ( glyph == null )
                {
                    if ( MissingGlyph == null )
                    {
                        continue;
                    }

                    glyph = MissingGlyph;
                }

                glyphs.Add( glyph );

                xAdvances.Add
                    (
                    lastGlyph == null // First glyph on line, adjust the position so it isn't drawn left of 0.
                        ? glyph.fixedWidth ? 0 : ( -glyph.xoffset * scaleX ) - PadLeft
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
        ///     Returns the first valid glyph index to use to wrap to the next line,
        ///     starting at the specified start index and (typically) moving toward
        ///     the beginning of the glyphs array.
        /// </summary>
        public int GetWrapIndex( List< Glyph > glyphList, int start )
        {
            var i  = start - 1;
            var ch = ( char )glyphList[ i ].id;

            if ( IsWhitespace( ch ) )
            {
                return i;
            }

            if ( IsBreakChar( ch ) )
            {
                i--;
            }

            for ( ; i > 0; i-- )
            {
                ch = ( char )glyphList[ i ].id;

                if ( IsWhitespace( ch ) || IsBreakChar( ch ) )
                {
                    return i + 1;
                }
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
        public bool IsWhitespace( char c )
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
        ///     Scales the font by the specified amounts on both axes
        ///     <para>
        ///         Note that smoother scaling can be achieved if the texture backing
        ///         the BitmapFont is using <see cref="TextureFilter.Linear" />.
        ///         The default is Nearest, so use a BitmapFont constructor that takes
        ///         a <see cref="TextureRegion" />.
        ///     </para>
        /// </summary>
        /// <exception cref="ArgumentException">if scaleX or scaleY is zero.</exception>
        public void SetScale( float scalex, float scaley )
        {
            if ( scalex == 0 )
            {
                throw new ArgumentException( "scaleX cannot be 0." );
            }

            if ( scaley == 0 )
            {
                throw new ArgumentException( "scaleY cannot be 0." );
            }

            var x = scalex / ScaleX;
            var y = scaley / ScaleY;

            ScaleX = scalex;
            ScaleY = scaley;

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
        ///     Scales the font by the specified amount in both directions.
        /// </summary>
        /// See also
        /// <see cref="SetScale(float, float)" />
        /// <exception cref="ArgumentException">if scaleX or scaleY is zero.</exception>
        public void SetScale( float scaleXy ) => SetScale( scaleXy, scaleXy );

        /// <summary>
        ///     Sets the font's scale relative to the current scale.
        /// </summary>
        /// See also
        /// <see cref="SetScale(float, float)" />
        /// <exception cref="ArgumentException">if the resulting scale is zero.</exception>
        public void Scale( float amount ) => SetScale( ScaleX + amount, ScaleY + amount );

        public override string? ToString() => Name ?? base.ToString();
    }
}
