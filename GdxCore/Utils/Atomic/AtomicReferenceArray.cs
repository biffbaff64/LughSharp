using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LibGDXSharp.Utils.Atomic;

/// <summary>
/// An array of object references in which elements may be updated atomically.
/// </summary>
/// <typeparam name="T"></typeparam>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class AtomicReferenceArray<T>
{
    private readonly int  _base;
    private readonly int  _shift;
    private readonly long _arrayFieldOffset;

    private readonly object[] _array; // must have exact type object[]

    static AtomicReferenceArray()
    {
        try
        {
            Unsafe.
                
                _arrayFieldOffset = Unsafe.ObjectFieldOffset( typeof(AtomicReferenceArray).getDeclaredField( "array" ) );
            _base = _unsafe.arrayBaseOffset( typeof(object[]) );
            int scale = _unsafe.arrayIndexScale( typeof(object[]) );

            if ( ( scale & ( scale - 1 ) ) != 0 )
            {
                throw new Exception( "data type scale not a power of two" );
            }

            _shift = 31 - Integer.numberOfLeadingZeros( scale );
        }
        catch ( Exception e )
        {
            throw new Exception( e );
        }
    }

    public AtomicReferenceArray( int capacity )
    {
        _array = new object[ capacity ];
    }
}