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

using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Scenes.Scene2D.UI;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// Manages drag and drop operations through registered
/// drag sources and drop targets.
/// </summary>
[PublicAPI]
public class DragAndDrop
{
    /// <summary>
    /// Time in milliseconds that a drag must take before a drop will be
    /// considered valid. This ignores an accidental drag and drop that
    /// was meant to be a click. Default is 250ms.
    /// </summary>
    public int DragTime { get; set; } = 250;

    private readonly static Vector2                            TmpVector        = new();
    private readonly        Dictionary< Source, DragListener > _sourceListeners = new();
    private readonly        List< Target >                     _targets         = new();

    private Source       _dragSource;
    private Payload      _payload;
    private Actor        _dragActor;
    private Target       _target;
    private DragListener _dragListener;

    private bool  _cancelTouchFocus = true;
    private bool  _keepWithinStage  = true;
    private bool  _removeDragActor;
    private bool  _isValidTarget;
    private float _tapSquareSize = 8;
    private float _dragActorX    = 0;
    private float _dragActorY    = 0;
    private float _touchOffsetX;
    private float _touchOffsetY;
    private long  _dragValidTime;
    private int   _button;

    protected int activePointer = -1;

    // ------------------------------------------------------------------------

    internal class DragListenerImpl : DragListener
    {
        private readonly DragAndDrop _parent;
        private readonly Source      _source;

        internal DragListenerImpl( DragAndDrop parent, Source source )
        {
            this._parent = parent;
            this._source = source;
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
            _parent._dragSource    = _source;
            _parent._payload       = _source.DragStart( ev, TouchDownX, TouchDownY, pointer )!;

            ev.Stop();
        }
    }

    public void AddSource( Source source )
    {
        DragListener listener = new
        {
 

            public void dragStart( InputEvent ev, float x, float y, int pointer )
            {
            if (activePointer != -1)
            {
            ev.stop();
            return;
        }

        activePointer = pointer;

        _dragValidTime = TimeUtils.Millis() + DragTime;
        _dragSource    = source;
        _payload       = source.dragStart( ev, getTouchDownX(), getTouchDownY(), pointer );

        ev.stop();

        if ( _cancelTouchFocus && ( _payload != null ) )
        {
            Stage stage = source.getActor().getStage();

            if ( stage != null )
            {
                stage.cancelTouchFocusExcept( this, source.getActor() );
            }
        }

        }

        public void Drag( InputEvent event, float x, float y, int pointer )
        {
            if ( _payload == null )
            {
                return;
            }

            if ( pointer != activePointer )
            {
                return;
            }

            source.drag(  event, x, y, pointer);

            Stage stage =  event.getStage();

            // Move the drag actor away, so it cannot be hit.
            Actor oldDragActor  = _dragActor;
            float oldDragActorX = 0, oldDragActorY = 0;

            if ( oldDragActor != null )
            {
                oldDragActorX = oldDragActor.getX();
                oldDragActorY = oldDragActor.getY();
                oldDragActor.setPosition( Integer.MAX_VALUE, Integer.MAX_VALUE );
            }

            float stageX =  event.getStageX() + _touchOffsetX, stageY =  event.getStageY() + _touchOffsetY;
            Actor hit    =  event.getStage().hit( stageX, stageY, true ); // Prefer touchable actors.

            if ( hit == null )
            {
                hit =  event.
            }

            getStage().hit( stageX, stageY, false );

            if ( oldDragActor != null )
            {
                oldDragActor.setPosition( oldDragActorX, oldDragActorY );
            }

            // Find target.
            Target newTarget = null;
            _isValidTarget = false;

            if ( hit != null )
            {
                for ( int i = 0, n = _targets.size; i < n; i++ )
                {
                    Target target = _targets.get( i );

                    if ( !target.actor.isAscendantOf( hit ) )
                    {
                        continue;
                    }

                    newTarget = target;
                    target.actor.stageToLocalCoordinates( TmpVector.set( stageX, stageY ) );

                    break;
                }
            }

            // If over a new target, notify the former target that it's being left behind.
            if ( newTarget != _target )
            {
                if ( _target != null )
                {
                    _target.reset( source, _payload );
                }

                _target = newTarget;
            }

            // Notify new target of drag.
            if ( newTarget != null )
            {
                _isValidTarget = newTarget.drag( source, _payload, TmpVector.x, TmpVector.y, pointer );
            }

            // Determine the drag actor, remove the old one if it was added by DragAndDrop, and add the new one.
            Actor actor = null;

            if ( _target != null )
            {
                actor = _isValidTarget ? _payload.validDragActor : _payload.invalidDragActor;
            }

            if ( actor == null )
            {
                actor = _payload.dragActor;
            }

            if ( actor != oldDragActor )
            {
                if ( ( oldDragActor != null ) && _removeDragActor )
                {
                    oldDragActor.remove();
                }

                _dragActor       = actor;
                _removeDragActor = actor.getStage() == null; // Only remove later if not already in the stage now.

                if ( _removeDragActor )
                {
                    stage.addActor( actor );
                }
            }

            if ( actor == null )
            {
                return;
            }

            // Position the drag actor.
            float actorX =  event.( getStageX() - actor.getWidth() ) + _dragActorX;
            float actorY =  event.getStageY() + _dragActorY;

            if ( _keepWithinStage )
            {
                if ( actorX < 0 )
                {
                    actorX = 0;
                }

                if ( actorY < 0 )
                {
                    actorY = 0;
                }

                if ( ( actorX + actor.getWidth() ) > stage.getWidth() )
                {
                    actorX = stage.getWidth() - actor.getWidth();
                }

                if ( ( actorY + actor.getHeight() ) > stage.getHeight() )
                {
                    actorY = stage.getHeight() - actor.getHeight();
                }
            }

            actor.SetPosition( actorX, actorY );
        }

        public void DragStop( InputEvent event, float x, float y, int pointer )
        {
            if ( pointer != activePointer )
            {
                return;
            }

            activePointer = -1;

            if ( _payload == null )
            {
                return;
            }

            if ( System.currentTimeMillis() < _dragValidTime )
            {
                _isValidTarget = false;
            }

            if ( ( _dragActor != null ) && _removeDragActor )
            {
                _dragActor.remove();
            }

            if ( _isValidTarget )
            {
                float stageX =  event.getStageX() + _touchOffsetX, stageY =  event.getStageY() + _touchOffsetY;
                _target.actor.stageToLocalCoordinates( TmpVector.set( stageX, stageY ) );
                _target.drop( source, _payload, TmpVector.x, TmpVector.y, pointer );
            }

            source.dragStop(  event, x, y, pointer, _payload, _isValidTarget ? _target : null);

            if ( _target != null )
            {
                _target.Reset( source, _payload );
            }

            _dragSource    = null;
            _payload       = null;
            _target        = null;
            _isValidTarget = false;
            _dragActor     = null;
        }

        };

        listener.SetTapSquareSize( _tapSquareSize );
        listener.SetButton( _button );

        source.Actor.AddCaptureListener( listener );

        _sourceListeners[ source ] = listener;
    }

    public void RemoveSource( Source source )
    {
        _sourceListeners.Remove( source, out DragListener? dragListener );

        source.Actor.RemoveCaptureListener( dragListener! );
    }

    public void AddTarget( Target target )
    {
        _targets.Add( target );
    }

    public void RemoveTarget( Target target )
    {
        _targets.Remove( target );
    }

    /** Removes all targets and sources. */
    public void Clear()
    {
        _targets.Clear();

        foreach ( KeyValuePair< Source, DragListener > entry in _sourceListeners )
        {
            entry.Key.Actor.RemoveCaptureListener( entry.Value );
        }

        _sourceListeners.Clear();
    }

    /** Cancels the touch focus for everything except the specified source. */
    public void CancelTouchFocusExcept( Source except )
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

    /** Sets the distance a touch must travel before being considered a drag. */
    public void SetTapSquareSize( float halfTapSquareSize )
    {
        _tapSquareSize = halfTapSquareSize;
    }

    /** Sets the button to listen for, all other buttons are ignored. Default is {@link Buttons#LEFT}. Use -1 for any button. */
    public void SetButton( int button )
    {
        this._button = button;
    }

    public void SetDragActorPosition( float dragActorX, float dragActorY )
    {
        this._dragActorX = dragActorX;
        this._dragActorY = dragActorY;
    }

    /** Sets an offset in stage coordinates from the touch position which is used to determine the drop location. Default is
     * 0,0. */
    public void SetTouchOffset( float touchOffsetX, float touchOffsetY )
    {
        this._touchOffsetX = touchOffsetX;
        this._touchOffsetY = touchOffsetY;
    }

    public bool IsDragging()
    {
        return _payload != null;
    }

    /** Returns the current drag actor, or null. */
    public Actor? GetDragActor()
    {
        return _dragActor;
    }

    /** Returns the current drag payload, or null. */
    public Payload? GetDragPayload()
    {
        return _payload;
    }

    /** Returns the current drag source, or null. */
    public Source? GetDragSource()
    {
        return _dragSource;
    }

    /** Returns true if a drag is in progress and the {@link #setDragTime(int) drag time} has elapsed since the drag started. */
    public bool IsDragValid()
    {
        return ( _payload != null ) && ( TimeUtils.Millis() >= _dragValidTime );
    }

    /** When true (default), the {@link Stage#cancelTouchFocus()} touch focus} is cancelled if
     * {@link Source#dragStart(InputEvent, float, float, int) dragStart} returns non-null. This ensures the DragAndDrop is the only
     * touch focus listener, eg when the source is inside a {@link ScrollPane} with flick scroll enabled. */
    public void SetCancelTouchFocus( bool cancelTouchFocus )
    {
        this._cancelTouchFocus = cancelTouchFocus;
    }

    public void SetKeepWithinStage( bool keepWithinStage )
    {
        this._keepWithinStage = keepWithinStage;
    }

    /// <summary>
    /// A source where a payload can be dragged from.
    /// </summary>
    [PublicAPI]
    public class Source
    {
        public Actor Actor { get; set; }

        public Source( Actor actor )
        {
            ArgumentNullException.ThrowIfNull( actor );

            this.Actor = actor;
        }

        /** Called when a drag is started on the source. The coordinates are in the source's local coordinate system.
         * @return If null the drag will not affect any targets.
         */
        public virtual Payload? DragStart( InputEvent ev, float x, float y, int pointer )
        {
        }

        /**
         * Called repeatedly during a drag which started on this source.
         */
        public virtual void Drag( InputEvent ev, float x, float y, int pointer )
        {
        }

        /** Called when a drag for the source is stopped. The coordinates are in the source's local coordinate system.
         * @param payload null if dragStart returned null.
         * @param target null if not dropped on a valid target. */
        public virtual void DragStop( InputEvent ev, float x, float y, int pointer, Payload? payload, Target? target )
        {
        }
    }

    /**
     * A target where a payload can be dropped to.
     */
    [PublicAPI]
    public abstract class Target
    {
        public Actor Actor { get; set; }

        public Target( Actor actor )
        {
            ArgumentNullException.ThrowIfNull( actor );

            this.Actor = actor;
            Stage? stage = actor.Stage;

            if ( ( stage != null ) && ( actor == stage.Root ) )
            {
                throw new ArgumentException( "The stage root cannot be a drag and drop target." );
            }
        }

        /**
         * Called when the payload is dragged over the target. The coordinates are in the target's local coordinate system.
         * @return true if this is a valid target for the payload.
         */
        public abstract bool Drag( Source source, Payload payload, float x, float y, int pointer );

        /**
         * Called when the payload is no longer over the target, whether because the touch was moved or a drop occurred. This is
         * called even if {@link #drag(Source, Payload, float, float, int)} returned false.
         */
        public void Reset( Source source, Payload payload )
        {
        }

        /**
         * Called when the payload is dropped on the target. The coordinates are in the
         * target's local coordinate system.
         * This is not called if {@link #drag(Source, Payload, float, float, int)} returned false.
         */
        public abstract void Drop( Source source, Payload payload, float x, float y, int pointer );
    }

    /// <summary>
    /// The payload of a drag and drop operation. Actors can be optionally
    /// provided to follow the cursor and change when over a target. Such
    /// actors will be added the stage automatically during the drag operation
    /// as necessary and they will only be removed from the stage if they were
    /// added automatically. A source actor can be used a payload drag actor.
    /// </summary>
    [PublicAPI]
    public class Payload
    {
        public Actor?  DragActor        { get; set; }
        public Actor?  ValidDragActor   { get; set; }
        public Actor?  InvalidDragActor { get; set; }
        public object? Object           { get; set; }
    }
}
