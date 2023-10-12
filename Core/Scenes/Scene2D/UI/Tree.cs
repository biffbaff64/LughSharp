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

using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[PublicAPI]
public class Tree<TN> : WidgetGroup where TN : Node< TN, object, Actor >
{
    public class TreeStyle
    {
    }
}

/// <summary>
/// A <see cref="Tree{TN}"/> node which has an actor and value.
/// <para>
/// A subclass can be used so the generic type parameters don't need
/// to be specified repeatedly.
/// </para>
/// </summary>
/// <typeparam name="TN"> The type for the node's parent and child nodes. </typeparam>
/// <typeparam name="TV"> The type for the node's value. </typeparam>
/// <typeparam name="TA"> The type for the node's actor. </typeparam>
[PublicAPI]
public class Node<TN, TV, TA> where TA : Actor
{
    private TN         _parent;
    private TA         _actor;
    private TV         _value;
    private List< TN > _children   = new( 0 );
    private bool       _selectable = true;
    private bool       _expanded;
    private IDrawable  _icon;
    private float      _height;
}
