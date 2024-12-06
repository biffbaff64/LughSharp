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

namespace Corelib.Lugh.Graphics.Viewport;

/// <summary>
/// A viewport where the world size is based on the size of the screen.
/// By default 1 world unit == 1 screen pixel, but this ratio can be
/// changed by modifying <see cref="UnitsPerPixel"/>.
/// </summary>
[PublicAPI]
public class ScreenViewport : Viewport
{
    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>.
    /// </summary>
    public ScreenViewport() : base( new OrthographicCamera() )
    {
    }

    public static float UnitsPerPixel => 1;

    /// <summary>
    /// Configures this viewports screen bounds and applies it to the camera
    /// </summary>
    /// <param name="screenWidth"> Screen width in pixels. </param>
    /// <param name="screenHeight"> Screen height in pixels. </param>
    /// <param name="centerCamera"> True to center the camera in the middle of the screen. </param>
    public override void Update( int screenWidth, int screenHeight, bool centerCamera = false )
    {
        SetScreenBounds( 0, 0, screenWidth, screenHeight );
        SetWorldSize( screenWidth * UnitsPerPixel, screenHeight * UnitsPerPixel );
        Apply( centerCamera );
    }
}
