// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Reflection;

using LibGDXSharp.Utils;

using BindingFlags = System.Reflection.BindingFlags;

namespace LibGDXSharp.GdxCore.Utils.Pooling;

/// <summary>
/// Pool that creates new instances of a type using reflection.
/// The type must have a zero argument constructor.
/// </summary>
/// <typeparam name="T"></typeparam>
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class ReflectionPool<T> : Pool< T >
{
    private readonly System.Reflection.ConstructorInfo? _constructor;

    public ReflectionPool( int initialCapacity )
        : this( initialCapacity, int.MaxValue )
    {
    }

    public ReflectionPool( int initialCapacity = 16, int max = int.MaxValue )
        : base( initialCapacity, max )
    {
        _constructor = FindConstructor( typeof( T ) );

        if ( _constructor == null )
        {
            throw new Exception( $"Class cannot be created (missing no-arg constructor): {typeof( T ).FullName}" );
        }
    }

    private System.Reflection.ConstructorInfo? FindConstructor( Type type )
    {
        try
        {
            return type.GetConstructor
                (
                BindingFlags.Public | BindingFlags.Instance,
                null,
                CallingConventions.Any,
                null!,
                null
                );

//            return ClassReflection.GetDeclaredConstructor( type, ( Type[] )null );
        }
        catch ( Exception )
        {
//            try
//            {
//                System.Reflection.ConstructorInfo constructor
//                    = ClassReflection.GetDeclaredConstructor( type, ( Type[] )null! );

//                constructor.SetAccessible( true );

//                return constructor;
//            }
//            catch ( ReflectionException )
//            {
                return null;
//            }
        }
    }

    protected new T NewObject()
    {
//        try
//        {
//            return ( T )_constructor.NewInstance( ( object[] )null );
//        }
//        catch ( Exception ex )
//        {
//            throw new GdxRuntimeException
//                ( $"Unable to create new instance: {_constructor.GetDeclaringClass().getName()}", ex );
//        }

        throw new NotImplementedException();
    }
}