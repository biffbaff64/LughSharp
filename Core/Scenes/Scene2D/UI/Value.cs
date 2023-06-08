using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using LibGDXSharp.Scenes.Scene2D;
using LibGDXSharp.Scenes.Scene2D.UI;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// Value placeholder, allowing the value to be computed on request.
/// Values can be provided an actor for context to reduce the number of value
/// instances that need to be created and reduce verbosity in code that specifies
/// values.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract partial class Value
{
    public abstract float Get( Actor? context = null );

    public readonly static Fixed Zero = new(0);

    /// <summary>
    /// A fixed value that is not computed each time it is used.
    /// </summary>
    public sealed class Fixed
    {
        private readonly static Fixed?[] cache = new Fixed[ 111 ];

        public float Value { get; }

        public Fixed( float value )
        {
            this.Value = value;
        }

        public float Get( Actor? context )
        {
            return Value;
        }

        public new string ToString()
        {
            return Value.ToString( CultureInfo.InvariantCulture );
        }

        public Fixed ValueOf( float value )
        {
            if ( value == 0 ) return Zero;

            if ( value is >= -10 and <= 100 && value.Equals( ( int )value ) )
            {
                Fixed? f = cache[ ( int )value + 10 ];

                if ( f == null )
                {
                    cache[ ( int )value + 10 ] = f = new Fixed( value );
                }

                return f;
            }

            return new Fixed( value );
        }
    }

    // ------------------------------------------------------------------------

    public static Value MinWidth   { get; set; } = new ValueMinWidthInnerClass();
    public static Value MinHeight  { get; set; } = new ValueMinHeightInnerClass();
    public static Value MaxWidth   { get; set; } = new ValueMaxWidthInnerClass();
    public static Value MaxHeight  { get; set; } = new ValueMaxHeightInnerClass();
    public static Value PrefWidth  { get; set; } = new ValuePrefWidthInnerClass();
    public static Value PrefHeight { get; set; } = new ValuePrefHeightInnerClass();

    // ------------------------------------------------------------------------
    
    public static Value PercentWidth( float percent ) => new ValuePercentWidth( percent );
    public static Value PercentHeight( float percent ) => new ValuePercentHeight( percent );

    public static Value PercentWidth( float percent, Actor? actor )
    {
        ArgumentNullException.ThrowIfNull( actor );
        
        return new ValuePercentWidth( percent, actor );
    }

    public static Value PercentHeight( float percent, Actor? actor )
    {
        ArgumentNullException.ThrowIfNull( actor );
        
        return new ValuePercentHeight( percent, actor );
    }
}
