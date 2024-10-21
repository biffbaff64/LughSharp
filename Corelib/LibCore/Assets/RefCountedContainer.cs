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


namespace Corelib.LibCore.Assets;

/// <summary>
/// An interface that provides reference counting functionality for objects.
/// Implementations of this interface should allow for the reference count to be
/// incremented or decremented to track how many times an object is being utilized.
/// </summary>
[PublicAPI]
public interface IRefCountedContainer
{
    object? Asset    { get; }
    int     RefCount { get; set; }
}

// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------

/// <summary>
/// A class that stores a reference to an object, as well as counts the number
/// of times it has been referenced. <see cref="RefCount"/> must be incremented
/// each time you start using the object, and decrement it after you're done
/// using it. AssetManager handles this automatically.
/// </summary>
[PublicAPI]
public class RefCountedContainer( object obj ) : IRefCountedContainer
{
    public object? Asset    { get; set; } = obj ?? throw new ArgumentException( "Object must not be null" );
    public int     RefCount { get; set; } = 1;

    // ------------------------------------------------------------------------
}

// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------
