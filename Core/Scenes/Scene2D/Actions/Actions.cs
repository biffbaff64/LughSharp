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

using LibGDXSharp.Maths;
using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Static convenience methods for using pooled actions, intended for static import.
/// </summary>
public class Actions
{
    /// <summary>
    /// Returns a new or pooled action of the specified type.
    /// </summary>
    public static T Action<T>() where T : Action
    {
        Pool< T > pool = Pools< T >.Get();

        T action = pool.Obtain();

        action.Pool = pool as Pool< object >;

        return action;
    }

    public static AddAction AddAction( Action action )
    {
        var addAction = Action< AddAction >();
        addAction.Action = action;

        return addAction;
    }

    public static AddAction AddAction( Action action, Actor targetActor )
    {
        var addAction = Action< AddAction >();
        addAction.Target = targetActor;
        addAction.Action = action;

        return addAction;
    }

    public static RemoveAction RemoveAction( Action action )
    {
        var removeAction = Action< RemoveAction >();
        removeAction.Action = action;

        return removeAction;
    }

    public static RemoveAction RemoveAction( Action action, Actor targetActor )
    {
        var removeAction = Action< RemoveAction >();
        removeAction.Target = targetActor;
        removeAction.Action = action;

        return removeAction;
    }

    /// <summary>
    /// Moves the actor instantly.
    /// </summary>
    public static MoveToAction MoveTo( float x,
                                       float y,
                                       float duration = 0,
                                       IInterpolation? interpolation = null )
    {
        var action = Action< MoveToAction >();

        action.SetPosition( x, y );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static MoveToAction MoveToAligned( float x,
                                              float y,
                                              int alignment,
                                              float duration = 0,
                                              IInterpolation? interpolation = null )
    {
        var action = Action< MoveToAction >();

        action.SetPosition( x, y, alignment );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Moves the actor instantly.
    /// </summary>
    public static MoveByAction MoveBy( float amountX,
                                       float amountY,
                                       float duration = 0,
                                       IInterpolation? interpolation = null )
    {
        var action = Action< MoveByAction >();

        action.SetAmount( amountX, amountY );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Sizes the actor instantly.
    /// </summary>
    public static SizeToAction SizeTo( float x,
                                       float y,
                                       float duration = 0,
                                       IInterpolation? interpolation = null )
    {
        var action = Action< SizeToAction >();

        action.SetSize( x, y );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Sizes the actor instantly.
    /// </summary>
    public static SizeByAction SizeBy( float amountX,
                                       float amountY,
                                       float duration = 0,
                                       IInterpolation? interpolation = null )
    {
        var action = Action< SizeByAction >();

        action.SetAmount( amountX, amountY );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /** Scales the actor instantly. */
    public static ScaleToAction ScaleTo( float x, float y, float duration = 0, IInterpolation? interpolation = null )
    {
        var action = Action< ScaleToAction >();

        action.SetScale( x, y );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /** Scales the actor instantly. */
    public static ScaleByAction ScaleBy( float amountX,
                                         float amountY,
                                         float duration = 0,
                                         IInterpolation? interpolation = null )
    {
        var action = Action< ScaleByAction >();

        action.SetAmount( amountX, amountY );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /** Rotates the actor instantly. */
    public static RotateToAction RotateTo( float rotation, float duration = 0, IInterpolation? interpolation = null )
    {
        var action = Action< RotateToAction >();

        action.Rotation      = rotation;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /** Rotates the actor instantly. */
    public static RotateByAction RotateBy( float rotationAmount,
                                           float duration = 0,
                                           IInterpolation? interpolation = null )
    {
        var action = Action< RotateByAction >();

        action.Amount        = rotationAmount;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// Sets the actor's color instantly.
    /// Transitions from the color at the time this action starts to the specified color.
    public static ColorAction Color( Color color, float duration = 0, IInterpolation? interpolation = null )
    {
        var action = Action< ColorAction >();

        action.EndColor      = color;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /** Sets the actor's alpha instantly. */
    /** Transitions from the alpha at the time this action starts to the specified alpha. */
    public static AlphaAction Alpha( float a, float duration = 0, IInterpolation? interpolation = null )
    {
        var action = Action< AlphaAction >();

        action.Alpha         = a;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 0. */
    public static AlphaAction FadeOut( float duration )
    {
        return Alpha( 0, duration );
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 0. */
    public static AlphaAction FadeOut( float duration, IInterpolation? interpolation )
    {
        var action = Action< AlphaAction >();

        action.Alpha         = 0;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 1. */
    public static AlphaAction FadeIn( float duration )
    {
        return Alpha( 1, duration );
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 1. */
    public static AlphaAction FadeIn( float duration, IInterpolation? interpolation )
    {
        var action = Action< AlphaAction >();

        action.Alpha         = 1;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static VisibleAction Show() => Visible( true );

    public static VisibleAction Hide() => Visible( false );

    public static VisibleAction Visible( bool visible )
    {
        var action = Action< VisibleAction >();

        action.Visible = visible;

        return action;
    }

    public static TouchableAction Touchable( Touchable touchable )
    {
        var action = Action< TouchableAction >();

        action.Touchable = touchable;

        return action;
    }

    public static RemoveActorAction RemoveActor()
    {
        return Action< RemoveActorAction >();
    }

    public static RemoveActorAction RemoveActor( Actor removeActor )
    {
        var action = Action< RemoveActorAction >();

        action.Target = removeActor;

        return action;
    }

    public static DelayAction Delay( float duration )
    {
        var action = Action< DelayAction >();

        action.Duration = duration;

        return action;
    }

    public static DelayAction Delay( float duration, Action delayedAction )
    {
        var action = Action< DelayAction >();

        action.Duration = duration;
        action.Action   = delayedAction;

        return action;
    }

    public static TimeScaleAction TimeScale( float scale, Action scaledAction )
    {
        var action = Action< TimeScaleAction >();

        action.Scale  = scale;
        action.Action = scaledAction;

        return action;
    }

    public static SequenceAction Sequence( Action action1 )
    {
        var action = Action< SequenceAction >();

        action.AddAction( action1 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2 )
    {
        var action = Action< SequenceAction >();

        action.AddAction( action1 );
        action.AddAction( action2 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3 )
    {
        var action = Action< SequenceAction >();

        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3, Action action4 )
    {
        var action = Action< SequenceAction >();

        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );

        return action;
    }

    public static SequenceAction Sequence( Action action1,
                                           Action action2,
                                           Action action3,
                                           Action action4,
                                           Action action5 )
    {
        var action = Action< SequenceAction >();

        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );
        action.AddAction( action5 );

        return action;
    }

    public static SequenceAction Sequence( params Action[] actions )
    {
        var action = Action< SequenceAction >();

        for ( int i = 0, n = actions.Length; i < n; i++ )
        {
            action.AddAction( actions[ i ] );
        }

        return action;
    }

    public static SequenceAction Sequence()
    {
        return Action< SequenceAction >();
    }

    public static ParallelAction Parallel( Action action1 )
    {
        var action = Action< ParallelAction >();
        action.AddAction( action1 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2 )
    {
        var action = Action< ParallelAction >();
        action.AddAction( action1 );
        action.AddAction( action2 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3 )
    {
        var action = Action< ParallelAction >();
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3, Action action4 )
    {
        var action = Action< ParallelAction >();
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );

        return action;
    }

    public static ParallelAction Parallel( Action action1,
                                           Action action2,
                                           Action action3,
                                           Action action4,
                                           Action action5 )
    {
        var action = Action< ParallelAction >();

        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );
        action.AddAction( action5 );

        return action;
    }

    public static ParallelAction Parallel( params Action[] actions )
    {
        var action = Action< ParallelAction >();

        for ( int i = 0, n = actions.Length; i < n; i++ )
        {
            action.AddAction( actions[ i ] );
        }

        return action;
    }

    public static ParallelAction Parallel()
    {
        return Action< ParallelAction >();
    }

    public static RepeatAction Repeat( int count, Action repeatedAction )
    {
        var action = Action< RepeatAction >();
        action.RepeatCount = count;
        action.Action      = repeatedAction;

        return action;
    }

    public static RepeatAction Forever( Action repeatedAction )
    {
        var action = Action< RepeatAction >();

        action.RepeatCount = RepeatAction.FOREVER;
        action.Action      = repeatedAction;

        return action;
    }

    public static RunnableAction Run( ThreadStart runnable )
    {
        var action = Action< RunnableAction >();
        action.Runnable = runnable;

        return action;
    }

    public static LayoutAction Layout( bool enabled )
    {
        var action = Action< LayoutAction >();

        action.Enabled = enabled;

        return action;
    }

    public static AfterAction After( Action action )
    {
        var afterAction = Action< AfterAction >();

        afterAction.Action = action;

        return afterAction;
    }

    public static AddListenerAction AddListener( IEventListener listener, bool capture )
    {
        var addAction = Action< AddListenerAction >();

        addAction.Listener = listener;
        addAction.Capture  = capture;

        return addAction;
    }

    public static AddListenerAction AddListener( IEventListener listener, bool capture, Actor targetActor )
    {
        var addAction = Action< AddListenerAction >();

        addAction.Target   = targetActor;
        addAction.Listener = listener;
        addAction.Capture  = capture;

        return addAction;
    }

    public static RemoveListenerAction RemoveListener( IEventListener listener, bool capture )
    {
        var addAction = Action< RemoveListenerAction >();

        addAction.Listener = listener;
        addAction.Capture  = capture;

        return addAction;
    }

    public static RemoveListenerAction RemoveListener( IEventListener listener, bool capture, Actor targetActor )
    {
        var addAction = Action< RemoveListenerAction >();

        addAction.Target   = targetActor;
        addAction.Listener = listener;
        addAction.Capture  = capture;

        return addAction;
    }

    /// <summary>
    /// Sets the target of an action and returns the action.
    /// </summary>
    /// <param name="target"> the desired target of the action </param>
    /// <param name="action"> the action on which to set the target </param>
    /// <returns> the action with its target set </returns>
    public static Action Targeting( Actor target, Action action )
    {
        action.Target = target;

        return action;
    }
}