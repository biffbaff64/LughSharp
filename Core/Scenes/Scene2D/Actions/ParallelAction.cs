using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class ParallelAction : Action
{
    private readonly List< Action > _actions = new( 4 );
    private bool _complete;

    protected ParallelAction()
    {
    }

    public ParallelAction( Action action1 )
    {
        AddAction( action1 );
    }

    public ParallelAction( Action action1, Action action2 )
    {
        AddAction( action1 );
        AddAction( action2 );
    }

    public ParallelAction( Action action1, Action action2, Action action3 )
    {
        AddAction( action1 );
        AddAction( action2 );
        AddAction( action3 );
    }

    public ParallelAction( Action action1, Action action2, Action action3, Action action4 )
    {
        AddAction( action1 );
        AddAction( action2 );
        AddAction( action3 );
        AddAction( action4 );
    }

    public ParallelAction( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        AddAction( action1 );
        AddAction( action2 );
        AddAction( action3 );
        AddAction( action4 );
        AddAction( action5 );
    }

    public override bool Act( float delta )
    {
        if ( _complete ) return true;

        _complete = true;
        
        Pool<object>? pool = base.Pool;
        
        // Ensure this action can't be returned to the pool while executing.
        base.Pool = null;

        try
        {
            for ( int i = 0, n = this._actions.Count; ( i < n ) && ( Actor != null ); i++ )
            {
                Action currentAction = this._actions[ i ];

                if ( ( currentAction.Actor != null ) && !currentAction.Act( delta ) )
                {
                    _complete = false;
                }

                if ( Actor == null ) return true; // This action was removed.
            }

            return _complete;
        }
        finally
        {
            base.Pool = pool;
        }
    }

    public new void Restart()
    {
        _complete = false;

        for ( int i = 0, n = this._actions.Count; i < n; i++ )
        {
            this._actions[ i ].Restart();
        }
    }

    public new void Reset()
    {
        base.Reset();
        _actions.Clear();
    }

    public void AddAction( Action action )
    {
        _actions.Add( action );
        
        if ( Actor != null ) action.Actor = Actor;
    }

    public void SetActor( Actor actor )
    {
        for ( int i = 0, n = this._actions.Count; i < n; i++ )
        {
            this._actions[ i ].Actor = actor;
        }

        base.Actor = actor;
    }

    public List< Action > GetActions()
    {
        return _actions;
    }

    public new string ToString()
    {
        var buffer = new StringBuilder( 64 );
        
        buffer.Append( base.ToString() );
        buffer.Append( '(' );
        
        for ( int i = 0, n = this._actions.Count; i < n; i++ )
        {
            if ( i > 0 ) buffer.Append( ", " );
            buffer.Append( this._actions[ i ] );
        }

        buffer.Append( ')' );

        return buffer.ToString();
    }
}