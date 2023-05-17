using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps.Objects;

public class PolylineMapObject : MapObject
{
    /// <returns> polyline shape </returns>
    public virtual Polyline Polyline { get; set; }


    /// <summary>
    /// Creates empty polyline </summary>
    public PolylineMapObject() : this(new float[0])
    {
    }

    /// <param name="vertices"> polyline defining vertices </param>
    public PolylineMapObject(float[] vertices)
    {
        this.Polyline = new Polyline(vertices);
    }

    /// <param name="polyline"> the polyline </param>
    public PolylineMapObject(Polyline polyline)
    {
        this.Polyline = polyline;
    }
}