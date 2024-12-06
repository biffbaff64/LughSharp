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

using Corelib.Lugh.Graphics.G2D;

namespace Corelib.Lugh.Scenes.Scene2D.Utils;

/// <summary>
/// Drawable that stores the size information but doesn't draw anything.
/// </summary>
[PublicAPI]
public class BaseDrawable : IDrawable
{
    public string? Name         { get; set; }
    public float   LeftWidth    { get; set; }
    public float   RightWidth   { get; set; }
    public float   TopHeight    { get; set; }
    public float   BottomHeight { get; set; }
    public float   MinWidth     { get; set; }
    public float   MinHeight    { get; set; }

    // ========================================================================

    /// <summary>
    /// Creates a new, empty, BaseDrawable object.
    /// </summary>
    protected BaseDrawable()
    {
    }

    /// <summary>
    /// Creates a new empty drawable with the same sizing information as the specified drawable.
    /// </summary>
    protected BaseDrawable( IDrawable? drawable )
    {
        ArgumentNullException.ThrowIfNull( drawable );

        if ( drawable is BaseDrawable baseDrawable )
        {
            Name = baseDrawable.Name;
        }

        LeftWidth    = drawable.LeftWidth;
        RightWidth   = drawable.RightWidth;
        TopHeight    = drawable.TopHeight;
        BottomHeight = drawable.BottomHeight;
        MinWidth     = drawable.MinWidth;
        MinHeight    = drawable.MinHeight;
    }

    /// <summary>
    /// Draws this drawable at the specified bounds. The drawable should be tinted
    /// with <see cref="IBatch.Color"/>, possibly by mixing its own color.
    /// The default implementation does nothing.
    /// </summary>
    public virtual void Draw( IBatch batch, float x, float y, float width, float height )
    {
    }

    /// <summary>
    /// Sets the padding amounts for this drawable.
    /// </summary>
    public virtual void SetPadding( float topHeight, float leftWidth, float bottomHeight, float rightWidth )
    {
        TopHeight    = topHeight;
        LeftWidth    = leftWidth;
        BottomHeight = bottomHeight;
        RightWidth   = rightWidth;
    }

    /// <summary>
    /// Sets the minimum width and height for this drawable.
    /// </summary>
    public virtual void SetMinSize( float minWidth, float minHeight )
    {
        MinWidth  = minWidth;
        MinHeight = minHeight;
    }
}
