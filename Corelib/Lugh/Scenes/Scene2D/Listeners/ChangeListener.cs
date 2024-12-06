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


namespace Corelib.Lugh.Scenes.Scene2D.Listeners;

/// <summary>
/// Listener for <see cref="ChangeEvent"/>s.
/// </summary>
[PublicAPI]
public class ChangeListener : IEventListener
{
//    public ChangeListener()
//    {
//    }

//    public ChangeListener( Action< object > action )
//    {
//    }

    public virtual bool Handle( Event ev )
    {
        if ( ev is ChangeEvent changeEvent )
        {
            Changed( changeEvent, changeEvent.TargetActor );
        }

        return false;
    }

    /// <summary>
    /// Handles any <see cref="ChangeEvent"/>s generated.
    /// </summary>
    /// <param name="ev"> The change event. </param>
    /// <param name="actor">
    /// The event target, which is the actor that emitted the change event.
    /// </param>
    public virtual void Changed( ChangeEvent ev, Actor? actor )
    {
    }

    /// <summary>
    /// Fired when something in an actor has changed. This is a generic event, exactly
    /// what changed in an actor will vary.
    /// </summary>
    public class ChangeEvent : Event
    {
    }
}
