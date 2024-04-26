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


using Exception = System.Exception;

namespace LughSharp.Backends.DesktopGL.Utils;

public class DesktopGLPreferences : IPreferences
{
    private readonly string _filePath;

    private readonly Dictionary< string, object >? _properties;
    private readonly string                        _propertiesFile;
    private readonly XDocument?                    _xDocument;

    /// <summary>
    /// </summary>
    /// <param name="filename"></param>
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
    /// </summary>
    /// <param name="key"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
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
    /// </summary>
    /// <param name="vals"></param>
    /// <returns></returns>
    public IPreferences PutAll( Dictionary< string, object > vals )
    {
        foreach ( KeyValuePair< string, object > entry in vals )
        {
            PutEntry( entry.Key, entry.Value );
        }

        return this;
    }

    public bool GetBool( string key )
    {
        return GetBool( key, false );
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
    public bool GetBool( string key, bool defValue )
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

        return ( string ) value;
    }

    public Dictionary< string, object > Get()
    {
        return _properties!;
    }

    public bool Contains( string key )
    {
        return ( _properties != null ) && _properties.ContainsKey( key );
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