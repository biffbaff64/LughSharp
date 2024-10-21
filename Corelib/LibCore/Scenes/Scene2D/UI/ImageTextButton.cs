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

using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

[PublicAPI]
public class ImageTextButton : Button
{
    private readonly Image?                _image;
    private          Label?                _label;
    private          ImageTextButtonStyle? _style;

    public ImageTextButton( string text, Skin skin )
        : this( text, skin.Get< ImageTextButtonStyle >() )
    {
        Skin = skin;
    }

    public ImageTextButton( string text, Skin skin, string styleName )
        : this( text, skin.Get< ImageTextButtonStyle >( styleName ) )
    {
        Skin = skin;
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="style"></param>
    public ImageTextButton( string text, ImageTextButtonStyle style )
        : base( style )
    {
        _style = style;

        CellDefaults.Space( 3 );

        _image = new Image();
        _image.SetScaling( Scaling.Fit );

        _label = new Label( text, new Label.LabelStyle( style.Font!, style.FontColor! ) );
        _label.SetAlignment( Align.CENTER );

        Add( _image );
        Add( _label );

        SetStyleAndSize( style );
    }

    private void SetStyleAndSize( ImageTextButtonStyle style )
    {
        SetStyle( style );
        SetSize( PrefWidth, PrefHeight );
    }

    public void SetStyle( ButtonStyle style )
    {
        if ( !( style is ImageTextButtonStyle textButtonStyle ) )
        {
            throw new ArgumentException( "style must be a ImageTextButtonStyle." );
        }

        _style = textButtonStyle;
        Style  = textButtonStyle;

        if ( _image != null )
        {
            UpdateImage();
        }

        if ( _label != null )
        {
            var labelStyle = _label.Style;

            labelStyle.Font      = textButtonStyle.Font!;
            labelStyle.FontColor = GetFontColor();
            _label.Style         = labelStyle;
        }
    }

    /// <summary>
    /// Returns the appropriate image drawable from the style based on the
    /// current button state.
    /// </summary>
    public virtual IDrawable? GetImageDrawable()
    {
        if ( IsDisabled && ( _style?.ImageDisabled != null ) )
        {
            return _style.ImageDisabled;
        }

        if ( IsPressed() )
        {
            if ( IsChecked && ( _style?.ImageCheckedDown != null ) )
            {
                return _style.ImageCheckedDown;
            }

            if ( _style?.ImageDown != null )
            {
                return _style.ImageDown;
            }
        }

        if ( IsOver() )
        {
            if ( IsChecked )
            {
                if ( _style?.ImageCheckedOver != null )
                {
                    return _style.ImageCheckedOver;
                }
            }
            else
            {
                if ( _style?.ImageOver != null )
                {
                    return _style.ImageOver;
                }
            }
        }

        if ( IsChecked )
        {
            if ( _style?.ImageChecked != null )
            {
                return _style.ImageChecked;
            }

            if ( IsOver() && ( _style?.ImageOver != null ) )
            {
                return _style.ImageOver;
            }
        }

        return _style?.ImageUp;
    }

    /// <summary>
    /// Sets the image drawable based on the current button state. The default
    /// implementation sets the image drawable using <see cref="GetImageDrawable()"/>.
    /// </summary>
    public virtual void UpdateImage()
    {
        _image?.SetDrawable( GetImageDrawable() );
    }

    /// <summary>
    /// Returns the appropriate label font color from the style based on the current button state.
    /// </summary>
    protected Color? GetFontColor()
    {
        if ( IsDisabled && ( _style?.DisabledFontColor != null ) )
        {
            return _style.DisabledFontColor;
        }

        if ( IsPressed() )
        {
            if ( IsChecked && ( _style?.CheckedDownFontColor != null ) )
            {
                return _style.CheckedDownFontColor;
            }

            if ( _style?.DownFontColor != null )
            {
                return _style.DownFontColor;
            }
        }

        if ( IsOver() )
        {
            if ( IsChecked )
            {
                if ( _style?.CheckedOverFontColor != null )
                {
                    return _style.CheckedOverFontColor;
                }
            }
            else
            {
                if ( _style?.OverFontColor != null )
                {
                    return _style.OverFontColor;
                }
            }
        }

        var focused = HasKeyboardFocus();

        if ( IsChecked )
        {
            if ( focused && ( _style?.CheckedFocusedFontColor != null ) )
            {
                return _style.CheckedFocusedFontColor;
            }

            if ( _style?.CheckedFontColor != null )
            {
                return _style.CheckedFontColor;
            }

            if ( IsOver() && ( _style?.OverFontColor != null ) )
            {
                return _style.OverFontColor;
            }
        }

        if ( focused && ( _style?.FocusedFontColor != null ) )
        {
            return _style.FocusedFontColor;
        }

        return _style?.FontColor;
    }

    /// <inheritdoc />
    public override void Draw( IBatch batch, float parentAlpha )
    {
        UpdateImage();

        if ( _label != null )
        {
            _label.Style.FontColor = GetFontColor();
        }

        base.Draw( batch, parentAlpha );
    }

    public Image? GetImage()
    {
        return _image;
    }

    public Label? GetLabel()
    {
        return _label;
    }

    public Cell? GetImageCell()
    {
        return GetCell( _image! );
    }

    public Cell? GetLabelCell()
    {
        return GetCell( _label! );
    }

    public void SetLabel( Label label )
    {
        GetLabelCell()!.Actor = label;
        _label                = label;
    }

    public string GetText()
    {
        return _label?.Text.ToString() ?? "";
    }

    public void SetText( string text )
    {
        if ( _label != null )
        {
            _label.Text.Clear();
            _label.Text.Append( text );
        }
    }

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

        return ( className.IndexOf( '$' ) != -1 ? "ImageTextButton " : "" )
             + className
             + $":  {_image?.Drawable}  {_label?.Text}";
    }

    /// <summary>
    /// The style for an image text button, see <see cref="ImageTextButton"/>.
    /// </summary>
    [PublicAPI]
    public class ImageTextButtonStyle : TextButton.TextButtonStyle
    {
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

        public IDrawable? ImageUp          { get; set; }
        public IDrawable? ImageDown        { get; set; }
        public IDrawable? ImageOver        { get; set; }
        public IDrawable? ImageDisabled    { get; set; }
        public IDrawable? ImageChecked     { get; set; }
        public IDrawable? ImageCheckedDown { get; set; }
        public IDrawable? ImageCheckedOver { get; set; }
    }
}
