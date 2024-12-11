// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
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

namespace LughGlfw.Glfw;

[PublicAPI]
internal static class DllLoader
{
    internal static class Win32
    {
        [DllImport( "kernel32.dll", SetLastError = true )]
        internal static extern IntPtr LoadLibrary( string dllToLoad );

        [DllImport( "kernel32.dll", SetLastError = true )]
        internal static extern IntPtr GetProcAddress( IntPtr hModule, string procedureName );
    }

    // ========================================================================
    
    internal static class Unix
    {
        [DllImport( "libdl" )]
        internal static extern IntPtr dlopen( string fileName, int flags );

        [DllImport( "libdl" )]
        internal static extern IntPtr dlsym( IntPtr handle, string name );

        [DllImport( "libdl" )]
        internal static extern IntPtr dlerror();
    }

    // ========================================================================

    internal delegate IntPtr GetProcAddressDelegate( string name );

    internal static GetProcAddressDelegate GetLoadFunctionPointerDelegate( string libraryName )
    {
        var assemblyDirectory = Path.GetDirectoryName( System.AppContext.BaseDirectory );

        if ( RuntimeInformation.IsOSPlatform( OSPlatform.Windows ) )
        {
            var assemblyPathInRuntimes = Path.Combine( assemblyDirectory!,
                                                       "runtimes",
                                                       Environment.Is64BitProcess ? "win-x64" : "win-x86",
                                                       "native",
                                                       $"{libraryName}.dll" );

            var assemblyPathNextToExe = Path.Combine( assemblyDirectory!, $"{libraryName}.dll" );
            var assemblyPath          = File.Exists( assemblyPathNextToExe ) ? assemblyPathNextToExe : assemblyPathInRuntimes;

            // Discard the result, we only need it to be loaded into the process for later access
            var library = Win32.LoadLibrary( assemblyPath );

            return ( GetProcAddressDelegate )( name =>
            {
                #if DEBUG
                Console.WriteLine( $"Requested procedure name: {name}" );
                #endif
                
                return Win32.GetProcAddress( library, name );
            } );
        }

        // --------------------------------------------------------------------
        
        if ( RuntimeInformation.IsOSPlatform( OSPlatform.OSX ) )
        {
            var runtimeSuffix = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "arm64" : "x64";
            var assemblyPathInRuntimes = Path.Combine( assemblyDirectory!,
                                                       "runtimes",
                                                       $"osx-{runtimeSuffix}",
                                                       "native",
                                                       $"lib{libraryName}.dylib" );

            var assemblyPathNextToExe = Path.Combine( assemblyDirectory!, $"lib{libraryName}.dylib" );
            var assemblyPath          = File.Exists( assemblyPathNextToExe ) ? assemblyPathNextToExe : assemblyPathInRuntimes;

            // Discard the result, we only need it to be 
            // loaded into the process for later access
            var library = Unix.dlopen( assemblyPath, 2 );
            var errPtr  = Unix.dlerror();

            if ( errPtr != IntPtr.Zero )
            {
                var err = Marshal.PtrToStringAnsi( errPtr );

                throw new Exception( $"Failed to load GLFW library: {err}" );
            }

            return name => Unix.dlsym( library, name );
        }

        // --------------------------------------------------------------------

        // Assume linux
        {
            var library = Unix.dlopen( $"lib{libraryName}.so", 2 );
            var errPtr  = Unix.dlerror();

            if ( errPtr != IntPtr.Zero )
            {
                var err = Marshal.PtrToStringAnsi( errPtr );

                throw new Exception( $"Failed to load GLFW library: {err}" );
            }

            return name => Unix.dlsym( library, name );
        }
    }
    
    // ========================================================================
}