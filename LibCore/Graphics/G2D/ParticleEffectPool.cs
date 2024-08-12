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


using LughSharp.LibCore.Utils.Pooling;

namespace LughSharp.LibCore.Graphics.G2D;

[PublicAPI]
public class ParticleEffectPool : Pool< ParticleEffectPool.PooledEffect >
{
    private readonly ParticleEffect _effect;

    public ParticleEffectPool( ParticleEffect effect, int initialCapacity, int max )
        : base( initialCapacity, max )
    {
        _effect = effect;
    }

    //TODO: Tests needed for this method, check it's called properly
    public new PooledEffect NewObject()
    {
        var pooledEffect = new PooledEffect( _effect, this );
        pooledEffect.Start();

        return pooledEffect;
    }

    public override void Free( PooledEffect effect )
    {
        base.Free( effect );

        effect.Reset( false ); // copy parameters exactly to avoid introducing error

        if ( !effect.XSizeScale.Equals( _effect.XSizeScale )
          || !effect.YSizeScale.Equals( _effect.YSizeScale )
          || !effect.MotionScale.Equals( _effect.MotionScale ) )
        {
            List< ParticleEmitter > emitters         = effect.GetEmitters();
            List< ParticleEmitter > templateEmitters = _effect.GetEmitters();

            for ( var i = 0; i < emitters.Count; i++ )
            {
                var emitter         = emitters[ i ];
                var templateEmitter = templateEmitters[ i ];

                emitter.MatchSize( templateEmitter );
                emitter.MatchMotion( templateEmitter );
            }

            effect.XSizeScale  = _effect.XSizeScale;
            effect.YSizeScale  = _effect.YSizeScale;
            effect.MotionScale = _effect.MotionScale;
        }
    }


    public class PooledEffect : ParticleEffect
    {
        private readonly ParticleEffectPool _effectPool;

        public PooledEffect( ParticleEffect effect, ParticleEffectPool pep )
            : base( effect )
        {
            _effectPool = pep;
        }

        public void Free()
        {
            _effectPool.Free( this );
        }
    }
}
