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

using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Scenes.Scene2D.Listeners;
using LughSharp.Lugh.Scenes.Scene2D.Utils;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;
using LughSharp.Lugh.Utils.Pooling;

namespace LughSharp.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// A text input field with multiple lines.
/// </summary>
[PublicAPI]
public class TextArea : TextField
{
    // ========================================================================

    // Last text processed. This attribute is used to avoid unnecessary
    // computations while calculating offsets
    private string? _lastText;

    // Variable to maintain the x offset of the cursor when moving up and
    // down. If it's set to -1, the offset is reset
    private float _moveOffset;

    private float _prefRows;

    // ========================================================================

    public TextArea( string text, Skin skin )
        : base( text, skin )
    {
    }

    public TextArea( string text, Skin skin, string styleName )
        : base( text, skin, styleName )
    {
    }

    public TextArea( string text, TextFieldStyle style )
        : base( text, style )
    {
    }

    // Current line for the cursor
    public int CursorLine { get; set; }

    // Index of the first line showed by the text area
    public int FirstLineShowing { get; set; }

    // Number of lines showed by the text area
    public int LinesShowing { get; set; }

    // Array storing lines breaks positions
    public List< int >? LinesBreak { get; set; }

    /// <summary>
    /// Initialise this TextArea.
    /// </summary>
    public override void Initialise()
    {
        base.Initialise();

        WriteEnters      = true;
        LinesBreak       = [ ];
        CursorLine       = 0;
        FirstLineShowing = 0;
        _moveOffset      = -1;
        LinesShowing     = 0;
    }

    protected override int LetterUnderCursor( float x )
    {
        if ( LinesBreak?.Count <= 0 )
        {
            return 0;
        }

        if ( ( CursorLine * 2 ) >= LinesBreak?.Count )
        {
            return Text?.Length ?? throw new GdxRuntimeException( "member 'text' is null!" );
        }

        var glyphPos = GlyphPositions.ToArray();
        var start    = LinesBreak![ CursorLine * 2 ];

        x += glyphPos[ start ];

        var end = LinesBreak?[ ( CursorLine * 2 ) + 1 ];
        var i   = start;

        for ( ; i < end; i++ )
        {
            if ( glyphPos[ i ] > x )
            {
                break;
            }
        }

        if ( ( i > 0 ) && ( ( glyphPos[ i ] - x ) <= ( x - glyphPos[ i - 1 ] ) ) )
        {
            return i;
        }

        return Math.Max( 0, i - 1 );
    }

    public override void SetStyle( TextFieldStyle? style )
    {
        ArgumentNullException.ThrowIfNull( style );

        Style = style;

        // no extra descent to fake line height
        TextHeight = ( float ) ( style.Font?.GetCapHeight() - style.Font?.GetDescent() )!;

        if ( Text != null )
        {
            UpdateDisplayText();
        }

        InvalidateHierarchy();
    }

    /// <summary>
    /// Sets the preferred number of rows (lines) for this text area.
    /// Used to calculate preferred height
    /// </summary>
    public void SetPrefRows( float prefRows )
    {
        _prefRows = prefRows;
    }

    public override float GetPrefHeight()
    {
        if ( _prefRows <= 0 )
        {
            return base.GetPrefHeight();
        }

        if ( Style?.Font == null )
        {
            return base.GetPrefHeight();
        }

        // without ceil we might end up with one less row then expected due to
        // how linesShowing is calculated in SizeChanged and GetHeight() returning
        // rounded value.
        float prefHeight = MathUtils.Ceil( Style.Font.GetLineHeight() * _prefRows );

        if ( Style.Background != null )
        {
            prefHeight = Math.Max( prefHeight + Style.Background.BottomHeight + Style.Background.TopHeight,
                                   Style.Background.MinHeight );
        }

        return prefHeight;
    }

    /// <summary>
    /// Returns total number of lines that the text occupies
    /// </summary>
    public int GetLines()
    {
        return ( LinesBreak!.Count / 2 ) + ( NewLineAtEnd() ? 1 : 0 );
    }

    /// <summary>
    /// Returns if there's a new line at then end of the text
    /// </summary>
    public bool NewLineAtEnd()
    {
        if ( Text == null )
        {
            return false;
        }

        return ( Text.Length != 0 ) && ( ( Text[ Text.Length - 1 ] == NEWLINE )
                                      || ( Text[ Text.Length - 1 ] == CARRIAGE_RETURN ) );
    }

    /// <summary>
    /// Moves the cursor to the given number line.
    /// </summary>
    public void MoveCursorLine( int line )
    {
        if ( ( Text == null ) || ( LinesBreak == null ) )
        {
            return;
        }

        if ( line < 0 )
        {
            CursorLine  = 0;
            Cursor      = 0;
            _moveOffset = -1;
        }
        else if ( line >= GetLines() )
        {
            var newLine = GetLines() - 1;
            Cursor = Text.Length;

            if ( ( line > GetLines() ) || ( newLine == CursorLine ) )
            {
                _moveOffset = -1;
            }

            CursorLine = newLine;
        }
        else if ( line != CursorLine )
        {
            if ( _moveOffset < 0 )
            {
                _moveOffset = LinesBreak.Count <= ( CursorLine * 2 )
                                  ? 0
                                  : GlyphPositions[ Cursor ] - GlyphPositions[ LinesBreak[ CursorLine * 2 ] ];
            }

            CursorLine = line;

            Cursor = ( CursorLine * 2 ) >= LinesBreak.Count
                         ? Text.Length
                         : LinesBreak[ CursorLine * 2 ];

            while ( ( Cursor < Text.Length )
                 && ( Cursor <= ( LinesBreak[ ( CursorLine * 2 ) + 1 ] - 1 ) )
                 && ( ( GlyphPositions[ Cursor ] - GlyphPositions[ LinesBreak[ CursorLine * 2 ] ] ) < _moveOffset ) )
            {
                Cursor++;
            }

            ShowCursor();
        }
    }

    /// <summary>
    /// Updates the current line, checking the cursor position in the text
    /// </summary>
    private void UpdateCurrentLine()
    {
        var index = CalculateCurrentLineIndex( Cursor );
        var line  = index / 2;

        if ( ( Text == null ) || ( LinesBreak == null ) )
        {
            return;
        }

        // Special case when cursor moves to the beginning of the line from
        // the end of another and a word wider than the box
        if ( ( ( index % 2 ) == 0 )
          || ( ( index + 1 ) >= LinesBreak.Count )
          || ( Cursor != LinesBreak[ index ] )
          || ( LinesBreak[ index + 1 ] != LinesBreak[ index ] ) )
        {
            if ( ( line < ( LinesBreak.Count / 2 ) )
              || ( Text.Length == 0 )
              || ( Text[ Text.Length - 1 ] == NEWLINE )
              || ( Text[ Text.Length - 1 ] == CARRIAGE_RETURN ) )
            {
                CursorLine = line;
            }
        }

        // fix for drag-selecting text out of the TextArea's bounds
        UpdateFirstLineShowing();
    }

    /// <summary>
    /// Scroll the text area to show the line of the cursor
    /// </summary>
    private void ShowCursor()
    {
        UpdateCurrentLine();
        UpdateFirstLineShowing();
    }

    private void UpdateFirstLineShowing()
    {
        if ( CursorLine != FirstLineShowing )
        {
            var step = CursorLine >= FirstLineShowing ? 1 : -1;

            while ( ( FirstLineShowing > CursorLine )
                 || ( ( ( FirstLineShowing + LinesShowing ) - 1 ) < CursorLine ) )
            {
                FirstLineShowing += step;
            }
        }
    }

    /// <summary>
    /// Calculates the text area line for the given cursor position
    /// </summary>
    private int CalculateCurrentLineIndex( int cursor )
    {
        var index = 0;

        while ( ( index < LinesBreak?.Count ) && ( cursor > LinesBreak[ index ] ) )
        {
            index++;
        }

        return index;
    }

    public override void SizeChanged()
    {
        // Cause calculateOffsets to recalculate the line breaks.
        _lastText = null;

        // The number of lines showed must be updated whenever the height is updated
        var font       = Style?.Font;
        var background = Style?.Background;

        var availableHeight = Height
                            - ( background == null
                                    ? 0
                                    : background.BottomHeight + background.TopHeight );

        LinesShowing = ( int ) Math.Floor( availableHeight / font!.GetLineHeight() );
    }

    protected override float GetTextY( BitmapFont font, IDrawable? background )
    {
        var textY = Height;

        if ( background != null )
        {
            textY -= background.TopHeight;
        }

        if ( font.UseIntegerPositions )
        {
            textY = ( int ) textY;
        }

        return textY;
    }

    protected override void DrawSelection( IDrawable selection, IBatch batch, BitmapFont font, float x, float y )
    {
        var i          = FirstLineShowing * 2;
        var offsetY    = 0f;
        var minIndex   = Math.Min( Cursor, SelectionStart );
        var maxIndex   = Math.Max( Cursor, SelectionStart );
        var lineHeight = Style!.Font!.GetLineHeight();

        var fontData = font.Data;

        while ( ( ( i + 1 ) < LinesBreak?.Count ) && ( i < ( ( FirstLineShowing + LinesShowing ) * 2 ) ) )
        {
            var lineStart = LinesBreak[ i ];
            var lineEnd   = LinesBreak[ i + 1 ];

            if ( !( ( ( minIndex < lineStart )
                   && ( minIndex < lineEnd )
                   && ( maxIndex < lineStart )
                   && ( maxIndex < lineEnd ) )
                 || ( ( minIndex > lineStart )
                   && ( minIndex > lineEnd )
                   && ( maxIndex > lineStart )
                   && ( maxIndex > lineEnd ) ) ) )
            {
                var start = Math.Max( lineStart, minIndex );
                var end   = Math.Min( lineEnd, maxIndex );

                float fontLineOffsetX     = 0;
                float fontLineOffsetWidth = 0;

                // we can't use fontOffset as it is valid only for first glyph/line in the text
                // we will grab first character in this line and calculate proper offset for this line
                var lineFirst = fontData.GetGlyph( DisplayText![ lineStart ] );

                if ( lineFirst != null )
                {
                    // see BitmapFontData.getGlyphs()#852 for offset calculation
                    // if selection starts when line starts we want to offset width instead of moving the start as it looks better
                    if ( start == lineStart )
                    {
                        fontLineOffsetWidth = lineFirst.FixedWidth
                                                  ? 0
                                                  : ( -lineFirst.Xoffset * fontData.ScaleX ) - fontData.PadLeft;
                    }
                    else
                    {
                        fontLineOffsetX = lineFirst.FixedWidth
                                              ? 0
                                              : ( -lineFirst.Xoffset * fontData.ScaleX ) - fontData.PadLeft;
                    }
                }

                var selectionX     = GlyphPositions[ start ] - GlyphPositions[ lineStart ];
                var selectionWidth = GlyphPositions[ end ] - GlyphPositions[ start ];

                selection.Draw( batch,
                                x + selectionX + fontLineOffsetX,
                                y - lineHeight - offsetY,
                                selectionWidth + fontLineOffsetWidth,
                                font.GetLineHeight() );
            }

            offsetY += font.GetLineHeight();
            i       += 2;
        }
    }

    protected override void DrawText( IBatch batch, BitmapFont font, float x, float y )
    {
        if ( ( Style == null ) || ( LinesBreak == null ) )
        {
            return;
        }

        var offsetY = -( Style.Font?.GetLineHeight() - TextHeight ) / 2;

        for ( var i = FirstLineShowing * 2;
              ( i < ( ( FirstLineShowing + LinesShowing ) * 2 ) ) && ( i < LinesBreak.Count );
              i += 2 )
        {
            font.Draw( batch, DisplayText!, x, y + ( offsetY ?? 0 ), LinesBreak[ i ], LinesBreak[ i + 1 ], 0, Align.LEFT, false );
            offsetY -= font.GetLineHeight();
        }
    }

    protected override void DrawCursor( IDrawable cursorPatch, IBatch batch, BitmapFont font, float x, float y )
    {
        cursorPatch.Draw( batch, x + GetCursorX(), y + GetCursorY(), cursorPatch.MinWidth, font.GetLineHeight() );
    }

    public override void CalculateOffsets()
    {
        GdxRuntimeException.ThrowIfNull( Text );
        GdxRuntimeException.ThrowIfNull( Style );

        base.CalculateOffsets();

        if ( !Text.Equals( _lastText ) )
        {
            _lastText = Text;

            var maxWidthLine = Width
                             - ( Style.Background != null
                                     ? Style.Background.LeftWidth + Style.Background.RightWidth
                                     : 0 );

            LinesBreak?.Clear();

            var lineStart = 0;
            var lastSpace = 0;

            Pool< GlyphLayout > layoutPool = Pools< GlyphLayout >.Get();
            var                 layout     = layoutPool.Obtain();

            for ( var i = 0; i < Text.Length; i++ )
            {
                var lastCharacter = Text[ i ];

                if ( lastCharacter is CARRIAGE_RETURN or NEWLINE )
                {
                    LinesBreak?.Add( lineStart );
                    LinesBreak?.Add( i );
                    lineStart = i + 1;
                }
                else
                {
                    lastSpace = ContinueCursor( i, 0 ) ? lastSpace : i;
                    layout?.SetText( Style.Font!, Text.Substring( lineStart, i + 1 ) );

                    if ( layout?.Width > maxWidthLine )
                    {
                        if ( lineStart >= lastSpace )
                        {
                            lastSpace = i - 1;
                        }

                        LinesBreak?.Add( lineStart );
                        LinesBreak?.Add( lastSpace + 1 );

                        lineStart = lastSpace + 1;
                        lastSpace = lineStart;
                    }
                }
            }

            layoutPool.Free( layout! );

            // Add last line
            if ( lineStart < Text.Length )
            {
                LinesBreak?.Add( lineStart );
                LinesBreak?.Add( Text.Length );
            }

            ShowCursor();
        }
    }

    protected override InputListener CreateInputListener()
    {
        return new TextAreaListener( this );
    }

    public override void SetSelection( int selectionStart, int selectionEnd )
    {
        base.SetSelection( selectionStart, selectionEnd );
        UpdateCurrentLine();
    }

    protected override void MoveCursor( bool forward, bool jump )
    {
        var count = forward ? 1 : -1;
        var index = ( CursorLine * 2 ) + count;

        if ( ( index >= 0 )
          && ( ( index + 1 ) < LinesBreak?.Count )
          && ( LinesBreak[ index ] == Cursor )
          && ( LinesBreak[ index + 1 ] == Cursor ) )
        {
            CursorLine += count;

            if ( jump )
            {
                base.MoveCursor( forward, jump );
            }

            ShowCursor();
        }
        else
        {
            base.MoveCursor( forward, jump );
        }

        UpdateCurrentLine();
    }

    protected override bool ContinueCursor( int index, int offset )
    {
        if ( LinesBreak == null )
        {
            return false;
        }

        var pos = CalculateCurrentLineIndex( index + offset );

        return base.ContinueCursor( index, offset )
            && ( ( pos < 0 )
              || ( pos >= ( LinesBreak.Count - 2 ) )
              || ( LinesBreak[ pos + 1 ] != index )
              || ( LinesBreak[ pos + 1 ] == LinesBreak[ pos + 2 ] ) );
    }

    public float GetCursorX()
    {
        float textOffset = 0;
        var   fontData   = Style?.Font?.Data;

        if ( DisplayText != null )
        {
            if ( !( ( Cursor >= GlyphPositions.Count ) || ( ( CursorLine * 2 ) >= LinesBreak?.Count ) ) )
            {
                var   lineStart   = LinesBreak![ CursorLine * 2 ];
                float glyphOffset = 0;
                var   lineFirst   = fontData?.GetGlyph( DisplayText[ lineStart ] );

                if ( lineFirst != null )
                {
                    glyphOffset = lineFirst.FixedWidth ? 0 : ( -lineFirst.Xoffset * fontData!.ScaleX ) - fontData.PadLeft;
                }

                textOffset = ( GlyphPositions[ Cursor ] - GlyphPositions[ lineStart ] ) + glyphOffset;
            }
        }

        return textOffset + fontData!.CursorX;
    }

    public float GetCursorY()
    {
        return -( ( CursorLine - FirstLineShowing ) + 1 ) * Style!.Font!.GetLineHeight();
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Input listener for the text area.
    /// </summary>
    [PublicAPI]
    public class TextAreaListener : TextFieldClickListener
    {
        private readonly TextArea _parent;

        public TextAreaListener( TextArea ta )
        {
            _parent = ta;
        }

        protected override void SetCursorPosition( float x, float y )
        {
            _parent._moveOffset = -1;

            var background = _parent.Style?.Background;
            var font       = _parent.Style?.Font;

            var height = _parent.Height;

            if ( background != null )
            {
                height -= background.TopHeight;
                x      -= background.LeftWidth;
            }

            x = Math.Max( 0, x );

            if ( background != null )
            {
                y -= background.TopHeight;
            }

            _parent.CursorLine = ( int ) Math.Floor( ( height - y ) / font!.GetLineHeight() ) + _parent.FirstLineShowing;
            _parent.CursorLine = Math.Max( 0, Math.Min( _parent.CursorLine, _parent.GetLines() - 1 ) );

            base.SetCursorPosition( x, y );
            _parent.UpdateCurrentLine();
        }

        public override bool KeyDown( InputEvent? ev, int keycode )
        {
            var result = base.KeyDown( ev, keycode );

            if ( _parent.HasKeyboardFocus() )
            {
                var repeat = false;

                var shift = GdxApi.Input.IsKeyPressed( IInput.Keys.SHIFT_LEFT )
                         || GdxApi.Input.IsKeyPressed( IInput.Keys.SHIFT_RIGHT );

                if ( keycode == IInput.Keys.DOWN )
                {
                    if ( shift )
                    {
                        if ( !_parent.HasSelection )
                        {
                            _parent.SelectionStart = _parent.Cursor;
                            _parent.HasSelection   = true;
                        }
                    }
                    else
                    {
                        _parent.ClearSelection();
                    }

                    _parent.MoveCursorLine( _parent.CursorLine + 1 );

                    repeat = true;
                }
                else if ( keycode == IInput.Keys.UP )
                {
                    if ( shift )
                    {
                        if ( !_parent.HasSelection )
                        {
                            _parent.SelectionStart = _parent.Cursor;
                            _parent.HasSelection   = true;
                        }
                    }
                    else
                    {
                        _parent.ClearSelection();
                    }

                    _parent.MoveCursorLine( _parent.CursorLine - 1 );
                    repeat = true;
                }
                else
                {
                    _parent._moveOffset = -1;
                }

                if ( repeat )
                {
                    base.ScheduleKeyRepeatTask( keycode );
                }

                _parent.ShowCursor();

                return true;
            }

            return result;
        }

        protected override bool CheckFocusTraversal( char character )
        {
            return _parent.FocusTraversal && ( character == TAB );
        }

        public override bool KeyTyped( InputEvent? ev, char character )
        {
            var result = base.KeyTyped( ev, character );

            _parent.ShowCursor();

            return result;
        }

        protected override void GoHome( bool jump )
        {
            if ( jump )
            {
                _parent.Cursor = 0;
            }
            else if ( ( _parent.CursorLine * 2 ) < _parent.LinesBreak?.Count )
            {
                _parent.Cursor = _parent.LinesBreak[ _parent.CursorLine * 2 ];
            }
        }

        protected override void GoEnd( bool jump )
        {
            if ( jump || ( _parent.CursorLine >= _parent.GetLines() ) )
            {
                _parent.Cursor = _parent.Text!.Length;
            }
            else if ( ( ( _parent.CursorLine * 2 ) + 1 ) < _parent.LinesBreak!.Count )
            {
                _parent.Cursor = _parent.LinesBreak[ ( _parent.CursorLine * 2 ) + 1 ];
            }
        }
    }
}
