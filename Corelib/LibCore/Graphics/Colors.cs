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

using Corelib.LibCore.Graphics.G2D;

namespace Corelib.LibCore.Graphics;

/// <summary>
/// A general purpose class containing named colors that can be changed at will.
/// For example, the markup language defined by the <see cref="BitmapFontCache"/> class
/// uses this class to retrieve colors and the user can define his own colors.
/// </summary>
[PublicAPI]
public static class Colors
{
    public static Dictionary< string, Color > Map { get; set; } = new();

    // ------------------------------------------------------------------------
    
    /// <summary>
    /// Looks up a color by its name.
    /// </summary>
    /// <param name="name">The name of the color.</param>
    /// <returns>
    /// The <see cref="Color"/> associated with the specified <paramref name="name"/>,
    /// or <c>null</c> if no mapping was found.
    /// </returns>
    public static Color? Get( string name )
    {
        return Map.GetValueOrDefault( name );
    }

    /// <summary>
    /// Adds or replaces a color with the specified name.
    /// </summary>
    /// <param name="name">The name of the color.</param>
    /// <param name="color">The color to add or replace.</param>
    public static void Put( string name, Color color )
    {
        Map[ name ] = color;
    }

    /// <summary>
    /// Resets the color map to the predefined colors.
    /// </summary>
    public static void Reset()
    {
        Map.Clear();
        AddPredefinedColors();
    }

    /// <summary>
    /// Adds the predefined colors to the color map.
    /// </summary>
    private static void AddPredefinedColors()
    {
        Map[ "CLEAR" ]      = Color.Clear;
        Map[ "BLACK" ]      = Color.Black;
        Map[ "WHITE" ]      = Color.White;
        Map[ "LIGHT_GRAY" ] = Color.LightGray;
        Map[ "GRAY" ]       = Color.Gray;
        Map[ "DARK_GRAY" ]  = Color.DarkGray;
        Map[ "BLUE" ]       = Color.Blue;
        Map[ "NAVY" ]       = Color.Navy;
        Map[ "ROYAL" ]      = Color.Royal;
        Map[ "SLATE" ]      = Color.Slate;
        Map[ "SKY" ]        = Color.Sky;
        Map[ "CYAN" ]       = Color.Cyan;
        Map[ "TEAL" ]       = Color.Teal;
        Map[ "GREEN" ]      = Color.Green;
        Map[ "CHARTREUSE" ] = Color.Chartreuse;
        Map[ "LIME" ]       = Color.Lime;
        Map[ "FOREST" ]     = Color.Forest;
        Map[ "OLIVE" ]      = Color.Olive;
        Map[ "YELLOW" ]     = Color.Yellow;
        Map[ "GOLD" ]       = Color.Gold;
        Map[ "GOLDENROD" ]  = Color.Goldenrod;
        Map[ "ORANGE" ]     = Color.Orange;
        Map[ "BROWN" ]      = Color.Brown;
        Map[ "TAN" ]        = Color.Tan;
        Map[ "FIREBRICK" ]  = Color.Firebrick;
        Map[ "RED" ]        = Color.Red;
        Map[ "SCARLET" ]    = Color.Scarlet;
        Map[ "CORAL" ]      = Color.Coral;
        Map[ "SALMON" ]     = Color.Salmon;
        Map[ "PINK" ]       = Color.Pink;
        Map[ "MAGENTA" ]    = Color.Magenta;
        Map[ "PURPLE" ]     = Color.Purple;
        Map[ "VIOLET" ]     = Color.Violet;
        Map[ "MAROON" ]     = Color.Maroon;
    }
}
