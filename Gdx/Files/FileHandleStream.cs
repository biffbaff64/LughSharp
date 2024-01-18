// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Files;

/// <summary>
/// A FileHandle intended to be subclassed for the purpose of implementing <see cref="Read()"/>
/// and/or <see cref="Write(bool)"/>. Methods that would manipulate the file instead throw
/// UnsupportedOperationException.
/// </summary>
[PublicAPI]
public abstract class FileHandleStream : FileHandle
{
    /// <summary>
    /// Create a <see cref="FileType.Absolute"/> file at the given location.
    /// </summary>
    protected FileHandleStream( string path ) : base( path, FileType.Absolute )
    {
    }

    public override FileStream Read()
    {
        throw new UnsupportedOperationException();
    }

    public override FileStream Write( bool overwrite )
    {
        throw new UnsupportedOperationException();
    }

    public bool ISDirectory()
    {
        return false;
    }

    public override long Length()
    {
        return 0;
    }

    public bool Exists()
    {
        return true;
    }

    public FileHandle Child( string name )
    {
        throw new UnsupportedOperationException();
    }

    public FileHandle Sibling( string name )
    {
        throw new UnsupportedOperationException();
    }

    public FileHandle Parent()
    {
        throw new UnsupportedOperationException();
    }

    public FileHandle[] List()
    {
        throw new UnsupportedOperationException();
    }

    public void Mkdirs()
    {
        throw new UnsupportedOperationException();
    }

    public bool Delete()
    {
        throw new UnsupportedOperationException();
    }

    public bool DeleteDirectory()
    {
        throw new UnsupportedOperationException();
    }

    public void CopyTo( FileHandle dest )
    {
        throw new UnsupportedOperationException();
    }

    public void MoveTo( FileHandle dest )
    {
        throw new UnsupportedOperationException();
    }
}
