namespace LibGDXSharp.Maps
{
    public class MapObject
    {
        public string        Name       { get; set; }         = "";
        public float         Opacity    { get; set; }         = 1.0f;
        public bool          Visible    { get; set; }         = true;
        public Color         Color      { get; set; }         = Color.White;
        public MapProperties Properties { get; private set; } = new MapProperties();
    }
}
