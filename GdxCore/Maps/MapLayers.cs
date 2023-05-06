using System.Collections;

namespace LibGDXSharp.Maps;

public sealed class MapLayers : IEnumerable< MapLayer >
{
    private readonly List< MapLayer > _layers = new List< MapLayer >();

    public MapLayer Get( int index )
    {
        return _layers[ index ];
    }

    public MapLayer? Get( string name )
    {
        if ( name.Equals( string.Empty ) ) return null;
            
        for ( int i = 0, n = _layers.Count; i < n; i++ )
        {
            MapLayer layer = _layers[ i ];

            if ( name.Equals( layer.Name ) )
            {
                return layer;
            }
        }

        return null;
    }

    public int GetIndex( string name )
    {
        MapLayer? layer = Get( name );
            
        return layer != null ? GetIndex( layer ) : throw new NullReferenceException();
    }

    public int GetIndex( MapLayer layer )
    {
        return _layers.IndexOf( layer );
    }

    public int GetCount()
    {
        return _layers.Count;
    }

    public void Add( MapLayer layer )
    {
        this._layers.Add( layer );
    }

    public void Remove( int index )
    {
        _layers.RemoveAt( index );
    }

    public void Remove( MapLayer layer )
    {
        _layers.Remove( layer );
    }

    public int Size()
    {
        return _layers.Count;
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List< T > GetByType<T>() where T : MapLayer
    {
        return GetByType( new List< T >() );
    }

    /// <summary>
    /// </summary>
    /// <param name="fill"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List< T > GetByType<T>( List< T > fill ) where T : MapLayer
    {
        fill.Clear();
        fill.AddRange( _layers.OfType<T>() );

        return fill;
    }

    public IEnumerator< MapLayer > GetEnumerator()
    {
        return _layers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}