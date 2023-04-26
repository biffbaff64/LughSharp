using LibGDXSharp.G2D;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Graphics
{
    /// <summary>
    /// A general purpose class containing named colors that can be changed at will.
    /// For example, the markup language defined by the <see cref="BitmapFontCache"/> class
    /// uses this class to retrieve colors and the user can define his own colors.
    /// </summary>
    public class Colors
    {
        private readonly static Dictionary< string, Color > map = new Dictionary< string, Color >();

        static Colors()
        {
            Reset();
        }

        private Colors()
        {
        }

        /// <summary>
        /// Returns the Color map.
        /// </summary>
        public static Dictionary< string, Color > GetColors()
        {
            return map;
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
        public static Color Get( string name )
        {
            return map[ name ];
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
            return map[ name ] = color;
        }

        /// <summary>
        /// Resets the color map to the predefined colors.
        /// </summary>
        public static void Reset()
        {
            map.Clear();
            map.Put( "CLEAR", Color.Clear );
            map.Put( "BLACK", Color.Black );

            map.Put( "WHITE", Color.White );
            map.Put( "LIGHT_GRAY", Color.LightGray );
            map.Put( "GRAY", Color.Gray );
            map.Put( "DARK_GRAY", Color.DarkGray );

            map.Put( "BLUE", Color.Blue );
            map.Put( "NAVY", Color.Navy );
            map.Put( "ROYAL", Color.Royal );
            map.Put( "SLATE", Color.Slate );
            map.Put( "SKY", Color.Sky );
            map.Put( "CYAN", Color.Cyan );
            map.Put( "TEAL", Color.Teal );

            map.Put( "GREEN", Color.Green );
            map.Put( "CHARTREUSE", Color.Chartreuse );
            map.Put( "LIME", Color.Lime );
            map.Put( "FOREST", Color.Forest );
            map.Put( "OLIVE", Color.Olive );

            map.Put( "YELLOW", Color.Yellow );
            map.Put( "GOLD", Color.Gold );
            map.Put( "GOLDENROD", Color.Goldenrod );
            map.Put( "ORANGE", Color.Orange );

            map.Put( "BROWN", Color.Brown );
            map.Put( "TAN", Color.Tan );
            map.Put( "FIREBRICK", Color.Firebrick );

            map.Put( "RED", Color.Red );
            map.Put( "SCARLET", Color.Scarlet );
            map.Put( "CORAL", Color.Coral );
            map.Put( "SALMON", Color.Salmon );
            map.Put( "PINK", Color.Pink );
            map.Put( "MAGENTA", Color.Magenta );

            map.Put( "PURPLE", Color.Purple );
            map.Put( "VIOLET", Color.Violet );
            map.Put( "MAROON", Color.Maroon );
        }
    }
}
