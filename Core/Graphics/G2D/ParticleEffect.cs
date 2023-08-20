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

using LibGDXSharp.Maths.Collision;
using LibGDXSharp.Utils;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class ParticleEffect : IDisposable
{
    private const int DEFAULT_EMITTERS_SIZE = 8;

    protected float xSizeScale  = 1f;
    protected float ySizeScale  = 1f;
    protected float motionScale = 1f;

    private readonly List< ParticleEmitter > _emitters;
    private          BoundingBox?            _bounds;
    private          bool                    _ownsTexture;

    public ParticleEffect()
    {
        _emitters = new List< ParticleEmitter >( DEFAULT_EMITTERS_SIZE );
    }

    public ParticleEffect( ParticleEffect effect )
    {
        _emitters = new List< ParticleEmitter >( effect._emitters.Count );

        for ( int i = 0, n = effect._emitters.Count; i < n; i++ )
        {
            _emitters.Add( NewEmitter( effect._emitters[ i ] ) );
        }
    }

    public void Start()
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].Start();
        }
    }

    /// <summary>
    /// Resets the effect so it can be started again like a new effect.
    /// </summary>
    /// <param name="resetScaling">
    /// Whether to restore the original size and motion parameters if they
    /// were scaled. Repeated scaling and resetting may introduce error.
    /// </param>
    public void Reset( bool resetScaling = true )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].Reset();
        }

        if ( resetScaling && ( xSizeScale is not 1f || ySizeScale is not 1f || motionScale is not 1f ) )
        {
            ScaleEffect( 1f / xSizeScale, 1f / ySizeScale, 1f / motionScale );
            xSizeScale = ySizeScale = motionScale = 1f;
        }
    }

    public void Update( float delta )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].Update( delta );
        }
    }

    public void Draw( IBatch spriteBatch )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].Draw( spriteBatch );
        }
    }

    public void Draw( IBatch spriteBatch, float delta )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].Draw( spriteBatch, delta );
        }
    }

    public void AllowCompletion()
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].AllowCompletion();
        }
    }

    public bool IsComplete()
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( !emitter.IsComplete() )
            {
                return false;
            }
        }

        return true;
    }

    public void SetDuration( int duration )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            emitter.SetContinuous( false );
            emitter.Duration      = duration;
            emitter.DurationTimer = 0;
        }
    }

    public void SetPosition( float x, float y )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].SetPosition( x, y );
        }
    }

    public void SetFlip( bool flipX, bool flipY )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].SetFlip( flipX, flipY );
        }
    }

    public void FlipY()
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].FlipY();
        }
    }

    public List< ParticleEmitter > GetEmitters() => _emitters;

    /// <summary>
    /// Returns the emitter with the specified name, or null.
    /// </summary>
    public ParticleEmitter? FindEmitter( string name )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( emitter.GetName().Equals( name ) )
            {
                return emitter;
            }
        }

        return null;
    }

    /// <summary>
    /// Allocates all emitters particles.
    /// See <see cref="ParticleEmitter.PreAllocateParticles()"/>
    /// </summary>
    public void PreAllocateParticles()
    {
        foreach ( ParticleEmitter emitter in _emitters )
        {
            emitter.PreAllocateParticles();
        }
    }

    public void Save( StreamWriter output )
    {
        var index = 0;

        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( index++ > 0 )
            {
                output.Write( "\n" );
            }

            emitter.Save( output );
        }
    }

    public void Load( FileInfo effectFile, DirectoryInfo imagesDir )
    {
        LoadEmitters( effectFile );
        LoadEmitterImages( imagesDir );
    }

    public void Load( FileInfo effectFile, TextureAtlas atlas, string? atlasPrefix = null )
    {
        LoadEmitters( effectFile );
        LoadEmitterImages( atlas, atlasPrefix );
    }

    /// <summary>
    /// Loads particle emitters from the specified <paramref name="effectFile"/> and
    /// adds them to the collection.
    /// </summary>
    /// <param name="effectFile">The file containing the particle effect configuration.</param>
    /// <remarks>
    /// <para>
    /// This method reads the configuration from the <paramref name="effectFile"/> and
    /// creates particle emitters based on the provided information. Each emitter is
    /// constructed using the <see cref="NewEmitter(StreamReader)"/> method, and it is
    /// added to the collection of emitters.
    /// </para>
    /// <para>
    /// After loading all emitters, the existing emitters in the collection are cleared
    /// before adding the newly loaded ones.
    /// </para>
    /// </remarks>
    /// <exception cref="GdxRuntimeException">
    /// Thrown if an error occurs while loading the effect from the <paramref name="effectFile"/>.
    /// </exception>
    public void LoadEmitters( FileInfo effectFile )
    {
        Stream input = effectFile.OpenRead();
        
        _emitters.Clear();

        try
        {
            var reader = new StreamReader( input );

            while ( true )
            {
                ParticleEmitter emitter = NewEmitter( reader );
                _emitters.Add( emitter );

                if ( reader.ReadLine() == null )
                {
                    break;
                }
            }
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Error loading effect: " + effectFile, ex );
        }
    }

    /// <summary>
    /// Loads emitter images from a <paramref name="atlas"/> and associates them with
    /// the respective emitters.
    /// </summary>
    /// <param name="atlas">The texture atlas containing the emitter images.</param>
    /// <param name="atlasPrefix">An optional prefix to apply to image names in the atlas.</param>
    /// <remarks>
    /// <para>
    /// This method loads images from the provided <paramref name="atlas"/> and associates
    /// them with the corresponding emitters in the collection. Each emitter's image paths
    /// are retrieved using the <see cref="ParticleEmitter.GetImagePaths"/> method.
    /// If an emitter has no image paths, it is skipped.
    /// </para>
    /// <para>
    /// The method attempts to create sprites from the atlas using the specified
    /// <paramref name="atlasPrefix"/> for image names.
    /// If a sprite cannot be created, an <see cref="ArgumentException"/> is thrown,
    /// indicating the missing image.
    /// </para>
    /// </remarks>
    /// <seealso cref="ParticleEmitter.GetImagePaths"/>
    /// <seealso cref="ParticleEmitter.SetSprites"/>
    public void LoadEmitterImages( TextureAtlas atlas, string? atlasPrefix = null )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( !emitter.GetImagePaths().Any() )
            {
                continue;
            }

            var sprites = new List< Sprite >();

            foreach ( var imagePath in emitter.GetImagePaths() )
            {
                var imageName    = Path.GetFileName( imagePath.Replace( '\\', '/' ) );
                var lastDotIndex = imageName.LastIndexOf( '.' );

                if ( lastDotIndex != -1 )
                {
                    imageName = imageName.Substring( 0, lastDotIndex );
                }

                if ( atlasPrefix != null )
                {
                    imageName = atlasPrefix + imageName;
                }

                Sprite? sprite = atlas.CreateSprite( imageName );

                if ( sprite == null )
                {
                    throw new ArgumentException( "SpriteSheet missing image: " + imageName );
                }

                sprites.Add( sprite );
            }

            emitter.SetSprites( sprites );
        }
    }

    /// <summary>
    /// Loads emitter images from the specified directory and associates them with
    /// the respective emitters.
    /// </summary>
    /// <param name="imagesDir">The directory containing the emitter images.</param>
    /// <remarks>
    /// <para>
    /// This method loads images from the <paramref name="imagesDir"/> directory and
    /// associates them with the corresponding emitters in the collection. Each emitter's
    /// image paths are retrieved using the <see cref="ParticleEmitter.GetImagePaths()"/>
    /// method. If an emitter has no image paths, it is skipped.
    /// </para>
    /// <para>
    /// The loaded sprites are stored in a dictionary to avoid reloading the same sprite
    /// if it has already been loaded. This improves performance by reusing already loaded
    /// textures.
    /// </para>
    /// </remarks>
    /// <seealso cref="ParticleEmitter.GetImagePaths"/>
    /// <seealso cref="ParticleEmitter.SetSprites"/>
    public void LoadEmitterImages( DirectoryInfo imagesDir )
    {
        _ownsTexture = true;

        Dictionary< string, Sprite? > loadedSprites = new( _emitters.Count );

        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            if ( !_emitters[ i ].GetImagePaths().Any() )
            {
                continue;
            }

            var sprites = new List< Sprite >();

            foreach ( var imagePath in _emitters[ i ].GetImagePaths() )
            {
                var imageName = Path.GetFileName( imagePath.Replace( '\\', '/' ) );

                Sprite? sprite = loadedSprites[ imageName ];

                if ( sprite == null )
                {
                    sprite = new Sprite( LoadTexture( new FileInfo( imagePath ) ) );

                    loadedSprites[ imageName ] = sprite;
                }

                sprites.Add( sprite );
            }

            _emitters[ i ].SetSprites( sprites );
        }
    }

    // ------------------------------------------------------------------------

    protected ParticleEmitter NewEmitter( StreamReader reader )
    {
        return new ParticleEmitter( reader );
    }

    protected ParticleEmitter NewEmitter( ParticleEmitter emitter )
    {
        return new ParticleEmitter( emitter );
    }

    protected Texture LoadTexture( FileInfo file )
    {
        return new Texture( file, false );
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Disposes the texture for each sprite for each ParticleEmitter.
    /// </summary>
    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( !_ownsTexture )
            {
                return;
            }

            for ( int i = 0, n = _emitters.Count; i < n; i++ )
            {
                ParticleEmitter emitter = _emitters[ i ];

                foreach ( Sprite sprite in emitter.GetSprites() )
                {
                    sprite.Texture.Dispose();
                }
            }
        }
    }

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns the bounding box for all active particles. z axis will always be zero.
    /// </summary>
    public BoundingBox GetBoundingBox()
    {
        _bounds ??= new BoundingBox();

        BoundingBox box = this._bounds;
        box.ToInfinity();

        foreach ( ParticleEmitter emitter in this._emitters )
        {
            box.Extend( emitter.BoundingBox );
        }

        return box;
    }

    /// <summary>
    /// Permanently scales all the size and motion parameters of all the emitters
    /// in this effect. If this effect originated from a <see cref="ParticleEffectPool"/>,
    /// the scale will be reset when it is returned to the pool.
    /// </summary>
    /// <param name="scaleFactor"></param>
    public void ScaleEffect( float scaleFactor )
    {
        ScaleEffect( scaleFactor, scaleFactor, scaleFactor );
    }

    /// <summary>
    /// Permanently scales all the size and motion parameters of all the emitters
    /// in this effect. If this effect originated from a <see cref="ParticleEffectPool"/>,
    /// the scale will be reset when it is returned to the pool.
    /// </summary>
    /// <param name="scaleFactor"></param>
    /// <param name="motionScaleFactor"></param>
    public void ScaleEffect( float scaleFactor, float motionScaleFactor )
    {
        ScaleEffect( scaleFactor, scaleFactor, motionScaleFactor );
    }

    /// <summary>
    /// Permanently scales all the size and motion parameters of all the emitters
    /// in this effect. If this effect originated from a <see cref="ParticleEffectPool"/>,
    /// the scale will be reset when it is returned to the pool.
    /// </summary>
    /// <param name="xSizeScaleFactor"></param>
    /// <param name="ySizeScaleFactor"></param>
    /// <param name="motionScaleFactor"></param>
    public void ScaleEffect( float xSizeScaleFactor, float ySizeScaleFactor, float motionScaleFactor )
    {
        xSizeScale  *= xSizeScaleFactor;
        ySizeScale  *= ySizeScaleFactor;
        motionScale *= motionScaleFactor;

        foreach ( ParticleEmitter particleEmitter in _emitters )
        {
            particleEmitter.ScaleSize( xSizeScaleFactor, ySizeScaleFactor );
            particleEmitter.ScaleMotion( motionScaleFactor );
        }
    }

    /// <summary>
    /// Sets the 'CleansUpBlendFunction' <see cref="ParticleEmitter"/> currently
    /// in this ParticleEffect.
    /// <para>
    /// IMPORTANT: If set to false and if the next object to use this Batch expects
    /// alpha blending, you are responsible for setting the Batch's blend function
    /// to (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA) before that next object is drawn.
    /// </para>
    /// </summary>
    /// <param name="cleanUpBlendFunction"></param>
    public void SetEmittersCleanUpBlendFunction( bool cleanUpBlendFunction )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            _emitters[ i ].SetCleansUpBlendFunction( cleanUpBlendFunction );
        }
    }
}
