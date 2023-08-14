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

using LibGDXSharp.Maths;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class TemporalAction : Action
{
    public bool            Reverse       { get; set; }
    public float           Duration      { get; set; }
    public float           Time          { get; set; }
    public Interpolation? Interpolation { get; set; }

    private bool _began;
    private bool _complete;

    protected TemporalAction()
    {
    }

    protected TemporalAction( float duration )
    {
        this.Duration = duration;
    }

    protected TemporalAction( float duration, Interpolation? interpolation )
    {
        this.Duration      = duration;
        this.Interpolation = interpolation;
    }

    public override bool Act( float delta )
    {
        if ( _complete )
        {
            return true;
        }

        Pool< object >? pool = base.Pool;

        // Ensure this action can't be returned to the pool while executing.
        base.Pool = null;

        try
        {
            if ( !_began )
            {
                Begin();
                _began = true;
            }

            Time      += delta;
            _complete =  Time >= Duration;

            var percent = _complete ? 1 : Time / Duration;

            if ( this.Interpolation != null )
            {
                percent = this.Interpolation.Apply( percent );
            }
            
            Update( Reverse ? 1 - percent : percent );

            if ( _complete )
            {
                End();
            }

            return _complete;
        }
        finally
        {
            base.Pool = pool;
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
    public void Finish()
    {
        Time = Duration;
    }

    public override void Restart()
    {
        Time      = 0;
        _began    = false;
        _complete = false;
    }

    public override void Reset()
    {
        base.Reset();

        Reverse       = false;
        Interpolation = null;
    }

    /// <summary>
    /// Returns true after <see cref="Act(float)"/> has been called where time >= duration.
    /// </summary>
    public bool IsComplete() => _complete;
}