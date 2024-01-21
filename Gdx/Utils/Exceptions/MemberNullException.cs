// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Runtime.CompilerServices;

namespace LibGDXSharp.Utils;

[PublicAPI]
public class MemberNullException : Exception
{
    public MemberNullException()
    {
    }

    public MemberNullException( string? message = "" )
        : base( message )
    {
    }

    public MemberNullException( Exception? innerException )
        : this( "", innerException )
    {
    }

    public MemberNullException( string message, Exception? exception )
        : base( message, exception )
    {
    }

    /// <summary>
    ///     Throws an <see cref="MemberNullException" /> if <paramref name="argument" /> is null.
    /// </summary>
    /// <param name="argument">
    ///     The reference type argument to validate as non-null.
    /// </param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="argument" /> corresponds.
    /// </param>
    public static void ThrowIfNull( [System.Diagnostics.CodeAnalysis.NotNull] object? argument,
                                    [CallerArgumentExpression( "argument" )]
                                    string? paramName = null )
    {
        if ( argument is null )
        {
            Throw( paramName );
        }
    }

    [DoesNotReturn]
    internal static void Throw( string? paramName ) => throw new MemberNullException( paramName );
}
