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
    public static MoveToAction MoveTo( float x, float y, float duration = 0,
                                       Interpolation? interpolation = null )
    {
        var action = Action< MoveToAction >();

        action.SetPosition( x, y );
        action.Duration = duration;
        action.Interpolation = interpolation;

        return action;
    }

    public static MoveToAction MoveToAligned( float x, float y, int alignment,
                                              float duration = 0,
                                              Interpolation? interpolation = null )
    {
        MoveToAction action = Action< MoveToAction >();

        action.SetPosition( x, y, alignment );
        action.Duration = duration;
        action.Interpolation = interpolation;

        return action;
    }

    /// <summary>
    /// Moves the actor instantly.
    /// </summary>
    public static MoveByAction MoveBy( float amountX, float amountY,
                                       float duration = 0,
                                       Interpolation? interpolation = null )
    {
        MoveByAction action = Action< MoveByAction >();

        action.setAmount( amountX, amountY );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /// <summary>
    /// Sizes the actor instantly.
    /// </summary>
    public static SizeToAction SizeTo( float x, float y,
                                       float duration = 0,
                                       Interpolation? interpolation = null )
    {
        SizeToAction action = Action< SizeToAction >();

        action.setSize( x, y );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /// <summary>
    /// Sizes the actor instantly.
    /// </summary>
    public static SizeByAction SizeBy( float amountX, float amountY,
                                       float duration = 0,
                                       Interpolation? interpolation = null )
    {
        SizeByAction action = Action< SizeByAction >();
        
        action.setAmount( amountX, amountY );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Scales the actor instantly. */
    public static ScaleToAction ScaleTo( float x, float y, float duration = 0, Interpolation? interpolation = null )
    {
        ScaleToAction action = action( ScaleToAction.class);

        action.setScale( x, y );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Scales the actor instantly. */
    public static ScaleByAction ScaleBy( float amountX, float amountY, float duration = 0, Interpolation? interpolation = null )
    {
        ScaleByAction action = action( ScaleByAction.class);

        action.setAmount( amountX, amountY );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Rotates the actor instantly. */
    public static RotateToAction RotateTo( float rotation, float duration = 0, Interpolation? interpolation = null )
    {
        RotateToAction action = action( RotateToAction.class);

        action.setRotation( rotation );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Rotates the actor instantly. */
    public static RotateByAction RotateBy( float rotationAmount, float duration = 0, Interpolation? interpolation = null )
    {
        RotateByAction action = action( RotateByAction.class);

        action.setAmount( rotationAmount );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /// Sets the actor's color instantly.
    /// Transitions from the color at the time this action starts to the specified color.
    public static ColorAction Color( Color color, float duration = 0, Interpolation? interpolation = null )
    {
        ColorAction action = action( ColorAction.class);

        action.setEndColor( color );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Sets the actor's alpha instantly. */
    /** Transitions from the alpha at the time this action starts to the specified alpha. */
    public static AlphaAction Alpha( float a, float duration = 0, Interpolation? interpolation = null )
    {
        AlphaAction action = action( AlphaAction.class);

        action.setAlpha( a );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 0. */
    public static AlphaAction FadeOut( float duration )
    {
        return Alpha( 0, duration, null );
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 0. */
    public static AlphaAction FadeOut( float duration, Interpolation? interpolation )
    {
        AlphaAction action = action( AlphaAction.class);
        
        action.setAlpha( 0 );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 1. */
    public static AlphaAction FadeIn( float duration )
    {
        return Alpha( 1, duration, null );
    }

    /** Transitions from the alpha at the time this action starts to an alpha of 1. */
    public static AlphaAction FadeIn( float duration, Interpolation? interpolation )
    {
        AlphaAction action = action( AlphaAction.class);
        
        action.setAlpha( 1 );
        action.setDuration( duration );
        action.setInterpolation( interpolation );

        return action;
    }

    public static VisibleAction Show() => Visible( true );

    public static VisibleAction Hide() => Visible( false );

    public static VisibleAction Visible( bool visible )
    {
        VisibleAction action = action( VisibleAction.class);
        action.setVisible( visible );

        return action;
    }

    public static TouchableAction Touchable( Touchable touchable )
    {
        TouchableAction action = action( TouchableAction.class);
        action.setTouchable( touchable );

        return action;
    }

    public static RemoveActorAction RemoveActor()
    {
        return Action( RemoveActorAction.class);
    }

    public static RemoveActorAction RemoveActor( Actor removeActor )
    {
        RemoveActorAction action = action( RemoveActorAction.class);
        action.setTarget( removeActor );

        return action;
    }

    public static DelayAction Delay( float duration )
    {
        DelayAction action = Action< DelayAction >();

        action.setDuration( duration );

        return action;
    }

    public static DelayAction Delay( float duration, Action delayedAction )
    {
        DelayAction action = Action< DelayAction >();
        
        action.setDuration( duration );
        action.setAction( delayedAction );

        return action;
    }

    public static TimeScaleAction TimeScale( float scale, Action scaledAction )
    {
        TimeScaleAction action = Action< TimeScaleAction >();

        action.setScale( scale );
        action.setAction( scaledAction );

        return action;
    }

    public static SequenceAction Sequence( Action action1 )
    {
        SequenceAction action = Action< SequenceAction >();

        action.addAction( action1 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2 )
    {
        SequenceAction action = Action< SequenceAction >();

        action.addAction( action1 );
        action.addAction( action2 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3 )
    {
        SequenceAction action = Action< SequenceAction >();

        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3, Action action4 )
    {
        SequenceAction action = Action< SequenceAction >();

        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );

        return action;
    }

    public static SequenceAction Sequence( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        SequenceAction action = Action< SequenceAction >();
        
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );
        action.addAction( action5 );

        return action;
    }

    public static SequenceAction Sequence( params Action[] actions )
    {
        SequenceAction action = Action< SequenceAction >();

        for ( int i = 0, n = actions.Length; i < n; i++ )
        {
            action.addAction( actions[ i ] );
        }

        return action;
    }

    public static SequenceAction Sequence()
    {
        SequenceAction action = Action< SequenceAction >();
    }

    public static ParallelAction Parallel( Action action1 )
    {
        ParallelAction action = Action< ParallelAction >();
        action.addAction( action1 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2 )
    {
        ParallelAction action = Action< ParallelAction >();
        action.addAction( action1 );
        action.addAction( action2 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3 )
    {
        ParallelAction action = Action< ParallelAction >();
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3, Action action4 )
    {
        ParallelAction action = Action< ParallelAction >();
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );

        return action;
    }

    public static ParallelAction Parallel( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        ParallelAction action = Action< ParallelAction >();
        
        action.addAction( action1 );
        action.addAction( action2 );
        action.addAction( action3 );
        action.addAction( action4 );
        action.addAction( action5 );

        return action;
    }

    public static ParallelAction Parallel( params Action[] actions )
    {
        ParallelAction action = Action< ParallelAction >();

        for ( int i = 0, n = actions.length; i < n; i++ )
            action.addAction( actions[ i ] );

        return action;
    }

    public static ParallelAction Parallel()
    {
        return Action<ParallelAction>();
    }

    public static RepeatAction Repeat( int count, Action repeatedAction )
    {
        RepeatAction action = action( RepeatAction.class);
        action.setCount( count );
        action.setAction( repeatedAction );

        return action;
    }

    public static RepeatAction Forever( Action repeatedAction )
    {
        RepeatAction action = Action<RepeatAction>();

        action.setCount( RepeatAction.Forever );
        action.setAction( repeatedAction );

        return action;
    }

    public static RunnableAction Run( IRunnable runnable )
    {
        RunnableAction action = Action< RunnableAction >();
        action.setRunnable( runnable );

        return action;
    }

    public static LayoutAction Layout( bool enabled )
    {
        LayoutAction action = Action<LayoutAction>();
        action.setLayoutEnabled( enabled );

        return action;
    }

    public static AfterAction After( Action action )
    {
        AfterAction afterAction = Action<AfterAction>();
        afterAction.setAction( action );

        return afterAction;
    }

    public static AddListenerAction AddListener( EventListener listener, bool capture )
    {
        AddListenerAction addAction = Action<AddListenerAction>();

        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static AddListenerAction AddListener( IEventListener listener, bool capture, Actor targetActor )
    {
        AddListenerAction addAction = Action<AddListenerAction>();

        addAction.setTarget( targetActor );
        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static RemoveListenerAction RemoveListener( IEventListener listener, bool capture )
    {
        RemoveListenerAction addAction = Action<RemoveListenerAction>();

        addAction.setListener( listener );
        addAction.setCapture( capture );

        return addAction;
    }

    public static RemoveListenerAction RemoveListener( IEventListener listener, bool capture, Actor targetActor )
    {
        RemoveListenerAction addAction = Action<RemoveListenerAction>();

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
