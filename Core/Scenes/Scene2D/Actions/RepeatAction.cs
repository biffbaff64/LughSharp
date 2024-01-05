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

public class RepeatAction : DelegateAction
{
    public const int FOREVER = -1;

    public int  RepeatCount   { get; set; }
    public int  ExecutedCount { get; set; }
    public bool Finished      { get; set; }

    protected override bool Delegate( float delta )
    {
        if ( ExecutedCount == RepeatCount )
        {
            return true;
        }

        if ( Action == null )
        {
            return false;
        }

        if ( Action.Act( delta ) )
        {
            if ( Finished )
            {
                return true;
            }

            if ( RepeatCount > 0 )
            {
                ExecutedCount++;
            }

            if ( ExecutedCount == RepeatCount )
            {
                return true;
            }

            Action?.Restart();
        }

        return false;
    }

    /// <summary>
    ///     Causes the action to not repeat again.
    /// </summary>
    public void Finish() => Finished = true;

    public new void Restart()
    {
        base.Restart();

        ExecutedCount = 0;
        Finished      = false;
    }
}
