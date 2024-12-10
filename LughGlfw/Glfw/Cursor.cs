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

namespace LughGlfw.Glfw;

/// <summary>
/// An opaque handle to a GLFW cursor.
/// </summary>
[PublicAPI]
public class Cursor
{
    internal readonly IntPtr Handle;

    internal Cursor( IntPtr handle ) => Handle = handle;

    /// <summary>
    /// A NULL cursor handle. Often used for default values or error handling.
    /// </summary>
    public static readonly Cursor NULL = new( IntPtr.Zero );

    /// <summary>
    /// Performs equality check against another cursor handle.
    /// </summary>
    public override bool Equals( object? other )
    {
        if ( ReferenceEquals( null, other ) ) return false;
        if ( ReferenceEquals( this, other ) ) return true;

        if ( other is Cursor cursor )
        {
            return Handle.Equals( cursor.Handle );
        }

        return false;
    }

    /// <summary>
    /// Simple hash code implementation that uses the underlying pointer.
    /// </summary>
    public override int GetHashCode() => Handle.GetHashCode();

    /// <summary>
    /// Returns the underlying pointer.
    /// </summary>
    public IntPtr GetHandle() => Handle;

    /// <summary>
    /// Performs equality check against another cursor handle.
    /// </summary>
    public static bool operator ==( Cursor left, Cursor right ) => left.Equals( right );

    /// <summary>
    /// Performs inequality check against another cursor handle.
    /// </summary>
    public static bool operator !=( Cursor left, Cursor right ) => !left.Equals( right );

    /// <summary>
    /// Implicit conversion to GLFWcursor* handle that is used by Native GLFW functions.
    /// </summary>
    public static unsafe implicit operator NativeGlfw.GlfwCursor*( Cursor managed )
        => ( NativeGlfw.GlfwCursor* )( managed?.Handle ?? NULL.Handle );
}