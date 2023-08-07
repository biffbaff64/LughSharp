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

using LibGDXSharp.Utils;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
public abstract class DelegateAction : Action
{
    public Action? Action { get; set; }

    protected abstract bool Delegate( float delta );

    /// <summary>
    /// Updates the action based on time.
    /// Typically this is called each frame by <see cref="Actor"/>.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    /// true if the action is done. This method may continue to be called after
    /// the action is done.
    /// </returns>
    public override bool Act( float delta )
    {
        Pool< object >? pool = base.Pool;
        
        // Ensure this action can't be returned to the pool inside the delegate action.
        base.Pool = null;

        try
        {
            return Delegate( delta );
        }
        finally
        {
            base.Pool = pool;
        }
    }

    public new void Restart()
    {
        this.Action?.Restart();
    }

    public new void Reset()
    {
        base.Reset();
        this.Action = null;
    }

    public new Actor? Actor
    {
        get => base.Actor;
        set
        {
            if ( this.Action != null )
            {
                this.Action.Actor = value;
            }

            base.Actor = value;
        }
    }

    public new Actor? Target
    {
        get => base.Target;
        set
        {
            if ( this.Action != null )
            {
                this.Action.Target = value;
            }

            base.Target = value;
        }
    }

    public override string ToString()
    {
        return base.ToString() + ( this.Action == null ? "" : $"({this.Action})" );
    }
}
