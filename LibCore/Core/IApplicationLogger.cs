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


using Exception = System.Exception;

namespace LibGDXSharp.LibCore.Core;

/// <summary>
///     The ApplicationLogger provides an interface for a LibGDX Application to
///     log messages and exceptions. A default implementations is provided for
///     each backend, custom implementations can be provided and set using
///     IApplication.ApplicationLogger = ApplicationLogger
/// </summary>
[PublicAPI]
public interface IApplicationLogger
{
    /// <summary>
    ///     Logs a message with a tag.
    /// </summary>
    void Log( string tag, string message );

    /// <summary>
    ///     Logs a message and exception with a tag.
    /// </summary>
    void Log( string tag, string message, Exception exception );

    /// <summary>
    ///     Logs an error message with a tag.
    /// </summary>
    void Error( string tag, string message );

    /// <summary>
    ///     Logs an error message and exception with a tag.
    /// </summary>
    void Error( string tag, string message, Exception exception );

    /// <summary>
    ///     Logs a debug message with a tag.
    /// </summary>
    void Debug( string tag, string message );

    /// <summary>
    ///     Logs a debug message and exception with a tag.
    /// </summary>
    void Debug( string tag, string message, Exception exception );
}
