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

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class AfterAction : DelegateAction
{
    private readonly List< Action > _waitForActions = new( 4 );

    public void SetTarget( Actor? target )
    {
        if ( target != null )
        {
            _waitForActions.AddAll( target.Actions );
        }

        base.Target = target;
    }

    public new void Restart()
    {
        base.Restart();

        _waitForActions.Clear();
    }

    protected override bool Delegate( float delta )
    {
        List< Action >? currentActions = Target?.Actions;

        if ( currentActions?.Count == 1 )
        {
            _waitForActions.Clear();
        }

        for ( var i = _waitForActions.Count - 1; i >= 0; i-- )
        {
            Action action = _waitForActions[ i ];

            if ( currentActions?.IndexOf( action ) == -1 )
            {
                _waitForActions.RemoveAt( i );
            }
        }

        return ( _waitForActions.Count <= 0 ) && Action!.Act( delta );
    }
}
