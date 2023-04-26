namespace LibGDXSharp.Utils
{
    /// <summary>
    /// A typesafe enumeration for byte orders.
    /// </summary>
    public sealed class ByteOrder
    {
        private readonly string _name;

        private ByteOrder( string name )
        {
            this._name = name;
        }

        /// <summary>
        /// Constant denoting big-endian byte order.  In this order, the bytes of a
        /// multibyte value are ordered from most significant to least significant.
        /// </summary>
        public readonly static ByteOrder BigEndian = new ByteOrder( "BigEndian" );

        /// <summary>
        /// Constant denoting little-endian byte order.  In this order, the bytes of
        /// a multibyte value are ordered from least significant to most
        /// significant.
        /// </summary>
        public readonly static ByteOrder LittleEndian = new ByteOrder( "LittleEndian" );

//        /// <summary>
//        /// Retrieves the native byte order of the underlying platform.
//        /// <para>
//        /// This method is defined so that performance-sensitive code can allocate direct
//        /// buffers with the same byte order as the hardware. Native code libraries are often
//        /// more efficient when such buffers are used.
//        /// </para>
//        /// </summary>
//        /// <returns>  The native byte order of the hardware upon which this Java
//        ///          virtual machine is running
//        /// </returns>
//        public static ByteOrder NativeOrder()
//        {
//            return Bits.byteOrder();
//        }

        /// <summary>
        /// Constructs a string describing this object.
        /// 
        /// <para>
        /// This method returns the string <tt>"BigEndian"</tt> for <see cref="BigEndian"/>
        /// and <tt>"LittleEndian"</tt> for <see cref="LittleEndian"/>.
        /// </para>
        /// </summary>
        /// <returns>The specified string</returns>
        public override string ToString()
        {
            return _name;
        }
    }
}
