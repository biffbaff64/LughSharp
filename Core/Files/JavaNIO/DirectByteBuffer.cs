using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Files;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class DirectByteBuffer : MappedByteBuffer
{
    /// <summary>
    /// Tells whether or not this buffer is read-only.
    /// </summary>
    /// <returns>  <tt>true</tt> if this buffer is read-only </returns>
    public override bool IsReadOnly { get; set; }

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible
    /// array.
    /// 
    /// <para>
    /// If this method returns <tt>true</tt> then the <see cref="LibGDXSharp.Files.Buffer.Array"/>
    /// and <see cref="LibGDXSharp.Files.Buffer.ArrayOffset"/> methods may safely be invoked.
    /// </para>
    /// </summary>
    /// <returns>
    /// <tt>true</tt> if this buffer is backed by an array and is not read-only.
    /// </returns>
    public new bool HasArray() => throw new NotImplementedException();

    /// <summary>
    /// Returns the array that backs this buffer.
    /// 
    /// <para>
    /// Modifications to this buffer's content will cause the returned array's
    /// content to be modified, and vice versa.
    /// </para>
    /// 
    /// <para>
    /// Invoke the <see cref="LibGDXSharp.Files.Buffer.HasArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.  </para>
    /// </summary>
    /// <returns>  The array that backs this buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public new object Array() => throw new NotImplementedException();

    /// <summary>
    /// Returns the offset within this buffer's backing array of the first
    /// element of the buffer <i>(optional operation)</i>.
    /// 
    /// <para>
    /// If this buffer is backed by an array then buffer position <i>p</i>
    /// corresponds to array index <i>p</i> + <tt>arrayOffset()</tt>.
    /// </para>
    /// <para>
    /// Invoke the <see cref="LibGDXSharp.Files.Buffer.HasArray"/> method before invoking this method
    /// in order to ensure that this buffer has an accessible backing array.
    /// </para>
    /// </summary>
    /// <returns>
    /// The offset within this buffer's array of the first element of the buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public new int ArrayOffset() => throw new NotImplementedException();

    /// <summary>
    /// Tells whether or not this buffer is <tt><i>direct</i></tt>
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct</returns>
    public override bool Direct { get; }
}