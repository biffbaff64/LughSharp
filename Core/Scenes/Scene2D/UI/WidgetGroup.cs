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
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Scenes.Scene2D.UI;

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
    public bool FillParent { get; set; }

    private bool _layoutEnabled = true;

    protected WidgetGroup()
    {
    }

    /// <summary>
    /// Creates a new widget group containing the specified actors.
    /// </summary>
    public WidgetGroup( params Actor[] actors )
    {
        foreach ( Actor actor in actors )
        {
            AddActor( actor );
        }
    }

    public float MinWidth
    {
        get => PrefWidth;
        set { }
    }

    public float MinHeight
    {
        get => PrefHeight;
        set { }
    }

    public float MaxWidth   { get; set; } = 0;
    public float MaxHeight  { get; set; } = 0;
    public float PrefWidth  { get; set; } = 0;
    public float PrefHeight { get; set; } = 0;

    public void SetLayoutEnabled( bool enabled )
    {
        _layoutEnabled = enabled;
        SetLayoutEnabled( this, enabled );
    }

    private static void SetLayoutEnabled( Group parent, bool enabled )
    {
        SnapshotArray< Actor > children = parent.Children;

        for ( int i = 0, n = children.Size; i < n; i++ )
        {
            Actor actor = children.Get( i );

            if ( actor is ILayout layout )
            {
                layout.LayoutEnabled = enabled;
            }
            else if ( actor is Group group )
            {
                SetLayoutEnabled( group, enabled );
            }
        }
    }

    public void Validate()
    {
        if ( !_layoutEnabled )
        {
            return;
        }

        Group? parent = Parent;

        if ( FillParent && ( parent != null ) )
        {
            float  parentWidth, parentHeight;
            Stage? stage = Stage;

            if ( ( stage != null ) && ( parent == stage.Root ) )
            {
                parentWidth  = stage.StageWidth;
                parentHeight = stage.StageHeight;
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

        Layout();

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

                Layout();

                if ( !NeedsLayout )
                {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Returns true if the widget's layout has been invalidated.
    /// </summary>
    public bool NeedsLayout { get; private set; } = true;

    public void Invalidate()
    {
        NeedsLayout = true;
    }

    public void InvalidateHierarchy()
    {
        Invalidate();

        if ( Parent is ILayout layout )
        {
            layout.InvalidateHierarchy();
        }
    }

    protected new void ChildrenChanged()
    {
        InvalidateHierarchy();
    }

    protected new void SizeChanged()
    {
        Invalidate();
    }

    public void Pack()
    {
        SetSize( PrefWidth, PrefHeight );

        Validate();

        // Validating the layout may change the pref size. Eg, a wrapped label doesn't
        // know its pref height until it knows its width, so it calls invalidateHierarchy()
        // in layout() if its pref height has changed.
        SetSize( PrefWidth, PrefHeight );

        Validate();
    }

    public void Layout()
    {
    }

    /// <summary>
    /// If this method is overridden, the super method or <see cref="Validate()"/>
    /// should be called to ensure the widget group is laid out.
    /// </summary>
    protected new void Draw( IBatch batch, float parentAlpha )
    {
        Validate();
        base.Draw( batch, parentAlpha );
    }

    public bool LayoutEnabled
    {
        get => _layoutEnabled;
        set => _layoutEnabled = value;
    }

    public float GetMinWidth()   => MinWidth;
    public float GetMinHeight()  => MinHeight;
    public float GetPrefWidth()  => PrefWidth;
    public float GetPrefHeight() => PrefHeight;
    public float GetMaxWidth()   => MaxWidth;
    public float GetMaxHeight()  => MaxHeight;
}