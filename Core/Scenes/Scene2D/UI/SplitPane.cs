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

namespace LibGDXSharp.Scenes.Scene2D.UI;

public class SplitPane : WidgetGroup
{
    private SplitPaneStyle _style = null!;
    private Actor?         _firstWidget;
    private Actor?         _secondWidget;
    private bool           _vertical;
    private float          _splitAmount = 0.5f;
    private float          _minAmount;
    private float          _maxAmount = 1;
    private bool           _cursorOverHandle;

    private readonly RectangleShape _firstWidgetBounds  = new();
    private readonly RectangleShape _secondWidgetBounds = new();
    private readonly RectangleShape _handleBounds       = new();
    private readonly RectangleShape _tempScissors       = new();
    private readonly Vector2        _lastPoint          = new();
    private readonly Vector2        _handlePosition     = new();

    public SplitPane( Actor? firstWidget, Actor? secondWidget, bool vertical, Skin skin )
        : this( firstWidget, secondWidget, vertical, skin, "default-" + ( vertical ? "vertical" : "horizontal" ) )
    {
    }

    public SplitPane( Actor? firstWidget, Actor? secondWidget, bool vertical, Skin skin, String styleName )
        : this( firstWidget, secondWidget, vertical, skin.Get< SplitPaneStyle >( styleName ) )
    {
    }

    public SplitPane( Actor? firstWidget, Actor? secondWidget, bool vertical, SplitPaneStyle style )
    {
        this._vertical = vertical;

        SetStyle( style );
        SetFirstWidget( firstWidget );
        SetSecondWidget( secondWidget );
        SetSize( GetPrefWidth(), GetPrefHeight() );
        Initialise();
    }

    private void Initialise()
    {
        AddListener( new SplitPaneInputListener( this ) );
    }

    public void SetStyle( SplitPaneStyle style )
    {
        this._style = style;
        InvalidateHierarchy();
    }

    /// <summary>
    /// Returns the split pane's style. Modifying the returned style may not have
    /// an effect until <see cref="SetStyle(SplitPaneStyle)"/> is called.
    /// </summary>
    public SplitPaneStyle GetStyle()
    {
        return _style;
    }

    public void Layout()
    {
        ClampSplitAmount();

        if ( !_vertical )
        {
            CalculateHorizBoundsAndPositions();
        }
        else
        {
            CalculateVertBoundsAndPositions();
        }

        Actor? firstWidget = this._firstWidget;

        if ( firstWidget != null )
        {
            firstWidget.SetBounds( _firstWidgetBounds.X,
                                   _firstWidgetBounds.Y,
                                   _firstWidgetBounds.Width,
                                   _firstWidgetBounds.Height );

            if ( firstWidget is ILayout widget )
            {
                widget.Validate();
            }
        }

        Actor? secondWidget = this._secondWidget;

        if ( secondWidget != null )
        {
            secondWidget.SetBounds( _secondWidgetBounds.X,
                                    _secondWidgetBounds.Y,
                                    _secondWidgetBounds.Width,
                                    _secondWidgetBounds.Height );

            if ( secondWidget is ILayout widget )
            {
                widget.Validate();
            }
        }
    }

    private float GetPrefWidth()  => PrefWidth;
    private float GetPrefHeight() => PrefHeight;

    public override float PrefWidth
    {
        get
        {
            var first = _firstWidget switch
                        {
                            null           => 0,
                            ILayout widget => widget.PrefWidth,
                            _              => _firstWidget.Width
                        };

            var second = _secondWidget switch
                         {
                             null           => 0,
                             ILayout layout => layout.PrefWidth,
                             _              => _secondWidget.Width
                         };

            if ( _vertical )
            {
                return Math.Max( first, second );
            }

            return first + _style.Handle.MinWidth + second;
        }
    }

    public override float PrefHeight
    {
        get
        {
            var first = _firstWidget switch
                        {
                            null           => 0,
                            ILayout widget => widget.PrefHeight,
                            _              => _firstWidget.Height
                        };

            var second = _secondWidget switch
                         {
                             null           => 0,
                             ILayout layout => layout.PrefHeight,
                             _              => _secondWidget.Height
                         };

            if ( !_vertical )
            {
                return Math.Max( first, second );
            }

            return first + _style.Handle.MinHeight + second;
        }
    }

    public override float MinWidth
    {
        get
        {
            var first  = _firstWidget is ILayout layout ? layout.MinWidth : 0;
            var second = _secondWidget is ILayout widget ? widget.MinWidth : 0;

            if ( _vertical )
            {
                return Math.Max( first, second );
            }

            return first + _style.Handle.MinWidth + second;
        }
    }

    public override float MinHeight
    {
        get
        {
            var first  = _firstWidget is ILayout layout ? layout.MinHeight : 0;
            var second = _secondWidget is ILayout widget ? widget.MinHeight : 0;

            if ( !_vertical )
            {
                return Math.Max( first, second );
            }

            return first + _style.Handle.MinHeight + second;
        }
    }

    public void SetVertical( bool vertical )
    {
        if ( this._vertical == vertical )
        {
            return;
        }

        this._vertical = vertical;
        InvalidateHierarchy();
    }

    public bool ISVertical()
    {
        return _vertical;
    }

    private void CalculateHorizBoundsAndPositions()
    {
        IDrawable handle = _style.Handle;

        var height         = Height;
        var availWidth     = Width - handle.MinWidth;
        var leftAreaWidth  = ( availWidth * _splitAmount );
        var rightAreaWidth = availWidth - leftAreaWidth;
        var handleWidth    = handle.MinWidth;

        _firstWidgetBounds.Set( 0, 0, leftAreaWidth, height );
        _secondWidgetBounds.Set( leftAreaWidth + handleWidth, 0, rightAreaWidth, height );
        _handleBounds.Set( leftAreaWidth, 0, handleWidth, height );
    }

    private void CalculateVertBoundsAndPositions()
    {
        IDrawable handle = _style.Handle;

        float width  = Width;
        float height = Height;

        var   availHeight      = height - handle.MinHeight;
        float topAreaHeight    = ( availHeight * _splitAmount );
        var   bottomAreaHeight = availHeight - topAreaHeight;
        float handleHeight     = handle.MinHeight;

        _firstWidgetBounds.Set( 0, height - topAreaHeight, width, topAreaHeight );
        _secondWidgetBounds.Set( 0, 0, width, bottomAreaHeight );
        _handleBounds.Set( 0, bottomAreaHeight, width, handleHeight );
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        Stage? stage = Stage;

        if ( stage == null )
        {
            return;
        }

        Validate();

        Color color = Color;
        var   alpha = color.A * parentAlpha;

        ApplyTransform( batch, ComputeTransform() );

        if ( _firstWidget is { IsVisible: true } )
        {
            batch.Flush();
            stage.CalculateScissors( _firstWidgetBounds, _tempScissors );

            if ( ScissorStack.PushScissors( _tempScissors ) )
            {
                _firstWidget.Draw( batch, alpha );

                batch.Flush();
                ScissorStack.PopScissors();
            }
        }

        if ( _secondWidget is { IsVisible: true } )
        {
            batch.Flush();
            stage.CalculateScissors( _secondWidgetBounds, _tempScissors );

            if ( ScissorStack.PushScissors( _tempScissors ) )
            {
                _secondWidget.Draw( batch, alpha );

                batch.Flush();
                ScissorStack.PopScissors();
            }
        }

        batch.SetColor( color.R, color.G, color.B, alpha );
        _style.Handle.Draw( batch, _handleBounds.X, _handleBounds.Y, _handleBounds.Width, _handleBounds.Height );

        ResetTransform( batch );
    }

    /// <summary>
    /// </summary>
    /// <param name="splitAmount">
    /// The split amount between the min and max amount. This parameter is clamped during layout.
    /// </param>
    /// <seealso cref="ClampSplitAmount()"/>
    public void SetSplitAmount( float splitAmount )
    {
        this._splitAmount = splitAmount;
        Invalidate();
    }

    public float GetSplitAmount()
    {
        return _splitAmount;
    }

    /// <summary>
    /// Called during layout to clamp the <see cref="_splitAmount"/> within the set limits.
    /// By default it imposes the limits of the <see cref="GetMinSplitAmount()"/>",
    /// <see cref="GetMaxSplitAmount()"/>", and min sizes of the children.
    /// This method is internally called in response to layout, so it should not call
    /// <see cref="WidgetGroup.Invalidate()"/>.
    /// </summary>
    protected void ClampSplitAmount()
    {
        float effectiveMinAmount = _minAmount, effectiveMaxAmount = _maxAmount;

        if ( _vertical )
        {
            float availableHeight = Height - _style.Handle.MinHeight;

            if ( _firstWidget is ILayout layout )
            {
                effectiveMinAmount = Math.Max( effectiveMinAmount,
                                               Math.Min( layout.MinHeight / availableHeight, 1 ) );
            }

            if ( _secondWidget is ILayout layout2 )
            {
                effectiveMaxAmount = Math.Min( effectiveMaxAmount,
                                               1 - Math.Min( layout2.MinHeight / availableHeight, 1 ) );
            }
        }
        else
        {
            var availableWidth = Width - _style.Handle.MinWidth;

            if ( _firstWidget is ILayout layout )
            {
                effectiveMinAmount = Math.Max( effectiveMinAmount, Math.Min( layout.MinWidth / availableWidth, 1 ) );
            }

            if ( _secondWidget is ILayout layout2 )
            {
                effectiveMaxAmount = Math.Min( effectiveMaxAmount, 1 - Math.Min( layout2.MinWidth / availableWidth, 1 ) );
            }
        }

        if ( effectiveMinAmount > effectiveMaxAmount ) // Locked handle. Average the position.
        {
            _splitAmount = 0.5f * ( effectiveMinAmount + effectiveMaxAmount );
        }
        else
        {
            _splitAmount = Math.Max( Math.Min( _splitAmount, effectiveMaxAmount ), effectiveMinAmount );
        }
    }

    public float GetMinSplitAmount()
    {
        return _minAmount;
    }

    public void SetMinSplitAmount( float minAmount )
    {
        if ( minAmount is < 0 or > 1 )
        {
            throw new GdxRuntimeException( "minAmount has to be >= 0 and <= 1" );
        }

        this._minAmount = minAmount;
    }

    public float GetMaxSplitAmount()
    {
        return _maxAmount;
    }

    public void SetMaxSplitAmount( float maxAmount )
    {
        if ( maxAmount is < 0 or > 1 )
        {
            throw new GdxRuntimeException( "maxAmount has to be >= 0 and <= 1" );
        }

        this._maxAmount = maxAmount;
    }

    public void SetFirstWidget( Actor? widget )
    {
        if ( _firstWidget != null )
        {
            base.RemoveActor( _firstWidget, unfocus: true );
        }

        _firstWidget = widget;

        if ( widget != null )
        {
            base.AddActor( widget );
        }

        Invalidate();
    }

    public void SetSecondWidget( Actor? widget )
    {
        if ( _secondWidget != null )
        {
            base.RemoveActor( _secondWidget, unfocus: true );
        }

        _secondWidget = widget;

        if ( widget != null )
        {
            base.AddActor( widget );
        }

        Invalidate();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete( "SplitPane.AddActor is not supported, use SplitPane.SetWidget instead." )]
    public override void AddActor( Actor actor )
    {
        throw new UnsupportedOperationException( "Use SplitPane#setWidget." );
    }

    [Obsolete( "SplitPane.AddActorAt is not supported, use SplitPane.SetWidget instead." )]
    public override void AddActorAt( int index, Actor actor )
    {
        throw new UnsupportedOperationException( "Use SplitPane#setWidget." );
    }

    [Obsolete( "SplitPane.AddActorBefore is not supported, use SplitPane.SetWidget instead." )]
    public override void AddActorBefore( Actor actorBefore, Actor actor )
    {
        throw new UnsupportedOperationException( "Use SplitPane#setWidget." );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public bool RemoveActor( Actor actor )
    {
        if ( actor == null )
        {
            throw new ArgumentException( "actor cannot be null." );
        }

        if ( actor == _firstWidget )
        {
            SetFirstWidget( null );

            return true;
        }

        if ( actor == _secondWidget )
        {
            SetSecondWidget( null );

            return true;
        }

        return true;
    }

    public override bool RemoveActor( Actor actor, bool unfocus )
    {
        if ( actor == null )
        {
            throw new ArgumentException( "actor cannot be null." );
        }

        if ( actor == _firstWidget )
        {
            base.RemoveActor( actor, unfocus );
            _firstWidget = null;
            Invalidate();

            return true;
        }

        if ( actor == _secondWidget )
        {
            base.RemoveActor( actor, unfocus );
            _secondWidget = null;
            Invalidate();

            return true;
        }

        return false;
    }

    public override Actor RemoveActorAt( int index, bool unfocus )
    {
        Actor actor = base.RemoveActorAt( index, unfocus );

        if ( actor == _firstWidget )
        {
            base.RemoveActor( actor, unfocus );
            _firstWidget = null;
            Invalidate();
        }
        else if ( actor == _secondWidget )
        {
            base.RemoveActor( actor, unfocus );
            _secondWidget = null;
            Invalidate();
        }

        return actor;
    }

    public bool ISCursorOverHandle()
    {
        return _cursorOverHandle;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    public class SplitPaneInputListener : InputListener
    {
        private readonly SplitPane _parent;
        private          int       _draggingPointer = -1;

        public SplitPaneInputListener( SplitPane parent )
        {
            _parent = parent;
        }

        public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
        {
            if ( _draggingPointer != -1 )
            {
                return false;
            }

            if ( ( pointer == 0 ) && ( button != 0 ) )
            {
                return false;
            }

            if ( _parent._handleBounds.Contains( x, y ) )
            {
                _draggingPointer = pointer;

                _parent._lastPoint.Set( x, y );
                _parent._handlePosition.Set( _parent._handleBounds.X, _parent._handleBounds.Y );

                return true;
            }

            return false;
        }

        public override void TouchUp( InputEvent? ev, float x, float y, int pointer, int button )
        {
            if ( pointer == _draggingPointer )
            {
                _draggingPointer = -1;
            }
        }

        public override void TouchDragged( InputEvent? ev, float x, float y, int pointer )
        {
            if ( pointer != _draggingPointer )
            {
                return;
            }

            IDrawable handle = _parent._style.Handle;

            if ( !_parent._vertical )
            {
                var delta      = x - _parent._lastPoint.X;
                var availWidth = _parent.Width - handle.MinWidth;
                var dragX      = _parent._handlePosition.X + delta;

                _parent._handlePosition.X = dragX;

                dragX = Math.Max( 0, dragX );
                dragX = Math.Min( availWidth, dragX );

                _parent._splitAmount = dragX / availWidth;
                _parent._lastPoint.Set( x, y );
            }
            else
            {
                var delta       = y - _parent._lastPoint.Y;
                var availHeight = _parent.Height - handle.MinHeight;
                var dragY       = _parent._handlePosition.Y + delta;

                _parent._handlePosition.Y = dragY;
                
                dragY = Math.Max( 0, dragY );
                dragY = Math.Min( availHeight, dragY );
                
                _parent._splitAmount = 1 - ( dragY / availHeight );
                _parent._lastPoint.Set( x, y );
            }

            _parent.Invalidate();
        }

        public override bool MouseMoved( InputEvent? ev, float x, float y )
        {
            _parent._cursorOverHandle = _parent._handleBounds.Contains( x, y );

            return false;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class SplitPaneStyle
    {
        public IDrawable Handle { get; private set; }

        public SplitPaneStyle()
        {
            this.Handle = null!;
        }

        public SplitPaneStyle( IDrawable handle )
        {
            this.Handle = handle;
        }

        public SplitPaneStyle( SplitPaneStyle style )
        {
            Handle = style.Handle;
        }
    }
}
