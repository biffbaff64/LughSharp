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

namespace LibGDXSharp.Graphics;

/// <summary>
/// Class describing the bits per pixel, depth buffer precision,
/// stencil precision and number of MSAA samples.
/// </summary>
public record BufferFormat
{
    public int  R                { get; set; } // number of bits per color channel.
    public int  G                { get; set; } // ...
    public int  B                { get; set; } // ...
    public int  A                { get; set; } // ...
    public int  Depth            { get; set; } // number of bits for depth and stencil buffer.
    public int  Stencil          { get; set; } // ...
    public int  Samples          { get; set; } // number of samples for multi-sample anti-aliasing (MSAA).
    public bool CoverageSampling { get; set; } // whether coverage sampling anti-aliasing is used.
//                                                 If so, you have to clear the coverage buffer as well!

    public override string ToString()
    {
        return "r - " + R + ", g - " + G + ", b - " + B + ", a - " + A
               + ", depth - " + Depth + ", stencil - " + Stencil
               + ", num samples - " + Samples + ", coverage sampling - " + CoverageSampling;
    }
}