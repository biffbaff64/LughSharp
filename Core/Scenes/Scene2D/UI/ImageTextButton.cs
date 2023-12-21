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

namespace LibGDXSharp.Scenes.Scene2D.UI;

[PublicAPI]
public class ImageTextButton : Button
{
    private Image                _image;
    private Label                _label;
    private ImageTextButtonStyle style;

    public ImageTextButton( string text, Skin skin )
        : this( text, skin.Get< ImageTextButtonStyle >() )
    {
        setSkin( skin );
    }

    public ImageTextButton( @Null String text, Skin skin, String styleName) {
        this( text, skin.get( styleName, ImageTextButtonStyle.class));
        setSkin( skin );
    }

    public ImageTextButton( @Null String text, ImageTextButtonStyle style) {
        super( style );
        this.style = style;

        defaults().space( 3 );

        _image = new Image();
        _image.setScaling( Scaling.fit );

        _label = new Label( text, new Label.LabelStyle( style.font, style.fontColor ) );
        _label.setAlignment( Align.center );

        add( _image );
        add( _label );

        setStyle( style );

        setSize( getPrefWidth(), getPrefHeight() );
    }

    public void setStyle( ButtonStyle style )
    {
        if ( !( style instanceof ImageTextButtonStyle)) throw new IllegalArgumentException( "style must be a ImageTextButtonStyle." );
        this.style = ( ImageTextButtonStyle )style;
        super.setStyle( style );

        if ( _image != null ) updateImage();

        if ( _label != null )
        {
            ImageTextButtonStyle textButtonStyle = ( ImageTextButtonStyle )style;
            Label.LabelStyle     labelStyle      = _label.getStyle();
            labelStyle.font      = textButtonStyle.font;
            labelStyle.fontColor = getFontColor();
            _label.setStyle( labelStyle );
        }
    }

    public ImageTextButtonStyle getStyle()
    {
        return style;
    }

    /** Returns the appropriate image drawable from the style based on the current button state. */
    protected IDrawable? getImageDrawable()
    {
        if ( isDisabled() && style.imageDisabled != null ) return style.imageDisabled;

        if ( isPressed() )
        {
            if ( isChecked() && style.imageCheckedDown != null ) return style.imageCheckedDown;
            if ( style.imageDown != null ) return style.imageDown;
        }

        if ( isOver() )
        {
            if ( isChecked() )
            {
                if ( style.imageCheckedOver != null ) return style.imageCheckedOver;
            }
            else
            {
                if ( style.imageOver != null ) return style.imageOver;
            }
        }

        if ( isChecked() )
        {
            if ( style.imageChecked != null ) return style.imageChecked;
            if ( isOver() && style.imageOver != null ) return style.imageOver;
        }

        return style.imageUp;
    }

    /** Sets the image drawable based on the current button state. The default implementation sets the image drawable using
     * {@link #getImageDrawable()}. */
    protected void updateImage()
    {
        _image.setDrawable( getImageDrawable() );
    }

    /// <summary>
    /// Returns the appropriate label font color from the style based on the current button state.
    /// </summary>
    protected Color? GetFontColor()
    {
        if ( isDisabled() && style.disabledFontColor != null )
        {
            return style.disabledFontColor;
        }

        if ( isPressed() )
        {
            if ( isChecked() && style.checkedDownFontColor != null ) return style.checkedDownFontColor;
            if ( style.downFontColor != null ) return style.downFontColor;
        }

        if ( isOver() )
        {
            if ( isChecked() )
            {
                if ( style.checkedOverFontColor != null ) return style.checkedOverFontColor;
            }
            else
            {
                if ( style.overFontColor != null ) return style.overFontColor;
            }
        }

        boolean focused = hasKeyboardFocus();

        if ( isChecked() )
        {
            if ( focused && style.checkedFocusedFontColor != null ) return style.checkedFocusedFontColor;
            if ( style.checkedFontColor != null ) return style.checkedFontColor;
            if ( isOver() && style.overFontColor != null ) return style.overFontColor;
        }

        if ( focused && style.focusedFontColor != null ) return style.focusedFontColor;

        return style.fontColor;
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        updateImage();
        _label.Style.FontColor = GetFontColor();
        base.Draw( batch, parentAlpha );
    }

    public Image GetImage() => _image;

    public Cell? GetImageCell() => GetCell( _image );

    public void SetLabel( Label label )
    {
        GetLabelCell()!.Actor = label;
        this._label = label;
    }

    public Label GetLabel() => _label;

    public Cell? GetLabelCell() => GetCell( _label );

    public string GetText() => _label.Text.ToString();

    public void SetText( string text )
    {
        _label.Text.Clear();
        _label.Text.Append( text );
    }

    public new String ToString()
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

        return ( className.IndexOf( '$' ) != -1 ? "ImageTextButton " : "" )
             + className
             + ": "
             + _image.GetDrawable()
             + " "
             + _label.Text;
    }

    /// The style for an image text button, see {@link ImageTextButton}.
    [PublicAPI]
    public class ImageTextButtonStyle : TextButton.TextButtonStyle
    {
        public IDrawable? ImageUp          { get; set; }
        public IDrawable? ImageDown        { get; set; }
        public IDrawable? ImageOver        { get; set; }
        public IDrawable? ImageDisabled    { get; set; }
        public IDrawable? ImageChecked     { get; set; }
        public IDrawable? ImageCheckedDown { get; set; }
        public IDrawable? ImageCheckedOver { get; set; }

        public ImageTextButtonStyle()
        {
        }

        public ImageTextButtonStyle( IDrawable up, IDrawable down, IDrawable chcked, BitmapFont font )
            : base( up, down, chcked, font )
        {
        }

        public ImageTextButtonStyle( ImageTextButtonStyle style )
            : base( style )

        {
            ImageUp          = style.ImageUp;
            ImageDown        = style.ImageDown;
            ImageOver        = style.ImageOver;
            ImageDisabled    = style.ImageDisabled;
            ImageChecked     = style.ImageChecked;
            ImageCheckedDown = style.ImageCheckedDown;
            ImageCheckedOver = style.ImageCheckedOver;
        }

        public ImageTextButtonStyle( TextButton.TextButtonStyle style )
            : base( style )
        {
        }
    }
}
