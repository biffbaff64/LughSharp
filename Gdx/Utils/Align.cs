// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


using System.Text;

namespace LibGDXSharp.Gdx.Utils;

/// <summary>
///     Provides bit flag constants for alignment.
/// </summary>
[PublicAPI]
public static class Align
{
    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

    public static bool IsLeft( int align )             => ( align & LEFT ) != 0;
    public static bool IsRight( int align )            => ( align & RIGHT ) != 0;
    public static bool IsTop( int align )              => ( align & TOP ) != 0;
    public static bool IsBottom( int align )           => ( align & BOTTOM ) != 0;
    public static bool IsCenterHorizontal( int align ) => ( ( align & LEFT ) == 0 ) && ( ( align & RIGHT ) == 0 );
    public static bool IsCenterVertical( int align )   => ( ( align & TOP ) == 0 ) && ( ( align & BOTTOM ) == 0 );

    // ------------------------------------------------------------------------

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
