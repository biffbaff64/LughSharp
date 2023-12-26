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

using System.Collections.Immutable;

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class DesktopGLFiles : IFiles
{
    public readonly static string UserHomePath = Environment.GetEnvironmentVariable( "user.home" ) + Path.PathSeparator;
    public readonly static string LocalPath    = Path.GetFullPath( "" ) + Path.PathSeparator;

    /// <summary>
    /// Returns a handle representing a file or directory.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type">Determines how the path is resolved.</param>
    /// <exception cref="GdxRuntimeException">
    /// if the type is classpath or internal and the file does not exist.
    /// </exception>
    /// <seealso cref="FileType"/>
    /// 
    public FileInfo GetFileHandle( string path, FileType type ) => null!;

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Classpath"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Classpath( string path ) => null!;

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Internal"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Internal( string path ) => null!;

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.External"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo External( string path ) => null!;

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Absolute"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Absolute( string path ) => null!;

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Local"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Local( string path ) => null!;

    /// <summary>
    /// Returns the external storage path directory. This is the app
    /// external storage on Android and the home directory of the
    /// current user on the desktop.
    /// </summary>
    public string GetExternalStoragePath() => string.Empty;

    /// <summary>
    /// Returns true if the external storage is ready for file IO.
    /// </summary>
    public bool IsExternalStorageAvailable() => false;

    /// <summary>
    /// Returns the local storage path directory. This is the private
    /// files directory on Android and the directory of the jar on
    /// the desktop.
    /// </summary>
    public string GetLocalStoragePath() => string.Empty;

    /// <summary>
    /// Returns true if the local storage is ready for file IO.
    /// </summary>
    public bool IsLocalStorageAvailable() => false;
}
