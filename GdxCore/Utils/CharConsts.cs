namespace LibGDXSharp.Utils
{
    public class CharConsts
    {
        /// <summary>
        /// The maximum value of a Unicode high-surrogate code unit in
        /// the UTF-16 encoding, constant '\uDBFF'.
        /// A high-surrogate is also known as a leading-surrogate.
        /// </summary>
        public const char Max_High_Surrogate  = '\uDBFF';
        
        /// <summary>
        /// The minimum value of a Unicode high-surrogate code unit in
        /// the UTF-16 encoding, constant '\uD800'.
        /// A high-surrogate is also known as a leading-surrogate.
        /// </summary>
        public const char Min_High_Surrogate = '\uD800';

        /// <summary>
        /// The maximum value of a Unicode low-surrogate code unit in
        /// the UTF-16 encoding, constant '\uDFFF'.
        /// A low-surrogate is also known as a trailing-surrogate.
        /// </summary>
        public const char Max_Low_Surrogate = '\uDFFF';

        /// <summary>
        /// The minimum value of a Unicode low-surrogate code unit in
        /// the UTF-16 encoding, constant '\uDC00'.
        /// A low-surrogate is also known as a trailing-surrogate.
        /// </summary>
        public const char Min_Low_Surrogate = '\uDC00';
    }
}
