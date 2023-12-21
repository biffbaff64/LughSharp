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
public class Tree<TNode, TValue> : WidgetGroup where TNode : Tree< TNode, TValue >.Node
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
    private bool          _sizeInvalid      = true;
    private float         _iconSpacingLeft  = 2;
    private float         _iconSpacingRight = 2;
    private float         _paddingLeft;
    private float         _paddingRight;
    private float         _prefWidth;
    private float         _prefHeight;
    private TNode?        _foundNode;

    /// <summary>
    /// Construct a new Tree using the supplied <see cref="Skin"/>
    /// and a default <see cref="TreeStyle"/> from that skin.
    /// </summary>
    public Tree( Skin skin )
        : this( skin.Get< TreeStyle >() )
    {
    }

    /// <summary>
    /// 
    /// </summary>
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
        int actorIndex;

        if ( node.Parent != null )
        {
            node.Parent.Remove( node );
            node.Parent = null;
        }
        else
        {
            var existingIndex = RootNodes.IndexOf( node );

            if ( existingIndex != -1 )
            {
                if ( existingIndex == index )
                {
                    return;
                }

                if ( existingIndex < index )
                {
                    index--;
                }

                RootNodes.RemoveAt( existingIndex );

                actorIndex = node.Actor!.GetZIndex();

                if ( actorIndex != -1 )
                {
                    node.RemoveFromTree( this, actorIndex );
                }
            }
        }

        RootNodes.Insert( index, node );

        if ( index == 0 )
        {
            actorIndex = 0;
        }
        else if ( index < ( RootNodes.Count - 1 ) )
        {
            actorIndex = RootNodes[ index + 1 ].Actor!.GetZIndex();
        }
        else
        {
            TNode before = RootNodes[ index - 1 ];

            actorIndex = before.Actor!.GetZIndex() + before.CountActors();
        }

        node.AddToTree( this, actorIndex );
    }

    public void Remove( TNode node )
    {
        if ( node.Parent != null )
        {
            node.Parent.Remove( node );

            return;
        }

        if ( !RootNodes.Remove( node ) )
        {
            return;
        }

        var actorIndex = node.Actor!.GetZIndex();

        if ( actorIndex != -1 )
        {
            node.RemoveFromTree( this, actorIndex );
        }
    }

    public override void ClearChildren()
    {
        base.ClearChildren();

        OverNode = null;
        RootNodes.Clear();
        _selection.Clear();
    }

    public override void Invalidate()
    {
        base.Invalidate();

        _sizeInvalid = true;
    }

    private float PlusMinusWidth()
    {
        if ( Style == null )
        {
            throw new GdxRuntimeException( "Style should not be null!" );
        }

        var width = Math.Max( Style.Plus.MinWidth, Style.Minus.MinWidth );

        if ( Style.PlusOver != null )
        {
            width = Math.Max( width, Style.PlusOver.MinWidth );
        }

        if ( Style.MinusOver != null )
        {
            width = Math.Max( width, Style.MinusOver.MinWidth );
        }

        return width;
    }

    private void ComputeSize()
    {
        _sizeInvalid = false;
        _prefWidth   = PlusMinusWidth();
        _prefHeight  = 0;

        ComputeSize( RootNodes, 0, _prefWidth );

        _prefWidth += ( _paddingLeft + _paddingRight );
    }

    private void ComputeSize( List< TNode > nodes, float indent, float plusMinusWidth )
    {
        var ySpacing = this.YSpacing;
        var spacing  = ( _iconSpacingLeft + _iconSpacingRight );

        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            TNode  node     = nodes[ i ];
            var    rowWidth = indent + plusMinusWidth;
            Actor? actor    = node.Actor;

            if ( actor != null )
            {
                if ( actor is ILayout layout )
                {
                    rowWidth    += layout.PrefWidth;
                    node.Height =  layout.PrefHeight;
                }
                else
                {
                    rowWidth    += actor.Width;
                    node.Height =  actor.Height;
                }

                if ( node.Icon != null )
                {
                    rowWidth    += spacing + node.Icon.MinWidth;
                    node.Height =  Math.Max( node.Height, node.Icon.MinHeight );
                }

                _prefWidth  =  Math.Max( _prefWidth, rowWidth );
                _prefHeight += node.Height + ySpacing;

                if ( node.IsExpanded )
                {
                    ComputeSize( node.NodeChildren!, indent + IndentSpacing, plusMinusWidth );
                }
            }
        }
    }


    public override void Layout()
    {
        if ( sizeInvalid ) computeSize();
        layout( rootNodes, paddingLeft, getHeight() - ySpacing / 2, plusMinusWidth() );
    }

    private float Layout( List< TNode > nodes, float indent, float y, float plusMinusWidth )
    {
        float ySpacing        = this.ySpacing;
        float iconSpacingLeft = this.iconSpacingLeft;
        float spacing         = iconSpacingLeft + iconSpacingRight;

        for ( int i = 0, n = nodes.size; i < n; i++ )
        {
            N     node = nodes.get( i );
            float x    = indent + plusMinusWidth;

            if ( node.icon != null )
                x += spacing + node.icon.getMinWidth();
            else
                x += iconSpacingLeft;

            if ( node.actor instanceof Layout) ( ( Layout )node.actor ).pack();
            y -= node.getHeight();
            node.actor.setPosition( x, y );
            y -= ySpacing;
            if ( node.expanded ) y = layout( node.children, indent + indentSpacing, y, plusMinusWidth );
        }

        return y;
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        drawBackground( batch, parentAlpha );
        Color color = getColor();
        batch.setColor( color.r, color.g, color.b, color.a * parentAlpha );
        draw( batch, rootNodes, paddingLeft, plusMinusWidth() );
        super.draw( batch, parentAlpha ); // Draw node actors.
    }

    /// <summary>
    /// Called to draw the background.
    /// Default implementation draws the style background drawable.
    /// </summary>
    protected void DrawBackground( IBatch batch, float parentAlpha )
    {
        if ( style.background != null )
        {
            Color color = getColor();
            batch.setColor( color.r, color.g, color.b, color.a * parentAlpha );
            style.background.draw( batch, getX(), getY(), getWidth(), getHeight() );
        }
    }

    private void Draw( IBatch batch, List< TNode > nodes, float indent, float plusMinusWidth )
    {
        Rectangle cullingArea = getCullingArea();
        float     cullBottom  = 0, cullTop = 0;

        if ( cullingArea != null )
        {
            cullBottom = cullingArea.y;
            cullTop    = cullBottom + cullingArea.height;
        }

        TreeStyle style = this.style;
        float     x     = getX(), y = getY(), expandX = x + indent, iconX = expandX + plusMinusWidth + iconSpacingLeft;

        for ( int i = 0, n = nodes.size; i < n; i++ )
        {
            N     node   = nodes.get( i );
            Actor actor  = node.actor;
            float actorY = actor.getY(), height = node.height;

            if ( cullingArea == null || ( actorY + height >= cullBottom && actorY <= cullTop ) )
            {
                if ( selection.contains( node ) && style.selection != null )
                {
                    drawSelection( node, style.selection, batch, x, y + actorY - ySpacing / 2, getWidth(), height + ySpacing );
                }
                else if ( node == overNode && style.over != null )
                {
                    drawOver( node, style.over, batch, x, y + actorY - ySpacing / 2, getWidth(), height + ySpacing );
                }

                if ( node.icon != null )
                {
                    float iconY = y + actorY + Math.round( ( height - node.icon.getMinHeight() ) / 2 );
                    batch.setColor( actor.getColor() );
                    drawIcon( node, node.icon, batch, iconX, iconY );
                    batch.setColor( 1, 1, 1, 1 );
                }

                if ( node.children.size > 0 )
                {
                    Drawable expandIcon = getExpandIcon( node, iconX );
                    float    iconY      = y + actorY + Math.round( ( height - expandIcon.getMinHeight() ) / 2 );
                    drawExpandIcon( node, expandIcon, batch, expandX, iconY );
                }
            }
            else if ( actorY < cullBottom )
            {
                return;
            }

            if ( node.expanded && node.children.size > 0 ) draw( batch, node.children, indent + indentSpacing, plusMinusWidth );
        }
    }

    protected void DrawSelection( TNode node, IDrawable selection, IBatch batch, float x, float y, float width, float height )
    {
        selection.draw(batch, x, y, width, height);
    }

    protected void DrawOver( TNode node, IDrawable over, IBatch batch, float x, float y, float width, float height )
    {
        over.draw(batch, x, y, width, height);
    }

    protected void DrawExpandIcon( TNode node, IDrawable expandIcon, IBatch batch, float x, float y )
    {
        expandIcon.draw(batch, x, y, expandIcon.getMinWidth(), expandIcon.getMinHeight());
    }

    protected void DrawIcon( TNode node, IDrawable icon, IBatch batch, float x, float y )
    {
        icon.draw(batch, x, y, icon.getMinWidth(), icon.getMinHeight());
    }

    /// <summary>
    /// Returns the drawable for the expand icon. The default implementation returns
    /// <see cref="Tree{T,V}.TreeStyle.PlusOver"/> or <see cref="Tree{T,V}.TreeStyle.MinusOver"/>
    /// on the desktop if the node is the over node, the mouse is left of iconX, and
    /// clicking would expand the node.
    /// </summary>
    protected IDrawable? GetExpandIcon( TNode node, float iconX )
    {
        boolean over = false;
        if (node == overNode                                                    //
         && Gdx.app.getType() == ApplicationType.Desktop                        //
         && (!selection.getMultiple() || (!UIUtils.ctrl() && !UIUtils.shift())) //
           ) {
            float mouseX                            = screenToLocalCoordinates(tmp.set(Gdx.input.getX(), 0)).x;
            if (mouseX >= 0 && mouseX < iconX) over = true;
        }
        if (over) {
            Drawable icon = node.expanded ? style.minusOver : style.plusOver;
            if (icon != null) return icon;
        }
        return node.expanded ? style.minus : style.plus;
    }

    public TNode? GetNodeAt( float y )
    {
        foundNode = null;
        getNodeAt(rootNodes, y, getHeight());
        return foundNode;
    }

    private float GetNodeAt( List< TNode > nodes, float y, float rowY )
    {
        for (int i = 0, n = nodes.size; i < n; i++) {
            N     node   = nodes.get(i);
            float height = node.height;
            rowY -= node.getHeight() - height; // Node subclass may increase getHeight.
            if (y >= rowY - height - ySpacing && y < rowY) {
                foundNode = node;
                return -1;
            }
            rowY -= height + ySpacing;
            if (node.expanded) {
                rowY = getNodeAt(node.children, y, rowY);
                if (rowY == -1) return -1;
            }
        }
        return rowY;
    }

    private void SelectNodes( List< TNode > nodes, float low, float high )
    {
        for (int i = 0, n = nodes.size; i < n; i++) {
            N node = nodes.get(i);
            if (node.actor.getY() < low) break;
            if (!node.isSelectable()) continue;
            if (node.actor.getY() <= high) selection.add(node);
            if (node.expanded) selectNodes(node.children, low, high);
        }
    }

    public Selection< TNode >? GetSelection()
    {
        return _selection;
    }

    /// <summary>
    /// Returns the first selected node, or null.
    /// </summary>
    public TNode? GetSelectedNode()
    {
        return _selection.First();
    }

    /// <summary>
    /// Returns the first selected value, or null.
    /// </summary>
    public TValue? GetSelectedValue()
    {
        return default( TValue? );
    }

    /// <summary>
    /// Updates the order of the actors in the tree for all root nodes and all
    /// child nodes.
    /// This is useful after changing the order of <see cref="RootNodes"/>.
    /// </summary>
    public void UpdateRootNodes()
    {
        for (int i = 0, n = rootNodes.size; i < n; i++) {
            N   node       = rootNodes.get(i);
            int actorIndex = node.actor.getZIndex();
            if (actorIndex != -1) node.removeFromTree(this, actorIndex);
        }
        for (int i = 0, n = rootNodes.size, actorIndex = 0; i < n; i++)
            actorIndex += rootNodes.get(i).addToTree(this, actorIndex);
    }

    public void FindExpandedValues( List< TValue > values )
    {
        findExpandedValues(rootNodes, values);
    }

    private static bool FindExpandedValues( List< TNode > nodes, List< TValue > values )
    {
        boolean expanded = false;
        for (int i = 0, n = nodes.size; i < n; i++) {
            Node node = nodes.get(i);
            if (node.expanded && !findExpandedValues(node.children, values)) values.add(node.value);
        }
        return expanded;
    }

    public void RestoreExpandedValues( List< TValue > values )
    {
        for (int i = 0, n = values.size; i < n; i++) {
            N node = findNode(values.get(i));
            if (node != null) {
                node.setExpanded(true);
                node.expandTo();
            }
        }
    }

    /// <summary>
    /// Returns the node with the specified value, or null.
    /// </summary>
    public TNode? FindNode( TValue value )
    {
        if (value == null) throw new IllegalArgumentException("value cannot be null.");
        return (N)findNode(rootNodes, value);
    }

    private static Node? FindNode<T>( List< T > nodes, object value )
        where T : Node
    {
        for (int i = 0, n = nodes.size; i < n; i++) {
            Node node = nodes.get(i);
            if (value.equals(node.value)) return node;
        }
        for (int i = 0, n = nodes.size; i < n; i++) {
            Node node  = nodes.get(i);
            Node found = findNode(node.children, value);
            if (found != null) return found;
        }
        return null;
    }

    public void CollapseAll()
    {
        collapseAll(rootNodes);
    }

    private static void CollapseAll<T>( List< T > nodes )
        where T : Node
    {
        for (int i = 0, n = nodes.size; i < n; i++) {
            Node node = nodes.get(i);
            node.setExpanded(false);
            collapseAll(node.children);
        }
    }

    public void ExpandAll()
    {
        ExpandAll( RootNodes );
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
    /// A <see cref="Tree{TNode,TValue}"/> node which has an actor and value.
    /// <para>
    /// A subclass can be used so the generic type parameters don't need
    /// to be specified repeatedly.
    /// </para>
    /// </summary>
    [PublicAPI]
    public class Node
    {
        public TValue?    Value      { get; set; }
        public TNode?     Parent     { get; set; }
        public IDrawable? Icon       { get; set; }
        public bool       Selectable { get; set; } = true;
        public float      Height     { get; set; }
        public bool       IsExpanded { get; private set; }

        private Actor? _actor;

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
            if ( expanded == this.IsExpanded )
            {
                return;
            }

            this.IsExpanded = expanded;

            Tree< TNode, TValue >? tree = GetTree();

            if ( ( tree == null ) || ( this.NodeChildren == null ) || ( this._actor == null ) )
            {
                return;
            }

            if ( this.NodeChildren.Count == 0 )
            {
                return;
            }

            TNode[] children   = this.NodeChildren.ToArray();
            var     actorIndex = this._actor.GetZIndex() + 1;

            if ( expanded )
            {
                for ( int i = 0, n = this.NodeChildren!.Count; i < n; i++ )
                {
                    actorIndex += children[ i ].AddToTree( tree, actorIndex );
                }
            }
            else
            {
                for ( int i = 0, n = this.NodeChildren!.Count; i < n; i++ )
                {
                    children?[ i ].RemoveFromTree( tree, actorIndex );
                }
            }
        }

        /// <summary>
        /// Called to add the actor to the tree when the node's parent is expanded.
        /// </summary>
        /// <returns> The number of node actors added to the tree. </returns>
        public int AddToTree( Tree< TNode, TValue > tree, int actorIndex )
        {
            tree.AddActorAt( actorIndex, _actor! );

            if ( !IsExpanded )
            {
                return 1;
            }

            var      childIndex = actorIndex + 1;
            TNode[]? children   = this.NodeChildren?.ToArray();

            for ( int i = 0, n = this.NodeChildren!.Count; i < n; i++ )
            {
                childIndex += children![ i ].AddToTree( tree, childIndex );
            }

            return childIndex - actorIndex;
        }

        /// <summary>
        /// Called to remove the actor from the tree, eg when the node is
        /// removed or the node's parent is collapsed.
        /// </summary>
        public void RemoveFromTree( Tree< TNode, TValue > tree, int actorIndex )
        {
            if ( !IsExpanded )
            {
                return;
            }

            TNode[]? children = this.NodeChildren?.ToArray();

            if ( children is null )
            {
                return;
            }

            for ( int i = 0, n = this.NodeChildren!.Count; i < n; i++ )
            {
                children[ i ].RemoveFromTree( tree, actorIndex );
            }
        }

        public void Add( TNode node )
        {
            Insert( this.NodeChildren!.Count, node );
        }

        public void AddAll( List< TNode > nodes )
        {
            for ( int i = 0, n = nodes.Count; i < n; i++ )
            {
                Insert( this.NodeChildren!.Count, nodes[ i ] );
            }
        }

        public void Insert( int childIndex, TNode node )
        {
            if ( NodeChildren == null )
            {
                return;
            }

            node.Parent = this.Parent;

            NodeChildren.Insert( childIndex, node );

            if ( !IsExpanded )
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
                else if ( childIndex < ( NodeChildren?.Count - 1 ) )
                {
                    actorIndex = NodeChildren![ childIndex + 1 ]._actor!.GetZIndex();
                }
                else
                {
                    TNode before = NodeChildren![ childIndex - 1 ];
                    actorIndex = before.Actor!.GetZIndex() + before.CountActors();
                }

                node.AddToTree( tree, actorIndex );
            }
        }

        public int CountActors()
        {
            if ( !IsExpanded )
            {
                return 1;
            }

            var count = 1;

            for ( int i = 0, n = this.NodeChildren!.Count; i < n; i++ )
            {
                count += this.NodeChildren[ i ].CountActors();
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
        /// Remove the specified child node from this node.
        /// Does nothing if the node is not a child of this node.
        /// </summary>
        public void Remove( TNode? node )
        {
            if ( ( node == null ) || ( NodeChildren == null ) )
            {
                return;
            }

            if ( !NodeChildren.Remove( node ) )
            {
                return;
            }

            if ( !IsExpanded )
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
            if ( IsExpanded )
            {
                Tree< TNode, TValue >? tree = GetTree();

                if ( ( tree != null ) && ( this.NodeChildren != null ) )
                {
                    var actorIndex = _actor!.GetZIndex() + 1;

                    for ( int i = 0, n = this.NodeChildren.Count; i < n; i++ )
                    {
                        this.NodeChildren[ i ].RemoveFromTree( tree, actorIndex );
                    }
                }
            }

            NodeChildren?.Clear();
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

        /// <summary>
        /// If the children order is changed, <see cref="UpdateChildren()"/> must
        /// be called to ensure the node's actors are in the correct order. That
        /// is not necessary if this node is not in the tree or is not expanded,
        /// because then the child node's actors are not in the tree.
        /// </summary>
        public List< TNode >? NodeChildren { get; set; } = new();

        public bool HasChildren() => NodeChildren?.Count > 0;

        /// <summary>
        /// Updates the order of the actors in the tree for this node and all child nodes.
        /// This is useful after changing the order. of <see cref="NodeChildren"/>.
        /// </summary>
        /// <seealso cref="Tree{TNode,TValue}.UpdateRootNodes()"/>
        public void UpdateChildren()
        {
            if ( !IsExpanded )
            {
                return;
            }

            Tree< TNode, TValue >? tree = GetTree();

            if ( tree == null )
            {
                return;
            }

            TNode[]? children   = this.NodeChildren?.ToArray();
            var      n          = this.NodeChildren!.Count;
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

            return ( TNode? )Tree< TNode, TValue >.FindNode( this.NodeChildren!, value );
        }

        /// <summary>
        /// Collapses all nodes under and including this node.
        /// </summary>
        public void CollapseAll()
        {
            SetExpanded( false );
            Tree< TNode, TValue >.CollapseAll( this.NodeChildren! );
        }

        /// <summary>
        /// Expands all nodes under and including this node.
        /// </summary>
        public void ExpandAll()
        {
            SetExpanded( true );

            if ( NodeChildren?.Count > 0 )
            {
                Tree< TNode, TValue >.ExpandAll( NodeChildren );
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
            if ( IsExpanded
              && !Tree< TNode, TValue >.FindExpandedValues( this.NodeChildren!, values ) )
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
            return Height;
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
