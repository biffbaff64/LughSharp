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

using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Exceptions;
using Platform = Corelib.LibCore.Core.Platform;

namespace Corelib.LibCore.Graphics.GLUtils;

/// <summary>
/// Wrapper for the current OpenGL Version used by this library.
/// </summary>
[PublicAPI, DebuggerDisplay( "DebugVersionString" )]
public class GLVersion
{
    public string?              VendorString   { get; set; }
    public string?              RendererString { get; set; }
    public GraphicsBackend.Type GLtype         { get; set; }

    // ========================================================================

    private int _majorVersion;
    private int _minorVersion;
    private int _revisionVersion;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// </summary>
    /// <param name="appType"></param>
    /// <param name="vendorString"></param>
    /// <param name="renderString"></param>
    public unsafe GLVersion( Platform.ApplicationType appType,
                             byte* vendorString,
                             byte* renderString )
    {
        GLtype = appType switch
        {
            Platform.ApplicationType.Android   => GraphicsBackend.Type.OpenGLES,
            Platform.ApplicationType.WindowsGL => GraphicsBackend.Type.OpenGL,
            Platform.ApplicationType.WebGL     => GraphicsBackend.Type.WebGL,

            var _ => throw new GdxRuntimeException( $"Unknown Platform ApplicationType: {appType}" ),
        };

        VendorString   = vendorString == null ? "" : Marshal.PtrToStringUTF8( ( IntPtr )vendorString );
        RendererString = renderString == null ? "" : Marshal.PtrToStringUTF8( ( IntPtr )renderString );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public (int major, int minor, int revision) Get()
    {
        ExtractVersion( @"(\d(\.\d){0,2})", Gdx.GL.glGetStringSafe( IGL.GL_VERSION ) );

        return ( _majorVersion, _minorVersion, _revisionVersion );
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
        ExtractVersion( @"(\d(\.\d){0,2})", Gdx.GL.glGetStringSafe( IGL.GL_VERSION ) );

        return ( _majorVersion > testMajorVersion )
               || ( ( _majorVersion == testMajorVersion ) && ( _minorVersion >= testMinorVersion ) );
    }

    /// <summary>
    /// Returns a string with the current GL connection data.
    /// </summary>
    public string DebugVersionString()
    {
        ExtractVersion( @"(\d(\.\d){0,2})", Gdx.GL.glGetStringSafe( IGL.GL_VERSION ) );

        return $"Type: {GLtype}\n"
               + $"Version: {_majorVersion}:{_minorVersion}\n"
               + $"Vendor: {VendorString}\n"
               + $"Renderer: {RendererString}";
    }

    // ========================================================================

    /// <summary>
    /// </summary>
    /// <param name="patternString"></param>
    /// <param name="versionString"></param>
    private void ExtractVersion( string patternString, string? versionString )
    {
        ArgumentNullException.ThrowIfNull( versionString );

        var rx = new Regex( patternString );

        var matches = rx.Matches( versionString );

        if ( matches.Count > 0 )
        {
            var resultSplit = rx.Split( "\\." );

            _majorVersion    = ParseInt( resultSplit[ 0 ], 2 );
            _minorVersion    = resultSplit.Length < 2 ? 0 : GLVersion.ParseInt( resultSplit[ 1 ], 0 );
            _revisionVersion = resultSplit.Length < 3 ? 0 : GLVersion.ParseInt( resultSplit[ 2 ], 0 );
        }
        else
        {
            Logger.Error( $"Invalid version string: {versionString}" );

            _majorVersion    = 2;
            _minorVersion    = 0;
            _revisionVersion = 0;
        }
    }

    /// <summary>
    /// Forgiving parsing of gl major, minor and release versions as
    /// some manufacturers don't adhere to spec
    /// </summary>
    private static int ParseInt( string v, int defaultValue )
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
}