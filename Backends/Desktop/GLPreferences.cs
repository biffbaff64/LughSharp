using System.Xml.Linq;

using LibGDXSharp.Core;

namespace LibGDXSharp.Backends.Desktop
{
    public class GLPreferences : IPreferences
    {
        private readonly string     _filePath;
        private readonly string     _propertiesFile;
        private readonly XDocument? _xDocument;

        private readonly Dictionary< string, object >? _properties;

        /// <summary>
        /// </summary>
        /// <param name="filename"></param>
        public GLPreferences( string filename )
        {
            _filePath       = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile ) + "//.prefs//";
            _propertiesFile = filename;

            if ( !File.Exists( _filePath + _propertiesFile ) ) return;

            _properties = new Dictionary< string, object >();
            _xDocument  = new XDocument();
            _xDocument.Save( _filePath + _propertiesFile );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IPreferences PutEntry( string key, object? val )
        {
            if ( _properties == null ) throw new NullReferenceException();

            _properties[ key ] = val ?? throw new NullReferenceException();

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public IPreferences PutAll( Dictionary< string, object > vals )
        {
            foreach ( var entry in vals )
            {
                PutEntry( entry.Key, entry.Value );
            }

            return this;
        }

        public bool GetBoolean( string key )
        {
            return GetBoolean( key, false );
        }

        public int GetInteger( string key )
        {
            return GetInteger( key, 0 );
        }

        public long GetLong( string key )
        {
            return GetLong( key, 0 );
        }

        public float GetFloat( string key )
        {
            return GetFloat( key, 0 );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public bool GetBoolean( string key, bool defValue )
        {
            var value = _properties?[ key ];

            if ( ( value == null )
                 || ( ( value.ToString() != "true" ) && ( value.ToString() != "false" ) ) )
            {
                return defValue;
            }

            return Convert.ToBoolean( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public int GetInteger( string key, int defValue )
        {
            var value = _properties?[ key ];

            if ( value == null )
            {
                return defValue;
            }

            return Convert.ToInt16( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public long GetLong( string key, long defValue )
        {
            var value = _properties?[ key ];

            if ( value == null )
            {
                return defValue;
            }

            return Convert.ToInt32( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public float GetFloat( string key, float defValue )
        {
            var value = _properties?[ key ];

            if ( value == null )
            {
                return defValue;
            }

            return Convert.ToSingle( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public string GetString( string key, string defValue = "" )
        {
            var value = _properties?[ key ];

            if ( value == null )
            {
                return defValue;
            }

            return ( string )value;
        }

        public Dictionary< string, object > Get()
        {
            return _properties!;
        }

        public bool Contains( string key )
        {
            return _properties != null && ( _properties.ContainsKey( key ) );
        }

        public void Clear()
        {
            _properties?.Clear();
        }

        public void Remove( string key )
        {
            _properties?.Remove( key );
        }

        /// <summary>
        /// </summary>
        /// <exception cref="GdxRuntimeException"></exception>
        public void Flush()
        {
            try
            {
                var rootElement = new XElement( "properties" );
                _xDocument!.Add( rootElement );

                foreach ( var entry in _properties! )
                {
                    _xDocument.Add( new XElement( "entry" ) );
                    _xDocument.Add( new XAttribute( "key", entry.Key ) );
                    _xDocument.Add( new XAttribute( "value", entry.Value ) );
                }

                _xDocument.Save( _filePath + _propertiesFile );
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( "Error writing preferences!" );
            }
        }
    }
}
