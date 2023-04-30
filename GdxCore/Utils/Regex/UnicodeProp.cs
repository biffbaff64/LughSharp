namespace LibGDXSharp.Utils.Regex
{
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
        public static bool Is( this UnicodeProp ucp, int ch )
        {
            switch ( ucp )
            {
                // Letter, Digit, TitleCase, Lowercase, Uppercase
                case UnicodeProp.Alphabetic:
                    return CharHelper.IsAlphabetic( ch );

                // A....Z, a....z
                case UnicodeProp.Letter:
                    return char.IsLetter( (char)ch );

                case UnicodeProp.Ideographic:
                    return CharHelper.IsIdeographic( ch );
                
                // a....z
                case UnicodeProp.Lowercase:
                    return char.IsLower( (char)ch );

                // A....Z
                case UnicodeProp.Uppercase:
                    return char.IsUpper( ch );

                case UnicodeProp.Titlecase:
                    return CharHelper.IsTitleCase( ch );

                case UnicodeProp.White_Space:
                    return char.IsWhiteSpace( ch );
                
                case UnicodeProp.Control:
                    return char.IsControl( ch );

                case UnicodeProp.Punctuation:
                    return char.IsPunctuation( ch );

                // 0....9, A....F, a....f
                case UnicodeProp.Hex_Digit:
                    return char.IsAsciiHexDigit( ch );

                case UnicodeProp.Assigned:
                    return char.IsWhiteSpace( ch );

                case UnicodeProp.Noncharacter_Code_Point:
                    return char.IsWhiteSpace( ch );

                // 0....9
                case UnicodeProp.Digit:
                    return char.IsDigit( ch );

                // A....Z, a....z, 0....9
                case UnicodeProp.Alnum:
                    return char.IsAsciiLetterOrDigit( ch );

                case UnicodeProp.Blank:
                    return char.IsWhiteSpace( ch );

                case UnicodeProp.Graph:
                    return char.IsWhiteSpace( ch );

                case UnicodeProp.Print:
                    return char.IsWhiteSpace( ch );

                case UnicodeProp.Word:
                    return char.IsWhiteSpace( ch );

                case UnicodeProp.Join_Control:
                    return char.IsWhiteSpace( ch );

                default:
                    return false;

                    break;
            }
        }
    }
}

/*
{
            public bool is(int ch)
            {
                return Character.isAlphabetic(ch);
            }
        },

        LETTER
        {
            public bool is( int ch )
            {
                return char.IsLetter( ch );
            }
        },

        IDEOGRAPHIC

    {
    public bool is( int ch )
    {
        return Character.isIdeographic( ch );
    }
    },

    LOWERCASE

    {
    public bool is( int ch )
    {
        return char.IsLower( ch );
    }
    },

    UPPERCASE

    {
    public bool is( int ch )
    {
        return char.IsUpper( ch );
    }
    },

    TITLECASE

    {
    public bool is( int ch )
    {
        return Character.isTitleCase( ch );
    }
    },

    WHITE_SPACE

    {
    public bool is( int ch )
    {
        return ( ( ( ( 1 << UnicodeCategory.SpaceSeparator ) | ( 1 << UnicodeCategory.LineSeparator ) | ( 1 << UnicodeCategory.ParagraphSeparator ) ) >> char.GetUnicodeCategory( ch ) ) & 1 ) != 0 || ( ch >= 0x9 && ch <= 0xd ) || ( ch == 0x85 );
    }
    },

    CONTROL

    {
    public bool is( int ch )
    {
        return char.GetUnicodeCategory( ch ) == UnicodeCategory.Control;
    }
    },

PUNCTUATION

    {
    public bool is( int ch )
    {
        return ( ( ( ( 1 << UnicodeCategory.ConnectorPunctuation ) | ( 1 << UnicodeCategory.DashPunctuation ) | ( 1 << UnicodeCategory.OpenPunctuation ) | ( 1 << UnicodeCategory.ClosePunctuation ) | ( 1 << UnicodeCategory.OtherPunctuation ) | ( 1 << UnicodeCategory.InitialQuotePunctuation ) | ( 1 << UnicodeCategory.FinalQuotePunctuation ) ) >> char.GetUnicodeCategory( ch ) ) & 1 ) != 0;
    }
    },

HEX_DIGIT

    {
    public bool is( int ch )
    {
        return DIGIT.is( ch ) || ( ch >= 0x0030 && ch <= 0x0039 ) || ( ch >= 0x0041 && ch <= 0x0046 ) || ( ch >= 0x0061 && ch <= 0x0066 ) || ( ch >= 0xFF10 && ch <= 0xFF19 ) || ( ch >= 0xFF21 && ch <= 0xFF26 ) || ( ch >= 0xFF41 && ch <= 0xFF46 );
    }
    },

ASSIGNED

    {
    public bool is( int ch )
    {
        return char.GetUnicodeCategory( ch ) != UnicodeCategory.OtherNotAssigned;
    }
    },

NONCHARACTER_CODE_POINT

    {
    public bool is( int ch )
    {
        return ( ch & 0xfffe ) == 0xfffe || ( ch >= 0xfdd0 && ch <= 0xfdef );
    }
    },

DIGIT

    {
    // \p{gc=Decimal_Number}
    public bool Is( int ch )
    {
        return char.IsDigit( ch );
    }
    }
    , ALNUM

    {
    // \p{alpha}
    // \p{digit}
    public bool Is( int ch )
    {
        return ALPHABETIC.Is( ch ) || DIGIT.Is( ch );
    }
    }
    , BLANK

    {
    // \p{Whitespace} --
    // [\N{LF} \N{VT} \N{FF} \N{CR} \N{NEL}  -> 0xa, 0xb, 0xc, 0xd, 0x85
    //  \p{gc=Line_Separator}
    //  \p{gc=Paragraph_Separator}]
    public bool Is( int ch )
    {
        return char.GetUnicodeCategory( ch ) == UnicodeCategory.SpaceSeparator || ch == 0x9; // \N{HT}
    }
    }
    , GRAPH

    {
    // [^
    //  \p{space}
    //  \p{gc=Control}
    //  \p{gc=Surrogate}
    //  \p{gc=Unassigned}]
    public bool Is( int ch )
    {
        return ( ( ( ( 1 << UnicodeCategory.SpaceSeparator ) | ( 1 << UnicodeCategory.LineSeparator ) | ( 1 << UnicodeCategory.ParagraphSeparator ) | ( 1 << UnicodeCategory.Control ) | ( 1 << UnicodeCategory.Surrogate ) | ( 1 << UnicodeCategory.OtherNotAssigned ) ) >> char.GetUnicodeCategory( ch ) ) & 1 ) == 0;
    }
    }
    , PRINT

    {
    // \p{graph}
    // \p{blank}
    // -- \p{cntrl}
    public bool Is( int ch )
    {
        return ( GRAPH.Is( ch ) || BLANK.Is( ch ) ) && !CONTROL.Is( ch );
    }
    }
    , WORD

    {
    //  \p{alpha}
    //  \p{gc=Mark}
    //  \p{digit}
    //  \p{gc=Connector_Punctuation}
    //  \p{Join_Control}    200C..200D

    public bool Is( int ch )
    {
        return ALPHABETIC.Is( ch ) || ( ( ( ( 1 << UnicodeCategory.NonSpacingMark ) | ( 1 << UnicodeCategory.EnclosingMark ) | ( 1 << UnicodeCategory.SpacingCombiningMark ) | ( 1 << UnicodeCategory.DecimalDigitNumber ) | ( 1 << UnicodeCategory.ConnectorPunctuation ) ) >> char.GetUnicodeCategory( ch ) ) & 1 ) != 0 || JOIN_CONTROL.Is( ch );
    }
    }
    , JOIN_CONTROL

    {
    //  200C..200D    PropList.txt:Join_Control
    public bool Is( int ch )
    {
        return ( ch == 0x200C || ch == 0x200D );
    }
    }

    ;

    private static Dictionary< string, string > _posix = new Dictionary< string, string >();
    private static Dictionary< string, string > _aliases = new Dictionary< string, string >();

    static ImpliedClass()
    {
        _posix[ "ALPHA" ]  = "ALPHABETIC";
        _posix[ "LOWER" ]  = "LOWERCASE";
        _posix[ "UPPER" ]  = "UPPERCASE";
        _posix[ "SPACE" ]  = "WHITE_SPACE";
        _posix[ "PUNCT" ]  = "PUNCTUATION";
        _posix[ "XDIGIT" ] = "HEX_DIGIT";
        _posix[ "ALNUM" ]  = "ALNUM";
        _posix[ "CNTRL" ]  = "CONTROL";
        _posix[ "DIGIT" ]  = "DIGIT";
        _posix[ "BLANK" ]  = "BLANK";
        _posix[ "GRAPH" ]  = "GRAPH";
        _posix[ "PRINT" ]  = "PRINT";

        _aliases[ "WHITESPACE" ]            = "WHITE_SPACE";
        _aliases[ "HEXDIGIT" ]              = "HEX_DIGIT";
        _aliases[ "NONCHARACTERCODEPOINT" ] = "NONCHARACTER_CODE_POINT";
        _aliases[ "JOINCONTROL" ]           = "JOIN_CONTROL";
    }

    public static UnicodeProp ForName( string propName )
    {
        propName = propName.ToUpper( Locale.ENGLISH );
        string alias = aliases.get( propName );

        if ( !string.ReferenceEquals( alias, null ) )
        {
            propName = alias;
        }

        try
        {
            return valueOf( propName );
        }
        catch ( System.ArgumentException )
        {
        }

        return null;
    }

    public static UnicodeProp ForPOSIXName( string propName )
    {
        propName = posix.get( propName.ToUpper( Locale.ENGLISH ) );

        if ( string.ReferenceEquals( propName, null ) )
        {
            return null;
        }

        return valueOf( propName );
    }

    public abstract bool Is( int ch );
}
*/
