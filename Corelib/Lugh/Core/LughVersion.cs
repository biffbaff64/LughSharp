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

using Corelib.Lugh.Graphics;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;
using Exception = System.Exception;

namespace Corelib.Lugh.Core;

/// <summary>
/// The current LughSharp Library version.
/// </summary>
/// <remarks> Class name changed from GDXVersion 07/10/2024. </remarks>
[PublicAPI]
public partial class LughVersion
{
    public int LughMajorVersion    { get; private set; }
    public int LughMinorVersion    { get; private set; }
    public int LughRevisionVersion { get; private set; }

    private readonly Version? _version;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Gets the Library Version from the Assembly.
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public LughVersion()
    {
        _version = Assembly.GetEntryAssembly()?.GetName().Version;

        if ( _version == null )
        {
            throw new GdxRuntimeException( "NULL Assembly Version!" );
        }

        try
        {
            var matches = MyRegex().Matches( _version.ToString() );
            var v       = string.Empty;

            foreach ( var match in matches )
            {
                v += match;
            }

            LughMajorVersion    = v.Length < 1 ? 0 : int.Parse( v[ ..1 ] );
            LughMinorVersion    = v.Length < 2 ? 0 : int.Parse( v[ 1.. ] );
            LughRevisionVersion = v.Length < 3 ? 0 : int.Parse( v[ 2.. ] );

            Logger.Debug( $"Current LughSharp Version : {LughMajorVersion}.{LughMinorVersion}.{LughRevisionVersion}" );
        }
        catch ( Exception e )
        {
            throw new GdxRuntimeException( $"Invalid version {_version.ToString().Split( "\\." )}", e );
        }
    }

    /// <summary>
    /// Checks the provided version components against the current and reports
    /// TRUE if the CURRENT version is GREATER than the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public bool IsHigher( int major, int minor, int revision )
    {
        return IsHigherEqual( major, minor, revision + 1 );
    }

    /// <summary>
    /// Checks the provided version components against the current and reports TRUE if
    /// the CURRENT version is GREATER than or EQUAL to the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public bool IsHigherEqual( int major, int minor, int revision )
    {
        if ( LughMajorVersion != major )
        {
            return LughMajorVersion > major;
        }

        if ( LughMinorVersion != minor )
        {
            return LughMinorVersion > minor;
        }

        return LughRevisionVersion >= revision;
    }

    /// <summary>
    /// Checks the provided version components against the current and reports TRUE if
    /// the CURRENT version is LESS than the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public bool IsLower( int major, int minor, int revision )
    {
        return IsLowerEqual( major, minor, revision - 1 );
    }

    /// <summary>
    /// Checks the provided version components against the current and reports TRUE if
    /// the CURRENT version is LESS than or EQUAL to the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public bool IsLowerEqual( int major, int minor, int revision )
    {
        if ( LughMajorVersion != major )
        {
            return LughMajorVersion < major;
        }

        if ( LughMinorVersion != minor )
        {
            return LughMinorVersion < minor;
        }

        return LughRevisionVersion <= revision;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [System.Text.RegularExpressions.GeneratedRegex( "\\d+" )]
    private static partial Regex MyRegex();
}