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

using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Maths;
using Color = Corelib.Lugh.Graphics.Color;

namespace Corelib.Lugh.Scenes.Scene2D.Utils;

/// <summary>
/// Drawable for a <see cref="TextureRegion"/>.
/// </summary>
[PublicAPI]
public class TextureRegionDrawable : BaseDrawable, ITransformDrawable
{
    private readonly TextureRegion? _region;

    // ========================================================================

    /// <summary>
    /// Creates an uninitialized TextureRegionDrawable.
    /// The texture region must be set before use.
    /// </summary>
    public TextureRegionDrawable()
    {
    }

    /// <summary>
    /// Creates a new TextureRegionDrawable, initialised with a new <see cref="TextureRegion"/>
    /// from the supplied <see cref="Texture"/>
    /// </summary>
    public TextureRegionDrawable( Texture texture )
        : this( new TextureRegion( texture ) )
    {
    }

    /// <summary>
    /// Creates a new TextureRegionDrawable, initialised with the supplied <see cref="TextureRegion"/>
    /// </summary>
    public TextureRegionDrawable( TextureRegion region )
    {
        Region = region;
    }

    /// <summary>
    /// Creates a new TextureRegionDrawable using the <see cref="TextureRegion"/> from
    /// the given TextureRegionDrawable.
    /// </summary>
    /// <param name="drawable"></param>
    public TextureRegionDrawable( TextureRegionDrawable drawable ) : base( drawable )
    {
        Region = drawable.Region;
    }

    /// <summary>
    /// The <see cref="TextureRegion"/> component of this <see cref="IDrawable"/>
    /// </summary>
    protected TextureRegion? Region
    {
        get => _region;
        private init
        {
            _region = value;

            if ( _region != null )
            {
                MinWidth  = _region.RegionWidth;
                MinHeight = _region.RegionHeight;
            }
        }
    }

    /// <inheritdoc />
    public override void Draw( IBatch batch, float x, float y, float width, float height )
    {
        if ( Region != null )
        {
            batch.Draw( Region, x, y, width, height );
        }
    }

    /// <summary>
    /// Draws this drawable at the specified bounds.
    /// </summary>
    public virtual void Draw( IBatch batch,
                              GRect region,
                              Point2D origin,
                              Point2D scale,
                              float rotation )
    {
        if ( Region != null )
        {
            batch.Draw( Region, region, origin, scale, rotation );
        }
    }

    /// <summary>
    /// Creates a new drawable that renders the same as this drawable
    /// tinted the specified color.
    /// </summary>
    public virtual IDrawable Tint( Color tint )
    {
        if ( Region == null )
        {
            throw new NullReferenceException();
        }

        var sprite = Region is AtlasRegion region
                         ? new AtlasSprite( region )
                         : new Sprite( Region! );

        sprite.SetColor( tint );
        sprite.SetSize( MinWidth, MinHeight );

        var drawable = new SpriteDrawable( sprite )
        {
            LeftWidth    = LeftWidth,
            RightWidth   = RightWidth,
            TopHeight    = TopHeight,
            BottomHeight = BottomHeight,
        };

        return drawable;
    }
}
