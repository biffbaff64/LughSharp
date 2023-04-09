namespace LibGDXSharp.Assets
{
    internal sealed class RefCountedContainer
    {
        public object Object   { get; set; }
        public int    RefCount { get; set; } = 1;

        public RefCountedContainer( object? obj )
        {
            this.Object = obj ?? throw new System.ArgumentException( "Object must not be null" );
        }
    }
}
