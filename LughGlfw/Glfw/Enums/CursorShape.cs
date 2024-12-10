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

namespace LughGlfw.Glfw.Enums;

/// <summary>
/// Wrapping enum for CursorShape.
/// </summary>
[PublicAPI]
public enum CursorShape
{
    /// <inheritdoc cref="NativeGlfw.GLFW_ARROW_CURSOR" />
    Arrow = NativeGlfw.GLFW_ARROW_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_IBEAM_CURSOR" />
    Ibeam = NativeGlfw.GLFW_IBEAM_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_CROSSHAIR_CURSOR" />
    Crosshair = NativeGlfw.GLFW_CROSSHAIR_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_POINTING_HAND_CURSOR" />
    PointingHand = NativeGlfw.GLFW_POINTING_HAND_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_RESIZE_EW_CURSOR" />
    ResizeEw = NativeGlfw.GLFW_RESIZE_EW_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_RESIZE_NS_CURSOR" />
    ResizeNs = NativeGlfw.GLFW_RESIZE_NS_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_RESIZE_NWSE_CURSOR" />
    ResizeNwse = NativeGlfw.GLFW_RESIZE_NWSE_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_RESIZE_NESW_CURSOR" />
    ResizeNesw = NativeGlfw.GLFW_RESIZE_NESW_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_RESIZE_ALL_CURSOR" />
    ResizeAll = NativeGlfw.GLFW_RESIZE_ALL_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_NOT_ALLOWED_CURSOR" />
    NotAllowed = NativeGlfw.GLFW_NOT_ALLOWED_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_HRESIZE_CURSOR" />
    Hresize = NativeGlfw.GLFW_HRESIZE_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_VRESIZE_CURSOR" />
    Vresize = NativeGlfw.GLFW_VRESIZE_CURSOR,

    /// <inheritdoc cref="NativeGlfw.GLFW_HAND_CURSOR" />
    Hand = NativeGlfw.GLFW_HAND_CURSOR,
}