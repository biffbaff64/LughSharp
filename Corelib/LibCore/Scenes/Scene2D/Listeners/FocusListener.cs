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


namespace Corelib.LibCore.Scenes.Scene2D.Listeners;

/// <summary>
/// Fired when an actor gains or loses keyboard or scroll focus. Can be
/// cancelled to prevent losing or gaining focus.
/// </summary>
[PublicAPI]
public class FocusListener : IEventListener
{
    /// <summary>
    /// Try to handle the given event, if it is applicable.
    /// </summary>
    /// <returns>
    /// True if the event should be considered as handled by scene2d.
    /// </returns>
    public virtual bool Handle( Event e )
    {
        if ( e is not FocusEvent focusEvent )
        {
            return false;
        }

        if ( focusEvent.Type == FocusEvent.FeType.Keyboard )
        {
            KeyboardFocusChanged( focusEvent, e.TargetActor, focusEvent.Focused );
        }
        else if ( focusEvent.Type == FocusEvent.FeType.Scroll )
        {
            ScrollFocusChanged( focusEvent, e.TargetActor, focusEvent.Focused );
        }

        return false;
    }

    /// <param name="ev"></param>
    /// <param name="actor">
    /// The event target, which is the actor that emitted the focus event.
    /// </param>
    /// <param name="focused"></param>
    public virtual void KeyboardFocusChanged( FocusEvent ev, Actor? actor, bool focused )
    {
    }

    /// <param name="ev"></param>
    /// <param name="actor">
    /// The event target, which is the actor that emitted the focus event.
    /// </param>
    /// <param name="focused"></param>
    public virtual void ScrollFocusChanged( FocusEvent ev, Actor? actor, bool focused )
    {
    }

    // --------------------------------------------------------------------
    // --------------------------------------------------------------------

    /// <summary>
    /// </summary>
    [PublicAPI]
    public class FocusEvent : Event
    {
        public enum FeType
        {
            Keyboard,
            Scroll
        }

        public bool    Focused { get; set; }
        public FeType? Type    { get; set; }

        // --------------------------------------------------------------------

        /// <summary>
        /// The actor related to the event. When focus is lost, this is the new
        /// actor being focused, or null. When focus is gained, this is the
        /// previous actor that was focused, or null.
        /// </summary>
        public Actor? RelatedActor { get; set; }

        public override void Reset()
        {
            base.Reset();
            RelatedActor = null;
        }
    }
}
