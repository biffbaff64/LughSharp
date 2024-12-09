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
/// An InstanceData instance holds instance data for rendering with OpenGL.
/// It is implemented as either a <see cref="InstanceBufferObject"/> or a
/// <see cref="InstanceBufferObjectSubData"/>. Both require Open GL 3.3+.
/// </summary>
[PublicAPI]
public interface IInstanceData : IDisposable
{
    /// <summary>
    /// Returns the number of vertices this InstanceData stores
    /// </summary>
    int NumInstances { get; }

    /// <summary>
    /// Returns the number of vertices this InstanceData can store
    /// </summary>
    int NumMaxInstances { get; }

    /// <summary>
    /// Returns the <see cref="VertexAttributes"/> as specified during construction.
    /// </summary>
    VertexAttributes Attributes { get; }

    /// <summary>
    /// Sets the vertices of this InstanceData, discarding the old vertex data.
    /// The count must equal the number of floats per vertex times the number
    /// of vertices to be copied to this VertexData. The order of the vertex
    /// attributes must be the same as specified at construction time via
    /// <see cref="VertexAttributes"/>.
    /// <para>
    /// This can be called in between calls to bind and unbind. The vertex data
    /// will be updated instantly.
    /// </para>
    /// </summary>
    /// <param name="data">   the instance data </param>
    /// <param name="offset"> the offset to start copying the data from </param>
    /// <param name="count">  the number of floats to copy </param>
    void SetInstanceData( float[] data, int offset, int count );

    /// <summary>
    /// Update (a portion of) the vertices. Does not resize the backing buffer.
    /// </summary>
    /// <param name="targetOffset"></param>
    /// <param name="data"> the instance data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy </param>
    void UpdateInstanceData( int targetOffset, float[] data, int sourceOffset, int count );

    /// <summary>
    /// Sets the vertices of this InstanceData, discarding the old vertex data.
    /// The count must equal the number of floats per vertex times the number of
    /// vertices to be copied to this InstanceData. The order of the vertex
    /// attributes must be the same as specified at construction time via
    /// <see cref="VertexAttributes"/>.
    /// <para>
    /// This can be called in between calls to bind and unbind. The vertex data
    /// will be updated instantly.
    /// </para>
    /// </summary>
    /// <param name="data">  the instance data </param>
    /// <param name="count"> the number of floats to copy </param>
    void SetInstanceData( FloatBuffer data, int count );

    /// <summary>
    /// Update (a portion of) the vertices. Does not resize the backing buffer.
    /// </summary>
    /// <param name="targetOffset"></param>
    /// <param name="data"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count">  the number of floats to copy </param>
    void UpdateInstanceData( int targetOffset, FloatBuffer data, int sourceOffset, int count );

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="SetInstanceData(float[], int, int)"/>;
    /// Any modifications made to the Buffer *after* the call to bind will not
    /// automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data. </returns>
    FloatBuffer GetBuffer( bool forWriting );

    /// <summary>
    /// Binds this InstanceData for rendering via glDrawArraysInstanced or glDrawElementsInstanced.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations. </param>
    void Bind( ShaderProgram shader, int[]? locations );

    /// <summary>
    /// Unbinds this InstanceData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations. </param>
    void Unbind( ShaderProgram shader, int[] locations );

    /// <summary>
    /// Invalidates the InstanceData if applicable. Use this in case of a context loss.
    /// </summary>
    void Invalidate();
}
