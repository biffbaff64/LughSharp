// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.LibCore.Scenes.Scene2D.Listeners;

public class ChangeListener : IEventListener
{
    public ChangeListener()
    {
    }

    public ChangeListener( Action< object > action )
    {
    }

    public virtual bool Handle( Event ev )
    {
        if ( ev is not ChangeEvent changeEvent )
        {
            return false;
        }

        Changed( changeEvent, changeEvent.TargetActor );

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="actor">
    ///     The event target, which is the actor that emitted the change event.
    /// </param>
    public virtual void Changed( ChangeEvent ev, Actor? actor )
    {
    }

    /// <summary>
    ///     Fired when something in an actor has changed. This is a generic event, exactly
    ///     what changed in an actor will vary.
    /// </summary>
    public class ChangeEvent : Event
    {
    }
}
