namespace LibGDXSharp.Assets
{
    internal sealed class RefCountedContainer
    {
        private int    _refCount = 1;
        private object _object;

        public RefCountedContainer( object? obj )
        {
            this._object = obj ?? throw new System.ArgumentException( "Object must not be null" );
        }

        public void IncRefCount() => _refCount++;

        public void DecRefCount() => _refCount--;

        public int GetRefCount() => _refCount;

        public void SetRefCount( int count ) => _refCount = count;

        public object? GetObject<T>( Type type ) => ( T )_object;

        public void SetObject<T>( object obj ) => this._object = (T)obj;
    }
}
