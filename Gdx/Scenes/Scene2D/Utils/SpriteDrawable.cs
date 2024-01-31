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


using LibGDXSharp.Gdx.Graphics;
using LibGDXSharp.Gdx.Graphics.G2D;

namespace LibGDXSharp.Gdx.Scenes.Scene2D.Utils;

/// <summary>
///     Drawable for a <see cref="Sprite" />.
/// </summary>
public class SpriteDrawable : BaseDrawable, ITransformDrawable
{
    private Sprite? _sprite;

    /// <summary>
    ///     Creates an uninitialized SpriteDrawable. The sprite must be set before use.
    /// </summary>
    public SpriteDrawable()
    {
    }

    public SpriteDrawable( Sprite sprite ) => Sprite = sprite;

    public SpriteDrawable( SpriteDrawable? drawable ) : base( drawable ) => Sprite = drawable?.Sprite;

    public Sprite? Sprite
    {
        get => _sprite;
        set
        {
            _sprite = value;

            MinWidth  = value?.Width ?? 0;
            MinHeight = value?.Height ?? 0;
        }
    }

    public override void Draw( IBatch batch, float x, float y, float width, float height )
    {
        if ( Sprite == null )
        {
            return;
        }

        Color spriteColor = Sprite.Color;
        var   oldColor    = spriteColor.ToFloatBits();

        Sprite.SetColor( spriteColor.Mul( batch.Color ) );

        Sprite.Rotation = 0;
        Sprite.SetScale( 1, 1 );
        Sprite.SetBounds( x, y, width, height );
        Sprite.Draw( batch );

        Sprite.PackedColor = oldColor;
    }

    public void Draw( IBatch batch,
                      float x,
                      float y,
                      float originX,
                      float originY,
                      float width,
                      float height,
                      float scaleX,
                      float scaleY,
                      float rotation )
    {
        if ( Sprite == null )
        {
            return;
        }

        Color spriteColor = Sprite.Color;
        var   oldColor    = spriteColor.ToFloatBits();

        Sprite.SetColor( spriteColor.Mul( batch.Color ) );

        Sprite.SetOrigin( originX, originY );
        Sprite.Rotation = rotation;
        Sprite.SetScale( scaleX, scaleY );
        Sprite.SetBounds( x, y, width, height );
        Sprite.Draw( batch );
        Sprite.PackedColor = oldColor;
    }

    /// <summary>
    ///     Creates a new drawable that renders the same as this
    ///     drawable tinted the specified color.
    /// </summary>
    public SpriteDrawable Tint( Color tint )
    {
        Sprite newSprite;

        if ( _sprite is AtlasSprite sprite )
        {
            newSprite = new AtlasSprite( sprite );
        }
        else
        {
            newSprite = new Sprite( _sprite! );
        }

        newSprite.SetColor( tint );
        newSprite.SetSize( MinWidth, MinHeight );

        var drawable = new SpriteDrawable( newSprite )
        {
            LeftWidth    = LeftWidth,
            RightWidth   = RightWidth,
            TopHeight    = TopHeight,
            BottomHeight = BottomHeight
        };

        return drawable;
    }
}
