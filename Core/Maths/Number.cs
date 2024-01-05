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

namespace LibGDXSharp.Maths;

/// <summary>
///     The abstract class Number is the superclass of platform classes representing
///     numeric values that are convertible to the primitive types byte, double, float,
///     int, long, and short.
///     The specific semantics of the conversion from the numeric value of a particular
///     Number implementation to a given primitive type is defined by the Number
///     implementation in question. For platform classes, the conversion is often
///     analogous to a narrowing primitive conversion or a widening primitive conversion.
///     Therefore, conversions may lose information about the overall magnitude of a
///     numeric value, may lose precision, and may even return a result of a different
///     sign than the input.
///     See the documentation of a given Number implementation for conversion details.
/// </summary>
/// <remarks>This class is taken directly from java.lang.Number</remarks>
public abstract class Number
{

    /// <summary>
    /// </summary>
    private const long SERIAL_VERSION_UID = -8742448824652078965L;

    /// <summary>
    ///     Returns the value of the specified number as an <code>int</code>
    ///     which may involve rounding or truncation.
    /// </summary>
    /// <returns>
    ///     the numeric value represented by this object after conversion
    ///     to type <code>int</code>.
    /// </returns>
    public abstract int IntValue();

    /// <summary>
    ///     Returns the value of the specified number as a <code>long</code>
    ///     which may involve rounding or truncation.
    /// </summary>
    /// <returns>
    ///     the numeric value represented by this object after conversion
    ///     to type <code>long</code>.
    /// </returns>
    public abstract long LongValue();

    /// <summary>
    ///     Returns the value of the specified number as a <code>float</code>
    ///     which may involve rounding.
    /// </summary>
    /// <returns>
    ///     the numeric value represented by this object after conversion
    ///     to type <code>float</code>.
    /// </returns>
    public abstract float FloatValue();

    /// <summary>
    ///     Returns the value of the specified number as a <code>double</code>
    ///     which may involve rounding.
    /// </summary>
    /// <returns>
    ///     the numeric value represented by this object after conversion
    ///     to type <code>double</code>.
    /// </returns>
    public abstract double DoubleValue();

    /// <summary>
    ///     Returns the value of the specified number as a <code>byte</code>
    ///     which may involve rounding or truncation.
    ///     <para>
    ///         This implementation returns the result of <see cref="IntValue" /> cast
    ///         to a <code>byte</code>.
    ///     </para>
    /// </summary>
    /// <returns>
    ///     the numeric value represented by this object after conversion
    ///     to type <code>byte</code>.
    /// </returns>
    public sbyte ByteValue() => ( sbyte )IntValue();

    /// <summary>
    ///     Returns the value of the specified number as a <code>short</code>
    ///     which may involve rounding or truncation.
    ///     <para>
    ///         This implementation returns the result of <see cref="IntValue" />
    ///         cast to a <code>short</code>.
    ///     </para>
    /// </summary>
    /// <returns>
    ///     the numeric value represented by this object after conversion
    ///     to type <code>short</code>.
    /// </returns>
    public short ShortValue() => ( short )IntValue();
}
