using System.Text.RegularExpressions;

using LibGDXSharp.Core;

namespace LibGDXSharp.Graphics.GLUtils
{
    /// <summary>
    /// </summary>
    public class GLVersion
    {
        private const string Tag = "GLVersion";

        public enum GLType
        {
            None,
            OpenGL,
            GLES,
            WebGL,
        }

        public int     MajorVersion   { get; set; }
        public int     MinorVersion   { get; set; }
        public int     ReleaseVersion { get; set; }
        public string? VendorString   { get; set; }
        public string? RendererString { get; set; }
        public GLType  Gltype         { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="versionString"></param>
        /// <param name="vendorString"></param>
        /// <param name="rendererString"></param>
        public GLVersion( Application.ApplicationType appType,
                          string versionString,
                          string vendorString,
                          string rendererString )
        {
            if ( appType == Application.ApplicationType.Android )
            {
                this.Gltype = GLType.GLES;
            }
            else if ( appType == Application.ApplicationType.Desktop )
            {
                this.Gltype = GLType.OpenGL;
            }
            else if ( appType == Application.ApplicationType.WebGL )
            {
                this.Gltype = GLType.WebGL;
            }
            else
            {
                this.Gltype = GLType.None;
            }

            if ( Gltype == GLType.GLES )
            {
                //OpenGL<space>ES<space><version number><space><vendor-specific information>.
                ExtractVersion( "OpenGL ES (\\d(\\.\\d){0,2})", versionString );
            }
            else if ( Gltype == GLType.WebGL )
            {
                //WebGL<space><version number><space><vendor-specific information>
                ExtractVersion( "WebGL (\\d(\\.\\d){0,2})", versionString );
            }
            else if ( Gltype == GLType.OpenGL )
            {
                //<version number><space><vendor-specific information>
                ExtractVersion( "(\\d(\\.\\d){0,2})", versionString );
            }
            else
            {
                MajorVersion   = -1;
                MinorVersion   = -1;
                ReleaseVersion = -1;
                VendorString   = "";
                RendererString = "";
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
            var rx      = new Regex( patternString );
            var matches = rx.Matches( versionString );
            
//            Pattern pattern = Pattern.compile( patternString );
//            Matcher matcher = pattern.matcher( versionString );

//            bool found = matcher.find();

            if ( matches.Count > 0 )
            {
                string   result      = matcher.group( 1 );
                string[] resultSplit = result.Split( "\\." );

                MajorVersion   = ParseInt( resultSplit[ 0 ], 2 );
                MinorVersion   = resultSplit.Length < 2 ? 0 : ParseInt( resultSplit[ 1 ], 0 );
                ReleaseVersion = resultSplit.Length < 3 ? 0 : ParseInt( resultSplit[ 2 ], 0 );
            }
            else
            {
                Gdx.App!.Log( Tag, "Invalid version string: " + versionString );

                MajorVersion   = 2;
                MinorVersion   = 0;
                ReleaseVersion = 0;
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
                Gdx.App!.Error( "LibGDXSharp GL", "Error parsing number: " + v + ", assuming: " + defaultValue );

                return defaultValue;
            }
        }

        /// <summary>
        /// Checks to see if the current GL connection version is higher, or equal to the provided test versions.
        /// </summary>
        /// <param name="testMajorVersion"> the major version to test against </param>
        /// <param name="testMinorVersion"> the minor version to test against </param>
        /// <returns> true if the current version is higher or equal to the test version </returns>
        public bool IsVersionEqualToOrHigher( int testMajorVersion, int testMinorVersion )
        {
            return MajorVersion > testMajorVersion
                   || ( MajorVersion == testMajorVersion && MinorVersion >= testMinorVersion );
        }

        /// <summary>
        /// </summary>
        /// <returns> a string with the current GL connection data </returns>
        public string DebugVersionString()
        {
            return "Type: "
                   + Gltype
                   + "\n"
                   + "Version: "
                   + MajorVersion
                   + ":"
                   + MinorVersion
                   + ":"
                   + ReleaseVersion
                   + "\n"
                   + "Vendor: "
                   + VendorString
                   + "\n"
                   + "Renderer: "
                   + RendererString;
        }
    }
}
