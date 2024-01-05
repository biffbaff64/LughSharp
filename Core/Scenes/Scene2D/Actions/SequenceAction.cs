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

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
///     Executes a number of actions one at a time.
///     @author Nathan Sweet
/// </summary>
public class SequenceAction : ParallelAction
{
    private int _index;

    public SequenceAction()
    {
    }

    public SequenceAction( Action action1 ) => AddAction( action1 );

    public SequenceAction( Action action1, Action action2 )
    {
        AddAction( action1 );
        AddAction( action2 );
    }

    public SequenceAction( Action action1, Action action2, Action action3 )
    {
        AddAction( action1 );
        AddAction( action2 );
        AddAction( action3 );
    }

    public SequenceAction( Action action1, Action action2, Action action3, Action action4 )
    {
        AddAction( action1 );
        AddAction( action2 );
        AddAction( action3 );
        AddAction( action4 );
    }

    public SequenceAction( Action action1, Action action2, Action action3, Action action4, Action action5 )
    {
        AddAction( action1 );
        AddAction( action2 );
        AddAction( action3 );
        AddAction( action4 );
        AddAction( action5 );
    }

    public override bool Act( float delta )
    {
        if ( _index >= GetActions().Count )
        {
            return true;
        }

        Pool< Action >? pool = Pool;

        // Ensure this action can't be returned to the pool while executings.
        Pool = null;

        try
        {
            if ( GetActions()[ _index ].Act( delta ) )
            {
                if ( Actor == null )
                {
                    return true; // This action was removed.
                }

                _index++;

                if ( _index >= GetActions().Count )
                {
                    return true;
                }
            }

            return false;
        }
        finally
        {
            Pool = pool;
        }
    }

    public new void Restart()
    {
        base.Restart();
        _index = 0;
    }
}
