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
/// An action that runs a <see cref="Runnable"/>. Alternatively, the <see cref="Run()"/>
/// method can be overridden instead of setting a runnable.
/// </summary>
public class RunnableAction : Action
{
    public Task? Runnable { get; set; } = null!;

    private bool _ran = false;

    public override bool Act( float delta )
    {
        if ( !_ran )
        {
            _ran = true;

            Run();
        }

        return true;
    }

    /// <summary>
    ///     Called to run the runnable.
    /// </summary>
    public virtual void Run()
    {
        if ( Runnable == null )
        {
            throw new GdxRuntimeException( "Runnable is not initialised!" );
        }

        Pool< Action >? pool = base.Pool;

        // Ensure this action can't be returned to the pool inside the runnable.
        Pool = null;

        try
        {
            Runnable.Start();
        }
        finally
        {
            base.Pool = pool;
        }
    }

    public override void Restart() => _ran = false;

    public override void Reset()
    {
        base.Reset();
        Runnable = null;
    }
}
