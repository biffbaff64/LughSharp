// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace Corelib.LibCore.Maths;

/// <summary>
/// Simple struct with X and Y members of a generic type, and absolutely nothing else.
/// For use when no 'padding' is needed, such as methods or constructors.
/// </summary>
/// <typeparam name="T">The type of the X and Y members.</typeparam>
/// <example>
/// <code>
/// public int X { get; set; }
/// public int Y { get; set; }
/// 
/// internal void SetPosition(Point2D&lt;int&gt; pos)
/// {
///     X = pos.X;
///     Y = pos.Y;
/// }
/// </code>
/// </example>
[PublicAPI]
public struct Point2D< T >( T x, T y )
{
    /// <summary>
    /// The X coordinate.
    /// </summary>
    public T X { get; set; } = x;

    /// <summary>
    /// The Y coordinate.
    /// </summary>
    public T Y { get; set; } = y;
}

/// <summary>
/// Simple struct with X and Y members of type int, and absolutely nothing else.
/// For use when no 'padding' is needed, such as methods or constructors.
/// </summary>
[PublicAPI]
public struct Point2D( int x, int y )
{
    /// <summary>
    /// The X coordinate.
    /// </summary>
    public int X { get; set; } = x;

    /// <summary>
    /// The Y coordinate.
    /// </summary>
    public int Y { get; set; } = y;
}

/// <summary>
/// Simple struct with X and Y members of type float, and absolutely nothing else.
/// For use when no 'padding' is needed, such as methods or constructors.
/// </summary>
[PublicAPI]
public struct Point2Df( float x, float y )
{
    /// <summary>
    /// The X coordinate.
    /// </summary>
    public float X { get; set; } = x;

    /// <summary>
    /// The Y coordinate.
    /// </summary>
    public float Y { get; set; } = y;
}
