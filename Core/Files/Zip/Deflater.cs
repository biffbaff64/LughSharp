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

namespace LibGDXSharp.Files.Zip;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class Deflater
{
    public const int Default_Compression = 0;

    private readonly ZStreamRef _zsRef;

    private int    _compressionLevel = 0;
    private byte[] _buf              = new byte[ 0 ];
    private int    _off;
    private int    _len;
    private int    _level;
    private int    _strategy;
    private bool   _setParams;
    private bool   _finish;
    private bool   _finished;
    private long   _bytesRead;
    private long   _bytesWritten;

    public int CompressionLevel
    {
        private get => _compressionLevel;
        set
        {
            if ( ( ( _compressionLevel < 0 ) || ( _compressionLevel > 9 ) )
                 && ( _compressionLevel != Default_Compression ) )
            {
                throw new ArgumentException( "invalid compression level" );
            }

            lock ( _zsRef )
            {
                if ( this._compressionLevel != value )
                {
                    this._compressionLevel = value;
                    _setParams             = true;
                }
            }
        }
    }
}
