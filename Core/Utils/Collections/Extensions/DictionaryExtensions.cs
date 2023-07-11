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

namespace LibGDXSharp.Utils.Collections.Extensions;

public static class DictionaryExtension
{
    /// <summary>
    /// Returns the key for the specified value, or null if it is not in the map.
    /// Note this traverses the entire map and compares every value, which may be
    /// an expensive operation.
    /// </summary>
    public static TK? FindKey<TK, TV>( this Dictionary< TK, TV > self, TV value ) where TK : notnull
    {
        TK[] keyTable   = self.Keys.ToArray();
        TV[] valueTable = self.Values.ToArray();

        if ( value == null )
        {
            for ( var i = keyTable.Length - 1; i >= 0; i-- )
            {
                if ( valueTable[ i ] == null )
                {
                    return keyTable[ i ];
                }
            }
        }
        else
        {
            for ( var i = valueTable.Length - 1; i >= 0; i-- )
            {
                if ( value.Equals( valueTable[ i ] ) )
                {
                    return keyTable[ i ];
                }
            }
        }

        return default;
    }

    /// <summary>
    /// Allows to retrieve the value associated with the specified key from the dictionary.
    /// If there's no such key in the dictionary, the default value is returned instead.
    /// </summary>
    public static TV Get<TK, TV>( this Dictionary< TK, TV > self, TK key, TV defaultValue ) where TK : notnull
    {
        if ( key == null ) throw new GdxRuntimeException( "key is null" );

        return self.TryGetValue( key, out TV? value ) ? value : defaultValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public static TV Get<TK, TV>( this Dictionary< TK, TV > self, TK key ) where TK : notnull
    {
        if ( key == null ) throw new GdxRuntimeException( "key is null" );

        if ( self.TryGetValue( key, out TV? value ) ) return value;

        throw new KeyNotFoundException( key.ToString() );
    }

    /// <summary>
    /// Allows to add a new key to the dictionary, even if the dictionary already
    /// contains this key.
    /// </summary>
    public static void Put<TK, TV>( this Dictionary< TK, TV > self, TK key, TV value ) where TK : notnull
    {
        self.Remove( key );
        self.Add( key, value );
    }
}