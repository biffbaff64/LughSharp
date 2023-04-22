using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps.Objects
{
    public class EllipseMapObject : MapObject
    {
        public Ellipse Ellipse { get; set; }

        /// <symmary>
        /// Creates an <see cref="Ellipse"/> object whose lower left corner
        /// is at(0, 0) with width=1 and height=1
        /// </symmary>
        public EllipseMapObject()
        {
            this( 0.0f, 0.0f, 1.0f, 1.0f );
        }

        /// <summary>
        /// Creates an <see cref="Ellipse"/> object with the given X and Y coordinates
        /// along with a specified width and height.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        public EllipseMapObject( float x, float y, float width, float height )
        {
            Ellipse = new Ellipse( x, y, width, height );
        }
    }
}

