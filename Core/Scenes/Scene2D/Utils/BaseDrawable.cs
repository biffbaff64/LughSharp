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

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class BaseDrawable : IDrawable
{
    public string? Name         { get; set; }
    public float   LeftWidth    { get; set; }
    public float   RightWidth   { get; set; }
    public float   TopHeight    { get; set; }
    public float   BottomHeight { get; set; }
    public float   MinWidth     { get; set; }
    public float   MinHeight    { get; set; }

    protected BaseDrawable()
    {
    }

    /// <summary>
    /// Creates a new empty drawable with the same sizing information as the specified drawable.
    /// </summary>
    public BaseDrawable( IDrawable? drawable )
    {
        ArgumentNullException.ThrowIfNull( drawable );

        if ( drawable is BaseDrawable baseDrawable ) Name = baseDrawable.Name;

        LeftWidth    = drawable.LeftWidth;
        RightWidth   = drawable.RightWidth;
        TopHeight    = drawable.TopHeight;
        BottomHeight = drawable.BottomHeight;
        MinWidth     = drawable.MinWidth;
        MinHeight    = drawable.MinHeight;
    }

    /// <summary>
    /// Draws this drawable at the specified bounds. The drawable should be tinted
    /// with <see cref="IBatch.GetColor"/>, possibly by mixing its own color. 
    /// </summary>
    public void Draw( IBatch batch, float x, float y, float width, float height )
    {
    }

    public void SetPadding( float topHeight, float leftWidth, float bottomHeight, float rightWidth )
    {
        TopHeight    = topHeight;
        LeftWidth    = leftWidth;
        BottomHeight = bottomHeight;
        RightWidth   = rightWidth;
    }
}
