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

namespace LibGDXSharp.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> to load <see cref="ParticleEffect"/> instances. Passing a
/// <see cref="ParticleEffectParameter"/> to <see cref="AssetManager.Load(string, Type, AssetLoaderParameters)"/>
/// allows to specify an atlas file or an image directory to be used for the effect's images.
/// Per default images are loaded from the directory in which the effect file is found. 
/// </summary>
public class ParticleEffectLoader : SynchronousAssetLoader< ParticleEffect, ParticleEffectLoader.ParticleEffectParameter >
{
    public ParticleEffectLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    public override ParticleEffect Load( AssetManager? am,
                                         string? fileName,
                                         FileInfo? file,
                                         ParticleEffectParameter? param )
    {
        var effect = new ParticleEffect();

        if ( param is { AtlasFile: not null } )
        {
            effect.Load( file, am?.Get< TextureAtlas >( param.AtlasFile, typeof( TextureAtlas ) ), param.AtlasPrefix );
        }
        else if ( param is { ImagesDir: not null } )
        {
            effect.Load( file, param.ImagesDir );
        }
        else
        {
            effect.Load( file );
        }

        return effect;
    }

    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? param )
    {
        List< AssetDescriptor >? deps = null;

        if ( ( param != null ) && !string.ReferenceEquals( ( ( ParticleEffectParameter )param ).AtlasFile, null ) )
        {
            deps = new List< AssetDescriptor >
            {
                new( ( ( ParticleEffectParameter )param ).AtlasFile,
                     typeof( TextureAtlas ),
                     ( ParticleEffectParameter )param )
            };
        }

        return deps!;
    }

    /// <summary>
    /// Parameter to be passed to <see cref="AssetManager.Load(string, Type, AssetLoaderParameters)"/>
    /// if additional configuration is necessary for the <see cref="ParticleEffect"/>. 
    /// </summary>
    public class ParticleEffectParameter : AssetLoaderParameters
    {
        /// <summary>
        /// Atlas file name.
        /// </summary>
        public string? AtlasFile { get; set; }

        /// <summary>
        /// Optional prefix to image names
        /// </summary>
        public string? AtlasPrefix { get; set; }

        /// <summary>
        /// Image directory.
        /// </summary>
        public FileInfo? ImagesDir { get; set; }
    }
}
