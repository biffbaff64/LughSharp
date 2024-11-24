// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

namespace Corelib.LibCore.Utils;

/// <summary>
/// Provides bit flag constants for alignment.
/// </summary>
[PublicAPI]
public sealed class Align
{
    // ========================================================================

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

    // ========================================================================

    /// <summary>
    /// Returns TRUE if the supplied position is aligned to the LEFT.
    /// </summary>
    public static bool IsLeft( int position )
    {
        return ( position & LEFT ) != 0;
    }

    /// <summary>
    /// Returns TRUE if the supplied position is aligned to the RIGHT.
    /// </summary>
    public static bool IsRight( int position )
    {
        return ( position & RIGHT ) != 0;
    }

    /// <summary>
    /// Returns TRUE if the supplied position is aligned to the TOP.
    /// </summary>
    public static bool IsTop( int position )
    {
        return ( position & TOP ) != 0;
    }

    /// <summary>
    /// Returns TRUE if the supplied position is aligned to the BOTTOM.
    /// </summary>
    public static bool IsBottom( int position )
    {
        return ( position & BOTTOM ) != 0;
    }

    /// <summary>
    /// Returns TRUE if the supplied position is aligned horizontally central.
    /// </summary>
    public static bool IsCenterHorizontal( int position )
    {
        return ( ( position & LEFT ) == 0 ) && ( ( position & RIGHT ) == 0 );
    }

    /// <summary>
    /// Returns TRUE if the supplied position is aligned vertically central.
    /// </summary>
    public static bool IsCenterVertical( int position )
    {
        return ( ( position & TOP ) == 0 ) && ( ( position & BOTTOM ) == 0 );
    }

    // ========================================================================

    /// <inheritdoc cref="Object.ToString"/>
    public static string ToString( int position )
    {
        var buffer = new StringBuilder( "[" );

        if ( ( position & TOP ) != 0 )
        {
            buffer.Append( "Top" );
        }
        else if ( ( position & BOTTOM ) != 0 )
        {
            buffer.Append( "Bottom" );
        }
        else
        {
            buffer.Append( "Center" );
        }

        buffer.Append( "] [" );

        if ( ( position & LEFT ) != 0 )
        {
            buffer.Append( "Left" );
        }
        else if ( ( position & RIGHT ) != 0 )
        {
            buffer.Append( "Right" );
        }
        else
        {
            buffer.Append( "Center" );
        }

        buffer.Append( ']' );

        return buffer.ToString();
    }
}