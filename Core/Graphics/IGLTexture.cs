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
    int GLHandle { get; set; }
    int GLTarget { get; set; }
    float AnisotropicFilterLevel { get; }
    int Width { get; }
    int Height { get; }
    int Depth { get; }

    /// <returns> The <see cref="TextureFilter"/> used for minification. </returns>
    TextureFilter MinFilter { get; }

    /// <returns> The <see cref="TextureFilter"/> used for magnification. </returns>
    TextureFilter MagFilter { get; }

    /// <returns>
    /// The <see cref="TextureWrap"/> used for horizontal (U) texture coordinates.
    /// </returns>
    TextureWrap UWrap { get; set; }

    /// <returns>
    /// The <see cref="TextureWrap"/> used for vertical (V) texture coordinates.
    /// </returns>
    TextureWrap VWrap { get; set; }

    /// <returns>whether this texture is managed or not.</returns>
    bool IsManaged();

    /// <summary>
    /// Used internally to reload after context loss. Creates a new GL handle then
    /// calls <see cref="Texture.Load"/>.
    /// </summary>
    void Reload();

    /// <summary>
    /// Binds this texture. The texture will be bound to the currently active
    /// texture unit specified via <see cref="IGL20.GLActiveTexture(int)"/>. 
    /// </summary>
    void Bind();

    /// <summary>
    /// Binds the texture to the given texture unit.
    /// <para>
    /// Sets the currently active texture unit via <see cref="IGL20.GLActiveTexture(int)"/>.
    /// </para>
    /// </summary>
    /// <param name="unit"> the unit (0 to MAX_TEXTURE_UNITS).  </param>
    void Bind( int unit );

    /// <summary>
    /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis.
    /// Assumes the texture is bound and active!
    /// </summary>
    /// <param name="u"> the u wrap </param>
    /// <param name="v"> the v wrap </param>
    /// <param name="force">
    /// True to always set the values, even if they are the same as the current values.
    /// </param>
    void UnsafeSetWrap( TextureWrap? u, TextureWrap? v, bool force = false );

    /// <summary>
    /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis.
    /// This will bind this texture!
    /// </summary>
    /// <param name="u">the u wrap</param>
    /// <param name="v">the v wrap</param>
    void SetWrap( TextureWrap u, TextureWrap v );

    /// <summary>
    /// Sets the <see cref="TextureFilter"/> for this texture for minification and
    /// magnification. Assumes the texture is bound and active!
    /// </summary>
    /// <param name="minFilter"> the minification filter </param>
    /// <param name="magFilter"> the magnification filter  </param>
    /// <param name="force">
    /// True to always set the values, even if they are the same as the current values.
    /// Default is false.
    /// </param>
    void UnsafeSetFilter( TextureFilter? minFilter, TextureFilter? magFilter, bool force = false );

    /// <summary>
    /// Sets the <see cref="TextureFilter"/> for this texture for minification and
    /// magnification. This will bind this texture!
    /// </summary>
    /// <param name="minFilter"> the minification filter </param>
    /// <param name="magFilter"> the magnification filter  </param>
    void SetFilter( TextureFilter minFilter, TextureFilter magFilter );

    /// <summary>
    /// Sets the anisotropic filter level for the texture.
    /// Assumes the texture is bound and active!
    /// </summary>
    /// <param name="level">
    /// The desired level of filtering. The maximum level supported by
    /// the device up to this value will be used.
    /// </param>
    /// <param name="force"></param>
    /// <returns>
    /// The actual level set, which may be lower than the provided value
    /// due to device limitations.
    /// </returns>
    float UnsafeSetAnisotropicFilter( float level, bool force = false );

    /// <summary>
    ///
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    float SetAnisotropicFilter( float level );

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    float GetMaxAnisotropicFilterLevel();

    /// <summary>
    ///
    /// </summary>
    void Delete();

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="data"></param>
    /// <param name="miplevel"></param>
    void UploadImageData( int target, ITextureData? data, int miplevel = 0 );

    /// <summary>
    /// Convenience method for when 'GLHandle' isn't descriptive enough.
    /// </summary>
    int GetTextureObjectHandle();
}
