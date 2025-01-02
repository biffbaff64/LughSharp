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

namespace LughSharp.Lugh.Utils.Exceptions;

/// <summary>
/// Exception used for singalling asset load failures.
/// </summary>
[PublicAPI]
public class AssetNotLoadedException : ApplicationException
{
    /// <summary>
    /// Initializes a new AssetNotLoadedException with a specified error message.
    /// </summary>
    /// <param name="message"> The message that describes the error. </param>
    public AssetNotLoadedException( string? message = "" )
        : base( message )
    {
    }
    
    /// <summary>
    /// Initializes a new AssetNotLoadedException with a reference to the inner exception
    /// that is the cause of this exception.
    /// </summary>
    /// <param name="e">
    /// The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.
    /// </param>
    public AssetNotLoadedException( Exception? e )
        : this( "", e )
    {
    }

    /// <summary>
    /// Initializes a new AssetNotLoadedException with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="exception">
    /// The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.
    /// </param>`
    public AssetNotLoadedException( string message, Exception? exception )
        : base( message, exception )
    {
    }
}

