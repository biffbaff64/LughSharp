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
/// Defines an interface for classes that are able to write typed data to some
/// target. Typically, this data can be read in by a class which implements
/// DataInput. Types that can be written include byte, 16-bit short, 32-bit int,
/// 32-bit float, 64-bit long, 64-bit double, byte strings, and DataInput MUTF-8
/// encoded strings.
/// <para/>
/// Equivalent to Java's DataOutut interface.
/// </summary>
/// <seealso cref="DataOutputStream"/>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public interface IDataOutput
{
    /// <summary>
    /// Writes the entire contents of the byte array <paramref name="buffer"/> to this
    /// stream.
    /// </summary>
    /// <param name="buffer">The buffer to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadFully(byte[])"/>
    /// <seealso cref="IDataInput.ReadFully(byte[], int, int)"/>
    void Write( byte[] buffer );

    /// <summary>
    /// Writes <paramref name="count"/> bytes from the byte array <paramref name="buffer"/> starting at
    /// <paramref name="offset"/>.
    /// </summary>
    /// <param name="buffer">The buffer to write.</param>
    /// <param name="offset">The index of the first byte in <paramref name="buffer"/> to write.</param>
    /// <param name="count">The number of bytes from the <paramref name="buffer"/> to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadFully(byte[])"/>
    /// <seealso cref="IDataInput.ReadFully(byte[], int, int)"/>
    void Write( byte[] buffer, int offset, int count );

    /// <summary>
    /// Writes the specified 8-bit <see cref="byte"/>.
    /// </summary>
    /// <param name="oneByte">The <see cref="byte"/> to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadByte()"/>
    void Write( int oneByte );

    /// <summary>
    /// Writes the specified <see cref="bool"/>.
    /// </summary>
    /// <param name="value">The boolean value to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadBoolean()"/>
    void WriteBoolean( bool value );

    /// <summary>
    /// Writes the specified 8-bit <see cref="byte"/>.
    /// </summary>
    /// <param name="value">The <see cref="byte"/> value to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadByte()"/>
    /// <seealso cref="IDataInput.ReadUnsignedShort()"/>
    void WriteByte( int value );

    /// <summary>
    /// Writes the low order 8-bit bytes from the specified string.
    /// </summary>
    /// <param name="value">The string containing the bytes to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadFully(byte[])"/>
    /// <seealso cref="IDataInput.ReadFully(byte[], int, int)"/>
    void WriteBytes( string value );

    /// <summary>
    /// Writes the specified 16-bit character. Only the two least significant
    /// bytes of the integer <paramref name="value"/> are written, with the higher one
    /// written first. This represents the Unicode value of the char.
    /// </summary>
    /// <param name="value">The character to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadChar()"/>
    void WriteChar( int value );

    /// <summary>
    /// Writes the 16-bit characters contained in <paramref name="str"/>.
    /// </summary>
    /// <param name="str">The string that contains the characters to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadChar()"/>
    void WriteChars( string str );

    /// <summary>
    /// Writes the specified 64-bit <see cref="double"/>. The resulting output is the eight
    /// bytes returned by <see cref="NumberUtils.DoubleToLongBits(double)"/>.
    /// </summary>
    /// <param name="value">The <see cref="double"/> to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadDouble()"/>
    void WriteDouble( double value );

    /// <summary>
    /// Writes the specified 32-bit <see cref="float"/>. The resulting output is the four bytes
    /// returned by <see cref="NumberUtils.IntBitsToFloat(int)"/>.
    /// <para/>
    /// </summary>
    /// <param name="value">The <see cref="float"/> to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadFloat()"/>
    void WriteFloat( float value );

    /// <summary>
    /// Writes the specified 32-bit <see cref="int"/>. The resulting output is the four bytes,
    /// highest order first, of <paramref name="value"/>.
    /// <para/>
    /// </summary>
    /// <param name="value">The <see cref="int"/> to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadInt()"/>
    void WriteInt( int value );

    /// <summary>
    /// Writes the specified 64-bit <see cref="long"/>. The resulting output is the eight
    /// bytes, highest order first, of <paramref name="value"/>.
    /// <para/>
    /// </summary>
    /// <param name="value">The <see cref="long"/> to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadLong()"/>
    void WriteLong( long value );

    /// <summary>
    /// Writes the specified 16-bit <see cref="short"/>. Only the lower two bytes of <paramref name="value"/>
    /// are written with the higher one written first.
    /// <para/>
    /// </summary>
    /// <param name="value">The <see cref="short"/> to write.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadShort()"/>
    /// <seealso cref="IDataInput.ReadUnsignedShort()"/>
    void WriteShort( int value );

    /// <summary>
    /// Writes the specified <see cref="string"/> encoded in DataInput modified UTF-8.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to write encoded in DataInput modified UTF-8.</param>
    /// <exception cref="System.IO.IOException">If an I/O error occurs while writing.</exception>
    /// <seealso cref="IDataInput.ReadUTF()"/>
    void WriteUTF( string value );
}