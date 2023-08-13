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

namespace LibGDXSharp.G2D;

public class ParticleEmitter
{
    public int         Duration      { get; set; }
    public int         DurationTimer { get; set; }
    public BoundingBox BoundingBox   { get; set; }
    
    public ParticleEmitter( BufferedStream reader )
    {
    }

    public ParticleEmitter( ParticleEmitter emitter )
    {
    }

    public void Start()
    {
    }

    public void Update( float delta )
    {
    }

    public void Draw( IBatch spriteBatch )
    {
    }

    public void Draw( IBatch spriteBatch, float delta )
    {
    }

    public void AllowCompletion()
    {
    }

    public bool IsComplete()
    {
        return false;
    }

    public void SetContinuous( bool b )
    {
    }

    public void SetPosition( float f, float f1 )
    {
    }

    public void SetFlip( bool flipX, bool flipY )
    {
    }

    public void FlipY()
    {
    }

    public string GetName()
    {
        return null;
    }

    public void PreAllocateParticles()
    {
    }

    public void Save( StreamWriter output )
    {
    }

    public void SetCleansUpBlendFunction( bool cleanUpBlendFunction )
    {
    }

    public void ScaleSize( float xSizeScaleFactor, float ySizeScaleFactor )
    {
    }

    public void ScaleMotion( float motionScaleFactor )
    {
    }

    public void Reset()
    {
    }

    public IEnumerable<Sprite> GetSprites()
    {
        yield break;
    }

    public void SetSprites( List< Sprite > sprites )
    {
    }
}
