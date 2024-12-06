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
using Corelib.Lugh.Scenes.Scene2D.Utils;
using Corelib.Lugh.Utils.Collections;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// A <see cref="Group"/> that participates in layout and provides a minimum,
/// preferred, and maximum size.
/// <para>
/// The default preferred size of a widget group is 0 and this is almost always
/// overridden by a subclass. The default minimum size returns the preferred size,
/// so a subclass may choose to return 0 for minimum size if it wants to allow
/// itself to be sized smaller than the preferred size. The default maximum size
/// is 0, which means no maximum size.
/// </para>
/// See <see cref="ILayout"/> for details on how a widget group should participate
/// in layout. A widget group's mutator methods should call <see cref="Invalidate()"/>
/// or <see cref="InvalidateHierarchy()"/> as needed. By default, InvalidateHierarchy
/// is called when child widgets are added and removed.
/// </summary>
[PublicAPI]
public class WidgetGroup : Group, ILayout
{
    // ========================================================================

    protected WidgetGroup()
    {
    }

    /// <summary>
    /// Creates a new widget group containing the specified actors.
    /// </summary>
    public WidgetGroup( params Actor[] actors )
    {
        LoadActors( actors );
    }

    // Returns true if the widget's layout has been invalidated.
    public bool NeedsLayout { get; private set; } = true;

    public bool EnableLayout { get; set; } = true;

    public bool FillParent { get; set; }

    public virtual void Pack()
    {
        SetSize( PrefWidth, PrefHeight );

        Validate();

        // Validating the layout may change the pref size. Eg, a wrapped label doesn't
        // know its pref height until it knows its width, so it calls invalidateHierarchy()
        // in layout() if its pref height has changed.
        SetSize( PrefWidth, PrefHeight );

        Validate();
    }

    public virtual void SetLayout()
    {
    }

    /// <inheritdoc />
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
            var   stage = Stage;

            if ( ( stage != null ) && ( parent == stage.Root ) )
            {
                parentWidth  = stage.Width;
                parentHeight = stage.Height;
            }
            else
            {
                parentWidth  = parent.Width;
                parentHeight = parent.Height;
            }

            if ( !Width.Equals( parentWidth ) || !Height.Equals( parentHeight ) )
            {
                Width  = parentWidth;
                Height = parentHeight;
                Invalidate();
            }
        }

        if ( !NeedsLayout )
        {
            return;
        }

        NeedsLayout = false;

        SetLayout();

        // Widgets may call invalidateHierarchy during layout (eg, a wrapped label).
        // The root-most widget group retries layout a reasonable number of times.
        if ( NeedsLayout )
        {
            if ( parent is WidgetGroup )
            {
                return; // The parent widget will layout again.
            }

            for ( var i = 0; i < 5; i++ )
            {
                NeedsLayout = false;

                SetLayout();

                if ( !NeedsLayout )
                {
                    break;
                }
            }
        }
    }

    public virtual void Invalidate()
    {
        NeedsLayout = true;
    }

    public virtual void InvalidateHierarchy()
    {
        Invalidate();

        if ( Parent is ILayout layout )
        {
            layout.InvalidateHierarchy();
        }
    }

    protected void LoadActors( params Actor[] actors )
    {
        foreach ( var actor in actors )
        {
            AddActor( actor );
        }
    }

    public void SetLayoutEnabled( bool enabled )
    {
        EnableLayout = enabled;
        SetLayoutEnabled( this, enabled );
    }

    private static void SetLayoutEnabled( Group parent, bool enabled )
    {
        SnapshotArray< Actor > children = parent.Children;

        for ( int i = 0, n = children.Size; i < n; i++ )
        {
            var actor = children.GetAt( i );

            if ( actor is ILayout layout )
            {
                layout.EnableLayout = enabled;
            }
            else if ( actor is Group group )
            {
                SetLayoutEnabled( group, enabled );
            }
        }
    }

    /// <inheritdoc />
    protected override void ChildrenChanged()
    {
        InvalidateHierarchy();
    }

    /// <inheritdoc />
    public override void SizeChanged()
    {
        Invalidate();
    }

    /// <inheritdoc />
    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();
        base.Draw( batch, parentAlpha );
    }
}
