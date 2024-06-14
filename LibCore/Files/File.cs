// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace LughSharp.LibCore.Files;

[PublicAPI]
public class File
{
    public string Path         { get; private set; }
    public int    PrefixLength { get; private set; }

    // ------------------------------------------------------------------------

    private char _separatorChar = System.IO.Path.PathSeparator;

    // ------------------------------------------------------------------------

    public File()
    {
        this.Path = string.Empty;
    }

    public File( string pathName )
    {
        this.Path = pathName;
    }

    /// <summary>
    /// Returns the name of the file or directory denoted by the <b>Path</b>.
    /// This is just the last name in the Path's name sequence. If the Path's
    /// name sequence is empty, then the empty string is returned.
    /// </summary>
    public string Name
    {
        get
        {
            var index = Path.LastIndexOf( _separatorChar );

            if ( index < PrefixLength )
            {
                return Path.Substring( PrefixLength );
            }

            return Path.Substring( index + 1 );
        }
    }
}
