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

namespace LibGDXSharp.Core;

public class GDXVersion
{
    private readonly static Version version = new();

    // the current version of LibGDXSharp as a string in the major.minor.revision format
    public const string VersionString = "0.0.1";

    protected static int MajorVersion    { get; set; }
    protected static int MinorVersion    { get; set; }
    protected static int RevisionVersion { get; set; }

    static GDXVersion()
    {
        try
        {
            var v = version.ToString().Split( "\\." );

            MajorVersion    = v.Length < 1 ? 0 : int.Parse( v[ 0 ] );
            MinorVersion    = v.Length < 2 ? 0 : int.Parse( v[ 1 ] );
            RevisionVersion = v.Length < 3 ? 0 : int.Parse( v[ 2 ] );
        }
        catch ( Exception e )
        {
            throw new GdxRuntimeException( "Invalid version " + VersionString, e );
        }
    }

    /// <summary>
    /// Checks the provided version components against the current and reports
    /// TRUE if the CURRENT version is GREATER than the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public static bool IsHigher( int major, int minor, int revision )
    {
        return IsHigherEqual( major, minor, revision + 1 );
    }

    /// <summary>
    /// Checks the provided version components against the current
    /// and reports TRUE if the CURRENT version is GREATER than or
    /// EQUAL to the provided version.
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
    /// Checks the provided version components against the current and
    /// reports TRUE if the CURRENT version is LESS than the provided version.
    /// </summary>
    /// <param name="major">The Major version component.</param>
    /// <param name="minor">The Minor version component.</param>
    /// <param name="revision">The Revision version component.</param>
    public static bool IsLower( int major, int minor, int revision )
    {
        return IsLowerEqual( major, minor, revision - 1 );
    }

    /// <summary>
    /// Checks the provided version components against the current
    /// and reports TRUE if the CURRENT version is LESS than or
    /// EQUAL to the provided version.
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