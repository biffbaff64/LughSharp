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


namespace Corelib.LibCore.Utils;

/// <summary>
/// Various constants applicable to floats.
/// </summary>
[PublicAPI]
public static class FloatConstants
{
    public const float POSITIVE_INFINITY = float.PositiveInfinity;
    public const float NEGATIVE_INFINITY = float.NegativeInfinity;
    public const float NA_N              = float.NaN;
    public const float MAX_VALUE         = float.MaxValue;
    public const float MIN_VALUE         = float.MinValue;
    public const float MIN_NORMAL        = 1.17549435E-38f;
    public const int   SIGNIFICAND_WIDTH = 24;
    public const int   MAX_EXPONENT      = 127;
    public const int   MIN_EXPONENT      = -126;
    public const int   MIN_SUB_EXPONENT  = -149;
    public const int   EXP_BIAS          = 127;
    public const int   SIGN_BIT_MASK     = int.MinValue;
    public const int   EXP_BIT_MASK      = 2139095040;
    public const int   SIGNIF_BIT_MASK   = 8388607;
    public const float FLOAT_TOLERANCE   = 0.000001f; // 32 bits
}
