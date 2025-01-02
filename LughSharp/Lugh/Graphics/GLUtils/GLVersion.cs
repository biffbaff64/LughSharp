﻿// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

using Platform = LughSharp.Lugh.Core.Platform;

namespace LughSharp.Lugh.Graphics.GLUtils;

/// <summary>
/// Wrapper for the current OpenGL Version used by this library.
/// </summary>
/// <remarks>
/// It is HIGHLY likely that this class can be removed, with some minor work elsewhere.
/// </remarks>
[PublicAPI, DebuggerDisplay( "DebugVersionString" )]
public class GLVersion
{
    public string?                     VendorString   { get; set; } = "Unknown";
    public string?                     RendererString { get; set; } = "Unknown";
    public GraphicsBackend.BackendType BackendType    { get; set; }

    // ========================================================================

    private int _majorVersion    = 0;
    private int _minorVersion    = 0;
    private int _revisionVersion = 0;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// </summary>
    /// <param name="appType"></param>
    public unsafe GLVersion( Platform.ApplicationType appType )
    {
        BackendType = appType switch
        {
            Platform.ApplicationType.Android     => GraphicsBackend.BackendType.OpenGles,
            Platform.ApplicationType.WindowsGles => GraphicsBackend.BackendType.OpenGles,
            Platform.ApplicationType.WindowsGL   => GraphicsBackend.BackendType.OpenGL,
            Platform.ApplicationType.WebGL       => GraphicsBackend.BackendType.WebGL,

            var _ => throw new GdxRuntimeException( $"Unknown Platform ApplicationType: {appType}" ),
        };

        VendorString   ??= BytePointerToString.Convert( GdxApi.Bindings.GetString( IGL.GL_VENDOR ) );
        RendererString ??= BytePointerToString.Convert( GdxApi.Bindings.GetString( IGL.GL_RENDERER ) );

        var version = BytePointerToString.Convert( GdxApi.Bindings.GetString( IGL.GL_VERSION ) );

        _majorVersion    = ( int )char.GetNumericValue( version[ 0 ] );
        _minorVersion    = ( int )char.GetNumericValue( version[ 2 ] );
        _revisionVersion = ( int )char.GetNumericValue( version[ 4 ] );

        Logger.Debug( DebugVersionString() );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public (int major, int minor, int revision) Get()
    {
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
        return ( _majorVersion > testMajorVersion )
               || ( ( _majorVersion == testMajorVersion ) && ( _minorVersion >= testMinorVersion ) );
    }

    /// <summary>
    /// Returns a string with the current GL connection data.
    /// </summary>
    public string DebugVersionString()
    {
        return $"Type: {BackendType} :: Version: {_majorVersion}.{_minorVersion}.{_revisionVersion} :: "
               + $"Vendor: {VendorString} :: Renderer: {RendererString}";
    }

    // ========================================================================
}