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
public abstract class Value
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

    public static Value MinWidth   { get; set; } = new ValueMinWidthInnerClass();
    public static Value MinHeight  { get; set; } = new ValueMinHeightInnerClass();
    public static Value PrefWidth  { get; set; } = new ValuePrefWidthInnerClass();
    public static Value PrefHeight { get; set; } = new ValuePrefHeightInnerClass();

    private sealed class ValueMinWidthInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.MinWidth;

            return context?.Width ?? 0;
        }
    }

    private sealed class ValueMinHeightInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.MinHeight;

            return context?.Height ?? 0;
        }
    }


    // ########################################################################


    static public Value prefWidth = new Value()
    {
 

        public float get (@Null Actor context)
        {
        if (context instanceof Layout) return ((Layout)context).getPrefWidth();
        return context == null ? 0 : context.getWidth();
    }

};

static public Value prefHeight = new Value()
{
 

    public float get (@Null Actor context)
    {
    if (context instanceof Layout) return ((Layout)context).getPrefHeight();
    return context == null ? 0 : context.getHeight();
}

};

public static Value maxWidth = new Value()
{
 

    public float get (@Null Actor context)
    {
    if (context instanceof Layout) return ((Layout)context).getMaxWidth();
    return context == null ? 0 : context.getWidth();
}
};

public static Value MaxHeight = new Value()
{
 

    public float get (@Null Actor context)
    {
    if (context instanceof Layout)
    {
    return ((Layout)context).getMaxHeight();
}

return context == null ? 0 : context.getHeight();
}
};

public static Value PercentWidth( float percent )
{
    return new Value()
    {
 

        public float get (@Null Actor actor)
        {
        return actor.getWidth() * percent;
    }

    };
}

public static Value PercentHeight( float percent )
{
    return new Value()
    {
 

        public float get (@Null Actor actor)
        {
        return actor.getHeight() * percent;
    }

    };
}

public static Value PercentWidth( float percent, Actor actor )
{
    if ( actor == null ) throw new ArgumentException( "actor cannot be null." );

    return new Value()
    {
 

        public float? Get( Actor context )
        {
        return actor.getWidth() * percent;
    }

    };
}

public static Value PercentHeight( float percent, Actor actor )
{
    if ( actor == null ) throw new ArgumentException( "actor cannot be null." );

    return new Value()
    {
 

        public float get (@Null Actor context)
        {
        return actor.getHeight() * percent;
    }

    };
}
}