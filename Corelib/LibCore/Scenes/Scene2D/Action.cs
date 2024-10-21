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

[PublicAPI]
public abstract class Action : IAction, IPoolable
{
    private Actor? _actor;

    /// <inheritdoc />
    public Pool< Action >? Pool { get; set; }

    /// <inheritdoc />
    public virtual Actor? Target { get; set; }

    /// <summary>
    /// The actor this action is attached to, or null if it is not attached.
    /// </summary>
    public virtual Actor? Actor
    {
        // Returns null if the action is not attached to an actor.
        get => _actor;

        // Sets the actor this action is attached to. This also sets the target actor if it
        // is null. This method is called automatically when an action is added to an actor.
        // This method is also called with null when an action is removed from an actor.
        // When set to null, if the action has a pool then the action is returned to the pool
        // (which calls reset()) and the pool is set to null. If the action does not have a
        // pool, reset() is not called. This method is not typically a good place for an action
        // subclass to query the actor's state because the action may not be executed for some
        // time, eg it may be delayed. The actor's state is best queried in the first call to
        // Act(float). For a TemporalAction, use TemporalAction#begin().
        set
        {
            _actor = value;

            Target ??= value;

            if ( value == null )
            {
                if ( Pool != null )
                {
                    Pool.Free( this );
                    Pool = null;
                }
            }
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <inheritdoc cref="IAction.Reset()"/>
    public virtual void Reset()
    {
        Actor  = null;
        Target = null;
        Pool   = null;

        Restart();
    }

    /// <inheritdoc />
    public abstract bool Act( float delta );

    /// <inheritdoc />
    public virtual void Restart()
    {
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var name     = GetType().Name;
        var dotIndex = name.LastIndexOf( '.' );

        if ( dotIndex != -1 )
        {
            // Note: equivalent to name.SubString(startIndex: dotIndex+1)
            name = name[ ( dotIndex + 1 ).. ];
        }

        if ( name.EndsWith( "Action" ) )
        {
            // Note: equivalent to name.Substring( 0, name.Length - 6 );
            name = name[ ..^6 ];
        }

        return name;
    }
}
