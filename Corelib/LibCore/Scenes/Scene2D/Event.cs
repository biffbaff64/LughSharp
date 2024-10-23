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

using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D;

/// <summary>
/// The base class for all events.
/// By default an event will "bubble" up through an actor's parent's handlers
/// (see <see cref="Bubbles"/>).
/// <para>
/// An actor's capture listeners can stop() an event to prevent child actors
/// from seeing it.
/// </para>
/// <para>
/// An Event may be marked as "handled" which will end its propagation outside
/// of the Stage (see <see cref="IsHandled"/>). The default Actor.fire(Event)
/// will mark events handled if an EventListener returns true.
/// A cancelled event will be stopped and handled. Additionally, many actors
/// will undo the side-effects of a canceled event. (See <see cref="IsCancelled"/>)
/// </para>
/// </summary>
[PublicAPI]
public class Event : IPoolable
{
    // The Stage for the Actor the event was fired on.
    public Stage? Stage { get; set; } = null!;

    // The Actor this event originated from.
    public Actor? TargetActor { get; set; }

    // The Actor this listener is attached to.
    public Actor? ListenerActor { get; set; }

    // true means event occurred during the capture phase
    public bool Capture { get; set; }

    // If true, after the event is fired on the target actor, it will also
    // be fired on each of the parent actors, all the way to the root.
    public bool Bubbles { get; set; } = true;

    // true means the event was handled (the stage will eat the input)
    public bool IsHandled { get; set; }

    // true means event propagation was stopped
    public bool IsStopped { get; set; }

    // true means propagation was stopped and any action that this event
    // would cause should not happen
    public bool IsCancelled { get; set; }

    public virtual void Reset()
    {
        Stage         = null;
        TargetActor   = null;
        ListenerActor = null;
        Capture       = false;
        Bubbles       = true;
        IsHandled     = false;
        IsStopped     = false;
        IsCancelled   = false;
    }

    /// <summary>
    /// Marks this event as handled. This does not affect event propagation inside
    /// scene2d, but causes the <see cref="Stage"/> <see cref="IInputProcessor"/>
    /// methods to return true, which will consume the event so it is not passed
    /// on to the application under the stage.
    /// </summary>
    public virtual void SetHandled()
    {
        IsHandled = true;
    }

    /// <summary>
    /// Marks this event cancelled. This handles the event and stops the event
    /// propagation. It also cancels any default action that would have been taken
    /// by the code that fired the event.
    /// <para>
    /// Eg, if the event is for a checkbox being checked, cancelling
    /// the event could uncheck the checkbox.
    /// </para>
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
        IsStopped   = true;
        IsHandled   = true;
    }

    /// <summary>
    /// Marks this event as being stopped. This halts event propagation. Any other
    /// listeners on the <see cref="ListenerActor"/> are notified, but
    /// after that no other listeners are notified.
    /// </summary>
    public void Stop()
    {
        IsStopped = true;
    }
}
