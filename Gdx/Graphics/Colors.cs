// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LibGDXSharp.Gdx.Graphics.G2D;
using LibGDXSharp.Gdx.Utils.Collections.Extensions;

namespace LibGDXSharp.Gdx.Graphics;

/// <summary>
///     A general purpose class containing named colors that can be changed at will.
///     For example, the markup language defined by the <see cref="BitmapFontCache" /> class
///     uses this class to retrieve colors and the user can define his own colors.
/// </summary>
public class Colors
{
    static Colors() => Reset();

    private Colors()
    {
    }

    public static Dictionary< string, Color? > Map { get; set; } = new();

    /// <summary>
    ///     Convenience method to lookup a color by <code>name</code>.
    ///     The invocation of this method is equivalent to the expression
    ///     <code>Colors.GetColors().Get(name)</code>
    /// </summary>
    /// <param name="name">The name of the color</param>
    /// <returns>
    ///     The Color to which the specified <code>name</code> is mapped,
    ///     or <code>null</code> if no mapping was found.
    /// </returns>
    public static Color? Get( string name ) => Map[ name ];

    /// <summary>
    ///     Convenience method to add a color with its <code>name</code>.
    ///     The invocation of this method is equivalent to the expression
    ///     <tt>Colors.GetColors().Put(name, color)</tt>
    /// </summary>
    /// <param name="name">The name of the color.</param>
    /// <param name="color">The color.</param>
    /// <returns>
    ///     The previous Color associated with <code>name</code> or <code>null</code>
    ///     if no mapping was found.
    /// </returns>
    public static Color Put( string name, Color color ) => Map[ name ] = color;

    /// <summary>
    ///     Resets the color map to the predefined colors.
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
