namespace LibGDXSharp.Assets
{
    internal interface IReferencedObject
    {
        object Asset    { get; set; }
        int    RefCount { get; set; }
    }

    /// <summary>
    /// A class that stores a reference to an object, as well as counts
    /// the number of times it has been referenced.
    /// 
    /// This is not automated (within this class), and you have to increment
    /// the RefCount property each time you start using the object, and
    /// decrement it after you're done using it.
    /// 
    /// AssetManager handles this automatically.
    /// </summary>
    internal class ReferencedObject<T> : IReferencedObject
    {
        public T   Asset    { get; private set; }
        public int RefCount { get; set; }

        public ReferencedObject( T asset )
        {
            if ( asset == null ) throw new GdxRuntimeException( "asset is null" );
            
            RefCount = 1;
            Asset    = asset;
        }

        object IReferencedObject.Asset
        {
            get => Asset!;
            set => Asset = (T)value;
        }
    }
}

