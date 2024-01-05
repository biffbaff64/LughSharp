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

namespace LibGDXSharp.Utils;

/// <summary>
///     A typesafe enumeration for byte orders.
/// </summary>
/// <remarks>Not sure if this is still needed.</remarks>
public class ByteOrder
{

    public readonly static ByteOrder NativeOrder = new( "NativeOrder" );

    /// <summary>
    ///     Constant denoting big-endian byte order.  In this order, the bytes of a
    ///     multibyte value are ordered from most significant to least significant.
    /// </summary>
    public readonly static ByteOrder BigEndian = new( "BigEndian" );

    /// <summary>
    ///     Constant denoting little-endian byte order. In this order, the bytes of
    ///     a multibyte value are ordered from least significant to most significant.
    /// </summary>
    public readonly static ByteOrder LittleEndian = new( "LittleEndian" );
    private readonly string _name;

    private ByteOrder( string name ) => _name = name;

    /// <summary>
    ///     Constructs a string describing this object.
    ///     <para>
    ///         This method returns the string <tt>"BigEndian"</tt> for <see cref="BigEndian" />
    ///         and <tt>"LittleEndian"</tt> for <see cref="LittleEndian" />.
    ///     </para>
    /// </summary>
    /// <returns>The specified string</returns>
    public override string ToString() => _name;
}
