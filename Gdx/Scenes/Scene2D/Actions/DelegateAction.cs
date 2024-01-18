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

public abstract class DelegateAction : Action
{
    public Action? Action { get; set; }

    public override Actor? Actor
    {
        get => base.Actor;
        set
        {
            if ( Action != null )
            {
                Action.Actor = value;
            }

            base.Actor = value;
        }
    }

    public override Actor? Target
    {
        get => base.Target;
        set
        {
            if ( Action != null )
            {
                Action.Target = value;
            }

            base.Target = value;
        }
    }

    protected abstract bool Delegate( float delta );

    /// <summary>
    ///     Updates the action based on time.
    ///     Typically this is called each frame by <see cref="Actor" />.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    ///     true if the action is done. This method may continue to be called after
    ///     the action is done.
    /// </returns>
    public override bool Act( float delta )
    {
        Pool< Action >? pool = Pool;

        // Ensure this action can't be returned to the pool inside the delegate action.
        Pool = null;

        try
        {
            return Delegate( delta );
        }
        finally
        {
            Pool = pool;
        }
    }

    public override void Restart() => Action?.Restart();

    public override void Reset()
    {
        base.Reset();
        Action = null;
    }

    public override string ToString() => base.ToString() + ( Action == null ? "" : $"({Action})" );
}
