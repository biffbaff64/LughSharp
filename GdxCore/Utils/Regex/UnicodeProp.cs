namespace LibGDXSharp.Utils.Regex;

/// <summary>
/// Converted from Java.lang.utils.regex.UnicodeProp.java
/// </summary>
public enum UnicodeProp
{
    Alphabetic,
    Letter,
    Ideographic,
    Lowercase,
    Uppercase,
    Titlecase,
    White_Space,
    Control,
    Punctuation,
    Hex_Digit,
    Assigned,
    Noncharacter_Code_Point,
    Digit,
    Alnum,
    Blank,
    Graph,
    Print,
    Word,
    Join_Control
}

public static class UnicodePropExtensions
{
    private readonly static Dictionary< string, UnicodeProp > posix;
    private readonly static Dictionary< string, UnicodeProp > aliases;

    static UnicodePropExtensions()
    {
        posix   = new Dictionary< string, UnicodeProp >();
        aliases = new Dictionary< string, UnicodeProp >();

        //@formatter:off
        posix.Add( "ALPHA",     UnicodeProp.Alphabetic );
        posix.Add( "LOWER",     UnicodeProp.Lowercase );
        posix.Add( "UPPER",     UnicodeProp.Uppercase );
        posix.Add( "SPACE",     UnicodeProp.White_Space );
        posix.Add( "PUNCT",     UnicodeProp.Punctuation );
        posix.Add( "XDIGIT",    UnicodeProp.Hex_Digit );
        posix.Add( "ALNUM",     UnicodeProp.Alnum );
        posix.Add( "CNTRL",     UnicodeProp.Control );
        posix.Add( "DIGIT",     UnicodeProp.Digit );
        posix.Add( "BLANK",     UnicodeProp.Blank );
        posix.Add( "GRAPH",     UnicodeProp.Graph );
        posix.Add( "PRINT",     UnicodeProp.Print );

        aliases.Add( "WHITESPACE",              UnicodeProp.White_Space );
        aliases.Add( "HEXDIGIT",                UnicodeProp.Hex_Digit );
        aliases.Add( "NONCHARACTERCODEPOINT",   UnicodeProp.Noncharacter_Code_Point );
        aliases.Add( "JOINCONTROL",             UnicodeProp.Join_Control );
        //@formatter:on
    }

    /// <summary>
    /// Returns the property which matches the supplied string.
    /// </summary>
    public static UnicodeProp? ForName( string propName )
    {
        return aliases[ propName.ToUpper() ];
    }

    /// <summary>
    /// </summary>
    /// <param name="propName"></param>
    /// <returns></returns>
    public static UnicodeProp? ForPosixName( string propName )
    {
        return posix[ propName.ToUpper() ];
    }

    /// <summary>
    /// </summary>
    /// <param name="ucp"></param>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static bool Is( this UnicodeProp ucp, char ch )
    {
        // Note:
        // I considered changing this to a switch expression but the
        // resulting code was less readable.
        // Note: There is some recursiveness in this method which, although
        // it is managed, needs reworking.
        switch ( ucp )
        {
            // ------------------------------------------------------
            // Letter, Digit, TitleCase, Lowercase, Uppercase
            case UnicodeProp.Alphabetic:
                return CharHelper.IsAlphabetic( ch );

            // ------------------------------------------------------
            // A....Z, a....z
            case UnicodeProp.Letter:
                return char.IsLetter( ch );

            // ------------------------------------------------------
            case UnicodeProp.Ideographic:
                return CharHelper.IsIdeographic( ch );

            // ------------------------------------------------------
            // a....z
            case UnicodeProp.Lowercase:
                return char.IsLower( ch );

            // ------------------------------------------------------
            // A....Z
            case UnicodeProp.Uppercase:
                return char.IsUpper( ch );

            // ------------------------------------------------------
            case UnicodeProp.Titlecase:
                return CharHelper.IsTitleCase( ch );

            // ------------------------------------------------------
            case UnicodeProp.White_Space:
                return char.IsWhiteSpace( ch );

            // ------------------------------------------------------
            case UnicodeProp.Control:
                return char.IsControl( ch );

            // ------------------------------------------------------
            case UnicodeProp.Punctuation:
                return char.IsPunctuation( ch );

            // ------------------------------------------------------
            // 0....9, A....F, a....f
            case UnicodeProp.Hex_Digit:
                return char.IsAsciiHexDigit( ch );

            // ------------------------------------------------------
            case UnicodeProp.Assigned:
                return char.IsWhiteSpace( ch );

            // ------------------------------------------------------
            case UnicodeProp.Noncharacter_Code_Point:
                return char.IsWhiteSpace( ch );

            // ------------------------------------------------------
            // 0....9
            case UnicodeProp.Digit:
                return char.IsDigit( ch );

            // ------------------------------------------------------
            // A....Z, a....z, 0....9
            case UnicodeProp.Alnum:
                return char.IsAsciiLetterOrDigit( ch );

            // ------------------------------------------------------
            case UnicodeProp.Blank:
                return ( ch == CharHelper.SpaceSeparator ) || ( ch == 0x09 );

            // ------------------------------------------------------
            case UnicodeProp.Graph:
                return ( ( ( ( 1 << CharHelper.SpaceSeparator )
                             | ( 1 << CharHelper.LineSeparator )
                             | ( 1 << CharHelper.ParagraphSeparator )
                             | ( 1 << CharHelper.Control )
                             | ( 1 << CharHelper.Surrogate )
                             | ( 1 << CharHelper.Unassigned ) )
                           >> CharHelper.GetCharCat( ch ) )
                         & 1 )
                       == 0;

            // ------------------------------------------------------
            case UnicodeProp.Print:
                return Is( UnicodeProp.Graph, ch )
                       || Is( UnicodeProp.Blank, ch )
                       || Is( UnicodeProp.Control, ch );

            // ------------------------------------------------------
            case UnicodeProp.Word:
                return Is( UnicodeProp.Alphabetic, ch )
                       || ( ( ( ( ( 1 << CharHelper.NonSpacingMark )
                                  | ( 1 << CharHelper.EnclosingMark )
                                  | ( 1 << CharHelper.CombiningSpacingMark )
                                  | ( 1 << CharHelper.DecimalDigitNumber )
                                  | ( 1 << CharHelper.ConnectorPunctuation ) )
                                >> CharHelper.GetCharCat( ch ) )
                              & 1 )
                            == 0 )
                       || Is( UnicodeProp.Join_Control, ch );

            // ------------------------------------------------------
            case UnicodeProp.Join_Control:
                return ( ( ch == 0x200C ) || ( ch == 0x200D ) );

            // ------------------------------------------------------
            default:
                return false;
        }
    }
}