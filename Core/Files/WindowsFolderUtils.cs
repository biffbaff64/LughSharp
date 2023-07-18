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

using System.Reflection;
using System.Runtime.InteropServices;

namespace LibGDXSharp.Files;

public class WindowsFolderUtils
{
    /// <summary>
    /// Get the Application Guid
    /// </summary>
    public static Guid AppGuid
    {
        get
        {
            var asm  = Assembly.GetEntryAssembly();

            if ( asm == null ) throw new GdxRuntimeException( "AppGuid cannot be null!" );
            
            var attr = ( asm.GetCustomAttributes( typeof( GuidAttribute ), true ) );

            return new Guid( ( attr[ 0 ] as GuidAttribute )!.Value );
        }
    }

    /// <summary>
    /// Get the current assembly Guid.
    /// <remarks>
    /// Note that the Assembly Guid is not necessarily the same as the Application Guid
    /// - if this code is in a DLL, the Assembly Guid will be the Guid for the DLL, not
    /// the active EXE file.
    /// </remarks>
    /// </summary>
    public static Guid AssemblyGuid
    {
        get
        {
            var asm  = Assembly.GetExecutingAssembly();
            
            if ( asm == null ) throw new GdxRuntimeException( "AssemblyGuid cannot be null!" );

            var attr = ( asm.GetCustomAttributes( typeof( GuidAttribute ), true ) );

            return new Guid( ( attr[ 0 ] as GuidAttribute )!.Value );
        }
    }

    /// <summary>
    /// Get the current user data folder
    /// </summary>
    public static string UserDataFolder
    {
        get
        {
            Guid appGuid = AppGuid;

            var folderBase = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );

            var dir = $@"{folderBase}\{appGuid.ToString( "B" ).ToUpper()}\";

            return CheckDir( dir );
        }
    }

    /// <summary>
    /// Get the current user roaming data folder
    /// </summary>
    public static string UserRoamingDataFolder
    {
        get
        {
            Guid appGuid = AppGuid;

            var folderBase = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );

            var dir = $@"{folderBase}\{appGuid.ToString( "B" ).ToUpper()}\";

            return CheckDir( dir );
        }
    }

    /// <summary>
    /// Get all users data folder
    /// </summary>
    public static string AllUsersDataFolder
    {
        get
        {
            Guid appGuid = AppGuid;

            var folderBase = Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData );

            var dir = $@"{folderBase}\{appGuid.ToString( "B" ).ToUpper()}\";

            return CheckDir( dir );
        }
    }

    /// <summary>
    /// Check the specified folder, and create if it doesn't exist.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private static string CheckDir( string dir )
    {
        if ( !Directory.Exists( dir ) )
        {
            Directory.CreateDirectory( dir );
        }

        return dir;
    }
}