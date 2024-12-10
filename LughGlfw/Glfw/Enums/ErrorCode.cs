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
/// Wrapping enum for ErrorCode.
/// </summary>
[PublicAPI]
public enum ErrorCode
{
    /// <inheritdoc cref="NativeGlfw.GLFW_NO_ERROR" />
    NoError = NativeGlfw.GLFW_NO_ERROR,

    /// <inheritdoc cref="NativeGlfw.GLFW_NOT_INITIALIZED" />
    NotInitialized = NativeGlfw.GLFW_NOT_INITIALIZED,

    /// <inheritdoc cref="NativeGlfw.GLFW_NO_CURRENT_CONTEXT" />
    NoCurrentContext = NativeGlfw.GLFW_NO_CURRENT_CONTEXT,

    /// <inheritdoc cref="NativeGlfw.GLFW_INVALID_ENUM" />
    InvalidEnum = NativeGlfw.GLFW_INVALID_ENUM,

    /// <inheritdoc cref="NativeGlfw.GLFW_INVALID_VALUE" />
    InvalidValue = NativeGlfw.GLFW_INVALID_VALUE,

    /// <inheritdoc cref="NativeGlfw.GLFW_OUT_OF_MEMORY" />
    OutOfMemory = NativeGlfw.GLFW_OUT_OF_MEMORY,

    /// <inheritdoc cref="NativeGlfw.GLFW_API_UNAVAILABLE" />
    ApiUnavailable = NativeGlfw.GLFW_API_UNAVAILABLE,

    /// <inheritdoc cref="NativeGlfw.GLFW_VERSION_UNAVAILABLE" />
    VersionUnavailable = NativeGlfw.GLFW_VERSION_UNAVAILABLE,

    /// <inheritdoc cref="NativeGlfw.GLFW_PLATFORM_ERROR" />
    PlatformError = NativeGlfw.GLFW_PLATFORM_ERROR,

    /// <inheritdoc cref="NativeGlfw.GLFW_FORMAT_UNAVAILABLE" />
    FormatUnavailable = NativeGlfw.GLFW_FORMAT_UNAVAILABLE,

    /// <inheritdoc cref="NativeGlfw.GLFW_NO_WINDOW_CONTEXT" />
    NoWindowContext = NativeGlfw.GLFW_NO_WINDOW_CONTEXT,

    /// <inheritdoc cref="NativeGlfw.GLFW_CURSOR_UNAVAILABLE" />
    CursorUnavailable = NativeGlfw.GLFW_CURSOR_UNAVAILABLE,

    /// <inheritdoc cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE" />
    FeatureUnavailable = NativeGlfw.GLFW_FEATURE_UNAVAILABLE,

    /// <inheritdoc cref="NativeGlfw.GLFW_FEATURE_UNIMPLEMENTED" />
    FeatureUnimplemented = NativeGlfw.GLFW_FEATURE_UNIMPLEMENTED,

    /// <inheritdoc cref="NativeGlfw.GLFW_PLATFORM_UNAVAILABLE" />
    PlatformUnavailable = NativeGlfw.GLFW_PLATFORM_UNAVAILABLE,
}