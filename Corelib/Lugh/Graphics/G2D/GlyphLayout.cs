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

using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Collections;
using Corelib.Lugh.Utils.Exceptions;
using Corelib.Lugh.Utils.Pooling;

namespace Corelib.Lugh.Graphics.G2D;

/// <summary>
/// Stores <see cref="GlyphRun"/> runs of glyphs for a piece of text. The text may contain
/// newlines and color markup tags.
/// <para>
/// Where wrapping occurs is determined by <see cref="BitmapFont.BitmapFontData.GetWrapIndex"/>.
/// Additionally, when <see cref="BitmapFont.BitmapFontData.MarkupEnabled"/> is true wrapping
/// can occur at color start or end tags.
/// </para>
/// <para>
/// When wrapping occurs, whitespace is removed before and after the wrap position.
/// Whitespace is determined by <see cref="BitmapFont.BitmapFontData.IsWhitespace(char)"/>.
/// </para>
/// <para>
/// Glyphs positions are determined by <see cref="BitmapFont.BitmapFontData.GetGlyphs"/>.
/// </para>
/// <para>
/// This class is not thread safe, even if synchronized externally, and must only
/// be used from the game thread.
/// </para>
/// </summary>
[PublicAPI]
public class GlyphLayout : IResetable
{
    public List< GlyphRun > Runs   { get; set; } = new( 1 );
    public float            Width  { get; set; }
    public float            Height { get; set; }

    // ========================================================================

    private static readonly float            _epsilon      = 0.0001f;
    private readonly        Pool< Color >    _colorPool    = Pools< Color >.Get();
    private readonly        List< Color >    _colorStack   = new( 4 );
    private readonly        Pool< GlyphRun > _glyphRunPool = Pools< GlyphRun >.Get();

    // ========================================================================

    /// <summary>
    /// Creates an empty GlyphLayout.
    /// </summary>
    public GlyphLayout()
    {
    }

    /// <summary>
    /// Creates a new GlyphLayout, using the supplied <see cref="BitmapFont"/> and text.
    /// </summary>
    /// <param name="font"> The font to use. </param>
    /// <param name="str"> A string holding the text. </param>
    public GlyphLayout( BitmapFont font, string str )
    {
        SetText( font, str );
    }

    /// <summary>
    /// Creates a new GlyphLayout, using the supplied <see cref="BitmapFont"/>, text message,
    /// <see cref="Color"/>, target width, horizontal alignment, and wrap.
    /// </summary>
    /// <param name="font"> The font to use. </param>
    /// <param name="str"> A string holding the text. </param>
    /// <param name="color">
    /// The default color to use for the text (the BitmapFont <see cref="BitmapFont.GetColor()"/>
    /// is not used). If <see cref="BitmapFont.BitmapFontData.MarkupEnabled"/> is true, color
    /// markup tags in the specified string may change the color for portions of the text.
    /// </param>
    /// <param name="targetWidth"></param>
    /// <param name="halign"></param>
    /// <param name="wrap"></param>
    public GlyphLayout( BitmapFont font, string str, Color color, float targetWidth, int halign, bool wrap )
    {
        SetText( font, str, color, targetWidth, halign, wrap );
    }

    /// <summary>
    /// </summary>
    /// <param name="font"> The font to use. </param>
    /// <param name="str"> A string holding the text. </param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="color">
    /// The default color to use for the text (the BitmapFont <see cref="BitmapFont.GetColor()"/>
    /// is not used). If <see cref="BitmapFont.BitmapFontData.MarkupEnabled"/> is true, color
    /// markup tags in the specified string may change the color for portions of the text.
    /// </param>
    /// <param name="targetWidth"></param>
    /// <param name="halign"></param>
    /// <param name="wrap"></param>
    /// <param name="truncate"></param>
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

    /// <summary>
    /// Calls <see cref="SetText(BitmapFont, string, int, int, Color, float, int, bool, string)"/>
    /// with the whole string, the font's current color, and with no alignment or wrapping.
    /// </summary>
    /// <param name="font"> The font to use. </param>
    /// <param name="str"> A string holding the text. </param>
    public void SetText( BitmapFont font, string str )
    {
        SetText( font, str, 0, str.Length, font.GetColor(), 0, Align.LEFT, false, null );
    }

    /// <summary>
    /// Calls <see cref="SetText(BitmapFont, string, int, int, Color, float, int, bool, string)"/>
    /// with the whole string and no truncation.
    /// </summary>
    /// <param name="font"> The font to use. </param>
    /// <param name="str"> A string holding the text. </param>
    /// <param name="color">
    /// The default color to use for the text (the BitmapFont <see cref="BitmapFont.GetColor()"/>
    /// is not used). If <see cref="BitmapFont.BitmapFontData.MarkupEnabled"/> is true, color
    /// markup tags in the specified string may change the color for portions of the text.
    /// </param>
    /// <param name="targetWidth"></param>
    /// <param name="halign"></param>
    /// <param name="wrap"></param>
    public void SetText( BitmapFont font, string str, Color color, float targetWidth, int halign, bool wrap )
    {
        SetText( font, str, 0, str.Length, color, targetWidth, halign, wrap, null );
    }

    /// <summary>
    /// </summary>
    /// <param name="font"> The font to use. </param>
    /// <param name="str"> A string holding the text. </param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="color">
    /// The default color to use for the text (the BitmapFont <see cref="BitmapFont.GetColor()"/>
    /// is not used). If <see cref="BitmapFont.BitmapFontData.MarkupEnabled"/> is true, color
    /// markup tags in the specified string may change the color for portions of the text.
    /// </param>
    /// <param name="halign">
    /// Horizontal alignment of the text, see also <see cref="Align"/>.
    /// </param>
    /// <param name="targetWidth">
    /// The width used for alignment, line wrapping, and truncation. May be zero if
    /// those features are not used.
    /// </param>
    /// <param name="wrap"></param>
    /// <param name="truncate">
    /// If not null and the width of the glyphs exceed targetWidth, the glyphs are
    /// truncated and the glyphs for the specified truncate string are placed at the end.
    /// Empty string can be used to truncate without adding glyphs. Truncate should not
    /// be used with text that contains multiple lines. Wrap is ignored if truncate is
    /// not null.
    /// </param>
    public void SetText( BitmapFont font, string str, int start, int end, Color color, float targetWidth, int halign, bool wrap, string? truncate )
    {
        _glyphRunPool.FreeAll( Runs );
        Runs.Clear();

        var fontData = font.Data;

        if ( start == end )
        {
            // Empty string
            Width  = 0;
            Height = fontData.CapHeight;

            return;
        }

        PrepareForTextProcessing( fontData, color, targetWidth, ref wrap, truncate );

        float             x         = 0, y = 0;
        BitmapFont.Glyph? lastGlyph = null;
        var               runStart  = start;

        while ( true )
        {
            var (runEnd, newline, nextColor) = FindRunEnd( str, ref start, end, fontData, color );

            if ( runEnd == -1 )
            {
                break;
            }

            ProcessRun( font, str, fontData, runStart, runEnd, ref x, ref y, ref lastGlyph, newline, wrap, targetWidth, nextColor );

            if ( newline )
            {
                x         =  0;
                y         += newline ? fontData.Down * fontData.BlankLineScale : fontData.Down;
                lastGlyph =  null;
            }

            runStart = start;
            color    = nextColor;
        }

        FinalizeTextLayout( fontData, y, targetWidth, halign );
    }

    /// <summary>
    /// Prepares for text processing by setting the appropriate wrapping and markup options.
    /// </summary>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="color">Initial color for the text.</param>
    /// <param name="targetWidth">Target width for text wrapping.</param>
    /// <param name="wrap">Boolean indicating if text wrapping is enabled.</param>
    /// <param name="truncate">String for truncation if needed.</param>
    private void PrepareForTextProcessing( BitmapFont.BitmapFontData fontData, Color color, float targetWidth, ref bool wrap, string? truncate )
    {
        if ( truncate != null )
        {
            wrap = true;
        }
        else if ( targetWidth <= ( fontData.SpaceXadvance * 3 ) )
        {
            wrap = false;
        }

        if ( fontData.MarkupEnabled )
        {
            ClearColorStack( color );
        }
    }

    /// <summary>
    /// Clears the color stack and sets the initial color.
    /// </summary>
    /// <param name="color">Initial color to set.</param>
    private void ClearColorStack( Color color )
    {
        for ( int i = 1, n = _colorStack.Count; i < n; i++ )
        {
            _colorPool.Free( _colorStack[ i ] );
        }

        _colorStack.Clear();
        _colorStack.Add( color );
    }

    /// <summary>
    /// Finds the end of the current run, handling new lines and color markup if applicable.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <param name="start">Start index of the current run.</param>
    /// <param name="end">End index of the string.</param>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="color">Current color of the text.</param>
    /// <returns>
    /// Tuple containing the run end index, a boolean indicating if a newline was encountered,
    /// and the next color.
    /// </returns>
    private (int runEnd, bool newline, Color nextColor) FindRunEnd( string str, ref int start, int end, BitmapFont.BitmapFontData fontData, Color color )
    {
        var runEnd    = -1;
        var newline   = false;
        var nextColor = color;

        while ( start < end )
        {
            var c = str[ start++ ];

            if ( c == '\n' )
            {
                runEnd  = start - 1;
                newline = true;

                break;
            }
            else if ( ( c == '[' ) && fontData.MarkupEnabled )
            {
                var length = ParseColorMarkup( str, start, end, _colorPool );

                if ( length >= 0 )
                {
                    runEnd    =  start - 1;
                    start     += length + 1;
                    nextColor =  _colorStack.Peek();

                    break;
                }
                else if ( length == -2 )
                {
                    start++;
                }
            }
        }

        if ( runEnd == -1 )
        {
            runEnd = end;
        }

        return ( runEnd, newline, nextColor );
    }

    /// <summary>
    /// Processes a run of text, handling positioning, wrapping, and color changes.
    /// </summary>
    /// <param name="font">Bitmap font used for rendering.</param>
    /// <param name="str">Input string.</param>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="runStart">Start index of the current run.</param>
    /// <param name="runEnd">End index of the current run.</param>
    /// <param name="x">Current x position.</param>
    /// <param name="y">Current y position.</param>
    /// <param name="lastGlyph">Last processed glyph.</param>
    /// <param name="newline">Boolean indicating if a newline was encountered.</param>
    /// <param name="wrap">Boolean indicating if text wrapping is enabled.</param>
    /// <param name="targetWidth">Target width for text wrapping.</param>
    /// <param name="nextColor">Next color for the text.</param>
    private void ProcessRun( BitmapFont font,
                             string str,
                             BitmapFont.BitmapFontData fontData,
                             int runStart,
                             int runEnd,
                             ref float x,
                             ref float y,
                             ref BitmapFont.Glyph? lastGlyph,
                             bool newline,
                             bool wrap,
                             float targetWidth,
                             Color nextColor )
    {
        if ( runEnd == runStart )
        {
            return;
        }

        var run = _glyphRunPool.Obtain();
        GdxRuntimeException.ThrowIfNull( run, "Unable to obtain a GlyphRun!" );

        run.Color.Set( nextColor );
        fontData.GetGlyphs( run, str, runStart, runEnd, lastGlyph! );

        if ( run.Glyphs.Count == 0 )
        {
            _glyphRunPool.Free( run );

            return;
        }

        AdjustRunStartPosition( run, fontData, ref x, ref lastGlyph );

        lastGlyph = run.Glyphs.Peek();
        run.X     = x;
        run.Y     = y;

        if ( newline || ( runEnd == str.Length ) )
        {
            AdjustLastGlyph( fontData, run );
        }

        Runs.Add( run );

        if ( !wrap || ( run.XAdvances.Count == 0 ) )
        {
            AdvanceXForRun( run, fontData, ref x );

            return;
        }

        HandleWrapping( fontData, run, ref x, ref y, targetWidth, str );
    }

    /// <summary>
    /// Adjusts the starting position of the run based on the last glyph processed.
    /// </summary>
    /// <param name="run">Current glyph run.</param>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="x">Current x position.</param>
    /// <param name="lastGlyph">Last processed glyph.</param>
    private void AdjustRunStartPosition( GlyphRun run, BitmapFont.BitmapFontData fontData, ref float x, ref BitmapFont.Glyph? lastGlyph )
    {
        if ( lastGlyph != null )
        {
            x -= lastGlyph.FixedWidth
                     ? lastGlyph.Xadvance * fontData.ScaleX
                     : ( ( lastGlyph.Width + lastGlyph.Xoffset ) * fontData.ScaleX ) - fontData.PadRight;
        }
    }

    /// <summary>
    /// Advances the x position for the current run.
    /// </summary>
    /// <param name="run">Current glyph run.</param>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="x">Current x position.</param>
    private void AdvanceXForRun( GlyphRun run, BitmapFont.BitmapFontData fontData, ref float x )
    {
        if ( fontData.MarkupEnabled )
        {
            foreach ( var xAdvance in run.XAdvances )
            {
                x += xAdvance;
            }
        }
    }

    /// <summary>
    /// Handles text wrapping for the current run, creating new lines as necessary.
    /// </summary>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="run">Current glyph run.</param>
    /// <param name="x">Current x position.</param>
    /// <param name="y">Current y position.</param>
    /// <param name="targetWidth">Target width for text wrapping.</param>
    /// <param name="truncate">String for truncation if needed.</param>
    private void HandleWrapping( BitmapFont.BitmapFontData fontData, GlyphRun run, ref float x, ref float y, float targetWidth, string? truncate )
    {
        var xAdvances = run.XAdvances.ToArray();
        x += xAdvances[ 0 ] + xAdvances[ 1 ];

        for ( var i = 2; i < run.XAdvances.Count; i++ )
        {
            var glyph      = run.Glyphs[ i - 1 ];
            var glyphWidth = ( ( glyph.Width + glyph.Xoffset ) * fontData.ScaleX ) - fontData.PadRight;

            if ( ( ( x + glyphWidth ) - _epsilon ) <= targetWidth )
            {
                x += xAdvances[ i ];

                continue;
            }

            if ( truncate != null )
            {
                Truncate( fontData, run, targetWidth, truncate, i );

                return;
            }

            WrapRun( fontData, run, ref x, ref y, targetWidth, i );
        }
    }

    /// <summary>
    /// Wraps the current run, creating a new line and adjusting the y position.
    /// </summary>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="run">Current glyph run.</param>
    /// <param name="x">Current x position.</param>
    /// <param name="y">Current y position.</param>
    /// <param name="targetWidth">Target width for text wrapping.</param>
    /// <param name="wrapIndex">Index at which to wrap the text.</param>
    private void WrapRun( BitmapFont.BitmapFontData fontData, GlyphRun run, ref float x, ref float y, float targetWidth, int wrapIndex )
    {
        y += fontData.Down;

        var actualWrapIndex = fontData.GetWrapIndex( run.Glyphs, wrapIndex );
        actualWrapIndex = EnsureMinimumGlyphsPerLine( run, actualWrapIndex );

        var nextRun = CreateWrappedRun( fontData, run, actualWrapIndex, wrapIndex );

        if ( nextRun == null )
        {
            x = 0;

            return;
        }

        Runs.Add( nextRun );

        x = nextRun.XAdvances[ 0 ] + nextRun.XAdvances.ElementAtOrDefault( 1 );

        nextRun.X = 0;
        nextRun.Y = y;
    }

    /// <summary>
    /// Ensures a minimum number of glyphs per line when wrapping text.
    /// </summary>
    /// <param name="run">Current glyph run.</param>
    /// <param name="wrapIndex">Index at which to wrap the text.</param>
    /// <returns>Adjusted wrap index.</returns>
    private int EnsureMinimumGlyphsPerLine( GlyphRun run, int wrapIndex )
    {
        if ( ( ( wrapIndex == 0 ) && ( run.X == 0 ) ) || ( wrapIndex >= run.Glyphs.Count ) )
        {
            wrapIndex = run.Glyphs.Count - 1;
        }

        return wrapIndex;
    }

    /// <summary>
    /// Creates a wrapped run from the current run, adjusting for text wrapping.
    /// </summary>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="run">Current glyph run.</param>
    /// <param name="wrapIndex">Index at which to wrap the text.</param>
    /// <param name="i">Current index in the run.</param>
    /// <returns>New wrapped glyph run.</returns>
    private GlyphRun? CreateWrappedRun( BitmapFont.BitmapFontData fontData, GlyphRun run, int wrapIndex, int i )
    {
        var nextRun = Wrap( fontData, run, wrapIndex, i );

        if ( nextRun != null )
        {
            RemoveLeadingWhitespace( fontData, run, ref wrapIndex );
        }

        return nextRun;
    }

    /// <summary>
    /// Removes leading whitespace from the wrapped run.
    /// </summary>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="run">Current glyph run.</param>
    /// <param name="wrapIndex">Index at which to wrap the text.</param>
    private void RemoveLeadingWhitespace( BitmapFont.BitmapFontData fontData, GlyphRun run, ref int wrapIndex )
    {
        for ( ; wrapIndex < run.Glyphs.Count; wrapIndex++ )
        {
            if ( !fontData.IsWhitespace( ( char ) run.Glyphs[ wrapIndex ].ID ) )
            {
                break;
            }
        }

        if ( wrapIndex > 0 )
        {
            run.Glyphs.RemoveRange( 0, wrapIndex - 1 );
            run.XAdvances.RemoveRange( 1, wrapIndex );
        }
    }

    /// <summary>
    /// Finalizes the text layout, calculating heights and aligning runs based on the alignment option.
    /// </summary>
    /// <param name="fontData">Font data containing font-specific information.</param>
    /// <param name="y">Current y position.</param>
    /// <param name="targetWidth">Target width for text alignment.</param>
    /// <param name="halign">Horizontal alignment option.</param>
    private void FinalizeTextLayout( BitmapFont.BitmapFontData fontData, float y, float targetWidth, int halign )
    {
        Height = fontData.CapHeight + Math.Abs( y );

        CalculateRunWidths( fontData );

        if ( ( halign & Align.LEFT ) == 0 )
        {
            AlignRuns( targetWidth, halign );
        }
    }

    /// <summary>
    /// Calculates the widths of all glyph runs.
    /// </summary>
    private void CalculateRunWidths( BitmapFont.BitmapFontData fontData )
    {
        float width = 0;

        foreach ( var run in Runs )
        {
            var   xAdvances = run.XAdvances.ToArray();
            var   runWidth  = xAdvances[ 0 ];
            float maxWidth  = 0;

            foreach ( var glyph in run.Glyphs )
            {
                var glyphWidth = ( ( glyph.Width + glyph.Xoffset ) * fontData.ScaleX ) - fontData.PadRight;

                maxWidth =  Math.Max( maxWidth, runWidth + glyphWidth );
                runWidth += xAdvances.ElementAtOrDefault( run.Glyphs.IndexOf( glyph ) + 1 );
            }

            run.Width = Math.Max( runWidth, maxWidth );
            width     = Math.Max( width, run.X + run.Width );
        }

        Width = width;
    }

    /// <summary>
    /// Aligns all glyph runs based on the target width and alignment option.
    /// </summary>
    /// <param name="targetWidth">Target width for text alignment.</param>
    /// <param name="halign">Horizontal alignment option.</param>
    private void AlignRuns( float targetWidth, int halign )
    {
        var isCenterAligned = ( halign & Align.CENTER ) != 0;
        var lineWidth       = 0f;
        var lineY           = float.MinValue;
        var lineStart       = 0;

        foreach ( var run in Runs )
        {
            if ( !run.Y.Equals( lineY ) )
            {
                lineY = run.Y;
                var shift = targetWidth - lineWidth;
                shift = isCenterAligned ? shift / 2 : shift;

                while ( lineStart < Runs.IndexOf( run ) )
                {
                    Runs[ lineStart++ ].X += shift;
                }

                lineWidth = run.X + run.Width;
            }
            else
            {
                lineWidth = Math.Max( lineWidth, run.X + run.Width );
            }
        }

        var widthShift = targetWidth - lineWidth;
        widthShift = isCenterAligned ? widthShift / 2 : widthShift;

        while ( lineStart < Runs.Count )
        {
            Runs[ lineStart++ ].X += widthShift;
        }
    }

    /// <summary>
    /// Truncates a glyph run to fit within the specified target width by appending a truncate string if necessary.
    /// </summary>
    /// <param name="fontData">The font data used to obtain glyphs and their properties.</param>
    /// <param name="run">The glyph run to be truncated.</param>
    /// <param name="targetWidth">The maximum width that the text should occupy.</param>
    /// <param name="truncate">The string to append at the end if truncation is required.</param>
    /// <param name="widthIndex">The index where the truncation should be applied.</param>
    /// <exception cref="GdxRuntimeException">Thrown when a GlyphRun cannot be obtained from the pool.</exception>
    private void Truncate( BitmapFont.BitmapFontData fontData,
                           GlyphRun run,
                           float targetWidth,
                           string truncate,
                           int widthIndex )
    {
        // Obtain a GlyphRun for the truncate string.
        var truncateRun = _glyphRunPool.Obtain();

        if ( truncateRun == null )
        {
            throw new GdxRuntimeException( "Unable to obtain a GlyphRun!" );
        }

        // Populate the truncate run with glyphs from the truncate string.
        fontData.GetGlyphs( truncateRun, truncate, 0, truncate.Length, null );

        // Calculate the width of the truncate string.
        float truncateWidth = 0;

        if ( truncateRun.XAdvances.Count > 0 )
        {
            AdjustLastGlyph( fontData, truncateRun );
            var xAdvances = truncateRun.XAdvances.ToArray();

            // Sum the advances to get the total width, skipping the first advance for tighter bounds.
            for ( var i = 1; i < truncateRun.XAdvances.Count; i++ )
            {
                truncateWidth += xAdvances[ i ];
            }
        }

        // Subtract the truncate string width from the target width.
        targetWidth -= truncateWidth;

        // Determine how many glyphs from the original run fit within the remaining target width.
        var count        = 0;
        var width        = run.X;
        var runXAdvances = run.XAdvances.ToArray();

        while ( count < run.XAdvances.Count )
        {
            var xAdvance = runXAdvances[ count ];
            width += xAdvance;

            // Stop if adding the next glyph would exceed the target width.
            if ( width > targetWidth )
            {
                break;
            }

            count++;
        }

        if ( count > 1 )
        {
            // If at least one glyph from the original run fits, truncate the run and append the truncate glyphs.
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
            // If no glyphs from the original run fit, use only the truncate glyphs.
            run.Glyphs.Clear();
            run.XAdvances.Clear();
            run.XAdvances.AddAll( truncateRun.XAdvances );
        }

        // Add the truncate glyphs to the run.
        run.Glyphs.AddAll( truncateRun.Glyphs );

        // Free the truncate run.
        _glyphRunPool.Free( truncateRun );
    }

    /// <summary>
    /// Breaks a run into two runs at the specified wrapIndex.
    /// </summary>
    /// <param name="fontData"></param>
    /// <param name="first"></param>
    /// <param name="wrapIndex"></param>
    /// <param name="widthIndex"></param>
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
            if ( !fontData.IsWhitespace( ( char ) glyphs2[ firstEnd - 1 ].ID ) )
            {
                break;
            }
        }

        // Skip whitespace after the wrap index.
        var secondStart = wrapIndex;

        for ( ; secondStart < glyphCount; secondStart++ )
        {
            if ( !fontData.IsWhitespace( ( char ) glyphs2[ secondStart ].ID ) )
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
            xAdvances2[ 0 ]  = ( -glyphs2.First().Xoffset * fontData.ScaleX ) - fontData.PadLeft;
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
    /// Resets the object for reuse. Object references should be nulled and fields
    /// may be set to default values.
    /// </summary>
    public void Reset()
    {
        Pools< GlyphRun >.Get().FreeAll( Runs );
        Runs.Clear();

        Width  = 0;
        Height = 0;
    }

    /// <summary>
    /// Adjusts the xadvance of the last glyph to use its width instead of xadvance.
    /// </summary>
    /// <param name="fontData"></param>
    /// <param name="run"></param>
    /// <seealso cref="GlyphRun.XAdvances"/>
    private void AdjustLastGlyph( BitmapFont.BitmapFontData fontData, GlyphRun run )
    {
        var last = run.Glyphs.Peek();

        if ( last.FixedWidth )
        {
            return;
        }

        var width = ( ( last.Width + last.Xoffset ) * fontData.ScaleX ) - fontData.PadRight;

        run.XAdvances.ToArray()[ run.XAdvances.Count - 1 ] = width;
    }

    /// <summary>
    /// Parses a color markup within the specified string and range.
    /// </summary>
    /// <param name="str"> The input string containing the markup. </param>
    /// <param name="start"> The start index of the markup. </param>
    /// <param name="end"> The end index of the markup. </param>
    /// <param name="colorpool"> The pool from which to obtain color instances. </param>
    /// <returns>
    /// An integer indicating the number of characters processed:
    /// - -1 if the string ends with "["
    /// - -2 if the markup is "[["
    /// - 0 if the markup is "[]"
    /// - The number of characters processed for valid color markups
    /// </returns>
    private int ParseColorMarkup( string str, int start, int end, Pool< Color > colorpool )
    {
        if ( start == end )
        {
            return -1; // String ended with "[".
        }

        switch ( str[ start ] )
        {
            case '#':
                return ParseHexColor( str, start, end, colorpool );

            case '[': // "[[" is an escaped left square bracket.
                return -2;

            case ']': // "[]" is a "pop" color tag.
                if ( _colorStack.Count > 1 )
                {
                    colorpool.Free( _colorStack.Pop() );
                }

                return 0;

            default:
                return ParseNamedColor( str, start, end, colorpool );
        }
    }

    /// <summary>
    /// Parses a hexadecimal color markup within the specified string and range.
    /// </summary>
    /// <param name="str"> The input string containing the markup. </param>
    /// <param name="start"> The start index of the markup. </param>
    /// <param name="end"> The end index of the markup. </param>
    /// <param name="colorpool"> The pool from which to obtain color instances. </param>
    /// <returns>
    /// An integer indicating the number of characters processed, or -1 if the markup is invalid.
    /// </returns>
    private int ParseHexColor( string str, int start, int end, Pool< Color > colorpool )
    {
        uint colorInt = 0;

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
                    colorInt <<= ( 9 - ( i - start ) ) * 4;
                    colorInt |=  0xff;
                }

                var color = colorpool.Obtain();
                _colorStack.Add( color! );
                Color.RGBA8888ToColor( ref color!, colorInt );

                return i - start;
            }

            if ( NumberUtils.IsHexDigit( ch ) )
            {
                colorInt = ( uint ) ( ( colorInt * 16 ) + NumberUtils.HexValue( ch ) );
            }
            else
            {
                break; // Unexpected character in hex color.
            }
        }

        return -1;
    }

    /// <summary>
    /// Parses a named color markup within the specified string and range.
    /// </summary>
    /// <param name="str"> The input string containing the markup. </param>
    /// <param name="start"> The start index of the markup. </param>
    /// <param name="end"> The end index of the markup. </param>
    /// <param name="colorpool"> The pool from which to obtain color instances. </param>
    /// <returns>
    /// An integer indicating the number of characters processed, or -1 if the markup is invalid.
    /// </returns>
    private int ParseNamedColor( string str, int start, int end, Pool< Color > colorpool )
    {
        var colorStart = start;

        for ( var i = start + 1; i < end; i++ )
        {
            var ch = str[ i ];

            if ( ch == ']' )
            {
                var colorName  = str.Substring( colorStart, i - colorStart );
                var namedColor = Colors.Get( colorName );

                if ( namedColor == null )
                {
                    return -1; // Unknown color name.
                }

                var color = colorpool.Obtain();
                _colorStack.Add( color! );
                color?.Set( namedColor );

                return i - start;
            }
        }

        return -1; // Unclosed color tag.
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Stores glyphs and positions for a piece of text which is a single color and
    /// does not span multiple lines.
    /// </summary>
    [PublicAPI]
    public class GlyphRun : IResetable
    {
        public List< BitmapFont.Glyph > Glyphs    { get; set; } = null!;
        public List< float >            XAdvances { get; set; } = null!;
        public float                    X         { get; set; }
        public float                    Y         { get; set; }
        public float                    Width     { get; set; }
        public Color                    Color     { get; set; } = new();

        /// <summary>
        /// Resets the object for reuse. Object references should be nulled and fields
        /// may be set to default values.
        /// </summary>
        public void Reset()
        {
            Glyphs.Clear();
            XAdvances.Clear();
            Width = 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder buffer = new( Glyphs.Count + 32 );

            for ( int i = 0, n = Glyphs.Count; i < n; i++ )
            {
                var g = Glyphs[ i ];
                buffer.Append( ( char ) g.ID );
            }

            buffer.Append( $", #{Color}, {X}, {Y}, {Width}" );

            return buffer.ToString();
        }
    }
}