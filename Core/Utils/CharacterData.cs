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

namespace LibGDXSharp.Utils
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public abstract class CharacterData
    {
        public abstract int  GetProperties( int ch );
        public abstract int  GetType( int ch );
        public abstract bool IsWhitespace( int ch );
        public abstract bool IsMirrored( int ch );
        public abstract bool IsJavaIdentifierStart( int ch );
        public abstract bool IsJavaIdentifierPart( int ch );
        public abstract bool IsUnicodeIdentifierStart( int ch );
        public abstract bool IsUnicodeIdentifierPart( int ch );
        public abstract bool IsIdentifierIgnorable( int ch );
        public abstract int  ToLowerCase( int ch );
        public abstract int  ToUpperCase( int ch );
        public abstract int  ToTitleCase( int ch );
        public abstract int  Digit( int ch, int radix );
        public abstract int  GetNumericValue( int ch );
        public abstract byte GetDirectionality( int ch );

        public int ToUpperCaseEx( int ch )
        {
            return ToUpperCase( ch );
        }

        public char[] ToUpperCaseCharArray( int ch )
        {
            return null;
        }

        public bool IsOtherLowercase( int ch )
        {
            return false;
        }

        public bool IsOtherUppercase( int ch )
        {
            return false;
        }

        public bool IsOtherAlphabetic( int ch )
        {
            return false;
        }

        public bool IsIdeographic( int ch )
        {
            return false;
        }

        // Character <= 0xff (basic latin) is handled by internal fast-path
        // to avoid initializing large tables.
        // Note: performance of this "fast-path" code may be sub-optimal
        // in negative cases for some accessors due to complicated ranges.
        // Should revisit after optimization of table initialization.
        public static CharacterData Of( int ch )
        {
            if ( ( ch >>> 8 ) == 0 )
            {
                // fast-path
                return CharacterDataLatin1.instance;
            }
            else
            {
                switch ( ch >>> 16 )
                {
                    //plane 00-16
                    case ( 0 ):
                        return CharacterData00.instance;

                    case ( 1 ):
                        return CharacterData01.instance;

                    case ( 2 ):
                        return CharacterData02.instance;

                    case ( 14 ):
                        return CharacterData0E.instance;

                    case ( 15 ): // Private Use
                    case ( 16 ): // Private Use
                        return CharacterDataPrivateUse.instance;

                    default:
                        return CharacterDataUndefined.instance;
                }
            }
        }
    }
}