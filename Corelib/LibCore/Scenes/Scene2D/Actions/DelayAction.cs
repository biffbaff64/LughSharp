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


namespace Corelib.LibCore.Scenes.Scene2D.Actions;

[PublicAPI]
public class DelayAction : DelegateAction
{
    public DelayAction() : this( 0 )
    {
    }

    public DelayAction( float duration )
    {
        Duration = duration;
        Time     = 0;
    }

    public float Duration { get; set; }
    public float Time     { get; set; }

    protected override bool Delegate( float delta )
    {
        if ( Time < Duration )
        {
            Time += delta;

            if ( Time < Duration )
            {
                return false;
            }

            delta = Time - Duration;
        }

        return ( Action == null ) || Action.Act( delta );
    }

    public void Finish()
    {
        Time = Duration;
    }

    public override void Restart()
    {
        base.Restart();

        Time = 0;
    }
}
