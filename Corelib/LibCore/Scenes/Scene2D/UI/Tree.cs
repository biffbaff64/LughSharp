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
using Corelib.LibCore.Input;
using Corelib.LibCore.Scenes.Scene2D.Listeners;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils.Exceptions;
using Platform = Corelib.LibCore.Core.Platform;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

/// <summary>
/// A tree widget where each node has an icon, actor, and child nodes.
/// The preferred size of the tree is determined by the preferred size
/// of the actors for the expanded nodes.
/// </summary>
/// <typeparam name="TNode"> The type of nodes in the tree. </typeparam>
/// <typeparam name="TValue"> The type of values for each node. </typeparam>
[PublicAPI]
public class Tree< TNode, TValue > : WidgetGroup where TNode : Tree< TNode, TValue >.Node
{
    public TNode?         RangeStart    { get; set; }
    public ClickListener? ClickListener { get; set; }
    public TreeStyle?     Style         { get; set; }
    public List< TNode >  RootNodes     { get; set; } = [ ];
    public float          YSpacing      { get; set; } = 4;
    public float          IndentSpacing { get; set; }
    public TNode?         OverNode      { get; set; }

    // ========================================================================

    private readonly TreeSelection _selection;
    private readonly Vector2       _tmp = new();
    private          TNode?        _foundNode;
    private          float         _iconSpacingLeft  = 2;
    private          float         _iconSpacingRight = 2;
    private          float         _paddingLeft;
    private          float         _paddingRight;
    private          float         _prefHeight;
    private          float         _prefWidth;
    private          bool          _sizeInvalid = true;

    // ========================================================================

    /// <summary>
    /// Construct a new Tree using the supplied <see cref="Skin"/>
    /// and a default <see cref="TreeStyle"/> from that skin.
    /// </summary>
    public Tree( Skin skin ) : this( skin.Get< TreeStyle >() )
    {
    }

    /// <summary>
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
            Multiple = true,
        };

        SetStyle( style );
        Initialise();
    }

    private void Initialise()
    {
        ClickListener = new TreeClickListener();

        AddListener( ClickListener );
    }

    public void SetStyle( TreeStyle style )
    {
        Style = style;

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

        //TODO: Remove the need for the following two null suppressions

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
            var before = RootNodes[ index - 1 ];

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

        _prefWidth += _paddingLeft + _paddingRight;
    }

    private void ComputeSize( List< TNode > nodes, float indent, float plusMinusWidth )
    {
        var ySpacing = YSpacing;
        var spacing  = _iconSpacingLeft + _iconSpacingRight;

        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            var node     = nodes[ i ];
            var rowWidth = indent + plusMinusWidth;
            var actor    = node.Actor;

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

    public override void SetLayout()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        Layout( RootNodes, _paddingLeft, Height - ( YSpacing / 2 ), PlusMinusWidth() );
    }

    private float Layout( List< TNode > nodes, float indent, float y, float plusMinusWidth )
    {
        var ySpacing        = YSpacing;
        var iconSpacingLeft = _iconSpacingLeft;
        var spacing         = iconSpacingLeft + _iconSpacingRight;

        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            var node = nodes[ i ];
            var x    = indent + plusMinusWidth;

            if ( node.Icon != null )
            {
                x += spacing + node.Icon.MinWidth;
            }
            else
            {
                x += iconSpacingLeft;
            }

            if ( node.Actor is ILayout layout )
            {
                layout.Pack();
            }

            y -= node.Height;

            node.Actor!.SetPosition( x, y );

            y -= ySpacing;

            if ( node.IsExpanded )
            {
                y = Layout( node.NodeChildren!, indent + IndentSpacing, y, plusMinusWidth );
            }
        }

        return y;
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        DrawBackground( batch, parentAlpha );

        batch.SetColor( Color.R, Color.G, Color.B, Color.A * parentAlpha );

        Draw( batch, RootNodes, _paddingLeft, PlusMinusWidth() );

        // Draw node actors.
        base.Draw( batch, parentAlpha );
    }

    /// <summary>
    /// Called to draw the background.
    /// Default implementation draws the style background drawable.
    /// </summary>
    protected void DrawBackground( IBatch batch, float parentAlpha )
    {
        if ( Style?.Background != null )
        {
            batch.SetColor( Color.R, Color.G, Color.B, Color.A * parentAlpha );

            Style.Background.Draw( batch, X, Y, Width, Height );
        }
    }

    private void Draw( IBatch batch, List< TNode > nodes, float indent, float plusMinusWidth )
    {
        var   cullingArea = CullingArea;
        float cullBottom  = 0;
        float cullTop     = 0;

        if ( cullingArea != null )
        {
            cullBottom = cullingArea.Y;
            cullTop    = cullBottom + cullingArea.Height;
        }

        var style = Style;

        var x       = X;
        var y       = Y;
        var expandX = x + indent;
        var iconX   = expandX + plusMinusWidth + _iconSpacingLeft;

        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            var node   = nodes[ i ];
            var actor  = node.Actor ?? throw new GdxRuntimeException( "node.Actor cannot be null!" );
            var actorY = actor.Y;
            var height = node.Height;

            if ( ( cullingArea == null ) || ( ( ( actorY + height ) >= cullBottom ) && ( actorY <= cullTop ) ) )
            {
                if ( _selection.Contains( node ) && ( style?.Selection != null ) )
                {
                    DrawSelection( node,
                                   style.Selection,
                                   batch,
                                   x,
                                   ( y + actorY ) - ( YSpacing / 2 ),
                                   Width,
                                   height + YSpacing );
                }
                else if ( ( node == OverNode ) && ( style?.Over != null ) )
                {
                    DrawOver( node, style.Over, batch, x, ( y + actorY ) - ( YSpacing / 2 ), Width, height + YSpacing );
                }

                if ( node.Icon != null )
                {
                    var iconY = y + actorY + Math.Round( ( height - node.Icon.MinHeight ) / 2 );

                    batch.Color = actor.Color;
                    DrawIcon( node, node.Icon, batch, iconX, ( float ) iconY );
                    batch.SetColor( 1, 1, 1, 1 );
                }

                if ( node.NodeChildren?.Count > 0 )
                {
                    var expandIcon = GetExpandIcon( node, iconX );
                    var iconY      = y + actorY + Math.Round( ( height - expandIcon.MinHeight ) / 2 );

                    DrawExpandIcon( node, expandIcon, batch, expandX, ( float ) iconY );
                }
            }
            else if ( actorY < cullBottom )
            {
                return;
            }

            if ( node is { IsExpanded: true, NodeChildren.Count: > 0 } )
            {
                //TODO: Refactor to remove recursiveness 
                Draw( batch, node.NodeChildren, indent + IndentSpacing, plusMinusWidth );
            }
        }
    }

    protected void DrawSelection( TNode node, IDrawable selection, IBatch batch, float x, float y, float width, float height )
    {
        selection.Draw( batch, x, y, width, height );
    }

    protected void DrawOver( TNode node, IDrawable over, IBatch batch, float x, float y, float width, float height )
    {
        over.Draw( batch, x, y, width, height );
    }

    protected void DrawExpandIcon( TNode node, IDrawable expandIcon, IBatch batch, float x, float y )
    {
        expandIcon.Draw( batch, x, y, expandIcon.MinWidth, expandIcon.MinHeight );
    }

    protected void DrawIcon( TNode node, IDrawable icon, IBatch batch, float x, float y )
    {
        icon.Draw( batch, x, y, icon.MinWidth, icon.MinHeight );
    }

    /// <summary>
    /// Returns the drawable for the expand icon. The default implementation returns
    /// <see cref="Tree{T,V}.TreeStyle.PlusOver"/> or <see cref="Tree{T,V}.TreeStyle.MinusOver"/>
    /// on the desktop if the node is the over node, the mouse is left of iconX, and
    /// clicking would expand the node.
    /// </summary>
    protected IDrawable GetExpandIcon( TNode node, float iconX )
    {
        var over = false;

        if ( ( node == OverNode )
          && ( Gdx.App.AppType == Platform.ApplicationType.WindowsGL )
          && ( !_selection.Multiple || ( !InputUtils.CtrlKey() && !InputUtils.ShiftKey() ) ) )
        {
            var mouseX = ScreenToLocalCoordinates( _tmp.Set( Gdx.Input.GetX(), 0 ) ).X;

            if ( ( mouseX >= 0 ) && ( mouseX < iconX ) )
            {
                over = true;
            }
        }

        if ( Style == null )
        {
            throw new GdxRuntimeException( "Style is NULL!" );
        }

        if ( over )
        {
            var icon = node.IsExpanded ? Style.MinusOver : Style.PlusOver;

            if ( icon != null )
            {
                return icon;
            }
        }

        return node.IsExpanded ? Style.Minus : Style.Plus;
    }

    public TNode? GetNodeAt( float y )
    {
        _foundNode = null;
        GetNodeAt( RootNodes, y, Height );

        return _foundNode;
    }

    private float GetNodeAt( List< TNode > nodes, float y, float rowY )
    {
        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            var node   = nodes[ i ];
            var height = node.Height;

            rowY -= node.Height - height; // Node subclass may increase getHeight.

            if ( ( y >= ( rowY - height - YSpacing ) ) && ( y < rowY ) )
            {
                _foundNode = node;

                return -1;
            }

            rowY -= height + YSpacing;

            if ( node.IsExpanded )
            {
                //TODO: Refactor to remove recursiveness 
                rowY = GetNodeAt( node.NodeChildren!, y, rowY );

                if ( Math.Abs( rowY - -1 ) < 0.1f )
                {
                    return -1;
                }
            }
        }

        return rowY;
    }

    private void SelectNodes( List< TNode > nodes, float low, float high )
    {
        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            var node = nodes[ i ];

            if ( node.Actor?.Y < low )
            {
                break;
            }

            if ( !node.Selectable )
            {
                continue;
            }

            if ( node.Actor?.Y <= high )
            {
                _selection.Add( node );
            }

            if ( node.IsExpanded )
            {
                //TODO: Refactor to remove recursiveness 
                SelectNodes( node.NodeChildren!, low, high );
            }
        }
    }

    public Selection< TNode > GetSelection()
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
        for ( int i = 0, n = RootNodes.Count; i < n; i++ )
        {
            var node       = RootNodes[ i ];
            var actorIndex = node.Actor!.GetZIndex();

            if ( actorIndex != -1 )
            {
                node.RemoveFromTree( this, actorIndex );
            }
        }

        for ( int i = 0, n = RootNodes.Count, actorIndex = 0; i < n; i++ )
        {
            actorIndex += RootNodes[ i ].AddToTree( this, actorIndex );
        }
    }

    public void FindExpandedValues( List< TValue > values )
    {
        FindExpandedValues( RootNodes, values );
    }

    private static bool FindExpandedValues( List< TNode > nodes, List< TValue > values )
    {
        var expanded = false;

        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            Node node = nodes[ i ];

            if ( ( node.NodeChildren != null ) && ( node.Value != null ) )
            {
                //TODO: Refactor to remove recursiveness 
                if ( node.IsExpanded && !FindExpandedValues( node.NodeChildren, values ) )
                {
                    values.Add( node.Value );

                    expanded = true;
                }
            }
        }

        return expanded;
    }

    public void RestoreExpandedValues( List< TValue > values )
    {
        for ( int i = 0, n = values.Count; i < n; i++ )
        {
            var node = FindNode( values[ i ] );

            if ( node != null )
            {
                node.SetExpanded( true );
                node.ExpandTo();
            }
        }
    }

    /// <summary>
    /// Returns the node with the specified value, or null.
    /// </summary>
    public TNode? FindNode( TValue value )
    {
        ArgumentNullException.ThrowIfNull( value );

        return ( TNode? ) FindNode( RootNodes, value );
    }

    private static Node? FindNode< T >( List< T > nodes, object value ) where T : Node
    {
        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            if ( value.Equals( nodes[ i ].Value ) )
            {
                return nodes[ i ];
            }
        }

        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            //TODO: Refactor to remove recursiveness 
            var found = FindNode( nodes[ i ].NodeChildren!, value );

            if ( found != null )
            {
                return found;
            }
        }

        return null;
    }

    public void CollapseAll()
    {
        CollapseAll( RootNodes );
    }

    private static void CollapseAll< T >( List< T > nodes ) where T : Node
    {
        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            nodes[ i ].SetExpanded( false );

            //TODO: Refactor to remove recursiveness 
            CollapseAll( nodes[ i ].NodeChildren! );
        }
    }

    public void ExpandAll()
    {
        ExpandAll( RootNodes );
    }

    private static void ExpandAll< T >( List< T > nodes ) where T : Node
    {
        for ( int i = 0, n = nodes.Count; i < n; i++ )
        {
            nodes[ i ].ExpandAll();
        }
    }

    // ========================================================================

    public TValue? GetOverValue()
    {
        return OverNode == null ? default( TValue? ) : OverNode.Value;
    }

    public void SetPadding( float padding )
    {
        _paddingLeft  = padding;
        _paddingRight = padding;
    }

    public void SetPadding( float left, float right )
    {
        _paddingLeft  = left;
        _paddingRight = right;
    }

    public void SetIconSpacing( float left, float right )
    {
        _iconSpacingLeft  = left;
        _iconSpacingRight = right;
    }

    /// <summary>
    /// Gets the preferred width of this tree.
    /// </summary>
    public float GetPrefWidth()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _prefWidth;
    }

    /// <summary>
    /// Gets the preferred height of this tree.
    /// </summary>
    public float GetPrefHeight()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _prefHeight;
    }

    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    [PublicAPI]
    public class TreeSelection : Selection< TNode >
    {
        private readonly Tree< TNode, TValue > _parent;

        public TreeSelection( Tree< TNode, TValue > p )
        {
            _parent = p;
        }

        /// <inheritdoc />
        protected override void Changed()
        {
            _parent.RangeStart = Size() switch
            {
                0     => default( TNode ),
                1     => First(),
                var _ => _parent.RangeStart,
            };
        }
    }

// ========================================================================
// ========================================================================

    /// <summary>
    /// The style for a <see cref="Tree{TN,TV}"/>.
    /// </summary>
    [PublicAPI]
    public class TreeStyle
    {
        public TreeStyle( IDrawable plus, IDrawable minus, IDrawable? selection )
        {
            Plus      = plus;
            Minus     = minus;
            Selection = selection;
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

        public IDrawable  Plus       { get; set; }
        public IDrawable  Minus      { get; set; }
        public IDrawable? PlusOver   { get; set; } = null;
        public IDrawable? MinusOver  { get; set; } = null;
        public IDrawable? Over       { get; set; } = null;
        public IDrawable? Selection  { get; set; } = null;
        public IDrawable? Background { get; set; } = null;
    }

    // ========================================================================
    // ========================================================================

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

            _actor = actor;
        }

        public TValue?    Value      { get; set; }
        public TNode?     Parent     { get; set; }
        public IDrawable? Icon       { get; set; }
        public bool       Selectable { get; set; } = true;
        public float      Height     { get; set; }
        public bool       IsExpanded { get; private set; }

        /// <summary>
        /// If the children order is changed, <see cref="UpdateChildren()"/> must
        /// be called to ensure the node's actors are in the correct order. That
        /// is not necessary if this node is not in the tree or is not expanded,
        /// because then the child node's actors are not in the tree.
        /// </summary>
        public List< TNode >? NodeChildren { get; set; } = [ ];

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

        public void SetExpanded( bool expanded )
        {
            if ( expanded == IsExpanded )
            {
                return;
            }

            IsExpanded = expanded;

            Tree< TNode, TValue >? tree = GetTree();

            if ( ( tree == null ) || ( NodeChildren == null ) || ( _actor == null ) )
            {
                return;
            }

            if ( NodeChildren.Count == 0 )
            {
                return;
            }

            TNode[] children   = NodeChildren.ToArray();
            var     actorIndex = _actor.GetZIndex() + 1;

            if ( expanded )
            {
                for ( int i = 0, n = NodeChildren.Count; i < n; i++ )
                {
                    actorIndex += children[ i ].AddToTree( tree, actorIndex );
                }
            }
            else
            {
                for ( int i = 0, n = NodeChildren.Count; i < n; i++ )
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

            var     childIndex = actorIndex + 1;
            TNode[] children   = NodeChildren!.ToArray();

            for ( int i = 0, n = children.Length; i < n; i++ )
            {
                childIndex += children[ i ].AddToTree( tree, childIndex );
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

            TNode[]? children = NodeChildren?.ToArray();

            if ( children == null )
            {
                return;
            }

            for ( int i = 0, n = NodeChildren!.Count; i < n; i++ )
            {
                children[ i ].RemoveFromTree( tree, actorIndex );
            }
        }

        public void Add( TNode node )
        {
            GdxRuntimeException.ThrowIfNull( NodeChildren );

            Insert( NodeChildren.Count, node );
        }

        public void AddAll( List< TNode > nodes )
        {
            GdxRuntimeException.ThrowIfNull( NodeChildren );

            for ( int i = 0, n = nodes.Count; i < n; i++ )
            {
                Insert( NodeChildren.Count, nodes[ i ] );
            }
        }

        /// <summary>
        /// Inserts the supplied node into the <see cref="NodeChildren"/> list.
        /// </summary>
        /// <param name="childIndex"></param>
        /// <param name="node"></param>
        public void Insert( int childIndex, TNode node )
        {
            if ( ( NodeChildren == null ) || ( _actor == null ) )
            {
                return;
            }

            node.Parent = Parent;

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
                    actorIndex = _actor.GetZIndex() + 1;
                }
                else if ( childIndex < ( NodeChildren.Count - 1 ) )
                {
                    actorIndex = NodeChildren[ childIndex + 1 ]._actor!.GetZIndex();
                }
                else
                {
                    var before = NodeChildren[ childIndex - 1 ];
                    actorIndex = before.Actor!.GetZIndex() + before.CountActors();
                }

                node.AddToTree( tree, actorIndex );
            }
        }

        /// <summary>
        /// Return the current count of actors held in <see cref="NodeChildren"/>.
        /// If this node is not expanded, a count of 1 is returned by default.
        /// </summary>
        public int CountActors()
        {
            GdxRuntimeException.ThrowIfNull( NodeChildren );

            if ( !IsExpanded )
            {
                return 1;
            }

            var actorCount = 1;

            for ( int i = 0, n = NodeChildren.Count; i < n; i++ )
            {
                actorCount += NodeChildren[ i ].CountActors();
            }

            return actorCount;
        }

        /// <summary>
        /// Remove this node from its parent.
        /// </summary>
        public void Remove()
        {
            Tree< TNode, TValue >? tree = GetTree();

            if ( tree != null )
            {
                tree.Remove( Parent! );
            }
            else
            {
                Parent?.Remove( Parent! );
            }
        }

        /// <summary>
        /// Remove the specified child node from this node.
        /// Does nothing if the node is not a child of this node.
        /// </summary>
        public void Remove( TNode? node )
        {
            if ( ( node == null )
              || ( NodeChildren == null )
              || !NodeChildren.Remove( node )
              || !IsExpanded )
            {
                return;
            }

            Tree< TNode, TValue >? tree = GetTree();

            if ( ( tree == null ) || ( node.Actor == null ) )
            {
                return;
            }

            node.RemoveFromTree( tree, node.Actor.GetZIndex() );
        }

        /// <summary>
        /// Removes all children from this node.
        /// </summary>
        public void ClearChildren()
        {
            if ( IsExpanded && ( _actor != null ) )
            {
                Tree< TNode, TValue >? tree = GetTree();

                if ( ( tree != null ) && ( NodeChildren != null ) )
                {
                    var actorIndex = _actor.GetZIndex() + 1;

                    for ( int i = 0, n = NodeChildren.Count; i < n; i++ )
                    {
                        NodeChildren[ i ].RemoveFromTree( tree, actorIndex );
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

        public bool HasChildren()
        {
            return NodeChildren?.Count > 0;
        }

        /// <summary>
        /// Updates the order of the actors in the tree for this node and all child nodes.
        /// This is useful after changing the order. of <see cref="NodeChildren"/>.
        /// </summary>
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

            TNode[]? children   = NodeChildren?.ToArray();
            var      n          = NodeChildren?.Count;
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
            var level   = 0;
            var current = this;

            do
            {
                level++;
                current = current.Parent;
            } while ( current != null );

            return level;
        }

        /// <summary>
        /// Returns this node or the child node with the specified value, or null.
        /// </summary>
        public TNode? FindNode( TValue? value )
        {
            ArgumentNullException.ThrowIfNull( value );

            if ( value.Equals( Value ) )
            {
                return ( TNode ) this;
            }

            return ( TNode? ) Tree< TNode, TValue >.FindNode( NodeChildren!, value );
        }

        /// <summary>
        /// Collapses all nodes under and including this node.
        /// </summary>
        public void CollapseAll()
        {
            SetExpanded( false );
            Tree< TNode, TValue >.CollapseAll( NodeChildren! );
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
            var node = Parent;

            while ( node != null )
            {
                node.SetExpanded( true );
                node = node.Parent;
            }
        }

        public void FindExpandedValues( List< TValue > values )
        {
            if ( IsExpanded
              && !Tree< TNode, TValue >.FindExpandedValues( NodeChildren!, values ) )
            {
                values.Add( Value! );
            }
        }

        public void RestoreExpandedValues( List< TValue > values )
        {
            for ( int i = 0, n = values.Count; i < n; i++ )
            {
                var node = FindNode( values[ i ] );

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

            var current = node;

            do
            {
                if ( current == this )
                {
                    return true;
                }

                current = current.Parent;
            } while ( current != null );

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

            var parent = this;

            do
            {
                if ( parent == node )
                {
                    return true;
                }

                parent = parent.Parent;
            } while ( parent != null );

            return false;
        }
    }

    // ========================================================================
    // ========================================================================

    public class TreeClickListener : ClickListener
    {
        public readonly Tree< TNode, TValue > Tree = null!;

        public override void OnClicked( InputEvent ev, float x, float y )
        {
            var node = Tree.GetNodeAt( y );

            if ( node == null )
            {
                return;
            }

            if ( node != Tree.GetNodeAt( TouchDownY ) )
            {
                return;
            }

            if ( Tree._selection.Multiple && Tree._selection.NotEmpty() && InputUtils.ShiftKey() )
            {
                // Select range (shift).
                Tree.RangeStart ??= node;

                var rangeStart = Tree.RangeStart;

                if ( !InputUtils.CtrlKey() )
                {
                    Tree._selection.Clear();
                }

                if ( ( rangeStart.Actor == null ) || ( node.Actor == null ) )
                {
                    return;
                }

                var start = rangeStart.Actor.Y;
                var end   = node.Actor.Y;

                if ( start > end )
                {
                    Tree.SelectNodes( Tree.RootNodes, end, start );
                }
                else
                {
                    Tree.SelectNodes( Tree.RootNodes, start, end );
                    Tree._selection.Items().Reverse();
                }

                Tree._selection.FireChangeEvent();
                Tree.RangeStart = rangeStart;

                return;
            }

            if ( ( node.NodeChildren?.Count > 0 ) && ( !Tree._selection.Multiple || !InputUtils.CtrlKey() ) )
            {
                // Toggle expanded if left of icon.
                var rowX = node.Actor?.X;

                if ( node.Icon != null )
                {
                    rowX -= Tree._iconSpacingRight + node.Icon.MinWidth;
                }

                if ( x < rowX )
                {
                    node.SetExpanded( !node.IsExpanded );

                    return;
                }
            }

            if ( !node.Selectable )
            {
                return;
            }

            Tree._selection.Choose( node );

            if ( !Tree._selection.IsEmpty )
            {
                Tree.RangeStart = node;
            }
        }

        public override bool MouseMoved( InputEvent? ev, float x, float y )
        {
            Tree.OverNode = Tree.GetNodeAt( y );

            return false;
        }

        public override void Enter( InputEvent? ev, float x, float y, int pointer, Actor? fromActor )
        {
            base.Enter( ev, x, y, pointer, fromActor );
            Tree.OverNode = Tree.GetNodeAt( y );
        }

        public override void Exit( InputEvent? ev, float x, float y, int pointer, Actor? toActor )
        {
            base.Exit( ev, x, y, pointer, toActor );

            if ( ( toActor == null ) || !toActor.IsDescendantOf( Tree ) )
            {
                Tree.OverNode = null;
            }
        }
    }
}