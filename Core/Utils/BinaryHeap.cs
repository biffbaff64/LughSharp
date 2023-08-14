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

using System.Text;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Utils;

/// <summary>
/// Node record holding a value and its index.
/// To change the contents of <see cref="Value"/> use <see cref="BinaryHeap{T}.Add(ref T, float)"/>
/// if the node is NOT in the heap, otherwise use <see cref="BinaryHeap{T}.SetValue(ref T, float)"/>.
/// </summary>
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
public class BinaryHeap<T> where T : BinaryHeapNode
{
    public int Size { get; set; }

    private          BinaryHeapNode[] _nodes;
    private readonly bool             _isMaxHeap;

    public BinaryHeap( int capacity = 16, bool isMaxHeap = false )
    {
        this._isMaxHeap = isMaxHeap;
        this._nodes     = new BinaryHeapNode[ capacity ];
    }

    /// <summary>
    /// Adds the node to the heap using its current value.
    /// The node should not already be in the heap.
    /// </summary>
    /// <returns>The specified node.</returns>
    public T Add( ref T node )
    {
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
    public T Add( ref T node, float value )
    {
        node.Value = value;

        return Add( ref node );
    }

    /// <summary>
    /// Returns true if the heap contains the specified node.
    /// </summary>
    /// <param name="node">The <see cref="BinaryHeapNode"/>to look for.</param>
    /// <param name="identity">
    /// If true, == comparison will be used. If false, .Equals() comparison will be used.
    /// </param>
    public bool Contains( T node, bool identity )
    {
        ArgumentNullException.ThrowIfNull( node );

        foreach ( BinaryHeapNode n in _nodes )
        {
            if ( ( identity && ( n == node ) )
              || ( !identity && ( n.Equals( node ) ) ) )
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
        if ( Size == 0 )
        {
            throw new System.InvalidOperationException( "The heap is empty." );
        }

        return ( T )_nodes[ 0 ];
    }

    /// <summary>
    /// Removes the first item in the heap and returns it. This is
    /// the item with the lowest value (or highest value if this heap
    /// is configured as a max heap). 
    /// </summary>
    public virtual T Pop()
    {
        BinaryHeapNode removed = _nodes[ 0 ];

        if ( --Size > 0 )
        {
            _nodes[ 0 ]    = _nodes[ Size ];
            _nodes[ Size ] = null;

            Down( 0 );
        }
        else
        {
            _nodes[ 0 ] = null;
        }

        return ( T )removed;
    }

    /// <returns> The specified node. </returns>
    public virtual T Remove( T node )
    {
        if ( --Size > 0 )
        {
            BinaryHeapNode moved = _nodes[ Size ];
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
    /// Returns true if the heap has one or more items.
    /// </summary>
    public bool NotEmpty()
    {
        return Size > 0;
    }

    /// <summary>
    /// Returns true if the heap is empty.
    /// </summary>
    public bool IsEmpty()
    {
        return Size == 0;
    }

    public void Clear()
    {
        Array.Fill( _nodes, null, 0, Size );
        Size = 0;
    }

    /// <summary>
    /// Changes the value of the node, which should already be in the heap.
    /// </summary>
    public void SetValue( ref T node, float value )
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

    private void Up( int index )
    {
        var ix = index;

        while ( ix > 0 )
        {
            var parentIndex = ( ix - 1 ) >> 1;

            BinaryHeapNode parent = _nodes[ parentIndex ];

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

    private void Down( int index )
    {
        BinaryHeapNode node = this._nodes[ index ];

        var size  = this.Size;
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
            BinaryHeapNode leftNode  = this._nodes[ leftIndex ];
            var            leftValue = leftNode.Value;

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
                rightNode  = this._nodes[ rightIndex ];
                rightValue = rightNode.Value;
            }

            // The smallest of the three values is the parent.
            if ( ( leftValue < rightValue ) ^ _isMaxHeap )
            {
                if ( leftValue.Equals( value ) || ( ( leftValue > value ) ^ _isMaxHeap ) )
                {
                    break;
                }

                this._nodes[ index ] = leftNode;
                leftNode.Index       = index;
                index                = leftIndex;
            }
            else
            {
                if ( rightValue.Equals( value ) || ( ( rightValue > value ) ^ _isMaxHeap ) )
                {
                    break;
                }

                this._nodes[ index ] = rightNode!;

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

    public override bool Equals( object obj )
    {
        if ( obj is not BinaryHeap< T > other )
        {
            return false;
        }

        if ( other.Size != Size )
        {
            return false;
        }

        BinaryHeapNode[] nodes1 = this._nodes;
        BinaryHeapNode[] nodes2 = other._nodes;

        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( !nodes1[ i ].Value.Equals( nodes2[ i ].Value ) )
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var h = 1;

        BinaryHeapNode[] nodes = this._nodes;

        for ( int i = 0, n = Size; i < n; i++ )
        {
            h = ( h * 31 ) + NumberUtils.FloatToIntBits( nodes[ i ].Value );
        }

        return h;
    }

    public new string ToString()
    {
        if ( Size == 0 )
        {
            return "[]";
        }

        BinaryHeapNode[] nodes  = this._nodes;
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
}
