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

namespace LibGDXSharp.Maths;

[PublicAPI]
[PublicAPI]
public class Polyline : IShape2D
{
    private float[]? _worldVertices;
    private float    _length;
    private float    _scaledLength;
    private bool     _calculateScaledLength = true;
    private bool     _calculateLength       = true;
    private bool     _dirty                 = true;

    public Polyline()
    {
        this.LocalVertices = Array.Empty< float >();
    }

    public Polyline( float[] vertices )
    {
        if ( vertices.Length < 4 )
        {
            throw new ArgumentException( "polylines must contain at least 2 points." );
        }

        this.LocalVertices = vertices;
    }

    /// <summary>
    /// Returns vertices scaled, rotated, and offset by the polygon position.
    /// </summary>
    public float[]? GetTransformedVertices()
    {
        if ( !_dirty ) return _worldVertices;

        _dirty = false;

        var localVertices = this.LocalVertices;

        if ( ( _worldVertices == null ) || ( _worldVertices.Length < localVertices.Length ) )
        {
            _worldVertices = new float[ localVertices.Length ];
        }

        var worldVertices = this._worldVertices;
        var positionX     = X;
        var positionY     = Y;
        var originX       = this.OriginX;
        var originY       = this.OriginY;
        var scaleX        = this.ScaleX;
        var scaleY        = this.ScaleY;
        var scale         = scaleX is not 1 || scaleY is not 1;
        var rotation      = this.Rotation;
        var cos           = MathUtils.CosDeg( rotation );
        var sin           = MathUtils.SinDeg( rotation );

        for ( int i = 0, n = localVertices.Length; i < n; i += 2 )
        {
            var x = localVertices[ i ] - originX;
            var y = localVertices[ i + 1 ] - originY;

            // scale if needed
            if ( scale )
            {
                x *= scaleX;
                y *= scaleY;
            }

            // rotate if needed
            if ( rotation != 0 )
            {
                var oldX = x;
                x = ( cos * x ) - ( sin * y );
                y = ( sin * oldX ) + ( cos * y );
            }

            worldVertices[ i ]     = positionX + x + originX;
            worldVertices[ i + 1 ] = positionY + y + originY;
        }

        return worldVertices;
    }

    /** Returns the euclidean length of the polyline without scaling */
    public float GetLength()
    {
        if ( !_calculateLength ) return _length;

        _calculateLength = false;

        _length = 0;

        for ( int i = 0, n = LocalVertices.Length - 2; i < n; i += 2 )
        {
            var x = LocalVertices[ i + 2 ] - LocalVertices[ i ];
            var y = LocalVertices[ i + 1 ] - LocalVertices[ i + 3 ];
            _length += ( float )Math.Sqrt( ( x * x ) + ( y * y ) );
        }

        return _length;
    }

    /** Returns the euclidean length of the polyline */
    public float GetScaledLength()
    {
        if ( !_calculateScaledLength ) return _scaledLength;

        _calculateScaledLength = false;

        _scaledLength = 0;

        for ( int i = 0, n = LocalVertices.Length - 2; i < n; i += 2 )
        {
            var x = ( LocalVertices[ i + 2 ] * ScaleX ) - ( LocalVertices[ i ] * ScaleX );
            var y = ( LocalVertices[ i + 1 ] * ScaleY ) - ( LocalVertices[ i + 3 ] * ScaleY );
            _scaledLength += ( float )Math.Sqrt( ( x * x ) + ( y * y ) );
        }

        return _scaledLength;
    }

    /// <summary>
    /// Returns vertices without scaling or rotation and without
    /// being offset by the polyline position.
    /// </summary>
    public float[] LocalVertices { get; private set; }

    public float X { get; private set; }

    public float Y { get; private set; }

    public float OriginX { get; private set; }

    public float OriginY { get; private set; }

    public float Rotation { get; private set; }

    public float ScaleX { get; private set; } = 1;

    public float ScaleY { get; private set; } = 1;

    public void SetOrigin( float originX, float originY )
    {
        this.OriginX = originX;
        this.OriginY = originY;
        _dirty       = true;
    }

    public void SetPosition( float x, float y )
    {
        this.X = x;
        this.Y = y;
        _dirty = true;
    }

    public void SetVertices( float[] vertices )
    {
        if ( vertices.Length < 4 )
        {
            throw new ArgumentException( "polylines must contain at least 2 points." );
        }

        this.LocalVertices = vertices;
        _dirty             = true;
    }

    public void SetRotation( float degrees )
    {
        this.Rotation = degrees;
        _dirty        = true;
    }

    public void Rotate( float degrees )
    {
        Rotation += degrees;
        _dirty   =  true;
    }

    public void SetScale( float scaleX, float scaleY )
    {
        this.ScaleX            = scaleX;
        this.ScaleY            = scaleY;
        _dirty                 = true;
        _calculateScaledLength = true;
    }

    public void Scale( float amount )
    {
        this.ScaleX            += amount;
        this.ScaleY            += amount;
        _dirty                 =  true;
        _calculateScaledLength =  true;
    }

    public void SetCalculateLength()       => _calculateLength = true;
    public void SetCalculateScaledLength() => _calculateScaledLength = true;
    public void SetDirty()                 => _dirty = true;

    public void Translate( float x, float y )
    {
        this.X += x;
        this.Y += y;
        _dirty =  true;
    }

    public bool Contains( Vector2 point )
    {
        return false;
    }

    public bool Contains( float x, float y )
    {
        return false;
    }
}