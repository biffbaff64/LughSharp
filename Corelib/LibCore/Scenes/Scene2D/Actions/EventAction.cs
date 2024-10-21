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

using Corelib.LibCore.Scenes.Scene2D.Listeners;

namespace Corelib.LibCore.Scenes.Scene2D.Actions;

/// <summary>
/// Adds a listener to the actor for a specific event type and does not complete
/// until <see cref="Handle"/> returns true.
/// </summary>
[PublicAPI]
public abstract class EventAction< T > : Action, IEventListener where T : Event
{
    protected EventAction( T eventClass )
    {
        EventClass = eventClass;
    }

    public bool Active     { get; set; }
    public T    EventClass { get; set; }
    public bool Result     { get; set; }

    /// <summary>
    /// Called when the specific type of event occurs on the actor.
    /// </summary>
    /// <returns>
    /// true if the event should be considered handled by <see cref="Event.SetHandled"/>
    /// and this EventAction considered complete.
    /// </returns>
    public bool Handle( Event ev )
    {
        if ( !Active || ( ev.GetType() != EventClass.GetType() ) )
        {
            return false;
        }

        Result = HandleDelegate( ev );

        return Result;
    }

    public override void Restart()
    {
        Result = false;
        Active = false;
    }

    public void SetTarget( Actor target )
    {
        base.Target?.RemoveListener( this );
        base.Target = target;
        base.Target?.AddListener( this );
    }

    // ReSharper disable once MemberCanBeProtected.Global
    public abstract bool HandleDelegate( Event ev );

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
        Active = true;

        return Result;
    }
}
