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

[PublicAPI]
public record FloatConsts
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
}
