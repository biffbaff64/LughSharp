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

namespace LibGDXSharp.Scenes.Scene2D.UI;

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
public class Widget : Actor, ILayout
{
    private bool _layoutEnabled = true;

    /// <summary>
    /// Computes and caches any information needed for drawing and, if this actor
    /// has children, positions and sizes each child, calls <see cref="ILayout.Invalidate"/>
    /// on any each child whose width or height has changed, and calls <see cref="ILayout.Validate"/>
    /// on each child. This method should almost never be called directly, instead
    /// <see cref="ILayout.Validate"/> should be used. 
    /// </summary>
    public void Layout()
    {
    }

    /// <summary>
    /// Invalidates this actor's layout, causing <see cref="ILayout.Layout"/> to happen the
    /// next time <see cref="ILayout.Validate"/> is called. This method should be called when
    /// state changes in the actor that requires a layout but does not change the minimum,
    /// preferred, maximum, or actual size of the actor (meaning it does not affect the
    /// parent actor's layout). 
    /// </summary>
    public void Invalidate()
    {
        NeedsLayout = true;
    }

    /// <summary>
    /// Invalidates this actor and its ascendants, calling <see cref="ILayout.Invalidate"/> on each.
    /// This method should be called when state changes in the actor that affects the minimum,
    /// preferred, maximum, or actual size of the actor (meaning it potentially affects the
    /// parent actor's layout). 
    /// </summary>
    public void InvalidateHierarchy()
    {
        if ( !LayoutEnabled ) return;

        Invalidate();

        if ( Parent is ILayout layout ) layout.InvalidateHierarchy();
    }

    /// <summary>
    /// Ensures the actor has been laid out.
    /// <para>
    /// Calls <see cref="ILayout.Layout"/> if <see cref="ILayout.Invalidate"/> has been called since the
    /// last time <see cref="ILayout.Validate"/> was called, or if the actor otherwise needs to be
    /// laid out. This method is usually called in <see cref="Actor.Draw(IBatch, float)"/> by
    /// the actor itself before drawing is performed. 
    /// </para>
    /// </summary>
    public void Validate()
    {
        if ( !LayoutEnabled ) return;

        Group? parent = Parent;

        if ( FillParent && ( parent != null ) )
        {
            float parentWidth, parentHeight;

            if ( ( Stage != null ) && ( parent == Stage.Root ) )
            {
                parentWidth  = Stage.WorldWidth;
                parentHeight = Stage.WorldHeight;
            }
            else
            {
                parentWidth  = parent.Width;
                parentHeight = parent.Height;
            }

            SetSize( parentWidth, parentHeight );
        }

        if ( !NeedsLayout ) return;

        NeedsLayout = false;

        Layout();
    }

    /// <summary>
    /// If this method is overridden, the super method or <see cref="Validate"/>
    /// should be called to ensure the widget is laid out.
    /// </summary>
    public new void Draw( IBatch batch, float parentAlpha )
    {
        Validate();
    }

    /// <summary>
    /// Sizes this actor to its preferred width and height, then calls <see cref="ILayout.Validate"/>.
    /// <para>
    /// Generally this method should not be called in an actor's constructor because it calls
    /// <see cref="ILayout.Layout"/>, which means a subclass would have Layout() called before the
    /// subclass' constructor. Instead, in constructors simply set the actor's size
    /// to <see cref="ILayout.GetPrefWidth"/> and <see cref="ILayout.GetPrefHeight"/>. This allows
    /// the actor to have a size at construction time for more convenient use with groups that do
    /// not layout their children. 
    /// </para>
    /// </summary>
    public void Pack()
    {
        SetSize( GetPrefWidth(), GetPrefHeight() );
        Validate();
    }

    protected new void SizeChanged()
    {
        Invalidate();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// If true, this actor will be sized to the parent in <see cref="ILayout.Validate"/>. If the
    /// parent is the stage, the actor will be sized to the stage. This method is for convenience
    /// only when the widget's parent does not set the size of its children (such as the stage). 
    /// </summary>
    public bool FillParent { get; set; }
    
    /// <summary>
    /// </summary>
    public bool NeedsLayout { get; set; } = true;

    /// <summary>
    /// Enables or disables the layout for this actor and all child actors, recursively.
    /// When false, <see cref="ILayout.Validate"/> will not cause a layout to occur. This can be useful
    /// when an actor will be manipulated externally, such as with actions. Default is true. 
    /// </summary>
    public bool LayoutEnabled
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
    
    public float GetMinWidth()   => GetPrefWidth();
    public float GetMinHeight()  => GetPrefHeight();
    public float GetPrefWidth()  => 0;
    public float GetPrefHeight() => 0;
    public float GetMaxWidth()   => 0;
    public float GetMaxHeight()  => 0;
    
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
}
