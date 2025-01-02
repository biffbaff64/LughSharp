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

using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Utils.Collections;

[PublicAPI]
public static class DictionaryExtension
{
    /// <summary>
    /// Returns the key for the specified value, or null if it is not in the map.
    /// Note this traverses the entire map and compares every value, which may be
    /// an expensive operation.
    /// </summary>
    public static TK? FindKey< TK, TV >( this Dictionary< TK, TV > self, TV value ) where TK : notnull
    {
        var keyTable   = self.Keys.ToArray();
        var valueTable = self.Values.ToArray();

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

        return default( TK? );
    }

    /// <summary>
    /// Allows to retrieve the value associated with the specified key from the dictionary.
    /// If there's no such key in the dictionary, the default value is returned instead.
    /// </summary>
    public static TV Get< TK, TV >( this Dictionary< TK, TV > self, TK key, TV defaultValue ) where TK : notnull
    {
        if ( key == null )
        {
            throw new GdxRuntimeException( "key is null" );
        }

        return self.GetValueOrDefault( key, defaultValue );
    }

    /// <summary>
    /// Gets an entry from the Dictionary referenced by <paramref name="self"/>.
    /// </summary>
    /// <param name="self"> This Dictionary. </param>
    /// <param name="key"> The key index to use to get the entry. </param>
    /// <typeparam name="TK"> The Dictionarys Key type. </typeparam>
    /// <typeparam name="TV"> The Dictionarys Value type. </typeparam>
    /// <returns> The value associated with <paramref name="key"/>. </returns>
    public static TV? Get< TK, TV >( this Dictionary< TK, TV > self, TK key ) where TK : notnull
    {
        if ( key == null )
        {
            throw new GdxRuntimeException( "key is null" );
        }

        return self.TryGetValue( key, out var value ) ? value : default( TV );
    }
    
    /// <summary>
    /// Adds a new key to the dictionary, even if the dictionary already
    /// contains this key.
    /// </summary>
    public static void Put< TK, TV >( this Dictionary< TK, TV > self, TK key, TV value ) where TK : notnull
    {
        self.Remove( key );
        self.Add( key, value );
    }

    /// <summary>
    /// Puts all values in <paramref name="map"/> into <paramref name="self"/>.
    /// </summary>
    public static void PutAll< TK, TV >( this Dictionary< TK, TV > self, Dictionary< TK, TV > map ) where TK : notnull
    {
        self.EnsureCapacity( map.Count );

        var keyTable   = map.Keys.ToArray();
        var valueTable = map.Values.ToArray();

        for ( int i = 0, n = keyTable.Length; i < n; i++ )
        {
            self.Put( keyTable[ i ], valueTable[ i ] );
        }
    }
}
