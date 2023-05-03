using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils
{
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

        public static bool IsTitleCase( char ch )
        {
            return false;
        }

        public static int GetCharCat( char ch )
        {
            return 0;
        }
    }
}
