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


namespace LughSharp.LibCore.Scenes.Scene2D.Utils;

/// <summary>
///     Drawable for a <see cref="TextureRegion" />.
/// </summary>
[PublicAPI]
public class TextureRegionDrawable : BaseDrawable, ITransformDrawable
{
    private readonly TextureRegion? _region;

    // ------------------------------------------------------------------------
    
    #region constructors
    
    /// <summary>
    ///     Creates an uninitialized TextureRegionDrawable.
    ///     The texture region must be set before use.
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

    #endregion constructors

    // ------------------------------------------------------------------------
    
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

    /// <inheritdoc/>
    public override void Draw( IBatch batch, float x, float y, float width, float height )
    {
        if ( Region != null )
        {
            batch.Draw( Region, x, y, width, height );
        }
    }

    /// <summary>
    ///     Draws this drawable at the specified bounds.
    /// </summary>
    /// <param name="batch"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="originX"></param>
    /// <param name="originY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="rotation"></param>
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

    /// <summary>
    ///     Creates a new drawable that renders the same as this drawable
    ///     tinted the specified color.
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
            LeftWidth    = LeftWidth,
            RightWidth   = RightWidth,
            TopHeight    = TopHeight,
            BottomHeight = BottomHeight
        };

        return drawable;
    }
}