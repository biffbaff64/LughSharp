using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class FloatBuffer : Buffer, IComparable< FloatBuffer >
{
    public abstract float Get( int index );
    
    /// <summary>
    /// Tells whether or not this buffer is read-only.
    /// </summary>
    /// <returns>  <tt>true</tt> if this buffer is read-only </returns>
    public override bool ReadOnly { get; }

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible
    /// array.
    /// 
    /// <para> If this method returns <tt>true</tt> then the <see cref="Buffer.Array"/>
    /// and <see cref="Buffer.ArrayOffset"/> methods may safely be invoked.
    /// </para>
    /// </summary>
    /// <returns>  <tt>true</tt> if this buffer
    ///          is backed by an array and is not read-only
    /// </returns>
    public override bool HasArray() => throw new NotImplementedException();

    /// <summary>
    /// Returns the array that backs this buffer.
    /// 
    /// <para>
    /// Modifications to this buffer's content will cause the returned array's
    /// content to be modified, and vice versa.
    /// </para>
    /// 
    /// <para>
    /// Invoke the <see cref="Buffer.HasArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.  </para>
    /// </summary>
    /// <returns>  The array that backs this buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///          If this buffer is not backed by an accessible array
    /// </exception>
    public override object Array() => throw new NotImplementedException();

    /// <summary>
    /// Returns the offset within this buffer's backing array of the first
    /// element of the buffer <i>(optional operation)</i>.
    /// 
    /// <para> If this buffer is backed by an array then buffer position <i>p</i>
    /// corresponds to array index <i>p</i> + <tt>arrayOffset()</tt>.
    /// 
    /// </para>
    /// <para> Invoke the <see cref="Buffer.HasArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.  </para>
    /// </summary>
    /// <returns>  The offset within this buffer's array
    ///          of the first element of the buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///          If this buffer is not backed by an accessible array
    /// </exception>
    public override int ArrayOffset() => throw new NotImplementedException();

    /// <summary>
    /// Tells whether or not this buffer is <tt><i>direct</i></tt>
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct</returns>
    public override bool Direct { get; }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
    public int CompareTo( FloatBuffer? other ) => throw new NotImplementedException();
}
