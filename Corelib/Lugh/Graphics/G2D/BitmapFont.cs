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

using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Collections;
using Corelib.Lugh.Utils.Exceptions;
using Exception = System.Exception;

namespace Corelib.Lugh.Graphics.G2D;

/// <summary>
/// Renders bitmap fonts. The font consists of 2 files: an image file or <see cref="TextureRegion"/>
/// containing the glyphs and a file in the <b>AngleCode BMFont</b> text format that describes where
/// each glyph is on the image.
/// <para>
/// Text is drawn using a <see cref="IBatch"/>. Text can be cached in a <see cref="BitmapFontCache"/>
/// for faster rendering of static text, which saves needing to compute the location of each glyph
/// each frame.
/// </para>
/// <para>
/// The texture for a BitmapFont loaded from a file is managed. <see cref="Dispose()"/> must be
/// called to free the texture when no longer needed. A BitmapFont which has loaded using a
/// <see cref="TextureRegion"/> is managed if the region's texture is managed. Disposing the
/// BitmapFont disposes the region's texture, which may not be desirable if the texture is still
/// being used elsewhere.
/// </para>
/// <para>
/// The code was originally based on Matthias Mann's TWL BitmapFont class. Thanks for sharing, Matthias! :)
/// </para>
/// </summary>
[PublicAPI]
public class BitmapFont
{
    private const string REGEX_PATTERN      = ".*id=(\\d+)";
    private const string DEFAULT_FONT       = "lsans-15.fnt";
    private const string DEFAULT_FONT_IMAGE = "lsans-15.png";
    private const int    LOG2_PAGE_SIZE     = 9;
    private const int    PAGE_SIZE          = 1 << LOG2_PAGE_SIZE;
    private const int    PAGES              = 0x10000 / PAGE_SIZE;

    // ========================================================================

    public bool Flipped     { get; set; }
    public bool OwnsTexture { get; set; }

    /// <summary>
    /// The BitmapFontCache used by this font, for rendering to a sprite batch.
    /// This can be used, for example, to manipulate glyph colors within a
    /// specific index.
    /// </summary>
    public BitmapFontCache Cache { get; set; }

    /// <summary>
    /// The underlying <see cref="BitmapFontData"/> for this BitmapFont.
    /// </summary>
    public BitmapFontData Data { get; set; }

    // ========================================================================

    private readonly PathTypes             _fileType;
    private readonly List< TextureRegion > _regions;

    private bool _integer;

    // ========================================================================

    /// <summary>
    /// Creates a BitmapFont using the default 15pt Arial font included in the library.
    /// This is convenient to easily display text without bothering without generating
    /// a bitmap font yourself.
    /// </summary>
    public BitmapFont()
        : this( GdxApi.Files.Internal( DEFAULT_FONT ).File, GdxApi.Files.Internal( DEFAULT_FONT_IMAGE ).File, false )
    {
        _fileType = PathTypes.Internal;
    }

    /// <summary>
    /// Creates a BitmapFont using the default 15pt Arial font included in the
    /// LughSharp project.
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
        : this( GdxApi.Files.Internal( DEFAULT_FONT ).File, GdxApi.Files.Internal( DEFAULT_FONT ).File, flip )
    {
        _fileType = PathTypes.Internal;
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
        _fileType = PathTypes.Local;
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
        : this( new BitmapFontData( fontFile, flip ), ( TextureRegion? ) null, true )
    {
        _fileType = PathTypes.Local;
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
        : this( new BitmapFontData( fontFile, flip ),
                new TextureRegion( new Texture( imageFile, false ) ),
                integer )
    {
        OwnsTexture = true;
        _fileType   = PathTypes.Local;
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
    /// <param name="data"> The BitmapFontData. </param>
    /// <param name="region"> The TextureRegion. </param>
    /// <param name="integer">
    /// If true, rendering positions will be at integer values to avoid filtering
    /// artifacts.
    /// </param>
    public BitmapFont( BitmapFontData data, TextureRegion? region, bool integer )
        : this( data, region != null ? ListExtensions.New( region ) : null, integer )
    {
        _fileType = PathTypes.Local;
    }

    /// <summary>
    /// Constructs a new BitmapFont from the given <see cref="BitmapFontData"/> and array
    /// of <see cref="TextureRegion"/>. If the TextureRegion is null or empty, the image
    /// path(s) will be read from the BitmapFontData. The dispose() method will not dispose
    /// the texture of the region(s) if the regions array is != null and not empty.
    /// </summary>
    /// <param name="data"> The BitmapFontData. </param>
    /// <param name="pageRegions"> The list of TextureRegions. </param>
    /// <param name="integer">
    /// If true, rendering positions will be at integer values to avoid filtering artifacts.
    /// </param>
    public BitmapFont( BitmapFontData data, List< TextureRegion >? pageRegions, bool integer )
    {
        Flipped             = data.Flipped;
        Data                = data;
        UseIntegerPositions = integer;
        _fileType           = PathTypes.Local;

        if ( ( pageRegions == null ) || ( pageRegions.Count == 0 ) )
        {
            if ( data.ImagePaths == null )
            {
                throw new ArgumentException
                    ( "If no regions are specified, the font data must have an images path." );
            }

            // Load each path.
            var n = data.ImagePaths.Length;

            _regions = new List< TextureRegion >( n );

            for ( var i = 0; i < n; i++ )
            {
                var file = data.FontFile == null
                               ? GdxApi.Files.Internal( data.ImagePaths[ i ] )
                               : GdxApi.Files.GetFileHandle( data.ImagePaths[ i ], _fileType );

                _regions.Add( new TextureRegion( new Texture( file.File, false ) ) );
            }

            OwnsTexture = true;
        }
        else
        {
            _regions    = pageRegions;
            OwnsTexture = false;
        }

        Cache = new BitmapFontCache( this, UseIntegerPositions );

        InitialLoad( data );
    }

    // ========================================================================

    /// <summary>
    /// Specifies whether to use integer positions.
    /// Default is to use them so filtering doesn't kick in as badly.
    /// </summary>
    public bool UseIntegerPositions
    {
        get => _integer;
        set
        {
            _integer                  = value;
            Cache.UseIntegerPositions = value;
        }
    }

    /// <summary>
    /// Returns the <see cref="BitmapFontData.ScaleX"/> value.
    /// </summary>
    public float GetScaleX() => Data.ScaleX;

    /// <summary>
    /// Returns the <see cref="BitmapFontData.ScaleY"/> value.
    /// </summary>
    public float GetScaleY() => Data.ScaleY;

    // ========================================================================

    /// <summary>
    /// Helper method, allowing a call to <see cref="Load(BitmapFontData)"/>,
    /// which is a <b>virtual</b> method, from constructors.
    /// </summary>
    private void InitialLoad( BitmapFontData data )
    {
        Load( data );
    }

    /// <summary>
    /// Loads the glyph regions for each glyph in the provided font data.
    /// </summary>
    /// <param name="data">
    /// The BitmapFontData containing information about the glyphs and their regions.
    /// </param>
    protected virtual void Load( BitmapFontData data )
    {
        // Iterate through each page of glyphs in the font data.
        foreach ( Glyph?[]? page in data.Glyphs )
        {
            // Skip null pages.
            if ( page == null )
            {
                continue;
            }

            // Iterate through each glyph in the page.
            foreach ( var glyph in page )
            {
                // Set the glyph region if the glyph is not null.
                if ( glyph != null )
                {
                    data.SetGlyphRegion( glyph, _regions[ glyph.Page ] );
                }
            }
        }

        // Set the glyph region for the missing glyph if it exists.
        if ( data.MissingGlyph != null )
        {
            data.MissingGlyph = data.SetGlyphRegion( data.MissingGlyph, _regions[ data.MissingGlyph.Page ] );
        }
    }

    // ========================================================================

    #region font drawing

    /// <summary>
    /// Draws text at the specified position.
    /// </summary>
    /// <param name="batch"> The <see cref="IBatch"/> to use. </param>
    /// <param name="str"> The text message to draw. </param>
    /// <param name="x"> X coordinate. </param>
    /// <param name="y"> Y coordinate. </param>
    public GlyphLayout Draw( IBatch batch, string str, float x, float y )
    {
        Cache.Clear();

        var layout = Cache.AddText( str, x, y );

        Cache.Draw( batch );

        return layout;
    }

    /// <summary>
    /// Draws text at the specified position.
    /// </summary>
    public GlyphLayout Draw( IBatch batch, string str, float x, float y, int targetWidth, int halign, bool wrap )
    {
        Cache.Clear();

        var layout = Cache.AddText( str, x, y, targetWidth, halign, wrap );

        Cache.Draw( batch );

        return layout;
    }

    /// <summary>
    /// Draws text at the specified position.
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
        Cache.Clear();

        var layout = Cache.AddText( str, x, y, start, end, targetWidth, halign, wrap );

        Cache.Draw( batch );

        return layout;
    }

    /// <summary>
    /// Draws text at the specified position.
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
        Cache.Clear();

        var layout = Cache.AddText( str, x, y, start, end, targetWidth, halign, wrap, truncate );

        Cache.Draw( batch );

        return layout;
    }

    /// <summary>
    /// Draws text at the specified position.
    /// </summary>
    public void Draw( IBatch batch, GlyphLayout layout, float x, float y )
    {
        Cache.Clear();
        Cache.AddText( layout, x, y );
        Cache.Draw( batch );
    }

    #endregion font drawing

    // ========================================================================

    /// <summary>
    /// Returns the color of text drawn with this font.
    /// </summary>
    public Color GetColor() => Cache.GetColor();

    /// <summary>
    /// A convenience method for setting the font color.
    /// </summary>
    public void SetColor( Color color ) => Cache.GetColor().Set( color );

    /// <summary>
    /// A convenience method for setting the font color.
    /// </summary>
    public void SetColor( float r, float g, float b, float a )
    {
        Cache.GetColor().Set( r, g, b, a );
    }

    /// <summary>
    /// Returns the first texture region. This is included for backwards compatibility,
    /// and for convenience since most fonts only use one texture page.
    /// <para>
    /// For multi-page fonts, use <see cref="GetRegions()"/>.
    /// </para>
    /// </summary>
    /// <returns>the first texture region</returns>
    public TextureRegion GetRegion() => _regions.First();

    /// <summary>
    /// Returns the array of TextureRegions that represents each texture page of glyphs.
    /// </summary>
    /// <returns>
    /// the array of texture regions; modifying it may produce undesirable results
    /// </returns>
    public List< TextureRegion > GetRegions() => _regions;

    /// <summary>
    /// Returns the texture page at the given index.
    /// </summary>
    public TextureRegion GetRegion( int index ) => _regions[ index ];

    /// <summary>
    /// Returns the line height, which is the distance from one line of text to the next.
    /// </summary>
    public float GetLineHeight() => Data.LineHeight;

    /// <summary>
    /// Returns the x-advance of the space character.
    /// </summary>
    public virtual float GetSpaceXadvance() => Data.SpaceXadvance;

    /// <summary>
    /// Returns the x-height, which is the distance from the top of most lowercase
    /// characters to the baseline.
    /// </summary>
    public float GetXHeight() => Data.XHeight;

    /// <summary>
    /// Returns the cap height, which is the distance from the top of most uppercase
    /// characters to the baseline. Since the drawing position is the cap height of
    /// the first line, the cap height can be used to get the location of the baseline.
    /// </summary>
    public float GetCapHeight() => Data.CapHeight;

    /// <summary>
    /// Returns the ascent, which is the distance from the cap height to the top of
    /// the tallest glyph.
    /// </summary>
    public float GetAscent() => Data.Ascent;

    /// <summary>
    /// Returns the descent, which is the distance from the bottom of the glyph that
    /// extends the lowest to the baseline. This number is negative.
    /// </summary>
    public float GetDescent() => Data.Descent;

    /// <summary>
    /// Makes the specified glyphs fixed width. This can be useful to make the numbers
    /// in a font fixed width. Eg, when horizontally centering a score or loading
    /// percentage text, it will not jump around as different numbers are shown.
    /// </summary>
    public void SetFixedWidthGlyphs( string glyphs )
    {
        var data       = Data;
        var maxAdvance = 0;

        for ( int index = 0, end = glyphs.Length; index < end; index++ )
        {
            var g = data.GetGlyph( glyphs[ index ] );

            if ( ( g != null ) && ( g.Xadvance > maxAdvance ) )
            {
                maxAdvance = g.Xadvance;
            }
        }

        for ( int index = 0, end = glyphs.Length; index < end; index++ )
        {
            var g = data.GetGlyph( glyphs[ index ] );

            if ( g == null )
            {
                continue;
            }

            g.Xoffset    += ( maxAdvance - g.Xadvance ) / 2;
            g.Xadvance   =  maxAdvance;
            g.Kerning    =  null;
            g.FixedWidth =  true;
        }
    }

    /// <summary>
    /// Creates a new BitmapFontCache for this font. Using this method allows the
    /// font to provide the BitmapFontCache implementation to customize rendering.
    /// </summary>
    /// <para>
    /// Note this method is called by the BitmapFont constructors. If a subclass
    /// overrides this method, it will be called before the subclass constructors.
    /// </para>
    public virtual BitmapFontCache NewFontCache()
    {
        return new BitmapFontCache( this, UseIntegerPositions );
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return Data.Name ?? base.ToString();
    }

    /// <summary>
    /// Returns the index of the character 'ch' in the supplied text string.
    /// Scanning for the character begins at the index specified by 'start'.
    /// </summary>
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
    /// Disposes the texture used by this BitmapFont's region IF this BitmapFont
    /// created the texture.
    /// </summary>
    public void Dispose()
    {
        if ( OwnsTexture )
        {
            foreach ( var t in _regions )
            {
                t.Texture.Dispose();
            }
        }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Represents a single character in a font page.
    /// </summary>
    [PublicAPI]
    public class Glyph
    {
        public bool       FixedWidth { get; set; }
        public int        Height     { get; set; }
        public int        ID         { get; set; }
        public byte[]?[]? Kerning    { get; set; }
        public int        SrcX       { get; set; }
        public int        SrcY       { get; set; }
        public float      U          { get; set; }
        public float      U2         { get; set; }
        public float      V          { get; set; }
        public float      V2         { get; set; }
        public int        Width      { get; set; }
        public int        Xadvance   { get; set; }
        public int        Xoffset    { get; set; }
        public int        Yoffset    { get; set; }

        /// <summary>
        /// The index to the texture page that holds this glyph.
        /// </summary>
        public int Page { get; set; }

        public int GetKerning( char ch )
        {
            if ( Kerning != null )
            {
                var page = Kerning[ ch >>> LOG2_PAGE_SIZE ];
                return page != null ? page[ ch & ( PAGE_SIZE - 1 ) ] : 0;
            }

            return 0;
        }

        public void SetKerning( int ch, int value )
        {
            Kerning ??= new byte[ PAGES ][];

            var page = Kerning[ ch >>> LOG2_PAGE_SIZE ];

            if ( page == null )
            {
                Kerning[ ch >>> LOG2_PAGE_SIZE ] = page = new byte[ PAGE_SIZE ];
            }

            page[ ch & ( PAGE_SIZE - 1 ) ] = ( byte ) value;
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }

    /// <summary>
    /// Backing data for a <see cref="BitmapFont"/>.
    /// </summary>
    [PublicAPI]
    public class BitmapFontData
    {
        public string?     Name          { get; set; }
        public string[]?   ImagePaths    { get; set; }
        public FileInfo    FontFile      { get; set; }
        public bool        Flipped       { get; set; }
        public float       PadTop        { get; set; }
        public float       PadRight      { get; set; }
        public float       PadBottom     { get; set; }
        public float       PadLeft       { get; set; }
        public float       ScaleX        { get; set; } = 1;
        public float       ScaleY        { get; set; } = 1;
        public bool        MarkupEnabled { get; set; }
        public Glyph?[]?[] Glyphs        { get; set; } = new Glyph[ PAGES ][];

        // ====================================================================

        /// <summary>
        /// Additional characters besides whitespace where text is wrapped.
        /// Eg, a hypen (-).
        /// </summary>
        public readonly char[]? BreakChars;

        public readonly char[] CapChars =
        [
            'M', 'N', 'B', 'D', 'C', 'E', 'F', 'K', 'A', 'G', 'H', 'I', 'J',
            'L', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        ];

        public readonly char[] XChars =
        [
            'x', 'e', 'a', 'o', 'n', 's', 'r', 'c', 'u', 'm', 'v', 'w', 'z',
        ];

        /// <summary>
        /// Creates an empty BitmapFontData for configuration before calling
        /// <see cref="Load(FileInfo, bool)"/>, to subclass, or to populate
        /// yourself, e.g. using stb-truetype or FreeType.
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

        /// <summary>
        /// The amount to add to the glyph X position when drawing a cursor between
        /// glyphs. This field is not set by the BMFont file, it needs to be set
        /// manually depending on how the glyphs are rendered on the backing textures.
        /// </summary>
        public float CursorX { get; set; }

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

        // ====================================================================

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

            Name = Path.GetFileNameWithoutExtension( file.Name );

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
                    catch ( Exception e )
                    {
                        Logger.Error( $"IGNORED NumberFormatException: {e.Message}" );
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
                    var rx      = new Regex( ".*id=(\\d+)" );
                    var matches = rx.Matches( line );

                    if ( matches.Count > 0 )
                    {
                        rx = new Regex( "\\d+" );
                        var id = rx.Matches( matches[ 0 ].Value )[ 0 ];

                        try
                        {
                            var pageID = int.Parse( id.Value );

                            if ( pageID != p )
                            {
                                throw new GdxRuntimeException( $"Page IDs must be indices starting at 0: {id}" );
                            }
                        }
                        catch ( Exception ex )
                        {
                            throw new GdxRuntimeException( $"Invalid page id: {id}", ex );
                        }
                    }

                    rx = new Regex( ".*file=\"?([^\"]+)\"?" );

                    matches = rx.Matches( line );

                    if ( matches.Count <= 0 )
                    {
                        throw new GdxRuntimeException( "Missing: file" );
                    }

                    ImagePaths[ p ] = FontFile.FullName.Replace( @"\\\\", "/" );
                }

                Descent = 0;

                while ( true )
                {
                    line = reader.ReadLine();

                    if ( line == null )
                    {
                        break; // EOF
                    }

                    Logger.Debug( line! );

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

                    var glyph  = new Glyph();
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

                    glyph.ID = ch;
                    tokens.NextToken();

                    glyph.SrcX = int.Parse( tokens.NextToken() );
                    tokens.NextToken();

                    glyph.SrcY = int.Parse( tokens.NextToken() );
                    tokens.NextToken();

                    glyph.Width = int.Parse( tokens.NextToken() );
                    tokens.NextToken();

                    glyph.Height = int.Parse( tokens.NextToken() );
                    tokens.NextToken();

                    glyph.Xoffset = int.Parse( tokens.NextToken() );
                    tokens.NextToken();

                    if ( flip )
                    {
                        glyph.Yoffset = int.Parse( tokens.NextToken() );
                    }
                    else
                    {
                        glyph.Yoffset = -( glyph.Height + int.Parse( tokens.NextToken() ) );
                    }

                    tokens.NextToken();

                    glyph.Xadvance = int.Parse( tokens.NextToken() );

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
                        catch ( Exception ignored )
                        {
                            Logger.Error( $"IGNORED NumberFormatException: {ignored.Message}" );
                        }
                    }

                    if ( glyph is { Width: > 0, Height: > 0 } )
                    {
                        Descent = Math.Min( baseLine + glyph.Yoffset, Descent );
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

                    Logger.Debug( line );

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

                    var glyph = GetGlyph( ( char ) first );

                    tokens.NextToken();

                    var amount = int.Parse( tokens.NextToken() );

                    // Kernings may exist for glyph pairs not contained in the font.
                    glyph?.SetKerning( second, amount );
                }

                var hasMetricsOverride = false;

                var overrideAscent        = 0f;
                var overrideDescent       = 0f;
                var overrideDown          = 0f;
                var overrideCapHeight     = 0f;
                var overrideLineHeight    = 0f;
                var overrideSpaceXAdvance = 0f;
                var overrideXHeight       = 0f;

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

                var spaceGlyph = GetGlyph( ' ' );

                if ( spaceGlyph == null )
                {
                    spaceGlyph = new Glyph { ID = ' ' };

                    var xadvanceGlyph = GetGlyph( 'l' ) ?? GetFirstGlyph();

                    spaceGlyph.Xadvance = xadvanceGlyph.Xadvance;

                    SetGlyph( ' ', spaceGlyph );
                }

                if ( spaceGlyph.Width == 0 )
                {
                    spaceGlyph.Width   = ( int ) ( PadLeft + spaceGlyph.Xadvance + PadRight );
                    spaceGlyph.Xoffset = ( int ) -PadLeft;
                }

                SpaceXadvance = spaceGlyph.Xadvance;

                Glyph? xGlyph = null;

                foreach ( var xChar in XChars )
                {
                    xGlyph = GetGlyph( xChar );

                    if ( xGlyph != null )
                    {
                        break;
                    }
                }

                xGlyph ??= GetFirstGlyph();

                XHeight = xGlyph.Height - padY;

                Glyph? capGlyph = null;

                foreach ( var capChar in CapChars )
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

                        foreach ( var glyph in page )
                        {
                            if ( ( glyph == null )
                              || ( glyph.Height == 0 )
                              || ( glyph.Width == 0 ) )
                            {
                                continue;
                            }

                            CapHeight = Math.Max( CapHeight, glyph.Height );
                        }
                    }
                }
                else
                {
                    CapHeight = capGlyph.Height;
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
        /// A reference to the Glyph whose region is to be set.
        /// </param>
        /// <param name="region"> The <see cref="TextureRegion"/>. </param>
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
                offsetX = ( int ) atlasRegion.OffsetX;

                offsetY = ( int ) ( atlasRegion.OriginalHeight
                                  - atlasRegion.PackedHeight
                                  - atlasRegion.OffsetY );
            }

            var x  = glyph.SrcX;
            var x2 = glyph.SrcX + glyph.Width;
            var y  = glyph.SrcY;
            var y2 = glyph.SrcY + glyph.Height;

            // Shift glyph for left and top edge stripped whitespace.
            // Clip glyph for right and bottom edge stripped whitespace.
            // Note: if the font region has padding, whitespace stripping must not be used.
            if ( offsetX > 0 )
            {
                x -= offsetX;

                if ( x < 0 )
                {
                    glyph.Width   += x;
                    glyph.Xoffset -= x;

                    x = 0;
                }

                x2 -= offsetX;

                if ( x2 > region.RegionWidth )
                {
                    glyph.Width -= x2 - region.RegionWidth;

                    x2 = region.RegionWidth;
                }
            }

            if ( offsetY > 0 )
            {
                y -= offsetY;

                if ( y < 0 )
                {
                    glyph.Height += y;

                    if ( glyph.Height < 0 )
                    {
                        glyph.Height = 0;
                    }

                    y = 0;
                }

                y2 -= offsetY;

                if ( y2 > region.RegionHeight )
                {
                    var amount = y2 - region.RegionHeight;

                    glyph.Height  -= amount;
                    glyph.Yoffset += amount;

                    y2 = region.RegionHeight;
                }
            }

            glyph.U  = u + ( x * invTexWidth );
            glyph.U2 = u + ( x2 * invTexWidth );

            if ( Flipped )
            {
                glyph.V  = v + ( y * invTexHeight );
                glyph.V2 = v + ( y2 * invTexHeight );
            }
            else
            {
                glyph.V2 = v + ( y * invTexHeight );
                glyph.V  = v + ( y2 * invTexHeight );
            }

            return glyph;
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
                    foreach ( var glyph in page )
                    {
                        if ( ( glyph == null ) || ( glyph.Height == 0 ) || ( glyph.Width == 0 ) )
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
        /// Returns true if the font has the glyph, or if the font has a <see cref="MissingGlyph"/>.
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
        /// Returns the glyph for the specified character, or null if no such
        /// glyph exists. Note that
        /// </summary>
        /// See also
        /// <see cref="GetGlyphs"/>
        /// should be be used to shape a string
        /// of characters into a list of glyphs.
        public virtual Glyph? GetGlyph( char ch )
        {
            return Glyphs[ ch / PAGE_SIZE ]?[ ch & ( PAGE_SIZE - 1 ) ];
        }

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
        public virtual void GetGlyphs( GlyphLayout.GlyphRun? run, string str, int start, int end, Glyph? lastGlyph )
        {
            ArgumentNullException.ThrowIfNull( run );

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

                var glyph = GetGlyph( ch );

                if ( glyph == null )
                {
                    if ( MissingGlyph == null )
                    {
                        continue;
                    }

                    glyph = MissingGlyph;
                }

                glyphs.Add( glyph );

                xAdvances.Add( lastGlyph == null // First glyph on line, adjust the position so it isn't drawn left of 0.
                                   ? glyph.FixedWidth ? 0 : ( -glyph.Xoffset * scaleX ) - PadLeft
                                   : ( lastGlyph.Xadvance + lastGlyph.GetKerning( ch ) ) * scaleX );

                lastGlyph = glyph;

                // "[[" is an escaped left square bracket, skip second character.
                if ( markupEnabled
                  && ( ch == '[' )
                  && ( start < end )
                  && ( str[ start ] == '[' ) )
                {
                    start++;
                }
            } while ( start < end );

            if ( lastGlyph != null )
            {
                var lastGlyphWidth = lastGlyph.FixedWidth
                                         ? lastGlyph.Xadvance * scaleX
                                         : ( ( lastGlyph.Width + lastGlyph.Xoffset ) * scaleX ) - PadRight;

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
            var ch = ( char ) glyphList[ i ].ID;

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
                ch = ( char ) glyphList[ i ].ID;

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
            if ( BreakChars == null )
            {
                return false;
            }

            foreach ( var br in BreakChars )
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
        /// Scales the font by the specified amounts on both axes
        /// <para>
        /// Note that smoother scaling can be achieved if the texture backing
        /// the BitmapFont is using <see cref="Texture.TextureFilter.Linear"/>.
        /// The default is Nearest, so use a BitmapFont constructor that takes
        /// a <see cref="TextureRegion"/>.
        /// </para>
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
        /// Scales the font by the specified amount in both directions.
        /// </summary>
        /// See also
        /// <see cref="SetScale(float, float)"/>
        /// <exception cref="ArgumentException">if scaleX or scaleY is zero.</exception>
        public void SetScale( float scaleXy )
        {
            SetScale( scaleXy, scaleXy );
        }

        /// <summary>
        /// Sets the font's scale relative to the current scale.
        /// </summary>
        /// See also
        /// <see cref="SetScale(float, float)"/>
        /// <exception cref="ArgumentException">if the resulting scale is zero.</exception>
        public void Scale( float amount )
        {
            SetScale( ScaleX + amount, ScaleY + amount );
        }

        /// <inheritdoc />
        public override string? ToString()
        {
            return Name ?? base.ToString();
        }
    }
}