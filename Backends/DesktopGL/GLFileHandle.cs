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

namespace LibGDXSharp.Backends.Desktop;

/// <summary>
/// </summary>
public class GLFileHandle : FileHandle
{
    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public GLFileHandle( string fileName, FileType type )
        : base( fileName, type )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="type"></param>
    public GLFileHandle( FileHandle file, FileType type )
//        : base( file, type )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public FileHandle Child( string name )
    {
//        if ( base.File != null )
//        {
//            if ( base.File.FullName.Length == 0 )
//            {
//                return new GLFileHandle( new FileHandle( name ), Type );
//            }
//        }

//        return new GLFileHandle( new FileHandle( name ), Type );

        throw new NotImplementedException();
    }

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public FileHandle Sibling( string name )
    {
//            if ( FileInfo != null )
//            {
//                if ( FileInfo.FullName.Length == 0 )
//                {
//                    throw new GdxRuntimeException( "Cannot get the sibling of the root." );
//                }
//            }

//            return new GLFileHandle( new File( file.getParent(), name ), type );
        throw new NotImplementedException();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public FileHandle Parent()
    {
//            File parent = file.getParentFile();
//
//            if ( parent == null )
//            {
//                if ( type == IFile.FileType.Absolute )
//                {
//                    parent = new File( "/" );
//                }
//                else
//                {
//                    parent = new File( "" );
//                }
//            }
//
//            return new GLFileHandle( parent, type );
        throw new NotImplementedException();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public FileHandle? GetFile()
    {
//            if ( Type == IFile.FileType.External )
//            {
//                return new FileInfo( GLFiles.ExternalPath + FileInfo!.FullName );
//            }

//            if ( Type == IFile.FileType.Local )
//            {
//                return new FileInfo( GLFiles.LocalPath + FileInfo!.FullName );
//            }

//        return base.File;

        throw new NotImplementedException();
    }
}