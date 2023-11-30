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
using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A tree widget where each node has an icon, actor, and child nodes.
/// The preferred size of the tree is determined by the preferred size
/// of the actors for the expanded nodes.
/// </summary>
/// <typeparam name="TNode"> The type of nodes in the tree. </typeparam>
/// <typeparam name="TValue"> The type of values for each node. </typeparam>
[PublicAPI]
public class Tree<TNode, TValue> : WidgetGroup
    where TNode : Tree< TNode, TValue >.Node
{
    private readonly Vector2 _tmp = new();

    public TNode?         RangeStart    { get; set; }
    public ClickListener? ClickListener { get; set; }
    public TreeStyle?     Style         { get; set; }
    public List< TNode >  RootNodes     { get; set; } = new();
    public float          YSpacing      { get; set; } = 4;
    public float          IndentSpacing { get; set; }
    public TNode?         OverNode      { get; set; }

    private TreeSelection _selection;
    private float         _iconSpacingLeft  = 2;
    private float         _iconSpacingRight = 2;
    private float         _paddingLeft;
    private float         _paddingRight;
    private float         _prefWidth;
    private float         _prefHeight;
    private bool          _sizeInvalid = true;
    private TNode?        _foundNode;

    public Tree( Skin skin )
        : this( skin.Get< TreeStyle >() )
    {
    }

    public Tree( Skin skin, string styleName )
        : this( skin.Get< TreeStyle >( styleName ) )
    {
    }

    public Tree( TreeStyle style )
    {
        _selection = new TreeSelection( this )
        {
            Actor    = this,
            Multiple = true
        };

        SetStyle( style );
        Initialise();
    }

    private void Initialise()
    {
    }

    public void SetStyle( TreeStyle style )
    {
        this.Style = style;

        // Reasonable default.
        if ( IndentSpacing == 0 )
        {
            IndentSpacing = PlusMinusWidth();
        }
    }

    public void Add( TNode node )
    {
        Insert( RootNodes.Count, node );
    }

    public void Insert( int index, TNode node )
    {
    }

    public void Remove( TNode node )
    {
    }

    public override void ClearChildren()
    {
    }

    public override void Invalidate()
    {
    }

    private float PlusMinusWidth()
    {
        return 0f;
    }

    private void ComputeSize()
    {
    }

    private void ComputeSize( List< TNode > nodes, float indent, float plusMinusWidth )
    {
    }

    public override void Layout()
    {
    }

    private float Layout( List< TNode > nodes, float indent, float y, float plusMinusWidth )
    {
        return 0f;
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
    }

    protected void DrawBackground( IBatch batch, float parentAlpha )
    {
    }

    private void Draw( IBatch batch, List< TNode > nodes, float indent, float plusMinusWidth )
    {
    }

    protected void DrawSelection( TNode node, IDrawable selection, IBatch batch, float x, float y, float width, float height )
    {
    }

    protected void DrawOver( TNode node, IDrawable over, IBatch batch, float x, float y, float width, float height )
    {
    }

    protected void DrawExpandIcon( TNode node, IDrawable expandIcon, IBatch batch, float x, float y )
    {
    }

    protected void DrawIcon( TNode node, IDrawable icon, IBatch batch, float x, float y )
    {
    }

    protected IDrawable? GetExpandIcon( TNode node, float iconX )
    {
        return null;
    }

    public TNode? GetNodeAt( float y )
    {
        return null;
    }

    private float GetNodeAt( List< TNode > nodes, float y, float rowY )
    {
        return 0f;
    }

    private void SelectNodes( List< TNode > nodes, float low, float high )
    {
    }

    public Selection< TNode >? GetSelection()
    {
        return null;
    }

    public TNode? GetSelectedNode()
    {
        return null;
    }

    public TValue? GetSelectedValue()
    {
        return default( TValue? );
    }

    public void UpdateRootNodes()
    {
    }

    public void FindExpandedValues( List< TValue > values )
    {
    }

    private static bool FindExpandedValues( List< TNode > nodes, List< TValue > values )
    {
        return false;
    }

    public void RestoreExpandedValues( List< TValue > values )
    {
    }

    public TNode? FindNode( TValue value )
    {
        return null;
    }

    private static Node? FindNode<T>( List< T > nodes, object value )
        where T : Node
    {
        return null;
    }

    public void CollapseAll()
    {
    }

    private static void CollapseAll<T>( List< T > nodes )
        where T : Node
    {
    }

    public void ExpandAll()
    {
    }

    private static void ExpandAll<T>( List< T > nodes )
        where T : Node
    {
    }

    // ------------------------------------------------------------------------

    public TValue? GetOverValue()
    {
        if ( OverNode == null )
        {
            return default( TValue? );
        }

        return OverNode.Value;
    }

    public void SetPadding( float padding )
    {
        this._paddingLeft  = padding;
        this._paddingRight = padding;
    }

    public void SetPadding( float left, float right )
    {
        this._paddingLeft  = left;
        this._paddingRight = right;
    }

    public void SetIconSpacing( float left, float right )
    {
        this._iconSpacingLeft  = left;
        this._iconSpacingRight = right;
    }

    public float GetPrefWidth()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _prefWidth;
    }

    public float GetPrefHeight()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _prefHeight;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    [PublicAPI]
    public class TreeSelection : Selection< TNode >
    {
        private readonly Tree< TNode, TValue > _parent;

        public TreeSelection( Tree< TNode, TValue > p )
        {
            this._parent = p;
        }

        /// <inheritdoc/>
        protected override void Changed()
        {
            switch ( Size() )
            {
                case 0:
                {
                    _parent.RangeStart = default( TNode );

                    break;
                }

                case 1:
                {
                    _parent.RangeStart = First();

                    break;
                }
            }
        }
    }

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

    /// <summary>
    /// The style for a <see cref="Tree{TN,TV}"/>.
    /// </summary>
    [PublicAPI]
    public class TreeStyle
    {
        public IDrawable  Plus       { get; set; }
        public IDrawable  Minus      { get; set; }
        public IDrawable? PlusOver   { get; set; } = null;
        public IDrawable? MinusOver  { get; set; } = null;
        public IDrawable? Over       { get; set; } = null;
        public IDrawable? Selection  { get; set; } = null;
        public IDrawable? Background { get; set; } = null;

        public TreeStyle( IDrawable plus, IDrawable minus, IDrawable? selection )
        {
            this.Plus      = plus;
            this.Minus     = minus;
            this.Selection = selection;
        }

        public TreeStyle( TreeStyle style )
        {
            Plus  = style.Plus;
            Minus = style.Minus;

            PlusOver  = style.PlusOver;
            MinusOver = style.MinusOver;

            Over       = style.Over;
            Selection  = style.Selection;
            Background = style.Background;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// A <see cref="Tree{TN,TV}"/> node which has an actor and value.
    /// <para>
    /// A subclass can be used so the generic type parameters don't need
    /// to be specified repeatedly.
    /// </para>
    /// </summary>

//    /// <typeparam name="TNode"> The type for the node's parent and child nodes. </typeparam>
//    /// <typeparam name="TValue"> The type for the node's value. </typeparam>
//    /// <typeparam name="TNodeActor"> The type for the node's actor. </typeparam>
    [PublicAPI]
    public class Node
    {
        public TValue?    Value      { get; set; }
        public TNode?     Parent     { get; set; }
        public IDrawable? Icon       { get; set; }
        public bool       Selectable { get; set; } = true;

        private Actor?         _actor;
        private List< TNode >? _children = new();
        private bool           _expanded;
        private float          _height;

        /// <summary>
        /// Creates a node without an actor. An actor must be
        /// set before this node can be used.
        /// </summary>
        public Node()
        {
        }

        public Node( Actor actor )
        {
            ArgumentNullException.ThrowIfNull( actor );

            this._actor = actor;
        }

        public void SetExpanded( bool expanded )
        {
            if ( expanded == this._expanded )
            {
                return;
            }

            this._expanded = expanded;

            Tree< TNode, TValue >? tree = GetTree();

            if ( ( tree == null ) || ( this._children == null ) || ( this._actor == null ) )

            {
                return;
            }

            if ( this._children.Count == 0 )
            {
                return;
            }

            TNode[] children   = this._children.ToArray();
            var     actorIndex = this._actor.GetZIndex() + 1;

            if ( expanded )
            {
                for ( int i = 0, n = this._children!.Count; i < n; i++ )
                {
                    actorIndex += children[ i ].AddToTree( tree, actorIndex );
                }
            }
            else
            {
                for ( int i = 0, n = this._children!.Count; i < n; i++ )
                {
                    children?[ i ].RemoveFromTree( tree, actorIndex );
                }
            }
        }

        /// <summary>
        /// Called to add the actor to the tree when the node's parent is expanded.
        /// </summary>
        /// <returns> The number of node actors added to the tree. </returns>
        protected int AddToTree( Tree< TNode, TValue > tree, int actorIndex )
        {
            tree.AddActorAt( actorIndex, _actor! );

            if ( !_expanded )
            {
                return 1;
            }

            var      childIndex = actorIndex + 1;
            TNode[]? children   = this._children?.ToArray();

            for ( int i = 0, n = this._children!.Count; i < n; i++ )
            {
                childIndex += children![ i ].AddToTree( tree, childIndex );
            }

            return childIndex - actorIndex;
        }

        /// <summary>
        /// Called to remove the actor from the tree, eg when the node is
        /// removed or the node's parent is collapsed.
        /// </summary>
        protected void RemoveFromTree( Tree< TNode, TValue > tree, int actorIndex )
        {
            if ( !_expanded )
            {
                return;
            }

            TNode[]? children = this._children?.ToArray();

            if ( children is null )
            {
                return;
            }

            for ( int i = 0, n = this._children!.Count; i < n; i++ )
            {
                children[ i ].RemoveFromTree( tree, actorIndex );
            }
        }

        public void Add( TNode node )
        {
            Insert( this._children!.Count, node );
        }

        public void AddAll( List< TNode > nodes )
        {
            for ( int i = 0, n = nodes.Count; i < n; i++ )
            {
                Insert( this._children!.Count, nodes[ i ] );
            }
        }

        public void Insert( int childIndex, TNode node )
        {
            if ( _children == null )
            {
                return;
            }
            
            node.Parent = this.Parent;
            
            _children.Insert( childIndex, node );

            if ( !_expanded )
            {
                return;
            }

            Tree< TNode, TValue >? tree = GetTree();

            if ( tree != null )
            {
                int actorIndex;

                if ( childIndex == 0 )
                {
                    actorIndex = _actor!.GetZIndex() + 1;
                }
                else if ( childIndex < ( _children?.Count - 1 ) )
                {
                    actorIndex = _children![ childIndex + 1 ]._actor!.GetZIndex();
                }
                else
                {
                    TNode before = _children![ childIndex - 1 ];
                    actorIndex = before.Actor!.GetZIndex() + before.CountActors();
                }

                node.AddToTree( tree, actorIndex );
            }
        }

        protected int CountActors()
        {
            if ( !_expanded )
            {
                return 1;
            }

            var count = 1;

            for ( int i = 0, n = this._children!.Count; i < n; i++ )
            {
                count += this._children[ i ].CountActors();
            }

            return count;
        }

        /// <summary>
        /// Remove this node from its parent.
        /// </summary>
        public void Remove()
        {
            Tree< TNode, TValue >? tree = GetTree();

            if ( tree != null )
            {
                tree.Remove( this.Parent! );
            }
            else if ( Parent != null )
            {
                Parent.Remove( this.Parent! );
            }
        }

        /// <summary>
        /// Remove the specified child node from this node. Does nothing if
        /// the node is not a child of this node.
        /// </summary>
        public void Remove( TNode? node )
        {
            if ( ( node == null ) || ( _children == null ) )
            {
                return;
            }

            if ( !_children.Remove( node ) )
            {
                return;
            }

            if ( !_expanded )
            {
                return;
            }

            node.RemoveFromTree( GetTree()!, node.Actor!.GetZIndex() );
        }

        /// <summary>
        /// Removes all children from this node.
        /// </summary>
        public void ClearChildren()
        {
            if ( _expanded )
            {
                Tree< TNode, TValue >? tree = GetTree();

                if ( ( tree != null ) && ( this._children != null ) )
                {
                    var actorIndex = _actor!.GetZIndex() + 1;

                    for ( int i = 0, n = this._children.Count; i < n; i++ )
                    {
                        this._children[ i ].RemoveFromTree( tree, actorIndex );
                    }
                }
            }

            _children?.Clear();
        }

        /// <summary>
        /// Returns the tree this node's actor is currently in, or null.
        /// The actor is only in the tree when all of its parent nodes
        /// are expanded.
        /// </summary>
        public Tree< TNode, TValue >? GetTree()
        {
            GdxRuntimeException.ThrowIfNull( _actor );

            if ( _actor.Parent is Tree< TNode, TValue > tree )
            {
                return tree;
            }

            return null;
        }

        public Actor? Actor
        {
            get => _actor;
            set
            {
                if ( _actor != null )
                {
                    Tree< TNode, TValue >? tree = GetTree();

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

        public bool IsExpanded()
        {
            return _expanded;
        }

        /// <summary>
        /// If the children order is changed, <see cref="UpdateChildren()"/> must
        /// be called to ensure the node's actors are in the correct order. That
        /// is not necessary if this node is not in the tree or is not expanded,
        /// because then the child node's actors are not in the tree.
        /// </summary>
        public List< TNode >? GetChildren()
        {
            return _children;
        }

        public bool HasChildren()
        {
            return _children?.Count > 0;
        }

        /// <summary>
        /// Updates the order of the actors in the tree for this node and all
        /// child nodes. This is useful after changing the order.
        /// of <see cref="GetChildren()"/>.
        /// </summary>
        /// <seealso cref="Tree{TNode,TValue}.UpdateRootNodes()"/>
        public void UpdateChildren()
        {
            if ( !_expanded )
            {
                return;
            }

            Tree< TNode, TValue >? tree = GetTree();

            if ( tree == null )
            {
                return;
            }

            TNode[]? children   = this._children?.ToArray();
            var      n          = this._children!.Count;
            var      actorIndex = _actor!.GetZIndex() + 1;

            for ( var i = 0; i < n; i++ )
            {
                children?[ i ].RemoveFromTree( tree, actorIndex );
            }

            for ( var i = 0; i < n; i++ )
            {
                actorIndex += children![ i ].AddToTree( tree, actorIndex );
            }
        }

        public int GetLevel()
        {
            var   level   = 0;
            Node? current = this;

            do
            {
                level++;
                current = current.Parent;
            }
            while ( current != null );

            return level;
        }

        /// <summary>
        /// Returns this node or the child node with the specified value, or null.
        /// </summary>
        public TNode? FindNode( TValue? value )
        {
            ArgumentNullException.ThrowIfNull( value );

            if ( value.Equals( this.Value ) )
            {
                return ( TNode )this;
            }

            return ( TNode? )Tree< TNode, TValue >.FindNode( this._children!, value );
        }

        /// <summary>
        /// Collapses all nodes under and including this node.
        /// </summary>
        public void CollapseAll()
        {
            SetExpanded( false );
            Tree< TNode, TValue >.CollapseAll( this._children! );
        }

        /// <summary>
        /// Expands all nodes under and including this node.
        /// </summary>
        public void ExpandAll()
        {
            SetExpanded( true );

            if ( _children?.Count > 0 )
            {
                Tree< TNode, TValue >.ExpandAll( _children );
            }
        }

        /// <summary>
        /// Expands all parent nodes of this node.
        /// </summary>
        public void ExpandTo()
        {
            TNode? node = Parent;

            while ( node != null )
            {
                node.SetExpanded( true );
                node = node.Parent;
            }
        }

        public void FindExpandedValues( List< TValue > values )
        {
            if ( _expanded
              && !Tree< TNode, TValue >.FindExpandedValues( this._children!, values ) )
            {
                values.Add( Value! );
            }
        }

        public void RestoreExpandedValues( List< TValue > values )
        {
            for ( int i = 0, n = values.Count; i < n; i++ )
            {
                TNode? node = FindNode( values[ i ] );

                if ( node is not null )
                {
                    node.SetExpanded( true );
                    node.ExpandTo();
                }
            }
        }

        /// <summary>
        /// Returns the height of the node as calculated for layout. A subclass
        /// may override and increase the returned height to create a blank space
        /// in the tree above the node, eg for a separator.
        /// </summary>
        public virtual float GetHeight()
        {
            return _height;
        }

        /// <summary>
        /// Returns true if the specified node is this node or an ascendant of this node.
        /// </summary>
        public bool IsAscendantOf( TNode node )
        {
            ArgumentNullException.ThrowIfNull( node );

            TNode? current = node;

            do
            {
                if ( current == this )
                {
                    return true;
                }

                current = current.Parent;
            }
            while ( current != null );

            return false;
        }

        /// <summary>
        /// Returns true if the specified node is this node or an descendant of this node.
        /// </summary>
        public bool IsDescendantOf( TNode? node )
        {
            if ( node == null )
            {
                return false;
            }

            Node? parent = this;

            do
            {
                if ( parent == node )
                {
                    return true;
                }

                parent = parent.Parent;
            }
            while ( parent != null );

            return false;
        }
    }
}
