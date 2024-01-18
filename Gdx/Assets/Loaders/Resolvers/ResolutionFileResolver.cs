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

namespace LibGDXSharp.Assets.Loaders.Resolvers;

/// <summary>
///     This <see cref="IFileHandleResolver" /> uses a given list of <see cref="Resolution" />s to
///     determine the best match based on the current back buffer size. An example of how this
///     resolver works:
///     <p>
///         Let's assume that we have only a single <see cref="Resolution" />; added to this
///         resolver. This resolution has the following properties:
///     </p>
///     <ul>
///         <li>
///             <tt> portraitWidth = 1920</tt>
///         </li>
///         <li>
///             <tt> portraitHeight = 1080</tt>
///         </li>
///         <li>
///             <tt> folder = "1920x1080"</tt>
///         </li>
///     </ul>
///     <p>
///         One would now supply a file to be found to the resolver. For this example, we assume it is
///         <tt>"textures/walls/brick.png"</tt>. Since there is only a single <see cref="Resolution" />,
///         this will be the best match for any screen size. The resolver will now try to find the file
///         in the following ways:
///     </p>
///     <ul>
///         <li>
///             <tt>"textures/walls/1920x1080/brick.png"</tt>
///         </li>
///         <li>
///             <tt>"textures/walls/brick.png"</tt>
///         </li>
///     </ul>
///     <p>
///         The files are ultimately resolved via the given <see cref="baseResolver" />. In case the
///         first version cannot be resolved, the fallback will try to search for the file without
///         the resolution folder.
///     </p>
/// </summary>
public class ResolutionFileResolver : IFileHandleResolver
{
    protected readonly IFileHandleResolver baseResolver;
    protected readonly Resolution[]        descriptors;

    /// <summary>
    ///     Creates a <see cref="ResolutionFileResolver" /> based on a given
    ///     <see cref="IFileHandleResolver" /> and a list of <see cref="Resolution" />s.
    /// </summary>
    /// <param name="baseResolver">
    ///     The <see cref="IFileHandleResolver" /> that will ultimately used to resolve the file.
    /// </param>
    /// <param name="descs">
    ///     A list of <see cref="Resolution" />s. At least one has to be supplied.
    /// </param>
    public ResolutionFileResolver( IFileHandleResolver baseResolver, params Resolution[] descs )
    {
        if ( descs.Length == 0 )
        {
            throw new ArgumentException( "At least one Resolution needs to be supplied." );
        }

        this.baseResolver = baseResolver;
        descriptors       = descs;
    }

    public FileInfo Resolve( string fileName )
    {
        Resolution bestResolution = Choose( descriptors );
        FileInfo   originalHandle = new( fileName );
        FileInfo   handle         = baseResolver.Resolve( Resolve( originalHandle, bestResolution.Folder ) );

        if ( !handle.Exists )
        {
            handle = baseResolver.Resolve( fileName );
        }

        return handle;
    }

    protected static string Resolve( FileInfo originalHandle, string suffix )
    {
        var            parentstring = "";
        DirectoryInfo? parent       = originalHandle.Directory;

        if ( ( parent != null ) && !parent.Name.Equals( "" ) )
        {
            parentstring = parent + "/";
        }

        return parentstring + suffix + "/" + originalHandle.Name;
    }

    public static Resolution Choose( params Resolution[] descs )
    {
        var w = Gdx.Graphics.BackBufferWidth;
        var h = Gdx.Graphics.BackBufferHeight;

        // Prefer the shortest side.
        Resolution best = descs[ 0 ];

        if ( w < h )
        {
            for ( int i = 0, n = descs.Length; i < n; i++ )
            {
                Resolution other = descs[ i ];

                if ( ( w >= other.PortraitWidth )
                  && ( other.PortraitWidth >= best.PortraitWidth )
                  && ( h >= other.PortraitHeight )
                  && ( other.PortraitHeight >= best.PortraitHeight ) )
                {
                    best = descs[ i ];
                }
            }
        }
        else
        {
            for ( int i = 0, n = descs.Length; i < n; i++ )
            {
                Resolution other = descs[ i ];

                if ( ( w >= other.PortraitHeight )
                  && ( other.PortraitHeight >= best.PortraitHeight )
                  && ( h >= other.PortraitWidth )
                  && ( other.PortraitWidth >= best.PortraitWidth ) )
                {
                    best = descs[ i ];
                }
            }
        }

        return best;
    }

    public class Resolution
    {
        /// <summary>
        /// Constructs a {@code Resolution}.
        /// </summary>
        /// <param name="portraitWidth"> This resolution's width. </param>
        /// <param name="portraitHeight"> This resolution's height. </param>
        /// <param name="folder">
        /// The name of the folder, where the assets which fit this resolution, are located.
        /// </param>
        public Resolution( int portraitWidth, int portraitHeight, string folder )
        {
            PortraitWidth  = portraitWidth;
            PortraitHeight = portraitHeight;
            Folder         = folder;
        }

        public int PortraitWidth  { get; }
        public int PortraitHeight { get; }

        // The name of the folder, where the assets which fit this resolution, are located.
        public string Folder { get; }
    }
}
