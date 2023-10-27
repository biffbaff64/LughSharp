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

using NotImplementedException = System.NotImplementedException;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[PublicAPI]
public class Tree<TN, TV> : WidgetGroup where TN : Tree< TN, TV >.Node< TN, TV, Actor >
{
    private readonly Vector2 _tmp = new();

    private TreeStyle?      _style;
    private List< TN >      _rootNodes = new();
    private Selection< TN > _selection;
    private float           _ySpacing         = 4;
    private float           _iconSpacingLeft  = 2;
    private float           _iconSpacingRight = 2;
    private float           _paddingLeft;
    private float           _paddingRight;
    private float           _indentSpacing;
    private float           _prefWidth;
    private float           _prefHeight;
    private bool            _sizeInvalid = true;
    private TN?             _foundNode;
    private TN?             _overNode;
    private TN?             _rangeStart;
    private ClickListener   _clickListener;

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
        _selection = new Selection< TN >
        {
            Actor    = this,
            Multiple = true
        };

        SetStyle( style );
    }

    private void Initialise()
    {
    }


    public void SetStyle( TreeStyle style )
    {
        this._style = style;

        // Reasonable default.
        if ( _indentSpacing == 0 )
        {
            _indentSpacing = PlusMinusWidth();
        }
    }

    public void Add( TN node )
    {
        Insert( _rootNodes.Count, node );
    }

    public void Insert( int index, TN node )
    {
    }

    public void Remove( TN node )
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

    private void ComputeSize( List< TN > nodes, float indent, float plusMinusWidth )
    {
    }

    public override void Layout()
    {
    }

    private float Layout( List< TN > nodes, float indent, float y, float plusMinusWidth )
    {
        return 0f;
    }

    protected override void Draw( IBatch batch, float parentAlpha )
    {
    }

    protected void DrawBackground( IBatch batch, float parentAlpha )
    {
    }

    private void Draw( IBatch batch, List< TN > nodes, float indent, float plusMinusWidth )
    {
    }

    protected void DrawSelection( TN node, IDrawable selection, IBatch batch, float x, float y, float width, float height )
    {
    }

    protected void DrawOver( TN node, IDrawable over, IBatch batch, float x, float y, float width, float height )
    {
    }

    protected void DrawExpandIcon( TN node, IDrawable expandIcon, IBatch batch, float x, float y )
    {
    }

    protected void DrawIcon( TN node, IDrawable icon, IBatch batch, float x, float y )
    {
    }

    protected IDrawable? GetExpandIcon( TN node, float iconX )
    {
        return null;
    }

    public TN? GetNodeAt( float y )
    {
        return null;
    }

    private float GetNodeAt( List< TN > nodes, float y, float rowY )
    {
        return 0f;
    }

    private void SelectNodes( List< TN > nodes, float low, float high )
    {
    }

    public Selection< TN >? GetSelection()
    {
        return null;
    }

    public TN? GetSelectedNode()
    {
        return null;
    }

    public TV? GetSelectedValue()
    {
        return default( TV? );
    }

    public List< TN >? GetRootNodes()
    {
        return null;
    }

    public List< TN >? GetNodes()
    {
        return null;
    }

    public void UpdateRootNodes()
    {
    }

    public void FindExpandedValues( List< TV > values )
    {
    }

    public void RestoreExpandedValues( List< TV > values )
    {
    }

    private static bool FindExpandedValues( List<? extends Tree< , >.Node< ,, > > nodes, List Values)
    {
    }

    public TN? FindNode( TV value )
    {
        return null;
    }

    private static Node? FindNode( List<? extends Node > nodes, Object Value )
    {
    }

    public void CollapseAll()
    {
    }

    private static void CollapseAll<T>( List< T > nodes ) where T : Node< TN, TV, Actor >
    {
    }

    public void ExpandAll()
    {
    }

    private static void ExpandAll<T>( List< T > nodes ) where T : Node< TN, TV, Actor >
    {
    }

    // ------------------------------------------------------------------------

    public Tree< TN, TV >.TreeStyle? GetStyle()
    {
        return _style;
    }
    
    public TN? GetOverNode()
    {
        return null;
    }

    public TV? GetOverValue()
    {
        return default( TV? );
    }

    public void SetOverNode( TN? overNode )
    {
    }

    public void SetPadding( float padding )
    {
    }

    public void SetPadding( float left, float right )
    {
    }

    public void SetIndentSpacing( float indentSpacing )
    {
    }

    public float GetIndentSpacing()
    {
        return 0f;
    }

    public void SetYSpacing( float ySpacing )
    {
    }

    public float GetYSpacing()
    {
        return 0f;
    }

    public void SetIconSpacing( float left, float right )
    {
    }

    public float GetPrefWidth()
    {
        return 0f;
    }

    public float GetPrefHeight()
    {
        return 0f;
    }

    public ClickListener? GetClickListener()
    {
        return _clickListener;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class TreeStyle
    {
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
    /// <typeparam name="TNn"> The type for the node's parent and child nodes. </typeparam>
    /// <typeparam name="TVn"> The type for the node's value. </typeparam>
    /// <typeparam name="TAn"> The type for the node's actor. </typeparam>
    [PublicAPI]
    public class Node<TNn, TVn, TAn> where TNn : Node< TNn, TVn, TAn > where TAn : Actor
    {
    }
}
