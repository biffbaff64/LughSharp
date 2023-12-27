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

using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Graphics.G2D;

namespace LibGDXSharp.Graphics;

/// <summary>
/// A general purpose class containing named colors that can be changed at will.
/// For example, the markup language defined by the <see cref="BitmapFontCache"/> class
/// uses this class to retrieve colors and the user can define his own colors.
/// </summary>
[PublicAPI]
public class Colors
{
    public static Dictionary< string, Color? > Map { get; set; } = new();

    static Colors()
    {
        Reset();
    }

    private Colors()
    {
    }

    /// <summary>
    /// Convenience method to lookup a color by <code>name</code>.
    /// The invocation of this method is equivalent to the expression
    /// <code>Colors.GetColors().Get(name)</code>
    /// </summary>
    /// <param name="name">The name of the color</param>
    /// <returns>
    /// The Color to which the specified <code>name</code> is mapped,
    /// or <code>null</code> if no mapping was found.
    /// </returns>
    public static Color? Get( string name )
    {
        return Map[ name ];
    }

    /// <summary>
    /// Convenience method to add a color with its <code>name</code>.
    /// The invocation of this method is equivalent to the expression
    /// <tt>Colors.GetColors().Put(name, color)</tt>
    /// </summary>
    /// <param name="name">The name of the color.</param>
    /// <param name="color">The color.</param>
    /// <returns>
    /// The previous Color associated with <code>name</code> or <code>null</code>
    /// if no mapping was found.
    /// </returns>
    public static Color Put( string name, Color color )
    {
        return Map[ name ] = color;
    }

    /// <summary>
    /// Resets the color map to the predefined colors.
    /// </summary>
    public static void Reset()
    {
        Map.Clear();
        Map.Put( "CLEAR", Color.Clear );
        Map.Put( "BLACK", Color.Black );

        Map.Put( "WHITE", Color.White );
        Map.Put( "LIGHT_GRAY", Color.LightGray );
        Map.Put( "GRAY", Color.Gray );
        Map.Put( "DARK_GRAY", Color.DarkGray );

        Map.Put( "BLUE", Color.Blue );
        Map.Put( "NAVY", Color.Navy );
        Map.Put( "ROYAL", Color.Royal );
        Map.Put( "SLATE", Color.Slate );
        Map.Put( "SKY", Color.Sky );
        Map.Put( "CYAN", Color.Cyan );
        Map.Put( "TEAL", Color.Teal );

        Map.Put( "GREEN", Color.Green );
        Map.Put( "CHARTREUSE", Color.Chartreuse );
        Map.Put( "LIME", Color.Lime );
        Map.Put( "FOREST", Color.Forest );
        Map.Put( "OLIVE", Color.Olive );

        Map.Put( "YELLOW", Color.Yellow );
        Map.Put( "GOLD", Color.Gold );
        Map.Put( "GOLDENROD", Color.Goldenrod );
        Map.Put( "ORANGE", Color.Orange );

        Map.Put( "BROWN", Color.Brown );
        Map.Put( "TAN", Color.Tan );
        Map.Put( "FIREBRICK", Color.Firebrick );

        Map.Put( "RED", Color.Red );
        Map.Put( "SCARLET", Color.Scarlet );
        Map.Put( "CORAL", Color.Coral );
        Map.Put( "SALMON", Color.Salmon );
        Map.Put( "PINK", Color.Pink );
        Map.Put( "MAGENTA", Color.Magenta );

        Map.Put( "PURPLE", Color.Purple );
        Map.Put( "VIOLET", Color.Violet );
        Map.Put( "MAROON", Color.Maroon );
    }
}