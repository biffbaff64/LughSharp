using LibGDXSharp.Core;

namespace LibGDXSharp.Backends.Desktop
{
    /// <summary>
    /// </summary>
    public sealed class GLFileHandle : FileHandle
    {
        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        public GLFileHandle( string fileName, IFile.FileType type )
            : base( fileName, type )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        public GLFileHandle( FileInfo file, IFile.FileType type )
            : base( file, type )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FileHandle Child( string name )
        {
            if ( FileInfo != null )
            {
                if ( FileInfo.FullName.Length == 0 )
                {
                    return new GLFileHandle( new FileInfo( name ), Type );
                }
            }

            return new GLFileHandle( new FileInfo( name ), Type );
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
        public FileInfo? File()
        {
//            if ( Type == IFile.FileType.External )
//            {
//                return new FileInfo( GLFiles.ExternalPath + FileInfo!.FullName );
//            }

//            if ( Type == IFile.FileType.Local )
//            {
//                return new FileInfo( GLFiles.LocalPath + FileInfo!.FullName );
//            }

            return FileInfo;
        }
    }
}
