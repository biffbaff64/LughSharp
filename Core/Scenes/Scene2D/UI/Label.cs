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

using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A text label, with optional word wrapping.
/// <para>
/// The preferred size of the label is determined by the actual text bounds,
/// unless <see cref="SetWrap(bool)"/> is enabled.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class Label : Widget
{
    private readonly static Color       _tempColor      = new();
    private readonly static GlyphLayout _prefSizeLayout = new();

    protected BitmapFontCache FontCache  { get; set; }
    public    StringBuilder   Text       { get; }      = new();
    public    int             LabelAlign { get; set; } = Align.Left;
    public    int             LineAlign  { get; set; } = Align.Left;

    private          LabelStyle  _style;
    private readonly GlyphLayout _layout   = new();
    private readonly Vector2     _prefSize = new();
    private          int         _intValue = int.MinValue;
    private          float       _lastPrefHeight;
    private          bool        _prefSizeInvalid  = true;
    private          float       _fontScaleX       = 1;
    private          float       _fontScaleY       = 1;
    private          bool        _fontScaleChanged = false;
    private          string?     _ellipsis;
    private          bool        _wrap;

    public Label( string text, Skin skin )
        : this( text, skin.Get< LabelStyle >() )
    {
    }

    public Label( string text, Skin skin, string styleName )
        : this( text, ( LabelStyle )skin.Get< LabelStyle >( styleName ) )
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
            this.Text.Append( text );
        }

        this.Style = style;

        if ( text is { Length: > 0 } )
        {
            SetSize( PrefWidth, PrefHeight );
        }
    }

    public LabelStyle Style
    {
        // Returns the label's style. Modifying the returned style may not have an
        // effect until <see cref="SetStyle(LabelStyle)"/> is called.
        get => _style;
        set
        {
            if ( value == null ) throw new ArgumentException( "style cannot be null." );
            if ( value.Font == null ) throw new ArgumentException( "Missing LabelStyle font." );

            this._style = value;

            FontCache = _style.Font.NewFontCache();

            InvalidateHierarchy();
        }
    }

    /// <summary>
    /// Sets the text to the specified integer value. If the text is already equivalent
    /// to the specified value, a string is not allocated.
    /// </summary>
    /// <returns> true if the text was changed. </returns>
    public bool SetText( int value )
    {
        if ( this._intValue == value ) return false;

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
            if ( Text.Length == 0 ) return;

            Text.Clear();
        }
        else
        {
            if ( TextEquals( newText ) ) return;

            Text.Clear();
            Text.Append( newText );
        }

        _intValue = int.MinValue;

        InvalidateHierarchy();
    }

    public bool TextEquals( string other )
    {
        if ( Text.Length != other.Length ) return false;

        for ( var i = 0; i < Text.Length; i++ )
        {
            if ( Text[ i ] != other[ i ] ) return false;
        }

        return true;
    }

    public new void Invalidate()
    {
        base.Invalidate();
        _prefSizeInvalid = true;
    }

    private void ScaleAndComputePrefSize()
    {
        BitmapFont font = FontCache.GetFont();

        var oldScaleX = font.GetScaleX();
        var oldScaleY = font.GetScaleY();

        if ( _fontScaleChanged ) font.GetData().SetScale( _fontScaleX, _fontScaleY );

        ComputePrefSize();

        if ( _fontScaleChanged ) font.GetData().SetScale( oldScaleX, oldScaleY );
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

            _prefSizeLayout.SetText( FontCache.GetFont(), Text.ToString(), Color.White, width, Align.Left, true );
        }
        else
        {
            _prefSizeLayout.SetText( FontCache.GetFont(), Text.ToString() );
        }

        _prefSize.Set( _prefSizeLayout.Width, _prefSizeLayout.Height );
    }

    public new void Layout()
    {
        BitmapFont font = FontCache.GetFont();

        var oldScaleX = font.GetScaleX();
        var oldScaleY = font.GetScaleY();

        if ( _fontScaleChanged ) font.GetData().SetScale( _fontScaleX, _fontScaleY );

        var wrap = this.Wrap && ( _ellipsis == null );

        if ( wrap )
        {
            var prefHeight = getPrefHeight();

            if ( !prefHeight.Equals( _lastPrefHeight ) )
            {
                _lastPrefHeight = prefHeight;
                InvalidateHierarchy();
            }
        }

        var        width      = Width;
        var        height     = Height;
        IDrawable? background = _style.Background;
        float      x          = 0;
        float      y          = 0;

        if ( background != null )
        {
            x      =  background.LeftWidth;
            y      =  background.BottomHeight;
            width  -= ( background.LeftWidth + background.RightWidth );
            height -= ( background.BottomHeight + background.TopHeight );
        }

        GlyphLayout layout = this._layout;
        float       textWidth, textHeight;

        if ( wrap || ( Text.ToString().IndexOf( "\n", StringComparison.Ordinal ) != -1 ) )
        {
            // If the text can span multiple lines, determine the text's actual
            // size so it can be aligned within the label.
            layout.SetText( font, Text.ToString(), 0, Text.Length, Color.White, width, LineAlign, wrap, _ellipsis );

            textWidth  = layout.Width;
            textHeight = layout.Height;

            if ( ( LabelAlign & Align.Left ) == 0 )
            {
                if ( ( LabelAlign & Align.Right ) != 0 )
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
            textHeight = font.GetData().CapHeight;
        }

        if ( ( LabelAlign & Align.Top ) != 0 )
        {
            y += FontCache.GetFont().IsFlipped() ? 0 : height - textHeight;
            y += Style.Font.GetDescent();
        }
        else if ( ( LabelAlign & Align.Bottom ) != 0 )
        {
            y += FontCache.GetFont().IsFlipped() ? height - textHeight : 0;
            y -= Style.Font.GetDescent();
        }
        else
        {
            y += ( height - textHeight ) / 2;
        }

        if ( !FontCache.GetFont().IsFlipped() )
        {
            y += textHeight;
        }

        layout.SetText( font, Text.ToString(), 0, Text.Length, Color.White, textWidth, LineAlign, wrap, _ellipsis );
        FontCache.SetText( layout, x, y );

        if ( _fontScaleChanged ) font.GetData().SetScale( oldScaleX, oldScaleY );
    }

    public new void Draw( IBatch batch, float parentAlpha )
    {
        Validate();
        
        Color color = _tempColor.Set( Color! );
        color.A *= parentAlpha;

        if ( Style.Background != null )
        {
            batch.SetColor( color.R, color.G, color.B, color.A );
            Style.Background.Draw( batch, X, Y, Width, Height );
        }

        if ( Style.FontColor != null ) color.Mul( Style.FontColor );
        
        FontCache.Tint( color );
        FontCache.SetPosition( X, Y );
        FontCache.Draw( batch );
    }

    public float getPrefWidth()
    {
        if ( wrap ) return 0;
        if ( prefSizeInvalid ) ScaleAndComputePrefSize();
        float    width      = prefSize.x;
        Drawable background = style.background;

        if ( background != null )
            width = Math.max
                ( width + background.getLeftWidth() + background.getRightWidth(), background.getMinWidth() );

        return width;
    }

    public float getPrefHeight()
    {
        if ( prefSizeInvalid ) ScaleAndComputePrefSize();
        float descentScaleCorrection = 1;
        if ( fontScaleChanged ) descentScaleCorrection = fontScaleY / style.font.getScaleY();
        var      height = prefSize.y - style.font.getDescent() * descentScaleCorrection * 2;
        Drawable background = style.background;

        if ( background != null )
            height = Math.max
                ( height + background.getTopHeight() + background.getBottomHeight(), background.getMinHeight() );

        return height;
    }

    public GlyphLayout getGlyphLayout()
    {
        return Layout;
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
        this.LabelAlign = labelAlign;

        if ( ( lineAlign & Align.Left ) != 0 )
        {
            this.LineAlign = Align.Left;
        }
        else if ( ( lineAlign & Align.Right ) != 0 )
        {
            this.LineAlign = Align.Right;
        }
        else
        {
            this.LineAlign = Align.Center;
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

        this.FontScaleX = fontScaleX;
        this.FontScaleY = fontScaleY;

        InvalidateHierarchy();
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
    /// When non-null the text will be truncated "..." if it does not fit within
    /// the width of the label. Wrapping will not occur when ellipsis is enabled.
    /// Default is false.
    /// </summary>
    public void SetEllipsis( string? ellipsis )
    {
        this._ellipsis = ellipsis;
    }

    /// <summary>
    /// When true the text will be truncated "..." if it does not fit within the
    /// width of the label. Wrapping will not occur when ellipsis is true. Default
    /// is false.
    /// </summary>
    public void SetEllipsis( bool ellipsis )
    {
        this._ellipsis = ellipsis ? "..." : null;
    }

    public new string ToString()
    {
        var name = Name;

        if ( name != null ) return name;

        var className = GetType().Name;
        var dotIndex  = className.LastIndexOf( '.' );

        if ( dotIndex != -1 ) className = className.Substring( dotIndex + 1 );

        return ( className.IndexOf( '$' ) != -1 ? "Label " : "" ) + className + ": " + Text;
    }

    // ------------------------------------------------------------------------

    #region labelstyle

    /// <summary>
    /// The style for a label, see <seealso cref="Label"/>.
    /// @author Nathan Sweet 
    /// </summary>
    public class LabelStyle
    {
        public BitmapFont? Font       { get; set; }
        public Color?      FontColor  { get; set; }
        public IDrawable?  Background { get; set; }

        public LabelStyle()
        {
        }

        public LabelStyle( BitmapFont font, Color fontColor )
        {
            this.Font      = font;
            this.FontColor = fontColor;
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
    }

    #endregion labelstyle

}