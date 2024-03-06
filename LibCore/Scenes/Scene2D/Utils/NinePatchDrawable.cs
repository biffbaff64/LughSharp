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


using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Graphics.G2D;

namespace LibGDXSharp.LibCore.Scenes.Scene2D.Utils;

/// <summary>
///     Drawable for a <see cref="NinePatch" />.
///     <para>
///         The drawable sizes are set when the ninepatch is set, but they are separate
///         values. Eg, <see cref="IDrawable.LeftWidth" /> could be set to more than
///         <see cref="NinePatch.LeftWidth" /> in order to provide more space on the left
///         than actually exists in the ninepatch.
///     </para>
///     The min size is set to the ninepatch total size by default. It could be set
///     to the left+right and top+bottom, excluding the middle size, to allow the
///     drawable to be sized down as small as possible.
/// </summary>
public class NinePatchDrawable : BaseDrawable, ITransformDrawable
{
    /// <summary>
    ///     Creates an uninitialized NinePatchDrawable. The ninepatch must be
    ///     set before use.
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
        Patch = drawable.Patch;
    }

    public NinePatch? Patch { get; set; }

    public override void Draw( IBatch batch, float x, float y, float width, float height )
    {
        Patch?.Draw( batch, x, y, width, height );
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
    }

    /// <summary>
    ///     Sets this drawable's ninepatch and set the min width, min height,
    ///     top height, right width, bottom height, and left width to the
    ///     patch's padding.
    /// </summary>
    public void SetPatch( NinePatch patch )
    {
        Patch = patch;

        if ( Patch != null )
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
    ///     Creates a new drawable that renders the same as this
    ///     drawable tinted the specified color.
    /// </summary>
    public NinePatchDrawable Tint( Color tint )
    {
        var drawable = new NinePatchDrawable( this );

        drawable.Patch = new NinePatch( drawable.Patch!, tint );

        return drawable;
    }
}
