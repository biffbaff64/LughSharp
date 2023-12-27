// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Runtime.Serialization;

namespace LibGDXSharp.Utils.Xml;

[PublicAPI]
public class XmlParser
{
    private const int XML_START           = 1;
    private const int XML_FIRST_FINAL     = 34;
    private const int XML_ERROR           = 0;
    private const int XML_EN_ELEMENT_BODY = 15;
    private const int XML_EN_MAIN         = 1;

    private readonly XmlReader _xml;
    
    public XmlParser( XmlReader reader )
    {
        this._xml = reader;
    }
    
    public virtual XmlReader.Element? Parse( string xml )
    {
        var data = xml.ToCharArray();

        return Parse( data, 0, data.Length );
    }

    public virtual XmlReader.Element? Parse( StreamReader? reader )
    {
        ArgumentNullException.ThrowIfNull( reader );
        
        try
        {
            var data   = new char[ 1024 ];
            var offset = 0;

            while ( true )
            {
                var length = reader.Read( data );

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

    public virtual XmlReader.Element? Parse( FileInfo? file )
    {
        try
        {
            return Parse( file?.OpenText() );
        }
        catch ( System.Exception ex )
        {
            throw new SerializationException( "Error parsing file: " + file, ex );
        }
    }

    private XmlReader.Element? Parse( char[] data, int offset, int length )
    {
        //TODO: establish from Java LibGDX the meanings behind these variable names, then rename.
        var cs       = XML_START;
        var p        = offset;          // position ??
        var pe       = length;          // position end ??
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
                        int keys = XmlReader.xmlKeyOffsets[ cs ];
                        trans = XmlReader.xmlIndexOffsets[ cs ];
                        int klen = XmlReader.xmlSingleLengths[ cs ];

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

                                if ( data[ p ] < XmlReader.xmlTransKeys[ mid ] )
                                {
                                    upper = mid - 1;
                                }
                                else if ( data[ p ] > XmlReader.xmlTransKeys[ mid ] )
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

                        klen = XmlReader.xmlRangeLengths[ cs ];

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

                                if ( data[ p ] < XmlReader.xmlTransKeys[ mid ] )
                                {
                                    upper = mid - 2;
                                }
                                else if ( data[ p ] > XmlReader.xmlTransKeys[ mid + 1 ] )
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

                    trans = XmlReader.xmlIndicies[ trans ];
                    cs    = XmlReader.xmlTransTargs[ trans ];

                    if ( XmlReader.xmlTransActions[ trans ] != 0 )
                    {
                        int acts  = XmlReader.xmlTransActions[ trans ];
                        int nacts = XmlReader.xmlActions[ acts++ ];

                        while ( nacts-- > 0 )
                        {
                            switch ( XmlReader.xmlActions[ acts++ ] )
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

                                            _xml.Text( new string( data, s, p - s - 2 ) );
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
                                    _xml.Open( new string( data, s, p - s ) );

                                    break;
                                }

                                case 2:
                                {
                                    hasBody = false;
                                    _xml.Close();

                                    cs       = 15;
                                    gotoTarg = 2;

                                    if ( true )
                                    {
                                        goto _goto;
                                    }
                                }

                                case 3:
                                {
                                    _xml.Close();

                                    cs       = 15;
                                    gotoTarg = 2;

                                    if ( true )
                                    {
                                        goto _goto;
                                    }
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
                                    _xml.Attribute( attributeName!, new string( data, s, p - s ) );

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
                ( $"Error parsing XML on line {lineNumber} near: {new string( data, p, Math.Min( 32, pe - p ) )}" );
        }

        if ( _elements.Count != 0 )
        {
            XmlReader.Element element = _elements.Peek();
            _elements.Clear();

            throw new SerializationException( "Error parsing XML, unclosed element: " + element.Name );
        }

        XmlReader.Element? root = _root;
        _root = null;

        return root;
    }
}

