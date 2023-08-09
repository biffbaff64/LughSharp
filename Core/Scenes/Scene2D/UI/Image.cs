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

/// <summary>
/// Displays a <see cref="IDrawable"/>, scaled various way within the widgets
/// bounds. The preferred size is the min size of the drawable. Only when using
/// a <see cref="TextureRegionDrawable"/> will the actor's scale, rotation, and
/// origin be used when drawing.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class Image : Widget
{
    public float ImageX      { get; set; }
    public float ImageY      { get; set; }
    public float ImageWidth  { get; set; }
    public float ImageHeight { get; set; }

    private Scaling    _scaling;
    private int        _alignment;      // Backing value for property Alignment
    private IDrawable? _drawable;

    public Image() : this( null as IDrawable )
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
        ArgumentNullException.ThrowIfNull( drawable );

        SetDrawable( drawable );

        this._scaling  = scaling;
        this.Alignment = align;

        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    public new void Layout()
    {
        if ( _drawable == null ) return;

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
                drawable.Draw
                    (
                    batch,
                    x + ImageX, y + ImageY,
                    OriginX - ImageX, OriginY - ImageY,
                    ImageWidth, ImageHeight,
                    scaleX, scaleY,
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
    /// Sets a new drawable for the image. The image's pref size is the drawable's min
    /// size. If using the image actor's size rather than the pref size, <see cref="Widget.Pack"/>
    /// can be used to size the image to its pref size.
    /// </summary>
    /// <param name="drawable"> May be null. </param>
    public void SetDrawable( IDrawable? drawable )
    {
        if ( this._drawable == drawable ) return;

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

        this._drawable = drawable;
    }

    public IDrawable? GetDrawable()
    {
        return _drawable;
    }

    public void SetScaling( Scaling scale )
    {
        ArgumentNullException.ThrowIfNull( scale );

        this._scaling = scale;
        Invalidate();
    }

    public int Alignment
    {
        get => _alignment;
        set
        {
            this._alignment = value;
            Invalidate();
        }
    }

    public float GetMinWidth() => 0;

    public float GetMinHeight() => 0;

    public float GetPrefWidth()
    {
        if ( _drawable != null ) return _drawable.MinWidth;

        return 0;
    }

    public float GetPrefHeight()
    {
        if ( _drawable != null ) return _drawable.MinHeight;

        return 0;
    }

    public new string ToString()
    {
        var name = Name;

        if ( name != null ) return name;

        var className                   = GetType().Name;
        var dotIndex                    = className.LastIndexOf( '.' );
        if ( dotIndex != -1 ) className = className.Substring( dotIndex + 1 );

        return ( className.IndexOf( '$' ) != -1 ? "Image " : "" ) + className + ": " + _drawable;
    }
}