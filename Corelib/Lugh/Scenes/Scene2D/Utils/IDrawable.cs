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

using Corelib.Lugh.Graphics.G2D;

namespace Corelib.Lugh.Scenes.Scene2D.Utils;

/// <summary>
/// A drawable knows how to draw itself at a given rectangular size. It provides
/// padding sizes and a minimum size so that other code can determine how to size
/// and position content.
/// </summary>
[PublicAPI]
public interface IDrawable
{
    float LeftWidth    { get; set; }
    float RightWidth   { get; set; }
    float TopHeight    { get; set; }
    float BottomHeight { get; set; }
    float MinWidth     { get; set; }
    float MinHeight    { get; set; }

    /// <summary>
    /// Draws this drawable at the specified bounds. The drawable should be tinted
    /// with <see cref="IBatch.Color"/>, possibly by mixing its own color.
    /// </summary>
    void Draw( IBatch batch, float x, float y, float width, float height );
}
