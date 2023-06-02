using System.Diagnostics.Tracing;

using LibGDXSharp.Maths;
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

        action.Pool = pool;

        return action;
    }

    public static AddAction AddAction( Action action )
    {
        AddAction addAction = action( typeof( AddAction ));
        addAction.setAction( action );

        return addAction;
    }

    public static AddAction AddAction( Action action, Actor targetActor )
    {
        AddAction addAction = action( AddAction.class);
        addAction.setTarget( targetActor );
        addAction.setAction( action );

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

    public static MoveToAction moveTo( float x, float y, float duration, Interpolation? interpolation )
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

    public static MoveToAction moveToAligned( float x, float y, int alignment, float duration,
                                              Interpolation? interpolation )
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

    public static MoveByAction moveBy( float amountX, float amountY, float duration, Interpolation? interpolation )
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

    public static SizeToAction sizeTo( float x, float y, float duration, Interpolation? interpolation )
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

    public static SizeByAction sizeBy( float amountX, float amountY, float duration, Interpolation? interpolation )
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

    public static ScaleToAction scaleTo( float x, float y, float duration, Interpolation? interpolation )
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

    public static ScaleByAction scaleBy( float amountX, float amountY, float duration, Interpolation? interpolation )
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

    public static RotateToAction rotateTo( float rotation, float duration, Interpolation? interpolation )
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

    public static RotateByAction rotateBy( float rotationAmount, float duration, Interpolation? interpolation )
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
    public static ColorAction color( Color color, float duration, Interpolation? interpolation )
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
    public static AlphaAction alpha( float a, float duration, Interpolation? interpolation )
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
    public static AlphaAction fadeOut( float duration, Interpolation? interpolation )
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
    public static AlphaAction fadeIn( float duration, Interpolation? interpolation )
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

    public static VisibleAction visible( bool visible )
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
        return Action( RemoveActorAction.class);
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

    public static SequenceAction Sequence( params Action[] actions )
    {
        SequenceAction action = action( typeof(SequenceAction) );

        for ( int i = 0, n = actions.Length; i < n; i++ )
        {
            action.addAction( actions[ i ] );
        }

        return action;
    }

    public static SequenceAction sequence()
    {
        return Action( SequenceAction.class);
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

    public static ParallelAction parallel( params Action[] actions )
    {
        ParallelAction action = action( ParallelAction.class);

        for ( int i = 0, n = actions.length; i < n; i++ )
            action.addAction( actions[ i ] );

        return action;
    }

    public static ParallelAction parallel()
    {
        return Action( ParallelAction.class);
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
        RepeatAction action = Action( typeof(RepeatAction) );

        action.setCount( RepeatAction.Forever );
        action.setAction( repeatedAction );

        return action;
    }

    public static RunnableAction run( IRunnable runnable )
    {
        RunnableAction action = action( RunnableAction.class);
        action.setRunnable( runnable );

        return action;
    }

    public static LayoutAction layout( bool enabled )
    {
        LayoutAction action = action( LayoutAction.class);
        action.setLayoutEnabled( enabled );

        return action;
    }

    public static AfterAction after( Action action )
    {
        AfterAction afterAction = action( typeof(AfterAction) );
        afterAction.setAction( action );

        return afterAction;
    }

    public static AddListenerAction addListener( EventListener listener, bool capture )
    {
        AddListenerAction addAction = Action( typeof(AddListenerAction) );

        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static AddListenerAction addListener( IEventListener listener, bool capture, Actor targetActor )
    {
        AddListenerAction addAction = Action( typeof(AddListenerAction) );

        addAction.setTarget( targetActor );
        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static RemoveListenerAction removeListener( IEventListener listener, bool capture )
    {
        RemoveListenerAction addAction = Action( typeof(RemoveListenerAction) );

        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static RemoveListenerAction RemoveListener( IEventListener listener, bool capture, Actor targetActor )
    {
        RemoveListenerAction addAction = Action( typeof(RemoveListenerAction) );

        addAction.setTarget( targetActor );
        addAction.setListener( listener );
        addAction.setCapture( capture );

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
