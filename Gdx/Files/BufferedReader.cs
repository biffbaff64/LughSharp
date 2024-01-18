// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except _reader compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to _reader writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Text;

namespace LibGDXSharp.Files;

public class BufferedReader : Reader
{
    private const int DEFAULT_BUFFER_SIZE = 8192;
        
    private readonly Reader _reader;
    private readonly char[] _buffer;
    private          int    _position;
    private          int    _limit;

    public BufferedReader( Reader reader, int bufferSize = DEFAULT_BUFFER_SIZE )
    {
        this._reader = reader;
        this._buffer = new char[ bufferSize ];
    }

    private void Fill()
    {
        _position = 0;
        _limit    = _reader.Read( _buffer );
    }

    public string? ReadLine()
    {
        var sb = new StringBuilder();

        while ( true )
        {
            if ( _position >= _limit )
            {
                Fill();
            }

            if ( _position >= _limit )
            {
                return ( sb.Length == 0 ) ? null : sb.ToString();
            }

            for ( var i = _position; i < _limit; ++i )
            {
                if ( _buffer[ i ] == '\r' )
                {
                    sb.Append( _buffer, _position, i - _position );

                    _position = i + 1;

                    if ( ( i + 1 ) < _limit )
                    {
                        if ( _buffer[ i + 1 ] == '\n' )
                        {
                            _position = i + 2;
                        }
                    }
                    else
                    {
                        Fill();

                        if ( _buffer[ _position ] == '\n' )
                        {
                            _position += 1;
                        }
                    }

                    return sb.ToString();
                }
                
                if ( _buffer[ i ] == '\n' )
                {
                    sb.Append( _buffer, _position, i - _position );
                    
                    _position = i + 1;

                    return sb.ToString();
                }
            }

            sb.Append( _buffer, _position, _limit - _position );
            
            _position = _limit;
        }
    }

    public int Read( char[] b, int offset, int length )
    {
        var count = 0;

        if ( ( _position >= _limit ) && ( length < _buffer.Length ) )
        {
            Fill();
        }

        if ( _position < _limit )
        {
            var remaining = _limit - _position;

            if ( remaining > length )
            {
                remaining = length;
            }

            Array.Copy( _buffer, _position, b, offset, remaining );

            count    += remaining;
            _position += remaining;
            offset   += remaining;
            length   -= remaining;
        }

        if ( length > 0 )
        {
            int c = _reader.Read( b, offset, length );

            if ( c == -1 )
            {
                if ( count == 0 )
                {
                    count = -1;
                }
            }
            else
            {
                count += c;
            }
        }

        return count;
    }

    public void Close()
    {
        _reader.Close();
    }
}
