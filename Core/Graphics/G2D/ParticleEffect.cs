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
    private          BoundingBox?             _bounds;
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

            if ( !emitter.IsComplete() ) return false;
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

    /** Returns the emitter with the specified name, or null. */
    public ParticleEmitter? FindEmitter( string name )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( emitter.GetName().Equals( name ) ) return emitter;
        }

        return null;
    }

    /** Allocates all emitters particles. See {@link com.badlogic.gdx.graphics.g2d.ParticleEmitter#preAllocateParticles()} */
    public void PreAllocateParticles()
    {
        foreach ( ParticleEmitter emitter in _emitters )
        {
            emitter.PreAllocateParticles();
        }
    }

    public void Save( StreamWriter output )
    {
        int index = 0;

        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( index++ > 0 ) output.Write( "\n" );

            emitter.Save( output );
        }
    }

    public void Load( FileInfo effectFile, FileInfo imagesDir )
    {
        LoadEmitters( effectFile );
        LoadEmitterImages( imagesDir );
    }

    public void Load( FileInfo effectFile, TextureAtlas atlas, string? atlasPrefix = null )
    {
        LoadEmitters( effectFile );
        LoadEmitterImages( atlas, atlasPrefix );
    }

    public void LoadEmitters( FileInfo effectFile )
    {
        StreamReader input = effectFile.Read();
        _emitters.Clear();
        
        BufferedReader reader = null;

        try
        {
            reader = new BufferedReader( new StreamReader( input ), 512 );

            while ( true )
            {
                ParticleEmitter emitter = NewEmitter( reader );
                _emitters.Add( emitter );

                if ( reader.readLine() == null )
                {
                    break;
                }
            }
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Error loading effect: " + effectFile, ex );
        }
        finally
        {
            StreamUtils.CloseQuietly( reader );
        }
    }

    public void LoadEmitterImages( TextureAtlas atlas, string? atlasPrefix = null )
    {
        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( _emitters.GetImagePaths().size == 0 ) continue;

            var sprites = new List< Sprite >();

            foreach ( string imagePath in _emitters.GetImagePaths() )
            {
                string imageName    = new File( imagePath.Replace( '\\', '/' ) ).getName();
                var    lastDotIndex = imageName.LastIndexOf( '.' );

                if ( lastDotIndex != -1 ) imageName  = imageName.Substring( 0, lastDotIndex );
                if ( atlasPrefix != null ) imageName = atlasPrefix + imageName;

                Sprite? sprite = atlas.CreateSprite( imageName );

                if ( sprite == null ) throw new ArgumentException( "SpriteSheet missing image: " + imageName );

                sprites.Add( sprite );
            }

            emitter.SetSprites( sprites );
        }
    }

    public void LoadEmitterImages( DirectoryInfo imagesDir )
    {
        _ownsTexture = true;

        Dictionary< string, Sprite? > loadedSprites = new( _emitters.Count );

        for ( int i = 0, n = _emitters.Count; i < n; i++ )
        {
            ParticleEmitter emitter = _emitters[ i ];

            if ( _emitters.GetImagePaths().size == 0 ) continue;

            var sprites = new List< Sprite >();

            foreach ( string imagePath in _emitters.GetImagePaths() )
            {
                string imageName = File( imagePath.Replace( '\\', '/' ) ).GetName();

                Sprite? sprite = loadedSprites[ imageName ];

                if ( sprite == null )
                {
                    sprite = new Sprite( LoadTexture( imagesDir.Child( imageName ) ) );

                    loadedSprites[ imageName ] = sprite;
                }

                sprites.Add( sprite );
            }

            emitter.SetSprites( sprites );
        }
    }

    protected ParticleEmitter NewEmitter( BufferedStream reader )
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
    /// Sets the 'CleansUpBlendFunction' <see cref="ParticleEmitter"/> currently in this ParticleEffect.
    /// <para>
    /// IMPORTANT: If set to false and if the next object to use this Batch expects alpha blending,
    /// you are responsible for setting the Batch's blend function to (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA)
    /// before that next object is drawn.
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
