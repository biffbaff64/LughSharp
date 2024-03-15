// ///////////////////////////////////////////////////////////////////////////////
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


using LughSharp.LibCore.Graphics.G2D;
using LughSharp.LibCore.Scenes.Scene2D.Utils;

namespace LughSharp.LibCore.Scenes.Scene2D.UI;

/// <summary>
///     A button with a child <see cref="UI.Image" /> to display an image. This is useful when
///     the button must be larger than the image and the image centered on the button. If
///     the image is the size of the button, a Button without any children can be used,
///     where the <see cref="Button.ButtonStyle.Up" />, <see cref="Button.ButtonStyle.Down" />,
///     and <see cref="Button.ButtonStyle.Checked" /> nine patches define the image.
/// </summary>
public class ImageButton : Button
{
    public ImageButton( Skin skin )
        : this( skin.Get< ImageButtonStyle >() )
    {
        Skin = skin;
    }

    public ImageButton( Skin skin, String styleName )
        : this( skin.Get< ImageButtonStyle >( styleName ) )
    {
        Skin = skin;
    }

    public ImageButton( ImageButtonStyle style )
        : base( style )
    {
        Image = new Image();
        Image.SetScaling( Scaling.Fit );

        Add( Image );
        SetStyle( style );
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    public ImageButton( IDrawable? imageUp )
        : this( new ImageButtonStyle( null, null, null, imageUp, null, null ) )
    {
    }

    public ImageButton( IDrawable? imageUp, IDrawable? imageDown )
        : this( new ImageButtonStyle( null, null, null, imageUp, imageDown, null ) )
    {
    }

    public ImageButton( IDrawable? imageUp, IDrawable? imageDown, IDrawable? imageChecked )
        : this( new ImageButtonStyle( null, null, null, imageUp, imageDown, imageChecked ) )
    {
    }

    public     Image            Image { get; }
    public new ImageButtonStyle Style { get; private set; } = null!;

    public void SetStyle( ButtonStyle style )
    {
        Style      = style as ImageButtonStyle ?? throw new ArgumentException( "style must be an ImageButtonStyle." );
        base.Style = style;

        UpdateImage();
    }

    /// <summary>
    ///     Returns the appropriate image drawable from the style based on the current button state.
    /// </summary>
    protected IDrawable? GetImageDrawable()
    {
        if ( IsDisabled && ( Style.imageDisabled != null ) )
        {
            return Style.imageDisabled;
        }

        if ( IsPressed() )
        {
            if ( IsChecked && ( Style.imageCheckedDown != null ) )
            {
                return Style.imageCheckedDown;
            }

            if ( Style.imageDown != null )
            {
                return Style.imageDown;
            }
        }

        if ( IsOver() )
        {
            if ( IsChecked )
            {
                if ( Style.imageCheckedOver != null )
                {
                    return Style.imageCheckedOver;
                }
            }
            else
            {
                if ( Style.imageOver != null )
                {
                    return Style.imageOver;
                }
            }
        }

        if ( IsChecked )
        {
            if ( Style.imageChecked != null )
            {
                return Style.imageChecked;
            }

            if ( IsOver() && ( Style.imageOver != null ) )
            {
                return Style.imageOver;
            }
        }

        return Style.imageUp;
    }

    /// <summary>
    ///     Sets the image drawable based on the current button state. The default implementation
    ///     sets the image drawable using <see cref="GetImageDrawable()" />.
    /// </summary>
    protected void UpdateImage()
    {
        Image.SetDrawable( GetImageDrawable() );
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        UpdateImage();
        base.Draw( batch, parentAlpha );
    }

    public Cell? GetImageCell()
    {
        return GetCell( Image );
    }

    protected override string ToString()
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

        return ( className.IndexOf( '$' ) != -1 ? "ImageButton " : "" ) + className + ": " + Image.GetDrawable();
    }

    /// <summary>
    ///     The style for an image button.
    /// </summary>
    public class ImageButtonStyle : ButtonStyle
    {
        public readonly IDrawable? imageChecked, imageCheckedDown, imageCheckedOver;
        public readonly IDrawable? imageUp,      imageDown,        imageOver, imageDisabled;

        public ImageButtonStyle()
        {
        }

        public ImageButtonStyle( IDrawable? up,
                                 IDrawable? down,
                                 IDrawable? chcked,
                                 IDrawable? imageUp,
                                 IDrawable? imageDown,
                                 IDrawable? imageChecked )
            : base( up, down, chcked )
        {
            this.imageUp      = imageUp;
            this.imageDown    = imageDown;
            this.imageChecked = imageChecked;
        }

        public ImageButtonStyle( ImageButtonStyle style )
            : base( style )
        {
            imageUp       = style.imageUp;
            imageDown     = style.imageDown;
            imageOver     = style.imageOver;
            imageDisabled = style.imageDisabled;

            imageChecked     = style.imageChecked;
            imageCheckedDown = style.imageCheckedDown;
            imageCheckedOver = style.imageCheckedOver;
        }

        public ImageButtonStyle( ButtonStyle style )
            : base( style )
        {
        }
    }
}
