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
///     The current LibGDXSharp Library version.
/// </summary>
[PublicAPI]
public class GDXVersion
{
    // the current version of LibGDXSharp as a string in the major.minor.revision format
    public const string LIBRARY_VERSION_STRING = "0.0.1";

    private readonly static Version Version = new();

    static GDXVersion()
    {
        try
        {
            var v = Version.ToString().Split( "\\." );

            MajorVersion    = v.Length < 1 ? 0 : int.Parse( v[ 0 ] );
            MinorVersion    = v.Length < 2 ? 0 : int.Parse( v[ 1 ] );
            RevisionVersion = v.Length < 3 ? 0 : int.Parse( v[ 2 ] );
        }
        catch ( Exception e )
        {
            throw new GdxRuntimeException( $"Invalid version {LIBRARY_VERSION_STRING}", e );
        }
    }

    protected static int MajorVersion    { get; set; }
    protected static int MinorVersion    { get; set; }
    protected static int RevisionVersion { get; set; }

    /// <summary>
    ///     Checks the provided version components against the current and reports
    ///     TRUE if the CURRENT version is GREATER than the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public static bool IsHigher( int major, int minor, int revision ) => IsHigherEqual( major, minor, revision + 1 );

    /// <summary>
    ///     Checks the provided version components against the current
    ///     and reports TRUE if the CURRENT version is GREATER than or
    ///     EQUAL to the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public static bool IsHigherEqual( int major, int minor, int revision )
    {
        if ( MajorVersion != major )
        {
            return MajorVersion > major;
        }

        if ( MinorVersion != minor )
        {
            return MinorVersion > minor;
        }

        return RevisionVersion >= revision;
    }

    /// <summary>
    ///     Checks the provided version components against the current and
    ///     reports TRUE if the CURRENT version is LESS than the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public static bool IsLower( int major, int minor, int revision ) => IsLowerEqual( major, minor, revision - 1 );

    /// <summary>
    ///     Checks the provided version components against the current
    ///     and reports TRUE if the CURRENT version is LESS than or
    ///     EQUAL to the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public static bool IsLowerEqual( int major, int minor, int revision )
    {
        if ( MajorVersion != major )
        {
            return MajorVersion < major;
        }

        if ( MinorVersion != minor )
        {
            return MinorVersion < minor;
        }

        return RevisionVersion <= revision;
    }
}
