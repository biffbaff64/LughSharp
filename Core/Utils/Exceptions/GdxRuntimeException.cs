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

using NotNull = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace LibGDXSharp.Utils;

/// <summary>
/// Typed runtime exception used throughout LibGDXSharp.
/// </summary>
[PublicAPI]
public class GdxRuntimeException : Exception
{
    public GdxRuntimeException( string message ) : base( message )
    {
    }

    public GdxRuntimeException( Exception e ) : this( "", e )
    {
    }

    public GdxRuntimeException( string message, Exception? exception )
        : base( message, exception )
    {
    }

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
    internal static void Throw( string? paramName ) => throw new GdxRuntimeException( paramName );
}
