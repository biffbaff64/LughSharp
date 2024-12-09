// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace Corelib.Lugh.Maths;

/// <summary>
/// The abstract class Number is the superclass of platform classes representing
/// numeric values that are convertible to the primitive types byte, double, float,
/// int, long, and short.
/// The specific semantics of the conversion from the numeric value of a particular
/// Number implementation to a given primitive type is defined by the Number
/// implementation in question. For platform classes, the conversion is often
/// analogous to a narrowing primitive conversion or a widening primitive conversion.
/// Therefore, conversions may lose information about the overall magnitude of a
/// numeric value, may lose precision, and may even return a result of a different
/// sign than the input.
/// See the documentation of a given Number implementation for conversion details.
/// </summary>
/// <remarks>This class is taken directly from java.lang.Number</remarks>
[PublicAPI]
public abstract class Number
{
    //TODO: This class can probably go

    /// <summary>
    /// Returns the value of the specified number as an <tt>int</tt>
    /// which may involve rounding or truncation.
    /// </summary>
    /// <returns>
    /// the numeric value represented by this object after conversion
    /// to type <tt>int</tt>.
    /// </returns>
    public abstract int IntValue();

    /// <summary>
    /// Returns the value of the specified number as a <tt>long</tt>
    /// which may involve rounding or truncation.
    /// </summary>
    /// <returns>
    /// the numeric value represented by this object after conversion
    /// to type <tt>long</tt>.
    /// </returns>
    public abstract long LongValue();

    /// <summary>
    /// Returns the value of the specified number as a <tt>float</tt>
    /// which may involve rounding.
    /// </summary>
    /// <returns>
    /// the numeric value represented by this object after conversion
    /// to type <tt>float</tt>.
    /// </returns>
    public abstract float FloatValue();

    /// <summary>
    /// Returns the value of the specified number as a <tt>double</tt>
    /// which may involve rounding.
    /// </summary>
    /// <returns>
    /// the numeric value represented by this object after conversion
    /// to type <tt>double</tt>.
    /// </returns>
    public abstract double DoubleValue();

    /// <summary>
    /// Returns the value of the specified number as a <tt>byte</tt>
    /// which may involve rounding or truncation.
    /// <para>
    /// This implementation returns the result of <see cref="IntValue"/> cast
    /// to a <tt>byte</tt>.
    /// </para>
    /// </summary>
    /// <returns>
    /// the numeric value represented by this object after conversion
    /// to type <tt>byte</tt>.
    /// </returns>
    public sbyte ByteValue()
    {
        return ( sbyte ) IntValue();
    }

    /// <summary>
    /// Returns the value of the specified number as a <tt>short</tt>
    /// which may involve rounding or truncation.
    /// <para>
    /// This implementation returns the result of <see cref="IntValue"/>
    /// cast to a <tt>short</tt>.
    /// </para>
    /// </summary>
    /// <returns>
    /// the numeric value represented by this object after conversion
    /// to type <tt>short</tt>.
    /// </returns>
    public short ShortValue()
    {
        return ( short ) IntValue();
    }
}
