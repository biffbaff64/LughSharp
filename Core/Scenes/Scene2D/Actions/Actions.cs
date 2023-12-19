// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Diagnostics.Tracing;

using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[PublicAPI]
public class Actions
{
    /// <summary>
    /// Returns a new or pooled action of the specified type.
    /// </summary>
    public static T Action<T>() where T : Action
    {
        Pool< T > pool   = Pools< T >.Get();
        T?        action = pool.Obtain();

        action!.Pool = pool;

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

    public static RemoveAction removeAction( Action action )
    {
        RemoveAction removeAction = action( RemoveAction.class);
        removeAction.setAction( action );

        return removeAction;
    }

    public static RemoveAction removeAction( Action action, Actor targetActor )
    {
        RemoveAction removeAction = action( RemoveAction.class);
        removeAction.setTarget( targetActor );
        removeAction.setAction( action );

        return removeAction;
    }

    /** Moves the actor instantly. */
    public static MoveToAction moveTo( float x, float y )
    {
        return moveTo( x, y, 0, null );
    }

    public static MoveToAction moveTo( float x, float y, float duration )
    {
        return moveTo( x, y, duration, null );
    }

    public static MoveToAction moveTo( float x, float y, float duration, @Null Interpolation interpolation )
    {
        MoveToAction action = action( MoveToAction.class);
        action.setPosition( x, y );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    public static MoveToAction moveToAligned( float x, float y, int alignment )
    {
        return moveToAligned( x, y, alignment, 0, null );
    }

    public static MoveToAction moveToAligned( float x, float y, int alignment, float duration )
    {
        return moveToAligned( x, y, alignment, duration, null );
    }

    public static MoveToAction moveToAligned( float x, float y, int alignment, float duration, @Null Interpolation interpolation )
    {
        MoveToAction action = action( MoveToAction.class);
        action.setPosition( x, y, alignment );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Moves the actor instantly. */
    public static MoveByAction moveBy( float amountX, float amountY )
    {
        return moveBy( amountX, amountY, 0, null );
    }

    public static MoveByAction moveBy( float amountX, float amountY, float duration )
    {
        return moveBy( amountX, amountY, duration, null );
    }

    public static MoveByAction moveBy( float amountX, float amountY, float duration, @Null Interpolation interpolation )
    {
        MoveByAction action = action( MoveByAction.class);
        action.setAmount( amountX, amountY );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Sizes the actor instantly. */
    public static SizeToAction sizeTo( float x, float y )
    {
        return sizeTo( x, y, 0, null );
    }

    public static SizeToAction sizeTo( float x, float y, float duration )
    {
        return sizeTo( x, y, duration, null );
    }

    public static SizeToAction sizeTo( float x, float y, float duration, @Null Interpolation interpolation )
    {
        SizeToAction action = action( SizeToAction.class);
        action.setSize( x, y );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Sizes the actor instantly. */
    public static SizeByAction sizeBy( float amountX, float amountY )
    {
        return sizeBy( amountX, amountY, 0, null );
    }

    public static SizeByAction sizeBy( float amountX, float amountY, float duration )
    {
        return sizeBy( amountX, amountY, duration, null );
    }

    public static SizeByAction sizeBy( float amountX, float amountY, float duration, @Null Interpolation interpolation )
    {
        SizeByAction action = action( SizeByAction.class);
        action.setAmount( amountX, amountY );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Scales the actor instantly. */
    public static ScaleToAction scaleTo( float x, float y )
    {
        return scaleTo( x, y, 0, null );
    }

    public static ScaleToAction scaleTo( float x, float y, float duration )
    {
        return scaleTo( x, y, duration, null );
    }

    public static ScaleToAction scaleTo( float x, float y, float duration, @Null Interpolation interpolation )
    {
        ScaleToAction action = action( ScaleToAction.class);
        action.setScale( x, y );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Scales the actor instantly. */
    public static ScaleByAction scaleBy( float amountX, float amountY )
    {
        return scaleBy( amountX, amountY, 0, null );
    }

    public static ScaleByAction scaleBy( float amountX, float amountY, float duration )
    {
        return scaleBy( amountX, amountY, duration, null );
    }

    public static ScaleByAction scaleBy( float amountX, float amountY, float duration, @Null Interpolation interpolation )
    {
        ScaleByAction action = action( ScaleByAction.class);
        action.setAmount( amountX, amountY );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Rotates the actor instantly. */
    public static RotateToAction rotateTo( float rotation )
    {
        return rotateTo( rotation, 0, null );
    }

    public static RotateToAction rotateTo( float rotation, float duration )
    {
        return rotateTo( rotation, duration, null );
    }

    public static RotateToAction rotateTo( float rotation, float duration, @Null Interpolation interpolation )
    {
        RotateToAction action = action( RotateToAction.class);
        action.setRotation( rotation );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Rotates the actor instantly. */
    public static RotateByAction rotateBy( float rotationAmount )
    {
        return rotateBy( rotationAmount, 0, null );
    }

    public static RotateByAction rotateBy( float rotationAmount, float duration )
    {
        return rotateBy( rotationAmount, duration, null );
    }

    public static RotateByAction rotateBy( float rotationAmount, float duration, @Null Interpolation interpolation )
    {
        RotateByAction action = action( RotateByAction.class);
        action.setAmount( rotationAmount );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Sets the actor's color instantly. */
    public static ColorAction color( Color color )
    {
        return color( color, 0, null );
    }

    /** Transitions from the color at the time this action starts to the specified color. */
    public static ColorAction color( Color color, float duration )
    {
        return color( color, duration, null );
    }

    /** Transitions from the color at the time this action starts to the specified color. */
    public static ColorAction color( Color color, float duration, @Null Interpolation interpolation )
    {
        ColorAction action = action( ColorAction.class);
        action.setEndColor( color );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Sets the actor's alpha instantly. */
    public static AlphaAction alpha( float a )
    {
        return alpha( a, 0, null );
    }

    /** Transitions from the alpha at the time this action starts to the specified alpha. */
    public static AlphaAction alpha( float a, float duration )
    {
        return alpha( a, duration, null );
    }

    /** Transitions from the alpha at the time this action starts to the specified alpha. */
    public static AlphaAction alpha( float a, float duration, @Null Interpolation interpolation )
    {
        AlphaAction action = action( AlphaAction.class);
        action.setAlpha( a );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 0. */
    public static AlphaAction fadeOut( float duration )
    {
        return alpha( 0, duration, null );
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 0. */
    public static AlphaAction fadeOut( float duration, @Null Interpolation interpolation )
    {
        AlphaAction action = action( AlphaAction.class);
        action.setAlpha( 0 );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 1. */
    public static AlphaAction fadeIn( float duration )
    {
        return alpha( 1, duration, null );
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 1. */
    public static AlphaAction fadeIn( float duration, @Null Interpolation interpolation )
    {
        AlphaAction action = action( AlphaAction.class);
        action.setAlpha( 1 );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    public static VisibleAction show()
    {
        return visible( true );
    }

    public static VisibleAction hide()
    {
        return visible( false );
    }

    public static VisibleAction visible( boolean visible )
    {
        VisibleAction action = action( VisibleAction.class);
        action.setVisible( visible );

        return action;
    }

    public static TouchableAction touchable( Touchable touchable )
    {
        TouchableAction action = action( TouchableAction.class);
        action.setTouchable( touchable );

        return action;
    }

    public static RemoveActorAction removeActor()
    {
        return action( RemoveActorAction.class);
    }

    public static RemoveActorAction removeActor( Actor removeActor )
    {
        RemoveActorAction action = action( RemoveActorAction.class);
        action.setTarget( removeActor );

        return action;
    }

    public static DelayAction delay( float duration )
    {
        DelayAction action = action( DelayAction.class);
        action.setDuration( duration );

        return action;
    }

    public static DelayAction delay( float duration, Action delayedAction )
    {
        DelayAction action = action( DelayAction.class);
        action.setDuration( duration );
        action.setAction( delayedAction );

        return action;
    }

    public static TimeScaleAction timeScale( float scale, Action scaledAction )
    {
        TimeScaleAction action = action( TimeScaleAction.class);
        action.setScale( scale );
        action.setAction( scaledAction );

        return action;
    }

    public static SequenceAction sequence( Action action1 )
    {
        SequenceAction action = action( SequenceAction.class);
        action.addAction( action1 );

        return action;
    }

    public static SequenceAction sequence( Action action1, Action action2 )
    {
        SequenceAction action = action( SequenceAction.class);
        action.addAction( action1 );
        action.addAction( action2 );

        return action;
    }

    public static SequenceAction sequence( Action action1, Action action2, Action action3 )
    {
        SequenceAction action = action( SequenceAction.class);
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );

        return action;
    }

    public static SequenceAction sequence( Action action1, Action action2, Action action3, Action action4 )
    {
        SequenceAction action = action( SequenceAction.class);
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );

        return action;
    }

    public static SequenceAction sequence( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        SequenceAction action = action( SequenceAction.class);
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );
        action.addAction( action5 );

        return action;
    }

    public static SequenceAction sequence( Action...actions )
    {
        SequenceAction action = action( SequenceAction.class);

        for ( int i = 0, n = actions.length; i < n; i++ )
            action.addAction( actions[ i ] );

        return action;
    }

    public static SequenceAction sequence()
    {
        return action( SequenceAction.class);
    }

    public static ParallelAction parallel( Action action1 )
    {
        ParallelAction action = action( ParallelAction.class);
        action.addAction( action1 );

        return action;
    }

    public static ParallelAction parallel( Action action1, Action action2 )
    {
        ParallelAction action = action( ParallelAction.class);
        action.addAction( action1 );
        action.addAction( action2 );

        return action;
    }

    public static ParallelAction parallel( Action action1, Action action2, Action action3 )
    {
        ParallelAction action = action( ParallelAction.class);
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );

        return action;
    }

    public static ParallelAction parallel( Action action1, Action action2, Action action3, Action action4 )
    {
        ParallelAction action = action( ParallelAction.class);
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );

        return action;
    }

    public static ParallelAction parallel( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        ParallelAction action = action( ParallelAction.class);
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );
        action.addAction( action5 );

        return action;
    }

    public static ParallelAction parallel( Action...actions )
    {
        ParallelAction action = action( ParallelAction.class);

        for ( int i = 0, n = actions.length; i < n; i++ )
            action.addAction( actions[ i ] );

        return action;
    }

    public static ParallelAction parallel()
    {
        return action( ParallelAction.class);
    }

    public static RepeatAction repeat( int count, Action repeatedAction )
    {
        RepeatAction action = action( RepeatAction.class);
        action.setCount( count );
        action.setAction( repeatedAction );

        return action;
    }

    public static RepeatAction forever( Action repeatedAction )
    {
        RepeatAction action = action( RepeatAction.class);
        action.setCount( RepeatAction.FOREVER );
        action.setAction( repeatedAction );

        return action;
    }

    public static RunnableAction run( Runnable runnable )
    {
        RunnableAction action = action( RunnableAction.class);
        action.setRunnable( runnable );

        return action;
    }

    public static LayoutAction layout( boolean enabled )
    {
        LayoutAction action = action( LayoutAction.class);
        action.setLayoutEnabled( enabled );

        return action;
    }

    public static AfterAction after( Action action )
    {
        AfterAction afterAction = action( AfterAction.class);
        afterAction.setAction( action );

        return afterAction;
    }

    public static AddListenerAction addListener( EventListener listener, boolean capture )
    {
        AddListenerAction addAction = action( AddListenerAction.class);
        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static AddListenerAction addListener( EventListener listener, boolean capture, Actor targetActor )
    {
        AddListenerAction addAction = action( AddListenerAction.class);
        addAction.setTarget( targetActor );
        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static RemoveListenerAction removeListener( EventListener listener, boolean capture )
    {
        RemoveListenerAction addAction = action( RemoveListenerAction.class);
        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static RemoveListenerAction removeListener( EventListener listener, boolean capture, Actor targetActor )
    {
        RemoveListenerAction addAction = action( RemoveListenerAction.class);
        addAction.setTarget( targetActor );
        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    /**
     * Sets the target of an action and returns the action.
     * @param target the desired target of the action
     * @param action the action on which to set the target
     * @return the action with its target set
     */
    public static Action targeting( Actor target, Action action )
    {
        action.setTarget( target );

        return action;
    }
}
