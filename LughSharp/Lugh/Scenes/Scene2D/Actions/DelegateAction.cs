// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Utils.Pooling;

namespace LughSharp.Lugh.Scenes.Scene2D.Actions;

[PublicAPI]
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

    public override void Restart()
    {
        Action?.Restart();
    }

    public override void Reset()
    {
        base.Reset();
        Action = null;
    }

    public override string ToString()
    {
        return base.ToString() + ( Action == null ? "" : $"({Action})" );
    }
}
