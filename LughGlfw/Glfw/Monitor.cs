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
/// An opaque handle to a GLFW monitor.
/// </summary>
[PublicAPI]
public class Monitor
{
    internal readonly IntPtr Handle;

    internal Monitor( IntPtr handle ) => Handle = handle;

    /// <summary>
    /// A NULL monitor handle. Often used for default values or error handling.
    /// </summary>
    public static readonly Monitor NULL = new Monitor( IntPtr.Zero );

    /// <summary>
    /// Performs equality check against another monitor handle.
    /// </summary>
    public override bool Equals( object? other )
    {
        if ( ReferenceEquals( null, other ) ) return false;
        if ( ReferenceEquals( this, other ) ) return true;

        if ( other is Monitor monitor )
            return Handle.Equals( monitor.Handle );

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
    /// Performs equality check against another monitor handle.
    /// </summary>
    public static bool operator ==( Monitor left, Monitor right ) => left.Equals( right );

    /// <summary>
    /// Performs inequality check against another monitor handle.
    /// </summary>
    public static bool operator !=( Monitor left, Monitor right ) => !left.Equals( right );

    /// <summary>
    /// Implicit conversion to GLFWmonitor* handle that is used by Native GLFW functions.
    /// </summary>
    public static unsafe implicit operator NativeGlfw.GlfwMonitor*( Monitor managed ) => ( NativeGlfw.GlfwMonitor* )( managed?.Handle ?? NULL.Handle );
}