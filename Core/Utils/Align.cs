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

using System.Text;

namespace LibGDXSharp.Utils;

public static class Align
{
    public const int NONE   = 0;
    public const int CENTER = 1 << 0;
    public const int TOP    = 1 << 1;
    public const int BOTTOM = 1 << 2;
    public const int LEFT   = 1 << 3;
    public const int RIGHT  = 1 << 4;

    public const int TOP_LEFT     = TOP | LEFT;
    public const int TOP_RIGHT    = TOP | RIGHT;
    public const int BOTTOM_LEFT  = BOTTOM | LEFT;
    public const int BOTTOM_RIGHT = BOTTOM | RIGHT;

    public static bool IsLeft( int align )             => ( align & LEFT ) != 0;
    public static bool IsRight( int align )            => ( align & RIGHT ) != 0;
    public static bool IsTop( int align )              => ( align & TOP ) != 0;
    public static bool IsBottom( int align )           => ( align & BOTTOM ) != 0;
    public static bool IsCenterHorizontal( int align ) => ( ( ( align & LEFT ) == 0 ) && ( ( align & RIGHT ) == 0 ) );
    public static bool IsCenterVertical( int align )   => ( ( ( align & TOP ) == 0 ) && ( ( align & BOTTOM ) == 0 ) );

    /// <summary>
    /// </summary>
    /// <param name="align"></param>
    /// <returns></returns>
    public static string ToString( int align )
    {
        var buffer = new StringBuilder();

        if ( ( align & TOP ) != 0 )
        {
            buffer.Append( "Top," );
        }
        else if ( ( align & BOTTOM ) != 0 )
        {
            buffer.Append( "Bottom," );
        }
        else
        {
            buffer.Append( "Center," );
        }

        if ( ( align & LEFT ) != 0 )
        {
            buffer.Append( "Left" );
        }
        else if ( ( align & RIGHT ) != 0 )
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