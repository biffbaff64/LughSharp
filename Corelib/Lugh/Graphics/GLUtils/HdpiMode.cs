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

using Corelib.Lugh.Graphics.OpenGL;

namespace Corelib.Lugh.Graphics.GLUtils;

[PublicAPI]
public enum HdpiMode
{
    /// <summary>
    /// Mouse coordinates, <see cref="IGraphics.Width"/> and <see cref="IGraphics.Height"/>
    /// will return logical coordinates according to the system defined HDPI scaling.
    /// Rendering will be performed to a backbuffer at raw resolution. Use <see cref="HdpiUtils"/>
    /// when calling <see cref="GLBindings.glScissor"/> or <see cref="GLBindings.glViewport"/>
    /// which expect raw coordinates.
    /// </summary>
    Logical,

    /// <summary>
    /// Mouse coordinates, <see cref="IGraphics.Width"/> and <see cref="IGraphics.Height"/>
    /// will return raw pixel coordinates irrespective of the system defined HDPI scaling.
    /// </summary>
    Pixels,
}
