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


namespace LughSharp.Lugh.Utils;

[PublicAPI]
public static class TimeUtils
{
    private const long NANOS_PER_MILLI = 1000000;

    /// <summary>
    /// The current value of the system timer, in nanoseconds.
    /// </summary>
    public static long NanoTime()
    {
        return DateTime.Now.Nanosecond;
    }

    /// <summary>
    /// The current value of the system timer, in milliseconds.
    /// </summary>
    public static long Millis()
    {
        return DateTime.Now.Millisecond;
    }

    /// <summary>
    /// Convert nanoseconds time to milliseconds
    /// </summary>
    /// <param name="nanos">Must be in nanoseconds.</param>
    /// <returns></returns>
    public static long NanosToMillis( long nanos )
    {
        return nanos / NANOS_PER_MILLI;
    }

    /// <summary>
    /// Converts the CURRENT time in nanoseconds to milliseconds.
    /// </summary>
    /// <returns></returns>
    public static long NanosToMillis()
    {
        return NanoTime() / NANOS_PER_MILLI;
    }

    /// <summary>
    /// Convert milliseconds time to nanoseconds
    /// </summary>
    /// <param name="millis">Must be in milliseconds.</param>
    /// <returns></returns>
    public static long MillisToNanos( long millis )
    {
        return millis * NANOS_PER_MILLI;
    }

    /// <summary>
    /// Get the time in nanos passed since a previous time
    /// </summary>
    /// <param name="prevTime"></param>
    /// <returns></returns>
    public static long TimeSinceNanos( long prevTime )
    {
        return NanoTime() - prevTime;
    }

    /// <summary>
    /// Get the time in millis passed since a previous time
    /// </summary>
    /// <param name="prevTime"></param>
    /// <returns></returns>
    public static long TimeSinceMillis( long prevTime )
    {
        return Millis() - prevTime;
    }
}
