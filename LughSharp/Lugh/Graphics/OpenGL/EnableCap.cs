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

namespace LughSharp.Lugh.Graphics.OpenGL;

[PublicAPI]
public enum EnableCap : int
{
    // --------------------------------
    ColorArray    = IGL.GL_COLOR_ARRAY,
    ColorLogicOp  = IGL.GL_COLOR_LOGIC_OP,
    ColorMaterial = IGL.GL_COLOR_MATERIAL,

    // --------------------------------
    CullFace = IGL.GL_CULL_FACE,
    Fog      = IGL.GL_FOG,
    Lighting = IGL.GL_LIGHTING,

    // --------------------------------
    DebugOutput            = IGL.GL_DEBUG_OUTPUT,
    DebugOutputSynchronous = IGL.GL_DEBUG_OUTPUT_SYNCHRONOUS,

    // --------------------------------
    TextureCubemapSeamless = IGL.GL_TEXTURE_CUBE_MAP_SEAMLESS,
    Texture1D              = IGL.GL_TEXTURE_1D,
    Texture2D              = IGL.GL_TEXTURE_2D,
    TextureCoordArray      = IGL.GL_TEXTURE_COORD_ARRAY,

    // --------------------------------
    Blend  = IGL.GL_BLEND,
    Dither = IGL.GL_DITHER,

    // --------------------------------
//    AlphaTest   = IGL.GL_ALPHA_TEST,
    ScissorTest = IGL.GL_SCISSOR_TEST,
    StencilTest = IGL.GL_STENCIL_TEST,
    DepthTest   = IGL.GL_DEPTH_TEST,

    // --------------------------------
    NormalArray = IGL.GL_NORMAL_ARRAY,
    Normalize   = IGL.GL_NORMALIZE,

    // --------------------------------
    VertexArray = IGL.GL_VERTEX_ARRAY,

    // --------------------------------
    SampleAlphaToCoverage = IGL.GL_SAMPLE_ALPHA_TO_COVERAGE,
    SampleAlphaToOne      = IGL.GL_SAMPLE_ALPHA_TO_ONE,
    SampleCoverage        = IGL.GL_SAMPLE_COVERAGE,
}