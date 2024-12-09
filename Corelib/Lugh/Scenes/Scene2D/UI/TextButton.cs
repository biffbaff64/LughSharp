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

using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Scenes.Scene2D.Utils;
using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

[PublicAPI]
public class TextButton : Button
{
    private Label?           _label;
    private TextButtonStyle? _style;

    // ========================================================================

    /// <summary>
    /// Creates a new TextButton using the supplied <see cref="Skin"/>, and
    /// setting its text property to the supplied text.
    /// The default <see cref="TextButtonStyle"/> will be used.
    /// </summary>
    public TextButton( string? text, Skin skin )
        : this( text, skin.Get< TextButtonStyle >() )
    {
        Skin = skin;
    }

    /// <summary>
    /// Creates a new TextButton using the supplied <see cref="Skin"/>, and
    /// setting its text property to the supplied text.
    /// A <see cref="TextButtonStyle"/> identified by <tt>styleName</tt> will be used.
    /// </summary>
    public TextButton( string? text, Skin skin, string styleName )
        : this( text, skin.Get< TextButtonStyle >( styleName ) )
    {
        Skin = skin;
    }

    /// <summary>
    /// Creates a new TextButton, setting its text property to the supplied text.
    /// The supplied <see cref="TextButtonStyle"/> will be used.
    /// </summary>
    public TextButton( string? text, TextButtonStyle style )
    {
        Style = style;

        _label = new Label( text, new Label.LabelStyle( style.Font!, style.FontColor! ) );
        _label.SetAlignment( Align.CENTER );

        Add( _label ).Expand().SetFill();

        NonVirtualSetup();
    }

    /// <summary>
    /// Property: The <see cref="TextButtonStyle"/> for this TextButton.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if an attempt to set Style to null is made.
    /// </exception>
    public new TextButtonStyle? Style
    {
        get => _style;
        set
        {
            ArgumentNullException.ThrowIfNull( value );

            if ( value.GetType() != typeof( TextButtonStyle ) )
            {
                throw new ArgumentException( "style must be a TextButtonStyle." );
            }

            _style     = value;
            base.Style = value;

            if ( Label != null )
            {
                var labelStyle = Label.Style;

                labelStyle.Font      = value.Font!;
                labelStyle.FontColor = value.FontColor;
                Label.Style          = labelStyle;
            }
        }
    }

    /// <summary>
    /// A Text<see cref="Label"/> which is used to store the text for this button.
    /// </summary>
    public Label? Label
    {
        get => _label;
        set
        {
            ArgumentNullException.ThrowIfNull( value );

            GetLabelCell()!.Actor = value;
            _label                = value;
        }
    }

    /// <summary>
    /// Returns the appropriate label font color from the style based on
    /// the current button state.
    /// </summary>
    public Color? GetFontColor()
    {
        Debug.Assert( Style != null, "Style is null" );

        if ( IsDisabled && ( Style.DisabledFontColor != null ) )
        {
            return Style.DisabledFontColor;
        }

        if ( IsPressed() )
        {
            if ( IsChecked && ( Style.CheckedDownFontColor != null ) )
            {
                return Style.CheckedDownFontColor;
            }

            if ( Style.DownFontColor != null )
            {
                return Style.DownFontColor;
            }
        }

        if ( IsOver() )
        {
            if ( IsChecked )
            {
                if ( Style.CheckedOverFontColor != null )
                {
                    return Style.CheckedOverFontColor;
                }
            }
            else
            {
                if ( Style.OverFontColor != null )
                {
                    return Style.OverFontColor;
                }
            }
        }

        var focused = HasKeyboardFocus();

        if ( IsChecked )
        {
            if ( focused && ( Style.CheckedFocusedFontColor != null ) )
            {
                return Style.CheckedFocusedFontColor;
            }

            if ( Style.CheckedFontColor != null )
            {
                return Style.CheckedFontColor;
            }

            if ( IsOver() && ( Style.OverFontColor != null ) )
            {
                return Style.OverFontColor;
            }
        }

        if ( focused && ( Style?.FocusedFontColor != null ) )
        {
            return Style.FocusedFontColor;
        }

        return Style?.FontColor;
    }

    /// <inheritdoc />
    public override void Draw( IBatch batch, float parentAlpha )
    {
        if ( Label != null )
        {
            Label.Style.FontColor = GetFontColor();
            base.Draw( batch, parentAlpha );
        }
    }

    public Cell?   GetLabelCell()          => GetCell( _label! );
    public void    SetText( string? text ) => _label?.SetText( text );
    public string? GetText()               => _label?.Text.ToString();

    /// <summary>
    /// Private setup method to allow calls to virtual methods that can't
    /// be called from constructors.
    /// </summary>
    private void NonVirtualSetup()
    {
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    // ========================================================================

    /// <inheritdoc />
    public override string ToString()
    {
        if ( Name != null )
        {
            return Name;
        }

        var className = GetType().Name;
        var dotIndex  = className.LastIndexOf( '.' );

        if ( dotIndex != -1 )
        {
            className = className.Substring( dotIndex + 1 );
        }

        return $"{( className.IndexOf( '$' ) != -1 ? "TextButton " : "" )}{className}: {_label?.Text}";
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// The style for a text button, see <see cref="TextButton"/>.
    /// </summary>
    [PublicAPI]
    public class TextButtonStyle : ButtonStyle
    {
        protected TextButtonStyle()
        {
        }

        public TextButtonStyle( IDrawable upImage, IDrawable downImage, IDrawable checkedImage, BitmapFont font )
            : base( upImage, downImage, checkedImage )
        {
            Font = font;
        }

        protected TextButtonStyle( TextButtonStyle style ) : base( style )
        {
            Font = style.Font;

            if ( style.FontColor != null )
            {
                FontColor = new Color( style.FontColor );
            }

            if ( style.DownFontColor != null )
            {
                DownFontColor = new Color( style.DownFontColor );
            }

            if ( style.OverFontColor != null )
            {
                OverFontColor = new Color( style.OverFontColor );
            }

            if ( style.FocusedFontColor != null )
            {
                FocusedFontColor = new Color( style.FocusedFontColor );
            }

            if ( style.DisabledFontColor != null )
            {
                DisabledFontColor = new Color( style.DisabledFontColor );
            }

            if ( style.CheckedFontColor != null )
            {
                CheckedFontColor = new Color( style.CheckedFontColor );
            }

            if ( style.CheckedDownFontColor != null )
            {
                CheckedDownFontColor = new Color( style.CheckedDownFontColor );
            }

            if ( style.CheckedOverFontColor != null )
            {
                CheckedOverFontColor = new Color( style.CheckedOverFontColor );
            }

            if ( style.CheckedFocusedFontColor != null )
            {
                CheckedFocusedFontColor = new Color( style.CheckedFocusedFontColor );
            }
        }

        public BitmapFont? Font      { get; protected init; }
        public Color?      FontColor { get; protected init; }

        public Color? DownFontColor           { get; set; }
        public Color? OverFontColor           { get; set; }
        public Color? FocusedFontColor        { get; set; }
        public Color? DisabledFontColor       { get; set; }
        public Color? CheckedFontColor        { get; set; }
        public Color? CheckedDownFontColor    { get; set; }
        public Color? CheckedOverFontColor    { get; set; }
        public Color? CheckedFocusedFontColor { get; set; }
    }
}
