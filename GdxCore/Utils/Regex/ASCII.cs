namespace LibGDXSharp.Utils.Regex
{
    public class ASCII
    {
        public const int Upper = 0x00000100;

        public const int Lower = 0x00000200;

        public const int Digit = 0x00000400;

        public const int Space = 0x00000800;

        public const int Punct = 0x00001000;

        public const int Cntrl = 0x00002000;

        public const int Blank = 0x00004000;

        public const int Hex = 0x00008000;

        public const int Under = 0x00010000;

        public const int Ascii = 0x0000FF00;

        public const int Alpha = ( Upper | Lower );

        public const int Alnum = ( Upper | Lower | Digit );

        public const int Graph = ( Punct | Upper | Lower | Digit );

        public const int Word = ( Upper | Lower | Under | Digit );

        public const int Xdigit = ( Hex );

        public readonly static int[] CType =
        {
            Cntrl,                 /* 00 (NUL) */
            Cntrl,                 /* 01 (SOH) */
            Cntrl,                 /* 02 (STX) */
            Cntrl,                 /* 03 (ETX) */
            Cntrl,                 /* 04 (EOT) */
            Cntrl,                 /* 05 (ENQ) */
            Cntrl,                 /* 06 (ACK) */
            Cntrl,                 /* 07 (BEL) */
            Cntrl,                 /* 08 (BS)  */
            Space + Cntrl + Blank, /* 09 (HT)  */
            Space + Cntrl,         /* 0A (LF)  */
            Space + Cntrl,         /* 0B (VT)  */
            Space + Cntrl,         /* 0C (FF)  */
            Space + Cntrl,         /* 0D (CR)  */
            Cntrl,                 /* 0E (SI)  */
            Cntrl,                 /* 0F (SO)  */
            Cntrl,                 /* 10 (DLE) */
            Cntrl,                 /* 11 (DC1) */
            Cntrl,                 /* 12 (DC2) */
            Cntrl,                 /* 13 (DC3) */
            Cntrl,                 /* 14 (DC4) */
            Cntrl,                 /* 15 (NAK) */
            Cntrl,                 /* 16 (SYN) */
            Cntrl,                 /* 17 (ETB) */
            Cntrl,                 /* 18 (CAN) */
            Cntrl,                 /* 19 (EM)  */
            Cntrl,                 /* 1A (SUB) */
            Cntrl,                 /* 1B (ESC) */
            Cntrl,                 /* 1C (FS)  */
            Cntrl,                 /* 1D (GS)  */
            Cntrl,                 /* 1E (RS)  */
            Cntrl,                 /* 1F (US)  */
            Space + Blank,         /* 20 SPACE */
            Punct,                 /* 21 !     */
            Punct,                 /* 22 "     */
            Punct,                 /* 23 #     */
            Punct,                 /* 24 $     */
            Punct,                 /* 25 %     */
            Punct,                 /* 26 &     */
            Punct,                 /* 27 '     */
            Punct,                 /* 28 (     */
            Punct,                 /* 29 )     */
            Punct,                 /* 2A *     */
            Punct,                 /* 2B +     */
            Punct,                 /* 2C ,     */
            Punct,                 /* 2D -     */
            Punct,                 /* 2E .     */
            Punct,                 /* 2F /     */
            Digit + Hex + 0,       /* 30 0     */
            Digit + Hex + 1,       /* 31 1     */
            Digit + Hex + 2,       /* 32 2     */
            Digit + Hex + 3,       /* 33 3     */
            Digit + Hex + 4,       /* 34 4     */
            Digit + Hex + 5,       /* 35 5     */
            Digit + Hex + 6,       /* 36 6     */
            Digit + Hex + 7,       /* 37 7     */
            Digit + Hex + 8,       /* 38 8     */
            Digit + Hex + 9,       /* 39 9     */
            Punct,                 /* 3A :     */
            Punct,                 /* 3B ;     */
            Punct,                 /* 3C <     */
            Punct,                 /* 3D =     */
            Punct,                 /* 3E >     */
            Punct,                 /* 3F ?     */
            Punct,                 /* 40 @     */
            Upper + Hex + 10,      /* 41 A     */
            Upper + Hex + 11,      /* 42 B     */
            Upper + Hex + 12,      /* 43 C     */
            Upper + Hex + 13,      /* 44 D     */
            Upper + Hex + 14,      /* 45 E     */
            Upper + Hex + 15,      /* 46 F     */
            Upper + 16,            /* 47 G     */
            Upper + 17,            /* 48 H     */
            Upper + 18,            /* 49 I     */
            Upper + 19,            /* 4A J     */
            Upper + 20,            /* 4B K     */
            Upper + 21,            /* 4C L     */
            Upper + 22,            /* 4D M     */
            Upper + 23,            /* 4E N     */
            Upper + 24,            /* 4F O     */
            Upper + 25,            /* 50 P     */
            Upper + 26,            /* 51 Q     */
            Upper + 27,            /* 52 R     */
            Upper + 28,            /* 53 S     */
            Upper + 29,            /* 54 T     */
            Upper + 30,            /* 55 U     */
            Upper + 31,            /* 56 V     */
            Upper + 32,            /* 57 W     */
            Upper + 33,            /* 58 X     */
            Upper + 34,            /* 59 Y     */
            Upper + 35,            /* 5A Z     */
            Punct,                 /* 5B [     */
            Punct,                 /* 5C \     */
            Punct,                 /* 5D ]     */
            Punct,                 /* 5E ^     */
            Punct | Under,         /* 5F _     */
            Punct,                 /* 60 `     */
            Lower + Hex + 10,      /* 61 a     */
            Lower + Hex + 11,      /* 62 b     */
            Lower + Hex + 12,      /* 63 c     */
            Lower + Hex + 13,      /* 64 d     */
            Lower + Hex + 14,      /* 65 e     */
            Lower + Hex + 15,      /* 66 f     */
            Lower + 16,            /* 67 g     */
            Lower + 17,            /* 68 h     */
            Lower + 18,            /* 69 i     */
            Lower + 19,            /* 6A j     */
            Lower + 20,            /* 6B k     */
            Lower + 21,            /* 6C l     */
            Lower + 22,            /* 6D m     */
            Lower + 23,            /* 6E n     */
            Lower + 24,            /* 6F o     */
            Lower + 25,            /* 70 p     */
            Lower + 26,            /* 71 q     */
            Lower + 27,            /* 72 r     */
            Lower + 28,            /* 73 s     */
            Lower + 29,            /* 74 t     */
            Lower + 30,            /* 75 u     */
            Lower + 31,            /* 76 v     */
            Lower + 32,            /* 77 w     */
            Lower + 33,            /* 78 x     */
            Lower + 34,            /* 79 y     */
            Lower + 35,            /* 7A z     */
            Punct,                 /* 7B {     */
            Punct,                 /* 7C |     */
            Punct,                 /* 7D }     */
            Punct,                 /* 7E ~     */
            Cntrl,                 /* 7F (DEL) */
        };

        public static int GetCType( int ch )
        {
            return ( ( ch & 0xFFFFFF80 ) == 0 ? CType[ ch ] : 0 );
        }

        public static bool IsType( int ch, int type )
        {
            return ( GetCType( ch ) & type ) != 0;
        }

        public static bool IsAscii( int ch )
        {
            return ( ( ch & 0xFFFFFF80 ) == 0 );
        }

        public static bool IsAlpha( int ch )
        {
            return IsType( ch, Alpha );
        }

        public static bool IsDigit( int ch )
        {
            return ( ( ch - '0' ) | ( '9' - ch ) ) >= 0;
        }

        public static bool IsAlnum( int ch )
        {
            return IsType( ch, Alnum );
        }

        public static bool IsGraph( int ch )
        {
            return IsType( ch, Graph );
        }

        public static bool IsPrint( int ch )
        {
            return ( ( ch - 0x20 ) | ( 0x7E - ch ) ) >= 0;
        }

        public static bool IsPunct( int ch )
        {
            return IsType( ch, Punct );
        }

        public static bool IsSpace( int ch )
        {
            return IsType( ch, Space );
        }

        public static bool IsHexDigit( int ch )
        {
            return IsType( ch, Hex );
        }

        public static bool IsOctDigit( int ch )
        {
            return ( ( ch - '0' ) | ( '7' - ch ) ) >= 0;
        }

        public static bool IsCntrl( int ch )
        {
            return IsType( ch, Cntrl );
        }

        public static bool IsLower( int ch )
        {
            return ( ( ch - 'a' ) | ( 'z' - ch ) ) >= 0;
        }

        public static bool IsUpper( int ch )
        {
            return ( ( ch - 'A' ) | ( 'Z' - ch ) ) >= 0;
        }

        public static bool IsWord( int ch )
        {
            return IsType( ch, Word );
        }

        public static int ToDigit( int ch )
        {
            return ( CType[ ch & 0x7F ] & 0x3F );
        }

        public static int ToLower( int ch )
        {
            return IsUpper( ch ) ? ( ch + 0x20 ) : ch;
        }

        public static int ToUpper( int ch )
        {
            return IsLower( ch ) ? ( ch - 0x20 ) : ch;
        }
    }
}
