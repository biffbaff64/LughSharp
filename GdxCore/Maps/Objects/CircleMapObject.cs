using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps.Objects
{
    public class CircleMapObject : MapObject
    {
        public Circle Circle { get; set; }

        /// <summary>
        /// Creates a circle map object at (0,0) with r=1.0
        /// </summary>
        public CircleMapObject() : this( 0.0f, 0.0f, 1.0f )
        {
        }

        /// <summary>
        /// Creates a circle map object
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="radius">Radius of the circle object.</param>
        public CircleMapObject( float x, float y, float radius )
        {
            Circle = new Circle( x, y, radius );
        }
    }
}

