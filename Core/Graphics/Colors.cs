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

using LibGDXSharp.G2D;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Graphics;

/// <summary>
/// A general purpose class containing named colors that can be changed at will.
/// For example, the markup language defined by the <see cref="BitmapFontCache"/> class
/// uses this class to retrieve colors and the user can define his own colors.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
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
        Map.Put( "CLEAR", Color.CLEAR );
        Map.Put( "BLACK", Color.BLACK );

        Map.Put( "WHITE", Color.WHITE );
        Map.Put( "LIGHT_GRAY", Color.LIGHT_GRAY );
        Map.Put( "GRAY", Color.GRAY );
        Map.Put( "DARK_GRAY", Color.DARK_GRAY );

        Map.Put( "BLUE", Color.BLUE );
        Map.Put( "NAVY", Color.NAVY );
        Map.Put( "ROYAL", Color.ROYAL );
        Map.Put( "SLATE", Color.SLATE );
        Map.Put( "SKY", Color.SKY );
        Map.Put( "CYAN", Color.CYAN );
        Map.Put( "TEAL", Color.TEAL );

        Map.Put( "GREEN", Color.GREEN );
        Map.Put( "CHARTREUSE", Color.CHARTREUSE );
        Map.Put( "LIME", Color.LIME );
        Map.Put( "FOREST", Color.FOREST );
        Map.Put( "OLIVE", Color.OLIVE );

        Map.Put( "YELLOW", Color.YELLOW );
        Map.Put( "GOLD", Color.GOLD );
        Map.Put( "GOLDENROD", Color.GOLDENROD );
        Map.Put( "ORANGE", Color.ORANGE );

        Map.Put( "BROWN", Color.BROWN );
        Map.Put( "TAN", Color.TAN );
        Map.Put( "FIREBRICK", Color.FIREBRICK );

        Map.Put( "RED", Color.RED );
        Map.Put( "SCARLET", Color.SCARLET );
        Map.Put( "CORAL", Color.CORAL );
        Map.Put( "SALMON", Color.SALMON );
        Map.Put( "PINK", Color.PINK );
        Map.Put( "MAGENTA", Color.MAGENTA );

        Map.Put( "PURPLE", Color.PURPLE );
        Map.Put( "VIOLET", Color.VIOLET );
        Map.Put( "MAROON", Color.MAROON );
    }
}