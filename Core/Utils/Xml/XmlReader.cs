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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;

using LibGDXSharp.Core.Utils.Collections;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Utils.Xml;

/// <summary>
/// Lightweight XML parser. Supports a subset of XML features: elements, attributes,
/// text, predefined entities, CDATA, mixed content. Namespaces are parsed as part
/// of the element or attribute name. Prologs and doctypes are ignored. Only 8-bit
/// character encodings are supported. Input is assumed to be well formed.
/// <p>
/// The default behavior is to parse the XML into a DOM. Extends this class and
/// override methods to perform event driven parsing. When this is done, the parse
/// methods will return null.
/// </p>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[Obsolete]
public sealed class XmlReader
{
    private const int Xml_Start          = 1;
    private const int Xml_First_Final    = 34;
    private const int Xml_Error          = 0;
    private const int Xml_En_ElementBody = 15;
    private const int Xml_En_Main        = 1;

    private readonly List< Element > _elements   = new( 8 );
    private readonly StringBuilder   _textBuffer = new( 64 );

    private Element? _root;
    private Element? _current;

    private static byte[] Init_XmlActions_0()
    {
        return new byte[]
        {
            0, 1, 0, 1, 1, 1, 2, 1, 3, 1, 4, 1, 5,
            1, 6, 1, 7, 2, 0, 6, 2, 1, 4, 2, 2, 4
        };
    }

    private static byte[] Init_XmlKeyOffsets_0()
    {
        return new byte[]
        {
            0, 0, 4, 9, 14, 20, 26, 30, 35, 36, 37, 42, 46, 50, 51,
            52, 56, 57, 62, 67, 73, 79, 83, 88, 89, 90, 95, 99, 103,
            104, 108, 109, 110, 111, 112, 115
        };
    }

    private static char[] Init_XmlTransKeys_0()
    {
        return new[]
        {
            ( char )32, ( char )60, ( char )9, ( char )13, ( char )32, ( char )47, ( char )62, ( char )9, ( char )13,
            ( char )32, ( char )47, ( char )62, ( char )9, ( char )13, ( char )32, ( char )47, ( char )61, ( char )62,
            ( char )9, ( char )13, ( char )32, ( char )47, ( char )61, ( char )62, ( char )9, ( char )13, ( char )32,
            ( char )61, ( char )9, ( char )13, ( char )32, ( char )34, ( char )39, ( char )9, ( char )13, ( char )34,
            ( char )34, ( char )32, ( char )47, ( char )62, ( char )9, ( char )13, ( char )32, ( char )62, ( char )9,
            ( char )13, ( char )32, ( char )62, ( char )9, ( char )13, ( char )39, ( char )39, ( char )32, ( char )60,
            ( char )9, ( char )13, ( char )60, ( char )32, ( char )47, ( char )62, ( char )9, ( char )13, ( char )32,
            ( char )47, ( char )62, ( char )9, ( char )13, ( char )32, ( char )47, ( char )61, ( char )62, ( char )9,
            ( char )13, ( char )32, ( char )47, ( char )61, ( char )62, ( char )9, ( char )13, ( char )32, ( char )61,
            ( char )9, ( char )13, ( char )32, ( char )34, ( char )39, ( char )9, ( char )13, ( char )34, ( char )34,
            ( char )32, ( char )47, ( char )62, ( char )9, ( char )13, ( char )32, ( char )62, ( char )9, ( char )13,
            ( char )32, ( char )62, ( char )9, ( char )13, ( char )60, ( char )32, ( char )47, ( char )9, ( char )13,
            ( char )62, ( char )62, ( char )39, ( char )39, ( char )32, ( char )9, ( char )13, ( char )0
        };
    }

    private static byte[] Init_XmlSingleLengths_0()
    {
        return new byte[]
        {
            0, 2, 3, 3, 4, 4, 2, 3, 1, 1, 3, 2, 2, 1, 1, 2, 1, 3,
            3, 4, 4, 2, 3, 1, 1, 3, 2, 2, 1, 2, 1, 1, 1, 1, 1, 0
        };
    }

    private static byte[] Init_XmlRangeLengths_0()
    {
        return new byte[]
        {
            0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 1,
            1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0
        };
    }

    private static short[] Init_XmlIndexOffsets_0()
    {
        return new short[]
        {
            0, 0, 4, 9, 14, 20, 26, 30, 35, 37, 39, 44, 48, 52, 54, 56, 60,
            62, 67, 72, 78, 84, 88, 93, 95, 97, 102, 106, 110, 112, 116, 118,
            120, 122, 124, 127
        };
    }

    private static byte[] Init_XmlIndicies_0()
    {
        return new byte[]
        {
            0, 2, 0, 1, 2, 1, 1, 2, 3, 5, 6, 7, 5, 4, 9, 10, 1, 11, 9, 8, 13,
            1, 14, 1, 13, 12, 15, 16, 15, 1, 16, 17, 18, 16, 1, 20, 19, 22, 21,
            9, 10, 11, 9, 1, 23, 24, 23, 1, 25, 11, 25, 1, 20, 26, 22, 27, 29,
            30, 29, 28, 32, 31, 30, 34, 1, 30, 33, 36, 37, 38, 36, 35, 40, 41,
            1, 42, 40, 39, 44, 1, 45, 1, 44, 43, 46, 47, 46, 1, 47, 48, 49, 47,
            1, 51, 50, 53, 52, 40, 41, 42, 40, 1, 54, 55, 54, 1, 56, 42, 56, 1,
            57, 1, 57, 34, 57, 1, 1, 58, 59, 58, 51, 60, 53, 61, 62, 62, 1, 1, 0
        };
    }

    private static byte[] Init_XmlTransTargs_0()
    {
        return new byte[]
        {
            1, 0, 2, 3, 3, 4, 11, 34, 5, 4, 11, 34, 5, 6, 7, 6, 7, 8, 13, 9, 10, 9,
            10, 12, 34, 12, 14, 14, 16, 15, 17, 16, 17, 18, 30, 18, 19, 26, 28, 20,
            19, 26, 28, 20, 21, 22, 21, 22, 23, 32, 24, 25, 24, 25, 27, 28, 27, 29,
            31, 35, 33, 33, 34
        };
    }

    private static byte[] Init_XmlTransActions_0()
    {
        return new byte[]
        {
            0, 0, 0, 1, 0, 3, 3, 20, 1, 0, 0, 9, 0, 11, 11, 0, 0, 0, 0, 1, 17, 0, 13,
            5, 23, 0, 1, 0, 1, 0, 0, 0, 15, 1, 0, 0, 3, 3, 20, 1, 0, 0, 9, 0, 11, 11,
            0, 0, 0, 0, 1, 17, 0, 13, 5, 23, 0, 0, 0, 7, 1, 0, 0
        };
    }

    private readonly static byte[]  xmlActions       = Init_XmlActions_0();
    private readonly static byte[]  xmlKeyOffsets    = Init_XmlKeyOffsets_0();
    private readonly static char[]  xmlTransKeys     = Init_XmlTransKeys_0();
    private readonly static byte[]  xmlSingleLengths = Init_XmlSingleLengths_0();
    private readonly static byte[]  xmlRangeLengths  = Init_XmlRangeLengths_0();
    private readonly static short[] xmlIndexOffsets  = Init_XmlIndexOffsets_0();
    private readonly static byte[]  xmlIndicies      = Init_XmlIndicies_0();
    private readonly static byte[]  xmlTransTargs    = Init_XmlTransTargs_0();
    private readonly static byte[]  xmlTransActions  = Init_XmlTransActions_0();

    // ----------------------------------------------------------
    // 
    // ----------------------------------------------------------

    [Obsolete]
    public Element Parse( string xml )
    {
        var data = xml.ToCharArray();

        return Parse( data, 0, data.Length );
    }

    [Obsolete]
    public Element Parse( Reader reader )
    {
        try
        {
            var data   = new char[ 1024 ];
            var offset = 0;

            while ( true )
            {
                var length = reader.Read( data, offset, data.Length - offset );

                if ( length == -1 ) break;

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
        finally
        {
            StreamUtils.CloseQuietly( reader );
        }
    }

    [Obsolete]
    public Element Parse( InputStream input )
    {
        try
        {
            return Parse( new InputStreamReader( input, "UTF-8" ) );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex.Message );
        }
        finally
        {
            StreamUtils.CloseQuietly( input );
        }
    }

    [Obsolete]
    public static Element Parse( FileInfo file )
    {
//        try
//        {
//            return Parse( file.OpenRead() );
//        }
//        catch ( Exception ex )
//        {
//            throw new SerializationException( "Error parsing file: " + file, ex );
//        }

        throw new NotImplementedException();
    }

    [Obsolete]
    private Element Parse( char[] data, int offset, int length )
    {
        int cs;
        var p  = offset;
        var pe = length;

        var     s             = 0;
        string? attributeName = null;
        var     hasBody       = false;

        // line 93 "XmlReader.java"
        {
            cs = Xml_Start;
        }

        // line 97 "XmlReader.java"
        {
            int klen;
            int trans = 0;
            int acts;
            int nacts;
            int keys;
            int gotoTarg = 0;

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

                        if ( cs == 0 )
                        {
                            gotoTarg = 5;

                            goto _goto;
                        }

                        break;
                    
                    case 1:
                        _match:

                        do
                        {
                            keys  = xmlKeyOffsets[ cs ];
                            trans = xmlIndexOffsets[ cs ];
                            klen  = xmlSingleLengths[ cs ];

                            if ( klen > 0 )
                            {
                                int lower = keys;
                                int mid;
                                int upper = ( keys + klen ) - 1;

                                while ( true )
                                {
                                    if ( upper < lower ) break;

                                    mid = lower + ( ( upper - lower ) >> 1 );

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
                                        trans += ( mid - keys );

                                        goto _match;
                                    }
                                }

                                keys  += klen;
                                trans += klen;
                            }

                            klen = xmlRangeLengths[ cs ];

                            if ( klen > 0 )
                            {
                                int lower = keys;
                                int mid;
                                int upper = ( keys + ( klen << 1 ) ) - 2;

                                while ( true )
                                {
                                    if ( upper < lower ) break;

                                    mid = lower + ( ( ( upper - lower ) >> 1 ) & ~1 );

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
                                        trans += ( ( mid - keys ) >> 1 );

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
                            acts  = xmlTransActions[ trans ];
                            nacts = ( int )xmlActions[ acts++ ];

                            while ( nacts-- > 0 )
                            {
                                switch ( xmlActions[ acts++ ] )
                                {
                                    case 0:
                                        // line 94 "XmlReader.rl"
                                    {
                                        s = p;
                                    }

                                        break;

                                    case 1:
                                        // line 95 "XmlReader.rl"
                                    {
                                        char c = data[ s ];

                                        if ( ( c == '?' ) || ( c == '!' ) )
                                        {
                                            if ( ( data[ s + 1 ] == '[' )
                                                 && //
                                                 ( data[ s + 2 ] == 'C' )
                                                 && //
                                                 ( data[ s + 3 ] == 'D' )
                                                 && //
                                                 ( data[ s + 4 ] == 'A' )
                                                 && //
                                                 ( data[ s + 5 ] == 'T' )
                                                 && //
                                                 ( data[ s + 6 ] == 'A' )
                                                 && //
                                                 ( data[ s + 7 ] == '[' ) )
                                            {
                                                s += 8;
                                                p =  s + 2;

                                                while ( ( data[ p - 2 ] != ']' )
                                                        || ( data[ p - 1 ] != ']' )
                                                        || ( data[ p ] != '>' ) )
                                                    p++;

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
                                                    p++;

                                                p += 2;
                                            }
                                            else
                                                while ( data[ p ] != '>' )
                                                    p++;

                                            {
                                                cs       = 15;
                                                gotoTarg = 2;

                                                if ( true ) goto _goto;
                                            }
                                        }

                                        hasBody = true;
                                        Open( new string( data, s, p - s ) );
                                    }

                                        break;

                                    case 2:
                                        // line 125 "XmlReader.rl"
                                    {
                                        hasBody = false;
                                        Close();

                                        {
                                            cs       = 15;
                                            gotoTarg = 2;

                                            if ( true )
                                                goto
                                                    _goto;
                                        }
                                    }

                                        break;

                                    case 3:
                                        // line 130 "XmlReader.rl"
                                    {
                                        Close();

                                        {
                                            cs       = 15;
                                            gotoTarg = 2;

                                            if ( true )
                                                goto
                                                    _goto;
                                        }
                                    }

                                        break;

                                    case 4:
                                        // line 134 "XmlReader.rl"
                                    {
                                        if ( hasBody )
                                        {
                                            cs       = 15;
                                            gotoTarg = 2;

                                            if ( true )
                                                goto
                                                    _goto;
                                        }
                                    }

                                        break;

                                    case 5:
                                        // line 137 "XmlReader.rl"
                                    {
                                        attributeName = new string( data, s, p - s );
                                    }

                                        break;

                                    case 6:
                                        // line 140 "XmlReader.rl"
                                    {
                                        Attribute( attributeName, new string( data, s, p - s ) );
                                    }

                                        break;

                                    case 7:
                                        // line 143 "XmlReader.rl"
                                    {
                                        int end = p;

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

                                        int  current     = s;
                                        bool entityFound = false;

                                        while ( current != end )
                                        {
                                            if ( data[ current++ ] != '&' ) continue;
                                            int entityStart = current;

                                            while ( current != end )
                                            {
                                                if ( data[ current++ ] != ';' ) continue;
                                                _textBuffer.Append( data, s, entityStart - s - 1 );

                                                string name = new string
                                                    ( data, entityStart, current - entityStart - 1 );

                                                string value = Entity( name );
                                                _textBuffer.Append( value != null ? value : name );
                                                s           = current;
                                                entityFound = true;

                                                break;
                                            }
                                        }

                                        if ( entityFound )
                                        {
                                            if ( s < end ) _textBuffer.Append( data, s, end - s );
                                            Text( _textBuffer.ToString() );
                                            _textBuffer.Length = 0;
                                        }
                                        else
                                        {
                                            Text( new string( data, s, end - s ) );
                                        }
                                    }

                                        break;
                                    // line 286 "XmlReader.java"
                                }
                            }
                        }

                        break;

                    case 2:
                        if ( cs == 0 )
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
        }

        // line 190 "XmlReader.rl"

        if ( p < pe )
        {
            int lineNumber = 1;

            for ( int i = 0; i < p; i++ )
                if ( data[ i ] == '\n' )
                    lineNumber++;

            throw new SerializationException
                (
                 "Error parsing XML on line " + lineNumber + " near: " + new string( data, p, Math.Min( 32, pe - p ) )
                );
        }
        else if ( _elements.Count != 0 )
        {
            Element element = _elements.Peek();
            _elements.Clear();

            throw new SerializationException( "Error parsing XML, unclosed element: " + element.Name );
        }

        Element? root = this._root;
        this._root = null;

        return root;
    }

    [Obsolete]
    private void Open( string name )
    {
        var child = new Element( name, _current );

        _current?.AddChild( child );

        _elements.Add( child );
        _current = child;
    }

    [Obsolete]
    private void Attribute( string name, string value )
    {
        _current?.SetAttribute( name, value );
    }

    [Obsolete]
    public string? Entity( string name )
    {
        if ( name.Equals( "lt" ) ) return "<";
        if ( name.Equals( "gt" ) ) return ">";
        if ( name.Equals( "amp" ) ) return "&";
        if ( name.Equals( "apos" ) ) return "'";
        if ( name.Equals( "quot" ) ) return "\"";

        return name.StartsWith( "#x" ) ? char.Parse( name[ 2.. ] ).ToString() : null;
    }

    [Obsolete]
    private void Text( string text )
    {
        string? existing = _current?.Text;
        _current!.Text = ( existing != null ? existing + text : text );
    }

    [Obsolete]
    private void Close()
    {
        _root    = _elements.Pop();
        _current = _elements.Count > 0 ? _elements.Peek() : null;
    }

    // --------------------------------------------------------------------
    //
    // --------------------------------------------------------------------

    [Obsolete]
    public sealed class Element
    {
        public string                         Name       { get; private set; }
        public ObjectMap< string, string? >? Attributes { get; set; }
        public Element                        Parent     { get; set; }
        public string                         Text       { get; set; }

        private List< Element >? _children;

        [Obsolete]
        public Element( string name, Element parent )
        {
            this.Name   = name;
            this.Parent = parent;
            this.Text   = string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        [Obsolete]
        public string GetAttribute( string name )
        {
            ArgumentNullException.ThrowIfNull( name );

            if ( Attributes == null )
            {
                throw new GdxRuntimeException( $"Element {this.Name} doesn't have attribute: {name}" );
            }

            var value = Attributes.Get( name );

            if ( string.ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException( $"Element {this.Name} doesn't have attribute: {name}" );
            }

            return value;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Obsolete]
        public string? GetAttribute( string name, string? defaultValue )
        {
            if ( Attributes == null ) return defaultValue;

            var value = Attributes.Get( name );

            return value ?? defaultValue;

        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete]
        public bool HasAttribute( string name )
        {
            return ( Attributes != null ) && Attributes.ContainsKey( name );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [Obsolete]
        public void SetAttribute( string name, string value )
        {
            Attributes ??= new ObjectMap< string, string? >( 8 );

            Attributes.Put( name, value );
        }

        /// <summary>
        /// </summary>
        [Obsolete]
        public int ChildCount => _children?.Count ?? 0;

        [Obsolete]
        public Element GetChild( int index )
        {
            if ( _children == null ) throw new GdxRuntimeException( "Element has no children: " + Name );

            return _children[ index ];
        }

        [Obsolete]
        public void AddChild( Element element )
        {
            _children ??= new List< Element >( 8 );

            _children.Add( element );
        }

        [Obsolete]
        public void RemoveChild( int index ) => _children?.RemoveAt( index );

        [Obsolete]
        public void RemoveChild( Element child ) => _children?.Remove( child );

        [Obsolete]
        public void Remove() => Parent.RemoveChild( this );

        /// <summary>
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        [Obsolete]
        public string ToString( string indent = "" )
        {
            var buffer = new StringBuilder( 128 );

            buffer.Append( indent );
            buffer.Append( '<' );
            buffer.Append( Name );

            if ( Attributes != null )
            {
                ObjectMap< string, string? >.Entries< string, string? > array = Attributes.GetEntries();

                List< string > keys = Attributes.GetKeys().ToArray();
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
        /// <param name="name">The name of the child <see cref="Element"/></param>
        /// <returns>The first child having the given name or null, does not recurse.</returns>
        [Obsolete]
        internal Element? GetChildByName( string name )
        {
            if ( _children == null ) return null;

            foreach ( Element element in _children )
            {
                if ( ( element.Name != null ) && element.Name.Equals( name ) ) return element;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete]
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
        /// <param name="name"> the name of the child <see cref="Element"/></param>
        /// <returns> the first child having the given name or null, recurses </returns>
        [Obsolete]
        public Element? GetChildByNameRecursive( string name )
        {
            if ( _children == null ) return null;

            foreach ( Element? element in _children )
            {
                if ( ( element.Name != null ) && element.Name.Equals( name ) ) return element;

                Element? found = element.GetChildByNameRecursive( name );

                if ( found != null ) return found;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete]
        public bool HasChildRecursive( string name )
        {
            if ( _children == null )
            {
                return false;
            }

            return GetChildByNameRecursive( name ) != null;
        }

        /// <param name="name"> the name of the children </param>
        /// <returns> the children with the given name or an empty <seealso cref="Array"/>  </returns>
        [Obsolete]
        public List< Element > GetChildrenByName( string name )
        {
            var result = new List< Element >();

            if ( _children == null )
            {
                return result;
            }

            foreach ( Element child in _children )
            {
                if ( ( child.Name != null ) && child.Name.Equals( name ) )
                {
                    result.Add( child );
                }
            }

            return result;
        }

        /// <param name="name"> the name of the children </param>
        /// <returns> the children with the given name or an empty <seealso cref="Array"/>  </returns>
        [Obsolete]
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
        [Obsolete]
        private void GetChildrenByNameRecursively( string name, List< Element > result )
        {
            if ( _children == null ) return;

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
        [Obsolete]
        public float GetFloatAttribute( string name )
        {
            return float.Parse( GetAttribute( name ) );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Obsolete]
        public float GetFloatAttribute( string name, float defaultValue )
        {
            var value = GetAttribute( name, null );

            if ( string.ReferenceEquals( value, null ) )
            {
                return defaultValue;
            }

            return float.Parse( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete]
        public int GetIntAttribute( string name )
        {
            return int.Parse( GetAttribute( name ) );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Obsolete]
        public int GetIntAttribute( string name, int defaultValue )
        {
            var value = GetAttribute( name, null );

            if ( string.ReferenceEquals( value, null ) )
            {
                return defaultValue;
            }

            return int.Parse( value );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete]
        public bool GetboolAttribute( string name )
        {
            return bool.Parse( GetAttribute( name ) );
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Obsolete]
        public bool GetboolAttribute( string name, bool defaultValue )
        {
            var value = GetAttribute( name, null );

            if ( string.ReferenceEquals( value, null ) )
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
        [Obsolete]
        public string Get( string name )
        {
            var value = Get( name, null );

            if ( string.ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException( $"Element {this.Name} doesn't have attribute or child: {name}" );
            }

            return value;
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        [Obsolete]
        public string? Get( string name, string? defaultValue )
        {
            string? str;

            if ( Attributes != null )
            {
                str = Attributes.Get( name );

                if ( !string.ReferenceEquals( str, null ) )
                {
                    return str;
                }
            }

            Element? child = GetChildByName( name );

            if ( child == null ) return defaultValue;

            str = child.Text;

            return str ?? defaultValue;
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        [Obsolete]
        public int GetInt( string name )
        {
            var value = Get( name, null );

            if ( string.ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException
                    ( $"Element {this.Name} doesn't have attribute or child: {name}" );
            }

            return int.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        [Obsolete]
        public int GetInt( string name, int defaultValue )
        {
            var value = Get( name, null );

            return string.ReferenceEquals( value, null ) ? defaultValue : int.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        [Obsolete]
        public float GetFloat( string name )
        {
            var value = Get( name, null );

            if ( string.ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException
                    ( $"Element {this.Name} doesn't have attribute or child: {name}" );
            }

            return float.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        [Obsolete]
        public float GetFloat( string name, float defaultValue )
        {
            var value = Get( name, null );

            return string.ReferenceEquals( value, null ) ? defaultValue : float.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        [Obsolete]
        public bool Getbool( string name )
        {
            var value = Get( name, null );

            if ( string.ReferenceEquals( value, null ) )
            {
                throw new GdxRuntimeException
                    ( $"Element {this.Name} doesn't have attribute or child: {name}" );
            }

            return bool.Parse( value );
        }

        /// <summary>
        /// Returns the attribute value with the specified name, or if no attribute
        /// is found, the text of a child with the name.
        /// </summary>
        /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
        [Obsolete]
        public bool Getbool( string name, bool defaultValue )
        {
            var value = Get( name, null );

            return string.ReferenceEquals( value, null ) ? defaultValue : bool.Parse( value );
        }
    }
}
