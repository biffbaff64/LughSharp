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

[PublicAPI]
public static class BufferedStreamExtensions
{
    /// <summary>
    /// Marks the current position in this input stream. A subsequent call to
    /// the <tt>reset</tt> method repositions this stream at the last marked
    /// position so that subsequent reads re-read the same bytes.
    /// <para>
    /// The <tt>readlimit</tt> arguments tells this input stream to allow that
    /// many bytes to be read before the mark position gets invalidated.
    /// </para>
    /// <para>
    /// The general contract of <tt>mark</tt> is that, if the method <tt>markSupported</tt>
    /// returns <tt>true</tt>, the stream somehow remembers all the bytes read after the
    /// call to <tt>mark</tt> and stands ready to supply those same bytes again if and
    /// whenever the method <tt>reset</tt> is called.  However, the stream is not required
    /// to remember any data at all if more than <tt>readlimit</tt> bytes are read from the
    /// stream before <tt>reset</tt> is called.
    /// </para>
    /// <para>
    /// Marking a closed stream should not have any effect on the stream.
    /// </para>
    /// <para>
    /// The <tt>mark</tt> method of <tt>InputStream</tt> does nothing.
    /// </para>
    /// </summary>
    /// <param name="bs"></param>
    /// <param name="readLimit">
    /// the maximum limit of bytes that can be read before the mark position becomes invalid.
    /// </param>
    public static void Mark( this BufferedStream bs, int readLimit )
    {
    }

    public static void Reset( this BufferedStream bs )
    {
    }
}

