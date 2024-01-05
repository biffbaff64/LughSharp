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

using LibGDXSharp.Files;

namespace LibGDXSharp.Backends.Desktop;

/// <summary>
/// </summary>
public class DesktopGLFileHandle : FileHandle
{
    public DesktopGLFileHandle( string fileName, FileType type )
        : base( fileName, type )
    {
    }

    public DesktopGLFileHandle( FileInfo file, FileType type )
        : base( file.Name, type )
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    //TODO: Updates to Child(string) and Sibling(string) needed. I'm not sure either are correct.

    /// <summary>
    ///     Returns a handle to the child with the specified name.
    /// </summary>
    public FileHandle Child( string name )
    {
        if ( System.IO.Path.GetFullPath( FileInfo.Name ).Length == 0 )
        {
            return new DesktopGLFileHandle( new FileInfo( FileInfo.Name ), FileType );
        }

        return new DesktopGLFileHandle( new FileInfo( System.IO.Path.GetDirectoryName( FileInfo.Name ) + name ),
                                        FileType );
    }

    /// <summary>
    ///     Returns a handle to the sibling with the specified name.
    /// </summary>
    public FileHandle Sibling( string name )
    {
        if ( System.IO.Path.GetFullPath( name ).Length == 0 )
        {
            throw new GdxRuntimeException( "Cannot get the sibling of the root." );
        }

        return new FileHandle( name, FileType );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Returns the abstract pathname of this abstract pathname's parent, or null if this pathname
    ///     does not name a parent directory.
    ///     <para>
    ///         The parent of an abstract pathname consists of the pathname's prefix, if any, and each name
    ///         in the pathname's name sequence except for the last. If the name sequence is empty then the
    ///         pathname does not name a parent directory.
    ///     </para>
    /// </summary>
    public DirectoryInfo ParentFolder()
    {
        DirectoryInfo directoryInfo;

        if ( !Directory.Exists( base.Path() ) )
        {
            directoryInfo = FileType == FileType.Absolute
                ? new DirectoryInfo( "/" )
                : new DirectoryInfo( "" );
        }
        else
        {
            directoryInfo = new DirectoryInfo( System.IO.Path.GetFullPath( base.Name() ) );
        }

        return directoryInfo;
    }

    /// <summary>
    ///     Returns a FileInfo that represents this file handle. Note the returned file will
    ///     only be usable for <see cref="FileType.Absolute" /> and <see cref="FileType.External" />
    ///     file handles.
    /// </summary>
    public FileInfo File()
    {
        if ( FileType == FileType.External )
        {
            return new FileInfo( DesktopGLFiles.UserHomePath + base.Name() );
        }

        if ( FileType == FileType.Local )
        {
            return new FileInfo( DesktopGLFiles.LocalPath + base.Name() );
        }

        return FileInfo;
    }
}
