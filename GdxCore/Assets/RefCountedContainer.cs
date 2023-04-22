namespace LibGDXSharp.Assets
{
    internal interface IRefCountedContainer
    {
        public object? Asset    { get; set; }
        public int     RefCount { get; set; }
    }

    /// <summary>
    /// A class that stores a reference to an object, as well as counts
    /// the number of times it has been referenced.
    /// <see cref="RefCount"/>must be incremented each time you start
    /// using the object, and decrement it after you're done using it.
    /// AssetManager handles this automatically.
    /// </summary>
    internal sealed class RefCountedContainer : IRefCountedContainer
    {
        public int RefCount { get; set; } = 1;

        private object? _object;

        public RefCountedContainer( object obj )
        {
            this._object = obj ?? throw new System.ArgumentException( "Object must not be null" );
        }

        object? IRefCountedContainer.Asset
        {
            get => _object;
            set => _object = value;
        }
    }
}
