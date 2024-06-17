// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


using Exception = System.Exception;

namespace LughSharp.LibCore.Utils.Buffers;

[PublicAPI, Serializable]
public class CircularByteBuffer
{
    private byte[] _buffer;
    private int    _index;
    private int    _length;
    private int    _numValid;

    public CircularByteBuffer( int size )
    {
        _buffer = new byte[ size ];
        _length = size;
    }

    /// <summary>
    /// Initialize by copying the CircularByteBuffer passed in
    /// </summary>
    public CircularByteBuffer( CircularByteBuffer cdb )
    {
        lock ( cdb )
        {
            _length   = cdb._length;
            _numValid = cdb._numValid;
            _index    = cdb._index;
            _buffer   = new byte[ _length ];

            for ( var c = 0; c < _length; c++ )
            {
                _buffer[ c ] = cdb._buffer[ c ];
            }
        }
    }

    /// <summary>
    /// The physical size of the Buffer (read/write)
    /// </summary>
    public int BufferSize
    {
        get => _length;
        set
        {
            var newDataArray = new byte[ value ];

            var minLength = _length > value ? value : _length;

            for ( var i = 0; i < minLength; i++ )
            {
                newDataArray[ i ] = Get( ( i - _length ) + 1 );
            }

            _buffer = newDataArray;
            _index  = minLength - 1;
            _length = value;
        }
    }

    /// <summary>
    /// e.g. Offset[0] is the current value
    /// </summary>
    public byte this[ int index ]
    {
        get => Get( -1 - index );
        set => Set( -1 - index, value );
    }

    /// <summary>
    /// How far back it is safe to look (read/write).  Write only to reduce NumValid.
    /// </summary>
    public int NumValid
    {
        get => _numValid;
        set
        {
            if ( value > _numValid )
            {
                throw new Exception( $"Can't set NumValid to {value} which is greater"
                                   + $" than the current numValid value of {_numValid}" );
            }

            _numValid = value;
        }
    }

    public CircularByteBuffer Copy()
    {
        return new CircularByteBuffer( this );
    }

    public void Reset()
    {
        _index    = 0;
        _numValid = 0;
    }

    /// <summary>
    /// Push a byte into the buffer.  Returns the value of whatever comes off.
    /// </summary>
    public byte Push( byte newValue )
    {
        byte ret;

        lock ( this )
        {
            ret = Get( _length );

            _buffer[ _index ] = newValue;
            _numValid++;

            if ( _numValid > _length )
            {
                _numValid = _length;
            }

            _index++;
            _index %= _length;
        }

        return ret;
    }

    /// <summary>
    /// Pop an integer off the start of the buffer.
    /// </summary>
    /// <exception cref="System.Exception">if the buffer is empty (NumValid == 0)</exception>
    public byte Pop()
    {
        lock ( this )
        {
            if ( _numValid == 0 )
            {
                throw new Exception( "Can't pop off an empty CircularByteBuffer" );
            }

            _numValid--;

            return this[ _numValid ];
        }
    }

    /// <summary>
    /// Returns what would fall out of the buffer on a Push.
    /// NOT the same as what you'd get with a Pop().
    /// </summary>
    public byte Peek()
    {
        lock ( this )
        {
            return Get( _length );
        }
    }

    private byte Get( int offset )
    {
        var ind = _index + offset;

        // Do thin modulo (should just drop through)
        for ( ; ind >= _length; ind -= _length )
        {
        }

        for ( ; ind < 0; ind += _length )
        {
        }

        // Set value
        return _buffer[ ind ];
    }

    private void Set( int offset, byte valueToSet )
    {
        var ind = _index + offset;

        // Do thin modulo (should just drop through)
        for ( ; ind > _length; ind -= _length )
        {
        }

        for ( ; ind < 0; ind += _length )
        {
        }

        // Set value
        _buffer[ ind ] = valueToSet;
    }

    /// <summary>
    /// Returns a range (in terms of Offsets) in an int array in chronological
    /// (oldest-to-newest) order. e.g. (3, 0) returns the last four ints pushed,
    /// with result[3] being the most recent.
    /// </summary>
    public byte[] GetRange( int str, int stp )
    {
        var outByte = new byte[ ( str - stp ) + 1 ];

        for ( int i = str, j = 0; i >= stp; i--, j++ )
        {
            outByte[ j ] = this[ i ];
        }

        return outByte;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var ret = "";

        foreach ( var t in _buffer )
        {
            ret += t + " ";
        }

        ret += $"\n index = {_index} numValid = {NumValid}";

        return ret;
    }
}
