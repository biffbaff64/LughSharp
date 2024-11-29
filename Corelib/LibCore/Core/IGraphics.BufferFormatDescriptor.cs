// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace Corelib.LibCore.Core;

public partial interface IGraphics
{
    /// <summary>
    /// Class describing the bits per pixel, depth buffer precision,
    /// stencil precision and number of MSAA samples.
    /// </summary>
    [PublicAPI]
    public class BufferFormatDescriptor
    {
        public int R       { get; set; } // number of bits per color channel.
        public int G       { get; set; } // ...
        public int B       { get; set; } // ...
        public int A       { get; set; } // ...
        public int Depth   { get; set; } // number of bits for depth and stencil buffer.
        public int Stencil { get; set; } // ...
        public int Samples { get; set; } // number of samples for multi-sample anti-aliasing (MSAA).

        /// <summary>
        /// Whether coverage sampling anti-aliasing is used. If so, you have
        /// to clear the coverage buffer as well!
        /// </summary>
        public bool CoverageSampling { get; set; }

        public override string ToString()
        {
            return $"r - {R}, g - {G}, b - {B}, a - {A}, depth - {Depth}, stencil - "
                   + $"{Stencil}, num samples - {Samples}, coverage sampling - {CoverageSampling}";
        }
    }
}