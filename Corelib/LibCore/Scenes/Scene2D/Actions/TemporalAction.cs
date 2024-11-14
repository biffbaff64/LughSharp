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

using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D.Actions;

/// <summary>
/// Base class for actions that transition over time using percent complete.
/// </summary>
[PublicAPI]
public abstract class TemporalAction : Action
{
    private bool _began;

    // ========================================================================

    protected TemporalAction()
    {
    }

    protected TemporalAction( float duration )
    {
        Duration = duration;
    }

    protected TemporalAction( float duration, IInterpolation? interpolation )
    {
        Duration      = duration;
        Interpolation = interpolation;
    }

    public bool            Reverse       { get; set; }
    public float           Duration      { get; set; }
    public float           Time          { get; set; }
    public IInterpolation? Interpolation { get; set; }

    /// <summary>
    /// Returns true after <see cref="Act(float)"/> has been called where time >= duration.
    /// </summary>
    public bool IsComplete { get; private set; }

    /// <inheritdoc />
    public override bool Act( float delta )
    {
        if ( IsComplete )
        {
            return true;
        }

        Pool< Action >? pool = Pool;

        // Ensure this action can't be returned to the pool while executing.
        Pool = null;

        try
        {
            if ( !_began )
            {
                Begin();
                _began = true;
            }

            Time       += delta;
            IsComplete =  Time >= Duration;

            var percent = IsComplete ? 1 : Time / Duration;

            if ( Interpolation != null )
            {
                percent = Interpolation.Apply( percent );
            }

            Update( Reverse ? 1 - percent : percent );

            if ( IsComplete )
            {
                End();
            }

            return IsComplete;
        }
        finally
        {
            Pool = pool;
        }
    }

    /// <summary>
    /// Called the first time <see cref="Act(float)"/> is called. This is a good place
    /// to query the <see cref="Actor"/>'s starting state.
    /// </summary>
    protected virtual void Begin()
    {
    }

    /// <summary>
    /// Called the last time <see cref="Act(float)"/> is called.
    /// </summary>
    protected virtual void End()
    {
    }

    /// <summary>
    /// Called each frame.
    /// </summary>
    /// <param name="percent">
    /// The percentage of completion for this action, growing from 0 to 1 over the
    /// duration. If <see cref="Reverse"/> is true, this will shrink from 1 to 0.
    /// </param>
    protected abstract void Update( float percent );

    /// <summary>
    /// Skips to the end of the transition.
    /// </summary>
    public virtual void Finish()
    {
        Time = Duration;
    }

    public override void Restart()
    {
        Time       = 0;
        _began     = false;
        IsComplete = false;
    }

    public override void Reset()
    {
        base.Reset();

        Reverse       = false;
        Interpolation = null;
    }
}
