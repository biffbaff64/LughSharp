using System.Reflection;
using System.Security;

namespace LibGDXSharp.Utils.Reflect
{
    public class ClassReflection
    {
        /// <summary>
        /// Returns the Class object associated with the class or interface with the supplied string name.
        /// </summary>
        public static Type ForName( string name )
        {
            try
            {
                return Type.ForName( name );
            }
            catch ( ClassNotFoundException e )
            {
                throw new ReflectionException( "Class not found: " + name, e );
            }
        }

        /// <summary>
        /// Returns the simple name of the underlying class as supplied in the source code.
        /// </summary>
        public static string GetSimpleName( Type c )
        {
            return c.Name;
        }

        /// <summary>
        /// Determines if the supplied Object is assignment-compatible with the object
        /// represented by supplied Class.
        /// </summary>
        public static bool IsInstance( Type c, object obj )
        {
            return c.IsInstanceOfType( obj );
        }

        /// <summary>
        /// Determines if the class or interface represented by first Class parameter is
        /// either the same as, or is a superclass or superinterface of, the class or
        /// interface represented by the second Class parameter. 
        /// </summary>
        public static bool IsAssignableFrom( Type c1, Type c2 )
        {
            return c1.IsAssignableFrom( c2 );
        }

        /// <summary>
        /// Returns true if the class or interface represented by the supplied Class is
        /// a member class.
        /// </summary>
        public static bool IsMemberClass( Type c )
        {
            return c.isMemberClass();
        }

        /// <summary>
        /// Returns true if the class or interface represented by the supplied Class is
        /// a static class.
        /// </summary>
        public static bool IsStaticClass( Type c )
        {
            return Modifier.isStatic( c.getModifiers() );
        }

        /// <summary>
        /// Determines if the supplied Class object represents an array class.
        /// </summary>
        public static bool IsArray( Type c )
        {
            return c.IsArray;
        }

        /// <summary>
        /// Determines if the supplied Class object represents a primitive type.
        /// </summary>
        public static bool IsPrimitive( Type c )
        {
            return c.IsPrimitive;
        }

        /// <summary>
        /// Determines if the supplied Class object represents an enum type.
        /// </summary>
        public static bool IsEnum( Type c )
        {
            return c.IsEnum;
        }

        /// <summary>
        /// Determines if the supplied Class object represents an annotation type.
        /// </summary>
        public static bool IsAnnotation( Type c )
        {
            return c.isAnnotation();
        }

        /// <summary>
        /// Determines if the supplied Class object represents an interface type.
        /// </summary>
        public static bool IsInterface( Type c )
        {
            return c.IsInterface;
        }

        /// <summary>
        /// Determines if the supplied Class object represents an abstract type.
        /// </summary>
        public static bool IsAbstract( Type c )
        {
            return Modifier.IsAbstract( c.getModifiers() );
        }

        /// <summary>
        /// Creates a new instance of the class represented by the supplied Class.
        /// </summary>
        public static T NewInstance<T>( Type c )
        {
            try
            {
                return System.Activator.CreateInstance( c );
            }
            catch ( InstantiationException e )
            {
                throw new ReflectionException( "Could not instantiate instance of class: " + c.FullName, e );
            }
            catch ( IllegalAccessException e )
            {
                throw new ReflectionException( "Could not instantiate instance of class: " + c.FullName, e );
            }
        }

        /// <summary>
        /// Returns the Class representing the component type of an array. If this class does not
        /// represent an array class this method returns null.
        /// </summary>
        public static Type GetComponentType( Type c )
        {
            return c.GetElementType();
        }

        /// <summary>
        /// Returns an array of <see cref="System.Reflection.ConstructorInfo"/> containing the public
        /// constructors of the class represented by the supplied Class.
        /// </summary>
        public static System.Reflection.ConstructorInfo[] GetConstructors( Type c )
        {
            System.Reflection.ConstructorInfo[] constructors = c.GetConstructors();
            System.Reflection.ConstructorInfo[] result       = new System.Reflection.ConstructorInfo[ constructors.Length ];

            for ( int i = 0, j = constructors.Length; i < j; i++ )
            {
                result[ i ] = new System.Reflection.ConstructorInfo( constructors[ i ] );
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.Reflection.ConstructorInfo"/> that represents the public
        /// constructor for the supplied class which takes the supplied parameter types. 
        /// </summary>
        public static System.Reflection.ConstructorInfo GetConstructor( Type c, params Type[] parameterTypes )
        {
            try
            {
                return new System.Reflection.ConstructorInfo( c.GetConstructor( parameterTypes ) );
            }
            catch ( SecurityException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Security violation occurred while getting constructor for class: '" + c.FullName + "'.", e );
            }
            catch ( NoSuchMethodException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Constructor not found for class: " + c.FullName, e );
            }
        }

        /// <summary>
        /// Returns a <seealso cref="System.Reflection.ConstructorInfo"/> that represents the constructor for the supplied class which takes the supplied parameter
        /// types. 
        /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static Constructor getDeclaredConstructor(Class c, Class... parameterTypes) throws ReflectionException
        public static System.Reflection.ConstructorInfo GetDeclaredConstructor( Type c, params Type[] parameterTypes )
        {
            try
            {
                return new System.Reflection.ConstructorInfo( c.GetDeclaredConstructor( parameterTypes ) );
            }
            catch ( SecurityException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Security violation while getting constructor for class: " + c.FullName, e );
            }
            catch ( NoSuchMethodException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Constructor not found for class: " + c.FullName, e );
            }
        }

        /// <summary>
        /// Returns the elements of this enum class or null if this Class object does not represent an enum type. </summary>
        public static object[] GetEnumConstants( Type c )
        {
            return c.getEnumConstants();
        }

        /// <summary>
        /// Returns an array of <seealso cref="System.Reflection.MethodInfo"/> containing the public member methods of the class represented by the supplied Class. </summary>
        public static System.Reflection.MethodInfo[] GetMethods( Type c )
        {
            System.Reflection.MethodInfo[] methods = c.GetMethods();
            System.Reflection.MethodInfo[] result  = new System.Reflection.MethodInfo[ methods.Length ];

            for ( int i = 0, j = methods.Length; i < j; i++ )
            {
                result[ i ] = new System.Reflection.MethodInfo( methods[ i ] );
            }

            return result;
        }

        /// <summary>
        /// Returns a <seealso cref="System.Reflection.MethodInfo"/> that represents the public member method for the supplied class which takes the supplied parameter
        /// types. 
        /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static Method getMethod(Class c, String name, Class... parameterTypes) throws ReflectionException
        public static System.Reflection.MethodInfo GetMethod( Type c, string name, params Type[] parameterTypes )
        {
            try
            {
                return new System.Reflection.MethodInfo( c.GetMethod( name, parameterTypes ) );
            }
            catch ( SecurityException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Security violation while getting method: " + name + ", for class: " + c.FullName, e );
            }
            catch ( NoSuchMethodException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Method not found: " + name + ", for class: " + c.FullName, e );
            }
        }

        /// <summary>
        /// Returns an array of <seealso cref="System.Reflection.MethodInfo"/> containing the methods declared by the class represented by the supplied Class. </summary>
        public static System.Reflection.MethodInfo[] GetDeclaredMethods( Type c )
        {
            System.Reflection.MethodInfo[] methods = c.GetMethods( BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance );
            System.Reflection.MethodInfo[] result  = new System.Reflection.MethodInfo[ methods.Length ];

            for ( int i = 0, j = methods.Length; i < j; i++ )
            {
                result[ i ] = new System.Reflection.MethodInfo( methods[ i ] );
            }

            return result;
        }

        /// <summary>
        /// Returns a <seealso cref="System.Reflection.MethodInfo"/> that represents the method declared by the supplied class which takes the supplied parameter types. </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static Method getDeclaredMethod(Class c, String name, Class... parameterTypes) throws ReflectionException
        public static System.Reflection.MethodInfo GetDeclaredMethod( Type c, string name, params Type[] parameterTypes )
        {
            try
            {
                return new System.Reflection.MethodInfo( c.GetDeclaredMethod( name, parameterTypes ) );
            }
            catch ( SecurityException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Security violation while getting method: " + name + ", for class: " + c.FullName, e );
            }
            catch ( NoSuchMethodException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Method not found: " + name + ", for class: " + c.FullName, e );
            }
        }

        /// <summary>
        /// Returns an array of <seealso cref="System.Reflection.FieldInfo"/> containing the public fields of the class represented by the supplied Class. </summary>
        public static System.Reflection.FieldInfo[] GetFields( Type c )
        {
            System.Reflection.FieldInfo[] fields = c.GetFields();
            System.Reflection.FieldInfo[] result = new System.Reflection.FieldInfo[ fields.Length ];

            for ( int i = 0, j = fields.Length; i < j; i++ )
            {
                result[ i ] = new System.Reflection.FieldInfo( fields[ i ] );
            }

            return result;
        }

        /// <summary>
        /// Returns a <seealso cref="System.Reflection.FieldInfo"/> that represents the specified public member field for the supplied class. </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static Field getField(Class c, String name) throws ReflectionException
        public static System.Reflection.FieldInfo GetField( Type c, string name )
        {
            try
            {
                return new System.Reflection.FieldInfo( c.GetField( name ) );
            }
            catch ( SecurityException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Security violation while getting field: " + name + ", for class: " + c.FullName, e );
            }
            catch ( NoSuchFieldException e )
            {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new ReflectionException( "Field not found: " + name + ", for class: " + c.FullName, e );
            }
        }
    }
}
