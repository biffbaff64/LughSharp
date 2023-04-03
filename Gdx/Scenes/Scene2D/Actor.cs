using LibGDXSharp.Core;
using LibGDXSharp.G2D;
using LibGDXSharp.Maths;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Utils.Reflect;

namespace LibGDXSharp.Scenes.Scene2D
{
    public class Actor
    {
        public Stage?    Stage      { get; set; }
        public Group?    Parent     { get; set; }
        public string?   Name       { get; set; }
        public object?   UserObject { get; set; }
        public Touchable Touchable  { get; set; } = Touchable.Enabled;
        public bool      Visible    { get; set; } = true;
        public Color     Color      { get; set; } = new Color( 1, 1, 1, 1 );
        public float     OriginX    { get; set; }
        public float     OriginY    { get; set; }

        public DelayedRemovalArray< IEventListener > Listeners        { get; private set; }
        public DelayedRemovalArray< IEventListener > CaptureListeners { get; private set; }

        private readonly Array< Action > _actions = new Array< Action >();

        private bool  _debug;
        private float _scaleX = 1;
        private float _scaleY = 1;
        private float _rotation;

        protected Actor()
        {
            Listeners        = new DelayedRemovalArray< IEventListener >( 0 );
            CaptureListeners = new DelayedRemovalArray< IEventListener >( 0 );
        }

        /// <summary>
        /// Draws the actor. The batch is configured to draw in the parent's coordinate
        /// system. This draw method is convenient to draw a rotated and scaled TextureRegion.
        /// Batch.begin() has already been called on the batch. If Batch.end() is called to
        /// draw without the batch then Batch.begin() must be called before the method returns.
        /// The default implementation does nothing.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="parentAlpha">
        /// The parent alpha, to be multiplied with this actor's alpha,
        /// allowing the parent's alpha to affect all children.
        /// </param>
        public void Draw( IBatch batch, float parentAlpha )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="delta"></param>
        /// <exception cref="SystemException"></exception>
        public void Act( float delta )
        {
            if ( _actions.Size == 0 ) return;

            if ( ( Stage != null ) && Stage.GetActionsRequestRendering() )
            {
                Gdx.Graphics?.RequestRendering();
            }

            try
            {
                for ( var i = 0; i < _actions.Size; i++ )
                {
                    if ( _actions.Get( i )!.Act( delta ) && i < _actions.Size )
                    {
                        var current     = _actions.Get( i );
                        var actionIndex = current == _actions.Get( i ) ? i : _actions.IndexOf( _actions.Get( i ) );

                        if ( actionIndex != -1 )
                        {
                            _actions.RemoveIndex( actionIndex );
                            _actions.Get( i )?.SetActor( null );

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
                    throw new SystemException
                        (
                         string.Concat( "Actor - ", context.AsSpan( 0, System.Math.Min( context.Length, 128 ) ) ),
                         ex
                        );
                }
            }
        }

        /// <summary>
        /// Sets this actor as the event target and propagates the event to
        /// this actor and ascendants as necessary. If this actor is not in
        /// the stage, the stage must be set before calling this method.
        /// Events are fired in 2 phases:
        /// 
        /// 1.  The first phase (the "capture" phase) notifies listeners on
        ///     each actor starting at the root and propagating down the
        ///     hierarchy to (and including) this actor.
        /// 2.  The second phase notifies listeners on each actor starting
        ///     at this actor and, if Event.getBubbles() is true, propagating
        ///     upward to the root.
        /// 
        /// If the event is stopped at any time, it will not propagate to the
        /// next actor.
        /// </summary>
        /// <param name="ev">The <see cref="Event"/></param>
        /// <returns>True if the event was cancelled.</returns>
        public bool Fire( Event ev )
        {
            ev.Stage ??= this.Stage;

            ev.TargetActor = this;

            // Collect ascendants so event propagation is unaffected by
            // hierarchy changes.
            List< Group >? ascendants = Pools.Obtain( typeof(Array) );

            Group? parent = this.Parent;

            while ( parent != null )
            {
                ascendants.Add( parent );
                parent = parent.Parent;
            }

            try
            {
                // Notify ascendants' capture listeners, starting at the root.
                // Ascendants may stop an event before children receive it.
                var ascendantsArray = ascendants.Items;

                if ( ascendantsArray == null ) return ev.IsCancelled;

                for ( var i = ascendants.Size - 1; i >= 0; i-- )
                {
                    var currentTarget = ( Group )ascendantsArray[ i ];

                    currentTarget.Notify( ev, true );

                    if ( ev.IsStopped ) return ev.IsCancelled;
                }

                // Notify the target capture listeners.
                Notify( ev, true );

                if ( ev.IsStopped ) return ev.IsCancelled;

                // Notify the target listeners.
                Notify( ev, false );

                if ( !ev.Bubbles ) return ev.IsCancelled;
                if ( ev.IsStopped ) return ev.IsCancelled;

                // Notify ascendants' actor listeners, starting at the target.
                // Children may stop an event before ascendants receive it.
                for ( int i = 0, n = ascendants.Size; i < n; i++ )
                {
                    ( ( Group )ascendantsArray[ i ] ).Notify( ev, false );

                    if ( ev.IsStopped ) return ev.IsCancelled;
                }

                return ev.IsCancelled;
            }
            finally
            {
                ascendants.Clear();

                Pools.Free( ascendants );
            }
        }

        public bool Notify( Event ev, bool capture )
        {
            if ( ev.TargetActor == null )
            {
                throw new ArgumentException( "The event target cannot be null." );
            }

            var listeners = capture ? CaptureListeners : this.Listeners;

            if ( listeners.Count == 0 ) return ev.IsCancelled;

            ev.ListenerActor = this;
            ev.Capture       = capture;

            ev.Stage ??= this.Stage;

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
                    throw new SystemException
                        (
                         string.Concat( "Actor - ", context.AsSpan( 0, System.Math.Min( context.Length, 128 ) ) ),
                         ex
                        );
                }
            }

            return ev.IsCancelled;
        }

        /// <summary>
        /// Returns the deepest visible (and optionally, touchable) actor that
        /// contains the specified point, or null if no actor was hit. The point
        /// is specified in the actor's local coordinate system (0,0 is the bottom
        /// left of the actor and width,height is the upper right).
        /// This method is used to delegate touchDown, mouse, and enter/exit events.
        /// If this method returns null, those events will not occur on this Actor.
        /// The default implementation returns this actor if the point is within
        /// this actor's bounds and this actor is visible.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="touchable"></param>
        /// <returns></returns>
        public Actor? Hit( float x, float y, bool touchable )
        {
            if ( touchable && this.Touchable != Touchable.Enabled ) return null;

            if ( !Visible ) return null;

            return ( ( x >= 0 ) && ( x < _width ) && ( y >= 0 ) && ( y < _height ) ) ? this : null;
        }

        /// <summary>
        /// Removes this actor from its parent, if it has a parent.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool Remove()
        {
            if ( Parent != null ) return Parent.RemoveActor( this, true );

            return false;
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
        /// 
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool RemoveListener( IEventListener listener )
        {
            return Listeners.RemoveValue( listener );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool AddCaptureListener( IEventListener listener )
        {
            if ( !CaptureListeners.Contains( listener ) ) CaptureListeners.Add( listener );

            return true;
        }

        /// <summary>
        /// 
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
        public void AddAction( Action action )
        {
            action.SetActor( this );

            _actions.Add( action );

            if ( Stage != null && Stage.GetActionsRequestRendering() )
            {
                Gdx.Graphics?.RequestRendering();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction( Action? action )
        {
            if ( ( action != null ) && _actions.RemoveValue( action ) )
            {
                action.SetActor( null );
            }
        }

        /// <summary>
        /// Returns true if the actor has one or more actions.
        /// </summary>
        public bool HasActions()
        {
            return _actions.Size > 0;
        }

        /// <summary>
        /// Removes all actions on this actor.
        /// </summary>
        public void ClearActions()
        {
            for ( var i = _actions.Size - 1; i >= 0; i-- )
            {
                _actions.Get( i )?.SetActor( null );
            }

            _actions.Clear();
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
        public void Clear()
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
        public bool IsDescendantOf( Actor actor )
        {
            if ( actor == null ) throw new ArgumentException( "actor cannot be null." );

            var parent = this;

            do
            {
                if ( parent == actor ) return true;

                parent = parent.Parent;
            }
            while ( parent != null );

            return false;
        }


        /// <summary>
        /// Returns true if this actor is the same as or is the ascendant of the specified actor.
        /// </summary>
        public bool IsAscendantOf( Actor actor )
        {
            if ( actor == null ) throw new ArgumentException( "actor cannot be null." );

            do
            {
                if ( actor == this ) return true;

                actor = actor.Parent;
            }
            while ( actor != null );

            return false;
        }

        /// <summary>
        /// Returns this actor or the first ascendant of this actor that is assignable
        /// with the specified type, or null if none were found.
        /// </summary>
        public T? FirstAscendant<T>( Type type ) where T : Actor
        {
            if ( type == null ) throw new ArgumentException( "actor cannot be null." );

            Actor? actor = this;

            do
            {
                if ( ClassReflection.IsInstance( type, actor ) ) return ( T )actor;

                actor = actor.Parent;
            }
            while ( actor != null );

            return null;
        }

        /** Returns true if the actor's parent is not null. */
        public bool HasParent()
        {
            return Parent != null;
        }

        /** Returns true if input events are processed by this actor. */
        public bool IsTouchable()
        {
            return Touchable == Scene2D.Touchable.Enabled;
        }

        /** Returns true if this actor and all ascendants are visible. */
        public bool AscendantsVisible()
        {
            Actor actor = this;

            do
            {
                if ( !actor.IsVisible() ) return false;
                actor = actor.parent;
            }
            while ( actor != null );

            return true;
        }

        /// <summary>
        /// Returns true if this actor is the <see cref="Stage.GetKeyboardFocus()"/> keyboard focus actor.
        /// </summary>
        public bool HasKeyboardFocus()
        {
            return Stage != null && Stage.GetKeyboardFocus() == this;
        }

        /** Returns true if this actor is the {@link Stage#getScrollFocus() scroll focus} actor. */
        public bool HasScrollFocus()
        {
            return Stage != null && Stage.GetScrollFocus() == this;
        }

        /// <summary>
        /// Returns true if this actor is a target actor for touch focus.
        /// @see Stage#addTouchFocus(EventListener, Actor, Actor, int, int)
        /// </summary>
        public bool IsTouchFocusTarget()
        {
            if ( Stage == null ) return false;

            for ( int i = 0, n = Stage.touchFocuses.Size; i < n; i++ )
            {
                if ( Stage.touchFocuses.Get( i )?.target == this )
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
            if ( Stage == null ) return false;

            for ( int i = 0, n = Stage.touchFocuses.Size; i < n; i++ )
            {
                if ( Stage.touchFocuses.Get( i )?.listenerActor == this )
                {
                    return true;
                }
            }

            return false;
        }

        private float _x;
        private float _y;

        /// <summary>
        /// The X position of the actor's left edge.
        /// </summary>
        public float X
        {
            get => _x;
            set
            {
                if ( MathUtils.IsNotEqual( this._x, value ) )
                {
                    this._x = value;
                    PositionChanged();
                }
            }
        }

        /// <summary>
        /// Returns the X position of the specified <see cref="Align"/>.
        /// </summary>
        public float GetX( int alignment )
        {
            var x = this._x;

            if ( ( alignment & Align.Right ) != 0 )
            {
                x += _width;
            }
            else if ( ( alignment & Align.Left ) == 0 )
            {
                x += _width / 2;
            }

            return x;
        }

        /// <summary>
        /// Sets the x position using the specified <see cref="Align"/>.
        /// Note this may set the position to non-integer coordinates. 
        /// </summary>
        public void SetX( float x, int alignment )
        {
            if ( ( alignment & Align.Right ) != 0 )
            {
                x -= _width;
            }
            else if ( ( alignment & Align.Left ) == 0 )
            {
                x -= _width / 2;
            }

            if ( MathUtils.IsNotEqual( this._x, x ) )
            {
                this._x = x;
                PositionChanged();
            }
        }

        /// <summary>
        /// Returns the Y position of the actor's bottom edge.
        /// </summary>
        public float Y
        {
            get => _y;
            set
            {
                if ( MathUtils.IsNotEqual( this._y, value ) )
                {
                    this._y = value;
                    PositionChanged();
                }
            }
        }

        /// <summary>
        /// Sets the y position using the specified <see cref="Align"/>.
        /// Note this may set the position to non-integer
        /// coordinates. 
        /// </summary>
        public void SetY( float y, int alignment )
        {
            if ( ( alignment & Align.Top ) != 0 )
            {
                y -= _height;
            }
            else if ( ( alignment & Align.Bottom ) == 0 )
            {
                y -= _height / 2;
            }

            if ( MathUtils.IsNotEqual( this._y, y ) )
            {
                this._y = y;
                PositionChanged();
            }
        }

        /// <summary>
        /// Returns the Y position of the specified <see cref="Align"/>.
        /// </summary>
        public float GetY( int alignment )
        {
            float y = this._y;

            if ( ( alignment & Align.Top ) != 0 )
            {
                y += _height;
            }
            else if ( ( alignment & Align.Bottom ) == 0 )
            {
                y += _height / 2;
            }

            return y;
        }

        /** Sets the position of the actor's bottom left corner. */
        public void SetPosition( float x, float y )
        {
            if ( this.x != x || this.y != y )
            {
                this.x = x;
                this.y = y;
                PositionChanged();
            }
        }

        /** Sets the position using the specified {@link Align alignment}. Note this may set the position to non-integer
	 * coordinates. */
        public void SetPosition( float x, float y, int alignment )
        {
            if ( ( alignment & right ) != 0 )
                x -= width;
            else if ( ( alignment & left ) == 0 ) //
                x -= width / 2;

            if ( ( alignment & top ) != 0 )
                y -= height;
            else if ( ( alignment & bottom ) == 0 ) //
                y -= height / 2;

            if ( this.x != x || this.y != y )
            {
                this.x = x;
                this.y = y;
                PositionChanged();
            }
        }

        /// <summary>
        /// Add x and y to current position
        /// </summary>
        public void MoveBy( float x, float y )
        {
            if ( x != 0 || y != 0 )
            {
                this._x += x;
                this._y += y;
                PositionChanged();
            }
        }

        private float _width;
        private float _height;

        public float Width
        {
            get => _width;
            set
            {
                if ( MathUtils.IsNotEqual( this._width, value ) )
                {
                    this._width = value;
                    SizeChanged();
                }
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                if ( MathUtils.IsNotEqual( this._height, value ) )
                {
                    this._height = value;
                    SizeChanged();
                }
            }
        }

        public float Top   => _y + _height;
        public float Right => _x + _width;

        /// <summary>
        /// Called when the actor's position has been changed.
        /// </summary>
        protected virtual void PositionChanged()
        {
        }

        /// <summary>
        /// Called when the actor's size has been changed.
        /// </summary>
        protected virtual void SizeChanged()
        {
        }

        /// <summary>
        /// Called when the actor's scale has been changed.
        /// </summary>
        protected virtual void ScaleChanged()
        {
        }

        /// <summary>
        /// Called when the actor's rotation has been changed.
        /// </summary>
        protected virtual void RotationChanged()
        {
        }

        /// <summary>
        /// Sets the width and height.
        /// </summary>
        public virtual void SetSize( float width, float height )
        {
            if ( MathUtils.IsNotEqual( this._width, width )
                 || MathUtils.IsNotEqual( this._height, height ) )
            {
                this._width  = width;
                this._height = height;
                SizeChanged();
            }
        }

        /// <summary>
        /// Adds the specified size to the current size.
        /// </summary>
        public virtual void SizeBy( float size )
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
        public virtual void SizeBy( float width, float height )
        {
            if ( width != 0 || height != 0 )
            {
                this._width  += width;
                this._height += height;
                SizeChanged();
            }
        }

        /// <summary>
        /// Set bounds the x, y, width, and height.
        /// </summary>
        public void SetBounds( float x, float y, float width, float height )
        {
            if ( MathUtils.IsNotEqual( this._x, x ) || MathUtils.IsNotEqual( this._y, y ) )
            {
                this._x = x;
                this._y = y;
                PositionChanged();
            }

            if ( MathUtils.IsNotEqual( this._width, width ) || MathUtils.IsNotEqual( this._height, height ) )
            {
                this._width  = width;
                this._height = height;
                SizeChanged();
            }
        }

        /** Sets the origin position which is relative to the actor's bottom left corner. */
        public void SetOrigin( float originX, float originY )
        {
            this.OriginX = originX;
            this.OriginY = originY;
        }

        /** Sets the origin position to the specified {@link Align alignment}. */
        public void SetOrigin( int alignment )
        {
            if ( ( alignment & left ) != 0 )
                OriginX = 0;
            else if ( ( alignment & right ) != 0 )
                OriginX = width;
            else
                OriginX = width / 2;

            if ( ( alignment & bottom ) != 0 )
                OriginY = 0;
            else if ( ( alignment & top ) != 0 )
                OriginY = height;
            else
                OriginY = height / 2;
        }

        public float GetScaleX()
        {
            return scaleX;
        }

        public void SetScaleX( float scaleX )
        {
            if ( this.scaleX != scaleX )
            {
                this.scaleX = scaleX;
                scaleChanged();
            }
        }

        public float GetScaleY()
        {
            return scaleY;
        }

        public void SetScaleY( float scaleY )
        {
            if ( this.scaleY != scaleY )
            {
                this.scaleY = scaleY;
                scaleChanged();
            }
        }

        /** Sets the scale for both X and Y */
        public void SetScale( float scaleXY )
        {
            if ( this.scaleX != scaleXY || this.scaleY != scaleXY )
            {
                this.scaleX = scaleXY;
                this.scaleY = scaleXY;
                scaleChanged();
            }
        }

        /** Sets the scale X and scale Y. */
        public void SetScale( float scaleX, float scaleY )
        {
            if ( this.scaleX != scaleX || this.scaleY != scaleY )
            {
                this.scaleX = scaleX;
                this.scaleY = scaleY;
                scaleChanged();
            }
        }

        /** Adds the specified scale to the current scale. */
        public void ScaleBy( float scale )
        {
            if ( scale != 0 )
            {
                scaleX += scale;
                scaleY += scale;
                scaleChanged();
            }
        }

        /** Adds the specified scale to the current scale. */
        public void ScaleBy( float scaleX, float scaleY )
        {
            if ( scaleX != 0 || scaleY != 0 )
            {
                this.scaleX += scaleX;
                this.scaleY += scaleY;
                scaleChanged();
            }
        }

        public float GetRotation()
        {
            return rotation;
        }

        public void SetRotation( float degrees )
        {
            if ( this.rotation != degrees )
            {
                this.rotation = degrees;
                rotationChanged();
            }
        }

        /** Adds the specified rotation to the current rotation. */
        public void RotateBy( float amountInDegrees )
        {
            if ( amountInDegrees != 0 )
            {
                rotation = ( rotation + amountInDegrees ) % 360;
                rotationChanged();
            }
        }

        public void SetColor( Color color )
        {
            this.color.set( color );
        }

        public void SetColor( float r, float g, float b, float a )
        {
            color.set( r, g, b, a );
        }

        /** Returns the color the actor will be tinted when drawn. The returned instance can be modified to change the color. */
        public Color GetColor()
        {
            return color;
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
            if ( index < 0 ) throw new ArgumentException( "ZIndex cannot be < 0." );

            if ( this.Parent == null ) return false;

            if ( Parent.Children.Size <= 1 ) return false;

            index = Math.Min( index, this.Parent.Children.Size - 1 );

            if ( Parent.Children.Get( index ) == this ) return false;
            if ( !Parent.Children.RemoveValue( this ) ) return false;

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
            if ( Parent == null ) return -1;

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
            if ( width <= 0 || height <= 0 ) return false;

            if ( this.Stage == null ) return false;

            var tableBounds = RectangleShape.Tmp;

            tableBounds.X      = x;
            tableBounds.Y      = y;
            tableBounds.Width  = width;
            tableBounds.Height = height;

            var scissorBounds = Pools.Obtain< RectangleShape >( typeof(RectangleShape) );

            this.Stage.CalculateScissors( tableBounds, scissorBounds );

            if ( ScissorStack.PushScissors( scissorBounds ) ) return true;

            Pools.Free( scissorBounds );

            return false;
        }

        /// <summary>
        /// Ends clipping begun by <see cref="ClipBegin(float, float, float, float)"/>.
        /// </summary>
        public void ClipEnd()
        {
            Pools.Free( ScissorStack.PopScissors() );
        }

        /// <summary>
        /// Transforms the specified point in screen coordinates to the actor's
        /// local coordinate system.
        /// </summary>
        /// <see cref="Stage.ScreenToStageCoordinates(Vector2)"/>
        public Vector2 ScreenToLocalCoordinates( Vector2 screenCoords )
        {
            if ( this.Stage == null ) return screenCoords;

            return StageToLocalCoordinates( this.Stage.ScreenToStageCoordinates( screenCoords ) );
        }

        /// <summary>
        /// Transforms the specified point in the stage's coordinates to
        /// the actor's local coordinate system.
        /// </summary>
        public Vector2 StageToLocalCoordinates( Vector2 stageCoords )
        {
            Parent?.StageToLocalCoordinates( stageCoords );

            ParentToLocalCoordinates( stageCoords );

            return stageCoords;
        }

        /// <summary>
        /// Converts the coordinates given in the parent's coordinate system
        /// to this actor's coordinate system.
        /// </summary>
        public Vector2 ParentToLocalCoordinates( Vector2 parentCoords )
        {
            var rotation = this._rotation;
            var scaleX   = this._scaleX;
            var scaleY   = this._scaleY;
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
                    var originX = this.OriginX;
                    var originY = this.OriginY;

                    parentCoords.X = ( parentCoords.X - childX - originX ) / scaleX + originX;
                    parentCoords.Y = ( parentCoords.Y - childY - originY ) / scaleY + originY;
                }
            }
            else
            {
                var cos = ( float )Math.Cos( rotation * MathUtils.DegreesToRadians );
                var sin = ( float )Math.Sin( rotation * MathUtils.DegreesToRadians );

                var originX = this.OriginX;
                var originY = this.OriginY;
                var tox     = parentCoords.X - childX - originX;
                var toy     = parentCoords.Y - childY - originY;

                parentCoords.X = ( tox * cos + toy * sin ) / scaleX + originX;
                parentCoords.Y = ( tox * -sin + toy * cos ) / scaleY + originY;
            }

            return parentCoords;
        }

        /// <summary>
        /// Transforms the specified point in the actor's coordinates to be in screen coordinates.
        /// </summary>
        /// <see cref="Stage.StageToScreenCoordinates(Vector2)"/>
        public Vector2 LocalToScreenCoordinates( Vector2 localCoords )
        {
            if ( this.Stage == null ) return localCoords;

            return this.Stage.StageToScreenCoordinates( LocalToAscendantCoordinates( null, localCoords ) );
        }

        /// <system>
        /// Transforms the specified point in the actor's coordinates
        /// to be in the stage's coordinates.
        /// </system>
        public Vector2 LocalToStageCoordinates( Vector2 localCoords )
        {
            return LocalToAscendantCoordinates( null, localCoords );
        }

        /// <system>
        /// Transforms the specified point in the actor's coordinates
        /// to be in the parent's coordinates.
        /// </system>
        public Vector2 LocalToParentCoordinates( Vector2 localCoords )
        {
            var rotation = -this._rotation;
            var scaleX   = this._scaleX;
            var scaleY   = this._scaleY;
            var x        = this._x;
            var y        = this._y;

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
                    var originX = this.OriginX;
                    var originY = this.OriginY;

                    localCoords.X = ( localCoords.X - originX ) * scaleX + originX + x;
                    localCoords.Y = ( localCoords.Y - originY ) * scaleY + originY + y;
                }
            }
            else
            {
                var cos = ( float )Math.Cos( rotation * MathUtils.DegreesToRadians );
                var sin = ( float )Math.Sin( rotation * MathUtils.DegreesToRadians );

                var originX = this.OriginX;
                var originY = this.OriginY;
                var tox     = ( localCoords.X - originX ) * scaleX;
                var toy     = ( localCoords.Y - originY ) * scaleY;

                localCoords.X = ( tox * cos + toy * sin ) + originX + x;
                localCoords.Y = ( tox * -sin + toy * cos ) + originY + y;
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
        public Vector2 LocalToAscendantCoordinates( Actor? ascendant, Vector2 localCoords )
        {
            var actor = this;

            do
            {
                actor.LocalToParentCoordinates( localCoords );

                actor = actor.Parent;

                if ( actor == ascendant ) break;
            }
            while ( actor != null );

            return localCoords;
        }

        /// <summary>
        /// Converts coordinates for this actor to those of another actor,
        /// which can be anywhere in the stage.
        /// </summary>
        public Vector2 LocalToActorCoordinates( Actor actor, Vector2 localCoords )
        {
            LocalToStageCoordinates( localCoords );

            return actor.StageToLocalCoordinates( localCoords );
        }

        /// <summary>
        /// Draws this actor's debug lines if <see cref="DebugFlag"/> is true.
        /// </summary>
        public void DrawDebug( ShapeRenderer shapes )
        {
            DrawDebugBounds( shapes );
        }

        /// <summary>
        /// Draws a rectangle for the bounds of this actor
        /// if <see cref="DebugFlag"/> is true.
        /// </summary>
        protected void DrawDebugBounds( ShapeRenderer shapes )
        {
            if ( !DebugFlag ) return;

            shapes.Set( ShapeRenderer.ShapeType.Line );

            if ( Stage != null )
            {
                shapes.SetColor( Stage.GetDebugColor() );
            }

            shapes.Rect( _x, _y, OriginX, OriginY, _width, _height, _scaleX, _scaleY, _rotation );
        }

        /// <summary>
        /// If true, <see cref="DrawDebug(ShapeRenderer)"/> will be called for this actor.
        /// </summary>
        public bool DebugFlag
        {
            get => _debug;
            set
            {
                if ( Stage != null )
                {
                    _debug = value;

                    if ( value )
                    {
                        this.Stage.debug = true;
                    }
                }
            }
        }

        /// <summary>
        /// Enables Debug for this actor.
        /// </summary>
        /// <returns>This Actor for chaining.</returns>
        public Actor Debug()
        {
            DebugFlag = true;

            return this;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public new string? ToString()
        {
            return this.Name;
        }
    }
}
