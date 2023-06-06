using LibGDXSharp.Core;

namespace LibGDXSharp.Files;

/// <summary>
/// A FileHandle intended to be subclassed for the purpose of implementing Read()
/// and/or Write(bool). Methods that would manipulate the file instead throw
/// UnsupportedOperationException.
/// </summary>
public class FileHandleStream : FileHandle
{
    /// <summary>
    /// </summary>
    /// <param name="path"></param>
    public FileHandleStream( string path ) : base( path, FileType.Absolute )
    {
    }

    public     bool IsDirectory() => false;
    public new long Length()      => 0;
    public     bool Exists()      => true;

    public FileHandle Child( string name )
    {
        throw new NotSupportedException();
    }

    public FileHandle Sibling( string name )
    {
        throw new NotSupportedException();
    }

    public FileHandle Parent()
    {
        throw new NotSupportedException();
    }

    public new StreamReader Read()
    {
        throw new NotSupportedException();
    }

    public new StreamWriter Write( bool overwrite )
    {
        throw new NotSupportedException();
    }

    public FileHandle[] List()
    {
        throw new NotSupportedException();
    }

    public void Mkdirs()
    {
        throw new NotSupportedException();
    }

    public bool Delete()
    {
        throw new NotSupportedException();
    }

    public bool DeleteDirectory()
    {
        throw new NotSupportedException();
    }

    public void CopyTo( FileHandle dest )
    {
        throw new NotSupportedException();
    }

    public void MoveTo( FileHandle dest )
    {
        throw new NotSupportedException();
    }
}