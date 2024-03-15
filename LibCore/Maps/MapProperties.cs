// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.LibCore.Maps;

/// <summary>
///     Set of string indexed values representing map elements' properties, allowing
///     to retrieve, modify and add properties to the set.
/// </summary>
public class MapProperties
{
    // The properties map.
    private Dictionary< string, object? > _properties = new();

    /// <summary>
    ///     Gets the property matching the specified key.
    /// </summary>
    /// <param name="key"> The Key. </param>
    /// <typeparam name="T"> The Type of the required property. </typeparam>
    /// <returns></returns>
    public T? Get<T>( string key )
    {
        return ( T? )Get( key );
    }

    /// <summary>
    ///     Gets the property matching the specified key. If the fetched property
    ///     is null the provided <paramref name="defaultValue" /> is returned instead.
    /// </summary>
    /// <param name="key"> The Key. </param>
    /// <param name="defaultValue"> The default value. </param>
    /// <typeparam name="T"> The Type of the required property. </typeparam>
    /// <returns></returns>
    public T Get<T>( string key, T defaultValue )
    {
        var obj = Get( key );

        return obj == null ? defaultValue : ( T )obj;
    }

    public object? Get( string key )
    {
        return _properties[ key ];
    }

    /// <summary>
    ///     Returns true if the properties map contains the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey( string key )
    {
        return _properties.ContainsKey( key );
    }

    public void Put( string key, object? value )
    {
        _properties[ key ] = value;
    }

    public void PutAll( MapProperties properties )
    {
        _properties = new Dictionary< string, object? >( properties._properties );
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
