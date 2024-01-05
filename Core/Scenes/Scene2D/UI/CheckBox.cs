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

using LibGDXSharp.Graphics.G2D;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

public class CheckBox : TextButton
{
    private CheckBoxStyle? _style;

    public CheckBox( string text, Skin skin )
        : this( text, skin.Get< CheckBoxStyle >() )
    {
    }

    public CheckBox( string text, Skin skin, string styleName )
        : this( text, skin.Get< CheckBoxStyle >( styleName ) )
    {
    }

    public CheckBox( string text, CheckBoxStyle style ) : base( text, style )
    {
        ClearChildren();

        Label? label = Label;

        Image = new Image( style.CheckboxOff, Scaling.None );

        ImageCell = Add( Image );

        Add( label );

        label?.SetAlignment( Align.LEFT );

        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    public new ButtonStyle? Style
    {
        get => _style;
        set
        {
            if ( value is not CheckBoxStyle style )
            {
                throw new ArgumentException( "style must be a CheckBoxStyle." );
            }

            _style     = style;
            base.Style = style;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public Image? Image     { get; set; }
    public Cell   ImageCell { get; set; }

    public new void Draw( IBatch batch, float parentAlpha )
    {
        IDrawable? checkbox = null;

        if ( IsDisabled )
        {
            if ( IsChecked && ( _style?.CheckboxOnDisabled != null ) )
            {
                checkbox = _style.CheckboxOnDisabled;
            }
            else
            {
                checkbox = _style?.CheckboxOffDisabled;
            }
        }

        if ( checkbox == null )
        {
            var over = IsOver() && !IsDisabled;

            if ( IsChecked && ( _style?.CheckboxOn != null ) )
            {
                checkbox = over && ( _style.CheckboxOnOver != null )
                    ? _style.CheckboxOnOver
                    : _style.CheckboxOn;
            }
            else if ( over && ( _style?.CheckboxOver != null ) )
            {
                checkbox = _style.CheckboxOver;
            }
            else
            {
                checkbox = _style?.CheckboxOff;
            }
        }

        Image?.SetDrawable( checkbox );

        base.Draw( batch, parentAlpha );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     The style for a select box, see <seealso cref="CheckBox" />.
    /// </summary>
    public class CheckBoxStyle : TextButtonStyle
    {

        public CheckBoxStyle()
        {
        }

        public CheckBoxStyle( IDrawable checkboxOff, IDrawable checkboxOn, BitmapFont font, Color fontColor )
        {
            CheckboxOff = checkboxOff;
            CheckboxOn  = checkboxOn;
            Font        = font;
            FontColor   = fontColor;
        }

        public CheckBoxStyle( CheckBoxStyle style ) : base( style )
        {
            CheckboxOff = style.CheckboxOff;
            CheckboxOn  = style.CheckboxOn;

            CheckboxOnOver      = style.CheckboxOnOver;
            CheckboxOver        = style.CheckboxOver;
            CheckboxOnDisabled  = style.CheckboxOnDisabled;
            CheckboxOffDisabled = style.CheckboxOffDisabled;
        }

        public IDrawable? CheckboxOn          { get; set; }
        public IDrawable? CheckboxOff         { get; set; }
        public IDrawable? CheckboxOnOver      { get; set; }
        public IDrawable? CheckboxOver        { get; set; }
        public IDrawable? CheckboxOnDisabled  { get; set; }
        public IDrawable? CheckboxOffDisabled { get; set; }
    }
}
