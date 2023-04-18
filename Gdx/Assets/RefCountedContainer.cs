namespace LibGDXSharp.Assets
{
    internal sealed class RefCountedContainer
    {
        public int RefCount { get; set; } = 1;

        private object? _object;

        public RefCountedContainer( object obj )
        {
            this._object = obj ?? throw new System.ArgumentException( "Object must not be null" );
        }
        
        public object? GetObject()
        {
            return _object;
        }
        
        public void SetObject<T>( T obj )
        {
            this._object = obj!;
        }
    }
}
