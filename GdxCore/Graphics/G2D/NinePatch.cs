using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Maths;

namespace LibGDXSharp.G2D;

/// <summary>
/// A 3x3 grid of texture regions. Any of the regions may be omitted.
/// Padding may be set as a hint on how to inset content on top of the ninepatch
/// (by default the eight "edge" textures of the ninepatch define the padding).
/// When drawn, the four corner patches will not be scaled, the interior patch will
/// be scaled in both directions, and the middle patch for each edge will be scaled
/// in only one direction.
/// <para>
/// Note this class does not accept ".9.png" textures that include the metadata
/// border pixels describing the splits (and padding) for the ninepatch. That
/// information is either passed to a constructor or defined implicitly by the
/// size of the individual patch textures. TextureAtlas is one way to generate
/// a postprocessed ninepatch texture regions from ".9.png" files.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class NinePatch
{
    private const float Tolerance = 0.1f;

    public const int Top_Left      = 0;
    public const int Top_Center    = 1;
    public const int Top_Right     = 2;
    public const int Middle_Left   = 3;
    public const int Middle_Center = 4;
    public const int Middle_Right  = 5;
    public const int Bottom_Left   = 6;
    public const int Bottom_Center = 7;
    public const int Bottom_Right  = 8;

    public float[] Vertices { get; set; } = new float[ 9 * 4 * 5 ];
    public Color   Color    { get; set; } = new(Color.White);

    public Texture Texture      { get; set; }
    public int     Idx          { get; set; }
    public int     BottomLeft   { get; set; }
    public int     BottomCenter { get; set; }
    public int     BottomRight  { get; set; }
    public int     MiddleLeft   { get; set; }
    public int     MiddleCenter { get; set; }
    public int     MiddleRight  { get; set; }
    public int     TopLeft      { get; set; }
    public int     TopCenter    { get; set; }
    public int     TopRight     { get; set; }
    public float   LeftWidth    { get; set; }
    public float   RightWidth   { get; set; }
    public float   MiddleWidth  { get; set; }
    public float   MiddleHeight { get; set; }
    public float   TopHeight    { get; set; }
    public float   BottomHeight { get; set; }

    private readonly static Color tmpDrawColor = new();

    /// <summary>
    /// Create a ninepatch by cutting up the given texture into nine patches.
    /// The subsequent parameters define the 4 lines that will cut the texture
    /// region into 9 pieces.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="left">Pixels from left edge.</param>
    /// <param name="right">Pixels from right edge.</param>
    /// <param name="top">Pixels from top edge.</param>
    /// <param name="bottom">Pixels from bottom edge.</param>
    public NinePatch( Texture texture, int left, int right, int top, int bottom )
        : this( new TextureRegion( texture ), left, right, top, bottom )
    {
    }

    /// <summary>
    /// Create a ninepatch by cutting up the given texture region into nine patches.
    /// The subsequent parameters define the 4 lines that will cut the texture region
    /// into 9 pieces.
    /// </summary>
    /// <param name="region"></param>
    /// <param name="left"> Pixels from left edge. </param>
    /// <param name="right"> Pixels from right edge. </param>
    /// <param name="top"> Pixels from top edge. </param>
    /// <param name="bottom"> Pixels from bottom edge.  </param>
    public NinePatch( TextureRegion region, int left, int right, int top, int bottom )
    {
        if ( region == null )
        {
            throw new System.ArgumentException( "region cannot be null." );
        }

        var middleWidth  = region.RegionWidth - left - right;
        var middleHeight = region.RegionHeight - top - bottom;

        var patches = new TextureRegion?[ 9 ];

        if ( top > 0 )
        {
            if ( left > 0 )
            {
                patches[ Top_Left ]
                    = new TextureRegion( region, 0, 0, left, top );
            }

            if ( middleWidth > 0 )
            {
                patches[ Top_Center ]
                    = new TextureRegion( region, left, 0, middleWidth, top );
            }

            if ( right > 0 )
            {
                patches[ Top_Right ]
                    = new TextureRegion( region, left + middleWidth, 0, right, top );
            }
        }

        if ( middleHeight > 0 )
        {
            if ( left > 0 )
            {
                patches[ Middle_Left ]
                    = new TextureRegion( region, 0, top, left, middleHeight );
            }

            if ( middleWidth > 0 )
            {
                patches[ Middle_Center ]
                    = new TextureRegion( region, left, top, middleWidth, middleHeight );
            }

            if ( right > 0 )
            {
                patches[ Middle_Right ]
                    = new TextureRegion( region, left + middleWidth, top, right, middleHeight );
            }
        }

        if ( bottom > 0 )
        {
            if ( left > 0 )
            {
                patches[ Bottom_Left ]
                    = new TextureRegion( region, 0, top + middleHeight, left, bottom );
            }

            if ( middleWidth > 0 )
            {
                patches[ Bottom_Center ]
                    = new TextureRegion( region, left, top + middleHeight, middleWidth, bottom );
            }

            if ( right > 0 )
            {
                patches[ Bottom_Right ]
                    = new TextureRegion
                        (
                         region,
                         left + middleWidth,
                         top + middleHeight,
                         right,
                         bottom
                        );
            }
        }

        // If split only vertical, move splits from right to center.
        if ( ( left == 0 ) && ( middleWidth == 0 ) )
        {
            patches[ Top_Center ]    = patches[ Top_Right ];
            patches[ Middle_Center ] = patches[ Middle_Right ];
            patches[ Bottom_Center ] = patches[ Bottom_Right ];
            patches[ Top_Right ]     = null;
            patches[ Middle_Right ]  = null;
            patches[ Bottom_Right ]  = null;
        }

        // If split only horizontal, move splits from bottom to center.
        if ( ( top == 0 ) && ( middleHeight == 0 ) )
        {
            patches[ Middle_Left ]   = patches[ Bottom_Left ];
            patches[ Middle_Center ] = patches[ Bottom_Center ];
            patches[ Middle_Right ]  = patches[ Bottom_Right ];
            patches[ Bottom_Left ]   = null;
            patches[ Bottom_Center ] = null;
            patches[ Bottom_Right ]  = null;
        }

        Load( patches );
    }

    /// <summary>
    /// Construct a degenerate "nine" patch with only a center component.
    /// </summary>
    public NinePatch( Texture texture, Color color )
        : this( texture )
    {
        this.Color = color;
    }

    /// Construct a degenerate "nine" patch with only a center component.
    public NinePatch( Texture texture )
        : this( new TextureRegion( texture ) )
    {
    }

    /// <summary>
    /// Construct a degenerate "nine" patch with only a center component.
    /// </summary>
    public NinePatch( TextureRegion region, Color color )
        : this( region )
    {
        this.Color = color;
    }

    /// <summary>
    /// Construct a degenerate "nine" patch with only a center component.
    /// </summary>
    public NinePatch( TextureRegion region )
    {
        //@formatter:off
        Load( new[]
             {
                 null, null, null,
                 null, region, null,
                 null, null, null 
             } );
        //@formatter:on
    }

    /// <summary>
    /// Construct a nine patch from the given nine texture regions. The
    /// provided patches must be consistently sized (e.g., any left
    /// edge textures must have the same width, etc). Patches may be
    /// <tt>null</tt>. Patch indices are specified via the public
    /// members <see cref="Top_Left"/>, <see cref="Top_Center"/>, etc. 
    /// </summary>
    public NinePatch( params TextureRegion[] patches )
    {
        if ( patches is not { Length: 9 } )
        {
            throw new System.ArgumentException( "NinePatch needs nine TextureRegions" );
        }

        Load( patches );

        if ( ( ( patches[ Top_Left ] != null )
               && ( Math.Abs( patches[ Top_Left ].RegionWidth - LeftWidth ) > Tolerance ) )
             || ( ( patches[ Middle_Left ] != null )
                  && ( Math.Abs( patches[ Middle_Left ].RegionWidth - LeftWidth ) > Tolerance ) )
             || ( ( patches[ Bottom_Left ] != null )
                  && ( Math.Abs( patches[ Bottom_Left ].RegionWidth - LeftWidth ) > Tolerance ) ) )
        {
            throw new GdxRuntimeException( "Left side patches must have the same width" );
        }

        if ( ( ( patches[ Top_Right ] != null )
               && ( Math.Abs( patches[ Top_Right ].RegionWidth - RightWidth ) > Tolerance ) )
             || ( ( patches[ Middle_Right ] != null )
                  && ( Math.Abs( patches[ Middle_Right ].RegionWidth - RightWidth ) > Tolerance ) )
             || ( ( patches[ Bottom_Right ] != null )
                  && ( Math.Abs( patches[ Bottom_Right ].RegionWidth - RightWidth ) > Tolerance ) ) )
        {
            throw new GdxRuntimeException( "Right side patches must have the same width" );
        }

        if ( ( ( patches[ Bottom_Left ] != null )
               && ( Math.Abs( patches[ Bottom_Left ].RegionHeight - BottomHeight ) > Tolerance ) )
             || ( ( patches[ Bottom_Center ] != null )
                  && ( Math.Abs( patches[ Bottom_Center ].RegionHeight - BottomHeight ) > Tolerance ) )
             || ( ( patches[ Bottom_Right ] != null )
                  && ( Math.Abs( patches[ Bottom_Right ].RegionHeight - BottomHeight ) > Tolerance ) ) )
        {
            throw new GdxRuntimeException( "Bottom side patches must have the same height" );
        }

        if ( ( ( patches[ Top_Left ] != null )
               && ( Math.Abs( patches[ Top_Left ].RegionHeight - TopHeight ) > Tolerance ) )
             || ( ( patches[ Top_Center ] != null )
                  && ( Math.Abs( patches[ Top_Center ].RegionHeight - TopHeight ) > Tolerance ) )
             || ( ( patches[ Top_Right ] != null )
                  && ( Math.Abs( patches[ Top_Right ].RegionHeight - TopHeight ) > Tolerance ) ) )
        {
            throw new GdxRuntimeException( "Top side patches must have the same height" );
        }
    }

    public NinePatch( NinePatch ninePatch )
        : this( ninePatch, ninePatch.Color )
    {
    }

    /// <summary>
    /// Creates a ninepatch from the supplied ninepatch and sets
    /// it to the supplied Color. 
    /// </summary>
    /// <param name="ninePatch"></param>
    /// <param name="color"></param>
    public NinePatch( NinePatch ninePatch, Color color )
    {
        Texture = ninePatch.Texture;

        BottomLeft   = ninePatch.BottomLeft;
        BottomCenter = ninePatch.BottomCenter;
        BottomRight  = ninePatch.BottomRight;
        MiddleLeft   = ninePatch.MiddleLeft;
        MiddleCenter = ninePatch.MiddleCenter;
        MiddleRight  = ninePatch.MiddleRight;
        TopLeft      = ninePatch.TopLeft;
        TopCenter    = ninePatch.TopCenter;
        TopRight     = ninePatch.TopRight;

        LeftWidth    = ninePatch.LeftWidth;
        RightWidth   = ninePatch.RightWidth;
        MiddleWidth  = ninePatch.MiddleWidth;
        MiddleHeight = ninePatch.MiddleHeight;
        TopHeight    = ninePatch.TopHeight;
        BottomHeight = ninePatch.BottomHeight;

        _padLeft   = ninePatch._padLeft;
        _padTop    = ninePatch._padTop;
        _padBottom = ninePatch._padBottom;
        _padRight  = ninePatch._padRight;

        Vertices = new float[ ninePatch.Vertices.Length ];

        Array.Copy
            (
             ninePatch.Vertices,
             0,
             Vertices,
             0,
             ninePatch.Vertices.Length
            );

        Idx = ninePatch.Idx;
        this.Color.Set( color );
    }

    /// <summary>
    /// </summary>
    /// <param name="patches"></param>
    private void Load( IReadOnlyList< TextureRegion? > patches )
    {
        if ( patches[ Bottom_Left ] != null )
        {
            BottomLeft   = Add( patches[ Bottom_Left ], false, false );
            LeftWidth    = patches[ Bottom_Left ].RegionWidth;
            BottomHeight = patches[ Bottom_Left ].RegionHeight;
        }
        else
        {
            BottomLeft = -1;
        }

        if ( patches[ Bottom_Center ] != null )
        {
            BottomCenter = Add
                (
                 patches[ Bottom_Center ],
                 ( patches[ Bottom_Left ] != null ) || ( patches[ Bottom_Right ] != null ),
                 false
                );

            MiddleWidth  = Math.Max( MiddleWidth, patches[ Bottom_Center ].RegionWidth );
            BottomHeight = Math.Max( BottomHeight, patches[ Bottom_Center ].RegionHeight );
        }
        else
        {
            BottomCenter = -1;
        }

        if ( patches[ Bottom_Right ] != null )
        {
            BottomRight  = Add( patches[ Bottom_Right ], false, false );
            RightWidth   = Math.Max( RightWidth, patches[ Bottom_Right ].RegionWidth );
            BottomHeight = Math.Max( BottomHeight, patches[ Bottom_Right ].RegionHeight );
        }
        else
        {
            BottomRight = -1;
        }

        if ( patches[ Middle_Left ] != null )
        {
            MiddleLeft = Add
                (
                 patches[ Middle_Left ],
                 false,
                 ( patches[ Top_Left ] != null ) || ( patches[ Bottom_Left ] != null )
                );

            LeftWidth    = Math.Max( LeftWidth, patches[ Middle_Left ].RegionWidth );
            MiddleHeight = Math.Max( MiddleHeight, patches[ Middle_Left ].RegionHeight );
        }
        else
        {
            MiddleLeft = -1;
        }

        if ( patches[ Middle_Center ] != null )
        {
            MiddleCenter = Add
                (
                 patches[ Middle_Center ],
                 ( patches[ Middle_Left ] != null ) || ( patches[ Middle_Right ] != null ),
                 ( patches[ Top_Center ] != null ) || ( patches[ Bottom_Center ] != null )
                );

            MiddleWidth  = Math.Max( MiddleWidth, patches[ Middle_Center ].RegionWidth );
            MiddleHeight = Math.Max( MiddleHeight, patches[ Middle_Center ].RegionHeight );
        }
        else
        {
            MiddleCenter = -1;
        }

        if ( patches[ Middle_Right ] != null )
        {
            MiddleRight = Add
                (
                 patches[ Middle_Right ],
                 false,
                 ( patches[ Top_Right ] != null ) || ( patches[ Bottom_Right ] != null )
                );

            RightWidth   = Math.Max( RightWidth, patches[ Middle_Right ].RegionWidth );
            MiddleHeight = Math.Max( MiddleHeight, patches[ Middle_Right ].RegionHeight );
        }
        else
        {
            MiddleRight = -1;
        }

        if ( patches[ Top_Left ] != null )
        {
            TopLeft   = Add( patches[ Top_Left ], false, false );
            LeftWidth = Math.Max( LeftWidth, patches[ Top_Left ].RegionWidth );
            TopHeight = Math.Max( TopHeight, patches[ Top_Left ].RegionHeight );
        }
        else
        {
            TopLeft = -1;
        }

        if ( patches[ Top_Center ] != null )
        {
            TopCenter = Add
                ( patches[ Top_Center ], ( patches[ Top_Left ] != null ) || ( patches[ Top_Right ] != null ), false );

            MiddleWidth = Math.Max( MiddleWidth, patches[ Top_Center ].RegionWidth );
            TopHeight   = Math.Max( TopHeight, patches[ Top_Center ].RegionHeight );
        }
        else
        {
            TopCenter = -1;
        }

        if ( patches[ Top_Right ] != null )
        {
            TopRight   = Add( patches[ Top_Right ], false, false );
            RightWidth = Math.Max( RightWidth, patches[ Top_Right ].RegionWidth );
            TopHeight  = Math.Max( TopHeight, patches[ Top_Right ].RegionHeight );
        }
        else
        {
            TopRight = -1;
        }

        if ( Idx < Vertices.Length )
        {
            var newVertices = new float[ Idx ];

            Array.Copy( Vertices, 0, newVertices, 0, Idx );
            Vertices = newVertices;
        }
    }

    private void Load( TextureRegion?[] patches )
    {
        if ( patches[ Bottom_Left ] != null )
        {
            BottomLeft   = Add( patches[ Bottom_Left ], false, false );
            LeftWidth    = patches[ Bottom_Left ].RegionWidth;
            BottomHeight = patches[ Bottom_Left ].RegionHeight;
        }
        else
        {
            BottomLeft = -1;
        }

        if ( patches[ Bottom_Center ] != null )
        {
            BottomCenter = Add
                (
                 patches[ Bottom_Center ],
                 ( patches[ Bottom_Left ] != null ) || ( patches[ Bottom_Right ] != null ),
                 false
                );

            MiddleWidth  = Math.Max( MiddleWidth, patches[ Bottom_Center ].RegionWidth );
            BottomHeight = Math.Max( BottomHeight, patches[ Bottom_Center ].RegionHeight );
        }
        else
        {
            BottomCenter = -1;
        }

        if ( patches[ Bottom_Right ] != null )
        {
            BottomRight  = Add( patches[ Bottom_Right ], false, false );
            RightWidth   = Math.Max( RightWidth, patches[ Bottom_Right ].RegionWidth );
            BottomHeight = Math.Max( BottomHeight, patches[ Bottom_Right ].RegionHeight );
        }
        else
        {
            BottomRight = -1;
        }

        if ( patches[ Middle_Left ] != null )
        {
            MiddleLeft = Add
                (
                 patches[ Middle_Left ],
                 false,
                 ( patches[ Top_Left ] != null ) || ( patches[ Bottom_Left ] != null )
                );

            LeftWidth    = Math.Max( LeftWidth, patches[ Middle_Left ].RegionWidth );
            MiddleHeight = Math.Max( MiddleHeight, patches[ Middle_Left ].RegionHeight );
        }
        else
        {
            MiddleLeft = -1;
        }

        if ( patches[ Middle_Center ] != null )
        {
            MiddleCenter = Add
                (
                 patches[ Middle_Center ],
                 ( patches[ Middle_Left ] != null ) || ( patches[ Middle_Right ] != null ),
                 ( patches[ Top_Center ] != null ) || ( patches[ Bottom_Center ] != null )
                );

            MiddleWidth  = Math.Max( MiddleWidth, patches[ Middle_Center ].RegionWidth );
            MiddleHeight = Math.Max( MiddleHeight, patches[ Middle_Center ].RegionHeight );
        }
        else
        {
            MiddleCenter = -1;
        }

        if ( patches[ Middle_Right ] != null )
        {
            MiddleRight = Add
                (
                 patches[ Middle_Right ],
                 false,
                 ( patches[ Top_Right ] != null ) || ( patches[ Bottom_Right ] != null )
                );

            RightWidth   = Math.Max( RightWidth, patches[ Middle_Right ].RegionWidth );
            MiddleHeight = Math.Max( MiddleHeight, patches[ Middle_Right ].RegionHeight );
        }
        else
        {
            MiddleRight = -1;
        }

        if ( patches[ Top_Left ] != null )
        {
            TopLeft   = Add( patches[ Top_Left ], false, false );
            LeftWidth = Math.Max( LeftWidth, patches[ Top_Left ].RegionWidth );
            TopHeight = Math.Max( TopHeight, patches[ Top_Left ].RegionHeight );
        }
        else
        {
            TopLeft = -1;
        }

        if ( patches[ Top_Center ] != null )
        {
            TopCenter = Add
                ( patches[ Top_Center ], ( patches[ Top_Left ] != null ) || ( patches[ Top_Right ] != null ), false );

            MiddleWidth = Math.Max( MiddleWidth, patches[ Top_Center ].RegionWidth );
            TopHeight   = Math.Max( TopHeight, patches[ Top_Center ].RegionHeight );
        }
        else
        {
            TopCenter = -1;
        }

        if ( patches[ Top_Right ] != null )
        {
            TopRight   = Add( patches[ Top_Right ], false, false );
            RightWidth = Math.Max( RightWidth, patches[ Top_Right ].RegionWidth );
            TopHeight  = Math.Max( TopHeight, patches[ Top_Right ].RegionHeight );
        }
        else
        {
            TopRight = -1;
        }

        if ( Idx < Vertices.Length )
        {
            var newVertices = new float[ Idx ];

            Array.Copy
                (
                 Vertices,
                 0,
                 newVertices,
                 0,
                 Idx
                );

            Vertices = newVertices;
        }
    }

    private int Add( TextureRegion region, bool isStretchW, bool isStretchH )
    {
        if ( Texture == null )
        {
            Texture = region.Texture;
        }
        else if ( Texture != region.Texture )
        {
            throw new System.ArgumentException( "All regions must be from the same texture." );
        }

        // Add half pixel offsets on stretchable dimensions to avoid color bleeding when GL_LINEAR
        // filtering is used for the texture. This nudges the texture coordinate to the center
        // of the texel where the neighboring pixel has 0% contribution in linear blending mode.
        var u  = region.U;
        var v  = region.V2;
        var u2 = region.U2;
        var v2 = region.V;

        if ( ( Texture.MagFilter == TextureFilter.Linear ) || ( Texture.MinFilter == TextureFilter.Linear ) )
        {
            if ( isStretchW )
            {
                var halfTexelWidth = ( 0.5f * 1f ) / Texture.Width;
                u  += halfTexelWidth;
                u2 -= halfTexelWidth;
            }

            if ( isStretchH )
            {
                var halfTexelHeight = ( 0.5f * 1f ) / Texture.Height;
                v  -= halfTexelHeight;
                v2 += halfTexelHeight;
            }
        }

        var i = Idx;

        this.Vertices[ i + 3 ] = u;
        this.Vertices[ i + 4 ] = v;

        this.Vertices[ i + 8 ] = u;
        this.Vertices[ i + 9 ] = v2;

        this.Vertices[ i + 13 ] = u2;
        this.Vertices[ i + 14 ] = v2;

        this.Vertices[ i + 18 ] = u2;
        this.Vertices[ i + 19 ] = v;

        Idx += 20;

        return i;
    }

    /// <summary>
    /// Set the coordinates and color of a ninth of the patch. </summary>
    private void Set( int idx, float x, float y, float width, float height, float color )
    {
        var fx2 = x + width;
        var fy2 = y + height;

        this.Vertices[ idx ]     = x;
        this.Vertices[ idx + 1 ] = y;
        this.Vertices[ idx + 2 ] = color;

        this.Vertices[ idx + 5 ] = x;
        this.Vertices[ idx + 6 ] = fy2;
        this.Vertices[ idx + 7 ] = color;

        this.Vertices[ idx + 10 ] = fx2;
        this.Vertices[ idx + 11 ] = fy2;
        this.Vertices[ idx + 12 ] = color;

        this.Vertices[ idx + 15 ] = fx2;
        this.Vertices[ idx + 16 ] = y;
        this.Vertices[ idx + 17 ] = color;
    }

    private void PrepareVertices( IBatch batch, float x, float y, float width, float height )
    {
        var centerX      = x + LeftWidth;
        var centerY      = y + BottomHeight;
        var centerWidth  = width - RightWidth - LeftWidth;
        var centerHeight = height - TopHeight - BottomHeight;
        var rightX       = ( x + width ) - RightWidth;
        var topY         = ( y + height ) - TopHeight;
        var c            = tmpDrawColor.Set( Color ).Mul( batch.GetColor() ).ToFloatBits();

        if ( BottomLeft != -1 )
        {
            Set( BottomLeft, x, y, LeftWidth, BottomHeight, c );
        }

        if ( BottomCenter != -1 )
        {
            Set( BottomCenter, centerX, y, centerWidth, BottomHeight, c );
        }

        if ( BottomRight != -1 )
        {
            Set( BottomRight, rightX, y, RightWidth, BottomHeight, c );
        }

        if ( MiddleLeft != -1 )
        {
            Set( MiddleLeft, x, centerY, LeftWidth, centerHeight, c );
        }

        if ( MiddleCenter != -1 )
        {
            Set( MiddleCenter, centerX, centerY, centerWidth, centerHeight, c );
        }

        if ( MiddleRight != -1 )
        {
            Set( MiddleRight, rightX, centerY, RightWidth, centerHeight, c );
        }

        if ( TopLeft != -1 )
        {
            Set( TopLeft, x, topY, LeftWidth, TopHeight, c );
        }

        if ( TopCenter != -1 )
        {
            Set( TopCenter, centerX, topY, centerWidth, TopHeight, c );
        }

        if ( TopRight != -1 )
        {
            Set( TopRight, rightX, topY, RightWidth, TopHeight, c );
        }
    }

    public void Draw( IBatch batch, float x, float y, float width, float height )
    {
        PrepareVertices( batch, x, y, width, height );
        batch.Draw( Texture, Vertices, 0, Idx );
    }

    public void Draw( IBatch batch,
                      float x,
                      float y,
                      float originX,
                      float originY,
                      float width,
                      float height,
                      float scaleX,
                      float scaleY,
                      float rotation )
    {
        PrepareVertices( batch, x, y, width, height );

        float worldOriginX = x + originX, worldOriginY = y + originY;

        var n        = this.Idx;
        var vertices = this.Vertices;

        if ( rotation != 0 )
        {
            for ( var i = 0; i < n; i += 5 )
            {
                var vx = ( vertices[ i ] - worldOriginX ) * scaleX;
                var vy = ( vertices[ i + 1 ] - worldOriginY ) * scaleY;

                float cos = MathUtils.CosDeg( rotation ), sin = MathUtils.SinDeg( rotation );

                vertices[ i ]     = ( ( cos * vx ) - ( sin * vy ) ) + worldOriginX;
                vertices[ i + 1 ] = ( sin * vx ) + ( cos * vy ) + worldOriginY;
            }
        }
        else if ( ( scaleX != 1 ) || ( scaleY != 1 ) )
        {
            for ( var i = 0; i < n; i += 5 )
            {
                vertices[ i ]     = ( ( vertices[ i ] - worldOriginX ) * scaleX ) + worldOriginX;
                vertices[ i + 1 ] = ( ( vertices[ i + 1 ] - worldOriginY ) * scaleY ) + worldOriginY;
            }
        }

        batch.Draw( this.Texture, vertices, 0, n );
    }

    public float TotalWidth => ( LeftWidth + MiddleWidth + RightWidth );

    public float TotalHeight => ( TopHeight + MiddleHeight + BottomHeight );

    /// <summary>
    /// Set the padding for content inside this ninepatch. By default the padding
    /// is set to match the exterior of the ninepatch, so the content should fit
    /// exactly within the middle patch. 
    /// </summary>
    public void SetPadding( float left, float right, float top, float bottom )
    {
        this.PadLeft   = left;
        this.PadRight  = right;
        this.PadTop    = top;
        this.PadBottom = bottom;
    }

    private float _padLeft   = -1;
    private float _padRight  = -1;
    private float _padTop    = -1;
    private float _padBottom = -1;

    public float PadLeft
    {
        get => _padLeft <= 0.0001f ? LeftWidth : _padLeft;
        set => _padLeft = value;
    }

    public float PadRight
    {
        get => _padRight <= 0.0001f ? RightWidth : _padRight;
        set => _padRight = value;
    }

    public float PadTop
    {
        get => _padTop <= 0.0001f ? TopHeight : _padTop;
        set => _padTop = value;
    }

    public float PadBottom
    {
        get => _padBottom <= 0.0001f ? BottomHeight : _padBottom;
        set => _padBottom = value;
    }

    /// <summary>
    /// Multiplies the top/left/bottom/right sizes and padding by the specified amount.
    /// </summary>
    public void Scale( float scaleX, float scaleY )
    {
        LeftWidth    *= scaleX;
        RightWidth   *= scaleX;
        TopHeight    *= scaleY;
        BottomHeight *= scaleY;
        MiddleWidth  *= scaleX;
        MiddleHeight *= scaleY;

        if ( PadLeft != -1 )
        {
            PadLeft *= scaleX;
        }

        if ( PadRight != -1 )
        {
            PadRight *= scaleX;
        }

        if ( PadTop != -1 )
        {
            PadTop *= scaleY;
        }

        if ( PadBottom != -1 )
        {
            PadBottom *= scaleY;
        }
    }
}