using System.Runtime.Serialization;

namespace LibGDXSharp.Maths.Collision
{
    /// <summary>
    /// Encapsulates an axis aligned bounding box represented by a minimum
    /// and a maximum Vector. Additionally you can query for the bounding
    /// box's center, dimensions and corner points.
    /// </summary>
    public class BoundingBox : ISerializable
    {

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" />
        /// with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public void GetObjectData( SerializationInfo info, StreamingContext context )
        {
        }
    }
}

