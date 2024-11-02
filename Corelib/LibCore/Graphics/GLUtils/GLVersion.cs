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

using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Exceptions;
using Platform = Corelib.LibCore.Core.Platform;

namespace Corelib.LibCore.Graphics.GLUtils;

//TODO: Can this be combined with LughVersion?

/// <summary>
/// </summary>
[PublicAPI]
public class GLVersion : LughVersion
{
    /// <summary>
    /// </summary>
    /// <param name="appType"></param>
    /// <param name="versionString"></param>
    /// <param name="vendorString"></param>
    /// <param name="renderString"></param>
    public unsafe GLVersion( Platform.ApplicationType appType,
                             string versionString,
                             byte* vendorString,
                             byte* renderString )
    {
        Logger.Checkpoint();

        //TODO: WHY? WHY? WHY? WHY? Don't like this, do it better!
        GLtype = appType switch
        {
            Platform.ApplicationType.Android   => GraphicsBackend.Type.OpenGLES,
            Platform.ApplicationType.WindowsGL => GraphicsBackend.Type.OpenGL,
            Platform.ApplicationType.WebGL     => GraphicsBackend.Type.WebGL,

            var _ => throw new GdxRuntimeException( $"Unknown Platform ApplicationType: {appType}" ),
        };

        VendorString   = vendorString == null ? "" : Marshal.PtrToStringUTF8( ( IntPtr )vendorString );
        RendererString = renderString == null ? "" : Marshal.PtrToStringUTF8( ( IntPtr )renderString );

        if ( GLtype == GraphicsBackend.Type.OpenGLES )
        {
            //OpenGL<space>ES<space><version number><space><vendor-specific information>.
            ExtractVersion( @"OpenGL ES (\d(\.\d){0,2})", versionString );
        }
        else if ( GLtype == GraphicsBackend.Type.WebGL )
        {
            //WebGL<space><version number><space><vendor-specific information>
            ExtractVersion( @"WebGL (\d(\.\d){0,2})", versionString );
        }
        else if ( GLtype == GraphicsBackend.Type.OpenGL )
        {
            //<version number><space><vendor-specific information>
            ExtractVersion( @"(\d(\.\d){0,2})", versionString );
        }
        else
        {
            throw new GdxRuntimeException( $"Unknown GraphicsBackend: {GLtype}" );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="patternString"></param>
    /// <param name="versionString"></param>
    private void ExtractVersion( string patternString, string versionString )
    {
        var rx = new Regex( patternString );

        var matches = rx.Matches( versionString );

        if ( matches.Count > 0 )
        {
            var resultSplit = rx.Split( "\\." );

            MajorVersion    = ParseInt( resultSplit[ 0 ], 2 );
            MinorVersion    = resultSplit.Length < 2 ? 0 : ParseInt( resultSplit[ 1 ], 0 );
            RevisionVersion = resultSplit.Length < 3 ? 0 : ParseInt( resultSplit[ 2 ], 0 );
        }
        else
        {
            Logger.Error( $"Invalid version string: {versionString}" );

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
            Logger.Error( $"Error parsing number: {v}, assuming: {defaultValue}" );

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
    /// Returns a string with the current GL connection data.
    /// </summary>
    public string DebugVersionString()
    {
        return $"Type: {GLtype}\n"
               + $"Version: {MajorVersion}:{MinorVersion}:{RevisionVersion}\n"
               + $"Vendor: {VendorString}\n"
               + $"Renderer: {RendererString}";
    }
}