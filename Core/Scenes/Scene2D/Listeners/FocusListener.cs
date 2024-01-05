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

using LibGDXSharp.Scenes.Scene2D;

namespace LibGDXSharp.Scenes.Listeners;

/// <summary>
///     Fired when an actor gains or loses keyboard or scroll focus. Can be
///     cancelled to prevent losing or gaining focus.
/// </summary>
public class FocusListener : IEventListener
{

    /// <summary>
    ///     Try to handle the given event, if it is applicable.
    /// </summary>
    /// <returns>
    ///     True if the event should be considered as handled by scene2d.
    /// </returns>
    public virtual bool Handle( Event e )
    {
        if ( !( e is FocusEvent focusEvent ) )
        {
            return false;
        }

        switch ( focusEvent.Type )
        {
            case FocusEvent.FeType.Keyboard:
                KeyboardFocusChanged( focusEvent, e.TargetActor, focusEvent.Focused );

                break;

            case FocusEvent.FeType.Scroll:
                ScrollFocusChanged( focusEvent, e.TargetActor, focusEvent.Focused );

                break;
        }

        return false;
    }

    /// <param name="ev"></param>
    /// <param name="actor">
    ///     The event target, which is the actor that emitted the focus event.
    /// </param>
    /// <param name="focused"></param>
    public virtual void KeyboardFocusChanged( FocusEvent ev, Actor? actor, bool focused )
    {
    }

    /// <param name="ev"></param>
    /// <param name="actor">
    ///     The event target, which is the actor that emitted the focus event.
    /// </param>
    /// <param name="focused"></param>
    public virtual void ScrollFocusChanged( FocusEvent ev, Actor? actor, bool focused )
    {
    }

    public class FocusEvent : Event
    {

        public enum FeType
        {
            Keyboard,
            Scroll
        }

        public bool    Focused { get; set; }
        public FeType? Type    { get; set; }

        /// <summary>
        ///     The actor related to the event. When focus is lost, this is the new
        ///     actor being focused, or null. When focus is gained, this is the
        ///     previous actor that was focused, or null.
        /// </summary>
        public Actor? RelatedActor { get; set; }

        public new void Reset()
        {
            base.Reset();
            RelatedActor = null;
        }
    }
}
