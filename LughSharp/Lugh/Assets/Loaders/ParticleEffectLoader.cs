// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Assets.Loaders.Resolvers;
using LughSharp.Lugh.Graphics.G2D;

namespace LughSharp.Lugh.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> to load <see cref="ParticleEffect"/> instances. Passing a
/// <see cref="ParticleEffectParameter"/> to <see cref="AssetManager.AddToLoadqueue"/>
/// allows to specify an atlas file or an image directory to be used for the effect's images.
/// Per default images are loaded from the directory in which the effect file is found.
/// </summary>
[PublicAPI]
public class ParticleEffectLoader
    : SynchronousAssetLoader< ParticleEffect, ParticleEffectLoader.ParticleEffectParameter >
{
    /// <summary>
    /// Creates a new <see cref="ParticleEffect"/> loader using the
    /// provided <see cref="IFileHandleResolver"/>
    /// </summary>
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
    /// <param name="file">The file information associated with the asset.</param>
    /// <param name="param">The parameters used for loading the asset.</param>
    /// <returns>
    /// A loaded instance of the <see cref="ParticleEffect"/> class.
    /// </returns>
    public override ParticleEffect Load( AssetManager? am, FileInfo? file, ParticleEffectParameter? param )
    {
        ArgumentNullException.ThrowIfNull( am );
        ArgumentNullException.ThrowIfNull( file );

        var effect = new ParticleEffect();

        switch ( param )
        {
            case { AtlasFile: not null }:
                var atlas = am.Get( param.AtlasFile ) as TextureAtlas;

                effect.Load( file, atlas!, param.AtlasPrefix );

                break;

            case { ImagesDir: not null }:
                effect.Load( file, param.ImagesDir );

                break;

            default:
                effect.Load( file, file.Directory! );

                break;
        }

        return effect;
    }

    /// <summary>
    /// Retrieves a list of asset dependencies required for loading a particle effect.
    /// <para>
    /// This method checks if the provided <paramref name="parameters"/> is not null and if the associated
    /// <see cref="ParticleEffectParameter.AtlasFile"/> is not null. If both conditions are met, the
    /// method creates a list of asset descriptors containing a single <see cref="AssetDescriptor"/>
    /// representing the dependency on a <see cref="TextureAtlas"/> loaded from the specified
    /// <see cref="ParticleEffectParameter.AtlasFile"/>.
    /// </para>
    /// </summary>
    /// <param name="fileName">The name of the file associated with the asset.</param>
    /// <param name="file">The file information associated with the asset.</param>
    /// <param name="parameters">The parameters used for loading the asset.</param>
    /// <returns>
    /// A list of asset descriptors representing the dependencies required for loading the particle effect.
    /// </returns>
    public override List< AssetDescriptor > GetDependencies< TP >( string fileName,
                                                                   FileInfo file,
                                                                   TP? parameters ) where TP : class
    {
        var p = parameters as ParticleEffectParameter;
        
        List< AssetDescriptor >? deps = null;

        if ( p?.AtlasFile != null )
        {
            deps = [ new AssetDescriptor( p.AtlasFile!, typeof( TextureAtlas ), p ) ];
        }

        return deps!;
    }

    /// <summary>
    /// Parameter to be passed to <see cref="AssetManager.AddToLoadqueue"/>
    /// if additional configuration is necessary for the <see cref="ParticleEffect"/>.
    /// </summary>
    [PublicAPI]
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