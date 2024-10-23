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
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.Listeners;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D;

[PublicAPI]
public class Actor : IActor, IComparable< Actor >
{
    public Stage?    Stage      { get; set; }
    public Group?    Parent     { get; set; }
    public string?   Name       { get; set; }
    public object?   UserObject { get; set; }
    public Touchable Touchable  { get; set; } = Touchable.Enabled;
    public bool      IsVisible  { get; set; } = true;

    public virtual float MinWidth   { get; set; } = 0;
    public virtual float MinHeight  { get; set; } = 0;
    public virtual float MaxWidth   { get; set; } = 0;
    public virtual float MaxHeight  { get; set; } = 0;
    public virtual float PrefWidth  { get; set; } = 0;
    public virtual float PrefHeight { get; set; } = 0;

    public DelayedRemovalList< IEventListener > Listeners        { get; }      = [ ];
    public DelayedRemovalList< IEventListener > CaptureListeners { get; }      = [ ];
    public List< Action >                       Actions          { get; set; } = [ ];

    // ------------------------------------------------------------------------

    protected float OriginX { get; set; }
    protected float OriginY { get; set; }

    // ------------------------------------------------------------------------

    // Backing values for properties
    private float _x;
    private float _y;
    private float _width;
    private float _height;
    private float _scaleX;
    private float _scaleY;
    private float _rotation = 0.0f;
    private Color _color    = new( 1, 1, 1, 1 );
    private bool  _debug    = false;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Default Constructor.
    /// </summary>
    protected Actor()
    {
    }

    /// <summary>
    /// The X coordinate of the actor's left edge.
    /// </summary>
    public float X
    {
        get => _x;
        set
        {
            if ( MathUtils.IsNotEqual( _x, value ) )
            {
                _x = value;
                PositionChanged();
            }
        }
    }

    /// <summary>
    /// The Y coordinate of the actor's bottom edge.
    /// </summary>
    public float Y
    {
        get => _y;
        set
        {
            if ( MathUtils.IsNotEqual( _y, value ) )
            {
                _y = value;
                PositionChanged();
            }
        }
    }

    /// <summary>
    /// The actors width.
    /// </summary>
    public float Width
    {
        get => _width;
        set
        {
            if ( MathUtils.IsNotEqual( _width, value ) )
            {
                _width = value;
                SizeChanged();
            }
        }
    }

    /// <summary>
    /// The actors height.
    /// </summary>
    public float Height
    {
        get => _height;
        set
        {
            if ( MathUtils.IsNotEqual( _height, value ) )
            {
                _height = value;
                SizeChanged();
            }
        }
    }

    /// <summary>
    /// Top edge of this actor.
    /// </summary>
    public float TopEdge => _y + _height;

    /// <summary>
    /// Right side edge of this actor.
    /// </summary>
    public float RightEdge => _x + _width;

    /// <summary>
    /// The X Scaling factor
    /// </summary>
    public float ScaleX
    {
        get => _scaleX;
        set
        {
            if ( !_scaleX.Equals( value ) )
            {
                _scaleX = value;
                ScaleChanged();
            }
        }
    }

    /// <summary>
    /// The Y Scaling factor
    /// </summary>
    public float ScaleY
    {
        get => _scaleY;
        set
        {
            if ( !_scaleY.Equals( value ) )
            {
                _scaleY = value;
                ScaleChanged();
            }
        }
    }

    /// <summary>
    /// This actors rotation.
    /// </summary>
    public float Rotation
    {
        get => _rotation;
        set
        {
            if ( !_rotation.Equals( value ) )
            {
                _rotation = value;
                RotationChanged();
            }
        }
    }

    /// <summary>
    /// This actors Color.
    /// </summary>
    public Color Color
    {
        get => _color;
        set => _color.Set( value );
    }

    /// <summary>
    /// If true, <see cref="DrawDebug(ShapeRenderer)"/> will be called for this actor.
    /// </summary>
    public bool DebugActive
    {
        get => _debug;
        set
        {
            if ( Stage != null )
            {
                _debug = value;

                if ( value )
                {
                    Stage.Debug = true;
                }
            }
        }
    }

    /// <summary>
    /// Draws the actor. The batch is configured to draw in the parent's coordinate system. This
    /// draw method is convenient to draw a rotated and scaled TextureRegion.
    /// <para>
    /// <see cref="IBatch.Begin()"/> has already been called on the batch. If <see cref="IBatch.End()"/>
    /// is called to draw without the batch then <see cref="IBatch.Begin()"/> must be called before
    /// the method returns.
    /// </para>
    /// <para>
    /// <b>The default implementation does nothing. Child classes should override and implement.</b>
    /// </para>
    /// </summary>
    /// <param name="batch"> The <see cref="IBatch"/> to use. </param>
    /// <param name="parentAlpha">
    /// The parent alpha, to be multiplied with this actor's alpha,
    /// allowing the parent's alpha to affect all children.
    /// </param>
    public virtual void Draw( IBatch batch, float parentAlpha )
    {
    }

    /// <summary>
    /// Handles all actions attached to this actor.
    /// </summary>
    /// <param name="delta"> Time in seconds since the last update. </param>
    public virtual void Act( float delta )
    {
        if ( Actions.Count == 0 )
        {
            return;
        }

        if ( Stage is { ActionsRequestRendering: true } )
        {
            Gdx.Graphics.RequestRendering();
        }

        try
        {
            for ( var i = 0; i < Actions.Count; i++ )
            {
                if ( Actions[ i ].Act( delta ) && ( i < Actions.Count ) )
                {
                    var current     = Actions[ i ];
                    var actionIndex = current == Actions[ i ] ? i : Actions.IndexOf( Actions[ i ] );

                    if ( actionIndex != -1 )
                    {
                        Actions.RemoveIndex( actionIndex );
                        Actions[ i ].Actor = null;

                        i--;
                    }
                }
            }
        }
        catch ( SystemException ex )
        {
            var context = ToString();

            if ( context != null )
            {
                throw new SystemException( $"Actor - {context.AsSpan( 0, Math.Min( context.Length, 128 ) )}", ex );
            }
        }
    }

    /// <summary>
    /// Sets this actor as the event target and propagates the event to this actor and
    /// ascendants as necessary. If this actor is not in the stage, the stage must be
    /// set before calling this method.
    /// <para>
    /// Events are fired in 2 phases:
    /// <li>
    /// The first phase (the "capture" phase) notifies listeners on each actor starting
    /// at the root and propagating down the hierarchy to (and including) this actor.
    /// </li>
    /// <li>
    /// The second phase notifies listeners on each actor starting at this actor and, if
    /// <see cref="Event.Bubbles()"/> is true, propagating upward to the root.
    /// </li>
    /// </para>
    /// <para>
    /// If the event is stopped at any time, it will not propagate to the next actor.
    /// </para>
    /// </summary>
    /// <param name="ev"> The <see cref="Event"/> to fire. </param>
    /// <returns> True if the event was cancelled. </returns>
    public virtual bool Fire( Event? ev )
    {
        ArgumentNullException.ThrowIfNull( ev );

        ev.Stage       ??= Stage;
        ev.TargetActor =   this;

        // Collect ascendants so event propagation is unaffected by
        // hierarchy changes.
        List< Group >? ascendants = Pools< List< Group > >.Obtain();

        if ( ascendants == null )
        {
            return ev.IsCancelled;
        }

        var parent = Parent;

        while ( parent != null )
        {
            ascendants.Add( parent );
            parent = parent.Parent;
        }

        try
        {
            // Notify ascendants' capture listeners, starting at the root.
            // Ascendants may stop an event before children receive it.
            Group[] ascendantsArray = ascendants.ToArray();

            for ( var i = ascendants.Count - 1; i >= 0; i-- )
            {
                var currentTarget = ascendantsArray[ i ];

                currentTarget.Notify( ev, true );

                if ( ev.IsStopped )
                {
                    return ev.IsCancelled;
                }
            }

            // Notify the target capture listeners.
            Notify( ev, true );

            if ( ev.IsStopped )
            {
                return ev.IsCancelled;
            }

            // Notify the target listeners.
            Notify( ev, false );

            if ( !ev.Bubbles )
            {
                return ev.IsCancelled;
            }

            if ( ev.IsStopped )
            {
                return ev.IsCancelled;
            }

            // Notify ascendants' actor listeners, starting at the target.
            // Children may stop an event before ascendants receive it.
            for ( int i = 0, n = ascendants.Count; i < n; i++ )
            {
                ascendantsArray[ i ].Notify( ev, false );

                if ( ev.IsStopped )
                {
                    return ev.IsCancelled;
                }
            }

            return ev.IsCancelled;
        }
        finally
        {
            ascendants.Clear();

            Pools< List< Group > >.Free( ascendants );
        }
    }

    /// <summary>
    /// Responsible for notifying event listeners of an event.
    /// <para>
    /// This method first verifies that the event has a valid target actor. Depending on
    /// whether the event is in the capture phase, it selects the appropriate listener list.
    /// It then iterates through these listeners and notifies them of the event. If any
    /// listener handles the event, the event is marked as handled.
    /// </para>
    /// <para>
    /// If an exception occurs during this process, a new exception is thrown with additional
    /// context.
    /// </para>
    /// </summary>
    /// <param name="ev"> The event. </param>
    /// <param name="capture">
    /// true for <see cref="CaptureListeners"/>, false for <see cref="Listeners"/>. </param>
    /// <returns></returns>
    public virtual bool Notify( Event ev, bool capture )
    {
        if ( ev.TargetActor == null )
        {
            throw new ArgumentException( "The event target cannot be null." );
        }

        DelayedRemovalList< IEventListener > listeners = capture ? CaptureListeners : Listeners;

        if ( listeners.Count == 0 )
        {
            return ev.IsCancelled;
        }

        ev.ListenerActor = this;
        ev.Capture       = capture;

        ev.Stage ??= Stage;

        try
        {
            listeners.Begin();

            for ( int i = 0, n = listeners.Count; i < n; i++ )
            {
                if ( listeners[ i ].Handle( ev ) )
                {
                    ev.IsHandled = true;
                }
            }

            listeners.End();
        }
        catch ( SystemException ex )
        {
            var context = ToString();

            if ( context != null )
            {
                throw new SystemException( $"Actor - {context.AsSpan( 0, Math.Min( context.Length, 128 ) )}", ex );
            }
        }

        return ev.IsCancelled;
    }

    /// <summary>
    /// Returns the deepest visible (and optionally, touchable) actor that contains the
    /// specified point, or null if no actor was hit. The point is specified in the actor's
    /// local coordinate system (0,0 is the bottom left of the actor and width, height is
    /// the upper right).
    /// <para>
    /// This method is used to delegate touchDown, mouse, and enter/exit events.
    /// </para>
    /// <para>
    /// If this method returns null, those events will not occur on this Actor. The default
    /// implementation returns this actor if the point is within this actor's bounds and
    /// this actor is visible.
    /// </para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="touchable"></param>
    /// <returns></returns>
    public virtual Actor? Hit( float x, float y, bool touchable )
    {
        if ( touchable && ( Touchable != Touchable.Enabled ) )
        {
            return null;
        }

        if ( !IsVisible )
        {
            return null;
        }

        return ( x >= 0 ) && ( x < _width ) && ( y >= 0 ) && ( y < _height ) ? this : null;
    }

    /// <summary>
    /// Removes this actor from its parent, if it has a parent.
    /// </summary>
    /// <returns> True if successful. </returns>
    public virtual bool Remove()
    {
        return ( Parent != null ) && Parent.RemoveActor( this, true );
    }

    /// <summary>
    /// Add a listener to receive events that hit this actor.
    /// </summary>
    public bool AddListener( IEventListener listener )
    {
        if ( !Listeners.Contains( listener ) )
        {
            Listeners.Add( listener );

            return true;
        }

        return false;
    }

    /// <summary>
    /// Remove the specified listener from this Actor.
    /// </summary>
    /// <param name="listener"> The listener to remove. </param>
    /// <returns> True if listener successfully removed. </returns>
    public bool RemoveListener( IEventListener listener )
    {
        return Listeners.RemoveValue( listener );
    }

    /// <summary>
    /// </summary>
    /// <param name="listener"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public bool AddCaptureListener( IEventListener listener )
    {
        if ( !CaptureListeners.Contains( listener ) )
        {
            CaptureListeners.Add( listener );
        }

        return true;
    }

    /// <summary>
    /// </summary>
    /// <param name="listener"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public bool RemoveCaptureListener( IEventListener listener )
    {
        return CaptureListeners.RemoveValue( listener );
    }

    /// <summary>
    /// </summary>
    /// <param name="action"></param>
    public void AddAction( Action? action )
    {
        if ( action != null )
        {
            action.Actor = this;

            Actions.Add( action );

            if ( Stage is { ActionsRequestRendering: true } )
            {
                Gdx.Graphics.RequestRendering();
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="action"></param>
    public void RemoveAction( Action? action )
    {
        if ( ( action != null ) && Actions.Remove( action ) )
        {
            action.Actor = null;
        }
    }

    /// <summary>
    /// Returns true if the actor has one or more actions.
    /// </summary>
    public bool HasActions()
    {
        return Actions.Count > 0;
    }

    /// <summary>
    /// Removes all actions on this actor.
    /// </summary>
    public void ClearActions()
    {
        for ( var i = Actions.Count - 1; i >= 0; i-- )
        {
            Actions[ i ].Actor = null;
        }

        Actions.Clear();
    }

    /// <summary>
    /// Removes all listeners on this actor.
    /// </summary>
    public void ClearListeners()
    {
        Listeners.Clear();
        CaptureListeners.Clear();
    }

    /// <summary>
    /// Removes all actions and listeners on this actor.
    /// </summary>
    public virtual void Clear()
    {
        ClearActions();
        ClearListeners();
    }

    /// <summary>
    /// Returns true if this actor is the same as or is the descendant
    /// of the specified actor.
    /// </summary>
    /// <param name="actor"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public bool IsDescendantOf( Actor? actor )
    {
        ArgumentNullException.ThrowIfNull( actor );

        var parent = this;

        do
        {
            if ( parent == actor )
            {
                return true;
            }

            parent = parent.Parent;
        }
        while ( parent != null );

        return false;
    }


    /// <summary>
    /// Returns true if this actor is the same as or is the ascendant of the specified actor.
    /// </summary>
    public bool IsAscendantOf( Actor? actor )
    {
        ArgumentNullException.ThrowIfNull( actor );

        do
        {
            if ( actor == this )
            {
                return true;
            }

            actor = actor.Parent;
        }
        while ( actor != null );

        return false;
    }

    /// <summary>
    /// Returns this actor or the first ascendant of this actor that is assignable
    /// with the specified type, or null if none were found.
    /// </summary>
    public T? FirstAscendant< T >( T type ) where T : Actor
    {
        var actor = this;

        do
        {
            if ( actor.GetType() == type.GetType() )
            {
                return ( T ) actor;
            }

            actor = actor.Parent;
        }
        while ( actor != null );

        return null;
    }

    /// <summary>
    /// Returns true if the actor's parent is not null.
    /// </summary>
    public bool HasParent()
    {
        return Parent != null;
    }

    /// <summary>
    /// Returns true if input events are processed by this actor.
    /// </summary>
    public bool IsTouchable()
    {
        return Touchable == Touchable.Enabled;
    }

    /// <summary>
    /// Returns true if this actor and all ascendants are visible.
    /// </summary>
    public bool AscendantsVisible()
    {
        var actor = this;

        do
        {
            if ( !actor.IsVisible )
            {
                return false;
            }

            actor = actor.Parent;
        }
        while ( actor != null );

        return true;
    }

    /// <summary>
    /// Returns true if this actor is the <see cref="Stage.KeyboardFocus"/> keyboard focus actor.
    /// </summary>
    public bool HasKeyboardFocus()
    {
        return ( Stage != null ) && ( Stage.KeyboardFocus == this );
    }

    /// <summary>
    /// Returns true if this actor is the <see cref="Stage.ScrollFocus"/> actor.
    /// </summary>
    public bool HasScrollFocus()
    {
        return ( Stage != null ) && ( Stage.ScrollFocus == this );
    }

    /// <summary>
    /// Returns true if this actor is a target actor for touch focus.
    /// @see Stage#addTouchFocus(EventListener, Actor, Actor, int, int)
    /// </summary>
    public bool IsTouchFocusTarget()
    {
        if ( Stage == null )
        {
            return false;
        }

        for ( int i = 0, n = Stage.TouchFocuses.Size; i < n; i++ )
        {
            if ( Stage.TouchFocuses.GetAt( i ).Target == this )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if this actor is a listener actor for touch focus.
    /// <see cref="Stage.AddTouchFocus(IEventListener, Actor, Actor, int, int)"/>
    /// </summary>
    public bool IsTouchFocusListener()
    {
        if ( Stage == null )
        {
            return false;
        }

        for ( int i = 0, n = Stage.TouchFocuses.Size; i < n; i++ )
        {
            if ( Stage.TouchFocuses.GetAt( i ).ListenerActor == this )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the X position of the specified <see cref="Align"/>.
    /// </summary>
    public float GetX( int alignment )
    {
        var x = _x;

        if ( ( alignment & Align.RIGHT ) != 0 )
        {
            x += _width;
        }
        else if ( ( alignment & Align.LEFT ) == 0 )
        {
            x += _width / 2;
        }

        return x;
    }

    /// <summary>
    /// Sets the x position using the specified <see cref="Align"/>.
    /// Note this may set the position to non-integer coordinates.
    /// </summary>
    public void SetXWithAlignment( float x, int alignment )
    {
        if ( ( alignment & Align.RIGHT ) != 0 )
        {
            x -= _width;
        }
        else if ( ( alignment & Align.LEFT ) == 0 )
        {
            x -= _width / 2;
        }

        if ( MathUtils.IsNotEqual( _x, x ) )
        {
            _x = x;
            PositionChanged();
        }
    }

    /// <summary>
    /// Sets the y position using the specified <see cref="Align"/>.
    /// Note this may set the position to non-integer
    /// coordinates.
    /// </summary>
    public void SetYWithAlignment( float y, int alignment )
    {
        if ( ( alignment & Align.TOP ) != 0 )
        {
            y -= _height;
        }
        else if ( ( alignment & Align.BOTTOM ) == 0 )
        {
            y -= _height / 2;
        }

        if ( MathUtils.IsNotEqual( _y, y ) )
        {
            _y = y;
            PositionChanged();
        }
    }

    /// <summary>
    /// Returns the Y position of the specified <see cref="Align"/>.
    /// </summary>
    public float GetY( int alignment )
    {
        var y = _y;

        if ( ( alignment & Align.TOP ) != 0 )
        {
            y += _height;
        }
        else if ( ( alignment & Align.BOTTOM ) == 0 )
        {
            y += _height / 2;
        }

        return y;
    }

    /// <summary>
    /// Sets the position of the actor's bottom left corner.
    /// </summary>
    public void SetPosition( float x, float y )
    {
        if ( !X.Equals( x ) || !X.Equals( y ) )
        {
            X = x;
            Y = y;
            PositionChanged();
        }
    }

    /// <summary>
    /// Sets the position using the specified <see cref="Align"/> alignment.
    /// Note this may set the position to non-integer coordinates.
    /// </summary>
    public void SetPosition( float x, float y, int alignment )
    {
        if ( ( alignment & Align.RIGHT ) != 0 )
        {
            x -= Width;
        }
        else if ( ( alignment & Align.LEFT ) == 0 )
        {
            x -= Width / 2;
        }

        if ( ( alignment & Align.TOP ) != 0 )
        {
            y -= Height;
        }
        else if ( ( alignment & Align.BOTTOM ) == 0 )
        {
            y -= Height / 2;
        }

        if ( !X.Equals( x ) || !Y.Equals( y ) )
        {
            X = x;
            Y = y;
            PositionChanged();
        }
    }

    /// <summary>
    /// Add x and y to current position
    /// </summary>
    public void MoveBy( float x, float y )
    {
        if ( ( x != 0 ) || ( y != 0 ) )
        {
            _x += x;
            _y += y;
            PositionChanged();
        }
    }

    /// <summary>
    /// Called when the actor's position has been changed.
    /// </summary>
    public virtual void PositionChanged()
    {
    }

    /// <summary>
    /// Called when the actor's size has been changed.
    /// </summary>
    public virtual void SizeChanged()
    {
    }

    /// <summary>
    /// Called when the actor's scale has been changed.
    /// </summary>
    public virtual void ScaleChanged()
    {
    }

    /// <summary>
    /// Called when the actor's rotation has been changed.
    /// </summary>
    public virtual void RotationChanged()
    {
    }

    /// <summary>
    /// Sets the width and height.
    /// </summary>
    public void SetSize( float width, float height )
    {
        if ( MathUtils.IsNotEqual( _width, width )
          || MathUtils.IsNotEqual( _height, height ) )
        {
            _width  = width;
            _height = height;
            SizeChanged();
        }
    }

    /// <summary>
    /// Adds the specified size to the current size.
    /// </summary>
    public void SizeBy( float size )
    {
        if ( size != 0 )
        {
            _width  += size;
            _height += size;
            SizeChanged();
        }
    }

    /// <summary>
    /// Adds the specified size to the current size.
    /// </summary>
    public void SizeBy( float width, float height )
    {
        if ( ( width != 0 ) || ( height != 0 ) )
        {
            _width  += width;
            _height += height;
            SizeChanged();
        }
    }

    /// <summary>
    /// Set bounds the x, y, width, and height.
    /// </summary>
    public virtual void SetBounds( float x, float y, float width, float height )
    {
        if ( MathUtils.IsNotEqual( _x, x ) || MathUtils.IsNotEqual( _y, y ) )
        {
            _x = x;
            _y = y;
            PositionChanged();
        }

        if ( MathUtils.IsNotEqual( _width, width ) || MathUtils.IsNotEqual( _height, height ) )
        {
            _width  = width;
            _height = height;
            SizeChanged();
        }
    }

    /// <summary>
    /// Sets the origin position which is relative to the actor's bottom left corner.
    /// </summary>
    public virtual void SetOrigin( float originX, float originY )
    {
        OriginX = originX;
        OriginY = originY;
    }

    /// <summary>
    /// Sets the origin position to the specified <see cref="Align"/> alignment.
    /// </summary>
    public virtual void SetOrigin( int alignment )
    {
        if ( ( alignment & Align.LEFT ) != 0 )
        {
            OriginX = 0;
        }
        else if ( ( alignment & Align.RIGHT ) != 0 )
        {
            OriginX = Width;
        }
        else
        {
            OriginX = Width / 2;
        }

        if ( ( alignment & Align.BOTTOM ) != 0 )
        {
            OriginY = 0;
        }
        else if ( ( alignment & Align.TOP ) != 0 )
        {
            OriginY = Height;
        }
        else
        {
            OriginY = Height / 2;
        }
    }

    /// <summary>
    /// Sets the X and Y scale to the the same value as specified
    /// in <paramref name="scaleXY"/>.
    /// </summary>
    public void SetScale( float scaleXY )
    {
        if ( !ScaleX.Equals( scaleXY ) || !ScaleY.Equals( scaleXY ) )
        {
            ScaleX = scaleXY;
            ScaleY = scaleXY;
            ScaleChanged();
        }
    }

    /// <summary>
    /// Sets the X and Y scale.
    /// </summary>
    /// <param name="scaleX"> The new X sc ale value. </param>
    /// <param name="scaleY"> The new Y sc ale value. </param>
    public void SetScale( float scaleX, float scaleY )
    {
        if ( !ScaleX.Equals( scaleX ) || !ScaleY.Equals( scaleY ) )
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleChanged();
        }
    }

    /// <summary>
    /// Adds the specified scale to the current X andf Y scale values..
    /// </summary>
    public void ScaleBy( float scale )
    {
        if ( scale != 0 )
        {
            ScaleX += scale;
            ScaleY += scale;
            ScaleChanged();
        }
    }

    /// <summary>
    /// Adds the specified X and Y scales to the current X andf Y scale values..
    /// </summary>
    public void ScaleBy( float scaleX, float scaleY )
    {
        if ( ( scaleX != 0 ) || ( scaleY != 0 ) )
        {
            ScaleX += scaleX;
            ScaleY += scaleY;
            ScaleChanged();
        }
    }

    /// <summary>
    /// Adds the specified rotation to the current rotation.
    /// </summary>
    public void RotateBy( float amountInDegrees )
    {
        if ( amountInDegrees != 0 )
        {
            Rotation = ( Rotation + amountInDegrees ) % 360;
            RotationChanged();
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    public void SetColor( float r, float g, float b, float a )
    {
        _color.Set( r, g, b, a );
    }

    /// <summary>
    /// Changes the z-order for this actor so it is in front of all siblings.
    /// </summary>
    public void ToFront()
    {
        SetZIndex( int.MaxValue );
    }

    /// <summary>
    /// Changes the z-order for this actor so it is in back of all siblings.
    /// </summary>
    public void ToBack()
    {
        SetZIndex( 0 );
    }

    /// <summary>
    /// Sets the z-index of this actor. The z-index is the index into the parent's
    /// <see cref="Group.Children"/> children, where a lower index is below a
    /// higher index. Setting a z-index higher than the number of children will move
    /// the child to the front. Setting a z-index less than zero is invalid.
    /// </summary>
    /// <returns>true if the z-index changed.</returns>
    public bool SetZIndex( int index )
    {
        if ( index < 0 )
        {
            throw new ArgumentException( "ZIndex cannot be < 0." );
        }

        if ( ( Parent == null ) || ( Parent.Children.Size <= 1 ) )
        {
            return false;
        }

        index = Math.Min( index, Parent.Children.Size - 1 );

        if ( Parent.Children.GetAt( index ) == this )
        {
            return false;
        }

        if ( !Parent.Children.Remove( this ) )
        {
            return false;
        }

        Parent.Children.Insert( index, this );

        return true;
    }

    /// <summary>
    /// Returns the z-index of this actor, or -1 if the actor is not in a group.
    /// </summary>
    /// <see cref="SetZIndex(int)"/>
    /// <returns></returns>
    public int GetZIndex()
    {
        if ( Parent == null )
        {
            return -1;
        }

        return Parent.Children.IndexOf( this );
    }

    /// <summary>
    /// Calls <see cref="ClipBegin(float, float, float, float)"/> to clip this actor's bounds.
    /// </summary>
    public bool ClipBegin()
    {
        return ClipBegin( _x, _y, _width, _height );
    }

    /// <summary>
    /// Clips the specified screen aligned rectangle, specified relative to the
    /// transform matrix of the stage's Batch. The transform matrix and the stage's
    /// camera must not have rotational components. Calling this method must be
    /// followed by a call to <see cref="ClipEnd()"/> if true is returned.
    /// </summary>
    /// <returns>false if the clipping area is zero and no drawing should occur.</returns>
    /// <see cref="ScissorStack"/>
    public bool ClipBegin( float x, float y, float width, float height )
    {
        if ( ( width <= 0 ) || ( height <= 0 ) )
        {
            return false;
        }

        if ( Stage == null )
        {
            return false;
        }

        var tableBounds = RectangleShape.Tmp;

        tableBounds.X      = x;
        tableBounds.Y      = y;
        tableBounds.Width  = width;
        tableBounds.Height = height;

        var scissorBounds = Pools< RectangleShape >.Obtain();

        if ( scissorBounds == null )
        {
            return false;
        }

        Stage.CalculateScissors( tableBounds, scissorBounds );

        if ( ScissorStack.PushScissors( scissorBounds ) )
        {
            return true;
        }

        Pools< RectangleShape >.Free( scissorBounds );

        return false;
    }

    /// <summary>
    /// Ends clipping begun by <see cref="ClipBegin(float, float, float, float)"/>.
    /// </summary>
    public void ClipEnd()
    {
        Pools< object >.Free( ScissorStack.PopScissors() );
    }

    /// <summary>
    /// Transforms the specified point in screen coordinates to the actor's
    /// local coordinate system.
    /// </summary>
    /// <see cref="Stage.ScreenToStageCoordinates(Vector2)"/>
    public virtual Vector2 ScreenToLocalCoordinates( Vector2 screenCoords )
    {
        return Stage == null
                   ? screenCoords
                   : StageToLocalCoordinates( Stage.ScreenToStageCoordinates( screenCoords ) );
    }

    /// <summary>
    /// Transforms the specified point in the stage's coordinates to
    /// the actor's local coordinate system.
    /// </summary>
    public virtual Vector2 StageToLocalCoordinates( Vector2 stageCoords )
    {
        Parent?.StageToLocalCoordinates( stageCoords );

        ParentToLocalCoordinates( stageCoords );

        return stageCoords;
    }

    /// <summary>
    /// Converts the coordinates given in the parent's coordinate system
    /// to this actor's coordinate system.
    /// </summary>
    public virtual Vector2 ParentToLocalCoordinates( Vector2 parentCoords )
    {
        var rotation = Rotation;
        var scaleX   = ScaleX;
        var scaleY   = ScaleY;
        var childX   = _x;
        var childY   = _y;

        if ( rotation == 0 )
        {
            if ( ( Math.Abs( scaleX - 1f ) < 0.001f )
              && ( Math.Abs( scaleY - 1f ) < 0.001f ) )
            {
                parentCoords.X -= childX;
                parentCoords.Y -= childY;
            }
            else
            {
                var originX = OriginX;
                var originY = OriginY;

                parentCoords.X = ( ( parentCoords.X - childX - originX ) / scaleX ) + originX;
                parentCoords.Y = ( ( parentCoords.Y - childY - originY ) / scaleY ) + originY;
            }
        }
        else
        {
            var cos = ( float ) Math.Cos( rotation * MathUtils.DEGREES_TO_RADIANS );
            var sin = ( float ) Math.Sin( rotation * MathUtils.DEGREES_TO_RADIANS );

            var originX = OriginX;
            var originY = OriginY;
            var tox     = parentCoords.X - childX - originX;
            var toy     = parentCoords.Y - childY - originY;

            parentCoords.X = ( ( ( tox * cos ) + ( toy * sin ) ) / scaleX ) + originX;
            parentCoords.Y = ( ( ( tox * -sin ) + ( toy * cos ) ) / scaleY ) + originY;
        }

        return parentCoords;
    }

    /// <summary>
    /// Transforms the specified point in the actor's coordinates to be in screen coordinates.
    /// </summary>
    /// <see cref="Stage.StageToScreenCoordinates(Vector2)"/>
    public virtual Vector2 LocalToScreenCoordinates( Vector2 localCoords )
    {
        return Stage == null
                   ? localCoords
                   : Stage.StageToScreenCoordinates( LocalToAscendantCoordinates( null, localCoords ) );
    }

    /// <system>
    /// Transforms the specified point in the actor's coordinates
    /// to be in the stage's coordinates.
    /// </system>
    public virtual Vector2 LocalToStageCoordinates( Vector2 localCoords )
    {
        return LocalToAscendantCoordinates( null, localCoords );
    }

    /// <system>
    /// Transforms the specified point in the actor's coordinates
    /// to be in the parent's coordinates.
    /// </system>
    public virtual Vector2 LocalToParentCoordinates( Vector2 localCoords )
    {
        var rotation = -Rotation;
        var scaleX   = ScaleX;
        var scaleY   = ScaleY;
        var x        = _x;
        var y        = _y;

        if ( rotation == 0 )
        {
            if ( ( Math.Abs( scaleX - 1f ) < 0.001f )
              && ( Math.Abs( scaleY - 1f ) < 0.001f ) )
            {
                localCoords.X += x;
                localCoords.Y += y;
            }
            else
            {
                var originX = OriginX;
                var originY = OriginY;

                localCoords.X = ( ( localCoords.X - originX ) * scaleX ) + originX + x;
                localCoords.Y = ( ( localCoords.Y - originY ) * scaleY ) + originY + y;
            }
        }
        else
        {
            var cos = ( float ) Math.Cos( rotation * MathUtils.DEGREES_TO_RADIANS );
            var sin = ( float ) Math.Sin( rotation * MathUtils.DEGREES_TO_RADIANS );

            var originX = OriginX;
            var originY = OriginY;
            var tox     = ( localCoords.X - originX ) * scaleX;
            var toy     = ( localCoords.Y - originY ) * scaleY;

            localCoords.X = ( tox * cos ) + ( toy * sin ) + originX + x;
            localCoords.Y = ( tox * -sin ) + ( toy * cos ) + originY + y;
        }

        return localCoords;
    }

    /// <summary>
    /// Converts coordinates for this actor to those of an ascendant.
    /// The ascendant is not required to be the immediate parent.
    /// </summary>
    /// <param name="ascendant"></param>
    /// <param name="localCoords"></param>
    /// <returns></returns>
    public virtual Vector2 LocalToAscendantCoordinates( Actor? ascendant, Vector2 localCoords )
    {
        var actor = this;

        do
        {
            actor.LocalToParentCoordinates( localCoords );

            actor = actor.Parent;

            if ( actor == ascendant )
            {
                break;
            }
        }
        while ( actor != null );

        return localCoords;
    }

    /// <summary>
    /// Converts coordinates for this actor to those of another actor,
    /// which can be anywhere in the stage.
    /// </summary>
    public virtual Vector2 LocalToActorCoordinates( Actor actor, Vector2 localCoords )
    {
        LocalToStageCoordinates( localCoords );

        return actor.StageToLocalCoordinates( localCoords );
    }

    /// <summary>
    /// Draws this actor's debug lines if <see cref="DebugActive"/> is true.
    /// </summary>
    public virtual void DrawDebug( ShapeRenderer shapes )
    {
        DrawDebugBounds( shapes );
    }

    /// <summary>
    /// Draws a rectangle for the bounds of this actor if <see cref="DebugActive"/> is true.
    /// </summary>
    protected virtual void DrawDebugBounds( ShapeRenderer shapes )
    {
        if ( !DebugActive )
        {
            return;
        }

        shapes.Set( ShapeRenderer.ShapeTypes.Lines );

        if ( Stage != null )
        {
            shapes.Color = Stage.DebugColor;
        }

        shapes.Rect( _x, _y, OriginX, OriginY, _width, _height, ScaleX, ScaleY, Rotation );
    }

    /// <summary>
    /// Enables Debug for this actor.
    /// </summary>
    /// <returns> This Actor for chaining. </returns>
    public virtual Actor EnableDebug()
    {
        DebugActive = true;

        return this;
    }

    /// <inheritdoc />
    public int CompareTo( Actor? other )
    {
        //TODO:
        return other == null ? 1 : 0;
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return Name;
    }
}
