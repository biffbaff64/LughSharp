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

public class DelayAction : DelegateAction
{
    public float Duration { get; set; }
    public float Time     { get; set; }

    public DelayAction()
    {
    }

    public DelayAction( float duration )
    {
        this.Duration = duration;
    }

    protected override bool Delegate( float delta )
    {
        if ( Time < Duration )
        {
            Time += delta;

            if ( Time < Duration ) return false;
            
            delta = Time - Duration;
        }

        return ( Action == null ) || Action.Act( delta );
    }

    public void Finish()
    {
        Time = Duration;
    }

    public new void Restart()
    {
        base.Restart();
        
        Time = 0;
    }
}
