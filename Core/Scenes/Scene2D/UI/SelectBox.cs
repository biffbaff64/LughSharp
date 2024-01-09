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

using LibGDXSharp.Graphics.G2D;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.UI;

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
public class SelectBox<T> : Widget, IDisableable
{
    public ClickListener ClickListener { get; set; }
    public bool          IsDisabled    { get; set; }

    private readonly List< T >           _items;
    private readonly SelectBoxList< T >? _selectBoxList;
    private readonly ArraySelection< T > _selection;

    private float _prefWidth;
    private float _prefHeight;
    private int   _alignment;
    private bool  _selectedPrefWidth;

    private readonly static Vector2 _temp = new();

    public SelectBox( Skin skin ) : this( skin.Get< SelectBoxStyle >() )
    {
    }

    public SelectBox( Skin skin, String styleName )
        : this( skin.Get< SelectBoxStyle >( styleName ) )
    {
    }

    public SelectBox( SelectBoxStyle style )
    {
        this._alignment = Align.LEFT;
        this._items     = new List< T >();

        _selection = new ArraySelection< T >( _items )
        {
            Actor    = this,
            Required = true
        };

        _selectBoxList = new SelectBoxList< T >( this );
        ClickListener  = new SelectBoxClickListener( this );

        Setup( style );
    }

    private void Setup( SelectBoxStyle style )
    {
        SetStyle( style );
        SetSize( PrefWidth, PrefHeight );
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

    protected void SetStage( Stage? stage )
    {
        if ( stage == null )
        {
            _selectBoxList?.Hide();
        }

        base.Stage = stage;
    }

    public SelectBoxStyle? BoxStyle { get; set; }

    public void SetStyle( SelectBoxStyle style )
    {
        ArgumentNullException.ThrowIfNull( style );

        this.BoxStyle = style;

        if ( _selectBoxList != null )
        {
            _selectBoxList.SetStyle( style.ScrollStyle );
            _selectBoxList.ListBox.Style = style.ListStyle;
        }

        InValidateHierarchy();
    }

    /// <summary>
    /// Set the backing List that makes up the choices available in the SelectBox
    /// </summary>
    public void SetItems( params T[] newItems )
    {
        ArgumentNullException.ThrowIfNull( newItems );

        float oldPrefWidth = PrefWidth;

        _items.Clear();
        _items.AddAll( newItems );
        _selection.Validate();
        _selectBoxList?.ListBox.SetItems( _items );

        InValidate();

        if ( oldPrefWidth != PrefWidth )
        {
            InValidateHierarchy();
        }
    }

    /// <summary>
    /// Sets the items visible in the select box.
    /// </summary>
    public void SetItems( List< T > newItems )
    {
        if ( newItems == null )
        {
            throw new IllegalArgumentException( "newItems cannot be null." );
        }

        float oldPrefWidth = PrefWidth;

        if ( newItems != _items )
        {
            _items.clear();
            _items.addAll( newItems );
        }

        _selection.Validate();
        _selectBoxList.list.setItems( _items );

        InValidate();

        if ( oldPrefWidth != PrefWidth )
        {
            InValidateHierarchy();
        }
    }

    public void ClearItems()
    {
        if ( _items.size == 0 )
        {
            return;
        }

        _items.clear();
        _selection.clear();
        InValidateHierarchy();
    }

    /** Returns the internal items array. If modified, {@link #setItems(List)} must be called to reflect the changes. */
    public List< T > GetItems()
    {
        return _items;
    }

    public void Layout()
    {
        IDrawable  bg   = BoxStyle.Background;
        BitmapFont font = BoxStyle.Font;

        if ( bg != null )
        {
            _prefHeight = Math.max( ( bg.getTopHeight() + bg.getBottomHeight() + font.getCapHeight() ) - ( font.getDescent() * 2 ),
                                    bg.getMinHeight() );
        }
        else
        {
            _prefHeight = font.getCapHeight() - ( font.getDescent() * 2 );
        }

        Pool< GlyphLayout > layoutPool = Pools< T >.get( GlyphLayout.class);
        GlyphLayout         layout     = layoutPool.obtain();

        if ( _selectedPrefWidth )
        {
            _prefWidth = 0;

            if ( bg != null )
            {
                _prefWidth = bg.getLeftWidth() + bg.getRightWidth();
            }

            T selected = GetSelected();

            if ( selected != null )
            {
                layout.setText( font, ToString( selected ) );
                _prefWidth += layout.width;
            }
        }
        else
        {
            float maxItemWidth = 0;

            for ( int i = 0; i < _items.size; i++ )
            {
                layout.setText( font, ToString( _items.get( i ) ) );
                maxItemWidth = Math.max( layout.width, maxItemWidth );
            }

            _prefWidth = maxItemWidth;

            if ( bg != null )
            {
                _prefWidth = Math.max( _prefWidth + bg.getLeftWidth() + bg.getRightWidth(), bg.getMinWidth() );
            }

            ListBox< T >.ListStyle     listStyle   = BoxStyle.ListStyle;
            ScrollPane.ScrollPaneStyle scrollStyle = BoxStyle.ScrollStyle;
            float                      listWidth   = maxItemWidth + listStyle.selection.getLeftWidth() + listStyle.selection.getRightWidth();
            bg = scrollStyle.background;

            if ( bg != null )
            {
                listWidth = Math.max( listWidth + bg.getLeftWidth() + bg.getRightWidth(), bg.getMinWidth() );
            }

            if ( ( _selectBoxList == null ) || !_selectBoxList.disableY )
            {
                listWidth += Math.max( BoxStyle.ScrollStyle.vScroll != null ? BoxStyle.ScrollStyle.vScroll.getMinWidth() : 0,
                                       BoxStyle.ScrollStyle.vScrollKnob != null ? BoxStyle.ScrollStyle.vScrollKnob.getMinWidth() : 0 );
            }

            _prefWidth = Math.max( _prefWidth, listWidth );
        }

        layoutPool.free( layout );
    }

    /** Returns appropriate background Drawable from the style based on the current select box state. */
    protected IDrawable GetBackgroundIDrawable()
    {
        if ( IsDisabled() && ( BoxStyle.BackgroundDisabled != null ) )
        {
            return BoxStyle.BackgroundDisabled;
        }

        if ( _selectBoxList.hasParent() && ( BoxStyle.BackgroundOpen != null ) )
        {
            return BoxStyle.BackgroundOpen;
        }

        if ( IsOver() && ( BoxStyle.BackgroundOver != null ) )
        {
            return BoxStyle.BackgroundOver;
        }

        return BoxStyle.Background;
    }

    /** Returns the appropriate label font color from the style based on the current button state. */
    protected Color GetFontColor()
    {
        if ( IsDisabled() && ( BoxStyle.DisabledFontColor != null ) )
        {
            return BoxStyle.DisabledFontColor;
        }

        if ( ( BoxStyle.OverFontColor != null ) && ( IsOver() || _selectBoxList.hasParent() ) )
        {
            return BoxStyle.OverFontColor;
        }

        return BoxStyle.FontColor;
    }

    public void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        IDrawable  background = GetBackgroundIDrawable();
        Color      fontColor  = GetFontColor();
        BitmapFont font       = BoxStyle.Font;

        Color color = getColor();
        float x     = getX(),     y      = getY();
        float width = getWidth(), height = getHeight();

        batch.setColor( color.r, color.g, color.b, color.a * parentAlpha );

        if ( background != null )
        {
            background.Draw( batch, x, y, width, height );
        }

        T selected = _selection.first();

        if ( selected != null )
        {
            if ( background != null )
            {
                width  -= background.getLeftWidth() + background.getRightWidth();
                height -= background.getBottomHeight() + background.getTopHeight();
                x      += background.getLeftWidth();
                y      += ( int )( ( height / 2 ) + background.getBottomHeight() + ( font.getData().capHeight / 2 ) );
            }
            else
            {
                y += ( int )( ( height / 2 ) + ( font.getData().capHeight / 2 ) );
            }

            font.setColor( fontColor.r, fontColor.g, fontColor.b, fontColor.a * parentAlpha );
            DrawItem( batch, font, selected, x, y, width );
        }
    }

    protected GlyphLayout DrawItem( IBatch batch, BitmapFont font, T item, float x, float y, float width )
    {
        String string = ToString( item );

        return font.Draw( batch, string, x, y, 0, string.length(), width, _alignment, false, "..." );
    }

    /** Sets the alignment of the selected item in the select box. See {@link #getList()} and {@link List#setAlignment(int)} to set
     * the alignment in the list shown when the select box is open.
     * @param alignment See {@link Align}. */
    public void SetAlignment( int alignment )
    {
        this._alignment = alignment;
    }

    /** Get the set of selected items, useful when multiple items are selected
     * @return a Selection object containing the selected elements */
    public ListSelection< T > GetSelection()
    {
        return _selection;
    }

    /** Returns the first selected item, or null. For multiple selections use {@link SelectBox#getSelection()}. */
    public T GetSelected()
    {
        return _selection.first();
    }

    /** Sets the selection to only the passed item, if it is a possible choice, else selects the first item. */
    public void SetSelected( T item )
    {
        if ( _items.contains( item, false ) )
        {
            _selection.set( item );
        }
        else if ( _items.size > 0 )
        {
            _selection.set( _items.first() );
        }
        else
        {
            _selection.clear();
        }
    }

    /** @return The index of the first selected item. The top item has an index of 0. Nothing selected has an index of -1. */
    public int GetSelectedIndex()
    {
        ObjectSet< T > selected = _selection.items();

        return selected.size == 0 ? -1 : _items.indexOf( selected.first(), false );
    }

    /** Sets the selection to only the selected index. */
    public void SetSelectedIndex( int index )
    {
        _selection.set( _items.get( index ) );
    }

    /** When true the pref width is based on the selected item. */
    public void SetSelectedPrefWidth( bool selectedPrefWidth )
    {
        this._selectedPrefWidth = selectedPrefWidth;
    }

    /** Returns the pref width of the select box if the widest item was selected, for use when
     * {@link #setSelectedPrefWidth(bool)} is true. */
    public float GetMaxSelectedPrefWidth()
    {
        Pool< GlyphLayout > layoutPool = Pools.get( GlyphLayout.class);
        GlyphLayout         layout     = layoutPool.obtain();
        float               width      = 0;

        for ( int i = 0; i < _items.size; i++ )
        {
            layout.setText( BoxStyle.Font, ToString( _items.get( i ) ) );
            width = Math.max( layout.width, width );
        }

        IDrawable bg = BoxStyle.Background;

        if ( bg != null )
        {
            width = Math.max( width + bg.getLeftWidth() + bg.getRightWidth(), bg.getMinWidth() );
        }

        return width;
    }

    public void SetDisabled( bool disabled )
    {
        if ( disabled && !this.IsDisabled )
        {
            HideList();
        }

        this.IsDisabled = disabled;
    }

    protected string? ToString( T? item )
    {
        return item?.ToString();
    }

    public void ShowList()
    {
        if ( _items.size == 0 )
        {
            return;
        }

        if ( Stage != null )
        {
            _selectBoxList.show( Stage );
        }
    }

    public void HideList()
    {
        _selectBoxList.hide();
    }

    /** Returns the list shown when the select box is open. */
    public List< T > GetList()
    {
        return _selectBoxList.list;
    }

    /** Disables scrolling of the list shown when the select box is open. */
    public void SetScrollingDisabled( bool y )
    {
        _selectBoxList.setScrollingDisabled( true, y );
        InValidateHierarchy();
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
        return ClickListener.IsOver();
    }

    protected void OnShow( Actor selectBoxList, bool below )
    {
        selectBoxList.getColor().a = 0;
        selectBoxList.addAction( fadeIn( 0.3f, Interpolation.fade ) );
    }

    protected void OnHide( Actor selectBoxList )
    {
        selectBoxList.getColor().a = 1;
        selectBoxList.addAction( sequence( fadeOut( 0.15f, Interpolation.fade ), removeActor() ) );
    }

    /** @author Nathan Sweet */
    public class SelectBoxList<T> : ScrollPane
    {
        public int          MaxListCount { get; set; }
        public ListBox< T > ListBox      { get; set; }

        private SelectBox< T > _selectBox;
        private Vector2        _stagePosition;
        private InputListener  _hideListener;
        private Actor?         _previousScrollFocus;

        public SelectBoxList( SelectBox< T > selectBox )
            : base( null, selectBox.BoxStyle.ScrollStyle )
        {
            this._selectBox = selectBox;

            setOverscroll( false, false );
            setFadeScrollBars( false );
            SetScrollingDisabled( true, false );

            ListBox = new List< T >( selectBox.BoxStyle.ListStyle )
            {
 


                public String toString (T obj)
                {
                return selectBox.toString(obj);
            }

            };

            ListBox.setTouchable( Touchable.disabled );
            ListBox.setTypeToSelect( true );
            setActor( ListBox );

            //    list.addListener( new ClickListener()
            //    {
            //        public void clicked (InputEvent event, float x, float y)
            //        {
            //            T selected = list.getSelected();
            //
            //            // Force clicking the already selected item to trigger a change event.
            //            if (selected != null) selectBox.selection.items().clear();
            //            selectBox.selection.choose(selected);
            //            hide();
            //        }
            //
            //        public bool mouseMoved( InputEvent event, float x, float y )
            //        {
            //            int index = list.getItemIndexAt( y );
            //            if ( index != -1 ) list.setSelectedIndex( index );
            //
            //            return true;
            //        }
            //    });

            //    addListener( new InputListener()
            //    {
            //        public void exit (InputEvent event, float x, float y, int pointer, Actor toActor) {
            //        if (toActor == null || !isAscendantOf(toActor)) list.selection.set(selectBox.getSelected());
            //    }
            //    });

            //    hideListener = new InputListener()
            //    {
            //        public bool touchDown (InputEvent event, float x, float y, int pointer, int button) {
            //        Actor target = event.getTarget();
            //        if (isAscendantOf(target)) return false;
            //        list.selection.set(selectBox.getSelected());
            //        hide();
            //        return false;
            //    }
            //
            //    public bool keyDown( InputEvent event, int keycode )
            //    {
            //        switch ( keycode )
            //        {
            //            case Keys.NUMPAD_ENTER:
            //            case Keys.ENTER:
            //                selectBox.selection.choose( list.getSelected() );
            //
            //            // Fall thru.
            //            case Keys.ESCAPE:
            //                hide();
            //                    event.stop();
            //
            //                return true;
            //        }
            //
            //        return false;
            //    }
            //    };
        }

        public void Show( Stage stage )
        {
            if ( ListBox.IsTouchable() )
            {
                return;
            }

            stage.AddActor( this );
            stage.AddCaptureListener( _hideListener );
            stage.AddListener( ListBox.getKeyListener() );

            _selectBox.localToStageCoordinates( _stagePosition.set( 0, 0 ) );

            // Show the list above or below the select box, limited to a number
            // of items and the available height in the stage.
            float itemHeight = ListBox.getItemHeight();

            float height = itemHeight
                         * ( MaxListCount <= 0
                               ? _selectBox.items.size
                               : Math.min( MaxListCount, _selectBox.items.size ) );

            IDrawable scrollPaneBackground = GetStyle().Background;

            if ( scrollPaneBackground != null )
            {
                height += scrollPaneBackground.getTopHeight() + scrollPaneBackground.getBottomHeight();
            }

            IDrawable listBackground = ListBox.getStyle().background;

            if ( listBackground != null )
            {
                height += listBackground.TopHeight + listBackground.BottomHeight;
            }

            var heightBelow = _stagePosition.Y;
            var heightAbove = stage.Height - heightBelow - _selectBox.Height;
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
                this.Y = ( _stagePosition.Y - height );
            }
            else
            {
                this.Y = ( _stagePosition.Y + _selectBox.Height );
            }

            this.X = _stagePosition.X;
            this.Height = height;

            Validate();

            float width = Math.Max( PrefWidth, _selectBox.Width );

            if ( ( PrefHeight > height ) && !DisableY )
            {
                width += GetScrollBarWidth();
            }

            this.Width = width;

            Validate();

            ScrollTo( 0, ListBox.Height - ( _selectBox.GetSelectedIndex() * itemHeight ) - ( itemHeight / 2 ), 0, 0, true, true );

            UpdateVisualScroll();

            _previousScrollFocus = null;
            Actor? actor = stage.ScrollFocus;

            if ( ( actor != null ) && !actor.IsDescendantOf( this ) )
            {
                _previousScrollFocus = actor;
            }

            stage.ScrollFocus = this;

            ListBox.Selection?.Set( _selectBox.GetSelected() );
            ListBox.Touchable = Touchable.Enabled;

            ClearActions();

            _selectBox.OnShow( this, below );
        }

        public void Hide()
        {
            if ( !ListBox.IsTouchable() || !HasParent() )
            {
                return;
            }

            ListBox.Touchable = Touchable.Disabled;

            if ( this.Stage != null )
            {
                this.Stage.RemoveCaptureListener( _hideListener );
                this.Stage.RemoveListener( ListBox.KeyListener! );

                if ( _previousScrollFocus is { Stage: null } )
                {
                    _previousScrollFocus = null;
                }

                if ( ( this.Stage.ScrollFocus == null ) || IsAscendantOf( this.Stage.ScrollFocus ) )
                {
                    this.Stage.ScrollFocus = _previousScrollFocus;
                }
            }

            ClearActions();
            _selectBox.OnHide( this );
        }

        [PublicAPI]
        public override void Draw( IBatch batch, float parentAlpha )
        {
            _selectBox.LocalToStageCoordinates( _temp.Set( 0, 0 ) );

            if ( !_temp.Equals( _stagePosition ) )
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

            base.Stage = stage;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

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
                _parent.InValidateHierarchy();
            }

            return base.FireChangeEvent();
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// The Style for a <see cref="SelectBox{T}"/>.
    /// </summary>
    public class SelectBoxStyle
    {
        public BitmapFont?                 Font               { get; }
        public Color                       FontColor          { get; } = new( 1, 1, 1, 1 );
        public Color?                      OverFontColor      { get; }
        public Color?                      DisabledFontColor  { get; }
        public IDrawable?                  Background         { get; }
        public ScrollPane.ScrollPaneStyle? ScrollStyle        { get; }
        public ListBox< T >.ListStyle?     ListStyle          { get; }
        public IDrawable?                  BackgroundOver     { get; }
        public IDrawable?                  BackgroundOpen     { get; }
        public IDrawable?                  BackgroundDisabled { get; }

        public SelectBoxStyle()
        {
        }

        public SelectBoxStyle( BitmapFont font,
                               Color fontColor,
                               IDrawable background,
                               ScrollPane.ScrollPaneStyle scrollStyle,
                               ListBox< T >.ListStyle listStyle )
        {
            this.Font = font;
            this.FontColor.Set( fontColor );
            this.Background  = background;
            this.ScrollStyle = scrollStyle;
            this.ListStyle   = listStyle;
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
            ScrollStyle = new ScrollPane.ScrollPaneStyle( style.ScrollStyle! );
            ListStyle   = new ListBox< T >.ListStyle( style.ListStyle! );

            BackgroundOver     = style.BackgroundOver;
            BackgroundOpen     = style.BackgroundOpen;
            BackgroundDisabled = style.BackgroundDisabled;
        }
    }
}
