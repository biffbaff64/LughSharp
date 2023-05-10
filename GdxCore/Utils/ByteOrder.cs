using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils;

/// <summary>
/// A typesafe enumeration for byte orders.
/// </summary>
/// <remarks>Not sure if this is still needed.</remarks>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class ByteOrder
{
    private readonly string _name;

    private ByteOrder( string name )
    {
        this._name = name;
    }

    public readonly static ByteOrder NativeOrder = new("NativeOrder");

    /// <summary>
    /// Constant denoting big-endian byte order.  In this order, the bytes of a
    /// multibyte value are ordered from most significant to least significant.
    /// </summary>
    public readonly static ByteOrder BigEndian = new( "BigEndian" );

    /// <summary>
    /// Constant denoting little-endian byte order.  In this order, the bytes of
    /// a multibyte value are ordered from least significant to most
    /// significant.
    /// </summary>
    public readonly static ByteOrder LittleEndian = new( "LittleEndian" );

    /// <summary>
    /// Constructs a string describing this object.
    /// 
    /// <para>
    /// This method returns the string <tt>"BigEndian"</tt> for <see cref="BigEndian"/>
    /// and <tt>"LittleEndian"</tt> for <see cref="LittleEndian"/>.
    /// </para>
    /// </summary>
    /// <returns>The specified string</returns>
    public override string ToString()
    {
        return _name;
    }
}
