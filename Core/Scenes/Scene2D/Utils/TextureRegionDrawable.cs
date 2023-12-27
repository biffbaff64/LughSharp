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

using LibGDXSharp.Graphics.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// Drawable for a <see cref="TextureRegion"/>.
/// </summary>
[PublicAPI]
public class TextureRegionDrawable : BaseDrawable, ITransformDrawable
{
    private readonly TextureRegion? _region;

    /// <summary>
    /// Creates an uninitialized TextureRegionDrawable.
    /// The texture region must be set before use.
    /// </summary>
    public TextureRegionDrawable()
    {
    }

    public TextureRegionDrawable( Texture texture )
    {
        Region = new TextureRegion( texture );
    }

    public TextureRegionDrawable( TextureRegion region )
    {
        Region = region;
    }

    public TextureRegionDrawable( TextureRegionDrawable drawable ) : base( drawable )
    {
        Region = drawable.Region;
    }

    public virtual new void Draw( IBatch batch, float x, float y, float width, float height )
    {
        if ( Region != null )
        {
            batch.Draw( Region, x, y, width, height );
        }
    }

    public virtual void Draw( IBatch batch,
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
        if ( Region != null )
        {
            batch.Draw( Region, x, y, originX, originY, width, height, scaleX, scaleY, rotation );
        }
    }

    protected TextureRegion? Region
    {
        get => _region;
        private init
        {
            this._region = value;

            if ( _region != null )
            {
                MinWidth  = _region.RegionWidth;
                MinHeight = _region.RegionHeight;
            }
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
        
        Sprite sprite;

        if ( Region is AtlasRegion region )
        {
            sprite = new AtlasSprite( region );
        }
        else
        {
            sprite = new Sprite( Region! );
        }

        sprite.SetColor( tint );
        sprite.SetSize( MinWidth, MinHeight );
        
        var drawable = new SpriteDrawable( sprite )
        {
            LeftWidth = LeftWidth,
            RightWidth = RightWidth,
            TopHeight = TopHeight,
            BottomHeight = BottomHeight
        };

        return drawable;
    }
}