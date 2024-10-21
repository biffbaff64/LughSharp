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

using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

/// <summary>
/// A button with a child <see cref="UI.Image"/> to display an image. This is useful when
/// the button must be larger than the image and the image centered on the button. If
/// the image is the size of the button, a Button without any children can be used,
/// where the <see cref="Button.ButtonStyle.Up"/>, <see cref="Button.ButtonStyle.Down"/>,
/// and <see cref="Button.ButtonStyle.Checked"/> nine patches define the image.
/// </summary>
[PublicAPI]
public class ImageButton : Button
{
    public     Image            Image { get; }
    public new ImageButtonStyle Style { get; private set; } = null!;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new ImageButton using the supplied <see cref="Skin"/>. The
    /// skin should contain an <see cref="ImageButtonStyle"/>.
    /// </summary>
    /// <param name="skin"></param>
    public ImageButton( Skin skin )
        : this( skin.Get< ImageButtonStyle >() )
    {
        Skin = skin;
    }

    /// <summary>
    /// </summary>
    /// <param name="skin"></param>
    /// <param name="styleName"></param>
    public ImageButton( Skin skin, string styleName )
        : this( skin.Get< ImageButtonStyle >( styleName ) )
    {
        Skin = skin;
    }

    /// <summary>
    /// </summary>
    /// <param name="style"></param>
    public ImageButton( ImageButtonStyle style )
        : base( style )
    {
        Image = new Image();
        Image.SetScaling( Scaling.Fit );

        Add( Image );
        SetStyle( style );

        ConstructorHelper();
    }

    /// <summary>
    /// </summary>
    /// <param name="imageUp"></param>
    public ImageButton( IDrawable? imageUp )
        : this( new ImageButtonStyle( null, null, null, imageUp, null, null ) )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="imageUp"></param>
    /// <param name="imageDown"></param>
    public ImageButton( IDrawable? imageUp, IDrawable? imageDown )
        : this( new ImageButtonStyle( null, null, null, imageUp, imageDown, null ) )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="imageUp"></param>
    /// <param name="imageDown"></param>
    /// <param name="imageChecked"></param>
    public ImageButton( IDrawable? imageUp, IDrawable? imageDown, IDrawable? imageChecked )
        : this( new ImageButtonStyle( null, null, null, imageUp, imageDown, imageChecked ) )
    {
    }

    private void ConstructorHelper()
    {
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    /// <summary>
    /// </summary>
    /// <param name="style"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SetStyle( ButtonStyle style )
    {
        Style      = style as ImageButtonStyle ?? throw new ArgumentException( "style must be an ImageButtonStyle." );
        base.Style = style;

        UpdateImage();
    }

    /// <summary>
    /// Returns the appropriate image drawable from the style based on the current button state.
    /// </summary>
    protected IDrawable? GetImageDrawable()
    {
        if ( IsDisabled && ( Style.ImageDisabled != null ) )
        {
            return Style.ImageDisabled;
        }

        if ( IsPressed() )
        {
            if ( IsChecked && ( Style.ImageCheckedDown != null ) )
            {
                return Style.ImageCheckedDown;
            }

            if ( Style.ImageDown != null )
            {
                return Style.ImageDown;
            }
        }

        if ( IsOver() )
        {
            if ( IsChecked )
            {
                if ( Style.ImageCheckedOver != null )
                {
                    return Style.ImageCheckedOver;
                }
            }
            else
            {
                if ( Style.ImageOver != null )
                {
                    return Style.ImageOver;
                }
            }
        }

        if ( IsChecked )
        {
            if ( Style.ImageChecked != null )
            {
                return Style.ImageChecked;
            }

            if ( IsOver() && ( Style.ImageOver != null ) )
            {
                return Style.ImageOver;
            }
        }

        return Style.ImageUp;
    }

    /// <summary>
    /// Sets the image drawable based on the current button state. The default implementation
    /// sets the image drawable using <see cref="GetImageDrawable()"/>.
    /// </summary>
    protected void UpdateImage()
    {
        Image.SetDrawable( GetImageDrawable() );
    }

    /// <summary>
    /// Draws the group and its children. The default implementation calls
    /// <see cref="Group.ApplyTransform(Corelib.LibCore.Graphics.G2D.IBatch,Matrix4)"/> if needed, then
    /// <see cref="Button.DrawChildren(IBatch, float)"/>, followed by
    /// <see cref="Button.ResetTransform(IBatch)"/> if needed. 
    /// </summary>
    /// <param name="batch"> The <see cref="IBatch"/> </param>
    /// <param name="parentAlpha"></param>
    public override void Draw( IBatch batch, float parentAlpha )
    {
        UpdateImage();
        base.Draw( batch, parentAlpha );
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

        return ( className.IndexOf( '$' ) != -1 ? "ImageButton " : "" ) + className + ": " + Image.Drawable;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// The style for an image button.
    /// </summary>
    [PublicAPI]
    public class ImageButtonStyle : ButtonStyle
    {
        public readonly IDrawable? ImageUp;
        public readonly IDrawable? ImageDown;
        public readonly IDrawable? ImageChecked;
        public readonly IDrawable? ImageCheckedDown;
        public readonly IDrawable? ImageCheckedOver;
        public readonly IDrawable? ImageDisabled;
        public readonly IDrawable? ImageOver;

        /// <summary>
        /// Creates a new, unitialised, ImageButtonStyle instance.
        /// </summary>
        public ImageButtonStyle()
        {
        }

        /// <summary>
        /// Creates a new ImageButtonStyle instance, using the supplied <see cref="IDrawable"/>
        /// images for <see cref="ImageUp"/>, <see cref="ImageDown"/> and <see cref="ImageChecked"/>.
        /// </summary>
        public ImageButtonStyle( IDrawable? up,
                                 IDrawable? down,
                                 IDrawable? chcked,
                                 IDrawable? imageUp,
                                 IDrawable? imageDown,
                                 IDrawable? imageChecked )
            : base( up, down, chcked )
        {
            ImageUp      = imageUp;
            ImageDown    = imageDown;
            ImageChecked = imageChecked;
        }

        /// <summary>
        /// Creates a new ImageButtonStyle instance, using the given <see cref="ImageButtonStyle"/> 
        /// </summary>
        public ImageButtonStyle( ImageButtonStyle style )
            : base( style )
        {
            ImageUp       = style.ImageUp;
            ImageDown     = style.ImageDown;
            ImageOver     = style.ImageOver;
            ImageDisabled = style.ImageDisabled;

            ImageChecked     = style.ImageChecked;
            ImageCheckedDown = style.ImageCheckedDown;
            ImageCheckedOver = style.ImageCheckedOver;
        }

        /// <summary>
        /// Creates a new ImageButtonStyle instance, using the supplied
        /// <see cref="ButtonStyle"/>.
        /// </summary>
        public ImageButtonStyle( ButtonStyle style )
            : base( style )
        {
        }
    }
}
