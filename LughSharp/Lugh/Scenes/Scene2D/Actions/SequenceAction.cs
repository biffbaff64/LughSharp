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

using LughSharp.Lugh.Utils.Pooling;

namespace LughSharp.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// Executes a number of actions one at a time.
/// </summary>
[PublicAPI]
public class SequenceAction : ParallelAction
{
    private int _index;

    public SequenceAction()
    {
    }

    public SequenceAction( params Action[] actions )
    {
        foreach ( var action in actions )
        {
            AddAction( action );
        }
    }

    /// <inheritdoc />
    public override bool Act( float delta )
    {
        if ( _index >= GetActions().Count )
        {
            return true;
        }

        Pool< Action >? pool = Pool;

        // Ensure this action can't be returned to the pool while executings.
        Pool = null;

        try
        {
            if ( GetActions()[ _index ].Act( delta ) )
            {
                if ( Actor == null )
                {
                    return true; // This action was removed.
                }

                _index++;

                if ( _index >= GetActions().Count )
                {
                    return true;
                }
            }

            return false;
        }
        finally
        {
            Pool = pool;
        }
    }

    /// <inheritdoc />
    public override void Restart()
    {
        base.Restart();
        _index = 0;
    }
}
