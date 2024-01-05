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

namespace LibGDXSharp.Maps;

/// <summary>
///     Set of string indexed values representing map elements' properties, allowing
///     to retrieve, modify and add properties to the set.
/// </summary>
public class MapProperties
{
    private Dictionary< string, object? > _properties;

    public MapProperties() => _properties = new Dictionary< string, object? >();

    public bool ContainsKey( string key ) => _properties.ContainsKey( key );

    public object? Get( string key ) => _properties[ key ];

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? Get<T>( string key ) => ( T? )Get( key );

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

        return obj == null ? defaultValue : ( T )obj;
    }

    public void Put( string key, object? value ) => _properties[ key ] = value;

    public void PutAll( MapProperties properties ) => _properties = new Dictionary< string, object? >( properties._properties );

    public void Remove( string key ) => _properties.Remove( key );

    public void Clear() => _properties.Clear();

    public Dictionary< string, object? >.KeyCollection GetKeys() => _properties.Keys;

    public Dictionary< string, object? >.ValueCollection GetValues() => _properties.Values;
}
