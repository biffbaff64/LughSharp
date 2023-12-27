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
/// Drawable for a <see cref="NinePatch"/>.
/// <para>
/// The drawable sizes are set when the ninepatch is set, but they are separate
/// values. Eg, <see cref="IDrawable.LeftWidth"/> could be set to more than
/// <see cref="NinePatch.LeftWidth"/> in order to provide more space on the left
/// than actually exists in the ninepatch.
/// </para>
/// The min size is set to the ninepatch total size by default. It could be set
/// to the left+right and top+bottom, excluding the middle size, to allow the
/// drawable to be sized down as small as possible.
/// </summary>
[PublicAPI]
public class NinePatchDrawable : BaseDrawable, ITransformDrawable
{
    public NinePatch? Patch { get; set; }

    /// <summary>
    /// Creates an uninitialized NinePatchDrawable. The ninepatch must be
    /// set before use.
    /// </summary>
    public NinePatchDrawable()
    {
    }

    public NinePatchDrawable( NinePatch patch )
    {
        SetPatch( patch );
    }

    public NinePatchDrawable( NinePatchDrawable drawable )
        : base( drawable )
    {
        this.Patch = drawable.Patch;
    }

    public override void Draw( IBatch batch, float x, float y, float width, float height )
    {
        Patch?.Draw( batch, x, y, width, height );
    }

    public void Draw( IBatch batch, float x, float y, float originX, float originY,
                      float width, float height, float scaleX, float scaleY, float rotation )
    {
    }

    /// <summary>
    /// Sets this drawable's ninepatch and set the min width, min height,
    /// top height, right width, bottom height, and left width to the
    /// patch's padding.
    /// </summary>
    public void SetPatch( NinePatch patch )
    {
        this.Patch = patch;

        if ( this.Patch != null )
        {
            MinWidth     = patch.TotalWidth;
            MinHeight    = patch.TotalHeight;
            TopHeight    = patch.PadTop;
            RightWidth   = patch.PadRight;
            BottomHeight = patch.PadBottom;
            LeftWidth    = patch.PadLeft;
        }
    }

    /// <summary>
    /// Creates a new drawable that renders the same as this
    /// drawable tinted the specified color.
    /// </summary>
    public NinePatchDrawable Tint( Color tint )
    {
        var drawable = new NinePatchDrawable( this );
        
        drawable.Patch = new NinePatch( drawable.Patch!, tint );

        return drawable;
    }
}
