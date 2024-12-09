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

using Corelib.Lugh.Utils.Buffers;

namespace Corelib.Lugh.Graphics.GLUtils;

/// <summary>
/// An IndexData instance holds index data.
/// Can be either a plain short buffer or an OpenGL buffer object.
/// </summary>
[PublicAPI]
public interface IIndexData : IDisposable
{
    /// <returns> the number of indices currently stored in this buffer </returns>
    int NumIndices { get; }

    /// <returns> the maximum number of indices this IndexBufferObject can store. </returns>
    int NumMaxIndices { get; }

    /// <summary>
    ///     <para>
    ///     Sets the indices of this IndexBufferObject, discarding the old indices.
    ///     The count must equal the number of indices to be copied to this IndexBufferObject.
    ///     </para>
    ///     <para>
    ///     This can be called in between calls to <see cref="Bind()"/> and
    ///     <see cref="Unbind()"/>. The index data will be updated instantly.
    ///     </para>
    /// </summary>
    /// <param name="indices"> the index data </param>
    /// <param name="offset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of shorts to copy  </param>
    void SetIndices( short[] indices, int offset, int count );

    /// <summary>
    /// Copies the specified indices to the indices of this IndexBufferObject,
    /// discarding the old indices. Copying start at the current
    /// <see cref="ShortBuffer.Position()"/> of the specified buffer and copied
    /// the <see cref="ShortBuffer.Remaining()"/> amount of indices. This can be
    /// called in between calls to <see cref="Bind()"/> and <see cref="Unbind()"/>.
    /// The index data will be updated instantly.
    /// </summary>
    /// <param name="indices"> the index data to copy  </param>
    void SetIndices( ShortBuffer indices );

    /// <summary>
    /// Update (a portion of) the indices.
    /// </summary>
    /// <param name="targetOffset"> offset in indices buffer </param>
    /// <param name="indices"> the index data </param>
    /// <param name="offset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of shorts to copy  </param>
    void UpdateIndices( int targetOffset, short[] indices, int offset, int count );

    /// <summary>
    /// Returns the underlying ShortBuffer. If you modify the buffer contents they
    /// wil be uploaded on the call to <see cref="Bind()"/>. If you need immediate
    /// uploading use <see cref="SetIndices(short[], int, int)"/>.
    /// </summary>
    /// <returns> the underlying short buffer. </returns>
    ShortBuffer GetBuffer( bool forWriting );

    /// <summary>
    /// Binds this IndexBufferObject for rendering with glDrawElements.
    /// </summary>
    void Bind();

    /// <summary>
    /// Unbinds this IndexBufferObject.
    /// </summary>
    void Unbind();

    /// <summary>
    /// Invalidates the IndexBufferObject so a new OpenGL buffer handle is created.
    /// Use this in case of a context loss.
    /// </summary>
    void Invalidate();
}
