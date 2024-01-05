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

namespace LibGDXSharp.Utils.Viewport;

/// <summary>
///     A ScalingViewport that uses <seealso cref="Scaling.Stretch" /> so it does not
///     keep the aspect ratio, the world is scaled to take the whole screen.
/// </summary>
public class StretchViewport : ScalingViewport
{
    /// <summary>
    ///     Creates a new viewport using a new <seealso cref="OrthographicCamera" />.
    /// </summary>
    public StretchViewport( float worldWidth, float worldHeight )
        : base( Scaling.Stretch, worldWidth, worldHeight )
    {
    }

    public StretchViewport( float worldWidth, float worldHeight, Camera camera )
        : base( Scaling.Stretch, worldWidth, worldHeight, camera )
    {
    }
}
