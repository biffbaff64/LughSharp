// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Utils;

/// <summary>
/// Node record holding a value and its index.
/// To change the contents of <see cref="Value"/> use <see cref="BinaryHeap{T}.Add(T, float)"/>
/// if the node is NOT in the heap, otherwise use <see cref="BinaryHeap{T}.SetValue(T, float)"/>.
/// </summary>
[PublicAPI]
public record BinaryHeapNode
{
    public float Value { get; set; }
    public int   Index { get; set; }
}

/// <summary>
/// A binary heap that stores nodes which each have a float value and are
/// sorted either lowest first or highest first.
/// <para>
/// The <see cref="BinaryHeapNode"/> class can be extended to store additional information.
/// </para>
/// </summary>
[PublicAPI]
public class BinaryHeap< T > where T : BinaryHeapNode
{
    // ========================================================================

    private const int DEFAULT_HEAP_CAPACITY = 16;

    // ========================================================================

    private readonly bool              _isMaxHeap;
    private          BinaryHeapNode[]? _nodes;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new BinaryHeap with the stated capacity.
    /// </summary>
    /// <param name="capacity"> The capacity to use. Default is 16. </param>
    /// <param name="isMaxHeap"></param>
    public BinaryHeap( int capacity = DEFAULT_HEAP_CAPACITY, bool isMaxHeap = false )
    {
        _isMaxHeap = isMaxHeap;
        _nodes     = new BinaryHeapNode[ capacity ];
    }

    /// <summary>
    /// Adds the node to the heap using its current value.
    /// The node should not already be in the heap.
    /// </summary>
    /// <returns>The specified node.</returns>
    public virtual T Add( T node )
    {
        GdxRuntimeException.ThrowIfNull( _nodes );

        // Expand if necessary.
        if ( Size == _nodes.Length )
        {
            var newNodes = new BinaryHeapNode[ Size << 1 ];

            Array.Copy( _nodes, 0, newNodes, 0, Size );

            _nodes = newNodes;
        }

        // Insert at end and bubble up.
        node.Index = Size;

        _nodes[ Size ] = node;

        Up( Size++ );

        return node;
    }

    /// <summary>
    /// Sets the node's value and adds it to the heap.
    /// The node should not already be in the heap.
    /// </summary>
    /// <returns>The specified node.</returns>
    public virtual T Add( T node, float value )
    {
        node.Value = value;

        return Add( node );
    }

    /// <summary>
    /// Returns true if the heap contains the specified node.
    /// </summary>
    /// <param name="node"> The <see cref="BinaryHeapNode"/> to look for. </param>
    /// <param name="identity">
    /// If true, == comparison will be used. If false an Equals() comparison will be used.
    /// </param>
    public bool Contains( T node, bool identity )
    {
        ArgumentNullException.ThrowIfNull( node );

        if ( _nodes == null )
        {
            return false;
        }

        foreach ( var n in _nodes )
        {
            if ( ( identity && ( n == node ) )
              || ( !identity && n.Equals( node ) ) )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the first item in the heap. This is the item with the lowest
    /// value (or highest value if this heap is configured as a max heap).
    /// </summary>
    public virtual T Peek()
    {
        GdxRuntimeException.ThrowIfNull( _nodes );

        if ( Size == 0 )
        {
            throw new InvalidOperationException( "The heap is empty." );
        }

        return ( T ) _nodes[ 0 ];
    }

    /// <summary>
    /// Removes the first item in the heap and returns it. This is
    /// the item with the lowest value (or highest value if this heap
    /// is configured as a max heap).
    /// </summary>
    public virtual T Pop()
    {
        GdxRuntimeException.ThrowIfNull( _nodes );

        var removed = _nodes[ 0 ];

        if ( --Size > 0 )
        {
            _nodes[ 0 ]    = _nodes[ Size ];
            _nodes[ Size ] = null!;

            Down( 0 );
        }
        else
        {
            _nodes[ 0 ] = null!;
        }

        return ( T ) removed;
    }

    /// <summary>
    /// Removes the specified node from the heap.
    /// </summary>
    /// <returns> The specified node. </returns>
    /// <exception cref="GdxRuntimeException"> If the heap is null. </exception>
    public virtual T Remove( T node )
    {
        GdxRuntimeException.ThrowIfNull( _nodes );

        if ( --Size > 0 )
        {
            var moved = _nodes[ Size ];
            _nodes[ Size ]       = null!;
            _nodes[ node.Index ] = moved;

            if ( ( moved.Value < node.Value ) ^ _isMaxHeap )
            {
                Up( node.Index );
            }
            else
            {
                Down( node.Index );
            }
        }
        else
        {
            _nodes[ 0 ] = null!;
        }

        return node;
    }

    /// <summary>
    /// Reset all elements in the heap to null.
    /// <para>
    /// If the heap does not exist, it will be created first with
    /// capacity set to <see cref="DEFAULT_HEAP_CAPACITY"/>
    /// </para>
    /// </summary>
    public void Clear()
    {
        // If _nodes hasn't been created, fix that problem...
        _nodes ??= new BinaryHeapNode[ DEFAULT_HEAP_CAPACITY ];

        Array.Fill( _nodes, null, 0, Size );
        Size = 0;
    }

    /// <summary>
    /// Changes the value of the node, which should already be in the heap.
    /// </summary>
    public void SetValue( T node, float value )
    {
        var oldValue = node.Value;

        node.Value = value;

        if ( ( value < oldValue ) ^ _isMaxHeap )
        {
            Up( node.Index );
        }
        else
        {
            Down( node.Index );
        }
    }

    /// <summary>
    /// Moves the element at position 'index' up
    /// </summary>
    /// <param name="index"></param>
    private void Up( int index )
    {
        GdxRuntimeException.ThrowIfNull( _nodes );

        var ix = index;

        while ( ix > 0 )
        {
            var parentIndex = ( ix - 1 ) >> 1;

            var parent = _nodes[ parentIndex ];

            if ( ( _nodes[ index ].Value < parent.Value ) ^ _isMaxHeap )
            {
                _nodes[ ix ] = parent;
                parent.Index = ix;
                ix           = parentIndex;
            }
            else
            {
                break;
            }
        }

        _nodes[ ix ]          = _nodes[ index ];
        _nodes[ index ].Index = ix;
    }

    /// <summary>
    /// Moves the element at position 'index' down
    /// </summary>
    /// <param name="index"></param>
    private void Down( int index )
    {
        GdxRuntimeException.ThrowIfNull( _nodes );

        var node = _nodes[ index ];

        var size  = Size;
        var value = node.Value;

        while ( true )
        {
            var leftIndex  = 1 + ( index << 1 );
            var rightIndex = leftIndex + 1;

            if ( leftIndex >= size )
            {
                break;
            }

            // Always has a left child.
            var leftNode  = _nodes[ leftIndex ];
            var leftValue = leftNode.Value;

            // May have a right child.
            BinaryHeapNode? rightNode;
            float           rightValue;

            if ( rightIndex >= size )
            {
                rightNode  = null;
                rightValue = _isMaxHeap ? -float.MaxValue : float.MaxValue;
            }
            else
            {
                rightNode  = _nodes[ rightIndex ];
                rightValue = rightNode.Value;
            }

            // The smallest of the three values is the parent.
            if ( ( leftValue < rightValue ) ^ _isMaxHeap )
            {
                if ( leftValue.Equals( value ) || ( ( leftValue > value ) ^ _isMaxHeap ) )
                {
                    break;
                }

                _nodes[ index ] = leftNode;
                leftNode.Index  = index;
                index           = leftIndex;
            }
            else
            {
                if ( rightValue.Equals( value ) || ( ( rightValue > value ) ^ _isMaxHeap ) )
                {
                    break;
                }

                _nodes[ index ] = rightNode!;

                if ( rightNode != null )
                {
                    rightNode.Index = index;
                }

                index = rightIndex;
            }
        }

        node.Index      = index;
        _nodes[ index ] = node;
    }

    /// <inheritdoc />
    public override bool Equals( object? obj )
    {
        if ( obj is not BinaryHeap< T > other )
        {
            return false;
        }

        if ( other.Size != Size )
        {
            return false;
        }

        Debug.Assert( _nodes != null );
        Debug.Assert( other._nodes != null );

        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( !_nodes[ i ].Value.Equals( other._nodes[ i ].Value ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = 37 + GetType().GetHashCode();

        hash = ( hash * 67 ) + NumberUtils.FloatToIntBits( hash );

        return hash;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if ( ( Size == 0 ) || ( _nodes == null ) )
        {
            return "[]";
        }

        BinaryHeapNode[] nodes  = _nodes;
        var              buffer = new StringBuilder( 32 );

        buffer.Append( '[' );
        buffer.Append( nodes[ 0 ].Value );

        for ( var i = 1; i < Size; i++ )
        {
            buffer.Append( ", " );
            buffer.Append( nodes[ i ].Value );
        }

        buffer.Append( ']' );

        return buffer.ToString();
    }

    // ========================================================================

    #region properties

    public int Size { get; set; }

    /// <summary>
    /// Returns true if the heap has one or more items.
    /// </summary>
    public virtual bool NotEmpty => Size > 0;

    /// <summary>
    /// Returns true if the heap is empty.
    /// </summary>
    public virtual bool IsEmpty => Size == 0;

    #endregion properties
}
