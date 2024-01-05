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

namespace LibGDXSharp.Graphics.G2D;

public class ParticleEffectPool : Pool< ParticleEffectPool.PooledEffect >
{
    private readonly ParticleEffect _effect;

    public ParticleEffectPool( ParticleEffect effect, int initialCapacity, int max )
        : base( initialCapacity, max ) => _effect = effect;

    protected new PooledEffect NewObject()
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
                ParticleEmitter emitter         = emitters[ i ];
                ParticleEmitter templateEmitter = templateEmitters[ i ];

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
            : base( effect ) => _effectPool = pep;

        public void Free() => _effectPool.Free( this );
    }
}
