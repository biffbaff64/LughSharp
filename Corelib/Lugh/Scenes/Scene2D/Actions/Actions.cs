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

using Corelib.Lugh.Graphics;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Scenes.Scene2D.Listeners;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Pooling;

namespace Corelib.Lugh.Scenes.Scene2D.Actions;

[PublicAPI]
public class Actions
{
    /// <summary>
    /// Returns a new or pooled action of the specified type.
    /// </summary>
    public static Action Action( Type a )
    {
        Pool< Action > pool   = Pools< Action >.Get();
        var            action = pool.Obtain();

        action!.Pool = pool;

        return action;
    }

    public static AddAction AddAction( Action action )
    {
        var addAction = ( AddAction ) Action( typeof( AddAction ) );
        addAction.Action = action;

        return addAction;
    }

    public static AddAction AddAction( Action action, Actor targetActor )
    {
        var addAction = ( AddAction ) Action( typeof( AddAction ) );
        addAction.Target = targetActor;
        addAction.Action = action;

        return addAction;
    }

    public static RemoveAction RemoveAction( Action action )
    {
        var removeAction = ( RemoveAction ) Action( typeof( RemoveAction ) );
        removeAction.Action = action;

        return removeAction;
    }

    public static RemoveAction RemoveAction( Action action, Actor targetActor )
    {
        var removeAction = ( RemoveAction ) Action( typeof( RemoveAction ) );
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
        var action = ( MoveToAction ) Action( typeof( MoveToAction ) );
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
        var action = ( MoveToAction ) Action( typeof( MoveToAction ) );
        action.SetPosition( x, y, alignment );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static MoveByAction MoveBy( float amountX,
                                       float amountY,
                                       float duration = 0,
                                       IInterpolation? interpolation = null )
    {
        var action = ( MoveByAction ) Action( typeof( MoveByAction ) );
        action.SetAmount( amountX, amountY );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static SizeToAction SizeTo( float x,
                                       float y,
                                       float duration = 0,
                                       IInterpolation? interpolation = null )
    {
        var action = ( SizeToAction ) Action( typeof( SizeToAction ) );
        action.SetSize( x, y );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static SizeByAction SizeBy( float amountX,
                                       float amountY,
                                       float duration = 0,
                                       IInterpolation? interpolation = null )
    {
        var action = ( SizeByAction ) Action( typeof( SizeByAction ) );
        action.SetAmount( amountX, amountY );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static ScaleToAction ScaleTo( float x,
                                         float y,
                                         float duration = 0,
                                         IInterpolation? interpolation = null )
    {
        var action = ( ScaleToAction ) Action( typeof( ScaleToAction ) );
        action.SetScale( x, y );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static ScaleByAction ScaleBy( float amountX,
                                         float amountY,
                                         float duration = 0,
                                         IInterpolation? interpolation = null )
    {
        var action = ( ScaleByAction ) Action( typeof( ScaleByAction ) );
        action.SetAmount( amountX, amountY );
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static RotateToAction RotateTo( float rotation,
                                           float duration = 0,
                                           IInterpolation? interpolation = null )
    {
        var action = ( RotateToAction ) Action( typeof( RotateToAction ) );
        action.Rotation      = rotation;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static RotateByAction RotateBy( float rotationAmount,
                                           float duration = 0,
                                           IInterpolation? interpolation = null )
    {
        var action = ( RotateByAction ) Action( typeof( RotateByAction ) );
        action.Amount        = rotationAmount;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Transitions from the color at the time this action starts to the specified color.
    /// </summary>
    public static ColorAction Color( Color color, float duration = 0, IInterpolation? interpolation = null )
    {
        var action = ( ColorAction ) Action( typeof( ColorAction ) );
        action.EndColor      = color;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Transitions from the alpha at the time this action starts to the specified alpha.
    /// </summary>
    public static AlphaAction Alpha( float a,
                                     float duration = 0,
                                     IInterpolation? interpolation = null )
    {
        var action = ( AlphaAction ) Action( typeof( AlphaAction ) );
        action.Alpha         = a;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Transitions from the alpha at the time this action starts to an alpha of 0.
    /// </summary>
    public static AlphaAction FadeOut( float duration )
    {
        return Alpha( 0, duration );
    }

    /// <summary>
    /// Transitions from the alpha at the time this action starts to an alpha of 0.
    /// </summary>
    public static AlphaAction FadeOut( float duration, IInterpolation interpolation )
    {
        var action = ( AlphaAction ) Action( typeof( AlphaAction ) );
        action.Alpha         = 0;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Transitions from the alpha at the time this action starts to an alpha of 1.
    /// </summary>
    public static AlphaAction FadeIn( float duration )
    {
        return Alpha( 1, duration );
    }

    /// <summary>
    /// Transitions from the alpha at the time this action starts to an alpha of 1.
    /// </summary>
    public static AlphaAction FadeIn( float duration, IInterpolation interpolation )
    {
        var action = ( AlphaAction ) Action( typeof( AlphaAction ) );
        action.Alpha         = 1;
        action.Duration      = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static VisibleAction Show()
    {
        return Visible( true );
    }

    public static VisibleAction Hide()
    {
        return Visible( false );
    }

    public static VisibleAction Visible( bool visible )
    {
        var action = ( VisibleAction ) Action( typeof( VisibleAction ) );
        action.Visible = visible;

        return action;
    }

    public static TouchableAction Touchable( Touchable touchable )
    {
        var action = ( TouchableAction ) Action( typeof( TouchableAction ) );
        action.Touchable = touchable;

        return action;
    }

    public static RemoveActorAction RemoveActor()
    {
        return ( RemoveActorAction ) Action( typeof( RemoveActorAction ) );
    }

    public static RemoveActorAction RemoveActor( Actor removeActor )
    {
        var action = ( RemoveActorAction ) Action( typeof( RemoveActorAction ) );
        action.Target = removeActor;

        return action;
    }

    public static DelayAction Delay( float duration )
    {
        var action = ( DelayAction ) Action( typeof( DelayAction ) );
        action.Duration = duration;

        return action;
    }

    public static DelayAction Delay( float duration, Action delayedAction )
    {
        var action = ( DelayAction ) Action( typeof( DelayAction ) );
        action.Duration = duration;
        action.Action   = delayedAction;

        return action;
    }

    public static TimeScaleAction TimeScale( float scale, Action scaledAction )
    {
        var action = ( TimeScaleAction ) Action( typeof( TimeScaleAction ) );
        action.Scale  = scale;
        action.Action = scaledAction;

        return action;
    }

    public static SequenceAction Sequence( Action action1 )
    {
        var action = ( SequenceAction ) Action( typeof( SequenceAction ) );
        action.AddAction( action1 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2 )
    {
        var action = ( SequenceAction ) Action( typeof( SequenceAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3 )
    {
        var action = ( SequenceAction ) Action( typeof( SequenceAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3, Action action4 )
    {
        var action = ( SequenceAction ) Action( typeof( SequenceAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        var action = ( SequenceAction ) Action( typeof( SequenceAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );
        action.AddAction( action5 );

        return action;
    }

    public static SequenceAction Sequence( params Action[] actions )
    {
        var action = ( SequenceAction ) Action( typeof( SequenceAction ) );

        for ( int i = 0, n = actions.Length; i < n; i++ )
        {
            action.AddAction( actions[ i ] );
        }

        return action;
    }

    public static SequenceAction Sequence()
    {
        return ( SequenceAction ) Action( typeof( SequenceAction ) );
    }

    public static ParallelAction Parallel( Action action1 )
    {
        var action = ( ParallelAction ) Action( typeof( ParallelAction ) );
        action.AddAction( action1 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2 )
    {
        var action = ( ParallelAction ) Action( typeof( ParallelAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3 )
    {
        var action = ( ParallelAction ) Action( typeof( ParallelAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3, Action action4 )
    {
        var action = ( ParallelAction ) Action( typeof( ParallelAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        var action = ( ParallelAction ) Action( typeof( ParallelAction ) );
        action.AddAction( action1 );
        action.AddAction( action2 );
        action.AddAction( action3 );
        action.AddAction( action4 );
        action.AddAction( action5 );

        return action;
    }

    public static ParallelAction Parallel( params Action[] actions )
    {
        var action = ( ParallelAction ) Action( typeof( ParallelAction ) );

        for ( int i = 0, n = actions.Length; i < n; i++ )
        {
            action.AddAction( actions[ i ] );
        }

        return action;
    }

    public static ParallelAction Parallel()
    {
        return ( ParallelAction ) Action( typeof( ParallelAction ) );
    }

    public static RepeatAction Repeat( int count, Action repeatedAction )
    {
        var action = ( RepeatAction ) Action( typeof( RepeatAction ) );
        action.RepeatCount = count;
        action.Action      = repeatedAction;

        return action;
    }

    public static RepeatAction Forever( Action repeatedAction )
    {
        var action = ( RepeatAction ) Action( typeof( RepeatAction ) );
        action.RepeatCount = RepeatAction.FOREVER;
        action.Action      = repeatedAction;

        return action;
    }

    public static RunnableAction Run( IRunnable.Runnable runnable )
    {
        var action = ( RunnableAction ) Action( typeof( RunnableAction ) );
        action.RunnableTask = runnable;

        return action;
    }

    public static LayoutAction Layout( bool enabled )
    {
        var action = ( LayoutAction ) Action( typeof( LayoutAction ) );
        action.Enabled = enabled;

        return action;
    }

    public static AfterAction After( Action action )
    {
        var afterAction = ( AfterAction ) Action( typeof( AfterAction ) );
        afterAction.Action = action;

        return afterAction;
    }

    public static AddListenerAction AddListener( IEventListener listener, bool capture )
    {
        var addAction = ( AddListenerAction ) Action( typeof( AddListenerAction ) );
        addAction.Listener  = listener;
        addAction.IsCapture = capture;

        return addAction;
    }

    public static AddListenerAction AddListener( IEventListener listener, bool capture, Actor targetActor )
    {
        var addAction = ( AddListenerAction ) Action( typeof( AddListenerAction ) );
        addAction.Target    = targetActor;
        addAction.Listener  = listener;
        addAction.IsCapture = capture;

        return addAction;
    }

    public static RemoveListenerAction RemoveListener( IEventListener listener, bool capture )
    {
        var addAction = ( RemoveListenerAction ) Action( typeof( RemoveListenerAction ) );
        addAction.Listener = listener;
        addAction.Capture  = capture;

        return addAction;
    }

    public static RemoveListenerAction RemoveListener( IEventListener listener, bool capture, Actor targetActor )
    {
        var addAction = ( RemoveListenerAction ) Action( typeof( RemoveListenerAction ) );
        addAction.Target   = targetActor;
        addAction.Listener = listener;
        addAction.Capture  = capture;

        return addAction;
    }

    /// <summary>
    /// Sets the target of an action and returns the action
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
