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

using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// Value placeholder, allowing the value to be computed on request.
/// Values can be provided to an actor for context to reduce the number of value
/// instances that need to be created and reduce verbosity in code that specifies
/// values.
/// </summary>
[PublicAPI]
public abstract class Value
{
    public abstract float Get( Actor? context = null );

    public readonly static Fixed Zero = new( 0 );

    /// <summary>
    /// A fixed value that is not computed each time it is used.
    /// </summary>
    [PublicAPI]
    public class Fixed : Value
    {
        private readonly static Fixed?[] Cache = new Fixed[ 111 ];

        public float Value { get; }

        public Fixed( float value )
        {
            this.Value = value;
        }

        public override float Get( Actor? context = null)
        {
            return Value;
        }

        public new string ToString()
        {
            return Value.ToString( CultureInfo.InvariantCulture );
        }

        public static Fixed ValueOf( float value )
        {
            if ( value == 0 )
            {
                return Zero;
            }

            if ( value is >= -10 and <= 100 && value.Equals( ( int )value ) )
            {
                Fixed? f = Cache[ ( int )value + 10 ];

                if ( f == null )
                {
                    Cache[ ( int )value + 10 ] = f = new Fixed( value );
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

    /// <summary>
    /// Returns a value that is a percentage of the actor's width.
    /// </summary>
    private class ValuePercentWidth : Value
    {
        private readonly float  _percent;
        private readonly Actor? _actor;

        public ValuePercentWidth( float percent, Actor? actor = null )
        {
            this._percent = percent;
            this._actor   = actor;
        }

        public override float Get( Actor? actor = null )
        {
            return ( this._actor == null ) ? 0 : _actor.Width * _percent;
        }
    }

    /// <summary>
    /// Returns a value that is a percentage of the actor's height.
    /// </summary>
    private class ValuePercentHeight : Value
    {
        private readonly float  _percent;
        private readonly Actor? _actor;

        public ValuePercentHeight( float percent, Actor? actor = null )
        {
            this._percent = percent;
            this._actor   = actor;
        }

        public override float Get( Actor? actor = null )
        {
            return ( this._actor == null ) ? 0 : _actor.Height * _percent;
        }
    }
    
    // ========================================================================
    
    /// <summary>
    /// Value that is the minWidth of the actor in the cell.
    /// </summary>
    private class ValueMinWidthInnerClass : Value
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

    /// <summary>
    /// Value that is the minHeight of the actor in the cell.
    /// </summary>
    private class ValueMinHeightInnerClass : Value
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

    /// <summary>
    /// Value that is the prefWidth of the actor in the cell.
    /// </summary>
    private class ValuePrefWidthInnerClass : Value
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

    /// <summary>
    /// Value that is the prefHeight of the actor in the cell.
    /// </summary>
    private class ValuePrefHeightInnerClass : Value
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

    /// <summary>
    /// Value that is the maxWidth of the actor in the cell.
    /// </summary>
    private class ValueMaxWidthInnerClass : Value
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

    /// <summary>
    /// Value that is the maxWidth of the actor in the cell.
    /// </summary>
    private class ValueMaxHeightInnerClass : Value
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
