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

using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// An InstanceData instance holds instance data for rendering with OpenGL.
/// It is implemented as either a <see cref="InstanceBufferObject"/> or a
/// <see cref="InstanceBufferObjectSubData"/>. Both require Open GL 3.3+.
/// </summary>
[PublicAPI]
public interface IInstanceData : IDisposable
{

    /// <returns> the number of vertices this InstanceData stores </returns>
    int NumInstances { get; }

    /// <returns> the number of vertices this InstanceData can store </returns>
    int NumMaxInstances { get; }

    /// <returns> the <seealso cref="VertexAttributes"/> as specified during construction. </returns>
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
    ///<param name="targetOffset"></param>
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

//    /// <summary>
//    /// Disposes this InstanceData and all its associated OpenGL resources.
//    /// </summary>
//    void Dispose();
}