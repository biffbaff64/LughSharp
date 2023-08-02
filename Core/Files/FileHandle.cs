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

[SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
public class FileHandle
{
    [SuppressMessage( "ReSharper", "UnusedType.Local" )]
    private enum PathStatus
    {
        Invalid,
        Checked
    };

    public string   FileName { get; set; }
    public File?    File     { get; set; }
    public FileType FileType { get; set; }

    protected FileHandle() : this( "" )
    {
    }

    public FileHandle( File file ) : this( "" )
    {
        this.File = file;
    }

    public FileHandle( string fileName, FileType type = FileType.Absolute )
    {
        this.FileName = fileName;
        this.FileType = type;
    }

    /// <summary>
    /// Convenience property for the path of the file.
    /// </summary>
    public string Path => System.IO.Path.GetFullPath( FileName );
}