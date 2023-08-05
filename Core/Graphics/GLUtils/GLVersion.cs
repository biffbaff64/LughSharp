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

using System.Text.RegularExpressions;

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class GLVersion : LibGDXSharp.Core.GDXVersion
{
    private const string Tag = "GLVersion";

    public enum GLType
    {
        None,
        OpenGL,
        GLES,
        WebGL,
    }

    public string? VendorString   { get; set; }
    public string? RendererString { get; set; }
    public GLType  GLtype         { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="appType"></param>
    /// <param name="versionString"></param>
    /// <param name="vendorString"></param>
    /// <param name="rendererString"></param>
    public GLVersion( IApplication.ApplicationType appType,
                      string versionString,
                      string vendorString,
                      string rendererString )
    {
        this.GLtype = appType switch
                      {
                          IApplication.ApplicationType.Android => GLType.GLES,
                          IApplication.ApplicationType.Desktop => GLType.OpenGL,
                          IApplication.ApplicationType.WebGL   => GLType.WebGL,
                          _                                    => GLType.None
                      };

        if ( GLtype == GLType.GLES )
        {
            //OpenGL<space>ES<space><version number><space><vendor-specific information>.
            ExtractVersion( "OpenGL ES (\\d(\\.\\d){0,2})", versionString );
        }
        else if ( GLtype == GLType.WebGL )
        {
            //WebGL<space><version number><space><vendor-specific information>
            ExtractVersion( "WebGL (\\d(\\.\\d){0,2})", versionString );
        }
        else if ( GLtype == GLType.OpenGL )
        {
            //<version number><space><vendor-specific information>
            ExtractVersion( "(\\d(\\.\\d){0,2})", versionString );
        }
        else
        {
            MajorVersion    = -1;
            MinorVersion    = -1;
            RevisionVersion = -1;
            VendorString    = "";
            RendererString  = "";
        }

        this.VendorString   = vendorString;
        this.RendererString = rendererString;
    }

    /// <summary>
    /// </summary>
    /// <param name="patternString"></param>
    /// <param name="versionString"></param>
    private void ExtractVersion( string patternString, string versionString )
    {
        var rx = new Regex( patternString );

        MatchCollection matches = rx.Matches( versionString );

        if ( matches.Count > 0 )
        {
            var resultSplit = rx.Split( "\\." );
            
            MajorVersion    = ParseInt( resultSplit[ 0 ], 2 );
            MinorVersion    = resultSplit.Length < 2 ? 0 : ParseInt( resultSplit[ 1 ], 0 );
            RevisionVersion = resultSplit.Length < 3 ? 0 : ParseInt( resultSplit[ 2 ], 0 );
        }
        else
        {
            Gdx.App.Log( Tag, "Invalid version string: " + versionString );

            MajorVersion    = 2;
            MinorVersion    = 0;
            RevisionVersion = 0;
        }
    }

    /// <summary>
    /// Forgiving parsing of gl major, minor and release versions as
    /// some manufacturers don't adhere to spec
    /// </summary>
    private int ParseInt( string v, int defaultValue )
    {
        try
        {
            return int.Parse( v );
        }
        catch ( FormatException )
        {
            Gdx.App.Error( "LibGDXSharp GL", $"Error parsing number: {v}, assuming: {defaultValue}" );

            return defaultValue;
        }
    }

    /// <summary>
    /// Checks to see if the current GL connection version is higher, or
    /// equal to the provided test versions.
    /// </summary>
    /// <param name="testMajorVersion"> the major version to test against </param>
    /// <param name="testMinorVersion"> the minor version to test against </param>
    /// <returns> true if the current version is higher or equal to the test version </returns>
    public bool IsVersionEqualToOrHigher( int testMajorVersion, int testMinorVersion )
    {
        return ( MajorVersion > testMajorVersion )
               || ( ( MajorVersion == testMajorVersion ) && ( MinorVersion >= testMinorVersion ) );
    }

    /// <summary>
    /// </summary>
    /// <returns> a string with the current GL connection data </returns>
    public string DebugVersionString()
    {
        return $"Type: {GLtype}\n"
               + $"Version: {MajorVersion}:{MinorVersion}:{RevisionVersion}\n"
               + $"Vendor: {VendorString}\n"
               + $"Renderer: {RendererString}";
    }
}
