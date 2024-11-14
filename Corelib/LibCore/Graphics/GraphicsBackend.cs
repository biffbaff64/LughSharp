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

namespace Corelib.LibCore.Graphics;

[PublicAPI]
public class GraphicsBackend
{
    /// <summary>
    /// The supported underlying graphics backends.
    /// </summary>
    [PublicAPI]
    public enum Type
    {
        /// <summary>
        /// OpenGL graphics backend.
        /// </summary>
        OpenGL,
        OpenGLES,
        WebGL,

        /// <summary>
        /// Vulkan graphics backend.
        /// </summary>
        Vulkan,

        /// <summary>
        /// Microsoft DirectX 11 graphics backend.
        /// </summary>
        DirectX,

        // Possible: Apple Metal ?
        // Possible: DirectX12, seperately from DirectX ?
    }

    /// <summary>
    /// Sub-categories for OpenGL
    /// </summary>
    [PublicAPI]
    public enum OpenGLSubCategories
    {
        GL10,
        GL20,
        GL30,
        GL40,
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct BackendInfo
    {
        public Type                Type        { get; set; }
        public OpenGLSubCategories SubCategory { get; set; }
    }
    
    public BackendInfo Data { get; set; }
}