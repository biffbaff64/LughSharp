using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Objects
{
    public class TextureMapObject : MapObject
    {
        public float X                      { get; set; } = 0.0f;
        public float Y                      { get; set; } = 0.0f;
        public float OriginX                { get; set; } = 0.0f;
        public float OriginY                { get; set; } = 0.0f;
        public float ScaleX                 { get; set; } = 1.0f;
        public float ScaleY                 { get; set; } = 1.0f;
        public float Rotation               { get; set; } = 0.0f;
        public TextureRegion? TextureRegion { get; set; } = null;

        /// <summary>
        /// Creates an empty texture map object
        /// </summary>
        public TextureMapObject() : this( null )
        {
        }

        /// <summary>
        /// Creates texture map object with the given region
        /// </summary>
        /// <param name="textureRegion">the <see cref="TextureRegion"/> to use.</param>
        public TextureMapObject (TextureRegion? textureRegion)
        {
            this.TextureRegion = textureRegion;
        }
    }
}

