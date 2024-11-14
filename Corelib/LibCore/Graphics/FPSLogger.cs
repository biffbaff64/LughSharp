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

using Corelib.LibCore.Utils;

namespace Corelib.LibCore.Graphics;

/// <summary>
/// A simple helper class to log the frames per seconds achieved. Just invoke the
/// Log() method in your rendering method. The output will be logged once per second.
/// </summary>
[PublicAPI]
public class FPSLogger
{
    private readonly int  _bound;
    private          long _lastLogTime;

    // ========================================================================
    
    /// <summary>
    /// Constructs a new FPSLogger instance.
    /// </summary>
    /// <param name="bound"></param>
    public FPSLogger( int bound = int.MaxValue )
    {
        this._bound       = bound;
        this._lastLogTime = TimeUtils.NanoTime();
    }

    /// <summary>
    /// Logs the current frames per second to the console.
    /// </summary>
    public void Log()
    {
        var currentTime = TimeUtils.NanoTime();
        var elapsedTime = TimeSpan.FromTicks( currentTime - _lastLogTime );

        // Log FPS if at least one second has passed
        if ( elapsedTime.TotalSeconds >= 1 )
        {
            var fps = Gdx.Graphics.GetFramesPerSecond();

            if ( fps <= _bound )
            {
                Logger.Debug( $"FPS: {fps}" );
            }

            _lastLogTime = currentTime;
        }
    }
}
