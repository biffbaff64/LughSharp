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

using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// A drawable that supports scale and rotation.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public interface ITransformDrawable : IDrawable
{
    void Draw( IBatch batch,
               float x,
               float y,
               float originX,
               float originY,
               float width,
               float height,
               float scaleX,
               float scaleY,
               float rotation );
}