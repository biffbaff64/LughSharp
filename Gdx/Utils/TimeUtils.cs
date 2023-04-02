namespace LibGDXSharp.Utils
{
    public class TimeUtils
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
}
