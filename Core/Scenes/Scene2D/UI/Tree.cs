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
public class Node<TN, TV, TA> where TN : Node< TN, TV, TA > where TA : Actor
{
    public bool Selectable { get; set; } = true;
    public bool Expanded   { get; private set; }
    public TV?  Value      { get; set; }

    /// <summary>
    /// Sets an icon that will be drawn to the left of the actor.
    /// </summary>
    public IDrawable? Icon { get; set; }

    private TN?         _parent;
    private TA?         _actor;
    private List< TN >? _children = new( 0 );
    private float       _height;

    /// <summary>
    /// Creates a node without an actor. An actor must be set using
    /// <see cref="SetActor(TA)"/> before this node can be used.
    /// </summary>
    public Node()
    {
    }

    public Node( TA actor )
    {
        ArgumentNullException.ThrowIfNull( actor );

        this._actor = actor;
    }

    public void SetExpanded( bool expanded )
    {
        Debug.Assert( _actor != null );
        Debug.Assert( _children != null );

        if ( expanded == this.Expanded )
        {
            return;
        }

        this.Expanded = expanded;

        if ( _children.Count == 0 )
        {
            return;
        }

        Tree< TN > tree = GetTree();

        if ( tree == null )
        {
            return;
        }

        var actorIndex = _actor?.GetZIndex() + 1;

        if ( expanded )
        {
            foreach ( TN child in _children )
            {
                actorIndex += child.AddToTree( tree, actorIndex );
            }
        }
        else
        {
            foreach ( TN child in _children )
            {
                child.RemoveFromTree( tree, actorIndex );
            }
        }
    }

    /** Called to add the actor to the tree when the node's parent is expanded.
     * @return The number of node actors added to the tree. */
    protected int AddToTree( Tree< TN, TV > tree, int actorIndex )
    {
        tree.addActorAt( actorIndex, actor );

        if ( !expanded )
        {
            return 1;
        }

        int      childIndex = actorIndex + 1;
        Object[] children   = this.children.items;

        for ( int i = 0, n = this.children.size; i < n; i++ )
        {
            childIndex += ( ( TN )children[ i ] ).addToTree( tree, childIndex );
        }

        return childIndex - actorIndex;
    }

    /** Called to remove the actor from the tree, eg when the node is removed or the node's parent is collapsed. */
    protected void RemoveFromTree( Tree< TN, TV > tree, int actorIndex )
    {
        Actor removeActorAt = tree.removeActorAt( actorIndex, true );

        // assert removeActorAt != actor; // If false, either 1) there's a bug, or 2) the children were modified.
        if ( !expanded )
        {
            return;
        }

        Object[] children = this.children.items;

        for ( int i = 0, n = this.children.size; i < n; i++ )
        {
            ( ( TN )children[ i ] ).removeFromTree( tree, actorIndex );
        }
    }

    public void Add( TN node )
    {
        Insert( children.size, node );
    }

    public void AddAll( Array< TN > nodes )
    {
        for ( int i = 0, n = nodes.size; i < n; i++ )
        {
            Insert( children.size, nodes.get( i ) );
        }
    }

    public void Insert( int childIndex, TN node )
    {
        node.parent = this;
        children.insert( childIndex, node );

        if ( !expanded )
        {
            return;
        }

        Tree tree = GetTree();

        if ( tree != null )
        {
            int actorIndex;

            if ( childIndex == 0 )
            {
                actorIndex = actor.getZIndex() + 1;
            }
            else if ( childIndex < children.size - 1 )
            {
                actorIndex = children.get( childIndex + 1 ).actor.getZIndex();
            }
            else
            {
                TN before = children.get( childIndex - 1 );
                actorIndex = before.actor.getZIndex() + before.countActors();
            }

            node.addToTree( tree, actorIndex );
        }
    }

    int CountActors()
    {
        if ( !expanded )
        {
            return 1;
        }

        int      count    = 1;
        Object[] children = this.children.items;

        for ( int i = 0, n = this.children.size; i < n; i++ )
        {
            count += ( ( TN )children[ i ] ).countActors();
        }

        return count;
    }

    /** Remove this node from its parent. */
    public void Remove()
    {
        Tree tree = GetTree();

        if ( tree != null )
        {
            tree.remove( this );
        }
        else if ( parent != null ) //
        {
            parent.remove( this );
        }
    }

    /** Remove the specified child node from this node. Does nothing if the node is not a child of this node. */
    public void Remove( TN node )
    {
        if ( !children.removeValue( node, true ) )
        {
            return;
        }

        if ( !expanded )
        {
            return;
        }

        Tree tree = GetTree();

        if ( tree != null )
        {
            node.removeFromTree( tree, node.actor.getZIndex() );
        }
    }

    /** Removes all children from this node. */
    public void ClearChildren()
    {
        if ( expanded )
        {
            Tree tree = GetTree();

            if ( tree != null )
            {
                int      actorIndex = actor.getZIndex() + 1;
                Object[] children   = this.children.items;

                for ( int i = 0, n = this.children.size; i < n; i++ )
                {
                    ( ( TN )children[ i ] ).removeFromTree( tree, actorIndex );
                }
            }
        }

        children.clear();
    }

    /** Returns the tree this node's actor is currently in, or null. The actor is only in the tree when all of its parent nodes
     * are expanded. */
    public Tree< TN, TV > GetTree()
    {
        Group                  parent = actor.getParent();
        if ( parent instanceof tree) return ( Tree )parent;
        return null;
    }

    public TA? Actor
    {
        get => _actor;
        set
        {
            if ( _actor != null )
            {
                Tree< TN, TV >? tree = GetTree();

                if ( tree != null )
                {
                    var index = _actor.GetZIndex();

                    tree.RemoveActorAt( index, true );
                    tree.AddActorAt( index, value! );
                }
            }

            _actor = value;
        }
    }

    /// <summary>
    /// If the children order is changed, <see cref="UpdateChildren()"/>" must
    /// be called to ensure the node's actors are in the correct order. That is
    /// not necessary if this node is not in the tree or is not expanded, because
    /// then the child node's actors are not in the tree.
    /// </summary>
    public List< TN >? GetChildren()
    {
        return _children;
    }

    public bool HasChildren()
    {
        return _children?.Count > 0;
    }

    /// <summary>
    /// Updates the order of the actors in the tree for this node and all child
    /// nodes. This is useful after changing the order of <see cref="GetChildren()"/>.
    /// </summary>
    /// <seealso cref="Tree.UpdateRootNodes()"/>
    public void UpdateChildren()
    {
        Debug.Assert( _actor != null );
        Debug.Assert( _children != null );

        if ( !Expanded )
        {
            return;
        }

        var tree = GetTree();

        if ( tree == null )
        {
            return;
        }

        var actorIndex = _actor.GetZIndex() + 1;

        foreach ( TN child in _children )
        {
            child.RemoveFromTree( tree, actorIndex );
        }

        foreach ( TN child in _children )
        {
            actorIndex += child.AddToTree( tree, actorIndex );
        }
    }

    public TN? GetParent()
    {
        return _parent;
    }

    public int GetLevel()
    {
        int   level   = 0;
        Node? current = this;

        do
        {
            level++;
            current = current.GetParent();
        }
        while ( current != null );

        return level;
    }

    /// <summary>
    /// Returns this node or the child node with the specified value, or null.
    /// </summary>
    public TN? FindNode( TV value )
    {
        ArgumentNullException.ThrowIfNull( value );

        if ( value.Equals( this.Value ) )
        {
            return ( TN? )this;
        }

        return ( TN )Tree.findNode( children, value );
    }

    /// <summary>
    /// Collapses all nodes under and including this node.
    /// </summary>
    public void CollapseAll()
    {
        SetExpanded( false );
        Tree.CollapseAll( _children );
    }

    /// <summary>
    /// Expands all nodes under and including this node.
    /// </summary>
    public void ExpandAll()
    {
        SetExpanded( true );

        if ( _children?.Count > 0 )
        {
            Tree < , >.ExpandAll( _children );
        }
    }

    /// <summary>
    /// Expands all parent nodes of this node.
    /// </summary>
    public void ExpandTo()
    {
        TN? node = _parent;

        while ( node != null )
        {
            node.SetExpanded( true );
            node = node._parent;
        }
    }

    public void FindExpandedValues( List< TV > values )
    {
        if ( Expanded && !Tree.FindExpandedValues( _children, values ) )
        {
            values.Add( Value );
        }
    }

    public void RestoreExpandedValues( List< TV > values )
    {
        for ( int i = 0, n = values.size; i < n; i++ )
        {
            TN node = FindNode( values.get( i ) );

            if ( node != null )
            {
                node.setExpanded( true );
                node.expandTo();
            }
        }
    }

    /// <summary>
    /// Returns the height of the node as calculated for layout. A subclass
    /// may override and increase the returned height to create a blank space
    /// in the tree above the node, eg for a separator.
    /// </summary>
    public float GetHeight()
    {
        return _height;
    }

    /// <summary>
    /// Returns true if the specified node is this node or an ascendant of this node.
    /// </summary>
    public bool ISAscendantOf( TN node )
    {
        ArgumentNullException.ThrowIfNull( node );

        Node current = node;

        do
        {
            if ( current == this )
            {
                return true;
            }

            current = current.parent;
        }
        while ( current != null );

        return false;
    }

    /// <summary>
    /// Returns true if the specified node is this node or an descendant of this node.
    /// </summary>
    public bool ISDescendantOf( TN node )
    {
        ArgumentNullException.ThrowIfNull( node );

        Node? parent = this;

        do
        {
            if ( parent == node )
            {
                return true;
            }

            parent = parent._parent;
        }
        while ( parent != null );

        return false;
    }
}
