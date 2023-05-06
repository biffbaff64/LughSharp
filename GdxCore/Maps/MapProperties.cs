using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maps;

/// <summary>
/// Set of string indexed values representing map elements' properties, allowing
/// to retrieve, modify and add properties to the set.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class MapProperties
{
    private Dictionary< string, object? > _properties;

    public MapProperties()
    {
        _properties = new Dictionary< string, object? >();
    }

    public bool ContainsKey( string key )
    {
        return _properties.ContainsKey( key );
    }

    public object? Get( string key )
    {
        return _properties[ key ];
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? Get<T>( string key )
    {
        return ( T? )Get( key );
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>( string key, T defaultValue, Type type )
    {
        var obj = Get( key );

        return ( obj == null ) ? defaultValue : ( T )obj;
    }

    public void Put( string key, object? value )
    {
        _properties[ key ] = value;
    }

    public void PutAll( MapProperties properties )
    {
        _properties = new Dictionary<string, object?>( properties._properties );
    }

    public void Remove( string key )
    {
        _properties.Remove( key );
    }

    public void Clear()
    {
        _properties.Clear();
    }

    public Dictionary< string, object? >.KeyCollection GetKeys()
    {
        return _properties.Keys;
    }

    public Dictionary< string, object? >.ValueCollection GetValues()
    {
        return _properties.Values;
    }
}