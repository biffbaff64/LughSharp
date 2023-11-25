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

using Color = LibGDXSharp.Graphics.Color;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[PublicAPI]
public class ScrollPane : WidgetGroup
{
    private ScrollPaneStyle      _style;
    private Actor?               _widget;
    private RectangleShape       _widgetArea        = new();
    private RectangleShape       _hScrollBounds     = new();
    private RectangleShape       _hKnobBounds       = new();
    private RectangleShape       _vScrollBounds     = new();
    private RectangleShape       _vKnobBounds       = new();
    private RectangleShape       _widgetCullingArea = new();
    private ActorGestureListener _flickScrollListener;

    private bool    _scrollX;
    private bool    _scrollY;
    private bool    _vScrollOnRight  = true;
    private bool    _hScrollOnBottom = true;
    private float   _amountX;
    private float   _amountY;
    private float   _visualAmountX;
    private float   _visualAmountY;
    private float   _maxX;
    private float   _maxY;
    private bool    _touchScrollH;
    private bool    _touchScrollV;
    private Vector2 _lastPoint       = new();
    private bool    _fadeScrollBars  = true;
    private bool    _smoothScrolling = true;
    private bool    _scrollBarTouch  = true;
    private float   _fadeAlpha;
    private float   _fadeAlphaSeconds = 1;
    private float   _fadeDelay;
    private float   _fadeDelaySeconds = 1;
    private bool    _cancelTouchFocus = true;
    private bool    _flickScroll      = true;
    private float   _flingTime        = 1f;
    private float   _flingTimer;
    private float   _velocityX;
    private float   _velocityY;
    private bool    _overscrollX        = true;
    private bool    _overscrollY        = true;
    private float   _overscrollDistance = 50;
    private float   _overscrollSpeedMin = 30;
    private float   _overscrollSpeedMax = 200;
    private bool    _forceScrollX;
    private bool    _forceScrollY;
    private bool    _disableX;
    private bool    _disableY;
    private bool    _clamp = true;
    private bool    _scrollbarsOnTop;
    private bool    _variableSizeKnobs = true;
    private int     _draggingPointer   = -1;

    public ScrollPane( Actor? widget )
        : this( widget, new ScrollPaneStyle() )
    {
    }

    public ScrollPane( Actor? widget, Skin skin )
        : this( widget, skin.Get< ScrollPaneStyle >() )
    {
    }

    public ScrollPane( Actor? widget, Skin skin, String styleName )
        : this( widget, skin.Get< ScrollPaneStyle >( styleName ) )

    {
    }

    public ScrollPane( Actor? widget, ScrollPaneStyle style )
    {
        ArgumentNullException.ThrowIfNull( style, "style cannot be null." );

        this._style = style;

        SetActor( widget );
        SetSize( 150, 150 );

        this._flickScrollListener = GetFlickScrollListener();

        AddCaptureListener();
        AddListener( this._flickScrollListener );
        AddScrollListener();
    }

    //TODO:
    protected void AddCaptureListener()
    {
//        addCaptureListener( new InputListener() {
//
//    private float handlePosition;
//
//    public bool touchDown( InputEvent event, float x, float y, int pointer, int button) {
//        if ( draggingPointer != -1 )
//        {
//            return false;
//        }
//
//        if ( pointer == 0 && button != 0 )
//        {
//            return false;
//        }
//
//        if ( getStage() != null )
//        {
//            getStage().setScrollFocus( ScrollPane.this );
//        }
//
//        if ( !flickScroll )
//        {
//            setScrollbarsVisible( true );
//        }
//
//        if ( fadeAlpha == 0 )
//        {
//            return false;
//        }
//
//        if ( scrollBarTouch && scrollX && hScrollBounds.contains( x, y ) )
//        {
//            event.stop();
//            setScrollbarsVisible( true );
//
//            if ( hKnobBounds.contains( x, y ) )
//            {
//                lastPoint.Set( x, y );
//                handlePosition  = hKnobBounds.X;
//                touchScrollH    = true;
//                draggingPointer = pointer;
//
//                return true;
//            }
//
//            setScrollX( amountX + widgetArea.Width * ( x < hKnobBounds.X ? -1 : 1 ) );
//
//            return true;
//        }
//
//        if ( scrollBarTouch && scrollY && vScrollBounds.contains( x, y ) )
//        {
//            event.stop();
//            setScrollbarsVisible( true );
//
//            if ( vKnobBounds.contains( x, y ) )
//            {
//                lastPoint.Set( x, y );
//                handlePosition  = vKnobBounds.Y;
//                touchScrollV    = true;
//                draggingPointer = pointer;
//
//                return true;
//            }
//
//            setScrollY( amountY + widgetArea.Height * ( y < vKnobBounds.Y ? 1 : -1 ) );
//
//            return true;
//        }
//
//        return false;
//    }
//
//    public void touchUp( InputEvent event, float x, float y, int pointer, int button) {
//        if ( pointer != draggingPointer )
//        {
//            return;
//        }
//
//        cancel();
//    }
//
//    public void touchDragged( InputEvent event, float x, float y, int pointer) {
//        if ( pointer != draggingPointer )
//        {
//            return;
//        }
//
//        if ( touchScrollH )
//        {
//            float delta   = x - lastPoint.X;
//            float scrollH = handlePosition + delta;
//            handlePosition = scrollH;
//            scrollH        = Math.Max( hScrollBounds.X, scrollH );
//            scrollH        = Math.Min( hScrollBounds.X + hScrollBounds.Width - hKnobBounds.Width, scrollH );
//            float total = hScrollBounds.Width - hKnobBounds.Width;
//            if ( total != 0 )
//            {
//                setScrollPercentX( ( scrollH - hScrollBounds.X ) / total );
//            }
//
//            lastPoint.Set( x, y );
//        }
//        else if ( touchScrollV )
//        {
//            float delta   = y - lastPoint.Y;
//            float scrollV = handlePosition + delta;
//            handlePosition = scrollV;
//            scrollV        = Math.Max( vScrollBounds.Y, scrollV );
//            scrollV        = Math.Min( vScrollBounds.Y + vScrollBounds.Height - vKnobBounds.Height, scrollV );
//            float total = vScrollBounds.Height - vKnobBounds.Height;
//            if ( total != 0 )
//            {
//                setScrollPercentY( 1 - ( ( scrollV - vScrollBounds.Y ) / total ) );
//            }
//
//            lastPoint.Set( x, y );
//        }
//    }
//
//    public bool mouseMoved( InputEvent event, float x, float y) {
//        if ( !flickScroll )
//        {
//            setScrollbarsVisible( true );
//        }
//
//        return false;
//    }
//});
//
    }

    //TODO:
    protected ActorGestureListener GetFlickScrollListener()
    {
        return _flickScrollListener;

//    return new ActorGestureListener()
//    {
//        public void pan (InputEvent event, float x, float y, float deltaX, float deltaY) {
//        setScrollbarsVisible(true);
//        amountX -= deltaX;
//        amountY += deltaY;
//        Clamp();
//        if (cancelTouchFocus && ((scrollX && deltaX != 0) || (scrollY && deltaY != 0))) cancelTouchFocus();
//    }
//
//    public void fling( InputEvent event, float x, float y, int button )
//    {
//        if ( Math.abs( x ) > 150 && scrollX )
//        {
//            flingTimer = flingTime;
//            velocityX  = x;
//            if ( cancelTouchFocus )
//            {
//                cancelTouchFocus();
//            }
//        }
//
//        if ( Math.abs( y ) > 150 && scrollY )
//        {
//            flingTimer = flingTime;
//            velocityY  = -y;
//            if ( cancelTouchFocus )
//            {
//                cancelTouchFocus();
//            }
//        }
//    }
//
//    public bool handle( Event event )
//    {
//        if ( base.handle(  event )) {
//            if ( ( ( InputEvent )event ).getType() == InputEvent.Type.touchDown) flingTimer = 0;
//
//            return true;
//        } else if (event instanceof InputEvent && ( ( InputEvent )event).isTouchFocusCancel()) //
//        cancel();
//
//        return false;
//    }
//
//    };
    }

    //TODO:
    protected void AddScrollListener()
    {
//    addListener( new InputListener()
//    {
//        public bool scrolled (InputEvent event, float x, float y, float scrollAmountX, float scrollAmountY) {
//        setScrollbarsVisible(true);
//        if (scrollY || scrollX) {
//        if (scrollY) {
//        if (!scrollX && scrollAmountY == 0) scrollAmountY = scrollAmountX;
//    } else {
//        if ( scrollX && scrollAmountX == 0 )
//        {
//            scrollAmountX = scrollAmountY;
//        }
//    }

//    setScrollY( amountY + getMouseWheelY() * scrollAmountY );
//    setScrollX( amountX + getMouseWheelX() * scrollAmountX );
//    } else

//    return false;
//    return true;
//    }
//    });
    }

    /// <summary>
    /// Shows or hides the scrollbars for when using <see cref="SetFadeScrollBars(bool)"/>"
    /// </summary>
    public void SetScrollbarsVisible( bool visible )
    {
        if ( visible )
        {
            _fadeAlpha = _fadeAlphaSeconds;
            _fadeDelay = _fadeDelaySeconds;
        }
        else
        {
            _fadeAlpha = 0;
            _fadeDelay = 0;
        }
    }

    /// <summary>
    /// Cancels the stage's touch focus for all listeners except this scroll
    /// pane's flick scroll listener. This causes any widgets inside the
    /// scrollpane that have received touchDown to receive touchUp.
    /// </summary>
    public void CancelTouchFocus()
    {
        if ( Stage != null )
        {
            Stage.CancelTouchFocusExcept( _flickScrollListener, this );
        }
    }

    /// If currently scrolling by tracking a touch down, stop scrolling.
    public void Cancel()
    {
        _draggingPointer = -1;
        _touchScrollH    = false;
        _touchScrollV    = false;
        _flickScrollListener.GetGestureDetector().Cancel();
    }

    private void Clamp()
    {
        if ( !_clamp )
        {
            return;
        }

        ScrollX( _overscrollX
                     ? MathUtils.Clamp( _amountX, -_overscrollDistance, _maxX + _overscrollDistance )
                     : MathUtils.Clamp( _amountX, 0, _maxX ) );

        ScrollY( _overscrollY
                     ? MathUtils.Clamp( _amountY, -_overscrollDistance, _maxY + _overscrollDistance )
                     : MathUtils.Clamp( _amountY, 0, _maxY ) );
    }

    public void SetStyle( ScrollPaneStyle? style )
    {
        ArgumentNullException.ThrowIfNull( style, "style cannot be null." );

        this._style = style;
        InvalidateHierarchy();
    }

    /// <summary>
    /// Returns the scroll pane's style. Modifying the returned style may
    /// not have an effect until <see cref="SetStyle(ScrollPaneStyle)"/> is called.
    /// </summary>
    public ScrollPaneStyle GetStyle()
    {
        return _style;
    }

    public override void Act( float delta )
    {
        base.Act( delta );

        var panning   = _flickScrollListener.GetGestureDetector().IsPanning();
        var animating = false;

        if ( ( _fadeAlpha > 0 ) && _fadeScrollBars && !panning && !_touchScrollH && !_touchScrollV )
        {
            _fadeDelay -= delta;

            if ( _fadeDelay <= 0 )
            {
                _fadeAlpha = Math.Max( 0, _fadeAlpha - delta );
            }

            animating = true;
        }

        if ( _flingTimer > 0 )
        {
            SetScrollbarsVisible( true );

            var alpha = _flingTimer / _flingTime;

            _amountX -= _velocityX * alpha * delta;
            _amountY -= _velocityY * alpha * delta;

            Clamp();

            // Stop fling if hit overscroll distance.
            if ( _amountX.Equals( -_overscrollDistance ) )
            {
                _velocityX = 0;
            }

            if ( _amountX >= ( _maxX + _overscrollDistance ) )
            {
                _velocityX = 0;
            }

            if ( _amountY.Equals( -_overscrollDistance ) )
            {
                _velocityY = 0;
            }

            if ( _amountY >= ( _maxY + _overscrollDistance ) )
            {
                _velocityY = 0;
            }

            _flingTimer -= delta;

            if ( _flingTimer <= 0 )
            {
                _velocityX = 0;
                _velocityY = 0;
            }

            animating = true;
        }

        // Scroll smoothly when grabbing the scrollbar if one pixel of scrollbar movement is > 10% of the scroll area.

        //TODO:
        if ( _smoothScrolling
          && ( _flingTimer <= 0 )
          && !panning
          && ( ( !_touchScrollH || ( _scrollX && ( ( _maxX / ( _hScrollBounds.Width - _hKnobBounds.Width ) ) > ( _widgetArea.Width * 0.1f ) ) ) )
            && ( !_touchScrollV || ( _scrollY && ( ( _maxY / ( _vScrollBounds.Height - _vKnobBounds.Height ) ) > ( _widgetArea.Height * 0.1f ) ) ) ) ) //
           )
        {
            if ( !_visualAmountX.Equals( _amountX ) )
            {
                //TODO:
                VisualScrollX( _visualAmountX < _amountX
                                   ? Math.Min( _amountX, _visualAmountX + Math.Max( 200 * delta, ( _amountX - _visualAmountX ) * 7 * delta ) )
                                   : Math.Max( _amountX, _visualAmountX - Math.Max( 200 * delta, ( _visualAmountX - _amountX ) * 7 * delta ) ) );

                animating = true;
            }

            if ( !_visualAmountY.Equals( _amountY ) )
            {
                //TODO:
                VisualScrollY( _visualAmountY < _amountY
                                   ? Math.Min( _amountY, _visualAmountY + Math.Max( 200 * delta, ( _amountY - _visualAmountY ) * 7 * delta ) )
                                   : Math.Max( _amountY, _visualAmountY - Math.Max( 200 * delta, ( _visualAmountY - _amountY ) * 7 * delta ) ) );

                animating = true;
            }
        }
        else
        {
            if ( !_visualAmountX.Equals( _amountX ) )
            {
                VisualScrollX( _amountX );
            }

            if ( !_visualAmountY.Equals( _amountY ) )
            {
                VisualScrollY( _amountY );
            }
        }

        if ( !panning )
        {
            if ( _overscrollX && _scrollX )
            {
                if ( _amountX < 0 )
                {
                    SetScrollbarsVisible( true );

                    _amountX += ( _overscrollSpeedMin
                                + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                    * -_amountX )
                                  / _overscrollDistance ) )
                              * delta;

                    if ( _amountX > 0 )
                    {
                        ScrollX( 0 );
                    }

                    animating = true;
                }
                else if ( _amountX > _maxX )
                {
                    SetScrollbarsVisible( true );

                    _amountX -= ( _overscrollSpeedMin
                                + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                    * -( _maxX - _amountX ) )
                                  / _overscrollDistance ) )
                              * delta;

                    if ( _amountX < _maxX )
                    {
                        ScrollX( _maxX );
                    }

                    animating = true;
                }
            }

            if ( _overscrollY && _scrollY )
            {
                if ( _amountY < 0 )
                {
                    SetScrollbarsVisible( true );

                    _amountY += ( _overscrollSpeedMin
                                + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                    * -_amountY )
                                  / _overscrollDistance ) )
                              * delta;

                    if ( _amountY > 0 )
                    {
                        ScrollY( 0 );
                    }

                    animating = true;
                }
                else if ( _amountY > _maxY )
                {
                    SetScrollbarsVisible( true );

                    _amountY -= ( _overscrollSpeedMin
                                + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                    * -( _maxY - _amountY ) )
                                  / _overscrollDistance ) )
                              * delta;

                    if ( _amountY < _maxY )
                    {
                        ScrollY( _maxY );
                    }

                    animating = true;
                }
            }
        }

        if ( animating )
        {
//            if ( ( Stage != null ) && Stage.ActionsRequestRendering )
            if ( Stage is { ActionsRequestRendering: true } )
            {
                Gdx.Graphics.RequestRendering();
            }
        }
    }

    public override void Layout()
    {
        IDrawable? bg             = _style.Background;
        IDrawable? hScrollKnob    = _style.HScrollKnob;
        IDrawable? vScrollKnob    = _style.VScrollKnob;
        float      bgLeftWidth    = 0;
        float      bgRightWidth   = 0;
        float      bgTopHeight    = 0;
        float      bgBottomHeight = 0;

        if ( bg != null )
        {
            //TODO:
            bgLeftWidth    = bg.LeftWidth;
            bgRightWidth   = bg.RightWidth;
            bgTopHeight    = bg.TopHeight;
            bgBottomHeight = bg.BottomHeight;
        }

        //TODO:
        var width  = Width;
        var height = Height;

        _widgetArea.Set( bgLeftWidth,
                         bgBottomHeight,
                         width - bgLeftWidth - bgRightWidth,
                         height - bgTopHeight - bgBottomHeight );

        if ( _widget == null )
        {
            return;
        }

        float scrollbarHeight = 0;
        float scrollbarWidth  = 0;

        if ( hScrollKnob != null )
        {
            scrollbarHeight = hScrollKnob.MinHeight;
        }

        if ( _style.HScroll != null )
        {
            scrollbarHeight = Math.Max( scrollbarHeight, _style.HScroll.MinHeight );
        }

        if ( vScrollKnob != null )
        {
            scrollbarWidth = vScrollKnob.MinWidth;
        }

        if ( _style.VScroll != null )
        {
            scrollbarWidth = Math.Max( scrollbarWidth, _style.VScroll.MinWidth );
        }

        // Get widget's desired width.
        float widgetWidth;
        float widgetHeight;

        if ( _widget is ILayout layout )
        {
            widgetWidth  = layout.PrefWidth;
            widgetHeight = layout.PrefHeight;
        }
        else
        {
            widgetWidth  = _widget.Width;
            widgetHeight = _widget.Height;
        }

        // Determine if horizontal/vertical scrollbars are needed.
        _scrollX = _forceScrollX || ( ( widgetWidth > _widgetArea.Width ) && !_disableX );
        _scrollY = _forceScrollY || ( ( widgetHeight > _widgetArea.Height ) && !_disableY );

        // Adjust widget area for scrollbar sizes and check if it causes the other scrollbar to show.
        if ( !_scrollbarsOnTop )
        {
            if ( _scrollY )
            {
                _widgetArea.Width -= scrollbarWidth;

                if ( !_vScrollOnRight )
                {
                    _widgetArea.X += scrollbarWidth;
                }

                // Horizontal scrollbar may cause vertical scrollbar to show.
                if ( !_scrollX && ( widgetWidth > _widgetArea.Width ) && !_disableX )
                {
                    _scrollX = true;
                }
            }

            if ( _scrollX )
            {
                _widgetArea.Height -= scrollbarHeight;

                if ( _hScrollOnBottom )
                {
                    _widgetArea.Y += scrollbarHeight;
                }

                // Vertical scrollbar may cause horizontal scrollbar to show.
                if ( !_scrollY && ( widgetHeight > _widgetArea.Height ) && !_disableY )
                {
                    _scrollY          =  true;
                    _widgetArea.Width -= scrollbarWidth;

                    if ( !_vScrollOnRight )
                    {
                        _widgetArea.X += scrollbarWidth;
                    }
                }
            }
        }

        // If the widget is smaller than the available space, make it take up the available space.
        widgetWidth  = _disableX ? _widgetArea.Width : Math.Max( _widgetArea.Width, widgetWidth );
        widgetHeight = _disableY ? _widgetArea.Height : Math.Max( _widgetArea.Height, widgetHeight );

        _maxX = widgetWidth - _widgetArea.Width;
        _maxY = widgetHeight - _widgetArea.Height;

        ScrollX( MathUtils.Clamp( _amountX, 0, _maxX ) );
        ScrollY( MathUtils.Clamp( _amountY, 0, _maxY ) );

        // Set the scrollbar and knob bounds.
        if ( _scrollX )
        {
            if ( hScrollKnob != null )
            {
                var x = _scrollbarsOnTop ? bgLeftWidth : _widgetArea.X;
                var y = _hScrollOnBottom ? bgBottomHeight : height - bgTopHeight - scrollbarHeight;

                _hScrollBounds.Set( x, y, _widgetArea.Width, scrollbarHeight );

                if ( _scrollY && _scrollbarsOnTop )
                {
                    _hScrollBounds.Width -= scrollbarWidth;

                    if ( !_vScrollOnRight )
                    {
                        _hScrollBounds.X += scrollbarWidth;
                    }
                }

                if ( _variableSizeKnobs )
                {
                    _hKnobBounds.Width = Math.Max( hScrollKnob.MinWidth,
                                                   ( int )( ( _hScrollBounds.Width * _widgetArea.Width ) / widgetWidth ) );
                }
                else
                {
                    _hKnobBounds.Width = hScrollKnob.MinWidth;
                }

                if ( _hKnobBounds.Width > widgetWidth )
                {
                    _hKnobBounds.Width = 0;
                }

                _hKnobBounds.Height = hScrollKnob.MinHeight;
                _hKnobBounds.X      = _hScrollBounds.X + ( int )( ( _hScrollBounds.Width - _hKnobBounds.Width ) * GetScrollPercentX() );
                _hKnobBounds.Y      = _hScrollBounds.Y;
            }
            else
            {
                _hScrollBounds.Set( 0, 0, 0, 0 );
                _hKnobBounds.Set( 0, 0, 0, 0 );
            }
        }

        if ( _scrollY )
        {
            if ( vScrollKnob != null )
            {
                var x = _vScrollOnRight ? width - bgRightWidth - scrollbarWidth : bgLeftWidth;
                var y = _scrollbarsOnTop ? bgBottomHeight : _widgetArea.Y;

                _vScrollBounds.Set( x, y, scrollbarWidth, _widgetArea.Height );

                if ( _scrollX && _scrollbarsOnTop )
                {
                    _vScrollBounds.Height -= scrollbarHeight;

                    if ( _hScrollOnBottom )
                    {
                        _vScrollBounds.Y += scrollbarHeight;
                    }
                }

                _vKnobBounds.Width = vScrollKnob.MinWidth;

                if ( _variableSizeKnobs )
                {
                    _vKnobBounds.Height = Math.Max( vScrollKnob.MinHeight,
                                                    ( int )( ( _vScrollBounds.Height * _widgetArea.Height ) / widgetHeight ) );
                }
                else
                {
                    _vKnobBounds.Height = vScrollKnob.MinHeight;
                }

                if ( _vKnobBounds.Height > widgetHeight )
                {
                    _vKnobBounds.Height = 0;
                }

                _vKnobBounds.X = _vScrollOnRight ? width - bgRightWidth - vScrollKnob.MinWidth : bgLeftWidth;
                _vKnobBounds.Y = _vScrollBounds.Y + ( int )( ( _vScrollBounds.Height - _vKnobBounds.Height ) * ( 1 - GetScrollPercentY() ) );
            }
            else
            {
                _vScrollBounds.Set( 0, 0, 0, 0 );
                _vKnobBounds.Set( 0, 0, 0, 0 );
            }
        }

        UpdateWidgetPosition();

        if ( _widget is ILayout widgetLayout )
        {
            _widget.SetSize( widgetWidth, widgetHeight );
            widgetLayout.Validate();
        }
    }

    private void UpdateWidgetPosition()
    {
        // Calculate the widget's position depending on the scroll state and available widget area.
        var x = _widgetArea.X - ( _scrollX ? ( int )_visualAmountX : 0 );
        var y = _widgetArea.Y - ( int )( _scrollY ? _maxY - _visualAmountY : _maxY );

        _widget?.SetPosition( x, y );

        if ( _widget is ICullable cullable )
        {
            _widgetCullingArea.X      = _widgetArea.X - x;
            _widgetCullingArea.Y      = _widgetArea.Y - y;
            _widgetCullingArea.Width  = _widgetArea.Width;
            _widgetCullingArea.Height = _widgetArea.Height;
            cullable.CullingArea      = _widgetCullingArea;
        }
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        if ( _widget == null )
        {
            return;
        }

        Validate();

        // Setup transform for this group.
        ApplyTransform( batch, ComputeTransform() );

        if ( _scrollX )
        {
            _hKnobBounds.X = _hScrollBounds.X
                           + ( int )( ( _hScrollBounds.Width - _hKnobBounds.Width )
                                    * GetVisualScrollPercentX() );
        }

        if ( _scrollY )
        {
            _vKnobBounds.Y = _vScrollBounds.Y
                           + ( int )( ( _vScrollBounds.Height - _vKnobBounds.Height )
                                    * ( 1 - GetVisualScrollPercentY() ) );
        }

        UpdateWidgetPosition();

        // Draw the background ninepatch.
        Color? color = Color;
        var    alpha = ( color!.A * parentAlpha );

        if ( _style.Background != null )
        {
            batch.SetColor( color.R, color.G, color.B, alpha );
            _style.Background.Draw( batch, 0, 0, Width, Height );
        }

        batch.Flush();

        if ( ClipBegin( _widgetArea.X, _widgetArea.Y, _widgetArea.Width, _widgetArea.Height ) )
        {
            DrawChildren( batch, parentAlpha );
            batch.Flush();
            ClipEnd();
        }

        // Render scrollbars and knobs on top if they will be visible.
        batch.SetColor( color.R, color.G, color.B, alpha );

        if ( _fadeScrollBars )
        {
            alpha *= Interpolation.fade.Apply( _fadeAlpha / _fadeAlphaSeconds );
        }

        DrawScrollBars( batch, color.R, color.G, color.B, alpha );

        ResetTransform( batch );
    }

    /// <summary>
    /// Renders the scrollbars after the children have been drawn. If the
    /// scrollbars faded out, a is zero and rendering can be skipped.
    /// </summary>
    protected void DrawScrollBars( IBatch batch, float r, float g, float b, float a )
    {
        if ( a <= 0 )
        {
            return;
        }

        batch.SetColor( r, g, b, a );

        var x = _scrollX && ( _hKnobBounds.Width > 0 );
        var y = _scrollY && ( _vKnobBounds.Height > 0 );

        if ( x && y )
        {
            if ( _style.Corner != null )
            {
                _style.Corner.Draw( batch,
                                    ( _hScrollBounds.X + _hScrollBounds.Width ),
                                    _hScrollBounds.Y,
                                    _vScrollBounds.Width,
                                    _vScrollBounds.Y );
            }
        }

        if ( x )
        {
            if ( _style.HScroll != null )
            {
                _style.HScroll.Draw( batch,
                                     _hScrollBounds.X,
                                     _hScrollBounds.Y,
                                     _hScrollBounds.Width,
                                     _hScrollBounds.Height );
            }

            if ( _style.HScrollKnob != null )
            {
                _style.HScrollKnob.Draw( batch,
                                         _hKnobBounds.X,
                                         _hKnobBounds.Y,
                                         _hKnobBounds.Width,
                                         _hKnobBounds.Height );
            }
        }

        if ( y )
        {
            if ( _style.VScroll != null )
            {
                _style.VScroll.Draw( batch,
                                     _vScrollBounds.X,
                                     _vScrollBounds.Y,
                                     _vScrollBounds.Width,
                                     _vScrollBounds.Height );
            }

            if ( _style.VScrollKnob != null )
            {
                _style.VScrollKnob.Draw( batch,
                                         _vKnobBounds.X,
                                         _vKnobBounds.Y,
                                         _vKnobBounds.Width,
                                         _vKnobBounds.Height );
            }
        }
    }

    /// <summary>
    /// Generate fling gesture.
    /// </summary>
    /// <param name="flingTime"> Time in seconds for which you want to fling last. </param>
    /// <param name="velocityX"> Velocity for horizontal direction. </param>
    /// <param name="velocityY"> Velocity for vertical direction. </param>
    public void Fling( float flingTime, float velocityX, float velocityY )
    {
        this._flingTimer = flingTime;
        this._velocityX  = velocityX;
        this._velocityY  = velocityY;
    }

    public float GetPrefWidth()
    {
        float width = 0;

        if ( _widget is ILayout layout )
        {
            width = layout.PrefWidth;
        }
        else if ( _widget != null )
        {
            width = _widget.Width;
        }

        IDrawable? background = _style.Background;

        if ( background != null )
        {
            width = Math.Max( width + background.LeftWidth + background.RightWidth, background.MinWidth );
        }

        if ( _scrollY )
        {
            float scrollbarWidth = 0;

            if ( _style.VScrollKnob != null )
            {
                scrollbarWidth = _style.VScrollKnob.MinWidth;
            }

            if ( _style.VScroll != null )
            {
                scrollbarWidth = Math.Max( scrollbarWidth, _style.VScroll.MinWidth );
            }

            width += scrollbarWidth;
        }

        return width;
    }

    public float GetPrefHeight()
    {
        float height = 0;

        if ( _widget is ILayout layout )
        {
            height = layout.PrefHeight;
        }
        else if ( _widget != null )
        {
            height = _widget.Height;
        }

        IDrawable? background = _style.Background;

        if ( background != null )
        {
            height = Math.Max( height + background.TopHeight + background.BottomHeight, background.MinHeight );
        }

        if ( _scrollX )
        {
            float scrollbarHeight = 0;

            if ( _style.HScrollKnob != null )
            {
                scrollbarHeight = _style.HScrollKnob.MinHeight;
            }

            if ( _style.HScroll != null )
            {
                scrollbarHeight = Math.Max( scrollbarHeight, _style.HScroll.MinHeight );
            }

            height += scrollbarHeight;
        }

        return height;
    }

    public float GetMinWidth()
    {
        return 0;
    }

    public float GetMinHeight()
    {
        return 0;
    }

    /// Sets the {@link Actor} embedded in this scroll pane.
    * @param actor May be null to remove any current actor.
    public void SetActor( Actor? actor )
    {
        if ( _widget == this )
        {
            throw new ArgumentException( "widget cannot be the ScrollPane." );
        }

        if ( this._widget != null )
        {
            base.RemoveActor( this._widget );
        }

        this._widget = actor;

        if ( _widget != null )
        {
            base.AddActor( _widget );
        }
    }

    /// <summary>
    /// Returns the actor embedded in this scroll pane, or null.
    /// </summary>
    public Actor? GetActor()
    {
        return _widget;
    }

    public bool RemoveActor( Actor? actor )
    {
        ArgumentNullException.ThrowIfNull( actor, "actor cannot be null." );

        if ( actor != _widget )
        {
            return false;
        }

        SetActor( null );

        return true;
    }

    public new bool RemoveActor( Actor actor, bool unfocus )
    {
        if ( actor == null )
        {
            throw new ArgumentException( "actor cannot be null." );
        }

        if ( actor != _widget )
        {
            return false;
        }

        this._widget = null;

        return base.RemoveActor( actor, unfocus );
    }

    public new Actor RemoveActorAt( int index, bool unfocus )
    {
        Actor actor = base.RemoveActorAt( index, unfocus );

        if ( actor == _widget )
        {
            this._widget = null;
        }

        return actor;
    }

    public override Actor? Hit( float x, float y, bool touchable )
    {
        if ( ( x < 0 ) || ( x >= Width ) || ( y < 0 ) || ( y >= Height ) )
        {
            return null;
        }

        if ( touchable && ( Touchable == Touchable.Enabled ) && IsVisible )
        {
            if ( _scrollX && _touchScrollH && _hScrollBounds.Contains( x, y ) )
            {
                return this;
            }

            if ( _scrollY && _touchScrollV && _vScrollBounds.Contains( x, y ) )
            {
                return this;
            }
        }

        return base.Hit( x, y, touchable );
    }

    /// <summary>
    /// Called whenever the x scroll amount is changed.
    /// </summary>
    protected void ScrollX( float pixelsX )
    {
        this._amountX = pixelsX;
    }

    /// <summary>
    /// Called whenever the y scroll amount is changed.
    /// </summary>
    protected void ScrollY( float pixelsY )
    {
        this._amountY = pixelsY;
    }

    /// <summary>
    /// Called whenever the visual x scroll amount is changed.
    /// </summary>
    protected void VisualScrollX( float pixelsX )
    {
        this._visualAmountX = pixelsX;
    }

    /// <summary>
    /// Called whenever the visual y scroll amount is changed.
    /// </summary>
    protected void VisualScrollY( float pixelsY )
    {
        this._visualAmountY = pixelsY;
    }

    /// <summary>
    /// Returns the amount to scroll horizontally when the mouse wheel is scrolled.
    /// </summary>
    protected float GetMouseWheelX()
    {
        return Math.Min( _widgetArea.Width, Math.Max( _widgetArea.Width * 0.9f, _maxX * 0.1f ) / 4 );
    }

    /// <summary>
    /// Returns the amount to scroll vertically when the mouse wheel is scrolled.
    /// </summary>
    protected float GetMouseWheelY()
    {
        return Math.Min( _widgetArea.Height, Math.Max( _widgetArea.Height * 0.9f, _maxY * 0.1f ) / 4 );
    }

    public void SetScrollX( float pixels )
    {
        ScrollX( MathUtils.Clamp( pixels, 0, _maxX ) );
    }

    /// <summary>
    /// Returns the x scroll position in pixels, where 0 is the left of the scroll pane.
    /// </summary>
    public float GetScrollX()
    {
        return _amountX;
    }

    public void SetScrollY( float pixels )
    {
        ScrollY( MathUtils.Clamp( pixels, 0, _maxY ) );
    }

    /// <summary>
    /// Returns the y scroll position in pixels, where 0 is the top of the scroll pane.
    /// </summary>
    public float GetScrollY()
    {
        return _amountY;
    }

    /// <summary>
    /// Sets the visual scroll amount equal to the scroll amount. This can
    /// be used when setting the scroll amount without animating.
    /// </summary>
    public void UpdateVisualScroll()
    {
        _visualAmountX = _amountX;
        _visualAmountY = _amountY;
    }

    public float GetVisualScrollX()
    {
        return !_scrollX ? 0 : _visualAmountX;
    }

    public float GetVisualScrollY()
    {
        return !_scrollY ? 0 : _visualAmountY;
    }

    public float GetVisualScrollPercentX()
    {
        if ( _maxX == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( _visualAmountX / _maxX, 0, 1 );
    }

    public float GetVisualScrollPercentY()
    {
        if ( _maxY == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( _visualAmountY / _maxY, 0, 1 );
    }

    public float GetScrollPercentX()
    {
        if ( _maxX == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( _amountX / _maxX, 0, 1 );
    }

    public void SetScrollPercentX( float percentX )
    {
        ScrollX( _maxX * MathUtils.Clamp( percentX, 0, 1 ) );
    }

    public float GetScrollPercentY()
    {
        if ( _maxY == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( _amountY / _maxY, 0, 1 );
    }

    public void SetScrollPercentY( float percentY )
    {
        ScrollY( _maxY * MathUtils.Clamp( percentY, 0, 1 ) );
    }

    public void SetFlickScroll( bool flickScroll )
    {
        if ( this._flickScroll == flickScroll )
        {
            return;
        }

        this._flickScroll = flickScroll;

        if ( flickScroll )
        {
            AddListener( _flickScrollListener );
        }
        else
        {
            RemoveListener( _flickScrollListener );
        }

        Invalidate();
    }

    public void SetFlickScrollTapSquareSize( float halfTapSquareSize )
    {
        _flickScrollListener.GetGestureDetector().SetTapSquareSize( halfTapSquareSize );
    }

    /// <summary>
    /// Sets the scroll offset so the specified rectangle is fully in view,
    /// if possible. Coordinates are in the scroll pane widget's coordinate
    /// system.
    /// </summary>
    public void ScrollTo( float x, float y, float width, float height )
    {
        ScrollTo( x, y, width, height, false, false );
    }

    /// <summary>
    /// Sets the scroll offset so the specified rectangle is fully in view, and
    /// optionally centered vertically and/or horizontally, if possible.
    /// Coordinates are in the scroll pane widget's coordinate system.
    /// </summary>
    public void ScrollTo( float x, float y, float width, float height, bool centerHorizontal, bool centerVertical )
    {
        Validate();

        var amountX = this._amountX;

        if ( centerHorizontal )
        {
            amountX = ( x - ( _widgetArea.Width / 2 ) ) + ( width / 2 );
        }
        else
        {
            if ( ( x + width ) > ( amountX + _widgetArea.Width ) )
            {
                amountX = ( x + width ) - _widgetArea.Width;
            }

            if ( x < amountX )
            {
                amountX = x;
            }
        }

        ScrollX( MathUtils.Clamp( amountX, 0, _maxX ) );

        var amountY = this._amountY;

        if ( centerVertical )
        {
            amountY = ( ( _maxY - y ) + ( _widgetArea.Height / 2 ) ) - ( height / 2 );
        }
        else
        {
            if ( amountY > ( ( _maxY - y - height ) + _widgetArea.Height ) )
            {
                amountY = ( _maxY - y - height ) + _widgetArea.Height;
            }

            if ( amountY < ( _maxY - y ) )
            {
                amountY = _maxY - y;
            }
        }

        ScrollY( MathUtils.Clamp( amountY, 0, _maxY ) );
    }

    /// <summary>
    /// Returns the maximum scroll value in the x direction.
    /// </summary>
    public float GetMaxX()
    {
        return _maxX;
    }

    /// <summary>
    /// Returns the maximum scroll value in the y direction.
    /// </summary>
    public float GetMaxY()
    {
        return _maxY;
    }

    public float GetScrollBarHeight()
    {
        if ( !_scrollX )
        {
            return 0;
        }

        float height = 0;

        if ( _style.HScrollKnob != null )
        {
            height = _style.HScrollKnob.MinHeight;
        }

        if ( _style.HScroll != null )
        {
            height = Math.Max( height, _style.HScroll.MinHeight );
        }

        return height;
    }

    public float GetScrollBarWidth()
    {
        if ( !_scrollY )
        {
            return 0;
        }

        float width = 0;

        if ( _style.VScrollKnob != null )
        {
            width = _style.VScrollKnob.MinWidth;
        }

        if ( _style.VScroll != null )
        {
            width = Math.Max( width, _style.VScroll.MinWidth );
        }

        return width;
    }

    /// <summary>
    /// Returns the width of the scrolled viewport.
    /// </summary>
    public float GetScrollWidth()
    {
        return _widgetArea.Width;
    }

    /// <summary>
    /// Returns the height of the scrolled viewport.
    /// </summary>
    public float GetScrollHeight()
    {
        return _widgetArea.Height;
    }

    /// <summary>
    /// Returns true if the widget is larger than the scroll pane horizontally.
    /// </summary>
    public bool ISScrollX()
    {
        return _scrollX;
    }

    /// <summary>
    /// Returns true if the widget is larger than the scroll pane vertically.
    /// </summary>
    public bool ISScrollY()
    {
        return _scrollY;
    }

    /// <summary>
    /// Disables scrolling in a direction. The widget will be sized to the
    /// FlickScrollPane in the disabled direction.
    /// </summary>
    public void SetScrollingDisabled( bool x, bool y )
    {
        _disableX = x;
        _disableY = y;

        Invalidate();
    }

    public bool ISScrollingDisabledX()
    {
        return _disableX;
    }

    public bool ISScrollingDisabledY()
    {
        return _disableY;
    }

    public bool ISLeftEdge()
    {
        return !_scrollX || ( _amountX <= 0 );
    }

    public bool ISRightEdge()
    {
        return !_scrollX || ( _amountX >= _maxX );
    }

    public bool ISTopEdge()
    {
        return !_scrollY || ( _amountY <= 0 );
    }

    public bool ISBottomEdge()
    {
        return !_scrollY || ( _amountY >= _maxY );
    }

    public bool ISDragging()
    {
        return _draggingPointer != -1;
    }

    public bool ISPanning()
    {
        return _flickScrollListener.GetGestureDetector().IsPanning();
    }

    public bool ISFlinging()
    {
        return _flingTimer > 0;
    }

    public void SetVelocityX( float velocityX )
    {
        this._velocityX = velocityX;
    }

    /// <summary>
    /// Gets the flick scroll x velocity.
    /// </summary>
    public float GetVelocityX()
    {
        return _velocityX;
    }

    public void SetVelocityY( float velocityY )
    {
        this._velocityY = velocityY;
    }

    /// <summary>
    /// Gets the flick scroll y velocity.
    /// </summary>
    public float GetVelocityY()
    {
        return _velocityY;
    }

    /// <summary>
    /// For flick scroll, if true the widget can be scrolled slightly past its
    /// bounds and will animate back to its bounds when scrolling is stopped.
    /// Default is true.
    /// </summary>
    public void SetOverscroll( bool overscrollX, bool overscrollY )
    {
        this._overscrollX = overscrollX;
        this._overscrollY = overscrollY;
    }

    /// <summary>
    /// For flick scroll, sets the overscroll distance in pixels and the speed
    /// it returns to the widget's bounds in seconds. Default is 50, 30, 200. 
    /// </summary>
    public void SetupOverscroll( float distance, float speedMin, float speedMax )
    {
        _overscrollDistance = distance;
        _overscrollSpeedMin = speedMin;
        _overscrollSpeedMax = speedMax;
    }

    public float GetOverscrollDistance()
    {
        return _overscrollDistance;
    }

    /// <summary>
    /// Forces enabling scrollbars (for non-flick scroll) and overscrolling
    /// (for flick scroll) in a direction, even if the contents do not exceed
    /// the bounds in that direction.
    /// </summary>
    public void SetForceScroll( bool x, bool y )
    {
        _forceScrollX = x;
        _forceScrollY = y;
    }

    public bool IsForceScrollX()
    {
        return _forceScrollX;
    }

    public bool IsForceScrollY()
    {
        return _forceScrollY;
    }

    /// <summary>
    /// For flick scroll, sets the amount of time in seconds that a fling
    /// will continue to scroll. Default is 1.
    /// </summary>
    public void SetFlingTime( float flingTime )
    {
        this._flingTime = flingTime;
    }

    /// <summary>
    /// For flick scroll, prevents scrolling out of the widget's bounds.
    /// Default is true.
    /// </summary>
    public void SetClamp( bool Clamp )
    {
        this._clamp = Clamp;
    }

    /// <summary>
    /// Set the position of the vertical and horizontal scroll bars.
    /// </summary>
    public void SetScrollBarPositions( bool bottom, bool right )
    {
        _hScrollOnBottom = bottom;
        _vScrollOnRight  = right;
    }

    /// <summary>
    /// When true the scrollbars don't reduce the scrollable size and fade out
    /// after some time of not being used.
    /// </summary>
    public void SetFadeScrollBars( bool fadeScrollBars )
    {
        if ( this._fadeScrollBars == fadeScrollBars )
        {
            return;
        }

        this._fadeScrollBars = fadeScrollBars;

        if ( !fadeScrollBars )
        {
            _fadeAlpha = _fadeAlphaSeconds;
        }

        Invalidate();
    }

    public void SetupFadeScrollBars( float fadeAlphaSeconds, float fadeDelaySeconds )
    {
        this._fadeAlphaSeconds = fadeAlphaSeconds;
        this._fadeDelaySeconds = fadeDelaySeconds;
    }

    public bool GetFadeScrollBars()
    {
        return _fadeScrollBars;
    }

    /// <summary>
    /// When false, the scroll bars don't respond to touch or mouse events.
    /// Default is true.
    /// </summary>
    public void SetScrollBarTouch( bool scrollBarTouch )
    {
        this._scrollBarTouch = scrollBarTouch;
    }

    public void SetSmoothScrolling( bool smoothScrolling )
    {
        this._smoothScrolling = smoothScrolling;
    }

    /// <summary>
    /// When false (the default), the widget is clipped so it is not drawn
    /// under the scrollbars. When true, the widget is clipped to the entire
    /// scroll pane bounds and the scrollbars are drawn on top of the widget.
    /// If <see cref="SetFadeScrollBars(bool)"/> is true, the scroll bars are
    /// always drawn on top.
    /// </summary>
    public void setScrollbarsOnTop( bool scrollbarsOnTop )
    {
        this._scrollbarsOnTop = scrollbarsOnTop;
        Invalidate();
    }

    public bool GetVariableSizeKnobs()
    {
        return _variableSizeKnobs;
    }

    /// If true, the scroll knobs are sized based on <see cref="GetMaxX()"/>" or
    /// <see cref="GetMaxY()"/>. If false, the scroll knobs are sized based on
    /// <see cref="IDrawable.MinWidth"/> or <see cref="IDrawable.MinHeight"/>".
    /// Default is true.
    public void SetVariableSizeKnobs( bool variableSizeKnobs )
    {
        this._variableSizeKnobs = variableSizeKnobs;
    }

    /// <summary>
    /// When true (default) and flick scrolling begins, <see cref="CancelTouchFocus()"/>"
    /// is called. This causes any widgets inside the scrollpane that have received
    /// touchDown to receive touchUp when flick scrolling begins.
    /// </summary>
    public void SetCancelTouchFocus( bool cancelFocus )
    {
        this._cancelTouchFocus = cancelFocus;
    }

    public override void DrawDebug( ShapeRenderer shapes )
    {
        DrawDebugBounds( shapes );
        ApplyTransform( shapes, ComputeTransform() );

        if ( ClipBegin( _widgetArea.X, _widgetArea.Y, _widgetArea.Width, _widgetArea.Height ) )
        {
            DrawDebugChildren( shapes );
            shapes.Flush();
            ClipEnd();
        }

        ResetTransform( shapes );
    }

    [PublicAPI]
    public class ScrollPaneStyle
    {
        public IDrawable? Background  { get; set; }
        public IDrawable? Corner      { get; set; }
        public IDrawable? HScroll     { get; set; }
        public IDrawable? HScrollKnob { get; set; }
        public IDrawable? VScroll     { get; set; }
        public IDrawable? VScrollKnob { get; set; }

        public ScrollPaneStyle()
        {
        }

        public ScrollPaneStyle( IDrawable background,
                                IDrawable hScroll,
                                IDrawable hScrollKnob,
                                IDrawable vScroll,
                                IDrawable vScrollKnob )
        {
            this.Background  = background;
            this.HScroll     = hScroll;
            this.HScrollKnob = hScrollKnob;
            this.VScroll     = vScroll;
            this.VScrollKnob = vScrollKnob;
        }

        public ScrollPaneStyle( ScrollPaneStyle style )
        {
            Background = style.Background;
            Corner     = style.Corner;

            HScroll     = style.HScroll;
            HScrollKnob = style.HScrollKnob;

            VScroll     = style.VScroll;
            VScrollKnob = style.VScrollKnob;
        }
    }
}
