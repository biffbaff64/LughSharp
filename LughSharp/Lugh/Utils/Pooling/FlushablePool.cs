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


namespace LughSharp.Lugh.Utils.Pooling;

/// <summary>
/// A <see cref="Pool{T}"/> which keeps track of the items obtained by calling
/// (see <see cref="Obtain()"/>), which can be freed all at once using the
/// <see cref="Flush()"/> method.
/// </summary>
[PublicAPI]
public abstract class FlushablePool< T > : Pool< T >
{
    private readonly List< T > _obtained = [ ];

    // ========================================================================
    
    /// <inheritdoc />
    protected FlushablePool()
    {
    }

    /// <inheritdoc />
    protected FlushablePool( int initialCapacity )
        : base( initialCapacity )
    {
    }

    /// <inheritdoc />
    protected FlushablePool( int initialCapacity, int max )
        : base( initialCapacity, max )
    {
    }

    /// <inheritdoc />
    public override T? Obtain()
    {
        var result = base.Obtain();

        _obtained.Add( result! );

        return result;
    }

    /// <summary>
    /// Frees all obtained instances.
    /// </summary>
    public virtual void Flush()
    {
        FreeAll( _obtained );
        
        _obtained.Clear();
    }

    /// <inheritdoc />
    public override void Free( T obj )
    {
        _obtained.Remove( obj );

        base.Free( obj );
    }

    /// <inheritdoc />
    public override void FreeAll( List< T > objects )
    {
        foreach ( var obj in objects )
        {
            _obtained.Remove( obj );
        }

        base.FreeAll( objects );
    }
}
