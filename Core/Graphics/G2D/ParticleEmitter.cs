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

using System.Drawing;

using LibGDXSharp.Maths.Collision;

namespace LibGDXSharp.G2D;

[PublicAPI]
public class ParticleEmitter
{
    public float duration = 1;
    public float durationTimer;
    public bool  Behind { get; set; }

    protected bool[] isActive;

    private readonly static int UpdateScale    = 1 << 0;
    private readonly static int UpdateAngle    = 1 << 1;
    private readonly static int UpdateRotation = 1 << 2;
    private readonly static int UpdateVelocity = 1 << 3;
    private readonly static int UpdateWind     = 1 << 4;
    private readonly static int UpdateGravity  = 1 << 5;
    private readonly static int UpdateTint     = 1 << 6;
    private readonly static int UpdateSprite   = 1 << 7;

    private RangedNumericValue            _delayValue        = new();
    private IndependentScaledNumericValue _lifeOffsetValue   = new();
    private RangedNumericValue            _durationValue     = new();
    private IndependentScaledNumericValue _lifeValue         = new();
    private ScaledNumericValue            _emissionValue     = new();
    private ScaledNumericValue            _xScaleValue       = new();
    private ScaledNumericValue            _yScaleValue       = new();
    private ScaledNumericValue            _rotationValue     = new();
    private ScaledNumericValue            _velocityValue     = new();
    private ScaledNumericValue            _angleValue        = new();
    private ScaledNumericValue            _windValue         = new();
    private ScaledNumericValue            _gravityValue      = new();
    private ScaledNumericValue            _transparencyValue = new();
    private GradientColorValue            _tintValue         = new();
    private RangedNumericValue            _xOffsetValue      = new ScaledNumericValue();
    private RangedNumericValue            _yOffsetValue      = new ScaledNumericValue();
    private ScaledNumericValue            _spawnWidthValue   = new();
    private ScaledNumericValue            _spawnHeightValue  = new();
    private SpawnShapeValue               _spawnShapeValue   = new();

    private RangedNumericValue[] _xSizeValues;
    private RangedNumericValue[] _ySizeValues;
    private RangedNumericValue[] _motionValues;

    private float          _accumulator;
    private List< Sprite > _sprites;
    private SpriteMode     _spriteMode = SpriteMode.Single;
    private Particle[]     _particles;
    private int            _minParticleCount;
    private int            _maxParticleCount = 4;
    private float          _x;
    private float          _y;
    private string         _name;
    private List< string > _imagePaths;
    private int            _activeCount;
    private bool           _firstUpdate;
    private bool           _flipX;
    private bool           _flipY;
    private int            _updateFlags;
    private bool           _allowCompletion;
    private BoundingBox    _bounds;

    private int   _emission;
    private int   _emissionDiff;
    private int   _emissionDelta;
    private int   _lifeOffset;
    private int   _lifeOffsetDiff;
    private int   _life;
    private int   _lifeDiff;
    private float _spawnWidth;
    private float _spawnWidthDiff;
    private float _spawnHeight;
    private float _spawnHeightDiff;
    private float _delay;
    private float _delayTimer;

    private bool _attached;
    private bool _continuous;
    private bool _aligned;
    private bool _additive              = true;
    private bool _premultipliedAlpha    = false;
    private bool _cleansUpBlendFunction = true;

    public ParticleEmitter()
    {
        Initialize();
    }

    public ParticleEmitter( BufferedReader reader )
    {
        Initialize();
        Load( reader );
    }

    public ParticleEmitter( ParticleEmitter emitter )
    {
        _sprites    = new List< Sprite >( emitter._sprites );
        _name       = emitter._name;
        _imagePaths = new List< string >( emitter._imagePaths );

        SetMaxParticleCount( emitter._maxParticleCount );

        _minParticleCount = emitter._minParticleCount;
        _delayValue.Load( emitter._delayValue );
        _durationValue.Load( emitter._durationValue );
        _emissionValue.Load( emitter._emissionValue );
        _lifeValue.Load( emitter._lifeValue );
        _lifeOffsetValue.Load( emitter._lifeOffsetValue );
        _xScaleValue.Load( emitter._xScaleValue );
        _yScaleValue.Load( emitter._yScaleValue );
        _rotationValue.Load( emitter._rotationValue );
        _velocityValue.Load( emitter._velocityValue );
        _angleValue.Load( emitter._angleValue );
        _windValue.Load( emitter._windValue );
        _gravityValue.Load( emitter._gravityValue );
        _transparencyValue.Load( emitter._transparencyValue );
        _tintValue.Load( emitter._tintValue );
        _xOffsetValue.Load( emitter._xOffsetValue );
        _yOffsetValue.Load( emitter._yOffsetValue );
        _spawnWidthValue.Load( emitter._spawnWidthValue );
        _spawnHeightValue.Load( emitter._spawnHeightValue );
        _spawnShapeValue.Load( emitter._spawnShapeValue );

        _attached           = emitter._attached;
        _continuous         = emitter._continuous;
        _aligned            = emitter._aligned;
        Behind              = emitter.Behind;
        _additive           = emitter._additive;
        _premultipliedAlpha = emitter._premultipliedAlpha;

        _cleansUpBlendFunction = emitter._cleansUpBlendFunction;
        _spriteMode            = emitter._spriteMode;

        SetPosition( emitter.GetX(), emitter.GetY() );
    }

    private void Initialize()
    {
        _sprites    = new List< Sprite >();
        _imagePaths = new List< string >();

        _durationValue.SetAlwaysActive( true );
        _emissionValue.SetAlwaysActive( true );
        _lifeValue.SetAlwaysActive( true );
        _xScaleValue.SetAlwaysActive( true );
        _transparencyValue.SetAlwaysActive( true );
        _spawnShapeValue.SetAlwaysActive( true );
        _spawnWidthValue.SetAlwaysActive( true );
        _spawnHeightValue.SetAlwaysActive( true );
    }

    public void SetMaxParticleCount( int maxParticleCount )
    {
        _maxParticleCount = maxParticleCount;
        isActive          = new bool[ maxParticleCount ];
        _activeCount      = 0;
        _particles        = new Particle[ maxParticleCount ];
    }

    public void AddParticle()
    {
        var activeCount = this._activeCount;

        if ( activeCount == _maxParticleCount )
        {
            return;
        }

        var active = this.isActive;

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            if ( !active[ i ] )
            {
                ActivateParticle( i );
                active[ i ]       = true;
                this._activeCount = activeCount + 1;

                break;
            }
        }
    }

    public void AddParticles( int count )
    {
        count = Math.Min( count, _maxParticleCount - _activeCount );

        if ( count == 0 )
        {
            return;
        }

        var active = this.isActive;
        int index  = 0, n = active.Length;

        outer:

        for ( var i = 0; i < count; i++ )
        {
            for ( ; index < n; index++ )
            {
                if ( !active[ index ] )
                {
                    ActivateParticle( index );
                    active[ index++ ] = true;

                    goto outer;
                }
            }

            break;
        }

        this._activeCount += count;
    }

    public void Update( float delta )
    {
        _accumulator += delta * 1000;

        if ( _accumulator < 1 )
        {
            return;
        }

        var deltaMillis = ( int )_accumulator;
        _accumulator -= deltaMillis;

        if ( _delayTimer < _delay )
        {
            _delayTimer += deltaMillis;
        }
        else
        {
            var done = false;

            if ( _firstUpdate )
            {
                _firstUpdate = false;
                AddParticle();
            }

            if ( durationTimer < duration )
            {
                durationTimer += deltaMillis;
            }
            else
            {
                if ( !_continuous || _allowCompletion )
                {
                    done = true;
                }
                else
                {
                    Restart();
                }
            }

            if ( !done )
            {
                _emissionDelta += deltaMillis;
                var emissionTime = _emission + ( _emissionDiff * _emissionValue.GetScale( durationTimer / ( float )duration ) );

                if ( emissionTime > 0 )
                {
                    emissionTime = 1000 / emissionTime;

                    if ( _emissionDelta >= emissionTime )
                    {
                        var emitCount = ( int )( _emissionDelta / emissionTime );
                        emitCount = Math.Min( emitCount, _maxParticleCount - _activeCount );

                        _emissionDelta -= emitCount * emissionTime;
                        _emissionDelta %= emissionTime;

                        AddParticles( emitCount );
                    }
                }

                if ( _activeCount < _minParticleCount )
                {
                    AddParticles( _minParticleCount - _activeCount );
                }
            }
        }

        Particle[] particles = this._particles;

        for ( int i = 0, n = this.isActive.Length; i < n; i++ )
        {
            if ( this.isActive[ i ] && !UpdateParticle( particles[ i ], delta, deltaMillis ) )
            {
                this.isActive[ i ] = false;
                this._activeCount--;
            }
        }
    }

    public void Draw( IBatch batch )
    {
        if ( _premultipliedAlpha )
        {
            batch.SetBlendFunction( IGL20.GL_ONE, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }
        else if ( _additive )
        {
            batch.SetBlendFunction( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE );
        }
        else
        {
            batch.SetBlendFunction( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }

        Particle[] particles = this._particles;
        var        active    = this.isActive;

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            if ( active[ i ] )
            {
                particles[ i ].Draw( batch );
            }
        }

        if ( _cleansUpBlendFunction && ( _additive || _premultipliedAlpha ) )
        {
            batch.SetBlendFunction( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }
    }

    /// <summary>
    /// Updates and draws the particles. This is slightly more efficient
    /// than calling {@link #update(float)} and <see cref="Draw(IBatch)"/>"
    /// separately.
    /// </summary>
    public void Draw( IBatch batch, float delta )
    {
        _accumulator += delta * 1000;

        if ( _accumulator < 1 )
        {
            Draw( batch );

            return;
        }

        var deltaMillis = ( int )_accumulator;
        _accumulator -= deltaMillis;

        if ( _premultipliedAlpha )
        {
            batch.SetBlendFunction( IGL20.GL_ONE, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }
        else if ( _additive )
        {
            batch.SetBlendFunction( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE );
        }
        else
        {
            batch.SetBlendFunction( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }

        Particle[] particles   = this._particles;
        var        active      = this.isActive;
        var        activeCount = this._activeCount;

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            if ( active[ i ] )
            {
                Particle particle = particles[ i ];

                if ( UpdateParticle( particle, delta, deltaMillis ) )
                {
                    particle.Draw( batch );
                }
                else
                {
                    active[ i ] = false;
                    activeCount--;
                }
            }
        }

        this._activeCount = activeCount;

        if ( _cleansUpBlendFunction && ( _additive || _premultipliedAlpha ) )
        {
            batch.SetBlendFunction( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }

        if ( _delayTimer < _delay )
        {
            _delayTimer += deltaMillis;

            return;
        }

        if ( _firstUpdate )
        {
            _firstUpdate = false;
            AddParticle();
        }

        if ( durationTimer < duration )
        {
            durationTimer += deltaMillis;
        }
        else
        {
            if ( !_continuous || _allowCompletion )
            {
                return;
            }

            Restart();
        }

        _emissionDelta += deltaMillis;
        var emissionTime = _emission + ( _emissionDiff * _emissionValue.GetScale( durationTimer / ( float )duration ) );

        if ( emissionTime > 0 )
        {
            emissionTime = 1000 / emissionTime;

            if ( _emissionDelta >= emissionTime )
            {
                var emitCount = ( int )( _emissionDelta / emissionTime );
                emitCount = Math.Min( emitCount, _maxParticleCount - activeCount );

                _emissionDelta -= ( int )( emitCount * emissionTime );
                _emissionDelta %= ( int )emissionTime;

                AddParticles( emitCount );
            }
        }

        if ( activeCount < _minParticleCount )
        {
            AddParticles( _minParticleCount - activeCount );
        }
    }

    public void Start()
    {
        _firstUpdate     = true;
        _allowCompletion = false;
        Restart();
    }

    public void Reset()
    {
        _emissionDelta = 0;
        durationTimer  = duration;
        var active = this.isActive;

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            active[ i ] = false;
        }

        _activeCount = 0;
        Start();
    }

    private void Restart()
    {
        _delay      = _delayValue._active ? _delayValue.NewLowValue() : 0;
        _delayTimer = 0;

        durationTimer -= duration;
        duration      =  _durationValue.NewLowValue();

        _emission     = ( int )_emissionValue.NewLowValue();
        _emissionDiff = ( int )_emissionValue.NewHighValue();

        if ( !_emissionValue.ISRelative() )
        {
            _emissionDiff -= _emission;
        }

        if ( !_lifeValue.Independent )
        {
            GenerateLifeValues();
        }

        if ( !_lifeOffsetValue.Independent )
        {
            GenerateLifeOffsetValues();
        }

        _spawnWidth     = _spawnWidthValue.NewLowValue();
        _spawnWidthDiff = _spawnWidthValue.NewHighValue();

        if ( !_spawnWidthValue.ISRelative() )
        {
            _spawnWidthDiff -= _spawnWidth;
        }

        _spawnHeight     = _spawnHeightValue.NewLowValue();
        _spawnHeightDiff = _spawnHeightValue.NewHighValue();

        if ( !_spawnHeightValue.ISRelative() )
        {
            _spawnHeightDiff -= _spawnHeight;
        }

        _updateFlags = 0;

        if ( _angleValue._active && ( _angleValue._timeline.Length > 1 ) )
        {
            _updateFlags |= UpdateAngle;
        }

        if ( _velocityValue._active )
        {
            _updateFlags |= UpdateVelocity;
        }

        if ( _xScaleValue._timeline.Length > 1 )
        {
            _updateFlags |= UpdateScale;
        }

        if ( _yScaleValue._active && ( _yScaleValue._timeline.Length > 1 ) )
        {
            _updateFlags |= UpdateScale;
        }

        if ( _rotationValue._active && ( _rotationValue._timeline.Length > 1 ) )
        {
            _updateFlags |= UpdateRotation;
        }

        if ( _windValue._active )
        {
            _updateFlags |= UpdateWind;
        }

        if ( _gravityValue._active )
        {
            _updateFlags |= UpdateGravity;
        }

        if ( _tintValue.Timeline.Length > 1 )
        {
            _updateFlags |= UpdateTint;
        }

        if ( _spriteMode == SpriteMode.animated )
        {
            _updateFlags |= UpdateSprite;
        }
    }

    protected Particle NewParticle( Sprite sprite )
    {
        return new Particle( sprite );
    }

    protected Particle[] GetParticles()
    {
        return _particles;
    }

    private void ActivateParticle( int index )
    {
        Sprite sprite = null;

        switch ( _spriteMode )
        {
            case single:
            case animated:
                sprite = _sprites.first();

                break;

            case random:
                sprite = _sprites.random();

                break;
        }

        Particle particle = _particles[ index ];

        if ( particle == null )
        {
            _particles[ index ] = particle = NewParticle( sprite );
            particle.flip( _flipX, FlipY );
        }
        else
        {
            particle.set( sprite );
        }

        var percent     = durationTimer / ( float )duration;
        var updateFlags = this._updateFlags;

        if ( _lifeValue.Independent )
        {
            GenerateLifeValues();
        }

        if ( _lifeOffsetValue.Independent )
        {
            GenerateLifeOffsetValues();
        }

        particle.currentLife = particle.life = _life + ( int )( _lifeDiff * _lifeValue.GetScale( percent ) );

        if ( _velocityValue._active )
        {
            particle.velocity     = _velocityValue.NewLowValue();
            particle.velocityDiff = _velocityValue.NewHighValue();

            if ( !_velocityValue.ISRelative() )
            {
                particle.velocityDiff -= particle.velocity;
            }
        }

        particle.angle     = _angleValue.NewLowValue();
        particle.angleDiff = _angleValue.NewHighValue();

        if ( !_angleValue.ISRelative() )
        {
            particle.angleDiff -= particle.angle;
        }

        float angle = 0;

        if ( ( updateFlags & UpdateAngle ) == 0 )
        {
            angle             = particle.angle + ( particle.angleDiff * _angleValue.GetScale( 0 ) );
            particle.angle    = angle;
            particle.angleCos = MathUtils.cosDeg( angle );
            particle.angleSin = MathUtils.sinDeg( angle );
        }

        float spriteWidth  = sprite.getWidth();
        float spriteHeight = sprite.getHeight();

        particle.xScale     = _xScaleValue.NewLowValue() / spriteWidth;
        particle.xScaleDiff = _xScaleValue.NewHighValue() / spriteWidth;

        if ( !_xScaleValue.ISRelative() )
        {
            particle.xScaleDiff -= particle.xScale;
        }

        if ( _yScaleValue._active )
        {
            particle.yScale     = _yScaleValue.NewLowValue() / spriteHeight;
            particle.yScaleDiff = _yScaleValue.NewHighValue() / spriteHeight;

            if ( !_yScaleValue.ISRelative() )
            {
                particle.yScaleDiff -= particle.yScale;
            }

            particle.setScale
                (
                 particle.xScale + ( particle.xScaleDiff * _xScaleValue.GetScale( 0 ) ),
                 particle.yScale + ( particle.yScaleDiff * _yScaleValue.GetScale( 0 ) )
                );
        }
        else
        {
            particle.setScale( particle.xScale + ( particle.xScaleDiff * _xScaleValue.GetScale( 0 ) ) );
        }

        if ( _rotationValue._active )
        {
            particle.rotation     = _rotationValue.NewLowValue();
            particle.rotationDiff = _rotationValue.NewHighValue();

            if ( !_rotationValue.ISRelative() )
            {
                particle.rotationDiff -= particle.rotation;
            }

            var rotation = particle.rotation + ( particle.rotationDiff * _rotationValue.GetScale( 0 ) );

            if ( _aligned )
            {
                rotation += angle;
            }

            particle.setRotation( rotation );
        }

        if ( _windValue._active )
        {
            particle.wind     = _windValue.NewLowValue();
            particle.windDiff = _windValue.NewHighValue();

            if ( !_windValue.ISRelative() )
            {
                particle.windDiff -= particle.wind;
            }
        }

        if ( _gravityValue._active )
        {
            particle.gravity     = _gravityValue.NewLowValue();
            particle.gravityDiff = _gravityValue.NewHighValue();

            if ( !_gravityValue.ISRelative() )
            {
                particle.gravityDiff -= particle.gravity;
            }
        }

        var color = particle.tint;

        if ( color == null )
        {
            particle.tint = color = new float[ 3 ];
        }

        var temp = _tintValue.GetColor( 0 );
        color[ 0 ] = temp[ 0 ];
        color[ 1 ] = temp[ 1 ];
        color[ 2 ] = temp[ 2 ];

        particle.transparency     = _transparencyValue.NewLowValue();
        particle.transparencyDiff = _transparencyValue.NewHighValue() - particle.transparency;

        // Spawn.
        var x = this._x;

        if ( _xOffsetValue._active )
        {
            x += _xOffsetValue.NewLowValue();
        }

        var y = this._y;

        if ( _yOffsetValue._active )
        {
            y += _yOffsetValue.NewLowValue();
        }

        switch ( _spawnShapeValue.Shape )
        {
            case square:
            {
                var width  = _spawnWidth + ( _spawnWidthDiff * _spawnWidthValue.GetScale( percent ) );
                var height = _spawnHeight + ( _spawnHeightDiff * _spawnHeightValue.GetScale( percent ) );
                x += MathUtils.random( width ) - ( width / 2 );
                y += MathUtils.random( height ) - ( height / 2 );

                break;
            }

            case ellipse:
            {
                var width   = _spawnWidth + ( _spawnWidthDiff * _spawnWidthValue.GetScale( percent ) );
                var height  = _spawnHeight + ( _spawnHeightDiff * _spawnHeightValue.GetScale( percent ) );
                var radiusX = width / 2;
                var radiusY = height / 2;

                if ( ( radiusX == 0 ) || ( radiusY == 0 ) )
                {
                    break;
                }

                var scaleY = radiusX / ( float )radiusY;

                if ( _spawnShapeValue.Edges )
                {
                    float spawnAngle;

                    switch ( _spawnShapeValue.Side )
                    {
                        case top:
                            spawnAngle = -MathUtils.random( 179f );

                            break;

                        case bottom:
                            spawnAngle = MathUtils.random( 179f );

                            break;

                        default:
                            spawnAngle = MathUtils.random( 360f );

                            break;
                    }

                    float cosDeg = MathUtils.cosDeg( spawnAngle );
                    float sinDeg = MathUtils.sinDeg( spawnAngle );
                    x += cosDeg * radiusX;
                    y += ( sinDeg * radiusX ) / scaleY;

                    if ( ( updateFlags & UpdateAngle ) == 0 )
                    {
                        particle.angle    = spawnAngle;
                        particle.angleCos = cosDeg;
                        particle.angleSin = sinDeg;
                    }
                }
                else
                {
                    var radius2 = radiusX * radiusX;

                    while ( true )
                    {
                        var px = MathUtils.random( width ) - radiusX;
                        var py = MathUtils.random( width ) - radiusX;

                        if ( ( ( px * px ) + ( py * py ) ) <= radius2 )
                        {
                            x += px;
                            y += py / scaleY;

                            break;
                        }
                    }
                }

                break;
            }

            case line:
            {
                var width  = _spawnWidth + ( _spawnWidthDiff * _spawnWidthValue.GetScale( percent ) );
                var height = _spawnHeight + ( _spawnHeightDiff * _spawnHeightValue.GetScale( percent ) );

                if ( width != 0 )
                {
                    var lineX = width * MathUtils.random();
                    x += lineX;
                    y += lineX * ( height / ( float )width );
                }
                else
                {
                    y += height * MathUtils.random();
                }

                break;
            }
        }

        particle.setBounds( x - ( spriteWidth / 2 ), y - ( spriteHeight / 2 ), spriteWidth, spriteHeight );

        var offsetTime = ( int )( _lifeOffset + ( _lifeOffsetDiff * _lifeOffsetValue.GetScale( percent ) ) );

        if ( offsetTime > 0 )
        {
            if ( offsetTime >= particle.currentLife )
            {
                offsetTime = particle.currentLife - 1;
            }

            UpdateParticle( particle, offsetTime / 1000f, offsetTime );
        }
    }

    private bool UpdateParticle( Particle particle, float delta, int deltaMillis )
    {
        var life = particle.currentLife - deltaMillis;

        if ( life <= 0 )
        {
            return false;
        }

        particle.currentLife = life;

        var percent     = 1 - ( particle.currentLife / ( float )particle.life );
        var updateFlags = this._updateFlags;

        if ( ( updateFlags & UpdateScale ) != 0 )
        {
            if ( _yScaleValue._active )
            {
                particle.setScale
                    (
                     particle.xScale + ( particle.xScaleDiff * _xScaleValue.GetScale( percent ) ),
                     particle.yScale + ( particle.yScaleDiff * _yScaleValue.GetScale( percent ) )
                    );
            }
            else
            {
                particle.setScale( particle.xScale + ( particle.xScaleDiff * _xScaleValue.GetScale( percent ) ) );
            }
        }

        if ( ( updateFlags & UpdateVelocity ) != 0 )
        {
            var velocity = ( particle.velocity + ( particle.velocityDiff * _velocityValue.GetScale( percent ) ) ) * delta;

            float velocityX, velocityY;

            if ( ( updateFlags & UpdateAngle ) != 0 )
            {
                var angle = particle.angle + ( particle.angleDiff * _angleValue.GetScale( percent ) );
                velocityX = velocity * MathUtils.cosDeg( angle );
                velocityY = velocity * MathUtils.sinDeg( angle );

                if ( ( updateFlags & UpdateRotation ) != 0 )
                {
                    var rotation = particle.rotation + ( particle.rotationDiff * _rotationValue.GetScale( percent ) );

                    if ( _aligned )
                    {
                        rotation += angle;
                    }

                    particle.setRotation( rotation );
                }
            }
            else
            {
                velocityX = velocity * particle.angleCos;
                velocityY = velocity * particle.angleSin;

                if ( _aligned || ( ( updateFlags & UpdateRotation ) != 0 ) )
                {
                    var rotation = particle.rotation + ( particle.rotationDiff * _rotationValue.GetScale( percent ) );

                    if ( _aligned )
                    {
                        rotation += particle.angle;
                    }

                    particle.setRotation( rotation );
                }
            }

            if ( ( updateFlags & UpdateWind ) != 0 )
            {
                velocityX += ( particle.wind + ( particle.windDiff * _windValue.GetScale( percent ) ) ) * delta;
            }

            if ( ( updateFlags & UpdateGravity ) != 0 )
            {
                velocityY += ( particle.gravity + ( particle.gravityDiff * _gravityValue.GetScale( percent ) ) ) * delta;
            }

            particle.translate( velocityX, velocityY );
        }
        else
        {
            if ( ( updateFlags & UpdateRotation ) != 0 )
            {
                particle.setRotation( particle.rotation + ( particle.rotationDiff * _rotationValue.GetScale( percent ) ) );
            }
        }

        float[] color;

        if ( ( updateFlags & UpdateTint ) != 0 )
        {
            color = _tintValue.GetColor( percent );
        }
        else
        {
            color = particle.tint;
        }

        if ( _premultipliedAlpha )
        {
            float alphaMultiplier = _additive ? 0 : 1;
            var   a               = particle.transparency + ( particle.transparencyDiff * _transparencyValue.GetScale( percent ) );
            particle.setColor( color[ 0 ] * a, color[ 1 ] * a, color[ 2 ] * a, a * alphaMultiplier );
        }
        else
        {
            particle.setColor
                (
                 color[ 0 ], color[ 1 ], color[ 2 ],
                 particle.transparency + ( particle.transparencyDiff * _transparencyValue.GetScale( percent ) )
                );
        }

        if ( ( updateFlags & UpdateSprite ) != 0 )
        {
            var frame = Math.Min( ( int )( percent * _sprites.size ), _sprites.size - 1 );

            if ( particle.frame != frame )
            {
                Sprite sprite           = _sprites.get( frame );
                float  prevSpriteWidth  = particle.getWidth();
                float  prevSpriteHeight = particle.getHeight();
                particle.setRegion( sprite );
                particle.setSize( sprite.getWidth(), sprite.getHeight() );
                particle.setOrigin( sprite.getOriginX(), sprite.getOriginY() );
                particle.translate( ( prevSpriteWidth - sprite.getWidth() ) / 2, ( prevSpriteHeight - sprite.getHeight() ) / 2 );
                particle.frame = frame;
            }
        }

        return true;
    }

    private void GenerateLifeValues()
    {
        _life     = ( int )_lifeValue.NewLowValue();
        _lifeDiff = ( int )_lifeValue.NewHighValue();

        if ( !_lifeValue.ISRelative() )
        {
            _lifeDiff -= _life;
        }
    }

    private void GenerateLifeOffsetValues()
    {
        _lifeOffset     = _lifeOffsetValue._active ? ( int )_lifeOffsetValue.NewLowValue() : 0;
        _lifeOffsetDiff = ( int )_lifeOffsetValue.NewHighValue();

        if ( !_lifeOffsetValue.ISRelative() )
        {
            _lifeOffsetDiff -= _lifeOffset;
        }
    }

    public void SetPosition( float x, float y )
    {
        if ( _attached )
        {
            var xAmount = x - this._x;
            var yAmount = y - this._y;
            var active  = this.isActive;

            for ( int i = 0, n = active.Length; i < n; i++ )
            {
                if ( active[ i ] )
                {
                    _particles[ i ].translate( xAmount, yAmount );
                }
            }
        }

        this._x = x;
        this._y = y;
    }

    public void SetSprites( List< Sprite > sprites )
    {
        this._sprites = sprites;

        if ( sprites.size == 0 )
        {
            return;
        }

        for ( int i = 0, n = _particles.Length; i < n; i++ )
        {
            Particle particle = _particles[ i ];

            if ( particle == null )
            {
                break;
            }

            Sprite sprite = null;

            switch ( _spriteMode )
            {
                case single:
                    sprite = sprites.first();

                    break;

                case random:
                    sprite = sprites.random();

                    break;

                case animated:
                    var percent = 1 - ( particle.currentLife / ( float )particle.life );
                    particle.frame = Math.Min( ( int )( percent * sprites.size ), sprites.size - 1 );
                    sprite         = sprites.get( particle.frame );

                    break;
            }

            particle.setRegion( sprite );
            particle.setOrigin( sprite.getOriginX(), sprite.getOriginY() );
        }
    }

    public void SetSpriteMode( SpriteMode spriteMode )
    {
        this._spriteMode = spriteMode;
    }

    /**
     * Allocates max particles emitter can hold. Usually called early on to avoid allocation on updates.
     * {@link #setSprites(List)} must have been set before calling this method
     */
    public void PreAllocateParticles()
    {
        if ( _sprites.isEmpty() )
        {
            throw new IllegalStateException( "ParticleEmitter.setSprites() must have been called before preAllocateParticles()" );
        }

        for ( var index = 0; index < _particles.Length; index++ )
        {
            Particle particle = _particles[ index ];

            if ( particle == null )
            {
                _particles[ index ] = particle = NewParticle( _sprites.first() );
                particle.flip( _flipX, FlipY );
            }
        }
    }


    /** Ignores the {@link #setContinuous(bool) continuous} setting until the emitter is started again. This allows the emitter
     * to stop smoothly. */
    public void AllowCompletion()
    {
        _allowCompletion = true;
        durationTimer    = duration;
    }

    public List< Sprite > GetSprites()
    {
        return _sprites;
    }

    public SpriteMode GetSpriteMode()
    {
        return _spriteMode;
    }

    public string GetName()
    {
        return _name;
    }

    public void SetName( string name )
    {
        this._name = name;
    }

    public ScaledNumericValue GetLife()
    {
        return _lifeValue;
    }

    public ScaledNumericValue GetXScale()
    {
        return _xScaleValue;
    }

    public ScaledNumericValue GetYScale()
    {
        return _yScaleValue;
    }

    public ScaledNumericValue GetRotation()
    {
        return _rotationValue;
    }

    public GradientColorValue GetTint()
    {
        return _tintValue;
    }

    public ScaledNumericValue GetVelocity()
    {
        return _velocityValue;
    }

    public ScaledNumericValue GetWind()
    {
        return _windValue;
    }

    public ScaledNumericValue GetGravity()
    {
        return _gravityValue;
    }

    public ScaledNumericValue GetAngle()
    {
        return _angleValue;
    }

    public ScaledNumericValue GetEmission()
    {
        return _emissionValue;
    }

    public ScaledNumericValue GetTransparency()
    {
        return _transparencyValue;
    }

    public RangedNumericValue GetDuration()
    {
        return _durationValue;
    }

    public RangedNumericValue GetDelay()
    {
        return _delayValue;
    }

    public ScaledNumericValue GetLifeOffset()
    {
        return _lifeOffsetValue;
    }

    public RangedNumericValue GetXOffsetValue()
    {
        return _xOffsetValue;
    }

    public RangedNumericValue GetYOffsetValue()
    {
        return _yOffsetValue;
    }

    public ScaledNumericValue GetSpawnWidth()
    {
        return _spawnWidthValue;
    }

    public ScaledNumericValue GetSpawnHeight()
    {
        return _spawnHeightValue;
    }

    public SpawnShapeValue GetSpawnShape()
    {
        return _spawnShapeValue;
    }

    public bool ISAttached()
    {
        return _attached;
    }

    public void SetAttached( bool attached )
    {
        this._attached = attached;
    }

    public bool ISContinuous()
    {
        return _continuous;
    }

    public void SetContinuous( bool continuous )
    {
        this._continuous = continuous;
    }

    public bool ISAligned()
    {
        return _aligned;
    }

    public void SetAligned( bool aligned )
    {
        this._aligned = aligned;
    }

    public bool ISAdditive()
    {
        return _additive;
    }

    public void SetAdditive( bool additive )
    {
        this._additive = additive;
    }

    /**
      * @return Whether this ParticleEmitter automatically returns the {@link com.badlogic.gdx.graphics.g2d.IBatch IBatch}'s blend
      * function to the alpha-blending default (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA) when done drawing.
     */
    public bool CleansUpBlendFunction()
    {
        return _cleansUpBlendFunction;
    }

    /** Set whether to automatically return the {@link com.badlogic.gdx.graphics.g2d.IBatch IBatch}'s blend function to the
     * alpha-blending default (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA) when done drawing. Is true by default. If set to false, the
     * IBatch's blend function is left as it was for drawing this ParticleEmitter, which prevents the IBatch from being flushed
     * repeatedly if consecutive ParticleEmitters with the same additive or pre-multiplied alpha state are drawn in a row.
     * <p>
     * IMPORTANT: If set to false and if the next object to use this IBatch expects alpha blending, you are responsible for setting
     * the IBatch's blend function to (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA) before that next object is drawn.
     * @param cleansUpBlendFunction */
    public void SetCleansUpBlendFunction( bool cleansUpBlendFunction )
    {
        this._cleansUpBlendFunction = cleansUpBlendFunction;
    }

    public bool ISPremultipliedAlpha()
    {
        return _premultipliedAlpha;
    }

    public void SetPremultipliedAlpha( bool premultipliedAlpha )
    {
        this._premultipliedAlpha = premultipliedAlpha;
    }

    public int GetMinParticleCount()
    {
        return _minParticleCount;
    }

    public void SetMinParticleCount( int minParticleCount )
    {
        this._minParticleCount = minParticleCount;
    }

    public int GetMaxParticleCount()
    {
        return _maxParticleCount;
    }

    public bool ISComplete()
    {
        if ( _continuous && !AllowCompletion )
        {
            return false;
        }

        if ( _delayTimer < _delay )
        {
            return false;
        }

        return ( durationTimer >= duration ) && ( _activeCount == 0 );
    }

    public float GetPercentComplete()
    {
        if ( _delayTimer < _delay )
        {
            return 0;
        }

        return Math.Min( 1, durationTimer / ( float )duration );
    }

    public float GetX()
    {
        return _x;
    }

    public float GetY()
    {
        return _y;
    }

    public int GetActiveCount()
    {
        return _activeCount;
    }

    public List< string > GetImagePaths()
    {
        return _imagePaths;
    }

    public void SetImagePaths( List< string > imagePaths )
    {
        this._imagePaths = imagePaths;
    }

    public void SetFlip( bool flipX, bool flipY )
    {
        this._flipX = flipX;
        this.FlipY  = flipY;

        if ( _particles == null )
        {
            return;
        }

        for ( int i = 0, n = _particles.Length; i < n; i++ )
        {
            Particle particle = _particles[ i ];

            if ( particle != null )
            {
                particle.flip( flipX, flipY );
            }
        }
    }

    public void FlipY()
    {
        _angleValue.SetHigh( -_angleValue.GetHighMin(), -_angleValue.GetHighMax() );
        _angleValue.SetLow( -_angleValue.GetLowMin(), -_angleValue.GetLowMax() );

        _gravityValue.SetHigh( -_gravityValue.GetHighMin(), -_gravityValue.GetHighMax() );
        _gravityValue.SetLow( -_gravityValue.GetLowMin(), -_gravityValue.GetLowMax() );

        _windValue.SetHigh( -_windValue.GetHighMin(), -_windValue.GetHighMax() );
        _windValue.SetLow( -_windValue.GetLowMin(), -_windValue.GetLowMax() );

        _rotationValue.SetHigh( -_rotationValue.GetHighMin(), -_rotationValue.GetHighMax() );
        _rotationValue.SetLow( -_rotationValue.GetLowMin(), -_rotationValue.GetLowMax() );

        _yOffsetValue.SetLow( -_yOffsetValue.GetLowMin(), -_yOffsetValue.GetLowMax() );
    }

    /** Returns the bounding box for all active particles. z axis will always be zero. */
    public BoundingBox GetBoundingBox()
    {
        if ( bounds == null )
        {
            bounds = new BoundingBox();
        }

        Particle[]  particles = this._particles;
        var         active    = this.isActive;
        BoundingBox bounds    = this._bounds;

        bounds.inf();

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            if ( active[ i ] )
            {
                Rectangle r = particles[ i ].getBoundingRectangle();
                bounds.ext( r.x, r.y, 0 );
                bounds.ext( r.x + r.width, r.y + r.height, 0 );
            }
        }

        return bounds;
    }

    protected RangedNumericValue[] GetXSizeValues()
    {
        if ( _xSizeValues == null )
        {
            _xSizeValues      = new RangedNumericValue[ 3 ];
            _xSizeValues[ 0 ] = _xScaleValue;
            _xSizeValues[ 1 ] = _spawnWidthValue;
            _xSizeValues[ 2 ] = _xOffsetValue;
        }

        return _xSizeValues;
    }

    protected RangedNumericValue[] GetYSizeValues()
    {
        if ( _ySizeValues == null )
        {
            _ySizeValues      = new RangedNumericValue[ 3 ];
            _ySizeValues[ 0 ] = _yScaleValue;
            _ySizeValues[ 1 ] = _spawnHeightValue;
            _ySizeValues[ 2 ] = _yOffsetValue;
        }

        return _ySizeValues;
    }

    protected RangedNumericValue[] GetMotionValues()
    {
        if ( _motionValues == null )
        {
            _motionValues      = new RangedNumericValue[ 3 ];
            _motionValues[ 0 ] = _velocityValue;
            _motionValues[ 1 ] = _windValue;
            _motionValues[ 2 ] = _gravityValue;
        }

        return _motionValues;
    }

    /** Permanently scales the size of the emitter by scaling all its ranged values related to size. */
    public void ScaleSize( float scale )
    {
        if ( scale == 1f )
        {
            return;
        }

        ScaleSize( scale, scale );
    }

    /** Permanently scales the size of the emitter by scaling all its ranged values related to size. */
    public void ScaleSize( float scaleX, float scaleY )
    {
        if ( ( scaleX == 1f ) && ( scaleY == 1f ) )
        {
            return;
        }

        for ( RangedNumericValue value :
        GetXSizeValues())
        value.scale( scaleX );
        for ( RangedNumericValue value :
        GetYSizeValues())
        value.scale( scaleY );
    }

    /** Permanently scales the speed of the emitter by scaling all its ranged values related to motion. */
    public void ScaleMotion( float scale )
    {
        if ( scale == 1f )
        {
            return;
        }

        for ( RangedNumericValue value :
        GetMotionValues())
        value.scale( scale );
    }

    /** Sets all size-related ranged values to match those of the template emitter. */
    public void MatchSize( ParticleEmitter template )
    {
        MatchXSize( template );
        MatchYSize( template );
    }

    /** Sets all horizontal size-related ranged values to match those of the template emitter. */
    public void MatchXSize( ParticleEmitter template )
    {
        RangedNumericValue[] values         = GetXSizeValues();
        RangedNumericValue[] templateValues = template.GetXSizeValues();

        for ( var i = 0; i < values.Length; i++ )
        {
            values[ i ].Set( templateValues[ i ] );
        }
    }

    /** Sets all vertical size-related ranged values to match those of the template emitter. */
    public void MatchYSize( ParticleEmitter template )
    {
        RangedNumericValue[] values         = GetYSizeValues();
        RangedNumericValue[] templateValues = template.GetYSizeValues();

        for ( var i = 0; i < values.Length; i++ )
        {
            values[ i ].Set( templateValues[ i ] );
        }
    }

    /** Sets all motion-related ranged values to match those of the template emitter. */
    public void MatchMotion( ParticleEmitter template )
    {
        RangedNumericValue[] values         = GetMotionValues();
        RangedNumericValue[] templateValues = template.GetMotionValues();

        for ( var i = 0; i < values.Length; i++ )
        {
            values[ i ].Set( templateValues[ i ] );
        }
    }

    public void Save( Writer output )
    {
        output.write( _name + "\n" );
        output.write( "- Delay -\n" );
        _delayValue.Save( output );
        output.write( "- Duration - \n" );
        _durationValue.Save( output );
        output.write( "- Count - \n" );
        output.write( "min: " + _minParticleCount + "\n" );
        output.write( "max: " + _maxParticleCount + "\n" );
        output.write( "- Emission - \n" );
        _emissionValue.Save( output );
        output.write( "- Life - \n" );
        _lifeValue.Save( output );
        output.write( "- Life Offset - \n" );
        _lifeOffsetValue.Save( output );
        output.write( "- X Offset - \n" );
        _xOffsetValue.Save( output );
        output.write( "- Y Offset - \n" );
        _yOffsetValue.Save( output );
        output.write( "- Spawn Shape - \n" );
        _spawnShapeValue.Save( output );
        output.write( "- Spawn Width - \n" );
        _spawnWidthValue.Save( output );
        output.write( "- Spawn Height - \n" );
        _spawnHeightValue.Save( output );
        output.write( "- X Scale - \n" );
        _xScaleValue.Save( output );
        output.write( "- Y Scale - \n" );
        _yScaleValue.Save( output );
        output.write( "- Velocity - \n" );
        _velocityValue.Save( output );
        output.write( "- Angle - \n" );
        _angleValue.Save( output );
        output.write( "- Rotation - \n" );
        _rotationValue.Save( output );
        output.write( "- Wind - \n" );
        _windValue.Save( output );
        output.write( "- Gravity - \n" );
        _gravityValue.Save( output );
        output.write( "- Tint - \n" );
        _tintValue.Save( output );
        output.write( "- Transparency - \n" );
        _transparencyValue.Save( output );
        output.write( "- Options - \n" );
        output.write( "attached: " + _attached + "\n" );
        output.write( "continuous: " + _continuous + "\n" );
        output.write( "aligned: " + _aligned + "\n" );
        output.write( "additive: " + _additive + "\n" );
        output.write( "behind: " + Behind + "\n" );
        output.write( "premultipliedAlpha: " + _premultipliedAlpha + "\n" );
        output.write( "spriteMode: " + _spriteMode.toString() + "\n" );
        output.write( "- Image Paths -\n" );
        for ( string imagePath :
        _imagePaths) {
            output.write( imagePath + "\n" );
        }

        output.write( "\n" );
    }

    public void Load( BufferedReader reader )
    {
        try
        {
            _name = ReadString( reader, "name" );
            reader.readLine();
            _delayValue.Load( reader );
            reader.readLine();
            _durationValue.Load( reader );
            reader.readLine();
            SetMinParticleCount( ReadInt( reader, "minParticleCount" ) );
            SetMaxParticleCount( ReadInt( reader, "maxParticleCount" ) );
            reader.readLine();
            _emissionValue.Load( reader );
            reader.readLine();
            _lifeValue.Load( reader );
            reader.readLine();
            _lifeOffsetValue.Load( reader );
            reader.readLine();
            _xOffsetValue.Load( reader );
            reader.readLine();
            _yOffsetValue.Load( reader );
            reader.readLine();
            _spawnShapeValue.Load( reader );
            reader.readLine();
            _spawnWidthValue.Load( reader );
            reader.readLine();
            _spawnHeightValue.Load( reader );
            string line = reader.readLine();

            if ( line.trim().equals( "- Scale -" ) )
            {
                _xScaleValue.Load( reader );
                _yScaleValue.SetActive( false );
            }
            else
            {
                _xScaleValue.Load( reader );
                reader.readLine();
                _yScaleValue.Load( reader );
            }

            reader.readLine();
            _velocityValue.Load( reader );
            reader.readLine();
            _angleValue.Load( reader );
            reader.readLine();
            _rotationValue.Load( reader );
            reader.readLine();
            _windValue.Load( reader );
            reader.readLine();
            _gravityValue.Load( reader );
            reader.readLine();
            _tintValue.Load( reader );
            reader.readLine();
            _transparencyValue.Load( reader );
            reader.readLine();
            _attached   = ReadBoolean( reader, "attached" );
            _continuous = ReadBoolean( reader, "continuous" );
            _aligned    = ReadBoolean( reader, "aligned" );
            _additive   = ReadBoolean( reader, "additive" );
            Behind      = ReadBoolean( reader, "behind" );

            // Backwards compatibility
            line = reader.readLine();

            if ( line.startsWith( "premultipliedAlpha" ) )
            {
                _premultipliedAlpha = ReadBoolean( line );
                line                = reader.readLine();
            }

            if ( line.startsWith( "spriteMode" ) )
            {
                _spriteMode = SpriteMode.valueOf( ReadString( line ) );
                line        = reader.readLine();
            }

            List< string > imagePaths = new();

            while ( ( ( line = reader.readLine() ) != null ) && !line.isEmpty() )
            {
                imagePaths.add( line );
            }

            SetImagePaths( imagePaths );
        }
        catch ( RuntimeException ex )
        {
            if ( _name == null )
            {
                throw ex;
            }

            throw new RuntimeException( "Error parsing emitter: " + _name, ex );
        }
    }

    static string ReadString( string line )
    {
        return line.substring( line.indexOf( ":" ) + 1 ).trim();
    }

    static string ReadString( BufferedReader reader, string name )
    {
        string line = reader.readLine();

        if ( line == null )
        {
            throw new IOException( "Missing value: " + name );
        }

        return ReadString( line );
    }

    static bool ReadBoolean( string line )
    {
        return Boolean.parseBoolean( ReadString( line ) );
    }

    static bool ReadBoolean( BufferedReader reader, string name )
    {
        return Boolean.parseBoolean( ReadString( reader, name ) );
    }

    static int ReadInt( BufferedReader reader, string name )
    {
        return Integer.parseInt( ReadString( reader, name ) );
    }

    static float ReadFloat( BufferedReader reader, string name )
    {
        return float.Parse( ReadString( reader, name ) );
    }

    public class Particle : Sprite
    {
        protected int     life,         currentLife;
        protected float   xScale,       xScaleDiff;
        protected float   yScale,       yScaleDiff;
        protected float   rotation,     rotationDiff;
        protected float   velocity,     velocityDiff;
        protected float   angle,        angleDiff;
        protected float   angleCos,     angleSin;
        protected float   transparency, transparencyDiff;
        protected float   wind,         windDiff;
        protected float   gravity,      gravityDiff;
        protected float[] tint;
        protected int     frame;

        public Particle( Sprite sprite )
            : base( sprite )
        {
        }
    }

    public class ParticleValue
    {
        bool                   _active;
        protected private bool _alwaysActive;

        public void SetAlwaysActive( bool alwaysActive )
        {
            this._alwaysActive = alwaysActive;
        }

        public bool ISAlwaysActive()
        {
            return _alwaysActive;
        }

        public bool ISActive()
        {
            return _alwaysActive || _active;
        }

        public void SetActive( bool active )
        {
            this._active = active;
        }

        public void Save( Writer output )
        {
            if ( !_alwaysActive )
            {
                output.write( "active: " + _active + "\n" );
            }
            else
            {
                _active = true;
            }
        }

        public void Load( BufferedReader reader )
        {
            if ( !_alwaysActive )
            {
                _active = ReadBoolean( reader, "active" );
            }
            else
            {
                _active = true;
            }
        }

        public void Load( ParticleValue value )
        {
            _active       = value._active;
            _alwaysActive = value._alwaysActive;
        }

    }

    public class NumericValue : ParticleValue
    {
        private float _value;

        public float GetValue()
        {
            return _value;
        }

        public void SetValue( float value )
        {
            this._value = value;
        }

        public void Save( Writer output )
        {
            base.Save( output );

            if ( !isActive )
            {
                return;
            }

            output.write( "value: " + _value + "\n" );
        }

        public void Load( BufferedReader reader )
        {
            base.Load( reader );

            if ( !isActive )
            {
                return;
            }

            _value = ReadFloat( reader, "value" );
        }

        public void Load( NumericValue value )
        {
            base.Load( value );
            this._value = value._value;
        }
    }

    public class RangedNumericValue : ParticleValue
    {
        private float _lowMin, _lowMax;

        public float NewLowValue()
        {
            return _lowMin + ( ( _lowMax - _lowMin ) * MathUtils.Random() );
        }

        public void SetLow( float value )
        {
            _lowMin = value;
            _lowMax = value;
        }

        public void SetLow( float min, float max )
        {
            _lowMin = min;
            _lowMax = max;
        }

        public float GetLowMin()
        {
            return _lowMin;
        }

        public void SetLowMin( float lowMin )
        {
            this._lowMin = lowMin;
        }

        public float GetLowMax()
        {
            return _lowMax;
        }

        public void SetLowMax( float lowMax )
        {
            this._lowMax = lowMax;
        }

        /** permanently scales the range by a scalar. */
        protected virtual void Scale( float scale )
        {
            _lowMin *= scale;
            _lowMax *= scale;
        }

        public void Set( RangedNumericValue value )
        {
            this._lowMin = value._lowMin;
            this._lowMax = value._lowMax;
        }

        public void Save( Writer output )
        {
            base.Save( output );

            if ( !isActive )
            {
                return;
            }

            output.write( "lowMin: " + _lowMin + "\n" );
            output.write( "lowMax: " + _lowMax + "\n" );
        }

        public void Load( BufferedReader reader )
        {
            base.Load( reader );

            if ( !isActive )
            {
                return;
            }

            _lowMin = ReadFloat( reader, "lowMin" );
            _lowMax = ReadFloat( reader, "lowMax" );
        }

        public void Load( RangedNumericValue value )
        {
            base.Load( value );
            _lowMax = value._lowMax;
            _lowMin = value._lowMin;
        }
    }

    public class ScaledNumericValue : RangedNumericValue
    {
        private float[] _scaling  = { 1 };
        private float[] _timeline = { 0 };
        private float   _highMin;
        private float   _highMax;
        private bool    _relative;

        public float NewHighValue()
        {
            return _highMin + ( ( _highMax - _highMin ) * MathUtils.Random() );
        }

        public void SetHigh( float value )
        {
            _highMin = value;
            _highMax = value;
        }

        public void SetHigh( float min, float max )
        {
            _highMin = min;
            _highMax = max;
        }

        public float GetHighMin()
        {
            return _highMin;
        }

        public void SetHighMin( float highMin )
        {
            this._highMin = highMin;
        }

        public float GetHighMax()
        {
            return _highMax;
        }

        public void SetHighMax( float highMax )
        {
            this._highMax = highMax;
        }

        public override void Scale( float scale )
        {
            base.Scale( scale );
            _highMin *= scale;
            _highMax *= scale;
        }

        public void Set( RangedNumericValue value )
        {
            if ( value is ScaledNumericValue )
            {
                Set( ( ScaledNumericValue )value );
            }
            else
            {
                base.Set( value );
            }
        }

        public void Set( ScaledNumericValue value )
        {
            base.Set( value );
            this._highMin = value._highMin;
            this._highMax = value._highMax;

            if ( _scaling.Length != value._scaling.Length )
            {
                _scaling = Arrays.copyOf( value._scaling, value._scaling.Length );
            }
            else
            {
                System.arraycopy( value._scaling, 0, _scaling, 0, _scaling.Length );
            }

            if ( _timeline.Length != value._timeline.Length )
            {
                _timeline = Arrays.copyOf( value._timeline, value._timeline.Length );
            }
            else
            {
                System.arraycopy( value._timeline, 0, _timeline, 0, _timeline.Length );
            }

            this._relative = value._relative;
        }

        public float[] GetScaling()
        {
            return _scaling;
        }

        public void SetScaling( float[] values )
        {
            this._scaling = values;
        }

        public float[] GetTimeline()
        {
            return _timeline;
        }

        public void SetTimeline( float[] timeline )
        {
            this._timeline = timeline;
        }

        public bool ISRelative()
        {
            return _relative;
        }

        public void SetRelative( bool relative )
        {
            this._relative = relative;
        }

        public float GetScale( float percent )
        {
            var endIndex = -1;
            var timeline = this._timeline;
            var n        = timeline.Length;

            for ( var i = 1; i < n; i++ )
            {
                var t = timeline[ i ];

                if ( t > percent )
                {
                    endIndex = i;

                    break;
                }
            }

            if ( endIndex == -1 )
            {
                return scaling[ n - 1 ];
            }

            var scaling    = this._scaling;
            var startIndex = endIndex - 1;
            var startValue = scaling[ startIndex ];
            var startTime  = timeline[ startIndex ];

            return startValue + ( ( scaling[ endIndex ] - startValue ) * ( ( percent - startTime ) / ( timeline[ endIndex ] - startTime ) ) );
        }

        public void Save( Writer output )
        {
            base.Save( output );

            if ( !isActive )
            {
                return;
            }

            output.write( "highMin: " + _highMin + "\n" );
            output.write( "highMax: " + _highMax + "\n" );
            output.write( "relative: " + _relative + "\n" );
            output.write( "scalingCount: " + _scaling.Length + "\n" );

            for ( var i = 0;
                  i < _scaling.Length;
                  i++ )
            {
                output.write( "scaling" + i + ": " + _scaling[ i ] + "\n" );
            }

            output.write( "timelineCount: " + _timeline.Length + "\n" );

            for ( var i = 0;
                  i < _timeline.Length;
                  i++ )
            {
                output.write( "timeline" + i + ": " + _timeline[ i ] + "\n" );
            }
        }

        public void Load( BufferedReader reader )
        {
            base.Load( reader );

            if ( !isActive )
            {
                return;
            }

            _highMin  = ReadFloat( reader, "highMin" );
            _highMax  = ReadFloat( reader, "highMax" );
            _relative = ReadBoolean( reader, "relative" );
            _scaling  = new float[ ReadInt( reader, "scalingCount" ) ];

            for ( var i = 0; i < _scaling.Length; i++ )
            {
                _scaling[ i ] = ReadFloat( reader, "scaling" + i );
            }

            _timeline = new float[ ReadInt( reader, "timelineCount" ) ];

            for ( var i = 0; i < _timeline.Length; i++ )
            {
                _timeline[ i ] = ReadFloat( reader, "timeline" + i );
            }
        }

        public void Load( ScaledNumericValue value )
        {
            base.Load( value );
            _highMax = value._highMax;
            _highMin = value._highMin;
            _scaling = new float[ value._scaling.Length ];
            System.arraycopy( value._scaling, 0, _scaling, 0, _scaling.Length );
            _timeline = new float[ value._timeline.Length ];
            System.arraycopy( value._timeline, 0, _timeline, 0, _timeline.Length );
            _relative = value._relative;
        }
    }

    public class IndependentScaledNumericValue : ScaledNumericValue
    {
        public bool Independent { get; set; }

        public void Set( RangedNumericValue value )
        {
            if ( value is IndependentScaledNumericValue )
            {
                Set( ( IndependentScaledNumericValue )value );
            }
            else
            {
                base.Set( value );
            }
        }

        public void Set( ScaledNumericValue value )
        {
            if ( value is IndependentScaledNumericValue )
            {
                Set( ( IndependentScaledNumericValue )value );
            }
            else
            {
                base.Set( value );
            }
        }

        public void Set( IndependentScaledNumericValue value )
        {
            base.Set( value );
            Independent = value.Independent;
        }

        public new void Save( Writer output )
        {
            base.Save( output );
            output.write( "independent: " + Independent + "\n" );
        }

        public new void Load( BufferedReader reader )
        {
            base.Load( reader );

            // For backwards compatibility, independent property may not be defined
            if ( reader.markSupported() )
            {
                reader.mark( 100 );
            }

            string line = reader.readLine();

            if ( line == null )
            {
                throw new IOException( "Missing value: independent" );
            }

            if ( line.Contains( "independent" ) )
            {
                Independent = bool.Parse( ReadString( line ) );
            }
            else if ( reader.markSupported() )
            {
                reader.reset();
            }
            else
            {
                // @see java.io.BufferedReader#markSupported may return false in some platforms (such as GWT),
                // in that case backwards commpatibility is not possible
                var errorMessage = "The loaded particle effect descriptor file uses an old invalid format. "
                                 + "Please download the latest version of the Particle Editor tool and recreate the file by"
                                 + " loading and saving it again.";

                Gdx.App.Error( "ParticleEmitter", errorMessage );

                throw new IOException( errorMessage );
            }
        }

        public void Load( IndependentScaledNumericValue value )
        {
            base.Load( value );
            Independent = value.Independent;
        }
    }

    public class GradientColorValue : ParticleValue
    {
        private readonly float[] _temp = new float[ 4 ];

        public float[] Timeline { get; set; } = { 0 };
        public float[] Colors   { get; set; } = { 1, 1, 1 };

        public GradientColorValue()
        {
            _alwaysActive = true;
        }

        public float[] GetColor( float percent )
        {
            int startIndex = 0, endIndex = -1;
            var timeline   = this.Timeline;
            var n          = timeline.Length;

            for ( var i = 1; i < n; i++ )
            {
                var t = timeline[ i ];

                if ( t > percent )
                {
                    endIndex = i;

                    break;
                }

                startIndex = i;
            }

            var startTime = timeline[ startIndex ];

            startIndex *= 3;

            var r1 = Colors[ startIndex ];
            var g1 = Colors[ startIndex + 1 ];
            var b1 = Colors[ startIndex + 2 ];

            if ( endIndex == -1 )
            {
                _temp[ 0 ] = r1;
                _temp[ 1 ] = g1;
                _temp[ 2 ] = b1;

                return _temp;
            }

            var factor = ( percent - startTime ) / ( timeline[ endIndex ] - startTime );

            endIndex *= 3;

            _temp[ 0 ] = r1 + ( ( Colors[ endIndex ] - r1 ) * factor );
            _temp[ 1 ] = g1 + ( ( Colors[ endIndex + 1 ] - g1 ) * factor );
            _temp[ 2 ] = b1 + ( ( Colors[ endIndex + 2 ] - b1 ) * factor );

            return _temp;
        }

        public new void Save( Writer output )
        {
            base.Save( output );

            if ( !isActive )
            {
                return;
            }

            output.write( "colorsCount: " + Colors.Length + "\n" );

            for ( var i = 0; i < Colors.Length; i++ )
            {
                output.write( "colors" + i + ": " + Colors[ i ] + "\n" );
            }

            output.write( "timelineCount: " + Timeline.Length + "\n" );

            for ( var i = 0; i < Timeline.Length; i++ )
            {
                output.write( "timeline" + i + ": " + Timeline[ i ] + "\n" );
            }
        }

        public new void Load( BufferedReader reader )
        {
            base.Load( reader );

            if ( !isActive )
            {
                return;
            }

            Colors = new float[ ReadInt( reader, "colorsCount" ) ];

            for ( var i = 0; i < Colors.Length; i++ )
            {
                Colors[ i ] = ReadFloat( reader, "colors" + i );
            }

            Timeline = new float[ ReadInt( reader, "timelineCount" ) ];

            for ( var i = 0; i < Timeline.Length; i++ )
            {
                Timeline[ i ] = ReadFloat( reader, "timeline" + i );
            }
        }

        public void Load( GradientColorValue value )
        {
            base.Load( value );

            Colors = new float[ value.Colors.Length ];

            Array.Copy( value.Colors, 0, Colors, 0, Colors.Length );

            Timeline = new float[ value.Timeline.Length ];

            Array.Copy( value.Timeline, 0, Timeline, 0, Timeline.Length );
        }
    }

    public class SpawnShapeValue : ParticleValue
    {
        public bool             Edges { get; set; }
        public SpawnShape       Shape { get; set; } = SpawnShape.Point;
        public SpawnEllipseSide Side  { get; set; } = SpawnEllipseSide.Both;

        public void Save( Writer output )
        {
            base.Save( output );

            if ( !isActive )
            {
                return;
            }

            output.write( "shape: " + Shape + "\n" );

            if ( Shape == SpawnShape.Ellipse )
            {
                output.write( "edges: " + Edges + "\n" );
                output.write( "side: " + Side + "\n" );
            }
        }

        public new void Load( BufferedReader reader )
        {
            base.Load( reader );

            if ( !isActive )
            {
                return;
            }

            Shape = Enum.Parse< SpawnShape >( "shape" );

            if ( Shape == SpawnShape.Ellipse )
            {
                Edges = ReadBoolean( reader, "edges" );
                Side  = Enum.Parse< SpawnEllipseSide >( ReadString( reader, "side" ) );
            }
        }

        public void Load( SpawnShapeValue value )
        {
            base.Load( value );
            Shape = value.Shape;
            Edges = value.Edges;
            Side  = value.Side;
        }
    }

    public enum SpawnShape
    {
        Point,
        Line,
        Square,
        Ellipse
    }

    public enum SpawnEllipseSide
    {
        Both,
        Top,
        Bottom
    }

    public enum SpriteMode
    {
        Single,
        Random,
        Animated
    }
}
