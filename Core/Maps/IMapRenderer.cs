// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps;

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
    /// <param name="projectionMatrix"></param>
    /// <param name="viewboundsX"></param>
    /// <param name="viewboundsY"></param>
    /// <param name="viewboundsWidth"></param>
    /// <param name="viewboundsHeight"></param>
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