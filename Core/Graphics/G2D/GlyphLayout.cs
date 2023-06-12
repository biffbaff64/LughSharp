namespace LibGDXSharp.G2D;

public class GlyphLayout
{
    public float Width  { get; set; }
    public float Height { get; set; }

    public class GlyphRun
    {
        public List< BitmapFont.Glyph > Glyphs    { get; set; }
        public List< float >            XAdvances { get; set; }
    }
}