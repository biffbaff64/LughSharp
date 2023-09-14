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

namespace LibGDXSharp.Graphics.GLUtils;

[PublicAPI]
public interface IImmediateModeRenderer : IDisposable
{
    public void Begin( Matrix4 projModelView, int primitiveType );

    public void End();

    public void Flush();

    public void SetColor( Color color );

    public void SetColor( float r, float g, float b, float a );

    public void SetColor( float colorBits );

    public void TexCoord( float u, float v );

    public void Normal( float x, float y, float z );

    public void Vertex( float x, float y, float z );

    public int NumVertices { get; set; }

    public int MaxVertices { get; set; }
}