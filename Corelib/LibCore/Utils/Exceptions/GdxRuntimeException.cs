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

using System.Runtime.CompilerServices;
using Exception = System.Exception;
using NotNull = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Corelib.LibCore.Utils.Exceptions;

/// <summary>
/// Typed runtime exception used throughout LughSharp.
/// </summary>
[PublicAPI]
public class GdxRuntimeException : ApplicationException
{
    /// <summary>
    /// Initializes a new GdxRuntimeException with a specified error message.
    /// </summary>
    /// <param name="message"> The message that describes the error. </param>
    public GdxRuntimeException( string? message = "" )
        : base( message )
    {
    }

    /// <summary>
    /// Initializes a new GdxRuntimeException with a reference to the inner exception
    /// that is the cause of this exception.
    /// </summary>
    /// <param name="e">
    /// The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.
    /// </param>
    public GdxRuntimeException( Exception? e )
        : this( "", e )
    {
    }

    /// <summary>
    /// Initializes a new GdxRuntimeException with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="exception">
    /// The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.
    /// </param>
    public GdxRuntimeException( string message, Exception? exception )
        : base( message, exception )
    {
    }

    /// <summary>
    /// Throws an GdxRuntimeException if argument is null.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="paramName">
    /// The name of the parameter with which argument corresponds. If you omit this
    /// parameter, the name of argument is used.
    /// </param>
    public static void ThrowIfNull( [NotNull] object? argument,
                                    [CallerArgumentExpression( "argument" )]
                                    string? paramName = null )
    {
        if ( argument is null )
        {
            Throw( paramName );
        }
    }

    [DoesNotReturn]
    internal static void Throw( string? message )
    {
        throw new GdxRuntimeException( message );
    }
}
