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

using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;
using Corelib.Lugh.Utils.Pooling;

namespace Corelib.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// An action that runs a <see cref="IRunnable.Runnable"/>. Alternatively, the <see cref="Run()"/>
/// method can be overridden instead of setting a runnable.
/// </summary>
[PublicAPI]
public class RunnableAction : Action
{
    private bool               _ran = false;
    public  IRunnable.Runnable RunnableTask { get; set; } = null!;

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
    public virtual void Run()
    {
        if ( RunnableTask == null )
        {
            throw new GdxRuntimeException( "Runnable is not initialised!" );
        }

        Pool< Action >? pool = Pool;

        // Ensure this action can't be returned to the pool inside the runnable.
        Pool = null;

        try
        {
            RunnableTask();
        }
        finally
        {
            Pool = pool;
        }
    }

    public override void Restart()
    {
        _ran = false;
    }

    public override void Reset()
    {
        base.Reset();
        RunnableTask = null!;
    }
}
