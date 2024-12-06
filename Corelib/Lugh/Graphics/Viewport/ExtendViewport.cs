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
/// A viewport that keeps the world aspect ratio by extending the world in one direction.
/// The world is first scaled to fit within the viewport, then the shorter dimension is
/// lengthened to fill the viewport. A maximum size can be specified to limit how much the
/// world is extended and black bars (letterboxing) are used for any remaining space.
/// </summary>
[PublicAPI]
public class ExtendViewport : Viewport
{
    // ========================================================================

    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>
    /// with no maximum world size.
    /// </summary>
    /// <param name="minWorldWidth"> The minimum allowable world width. </param>
    /// <param name="minWorldHeight"> The minimum allowable wortld height. </param>
    public ExtendViewport( float minWorldWidth, float minWorldHeight )
        : this( minWorldWidth, minWorldHeight, 0, 0, new OrthographicCamera() )
    {
    }

    /// <summary>
    /// Creates a new viewport with no maximum world size.
    /// </summary>
    /// <param name="minWorldWidth"> The minimum allowable world width. </param>
    /// <param name="minWorldHeight"> The minimum allowable wortld height. </param>
    /// <param name="camera"> The camera to associate with this viewport. </param>
    public ExtendViewport( float minWorldWidth, float minWorldHeight, Camera camera )
        : this( minWorldWidth, minWorldHeight, 0, 0, camera )
    {
    }

    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>
    /// and a maximum world size.
    /// </summary>
    /// <param name="minWorldWidth"> The minimum allowable world width. </param>
    /// <param name="minWorldHeight"> The minimum allowable wortld height. </param>
    /// <param name="maxWorldWidth"> Use zero for no maximum width. </param>
    /// <param name="maxWorldHeight"> Use zero for no maximum height. </param>
    public ExtendViewport( float minWorldWidth, float minWorldHeight, float maxWorldWidth, float maxWorldHeight )
        : this( minWorldWidth, minWorldHeight, maxWorldWidth, maxWorldHeight, new OrthographicCamera() )
    {
    }

    /// <summary>
    /// Creates a new viewport with a maximum world size.
    /// </summary>
    /// <param name="minWorldWidth"> The minimum allowable world width. </param>
    /// <param name="minWorldHeight"> The minimum allowable wortld height. </param>
    /// <param name="maxWorldWidth"> Use zero for no maximum width. </param>
    /// <param name="maxWorldHeight"> Use zero for no maximum height. </param>
    /// <param name="camera"> The camera to associate with this viewport. </param>
    public ExtendViewport( float minWorldWidth, float minWorldHeight, float maxWorldWidth, float maxWorldHeight, Camera camera )
        : base( camera )
    {
        MinWorldWidth  = minWorldWidth;
        MinWorldHeight = minWorldHeight;
        MaxWorldWidth  = maxWorldWidth;
        MaxWorldHeight = maxWorldHeight;
    }

    /// <summary>
    /// Update the viewport with the supplied width and height.
    /// </summary>
    /// <param name="screenWidth"> The Viewport width. </param>
    /// <param name="screenHeight"> The Viewport height. </param>
    /// <param name="centerCamera"></param>
    public override void Update( int screenWidth, int screenHeight, bool centerCamera = false )
    {
        // Fit min size to the screen.
        var worldWidth  = MinWorldWidth;
        var worldHeight = MinWorldHeight;
        var scaled      = Scaling.Fit.Apply( worldWidth, worldHeight, screenWidth, screenHeight );

        // Extend in the short direction.
        var viewportWidth  = ( int ) Math.Round( scaled.X, MidpointRounding.AwayFromZero );
        var viewportHeight = ( int ) Math.Round( scaled.Y, MidpointRounding.AwayFromZero );

        if ( viewportWidth < screenWidth )
        {
            var toViewportSpace = viewportHeight / worldHeight;
            var toWorldSpace    = worldHeight / viewportHeight;
            var lengthen        = ( screenWidth - viewportWidth ) * toWorldSpace;

            if ( MaxWorldWidth > 0 )
            {
                lengthen = Math.Min( lengthen, MaxWorldWidth - MinWorldWidth );
            }

            worldWidth    += lengthen;
            viewportWidth += ( int ) Math.Round( lengthen * toViewportSpace, MidpointRounding.AwayFromZero );
        }
        else if ( viewportHeight < screenHeight )
        {
            var toViewportSpace = viewportWidth / worldWidth;
            var toWorldSpace    = worldWidth / viewportWidth;
            var lengthen        = ( screenHeight - viewportHeight ) * toWorldSpace;

            if ( MaxWorldHeight > 0 )
            {
                lengthen = Math.Min( lengthen, MaxWorldHeight - MinWorldHeight );
            }

            worldHeight    += lengthen;
            viewportHeight += ( int ) Math.Round( lengthen * toViewportSpace, MidpointRounding.AwayFromZero );
        }

        SetWorldSize( worldWidth, worldHeight );

        // Center.
        SetScreenBounds(
                        ( screenWidth - viewportWidth ) / 2,
                        ( screenHeight - viewportHeight ) / 2,
                        viewportWidth,
                        viewportHeight
                       );

        Apply( centerCamera );
    }

    #region Properties

    public float MinWorldWidth  { get; }
    public float MinWorldHeight { get; }
    public float MaxWorldWidth  { get; }
    public float MaxWorldHeight { get; }

    #endregion
}
