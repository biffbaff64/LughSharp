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


namespace Corelib.Lugh.Scenes.Scene2D.Actions;

[PublicAPI]
public class ScaleToAction : TemporalAction
{
    private float _startX;
    private float _startY;
    public  float EndX { get; set; }
    public  float EndY { get; set; }

    protected override void Begin()
    {
        _startX = Target!.ScaleX;
        _startY = Target.ScaleY;
    }

    protected override void Update( float percent )
    {
        float x, y;

        if ( percent == 0 )
        {
            x = _startX;
            y = _startY;
        }
        else if ( percent is 1.0f )
        {
            x = EndX;
            y = EndY;
        }
        else
        {
            x = _startX + ( ( EndX - _startX ) * percent );
            y = _startY + ( ( EndY - _startY ) * percent );
        }

        Target?.SetScale( x, y );
    }

    public void SetScale( float x, float y )
    {
        EndX = x;
        EndY = y;
    }

    public void SetScale( float scale )
    {
        EndX = scale;
        EndY = scale;
    }
}
