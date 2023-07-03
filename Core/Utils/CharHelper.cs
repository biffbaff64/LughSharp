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
    /// <param name="codePoint"></param>
    /// <returns></returns>
    public static int CharCount( int codePoint )
    {
        return codePoint >= 0x010000 ? 2 : 1;
    }

    /// <summary>
    /// Returns the code point at the given index in the supplied string.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public static int CodePointAt( string str, int position )
    {
        return char.ConvertToUtf32( str, position );
    }
    
    /// <summary>
    /// The minimum radix available for conversion to and from strings.
    /// The constant value of this field is the smallest value permitted
    /// for the radix argument in radix-conversion methods such as the
    /// <tt>digit</tt> method, the <tt>forDigit</tt> method, and the
    /// <tt>toString</tt> method of class <tt>Integer</tt>.
    /// </summary>
    public const int Min_Radix = 2;

    /// <summary>
    /// The maximum radix available for conversion to and from strings.
    /// The constant value of this field is the largest value permitted
    /// for the radix argument in radix-conversion methods such as the
    /// <tt>digit</tt> method, the <tt>forDigit</tt> method, and the
    /// <tt>toString</tt> method of class <tt>Integer</tt>.
    /// </summary>
    public const int Max_Radix = 36;

    /// <summary>
    /// The constant value of this field is the smallest value of type
    /// <tt>char</tt>, <tt>'\u005Cu0000'</tt>.
    /// </summary>
    public const char Min_Value = '\u0000';

    /// <summary>
    /// The constant value of this field is the largest value of type
    /// <tt>char</tt>, <tt>'\u005CuFFFF'</tt>.
    /// </summary>
    public const char Max_Value = '\uFFFF';
    
	/// <summary>
	/// General category "Cn" in the Unicode specification.
	/// </summary>
	public const sbyte Unassigned = 0;

	/// <summary>
	/// General category "Lu" in the Unicode specification.
	/// </summary>
	public const sbyte UppercaseLetter = 1;

	/// <summary>
	/// General category "Ll" in the Unicode specification.
	/// </summary>
	public const sbyte LowercaseLetter = 2;

	/// <summary>
	/// General category "Lt" in the Unicode specification.
	/// </summary>
	public const sbyte TitlecaseLetter = 3;

	/// <summary>
	/// General category "Lm" in the Unicode specification.
	/// </summary>
	public const sbyte ModifierLetter = 4;

	/// <summary>
	/// General category "Lo" in the Unicode specification.
	/// </summary>
	public const sbyte OtherLetter = 5;

	/// <summary>
	/// General category "Mn" in the Unicode specification.
	/// </summary>
	public const sbyte NonSpacingMark = 6;

	/// <summary>
	/// General category "Me" in the Unicode specification.
	/// </summary>
	public const sbyte EnclosingMark = 7;

	/// <summary>
	/// General category "Mc" in the Unicode specification.
	/// </summary>
	public const sbyte CombiningSpacingMark = 8;

	/// <summary>
	/// General category "Nd" in the Unicode specification.
	/// </summary>
	public const sbyte DecimalDigitNumber = 9;

	/// <summary>
	/// General category "Nl" in the Unicode specification.
	/// </summary>
	public const sbyte LetterNumber = 10;

	/// <summary>
	/// General category "No" in the Unicode specification.
	/// </summary>
	public const sbyte OtherNumber = 11;

	/// <summary>
	/// General category "Zs" in the Unicode specification.
	/// </summary>
	public const sbyte SpaceSeparator = 12;

	/// <summary>
	/// General category "Zl" in the Unicode specification.
	/// </summary>
	public const sbyte LineSeparator = 13;

	/// <summary>
	/// General category "Zp" in the Unicode specification.
	/// </summary>
	public const sbyte ParagraphSeparator = 14;

	/// <summary>
	/// General category "Cc" in the Unicode specification.
	/// </summary>
	public const sbyte Control = 15;

	/// <summary>
	/// General category "Cf" in the Unicode specification.
	/// </summary>
	public const sbyte Format = 16;
	
	/// <summary>
	/// General category "Co" in the Unicode specification.
	/// </summary>
	public const sbyte PrivateUse = 18;

	/// <summary>
	/// General category "Cs" in the Unicode specification.
	/// </summary>
	public const sbyte Surrogate = 19;

	/// <summary>
	/// General category "Pd" in the Unicode specification.
	/// </summary>
	public const sbyte DashPunctuation = 20;

	/// <summary>
	/// General category "Ps" in the Unicode specification.
	/// </summary>
	public const sbyte StartPunctuation = 21;

	/// <summary>
	/// General category "Pe" in the Unicode specification.
	/// </summary>
	public const sbyte EndPunctuation = 22;

	/// <summary>
	/// General category "Pc" in the Unicode specification.
	/// </summary>
	public const sbyte ConnectorPunctuation = 23;

	/// <summary>
	/// General category "Po" in the Unicode specification.
	/// </summary>
	public const sbyte OtherPunctuation = 24;

	/// <summary>
	/// General category "Sm" in the Unicode specification.
	/// </summary>
	public const sbyte MathSymbol = 25;

	/// <summary>
	/// General category "Sc" in the Unicode specification.
	/// </summary>
	public const sbyte CurrencySymbol = 26;

	/// <summary>
	/// General category "Sk" in the Unicode specification.
	/// </summary>
	public const sbyte ModifierSymbol = 27;

	/// <summary>
	/// General category "So" in the Unicode specification.
	/// </summary>
	public const sbyte OtherSymbol = 28;

	/// <summary>
	/// General category "Pi" in the Unicode specification.
	/// </summary>
	public const sbyte InitialQuotePunctuation = 29;

	/// <summary>
	/// General category "Pf" in the Unicode specification.
	/// </summary>
	public const sbyte FinalQuotePunctuation = 30;

	/// <summary>
	/// Error flag. Use int (code point) to avoid confusion with U+FFFF.
	/// </summary>
	public const int Error = unchecked((int)0xFFFFFFFF);


	/// <summary>
	/// Undefined bidirectional character type. Undefined {@code char}
	/// values have undefined directionality in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityUndefined = -1;

	/// <summary>
	/// Strong bidirectional character type "L" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityLeftToRight = 0;
	
	/// <summary>
	/// Strong bidirectional character type "R" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityRightToLeft = 1;

	/// <summary>
	/// Strong bidirectional character type "AL" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityRightToLeftArabic = 2;

	/// <summary>
	/// Weak bidirectional character type "EN" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityEuropeanNumber = 3;

	/// <summary>
	/// Weak bidirectional character type "ES" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityEuropeanNumberSeparator = 4;

	/// <summary>
	/// Weak bidirectional character type "ET" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityEuropeanNumberTerminator = 5;

	/// <summary>
	/// Weak bidirectional character type "AN" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityArabicNumber = 6;

	/// <summary>
	/// Weak bidirectional character type "CS" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityCommonNumberSeparator = 7;

	/// <summary>
	/// Weak bidirectional character type "NSM" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityNonspacingMark = 8;

	/// <summary>
	/// Weak bidirectional character type "BN" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityBoundaryNeutral = 9;

	/// <summary>
	/// Neutral bidirectional character type "B" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityParagraphSeparator = 10;

	/// <summary>
	/// Neutral bidirectional character type "S" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalitySegmentSeparator = 11;

	/// <summary>
	/// Neutral bidirectional character type "WS" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityWhitespace = 12;

	/// <summary>
	/// Neutral bidirectional character type "ON" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityOtherNeutrals = 13;

	/// <summary>
	/// Strong bidirectional character type "LRE" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityLeftToRightEmbedding = 14;

	/// <summary>
	/// Strong bidirectional character type "LRO" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityLeftToRightOverride = 15;

	/// <summary>
	/// Strong bidirectional character type "RLE" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityRightToLeftEmbedding = 16;

	/// <summary>
	/// Strong bidirectional character type "RLO" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityRightToLeftOverride = 17;
	
	/// <summary>
	/// Weak bidirectional character type "PDF" in the Unicode specification.
	/// </summary>
	public const sbyte DirectionalityPopDirectionalFormat = 18;

	/// <summary>
	/// The minimum value of a
	/// <a href="http://www.unicode.org/glossary/#high_surrogate_code_unit">
	/// Unicode high-surrogate code unit</a>
	/// in the UTF-16 encoding, constant {@code '\u005CuD800'}.
	/// A high-surrogate is also known as a <i>leading-surrogate</i>.
	/// </summary>
	public const char MinHighSurrogate = '\uD800';

	/// <summary>
	/// The maximum value of a
	/// <a href="http://www.unicode.org/glossary/#high_surrogate_code_unit">
	/// Unicode high-surrogate code unit</a>
	/// in the UTF-16 encoding, constant {@code '\u005CuDBFF'}.
	/// A high-surrogate is also known as a <i>leading-surrogate</i>.
	/// </summary>
	public const char MaxHighSurrogate = '\uDBFF';

	/// <summary>
	/// The minimum value of a
	/// <a href="http://www.unicode.org/glossary/#low_surrogate_code_unit">
	/// Unicode low-surrogate code unit</a>
	/// in the UTF-16 encoding, constant {@code '\u005CuDC00'}.
	/// A low-surrogate is also known as a <i>trailing-surrogate</i>.
	/// </summary>
	public const char MinLowSurrogate = '\uDC00';

	/// <summary>
	/// The maximum value of a
	/// <a href="http://www.unicode.org/glossary/#low_surrogate_code_unit">
	/// Unicode low-surrogate code unit</a>
	/// in the UTF-16 encoding, constant {@code '\u005CuDFFF'}.
	/// A low-surrogate is also known as a <i>trailing-surrogate</i>.
	/// </summary>
	public const char MaxLowSurrogate = '\uDFFF';

	/// <summary>
	/// The minimum value of a Unicode surrogate code unit in the
	/// UTF-16 encoding, constant {@code '\u005CuD800'}.
	/// </summary>
	public const char MinSurrogate = MinHighSurrogate;

	/// <summary>
	/// The maximum value of a Unicode surrogate code unit in the
	/// UTF-16 encoding, constant {@code '\u005CuDFFF'}.
	/// </summary>
	public const char MaxSurrogate = MaxLowSurrogate;

	/// <summary>
	/// The minimum value of a
	/// <a href="http://www.unicode.org/glossary/#supplementary_code_point">
	/// Unicode supplementary code point</a>, constant {@code U+10000}.
	/// </summary>
	public const int MinSupplementaryCodePoint = 0x010000;

	/// <summary>
	/// The minimum value of a
	/// <a href="http://www.unicode.org/glossary/#code_point">
	/// Unicode code point</a>, constant {@code U+0000}.
	/// </summary>
	public const int MinCodePoint = 0x000000;

	/// <summary>
	/// The maximum value of a
	/// <a href="http://www.unicode.org/glossary/#code_point">
	/// Unicode code point</a>, constant {@code U+10FFFF}.
	/// </summary>
	public const int MaxCodePoint = 0X10FFFF;
}


