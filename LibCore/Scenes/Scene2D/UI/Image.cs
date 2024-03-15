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


using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.G2D;
using LughSharp.LibCore.Scenes.Scene2D.Utils;

namespace LughSharp.LibCore.Scenes.Scene2D.UI;

/// <summary>
///     Displays a <see cref="IDrawable" />, scaled various way within the widgets
///     bounds. The preferred size is the min size of the drawable. Only when using
///     a <see cref="TextureRegionDrawable" /> will the actor's scale, rotation, and
///     origin be used when drawing.
/// </summary>
public class Image : Widget
{
    private int        _alignment; // Backing value for property Alignment
    private IDrawable? _drawable;

    private Scaling _scaling;

    public Image() : this( ( IDrawable )null! )
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

    public Image( IDrawable? drawable ) : this( drawable, Scaling.Stretch )
    {
    }

    public Image( IDrawable? drawable, Scaling scaling, int align = Align.CENTER )
    {
        SetDrawable( drawable );

        _scaling  = scaling;
        Alignment = align;

        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    public float ImageX      { get; set; }
    public float ImageY      { get; set; }
    public float ImageWidth  { get; set; }
    public float ImageHeight { get; set; }

    public int Alignment
    {
        get => _alignment;
        set
        {
            _alignment = value;
            Invalidate();
        }
    }

    public override void SetLayout()
    {
        if ( _drawable == null )
        {
            return;
        }

        var regionWidth  = _drawable.MinWidth;
        var regionHeight = _drawable.MinHeight;
        var width        = Width;
        var height       = Height;

        Vector2 size = _scaling.Apply( regionWidth, regionHeight, width, height );

        ImageWidth  = size.X;
        ImageHeight = size.Y;

        if ( ( Alignment & Align.LEFT ) != 0 )
        {
            ImageX = 0;
        }
        else if ( ( Alignment & Align.RIGHT ) != 0 )
        {
            ImageX = ( int )( width - ImageWidth );
        }
        else
        {
            ImageX = ( int )( ( width / 2 ) - ( ImageWidth / 2 ) );
        }

        if ( ( Alignment & Align.TOP ) != 0 )
        {
            ImageY = ( int )( height - ImageHeight );
        }
        else if ( ( Alignment & Align.BOTTOM ) != 0 )
        {
            ImageY = 0;
        }
        else
        {
            ImageY = ( int )( ( height / 2 ) - ( ImageHeight / 2 ) );
        }
    }

    public new void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        Color color = Color ?? Color.Black;

        batch.SetColor( color.R, color.G, color.B, color.A * parentAlpha );

        var x      = X;
        var y      = Y;
        var scaleX = ScaleX;
        var scaleY = ScaleY;

        if ( _drawable is ITransformDrawable drawable )
        {
            var rotation = Rotation;

            if ( scaleX is not 1 || scaleY is not 1 || rotation is not 0 )
            {
                drawable.Draw(
                    batch,
                    x + ImageX,
                    y + ImageY,
                    OriginX - ImageX,
                    OriginY - ImageY,
                    ImageWidth,
                    ImageHeight,
                    scaleX,
                    scaleY,
                    rotation
                    );

                return;
            }
        }

        if ( _drawable != null )
        {
            _drawable.Draw( batch, x + ImageX, y + ImageY, ImageWidth * scaleX, ImageHeight * scaleY );
        }
    }

    public void SetDrawable( Skin skin, string drawableName )
    {
        SetDrawable( skin.GetDrawable( drawableName ) );
    }

    /// <summary>
    ///     Sets a new drawable for the image. The image's pref size is the
    ///     drawable's min size. If using the image actor's size rather than
    ///     the pref size, <see cref="Widget.Pack" /> can be used to size the
    ///     image to its pref size.
    /// </summary>
    /// <param name="drawable"> May be null. </param>
    public void SetDrawable( IDrawable? drawable )
    {
        if ( _drawable == drawable )
        {
            return;
        }

        if ( drawable != null )
        {
            if ( !GetPrefWidth().Equals( drawable.MinWidth ) || !GetPrefHeight().Equals( drawable.MinHeight ) )
            {
                InvalidateHierarchy();
            }
        }
        else
        {
            InvalidateHierarchy();
        }

        _drawable = drawable;
    }

    public IDrawable? GetDrawable()
    {
        return _drawable;
    }

    public void SetScaling( Scaling scale )
    {
        ArgumentNullException.ThrowIfNull( scale );

        _scaling = scale;
        Invalidate();
    }

    public float GetMinWidth()
    {
        return 0;
    }

    public float GetMinHeight()
    {
        return 0;
    }

    public float GetPrefWidth()
    {
        if ( _drawable != null )
        {
            return _drawable.MinWidth;
        }

        return 0;
    }

    public float GetPrefHeight()
    {
        if ( _drawable != null )
        {
            return _drawable.MinHeight;
        }

        return 0;
    }

    protected override string ToString()
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

        return ( className.IndexOf( '$' ) != -1 ? "Image " : "" ) + className + ": " + _drawable;
    }
}
