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

using Corelib.Lugh.Graphics.Cameras;
using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Graphics.Viewport;

/// <summary>
/// A viewport that scales the world using <see cref="Scaling"/>.
/// <para>
/// <see cref="Utils.Scaling.Fit"/> keeps the aspect ratio by scaling the world up to
/// fit the screen, adding black bars (letterboxing) for the remaining space.
/// </para>
/// <para>
/// <see cref="Utils.Scaling.Fill"/> keeps the aspect ratio by scaling the world up to
/// take the whole screen (some of the world may be off screen).
/// </para>
/// <para>
/// <see cref="Utils.Scaling.Stretch"/> does not keep the aspect ratio, the world is
/// scaled to take the whole screen.
/// </para>
/// <para>
/// <see cref="Utils.Scaling.None"/> keeps the aspect ratio by using a fixed size world
/// (the world may not fill the screen or some of the world may be off screen).
/// </para>
/// </summary>
[PublicAPI]
public class ScalingViewport : Viewport
{
    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>.
    /// </summary>
    /// <param name="scaling"> The <see cref="Scaling"/> to use. </param>
    /// <param name="worldWidth"> The world width in pixels. </param>
    /// <param name="worldHeight"> The world height in pixels. </param>
    protected ScalingViewport( Scaling scaling, float worldWidth, float worldHeight )
        : this( scaling, worldWidth, worldHeight, new OrthographicCamera() )
    {
    }

    /// <summary>
    /// Creates a new viewport using the supplied <see cref="OrthographicCamera"/>.
    /// </summary>
    /// <param name="scaling"> The <see cref="Scaling"/> to use. </param>
    /// <param name="worldWidth"> The world width in pixels. </param>
    /// <param name="worldHeight"> The world height in pixels. </param>
    /// <param name="camera"> The camera to use. </param>
    public ScalingViewport( Scaling scaling, float worldWidth, float worldHeight, Camera camera )
        : base( camera )
    {
        Scaling = scaling;

        WorldWidth  = worldWidth;
        WorldHeight = worldHeight;
    }

    protected Scaling Scaling { get; }

    /// <summary>
    /// Configures this viewports screen bounds and applies it to the camera
    /// </summary>
    /// <param name="screenWidth"> Screen width in pixels. </param>
    /// <param name="screenHeight"> Screen height in pixels. </param>
    /// <param name="centerCamera"> True to center the camera in the middle of the screen. </param>
    public override void Update( int screenWidth, int screenHeight, bool centerCamera = false )
    {
        var scaled = Scaling.Apply( WorldWidth, WorldHeight, screenWidth, screenHeight );

        var viewportWidth  = ( int ) Math.Round( scaled.X, MidpointRounding.AwayFromZero );
        var viewportHeight = ( int ) Math.Round( scaled.Y, MidpointRounding.AwayFromZero );

        // Center
        SetScreenBounds( ( screenWidth - viewportWidth ) / 2,
                         ( screenHeight - viewportHeight ) / 2,
                         viewportWidth,
                         viewportHeight );

        Apply( centerCamera );
    }
}
