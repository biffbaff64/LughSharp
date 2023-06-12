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
using System.Text;

namespace LibGDXSharp.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public static class Align
{
    public const int None   = 0;
    public const int Center = 1 << 0;
    public const int Top    = 1 << 1;
    public const int Bottom = 1 << 2;
    public const int Left   = 1 << 3;
    public const int Right  = 1 << 4;

    public const int TopLeft     = Top | Left;
    public const int TopRight    = Top | Right;
    public const int BottomLeft  = Bottom | Left;
    public const int BottomRight = Bottom | Right;

    public static bool IsLeft( int align )             => ( align & Left ) != 0;
    public static bool IsRight( int align )            => ( align & Right ) != 0;
    public static bool IsTop( int align )              => ( align & Top ) != 0;
    public static bool IsBottom( int align )           => ( align & Bottom ) != 0;
    public static bool IsCenterHorizontal( int align ) => ( ( ( align & Left ) == 0 ) && ( ( align & Right ) == 0 ) );
    public static bool IsCenterVertical( int align )   => ( ( ( align & Top ) == 0 ) && ( ( align & Bottom ) == 0 ) );

    /// <summary>
    /// </summary>
    /// <param name="align"></param>
    /// <returns></returns>
    public static string ToString( int align )
    {
        var buffer = new StringBuilder();

        if ( ( align & Top ) != 0 )
        {
            buffer.Append( "Top," );
        }
        else if ( ( align & Bottom ) != 0 )
        {
            buffer.Append( "Bottom," );
        }
        else
        {
            buffer.Append( "Center," );
        }

        if ( ( align & Left ) != 0 )
        {
            buffer.Append( "Left" );
        }
        else if ( ( align & Right ) != 0 )
        {
            buffer.Append( "Right" );
        }
        else
        {
            buffer.Append( "Center" );
        }

        return buffer.ToString();
    }
}