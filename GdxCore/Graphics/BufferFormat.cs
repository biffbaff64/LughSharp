namespace LibGDXSharp.Graphics
{
    /// <summary>
    /// Class describing the bits per pixel, depth buffer precision,
    /// stencil precision and number of MSAA samples.
    /// </summary>
    public record BufferFormat
    {
        public int  R                { get; set; }  // number of bits per color channel.
        public int  G                { get; set; }  // ...
        public int  B                { get; set; }  // ...
        public int  A                { get; set; }  // ...
        public int  Depth            { get; set; }  // number of bits for depth and stencil buffer.
        public int  Stencil          { get; set; }  // ...
        public int  Samples          { get; set; }  // number of samples for multi-sample anti-aliasing (MSAA).
        public bool CoverageSampling { get; set; }  // whether coverage sampling anti-aliasing is used.
                                                    // If so, you have to clear the coverage buffer as well!

        public override string ToString()
        {
            return "r - " + R + ", g - " + G + ", b - " + B + ", a - " + A
                   + ", depth - " + Depth + ", stencil - " + Stencil
                   + ", num samples - " + Samples + ", coverage sampling - " + CoverageSampling;
        }
    }
}
