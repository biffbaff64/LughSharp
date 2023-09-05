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

using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D;

/// <summary>
/// The base class for all events.
/// By default an event will "bubble" up through an actor's parent's handlers
/// (see <see cref="Bubbles"/>).
/// <p>
/// An actor's capture listeners can stop() an event to prevent child actors
/// from seeing it.
/// </p>
/// <p>
/// An Event may be marked as "handled" which will end its propagation outside
/// of the Stage (see <see cref="IsHandled"/>). The default Actor.fire(Event)
/// will mark events handled if an EventListener returns true.
/// A cancelled event will be stopped and handled. Additionally, many actors
/// will undo the side-effects of a canceled event. (See <see cref="IsCancelled"/>)
/// </p>
/// </summary>
/// <remarks>
/// The original getters and setters for 'cancelled', 'handled', 'bubbles',
/// 'stopped', 'stage', 'targetActor', 'listenerActor', and 'capture' have
/// been removed and replaced with C# Properties.
/// Therefore, Event.isHandled() is now Event.IsHandled etc.
/// </remarks>
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

    // true means propagation was stopped and any action that this event would cause should not happen
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Marks this event as handled. This does not affect event propagation inside
    /// scene2d, but causes the <see cref="Stage"/> <see cref="IInputProcessor"/>
    /// methods to return true, which will consume the event so it is not passed
    /// on to the application under the stage. 
    /// </summary>
    public void Handle()
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

    public void Reset()
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
}