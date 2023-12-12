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

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;

public class RandomAccessFileStream
{
    public static FileStream CreateRandomAccessFile( string fileName, string mode )
    {
        FileStream newFile;

        if ( string.Compare( mode, "rw", StringComparison.Ordinal ) == 0 )
            newFile = new FileStream( fileName,
                                      FileMode.OpenOrCreate,
                                      FileAccess.ReadWrite );
        else if ( string.Compare( mode, "r", StringComparison.Ordinal ) == 0 )
            newFile = new FileStream( fileName, FileMode.Open, FileAccess.Read );
        else
            throw new ArgumentException();

        return newFile;
    }
}
