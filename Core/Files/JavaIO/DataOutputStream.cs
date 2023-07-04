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

namespace LibGDXSharp.Files;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class DataOutputStream : IDataOutput, ICloseable
{
    public DataOutputStream( object getOutputStream )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Writes to the output stream the eight low-order bits of the argument <tt>b</tt>.
    /// The 24 high-order bits of <tt>b</tt> are ignored.
    /// </summary>
    /// <param name="b"> the byte to be written. </param>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public void Write( int b )
    {
    }

    /// <summary>
    /// Writes to the output stream all the bytes in array <tt>b</tt>.
    /// If <tt>b</tt> is <tt>null</tt>, a <tt>NullReferenceException</tt> is thrown.
    /// If <tt>b.length</tt> is zero, then no bytes are written. Otherwise, the byte
    /// <tt>b[0]</tt> is written first, then <tt>b[1]</tt>, and so on; the last byte
    /// written is <tt>b[b.length-1]</tt>.
    /// </summary>
    /// <param name="b"> the data. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    public void Write( byte[] b )
    {
    }

    /// <summary>
    /// Writes <tt>len</tt> bytes from array <tt>b</tt>, in order, to the output stream.
    /// If <tt>b</tt> is <tt>null</tt>, a <tt>NullPointerException</tt> is thrown. If
    /// <tt>off</tt> is negative, or <tt>len</tt> is negative, or <tt>off+len</tt> is
    /// greater than the length of the array <tt>b</tt>, then an <tt>IndexOutOfRangeException</tt>
    /// is thrown.  If <tt>len</tt> is zero, then no bytes are written. Otherwise, the
    /// byte <tt>b[off]</tt> is written first, then <tt>b[off+1]</tt>, and so on; the
    /// last byte written is <tt>b[off+len-1]</tt>.
    /// </summary>
    /// <param name="b"> the data. </param>
    /// <param name="off"> the start offset in the data. </param>
    /// <param name="len"> the number of bytes to write. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    public void Write( byte[] b, int off, int len )
    {
    }

    public void WriteBoolean( bool isPeripheralAvailable )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Writes to the output stream the eight low- order bits of the argument <tt>v</tt>.
    /// The 24 high-order bits of <tt>v</tt> are ignored. (This means  that <tt>writeByte</tt>
    /// does exactly the same thing as <tt>write</tt> for an integer argument.) The byte written
    /// by this method may be read by the <tt>readByte</tt> method of interface <tt>DataInput</tt>,
    /// which will then return a <tt>byte</tt> equal to <tt>(byte)v</tt>.
    /// </summary>
    /// <param name="v"> the byte value to be written. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    public void WriteByte( int v )
    {
    }

    /// <summary>
    /// Writes two bytes to the output stream to represent the value of the argument.
    /// The byte values to be written, in the order shown, are:
    /// <para><tt>(byte)(0xff $amp; (v >> 8))</tt></para>
    /// <para><tt>(byte)(0xff $amp; v)</tt></para>
    /// <para>
    /// The bytes written by this method may be read by the <tt>ReadShort</tt> method
    /// of interface <tt>IDataInput</tt>, which will then return a <tt>short</tt> equal
    /// to <tt>(short)v</tt>.
    /// </para>
    /// </summary>
    /// <param name="v"> the <tt>short</tt> value to be written. </param>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public void WriteShort( int v )
    {
    }

    /// <summary>
    /// Writes a <tt>char</tt> value, which is comprised of two bytes, to the
    /// output stream. The byte values to be written, in the  order shown, are:
    /// <para><tt>(byte)(0xff $amp; (v >> 8))</tt></para>
    /// <para><tt>(byte)(0xff $amp; v)</tt></para>
    /// <para>
    /// The bytes written by this method may be read by the <tt>readChar</tt> method
    /// of interface <tt>DataInput</tt> , which will then return a <tt>char</tt> equal
    /// to <tt>(char)v</tt>.
    /// </para>
    /// </summary>
    /// <param name="v"> the <tt>char</tt> value to be written. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    public void WriteChar( int v )
    {
    }

    public void WriteInt( int accel )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Writes a <tt>long</tt> value, which is comprised of eight bytes, to the output stream.
    /// The byte values to be written, in the  order shown, are:
    /// <para><tt>(byte)(0xff & (v >> 56))</tt></para>
    /// <para><tt>(byte)(0xff & (v >> 48))</tt></para>
    /// <para><tt>(byte)(0xff & (v >> 40))</tt></para>
    /// <para><tt>(byte)(0xff & (v >> 32))</tt></para>
    /// <para><tt>(byte)(0xff & (v >> 24))</tt></para>
    /// <para><tt>(byte)(0xff & (v >> 16))</tt></para>
    /// <para><tt>(byte)(0xff & (v >>  8))</tt></para>
    /// <para><tt>(byte)(0xff & v)</tt></para>
    /// <para>
    /// The bytes written by this method may be read by the <tt>readLong</tt> method
    /// of interface <tt>DataInput</tt> , which will then return a <tt>long</tt> equal
    /// to <tt>v</tt>.
    /// </para>
    /// </summary>
    /// <param name="v"> the <tt>long</tt> value to be written. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    public void WriteLong( long v )
    {
    }

    /// <summary>
    /// Writes a <tt>float</tt> value, which is comprised of four bytes, to the output stream.
    /// It does this as if it first converts this <tt>float</tt> value to an <tt>int</tt>
    /// in exactly the manner of the <tt>Float.floatToIntBits</tt> method and then writes
    /// the <tt>int</tt> value in exactly the manner of the <tt>writeInt</tt> method. The
    /// bytes written by this method may be read by the <tt>ReadFloat</tt> method of interface
    /// <tt>IDataInput</tt>, which will then return a <tt>float</tt> equal to <tt>v</tt>.
    /// </summary>
    /// <param name="v"> the <tt>float</tt> value to be written. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    public void WriteFloat( float v )
    {
    }

    /// <summary>
    /// Writes a <tt>double</tt> value, which is comprised of eight bytes, to the output stream.
    /// It does this as if it first converts this <tt>double</tt> value to a <tt>long</tt>
    /// in exactly the manner of the <tt>Double.doubleToLongBits</tt> method and then writes
    /// the <tt>long</tt> value in exactly the manner of the <tt>writeLong</tt> method. The
    /// bytes written by this method may be read by the <tt>readDouble</tt> method of interface
    /// <tt>DataInput</tt>, which will then return a <tt>double</tt> equal to <tt>v</tt>.
    /// </summary>
    /// <param name="v"> the <tt>double</tt> value to be written. </param>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public void WriteDouble( double v )
    {
    }

    /// <summary>
    /// Writes a string to the output stream. For every character in the string
    /// <tt>s</tt>, taken in order, one byte is written to the output stream. If
    /// <tt>s</tt> is <tt>null</tt>, a <tt>NullReferenceException</tt> is thrown.
    /// <para>
    /// If <tt>s.length</tt> is zero, then no bytes are written. Otherwise, the
    /// character <tt>s[0]</tt> is written first, then <tt>s[1]</tt>, and so on;
    /// the last character written is <tt>s[s.length-1]</tt>. For each character,
    /// one byte is written, the low-order byte, in exactly the manner of the
    /// <tt>writeByte</tt> method. The high-order eight bits of each character
    /// in the string are ignored.
    /// </para>
    /// </summary>
    /// <param name="s"> the string of bytes to be written. </param>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public void WriteBytes( string s )
    {
    }

    /// <summary>
    /// Writes every character in the string <tt>s</tt>, to the output stream, in order,
    /// two bytes per character. If <tt>s</tt> is <tt>null</tt>, a <tt>NullReferenceException</tt>
    /// is thrown. If <tt>s.length</tt> is zero, then no characters are written. Otherwise,
    /// the character <tt>s[0]</tt> is written first, then <tt>s[1]</tt>, and so on; the last
    /// character written is <tt>s[s.length-1]</tt>. For each character, two bytes are actually
    /// written, high-order byte first, in exactly the manner of the <tt>writeChar</tt> method.
    /// </summary>
    /// <param name="s"> the string value to be written. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    public void WriteChars( string s )
    {
    }

    /// <summary>
    /// Writes two bytes of length information to the output stream, followed by the
    /// <a href="DataInput.html#modified-utf-8">modified UTF-8</a> representation
    /// of every character in the string <tt>s</tt>. If <tt>s</tt> is <tt>null</tt>,
    /// a <tt>NullPointerException</tt> is thrown.
    /// <para>
    /// Each character in the string <tt>s</tt> is converted to a group of one, two, or
    /// three bytes, depending on the value of the character.
    /// </para>
    /// <para>
    /// If a character <tt>c</tt> is in the range <tt>&#92;u0001</tt> through
    /// <tt>&#92;u007f</tt>, it is represented by one byte.
    /// </para>
    /// <para>
    /// If a character <tt>c</tt> is <tt>&#92;u0000</tt> or is in the range <tt>&#92;u0080</tt>
    /// through <tt>&#92;u07ff</tt>, then it is represented by two bytes, to be written
    /// in the order shown:
    /// <para><tt>(byte)(0xc0 | (0x1f & (c >> 6)))</tt></para>
    /// <para><tt>(byte)(0x80 | (0x3f & c))</tt></para>
    /// </para>
    /// <para>
    /// If a character <tt>c</tt> is in the range <tt>&#92;u0800</tt> through <tt>uffff</tt>,
    /// then it is represented by three bytes, to be written in the order shown:
    /// <para><tt>(byte)(0xe0 | (0x0f & (c >> 12)))</tt></para>
    /// <para><tt>(byte)(0x80 | (0x3f & (c >>  6)))</tt></para>
    /// <para><tt>(byte)(0x80 | (0x3f & c))</tt></para>
    /// </para>
    /// <para>
    /// First, the total number of bytes needed to represent all the characters of <tt>s</tt> is
    /// calculated. If this number is larger than <tt>65535</tt>, then a <tt>UTFDataFormatException</tt>
    /// is thrown. Otherwise, this length is written to the output stream in exactly the manner
    /// of the <tt>writeShort</tt> method; after this, the one-, two-, or three-byte representation
    /// of each character in the string <tt>s</tt> is written.
    /// </para>
    /// <para>
    /// The bytes written by this method may be read by the <tt>readUTF</tt> method of interface
    /// <tt>DataInput</tt>, which will then return a <tt>String</tt> equal to <tt>s</tt>.
    /// </para>
    /// </summary>
    /// <param name="s"> the string value to be written. </param>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public void WriteUTF( string s )
    {
    }

    public void WriteFloat( object getAccelerometerX )
    {
        throw new NotImplementedException();
    }

    public void WriteChar( char character )
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
    }
}