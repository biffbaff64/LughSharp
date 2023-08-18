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

using LibGDXSharp.Core.Utils.Collections;

namespace LibGDXSharp.Utils.Xml;

/// <summary>
/// Builder style API for emitting XML:
/// <para>
/// <code>
/// StringWriter _writer = new StringWriter();
/// XmlWriter xml = new XmlWriter(_writer);
/// xml.element("meow")
/// .attribute("moo", "cow")
/// .element("child")
///		.attribute("moo", "cow")
///		.element("child")
///			.attribute("moo", "cow")
///			.text("All that we see or seem is but a dream within a dream.")
///		.pop()
/// .pop()
/// .pop();
/// Console.WriteLn(_writer);
/// </code>
/// </para>
/// </summary>
public class XmlWriter
{
    private readonly StreamWriter?   _writer;
    private readonly List< string >? _stack = new();
    private          string?         _currentElement;
    private          bool            _indentNextClose;

    public int Indentation { get; set; }

    public XmlWriter( StreamWriter writer )
    {
        this._writer = writer;
    }

    private void Indent()
    {
        var count = Indentation;

        if ( _currentElement != null ) count++;

        for ( var i = 0; i < count; i++ )
        {
            _writer?.Write( '\t' );
        }
    }

    public XmlWriter Element( string name )
    {
        if ( StartElementContent() ) _writer?.Write( '\n' );

        Indent();
        
        _writer?.Write( '<' );
        _writer?.Write( name );
        
        _currentElement = name;

        return this;
    }

    public XmlWriter Element( string name, object text )
    {
        return Element( name ).Text( text ).Pop();
    }

    private bool StartElementContent()
    {
        if ( _currentElement == null ) return false;
        
        Indentation++;
        
        _stack?.Add( _currentElement );
        _currentElement = null;
        
        _writer?.Write( ">" );

        return true;
    }

    public XmlWriter Attribute( string name, object? value )
    {
        if ( _currentElement == null ) throw new IllegalStateException( "_currentElement cannot be NULL!" );
        
        _writer?.Write( ' ' );
        _writer?.Write( name );
        _writer?.Write( "=\"" );
        _writer?.Write( value == null ? "null" : value.ToString()! );
        _writer?.Write( '"' );

        return this;
    }

    public XmlWriter Text( object? text )
    {
        StartElementContent();

        var str = text == null ? "null" : text.ToString();
        
        _indentNextClose = str?.Length > 64;

        if ( _indentNextClose )
        {
            _writer?.Write( '\n' );
            Indent();
        }

        _writer?.Write( str! );
        
        if ( _indentNextClose ) _writer?.Write( '\n' );

        return this;
    }

    public XmlWriter Pop()
    {
        if ( _currentElement != null )
        {
            _writer?.Write( "/>\n" );
            _currentElement = null;
        }
        else
        {
            Indentation = Math.Max( Indentation - 1, 0 );
            
            if ( _indentNextClose ) Indent();

            _writer?.Write( "</" );
            _writer?.Write( _stack?.Pop()! );
            _writer?.Write( ">\n" );
        }

        _indentNextClose = true;

        return this;
    }

    /// <summary>
    /// Calls <see cref="Pop()"/> for each remaining open element,
    /// if any, and closes the stream.
    /// </summary>
    public void Close()
    {
        while ( _stack?.Count != 0 )
        {
            Pop();
        }

        _writer?.Close();
    }

    public void Write( char[] cbuf, int off, int len )
    {
        StartElementContent();

        _writer?.Write( cbuf, off, len );
    }

    public void Flush()
    {
        _writer?.Flush();
    }
}