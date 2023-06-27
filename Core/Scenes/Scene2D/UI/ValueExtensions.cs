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

public abstract partial class Value
{
    /// <summary>
    /// Value that is the minWidth of the actor in the cell.
    /// </summary>
    private sealed class ValueMinWidthInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.GetMinWidth();

            return context?.Width ?? 0;
        }
    }

    /// <summary>
    /// Value that is the minHeight of the actor in the cell.
    /// </summary>
    private sealed class ValueMinHeightInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.GetMinHeight();

            return context?.Height ?? 0;
        }
    }

    /// <summary>
    /// Value that is the prefWidth of the actor in the cell.
    /// </summary>
    private sealed class ValuePrefWidthInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.GetPrefWidth();

            return context?.Width ?? 0;
        }
    }

    /// <summary>
    /// Value that is the prefHeight of the actor in the cell.
    /// </summary>
    private sealed class ValuePrefHeightInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.GetPrefHeight();

            return context?.Height ?? 0;
        }
    }

    /// <summary>
    /// Value that is the maxWidth of the actor in the cell.
    /// </summary>
    private sealed class ValueMaxWidthInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.GetMaxWidth();

            return context?.Width ?? 0;
        }
    }

    /// <summary>
    /// Value that is the maxWidth of the actor in the cell.
    /// </summary>
    private sealed class ValueMaxHeightInnerClass : Value
    {
        public override float Get( Actor? context = null )
        {
            if ( context is ILayout layout ) return layout.GetMaxHeight();

            return context?.Height ?? 0;
        }
    }

    /// <summary>
    /// Returns a value that is a percentage of the actor's width.
    /// </summary>
    private sealed class ValuePercentWidth : Value
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
            if ( this._actor == null )
            {
                return ( actor?.Width * _percent ) ?? 0;
            }

            return _actor.Width * _percent;
        }
    }

    /// <summary>
    /// Returns a value that is a percentage of the actor's height.
    /// </summary>
    private sealed class ValuePercentHeight : Value
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
            if ( this._actor == null )
            {
                return ( actor?.Height * _percent ) ?? 0;
            }

            return _actor.Height * _percent;
        }
    }
}