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

using LibGDXSharp.G2D;
using LibGDXSharp.Utils;

using Blendmode = LibGDXSharp.Maps.Tiled.ITiledMapTile.Blendmode;

namespace LibGDXSharp.Maps.Tiled.Tiles;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class AnimatedTiledMapTile : ITiledMapTile
{
    private readonly static long INITIAL_TIME_OFFSET = DateTime.Now.Millisecond;

    private static long _lastTiledMapRenderTime = 0;

    public int       ID        { get; set; }
    public Blendmode BlendMode { get; set; } = Blendmode.Alpha;

    private readonly int                  _loopDuration;
    private readonly StaticTiledMapTile[] _frameTiles;
    private readonly int[]                _animationIntervals;

    private MapProperties? _properties;
    private MapObjects?    _mapObjects;

    /// <summary>
    /// Creates an animated tile with the given animation interval and frame tiles.
    /// </summary>
    /// <param name="interval">The interval between each individual frame tile.</param>
    /// <param name="frameTiles">
    /// An array of <see cref="StaticTiledMapTile"/> that make up the animation.
    /// </param>
    public AnimatedTiledMapTile( float interval, List< StaticTiledMapTile > frameTiles )
    {
        this._frameTiles = new StaticTiledMapTile[ frameTiles.Count ];

        this._loopDuration       = frameTiles.Count * ( int )( interval * 1000f );
        this._animationIntervals = new int[ frameTiles.Count ];

        for ( var i = 0; i < frameTiles.Count; ++i )
        {
            this._frameTiles[ i ]         = frameTiles[ i ];
            this._animationIntervals[ i ] = ( int )( interval * 1000f );
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
        this._frameTiles = new StaticTiledMapTile[ frameTiles.Count ];

        this._animationIntervals = intervals.ToArray();
        this._loopDuration       = 0;

        for ( int i = 0; i < intervals.Count; ++i )
        {
            this._frameTiles[ i ] =  frameTiles[ i ];
            this._loopDuration    += intervals[ i ];
        }
    }

    public StaticTiledMapTile[] GetFrameTiles() => _frameTiles;

    public ITiledMapTile GetCurrentFrame() => _frameTiles[ GetCurrentFrameIndex() ];

    public int GetCurrentFrameIndex()
    {
        var currentTime = ( int )( _lastTiledMapRenderTime % _loopDuration );

        for ( var i = 0; i < _animationIntervals.Length; ++i )
        {
            var animationInterval = _animationIntervals[ i ];

            if ( currentTime <= animationInterval )
            {
                return i;
            }

            currentTime -= animationInterval;
        }

        throw new SystemException
            (
             "Could not determine current animation frame in AnimatedTiledMapTile.  This should never happen."
            );
    }

    public static void UpdateAnimationBaseTime()
    {
        _lastTiledMapRenderTime = DateTime.Now.Millisecond - INITIAL_TIME_OFFSET;
    }

    public TextureRegion TextureRegion
    {
        get => GetCurrentFrame().TextureRegion;
        set => throw new GdxRuntimeException( "Illegal action: Accessor only." );
    }

    public float OffsetX
    {
        get => GetCurrentFrame().OffsetX;
        set => throw new GdxRuntimeException( "Illegal action: Accessor only." );
    }

    public float OffsetY
    {
        get => GetCurrentFrame().OffsetY;
        set => throw new GdxRuntimeException( "Illegal action: Accessor only." );
    }

    public MapProperties GetProperties()
    {
        return _properties ??= new MapProperties();
    }

    public MapObjects GetObjects()
    {
        return _mapObjects ??= new MapObjects();
    }
}