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


using LibGDXSharp.LibCore.Scenes.Scene2D.Listeners;
using LibGDXSharp.LibCore.Scenes.Scene2D.UI;

namespace LibGDXSharp.LibCore.Scenes.Scene2D.Utils;

/// <summary>
///     Manages drag and drop operations through registered
///     drag sources and drop targets.
/// </summary>
public class DragAndDrop
{
    private readonly static Vector2                                TmpVector        = new();
    private readonly        Dictionary< DragSource, DragListener > _sourceListeners = new();
    private readonly        List< DragTarget >                     _targets         = new();
    private                 float                                  _dragActorX      = 0;
    private                 float                                  _dragActorY      = 0;

    private DragListener? _dragListener;
    private long          _dragValidTime;
    private bool          _isValidTarget;

    private bool  _removeDragActor;
    private float _tapSquareSize = 8;
    private float _touchOffsetX;
    private float _touchOffsetY;

    protected int activePointer = -1;

    /// <summary>
    ///     Time in milliseconds that a drag must take before a drop will be
    ///     considered valid. This ignores an accidental drag and drop that
    ///     was meant to be a click. Default is 250ms.
    /// </summary>
    public int DragTime { get; set; } = 250;

    public DragSource? Source          { get; set; }
    public DragTarget? Target          { get; set; }
    public Actor?      DragActor       { get; set; }
    public bool        KeepWithinStage { get; set; } = true;
    public Payload?    DragPayload     { get; set; }
    public int         Button          { get; set; }

    /// <summary>
    ///     When true (default), the touch focus (<see cref="Stage.CancelTouchFocus()" />) is cancelled
    ///     if <see cref="DragAndDrop.DragSource.DragStart(InputEvent, float, float, int)" />" returns
    ///     non-null. This ensures the DragAndDrop is the only touch focus listener, eg when the source
    ///     is inside a <see cref="ScrollPane" /> with flick scroll enabled.
    /// </summary>
    public bool CancelTouchFocus { get; set; } = true;

    public void AddSource( DragSource source )
    {
        _dragListener = new DragListenerImpl( this, source );

        _dragListener.TapSquareSize = _tapSquareSize;
        _dragListener.Button        = Button;

        source.Actor.AddCaptureListener( _dragListener );

        _sourceListeners[ source ] = _dragListener;
    }

    public void RemoveSource( DragSource source )
    {
        _sourceListeners.Remove( source, out DragListener? dragListener );

        source.Actor.RemoveCaptureListener( dragListener! );
    }

    public void AddTarget( DragTarget target )
    {
        _targets.Add( target );
    }

    public void RemoveTarget( DragTarget target )
    {
        _targets.Remove( target );
    }

    /// <summary>
    ///     Removes all targets and sources.
    /// </summary>
    public void Clear()
    {
        _targets.Clear();

        foreach ( KeyValuePair< DragSource, DragListener > entry in _sourceListeners )
        {
            entry.Key.Actor.RemoveCaptureListener( entry.Value );
        }

        _sourceListeners.Clear();
    }

    /// <summary>
    ///     Cancels the touch focus for everything except the specified source.
    /// </summary>
    public void CancelTouchFocusExcept( DragSource except )
    {
        DragListener listener;

        if ( ( listener = _sourceListeners[ except ] ) == null )
        {
            return;
        }

        Stage? stage = except.Actor.Stage;

        if ( stage != null )
        {
            stage.CancelTouchFocusExcept( listener, except.Actor );
        }
    }

    /// <summary>
    ///     Sets the distance a touch must travel before being considered a drag.
    /// </summary>
    public void SetTapSquareSize( float halfTapSquareSize )
    {
        _tapSquareSize = halfTapSquareSize;
    }

    public void SetDragActorPosition( float dragActorX, float dragActorY )
    {
        _dragActorX = dragActorX;
        _dragActorY = dragActorY;
    }

    /// <summary>
    ///     Sets an offset in stage coordinates from the touch position which
    ///     is used to determine the drop location. Default is 0,0.
    /// </summary>
    /// <param name="touchOffsetX"></param>
    /// <param name="touchOffsetY"></param>
    public void SetTouchOffset( float touchOffsetX, float touchOffsetY )
    {
        _touchOffsetX = touchOffsetX;
        _touchOffsetY = touchOffsetY;
    }

    public bool IsDragging()
    {
        return DragPayload != null;
    }

    /// <summary>
    ///     Returns true if a drag is in progress and the <see cref="DragTime" />"
    ///     has elapsed since the drag started.
    /// </summary>
    public bool IsDragValid()
    {
        return ( DragPayload != null ) && ( TimeUtils.Millis() >= _dragValidTime );
    }

    // ------------------------------------------------------------------------

    internal class DragListenerImpl : DragListener
    {
        private readonly DragAndDrop _parent;
        private readonly DragSource  _source;

        internal DragListenerImpl( DragAndDrop parent, DragSource source )
        {
            _parent = parent;
            _source = source;
        }

        public override void DragStart( InputEvent ev, float x, float y, int pointer )
        {
            if ( _parent.activePointer != -1 )
            {
                ev.Stop();

                return;
            }

            _parent.activePointer  = pointer;
            _parent._dragValidTime = TimeUtils.Millis() + _parent.DragTime;
            _parent.Source         = _source;
            _parent.DragPayload    = _source.DragStart( ev, TouchDownX, TouchDownY, pointer )!;

            ev.Stop();

            if ( _parent is { CancelTouchFocus: true, DragPayload: not null } )
            {
                Stage? stage = _source.Actor.Stage;

                if ( stage != null )
                {
                    stage.CancelTouchFocusExcept( this, _source.Actor );
                }
            }
        }

        public override void Drag( InputEvent ev, float x, float y, int pointer )
        {
            if ( ( _parent.DragPayload == null ) || ( pointer != _parent.activePointer ) )
            {
                return;
            }

            _source.Drag( ev, x, y, pointer );

            Stage? stage = ev.Stage;

            // Move the drag actor away, so it cannot be hit.
            Actor? oldDragActor  = _parent.DragActor;
            float  oldDragActorX = 0;
            float  oldDragActorY = 0;

            if ( oldDragActor != null )
            {
                oldDragActorX = oldDragActor.X;
                oldDragActorY = oldDragActor.Y;
                oldDragActor.SetPosition( int.MaxValue, int.MaxValue );
            }

            var stageX = ev.StageX + _parent._touchOffsetX;
            var stageY = ev.StageY + _parent._touchOffsetY;

            Actor? hit = ev.Stage?.Hit( stageX, stageY, true )
                      ?? ev.Stage?.Hit( stageX, stageY, false );

            if ( oldDragActor != null )
            {
                oldDragActor.SetPosition( oldDragActorX, oldDragActorY );
            }

            // Find target.
            DragTarget? newTarget = null;

            _parent._isValidTarget = false;

            if ( hit != null )
            {
                for ( int i = 0, n = _parent._targets.Count; i < n; i++ )
                {
                    DragTarget target = _parent._targets[ i ];

                    if ( !target.Actor.IsAscendantOf( hit ) )
                    {
                        continue;
                    }

                    newTarget = target;
                    target.Actor.StageToLocalCoordinates( TmpVector.Set( stageX, stageY ) );

                    break;
                }
            }

            // If over a new target, notify the former target that it's being left behind.
            if ( newTarget != _parent.Target )
            {
                if ( _parent.Target != null )
                {
                    _parent.Target.Reset( _source, _parent.DragPayload );
                }

                _parent.Target = newTarget;
            }

            // Notify new target of drag.
            if ( newTarget != null )
            {
                _parent._isValidTarget = newTarget.Drag( _source,
                                                         _parent.DragPayload,
                                                         TmpVector.X,
                                                         TmpVector.Y,
                                                         pointer );
            }

            // Determine the drag actor, remove the old one if it
            // was added by DragAndDrop, and add the new one.
            Actor? actor = null;

            if ( _parent.Target != null )
            {
                actor = _parent._isValidTarget
                    ? _parent.DragPayload.ValidDragActor
                    : _parent.DragPayload.InvalidDragActor;
            }

            actor ??= _parent.DragPayload.DragActor;

            if ( actor != oldDragActor )
            {
                if ( ( oldDragActor != null ) && _parent._removeDragActor )
                {
                    oldDragActor.Remove();
                }

                _parent.DragActor        = actor;
                _parent._removeDragActor = actor?.Stage == null; // Only remove later if not already in the stage now.

                if ( _parent._removeDragActor )
                {
                    stage?.AddActor( actor ?? throw new NullReferenceException() );
                }
            }

            if ( actor == null )
            {
                return;
            }

            // Position the drag actor.
            var actorX = ( ev.StageX - actor.Width ) + _parent._dragActorX;
            var actorY = ev.StageY + _parent._dragActorY;

            if ( _parent.KeepWithinStage )
            {
                if ( actorX < 0 )
                {
                    actorX = 0;
                }

                if ( actorY < 0 )
                {
                    actorY = 0;
                }

                if ( ( actorX + actor.Width ) > stage?.Width )
                {
                    actorX = stage.Width - actor.Width;
                }

                if ( ( actorY + actor.Height ) > stage?.Height )
                {
                    actorY = stage.Height - actor.Height;
                }
            }

            actor.SetPosition( actorX, actorY );
        }

        public override void DragStop( InputEvent ev, float x, float y, int pointer )
        {
            if ( pointer != _parent.activePointer )
            {
                return;
            }

            _parent.activePointer = -1;

            if ( _parent.DragPayload == null )
            {
                return;
            }

            if ( TimeUtils.Millis() < _parent._dragValidTime )
            {
                _parent._isValidTarget = false;
            }

            if ( _parent is { DragActor: not null, _removeDragActor: true } )
            {
                _parent.DragActor.Remove();
            }

            if ( _parent._isValidTarget )
            {
                var stageX = ev.StageX + _parent._touchOffsetX;
                var stageY = ev.StageY + _parent._touchOffsetY;

                _parent.Target?.Actor.StageToLocalCoordinates( TmpVector.Set( stageX, stageY ) );
                _parent.Target?.Drop( _parent.Source!, _parent.DragPayload, TmpVector.X, TmpVector.Y, pointer );
            }

            _parent.Source?.DragStop( ev,
                                      x,
                                      y,
                                      pointer,
                                      _parent.DragPayload,
                                      _parent._isValidTarget
                                          ? _parent.Target
                                          : null );

            if ( _parent.Target != null )
            {
                _parent.Target.Reset( _parent.Source!, _parent.DragPayload );
            }

            _parent.Source         = null;
            _parent.DragPayload    = null;
            _parent.Target         = null;
            _parent._isValidTarget = false;
            _parent.DragActor      = null;
        }
    }

    /// <summary>
    ///     A source where a payload can be dragged from.
    /// </summary>
    public class DragSource
    {
        public DragSource( Actor actor )
        {
            ArgumentNullException.ThrowIfNull( actor );

            Actor = actor;
        }

        public Actor Actor { get; set; }

        /// <summary>
        ///     Called when a drag is started on the source. The coordinates
        ///     are in the source's local coordinate system.
        /// </summary>
        /// <returns> If null the drag will not affect any targets. </returns>
        public virtual Payload? DragStart( InputEvent ev, float x, float y, int pointer )
        {
            return default( Payload? );
        }

        /// <summary>
        ///     Called repeatedly during a drag which started on this source.
        /// </summary>
        public virtual void Drag( InputEvent ev, float x, float y, int pointer )
        {
        }

        /// <summary>
        ///     Called when a drag for the source is stopped. The coordinates are
        ///     in the source's local coordinate system.
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pointer"></param>
        /// <param name="payload"> null if dragStart returned null. </param>
        /// <param name="target"> null if not dropped on a valid target. </param>
        public virtual void DragStop( InputEvent ev,
                                      float x,
                                      float y,
                                      int pointer,
                                      Payload? payload,
                                      DragTarget? target )
        {
        }
    }

    /// <summary>
    ///     A target where a payload can be dropped to.
    /// </summary>
    public abstract class DragTarget
    {
        /// <summary>
        ///     Constructor, creates a new Target actor.
        /// </summary>
        /// <param name="actor"></param>
        /// <exception cref="ArgumentException"></exception>
        protected DragTarget( Actor actor )
        {
            ArgumentNullException.ThrowIfNull( actor );

            Actor = actor;
            Stage? stage = actor.Stage;

            if ( ( stage != null ) && ( actor == stage.Root ) )
            {
                throw new ArgumentException( "The stage root cannot be a drag and drop target." );
            }
        }

        public Actor Actor { get; set; }

        /// <summary>
        ///     Called when the payload is dragged over the target. The coordinates are in the
        ///     target's local coordinate system.
        /// </summary>
        /// <returns> true if this is a valid target for the payload. </returns>
        public abstract bool Drag( DragSource source, Payload payload, float x, float y, int pointer );

        /// <summary>
        ///     Called when the payload is no longer over the target, whether
        ///     because the touch was moved or a drop occurred. This is called
        ///     even if <see cref="Drag(DragSource, Payload, float, float, int)" />
        ///     returned false.
        /// </summary>
        public void Reset( DragSource source, Payload payload )
        {
        }

        /// <summary>
        ///     Called when the payload is dropped on the target. The coordinates
        ///     are in the target's local coordinate system. This is not called if
        ///     <see cref="Drag(DragSource, Payload, float, float, int)" /> returned false.
        /// </summary>
        public abstract void Drop( DragSource source, Payload payload, float x, float y, int pointer );
    }

    /// <summary>
    ///     The payload of a drag and drop operation. Actors can be optionally provided to follow
    ///     the cursor and change when over a target. Such actors will be added the stage automatically
    ///     during the drag operation as necessary and they will only be removed from the stage if they
    ///     were added automatically. A source actor can be used a payload drag actor.
    /// </summary>
    public class Payload
    {
        public Actor?  DragActor        { get; set; }
        public Actor?  ValidDragActor   { get; set; }
        public Actor?  InvalidDragActor { get; set; }
        public object? Object           { get; set; }
    }
}
