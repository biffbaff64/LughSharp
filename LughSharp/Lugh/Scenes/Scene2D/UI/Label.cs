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

using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Scenes.Scene2D.Utils;
using LughSharp.Lugh.Utils;

namespace LughSharp.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// A text label, with optional word wrapping.
/// <para>
/// The preferred size of the label is determined by the actual text bounds,
/// unless <see cref="Wrap"/> is enabled.
/// </para>
/// </summary>
[PublicAPI]
public class Label : Widget
{
    private static readonly Color       _tempColor      = new();
    private static readonly GlyphLayout _prefSizeLayout = new();
    private readonly        Vector2     _prefSize       = new();

    private string?         _ellipsis;
    private bool            _fontScaleChanged = false;
    private float           _fontScaleX       = 1;
    private float           _fontScaleY       = 1;
    private int             _intValue         = int.MinValue;
    private float           _lastPrefHeight;
    private bool            _prefSizeInvalid = true;
    private LabelStyle      _style           = null!;
    private bool            _wrap;
    public  BitmapFontCache FontCache   { get; set; } = null!;
    public  StringBuilder   Text        { get; }      = new();
    public  int             LabelAlign  { get; set; } = Align.LEFT;
    public  int             LineAlign   { get; set; } = Align.LEFT;
    public  GlyphLayout     GlyphLayout { get; set; } = new();

    public LabelStyle Style
    {
        // Returns the label's style. Modifying the returned style may not have an
        // effect until <see cref="SetStyle(LabelStyle)"/> is called.
        get => _style;
        set
        {
            if ( value == null )
            {
                throw new ArgumentException( "style cannot be null." );
            }

            if ( value.Font == null )
            {
                throw new ArgumentException( "Missing LabelStyle font." );
            }

            _style = value;

            FontCache = _style.Font.NewFontCache();

            InvalidateHierarchy();
        }
    }

    /// <summary>
    /// If false, the text will only wrap where it contains newlines (\n). The preferred
    /// size of the label will be the text bounds.
    /// <para>
    /// If true, the text will word wrap using the width of the label. The preferred width
    /// of the label will be 0, it is expected that something external will set the width
    /// of the label. Wrapping will not occur when ellipsis is enabled.
    /// </para>
    /// <para>
    /// Default is false.
    /// </para>
    /// <para>
    /// When wrap is enabled, the label's preferred height depends on the width of the
    /// label. In some cases the parent of the label will need to layout twice: once to
    /// set the width of the label and a second time to adjust to the label's new preferred
    /// height.
    /// </para>
    /// </summary>
    public bool Wrap
    {
        get => _wrap;
        set
        {
            _wrap = value;

            InvalidateHierarchy();
        }
    }

    public float FontScaleX
    {
        get => _fontScaleX;
        set => SetFontScale( value, FontScaleY );
    }

    public float FontScaleY
    {
        get => _fontScaleY;
        set => SetFontScale( FontScaleX, value );
    }

    /// <summary>
    /// Sets the text to the specified integer value. If the text is already equivalent
    /// to the specified value, a string is not allocated.
    /// </summary>
    /// <returns> true if the text was changed. </returns>
    public bool SetText( int value )
    {
        if ( _intValue == value )
        {
            return false;
        }

        Text.Clear();
        Text.Append( value );
        _intValue = value;

        InvalidateHierarchy();

        return true;
    }

    public void SetText( string? newText )
    {
        if ( newText == null )
        {
            if ( Text.Length == 0 )
            {
                return;
            }

            Text.Clear();
        }
        else
        {
            if ( TextEquals( newText ) )
            {
                return;
            }

            Text.Clear();
            Text.Append( newText );
        }

        _intValue = int.MinValue;

        InvalidateHierarchy();
    }

    public bool TextEquals( string other )
    {
        if ( Text.Length != other.Length )
        {
            return false;
        }

        for ( var i = 0; i < Text.Length; i++ )
        {
            if ( Text[ i ] != other[ i ] )
            {
                return false;
            }
        }

        return true;
    }

    public override void Invalidate()
    {
        base.Invalidate();
        _prefSizeInvalid = true;
    }

    private void ScaleAndComputePrefSize()
    {
        var font = FontCache.Font;

        var oldScaleX = font.GetScaleX();
        var oldScaleY = font.GetScaleY();

        if ( _fontScaleChanged )
        {
            font.Data.SetScale( _fontScaleX, _fontScaleY );
        }

        ComputePrefSize();

        if ( _fontScaleChanged )
        {
            font.Data.SetScale( oldScaleX, oldScaleY );
        }
    }

    private void ComputePrefSize()
    {
        _prefSizeInvalid = false;

        if ( Wrap && ( _ellipsis == null ) )
        {
            var width = Width;

            if ( _style.Background != null )
            {
                width = Math.Max( width, _style.Background.MinWidth )
                      - _style.Background.LeftWidth
                      - _style.Background.RightWidth;
            }

            _prefSizeLayout.SetText( FontCache.Font, Text.ToString(), Color.White, width, Align.LEFT, true );
        }
        else
        {
            _prefSizeLayout.SetText( FontCache.Font, Text.ToString() );
        }

        _prefSize.Set( _prefSizeLayout.Width, _prefSizeLayout.Height );
    }

    public void Layout()
    {
        var font = FontCache.Font;

        var oldScaleX = font.GetScaleX();
        var oldScaleY = font.GetScaleY();

        if ( _fontScaleChanged )
        {
            font.Data.SetScale( _fontScaleX, _fontScaleY );
        }

        var wrap = Wrap && ( _ellipsis == null );

        if ( wrap )
        {
            var prefHeight = GetPrefHeight();

            if ( !prefHeight.Equals( _lastPrefHeight ) )
            {
                _lastPrefHeight = prefHeight;
                InvalidateHierarchy();
            }
        }

        var   width  = Width;
        var   height = Height;
        float x      = 0;
        float y      = 0;

        if ( _style.Background != null )
        {
            x      =  _style.Background.LeftWidth;
            y      =  _style.Background.BottomHeight;
            width  -= _style.Background.LeftWidth + _style.Background.RightWidth;
            height -= _style.Background.BottomHeight + _style.Background.TopHeight;
        }

        var   layout = GlyphLayout;
        float textWidth, textHeight;

        if ( wrap || ( Text.ToString().IndexOf( "\n", StringComparison.Ordinal ) != -1 ) )
        {
            // If the text can span multiple lines, determine the text's actual
            // size so it can be aligned within the label.
            layout.SetText( font, Text.ToString(), 0, Text.Length, Color.White, width, LineAlign, wrap, _ellipsis );

            textWidth  = layout.Width;
            textHeight = layout.Height;

            if ( ( LabelAlign & Align.LEFT ) == 0 )
            {
                if ( ( LabelAlign & Align.RIGHT ) != 0 )
                {
                    x += width - textWidth;
                }
                else
                {
                    x += ( width - textWidth ) / 2;
                }
            }
        }
        else
        {
            textWidth  = width;
            textHeight = font.Data.CapHeight;
        }

        Debug.Assert( Style.Font != null, "Style.Font != null" );

        if ( ( LabelAlign & Align.TOP ) != 0 )
        {
            y += FontCache.Font.Flipped ? 0 : height - textHeight;
            y += Style.Font.GetDescent();
        }
        else if ( ( LabelAlign & Align.BOTTOM ) != 0 )
        {
            y += FontCache.Font.Flipped ? height - textHeight : 0;
            y -= Style.Font.GetDescent();
        }
        else
        {
            y += ( height - textHeight ) / 2;
        }

        if ( !FontCache.Font.Flipped )
        {
            y += textHeight;
        }

        layout.SetText( font, Text.ToString(), 0, Text.Length, Color.White, textWidth, LineAlign, wrap, _ellipsis );
        FontCache.SetText( layout, x, y );

        if ( _fontScaleChanged )
        {
            font.Data.SetScale( oldScaleX, oldScaleY );
        }
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        var color = _tempColor.Set( Color );
        color.A *= parentAlpha;

        if ( Style.Background != null )
        {
            batch.SetColor( color.R, color.G, color.B, color.A );
            Style.Background.Draw( batch, X, Y, Width, Height );
        }

        if ( Style.FontColor != null )
        {
            color.Mul( Style.FontColor );
        }

        FontCache.Tint( color );
        FontCache.SetPosition( X, Y );
        FontCache.Draw( batch );
    }

    public float GetPrefWidth()
    {
        if ( Wrap )
        {
            return 0;
        }

        if ( _prefSizeInvalid )
        {
            ScaleAndComputePrefSize();
        }

        var width = _prefSize.X;

        if ( Style.Background != null )
        {
            width = Math.Max(
                             width + Style.Background.LeftWidth + Style.Background.RightWidth,
                             Style.Background.MinWidth
                            );
        }

        return width;
    }

    public float GetPrefHeight()
    {
        if ( _prefSizeInvalid )
        {
            ScaleAndComputePrefSize();
        }

        float descentScaleCorrection = 1;

        if ( _fontScaleChanged )
        {
            descentScaleCorrection = _fontScaleY / Style.Font.GetScaleY();
        }

        var height = _prefSize.Y - ( Style.Font.GetDescent() * descentScaleCorrection * 2 );

        if ( Style.Background != null )
        {
            height = Math.Max( height + Style.Background.TopHeight + Style.Background.BottomHeight, Style.Background.MinHeight );
        }

        return height;
    }

    /// <summary>
    /// </summary>
    /// <param name="alignment">
    /// Aligns all the text within the label (default left center) and each line
    /// of text horizontally (default is left).
    /// </param>
    /// <see cref="Align"/>
    public void SetAlignment( int alignment )
    {
        SetAlignment( alignment, alignment );
    }

    /// <summary>
    /// <param name="labelAlign"> Aligns all the text within the label (default left center). </param>
    /// <param name="lineAlign"> Aligns each line of text horizontally (default left). </param>
    /// See also <see cref="Align "/>
    /// </summary>
    public void SetAlignment( int labelAlign, int lineAlign )
    {
        LabelAlign = labelAlign;

        if ( ( lineAlign & Align.LEFT ) != 0 )
        {
            LineAlign = Align.LEFT;
        }
        else if ( ( lineAlign & Align.RIGHT ) != 0 )
        {
            LineAlign = Align.RIGHT;
        }
        else
        {
            LineAlign = Align.CENTER;
        }

        Invalidate();
    }

    public void SetFontScale( float fontScale )
    {
        SetFontScale( fontScale, fontScale );
    }

    public void SetFontScale( float fontScaleX, float fontScaleY )
    {
        _fontScaleChanged = true;

        _fontScaleX = fontScaleX;
        _fontScaleY = fontScaleY;

        InvalidateHierarchy();
    }

    /// <summary>
    /// When non-null the text will be truncated "..." if it does not fit within
    /// the width of the label. Wrapping will not occur when ellipsis is enabled.
    /// Default is false.
    /// </summary>
    public void SetEllipsis( string? ellipsis )
    {
        _ellipsis = ellipsis;
    }

    /// <summary>
    /// When true the text will be truncated "..." if it does not fit within the
    /// width of the label. Wrapping will not occur when ellipsis is true. Default
    /// is false.
    /// </summary>
    public void SetEllipsis( bool ellipsis )
    {
        _ellipsis = ellipsis ? "..." : null;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var name = Name;

        if ( name != null )
        {
            return name;
        }

        var className = GetType().Name;
        var dotIndex  = className.LastIndexOf( '.' );

        if ( dotIndex != -1 )
        {
            className = className.Substring( dotIndex + 1 );
        }

        return ( className.IndexOf( '$' ) != -1 ? "Label " : "" ) + className + ": " + Text;
    }

    // ========================================================================

    #region labelstyle

    /// <summary>
    /// The style for a label, see <see cref="Label"/>.
    /// </summary>
    [PublicAPI]
    public class LabelStyle
    {
        public LabelStyle()
        {
            Font       = default( BitmapFont )!;
            FontColor  = null;
            Background = null;
        }

        public LabelStyle( BitmapFont font, Color fontColor )
        {
            Font      = font;
            FontColor = fontColor;
        }

        public LabelStyle( LabelStyle style )
        {
            Font = style.Font;

            if ( style.FontColor != null )
            {
                FontColor = new Color( style.FontColor );
            }

            Background = style.Background;
        }

        public BitmapFont Font       { get; set; }
        public Color?     FontColor  { get; set; }
        public IDrawable? Background { get; set; }
    }

    #endregion labelstyle

    // ========================================================================
    // ========================================================================

    #region constructors

    public Label( string text, Skin skin )
        : this( text, skin.Get< LabelStyle >() )
    {
    }

    public Label( string text, Skin skin, string styleName )
        : this( text, skin.Get< LabelStyle >( styleName ) )
    {
    }

    /// <summary>
    /// Creates a label, using a <see cref="LabelStyle"/> that has a BitmapFont with
    /// the specified name from the skin and the specified color.
    /// </summary>
    public Label( string text, Skin skin, string fontName, Color color )
        : this( text, new LabelStyle( skin.GetFont( fontName ), color ) )
    {
    }

    /// <summary>
    /// Creates a label, using a <see cref="LabelStyle"/> that has a BitmapFont
    /// with the specified name and the specified color from the
    /// skin.
    /// </summary>
    public Label( string text, Skin skin, string fontName, string colorName )
        : this( text, new LabelStyle( skin.GetFont( fontName ), skin.GetColor( colorName ) ) )
    {
    }

    public Label( string? text, LabelStyle style )
    {
        if ( text != null )
        {
            Text.Append( text );
        }

        Style = style;

        if ( text is { Length: > 0 } )
        {
            SetSize( GetPrefWidth(), GetPrefHeight() );
        }
    }

    #endregion constructors
}
