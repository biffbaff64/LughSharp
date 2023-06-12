// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils;

public static class TimeUtils
{
    private const long NanosPerMilli = 1000000;

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
        return nanos / NanosPerMilli;
    }

    /// <summary>
    /// Converts the CURRENT time in nanoseconds to milliseconds.
    /// </summary>
    /// <returns></returns>
    public static long NanosToMillis()
    {
        return NanoTime() / NanosPerMilli;
    }
        
    /// <summary>
    /// Convert milliseconds time to nanoseconds
    /// </summary>
    /// <param name="millis">Must be in milliseconds.</param>
    /// <returns></returns>
    public static long MillisToNanos( long millis )
    {
        return millis * NanosPerMilli;
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