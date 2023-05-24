namespace LibGDXSharp.Graphics.GLUtils;

public interface IVertexData : IDisposable
{
    /// <returns> the number of vertices this VertexData stores </returns>
    public int GetNumVertices();

    /// <returns> the number of vertices this VertedData can store </returns>
    public int GetNumMaxVertices();

    /// <returns> the <see cref="VertexAttributes"/> as specified during construction. </returns>
    public VertexAttributes GetAttributes();

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
    public void SetVertices( float[] vertices, int offset, int count );

    /// <summary>
    /// Update (a portion of) the vertices. Does not resize the backing buffer. </summary>
    /// <param name="targetOffset"></param>
    /// <param name="vertices"> the vertex data </param>
    /// <param name="sourceOffset"> the offset to start copying the data from </param>
    /// <param name="count"> the number of floats to copy  </param>
    public void UpdateVertices( int targetOffset, float[] vertices, int sourceOffset, int count );

    /// <summary>
    /// Returns the underlying FloatBuffer and marks it as dirty, causing the buffer
    /// contents to be uploaded on the next call to bind. If you need immediate
    /// uploading use <see cref="SetVertices"/>; Any modifications made to the Buffer
    /// *after* the call to bind will not automatically be uploaded.
    /// </summary>
    /// <returns> the underlying FloatBuffer holding the vertex data.  </returns>
    public FloatBuffer GetBuffer();

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    public void Bind( ShaderProgram shader );

    /// <summary>
    /// Binds this VertexData for rendering via glDrawArrays or glDrawElements.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Bind( ShaderProgram shader, int[] locations );

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    public void Unbind( ShaderProgram shader );

    /// <summary>
    /// Unbinds this VertexData.
    /// </summary>
    /// <param name="shader"></param>
    /// <param name="locations"> array containing the attribute locations.  </param>
    public void Unbind( ShaderProgram shader, int[] locations );

    /// <summary>
    /// Invalidates the VertexData if applicable. Use this in case of a context loss. </summary>
    public void Invalidate();
}