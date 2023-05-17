using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.GdxCore.Utils.Pooling;

/// <summary>
/// Pool that creates new instances of a type using reflection.
/// The type must have a zero argument constructor.
/// </summary>
/// <typeparam name="T"></typeparam>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class ReflectionPool<T> : Pool<T>
{
    public ReflectionPool( int initialCapacity = 16, int max = int.MaxValue )
        : base( initialCapacity, max )
    {
    }

    protected new T NewObject()
    {
        return Activator.CreateInstance< T >();
    }
}