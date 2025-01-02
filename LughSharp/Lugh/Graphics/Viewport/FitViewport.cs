// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Utils;

namespace LughSharp.Lugh.Graphics.Viewport;

/// <summary>
/// A ScalingViewport that uses <see cref="Scaling.Fit"/> so it keeps the aspect
/// ratio by scaling the world up to fit the screen, adding black bars (letterboxing)
/// for the remaining space.
/// </summary>
[PublicAPI]
public class FitViewport : ScalingViewport
{
    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>.
    /// </summary>
    /// <param name="worldWidth"> World width in pixels. </param>
    /// <param name="worldHeight"> World height in pixels. </param>
    public FitViewport( float worldWidth, float worldHeight )
        : base( Scaling.Fit, worldWidth, worldHeight )
    {
    }

    /// <summary>
    /// Creates a new viewport using the supplied <see cref="OrthographicCamera"/>.
    /// </summary>
    /// <param name="worldWidth"> World width in pixels. </param>
    /// <param name="worldHeight"> World height in pixels. </param>
    /// <param name="camera"> The camera to use. </param>
    public FitViewport( float worldWidth, float worldHeight, Camera camera )
        : base( Scaling.Fit, worldWidth, worldHeight, camera )
    {
    }
}
