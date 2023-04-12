namespace LibGDXSharp.Maths
{
    public class FloatCounter
    {
        /// <summary>
        /// The amount of values added
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// The sum of all values
        /// </summary>
        public float Total { get; set; }
        /// <summary>
        /// The smallest value
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// The largest value
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// The average value (total / count)
        /// </summary>
        public float Average { get; set; }
        /// <summary>
        /// The latest raw value
        /// </summary>
        public float Latest { get; set; }
        /// <summary>
        /// The current windowed mean value
        /// </summary>
        public float Value { get; set; }
        /// <summary>
        /// Provides access to the WindowedMean if any (can be null)
        /// </summary>
        public WindowedMean Mean { get; set; }

        public FloatCounter( in int windowSize )
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Put( float current )
        {
            throw new NotImplementedException();
        }
    }
}

