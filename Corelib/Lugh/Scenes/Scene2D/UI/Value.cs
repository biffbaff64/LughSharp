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

using Corelib.Lugh.Scenes.Scene2D.Utils;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// Value placeholder, allowing the value to be computed on request.
/// Values can be provided to an actor for context to reduce the number of value
/// instances that need to be created and reduce verbosity in code that specifies
/// values.
/// </summary>
[PublicAPI]
public abstract class Value
{
    public static readonly Fixed Zero = new( 0 );
    public static          Value MinWidth   { get; set; } = new ValueMinWidthInnerClass();
    public static          Value MinHeight  { get; set; } = new ValueMinHeightInnerClass();
    public static          Value MaxWidth   { get; set; } = new ValueMaxWidthInnerClass();
    public static          Value MaxHeight  { get; set; } = new ValueMaxHeightInnerClass();
    public static          Value PrefWidth  { get; set; } = new ValuePrefWidthInnerClass();
    public static          Value PrefHeight { get; set; } = new ValuePrefHeightInnerClass();

    // ========================================================================

    public abstract float Get( Actor? context = null );

    // ========================================================================

    public static Value PercentWidth( float percent )
    {
        return new ValuePercentWidth( percent );
    }

    public static Value PercentHeight( float percent )
    {
        return new ValuePercentHeight( percent );
    }

    public static Value PercentWidth( float percent, Actor? actor )
    {
        return new ValuePercentWidth( percent, actor );
    }

    public static Value PercentHeight( float percent, Actor? actor )
    {
        return new ValuePercentHeight( percent, actor );
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// A fixed value that is not computed each time it is used.
    /// </summary>
    [PublicAPI]
    public class Fixed : Value
    {
        private static readonly Fixed?[] _cache = new Fixed[ 111 ];

        public Fixed( float value )
        {
            Value = value;
        }

        public float Value { get; }

        public override float Get( Actor? context = null )
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString( CultureInfo.InvariantCulture );
        }

        public static Fixed ValueOf( float value )
        {
            if ( value == 0 )
            {
                return Zero;
            }

            if ( value is >= -10 and <= 100 && value.Equals( ( int ) value ) )
            {
                var f = _cache[ ( int ) value + 10 ];

                if ( f == null )
                {
                    _cache[ ( int ) value + 10 ] = f = new Fixed( value );
                }

                return f;
            }

            return new Fixed( value );
        }
    }

    // ========================================================================

    /// <summary>
    /// Returns a value that is a percentage of the actor's width.
    /// </summary>
    private sealed class ValuePercentWidth : Value
    {
        private readonly Actor? _actor;
        private readonly float  _percent;

        public ValuePercentWidth( float percent, Actor? actor = null )
        {
            _percent = percent;
            _actor   = actor;
        }

        public override float Get( Actor? actor = null )
        {
            return ( _actor?.Width * _percent ) ?? 0;
        }
    }

    // ========================================================================

    /// <summary>
    /// Returns a value that is a percentage of the actor's height.
    /// </summary>
    private sealed class ValuePercentHeight : Value
    {
        private readonly Actor? _actor;
        private readonly float  _percent;

        public ValuePercentHeight( float percent, Actor? actor = null )
        {
            _percent = percent;
            _actor   = actor;
        }

        public override float Get( Actor? actor = null )
        {
            return ( _actor?.Height * _percent ) ?? 0;
        }
    }

    // ========================================================================

    /// <summary>
    /// Value that is the minWidth of the actor in the cell.
    /// </summary>
    private sealed class ValueMinWidthInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout )
            {
                return layout.MinWidth;
            }

            return context?.Width ?? 0;
        }
    }

    // ========================================================================

    /// <summary>
    /// Value that is the minHeight of the actor in the cell.
    /// </summary>
    private sealed class ValueMinHeightInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout )
            {
                return layout.MinHeight;
            }

            return context?.Height ?? 0;
        }
    }

    // ========================================================================

    /// <summary>
    /// Value that is the prefWidth of the actor in the cell.
    /// </summary>
    private sealed class ValuePrefWidthInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout )
            {
                return layout.PrefWidth;
            }

            return context?.Width ?? 0;
        }
    }

    // ========================================================================

    /// <summary>
    /// Value that is the prefHeight of the actor in the cell.
    /// </summary>
    private sealed class ValuePrefHeightInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout )
            {
                return layout.PrefHeight;
            }

            return context?.Height ?? 0;
        }
    }

    // ========================================================================

    /// <summary>
    /// Value that is the maxWidth of the actor in the cell.
    /// </summary>
    private sealed class ValueMaxWidthInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout )
            {
                return layout.MaxWidth;
            }

            return context?.Width ?? 0;
        }
    }

    // ========================================================================

    /// <summary>
    /// Value that is the maxWidth of the actor in the cell.
    /// </summary>
    private sealed class ValueMaxHeightInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout )
            {
                return layout.MaxHeight;
            }

            return context?.Height ?? 0;
        }
    }
}
