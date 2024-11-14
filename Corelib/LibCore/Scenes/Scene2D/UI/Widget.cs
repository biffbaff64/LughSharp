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

using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Scenes.Scene2D.Utils;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

/// <summary>
/// An <see cref="Actor"/> that participates in layout and provides a minimum,
/// preferred, and maximum size.
/// <para>
/// The default preferred size of a widget is 0 and this is almost always overridden
/// by a subclass. The default minimum size returns the preferred size, so a subclass
/// may choose to return 0 if it wants to allow itself to be sized smaller. The default
/// maximum size is 0, which means no maximum size.
/// </para>
/// See <see cref="ILayout"/> for details on how a widget should participate in layout.
/// A widget's mutator methods should call <see cref="Invalidate"/> or
/// <see cref="InvalidateHierarchy"/> as needed.
/// </summary>
[PublicAPI]
public class Widget : Actor, ILayout
{
    /// <summary>
    /// </summary>
    public bool NeedsLayout { get; set; } = true;

    private bool _layoutEnabled = true;

    // ========================================================================
    // ========================================================================
    
    /// <summary>
    /// Computes and caches any information needed for drawing and, if this actor
    /// has children, positions and sizes each child, calls <see cref="ILayout.Invalidate"/>
    /// on any each child whose width or height has changed, and calls <see cref="ILayout.Validate"/>
    /// on each child. This method should almost never be called directly, instead
    /// <see cref="ILayout.Validate"/> should be used.
    /// </summary>
    public virtual void SetLayout()
    {
    }

    /// <summary>
    /// Ensures the actor has been laid out.
    /// <para>
    /// Calls <see cref="ILayout.SetLayout"/> if <see cref="ILayout.Invalidate"/> has
    /// been called since the last time <see cref="ILayout.Validate"/> was called, or
    /// if the actor otherwise needs to be laid out. This method is usually called in
    /// <see cref="Actor.Draw(IBatch, float)"/> by the actor itself before drawing is
    /// performed.
    /// </para>
    /// </summary>
    public virtual void Validate()
    {
        if ( !EnableLayout )
        {
            return;
        }

        var parent = Parent;

        if ( FillParent && ( parent != null ) )
        {
            float parentWidth, parentHeight;

            if ( ( Stage != null ) && ( parent == Stage.Root ) )
            {
                parentWidth  = Stage.Width;
                parentHeight = Stage.Height;
            }
            else
            {
                parentWidth  = parent.Width;
                parentHeight = parent.Height;
            }

            SetSize( parentWidth, parentHeight );
        }

        if ( !NeedsLayout )
        {
            return;
        }

        NeedsLayout = false;

        SetLayout();
    }

    /// <summary>
    /// Invalidates this actor's layout, causing <see cref="ILayout.SetLayout"/> to happen the
    /// next time <see cref="ILayout.Validate"/> is called. This method should be called when
    /// state changes in the actor that requires a layout but does not change the minimum,
    /// preferred, maximum, or actual size of the actor (meaning it does not affect the
    /// parent actor's layout).
    /// </summary>
    public virtual void Invalidate()
    {
        NeedsLayout = true;
    }

    /// <summary>
    /// Invalidates this actor and its ascendants, calling <see cref="ILayout.Invalidate"/> on each.
    /// This method should be called when state changes in the actor that affects the minimum,
    /// preferred, maximum, or actual size of the actor (meaning it potentially affects the
    /// parent actor's layout).
    /// </summary>
    public virtual void InvalidateHierarchy()
    {
        if ( !EnableLayout )
        {
            return;
        }

        Invalidate();

        if ( Parent is ILayout layout )
        {
            layout.InvalidateHierarchy();
        }
    }

    /// <summary>
    /// Sizes this actor to its preferred width and height, then calls <see cref="ILayout.Validate"/>.
    /// <para>
    /// Generally this method should not be called in an actor's constructor because it calls
    /// <see cref="ILayout.SetLayout"/>, which means a subclass would have Layout() called before the
    /// subclass' constructor. Instead, in constructors simply set the actor's size
    /// to <see cref="ILayout.PrefWidth"/> and <see cref="ILayout.PrefHeight"/>. This allows
    /// the actor to have a size at construction time for more convenient use with groups that do
    /// not layout their children.
    /// </para>
    /// </summary>
    public virtual void Pack()
    {
        SetSize( PrefWidth, PrefHeight );
        Validate();
    }

    /// <summary>
    /// If true, this actor will be sized to the parent in <see cref="ILayout.Validate"/>. If the
    /// parent is the stage, the actor will be sized to the stage. This method is for convenience
    /// only when the widget's parent does not set the size of its children (such as the stage).
    /// </summary>
    public bool FillParent { get; set; }

    /// <summary>
    /// Enables or disables the layout for this actor and all child actors, recursively.
    /// When false, <see cref="ILayout.Validate"/> will not cause a layout to occur. This can be useful
    /// when an actor will be manipulated externally, such as with actions. Default is true.
    /// </summary>
    public bool EnableLayout
    {
        get => _layoutEnabled;
        set
        {
            _layoutEnabled = value;

            if ( _layoutEnabled )
            {
                InvalidateHierarchy();
            }
        }
    }

    /// <inheritdoc />
    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();
    }

    /// <inheritdoc />
    public override void SizeChanged()
    {
        Invalidate();
    }
}
