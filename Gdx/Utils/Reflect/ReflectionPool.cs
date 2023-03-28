namespace LibGDXSharp.Utils.Reflect
{
    /// <summary>
    /// Pool that creates new instances of a type using reflection. The type must
    /// have a zero argument constructor.
    /// <see cref="Constructor.SetAccessible(bool)"/> will be used if the class
    /// and/or constructor is not visible.
    /// </summary>
    public class ReflectionPool<T> : Pool< T >
    {
        private readonly System.Reflection.ConstructorInfo? _constructor;

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="initialCapacity"></param>
        public ReflectionPool( Type type, int initialCapacity )
            : this( type, initialCapacity, int.MaxValue )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="initialCapacity"></param>
        /// <param name="max"></param>
        /// <exception cref="Exception"></exception>
        public ReflectionPool( Type type, int initialCapacity = 16, int max = int.MaxValue )
            : base( initialCapacity, max )
        {
            _constructor = FindConstructor( type );

            if ( _constructor == null )
            {
                throw new Exception( "Class cannot be created (missing no-arg constructor): " + type.FullName );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Constructor? FindConstructor( Type type )
        {
            try
            {
                return ClassReflection.GetConstructor( type, ( Type[] )null );
            }
            catch ( Exception )
            {
                try
                {
                    Constructor constructor = ClassReflection.GetDeclaredConstructor( type, ( Type[] )null );

                    constructor.SetAccessible( true );

                    return constructor;
                }
                catch ( ReflectionException )
                {
                    return null;
                }
            }
        }
        
        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        protected override T NewObject()
        {
            try
            {
                return ( T )_constructor.NewInstance( ( object[] )null );
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException
                (
                    "Unable to create new instance: "
                    + _constructor.GetDeclaringClass().GetName(), ex
                );
            }
        }
    }
}
