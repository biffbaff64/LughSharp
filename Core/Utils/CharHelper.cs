// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public static class CharHelper
{
    /// <summary>
    /// The maximum value of a Unicode high-surrogate code unit in
    /// the UTF-16 encoding, constant '\uDBFF'.
    /// A high-surrogate is also known as a leading-surrogate.
    /// </summary>
    public const char Max_High_Surrogate = '\uDBFF';

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

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public const int Min_Supplementary_Code_Point = 0x010000;

    /// <summary>
    /// Determines the number of char values needed to represent the specified
    /// character (Unicode code point). If the specified character is equal to
    /// or greater than 0x10000, then the method returns 2. Otherwise, the
    /// method returns 1.
    /// <para>
    /// This method doesn't validate the specified character to be a valid
    /// Unicode code point. The caller must validate the character value using
    /// IsValidCodePoint if necessary.
    /// </para>
    /// </summary>
    /// <param name="c"></param>
    /// <param name="codePoint"></param>
    /// <returns></returns>
    public static int CharCount( int codePoint )
    {
        return codePoint >= 0x010000 ? 2 : 1;
    }

    /// <summary>
    /// Returns the code point at the given index in the supplied string.
    /// </summary>
    /// <param name="c"></param>
    /// <param name="str">The string.</param>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public static int CodePointAt( string str, int position )
    {
        return char.ConvertToUtf32( str, position );
    }
}

