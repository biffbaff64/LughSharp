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

using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Maths.Collision;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;
using Exception = System.Exception;

namespace Corelib.LibCore.Graphics.G2D;

[PublicAPI]
public class ParticleEmitter
{
    // ------------------------------------------------------------------------

    public enum SpawnEllipseSide
    {
        Both,
        Top,
        Bottom,
    }

    // ------------------------------------------------------------------------

    public enum SpawnShape
    {
        Point,
        Line,
        Square,
        Ellipse,
    }

    // ------------------------------------------------------------------------

    public enum SpriteModes
    {
        Single,
        Random,
        Animated,
    }

    private const int DEFAULT_MAX_PARTICLE_COUNT = 4;

    private static readonly int _updateScale    = 1 << 0;
    private static readonly int _updateAngle    = 1 << 1;
    private static readonly int _updateRotation = 1 << 2;
    private static readonly int _updateVelocity = 1 << 3;
    private static readonly int _updateWind     = 1 << 4;
    private static readonly int _updateGravity  = 1 << 5;
    private static readonly int _updateTint     = 1 << 6;
    private static readonly int _updateSprite   = 1 << 7;

    private float _accumulator;
    private bool  _allowCompletion;

    private BoundingBox?          _bounds;
    private float                 _delay;
    private float                 _delayTimer;
    private float                 _emission;
    private float                 _emissionDelta;
    private float                 _emissionDiff;
    private bool                  _firstUpdate;
    private bool                  _flipX;
    private bool                  _flipY;
    private int                   _life;
    private int                   _lifeDiff;
    private int                   _lifeOffset;
    private int                   _lifeOffsetDiff;
    private RangedNumericValue[]? _motionValues;
    private Particle?[]           _particles = new Particle[ 4 ];
    private float                 _spawnHeight;
    private float                 _spawnHeightDiff;
    private float                 _spawnWidth;
    private float                 _spawnWidthDiff;
    private int                   _updateFlags;
    private RangedNumericValue[]? _xSizeValues;
    private RangedNumericValue[]? _ySizeValues;

    // ------------------------------------------------------------------------

    public ParticleEmitter( StreamReader? reader = null )
    {
        Initialise();

        if ( reader != null )
        {
            Load( reader );
        }
    }

    /// <summary>
    /// Create a new Particvle Emitter which is a copy of the
    /// supplied emitter.
    /// </summary>
    /// <param name="emitter"></param>
    public ParticleEmitter( ParticleEmitter emitter )
    {
        Sprites    = [ ..emitter.Sprites ];
        Name       = emitter.Name;
        ImagePaths = [ ..emitter.ImagePaths ];

        SetMaxParticleCount( emitter.MaxParticleCount );

        MinParticleCount = emitter.MinParticleCount;
        DelayValue.Load( emitter.DelayValue );
        DurationValue.Load( emitter.DurationValue );
        EmissionValue.Load( emitter.EmissionValue );
        LifeValue.Load( emitter.LifeValue );
        LifeOffsetValue.Load( emitter.LifeOffsetValue );
        XScaleValue.Load( emitter.XScaleValue );
        YScaleValue.Load( emitter.YScaleValue );
        RotationValue.Load( emitter.RotationValue );
        VelocityValue.Load( emitter.VelocityValue );
        AngleValue.Load( emitter.AngleValue );
        WindValue.Load( emitter.WindValue );
        GravityValue.Load( emitter.GravityValue );
        TransparencyValue.Load( emitter.TransparencyValue );
        TintValue.Load( emitter.TintValue );
        XOffsetValue.Load( emitter.XOffsetValue );
        YOffsetValue.Load( emitter.YOffsetValue );
        SpawnWidthValue.Load( emitter.SpawnWidthValue );
        SpawnHeightValue.Load( emitter.SpawnHeightValue );
        SpawnShapeValue.Load( emitter.SpawnShapeValue );

        Attached              = emitter.Attached;
        Continuous            = emitter.Continuous;
        Aligned               = emitter.Aligned;
        Behind                = emitter.Behind;
        Additive              = emitter.Additive;
        PremultipliedAlpha    = emitter.PremultipliedAlpha;
        CleansUpBlendFunction = emitter.CleansUpBlendFunction;
        SpriteMode            = emitter.SpriteMode;

        SetPosition( emitter.X, emitter.Y );
    }

    /// <summary>
    /// Common initialisation method for all constructors
    /// </summary>
    private void Initialise()
    {
        DurationValue.AlwaysActive     = true;
        EmissionValue.AlwaysActive     = true;
        LifeValue.AlwaysActive         = true;
        XScaleValue.AlwaysActive       = true;
        TransparencyValue.AlwaysActive = true;
        SpawnShapeValue.AlwaysActive   = true;
        SpawnWidthValue.AlwaysActive   = true;
        SpawnHeightValue.AlwaysActive  = true;
    }

    public void SetMaxParticleCount( int maxParticleCount )
    {
        MaxParticleCount = maxParticleCount;
        IsActive         = new bool[ maxParticleCount ];
        ActiveCount      = 0;
        _particles       = new Particle[ maxParticleCount ];
    }

    public void AddParticle()
    {
        var activeCount = ActiveCount;

        if ( activeCount == MaxParticleCount )
        {
            return;
        }

        var active = IsActive;

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            if ( !active[ i ] )
            {
                ActivateParticle( i );
                active[ i ] = true;
                ActiveCount = activeCount + 1;

                break;
            }
        }
    }

    public void AddParticles( int count )
    {
        count = Math.Min( count, MaxParticleCount - ActiveCount );

        if ( count == 0 )
        {
            return;
        }

        var active = IsActive;
        int index  = 0, n = active.Length;

//    outer:

        for ( var i = 0; i < count; )
        {
            for ( ; index < n; index++ )
            {
                if ( !active[ index ] )
                {
                    ActivateParticle( index );
                    active[ index++ ] = true;

//                    goto outer;
                }
            }

            break;
        }

        ActiveCount += count;
    }

    public void Update( float delta )
    {
        _accumulator += delta * 1000;

        if ( _accumulator < 1 )
        {
            return;
        }

        var deltaMillis = ( int ) _accumulator;
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

            if ( DurationTimer < Duration )
            {
                DurationTimer += deltaMillis;
            }
            else
            {
                if ( !Continuous || _allowCompletion )
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

                var emissionTime = _emission + ( _emissionDiff * EmissionValue.GetScale( DurationTimer / Duration ) );

                if ( emissionTime > 0 )
                {
                    emissionTime = 1000 / emissionTime;

                    if ( _emissionDelta >= emissionTime )
                    {
                        var emitCount = _emissionDelta / emissionTime;
                        emitCount = Math.Min( emitCount, MaxParticleCount - ActiveCount );

                        _emissionDelta -= emitCount * emissionTime;
                        _emissionDelta %= emissionTime;

                        AddParticles( ( int ) emitCount );
                    }
                }

                if ( ActiveCount < MinParticleCount )
                {
                    AddParticles( MinParticleCount - ActiveCount );
                }
            }
        }

        for ( int i = 0, n = IsActive.Length; i < n; i++ )
        {
            if ( _particles[ i ] != null )
            {
                if ( IsActive[ i ] && !UpdateParticle( _particles[ i ]!, delta, deltaMillis ) )
                {
                    IsActive[ i ] = false;
                    ActiveCount--;
                }
            }
        }
    }

    public void Draw( IBatch batch )
    {
        if ( PremultipliedAlpha )
        {
            batch.SetBlendFunction( IGL.GL_ONE, IGL.GL_ONE_MINUS_SRC_ALPHA );
        }
        else if ( Additive )
        {
            batch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE );
        }
        else
        {
            batch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
        }

        for ( int i = 0, n = IsActive.Length; i < n; i++ )
        {
            if ( IsActive[ i ] )
            {
                _particles?[ i ]?.Draw( batch );
            }
        }

        if ( CleansUpBlendFunction && ( Additive || PremultipliedAlpha ) )
        {
            batch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
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

        var deltaMillis = ( int ) _accumulator;
        _accumulator -= deltaMillis;

        if ( PremultipliedAlpha )
        {
            batch.SetBlendFunction( IGL.GL_ONE, IGL.GL_ONE_MINUS_SRC_ALPHA );
        }
        else if ( Additive )
        {
            batch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE );
        }
        else
        {
            batch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
        }

        var active      = IsActive;
        var activeCount = ActiveCount;

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            if ( active[ i ] )
            {
                if ( UpdateParticle( _particles[ i ]!, delta, deltaMillis ) )
                {
                    _particles[ i ]?.Draw( batch );
                }
                else
                {
                    active[ i ] = false;
                    activeCount--;
                }
            }
        }

        ActiveCount = activeCount;

        if ( CleansUpBlendFunction && ( Additive || PremultipliedAlpha ) )
        {
            batch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
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

        if ( DurationTimer < Duration )
        {
            DurationTimer += deltaMillis;
        }
        else
        {
            if ( !Continuous || _allowCompletion )
            {
                return;
            }

            Restart();
        }

        _emissionDelta += deltaMillis;
        var emissionTime = _emission + ( _emissionDiff * EmissionValue.GetScale( DurationTimer / Duration ) );

        if ( emissionTime > 0 )
        {
            emissionTime = 1000 / emissionTime;

            if ( _emissionDelta >= emissionTime )
            {
                var emitCount = ( int ) ( _emissionDelta / emissionTime );
                emitCount = Math.Min( emitCount, MaxParticleCount - activeCount );

                _emissionDelta -= ( int ) ( emitCount * emissionTime );
                _emissionDelta %= ( int ) emissionTime;

                AddParticles( emitCount );
            }
        }

        if ( activeCount < MinParticleCount )
        {
            AddParticles( MinParticleCount - activeCount );
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
        DurationTimer  = Duration;
        var active = IsActive;

        for ( int i = 0, n = active.Length; i < n; i++ )
        {
            active[ i ] = false;
        }

        ActiveCount = 0;
        Start();
    }

    private void Restart()
    {
        _delay      = DelayValue.Active ? DelayValue.NewLowValue() : 0;
        _delayTimer = 0;

        DurationTimer -= Duration;
        Duration      =  DurationValue.NewLowValue();

        _emission     = ( int ) EmissionValue.NewLowValue();
        _emissionDiff = ( int ) EmissionValue.NewHighValue();

        if ( !EmissionValue.IsRelative )
        {
            _emissionDiff -= _emission;
        }

        if ( !LifeValue.Independent )
        {
            GenerateLifeValues();
        }

        if ( !LifeOffsetValue.Independent )
        {
            GenerateLifeOffsetValues();
        }

        _spawnWidth     = SpawnWidthValue.NewLowValue();
        _spawnWidthDiff = SpawnWidthValue.NewHighValue();

        if ( !SpawnWidthValue.IsRelative )
        {
            _spawnWidthDiff -= _spawnWidth;
        }

        _spawnHeight     = SpawnHeightValue.NewLowValue();
        _spawnHeightDiff = SpawnHeightValue.NewHighValue();

        if ( !SpawnHeightValue.IsRelative )
        {
            _spawnHeightDiff -= _spawnHeight;
        }

        _updateFlags = 0;

        if ( AngleValue is { Active: true, Timeline.Length: > 1 } )
        {
            _updateFlags |= _updateAngle;
        }

        if ( VelocityValue.Active )
        {
            _updateFlags |= _updateVelocity;
        }

        if ( XScaleValue.Timeline.Length > 1 )
        {
            _updateFlags |= _updateScale;
        }

        if ( YScaleValue is { Active: true, Timeline.Length: > 1 } )
        {
            _updateFlags |= _updateScale;
        }

        if ( RotationValue is { Active: true, Timeline.Length: > 1 } )
        {
            _updateFlags |= _updateRotation;
        }

        if ( WindValue.Active )
        {
            _updateFlags |= _updateWind;
        }

        if ( GravityValue.Active )
        {
            _updateFlags |= _updateGravity;
        }

        if ( TintValue.Timeline.Length > 1 )
        {
            _updateFlags |= _updateTint;
        }

        if ( SpriteMode == SpriteModes.Animated )
        {
            _updateFlags |= _updateSprite;
        }
    }

    private void ActivateParticle( int index )
    {
        var sprite = SpriteMode switch
        {
            SpriteModes.Single   => Sprites.First(),
            SpriteModes.Animated => Sprites.First(),
            SpriteModes.Random   => Sprites.Random(),
            var _                => null!,
        };

        if ( sprite == null )
        {
            throw new GdxRuntimeException( "sprite cannot be null!" );
        }

        if ( _particles[ index ] == null )
        {
            _particles[ index ] = new Particle( sprite );
            _particles[ index ]!.Flip( _flipX, _flipY );
        }
        else
        {
            _particles[ index ]?.Set( sprite );
        }

        // Note: At this point _particles[ index ] will definitely not be null,
        // and we already know that _particles is not null. There may be compiler
        // warnings though so we can suppress those...

        var percent     = DurationTimer / Duration;
        var updateFlags = _updateFlags;

        if ( LifeValue.Independent )
        {
            GenerateLifeValues();
        }

        if ( LifeOffsetValue.Independent )
        {
            GenerateLifeOffsetValues();
        }

        _particles[ index ]!.CurrentLife = _life + ( int ) ( _lifeDiff * LifeValue.GetScale( percent ) );

        if ( VelocityValue.Active )
        {
            _particles[ index ]!.Velocity     = VelocityValue.NewLowValue();
            _particles[ index ]!.VelocityDiff = VelocityValue.NewHighValue();

            if ( !VelocityValue.IsRelative )
            {
                _particles[ index ]!.VelocityDiff -= _particles[ index ]!.Velocity;
            }
        }

        _particles[ index ]!.Angle     = AngleValue.NewLowValue();
        _particles[ index ]!.AngleDiff = AngleValue.NewHighValue();

        if ( !AngleValue.IsRelative )
        {
            _particles[ index ]!.AngleDiff -= _particles[ index ]!.Angle;
        }

        float angle = 0;

        if ( ( updateFlags & _updateAngle ) == 0 )
        {
            angle = _particles[ index ]!.Angle + ( _particles[ index ]!.AngleDiff * AngleValue.GetScale( 0 ) );

            _particles[ index ]!.Angle    = angle;
            _particles[ index ]!.AngleCos = MathUtils.CosDeg( angle );
            _particles[ index ]!.AngleSin = MathUtils.SinDeg( angle );
        }

        var spriteWidth  = sprite.Width;
        var spriteHeight = sprite.Height;

        _particles[ index ]!.XScale     = XScaleValue.NewLowValue() / spriteWidth;
        _particles[ index ]!.XScaleDiff = XScaleValue.NewHighValue() / spriteWidth;

        if ( !XScaleValue.IsRelative )
        {
            _particles[ index ]!.XScaleDiff -= _particles[ index ]!.XScale;
        }

        if ( YScaleValue.Active )
        {
            _particles[ index ]!.YScale     = YScaleValue.NewLowValue() / spriteHeight;
            _particles[ index ]!.YScaleDiff = YScaleValue.NewHighValue() / spriteHeight;

            if ( !YScaleValue.IsRelative )
            {
                _particles[ index ]!.YScaleDiff -= _particles[ index ]!.YScale;
            }

            _particles[ index ]!.SetScale(
                                          _particles[ index ]!.XScale + ( _particles[ index ]!.XScaleDiff * XScaleValue.GetScale( 0 ) ),
                                          _particles[ index ]!.YScale + ( _particles[ index ]!.YScaleDiff * YScaleValue.GetScale( 0 ) )
                                         );
        }
        else
        {
            _particles[ index ]!.SetScale( _particles[ index ]!.XScale + ( _particles[ index ]!.XScaleDiff * XScaleValue.GetScale( 0 ) ) );
        }

        if ( RotationValue.Active )
        {
            _particles[ index ]!.Rotation     = RotationValue.NewLowValue();
            _particles[ index ]!.RotationDiff = RotationValue.NewHighValue();

            if ( !RotationValue.IsRelative )
            {
                _particles[ index ]!.RotationDiff -= _particles[ index ]!.Rotation;
            }

            var rotation = _particles[ index ]!.Rotation + ( _particles[ index ]!.RotationDiff * RotationValue.GetScale( 0 ) );

            if ( Aligned )
            {
                rotation += angle;
            }

            _particles[ index ]!.Rotation = rotation;
        }

        if ( WindValue.Active )
        {
            _particles[ index ]!.Wind     = WindValue.NewLowValue();
            _particles[ index ]!.WindDiff = WindValue.NewHighValue();

            if ( !WindValue.IsRelative )
            {
                _particles[ index ]!.WindDiff -= _particles[ index ]!.Wind;
            }
        }

        if ( GravityValue.Active )
        {
            _particles[ index ]!.Gravity     = GravityValue.NewLowValue();
            _particles[ index ]!.GravityDiff = GravityValue.NewHighValue();

            if ( !GravityValue.IsRelative )
            {
                _particles[ index ]!.GravityDiff -= _particles[ index ]!.Gravity;
            }
        }

        var color = _particles[ index ]?.Tint;

        if ( color == null )
        {
            _particles[ index ]!.Tint = color = new float[ 3 ];
        }

        var temp = TintValue.GetColor( 0 );
        color[ 0 ] = temp[ 0 ];
        color[ 1 ] = temp[ 1 ];
        color[ 2 ] = temp[ 2 ];

        _particles[ index ]!.Transparency     = TransparencyValue.NewLowValue();
        _particles[ index ]!.TransparencyDiff = TransparencyValue.NewHighValue() - _particles[ index ]!.Transparency;

        // Spawn.
        var x = X;

        if ( XOffsetValue.Active )
        {
            x += XOffsetValue.NewLowValue();
        }

        var y = Y;

        if ( YOffsetValue.Active )
        {
            y += YOffsetValue.NewLowValue();
        }

        switch ( SpawnShapeValue.Shape )
        {
            case SpawnShape.Square:
            {
                var width  = _spawnWidth + ( _spawnWidthDiff * SpawnWidthValue.GetScale( percent ) );
                var height = _spawnHeight + ( _spawnHeightDiff * SpawnHeightValue.GetScale( percent ) );

                x += MathUtils.Random( width ) - ( width / 2 );
                y += MathUtils.Random( height ) - ( height / 2 );

                break;
            }

            case SpawnShape.Ellipse:
            {
                var width   = _spawnWidth + ( _spawnWidthDiff * SpawnWidthValue.GetScale( percent ) );
                var height  = _spawnHeight + ( _spawnHeightDiff * SpawnHeightValue.GetScale( percent ) );
                var radiusX = width / 2;
                var radiusY = height / 2;

                if ( ( radiusX == 0 ) || ( radiusY == 0 ) )
                {
                    break;
                }

                var scaleY = radiusX / radiusY;

                if ( SpawnShapeValue.Edges )
                {
                    var spawnAngle = SpawnShapeValue.Side switch
                    {
                        // Note the minus sign...
                        SpawnEllipseSide.Top    => -MathUtils.Random( 179f ),
                        SpawnEllipseSide.Bottom => MathUtils.Random( 179f ),
                        var _                   => MathUtils.Random( 360f ),
                    };

                    var cosDeg = MathUtils.CosDeg( spawnAngle );
                    var sinDeg = MathUtils.SinDeg( spawnAngle );

                    x += cosDeg * radiusX;
                    y += ( sinDeg * radiusX ) / scaleY;

                    if ( ( updateFlags & _updateAngle ) == 0 )
                    {
                        _particles[ index ]!.Angle    = spawnAngle;
                        _particles[ index ]!.AngleCos = cosDeg;
                        _particles[ index ]!.AngleSin = sinDeg;
                    }
                }
                else
                {
                    var radius2 = radiusX * radiusX;

                    while ( true )
                    {
                        var px = MathUtils.Random( width ) - radiusX;
                        var py = MathUtils.Random( width ) - radiusX;

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

            case SpawnShape.Line:
            {
                var width  = _spawnWidth + ( _spawnWidthDiff * SpawnWidthValue.GetScale( percent ) );
                var height = _spawnHeight + ( _spawnHeightDiff * SpawnHeightValue.GetScale( percent ) );

                if ( width != 0 )
                {
                    var lineX = width * MathUtils.Random();
                    x += lineX;
                    y += lineX * ( height / width );
                }
                else
                {
                    y += height * MathUtils.Random();
                }

                break;
            }
        }

        _particles[ index ]!.SetBounds( x - ( spriteWidth / 2 ), y - ( spriteHeight / 2 ), spriteWidth, spriteHeight );

        var offsetTime = ( int ) ( _lifeOffset + ( _lifeOffsetDiff * LifeOffsetValue.GetScale( percent ) ) );

        if ( offsetTime > 0 )
        {
            if ( offsetTime >= _particles[ index ]!.CurrentLife )
            {
                offsetTime = _particles[ index ]!.CurrentLife - 1;
            }

            UpdateParticle( _particles[ index ]!, offsetTime / 1000f, offsetTime );
        }
    }

    private bool UpdateParticle( Particle particle, float delta, int deltaMillis )
    {
        ArgumentNullException.ThrowIfNull( particle );

        var life = particle.CurrentLife - deltaMillis;

        if ( life <= 0 )
        {
            return false;
        }

        particle.CurrentLife = life;

        var percent     = 1 - ( particle.CurrentLife / ( float ) particle.Life );
        var updateFlags = _updateFlags;

        if ( ( updateFlags & _updateScale ) != 0 )
        {
            if ( YScaleValue.Active )
            {
                particle.SetScale(
                                  particle.XScale + ( particle.XScaleDiff * XScaleValue.GetScale( percent ) ),
                                  particle.YScale + ( particle.YScaleDiff * YScaleValue.GetScale( percent ) )
                                 );
            }
            else
            {
                particle.SetScale( particle.XScale + ( particle.XScaleDiff * XScaleValue.GetScale( percent ) ) );
            }
        }

        if ( ( updateFlags & _updateVelocity ) != 0 )
        {
            var velocity = ( particle.Velocity + ( particle.VelocityDiff * VelocityValue.GetScale( percent ) ) ) * delta;

            float velocityX, velocityY;

            if ( ( updateFlags & _updateAngle ) != 0 )
            {
                var angle = particle.Angle + ( particle.AngleDiff * AngleValue.GetScale( percent ) );

                velocityX = velocity * MathUtils.CosDeg( angle );
                velocityY = velocity * MathUtils.SinDeg( angle );

                if ( ( updateFlags & _updateRotation ) != 0 )
                {
                    var rotation = particle.Rotation + ( particle.RotationDiff * RotationValue.GetScale( percent ) );

                    if ( Aligned )
                    {
                        rotation += angle;
                    }

                    particle.Rotation = rotation;
                }
            }
            else
            {
                velocityX = velocity * particle.AngleCos;
                velocityY = velocity * particle.AngleSin;

                if ( Aligned || ( ( updateFlags & _updateRotation ) != 0 ) )
                {
                    var rotation = particle.Rotation + ( particle.RotationDiff * RotationValue.GetScale( percent ) );

                    if ( Aligned )
                    {
                        rotation += particle.Angle;
                    }

                    particle.Rotation = rotation;
                }
            }

            if ( ( updateFlags & _updateWind ) != 0 )
            {
                velocityX += ( particle.Wind + ( particle.WindDiff * WindValue.GetScale( percent ) ) ) * delta;
            }

            if ( ( updateFlags & _updateGravity ) != 0 )
            {
                velocityY += ( particle.Gravity + ( particle.GravityDiff * GravityValue.GetScale( percent ) ) ) * delta;
            }

            particle.Translate( velocityX, velocityY );
        }
        else
        {
            if ( ( updateFlags & _updateRotation ) != 0 )
            {
                particle.Rotation = particle.Rotation + ( particle.RotationDiff * RotationValue.GetScale( percent ) );
            }
        }

        var color = ( updateFlags & _updateTint ) != 0
                        ? TintValue.GetColor( percent )
                        : particle.Tint;

        if ( PremultipliedAlpha )
        {
            float alphaMultiplier = Additive ? 0 : 1;
            var   a               = particle.Transparency + ( particle.TransparencyDiff * TransparencyValue.GetScale( percent ) );
            particle.SetColor( color[ 0 ] * a, color[ 1 ] * a, color[ 2 ] * a, a * alphaMultiplier );
        }
        else
        {
            particle.SetColor(
                              color[ 0 ],
                              color[ 1 ],
                              color[ 2 ],
                              particle.Transparency + ( particle.TransparencyDiff * TransparencyValue.GetScale( percent ) )
                             );
        }

        if ( ( updateFlags & _updateSprite ) != 0 )
        {
            var frame = Math.Min( ( int ) ( percent * Sprites.Count ), Sprites.Count - 1 );

            if ( particle.Frame != frame )
            {
                var sprite           = Sprites[ frame ];
                var prevSpriteWidth  = particle.Width;
                var prevSpriteHeight = particle.Height;

                particle.SetRegion( sprite );
                particle.SetSize( sprite.Width, sprite.Height );
                particle.SetOrigin( sprite.OriginX, sprite.OriginY );
                particle.Translate( ( prevSpriteWidth - sprite.Width ) / 2, ( prevSpriteHeight - sprite.Height ) / 2 );
                particle.Frame = frame;
            }
        }

        return true;
    }

    private void GenerateLifeValues()
    {
        _life     = ( int ) LifeValue.NewLowValue();
        _lifeDiff = ( int ) LifeValue.NewHighValue();

        if ( !LifeValue.IsRelative )
        {
            _lifeDiff -= _life;
        }
    }

    private void GenerateLifeOffsetValues()
    {
        _lifeOffset     = LifeOffsetValue.Active ? ( int ) LifeOffsetValue.NewLowValue() : 0;
        _lifeOffsetDiff = ( int ) LifeOffsetValue.NewHighValue();

        if ( !LifeOffsetValue.IsRelative )
        {
            _lifeOffsetDiff -= _lifeOffset;
        }
    }

    public void SetPosition( float x, float y )
    {
        if ( Attached )
        {
            var xAmount = x - X;
            var yAmount = y - Y;
            var active  = IsActive;

            for ( int i = 0, n = active.Length; i < n; i++ )
            {
                if ( active[ i ] )
                {
                    _particles[ i ]?.Translate( xAmount, yAmount );
                }
            }
        }

        X = x;
        Y = y;
    }

    public void SetSprites( List< Sprite > sprites )
    {
        Sprites = sprites;

        if ( sprites.Count == 0 )
        {
            return;
        }

        for ( int i = 0, n = _particles.Length; i < n; i++ )
        {
            if ( _particles[ i ] == null )
            {
                break;
            }

            Sprite? sprite = null;

            switch ( SpriteMode )
            {
                case SpriteModes.Single:
                    sprite = sprites.First();

                    break;

                case SpriteModes.Random:
                    sprite = sprites.Random();

                    break;

                case SpriteModes.Animated:
                    if ( _particles[ i ] != null )
                    {
                        var percent = 1 - ( _particles[ i ]!.CurrentLife / ( float ) _particles[ i ]!.Life );
                        _particles[ i ]!.Frame = Math.Min( ( int ) ( percent * sprites.Count ), sprites.Count - 1 );

                        sprite = sprites[ _particles[ i ]!.Frame ];
                    }

                    break;
            }

            if ( sprite != null )
            {
                _particles[ i ]?.SetRegion( sprite );
                _particles[ i ]?.SetOrigin( sprite.OriginX, sprite.OriginY );
            }
        }
    }

    /**
     * Allocates max particles emitter can hold. Usually called early on to avoid allocation on updates.
     * {@link #setSprites(List)} must have been set before calling this method
     */
    public void PreAllocateParticles()
    {
        if ( Sprites.Count == 0 )
        {
            throw new GdxRuntimeException( "ParticleEmitter.SetSprites() must have been called before PreAllocateParticles()" );
        }

        for ( var index = 0; index < _particles.Length; index++ )
        {
            if ( _particles[ index ] == null )
            {
                _particles[ index ] = new Particle( Sprites.First() );
                _particles[ index ]!.Flip( _flipX, _flipY );
            }
        }
    }

    /// <summary>
    /// Ignores the <see cref="Continuous"/> setting until the
    /// emitter is started again. This allows the emitter to stop smoothly.
    /// </summary>
    public void AllowCompletion()
    {
        _allowCompletion = true;
        DurationTimer    = Duration;
    }

    public bool IsComplete()
    {
        if ( ( Continuous && !_allowCompletion ) || ( _delayTimer < _delay ) )
        {
            return false;
        }

        return ( DurationTimer >= Duration ) && ( ActiveCount == 0 );
    }

    public float GetPercentComplete()
    {
        if ( _delayTimer < _delay )
        {
            return 0;
        }

        return Math.Min( 1, DurationTimer / Duration );
    }

    public void SetFlip( bool flipX, bool flipY )
    {
        _flipX = flipX;
        _flipY = flipY;

        for ( int i = 0, n = _particles.Length; i < n; i++ )
        {
            if ( _particles[ i ] != null )
            {
                _particles[ i ]?.Flip( flipX, flipY );
            }
        }
    }

    public void FlipY()
    {
        AngleValue.SetHigh( -AngleValue.HighMin, -AngleValue.HighMax );
        AngleValue.SetLow( -AngleValue.LowMin, -AngleValue.LowMax );

        GravityValue.SetHigh( -GravityValue.HighMin, -GravityValue.HighMax );
        GravityValue.SetLow( -GravityValue.LowMin, -GravityValue.LowMax );

        WindValue.SetHigh( -WindValue.HighMin, -WindValue.HighMax );
        WindValue.SetLow( -WindValue.LowMin, -WindValue.LowMax );

        RotationValue.SetHigh( -RotationValue.HighMin, -RotationValue.HighMax );
        RotationValue.SetLow( -RotationValue.LowMin, -RotationValue.LowMax );

        YOffsetValue.SetLow( -YOffsetValue.LowMin, -YOffsetValue.LowMax );
    }

    /// <summary>
    /// Returns the bounding box for all active particles.
    /// Z axis will always be zero.
    /// </summary>
    public BoundingBox GetBoundingBox()
    {
        _bounds ??= new BoundingBox();

        var bounds = _bounds;

        bounds.ToInfinity();

        for ( int i = 0, n = IsActive.Length; i < n; i++ )
        {
            if ( IsActive[ i ] )
            {
                var r = _particles[ i ]!.BoundingRectangle;

                bounds.Extend( r.X, r.Y, 0 );
                bounds.Extend( r.X + r.Width, r.Y + r.Height, 0 );
            }
        }

        return bounds;
    }

    protected RangedNumericValue[] GetXSizeValues()
    {
        if ( _xSizeValues == null )
        {
            _xSizeValues      = new RangedNumericValue[ 3 ];
            _xSizeValues[ 0 ] = XScaleValue;
            _xSizeValues[ 1 ] = SpawnWidthValue;
            _xSizeValues[ 2 ] = XOffsetValue;
        }

        return _xSizeValues;
    }

    protected RangedNumericValue[] GetYSizeValues()
    {
        if ( _ySizeValues == null )
        {
            _ySizeValues      = new RangedNumericValue[ 3 ];
            _ySizeValues[ 0 ] = YScaleValue;
            _ySizeValues[ 1 ] = SpawnHeightValue;
            _ySizeValues[ 2 ] = YOffsetValue;
        }

        return _ySizeValues;
    }

    protected RangedNumericValue[] GetMotionValues()
    {
        if ( _motionValues == null )
        {
            _motionValues      = new RangedNumericValue[ 3 ];
            _motionValues[ 0 ] = VelocityValue;
            _motionValues[ 1 ] = WindValue;
            _motionValues[ 2 ] = GravityValue;
        }

        return _motionValues;
    }

    /// <summary>
    /// Permanently scales the size of the emitter by scaling
    /// all of its ranged values related to size.
    /// </summary>
    public void ScaleSize( float scale )
    {
        if ( scale is 1f )
        {
            return;
        }

        ScaleSize( scale, scale );
    }

    /// <summary>
    /// Permanently scales the size of the emitter by scaling
    /// all of its ranged values related to size.
    /// </summary>
    public void ScaleSize( float scaleX, float scaleY )
    {
        if ( scaleX is 1f && scaleY is 1f )
        {
            return;
        }

        foreach ( var value in GetXSizeValues() )
        {
            value.Scale( scaleX );
        }

        foreach ( var value in GetYSizeValues() )
        {
            value.Scale( scaleY );
        }
    }

    /// <summary>
    /// Permanently scales the speed of the emitter by scaling all its
    /// ranged values related to motion.
    /// </summary>
    public void ScaleMotion( float scale )
    {
        if ( scale is 1f )
        {
            return;
        }

        foreach ( var value in GetMotionValues() )
        {
            value.Scale( scale );
        }
    }

    /**
     * Sets all size-related ranged values to match those of the template emitter.
     */
    public void MatchSize( ParticleEmitter template )
    {
        MatchXSize( template );
        MatchYSize( template );
    }

    /**
     * Sets all horizontal size-related ranged values to match those of the template emitter.
     */
    public void MatchXSize( ParticleEmitter template )
    {
        RangedNumericValue[] values         = GetXSizeValues();
        RangedNumericValue[] templateValues = template.GetXSizeValues();

        for ( var i = 0; i < values.Length; i++ )
        {
            values[ i ].Set( templateValues[ i ] );
        }
    }

    /**
     * Sets all vertical size-related ranged values to match those of the template emitter.
     */
    public void MatchYSize( ParticleEmitter template )
    {
        RangedNumericValue[] values         = GetYSizeValues();
        RangedNumericValue[] templateValues = template.GetYSizeValues();

        for ( var i = 0; i < values.Length; i++ )
        {
            values[ i ].Set( templateValues[ i ] );
        }
    }

    /**
     * Sets all motion-related ranged values to match those of the template emitter.
     */
    public void MatchMotion( ParticleEmitter template )
    {
        RangedNumericValue[] values         = GetMotionValues();
        RangedNumericValue[] templateValues = template.GetMotionValues();

        for ( var i = 0; i < values.Length; i++ )
        {
            values[ i ].Set( templateValues[ i ] );
        }
    }

    public void Save( StreamWriter output )
    {
        output.Write( Name + "\n" );
        output.Write( "- Delay -\n" );
        DelayValue.Save( output );
        output.Write( "- Duration - \n" );
        DurationValue.Save( output );
        output.Write( "- Count - \n" );
        output.Write( "min: " + MinParticleCount + "\n" );
        output.Write( "max: " + MaxParticleCount + "\n" );
        output.Write( "- Emission - \n" );
        EmissionValue.Save( output );
        output.Write( "- Life - \n" );
        LifeValue.Save( output );
        output.Write( "- Life Offset - \n" );
        LifeOffsetValue.Save( output );
        output.Write( "- X Offset - \n" );
        XOffsetValue.Save( output );
        output.Write( "- Y Offset - \n" );
        YOffsetValue.Save( output );
        output.Write( "- Spawn Shape - \n" );
        SpawnShapeValue.Save( output );
        output.Write( "- Spawn Width - \n" );
        SpawnWidthValue.Save( output );
        output.Write( "- Spawn Height - \n" );
        SpawnHeightValue.Save( output );
        output.Write( "- X Scale - \n" );
        XScaleValue.Save( output );
        output.Write( "- Y Scale - \n" );
        YScaleValue.Save( output );
        output.Write( "- Velocity - \n" );
        VelocityValue.Save( output );
        output.Write( "- Angle - \n" );
        AngleValue.Save( output );
        output.Write( "- Rotation - \n" );
        RotationValue.Save( output );
        output.Write( "- Wind - \n" );
        WindValue.Save( output );
        output.Write( "- Gravity - \n" );
        GravityValue.Save( output );
        output.Write( "- Tint - \n" );
        TintValue.Save( output );
        output.Write( "- Transparency - \n" );
        TransparencyValue.Save( output );
        output.Write( "- Options - \n" );
        output.Write( "attached: " + Attached + "\n" );
        output.Write( "continuous: " + Continuous + "\n" );
        output.Write( "aligned: " + Aligned + "\n" );
        output.Write( "additive: " + Additive + "\n" );
        output.Write( "behind: " + Behind + "\n" );
        output.Write( "premultipliedAlpha: " + PremultipliedAlpha + "\n" );
        output.Write( "spriteMode: " + SpriteMode + "\n" );
        output.Write( "- Image Paths -\n" );

        foreach ( var imagePath in ImagePaths )
        {
            output.Write( imagePath + "\n" );
        }

        output.Write( "\n" );
    }

    public void Load( StreamReader reader )
    {
        try
        {
            Name = ReadString( reader, "name" );
            reader.ReadLine();
            DelayValue.Load( reader );
            reader.ReadLine();
            DurationValue.Load( reader );
            reader.ReadLine();

            MinParticleCount = ReadInt( reader, "minParticleCount" );
            MaxParticleCount = ReadInt( reader, "maxParticleCount" );

            reader.ReadLine();
            EmissionValue.Load( reader );
            reader.ReadLine();
            LifeValue.Load( reader );
            reader.ReadLine();
            LifeOffsetValue.Load( reader );
            reader.ReadLine();
            XOffsetValue.Load( reader );
            reader.ReadLine();
            YOffsetValue.Load( reader );
            reader.ReadLine();
            SpawnShapeValue.Load( reader );
            reader.ReadLine();
            SpawnWidthValue.Load( reader );
            reader.ReadLine();
            SpawnHeightValue.Load( reader );

            var line = reader.ReadLine();

            if ( ( line != null ) && line.Trim().Equals( "- Scale -" ) )
            {
                XScaleValue.Load( reader );
                YScaleValue.Active = false;
            }
            else
            {
                XScaleValue.Load( reader );
                reader.ReadLine();
                YScaleValue.Load( reader );
            }

            reader.ReadLine();
            VelocityValue.Load( reader );
            reader.ReadLine();
            AngleValue.Load( reader );
            reader.ReadLine();
            RotationValue.Load( reader );
            reader.ReadLine();
            WindValue.Load( reader );
            reader.ReadLine();
            GravityValue.Load( reader );
            reader.ReadLine();
            TintValue.Load( reader );
            reader.ReadLine();
            TransparencyValue.Load( reader );
            reader.ReadLine();

            Attached   = ReadBoolean( reader, "attached" );
            Continuous = ReadBoolean( reader, "continuous" );
            Aligned    = ReadBoolean( reader, "aligned" );
            Additive   = ReadBoolean( reader, "additive" );
            Behind     = ReadBoolean( reader, "behind" );

            // Backwards compatibility
            line = reader.ReadLine();

            if ( line!.StartsWith( "premultipliedAlpha" ) )
            {
                PremultipliedAlpha = ReadBoolean( line );
                line               = reader.ReadLine();
            }

            if ( line!.StartsWith( "spriteMode" ) )
            {
                SpriteMode = Enum.Parse< SpriteModes >( ReadString( line ) );

//                line = reader.ReadLine();
            }

            List< string > imagePaths = [ ];

            while ( ( ( line = reader.ReadLine() ) != null ) && ( line.Length != 0 ) )
            {
                imagePaths.Add( line );
            }

            ImagePaths = imagePaths;
        }
        catch ( Exception ex )
        {
            //TODO: ???
            throw new GdxRuntimeException( "Error parsing emitter: " + Name, ex );
        }
    }

    private static string ReadString( string line )
    {
        return line.Substring( line.IndexOf( ":", StringComparison.Ordinal ) + 1 ).Trim();
    }

    private static string ReadString( StreamReader reader, string name )
    {
        var line = reader.ReadLine();

        if ( line == null )
        {
            throw new IOException( "Missing value: " + name );
        }

        return ReadString( line );
    }

    private static bool ReadBoolean( string line )
    {
        return bool.Parse( ReadString( line ) );
    }

    private static bool ReadBoolean( StreamReader reader, string name )
    {
        return bool.Parse( ReadString( reader, name ) );
    }

    private static int ReadInt( StreamReader reader, string name )
    {
        return int.Parse( ReadString( reader, name ) );
    }

    private static float ReadFloat( StreamReader reader, string name )
    {
        return float.Parse( ReadString( reader, name ) );
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class Particle : Sprite
    {
        public Particle( Sprite sprite ) : base( sprite )
        {
        }

        public int     CurrentLife      { get; set; }
        public float   XScale           { get; set; }
        public float   XScaleDiff       { get; set; }
        public float   Velocity         { get; set; }
        public float   VelocityDiff     { get; set; }
        public float   Angle            { get; set; }
        public float   AngleDiff        { get; set; }
        public float   AngleCos         { get; set; }
        public float   AngleSin         { get; set; }
        public int     Life             { get; set; }
        public float   YScale           { get; set; }
        public float   YScaleDiff       { get; set; }
        public float   RotationDiff     { get; set; }
        public float   Transparency     { get; set; }
        public float   TransparencyDiff { get; set; }
        public float   Wind             { get; set; }
        public float   WindDiff         { get; set; }
        public float   Gravity          { get; set; }
        public float   GravityDiff      { get; set; }
        public float[] Tint             { get; set; } = null!;
        public int     Frame            { get; set; }
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ParticleValue
    {
        public bool Active       { get; set; }
        public bool AlwaysActive { get; set; }

        public virtual bool IsActive()
        {
            return AlwaysActive || Active;
        }

        public virtual void Save( StreamWriter output )
        {
            if ( !AlwaysActive )
            {
                output.Write( "active: " + Active + "\n" );
            }
            else
            {
                Active = true;
            }
        }

        public virtual void Load( StreamReader reader )
        {
            Active = AlwaysActive || ReadBoolean( reader, "active" );
        }

        public virtual void Load( ParticleValue value )
        {
            Active       = value.Active;
            AlwaysActive = value.AlwaysActive;
        }
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class NumericValue : ParticleValue
    {
        public float Value { get; set; }

        public override void Save( StreamWriter output )
        {
            base.Save( output );

            if ( !Active )
            {
                return;
            }

            output.Write( "value: " + Value + "\n" );
        }

        public override void Load( StreamReader reader )
        {
            base.Load( reader );

            if ( !Active )
            {
                return;
            }

            Value = ReadFloat( reader, "value" );
        }

        public void Load( NumericValue value )
        {
            base.Load( value );
            Value = value.Value;
        }
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class RangedNumericValue : ParticleValue
    {
        public float LowMin { get; set; }
        public float LowMax { get; set; }

        public float NewLowValue()
        {
            return LowMin + ( ( LowMax - LowMin ) * MathUtils.Random() );
        }

        public void SetLow( float value )
        {
            LowMin = value;
            LowMax = value;
        }

        public void SetLow( float min, float max )
        {
            LowMin = min;
            LowMax = max;
        }

        /// <summary>
        /// permanently scales the range by a scalar.
        /// </summary>
        public virtual void Scale( float scale )
        {
            LowMin *= scale;
            LowMax *= scale;
        }

        public virtual void Set( RangedNumericValue value )
        {
            LowMin = value.LowMin;
            LowMax = value.LowMax;
        }

        public override void Save( StreamWriter output )
        {
            base.Save( output );

            if ( !Active )
            {
                return;
            }

            output.Write( "lowMin: " + LowMin + "\n" );
            output.Write( "lowMax: " + LowMax + "\n" );
        }

        public override void Load( StreamReader reader )
        {
            base.Load( reader );

            if ( !Active )
            {
                return;
            }

            LowMin = ReadFloat( reader, "lowMin" );
            LowMax = ReadFloat( reader, "lowMax" );
        }

        public virtual void Load( RangedNumericValue value )
        {
            base.Load( value );
            LowMax = value.LowMax;
            LowMin = value.LowMin;
        }
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ScaledNumericValue : RangedNumericValue
    {
        public float[] Scaling    { get; set; } = [ 1 ];
        public float[] Timeline   { get; set; } = [ 0 ];
        public float   HighMin    { get; set; }
        public float   HighMax    { get; set; }
        public bool    IsRelative { get; set; }

        public float NewHighValue()
        {
            return HighMin + ( ( HighMax - HighMin ) * MathUtils.Random() );
        }

        public void SetHigh( float value )
        {
            HighMin = value;
            HighMax = value;
        }

        public void SetHigh( float min, float max )
        {
            HighMin = min;
            HighMax = max;
        }

        public override void Scale( float scale )
        {
            base.Scale( scale );

            HighMin *= scale;
            HighMax *= scale;
        }

        public override void Set( RangedNumericValue value )
        {
            if ( value is ScaledNumericValue numericValue )
            {
                Set( numericValue );
            }
            else
            {
                base.Set( value );
            }
        }

        public virtual void Set( ScaledNumericValue value )
        {
            base.Set( value );
            HighMin = value.HighMin;
            HighMax = value.HighMax;

            if ( Scaling.Length != value.Scaling.Length )
            {
                Array.Copy( value.Scaling, Scaling, value.Scaling.Length );
            }
            else
            {
                Array.Copy( value.Scaling, 0, Scaling, 0, Scaling.Length );
            }

            if ( Timeline.Length != value.Timeline.Length )
            {
                Array.Copy( value.Timeline, Timeline, value.Timeline.Length );
            }
            else
            {
                Array.Copy( value.Timeline, 0, Timeline, 0, Timeline.Length );
            }

            IsRelative = value.IsRelative;
        }

        public float GetScale( float percent )
        {
            var endIndex = -1;
            var timeline = Timeline;
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
                return Scaling[ n - 1 ];
            }

            var scaling    = Scaling;
            var startIndex = endIndex - 1;
            var startValue = scaling[ startIndex ];
            var startTime  = timeline[ startIndex ];

            return startValue + ( ( scaling[ endIndex ] - startValue ) * ( ( percent - startTime ) / ( timeline[ endIndex ] - startTime ) ) );
        }

        public override void Save( StreamWriter output )
        {
            base.Save( output );

            if ( !Active )
            {
                return;
            }

            output.Write( "highMin: " + HighMin + "\n" );
            output.Write( "highMax: " + HighMax + "\n" );
            output.Write( "relative: " + IsRelative + "\n" );
            output.Write( "scalingCount: " + Scaling.Length + "\n" );

            for ( var i = 0; i < Scaling.Length; i++ )
            {
                output.Write( "scaling" + i + ": " + Scaling[ i ] + "\n" );
            }

            output.Write( "timelineCount: " + Timeline.Length + "\n" );

            for ( var i = 0; i < Timeline.Length; i++ )
            {
                output.Write( "timeline" + i + ": " + Timeline[ i ] + "\n" );
            }
        }

        public override void Load( StreamReader reader )
        {
            base.Load( reader );

            if ( !Active )
            {
                return;
            }

            HighMin    = ReadFloat( reader, "highMin" );
            HighMax    = ReadFloat( reader, "highMax" );
            IsRelative = ReadBoolean( reader, "relative" );
            Scaling    = new float[ ReadInt( reader, "scalingCount" ) ];

            for ( var i = 0; i < Scaling.Length; i++ )
            {
                Scaling[ i ] = ReadFloat( reader, "scaling" + i );
            }

            Timeline = new float[ ReadInt( reader, "timelineCount" ) ];

            for ( var i = 0; i < Timeline.Length; i++ )
            {
                Timeline[ i ] = ReadFloat( reader, "timeline" + i );
            }
        }

        public void Load( ScaledNumericValue value )
        {
            base.Load( value );
            HighMax = value.HighMax;
            HighMin = value.HighMin;
            Scaling = new float[ value.Scaling.Length ];

            Array.Copy( value.Scaling, 0, Scaling, 0, Scaling.Length );
            Timeline = new float[ value.Timeline.Length ];

            Array.Copy( value.Timeline, 0, Timeline, 0, Timeline.Length );
            IsRelative = value.IsRelative;
        }
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class IndependentScaledNumericValue : ScaledNumericValue
    {
        public bool Independent { get; set; }

        public override void Set( RangedNumericValue value )
        {
            if ( value is IndependentScaledNumericValue numericValue )
            {
                Set( numericValue );
            }
            else
            {
                base.Set( value );
            }
        }

        public override void Set( ScaledNumericValue value )
        {
            if ( value is IndependentScaledNumericValue numericValue )
            {
                Set( numericValue );
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

        public override void Save( StreamWriter output )
        {
            base.Save( output );
            output.Write( "independent: " + Independent + "\n" );
        }

        public override void Load( StreamReader reader )
        {
            base.Load( reader );

            // For backwards compatibility, independent property may not be defined
//            if ( reader.markSupported() )
//            {
//                reader.mark( 100 );
//            }

            var line = reader.ReadLine();

            if ( line == null )
            {
                throw new IOException( "Missing value: independent" );
            }

            if ( line.Contains( "independent" ) )
            {
                Independent = bool.Parse( ReadString( line ) );
            }

            //            else if ( reader.markSupported() )
            //            {
            //                reader.reset();
            //            }
            else
            {
                // BufferedReader.MarkSupported may return false in some platforms,
                // in which case backwards commpatibility is not possible.
                const string ERROR_MESSAGE = "The loaded particle effect descriptor file uses an old invalid format. "
                                           + "Please download the latest version of the Particle Editor tool and "
                                           + "recreate the file by loading and saving it again.";

                Logger.Error( ERROR_MESSAGE );

                throw new IOException( ERROR_MESSAGE );
            }
        }

        public void Load( IndependentScaledNumericValue value )
        {
            base.Load( value );
            Independent = value.Independent;
        }
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class GradientColorValue : ParticleValue
    {
        private readonly float[] _temp = new float[ 4 ];

        public GradientColorValue()
        {
            AlwaysActive = true;
        }

        public float[] Timeline { get; set; } = [ 0 ];
        public float[] Colors   { get; set; } = [ 1, 1, 1 ];

        public float[] GetColor( float percent )
        {
            int startIndex = 0, endIndex = -1;
            var timeline   = Timeline;
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

        public override void Save( StreamWriter output )
        {
            base.Save( output );

            if ( !Active )
            {
                return;
            }

            output.Write( "colorsCount: " + Colors.Length + "\n" );

            for ( var i = 0; i < Colors.Length; i++ )
            {
                output.Write( "colors" + i + ": " + Colors[ i ] + "\n" );
            }

            output.Write( "timelineCount: " + Timeline.Length + "\n" );

            for ( var i = 0; i < Timeline.Length; i++ )
            {
                output.Write( "timeline" + i + ": " + Timeline[ i ] + "\n" );
            }
        }

        public override void Load( StreamReader reader )
        {
            base.Load( reader );

            if ( !Active )
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

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ParticleSpawnShapeValue : ParticleValue
    {
        public bool             Edges { get; set; }
        public SpawnShape       Shape { get; set; } = SpawnShape.Point;
        public SpawnEllipseSide Side  { get; set; } = SpawnEllipseSide.Both;

        public override void Save( StreamWriter output )
        {
            base.Save( output );

            if ( !Active )
            {
                return;
            }

            output.Write( "shape: " + Shape + "\n" );

            if ( Shape == SpawnShape.Ellipse )
            {
                output.Write( "edges: " + Edges + "\n" );
                output.Write( "side: " + Side + "\n" );
            }
        }

        public override void Load( StreamReader reader )
        {
            base.Load( reader );

            if ( !Active )
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

        public void Load( ParticleSpawnShapeValue value )
        {
            base.Load( value );

            Shape = value.Shape;
            Edges = value.Edges;
            Side  = value.Side;
        }
    }

    // ------------------------------------------------------------------------

    #region properties

    /// <summary>
    /// Set whether to automatically return the <see cref="IBatch"/>'s blend
    /// function to the alpha-blending default (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA)
    /// when done drawing. Is true by default. If set to false, the IBatch's blend
    /// function is left as it was for drawing this ParticleEmitter, which prevents
    /// the IBatch from being flushed repeatedly if consecutive ParticleEmitters
    /// with the same additive or pre-multiplied alpha state are drawn in a row.
    /// <para>
    /// IMPORTANT: If set to false and if the next object to use this IBatch expects
    /// alpha blending, you are responsible for setting the IBatch's blend function
    /// to (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA) before that next object is drawn.
    /// </para>
    /// </summary>
    public bool CleansUpBlendFunction { get; set; } = true;

    public List< Sprite >                Sprites           { get; set; } = [ ];
    public SpriteModes                   SpriteMode        { get; set; } = SpriteModes.Single;
    public List< string >                ImagePaths        { get; set; } = [ ];
    public ScaledNumericValue            XScaleValue       { get; set; } = new();
    public ScaledNumericValue            YScaleValue       { get; set; } = new();
    public RangedNumericValue            DelayValue        { get; set; } = new();
    public IndependentScaledNumericValue LifeOffsetValue   { get; set; } = new();
    public RangedNumericValue            DurationValue     { get; set; } = new();
    public IndependentScaledNumericValue LifeValue         { get; set; } = new();
    public ScaledNumericValue            EmissionValue     { get; set; } = new();
    public ScaledNumericValue            RotationValue     { get; set; } = new();
    public ScaledNumericValue            VelocityValue     { get; set; } = new();
    public ScaledNumericValue            AngleValue        { get; set; } = new();
    public ScaledNumericValue            WindValue         { get; set; } = new();
    public ScaledNumericValue            GravityValue      { get; set; } = new();
    public ScaledNumericValue            TransparencyValue { get; set; } = new();
    public GradientColorValue            TintValue         { get; set; } = new();
    public RangedNumericValue            XOffsetValue      { get; set; } = new ScaledNumericValue();
    public RangedNumericValue            YOffsetValue      { get; set; } = new ScaledNumericValue();
    public ScaledNumericValue            SpawnWidthValue   { get; set; } = new();
    public ScaledNumericValue            SpawnHeightValue  { get; set; } = new();
    public ParticleSpawnShapeValue       SpawnShapeValue   { get; set; } = new();

    public bool[] IsActive           { get; set; } = [ false ];
    public float  Duration           { get; set; } = 1;
    public float  DurationTimer      { get; set; }
    public bool   Behind             { get; set; }
    public string Name               { get; set; } = "";
    public bool   Attached           { get; set; }
    public bool   Continuous         { get; set; }
    public bool   Aligned            { get; set; }
    public bool   Additive           { get; set; } = true;
    public bool   PremultipliedAlpha { get; set; } = false;
    public float  X                  { get; set; }
    public float  Y                  { get; set; }
    public int    ActiveCount        { get; set; }
    public int    MinParticleCount   { get; set; }
    public int    MaxParticleCount   { get; set; } = DEFAULT_MAX_PARTICLE_COUNT;

    #endregion properties
}
