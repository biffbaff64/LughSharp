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

public class ExtendViewport : Viewport
{
    #region Properties
    
    public float MinWorldWidth  { get; set; }
    public float MinWorldHeight { get; set; }
    public float MaxWorldWidth  { get; set; }
    public float MaxWorldHeight { get; set; }

    #endregion

    /// <summary>
    /// Creates a new viewport using a new <seealso cref="OrthographicCamera"/> with no maximum world size.
    /// </summary>
    public ExtendViewport( float minWorldWidth, float minWorldHeight )
        : this( minWorldWidth, minWorldHeight, 0, 0, new OrthographicCamera() )
    {
    }

    /// <summary>
    /// Creates a new viewport with no maximum world size.
    /// </summary>
    public ExtendViewport( float minWorldWidth, float minWorldHeight, Camera camera )
        : this( minWorldWidth, minWorldHeight, 0, 0, camera )
    {
    }

    /// <summary>
    /// Creates a new viewport using a new <seealso cref="OrthographicCamera"/> and a maximum world size.
    /// </summary>
    /// <see cref="ExtendViewport(float, float, float, float, Camera)"/>
    public ExtendViewport( float minWorldWidth, float minWorldHeight, float maxWorldWidth, float maxWorldHeight )
        : this( minWorldWidth, minWorldHeight, maxWorldWidth, maxWorldHeight, new OrthographicCamera() )
    {
    }

    /// <summary>
    /// Creates a new viewport with a maximum world size.
    /// </summary>
    /// <param name="minWorldWidth"></param>
    /// <param name="minWorldHeight"></param>
    /// <param name="maxWorldWidth"> User 0 for no maximum width. </param>
    /// <param name="maxWorldHeight"> User 0 for no maximum height.  </param>
    /// <param name="camera"></param>
    public ExtendViewport( float minWorldWidth,
                           float minWorldHeight,
                           float maxWorldWidth,
                           float maxWorldHeight,
                           Camera camera )
    {
        this.MinWorldWidth  = minWorldWidth;
        this.MinWorldHeight = minWorldHeight;
        this.MaxWorldWidth  = maxWorldWidth;
        this.MaxWorldHeight = maxWorldHeight;

        this.Camera = camera;
    }

    public new void Update( int screenWidth, int screenHeight, bool centerCamera )
    {
        // Fit min size to the screen.
        var     worldWidth  = MinWorldWidth;
        var     worldHeight = MinWorldHeight;
        Vector2 scaled      = Scaling.Fit.Apply( worldWidth, worldHeight, screenWidth, screenHeight );

        // Extend in the short direction.
        var viewportWidth  = ( int )Math.Round( scaled.X, MidpointRounding.AwayFromZero );
        var viewportHeight = ( int )Math.Round( scaled.Y, MidpointRounding.AwayFromZero );

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
            viewportWidth += ( int )Math.Round( lengthen * toViewportSpace, MidpointRounding.AwayFromZero );
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
            viewportHeight += ( int )Math.Round( lengthen * toViewportSpace, MidpointRounding.AwayFromZero );
        }

        SetWorldSize( worldWidth, worldHeight );

        // Center.
        SetScreenBounds
            (
             ( screenWidth - viewportWidth ) / 2,
             ( screenHeight - viewportHeight ) / 2,
             viewportWidth,
             viewportHeight
            );

        Apply( centerCamera );
    }
}