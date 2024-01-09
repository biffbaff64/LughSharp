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

namespace LibGDXSharp.Utils.Viewport;

/// <summary>
///     A viewport where the world size is based on the size of the screen.
///     By default 1 world unit == 1 screen pixel, but this ratio can be
///     changed by modifying <see cref="UnitsPerPixel" />.
/// </summary>
public class ScreenViewport : Viewport
{
    /// <summary>
    ///     Creates a new viewport using a new <see cref="OrthographicCamera" />.
    /// </summary>
    public ScreenViewport() : base( new OrthographicCamera() )
    {
    }

    public float UnitsPerPixel { get; set; } = 1;

    public override void Update( int screenWidth, int screenHeight, bool centerCamera = false )
    {
        SetScreenBounds( 0, 0, screenWidth, screenHeight );
        SetWorldSize( screenWidth * UnitsPerPixel, screenHeight * UnitsPerPixel );
        Apply( centerCamera );
    }
}
