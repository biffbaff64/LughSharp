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

using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.Listeners;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;
using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

/// <summary>
/// A select box (aka a drop-down list) allows a user to choose one of a number of values
/// from a list. When inactive, the selected value is displayed. When activated, it shows
/// the list of values that may be selected.
/// <para>
/// <see cref="ChangeListener.ChangeEvent"/> is fired when the selectbox selection changes.
/// </para>
/// <para>
/// The preferred size of the select box is determined by the maximum text bounds of the items and the size of the
/// {@link SelectBoxStyle#background}.
/// </para>
/// </summary>
[PublicAPI]
public class SelectBox< T > : Widget, IDisableable
{
    public ClickListener  ClickListener { get; set; }
    public SelectBoxStyle BoxStyle      { get; set; }
    public bool           IsDisabled    { get; set; }

    // ========================================================================
    
    private readonly List< T >           _items;
    private readonly SelectBoxList?      _selectBoxList;
    private readonly ArraySelection< T > _selection;
    private readonly Vector2             _temp = new();

    private int   _alignment;
    private float _prefHeight;
    private float _prefWidth;
    private bool  _selectedPrefWidth;

    // ========================================================================

    public SelectBox( Skin skin ) : this( skin.Get< SelectBoxStyle >() )
    {
    }

    public SelectBox( Skin skin, string styleName )
        : this( skin.Get< SelectBoxStyle >( styleName ) )
    {
    }

    public SelectBox( SelectBoxStyle style )
    {
        _alignment = Align.LEFT;
        _items     = [ ];

        _selection = new ArraySelection< T >( _items )
        {
            Actor    = this,
            Required = true,
        };

        _selectBoxList = new SelectBoxList( this );
        ClickListener  = new SelectBoxClickListener( this );
        BoxStyle       = null!;

        Setup( style );
    }

    /// <summary>
    /// Set the max number of items to display when the select box is opened. Set to
    /// 0 (the default) to display as many as fit in the stage height.
    /// </summary>
    public int MaxListCount
    {
        get => _selectBoxList!.MaxListCount;
        set => _selectBoxList!.MaxListCount = value;
    }

    public override float PrefWidth
    {
        get
        {
            Validate();

            return _prefWidth;
        }
    }

    public override float PrefHeight
    {
        get
        {
            Validate();

            return _prefHeight;
        }
    }

    private void Setup( SelectBoxStyle style )
    {
        SetStyle( style );
        SetSize( PrefWidth, PrefHeight );
    }

    protected void SetStage( Stage? stage )
    {
        if ( stage == null )
        {
            _selectBoxList?.Hide();
        }

        Stage = stage;
    }

    public void SetStyle( SelectBoxStyle style )
    {
        ArgumentNullException.ThrowIfNull( style );

        BoxStyle = style;

        if ( _selectBoxList != null )
        {
            _selectBoxList.SetStyle( style.ScrollStyle );
            _selectBoxList.ListBox.Style = style.ListStyle;
        }

        InvalidateHierarchy();
    }

    /// <summary>
    /// Set the backing List that makes up the choices available in the SelectBox
    /// </summary>
    public void SetItems( params T[] newItems )
    {
        ArgumentNullException.ThrowIfNull( newItems );

        var oldPrefWidth = PrefWidth;

        _items.Clear();
        _items.AddAll( newItems );
        _selection.Validate();
        _selectBoxList?.ListBox.SetItems( _items );

        Invalidate();

        if ( oldPrefWidth.Equals( PrefWidth ) )
        {
            InvalidateHierarchy();
        }
    }

    /// <summary>
    /// Sets the items visible in the select box.
    /// </summary>
    public void SetItems( List< T > newItems )
    {
        ArgumentNullException.ThrowIfNull( newItems );

        var oldPrefWidth = PrefWidth;

        if ( newItems != _items )
        {
            _items.Clear();
            _items.AddAll( newItems );
        }

        _selection.Validate();
        _selectBoxList?.ListBox.SetItems( _items );

        Invalidate();

        if ( oldPrefWidth.Equals( PrefWidth ) )
        {
            InvalidateHierarchy();
        }
    }

    public void ClearItems()
    {
        if ( _items.Count == 0 )
        {
            return;
        }

        _items.Clear();
        _selection.Clear();
        InvalidateHierarchy();
    }

    /// <summary>
    /// Returns the internal items array.
    /// </summary>
    public List< T > GetItems()
    {
        return _items;
    }

    public void Layout()
    {
        var bg   = BoxStyle.Background;
        var font = BoxStyle.Font;

        if ( bg != null )
        {
            _prefHeight = Math.Max( ( bg.TopHeight + bg.BottomHeight + font.GetCapHeight() )
                                  - ( font.GetDescent() * 2 ),
                                    bg.MinHeight );
        }
        else
        {
            _prefHeight = font.GetCapHeight() - ( font.GetDescent() * 2 );
        }

        Pool< GlyphLayout > layoutPool = Pools< GlyphLayout >.Get();
        var                 layout     = layoutPool.Obtain();

        if ( _selectedPrefWidth )
        {
            _prefWidth = 0;

            if ( bg != null )
            {
                _prefWidth = bg.LeftWidth + bg.RightWidth;
            }

            var selected = GetSelected();

            if ( selected != null )
            {
                layout?.SetText( font, ToString( selected ) ?? "" );
                _prefWidth += layout!.Width;
            }
        }
        else
        {
            float maxItemWidth = 0;

            foreach ( var t in _items )
            {
                layout?.SetText( font, ToString( t ) ?? "" );
                maxItemWidth = Math.Max( layout!.Width, maxItemWidth );
            }

            _prefWidth = maxItemWidth;

            if ( bg != null )
            {
                _prefWidth = Math.Max( _prefWidth + bg.LeftWidth + bg.RightWidth, bg.MinWidth );
            }

            var listStyle   = BoxStyle.ListStyle;
            var scrollStyle = BoxStyle.ScrollStyle;

            var listWidth = maxItemWidth + listStyle.Selection?.LeftWidth + listStyle.Selection?.RightWidth;

            bg = scrollStyle.Background;

            if ( bg != null )
            {
                listWidth = Math.Max( ( float ) ( listWidth + bg.LeftWidth + bg.RightWidth )!, bg.MinWidth );
            }

            if ( _selectBoxList is not { DisableYScroll: true } )
            {
                listWidth += Math.Max( BoxStyle.ScrollStyle.VScroll?.MinWidth ?? 0,
                                       BoxStyle.ScrollStyle.VScrollKnob?.MinWidth ?? 0 );
            }

            _prefWidth = Math.Max( _prefWidth, ( float ) listWidth! );
        }

        layoutPool.Free( layout! );
    }

    /// <summary>
    /// Returns appropriate background Drawable from the style
    /// based on the current select box state.
    /// </summary>
    protected IDrawable? GetBackgroundIDrawable()
    {
        if ( IsDisabled && ( BoxStyle.BackgroundDisabled != null ) )
        {
            return BoxStyle.BackgroundDisabled;
        }

        if ( _selectBoxList!.HasParent() && ( BoxStyle.BackgroundOpen != null ) )
        {
            return BoxStyle.BackgroundOpen;
        }

        if ( IsOver() && ( BoxStyle.BackgroundOver != null ) )
        {
            return BoxStyle.BackgroundOver;
        }

        return BoxStyle.Background;
    }

    /// <summary>
    /// Returns the appropriate label font color from the style based on the current button state.
    /// </summary>
    protected Color GetFontColor()
    {
        if ( IsDisabled && ( BoxStyle.DisabledFontColor != null ) )
        {
            return BoxStyle.DisabledFontColor;
        }

        if ( ( BoxStyle.OverFontColor != null ) && ( IsOver() || _selectBoxList!.HasParent() ) )
        {
            return BoxStyle.OverFontColor;
        }

        return BoxStyle.FontColor;
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        var background = GetBackgroundIDrawable();
        var fontColor  = GetFontColor();
        var font       = BoxStyle.Font;

        // Make copies of x,y,width and height for local modification
        // and preserving the originals.
        var x      = X;
        var y      = Y;
        var width  = Width;
        var height = Height;

        batch.SetColor( Color.R, Color.G, Color.B, Color.A * parentAlpha );

        background?.Draw( batch, x, y, width, height );

        var selected = _selection.First();

        if ( selected != null )
        {
            if ( background != null )
            {
                width  -= background.LeftWidth + background.RightWidth;
                height -= background.BottomHeight + background.TopHeight;
                x      += background.LeftWidth;
                y      += ( int ) ( ( height / 2 ) + background.BottomHeight + ( font.Data.CapHeight / 2 ) );
            }
            else
            {
                y += ( int ) ( ( height / 2 ) + ( font.Data.CapHeight / 2 ) );
            }

            font.SetColor( fontColor.R, fontColor.G, fontColor.B, fontColor.A * parentAlpha );

            DrawItem( batch, font, selected, x, y, width );
        }
    }

    protected GlyphLayout DrawItem( IBatch batch, BitmapFont font, T item, float x, float y, float width )
    {
        var str = ToString( item ) ?? "";

        return font.Draw( batch, str, x, y, 0, str.Length, width, _alignment, false, "..." );
    }

    /// <summary>
    /// Sets the alignment of the selected item in the select box. See <see cref="GetList()"/>
    /// and <see cref="SetAlignment(int)"/> to set the alignment in the list shown when the
    /// select box is open.
    /// </summary>
    /// <param name="alignment"> See <see cref="Align"/>. </param>
    public void SetAlignment( int alignment )
    {
        _alignment = alignment;
    }

    /// <summary>
    /// Get the set of selected items, useful when multiple items are selected
    /// </summary>
    /// <returns> a Selection object containing the selected elements </returns>
    public ArraySelection< T > GetSelection()
    {
        return _selection;
    }

    /// <summary>
    /// Returns the first selected item, or null. For multiple selections
    /// use <see cref="GetSelection()"/>.
    /// </summary>
    public T? GetSelected()
    {
        return _selection.First();
    }

    /// <summary>
    /// Sets the selection to only the passed item, if it is a possible
    /// choice, else selects the first item.
    /// </summary>
    public void SetSelected( T item )
    {
        if ( _items.Contains( item ) )
        {
            _selection.Set( item );
        }
        else if ( _items.Count > 0 )
        {
            _selection.Set( _items.First() );
        }
        else
        {
            _selection.Clear();
        }
    }

    /// <returns>
    /// The index of the first selected item. The top item has an index of 0.
    /// Nothing selected has an index of -1.
    /// </returns>
    public int GetSelectedIndex()
    {
        SortedSet< T > selected = _selection.Items();

        return selected.Count == 0 ? -1 : _items.IndexOf( selected.First() );
    }

    /// <summary>
    /// Sets the selection to only the selected index.
    /// </summary>
    public void SetSelectedIndex( int index )
    {
        _selection.Set( _items[ index ] );
    }

    /// <summary>
    /// When true the pref width is based on the selected item.
    /// </summary>
    public void SetSelectedPrefWidth( bool selectedPrefWidth )
    {
        _selectedPrefWidth = selectedPrefWidth;
    }

    /// <summary>
    /// Returns the pref width of the select box if the widest item was selected,
    /// for use when <see cref="SetSelectedPrefWidth(bool)"/> is true.
    /// </summary>
    public float GetMaxSelectedPrefWidth()
    {
        Pool< GlyphLayout > layoutPool = Pools< GlyphLayout >.Get();
        var                 layout     = layoutPool.Obtain();
        float               width      = 0;

        foreach ( var t in _items )
        {
            layout?.SetText( BoxStyle.Font, ToString( t ) ?? "" );
            width = Math.Max( layout!.Width, width );
        }

        var bg = BoxStyle.Background;

        if ( bg != null )
        {
            width = Math.Max( width + bg.LeftWidth + bg.RightWidth, bg.MinWidth );
        }

        return width;
    }

    public void SetDisabled( bool disabled )
    {
        if ( disabled && !IsDisabled )
        {
            HideList();
        }

        IsDisabled = disabled;
    }

    protected string? ToString( T? item )
    {
        return item?.ToString();
    }

    public void ShowList()
    {
        if ( ( _items.Count > 0 ) && ( Stage != null ) )
        {
            _selectBoxList?.Show( Stage );
        }
    }

    public void HideList()
    {
        _selectBoxList?.Hide();
    }

    /// <summary>
    /// Returns the list shown when the select box is open.
    /// </summary>
    public ListBox< T >? GetList()
    {
        return _selectBoxList?.ListBox;
    }

    /// <summary>
    /// Disables scrolling of the list shown when the select box is open.
    /// </summary>
    public void SetScrollingDisabled( bool y )
    {
        _selectBoxList?.SetScrollingDisabled( true, y );
        InvalidateHierarchy();
    }

    /// <summary>
    /// Returns the scroll pane containing the list that is shown
    /// when the select box is open.
    /// </summary>
    public ScrollPane? GetScrollPane()
    {
        return _selectBoxList;
    }

    public bool IsOver()
    {
        return ClickListener.Over;
    }

    protected void OnShow( Actor selectBoxList )
    {
        selectBoxList.Color.A = 0;
        selectBoxList.AddAction( Scene2D.Actions.Actions.FadeIn( 0.3f, Interpolation.Fade ) );
    }

    protected void OnHide( Actor selectBoxList )
    {
        selectBoxList.Color.A = 1f;

        selectBoxList.AddAction
            (
             Scene2D.Actions.Actions.Sequence
                 (
                  Scene2D.Actions.Actions.FadeOut( 0.15f, Interpolation.Fade ),
                  Scene2D.Actions.Actions.RemoveActor()
                 )
            );
    }

    // ========================================================================

    [PublicAPI]
    public class SelectBoxList : ScrollPane
    {
        public int            MaxListCount { get; set; }
        public ListBox< T >   ListBox      { get; set; }
        public SelectBox< T > SelectBox    { get; set; }

        // ====================================================================

        private readonly InputListener _hideListener;
        private readonly Vector2       _stagePosition = new();
        private          Actor?        _previousScrollFocus;

        // ====================================================================
        
        public SelectBoxList( SelectBox< T > selectBox )
            : base( null, selectBox.BoxStyle.ScrollStyle )
        {
            SelectBox = selectBox;

            SetOverscroll( false, false );
            SetFadeScrollBars( false );
            SetScrollingDisabled( true, false );

            ListBox = new ListBox< T >( SelectBox.BoxStyle.ListStyle )
            {
                Touchable    = Touchable.Disabled,
                TypeToSelect = true,
            };

            SetActor( ListBox );

            _hideListener = new SblHideListener( this );

            AddListener( new SelectBoxListClickListener( this ) );
            AddListener( new SelectBoxListInputListener( this ) );
        }

        public void Show( Stage stage )
        {
            if ( ListBox.IsTouchable() )
            {
                return;
            }

            stage.AddActor( this );
            stage.AddCaptureListener( _hideListener );

            stage.AddListener( ListBox.KeyListener
                            ?? throw new GdxRuntimeException( "No ListBox KeyListener available!" ) );

            SelectBox.LocalToStageCoordinates( _stagePosition.Set( 0, 0 ) );

            // Show the list above or below the select box, limited to a number
            // of items and the available height in the stage.
            var itemHeight = ListBox.ItemHeight;

            var height = itemHeight
                       * ( MaxListCount <= 0
                               ? SelectBox._items.Count
                               : Math.Min( MaxListCount, SelectBox._items.Count ) );

            var scrollPaneBackground = GetStyle().Background;

            if ( scrollPaneBackground != null )
            {
                height += scrollPaneBackground.TopHeight + scrollPaneBackground.BottomHeight;
            }

            var listBackground = ListBox.Style?.Background;

            if ( listBackground != null )
            {
                height += listBackground.TopHeight + listBackground.BottomHeight;
            }

            var heightBelow = _stagePosition.Y;
            var heightAbove = stage.Height - heightBelow - SelectBox.Height;
            var below       = true;

            if ( height > heightBelow )
            {
                if ( heightAbove > heightBelow )
                {
                    below  = false;
                    height = Math.Min( height, heightAbove );
                }
                else
                {
                    height = heightBelow;
                }
            }

            if ( below )
            {
                Y = _stagePosition.Y - height;
            }
            else
            {
                Y = _stagePosition.Y + SelectBox.Height;
            }

            X      = _stagePosition.X;
            Height = height;

            Validate();

            var width = Math.Max( PrefWidth, SelectBox.Width );

            if ( ( PrefHeight > height ) && !DisableYScroll )
            {
                width += GetScrollBarWidth();
            }

            Width = width;

            Validate();

            ScrollTo( 0,
                      ListBox.Height - ( SelectBox.GetSelectedIndex() * itemHeight ) - ( itemHeight / 2 ),
                      0,
                      0,
                      true,
                      true );

            UpdateVisualScroll();

            _previousScrollFocus = null;
            var actor = stage.ScrollFocus;

            if ( ( actor != null ) && !actor.IsDescendantOf( this ) )
            {
                _previousScrollFocus = actor;
            }

            stage.ScrollFocus = this;

            ListBox.Selection.Set( SelectBox.GetSelected() );
            ListBox.Touchable = Touchable.Enabled;

            ClearActions();

            SelectBox.OnShow( this );
        }

        public void Hide()
        {
            if ( !ListBox.IsTouchable() || !HasParent() )
            {
                return;
            }

            ListBox.Touchable = Touchable.Disabled;

            if ( Stage != null )
            {
                Stage.RemoveCaptureListener( _hideListener );
                Stage.RemoveListener( ListBox.KeyListener! );

                if ( _previousScrollFocus is { Stage: null } )
                {
                    _previousScrollFocus = null;
                }

                if ( ( Stage.ScrollFocus == null ) || IsAscendantOf( Stage.ScrollFocus ) )
                {
                    Stage.ScrollFocus = _previousScrollFocus;
                }
            }

            ClearActions();
            SelectBox.OnHide( this );
        }

        public override void Draw( IBatch batch, float parentAlpha )
        {
            SelectBox.LocalToStageCoordinates( SelectBox._temp.Set( 0, 0 ) );

            if ( !SelectBox._temp.Equals( _stagePosition ) )
            {
                Hide();
            }

            base.Draw( batch, parentAlpha );
        }

        public override void Act( float delta )
        {
            base.Act( delta );
            ToFront();
        }

        public override void SetStage( Stage? stage )
        {
            if ( stage != null )
            {
                stage.RemoveCaptureListener( _hideListener );
                stage.RemoveListener( ListBox.KeyListener! );
            }

            Stage = stage;
        }
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class SelectBoxArraySelection : ArraySelection< T >
    {
        private readonly SelectBox< T > _parent;

        public SelectBoxArraySelection( SelectBox< T > parent )
        {
            _parent = parent;
        }

        public override bool FireChangeEvent()
        {
            if ( _parent._selectedPrefWidth )
            {
                _parent.InvalidateHierarchy();
            }

            return base.FireChangeEvent();
        }
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class SelectBoxClickListener : ClickListener
    {
        private readonly SelectBox< T > _parent;

        public SelectBoxClickListener( SelectBox< T > parent )
        {
            _parent = parent;
        }

        public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
        {
            if ( ( pointer == 0 ) && ( button != 0 ) )
            {
                return false;
            }

            if ( _parent.IsDisabled )
            {
                return false;
            }

            if ( _parent._selectBoxList!.HasParent() )
            {
                _parent.HideList();
            }
            else
            {
                _parent.ShowList();
            }

            return true;
        }
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class SelectBoxListClickListener : ClickListener
    {
        private readonly SelectBoxList _parent;

        public SelectBoxListClickListener( SelectBoxList parent )
        {
            _parent = parent;
        }

        public override void OnClicked( InputEvent? ev, float x, float y )
        {
            var selected = _parent.ListBox.GetSelected();

            // Force clicking the already selected item to trigger a change event.
            if ( selected != null )
            {
                _parent.SelectBox._selection.Clear();
            }

            _parent.SelectBox._selection.Choose( selected! );
            _parent.Hide();
        }

        public override bool MouseMoved( InputEvent? ev, float x, float y )
        {
            var index = _parent.ListBox.GetItemIndexAt( y );

            if ( index != -1 )
            {
                _parent.ListBox.SetSelectedIndex( index );
            }

            return true;
        }
    }

    [PublicAPI]
    public class SelectBoxListInputListener : InputListener
    {
        private readonly SelectBoxList _parent;

        public SelectBoxListInputListener( SelectBoxList parent )
        {
            _parent = parent;
        }

        public override void Exit( InputEvent? ev, float x, float y, int pointer, Actor? toActor )
        {
            if ( ( toActor == null ) || !_parent.IsAscendantOf( toActor ) )
            {
                _parent.ListBox.Selection.Set( _parent.SelectBox.GetSelected() );
            }
        }
    }

    [PublicAPI]
    public class SblHideListener : InputListener
    {
        private readonly SelectBoxList _parent;

        public SblHideListener( SelectBoxList parent )
        {
            _parent = parent;
        }

        public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
        {
            var target = ev?.TargetActor;

            if ( _parent.IsAscendantOf( target ) )
            {
                return false;
            }

            _parent.ListBox.Selection.Set( _parent.SelectBox.GetSelected() );
            _parent.Hide();

            return false;
        }

        public override bool KeyDown( InputEvent? ev, int keycode )
        {
            switch ( keycode )
            {
                case IInput.Keys.NUMPAD_ENTER:
                case IInput.Keys.ENTER:
                {
                    _parent.SelectBox._selection.Choose( _parent.ListBox.GetSelected()! );
                    _parent.Hide();
                    ev?.Stop();

                    break;
                }

                // Fall thru.
                case IInput.Keys.ESCAPE:
                {
                    _parent.Hide();
                    ev?.Stop();

                    return true;
                }
            }

            return false;
        }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// The Style for a <see cref="SelectBox{T}"/>.
    /// </summary>
    [PublicAPI]
    public class SelectBoxStyle
    {
        public SelectBoxStyle()
        {
        }

        public SelectBoxStyle( BitmapFont font,
                               Color fontColor,
                               IDrawable background,
                               ScrollPane.ScrollPaneStyle scrollStyle,
                               ListBox< T >.ListStyle listStyle )
        {
            Font = font;
            FontColor.Set( fontColor );
            Background  = background;
            ScrollStyle = scrollStyle;
            ListStyle   = listStyle;
        }

        public SelectBoxStyle( SelectBoxStyle? style )
        {
            ArgumentNullException.ThrowIfNull( style );

            Font = style.Font;
            FontColor.Set( style.FontColor );

            if ( style.OverFontColor != null )
            {
                OverFontColor = new Color( style.OverFontColor );
            }

            if ( style.DisabledFontColor != null )
            {
                DisabledFontColor = new Color( style.DisabledFontColor );
            }

            Background  = style.Background;
            ScrollStyle = new ScrollPane.ScrollPaneStyle( style.ScrollStyle );
            ListStyle   = new ListBox< T >.ListStyle( style.ListStyle );

            BackgroundOver     = style.BackgroundOver;
            BackgroundOpen     = style.BackgroundOpen;
            BackgroundDisabled = style.BackgroundDisabled;
        }

        public BitmapFont                 Font               { get; } = null!;
        public ScrollPane.ScrollPaneStyle ScrollStyle        { get; } = null!;
        public ListBox< T >.ListStyle     ListStyle          { get; } = null!;
        public Color                      FontColor          { get; } = new( 1, 1, 1, 1 );
        public Color?                     OverFontColor      { get; }
        public Color?                     DisabledFontColor  { get; }
        public IDrawable?                 Background         { get; }
        public IDrawable?                 BackgroundOver     { get; }
        public IDrawable?                 BackgroundOpen     { get; }
        public IDrawable?                 BackgroundDisabled { get; }
    }
}
