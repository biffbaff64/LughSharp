// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;
using Environment = System.Environment;

namespace LughSharp.LibCore.Core;

/// <summary>
/// Platform specific flags and methods.
/// </summary>
[PublicAPI]
public static class Platform
{
    public static bool IsWindows { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.Windows );
    public static bool Is64Bit   { get; private set; } = Environment.Is64BitOperatingSystem;
    public static bool IsLinux   { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.Linux );
    public static bool IsMac     { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.OSX );
    public static bool IsARM     { get; private set; } = IsArmArchitecture();

    // ------------------------------------------------------------------------

    public static bool IsIos     { get; private set; } = false; //TODO: For the future, concentrating on desktop for now. 
    public static bool IsAndroid { get; private set; } = false; //TODO: For the future, concentrating on desktop for now.

    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns TRUE if the OS architecture is ARM based.
    /// </summary>
    public static bool IsArmArchitecture()
    {
        return RuntimeInformation.OSArchitecture switch
        {
            Architecture.Arm   => true,
            Architecture.Arm64 => true,
            var _              => false
        };
    }

    /// <summary>
    /// Returns a string representation of a new GUID structure.
    /// </summary>
    public static string RandomUUID()
    {
        return Guid.NewGuid().ToString();
    }
}
