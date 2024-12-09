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

using Corelib.Lugh.Utils.Pooling;

namespace Corelib.Lugh.Scenes.Scene2D;

[PublicAPI]
public interface IAction
{
    /// <summary>
    /// The pool of actions.
    /// </summary>
    Pool< Action >? Pool { get; set; }

    /// <summary>
    /// The actor this action targets, or null if a target has not been set.
    /// </summary>
    Actor? Target { get; set; }

    /// <summary>
    /// The <see cref="Actor"/> this Action is attached to.
    /// </summary>
    Actor? Actor
    {
        // Returns null if the action is not attached to an actor.
        get;

        // Sets the actor this action is attached to. This also sets the target actor if it
        // is null. This method is called automatically when an action is added to an actor.
        // This method is also called with null when an action is removed from an actor.
        // When set to null, if the action has a pool then the action is returned to the pool
        // (which calls reset()) and the pool is set to null. If the action does not have a
        // pool, reset() is not called. This method is not typically a good place for an action
        // subclass to query the actor's state because the action may not be executed for some
        // time, eg it may be delayed. The actor's state is best queried in the first call to
        // Act(float). For a TemporalAction, use TemporalAction#begin().
        set;
    }

    /// <summary>
    /// Resets the optional state of this action as if it were newly created, allowing the
    /// action to be pooled and reused. State required to be set for every usage of this action
    /// or computed during the action does not need to be reset.
    /// <para>
    /// The default implementation should call <see cref="Action.Restart"/>
    /// </para>
    /// <para>
    /// If a subclass has optional state, it must override this method, call super, and reset
    /// the optional state.
    /// </para>
    /// </summary>
    void Reset();

    /// <summary>
    /// Updates the action based on time.
    /// Typically this is called each frame by <see cref="Action.Actor"/>.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    /// true if the action is done. This method may continue to be called after
    /// the action is done.
    /// </returns>
    bool Act( float delta );

    /// <summary>
    /// Sets the state of the action so it can be run again.
    /// Default implementation does nothing.
    /// </summary>
    void Restart();
}
