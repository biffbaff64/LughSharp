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

namespace LibGDXSharp.Graphics.G3D.Models.Data;

/// <summary>
/// Returned by a <seealso cref="ModelLoader"/>, contains meshes, materials, nodes and animations. OpenGL resources like textures or vertex
/// buffer objects are not stored. Instead, a ModelData instance needs to be converted to a Model first.
/// @author badlogic 
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class ModelData
{
    public string?                ID         { get; set; }
    public short[]                Version    { get; set; } = new short[ 2 ];
    public List< ModelMesh >      Meshes     { get; set; } = new List< ModelMesh >();
    public List< ModelMaterial >? Materials  { get; set; } = new List< ModelMaterial >();
    public List< ModelNode >      Nodes      { get; set; } = new List< ModelNode >();
    public List< ModelAnimation > Animations { get; set; } = new List< ModelAnimation >();

    public virtual void AddMesh( ModelMesh mesh )
    {
        foreach ( ModelMesh? other in Meshes )
        {
            if ( other.ID.Equals( mesh.ID ) )
            {
                throw new GdxRuntimeException( "Mesh with id '" + other.ID + "' already in model" );
            }
        }

        Meshes.Add( mesh );
    }
}
