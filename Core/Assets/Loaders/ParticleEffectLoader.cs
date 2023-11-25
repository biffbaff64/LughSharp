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
[PublicAPI]
public class ParticleEffectLoader
    : SynchronousAssetLoader< ParticleEffect, ParticleEffectLoader.ParticleEffectParameter >
{
    public ParticleEffectLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    /// <summary>
    /// Loads a particle effect asset using the provided asset manager, file information, and parameters.
    /// <para>
    /// This method constructs a new instance of <see cref="ParticleEffect"/> and loads its configuration
    /// based on the provided parameters. If the <paramref name="param"/> contains a non-null value for
    /// <see cref="ParticleEffectParameter.AtlasFile"/>, the effect's configuration is loaded using the
    /// associated <see cref="TextureAtlas"/> and atlas prefix. If the <paramref name="param"/> contains a
    /// non-null value for <see cref="ParticleEffectParameter.ImagesDir"/>, the effect's configuration is
    /// loaded using the specified images directory. Otherwise, the effect's configuration is loaded from
    /// the provided <paramref name="file"/>.
    /// </para>
    /// </summary>
    /// <param name="am">The asset manager used for loading dependent assets.</param>
    /// <param name="fileName">The name of the file associated with the asset.</param>
    /// <param name="file">The file information associated with the asset.</param>
    /// <param name="param">The parameters used for loading the asset.</param>
    /// <returns>
    /// A loaded instance of the <see cref="ParticleEffect"/> class.
    /// </returns>
    /// <seealso cref="ParticleEffect"/>
    /// <seealso cref="ParticleEffectParameter"/>
    /// <seealso cref="TextureAtlas"/>
    public override ParticleEffect Load( AssetManager? am,
                                         string? fileName,
                                         FileInfo? file,
                                         ParticleEffectParameter? param )
    {
        ArgumentNullException.ThrowIfNull( am );
        ArgumentNullException.ThrowIfNull( file );
        
        var effect = new ParticleEffect();

        if ( param is { AtlasFile: not null } )
        {
            effect.Load
                (
                file,
                am.Get< TextureAtlas >( param.AtlasFile, typeof( TextureAtlas ) ),
                param.AtlasPrefix
                );
        }
        else if ( param is { ImagesDir: not null } )
        {
            effect.Load( file, param.ImagesDir );
        }
        else
        {
            effect.Load( file, file.Directory! );
        }

        return effect;
    }

    /// <summary>
    /// Retrieves a list of asset dependencies required for loading a particle effect.
    /// <para>
    /// This method checks if the provided <paramref name="param"/> is not null and if the associated
    /// <see cref="ParticleEffectParameter.AtlasFile"/> is not null. If both conditions are met, the
    /// method creates a list of asset descriptors containing a single <see cref="AssetDescriptor"/>
    /// representing the dependency on a <see cref="TextureAtlas"/> loaded from the specified
    /// <see cref="ParticleEffectParameter.AtlasFile"/>.
    /// </para>
    /// </summary>
    /// <param name="fileName">The name of the file associated with the asset.</param>
    /// <param name="file">The file information associated with the asset.</param>
    /// <param name="param">The parameters used for loading the asset.</param>
    /// <seealso cref="ParticleEffectParameter"/>
    /// <seealso cref="AssetDescriptor"/>
    /// <seealso cref="TextureAtlas"/>
    /// <returns>
    /// A list of asset descriptors representing the dependencies required for loading the particle effect.
    /// </returns>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? param )
    {
        List< AssetDescriptor >? deps = null;

        if ( ( param != null )
          && !string.ReferenceEquals( ( ( ParticleEffectParameter )param ).AtlasFile, null ) )
        {
            deps = new List< AssetDescriptor >
            {
                new( typeof( TextureAtlas ),
                     ( ParticleEffectParameter )param,
                     ( ( ParticleEffectParameter )param ).AtlasFile! )
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
        public DirectoryInfo? ImagesDir { get; set; }
    }
}
