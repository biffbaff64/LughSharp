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


namespace LughSharp.Backends.DesktopGL.Files;

//TODO: This file can be removed

/// <summary>
/// </summary>
public class DesktopGLFileHandle // : FileInfo
{
    public DesktopGLFileHandle( string fileName, FileType type )
//        : base( fileName, type )
    {
    }

    public DesktopGLFileHandle( FileInfo file, FileType type )
//        : base( file.Name, type )
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    //TODO: Updates to Child(string) and Sibling(string) needed. I'm not sure either are correct.

    /// <summary>
    ///     Returns a handle to the child with the specified name.
    /// </summary>
    public FileInfo Child( string name )
    {
//        if ( Path.GetFullPath( FileInfo.Name ).Length == 0 )
//        {
//            return new DesktopGLFileHandle( new FileInfo( FileInfo.Name ), FileType );
//        }

//        return new DesktopGLFileHandle( new FileInfo( Path.GetDirectoryName( FileInfo.Name ) + name ),
//                                        FileType );

        throw new NotImplementedException();
    }

    /// <summary>
    ///     Returns a handle to the sibling with the specified name.
    /// </summary>
    public FileInfo Sibling( string name )
    {
//        if ( Path.GetFullPath( name ).Length == 0 )
//        {
//            throw new GdxRuntimeException( "Cannot get the sibling of the root." );
//        }

//        return new FileInfo( name, FileType );
        
        throw new NotImplementedException();
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
//        DirectoryInfo directoryInfo;

//        if ( !Directory.Exists( base.Path() ) )
//        {
//            directoryInfo = FileType == FileType.Absolute
//                ? new DirectoryInfo( "/" )
//                : new DirectoryInfo( "" );
//        }
//        else
//        {
//            directoryInfo = new DirectoryInfo( Path.GetFullPath( base.Name() ) );
//        }

//        return directoryInfo;

        throw new NotImplementedException();
    }

    /// <summary>
    ///     Returns a FileInfo that represents this file handle. Note the returned file will
    ///     only be usable for <see cref="FileType.Absolute" /> and <see cref="FileType.External" />
    ///     file handles.
    /// </summary>
    public FileInfo File()
    {
//        if ( FileType == FileType.External )
//        {
//            return new FileInfo( DesktopGLFiles.UserHomePath + base.Name() );
//        }

//        if ( FileType == FileType.Local )
//        {
//            return new FileInfo( DesktopGLFiles.LocalPath + base.Name() );
//        }

//        return FileInfo;
        
        throw new NotImplementedException();
    }
}
