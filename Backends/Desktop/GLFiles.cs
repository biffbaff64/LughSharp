using LibGDXSharp.Core;

namespace LibGDXSharp.Backends.Desktop
{
    public class GLFiles : IFiles
    {
        /// <summary>
        /// Returns a handle representing a file or directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type">Determines how the path is resolved.</param>
        /// <exception cref="GdxRuntimeException">
        /// if the type is classpath or internal and the file does not exist.
        /// </exception>
        /// <seealso cref="IFiles.FileType"/>
        /// 
        public FileHandle GetFileHandle( string path, IFiles.FileType type )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="IFiles.FileType.Classpath"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileHandle Classpath( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="IFiles.FileType.Internal"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileHandle Internal( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="IFiles.FileType.External"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileHandle External( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="IFiles.FileType.Absolute"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileHandle? Absolute( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="IFiles.FileType.Local"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileHandle Local( string path )
        {
            return null;
        }

        /// <summary>
        /// Returns the external storage path directory. This is the app
        /// external storage on Android and the home directory of the
        /// current user on the desktop.
        /// </summary>
        public string GetExternalStoragePath()
        {
            return null;
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
            return null;
        }

        /// <summary>
        /// Returns true if the local storage is ready for file IO.
        /// </summary>
        public bool IsLocalStorageAvailable()
        {
            return false;
        }
    }
}

