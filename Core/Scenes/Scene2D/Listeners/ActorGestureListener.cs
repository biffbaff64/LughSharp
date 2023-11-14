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

using LibGDXSharp.Scenes.Scene2D;

namespace LibGDXSharp.Scenes.Listeners;

/// <summary>
/// Detects tap, long press, fling, pan, zoom, and pinch gestures on an actor.
/// If there is only a need to detect tap, use <see cref="ClickListener"/>.
/// </summary>
[PublicAPI]
public class ActorGestureListener : IEventListener
{
    private static Vector2 _tmpCoords  = new();
    private static Vector2 _tmpCoords2 = new();

    public GestureDetector? Detector        { get; set; }
    public Actor?           TouchDownTarget { get; set; }

    private InputEvent? _ev;
    private Actor?      _actor;

    /** @see GestureDetector#GestureDetector(com.badlogic.gdx.input.GestureDetector.GestureListener) */
    public ActorGestureListener()
        : this( 20, 0.4f, 1.1f, int.MaxValue )
    {
    }

    /** @see GestureDetector#GestureDetector(float, float, float, float, com.badlogic.gdx.input.GestureDetector.GestureListener) */
    public ActorGestureListener( float halfTapSquareSize, float tapCountInterval, float longPressDuration, float maxFlingDelay )
    {
//        Detector = new GestureDetector( halfTapSquareSize, tapCountInterval, longPressDuration, maxFlingDelay, new GestureAdapter()
//        {
//            private Vector2 _initialPointer1 = new();
//            private Vector2 _initialPointer2 = new();
//            private Vector2 _pointer1        = new();
//            private Vector2 _pointer2        = new();
//
//            public bool Tap( float stageX, float stageY, int count, int button )
//            {
//                _actor.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );
//                ActorGestureListener.Tap( _ev, _tmpCoords.X, _tmpCoords.Y, count, button );
//
//                return true;
//            }
//
//            public bool LongPress( float stageX, float stageY )
//            {
//                _actor.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );
//
//                return ActorGestureListener.this.LongPress( _actor, _tmpCoords.x, _tmpCoords.y );
//            }
//
//            public bool Fling( float velocityX, float velocityY, int button )
//            {
//                StageToLocalAmount( _tmpCoords.Set( velocityX, velocityY ) );
//                ActorGestureListener.Fling( _ev, _tmpCoords.X, _tmpCoords.Y, button );
//
//                return true;
//            }
//
//            public bool Pan( float stageX, float stageY, float deltaX, float deltaY )
//            {
//                StageToLocalAmount( _tmpCoords.Set( deltaX, deltaY ) );
//
//                deltaX = _tmpCoords.X;
//                deltaY = _tmpCoords.Y;
//                
//                _actor.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );
//                ActorGestureListener.Pan( _ev, _tmpCoords.X, _tmpCoords.Y, deltaX, deltaY );
//
//                return true;
//            }
//
//            public bool PanStop( float stageX, float stageY, int pointer, int button )
//            {
//                _actor.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );
//                ActorGestureListener.PanStop( _ev, _tmpCoords.X, _tmpCoords.Y, pointer, button );
//
//                return true;
//            }
//
//            public bool Zoom( float initialDistance, float distance )
//            {
//                ActorGestureListener.Zoom( _ev, initialDistance, distance );
//
//                return true;
//            }
//
//            public bool Pinch( Vector2 stageInitialPointer1,
//                               Vector2 stageInitialPointer2,
//                               Vector2 stagePointer1,
//                               Vector2 stagePointer2 )
//            {
//                _actor.StageToLocalCoordinates( _initialPointer1.Set( stageInitialPointer1 ) );
//                _actor.StageToLocalCoordinates( _initialPointer2.Set( stageInitialPointer2 ) );
//                _actor.StageToLocalCoordinates( _pointer1.Set( stagePointer1 ) );
//                _actor.StageToLocalCoordinates( _pointer2.Set( stagePointer2 ) );
//
//                ActorGestureListener.Pinch( _ev, _initialPointer1, _initialPointer2, _pointer1, _pointer2 );
//
//                return true;
//            }
//
//            private void StageToLocalAmount( Vector2 amount )
//            {
//                _actor.StageToLocalCoordinates( amount );
//                amount.Sub( _actor.StageToLocalCoordinates( _tmpCoords2.Set( 0, 0 ) ) );
//            }
//        });
    }

    public bool Handle( Event e )
    {
        if ( !( e is InputEvent ev ) )
        {
            return false;
        }

        switch ( ev.Type )
        {
            case InputEvent.EventType.TouchDown:
                _actor          = ev.ListenerActor;
                TouchDownTarget = ev.TargetActor;

                Detector.TouchDown( ev.StageX, ev.StageY, ev.Pointer, ev.Button );
                _actor?.StageToLocalCoordinates( _tmpCoords.Set( ev.StageX, ev.StageY ) );
                TouchDown( ev, _tmpCoords.X, _tmpCoords.Y, ev.Pointer, ev.Button );

                if ( ev.TouchFocus )
                {
                    ev.Stage?.AddTouchFocus( this,
                                             ev.ListenerActor,
                                             ev.TargetActor,
                                             ev.Pointer,
                                             ev.Button );
                }

                return true;

            case InputEvent.EventType.TouchUp:
                if ( ev.TouchFocusCancel )
                {
                    Detector.Reset();

                    return false;
                }

                this._ev = ev;
                _actor   = ev.ListenerActor;

                Detector.TouchUp( ev.StageX, ev.StageY, ev.Pointer, ev.Button );
                _actor?.StageToLocalCoordinates( _tmpCoords.Set( ev.StageX, ev.StageY ) );
                TouchUp( ev, _tmpCoords.X, _tmpCoords.Y, ev.Pointer, ev.Button );

                return true;

            case InputEvent.EventType.TouchDragged:
                this._ev = ev;
                _actor   = ev.ListenerActor;
                Detector.TouchDragged( ev.StageX, ev.StageY, ev.Pointer );

                return true;
        }

        return false;
    }

    public void TouchDown( InputEvent ev, float x, float y, int pointer, int button )
    {
    }

    public void TouchUp( InputEvent ev, float x, float y, int pointer, int button )
    {
    }

    public void Tap( InputEvent ev, float x, float y, int count, int button )
    {
    }

    /// <summary>
    /// If true is returned, additional gestures will not be triggered. No ev is
    /// provided because this ev is triggered by time passing, not by an InputEvent.
    /// </summary>
    public bool LongPress( Actor actor, float x, float y )
    {
        return false;
    }

    public void Fling( InputEvent ev, float velocityX, float velocityY, int button )
    {
    }

    /// <summary>
    /// The delta is the difference in stage coordinates since the last pan.
    /// </summary>
    public void Pan( InputEvent ev, float x, float y, float deltaX, float deltaY )
    {
    }

    public void PanStop( InputEvent ev, float x, float y, int pointer, int button )
    {
    }

    public void Zoom( InputEvent ev, float initialDistance, float distance )
    {
    }

    public void Pinch( InputEvent ev,
                       Vector2 initialPointer1,
                       Vector2 initialPointer2,
                       Vector2 pointer1,
                       Vector2 pointer2 )
    {
    }
}
