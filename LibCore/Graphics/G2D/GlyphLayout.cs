﻿// ///////////////////////////////////////////////////////////////////////////////
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


using System.Text;

using LughSharp.LibCore.Utils.Collections.Extensions;
using LughSharp.LibCore.Utils.Pooling;

namespace LughSharp.LibCore.Graphics.G2D;

/// <summary>
///     Stores <see cref="GlyphRun" /> runs of glyphs for a piece of text. The text
///     may contain newlines and color markup tags.
///     <para>
///         Where wrapping occurs is determined by <see cref="BitmapFont.BitmapFontData.GetWrapIndex" />.
///         Additionally, when <see cref="BitmapFont.BitmapFontData.MarkupEnabled" /> is true wrapping
///         can occur at color start or end tags.
///     </para>
///     <para>
///         When wrapping occurs, whitespace is removed before and after the wrap position.
///         Whitespace is determined by <see cref="BitmapFont.BitmapFontData.IsWhitespace(char)" />.
///     </para>
///     <para>
///         Glyphs positions are determined by <see cref="BitmapFont.BitmapFontData.GetGlyphs" />.
///     </para>
///     <para>
///         This class is not thread safe, even if synchronized externally, and must only
///         be used from the game thread.
///     </para>
/// </summary>
[PublicAPI]
public class GlyphLayout : IPoolable
{
    private readonly static float         _epsilon    = 0.0001f;
    private readonly        Pool< Color > _colorPool  = Pools< Color >.Get();
    private readonly        List< Color > _colorStack = new( 4 );

    // ------------------------------------------------------------------------

    private readonly Pool< GlyphRun > _glyphRunPool = Pools< GlyphRun >.Get();

    // ------------------------------------------------------------------------

    /// <summary>
    ///     Creates an empty GlyphLayout.
    /// </summary>
    public GlyphLayout()
    {
    }

    public GlyphLayout( BitmapFont font, string str )
    {
        SetText( font, str );
    }

    public GlyphLayout( BitmapFont font, string str, Color color, float targetWidth, int halign, bool wrap )
    {
        SetText( font, str, color, targetWidth, halign, wrap );
    }

    public GlyphLayout( BitmapFont font,
                        string str,
                        int start,
                        int end,
                        Color color,
                        float targetWidth,
                        int halign,
                        bool wrap,
                        string truncate )
    {
        SetText( font, str, start, end, color, targetWidth, halign, wrap, truncate );
    }

    public List< GlyphRun > Runs   { get; set; } = new( 1 );
    public float            Width  { get; set; }
    public float            Height { get; set; }

    /// <summary>
    ///     Resets the object for reuse. Object references should
    ///     be nulled and fields may be set to default values.
    /// </summary>
    public void Reset()
    {
        Pools< GlyphRun >.Get().FreeAll( Runs );
        Runs.Clear();

        Width  = 0;
        Height = 0;
    }

    /// <summary>
    ///     Calls <see cref="SetText(BitmapFont, string, int, int, Color, float, int, bool, string)" />
    ///     with the whole string, the font's current color, and no alignment or wrapping.
    /// </summary>
    public void SetText( BitmapFont font, string str )
    {
        SetText( font, str, 0, str.Length, font.GetColor(), 0, Align.LEFT, false, null );
    }

    /// <summary>
    ///     Calls <see cref="SetText(BitmapFont, string, int, int, Color, float, int, bool, string)" />
    ///     with the whole string and no truncation.
    /// </summary>
    public void SetText( BitmapFont font, string str, Color color, float targetWidth, int halign, bool wrap )
    {
        SetText( font, str, 0, str.Length, color, targetWidth, halign, wrap, null );
    }

    /// <summary>
    /// </summary>
    /// <param name="font"></param>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="color">
    ///     The default color to use for the text (the BitmapFont <see cref="BitmapFont.GetColor()" />
    ///     is not used). If <see cref="BitmapFont.BitmapFontData.MarkupEnabled" /> is true, color
    ///     markup tags in the specified string may change the color for portions of the text.
    /// </param>
    /// <param name="halign">
    ///     Horizontal alignment of the text, see also <see cref="Align" />.
    /// </param>
    /// <param name="targetWidth">
    ///     The width used for alignment, line wrapping, and truncation. May be zero if
    ///     those features are not used.
    /// </param>
    /// <param name="wrap"></param>
    /// <param name="truncate">
    ///     If not null and the width of the glyphs exceed targetWidth, the glyphs are
    ///     truncated and the glyphs for the specified truncate string are placed at the end.
    ///     Empty string can be used to truncate without adding glyphs. Truncate should not
    ///     be used with text that contains multiple lines. Wrap is ignored if truncate is
    ///     not null.
    /// </param>
    public void SetText( BitmapFont font,
                         string str,
                         int start,
                         int end,
                         Color color,
                         float targetWidth,
                         int halign,
                         bool wrap,
                         string? truncate )
    {
        _glyphRunPool.FreeAll( Runs );
        Runs.Clear();

        BitmapFont.BitmapFontData fontData = font.GetData();

        if ( start == end )
        {
            // Empty string.
            Width  = 0;
            Height = fontData.CapHeight;

            return;
        }

        if ( truncate != null )
        {
            // Ensures truncate code runs, doesn't actually cause wrapping.
            wrap = true;
        }
        else if ( targetWidth <= ( fontData.SpaceXadvance * 3 ) )
        {
            // Avoid one line per character, which is very inefficient.
            wrap = false;
        }

        Color nextColor     = color;
        var   markupEnabled = fontData.MarkupEnabled;

        if ( markupEnabled )
        {
            for ( int i = 1, n = _colorStack.Count; i < n; i++ )
            {
                _colorPool.Free( _colorStack[ i ] );
            }

            _colorStack.Clear();
            _colorStack.Add( color );
        }

        float             x         = 0;
        float             y         = 0;
        var               down      = fontData.Down;
        BitmapFont.Glyph? lastGlyph = null;
        var               runStart  = start;

        outer:

        //TODO: Refactor this loop, I don't like the way this is done.

        while ( true )
        {
            // Each run is delimited by newline or left square bracket.
            var runEnd  = -1;
            var newline = false;

            if ( start == end )
            {
                if ( runStart == end )
                {
                    break; // End of string with no run to process, we're done.
                }

                runEnd = end; // End of string, process last run.
            }
            else
            {
                switch ( str[ start++ ] )
                {
                    case '\n':
                        // End of line.
                        runEnd  = start - 1;
                        newline = true;

                        break;

                    case '[':
                        // Possible color tag.
                        if ( markupEnabled )
                        {
                            var length = ParseColorMarkup( str, start, end, _colorPool );

                            if ( length >= 0 )
                            {
                                runEnd    =  start - 1;
                                start     += length + 1;
                                nextColor =  _colorStack.Peek();
                            }
                            else if ( length == -2 )
                            {
                                // Skip first of "[[" escape sequence.
                                start++;

                                goto outer;
                            }
                        }

                        break;
                }
            }

            if ( runEnd != -1 )
            {
                if ( runEnd != runStart )
                {
                    // Eg, when a color tag is at text start or a line is "\n".
                    // Store the run that has ended.
                    GlyphRun? run = _glyphRunPool.Obtain();

                    GdxRuntimeException.ThrowIfNull( run, "run is null" );

                    run.Color.Set( color );
                    fontData.GetGlyphs( run, str, runStart, runEnd, lastGlyph! );

                    if ( run.Glyphs.Count == 0 )
                    {
                        _glyphRunPool.Free( run );

                        goto runEnded;
                    }

                    if ( lastGlyph != null )
                    {
                        // Move back the width of the last glyph from the previous run.
                        x -= lastGlyph.fixedWidth
                            ? lastGlyph.xadvance * fontData.ScaleX
                            : ( ( lastGlyph.width + lastGlyph.xoffset ) * fontData.ScaleX ) - fontData.PadRight;
                    }

                    lastGlyph = run.Glyphs.Peek();
                    run.X     = x;
                    run.Y     = y;

                    if ( newline || ( runEnd == end ) )
                    {
                        AdjustLastGlyph( fontData, run );
                    }

                    Runs.Add( run );

                    var n         = run.XAdvances.Count;
                    var xAdvances = run.XAdvances.ToArray();

                    if ( !wrap || ( n == 0 ) )
                    {
                        // No wrap or truncate, or no glyphs.
                        if ( markupEnabled )
                        {
                            // If disabled any subsequent run is sure to be on the next line.
                            for ( var i = 0; i < n; i++ )
                            {
                                x += xAdvances[ i ];
                            }
                        }

                        goto runEnded;
                    }

                    // Wrap or truncate.
                    // X offset relative to the drawing position + first xAdvance.
                    x += xAdvances[ 0 ] + xAdvances[ 1 ];

                    for ( var i = 2; i < n; i++ )
                    {
                        BitmapFont.Glyph glyph      = run.Glyphs[ i - 1 ];
                        var              glyphWidth = ( ( glyph.width + glyph.xoffset ) * fontData.ScaleX ) - fontData.PadRight;

                        if ( ( ( x + glyphWidth ) - _epsilon ) <= targetWidth )
                        {
                            // Glyph fits.
                            x += xAdvances[ i ];

                            continue;
                        }

                        if ( truncate != null )
                        {
                            Truncate( fontData, run, targetWidth, truncate, i );

                            goto outer;
                        }

                        // Wrap.
                        y         += down;
                        lastGlyph =  null;

                        var wrapIndex = fontData.GetWrapIndex( run.Glyphs, i );

                        // Require at least one glyph per line.
                        if ( ( ( wrapIndex == 0 ) && ( run.X == 0 ) )
                          || ( wrapIndex >= run.Glyphs.Count ) )
                        {
                            // Wrap at least the glyph that didn't fit.
                            wrapIndex = i - 1;
                        }

                        GlyphRun? next;

                        if ( wrapIndex == 0 )
                        {
                            // Move entire run to next line.
                            next = run;

                            // Remove leading whitespace.
                            for ( var glyphCount = run.Glyphs.Count; wrapIndex < glyphCount; wrapIndex++ )
                            {
                                if ( !fontData.IsWhitespace( ( char )run.Glyphs[ wrapIndex ].id ) )
                                {
                                    break;
                                }
                            }

                            if ( wrapIndex > 0 )
                            {
                                run.Glyphs.RemoveRange( 0, wrapIndex - 1 );
                                run.XAdvances.RemoveRange( 1, wrapIndex );
                            }

                            xAdvances[ 0 ] = ( -run.Glyphs.First().xoffset * fontData.ScaleX ) - fontData.PadLeft;

                            if ( Runs.Count > 1 )
                            {
                                // Previous run is now at the end of a line.
                                // Remove trailing whitespace and adjust last glyph.
                                GlyphRun previous  = Runs[ Runs.Count - 2 ];
                                var      lastIndex = previous.Glyphs.Count - 1;

                                for ( ; lastIndex > 0; lastIndex-- )
                                {
                                    if ( !fontData.IsWhitespace( ( char )previous.Glyphs[ lastIndex ].id ) )
                                    {
                                        break;
                                    }
                                }

                                previous.Glyphs.Truncate( lastIndex + 1 );
                                previous.XAdvances.Truncate( lastIndex + 2 );
                                AdjustLastGlyph( fontData, previous );
                            }
                        }
                        else
                        {
                            next = Wrap( fontData, run, wrapIndex, i );

                            if ( next == null )
                            {
                                // All wrapped glyphs were whitespace.
                                x = 0;

                                break;
                            }

                            Runs.Add( next );
                        }

                        // Start the loop over with the new run on the next line.
                        n         = next.XAdvances.Count;
                        xAdvances = next.XAdvances.ToArray();
                        x         = xAdvances[ 0 ];

                        if ( n > 1 )
                        {
                            x += xAdvances[ 1 ];
                        }

                        next.X = 0;
                        next.Y = y;
                        i      = 1;
                        run    = next;
                    }
                }

                runEnded:

                if ( newline )
                {
                    // Next run will be on the next line.
                    x = 0;

                    if ( runEnd == runStart ) // Blank line.
                    {
                        y += down * fontData.BlankLineScale;
                    }
                    else
                    {
                        y += down;
                    }

                    lastGlyph = null;
                }

                runStart = start;
                color    = nextColor;
            }
        }

        Height = fontData.CapHeight + Math.Abs( y );

        // Calculate run widths and the entire layout width.
        float      width     = 0;
        GlyphRun[] runsItems = Runs.ToArray();
        var        runsSize  = Runs.Count;

        for ( var i = 0; i < runsSize; i++ )
        {
            GlyphRun           run       = runsItems[ i ];
            var                xAdvances = run.XAdvances.ToArray();
            var                runWidth  = xAdvances[ 0 ];
            float              max       = 0;
            BitmapFont.Glyph[] glyphs    = run.Glyphs.ToArray();

            for ( int ii = 0, nn = run.Glyphs.Count; ii < nn; )
            {
                BitmapFont.Glyph glyph      = glyphs[ ii ];
                var              glyphWidth = ( ( glyph.width + glyph.xoffset ) * fontData.ScaleX ) - fontData.PadRight;

                // A glyph can extend past the right edge of subsequent glyphs.
                max = Math.Max( max, runWidth + glyphWidth );

                ii++;
                runWidth += xAdvances[ ii ];
            }

            run.Width = Math.Max( runWidth, max );
            width     = Math.Max( width, run.X + run.Width );
        }

        Width = width;

        // Align runs to center or right of targetWidth.
        if ( ( halign & Align.LEFT ) == 0 )
        {
            // Not left aligned, so must be center or right aligned.
            var center    = ( halign & Align.CENTER ) != 0;
            var lineWidth = 0f;
            var lineY     = float.MinValue;
            var lineStart = 0;

            for ( var i = 0; i < runsSize; i++ )
            {
                GlyphRun run = runsItems[ i ];

                if ( !run.Y.Equals( lineY ) )
                {
                    lineY = run.Y;

                    var shift = targetWidth - lineWidth;

                    if ( center )
                    {
                        shift /= 2;
                    }

                    while ( lineStart < i )
                    {
                        runsItems[ lineStart++ ].X += shift;
                    }

                    lineWidth = run.X + run.Width;
                }
                else
                {
                    lineWidth = Math.Max( lineWidth, run.X + run.Width );
                }
            }

            var widthShift = targetWidth - lineWidth;

            if ( center )
            {
                widthShift /= 2;
            }

            while ( lineStart < runsSize )
            {
                runsItems[ lineStart++ ].X += widthShift;
            }
        }
    }

    /// <param name="fontData"></param>
    /// <param name="run"></param>
    /// <param name="targetWidth"></param>
    /// <param name="truncate"> May be empty string. </param>
    /// <param name="widthIndex"></param>
    private void Truncate( BitmapFont.BitmapFontData fontData,
                           GlyphRun run,
                           float targetWidth,
                           string truncate,
                           int widthIndex )
    {
        // Determine truncate string size.
        GlyphRun? truncateRun = _glyphRunPool.Obtain();

        if ( truncateRun == null )
        {
            throw new GdxRuntimeException( "Null GlyphRub obtained!" );
        }

        fontData.GetGlyphs( truncateRun, truncate, 0, truncate.Length, null );

        float   truncateWidth = 0;
        float[] xAdvances;

        if ( truncateRun.XAdvances.Count > 0 )
        {
            AdjustLastGlyph( fontData, truncateRun );

            xAdvances = truncateRun.XAdvances.ToArray();

            for ( int i = 1, n = truncateRun.XAdvances.Count; i < n; i++ ) // Skip first for tight bounds.
            {
                truncateWidth += xAdvances[ i ];
            }
        }

        targetWidth -= truncateWidth;

        // Determine visible glyphs.
        var count = 0;
        var width = run.X;

        xAdvances = run.XAdvances.ToArray();

        while ( count < run.XAdvances.Count )
        {
            var xAdvance = xAdvances[ count ];

            width += xAdvance;

            if ( width > targetWidth )
            {
                break;
            }

            count++;
        }

        if ( count > 1 )
        {
            // Some run glyphs fit, append truncate glyphs.
            run.Glyphs.Truncate( count - 1 );
            run.XAdvances.Truncate( count );

            AdjustLastGlyph( fontData, run );

            if ( truncateRun.XAdvances.Count > 0 )
            {
                run.XAdvances.AddAll( truncateRun.XAdvances, 1, truncateRun.XAdvances.Count - 1 );
            }
        }
        else
        {
            // No run glyphs fit, use only truncate glyphs.
            run.Glyphs.Clear();
            run.XAdvances.Clear();
            run.XAdvances.AddAll( truncateRun.XAdvances );
        }

        run.Glyphs.AddAll( truncateRun.Glyphs );

        _glyphRunPool.Free( truncateRun );
    }

    /// <summary>
    ///     Breaks a run into two runs at the specified wrapIndex.
    /// </summary>
    /// <returns> May be null if second run is all whitespace. </returns>
    private GlyphRun? Wrap( BitmapFont.BitmapFontData? fontData, GlyphRun? first, int wrapIndex, int widthIndex )
    {
        ArgumentNullException.ThrowIfNull( fontData );
        ArgumentNullException.ThrowIfNull( first );

        List< BitmapFont.Glyph > glyphs2    = first.Glyphs; // Starts with all the glyphs.
        var                      glyphCount = first.Glyphs.Count;
        List< float >            xAdvances2 = first.XAdvances; // Starts with all the xAdvances.

        // Skip whitespace before the wrap index.
        var firstEnd = wrapIndex;

        for ( ; firstEnd > 0; firstEnd-- )
        {
            if ( !fontData.IsWhitespace( ( char )glyphs2[ firstEnd - 1 ].id ) )
            {
                break;
            }
        }

        // Skip whitespace after the wrap index.
        var secondStart = wrapIndex;

        for ( ; secondStart < glyphCount; secondStart++ )
        {
            if ( !fontData.IsWhitespace( ( char )glyphs2[ secondStart ].id ) )
            {
                break;
            }
        }

        // Copy wrapped glyphs and xAdvances to second run. The second run will
        // contain the remaining glyph data, so swap instances rather than copying.
        GlyphRun? second = null;

        if ( secondStart < glyphCount )
        {
            second = _glyphRunPool.Obtain();
            second?.Color.Set( first.Color );

            List< BitmapFont.Glyph >? glyphs1 = second?.Glyphs; // Starts empty.

            glyphs1?.AddAll( glyphs2, 0, firstEnd );
            glyphs2.RemoveRange( 0, secondStart - 1 );

            first.Glyphs   = glyphs1!;
            second!.Glyphs = glyphs2;

            List< float > xAdvances1 = second.XAdvances; // Starts empty.

            xAdvances1.AddAll( xAdvances2, 0, firstEnd + 1 );
            xAdvances2.RemoveRange( 1, secondStart ); // Leave first entry to be overwritten by next line.
            xAdvances2[ 0 ]  = ( -glyphs2.First().xoffset * fontData.ScaleX ) - fontData.PadLeft;
            first.XAdvances  = xAdvances1;
            second.XAdvances = xAdvances2;
        }
        else
        {
            // Second run is empty, just trim whitespace glyphs from end of first run.
            glyphs2.Truncate( firstEnd );
            xAdvances2.Truncate( firstEnd + 1 );
        }

        if ( firstEnd == 0 )
        {
            // If the first run is now empty, remove it.
            _glyphRunPool.Free( first );
            Runs.Pop();
        }
        else
        {
            AdjustLastGlyph( fontData, first );
        }

        return second;
    }

    /// <summary>
    ///     Adjusts the xadvance of the last glyph to use its width instead of xadvance.
    /// </summary>
    private void AdjustLastGlyph( BitmapFont.BitmapFontData fontData, GlyphRun run )
    {
        BitmapFont.Glyph last = run.Glyphs.Peek();

        if ( last.fixedWidth )
        {
            return;
        }

        var width = ( ( last.width + last.xoffset ) * fontData.ScaleX ) - fontData.PadRight;

        run.XAdvances.ToArray()[ run.XAdvances.Count - 1 ] = width;
    }

    private int ParseColorMarkup( string str, int start, int end, Pool< Color > colorpool )
    {
        if ( start == end )
        {
            return -1; // String ended with "[".
        }

        switch ( str[ start ] )
        {
            case '#':
                // Parse hex color RRGGBBAA where AA is optional and
                // defaults to 0xFF if less than 6 chars are used.
                var colorInt = 0;

                for ( var i = start + 1; i < end; i++ )
                {
                    var ch = str[ i ];

                    if ( ch == ']' )
                    {
                        if ( ( i < ( start + 2 ) ) || ( i > ( start + 9 ) ) )
                        {
                            break; // Illegal number of hex digits.
                        }

                        if ( ( i - start ) <= 7 )
                        {
                            // RRGGBB or fewer chars.
                            for ( int ii = 0, nn = 9 - ( i - start ); ii < nn; ii++ )
                            {
                                colorInt <<= 4;
                            }

                            colorInt |= 0xff;
                        }

                        Color? color = colorpool.Obtain();
                        _colorStack.Add( color! );
                        Color.Rgba8888ToColor( ref color!, colorInt );

                        return i - start;
                    }

                    if ( ch is >= '0' and <= '9' )
                    {
                        colorInt = ( colorInt * 16 ) + ( ch - '0' );
                    }
                    else if ( ch is >= 'a' and <= 'f' )
                    {
                        colorInt = ( colorInt * 16 ) + ( ch - ( 'a' - 10 ) );
                    }
                    else if ( ch is >= 'A' and <= 'F' )
                    {
                        colorInt = ( colorInt * 16 ) + ( ch - ( 'A' - 10 ) );
                    }
                    else
                    {
                        break; // Unexpected character in hex color.
                    }
                }

                return -1;

            case '[': // "[[" is an escaped left square bracket.
                return -2;

            case ']': // "[]" is a "pop" color tag.
                if ( _colorStack.Count > 1 )
                {
                    colorpool.Free( _colorStack.Pop() );
                }

                return 0;
        }

        // Parse named color.
        var colorStart = start;

        for ( var i = start + 1; i < end; i++ )
        {
            var ch = str[ i ];

            if ( ch != ']' )
            {
                continue;
            }

            Color? namedColor = Colors.Get( str.Substring( colorStart, i - colorStart ) );

            if ( namedColor == null )
            {
                return -1; // Unknown color name.
            }

            Color? color = colorpool.Obtain();

            _colorStack.Add( color! );
            color?.Set( namedColor );

            return i - start;
        }

        return -1; // Unclosed color tag.
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Stores glyphs and positions for a piece of text which is a single
    ///     color and does not span multiple lines.
    /// </summary>
    [PublicAPI]
    public class GlyphRun : IPoolable
    {
        public List< BitmapFont.Glyph > Glyphs    { get; set; } = null!;
        public List< float >            XAdvances { get; set; } = null!;
        public float                    X         { get; set; }
        public float                    Y         { get; set; }
        public float                    Width     { get; set; }
        public Color                    Color     { get; set; } = new();

        /// <summary>
        ///     Resets the object for reuse. Object references should
        ///     be nulled and fields may be set to default values.
        /// </summary>
        public void Reset()
        {
            Glyphs.Clear();
            XAdvances.Clear();
            Width = 0;
        }

        public override string ToString()
        {
            StringBuilder buffer = new( Glyphs.Count + 32 );

            for ( int i = 0, n = Glyphs.Count; i < n; i++ )
            {
                BitmapFont.Glyph g = Glyphs[ i ];
                buffer.Append( ( char )g.id );
            }

            buffer.Append( ", #" );
            buffer.Append( Color );
            buffer.Append( ", " );
            buffer.Append( X );
            buffer.Append( ", " );
            buffer.Append( Y );
            buffer.Append( ", " );
            buffer.Append( Width );

            return buffer.ToString();
        }
    }
}
