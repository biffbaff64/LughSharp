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

using Corelib.LibCore.Graphics.G2D;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

/// <summary>
/// ParticleEffectActor holds an ParticleEffect to use in Scene2d applications.
/// The particle effect is positioned at 0, 0 in the ParticleEffectActor. Its
/// bounding box is not limited to the size of this actor.
/// </summary>
public class ParticleEffectActor : Actor, IDisposable
{
    protected readonly bool ownsEffect;

    private bool _resetOnStart;

    protected float lastDelta;

    // ========================================================================
    // ========================================================================

    public ParticleEffectActor( ParticleEffect particleEffect, bool resetOnStart )
    {
        ParticleEffect = particleEffect;
        _resetOnStart  = resetOnStart;
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
        _resetOnStart = resetOnStart;

        return this;
    }

    public ParticleEffectActor SetAutoRemove( bool autoRemove )
    {
        AutoRemove = autoRemove;

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

    // ========================================================================

    #region dispose pattern

    public void Dispose()
    {
        Dispose( true );
    }

    protected void Dispose( bool disposing )
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
