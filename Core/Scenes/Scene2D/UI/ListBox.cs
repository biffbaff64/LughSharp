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
using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Utils.Pooling;

using Color = LibGDXSharp.Graphics.Color;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
///     A list box displays textual items and highlights the currently selected item.
///     <para>
///         <see cref="ChangeListener.ChangeEvent" /> is fired when the list selection changes.
///     </para>
///     <para>
///         The preferred size of the list is determined by the text bounds of the items
///         and the size of the <see cref="Selection{T}" />.
///     </para>
/// </summary>
public class ListBox<T> : Widget
{
    private int   _overIndex = -1;
    private float _prefHeight;
    private float _prefWidth;
    private int   _pressedIndex = -1;

    private ListStyle? _style;

    public ListBox( Skin skin )
        : this( skin.Get< ListStyle >() )
    {
    }

    public ListBox( Skin skin, string styleName )
        : this( skin.Get< ListStyle >( styleName ) )
    {
    }

    public ListBox( ListStyle style ) => Create( style );

    public RectangleShape?      CullingArea  { get; set; }
    public InputListener?       KeyListener  { get; set; }
    public ArraySelection< T >? Selection    { get; set; }
    public List< T >?           Items        { get; set; } = new();
    public float                ItemHeight   { get; set; }
    public int                  Alignment    { get; set; } = Align.LEFT;
    public bool                 TypeToSelect { get; set; }

    public override float PrefWidth
    {
        get
        {
            Validate();

            return _prefWidth;
        }
        set => _prefWidth = value;
    }

    public override float PrefHeight
    {
        get
        {
            Validate();

            return _prefHeight;
        }
        set => _prefHeight = value;
    }

    private void Create( ListStyle style )
    {
        Selection = new ArraySelection< T >( Items )
        {
            Actor    = this,
            Required = true
        };

        SetStyle( style );
        SetSize( PrefWidth, PrefHeight );

        KeyListener = new ListKeyListener( this );

        AddListener( KeyListener );
        AddListener( new ListInputListener( this ) );
    }

    /// <summary>
    ///     Returns the list's style. Modifying the returned style may not have an
    ///     effect until <see cref="SetStyle(ListStyle)" />" is called.
    /// </summary>
    public ListStyle? GetStyle() => _style;

    public void SetStyle( ListStyle style )
    {
        _style = style ?? throw new ArgumentException( "style cannot be null." );

        InvalidateHierarchy();
    }

    public override void SetLayout()
    {
        BitmapFont? font             = _style?.Font;
        IDrawable?  selectedDrawable = _style?.Selection;

        if ( font == null )
        {
            throw new GdxRuntimeException( "Layout: supplied style has a null font!" );
        }

        if ( selectedDrawable == null )
        {
            throw new GdxRuntimeException( "Layout: supplied style has a null selected drawable!" );
        }

        ItemHeight =  font.GetCapHeight() - ( font.GetDescent() * 2 );
        ItemHeight += selectedDrawable.TopHeight + selectedDrawable.BottomHeight;

        _prefWidth = 0;

        Pool< GlyphLayout > layoutPool = Pools< GlyphLayout >.Get();
        GlyphLayout?        layout     = layoutPool.Obtain();

        for ( var i = 0; i < Items!.Count; i++ )
        {
            layout?.SetText( font, ToString( Items[ i ] ) );
            _prefWidth = Math.Max( layout!.Width, _prefWidth );
        }

        layoutPool.Free( layout! );
        _prefWidth  += selectedDrawable.LeftWidth + selectedDrawable.RightWidth;
        _prefHeight =  Items.Count * ItemHeight;

        IDrawable? background = _style?.Background;

        if ( background != null )
        {
            _prefWidth  = Math.Max( _prefWidth + background.LeftWidth + background.RightWidth, background.MinWidth );
            _prefHeight = Math.Max( _prefHeight + background.TopHeight + background.BottomHeight, background.MinHeight );
        }
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        DrawBackground( batch, parentAlpha );

        BitmapFont? font                = _style?.Font;
        IDrawable?  selectedDrawable    = _style?.Selection;
        Color?      fontColorSelected   = _style?.FontColorSelected;
        Color?      fontColorUnselected = _style?.FontColorUnselected;

        batch.SetColor( Color!.R, Color.G, Color.B, Color.A * parentAlpha );

        var x      = X;
        var y      = Y;
        var width  = Width;
        var height = Height;
        var itemY  = height;

        IDrawable? background = _style?.Background;

        if ( background != null )
        {
            var leftWidth = background.LeftWidth;

            x     += leftWidth;
            itemY -= background.TopHeight;
            width -= leftWidth + background.RightWidth;
        }

        var textOffsetX = selectedDrawable?.LeftWidth;
        var textWidth   = width - textOffsetX - selectedDrawable?.RightWidth;
        var textOffsetY = selectedDrawable!.TopHeight - font!.GetDescent();

        font.SetColor( fontColorUnselected!.R, fontColorUnselected.G, fontColorUnselected.B, fontColorUnselected.A * parentAlpha );

        for ( var i = 0; i < Items!.Count; i++ )
        {
            if ( ( CullingArea == null )
              || ( ( ( itemY - ItemHeight ) <= ( CullingArea.Y + CullingArea.Height ) ) && ( itemY >= CullingArea.Y ) ) )
            {
                T          item     = Items[ i ];
                var        selected = Selection!.Contains( item );
                IDrawable? drawable = null;

                if ( ( _pressedIndex == i ) && ( _style?.Down != null ) )
                {
                    drawable = _style.Down;
                }
                else if ( selected )
                {
                    drawable = selectedDrawable;
                    font.SetColor( fontColorSelected!.R, fontColorSelected.G, fontColorSelected.B, fontColorSelected.A * parentAlpha );
                }
                else if ( ( _overIndex == i ) && ( _style?.Over != null ) )
                {
                    drawable = _style.Over;
                }

                if ( drawable != null )
                {
                    drawable.Draw( batch, x, ( y + itemY ) - ItemHeight, width, ItemHeight );
                }

                DrawItem( batch, font, i, item, ( float )( x + textOffsetX )!, ( y + itemY ) - textOffsetY, ( float )textWidth! );

                if ( selected )
                {
                    font.SetColor( fontColorUnselected.R,
                                   fontColorUnselected.G,
                                   fontColorUnselected.B,
                                   fontColorUnselected.A * parentAlpha );
                }
            }
            else if ( itemY < CullingArea.Y )
            {
                break;
            }

            itemY -= ItemHeight;
        }
    }

    /// <summary>
    ///     Called to draw the background. Default implementation draws the style background drawable.
    /// </summary>
    protected void DrawBackground( IBatch batch, float parentAlpha )
    {
        if ( _style?.Background != null )
        {
            batch.SetColor( Color!.R,
                            Color.G,
                            Color.B,
                            Color.A * parentAlpha );

            _style.Background.Draw( batch, X, Y, Width, Height );
        }
    }

    protected GlyphLayout DrawItem( IBatch batch, BitmapFont font, int index, T item, float x, float y, float width )
    {
        var str = ToString( item );

        return font.Draw( batch, str, x, y, 0, str.Length, width, Alignment, false, "..." );
    }

    /// <summary>
    ///     Returns the first selected item, or null.
    /// </summary>
    public T? GetSelected() => Selection!.First();

    /// <summary>
    ///     Sets the selection to only the passed item, if it is a possible choice.
    /// </summary>
    /// <param name="item"> May be null. </param>
    public void SetSelected( T item )
    {
        if ( Items!.Contains( item ) )
        {
            Selection!.Set( item );
        }
        else if ( Selection!.Required && ( Items.Count > 0 ) )
        {
            Selection!.Set( Items.First() );
        }
        else
        {
            Selection!.Clear();
        }
    }

    /// <summary>
    ///     Returns the index of the first selected item. The top item has an index of 0.
    ///     Nothing selected has an index of -1.
    /// </summary>
    public int GetSelectedIndex()
    {
        List< T >? selected = Selection?.ToArray();

        return selected?.Count == 0 ? -1 : Items!.IndexOf( selected!.First() );
    }

    /// <summary>
    ///     Sets the selection to only the selected index.
    /// </summary>
    /// <param name="index"> -1 to clear the selection. </param>
    public void SetSelectedIndex( int index )
    {
        if ( ( index < -1 ) || ( index >= Items?.Count ) )
        {
            throw new ArgumentException( $"index must be >= -1 and < {Items?.Count}: {index}" );
        }

        if ( index == -1 )
        {
            Selection?.Clear();
        }
        else
        {
            Selection?.Set( Items![ index ] );
        }
    }

    public T? GetOverItem() => _overIndex == -1 ? default( T? ) : Items![ _overIndex ];

    public T? GetPressedItem() => _pressedIndex == -1 ? default( T? ) : Items![ _pressedIndex ];

    public T? GetItemAt( float y )
    {
        var index = GetItemIndexAt( y );

        if ( index == -1 )
        {
            return default( T? );
        }

        return Items![ index ];
    }

    /// <summary>
    /// </summary>
    /// <returns> -1 if not over an item. </returns>
    public int GetItemIndexAt( float y )
    {
        var        height     = Height;
        IDrawable? background = _style?.Background;

        if ( background != null )
        {
            height -= background.TopHeight + background.BottomHeight;
            y      -= background.BottomHeight;
        }

        var index = ( int )( ( height - y ) / ItemHeight );

        if ( ( index < 0 ) || ( index >= Items!.Count ) )
        {
            return -1;
        }

        return index;
    }

    public void SetItems( params T[] newItems )
    {
        ArgumentNullException.ThrowIfNull( newItems );

        var oldPrefWidth  = PrefWidth;
        var oldPrefHeight = PrefHeight;

        Items?.Clear();
        Items?.AddAll( newItems );

        _overIndex    = -1;
        _pressedIndex = -1;

        Selection?.Validate();

        Invalidate();

        if ( !oldPrefWidth.Equals( PrefWidth ) || !oldPrefHeight.Equals( PrefHeight ) )
        {
            InvalidateHierarchy();
        }
    }

    /// <summary>
    ///     Sets the items visible in the list, clearing the selection if it is no longer valid. If a
    ///     selection is <see cref="ArraySelection{T}.Required()" />", the first item is selected. This
    ///     can safely be called with a (modified) array returned from <see cref="Items" />"
    /// </summary>
    public void SetItems( List< T > newItems )
    {
        ArgumentNullException.ThrowIfNull( newItems );

        var oldPrefWidth  = PrefWidth;
        var oldPrefHeight = PrefHeight;

        if ( !newItems.Equals( Items ) )
        {
            Items?.Clear();
            Items?.AddAll( newItems );
        }

        _overIndex    = -1;
        _pressedIndex = -1;
        Selection?.Validate();

        Invalidate();

        if ( !oldPrefWidth.Equals( PrefWidth ) || !oldPrefHeight.Equals( PrefHeight ) )
        {
            InvalidateHierarchy();
        }
    }

    public void ClearItems()
    {
        if ( Items?.Count == 0 )
        {
            return;
        }

        Items?.Clear();

        _overIndex    = -1;
        _pressedIndex = -1;

        Selection?.Clear();

        InvalidateHierarchy();
    }

    public string ToString( T? obj ) => obj?.ToString() ?? string.Empty;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------


    public class ListKeyListener : InputListener
    {
        private readonly ListBox< T > _parent;
        private          string       _prefix;
        private          long         _typeTimeout;

        public ListKeyListener( ListBox< T > lb )
        {
            _prefix = string.Empty;
            _parent = lb;
        }

        public override bool KeyDown( InputEvent ev, int keycode )
        {
            if ( _parent.Items?.Count == 0 )
            {
                return false;
            }

            int index;

            switch ( keycode )
            {
                case IInput.Keys.A:
                    if ( UIUtils.Ctrl() && ( bool )_parent.Selection?.Multiple )
                    {
                        _parent.Selection?.Clear();
                        _parent.Selection?.AddAll( _parent.Items! );

                        return true;
                    }

                    break;

                case IInput.Keys.HOME:
                    _parent.SetSelectedIndex( 0 );

                    return true;

                case IInput.Keys.END:
                    _parent.SetSelectedIndex( _parent.Items!.Count - 1 );

                    return true;

                case IInput.Keys.DOWN:
                    index = _parent.Items!.IndexOf( _parent.GetSelected()! ) + 1;

                    if ( index >= _parent.Items.Count )
                    {
                        index = 0;
                    }

                    _parent.SetSelectedIndex( index );

                    return true;

                case IInput.Keys.UP:
                    index = _parent.Items!.IndexOf( _parent.GetSelected()! ) - 1;

                    if ( index < 0 )
                    {
                        index = _parent.Items.Count - 1;
                    }

                    _parent.SetSelectedIndex( index );

                    return true;

                case IInput.Keys.ESCAPE:
                    if ( _parent.Stage != null )
                    {
                        _parent.Stage.KeyboardFocus = null;
                    }

                    return true;
            }

            return false;
        }

        public override bool KeyTyped( InputEvent ev, char character )
        {
            if ( !_parent.TypeToSelect )
            {
                return false;
            }

            var time = TimeUtils.Millis();

            if ( time > _typeTimeout )
            {
                _prefix = "";
            }

            _typeTimeout =  time + 300;
            _prefix      += char.ToLower( character );

            for ( int i = 0, n = _parent.Items!.Count; i < n; i++ )
            {
                if ( _parent.ToString( _parent.Items[ i ] ).ToLower().StartsWith( _prefix ) )
                {
                    _parent.SetSelectedIndex( i );

                    break;
                }
            }

            return false;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------


    public class ListInputListener : InputListener
    {
        private readonly ListBox< T > _parent;

        public ListInputListener( ListBox< T > lb ) => _parent = lb;

        public override bool TouchDown( InputEvent ev, float x, float y, int pointer, int button )
        {
            if ( ( pointer != 0 ) || ( button != 0 ) )
            {
                return true;
            }

            if ( ( bool )_parent.Selection?.IsDisabled )
            {
                return true;
            }

            if ( _parent.Stage != null )
            {
                _parent.Stage.KeyboardFocus = _parent;
            }

            if ( _parent.Items?.Count == 0 )
            {
                return true;
            }

            var index = _parent.GetItemIndexAt( y );

            if ( index == -1 )
            {
                return true;
            }

            _parent.Selection.Choose( _parent.Items![ index ] );
            _parent._pressedIndex = index;

            return true;
        }

        public override void TouchUp( InputEvent ev, float x, float y, int pointer, int button )
        {
            if ( ( pointer != 0 ) || ( button != 0 ) )
            {
                return;
            }

            _parent._pressedIndex = -1;
        }

        public override void TouchDragged( InputEvent ev, float x, float y, int pointer ) => _parent._overIndex = _parent.GetItemIndexAt( y );

        public override bool MouseMoved( InputEvent ev, float x, float y )
        {
            _parent._overIndex = _parent.GetItemIndexAt( y );

            return false;
        }

        public override void Exit( InputEvent ev, float x, float y, int pointer, Actor? toActor )
        {
            if ( pointer == 0 )
            {
                _parent._pressedIndex = -1;
            }

            if ( pointer == -1 )
            {
                _parent._overIndex = -1;
            }
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     The style for a list, see <see cref="ListBox{T}" />.
    /// </summary>
    public class ListStyle
    {

        public ListStyle() => Font = new BitmapFont();

        public ListStyle( BitmapFont font, Color fontColorSelected, Color fontColorUnselected, IDrawable selection )
        {
            Font = font;
            FontColorSelected.Set( fontColorSelected );
            FontColorUnselected.Set( fontColorUnselected );
            Selection = selection;
        }

        public ListStyle( ListStyle style )
        {
            Font = style.Font;
            FontColorSelected.Set( style.FontColorSelected );
            FontColorUnselected.Set( style.FontColorUnselected );
            Selection = style.Selection;

            Down       = style.Down;
            Over       = style.Over;
            Background = style.Background;
        }

        public BitmapFont Font                { get; set; }
        public Color      FontColorSelected   { get; set; } = new( 1, 1, 1, 1 );
        public Color      FontColorUnselected { get; set; } = new( 1, 1, 1, 1 );
        public IDrawable? Selection           { get; set; }
        public IDrawable? Down                { get; set; }
        public IDrawable? Over                { get; set; }
        public IDrawable? Background          { get; set; }
    }
}
