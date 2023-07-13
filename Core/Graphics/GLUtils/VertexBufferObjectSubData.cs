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

public class VertexBufferObjectSubData : IVertexData
{
    public VertexBufferObjectSubData( bool isStatic, int maxVertices, VertexAttributes attributes )
    {
        throw new NotImplementedException();
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
    }

    /// <returns> the number of vertices this VertexData stores </returns>
    public int NumVertices { get; set; }

    /// <returns> the number of vertices this VertedData can store </returns>
    public int NumMaxVertices { get; set; }

    /// <returns> the <see cref="VertexAttributes"/> as specified during construction. </returns>
    public VertexAttributes GetAttributes() => null;

    /// <summary>
    /// Sets the vertices of this VertexData, discarding the old vertex data. The
    /// count must equal the number of floats per vertex times the number of vertices
    /// to be copied to this VertexData. The order of the vertex attributes must be
    /// the same as specified at construction time via <see cref="VertexAttributes"/>.
    /// <para>
    /// This can be called in between calls to bind and unbind. The vertex data will
    /// be updated instantly.
    /// </para>
    /// </summary>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="offset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void SetVertices( float[] vertices, int offset, int count )
    {
    }

    /// <summary>
    /// Update (a portion of) the vertices. Does not resize the backing buffer. </summary>
    /// <param name="targetOffset"></param>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count )
    {
    }

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="IVertexData.SetVertices"/>; Any modifications made to the Buffer
    /// *after* the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.  </returns>
    public FloatBuffer GetBuffer() => null;

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    public void Bind( ShaderProgram shader )
    {
    }

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Bind( ShaderProgram shader, int[]? locations )
    {
    }

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    public void Unbind( ShaderProgram shader )
    {
    }

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Unbind( ShaderProgram? shader, int[]? locations )
    {
    }

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss. </summary>
    public void Invalidate()
    {
    }
}