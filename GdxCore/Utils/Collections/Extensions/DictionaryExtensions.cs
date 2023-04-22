using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils.Collections.Extensions
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public static class DictionaryExtension
    {
        /// <summary>
        /// Allows to retrieve the value associated with the specified key from the dictionary.
        /// If there's no such key in the dictionary, the default value is returned instead.
        /// </summary>
        public static TV Get<TK, TV>( this Dictionary<TK, TV> self, TK key, TV defaultValue ) where TK : notnull
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
        public static TV Get<TK, TV>( this Dictionary<TK, TV> self, TK key ) where TK : notnull
        {
            if ( key == null ) throw new GdxRuntimeException( "key is null" );
            
            if ( self.TryGetValue( key, out TV? value ) ) return value;
            
            throw new KeyNotFoundException( key.ToString() );
        }

        /// <summary>
        /// Allows to add a new key to the dictionary, even if the dictionary already
        /// contains this key.
        /// </summary>
        public static void Put<TK, TV>( this Dictionary<TK, TV> self, TK key, TV value ) where TK : notnull
        {
            self.Remove(key);
            self.Add( key, value );
        }
    }
}

