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

using System.Runtime.Serialization;
using System.Text;

using LibGDXSharp.Core.Utils.Collections;

namespace LibGDXSharp.Utils.Xml;

/// <summary>
/// Lightweight XML parser. Supports a subset of XML features: elements, attributes,
/// text, predefined entities, CDATA, mixed content. Namespaces are parsed as part
/// of the element or attribute name. Prologs and doctypes are ignored. Only 8-bit
/// character encodings are supported. Input is assumed to be well formed.
/// <para>
/// The default behavior is to parse the XML into a DOM. Extend this class and
/// override methods to perform event driven parsing. When this is done, the parse
/// methods will return null.
/// </para>
/// </summary>
[PublicAPI]
public partial class XmlReader
{
    private const int XML_START           = 1;
    private const int XML_FIRST_FINAL     = 34;
    private const int XML_ERROR           = 0;
    private const int XML_EN_ELEMENT_BODY = 15;
    private const int XML_EN_MAIN         = 1;

    private readonly List< Element > _elements   = new( 8 );
    private readonly StringBuilder   _textBuffer = new( 64 );
    private          Element?        _current;
    private          Element?        _root;

    // ----------------------------------------------------------
    // Code
    // ----------------------------------------------------------

    public virtual Element? Parse( string xml )
    {
        var data = xml.ToCharArray();

        return Parse( data, 0, data.Length );
    }

    public virtual Element? Parse( StreamReader reader )
    {
        try
        {
            var data   = new char[ 1024 ];
            var offset = 0;

            while ( true )
            {
                var length = reader.Read( data /*, offset, data.Length - offset*/ );

                if ( length == -1 )
                {
                    break;
                }

                if ( length == 0 )
                {
                    var newData = new char[ data.Length * 2 ];

                    Array.Copy( data, 0, newData, 0, data.Length );

                    data = newData;
                }
                else
                {
                    offset += length;
                }
            }

            return Parse( data, 0, offset );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex.Message );
        }
    }

    public virtual Element? Parse( FileInfo file )
    {
        try
        {
            return Parse( file.OpenText() );
        }
        catch ( Exception ex )
        {
            throw new SerializationException( "Error parsing file: " + file, ex );
        }
    }

    private Element? Parse( char[] data, int offset, int length )
    {
        //TODO: establish from Java LibGDX the meanings behind these variable names, then rename.
        var cs       = XML_START;
        var p        = offset;
        var pe       = length;
        var s        = 0;
        var hasBody  = false;
        var gotoTarg = 0;

        string? attributeName = null;

        _goto:

        while ( true )
        {
            switch ( gotoTarg )
            {
                case 0:
                    if ( p == pe )
                    {
                        gotoTarg = 4;

                        goto _goto;
                    }

                    if ( cs == XML_ERROR )
                    {
                        gotoTarg = 5;

                        goto _goto;
                    }

                    break;

                case 1:
                    _match:

                    int trans;

                    do
                    {
                        int keys = xmlKeyOffsets[ cs ];
                        trans = xmlIndexOffsets[ cs ];
                        int klen = xmlSingleLengths[ cs ];

                        if ( klen > 0 )
                        {
                            var lower = keys;
                            var upper = ( keys + klen ) - 1;

                            while ( true )
                            {
                                if ( upper < lower )
                                {
                                    break;
                                }

                                var mid = lower + ( ( upper - lower ) >> 1 );

                                if ( data[ p ] < xmlTransKeys[ mid ] )
                                {
                                    upper = mid - 1;
                                }
                                else if ( data[ p ] > xmlTransKeys[ mid ] )
                                {
                                    lower = mid + 1;
                                }
                                else
                                {
                                    trans += mid - keys;

                                    goto _match;
                                }
                            }

                            keys  += klen;
                            trans += klen;
                        }

                        klen = xmlRangeLengths[ cs ];

                        if ( klen > 0 )
                        {
                            var lower = keys;
                            var upper = ( keys + ( klen << 1 ) ) - 2;

                            while ( true )
                            {
                                if ( upper < lower )
                                {
                                    break;
                                }

                                var mid = lower + ( ( ( upper - lower ) >> 1 ) & ~1 );

                                if ( data[ p ] < xmlTransKeys[ mid ] )
                                {
                                    upper = mid - 2;
                                }
                                else if ( data[ p ] > xmlTransKeys[ mid + 1 ] )
                                {
                                    lower = mid + 2;
                                }
                                else
                                {
                                    trans += ( mid - keys ) >> 1;

                                    goto _match;
                                }
                            }

                            trans += klen;
                        }
                    }
                    while ( false );

                    trans = xmlIndicies[ trans ];
                    cs    = xmlTransTargs[ trans ];

                    if ( xmlTransActions[ trans ] != 0 )
                    {
                        int acts  = xmlTransActions[ trans ];
                        int nacts = xmlActions[ acts++ ];

                        while ( nacts-- > 0 )
                        {
                            switch ( xmlActions[ acts++ ] )
                            {
                                case 0:
                                {
                                    s = p;

                                    break;
                                }

                                case 1:
                                {
                                    var c = data[ s ];

                                    if ( ( c == '?' ) || ( c == '!' ) )
                                    {
                                        if ( ( data[ s + 1 ] == '[' )
                                          && ( data[ s + 2 ] == 'C' )
                                          && ( data[ s + 3 ] == 'D' )
                                          && ( data[ s + 4 ] == 'A' )
                                          && ( data[ s + 5 ] == 'T' )
                                          && ( data[ s + 6 ] == 'A' )
                                          && ( data[ s + 7 ] == '[' ) )
                                        {
                                            s += 8;
                                            p =  s + 2;

                                            while ( ( data[ p - 2 ] != ']' )
                                                 || ( data[ p - 1 ] != ']' )
                                                 || ( data[ p ] != '>' ) )
                                            {
                                                p++;
                                            }

                                            Text( new string( data, s, p - s - 2 ) );
                                        }
                                        else if ( ( c == '!' )
                                               && ( data[ s + 1 ] == '-' )
                                               && ( data[ s + 2 ] == '-' ) )
                                        {
                                            p = s + 3;

                                            while ( ( data[ p ] != '-' )
                                                 || ( data[ p + 1 ] != '-' )
                                                 || ( data[ p + 2 ] != '>' ) )
                                            {
                                                p++;
                                            }

                                            p += 2;
                                        }
                                        else
                                        {
                                            while ( data[ p ] != '>' )
                                            {
                                                p++;
                                            }
                                        }

                                        cs       = 15;
                                        gotoTarg = 2;

                                        if ( true )
                                        {
                                            goto _goto;
                                        }
                                    }

                                    hasBody = true;
                                    Open( new string( data, s, p - s ) );

                                    break;
                                }

                                case 2:
                                {
                                    hasBody = false;
                                    Close();

                                    cs       = 15;
                                    gotoTarg = 2;

                                    if ( true )
                                    {
                                        goto _goto;
                                    }

                                    break;
                                }

                                case 3:
                                {
                                    Close();

                                    cs       = 15;
                                    gotoTarg = 2;

                                    if ( true )
                                    {
                                        goto _goto;
                                    }

                                    break;
                                }

                                case 4:
                                {
                                    if ( hasBody )
                                    {
                                        cs       = 15;
                                        gotoTarg = 2;

                                        if ( true )
                                        {
                                            goto _goto;
                                        }
                                    }

                                    break;
                                }

                                case 5:
                                {
                                    attributeName = new string( data, s, p - s );

                                    break;
                                }

                                case 6:
                                {
                                    Attribute( attributeName!, new string( data, s, p - s ) );

                                    break;
                                }

                                case 7:
                                {
                                    var end = p;

                                    while ( end != s )
                                    {
                                        switch ( data[ end - 1 ] )
                                        {
                                            case ' ':
                                            case '\t':
                                            case '\n':
                                            case '\r':
                                                end--;

                                                continue;
                                        }

                                        break;
                                    }

                                    var current     = s;
                                    var entityFound = false;

                                    while ( current != end )
                                    {
                                        if ( data[ current++ ] != '&' )
                                        {
                                            continue;
                                        }

                                        var entityStart = current;

                                        while ( current != end )
                                        {
                                            if ( data[ current++ ] != ';' )
                                            {
                                                continue;
                                            }

                                            _textBuffer.Append( data, s, entityStart - s - 1 );

                                            var name  = new string( data, entityStart, current - entityStart - 1 );
                                            var value = Entity( name );

                                            _textBuffer.Append( value ?? name );

                                            s           = current;
                                            entityFound = true;

                                            break;
                                        }
                                    }

                                    if ( entityFound )
                                    {
                                        if ( s < end )
                                        {
                                            _textBuffer.Append( data, s, end - s );
                                        }

                                        Text( _textBuffer.ToString() );
                                        _textBuffer.Length = 0;
                                    }
                                    else
                                    {
                                        Text( new string( data, s, end - s ) );
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    break;

                case 2:
                    if ( cs == XML_ERROR )
                    {
                        gotoTarg = 5;

                        goto _goto;
                    }

                    if ( ++p != pe )
                    {
                        gotoTarg = 1;

                        goto _goto;
                    }

                    break;

                case 4:
                case 5:
                    break;
            }

            break;
        }

        if ( p < pe )
        {
            var lineNumber = 1;

            for ( var i = 0; i < p; i++ )
            {
                if ( data[ i ] == '\n' )
                {
                    lineNumber++;
                }
            }

            throw new SerializationException
                (
                 $"Error parsing XML on line {lineNumber} near: {new string( data, p, Math.Min( 32, pe - p ) )}"
                );
        }

        if ( _elements.Count != 0 )
        {
            Element element = _elements.Peek();
            _elements.Clear();

            throw new SerializationException( "Error parsing XML, unclosed element: " + element.Name );
        }

        Element? root = _root;
        _root = null;

        return root;
    }

    private void Open( string name )
    {
        var child = new Element( name, _current );

        _current?.AddChild( child );

        _elements.Add( child );
        _current = child;
    }

    private void Attribute( string name, string value )
    {
        _current?.SetAttribute( name, value );
    }

    public virtual string? Entity( string name )
    {
        if ( name.Equals( "lt" ) )
        {
            return "<";
        }

        if ( name.Equals( "gt" ) )
        {
            return ">";
        }

        if ( name.Equals( "amp" ) )
        {
            return "&";
        }

        if ( name.Equals( "apos" ) )
        {
            return "'";
        }

        if ( name.Equals( "quot" ) )
        {
            return "\"";
        }

        return name.StartsWith( "#x" ) ? char.Parse( name[ 2.. ] ).ToString() : null;
    }

    private void Text( string text )
    {
        var existing = _current?.Text;
        _current!.Text = existing != null ? existing + text : text;
    }

    private void Close()
    {
        _root    = _elements.Pop();
        _current = _elements.Count > 0 ? _elements.Peek() : null;
    }

    // --------------------------------------------------------------------
    //
    // --------------------------------------------------------------------

    [PublicAPI]
    public class Element
    {
        private List< Element >? _children;

        public string?                       Name       { get; }
        public ObjectMap< string, string? >? Attributes { get; set; }
        public Element?                      Parent     { get; set; }
        public string?                       Text       { get; set; }

        public Element( string name, Element? parent )
        {
            Name   = name;
            Parent = parent;
            Text   = string.Empty;
        }

        /// <summary>
        /// </summary>
        public int ChildCount => _children?.Count ?? 0;

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public string GetAttribute( string name )
        {
            ArgumentNullException.ThrowIfNull( name );

            if ( Attributes == null )
            {
                throw new GdxRuntimeException( $"Element {Name} doesn't have attribute: {name}" );
            }

            var value = Attributes.Get( name );

            if ( ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException( $"Element {Name} doesn't have attribute: {name}" );
            }

            return value;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string? GetAttribute( string name, string? defaultValue )
        {
            if ( Attributes == null )
            {
                return defaultValue;
            }

            var value = Attributes.Get( name );

            return value ?? defaultValue;

        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasAttribute( string name ) => ( Attributes != null ) && Attributes.ContainsKey( name );

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttribute( string name, string value )
        {
            Attributes ??= new ObjectMap< string, string? >( 8 );

            Attributes.Put( name, value );
        }

        public Element GetChild( int index )
        {
            if ( _children == null )
            {
                throw new GdxRuntimeException( "Element has no children: " + Name );
            }

            return _children[ index ];
        }

        public void AddChild( Element element )
        {
            _children ??= new List< Element >( 8 );

            _children.Add( element );
        }

        public void RemoveChild( int index ) => _children?.RemoveAt( index );

        public void RemoveChild( Element child ) => _children?.Remove( child );

        public void Remove() => Parent?.RemoveChild( this );

        /// <summary>
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToString( string indent = "" )
        {
            var buffer = new StringBuilder( 128 );

            buffer.Append( indent );
            buffer.Append( '<' );
            buffer.Append( Name );

            if ( Attributes != null )
            {
                List< string >  keys = Attributes.GetKeys().ToArray();
                List< string? > vals = Attributes.GetValues().ToArray();

                for ( var i = 0; i < Attributes.Size; i++ )
                {
                    buffer.Append( ' ' );
                    buffer.Append( keys[ i ] );
                    buffer.Append( "=\"" );
                    buffer.Append( vals[ i ] );
                    buffer.Append( '\"' );
                }
            }

            if ( ( _children == null ) && string.IsNullOrEmpty( Text ) )
            {
                buffer.Append( "/>" );
            }
            else
            {
                buffer.Append( ">\n" );

                var childIndent = indent + '\t';

                if ( !string.IsNullOrEmpty( Text ) )
                {
                    buffer.Append( childIndent );
                    buffer.Append( Text );
                    buffer.Append( '\n' );
                }

                if ( _children != null )
                {
                    foreach ( Element child in _children )
                    {
                        buffer.Append( child.ToString( childIndent ) );
                        buffer.Append( '\n' );
                    }
                }

                buffer.Append( indent );
                buffer.Append( "</" );
                buffer.Append( Name );
                buffer.Append( '>' );
            }

            return buffer.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="name">The name of the child <see cref="Element" /></param>
        /// <returns>The first child having the given name or null, does not recurse.</returns>
        public Element? GetChildByName( string name )
        {
            if ( _children == null )
            {
                return null;
            }

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach ( Element? element in _children )
            {
                if ( ( element.Name != null ) && ( element.Name.Equals( name ) ) )
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasChild( string name )
        {
            if ( _children == null )
            {
                return false;
            }

            return GetChildByName( name ) != null;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> the name of the child <see cref="Element" /></param>
        /// <returns> the first child having the given name or null, recurses </returns>
        public Element? GetChildByNameRecursive( string name )
        {
            if ( _children == null )
            {
                return null;
            }

            foreach ( Element? element in _children )
            {
                if ( ( element.Name != null ) && element.Name.Equals( name ) )
                {
                    return element;
                }

                Element? found = element.GetChildByNameRecursive( name );

                if ( found != null )
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasChildRecursive( string name )
        {
            if ( _children == null )
            {
                return false;
            }

            return GetChildByNameRecursive( name ) != null;
        }

        /// <param name="name"> the name of the children </param>
        /// <returns>
        /// the children with the given name or an empty <seealso cref="Array"/>
        /// </returns>
        public List< Element > GetChildrenByName( string name )
        {
            var result = new List< Element >();

            if ( _children == null )
            {
                return result;
            }

            // ----------------------------------------------------------------
            // FYI: This Linq...
            result.AddRange( _children.Where( child => ( child.Name != null ) && child.Name.Equals( name ) ) );

            // ...is the equivalent of this c#
            // foreach ( Element child in _children )
            // {
            //     if ( ( child.Name != null ) && child.Name.Equals( name ) )
            //     {
            //         result.Add( child );
            //     }
            // }

            // ----------------------------------------------------------------

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> the name of the children </param>
        /// <returns> the children with the given name or an empty <seealso cref="Array" />  </returns>
        public List< Element > GetChildrenByNameRecursively( string name )
        {
            var result = new List< Element >();

            GetChildrenByNameRecursively( name, result );

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="result"></param>
        private void GetChildrenByNameRecursively( string name, List< Element > result )
        {
            if ( _children == null )
            {
                return;
            }

            foreach ( Element child in _children )
            {
                if ( ( child.Name != null ) && child.Name.Equals( name ) )
                {
                    result.Add( child );
                }

                child.GetChildrenByNameRecursively( name, result );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetFloatAttribute( string name ) => float.Parse( GetAttribute( name ) );

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float GetFloatAttribute( string name, float defaultValue )
        {
            var value = GetAttribute( name, null );

            if ( ReferenceEquals( value, null ) )
            {
                return defaultValue;
            }

            return float.Parse( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetIntAttribute( string name ) => int.Parse( GetAttribute( name ) );

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetIntAttribute( string name, int defaultValue )
        {
            var value = GetAttribute( name, null );

            if ( ReferenceEquals( value, null ) )
            {
                return defaultValue;
            }

            return int.Parse( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetboolAttribute( string name ) => bool.Parse( GetAttribute( name ) );

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetboolAttribute( string name, bool defaultValue )
        {
            var value = GetAttribute( name, null );

            if ( ReferenceEquals( value, null ) )
            {
                return defaultValue;
            }

            return bool.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public string Get( string name )
        {
            var value = Get( name, null );

            if ( ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException( $"Element {Name} doesn't have attribute or child: {name}" );
            }

            return value;
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public string? Get( string name, string? defaultValue )
        {
            string? str;

            if ( Attributes != null )
            {
                str = Attributes.Get( name );

                if ( !ReferenceEquals( str, null ) )
                {
                    return str;
                }
            }

            Element? child = GetChildByName( name );

            if ( child == null )
            {
                return defaultValue;
            }

            str = child.Text;

            return str ?? defaultValue;
        }

        /// <summary>
        ///     Returns the attribute value with the specified name, or if no attribute
        ///     is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public int GetInt( string name )
        {
            var value = Get( name, null );

            if ( ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException
                    ( $"Element {Name} doesn't have attribute or child: {name}" );
            }

            return int.Parse( value );
        }

        /// <summary>
        ///     Returns the attribute value with the specified name, or if no attribute
        ///     is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public int GetInt( string name, int defaultValue )
        {
            var value = Get( name, null );

            return ReferenceEquals( value, null ) ? defaultValue : int.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public float GetFloat( string name )
        {
            var value = Get( name, null );

            if ( ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException
                    ( $"Element {Name} doesn't have attribute or child: {name}" );
            }

            return float.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public float GetFloat( string name, float defaultValue )
        {
            var value = Get( name, null );

            return ReferenceEquals( value, null ) ? defaultValue : float.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public bool GetBool( string name )
        {
            var value = Get( name, null );

            if ( ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException
                    ( $"Element {Name} doesn't have attribute or child: {name}" );
            }

            return bool.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        public bool GetBool( string name, bool defaultValue )
        {
            var value = Get( name, null );

            return ReferenceEquals( value, null ) ? defaultValue : bool.Parse( value );
        }
    }
}
