// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Utils.Exceptions;
using Environment = System.Environment;

namespace Corelib.LibCore.Core;

/// <summary>
/// Platform specific flags and methods.
/// </summary>
[PublicAPI]
public static class Platform
{
    /// <summary>
    /// Target application backends.
    /// </summary>
    [PublicAPI]
    public enum ApplicationType : int
    {
        /// <summary>
        /// LughSharp Windows32 Platform
        /// </summary>
        Windows,

        /// <summary>
        /// LughSharp Windows Cross Platform Opengl Platform
        /// </summary>
        WindowsGL,

        /// <summary>
        /// LughSharp Windows Vulkan Platform
        /// </summary>
        WindowsVK,

        /// <summary>
        /// LughSharp Windows Game Development Kit Platform
        /// </summary>
        WindowsGDK,

        /// <summary>
        /// LughSharp Windows32 Platform
        /// </summary>
        WebGL,

        /// <summary>
        /// LughSharp XBox One Platform
        /// </summary>
        XBoxOne,

        /// <summary>
        /// LughSharp XBox Series Platform
        /// </summary>
        XBoxSeries,

        /// <summary>
        /// LughSharp Nintendo Switch Platform
        /// </summary>
        NintendoSwitch,

        /// <summary>
        /// LughSharp PlayStation 4 Platform
        /// </summary>
        PlayStation4,

        /// <summary>
        /// LughSharp PlayStation 5 Platform
        /// </summary>
        PlayStation5,

        /// <summary>
        /// LughSharp Android Platform
        /// </summary>
        Android,

        /// <summary>
        /// LughSharp Apple iOS Platform
        /// </summary>
        iOS,

        /// <summary>
        /// LughSharp Mac OS Platform
        /// </summary>
        MacOS,

        Default = WindowsGL
    }

    /// <summary>
    /// Application type family groups
    /// </summary>
    public enum Family : int
    {
        Unknown,
        Mobile,  // Android, IOS
        Desktop, // WindowsGL, UWP, WebGL, Linux, MacOS
        Console, // XBox
    }

    // ------------------------------------------------------------------------

    public static bool IsWindows { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.Windows );
    public static bool Is64Bit   { get; private set; } = Environment.Is64BitOperatingSystem;
    public static bool IsLinux   { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.Linux );
    public static bool IsMac     { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.OSX );
    public static bool IsARM     { get; private set; } = IsArmArchitecture();

    // ------------------------------------------------------------------------

    public static bool IsIos     { get; private set; } = false; //TODO: For the future, concentrating on desktop for now. 
    public static bool IsAndroid { get; private set; } = false; //TODO: For the future, concentrating on desktop for now.
    public static bool IsXBox    { get; private set; } = false; //TODO: For the future, concentrating on desktop for now.

    // ------------------------------------------------------------------------

    private static ApplicationType _targetPlatform;
    private static Family          _familyGroup;

    // ------------------------------------------------------------------------

    /// <summary>
    /// The target platform for the app.
    /// Must be one of the enum <see cref="ApplicationType"/>
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public static ApplicationType TargetPlatform
    {
        get => _targetPlatform;
        set
            => _targetPlatform = value switch
            {
                // ----------------------------------------
                ApplicationType.Windows
                    or ApplicationType.WindowsGL
                    or ApplicationType.WindowsVK
                    or ApplicationType.WindowsGDK
                    or ApplicationType.WebGL
                    or ApplicationType.XBoxOne
                    or ApplicationType.XBoxSeries
                    or ApplicationType.NintendoSwitch
                    or ApplicationType.PlayStation4
                    or ApplicationType.PlayStation5
                    or ApplicationType.Android
                    or ApplicationType.iOS
                    or ApplicationType.MacOS => value,

                // ----------------------------------------
                var _ => throw new GdxRuntimeException( $"Illegal Target Platform: {value.ToString()}" )
            };
    }

    /// <summary>
    /// The target family group (mobile, console, desktop etc).
    /// Must be one of the enum <see cref="Family"/>.
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public static Family FamilyGroup
    {
        get => _familyGroup;
        set
            => _familyGroup = value switch
            {
                // ----------------------------------------
                Family.Console
                    or Family.Desktop
                    or Family.Mobile => value,

                // ----------------------------------------
                Family.Unknown => throw new GdxRuntimeException( $"Illegal Family Group: {value.ToString()}" ),
                var _          => throw new GdxRuntimeException( $"Illegal Family Group: {value.ToString()}" )
            };
    }

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
    public static string RandomUUID() => Guid.NewGuid().ToString();
}