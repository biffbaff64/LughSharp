// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Graphics.G2D;
using LibGDXSharp.LibCore.Maths;
using LibGDXSharp.LibCore.Scenes.Scene2D.Listeners;
using LibGDXSharp.LibCore.Scenes.Scene2D.Utils;
using LibGDXSharp.LibCore.Utils;

namespace LibGDXSharp.LibCore.Scenes.Scene2D.UI;

[PublicAPI]
public class SplitPane : WidgetGroup
{
    private readonly RectangleShape _firstWidgetBounds  = new();
    private readonly RectangleShape _handleBounds       = new();
    private readonly Vector2        _handlePosition     = new();
    private readonly Vector2        _lastPoint          = new();
    private readonly RectangleShape _secondWidgetBounds = new();
    private readonly RectangleShape _tempScissors       = new();
    private          bool           _cursorOverHandle;
    private          Actor?         _firstWidget;
    private          float          _maxAmount = 1;
    private          float          _minAmount;
    private          Actor?         _secondWidget;
    private          float          _splitAmount = 0.5f;
    private          SplitPaneStyle _style       = null!;
    private          bool           _vertical;

    public SplitPane( Actor? firstWidget, Actor? secondWidget, bool vertical, Skin skin )
        : this( firstWidget,
                secondWidget,
                vertical,
                skin,
                $"default-{( vertical ? "vertical" : "horizontal" )}" )
    {
    }

    public SplitPane( Actor? firstWidget, Actor? secondWidget, bool vertical, Skin skin, String styleName )
        : this( firstWidget,
                secondWidget,
                vertical,
                skin.Get< SplitPaneStyle >( styleName ) )
    {
    }

    public SplitPane( Actor? firstWidget, Actor? secondWidget, bool vertical, SplitPaneStyle style )
    {
        _vertical = vertical;

        SetStyle( style );
        SetFirstWidget( firstWidget );
        SetSecondWidget( secondWidget );
        SetSize( GetPrefWidth(), GetPrefHeight() );
        Initialise();
    }

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

    private void Initialise() => AddListener( new SplitPaneInputListener( this ) );

    public void SetStyle( SplitPaneStyle style )
    {
        _style = style;
        InvalidateHierarchy();
    }

    /// <summary>
    ///     Returns the split pane's style. Modifying the returned style may not have
    ///     an effect until <see cref="SetStyle(SplitPaneStyle)" /> is called.
    /// </summary>
    public SplitPaneStyle GetStyle() => _style;

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

        Actor? firstWidget = _firstWidget;

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

        Actor? secondWidget = _secondWidget;

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

    public void SetVertical( bool vertical )
    {
        if ( _vertical == vertical )
        {
            return;
        }

        _vertical = vertical;
        InvalidateHierarchy();
    }

    public bool ISVertical() => _vertical;

    private void CalculateHorizBoundsAndPositions()
    {
        IDrawable handle = _style.Handle;

        var height         = Height;
        var availWidth     = Width - handle.MinWidth;
        var leftAreaWidth  = availWidth * _splitAmount;
        var rightAreaWidth = availWidth - leftAreaWidth;
        var handleWidth    = handle.MinWidth;

        _firstWidgetBounds.Set( 0, 0, leftAreaWidth, height );
        _secondWidgetBounds.Set( leftAreaWidth + handleWidth, 0, rightAreaWidth, height );
        _handleBounds.Set( leftAreaWidth, 0, handleWidth, height );
    }

    private void CalculateVertBoundsAndPositions()
    {
        IDrawable handle = _style.Handle;

        var width  = Width;
        var height = Height;

        var availHeight      = height - handle.MinHeight;
        var topAreaHeight    = availHeight * _splitAmount;
        var bottomAreaHeight = availHeight - topAreaHeight;
        var handleHeight     = handle.MinHeight;

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
    ///     The split amount between the min and max amount. This parameter is clamped during layout.
    /// </param>
    /// <seealso cref="ClampSplitAmount()" />
    public void SetSplitAmount( float splitAmount )
    {
        _splitAmount = splitAmount;
        Invalidate();
    }

    public float GetSplitAmount() => _splitAmount;

    /// <summary>
    ///     Called during layout to clamp the <see cref="_splitAmount" /> within the set limits.
    ///     By default it imposes the limits of the <see cref="GetMinSplitAmount()" />",
    ///     <see cref="GetMaxSplitAmount()" />", and min sizes of the children.
    ///     This method is internally called in response to layout, so it should not call
    ///     <see cref="WidgetGroup.Invalidate()" />.
    /// </summary>
    protected void ClampSplitAmount()
    {
        float effectiveMinAmount = _minAmount, effectiveMaxAmount = _maxAmount;

        if ( _vertical )
        {
            var availableHeight = Height - _style.Handle.MinHeight;

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

    public float GetMinSplitAmount() => _minAmount;

    public void SetMinSplitAmount( float minAmount )
    {
        if ( minAmount is < 0 or > 1 )
        {
            throw new GdxRuntimeException( "minAmount has to be >= 0 and <= 1" );
        }

        _minAmount = minAmount;
    }

    public float GetMaxSplitAmount() => _maxAmount;

    public void SetMaxSplitAmount( float maxAmount )
    {
        if ( maxAmount is < 0 or > 1 )
        {
            throw new GdxRuntimeException( "maxAmount has to be >= 0 and <= 1" );
        }

        _maxAmount = maxAmount;
    }

    public void SetFirstWidget( Actor? widget )
    {
        if ( _firstWidget != null )
        {
            base.RemoveActor( _firstWidget, true );
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
            base.RemoveActor( _secondWidget, true );
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
    public override void AddActor( Actor actor ) => throw new GdxRuntimeException( "SplitPane.AddActor is not supported, use SplitPane.SetWidget instead." );

    [Obsolete( "SplitPane.AddActorAt is not supported, use SplitPane.SetWidget instead." )]
    public override void AddActorAt( int index, Actor actor )
        => throw new GdxRuntimeException( "SplitPane.AddActorAt is not supported, use SplitPane.SetWidget instead." );

    [Obsolete( "SplitPane.AddActorBefore is not supported, use SplitPane.SetWidget instead." )]
    public override void AddActorBefore( Actor actorBefore, Actor actor )
        => throw new GdxRuntimeException( "SplitPane.AddActorBefore is not supported, use SplitPane.SetWidget instead." );

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

    public bool ISCursorOverHandle() => _cursorOverHandle;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class SplitPaneInputListener : InputListener
    {
        private readonly SplitPane _parent;
        private          int       _draggingPointer = -1;

        public SplitPaneInputListener( SplitPane parent ) => _parent = parent;

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
        public SplitPaneStyle() => Handle = null!;

        public SplitPaneStyle( IDrawable handle ) => Handle = handle;

        public SplitPaneStyle( SplitPaneStyle style ) => Handle = style.Handle;

        public IDrawable Handle { get; }
    }
}
