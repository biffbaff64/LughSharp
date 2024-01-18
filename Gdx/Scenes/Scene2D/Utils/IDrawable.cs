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

using LibGDXSharp.Graphics.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
///     A drawable knows how to draw itself at a given rectangular size. It provides
///     padding sizes and a minimum size so that other code can determine how to size
///     and position content.
/// </summary>
public interface IDrawable
{
    float LeftWidth { get; set; }

    float RightWidth { get; set; }

    float TopHeight { get; set; }

    float BottomHeight { get; set; }

    float MinWidth { get; set; }

    float MinHeight { get; set; }

    /// <summary>
    ///     Draws this drawable at the specified bounds. The drawable should be tinted
    ///     with <see cref="IBatch.Color" />, possibly by mixing its own color.
    /// </summary>
    void Draw( IBatch batch, float x, float y, float width, float height );
}
