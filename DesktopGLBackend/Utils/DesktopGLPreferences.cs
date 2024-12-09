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

using System.Xml.Linq;
using Corelib.Lugh.Core;
using Corelib.Lugh.Utils.Exceptions;
using JetBrains.Annotations;
using Environment = System.Environment;

namespace DesktopGLBackend.Utils;

/// <summary>
/// Represents a class to manage desktop GL preferences with methods to
/// store and retrieve various types of data.
/// </summary>
[PublicAPI]
public class DesktopGLPreferences : IPreferences
{
    private readonly string                        _filePath;
    private readonly Dictionary< string, object >? _properties;
    private readonly string                        _propertiesFile;
    private readonly XDocument?                    _xDocument;

    // ========================================================================
    // ========================================================================
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DesktopGLPreferences"/> class.
    /// </summary>
    /// <param name="filename"> The name of the preferences file. </param>
    public DesktopGLPreferences( string filename )
    {
        _filePath       = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile ) + "//.prefs//";
        _propertiesFile = filename;

        if ( !Path.Exists( _filePath + _propertiesFile ) )
        {
            return;
        }

        _properties = new Dictionary< string, object >();
        _xDocument  = new XDocument();
        _xDocument.Save( _filePath + _propertiesFile );
    }

    /// <summary>
    /// Adds or updates an entry in the preferences.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <param name="val"> The value of the preference entry. </param>
    /// <returns> The current <see cref="IPreferences"/> instance. </returns>
    /// <exception cref="NullReferenceException"> Thrown when the properties dictionary is null. </exception>
    public IPreferences PutEntry( string key, object? val )
    {
        if ( _properties == null )
        {
            throw new NullReferenceException();
        }

        _properties[ key ] = val ?? throw new NullReferenceException();

        return this;
    }

    /// <summary>
    /// Adds or updates multiple entries in the preferences.
    /// </summary>
    /// <param name="vals"> The dictionary of key-value pairs to add or update. </param>
    /// <returns> The current <see cref="IPreferences"/> instance. </returns>
    public IPreferences PutAll( Dictionary< string, object > vals )
    {
        foreach ( KeyValuePair< string, object > entry in vals )
        {
            PutEntry( entry.Key, entry.Value );
        }

        return this;
    }

    /// <summary>
    /// Gets a boolean value from the preferences.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <returns> The boolean value. </returns>
    public bool GetBool( string key ) => GetBool( key, false );

    /// <summary>
    /// Gets an integer value from the preferences.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <returns> The integer value. </returns>
    public int GetInteger( string key ) => GetInteger( key, 0 );

    /// <summary>
    /// Gets a long value from the preferences.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <returns> The long value. </returns>
    public long GetLong( string key ) => GetLong( key, 0 );

    /// <summary>
    /// Gets a float value from the preferences.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <returns> The float value. </returns>
    public float GetFloat( string key ) => GetFloat( key, 0 );

    /// <summary>
    /// Gets a boolean value from the preferences with a default value.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <param name="defValue"> The default value if the key does not exist. </param>
    /// <returns> The boolean value. </returns>
    public bool GetBool( string key, bool defValue )
    {
        var value = _properties?[ key ];

        if ( ( value == null ) || ( ( value.ToString() != "true" ) && ( value.ToString() != "false" ) ) )
        {
            return defValue;
        }

        return Convert.ToBoolean( value );
    }

    /// <summary>
    /// Gets an integer value from the preferences with a default value.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <param name="defValue"> The default value if the key does not exist. </param>
    /// <returns> The integer value. </returns>
    public int GetInteger( string key, int defValue )
    {
        var value = _properties?[ key ];

        return value == null ? defValue : Convert.ToInt16( value );
    }

    /// <summary>
    /// Gets a long value from the preferences with a default value.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <param name="defValue"> The default value if the key does not exist. </param>
    /// <returns> The long value. </returns>
    public long GetLong( string key, long defValue )
    {
        var value = _properties?[ key ];

        return value == null ? defValue : Convert.ToInt32( value );
    }

    /// <summary>
    /// Gets a float value from the preferences with a default value.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <param name="defValue"> The default value if the key does not exist. </param>
    /// <returns> The float value. </returns>
    public float GetFloat( string key, float defValue )
    {
        var value = _properties?[ key ];

        return value == null ? defValue : Convert.ToSingle( value );
    }

    /// <summary>
    /// Gets a string value from the preferences with an optional default value.
    /// </summary>
    /// <param name="key"> The key of the preference entry. </param>
    /// <param name="defValue">
    /// The default value if the key does not exist. Default is an empty string.
    /// </param>
    /// <returns> The string value. </returns>
    public string GetString( string key, string defValue = "" )
    {
        var value = _properties?[ key ];

        return value == null ? defValue : ( string ) value;
    }

    /// <summary>
    /// Gets all properties in the preferences.
    /// </summary>
    /// <returns> A dictionary of all properties. </returns>
    public Dictionary< string, object > Get() => _properties!;

    /// <summary>
    /// Checks if a key exists in the preferences.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns> True if the key exists; otherwise, false. </returns>
    public bool Contains( string key ) => ( _properties != null ) && _properties.ContainsKey( key );

    /// <summary>
    /// Clears all entries in the preferences.
    /// </summary>
    public void Clear() => _properties?.Clear();

    /// <summary>
    /// Removes a specific entry from the preferences.
    /// </summary>
    /// <param name="key"> The key of the entry to remove. </param>
    public void Remove( string key ) => _properties?.Remove( key );

    /// <summary>
    /// Saves the preferences to the file.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// Thrown when there is an error writing preferences.
    /// </exception>
    public void Flush()
    {
        try
        {
            var rootElement = new XElement( "properties" );
            _xDocument!.Add( rootElement );

            foreach ( KeyValuePair< string, object > entry in _properties! )
            {
                _xDocument.Add( new XElement( "entry" ) );
                _xDocument.Add( new XAttribute( "key", entry.Key ) );
                _xDocument.Add( new XAttribute( "value", entry.Value ) );
            }

            _xDocument.Save( _filePath + _propertiesFile );
        }
        catch ( Exception )
        {
            throw new GdxRuntimeException( "Error writing preferences!" );
        }
    }
}
