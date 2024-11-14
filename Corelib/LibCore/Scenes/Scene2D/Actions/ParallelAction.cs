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

using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D.Actions;

[PublicAPI]
public class ParallelAction : Action
{
    private readonly List< Action > _actions = new( 4 );
    private          bool           _complete;

    // ========================================================================
    
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
                var currentAction = _actions[ i ];

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

    public override void Restart()
    {
        _complete = false;

        for ( int i = 0, n = _actions.Count; i < n; i++ )
        {
            _actions[ i ].Restart();
        }
    }

    public override void Reset()
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

    public List< Action > GetActions()
    {
        return _actions;
    }

    public override string ToString()
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
