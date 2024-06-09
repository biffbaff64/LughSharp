// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.Backends.DesktopGL.Files;

[PublicAPI]
public class DesktopGLFiles : IFiles
{
    public readonly static string UserHomePath = System.Environment.GetEnvironmentVariable( "user.home" ) + Path.PathSeparator;
    public readonly static string LocalPath    = Path.GetFullPath( "" ) + Path.PathSeparator;

    /// <summary>
    /// Returns a handle representing a file or directory.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type">Determines how the path is resolved.</param>
    /// <exception cref="GdxRuntimeException">
    /// if the type is classpath or internal and the file does not exist.
    /// </exception>
    public FileInfo GetFileHandle( string path, FileType type )
    {
        return null!;
    }

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Classpath"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Classpath( string path )
    {
        return null!;
    }

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Internal"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Internal( string path )
    {
        return null!;
    }

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.External"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo External( string path )
    {
        return null!;
    }

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Absolute"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Absolute( string path )
    {
        return null!;
    }

    /// <summary>
    /// Convenience method that returns a <see cref="FileType.Local"/> file handle.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FileInfo Local( string path )
    {
        return null!;
    }

    /// <summary>
    /// Returns the external storage path directory. This is the app
    /// external storage on Android and the home directory of the
    /// current user on the desktop.
    /// </summary>
    public string GetExternalStoragePath()
    {
        return string.Empty;
    }

    /// <summary>
    /// Returns true if the external storage is ready for file IO.
    /// </summary>
    public bool IsExternalStorageAvailable()
    {
        return false;
    }

    /// <summary>
    /// Returns the local storage path directory. This is the private
    /// files directory on Android and the directory of the jar on
    /// the desktop.
    /// </summary>
    public string GetLocalStoragePath()
    {
        return string.Empty;
    }

    /// <summary>
    /// Returns true if the local storage is ready for file IO.
    /// </summary>
    public bool IsLocalStorageAvailable()
    {
        return false;
    }
}
