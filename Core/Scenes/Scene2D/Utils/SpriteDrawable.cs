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

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// Drawable for a <see cref="Sprite"/>.
/// </summary>
[PublicAPI]
public class SpriteDrawable : BaseDrawable, ITransformDrawable
{
    private Sprite? _sprite;

    /// <summary>
    /// Creates an uninitialized SpriteDrawable. The sprite must be set before use.
    /// </summary>
    public SpriteDrawable()
    {
    }

    public SpriteDrawable( Sprite sprite )
    {
        Sprite = sprite;
    }

    public SpriteDrawable( SpriteDrawable? drawable ) : base( drawable )
    {
        Sprite = drawable?.Sprite;
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

    public Sprite? Sprite
    {
        get => _sprite;
        set
        {
            this._sprite = value;

            MinWidth  = value?.Width ?? 0;
            MinHeight = value?.Height ?? 0;
        }
    }
    
    /// <summary>
    /// Creates a new drawable that renders the same as this
    /// drawable tinted the specified color.
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