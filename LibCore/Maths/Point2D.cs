// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

namespace LughSharp.LibCore.Maths;


/// <summary>
/// Simple struct with just X and Y members, and absolutely nothing else.
/// For use when no 'padding' is needed, as in method parameters, for instance.
/// <example>
/// <code>
///    public int X { get; set; }
///    public int Y { get; set; }
///        
///    internal void SetPosition( Point2D&lt;int&gt; pos )
///    {
///        X = pos.X;
///        Y = pos.Y;
///    }
/// </code>
/// </example>
/// </summary>
[PublicAPI]
public struct Point2D< T > //TODO: replace the others with this
{
    public T X;
    public T Y;
}

/// <summary>
/// Simple struct with just X and Y members, and absolutely nothing else.
/// For use when no 'padding' is needed, as in method parameters, for instance.
/// </summary>
[PublicAPI]
public struct Point2D
{
    public int X;
    public int Y;
}

/// <summary>
/// Simple struct with just X and Y members, and absolutely nothing else.
/// For use when no 'padding' is needed, as in method parameters, for instance.
/// </summary>
[PublicAPI]
public struct Point2Df
{
    public float X;
    public float Y;
}
