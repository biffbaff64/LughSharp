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
using Matrix4 = Corelib.Lugh.Maths.Matrix4;

namespace Corelib.Lugh.Maps;

[PublicAPI]
public interface IMapRenderer
{
    /// <summary>
    /// Sets the projection matrix and viewbounds from the given camera.
    /// If the camera changes, you have to call this method again.
    /// The viewbounds are taken from the camera's position and viewport size as
    /// well as the scale. This method will only work if the camera's direction
    /// vector is (0,0,-1) and its up vector is (0, 1, 0), which are the defaults.
    /// <param name="camera">The <see cref="OrthographicCamera"/> to use.</param>
    /// </summary>
    public void SetView( OrthographicCamera camera );

    /// <summary>
    /// Sets the projection matrix for rendering, as well as the bounds of
    /// the map which should be rendered. Make sure that the frustum spanned
    /// by the projection matrix coincides with the viewbounds.
    /// </summary>
    public void SetView( Matrix4 projectionMatrix,
                         float viewboundsX,
                         float viewboundsY,
                         float viewboundsWidth,
                         float viewboundsHeight );

    /// <summary>
    /// Renders all the layers of a map.
    /// </summary>
    public void Render();

    /// <summary>
    /// Renders the given layer indexes of a map.
    /// </summary>
    /// <param name="layers">The layers to render.</param>
    public void Render( int[] layers );
}
