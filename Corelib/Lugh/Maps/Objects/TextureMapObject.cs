// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.Lugh.Graphics.G2D;

namespace Corelib.Lugh.Maps.Objects;

/// <summary>
/// Represents a map object containing a texture (region).
/// </summary>
[PublicAPI]
public class TextureMapObject : MapObject
{
    public float          X             { get; set; } = 0.0f;
    public float          Y             { get; set; } = 0.0f;
    public float          OriginX       { get; set; } = 0.0f;
    public float          OriginY       { get; set; } = 0.0f;
    public float          ScaleX        { get; set; } = 1.0f;
    public float          ScaleY        { get; set; } = 1.0f;
    public float          Rotation      { get; set; } = 0.0f;
    public TextureRegion? TextureRegion { get; set; } = null;

    // ========================================================================

    /// <summary>
    /// Creates an empty texture map object
    /// </summary>
    public TextureMapObject() : this( null )
    {
    }

    /// <summary>
    /// Creates a new texture map object with the given region
    /// </summary>
    /// <param name="textureRegion">the <see cref="TextureRegion"/> to use.</param>
    public TextureMapObject( TextureRegion? textureRegion )
    {
        TextureRegion = textureRegion;
    }
}
