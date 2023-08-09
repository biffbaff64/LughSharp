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

namespace LibGDXSharp.Utils.Regex;

/// <summary>
/// Converted from Java.lang.utils.regex.UnicodeProp.java
/// </summary>
[Obsolete]
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

[Obsolete]
public static class UnicodePropExtensions
{
    private readonly static Dictionary< string, UnicodeProp > Posix;
    private readonly static Dictionary< string, UnicodeProp > Aliases;

    static UnicodePropExtensions()
    {
        Posix   = new Dictionary< string, UnicodeProp >();
        Aliases = new Dictionary< string, UnicodeProp >();

        //@formatter:off
        Posix.Add( "ALPHA",     UnicodeProp.Alphabetic );
        Posix.Add( "LOWER",     UnicodeProp.Lowercase );
        Posix.Add( "UPPER",     UnicodeProp.Uppercase );
        Posix.Add( "SPACE",     UnicodeProp.White_Space );
        Posix.Add( "PUNCT",     UnicodeProp.Punctuation );
        Posix.Add( "XDIGIT",    UnicodeProp.Hex_Digit );
        Posix.Add( "ALNUM",     UnicodeProp.Alnum );
        Posix.Add( "CNTRL",     UnicodeProp.Control );
        Posix.Add( "DIGIT",     UnicodeProp.Digit );
        Posix.Add( "BLANK",     UnicodeProp.Blank );
        Posix.Add( "GRAPH",     UnicodeProp.Graph );
        Posix.Add( "PRINT",     UnicodeProp.Print );

        Aliases.Add( "WHITESPACE",              UnicodeProp.White_Space );
        Aliases.Add( "HEXDIGIT",                UnicodeProp.Hex_Digit );
        Aliases.Add( "NONCHARACTERCODEPOINT",   UnicodeProp.Noncharacter_Code_Point );
        Aliases.Add( "JOINCONTROL",             UnicodeProp.Join_Control );
        //@formatter:on
    }

    /// <summary>
    /// Returns the property which matches the supplied string.
    /// </summary>
    [Obsolete]
    public static UnicodeProp? ForName( string propName )
    {
        return Aliases[ propName.ToUpper() ];
    }

    /// <summary>
    /// </summary>
    /// <param name="propName"></param>
    /// <returns></returns>
    [Obsolete]
    public static UnicodeProp? ForPosixName( string propName )
    {
        return Posix[ propName.ToUpper() ];
    }

    /// <summary>
    /// </summary>
    /// <param name="ucp"></param>
    /// <param name="ch"></param>
    /// <returns></returns>
    [Obsolete]
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
//                return char.IsAlphabetic( ch );
                return char.IsDigit( ch ) || char.IsLetter( ch );
            
            // ------------------------------------------------------
            // A....Z, a....z
            case UnicodeProp.Letter:
                return char.IsLetter( ch );

            // ------------------------------------------------------
            case UnicodeProp.Ideographic:
//                return CharHelper.IsIdeographic( ch );

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
//                return CharHelper.IsTitleCase( ch );

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
                return ( ch == CharHelper.SPACE_SEPARATOR ) || ( ch == 0x09 );

            // ------------------------------------------------------
//            case UnicodeProp.Graph:
//                return ( ( ( ( 1 << CharHelper.SpaceSeparator )
//                             | ( 1 << CharHelper.LineSeparator )
//                             | ( 1 << CharHelper.ParagraphSeparator )
//                             | ( 1 << CharHelper.Control )
//                             | ( 1 << CharHelper.Surrogate )
//                             | ( 1 << CharHelper.Unassigned ) )
//                           >> char.GetUnicodeCategory( ch ) )
//                         & 1 )
//                       == 0;

            // ------------------------------------------------------
            case UnicodeProp.Print:
                return Is( UnicodeProp.Graph, ch )
                       || Is( UnicodeProp.Blank, ch )
                       || Is( UnicodeProp.Control, ch );

            // ------------------------------------------------------
//            case UnicodeProp.Word:
//                return Is( UnicodeProp.Alphabetic, ch )
//                       || ( ( ( ( ( 1 << CharHelper.NonSpacingMark )
//                                  | ( 1 << CharHelper.EnclosingMark )
//                                  | ( 1 << CharHelper.CombiningSpacingMark )
//                                  | ( 1 << CharHelper.DecimalDigitNumber )
//                                  | ( 1 << CharHelper.ConnectorPunctuation ) )
//                                >> CharHelper.GetCharCat( ch ) )
//                              & 1 )
//                            == 0 )
//                       || Is( UnicodeProp.Join_Control, ch );

            // ------------------------------------------------------
            case UnicodeProp.Join_Control:
                return ( ( ch == 0x200C ) || ( ch == 0x200D ) );

            // ------------------------------------------------------
            default:
                return false;
        }
    }
}
