// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Scenes.Scene2D;

public interface IAction
{
    /// <summary>
    /// </summary>
    Pool< Action >? Pool { get; set; }

    /// <summary>
    ///     The actor this action targets, or null if a target has not been set.
    /// </summary>
    Actor? Target { get; set; }

    /// <summary>
    ///     The <see cref="Actor" /> this Action is attached to.
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
    ///     Resets the optional state of this action to as if it were newly created, allowing the
    ///     action to be pooled and reused. State required to be set for every usage of this action
    ///     or computed during the action does not need to be reset.
    ///     <para>
    ///         The default implementation calls <see cref="Action.Restart" />
    ///     </para>
    ///     <para>
    ///         If a subclass has optional state, it must override this method, call super, and reset
    ///         the optional state.
    ///     </para>
    /// </summary>
    void Reset();

    /// <summary>
    ///     Updates the action based on time.
    ///     Typically this is called each frame by <see cref="Action.Actor" />.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    ///     true if the action is done. This method may continue to be called after
    ///     the action is done.
    /// </returns>
    bool Act( float delta );

    /// <summary>
    ///     Sets the state of the action so it can be run again.
    /// </summary>
    void Restart();
}
