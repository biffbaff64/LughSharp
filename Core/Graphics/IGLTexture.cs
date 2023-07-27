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

namespace LibGDXSharp.Graphics;

public interface IGLTexture
{
    /// <returns> the width of the texture in pixels </returns>
    int Width { get; }

    /// <returns> the height of the texture in pixels </returns>
    int Height { get; }

    /// <returns> the depth of the texture in pixels </returns>

    int Depth { get; }
    /// <summary>
    /// Used internally to reload after context loss. Creates a new GL handle then
    /// calls <see cref="Texture.Load"/>.
    /// </summary>
    void Reload();
    
    /// <returns>whether this texture is managed or not.</returns>
    bool IsManaged();

    string? ToString();

//    int GLHandle { get; set; }
//    int GLTarget { get; set; }
//    
//    float AnisotropicFilterLevel { get; }
//
//    /// <returns> The <see cref="TextureFilter"/> used for minification. </returns>
//    TextureFilter MinFilter { get; }
//
//    /// <returns> The <see cref="TextureFilter"/> used for magnification. </returns>
//    TextureFilter MagFilter { get; }
//
//    /// <returns>
//    /// The <see cref="TextureWrap"/> used for horizontal (U) texture coordinates.
//    /// </returns>
//    TextureWrap UWrap { get; set; }
//    
//    /// <returns>
//    /// The <see cref="TextureWrap"/> used for vertical (V) texture coordinates.
//    /// </returns>
//    TextureWrap VWrap { get; set; }
//
//    /// <summary>
//    /// Binds this texture. The texture will be bound to the currently active
//    /// texture unit specified via <see cref="IGL20.GLActiveTexture(int)"/>. 
//    /// </summary>
//    void Bind();
//
//    /// <summary>
//    /// Binds the texture to the given texture unit.
//    /// <para>
//    /// Sets the currently active texture unit via <see cref="IGL20.GLActiveTexture(int)"/>.
//    /// </para>
//    /// </summary>
//    /// <param name="unit"> the unit (0 to MAX_TEXTURE_UNITS).  </param>
//    void Bind( int unit );
//
//    /// <summary>
//    /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis.
//    /// This will bind this texture!
//    /// </summary>
//    /// <param name="u">the u wrap</param>
//    /// <param name="v">the v wrap</param>
//    void SetWrap( TextureWrap u, TextureWrap v );
//
//    /// <summary>
//    /// Sets the <see cref="TextureFilter"/> for this texture for minification and
//    /// magnification. This will bind this texture!
//    /// </summary>
//    /// <param name="minFilter"> the minification filter </param>
//    /// <param name="magFilter"> the magnification filter  </param>
//    void SetFilter( TextureFilter minFilter, TextureFilter magFilter );
//
//    /// <summary>
//    ///
//    /// </summary>
//    /// <param name="level"></param>
//    /// <returns></returns>
//    float SetAnisotropicFilter( float level );
//
//    /// <summary>
//    /// Convenience method for when 'GLHandle' isn't descriptive enough.
//    /// </summary>
//    // TODO: Check usages of GLHandle to see if if can be renamed to TextureObjectHandle without causing any issues.
//    int GetTextureObjectHandle();
}