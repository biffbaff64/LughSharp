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

using System.Text;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class ParallelAction : Action
{
    private readonly List< Action > _actions = new( 4 );
    private          bool           _complete;

    protected ParallelAction()
    {
    }

    public ParallelAction( Action action1 ) => AddAction( action1 );

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
        if ( _complete )
        {
            return true;
        }

        _complete = true;

        Pool< Action >? pool = Pool;

        // Ensure this action can't be returned to the pool while executing.
        Pool = null;

        try
        {
            for ( int i = 0, n = _actions.Count; ( i < n ) && ( Actor != null ); i++ )
            {
                Action currentAction = _actions[ i ];

                if ( ( currentAction.Actor != null ) && !currentAction.Act( delta ) )
                {
                    _complete = false;
                }

                if ( Actor == null )
                {
                    return true; // This action was removed.
                }
            }

            return _complete;
        }
        finally
        {
            Pool = pool;
        }
    }

    public new void Restart()
    {
        _complete = false;

        for ( int i = 0, n = _actions.Count; i < n; i++ )
        {
            _actions[ i ].Restart();
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

        if ( Actor != null )
        {
            action.Actor = Actor;
        }
    }

    public void SetActor( Actor actor )
    {
        for ( int i = 0, n = _actions.Count; i < n; i++ )
        {
            _actions[ i ].Actor = actor;
        }

        base.Actor = actor;
    }

    public List< Action > GetActions() => _actions;

    public new string ToString()
    {
        var buffer = new StringBuilder( 64 );

        buffer.Append( base.ToString() );
        buffer.Append( '(' );

        for ( int i = 0, n = _actions.Count; i < n; i++ )
        {
            if ( i > 0 )
            {
                buffer.Append( ", " );
            }

            buffer.Append( _actions[ i ] );
        }

        buffer.Append( ')' );

        return buffer.ToString();
    }
}
