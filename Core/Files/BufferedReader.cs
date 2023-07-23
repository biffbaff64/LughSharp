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

namespace LibGDXSharp.Files;

[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class BufferedReader : Reader
{
    private const int DefaultCharBufferSize     = 8192;
    private const int DefaultExpectedLineLength = 80;
    private const int Invalidated               = -2;
    private const int Unmarked                  = -1;

    private Reader? _reader;
    private char[]? _cb;
    private int     _nChars;
    private int     _nextChar;
    private int     _markedChar = Unmarked;

    /* Valid only when markedChar > 0 */
    private int _readAheadLimit = 0;

    /** If the next character is a line feed, skip it */
    private bool _skipLF = false;

    /** The skipLF flag when the mark was set */
    private bool _markedSkipLF = false;

    public BufferedReader( Reader input, int sz = DefaultCharBufferSize ) : base( input )
    {
    }

    /// <summary>
    /// Reads characters into a portion of an array.  This method will block
    /// until some input is available, an I/O error occurs, or the end of the
    /// stream is reached.
    /// </summary>
    /// <param name="cbuf"> Destination buffer </param>
    /// <param name="off"> Offset at which to start storing characters </param>
    /// <param name="len"> Maximum number of characters to read </param>
    ///
    /// <returns>
    /// The number of characters read, or -1 if the end of the stream has been reached
    /// </returns>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    protected override int Read( char[] cbuf, int off, int len ) => 0;
}