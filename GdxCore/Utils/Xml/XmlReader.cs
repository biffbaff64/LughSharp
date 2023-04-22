using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using LibGDXSharp.Scenes.Scene2D.UI;

namespace LibGDXSharp.Utils.Xml
{
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
    public class XmlReader
    {
        private readonly List< Element > _elements   = new List< Element >( 8 );
        private readonly StringBuilder   _textBuffer = new StringBuilder( 64 );

        private Element _root;
        private Element _current;

        // ----------------------------------------------------------
        // 
        // ----------------------------------------------------------
        
        public virtual Element Parse(string xml)
        {
            var data = xml.ToCharArray();
            
            return Parse(data, 0, data.Length);
        }

        public virtual Element Parse(Reader reader)
        {
            try
            {
                char[] data   = new char[1024];
                int    offset = 0;
                while (true)
                {
                    int length = reader.read(data, offset, data.Length - offset);
                    if (length == -1)
                    {
                        break;
                    }
                    if (length == 0)
                    {
                        char[] newData = new char[data.Length * 2];
                        Array.Copy(data, 0, newData, 0, data.Length);
                        data = newData;
                    }
                    else
                    {
                        offset += length;
                    }
                }
                return parse(data, 0, offset);
            }
            catch (IOException ex)
            {
                throw new SerializationException(ex);
            }
            finally
            {
                StreamUtils.closeQuietly(reader);
            }
        }

        // --------------------------------------------------------------------
        //
        // --------------------------------------------------------------------
        
        public class Element
        {
            public string?                       Name       { get; private set; }
            public Dictionary< string, string >? Attributes { get; set; }
            public Element?                      Parent     { get; set; }
            public string?                       Text       { get; set; }

            private List< Element >? _children;

            public Element( string name, Element parent )
            {
                this.Name   = name;
                this.Parent = parent;
            }

            /// <summary>
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            /// <exception cref="GdxRuntimeException"></exception>
            public string GetAttribute( string name )
            {
                if ( Attributes == null )
                {
                    throw new GdxRuntimeException( "Element " + this.Name + " doesn't have attribute: " + name );
                }

                string value = Attributes[ name ];

                if ( string.ReferenceEquals( value, null ) )
                {
                    throw new GdxRuntimeException( "Element " + this.Name + " doesn't have attribute: " + name );
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
                if ( Attributes == null ) return defaultValue;

                var value = Attributes[ name ];

                if ( string.ReferenceEquals( value, null ) ) return defaultValue;

                return value;
            }

            /// <summary>
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool HasAttribute( string name )
            {
                return Attributes != null && Attributes.ContainsKey( name );
            }

            /// <summary>
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            public void SetAttribute( string name, string value )
            {
                if ( Attributes == null )
                {
                    Attributes = new Dictionary< string, string >( 8 );
                }

                Attributes[ name ] = value;
            }

            /// <summary>
            /// </summary>
            public int ChildCount => _children?.Count ?? 0;

            public Element GetChild( int index )
            {
                if ( _children == null ) throw new GdxRuntimeException( "Element has no children: " + Name );

                return _children[ index ];
            }

            public void AddChild( Element element )
            {
                _children ??= new List< Element >( 8 );

                _children.Add( element );
            }

            public void RemoveChild( int index )
            {
                _children?.RemoveAt( index );
            }

            public void RemoveChild( Element child )
            {
                _children?.Remove( child );
            }

            public void Remove()
            {
                Parent?.RemoveChild( this );
            }

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
                    foreach ( Entry< string, string > entry in Attributes.Entries() )
                    {
                        buffer.Append( ' ' );
                        buffer.Append( entry.key );
                        buffer.Append( "=\"" );
                        buffer.Append( entry.value );
                        buffer.Append( '\"' );
                    }
                }

                if ( _children == null && string.IsNullOrEmpty( Text ) )
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
                            buffer.Append( child.toString( childIndent ) );
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
            internal Element? GetChildByName( string name )
            {
                if ( _children == null ) return null;

                foreach ( Element element in _children )
                {
                    if ( element.Name != null && element.Name.Equals( name ) ) return element;
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
            /// <param name="name"> the name of the child <see cref="Element"/></param>
            /// <returns> the first child having the given name or null, recurses </returns>
            public Element? GetChildByNameRecursive( string name )
            {
                if ( _children == null ) return null;

                foreach ( Element? element in _children )
                {
                    if ( element.Name != null && element.Name.Equals( name ) ) return element;

                    Element? found = element.GetChildByNameRecursive( name );

                    if ( found != null ) return found;
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
            /// <returns> the children with the given name or an empty <seealso cref="Array"/>  </returns>
            public List< Element > GetChildrenByName( string name )
            {
                var result = new List< Element >();

                if ( _children == null )
                {
                    return result;
                }

                foreach ( Element child in _children )
                {
                    if ( child.Name != null && child.Name.Equals( name ) )
                    {
                        result.Add( child );
                    }
                }

                return result;
            }

            /// <param name="name"> the name of the children </param>
            /// <returns> the children with the given name or an empty <seealso cref="Array"/>  </returns>
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
                if ( _children == null ) return;

                foreach ( Element child in _children )
                {
                    if ( child.Name != null && child.Name.Equals( name ) )
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
            public float GetFloatAttribute( string name )
            {
                return float.Parse( GetAttribute( name ) );
            }

            /// <summary>
            /// </summary>
            /// <param name="name"></param>
            /// <param name="defaultValue"></param>
            /// <returns></returns>
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
            public int GetIntAttribute( string name )
            {
                return int.Parse( GetAttribute( name ) );
            }

            /// <summary>
            /// </summary>
            /// <param name="name"></param>
            /// <param name="defaultValue"></param>
            /// <returns></returns>
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
            public bool GetBooleanAttribute( string name )
            {
                return bool.Parse( GetAttribute( name ) );
            }

            /// <summary>
            /// </summary>
            /// <param name="name"></param>
            /// <param name="defaultValue"></param>
            /// <returns></returns>
            public bool GetBooleanAttribute( string name, bool defaultValue )
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
            public string? Get( string name )
            {
                string? value = Get( name, null );

                if ( string.ReferenceEquals( value, null ) )
                {
                    throw new GdxRuntimeException
                        (
                         "Element "
                         + this.Name
                         + " doesn't have attribute or child: "
                         + name
                        );
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
                    str = Attributes[ name ];

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
            public int GetInt( string name )
            {
                var value = Get( name, null );

                if ( string.ReferenceEquals( value, null ) )
                {
                    throw new GdxRuntimeException
                        (
                         "Element "
                         + this.Name
                         + " doesn't have attribute or child: "
                         + name
                        );
                }

                return int.Parse( value );
            }

            /// <summary>
            /// Returns the attribute value with the specified name, or if no attribute
            /// is found, the text of a child with the name.
            /// </summary>
            /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
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
            public virtual float GetFloat( string name )
            {
                var value = Get( name, null );

                if ( string.ReferenceEquals( value, null ) )
                {
                    throw new GdxRuntimeException( "Element " + this.Name + " doesn't have attribute or child: " + name );
                }

                return float.Parse( value );
            }

            /// <summary>
            /// Returns the attribute value with the specified name, or if no attribute
            /// is found, the text of a child with the name.
            /// </summary>
            /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
            public virtual float GetFloat( string name, float defaultValue )
            {
                var value = Get( name, null );

                if ( string.ReferenceEquals( value, null ) )
                {
                    return defaultValue;
                }

                return float.Parse( value );
            }

            /// <summary>
            /// Returns the attribute value with the specified name, or if no attribute
            /// is found, the text of a child with the name.
            /// </summary>
            /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
            public virtual bool GetBoolean( string name )
            {
                var value = Get( name, null );

                if ( string.ReferenceEquals( value, null ) )
                {
                    throw new GdxRuntimeException
                        (
                         "Element "
                         + this.Name
                         + " doesn't have attribute or child: "
                         + name
                        );
                }

                return bool.Parse( value );
            }

            /// <summary>
            /// Returns the attribute value with the specified name, or if no attribute
            /// is found, the text of a child with the name.
            /// </summary>
            /// <exception cref="GdxRuntimeException">if no attribute or child was not found.</exception>
            public virtual bool GetBoolean( string name, bool defaultValue )
            {
                var value = Get( name, null );

                if ( string.ReferenceEquals( value, null ) )
                {
                    return defaultValue;
                }

                return bool.Parse( value );
            }
        }
    }
}
