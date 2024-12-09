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
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Scenes.Scene2D.Utils;
using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// Displays a <see cref="IDrawable"/>, scaled various way within the widgets
/// bounds. The preferred size is the min size of the drawable. Only when using
/// a <see cref="TextureRegionDrawable"/> will the actor's scale, rotation, and
/// origin be used when drawing.
/// </summary>
[PublicAPI]
public class Image : Widget
{
    public float      ImageX      { get; set; }
    public float      ImageY      { get; set; }
    public float      ImageWidth  { get; set; }
    public float      ImageHeight { get; set; }
    public IDrawable? Drawable    { get; private set; }

    // ========================================================================

    private int     _alignment; // Backing value for Alignment property
    private Scaling _scaling;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new, unitialised, Image instance.
    /// </summary>
    public Image() : this( ( IDrawable ) null! )
    {
    }

    public Image( NinePatch patch )
        : this( new NinePatchDrawable( patch ), Scaling.Stretch )
    {
    }

    public Image( TextureRegion region )
        : this( new TextureRegionDrawable( region ), Scaling.Stretch )
    {
    }

    public Image( Texture texture )
        : this( new TextureRegionDrawable( new TextureRegion( texture ) ) )
    {
    }

    public Image( Skin skin, string drawableName )
        : this( skin.GetDrawable( drawableName ), Scaling.Stretch )
    {
    }

    public Image( IDrawable? drawable )
        : this( drawable, Scaling.Stretch )
    {
    }

    public Image( IDrawable? drawable, Scaling scaling, int align = Align.CENTER )
    {
        SetDrawable( drawable );

        _scaling  = scaling;
        Alignment = align;

        NonVirtualConstructorHelper();
    }

    /// <summary>
    /// Helper method for constructors, allowing access to virtual members which are
    /// unsafe to be referenced from constructors.
    /// </summary>
    private void NonVirtualConstructorHelper()
    {
        SetSize( PrefWidth, PrefHeight );
    }

    public int Alignment
    {
        get => _alignment;
        set
        {
            _alignment = value;
            Invalidate();
        }
    }

    /// <inheritdoc />
    public override void SetLayout()
    {
        if ( Drawable == null )
        {
            return;
        }

        var regionWidth  = Drawable.MinWidth;
        var regionHeight = Drawable.MinHeight;
        var width        = Width;
        var height       = Height;

        var size = _scaling.Apply( regionWidth, regionHeight, width, height );

        ImageWidth  = size.X;
        ImageHeight = size.Y;

        if ( ( Alignment & Align.LEFT ) != 0 )
        {
            ImageX = 0;
        }
        else if ( ( Alignment & Align.RIGHT ) != 0 )
        {
            ImageX = ( int ) ( width - ImageWidth );
        }
        else
        {
            ImageX = ( int ) ( ( width / 2 ) - ( ImageWidth / 2 ) );
        }

        if ( ( Alignment & Align.TOP ) != 0 )
        {
            ImageY = ( int ) ( height - ImageHeight );
        }
        else if ( ( Alignment & Align.BOTTOM ) != 0 )
        {
            ImageY = 0;
        }
        else
        {
            ImageY = ( int ) ( ( height / 2 ) - ( ImageHeight / 2 ) );
        }
    }

    /// <inheritdoc />
    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        batch.SetColor( Color.R, Color.G, Color.B, Color.A * parentAlpha );

        var x      = X;
        var y      = Y;
        var scaleX = ScaleX;
        var scaleY = ScaleY;

        if ( Drawable is ITransformDrawable drawable )
        {
            var rotation = Rotation;

            if ( scaleX is not 1 || scaleY is not 1 || rotation is not 0 )
            {
                var region = new GRect
                {
                    X      = ( int ) ( x + ImageX ),
                    Y      = ( int ) ( y + ImageY ),
                    Width  = ( int ) ImageWidth,
                    Height = ( int ) ImageHeight,
                };

                var origin = new Point2D
                {
                    X = ( int ) ( OriginX - ImageX ),
                    Y = ( int ) ( OriginY - ImageY ),
                };

                var scale = new Point2D
                {
                    X = ( int ) scaleX,
                    Y = ( int ) scaleY,
                };

                drawable.Draw( batch, region, origin, scale, rotation );

                return;
            }
        }

        Drawable?.Draw( batch, x + ImageX, y + ImageY, ImageWidth * scaleX, ImageHeight * scaleY );
    }

    /// <summary>
    /// Sets the <see cref="IDrawable"/> for this Image.
    /// </summary>
    /// <param name="skin"></param>
    /// <param name="drawableName"></param>
    public void SetDrawable( Skin skin, string drawableName )
    {
        SetDrawable( skin.GetDrawable( drawableName ) );
    }

    /// <summary>
    /// Sets a new drawable for the image. The image's pref size is the drawable's min size.
    /// If using the image actor's size rather than the pref size, <see cref="Widget.Pack"/>
    /// can be used to size the image to its pref size.
    /// </summary>
    /// <param name="drawable"> May be null. </param>
    public void SetDrawable( IDrawable? drawable )
    {
        if ( Drawable == drawable )
        {
            return;
        }

        if ( drawable != null )
        {
            if ( !PrefWidth.Equals( drawable.MinWidth ) || !PrefHeight.Equals( drawable.MinHeight ) )
            {
                InvalidateHierarchy();
            }
        }
        else
        {
            InvalidateHierarchy();
        }

        Drawable = drawable;
    }

    /// <summary>
    /// Sets the <see cref="Scaling"/>Mode for this Image.
    /// </summary>
    public void SetScaling( Scaling scale )
    {
        ArgumentNullException.ThrowIfNull( scale );

        _scaling = scale;
        Invalidate();
    }

    /// <inheritdoc />
    public override float PrefWidth
    {
        get
        {
            if ( Drawable != null )
            {
                return Drawable.MinWidth;
            }

            return 0;
        }
        set { }
    }

    public override float PrefHeight
    {
        get
        {
            if ( Drawable != null )
            {
                return Drawable.MinWidth;
            }

            return 0;
        }
        set { }
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

        return ( className.IndexOf( '$' ) != -1 ? "Image " : "" ) + className + ": " + Drawable;
    }
}
