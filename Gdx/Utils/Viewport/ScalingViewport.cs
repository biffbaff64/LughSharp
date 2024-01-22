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
///     A viewport that scales the world using <see cref="Scaling" />.
///     <para>
///         <see cref="LibGDXSharp.Utils.Scaling.Fit" /> keeps the aspect ratio by scaling the world up to
///         fit the screen, adding black bars (letterboxing) for the remaining space.
///     </para>
///     <para>
///         <see cref="LibGDXSharp.Utils.Scaling.Fill" /> keeps the aspect ratio by scaling the world up to
///         take the whole screen (some of the world may be off screen).
///     </para>
///     <para>
///         <see cref="LibGDXSharp.Utils.Scaling.Stretch" /> does not keep the aspect ratio, the world is
///         scaled to take the whole screen.
///     </para>
///     <para>
///         <see cref="LibGDXSharp.Utils.Scaling.None" /> keeps the aspect ratio by using a fixed size world
///         (the world may not fill the screen or some of the world may be off screen).
///     </para>
/// </summary>
public class ScalingViewport : Viewport
{
    /// <summary>
    ///     Creates a new viewport using a new <see cref="OrthographicCamera" />.
    /// </summary>
    protected ScalingViewport( Scaling scaling, float worldWidth, float worldHeight )
        : this( scaling, worldWidth, worldHeight, new OrthographicCamera() )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="scaling"></param>
    /// <param name="worldWidth"></param>
    /// <param name="worldHeight"></param>
    /// <param name="camera"></param>
    public ScalingViewport( Scaling scaling, float worldWidth, float worldHeight, Camera camera )
        : base( camera )
    {
        Scaling = scaling;

        WorldWidth  = worldWidth;
        WorldHeight = worldHeight;
    }

    protected Scaling Scaling { get; }

    /// <summary>
    ///     Configures this viewports screen bounds and applies it to the camera
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    /// <param name="centerCamera"></param>
    public override void Update( int screenWidth, int screenHeight, bool centerCamera = false )
    {
        Vector2 scaled = Scaling.Apply(
            WorldWidth,
            WorldHeight,
            screenWidth,
            screenHeight
            );

        var viewportWidth  = ( int )Math.Round( scaled.X, MidpointRounding.AwayFromZero );
        var viewportHeight = ( int )Math.Round( scaled.Y, MidpointRounding.AwayFromZero );

        // Center
        SetScreenBounds(
            ( screenWidth - viewportWidth ) / 2,
            ( screenHeight - viewportHeight ) / 2,
            viewportWidth,
            viewportHeight
            );

        Apply( centerCamera );
    }
}
