using LibGDXSharp.Core;

namespace LibGDXSharp.Backends.Desktop
{
    public sealed class GLFiles : IFiles
    {
        /// <summary>
        /// Returns a handle representing a file or directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type">Determines how the path is resolved.</param>
        /// <exception cref="GdxRuntimeException">
        /// if the type is classpath or internal and the file does not exist.
        /// </exception>
        /// <see cref="FileType"/>
        /// 
        public FileInfo GetFileHandle( string path, FileType type )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Classpath"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo Classpath( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Internal"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo Internal( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.External"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo External( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Absolute"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo? Absolute( string path )
        {
            return null;
        }

        /// <summary>
        /// Convenience method that returns a <see cref="FileType.Local"/> file handle.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo Local( string path )
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
}

