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

using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Utils.Exceptions;
using Blendmode = Corelib.Lugh.Maps.Tiled.ITiledMapTile.Blendmode;

namespace Corelib.Lugh.Maps.Tiled.Tiles;

/// <summary>
/// Represents an animating <see cref="ITiledMapTile"/>.
/// </summary>
[PublicAPI]
public class AnimatedTiledMapTile : ITiledMapTile
{
    public int       ID        { get; set; }
    public Blendmode BlendMode { get; set; } = Blendmode.Alpha;

    private static readonly long _initialTimeOffset = DateTime.Now.Millisecond;

    private static   long                 _lastTiledMapRenderTime = 0;
    private readonly int[]                _animationIntervals;
    private readonly StaticTiledMapTile[] _frameTiles;
    private readonly int                  _loopDuration;

    private MapObjects?    _mapObjects;
    private MapProperties? _properties;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates an animated tile with the given animation interval and frame tiles.
    /// </summary>
    /// <param name="interval">The interval between each individual frame tile.</param>
    /// <param name="frameTiles">
    /// An array of <see cref="StaticTiledMapTile"/>s that make up the animation.
    /// </param>
    public AnimatedTiledMapTile( float interval, List< StaticTiledMapTile > frameTiles )
    {
        _frameTiles = new StaticTiledMapTile[ frameTiles.Count ];

        _loopDuration       = frameTiles.Count * ( int ) ( interval * 1000f );
        _animationIntervals = new int[ frameTiles.Count ];

        for ( var i = 0; i < frameTiles.Count; ++i )
        {
            _frameTiles[ i ]         = frameTiles[ i ];
            _animationIntervals[ i ] = ( int ) ( interval * 1000f );
        }
    }

    /// <summary>
    /// Creates an animated tile with the given animation intervals and frame tiles.
    /// </summary>
    /// <param name="intervals">
    /// The intervals between each individual frame tile in milliseconds.
    /// </param>
    /// <param name="frameTiles">
    /// An array of <see cref="StaticTiledMapTile"/> that make up the animation.
    /// </param>
    public AnimatedTiledMapTile( List< int > intervals, List< StaticTiledMapTile > frameTiles )
    {
        _frameTiles = new StaticTiledMapTile[ frameTiles.Count ];

        _animationIntervals = intervals.ToArray();
        _loopDuration       = 0;

        for ( var i = 0; i < intervals.Count; ++i )
        {
            _frameTiles[ i ] =  frameTiles[ i ];
            _loopDuration    += intervals[ i ];
        }
    }

    /// <summary>
    /// The currently displayed animation frame.
    /// </summary>
    public TextureRegion TextureRegion
    {
        get => GetCurrentFrame().TextureRegion;
        set { }
    }

    /// <summary>
    /// X-coordinate of the currently displayed animation frame in the tilemap.
    /// </summary>
    public float OffsetX
    {
        get => GetCurrentFrame().OffsetX;
        set { }
    }

    /// <summary>
    /// Y-coordinate of the currently displayed animation frame in the tilemap.
    /// </summary>
    public float OffsetY
    {
        get => GetCurrentFrame().OffsetY;
        set { }
    }

    /// <summary>
    /// Gets the index into the animation images array
    /// </summary>
    public int GetCurrentFrameIndex()
    {
        var currentTime = ( int ) ( _lastTiledMapRenderTime % _loopDuration );

        for ( var i = 0; i < _animationIntervals.Length; ++i )
        {
            var animationInterval = _animationIntervals[ i ];

            if ( currentTime <= animationInterval )
            {
                return i;
            }

            currentTime -= animationInterval;
        }

        throw new GdxRuntimeException
            ( "Could not determine current animation frame in AnimatedTiledMapTile. This should never happen." );
    }

    /// <summary>
    /// Update the animation time
    /// </summary>
    public static void UpdateAnimationBaseTime()
    {
        _lastTiledMapRenderTime = ( DateTime.Now.Millisecond - _initialTimeOffset );
    }

    public MapProperties Properties => _properties ??= new MapProperties();

    public MapObjects MapObjects => _mapObjects ??= new MapObjects();

    public StaticTiledMapTile[] GetFrameTiles() => _frameTiles;

    public ITiledMapTile GetCurrentFrame() => _frameTiles[ GetCurrentFrameIndex() ];
}
