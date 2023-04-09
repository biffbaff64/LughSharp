using System.Collections;

namespace LibGDXSharp.Maps.Tiled
{
    using System.Collections.Generic;

    /// <summary>
    /// A Collection of <see cref="TiledMapTileSet"/> objects.
    /// </summary>
    public class TiledMapTileSets : IEnumerable< TiledMapTileSet >
    {

        private List< TiledMapTileSet > _tilesets;

        /// <summary>
        /// Creates an empty collection of tilesets. </summary>
        public TiledMapTileSets()
        {
            _tilesets = new List< TiledMapTileSet >();
        }

        /// <param name="index"> index to get the desired <seealso cref="TiledMapTileSet"/> at. </param>
        /// <returns> tileset at index  </returns>
        public virtual TiledMapTileSet GetTileSet( int index )
        {
            return _tilesets[ index ];
        }

        /// <param name="name"> Name of the <see cref="TiledMapTileSet"/> to retrieve.</param>
        /// <returns> tileset with matching name, null if it doesn't exist  </returns>
        public virtual TiledMapTileSet? GetTileSet( string name )
        {
            foreach ( TiledMapTileSet tileset in _tilesets )
            {
                if ( name.Equals( tileset.Name ) )
                {
                    return tileset;
                }
            }

            return null;
        }

        /// <param name="tileset"> set to be added to the collection </param>
        public virtual void AddTileSet( TiledMapTileSet tileset )
        {
            _tilesets.Add( tileset );
        }

        /// <summary>
        /// Removes tileset at index
        /// </summary>
        /// <param name="index"> index at which to remove a tileset. </param>
        public virtual void RemoveTileSet( int index )
        {
            _tilesets.RemoveAt( index );
        }

        /// <param name="tileset"> set to be removed </param>
        public virtual void RemoveTileSet( TiledMapTileSet tileset )
        {
            _tilesets.Remove( tileset );
        }

        /// <param name="id"> id of the <seealso cref="ITiledMapTile"/> to get. </param>
        /// <returns> tile with matching id, null if it doesn't exist  </returns>
        public virtual ITiledMapTile? GetTile( int id )
        {
            // The purpose of backward iteration here is to maintain backwards compatibility
            // with maps created with earlier versions of a shared tileset.  The assumption
            // is that the tilesets are in order of ascending firstgid, and by backward
            // iterating precedence for conflicts is given to later tilesets in the list, 
            // which are likely to be the earlier version of any given gid.  
            // See TiledMapModifiedExternalTilesetTest for example of this issue.
            for ( int i = _tilesets.Count - 1; i >= 0; i-- )
            {
                TiledMapTileSet tileset = _tilesets[ i ];
                ITiledMapTile?  tile    = tileset.GetTile( id );

                if ( tile != null )
                {
                    return tile;
                }
            }

            return null;
        }

        /// <returns> iterator to tilesets </returns>
        public virtual IEnumerator< TiledMapTileSet > GetEnumerator()
        {
            return _tilesets.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used
        /// to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
