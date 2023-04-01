using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class ResolutionFileResolver : IFileHandleResolver
    {
        public class Resolution
        {
            public readonly int portraitWidth;
            public readonly int portraitHeight;

            /// <summary>
            /// The name of the folder, where the assets which fit this resolution, are located.
            /// </summary>
            public readonly string folder;

            /// <summary>
            /// Constructs a {@code Resolution}. </summary>
            /// <param name="portraitWidth"> This resolution's width. </param>
            /// <param name="portraitHeight"> This resolution's height. </param>
            /// <param name="folder"> The name of the folder, where the assets which fit this resolution, are located.  </param>
            public Resolution( int portraitWidth, int portraitHeight, string folder )
            {
                this.portraitWidth  = portraitWidth;
                this.portraitHeight = portraitHeight;
                this.folder         = folder;
            }
        }

        protected readonly IFileHandleResolver baseResolver;
        protected readonly Resolution[]        descriptors;

        /// <summary>
        /// Creates a <code>ResolutionFileResolver</code> based on a given
        /// <see cref="IFileHandleResolver"/> and a list of <see cref="Resolution"/>s.
        /// </summary>
        /// <param name="baseResolver">
        /// The <see cref="IFileHandleResolver"/> that will ultimately used to resolve the file.
        /// </param>
        /// <param name="descriptors">
        /// A list of <see cref="Resolution"/>s. At least one has to be supplied.
        /// </param>
        public ResolutionFileResolver( IFileHandleResolver baseResolver, params Resolution[] descriptors )
        {
            if ( descriptors.Length == 0 )
            {
                throw new System.ArgumentException( "At least one Resolution needs to be supplied." );
            }

            this.baseResolver = baseResolver;
            this.descriptors  = descriptors;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileHandle Resolve( string fileName )
        {
            Resolution bestResolution = Choose( descriptors );
            FileHandle originalHandle = new FileHandle( fileName );
            FileHandle? handle         = baseResolver.Resolve( Resolve( originalHandle, bestResolution.folder ) );

            if ( !handle.Exists() )
            {
                handle = baseResolver.Resolve( fileName );
            }

            return handle;
        }

        /// <summary>
        /// </summary>
        /// <param name="originalHandle"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        protected string Resolve( FileHandle originalHandle, string suffix )
        {
            var        parentString = "";
            FileHandle parent = originalHandle.Parent();

            if ( parent != null && !parent.Name().Equals( "" ) )
            {
                parentString = parent + "/";
            }

            return parentString + suffix + "/" + originalHandle.Name();
        }

        /// <summary>
        /// </summary>
        /// <param name="descriptors"></param>
        /// <returns></returns>
        public static Resolution Choose( params Resolution[] descriptors )
        {
            var w = Gdx.Graphics?.GetBackBufferWidth();
            var h = Gdx.Graphics?.GetBackBufferHeight();

            // Prefer the shortest side.
            Resolution best = descriptors[ 0 ];

            if ( w < h )
            {
                for ( int i = 0, n = descriptors.Length; i < n; i++ )
                {
                    Resolution other = descriptors[ i ];

                    if ( w >= other.portraitWidth
                         && other.portraitWidth >= best.portraitWidth
                         && h >= other.portraitHeight
                         && other.portraitHeight >= best.portraitHeight )
                    {
                        best = descriptors[ i ];
                    }
                }
            }
            else
            {
                for ( int i = 0, n = descriptors.Length; i < n; i++ )
                {
                    Resolution other = descriptors[ i ];

                    if ( w >= other.portraitHeight
                         && other.portraitHeight >= best.portraitHeight
                         && h >= other.portraitWidth
                         && other.portraitWidth >= best.portraitWidth )
                    {
                        best = descriptors[ i ];
                    }
                }
            }

            return best;
        }
    }
}
