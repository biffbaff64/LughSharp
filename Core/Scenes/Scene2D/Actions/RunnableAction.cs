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

using System.Threading;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class RunnableAction : Action
{
    private bool _ran;

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
    /// Called to run the runnable.
    /// </summary>
    public void Run()
    {
        if ( Runnable == null ) throw new GdxRuntimeException( "Runnable is not initialised!" );
        
        Pool< object >? pool = base.Pool;

        // Ensure this action can't be returned to the pool inside the runnable.
        base.Pool = null;

        try
        {
            var thread = new Thread( Runnable! );
            thread.Start();
        }
        finally
        {
            base.Pool = pool;
        }
    }

    public new void Restart()
    {
        _ran = false;
    }

    public new void Reset()
    {
        base.Reset();
        Runnable = null;
    }

    public ThreadStart? Runnable { get; set; }
}