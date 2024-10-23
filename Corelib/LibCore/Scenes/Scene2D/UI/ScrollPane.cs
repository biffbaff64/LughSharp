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
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.Listeners;
using Corelib.LibCore.Scenes.Scene2D.Utils;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

[PublicAPI]
public partial class ScrollPane : WidgetGroup
{
    private readonly ActorGestureListener _flickScrollListener;
    private readonly RectangleShape       _hKnobBounds       = new();
    private readonly RectangleShape       _hScrollBounds     = new();
    private readonly Vector2              _lastPoint         = new();
    private readonly RectangleShape       _vKnobBounds       = new();
    private readonly RectangleShape       _vScrollBounds     = new();
    private readonly RectangleShape       _widgetArea        = new();
    private readonly RectangleShape       _widgetCullingArea = new();

    private int    _draggingPointer = -1;
    private float  _fadeAlpha;
    private float  _fadeAlphaSeconds = 1;
    private float  _fadeDelay;
    private float  _fadeDelaySeconds = 1;
    private bool   _flickScroll      = true;
    private float  _flingTimer;
    private bool   _hScrollOnBottom    = true;
    private float  _overscrollSpeedMax = 200;
    private float  _overscrollSpeedMin = 30;
    private bool   _overscrollX        = true;
    private bool   _overscrollY        = true;
    private bool   _scrollbarsOnTop;
    private bool   _touchScrollH;
    private bool   _touchScrollV;
    private float  _visualAmountX;
    private float  _visualAmountY;
    private bool   _vScrollOnRight = true;
    private Actor? _widget;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public ScrollPane( Actor? widget )
        : this( widget, new ScrollPaneStyle() )
    {
    }

    public ScrollPane( Actor? widget, Skin skin )
        : this( widget, skin.Get< ScrollPaneStyle >() )
    {
    }

    public ScrollPane( Actor? widget, Skin skin, string styleName )
        : this( widget, skin.Get< ScrollPaneStyle >( styleName ) )
    {
    }

    public ScrollPane( Actor? widget, ScrollPaneStyle style )
    {
        ArgumentNullException.ThrowIfNull( style, "style cannot be null." );

        Style = style;

        _flickScrollListener = new ScrollPaneGestureListener( this );

        ConstructorHelper( widget );
    }

    private void ConstructorHelper( Actor? widget )
    {
        SetActor( widget );
        SetSize( 150, 150 );

        AddCaptureListener( new ScrollPaneCaptureListener( this ) );
        AddListener( _flickScrollListener );
        AddListener( new ScrollPaneScrollListener( this ) );
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
    public void TouchFocusCancel()
    {
        Stage?.CancelTouchFocusExcept( _flickScrollListener, this );
    }

    /// <summary>
    /// If currently scrolling by tracking a touch down, stop scrolling.
    /// </summary>
    public void Cancel()
    {
        _draggingPointer = -1;
        _touchScrollH    = false;
        _touchScrollV    = false;
        _flickScrollListener.Detector.Cancel();
    }

    private void ClampPane()
    {
        if ( !Clamp )
        {
            return;
        }

        AmountX = _overscrollX
                      ? MathUtils.Clamp( AmountX, -OverscrollDistance, MaxX + OverscrollDistance )
                      : MathUtils.Clamp( AmountX, 0, MaxX );

        AmountY = _overscrollY
                      ? MathUtils.Clamp( AmountY, -OverscrollDistance, MaxY + OverscrollDistance )
                      : MathUtils.Clamp( AmountY, 0, MaxY );
    }

    public void SetStyle( ScrollPaneStyle? style )
    {
        ArgumentNullException.ThrowIfNull( style, "style cannot be null." );

        Style = style;
        InvalidateHierarchy();
    }

    /// <summary>
    /// Returns the scroll pane's style. Modifying the returned style may
    /// not have an effect until <see cref="SetStyle(ScrollPaneStyle)"/> is called.
    /// </summary>
    public ScrollPaneStyle GetStyle()
    {
        return Style;
    }

    public override void Act( float delta )
    {
        base.Act( delta );

        var panning   = _flickScrollListener.Detector.IsPanning();
        var animating = false;

        if ( ( _fadeAlpha > 0 ) && FadeScrollBars && !panning && !_touchScrollH && !_touchScrollV )
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

            var alpha = _flingTimer / FlingTime;

            AmountX -= VelocityX * alpha * delta;
            AmountY -= VelocityY * alpha * delta;

            ClampPane();

            // Stop fling if hit overscroll distance.
            if ( AmountX.Equals( -OverscrollDistance ) )
            {
                VelocityX = 0;
            }

            if ( AmountX >= ( MaxX + OverscrollDistance ) )
            {
                VelocityX = 0;
            }

            if ( AmountY.Equals( -OverscrollDistance ) )
            {
                VelocityY = 0;
            }

            if ( AmountY >= ( MaxY + OverscrollDistance ) )
            {
                VelocityY = 0;
            }

            _flingTimer -= delta;

            if ( _flingTimer <= 0 )
            {
                VelocityX = 0;
                VelocityY = 0;
            }

            animating = true;
        }

        // Scroll smoothly when grabbing the scrollbar if one pixel of
        // scrollbar movement is > 10% of the scroll area.

        if ( SmoothScrolling
          && ( _flingTimer <= 0 )
          && !panning
          && ( !_touchScrollH || ( ScrollX && ( ( MaxX / ( _hScrollBounds.Width - _hKnobBounds.Width ) ) > ( _widgetArea.Width * 0.1f ) ) ) )
          && ( !_touchScrollV || ( ScrollY && ( ( MaxY / ( _vScrollBounds.Height - _vKnobBounds.Height ) ) > ( _widgetArea.Height * 0.1f ) ) ) ) //
           )
        {
            if ( !_visualAmountX.Equals( AmountX ) )
            {
                VisualScrollX( _visualAmountX < AmountX
                                   ? Math.Min( AmountX, _visualAmountX + Math.Max( 200 * delta, ( AmountX - _visualAmountX ) * 7 * delta ) )
                                   : Math.Max( AmountX, _visualAmountX - Math.Max( 200 * delta, ( _visualAmountX - AmountX ) * 7 * delta ) ) );

                animating = true;
            }

            if ( !_visualAmountY.Equals( AmountY ) )
            {
                VisualScrollY( _visualAmountY < AmountY
                                   ? Math.Min( AmountY, _visualAmountY + Math.Max( 200 * delta, ( AmountY - _visualAmountY ) * 7 * delta ) )
                                   : Math.Max( AmountY, _visualAmountY - Math.Max( 200 * delta, ( _visualAmountY - AmountY ) * 7 * delta ) ) );

                animating = true;
            }
        }
        else
        {
            if ( !_visualAmountX.Equals( AmountX ) )
            {
                VisualScrollX( AmountX );
            }

            if ( !_visualAmountY.Equals( AmountY ) )
            {
                VisualScrollY( AmountY );
            }
        }

        if ( !panning )
        {
            if ( _overscrollX && ScrollX )
            {
                if ( AmountX < 0 )
                {
                    SetScrollbarsVisible( true );

                    AmountX += ( _overscrollSpeedMin
                               + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                   * -AmountX )
                                 / OverscrollDistance ) )
                             * delta;

                    if ( AmountX > 0 )
                    {
                        AmountX = 0;
                    }

                    animating = true;
                }
                else if ( AmountX > MaxX )
                {
                    SetScrollbarsVisible( true );

                    AmountX -= ( _overscrollSpeedMin
                               + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                   * -( MaxX - AmountX ) )
                                 / OverscrollDistance ) )
                             * delta;

                    if ( AmountX < MaxX )
                    {
                        AmountX = MaxX;
                    }

                    animating = true;
                }
            }

            if ( _overscrollY && ScrollY )
            {
                if ( AmountY < 0 )
                {
                    SetScrollbarsVisible( true );

                    AmountY += ( _overscrollSpeedMin
                               + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                   * -AmountY )
                                 / OverscrollDistance ) )
                             * delta;

                    if ( AmountY > 0 )
                    {
                        AmountY = 0;
                    }

                    animating = true;
                }
                else if ( AmountY > MaxY )
                {
                    SetScrollbarsVisible( true );

                    AmountY -= ( _overscrollSpeedMin
                               + ( ( ( _overscrollSpeedMax - _overscrollSpeedMin )
                                   * -( MaxY - AmountY ) )
                                 / OverscrollDistance ) )
                             * delta;

                    if ( AmountY < MaxY )
                    {
                        AmountY = MaxY;
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

    public override void SetLayout()
    {
        var bg          = Style.Background;
        var hScrollKnob = Style.HScrollKnob;
        var vScrollKnob = Style.VScrollKnob;

        if ( bg == null )
        {
            return;
        }

        _widgetArea.Set( bg.LeftWidth,
                         bg.BottomHeight,
                         Width - bg.LeftWidth - bg.RightWidth,
                         Height - bg.TopHeight - bg.BottomHeight );

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

        if ( Style.HScroll != null )
        {
            scrollbarHeight = Math.Max( scrollbarHeight, Style.HScroll.MinHeight );
        }

        if ( vScrollKnob != null )
        {
            scrollbarWidth = vScrollKnob.MinWidth;
        }

        if ( Style.VScroll != null )
        {
            scrollbarWidth = Math.Max( scrollbarWidth, Style.VScroll.MinWidth );
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
        ScrollX = ForceScrollX || ( ( widgetWidth > _widgetArea.Width ) && !DisableXScroll );
        ScrollY = ForceScrollY || ( ( widgetHeight > _widgetArea.Height ) && !DisableYScroll );

        // Adjust widget area for scrollbar sizes and check if it causes the other scrollbar to show.
        if ( !_scrollbarsOnTop )
        {
            if ( ScrollY )
            {
                _widgetArea.Width -= scrollbarWidth;

                if ( !_vScrollOnRight )
                {
                    _widgetArea.X += scrollbarWidth;
                }

                // Horizontal scrollbar may cause vertical scrollbar to show.
                if ( !ScrollX && ( widgetWidth > _widgetArea.Width ) && !DisableXScroll )
                {
                    ScrollX = true;
                }
            }

            if ( ScrollX )
            {
                _widgetArea.Height -= scrollbarHeight;

                if ( _hScrollOnBottom )
                {
                    _widgetArea.Y += scrollbarHeight;
                }

                // Vertical scrollbar may cause horizontal scrollbar to show.
                if ( !ScrollY && ( widgetHeight > _widgetArea.Height ) && !DisableYScroll )
                {
                    ScrollY           =  true;
                    _widgetArea.Width -= scrollbarWidth;

                    if ( !_vScrollOnRight )
                    {
                        _widgetArea.X += scrollbarWidth;
                    }
                }
            }
        }

        // If the widget is smaller than the available space, make it take up the available space.
        widgetWidth  = DisableXScroll ? _widgetArea.Width : Math.Max( _widgetArea.Width, widgetWidth );
        widgetHeight = DisableYScroll ? _widgetArea.Height : Math.Max( _widgetArea.Height, widgetHeight );

        MaxX = widgetWidth - _widgetArea.Width;
        MaxY = widgetHeight - _widgetArea.Height;

        AmountX = MathUtils.Clamp( AmountX, 0, MaxX );
        AmountY = MathUtils.Clamp( AmountY, 0, MaxY );

        // Set the scrollbar and knob bounds.
        if ( ScrollX )
        {
            if ( hScrollKnob != null )
            {
                var x = _scrollbarsOnTop ? bg.LeftWidth : _widgetArea.X;
                var y = _hScrollOnBottom ? bg.BottomHeight : Height - bg.TopHeight - scrollbarHeight;

                _hScrollBounds.Set( x, y, _widgetArea.Width, scrollbarHeight );

                if ( ScrollY && _scrollbarsOnTop )
                {
                    _hScrollBounds.Width -= scrollbarWidth;

                    if ( !_vScrollOnRight )
                    {
                        _hScrollBounds.X += scrollbarWidth;
                    }
                }

                if ( VariableSizeKnobs )
                {
                    _hKnobBounds.Width = Math.Max( hScrollKnob.MinWidth,
                                                   ( int ) ( ( _hScrollBounds.Width * _widgetArea.Width ) / widgetWidth ) );
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
                _hKnobBounds.X      = _hScrollBounds.X + ( int ) ( ( _hScrollBounds.Width - _hKnobBounds.Width ) * GetScrollPercentX() );
                _hKnobBounds.Y      = _hScrollBounds.Y;
            }
            else
            {
                _hScrollBounds.Set( 0, 0, 0, 0 );
                _hKnobBounds.Set( 0, 0, 0, 0 );
            }
        }

        if ( ScrollY )
        {
            if ( vScrollKnob != null )
            {
                var x = _vScrollOnRight ? Width - bg.RightWidth - scrollbarWidth : bg.LeftWidth;
                var y = _scrollbarsOnTop ? bg.BottomHeight : _widgetArea.Y;

                _vScrollBounds.Set( x, y, scrollbarWidth, _widgetArea.Height );

                if ( ScrollX && _scrollbarsOnTop )
                {
                    _vScrollBounds.Height -= scrollbarHeight;

                    if ( _hScrollOnBottom )
                    {
                        _vScrollBounds.Y += scrollbarHeight;
                    }
                }

                _vKnobBounds.Width = vScrollKnob.MinWidth;

                if ( VariableSizeKnobs )
                {
                    _vKnobBounds.Height = Math.Max( vScrollKnob.MinHeight,
                                                    ( int ) ( ( _vScrollBounds.Height * _widgetArea.Height ) / widgetHeight ) );
                }
                else
                {
                    _vKnobBounds.Height = vScrollKnob.MinHeight;
                }

                if ( _vKnobBounds.Height > widgetHeight )
                {
                    _vKnobBounds.Height = 0;
                }

                _vKnobBounds.X = _vScrollOnRight ? Width - bg.RightWidth - vScrollKnob.MinWidth : bg.LeftWidth;
                _vKnobBounds.Y = _vScrollBounds.Y + ( int ) ( ( _vScrollBounds.Height - _vKnobBounds.Height ) * ( 1 - GetScrollPercentY() ) );
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
        var x = _widgetArea.X - ( ScrollX ? ( int ) _visualAmountX : 0 );
        var y = _widgetArea.Y - ( int ) ( ScrollY ? MaxY - _visualAmountY : MaxY );

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

        if ( ScrollX )
        {
            _hKnobBounds.X = _hScrollBounds.X
                           + ( int ) ( ( _hScrollBounds.Width - _hKnobBounds.Width )
                                     * GetVisualScrollPercentX() );
        }

        if ( ScrollY )
        {
            _vKnobBounds.Y = _vScrollBounds.Y
                           + ( int ) ( ( _vScrollBounds.Height - _vKnobBounds.Height )
                                     * ( 1 - GetVisualScrollPercentY() ) );
        }

        UpdateWidgetPosition();

        // Draw the background ninepatch.
        var color = Color;
        var alpha = color.A * parentAlpha;

        if ( Style.Background != null )
        {
            batch.SetColor( color.R, color.G, color.B, alpha );
            Style.Background.Draw( batch, 0, 0, Width, Height );
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

        if ( FadeScrollBars )
        {
            alpha *= Interpolation.Fade.Apply( _fadeAlpha / _fadeAlphaSeconds );
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

        var x = ScrollX && ( _hKnobBounds.Width > 0 );
        var y = ScrollY && ( _vKnobBounds.Height > 0 );

        if ( x && y )
        {
            if ( Style.Corner != null )
            {
                Style.Corner.Draw( batch,
                                   _hScrollBounds.X + _hScrollBounds.Width,
                                   _hScrollBounds.Y,
                                   _vScrollBounds.Width,
                                   _vScrollBounds.Y );
            }
        }

        if ( x )
        {
            if ( Style.HScroll != null )
            {
                Style.HScroll.Draw( batch,
                                    _hScrollBounds.X,
                                    _hScrollBounds.Y,
                                    _hScrollBounds.Width,
                                    _hScrollBounds.Height );
            }

            if ( Style.HScrollKnob != null )
            {
                Style.HScrollKnob.Draw( batch,
                                        _hKnobBounds.X,
                                        _hKnobBounds.Y,
                                        _hKnobBounds.Width,
                                        _hKnobBounds.Height );
            }
        }

        if ( y )
        {
            if ( Style.VScroll != null )
            {
                Style.VScroll.Draw( batch,
                                    _vScrollBounds.X,
                                    _vScrollBounds.Y,
                                    _vScrollBounds.Width,
                                    _vScrollBounds.Height );
            }

            if ( Style.VScrollKnob != null )
            {
                Style.VScrollKnob.Draw( batch,
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
        _flingTimer = flingTime;
        VelocityX   = velocityX;
        VelocityY   = velocityY;
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

        var background = Style.Background;

        if ( background != null )
        {
            width = Math.Max( width + background.LeftWidth + background.RightWidth, background.MinWidth );
        }

        if ( ScrollY )
        {
            float scrollbarWidth = 0;

            if ( Style.VScrollKnob != null )
            {
                scrollbarWidth = Style.VScrollKnob.MinWidth;
            }

            if ( Style.VScroll != null )
            {
                scrollbarWidth = Math.Max( scrollbarWidth, Style.VScroll.MinWidth );
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

        var background = Style.Background;

        if ( background != null )
        {
            height = Math.Max( height + background.TopHeight + background.BottomHeight, background.MinHeight );
        }

        if ( ScrollX )
        {
            float scrollbarHeight = 0;

            if ( Style.HScrollKnob != null )
            {
                scrollbarHeight = Style.HScrollKnob.MinHeight;
            }

            if ( Style.HScroll != null )
            {
                scrollbarHeight = Math.Max( scrollbarHeight, Style.HScroll.MinHeight );
            }

            height += scrollbarHeight;
        }

        return height;
    }

    /// <summary>
    /// Sets the <see cref="Actor"/> embedded in this scroll pane.
    /// </summary>
    /// <param name="actor"> May be null to remove zsany current actor. </param>
    public void SetActor( Actor? actor )
    {
        if ( _widget == this )
        {
            throw new ArgumentException( "widget cannot be the ScrollPane." );
        }

        if ( _widget != null )
        {
            base.RemoveActor( _widget, true );
        }

        _widget = actor;

        if ( _widget != null )
        {
            AddActor( _widget );
        }
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

    public override bool RemoveActor( Actor actor, bool unfocus )
    {
        if ( actor == null )
        {
            throw new ArgumentException( "actor cannot be null." );
        }

        if ( actor != _widget )
        {
            return false;
        }

        _widget = null;

        return base.RemoveActor( actor, unfocus );
    }

    public override Actor RemoveActorAt( int index, bool unfocus )
    {
        var actor = base.RemoveActorAt( index, unfocus );

        if ( actor == _widget )
        {
            _widget = null;
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
            if ( ScrollX && _touchScrollH && _hScrollBounds.Contains( x, y ) )
            {
                return this;
            }

            if ( ScrollY && _touchScrollV && _vScrollBounds.Contains( x, y ) )
            {
                return this;
            }
        }

        return base.Hit( x, y, touchable );
    }

    /// <summary>
    /// Returns the amount to scroll horizontally when the mouse wheel is scrolled.
    /// </summary>
    protected float GetMouseWheelX()
    {
        return Math.Min( _widgetArea.Width, Math.Max( _widgetArea.Width * 0.9f, MaxX * 0.1f ) / 4 );
    }

    /// <summary>
    /// Returns the amount to scroll vertically when the mouse wheel is scrolled.
    /// </summary>
    protected float GetMouseWheelY()
    {
        return Math.Min( _widgetArea.Height, Math.Max( _widgetArea.Height * 0.9f, MaxY * 0.1f ) / 4 );
    }

    //
//    /// <summary>
//    /// Called whenever the x scroll amount is changed.
//    /// </summary>
//    protected void ScrollX( float pixelsX )
//    {
//        this.ScrollPositionX = pixelsX;
//    }
//
//    /// <summary>
//    /// Called whenever the y scroll amount is changed.
//    /// </summary>
//    protected void ScrollY( float pixelsY )
//    {
//        this.ScrollPositionY = pixelsY;
//    }

    /// <summary>
    /// Sets the visual scroll amount equal to the scroll amount. This can
    /// be used when setting the scroll amount without animating.
    /// </summary>
    public void UpdateVisualScroll()
    {
        _visualAmountX = AmountX;
        _visualAmountY = AmountY;
    }

    public float GetVisualScrollPercentX()
    {
        if ( MaxX == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( _visualAmountX / MaxX, 0, 1 );
    }

    public float GetVisualScrollPercentY()
    {
        if ( MaxY == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( _visualAmountY / MaxY, 0, 1 );
    }

    public float GetScrollPercentX()
    {
        if ( MaxX == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( AmountX / MaxX, 0, 1 );
    }

    public void SetScrollPercentX( float percentX )
    {
        AmountX = MaxX * MathUtils.Clamp( percentX, 0, 1 );
    }

    public float GetScrollPercentY()
    {
        if ( MaxY == 0 )
        {
            return 0;
        }

        return MathUtils.Clamp( AmountY / MaxY, 0, 1 );
    }

    public void SetScrollPercentY( float percentY )
    {
        AmountY = MaxY * MathUtils.Clamp( percentY, 0, 1 );
    }

    public void SetFlickScroll( bool flickScroll )
    {
        if ( _flickScroll == flickScroll )
        {
            return;
        }

        _flickScroll = flickScroll;

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
        _flickScrollListener.Detector.SetTapSquareSize( halfTapSquareSize );
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

        var amountX = AmountX;

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

        AmountX = MathUtils.Clamp( amountX, 0, MaxX );

        var amountY = AmountY;

        if ( centerVertical )
        {
            amountY = ( ( MaxY - y ) + ( _widgetArea.Height / 2 ) ) - ( height / 2 );
        }
        else
        {
            if ( amountY > ( ( MaxY - y - height ) + _widgetArea.Height ) )
            {
                amountY = ( MaxY - y - height ) + _widgetArea.Height;
            }

            if ( amountY < ( MaxY - y ) )
            {
                amountY = MaxY - y;
            }
        }

        AmountY = MathUtils.Clamp( amountY, 0f, MaxY );
    }

    public float GetScrollBarHeight()
    {
        if ( !ScrollX )
        {
            return 0;
        }

        float height = 0;

        if ( Style.HScrollKnob != null )
        {
            height = Style.HScrollKnob.MinHeight;
        }

        if ( Style.HScroll != null )
        {
            height = Math.Max( height, Style.HScroll.MinHeight );
        }

        return height;
    }

    public float GetScrollBarWidth()
    {
        if ( !ScrollY )
        {
            return 0;
        }

        float width = 0;

        if ( Style.VScrollKnob != null )
        {
            width = Style.VScrollKnob.MinWidth;
        }

        if ( Style.VScroll != null )
        {
            width = Math.Max( width, Style.VScroll.MinWidth );
        }

        return width;
    }

    /// <summary>
    /// Disables scrolling in a direction. The widget will be sized to the
    /// FlickScrollPane in the disabled direction.
    /// </summary>
    public void SetScrollingDisabled( bool x, bool y )
    {
        DisableXScroll = x;
        DisableYScroll = y;

        Invalidate();
    }

    public bool IsPanning()
    {
        return _flickScrollListener.Detector.IsPanning();
    }

    /// <summary>
    /// For flick scroll, if true the widget can be scrolled slightly past its
    /// bounds and will animate back to its bounds when scrolling is stopped.
    /// Default is true.
    /// </summary>
    public void SetOverscroll( bool overscrollX, bool overscrollY )
    {
        _overscrollX = overscrollX;
        _overscrollY = overscrollY;
    }

    /// <summary>
    /// For flick scroll, sets the overscroll distance in pixels and the speed
    /// it returns to the widget's bounds in seconds. Default is 50, 30, 200.
    /// </summary>
    public void SetupOverscroll( float distance, float speedMin, float speedMax )
    {
        OverscrollDistance  = distance;
        _overscrollSpeedMin = speedMin;
        _overscrollSpeedMax = speedMax;
    }

    /// <summary>
    /// Forces enabling scrollbars (for non-flick scroll) and overscrolling
    /// (for flick scroll) in a direction, even if the contents do not exceed
    /// the bounds in that direction.
    /// </summary>
    public void SetForceScroll( bool x, bool y )
    {
        ForceScrollX = x;
        ForceScrollY = y;
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
        if ( FadeScrollBars == fadeScrollBars )
        {
            return;
        }

        FadeScrollBars = fadeScrollBars;

        if ( !fadeScrollBars )
        {
            _fadeAlpha = _fadeAlphaSeconds;
        }

        Invalidate();
    }

    public void SetupFadeScrollBars( float fadeAlphaSeconds, float fadeDelaySeconds )
    {
        _fadeAlphaSeconds = fadeAlphaSeconds;
        _fadeDelaySeconds = fadeDelaySeconds;
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
        _scrollbarsOnTop = scrollbarsOnTop;
        Invalidate();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region debug

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

    #endregion debug

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ScrollPaneStyle
    {
        public IDrawable? Background  { get; set; }
        public IDrawable? Corner      { get; set; }
        public IDrawable? HScroll     { get; set; }
        public IDrawable? HScrollKnob { get; set; }
        public IDrawable? VScroll     { get; set; }
        public IDrawable? VScrollKnob { get; set; }

        // --------------------------------------------------------------------

        public ScrollPaneStyle()
        {
        }
        
        public ScrollPaneStyle( IDrawable background,
                                IDrawable hScroll,
                                IDrawable hScrollKnob,
                                IDrawable vScroll,
                                IDrawable vScrollKnob )
        {
            Background  = background;
            HScroll     = hScroll;
            HScrollKnob = hScrollKnob;
            VScroll     = vScroll;
            VScrollKnob = vScrollKnob;
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

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region expression bodies

    /// <summary>
    /// Called whenever the visual x scroll amount is changed.
    /// </summary>
    protected void VisualScrollX( float pixelsX ) => _visualAmountX = pixelsX;

    /// <summary>
    /// Called whenever the visual y scroll amount is changed.
    /// </summary>
    protected void VisualScrollY( float pixelsY ) => _visualAmountY = pixelsY;

    /// <summary>
    /// Returns the actor embedded in this scroll pane, or null.
    /// </summary>
    public Actor? GetActor() => _widget;

    /// <summary>
    /// Returns the width of the scrolled viewport.
    /// </summary>
    public float GetScrollWidth() => _widgetArea.Width;

    /// <summary>
    /// Returns the height of the scrolled viewport.
    /// </summary>
    public float GetScrollHeight() => _widgetArea.Height;

    /// <summary>
    /// Returns true if the widget is larger than the scroll pane horizontally.
    /// </summary>
    public bool IsScrollX() => ScrollX; //TODO:

    /// <summary>
    /// Returns true if the widget is larger than the scroll pane vertically.
    /// </summary>
    public bool IsScrollY() => ScrollY; //TODO:

    public bool IsScrollingDisabledX() => DisableXScroll;

    public bool IsScrollingDisabledY() => DisableYScroll;

    public bool IsLeftEdge() => !ScrollX || ( AmountX <= 0 );

    public bool IsRightEdge() => !ScrollX || ( AmountX >= MaxX );

    public bool IsTopEdge() => !ScrollY || ( AmountY <= 0 );

    public bool IsBottomEdge() => !ScrollY || ( AmountY >= MaxY );

    public bool IsDragging() => _draggingPointer != -1;

    public float GetVisualScrollX() => !ScrollX ? 0 : _visualAmountX;

    public float GetVisualScrollY() => !ScrollY ? 0 : _visualAmountY;

    public void SetScrollX( float pixels ) => AmountX = MathUtils.Clamp( pixels, 0, MaxX );

    public void SetScrollY( float pixels ) => AmountY = MathUtils.Clamp( pixels, 0, MaxY );

    public bool IsFlinging() => _flingTimer > 0;

    public float GetMinWidth() => 0;

    public float GetMinHeight() => 0;

    #endregion expression bodies

    // ------------------------------------------------------------------------

    #region properties

    public ScrollPaneStyle Style              { get; set; }
    public float           AmountX            { get; set; }
    public float           AmountY            { get; set; }
    public bool            ForceScrollX       { get; set; }
    public bool            ForceScrollY       { get; set; }
    public bool            DisableXScroll     { get; set; }
    public bool            DisableYScroll     { get; set; }
    public float           OverscrollDistance { get; set; } = 50;
    public bool            FadeScrollBars     { get; set; } = true;
    public bool            SmoothScrolling    { get; set; } = true;

    /// <summary>
    /// Returns the maximum scroll value in the x direction.
    /// </summary>
    public float MaxX { get; set; }

    /// <summary>
    /// Returns the maximum scroll value in the y direction.
    /// </summary>
    public float MaxY { get; set; }

    /// <summary>
    /// The x scroll position in pixels, where 0 is the left of the scroll pane.
    /// </summary>
    public bool ScrollX { get; set; }

    /// <summary>
    /// The y scroll position in pixels, where 0 is the top of the scroll pane.
    /// </summary>
    public bool ScrollY { get; set; }

    /// <summary>
    /// Flick scroll X velocity.
    /// </summary>
    public float VelocityX { get; set; }

    /// <summary>
    /// Flick scroll Y velocity.
    /// </summary>
    public float VelocityY { get; set; }

    /// <summary>
    /// When false, the scroll bars don't respond to touch or mouse events.
    /// Default is true.
    /// </summary>
    public bool ScrollBarTouch { get; set; } = true;

    /// <summary>
    /// For flick scroll, sets the amount of time in seconds that a fling
    /// will continue to scroll. Default is 1.
    /// </summary>
    public float FlingTime { get; set; } = 1f;

    /// <summary>
    /// For flick scroll, prevents scrolling out of the widget's bounds.
    /// Default is true.
    /// </summary>
    public bool Clamp { get; set; } = true;

    /// If true, the scroll knobs are sized based on
    /// <see cref="MaxX()"/>
    /// " or
    /// <see cref="MaxY()"/>
    /// . If false, the scroll knobs are sized based on
    /// <see cref="IDrawable.MinWidth"/>
    /// or
    /// <see cref="IDrawable.MinHeight"/>
    /// ".
    /// Default is true.
    public bool VariableSizeKnobs { get; set; } = true;

    /// <summary>
    /// When true (default) and flick scrolling begins, <see cref="TouchFocusCancel"/>"
    /// is called. This causes any widgets inside the scrollpane that have received
    /// touchDown to receive touchUp when flick scrolling begins.
    /// </summary>
    public bool CancelTouchFocus { get; set; } = true;

    #endregion properties
}