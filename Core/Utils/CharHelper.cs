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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LibGDXSharp.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class CharHelper
{
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
    public const int Error = unchecked( ( int )0xFFFFFFFF );

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
    /// The constant value of this field is the largest value of type char, '\uFFFF'.
    /// </summary>
    public const char Max_Value = '\uFFFF';

    /// <summary>
    /// The constant value of this field is the smallest value of type char, '\u0000'.
    /// </summary>
    public const char Min_Value = '\u0000';

    public const int Min_Radix = 2;
    public const int Max_Radix = 36;

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
        return codePoint >= Min_Supplementary_Code_Point ? 2 : 1;
    }

    /// <summary>
    /// Returns the code point at thre given index in the supplied string.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public static int CodePointAt( string str, int position )
    {
        return char.ConvertToUtf32( str, position );
    }

    /// <summary>
    /// Determines if the specified character (Unicode code point) is an alphabet.
    /// A character is considered to be alphabetic if its general category type,
    /// provided by getType(codePoint), is any of the following:
    /// <para>
    ///     UPPERCASE_LETTER
    ///     LOWERCASE_LETTER
    ///     TITLECASE_LETTER
    ///     MODIFIER_LETTER
    ///     OTHER_LETTER
    ///     LETTER_NUMBER
    /// </para>
    /// <para>
    /// or it has contributory property Other_Alphabetic as defined by the Unicode Standard.
    /// </para>
    /// </summary>
    /// <param name="codePoint"></param>
    /// <returns></returns>
    public static bool IsAlphabetic( char codePoint )
    {
        return false;
    }

    public static bool IsIdeographic( char ch )
    {
        return false;
    }

	/// <summary>
	/// Determines if the specified character is a lowercase character.
	/// <para>
	/// A character is lowercase if its general category type, provided by <tt>CharHelper.GetType(ch)</tt>,
	/// is <tt>LOWERCASE_LETTER</tt>, or it has contributory property Other_Lowercase as defined
	/// by the Unicode Standard.
	/// </para>
	/// <para>
	/// The following are examples of lowercase characters:
	/// <para>
	/// a b c d e f g h i j k l m n o p q r s t u v w x y z
	/// '&#92;u00DF' '&#92;u00E0' '&#92;u00E1' '&#92;u00E2' '&#92;u00E3' '&#92;u00E4' '&#92;u00E5' '&#92;u00E6'
	/// '&#92;u00E7' '&#92;u00E8' '&#92;u00E9' '&#92;u00EA' '&#92;u00EB' '&#92;u00EC' '&#92;u00ED' '&#92;u00EE'
	/// '&#92;u00EF' '&#92;u00F0' '&#92;u00F1' '&#92;u00F2' '&#92;u00F3' '&#92;u00F4' '&#92;u00F5' '&#92;u00F6'
	/// '&#92;u00F8' '&#92;u00F9' '&#92;u00FA' '&#92;u00FB' '&#92;u00FC' '&#92;u00FD' '&#92;u00FE' '&#92;u00FF'
	/// </para>
	/// </para>
	/// <para> Many other Unicode characters are lowercase too. </para>
	/// <para><b>Note:</b> This method cannot handle supplementary characters. To support
	/// all Unicode characters, including supplementary characters, use the <see cref="IsLowerCase(int)"/>
	/// method.
	/// </para>
	/// </summary>
	/// <param name="ch"> the character to be tested. </param>
	/// <returns> <tt>true</tt> if the character is lowercase; <tt>false</tt> otherwise. </returns>
	/// <seealso cref="Character.isLowerCase(char)"/>
	/// <seealso cref="Character.isTitleCase(char)"/>
	/// <seealso cref="Character.toLowerCase(char)"/>
	/// <seealso cref="Character.getType(char)"/>
	public static bool IsLowerCase(char ch)
	{
		return IsLowerCase((int)ch);
	}

	/// <summary>
	/// Determines if the specified character (Unicode code point) is a lowercase character.
	/// <para>
	/// A character is lowercase if its general category type, provided
	/// by <seealso cref="Character.getType getType(codePoint)"/>, is
	/// {@code LOWERCASE_LETTER}, or it has contributory property
	/// Other_Lowercase as defined by the Unicode Standard.
	/// </para>
	/// <para>
	/// The following are examples of lowercase characters:
	/// <blockquote><pre>
	/// a b c d e f g h i j k l m n o p q r s t u v w x y z
	/// '&#92;u00DF' '&#92;u00E0' '&#92;u00E1' '&#92;u00E2' '&#92;u00E3' '&#92;u00E4' '&#92;u00E5' '&#92;u00E6'
	/// '&#92;u00E7' '&#92;u00E8' '&#92;u00E9' '&#92;u00EA' '&#92;u00EB' '&#92;u00EC' '&#92;u00ED' '&#92;u00EE'
	/// '&#92;u00EF' '&#92;u00F0' '&#92;u00F1' '&#92;u00F2' '&#92;u00F3' '&#92;u00F4' '&#92;u00F5' '&#92;u00F6'
	/// '&#92;u00F8' '&#92;u00F9' '&#92;u00FA' '&#92;u00FB' '&#92;u00FC' '&#92;u00FD' '&#92;u00FE' '&#92;u00FF'
	/// </pre></blockquote>
	/// </para>
	/// <para> Many other Unicode characters are lowercase too.
	/// 
	/// </para>
	/// </summary>
	/// <param name="codePoint"> the character (Unicode code point) to be tested. </param>
	/// <returns>  {@code true} if the character is lowercase;
	///          {@code false} otherwise. </returns>
	public static bool IsLowerCase(int codePoint)
	{
		return getType(codePoint) == UnicodeCategory.LowercaseLetter || CharacterData.Of(codePoint).isOtherLowercase(codePoint);
	}

	/// <summary>
	/// Determines if the specified character is an uppercase character.
	/// <para>
	/// A character is uppercase if its general category type, provided by
	/// {@code Character.getType(ch)}, is {@code UPPERCASE_LETTER}.
	/// or it has contributory property Other_Uppercase as defined by the Unicode Standard.
	/// </para>
	/// <para>
	/// The following are examples of uppercase characters:
	/// <blockquote><pre>
	/// A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
	/// '&#92;u00C0' '&#92;u00C1' '&#92;u00C2' '&#92;u00C3' '&#92;u00C4' '&#92;u00C5' '&#92;u00C6' '&#92;u00C7'
	/// '&#92;u00C8' '&#92;u00C9' '&#92;u00CA' '&#92;u00CB' '&#92;u00CC' '&#92;u00CD' '&#92;u00CE' '&#92;u00CF'
	/// '&#92;u00D0' '&#92;u00D1' '&#92;u00D2' '&#92;u00D3' '&#92;u00D4' '&#92;u00D5' '&#92;u00D6' '&#92;u00D8'
	/// '&#92;u00D9' '&#92;u00DA' '&#92;u00DB' '&#92;u00DC' '&#92;u00DD' '&#92;u00DE'
	/// </pre></blockquote>
	/// </para>
	/// <para> Many other Unicode characters are uppercase too.
	/// 
	/// </para>
	/// <para><b>Note:</b> This method cannot handle <a
	/// href="#supplementary"> supplementary characters</a>. To support
	/// all Unicode characters, including supplementary characters, use
	/// the <seealso cref="isUpperCase(int)"/> method.
	/// 
	/// </para>
	/// </summary>
	/// <param name="ch">   the character to be tested. </param>
	/// <returns>  {@code true} if the character is uppercase;
	///          {@code false} otherwise. </returns>
	public static bool IsUpperCase(char ch)
	{
		return IsUpperCase((int)ch);
	}

	/// <summary>
	/// Determines if the specified character (Unicode code point) is an uppercase character.
	/// <para>
	/// A character is uppercase if its general category type, provided by
	/// <seealso cref="Character.getType(int) getType(codePoint)"/>, is {@code UPPERCASE_LETTER},
	/// or it has contributory property Other_Uppercase as defined by the Unicode Standard.
	/// </para>
	/// <para>
	/// The following are examples of uppercase characters:
	/// <blockquote><pre>
	/// A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
	/// '&#92;u00C0' '&#92;u00C1' '&#92;u00C2' '&#92;u00C3' '&#92;u00C4' '&#92;u00C5' '&#92;u00C6' '&#92;u00C7'
	/// '&#92;u00C8' '&#92;u00C9' '&#92;u00CA' '&#92;u00CB' '&#92;u00CC' '&#92;u00CD' '&#92;u00CE' '&#92;u00CF'
	/// '&#92;u00D0' '&#92;u00D1' '&#92;u00D2' '&#92;u00D3' '&#92;u00D4' '&#92;u00D5' '&#92;u00D6' '&#92;u00D8'
	/// '&#92;u00D9' '&#92;u00DA' '&#92;u00DB' '&#92;u00DC' '&#92;u00DD' '&#92;u00DE'
	/// </pre></blockquote>
	/// </para>
	/// <para> Many other Unicode characters are uppercase too.<p>
	/// 
	/// </para>
	/// </summary>
	/// <param name="codePoint"> the character (Unicode code point) to be tested. </param>
	/// <returns>  {@code true} if the character is uppercase;
	///          {@code false} otherwise. </returns>
	public static bool IsUpperCase(int codePoint)
	{
		return getType(codePoint) == UnicodeCategory.UppercaseLetter || CharacterData.Of(codePoint).isOtherUppercase(codePoint);
	}

    public static bool IsTitleCase( char ch )
    {
        return false;
    }

    public static int GetCharCat( char ch )
    {
        return 0;
    }
    
	/// <summary>
	/// Converts the character argument to lowercase using case mapping information
	/// from the UnicodeData file.
	/// <para>
	/// Note that <tt>CharHelper.IsLowerCase(CharHelper.ToLowerCase(ch))</tt>
	/// does not always return <tt>true</tt> for some ranges of characters,
	/// particularly those that are symbols or ideographs.
	/// </para>
	/// <para>
	/// In general, <see cref="String.ToLower()"/> should be used to map
	/// characters to lowercase. String case mapping methods have several benefits
	/// over CharHelper case mapping methods. String case mapping methods can perform
	/// locale-sensitive mappings, context-sensitive mappings, and 1:M character
	/// mappings, whereas the Character case mapping methods cannot.
	/// </para>
	/// <para>
	/// <b>Note:</b> This method cannot handle supplementary characters. To support
	/// all Unicode characters, including supplementary characters, use the method
	/// <see cref="ToLowerCase(int)"/> instead.
	/// </para>
	/// </summary>
	/// <param name="ch"> the character to be converted. </param>
	/// <returns>
	/// the lowercase equivalent of the character, if any; otherwise, the character itself. </returns>
	public static char ToLowerCase(char ch)
	{
		return (char)ToLowerCase((int)ch);
	}

	/// <summary>
	/// Converts the character (Unicode code point) argument to
	/// lowercase using case mapping information from the UnicodeData
	/// file.
	/// 
	/// <para> Note that
	/// {@code Character.isLowerCase(Character.toLowerCase(codePoint))}
	/// does not always return {@code true} for some ranges of
	/// characters, particularly those that are symbols or ideographs.
	/// 
	/// </para>
	/// <para>In general, <seealso cref="String.toLowerCase()"/> should be used to map
	/// characters to lowercase. {@code String} case mapping methods
	/// have several benefits over {@code Character} case mapping methods.
	/// {@code String} case mapping methods can perform locale-sensitive
	/// mappings, context-sensitive mappings, and 1:M character mappings, whereas
	/// the {@code Character} case mapping methods cannot.
	/// 
	/// </para>
	/// </summary>
	/// <param name="codePoint">   the character (Unicode code point) to be converted. </param>
	/// <returns>  the lowercase equivalent of the character (Unicode code
	///          point), if any; otherwise, the character itself. </returns>
	/// <seealso cref="Character.isLowerCase(int)"/>
	/// <seealso cref="String.toLowerCase()"
	/// 
	/// @since   1.5/>
	public static int ToLowerCase(int codePoint)
	{
		return CharacterData.Of(codePoint).ToLowerCase(codePoint);
	}

	/// <summary>
	/// Converts the character argument to uppercase using case mapping
	/// information from the UnicodeData file.
	/// <para>
	/// Note that
	/// {@code Character.isUpperCase(Character.toUpperCase(ch))}
	/// does not always return {@code true} for some ranges of
	/// characters, particularly those that are symbols or ideographs.
	/// 
	/// </para>
	/// <para>In general, <seealso cref="String.toUpperCase()"/> should be used to map
	/// characters to uppercase. {@code String} case mapping methods
	/// have several benefits over {@code Character} case mapping methods.
	/// {@code String} case mapping methods can perform locale-sensitive
	/// mappings, context-sensitive mappings, and 1:M character mappings, whereas
	/// the {@code Character} case mapping methods cannot.
	/// 
	/// </para>
	/// <para><b>Note:</b> This method cannot handle <a
	/// href="#supplementary"> supplementary characters</a>. To support
	/// all Unicode characters, including supplementary characters, use
	/// the <seealso cref="toUpperCase(int)"/> method.
	/// 
	/// </para>
	/// </summary>
	/// <param name="ch">   the character to be converted. </param>
	/// <returns>  the uppercase equivalent of the character, if any;
	///          otherwise, the character itself. </returns>
	/// <seealso cref="Character.isUpperCase(char)"/>
	/// <seealso cref="String.toUpperCase()"/>
	public static char ToUpperCase(char ch)
	{
		return (char)ToUpperCase((int)ch);
	}

	/// <summary>
	/// Converts the character (Unicode code point) argument to
	/// uppercase using case mapping information from the UnicodeData
	/// file.
	/// 
	/// <para>Note that
	/// {@code Character.isUpperCase(Character.toUpperCase(codePoint))}
	/// does not always return {@code true} for some ranges of
	/// characters, particularly those that are symbols or ideographs.
	/// 
	/// </para>
	/// <para>In general, <seealso cref="String.toUpperCase()"/> should be used to map
	/// characters to uppercase. {@code String} case mapping methods
	/// have several benefits over {@code Character} case mapping methods.
	/// {@code String} case mapping methods can perform locale-sensitive
	/// mappings, context-sensitive mappings, and 1:M character mappings, whereas
	/// the {@code Character} case mapping methods cannot.
	/// 
	/// </para>
	/// </summary>
	/// <param name="codePoint">   the character (Unicode code point) to be converted. </param>
	/// <returns>  the uppercase equivalent of the character, if any;
	///          otherwise, the character itself. </returns>
	/// <seealso cref="Character.isUpperCase(int)"/>
	/// <seealso cref="String.toUpperCase()"
	/// 
	/// @since   1.5/>
	public static int ToUpperCase(int codePoint)
	{
		return CharacterData.Of(codePoint).ToUpperCase(codePoint);
	}

	/// <summary>
	/// Converts the character argument to titlecase using case mapping
	/// information from the UnicodeData file. If a character has no
	/// explicit titlecase mapping and is not itself a titlecase char
	/// according to UnicodeData, then the uppercase mapping is
	/// returned as an equivalent titlecase mapping. If the
	/// {@code char} argument is already a titlecase
	/// {@code char}, the same {@code char} value will be
	/// returned.
	/// <para>
	/// Note that
	/// {@code Character.isTitleCase(Character.toTitleCase(ch))}
	/// does not always return {@code true} for some ranges of
	/// characters.
	/// 
	/// </para>
	/// <para><b>Note:</b> This method cannot handle <a
	/// href="#supplementary"> supplementary characters</a>. To support
	/// all Unicode characters, including supplementary characters, use
	/// the <seealso cref="toTitleCase(int)"/> method.
	/// 
	/// </para>
	/// </summary>
	/// <param name="ch">   the character to be converted. </param>
	/// <returns>  the titlecase equivalent of the character, if any;
	///          otherwise, the character itself. </returns>
	/// <seealso cref="Character.isTitleCase(char)"/>
	/// <seealso cref="Character.toLowerCase(char)"/>
	/// <seealso cref="Character.toUpperCase(char)"
	/// @since   1.0.2/>
	public static char ToTitleCase(char ch)
	{
		return (char)ToTitleCase((int)ch);
	}

	/// <summary>
	/// Converts the character (Unicode code point) argument to titlecase using case mapping
	/// information from the UnicodeData file. If a character has no
	/// explicit titlecase mapping and is not itself a titlecase char
	/// according to UnicodeData, then the uppercase mapping is
	/// returned as an equivalent titlecase mapping. If the
	/// character argument is already a titlecase
	/// character, the same character value will be
	/// returned.
	/// 
	/// <para>Note that
	/// {@code Character.isTitleCase(Character.toTitleCase(codePoint))}
	/// does not always return {@code true} for some ranges of
	/// characters.
	/// 
	/// </para>
	/// </summary>
	/// <param name="codePoint">   the character (Unicode code point) to be converted. </param>
	/// <returns>  the titlecase equivalent of the character, if any;
	///          otherwise, the character itself. </returns>
	/// <seealso cref="Character.isTitleCase(int)"/>
	/// <seealso cref="Character.toLowerCase(int)"/>
	/// <seealso cref="Character.toUpperCase(int)"
	/// @since   1.5/>
	public static int ToTitleCase(int codePoint)
	{
		return CharacterData.Of(codePoint).ToTitleCase(codePoint);
	}
}