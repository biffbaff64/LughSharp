using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps.Objects
{
    public class PolygonMapObject : MapObject
    {        
        public Polygon Polygon { get; set; }

        /// <summary>
        /// Creates empty polygon map object
        /// </summary>
        public PolygonMapObject() : this( new float[0] )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="vertices">polygon defining vertices (at least 3)</param>
        public PolygonMapObject( float[] vertices )
        {
            this.Polygon = new Polygon(vertices);
        }

        /// <summary>
        /// </summary>
        /// <param name="polygon">the polygon</param>
        public PolygonMapObject( Polygon polygon )
        {
            this.Polygon = polygon;
        }

        /// <summary>
        /// <param name="polygon">new object's polygon shape</param>
        /// </summary>
        public void setPolygon( Polygon polygon )
        {
            this.Polygon = polygon;
        }
    }
}

