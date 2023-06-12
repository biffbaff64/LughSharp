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