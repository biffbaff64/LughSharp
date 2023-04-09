using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maps.Tiled
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public class TiledMap : Map, IDisposable
    {
        public TiledMapTileSets Tilesets       { get;         set; }
        public List< object >?  OwnedResources { private get; set; }

        public TiledMap()
        {
            Tilesets = new TiledMapTileSets();
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( OwnedResources != null )
                {
                    foreach( IDisposable disposable in OwnedResources )
                    {
                        disposable.Dispose();
                    }

                    OwnedResources = null;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Allows an object to try to free resources and perform other cleanup
        /// operations before it is reclaimed by garbage collection.
        /// </summary>
        ~TiledMap()
        {
            Dispose( false );
        }
    }
}
