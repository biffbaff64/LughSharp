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

public interface IDataOutput
{
    void Write( byte[] data );

    void Write( byte[] data, int ofs, int len );

    void Write( int v );

    void WriteBoolean( bool v );

    void WriteByte( int v );

    void WriteBytes( string s );

    void WriteChar( int v );

    void WriteChars( string s );

    void WriteDouble( double v );

    void WriteFloat( float v );

    void WriteInt( int v );

    void WriteLong( long v );

    void WriteShort( int v );

    void WriteUTF( string s );
}
