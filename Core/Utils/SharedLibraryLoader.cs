// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace LibGDXSharp.Utils;

[PublicAPI]
public class SharedLibraryLoader
{
    public static bool IsWindows { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.Windows );
    public static bool IsLinux   { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.Linux );
    public static bool IsMac     { get; private set; } = RuntimeInformation.IsOSPlatform( OSPlatform.OSX );
    public static bool IsIos     { get; private set; } = false; //TODO:
    public static bool IsAndroid { get; private set; } = false; //TODO:
    public static bool IsARM     { get; private set; } = SystemHelpers.IsArmArchitecture();
    public static bool Is64Bit   { get; private set; } = System.Environment.Is64BitOperatingSystem;
}

[PublicAPI]
public static class SystemHelpers
{
    public static bool IsArmArchitecture()
    {
        return RuntimeInformation.OSArchitecture switch
               {
                   System.Runtime.InteropServices.Architecture.Arm   => true,
                   System.Runtime.InteropServices.Architecture.Arm64 => true,
                   _                                                 => false
               };
    }

    public static String RandomUUID()
    {
        return Guid.NewGuid().ToString();
    }
}
