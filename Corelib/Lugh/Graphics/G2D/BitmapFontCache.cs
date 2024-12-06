// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;
using Corelib.Lugh.Utils.Pooling;

namespace Corelib.Lugh.Graphics.G2D;

/// <summary>
/// Caches glyph geometry for a BitmapFont, providing a fast way to render
/// static text. This saves needing to compute the glyph geometry each frame.
/// </summary>
[PublicAPI]
public class BitmapFontCache
{
    private readonly Color                _color         = new( 1, 1, 1, 1 );
    private readonly List< GlyphLayout? > _pooledLayouts = new();
    private readonly Color                _tempColor     = new( 1, 1, 1, 1 );
    private          float                _currentTint;
    private          int                  _glyphCount;

    /// <summary>
    /// Number of vertex data entries per page.
    /// </summary>
    private int[] _idx;

    /// <summary>
    /// For each page, an array with a value for each glyph from that page, where
    /// the value is the index of the character in the full text being cached.
    /// </summary>
    private List< int >[]? _pageGlyphIndices;

    /// <summary>
    /// Vertex data per page.
    /// </summary>
    private float[]?[] _pageVertices;

    /// <summary>
    /// Used internally to ensure a correct capacity for multi-page font vertex data.
    /// </summary>
    private int[] _tempGlyphCount;

    // ========================================================================

    public BitmapFontCache( BitmapFont font )
        : this( font, font.UseIntegerPositions )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="font"></param>
    /// <param name="integer">
    /// If true, rendering positions will be at integer values to avoid filtering artifacts.
    /// </param>
    /// <exception cref="ArgumentException"></exception>
    public BitmapFontCache( BitmapFont font, bool integer )
    {
        Font                = font;
        UseIntegerPositions = integer;

        var pageCount = font.GetRegions().Count;

        if ( pageCount == 0 )
        {
            throw new ArgumentException( "The specified font must contain at least one texture page." );
        }

        _pageVertices = new float[ pageCount ][];
        _idx          = new int[ pageCount ];

        if ( pageCount > 1 )
        {
            // Contains the indices of the glyph in the cache as they are added.
            _pageGlyphIndices = new List< int >[ pageCount ];

            for ( int i = 0, n = _pageGlyphIndices.Length; i < n; i++ )
            {
                _pageGlyphIndices[ i ] = new List< int >();
            }
        }

        _tempGlyphCount = new int[ pageCount ];
    }

    /// <summary>
    /// Returns the x position of the cached string, relative to the
    /// position when the string was cached.
    /// </summary>
    public float X { get; private set; }

    /// <summary>
    /// Returns the y position of the cached string, relative to the
    /// position when the string was cached.
    /// </summary>
    public float Y { get; private set; }

    public BitmapFont Font { get; }

    /// <summary>
    /// Specifies whether to use integer positions or not.
    /// Default is to use them so filtering doesn't kick in as badly.
    /// </summary>
    public bool UseIntegerPositions { get; set; }

    /// <summary>
    /// Sets the position of the text, relative to the position when
    /// the cached text was created.
    /// </summary>
    /// <param name="x"> The x coordinate </param>
    /// <param name="y"> The y coordinate </param>
    public void SetPosition( float x, float y )
    {
        Translate( x - X, y - Y );
    }

    /// <summary>
    /// Sets the position of the text, relative to its current position.
    /// </summary>
    /// <param name="xAmount"> The amount in x to move the text </param>
    /// <param name="yAmount"> The amount in y to move the text </param>
    public void Translate( float xAmount, float yAmount )
    {
        if ( ( xAmount == 0 ) && ( yAmount == 0 ) )
        {
            return;
        }

        if ( UseIntegerPositions )
        {
            xAmount = ( float )Math.Round( xAmount );
            yAmount = ( float )Math.Round( yAmount );
        }

        X += xAmount;
        Y += yAmount;

        for ( int i = 0, n = _pageVertices.Length; i < n; i++ )
        {
            var vertices = _pageVertices[ i ];

            for ( int ii = 0, nn = _idx[ i ]; ii < nn; ii += 5 )
            {
                vertices![ ii ]    += xAmount;
                vertices[ ii + 1 ] += yAmount;
            }
        }
    }

    /// <summary>
    /// Tints all text currently in the cache.Does not affect subsequently added text.
    /// </summary>
    public void Tint( Color tint )
    {
        var newTint = tint.ToFloatBitsABGR();

        if ( _currentTint.Equals( newTint ) )
        {
            return;
        }

        _currentTint = newTint;

        var tempGlyphCount = _tempGlyphCount;

        for ( int i = 0, n = tempGlyphCount.Length; i < n; i++ )
        {
            tempGlyphCount[ i ] = 0;
        }

        for ( int i = 0, n = Layouts.Count; i < n; i++ )
        {
            var layout = Layouts[ i ];

            for ( int ii = 0, nn = layout.Runs.Count; ii < nn; ii++ )
            {
                var run        = layout.Runs[ ii ];
                var glyphs     = run.Glyphs;
                var colorFloat = _tempColor.Set( run.Color ).Mul( tint ).ToFloatBitsABGR();

                for ( int iii = 0, nnn = glyphs.Count; iii < nnn; iii++ )
                {
                    var glyph  = glyphs[ iii ];
                    var page   = glyph.Page;
                    var offset = ( tempGlyphCount[ page ] * 20 ) + 2;

                    tempGlyphCount[ page ]++;

                    var vertices = _pageVertices[ page ];

                    for ( var v = 0; v < 20; v += 5 )
                    {
                        vertices![ offset + v ] = colorFloat;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets the alpha component of all text currently in the cache.
    /// Does not affect subsequently added text.
    /// </summary>
    public void SetAlphas( float alpha )
    {
        var   alphaBits = ( int )( 254 * alpha ) << 24;
        float prev      = 0, newColor = 0;

        for ( int j = 0, length = _pageVertices.Length; j < length; j++ )
        {
            var vertices = _pageVertices[ j ];

            for ( int i = 2, n = _idx[ j ]; i < n; i += 5 )
            {
                var c = vertices![ i ];

                if ( c.Equals( prev ) && !( ( float )i ).Equals( 2f ) )
                {
                    vertices[ i ] = newColor;
                }
                else
                {
                    prev = c;

                    var rgba = NumberUtils.FloatToIntColor( c );

                    rgba          = ( rgba & 0x00FFFFFF ) | alphaBits;
                    newColor      = NumberUtils.IntToFloatColor( rgba );
                    vertices[ i ] = newColor;
                }
            }
        }
    }

    /// <summary>
    /// Sets the color of all text currently in the cache.
    /// Does not affect subsequently added text.
    /// </summary>
    public void SetColors( float color )
    {
        for ( int j = 0, length = _pageVertices.Length; j < length; j++ )
        {
            var vertices = _pageVertices[ j ];

            for ( int i = 2, n = _idx[ j ]; i < n; i += 5 )
            {
                vertices![ i ] = color;
            }
        }
    }

    /// <summary>
    /// Sets the color of all text currently in the cache.
    /// Does not affect subsequently added text.
    /// </summary>
    public void SetColors( Color tint )
    {
        SetColors( tint.ToFloatBitsABGR() );
    }

    /// <summary>
    /// Sets the color of all text currently in the cache. Does not affect subsequently added text.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    public void SetColors( float r, float g, float b, float a )
    {
        var intBits = ( ( int )( 255 * a ) << 24 )
                      | ( ( int )( 255 * b ) << 16 )
                      | ( ( int )( 255 * g ) << 8 )
                      | ( int )( 255 * r );

        SetColors( NumberUtils.IntToFloatColor( intBits ) );
    }

    /// <summary>
    /// Sets the color of the specified characters. This may only be called
    /// after <see cref="SetText(string, float, float)"/> and is reset every
    /// time setText is called.
    /// </summary>
    public void SetColors( Color tint, int start, int end )
    {
        SetColors( tint.ToFloatBitsABGR(), start, end );
    }

    /// <summary>
    /// Sets the color of the specified characters. This may only be called
    /// after <see cref="SetText(string, float, float)"/> and is reset every
    /// time setText is called.
    /// </summary>
    public void SetColors( float color, int start, int end )
    {
        if ( _pageVertices.Length == 1 )
        {
            // One page.
            var vertices = _pageVertices[ 0 ];

            for ( int i = ( start * 20 ) + 2, n = Math.Min( end * 20, _idx[ 0 ] ); i < n; i += 5 )
            {
                vertices![ i ] = color;
            }

            return;
        }

        var pageCount = _pageVertices.Length;

        for ( var i = 0; i < pageCount; i++ )
        {
            var vertices     = _pageVertices[ i ];
            var glyphIndices = _pageGlyphIndices![ i ];

            // Loop through the indices and determine whether the glyph is inside begin/end.
            for ( int j = 0, n = glyphIndices.Count; j < n; j++ )
            {
                var glyphIndex = glyphIndices[ j ];

                // Break early if the glyph is out of bounds.
                if ( glyphIndex >= end )
                {
                    break;
                }

                // If inside start and end, change its colour.
                if ( glyphIndex >= start )
                {
                    // && glyphIndex < end
                    for ( var off = 0; off < 20; off += 5 )
                    {
                        vertices![ off + ( j * 20 ) + 2 ] = color;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns the color used for subsequently added text. Modifying the color
    /// affects text subsequently added to the cache, but does not affect existing
    /// text currently in the cache.
    /// </summary>
    public Color GetColor()
    {
        return _color;
    }

    /// <summary>
    /// A convenience method for setting the cache color. The color can also
    /// be set by modifying <see cref="GetColor()"/>.
    /// </summary>
    public void SetColor( Color col )
    {
        _color.Set( col );
    }

    /// <summary>
    /// A convenience method for setting the cache color. The color can
    /// also be set by modifying {@link #getColor()}.
    /// </summary>
    public void SetColor( float r, float g, float b, float a )
    {
        _color.Set( r, g, b, a );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteBatch"></param>
    public virtual void Draw( IBatch spriteBatch )
    {
        var regions = Font.GetRegions();

        for ( int j = 0, n = _pageVertices.Length; j < n; j++ )
        {
            if ( _idx[ j ] > 0 )
            {
                // ignore if this texture has no glyphs
                var vertices = _pageVertices[ j ];

                spriteBatch.Draw( regions[ j ].Texture, vertices!, 0, _idx[ j ] );
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    protected virtual void Draw( IBatch spriteBatch, int start, int end )
    {
        if ( _pageVertices.Length == 1 )
        {
            // 1 page.
            spriteBatch.Draw( Font.GetRegion().Texture, _pageVertices[ 0 ]!, start * 20, ( end - start ) * 20 );

            return;
        }

        // Determine vertex offset and count to render for each page. Some pages might not need to be rendered at all.
        var regions = Font.GetRegions();

        for ( int i = 0, pageCount = _pageVertices.Length; i < pageCount; i++ )
        {
            int offset = -1, count = 0;

            // For each set of glyph indices, determine where to begin within the start/end bounds.
            var glyphIndices = _pageGlyphIndices![ i ];

            for ( int ii = 0, n = glyphIndices.Count; ii < n; ii++ )
            {
                var glyphIndex = glyphIndices[ ii ];

                // Break early if the glyph is out of bounds.
                if ( glyphIndex >= end )
                {
                    break;
                }

                // Determine if this glyph is within bounds. Use the first match of that for the offset.
                if ( ( offset == -1 ) && ( glyphIndex >= start ) )
                {
                    offset = ii;
                }

                // Determine the vertex count by counting glyphs within bounds.
                if ( glyphIndex >= start )
                {
                    count++;
                }
            }

            // Page doesn't need to be rendered.
            if ( ( offset == -1 ) || ( count == 0 ) )
            {
                continue;
            }

            // Render the page vertex data with the offset and count.
            spriteBatch.Draw( regions[ i ].Texture, _pageVertices[ i ]!, offset * 20, count * 20 );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="alphaModulation"></param>
    public void Draw( IBatch spriteBatch, float alphaModulation )
    {
        if ( alphaModulation.Equals( 1 ) )
        {
            Draw( spriteBatch );

            return;
        }

        var color    = GetColor();
        var oldAlpha = color.A;

        color.A *= alphaModulation;
        SetColors( color );
        Draw( spriteBatch );
        color.A = oldAlpha;
        SetColors( color );
    }

    /// <summary>
    /// Removes all glyphs in the cache.
    /// </summary>
    public void Clear()
    {
        X = 0;
        Y = 0;

        Pools< GlyphLayout? >.FreeAll( _pooledLayouts, true );

        _pooledLayouts.Clear();
        Layouts.Clear();

        for ( int i = 0, n = _idx.Length; i < n; i++ )
        {
            if ( _pageGlyphIndices != null )
            {
                _pageGlyphIndices[ i ].Clear();
            }

            _idx[ i ] = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="layout"></param>
    private void RequireGlyphs( GlyphLayout layout )
    {
        if ( _pageVertices.Length == 1 )
        {
            // Simpler counting if we just have one page.
            var newGlyphCount = 0;

            for ( int i = 0, n = layout.Runs.Count; i < n; i++ )
            {
                newGlyphCount += layout.Runs[ i ].Glyphs.Count;
            }

            RequirePageGlyphs( 0, newGlyphCount );
        }
        else
        {
            var tempGlyphCount = _tempGlyphCount;

            for ( int i = 0, n = tempGlyphCount.Length; i < n; i++ )
            {
                tempGlyphCount[ i ] = 0;
            }

            // Determine # of glyphs in each page.
            for ( int i = 0, n = layout.Runs.Count; i < n; i++ )
            {
                var glyphs = layout.Runs[ i ].Glyphs;

                for ( int ii = 0, nn = glyphs.Count; ii < nn; ii++ )
                {
                    tempGlyphCount[ glyphs[ ii ].Page ]++;
                }
            }

            // Require that many for each page.
            for ( int i = 0, n = tempGlyphCount.Length; i < n; i++ )
            {
                RequirePageGlyphs( i, tempGlyphCount[ i ] );
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="glyphCount"></param>
    private void RequirePageGlyphs( int page, int glyphCount )
    {
        if ( _pageGlyphIndices != null )
        {
            if ( glyphCount > _pageGlyphIndices[ page ].Count )
            {
                _pageGlyphIndices[ page ].EnsureCapacity( glyphCount - _pageGlyphIndices[ page ].Count );
            }
        }

        var vertexCount = _idx[ page ] + ( glyphCount * 20 );
        var vertices    = _pageVertices[ page ];

        if ( vertices == null )
        {
            _pageVertices[ page ] = new float[ vertexCount ];
        }
        else if ( vertices.Length < vertexCount )
        {
            var newVertices = new float[ vertexCount ];
            Array.Copy( vertices, 0, newVertices, 0, _idx[ page ] );
            _pageVertices[ page ] = newVertices;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void AddToCache( GlyphLayout layout, float x, float y )
    {
        // Check if the number of font pages has changed.
        var pageCount = Font.GetRegions().Count;

        if ( _pageVertices.Length < pageCount )
        {
            var newPageVertices = new float[ pageCount ][];
            Array.Copy( _pageVertices, 0, newPageVertices, 0, _pageVertices.Length );
            _pageVertices = newPageVertices;

            var newIdx = new int[ pageCount ];
            Array.Copy( _idx, 0, newIdx, 0, _idx.Length );
            _idx = newIdx;

            var newPageGlyphIndices    = new List< int >[ pageCount ];
            var pageGlyphIndicesLength = 0;

            if ( _pageGlyphIndices != null )
            {
                pageGlyphIndicesLength = _pageGlyphIndices.Length;
                Array.Copy( _pageGlyphIndices, 0, newPageGlyphIndices, 0, _pageGlyphIndices.Length );
            }

            for ( var i = pageGlyphIndicesLength; i < pageCount; i++ )
            {
                newPageGlyphIndices[ i ] = new List< int >();
            }

            _pageGlyphIndices = newPageGlyphIndices;
            _tempGlyphCount   = new int[ pageCount ];
        }

        Layouts.Add( layout );

        RequireGlyphs( layout );

        for ( int i = 0, n = layout.Runs.Count; i < n; i++ )
        {
            var run       = layout.Runs[ i ];
            var glyphs    = run.Glyphs;
            var xAdvances = run.XAdvances;

            var color = run.Color.ToFloatBitsABGR();
            var gx    = x + run.X;
            var gy    = y + run.Y;

            for ( int ii = 0, nn = glyphs.Count; ii < nn; ii++ )
            {
                var glyph = glyphs[ ii ];

                gx += xAdvances[ ii ];

                AddGlyph( glyph, gx, gy, color );
            }
        }

        // Cached glyphs have changed, reset the current tint.
        _currentTint = Color.WhiteFloatBits;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="glyph"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    private void AddGlyph( BitmapFont.Glyph glyph, float x, float y, float color )
    {
        var scaleX = Font.Data.ScaleX;
        var scaleY = Font.Data.ScaleY;

        x += glyph.Xoffset * scaleX;
        y += glyph.Yoffset * scaleY;

        var width  = glyph.Width * scaleX;
        var height = glyph.Height * scaleY;
        var u      = glyph.U;
        var u2     = glyph.U2;
        var v      = glyph.V;
        var v2     = glyph.V2;

        if ( UseIntegerPositions )
        {
            x      = ( float )Math.Round( x );
            y      = ( float )Math.Round( y );
            width  = ( float )Math.Round( width );
            height = ( float )Math.Round( height );
        }

        var x2 = x + width;
        var y2 = y + height;

        var page = glyph.Page;
        var idx  = _idx[ page ];

        _idx[ page ] += 20;

        if ( _pageGlyphIndices != null )
        {
            _pageGlyphIndices[ page ].Add( _glyphCount++ );
        }

        _pageVertices[ page ]![ idx++ ] = x;
        _pageVertices[ page ]![ idx++ ] = y;
        _pageVertices[ page ]![ idx++ ] = color;
        _pageVertices[ page ]![ idx++ ] = u;
        _pageVertices[ page ]![ idx++ ] = v;

        _pageVertices[ page ]![ idx++ ] = x;
        _pageVertices[ page ]![ idx++ ] = y2;
        _pageVertices[ page ]![ idx++ ] = color;
        _pageVertices[ page ]![ idx++ ] = u;
        _pageVertices[ page ]![ idx++ ] = v2;

        _pageVertices[ page ]![ idx++ ] = x2;
        _pageVertices[ page ]![ idx++ ] = y2;
        _pageVertices[ page ]![ idx++ ] = color;
        _pageVertices[ page ]![ idx++ ] = u2;
        _pageVertices[ page ]![ idx++ ] = v2;

        _pageVertices[ page ]![ idx++ ] = x2;
        _pageVertices[ page ]![ idx++ ] = y;
        _pageVertices[ page ]![ idx++ ] = color;
        _pageVertices[ page ]![ idx++ ] = u2;
        _pageVertices[ page ]![ idx ]   = v;
    }

    /// <summary>
    /// Clears any cached glyphs and adds glyphs for the specified text.
    /// <see cref="AddText(string, float, float, int, int, float, int, bool, string)"/>
    /// </summary>
    public GlyphLayout SetText( string str, float x, float y )
    {
        Clear();

        return AddText( str, x, y, 0, str.Length, 0, Align.LEFT, false );
    }

    /// <summary>
    /// Clears any cached glyphs and adds glyphs for the specified text.
    /// <see cref="AddText(string, float, float, int, int, float, int, bool, string)"/>
    /// </summary>
    public GlyphLayout SetText( string str, float x, float y, float targetWidth, int halign, bool wrap )
    {
        Clear();

        return AddText( str, x, y, 0, str.Length, targetWidth, halign, wrap );
    }

    /// <summary>
    /// Clears any cached glyphs and adds glyphs for the specified text.
    /// <see cref="AddText(string, float, float, int, int, float, int, bool, string)"/>
    /// </summary>
    public GlyphLayout SetText( string str,
                                float x,
                                float y,
                                int start,
                                int end,
                                float targetWidth,
                                int halign,
                                bool wrap,
                                string? truncate = null )
    {
        Clear();

        return AddText( str, x, y, start, end, targetWidth, halign, wrap, truncate );
    }

    /// <summary>
    /// Clears any cached glyphs and adds glyphs for the specified text.
    /// <see cref="AddText(string, float, float, int, int, float, int, bool, string)"/>
    /// </summary>
    public void SetText( GlyphLayout layout, float x, float y )
    {
        Clear();
        AddText( layout, x, y );
    }

    /// <summary>
    /// Adds glyphs for the specified text.
    /// </summary>
    public GlyphLayout AddText( string str, float x, float y, float targetWidth, int halign, bool wrap )
    {
        return AddText( str, x, y, 0, str.Length, targetWidth, halign, wrap );
    }

    /// <summary>
    /// Adds glyphs for the the specified text.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="x"> The x position for the left most character. </param>
    /// <param name="y">
    /// The y position for the top of most capital letters in the font
    /// (the <see cref="BitmapFont.BitmapFontData.CapHeight"/>).
    /// </param>
    /// <param name="start"> The first character of the string to draw. </param>
    /// <param name="end"> The last character of the string to draw (exclusive). </param>
    /// <param name="targetWidth"> The width of the area the text will be drawn, for wrapping or truncation. </param>
    /// <param name="halign"> Horizontal alignment of the text, see <see cref="Align"/>. </param>
    /// <param name="wrap"> If true, the text will be wrapped within targetWidth. </param>
    /// <param name="truncate">
    /// If not null, the text will be truncated within targetWidth with this string appended.
    /// May be an empty string.
    /// </param>
    /// <returns>
    /// The glyph layout for the cached string (the layout's height is the distance from y to the baseline).
    /// </returns>
    public GlyphLayout AddText( string str,
                                float x,
                                float y,
                                int start,
                                int end,
                                float targetWidth,
                                int halign,
                                bool wrap,
                                string? truncate = null )
    {
        var layout = Pools< GlyphLayout >.Obtain() ?? throw new GdxRuntimeException( "Unable to obtain layout!" );

        _pooledLayouts.Add( layout );

        layout.SetText( Font, str, start, end, _color, targetWidth, halign, wrap, truncate );
        AddText( layout, x, y );

        return layout;
    }

    /// <summary>
    /// Adds glyphs for the specified text.
    /// </summary>
    public GlyphLayout AddText( string str, float x, float y )
    {
        return AddText( str, x, y, 0, str.Length, 0, Align.LEFT, false );
    }

    /// <summary>
    /// Adds the specified glyphs.
    /// </summary>
    public void AddText( GlyphLayout? layout, float x, float y )
    {
        if ( layout != null )
        {
            AddToCache( layout, x, y + Font.Data.Ascent );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public float[]? GetVertices( int page = 0 ) => _pageVertices[ page ];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public int GetVertexCount( int page ) => _idx[ page ];

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public List< GlyphLayout > Layouts { get; set; } = [ ];
}