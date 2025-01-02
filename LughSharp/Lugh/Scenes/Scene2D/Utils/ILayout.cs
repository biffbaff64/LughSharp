// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace LughSharp.Lugh.Scenes.Scene2D.Utils;

/// <summary>
/// Provides methods for an actor to participate in layout and to provide
/// a minimum, preferred, and maximum size.
/// </summary>
[PublicAPI]
public interface ILayout
{
    float MinWidth   { get; }
    float MinHeight  { get; }
    float MaxWidth   { get; }
    float MaxHeight  { get; }
    float PrefWidth  { get; }
    float PrefHeight { get; }

    // ========================================================================
    
    /// <summary>
    /// If true, this actor will be sized to the parent in <see cref="Validate()"/>. If the
    /// parent is the stage, the actor will be sized to the stage. This method is for convenience
    /// only when the widget's parent does not set the size of its children (such as the stage).
    /// </summary>
    bool FillParent { get; set; }

    /// <summary>
    /// Enables or disables the layout for this actor and all child actors, recursively.
    /// When false, <see cref="Validate()"/> will not cause a layout to occur. This can be useful
    /// when an actor will be manipulated externally, such as with actions. Default is true.
    /// </summary>
    bool EnableLayout { get; set; }

    /// <summary>
    /// Computes and caches any information needed for drawing and, if this actor
    /// has children, positions and sizes each child, calls <see cref="Invalidate()"/>
    /// on any each child whose width or height has changed, and calls <see cref="Validate()"/>
    /// on each child. This method should almost never be called directly, instead
    /// <see cref="Validate()"/> should be used.
    /// </summary>
    void SetLayout();

    /// <summary>
    /// Invalidates this actor's layout, causing <see cref="SetLayout"/> to happen the
    /// next time <see cref="Validate()"/> is called. This method should be called when
    /// state changes in the actor that requires a layout but does not change the minimum,
    /// preferred, maximum, or actual size of the actor (meaning it does not affect the
    /// parent actor's layout).
    /// </summary>
    void Invalidate();

    /// <summary>
    /// Invalidates this actor and its ascendants, calling <see cref="Invalidate()"/> on each.
    /// This method should be called when state changes in the actor that affects the minimum,
    /// preferred, maximum, or actual size of the actor (meaning it potentially affects the
    /// parent actor's layout).
    /// </summary>
    void InvalidateHierarchy();

    /// <summary>
    /// Ensures the actor has been laid out.
    /// <para>
    /// Calls <see cref="SetLayout"/> if <see cref="Invalidate()"/> has been called since the
    /// last time <see cref="Validate()"/> was called, or if the actor otherwise needs to be
    /// laid out. This method is usually called in <see cref="Actor.Draw"/> by
    /// the actor itself before drawing is performed.
    /// </para>
    /// </summary>
    void Validate();

    /// <summary>
    /// Sizes this actor to its preferred width and height, then calls <see cref="Validate()"/>.
    /// <para>
    /// Generally this method should not be called in an actor's constructor because it calls
    /// <see cref="SetLayout"/>, which means a subclass would have Layout() called before the
    /// subclass' constructor. Instead, in constructors simply set the actor's size
    /// to <see cref="PrefWidth"/> and <see cref="PrefHeight"/>. This allows the actor to have
    /// a size at construction time for more convenient use with groups that do not layout their
    /// children.
    /// </para>
    /// </summary>
    void Pack();
}
