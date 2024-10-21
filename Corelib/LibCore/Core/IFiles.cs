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

using Corelib.LibCore.Files;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Core;

[PublicAPI]
public interface IFiles
{
    /// <summary>
    /// Returns a handle representing a file or directory.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type">Determines how the path is resolved.</param>
    /// <exception cref="GdxRuntimeException">
    /// if the type is classpath or internal and the file does not exist.
    /// </exception>
    FileHandle GetFileHandle( string path, PathTypes type );

    /// <inheritdoc cref="PathTypes.Classpath"/>
    FileHandle Classpath( string path );

    /// <inheritdoc cref="PathTypes.Internal"/>
    FileHandle Internal( string path );

    /// <inheritdoc cref="PathTypes.External"/>
    FileHandle External( string path );

    /// <inheritdoc cref="PathTypes.Absolute"/>
    FileHandle Absolute( string path );

    /// <inheritdoc cref="PathTypes.Local"/>
    FileHandle Local( string path );

    /// <summary>
    /// Returns the external storage path directory. This is the app external storage
    /// on Android and the home directory of the current user on the desktop.
    /// </summary>
    string GetExternalStoragePath();

    /// <summary>
    /// Returns true if the external storage is ready for file IO.
    /// </summary>
    bool IsExternalStorageAvailable();

    /// <summary>
    /// Returns the local storage path directory. This is the private
    /// files directory on Android and the directory of the jar on
    /// the desktop.
    /// </summary>
    string GetLocalStoragePath();

    /// <summary>
    /// Returns true if the local storage is ready for file IO.
    /// </summary>
    bool IsLocalStorageAvailable();
}

[PublicAPI]
public enum PathTypes
{
    /// <summary>
    /// Path relative to the root of the classpath. Classpath files are always readonly.
    /// Note that classpath files are not compatible with some functionality on Android,
    /// such as Audio#newSound(FileInfo) and Audio#newMusic(FileInfo).
    /// </summary>
    Classpath,

    /// <summary>
    /// Path relative to the asset directory on Android and to the application's root
    /// directory on the desktop. On the desktop, if the file is not found, then the
    /// classpath is checked.
    /// Internal files are always readonly.
    /// </summary>
    Internal,

    /// <summary>
    /// Path relative to the root of the app external storage on Android and
    /// to the home directory of the current user on the desktop.
    /// </summary>
    External,

    /// <summary>
    /// Path that is a fully qualified, absolute filesystem path. To ensure
    /// portability across platforms use absolute files only when absolutely
    /// necessary.
    /// </summary>
    Absolute,

    /// <summary>
    /// Path relative to the private files directory on Android and to the
    /// application's root directory on the desktop.
    /// </summary>
    Local
}
