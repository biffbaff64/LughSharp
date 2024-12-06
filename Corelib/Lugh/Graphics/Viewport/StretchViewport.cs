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

using Corelib.Lugh.Graphics.Cameras;
using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Graphics.Viewport;

/// <summary>
/// A ScalingViewport that uses <see cref="Scaling.Stretch"/> so it does not
/// keep the aspect ratio, the world is scaled to take the whole screen.
/// </summary>
[PublicAPI]
public class StretchViewport : ScalingViewport
{
    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>.
    /// </summary>
    /// <param name="worldWidth"> The world width in pixels. </param>
    /// <param name="worldHeight"> The world height in pixels. </param>
    public StretchViewport( float worldWidth, float worldHeight )
        : base( Scaling.Stretch, worldWidth, worldHeight )
    {
    }

    /// <summary>
    /// Creates a new viewport using the supplied <see cref="OrthographicCamera"/>.
    /// </summary>
    /// <param name="worldWidth"> The world width in pixels. </param>
    /// <param name="worldHeight"> The world height in pixels. </param>
    /// <param name="camera"> The camera to use. </param>
    public StretchViewport( float worldWidth, float worldHeight, Camera camera )
        : base( Scaling.Stretch, worldWidth, worldHeight, camera )
    {
    }
}
