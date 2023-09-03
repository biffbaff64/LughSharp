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

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[PublicAPI]
public class TextButton : Button
{
    private Label?           _label;
    private TextButtonStyle? _style;

    public TextButton( string? text, Skin skin )
        : this( text, skin.Get< TextButtonStyle >() )
    {
        Skin = skin;
    }

    public TextButton( string? text, Skin skin, string styleName )
        : this( text, ( TextButtonStyle )skin.Get< TextButton.TextButtonStyle >( styleName ) )
    {
        Skin = skin;
    }

    public TextButton( string? text, TextButton.TextButtonStyle style )
    {
        this.Style = style;

        _label = new Label( text, new Label.LabelStyle( style.Font!, style.FontColor! ) );
        _label.SetAlignment( Align.CENTER );

        Add( _label ).Expand().SetFill();

        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    /// <summary>
    /// Returns the appropriate label font color from the style based on the current button state.
    /// </summary>
    public Color? GetFontColor()
    {
        System.Diagnostics.Debug.Assert( Style != null, "Style != null" );

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

    public new void Draw( IBatch batch, float parentAlpha )
    {
        if ( Label != null )
        {
            Label.Style.FontColor = GetFontColor();
            base.Draw( batch, parentAlpha );
        }
    }

    // This method needs to be public...
    // ReSharper disable once MemberCanBeProtected.Global
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

            this._style = value;
            base.Style  = value;

            if ( Label != null )
            {
                TextButtonStyle textButtonStyle = value;

                Label.LabelStyle labelStyle = Label.Style;

                labelStyle.Font      = textButtonStyle.Font!;
                labelStyle.FontColor = textButtonStyle.FontColor;
                Label.Style          = labelStyle;
            }
        }
    }

    public Label? Label
    {
        get => _label;
        set
        {
            ArgumentNullException.ThrowIfNull( value );

            GetLabelCell()!.Actor = value;
            this._label           = value;
        }
    }

    public Cell? GetLabelCell()
    {
        return GetCell( _label! );
    }

    public void SetText( string? text ) => _label?.SetText( text );

    public string? GetText() => _label?.Text.ToString();

    // ------------------------------------------------------------------------

    public new string ToString()
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

        return ( className.IndexOf( '$' ) != -1 ? "TextButton " : "" ) + className + ": " + _label?.Text;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// The style for a text button, see <see cref="TextButton"/>.
    /// </summary>
    public class TextButtonStyle : ButtonStyle
    {
        public BitmapFont? Font { get; protected init; }

        public Color? FontColor               { get; protected init; }
        public Color? DownFontColor           { get; set; }
        public Color? OverFontColor           { get; set; }
        public Color? FocusedFontColor        { get; set; }
        public Color? DisabledFontColor       { get; set; }
        public Color? CheckedFontColor        { get; set; }
        public Color? CheckedDownFontColor    { get; set; }
        public Color? CheckedOverFontColor    { get; set; }
        public Color? CheckedFocusedFontColor { get; set; }

        protected TextButtonStyle()
        {
        }

        public TextButtonStyle( IDrawable upImage, IDrawable downImage, IDrawable checkedImage, BitmapFont font )
            : base( upImage, downImage, checkedImage )
        {
            this.Font = font;
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
    }
}