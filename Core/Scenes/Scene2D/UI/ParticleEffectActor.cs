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

using LibGDXSharp.Graphics.G2D;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
///     ParticleEffectActor holds an ParticleEffect to use in Scene2d applications.
///     The particle effect is positioned at 0, 0 in the ParticleEffectActor. Its
///     bounding box is not limited to the size of this actor.
/// </summary>
public class ParticleEffectActor : Actor, IDisposable
{
    protected readonly bool  ownsEffect;

    private bool _resetOnStart;

    protected          float lastDelta;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public ParticleEffectActor( ParticleEffect particleEffect, bool resetOnStart )
    {
        this.ParticleEffect = particleEffect;
        this._resetOnStart  = resetOnStart;
    }

    public ParticleEffectActor( FileInfo particleFile, TextureAtlas atlas )
    {
        ParticleEffect = new ParticleEffect();
        ParticleEffect.Load( particleFile, atlas );
        ownsEffect = true;
    }

    public ParticleEffectActor( FileInfo particleFile, DirectoryInfo imagesDir )
    {
        ParticleEffect = new ParticleEffect();
        ParticleEffect.Load( particleFile, imagesDir );
        ownsEffect = true;
    }

    public bool           IsRunning      { get; set; }
    public bool           AutoRemove     { get; set; }
    public ParticleEffect ParticleEffect { get; }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        ParticleEffect.SetPosition( X, Y );

        if ( lastDelta > 0 )
        {
            ParticleEffect.Update( lastDelta );
            lastDelta = 0;
        }

        if ( IsRunning )
        {
            ParticleEffect.Draw( batch );
            IsRunning = !ParticleEffect.IsComplete();
        }
    }

    public override void Act( float delta )
    {
        base.Act( delta );

        // don't do particleEffect.update() here - the correct position
        // is set just while we are in draw() method. We save the delta
        // here to update in draw()
        lastDelta += delta;

        if ( AutoRemove && ParticleEffect.IsComplete() )
        {
            Remove();
        }
    }

    public void Start()
    {
        IsRunning = true;

        if ( _resetOnStart )
        {
            ParticleEffect.Reset( false );
        }

        ParticleEffect.Start();
    }

    public bool isResetOnStart()
    {
        return _resetOnStart;
    }

    public ParticleEffectActor setResetOnStart( bool resetOnStart )
    {
        this._resetOnStart = resetOnStart;

        return this;
    }

    public ParticleEffectActor SetAutoRemove( bool autoRemove )
    {
        this.AutoRemove = autoRemove;

        return this;
    }

    public override void ScaleChanged()
    {
        base.ScaleChanged();

        ParticleEffect.ScaleEffect( ScaleX, ScaleY, ScaleY );
    }

    public void Cancel()
    {
        IsRunning = true;
    }

    public void AllowCompletion()
    {
        ParticleEffect.AllowCompletion();
    }

    // ------------------------------------------------------------------------

    #region dispose pattern

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( ownsEffect )
            {
                ParticleEffect.Dispose();
            }
        }
    }

    #endregion dispose pattern
}
