// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

namespace LughSharp.Lugh.Graphics;

/// <summary>
/// A simple Rectangle shape struct, which contains only X, Y, Width, and Height
/// members. For use when no 'padding' is needed, as in method parameters, for instance.
/// <example>
/// <code>
/// public void SetRegion( Texture texture, GRect region )
/// {
///     . . .
/// }
/// </code>
/// </example>
/// </summary>
[PublicAPI]
public struct GRect
{
    public int X;
    public int Y;
    public int Width;
    public int Height;
}

/// <summary>
/// A Simple 2D Point struct with only X and Y members.
/// For use when no 'padding' is needed, as in method parameters, for instance.
/// <example>
/// <code>
/// public void SetPosition( GPoint pos )
/// {
///     Position.X = pos.X;
///     Position.Y = pos.Y;
/// }
/// </code>
/// </example>
/// </summary>
[PublicAPI]
public struct GPoint
{
    public int X;
    public int Y;
}

/// <summary>
/// A Simple 2D Size struct with only Width and Height members.
/// For use when no 'padding' is needed, as in method parameters, for instance.
/// <example>
/// <code>
/// public void SetSize( GSize newSize )
/// {
///     Width = newSize.Width;
///     Height = newSize.Height;
/// }
/// </code>
/// </example>
/// </summary>
[PublicAPI]
public struct GSize
{
    public int Width;
    public int Height;
}
