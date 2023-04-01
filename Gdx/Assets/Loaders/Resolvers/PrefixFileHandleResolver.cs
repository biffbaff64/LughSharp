namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    /// <summary>
    /// FileHandleResolver that adds a prefix to the filename before passing it to
    /// the base resolver. Can be used e.g. to use a given subfolder from the base
    /// resolver. The prefix is added as is, you have to include any trailing '/'
    /// character if needed.
    /// </summary>
    public class PrefixFileHandleResolver : IFileHandleResolver
    {
        public IFileHandleResolver BaseResolver { set; get; }
        public string              Prefix       { set; get; }

        public PrefixFileHandleResolver( IFileHandleResolver baseResolver, string prefix )
        {
            this.BaseResolver = baseResolver;
            this.Prefix       = prefix;
        }

        public FileHandle? Resolve( string fileName )
        {
            return BaseResolver?.Resolve( Prefix + fileName );
        }
    }
}
