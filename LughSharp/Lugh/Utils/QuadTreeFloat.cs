// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Utils.Pooling;

namespace LughSharp.Lugh.Utils;

/// <summary>
/// A quad tree that stores a float for each point.
/// </summary>
[PublicAPI]
public class QuadTreeFloat : IResetable
{
    public const int VALUE   = 0;
    public const int XPOS    = 1;
    public const int YPOS    = 2;
    public const int DISTSQR = 3;

    public int            MaxValues { get; }
    public int            MaxDepth  { get; }
    public float          X         { get; set; }
    public float          Y         { get; set; }
    public float          Width     { get; set; }
    public float          Height    { get; set; }
    public int            Depth     { get; set; }
    public QuadTreeFloat? Nw        { get; set; }
    public QuadTreeFloat? Ne        { get; set; }
    public QuadTreeFloat? Sw        { get; set; }
    public QuadTreeFloat? Se        { get; set; }

    // For each entry, stores the value, x, and y.
    public List< float > Values { get; set; }

    // The number of elements stored in 'values' (3 values per quad tree entry).
    public int Count { get; set; }

    private readonly Pool< QuadTreeFloat > _pool = new( 128, 4096 );

    // ========================================================================

    /// <summary>
    /// Creates a quad tree with 16 for maxValues and 8 for maxDepth.
    /// </summary>
    public QuadTreeFloat() : this( 16, 8 )
    {
    }

    /// <summary>
    /// Creates a quad tree with provided values for maxValues and maxDepth.
    /// </summary>
    /// <param name="maxValues">
    /// The maximum number of values stored in each quad tree node. When exceeded,
    /// the node is split into 4 child nodes. If the maxDepth has been reached,
    /// more than maxValues may be stored.
    /// </param>
    /// <param name="maxDepth">
    /// The maximum depth of the tree nodes. Nodes at the maxDepth will not be
    /// split and may store more than maxValues number of entries.
    /// </param>
    public QuadTreeFloat( int maxValues, int maxDepth )
    {
        MaxValues = maxValues * 3;
        MaxDepth  = maxDepth;
        Values    = new List< float >( MaxValues );

        _pool.NewObject = GetNewObject;
    }

    public void Reset()
    {
        if ( Count == -1 )
        {
            if ( Nw != null )
            {
                _pool.Free( Nw );
                Nw = null;
            }

            if ( Sw != null )
            {
                _pool.Free( Sw );
                Sw = null;
            }

            if ( Ne != null )
            {
                _pool.Free( Ne );
                Ne = null;
            }

            if ( Se != null )
            {
                _pool.Free( Se );
                Se = null;
            }
        }

        Count = 0;

        if ( Values.Count > MaxValues )
        {
            Values = new List< float >( MaxValues );
        }
    }

    public void SetBounds( float x, float y, float width, float height )
    {
        X      = x;
        Y      = y;
        Width  = width;
        Height = height;
    }

    public void Add( float value, float valueX, float valueY )
    {
        var count = Count;

        if ( count == -1 )
        {
            AddToChild( value, valueX, valueY );

            return;
        }

        if ( Depth < MaxDepth )
        {
            if ( count == MaxValues )
            {
                Split( value, valueX, valueY );

                return;
            }
        }
        else if ( count == Values.Count )
        {
            Values.EnsureCapacity( GrowValues() );
        }

        Values[ count ]     = value;
        Values[ count + 1 ] = valueX;
        Values[ count + 2 ] = valueY;

        Count += 3;
    }

    private void Split( float value, float valueX, float valueY )
    {
        for ( var i = 0; i < MaxValues; i += 3 )
        {
            AddToChild( Values[ i ], Values[ i + 1 ], Values[ i + 2 ] );
        }

        // values isn't nulled because the trees are pooled.
        Count = -1;

        AddToChild( value, valueX, valueY );
    }

    private void AddToChild( float value, float valueX, float valueY )
    {
        QuadTreeFloat? child;
        var            halfWidth  = Width / 2;
        var            halfHeight = Height / 2;

        if ( valueX < ( X + halfWidth ) )
        {
            if ( valueY < ( Y + halfHeight ) )
            {
                child = Sw ??= ObtainChild( X, Y, halfWidth, halfHeight, Depth + 1 );
            }
            else
            {
                child = Nw ??= ObtainChild( X, Y + halfHeight, halfWidth, halfHeight, Depth + 1 );
            }
        }
        else
        {
            if ( valueY < ( Y + halfHeight ) )
            {
                child = Se ??= ObtainChild( X + halfWidth, Y, halfWidth, halfHeight, Depth + 1 );
            }
            else
            {
                child = Ne ??= ObtainChild( X + halfWidth, Y + halfHeight, halfWidth, halfHeight, Depth + 1 );
            }
        }

        child?.Add( value, valueX, valueY );
    }

    private QuadTreeFloat? ObtainChild( float x, float y, float width, float height, int depth )
    {
        var child = _pool.Obtain();

        if ( child != null )
        {
            child.X      = x;
            child.Y      = y;
            child.Width  = width;
            child.Height = height;
            child.Depth  = depth;
        }

        return child;
    }

    /// <summary>
    /// Returns a new length for <see cref="Values"/> when it is not enough to
    /// hold all the entries after <see cref="MaxDepth"/> has been reached.
    /// </summary>
    protected int GrowValues()
    {
        return Count + ( 10 * 3 ); //TODO: No magic numbers!
    }

    /// <summary>
    /// </summary>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <param name="radius"></param>
    /// <param name="results">
    /// For each entry found within the radius, if any, the value, x, y, and
    /// square of the distance to the entry are added to this array.
    /// </param>
    public void Query( float centerX, float centerY, float radius, List< float > results )
    {
        Query(
              centerX,
              centerY,
              radius * radius,
              centerX - radius,
              centerY - radius,
              radius * 2,
              results
             );
    }

    private void Query( float centerX,
                        float centerY,
                        float radiusSqr,
                        float rectX,
                        float rectY,
                        float rectSize,
                        List< float > results )
    {
        if ( !( ( X < ( rectX + rectSize ) )
             && ( ( X + Width ) > rectX )
             && ( Y < ( rectY + rectSize ) )
             && ( ( Y + Height ) > rectY ) ) )
        {
            return;
        }

        var count = Count;

        if ( count != -1 )
        {
            List< float > values = Values;

            for ( var i = 1; i < count; i += 3 )
            {
                var px = values[ i ];
                var py = values[ i + 1 ];
                var dx = px - centerX;
                var dy = py - centerY;
                var d  = ( dx * dx ) + ( dy * dy );

                if ( d <= radiusSqr )
                {
                    results.Add( values[ i - 1 ] );
                    results.Add( px );
                    results.Add( py );
                    results.Add( d );
                }
            }
        }
        else
        {
            Nw?.Query( centerX, centerY, radiusSqr, rectX, rectY, rectSize, results );
            Sw?.Query( centerX, centerY, radiusSqr, rectX, rectY, rectSize, results );
            Ne?.Query( centerX, centerY, radiusSqr, rectX, rectY, rectSize, results );
            Se?.Query( centerX, centerY, radiusSqr, rectX, rectY, rectSize, results );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="result">
    /// For the entry nearest to the specified point, the value, x, y, and square
    /// of the distance to the value are added to this array after it is cleared.
    /// </param>
    /// <returns>
    /// False if no entry was found because the quad tree was empty or the specified
    /// point is farther than the larger of the quad tree's width or height from an
    /// entry. If false is returned the result array is empty.
    /// </returns>
    public bool Nearest( float x, float y, List< float > result )
    {
        // Find nearest value in a cell that contains the point.
        result.Clear();
        result.Add( 0 );
        result.Add( 0 );
        result.Add( 0 );
        result.Add( float.PositiveInfinity );

        FindNearestInternal( x, y, result );

        var nearValue = result.First();
        var nearX     = result[ 1 ];
        var nearY     = result[ 2 ];
        var nearDist  = result[ 3 ];

        var found = !float.IsPositiveInfinity( nearDist );

        if ( !found )
        {
            nearDist =  Math.Max( Width, Height );
            nearDist *= nearDist;
        }

        // Check for a nearer value in a neighboring cell.
        result.Clear();
        Query( x, y, ( float ) Math.Sqrt( nearDist ), result );

        for ( int i = 3, n = result.Count; i < n; i += 4 )
        {
            var dist = result[ i ];

            if ( dist < nearDist )
            {
                nearDist  = dist;
                nearValue = result[ i - 3 ];
                nearX     = result[ i - 2 ];
                nearY     = result[ i - 1 ];
            }
        }

        if ( !found && ( result.Count == 0 ) )
        {
            return false;
        }

        result.Clear();
        result.Add( nearValue );
        result.Add( nearX );
        result.Add( nearY );
        result.Add( nearDist );

        return true;
    }

    private void FindNearestInternal( float x, float y, List< float > result )
    {
        if ( !( ( X < x )
             && ( ( X + Width ) > x )
             && ( Y < y )
             && ( ( Y + Height ) > y ) ) )
        {
            return;
        }

        var count = Count;

        if ( count != -1 )
        {
            var nearValue = result.First();
            var nearX     = result[ 1 ];
            var nearY     = result[ 2 ];
            var nearDist  = result[ 3 ];

            List< float > values = Values;

            for ( var i = 1; i < count; i += 3 )
            {
                float px   = values[ i ], py = values[ i + 1 ];
                float dx   = px - x,      dy = py - y;
                var   dist = ( dx * dx ) + ( dy * dy );

                if ( dist < nearDist )
                {
                    nearDist  = dist;
                    nearValue = values[ i - 1 ];
                    nearX     = px;
                    nearY     = py;
                }
            }

            result[ 0 ] = nearValue;
            result[ 1 ] = nearX;
            result[ 2 ] = nearY;
            result[ 3 ] = nearDist;
        }
        else
        {
            Nw?.FindNearestInternal( x, y, result );
            Sw?.FindNearestInternal( x, y, result );
            Ne?.FindNearestInternal( x, y, result );
            Se?.FindNearestInternal( x, y, result );
        }
    }

    public static QuadTreeFloat GetNewObject()
    {
        return new QuadTreeFloat();
    }
}
