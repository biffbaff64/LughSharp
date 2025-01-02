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


namespace LughSharp.Lugh.Scenes.Scene2D.Actions;

[PublicAPI]
public class RepeatAction : DelegateAction
{
    public const int FOREVER = -1;

    public int  RepeatCount   { get; set; }
    public int  ExecutedCount { get; set; }
    public bool Finished      { get; set; }

    // ========================================================================
    
    protected override bool Delegate( float delta )
    {
        if ( ExecutedCount == RepeatCount )
        {
            return true;
        }

        if ( Action == null )
        {
            return false;
        }

        if ( Action.Act( delta ) )
        {
            if ( Finished )
            {
                return true;
            }

            if ( RepeatCount > 0 )
            {
                ExecutedCount++;
            }

            if ( ExecutedCount == RepeatCount )
            {
                return true;
            }

            Action?.Restart();
        }

        return false;
    }

    /// <summary>
    /// Causes the action to not repeat again.
    /// </summary>
    public void Finish()
    {
        Finished = true;
    }

    public override void Restart()
    {
        base.Restart();

        ExecutedCount = 0;
        Finished      = false;
    }
}
