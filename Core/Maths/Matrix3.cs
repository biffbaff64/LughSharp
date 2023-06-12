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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maths;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Matrix3
{
    public const int M00 = 0;
    public const int M01 = 3;
    public const int M02 = 6;
    public const int M10 = 1;
    public const int M11 = 4;
    public const int M12 = 7;
    public const int M20 = 2;
    public const int M21 = 5;
    public const int M22 = 8;

    public float[] val = new float[ 9 ];
        
    private float[] _tmp = new float[ 9 ];
}