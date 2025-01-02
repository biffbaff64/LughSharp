// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace LughSharp.Lugh.Utils;

/// <summary>
/// A <c>Readable</c> is a source of characters. Characters from a <c>Readable</c>
/// are made available to callers of the read method via a char buffer.
/// </summary>
[PublicAPI]
public interface IReadable
{
    /// <summary>
    /// Attempts to read characters into the specified character buffer.
    /// The buffer is used as a repository of characters as-is: the only
    /// changes made are the results of a put operation. No flipping or
    /// rewinding of the buffer is performed.
    /// </summary>
    /// <param name="cb"> the buffer to read characters into </param>
    /// <returns>
    /// The number of <tt>char</tt> values added to the buffer,
    /// or -1 if this source of characters is at its end
    /// </returns>
    int Read( char[] cb );
}
