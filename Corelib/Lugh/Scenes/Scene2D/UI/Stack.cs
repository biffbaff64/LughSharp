// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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
using Corelib.Lugh.Utils.Collections;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

[PublicAPI]
public class Stack : WidgetGroup
{
    // ========================================================================

    public Stack()
    {
        Transform = false;
        Width     = 150;
        Height    = 150;
        Touchable = Touchable.ChildrenOnly;
    }

    public Stack( params Actor[] actors ) : this()
    {
        LoadActors( actors );
    }

    public override float PrefWidth
    {
        get
        {
            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _prefWidth;
        }
    }

    public override float PrefHeight
    {
        get
        {
            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _prefHeight;
        }
    }

    public override float MinWidth
    {
        get
        {
            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _minWidth;
        }
    }

    public override float MinHeight
    {
        get
        {
            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _minHeight;
        }
    }

    public override float MaxWidth
    {
        get
        {
            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _maxWidth;
        }
    }

    public override float MaxHeight
    {
        get
        {
            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _maxHeight;
        }
    }

    public override void Invalidate()
    {
        base.Invalidate();
        _sizeInvalid = true;
    }

    private void ComputeSize()
    {
        _sizeInvalid = false;
        _prefWidth   = 0;
        _prefHeight  = 0;
        _minWidth    = 0;
        _minHeight   = 0;
        _maxWidth    = 0;
        _maxHeight   = 0;

        SnapshotArray< Actor > children = Children;

        for ( int i = 0, n = children.Size; i < n; i++ )
        {
            var   child = children.GetAt( i );
            float childMaxWidth, childMaxHeight;

            if ( child is ILayout layout )
            {
                _prefWidth  = Math.Max( _prefWidth, layout.PrefWidth );
                _prefHeight = Math.Max( _prefHeight, layout.PrefHeight );
                _minWidth   = Math.Max( _minWidth, layout.MinWidth );
                _minHeight  = Math.Max( _minHeight, layout.MinHeight );

                childMaxWidth  = layout.MaxWidth;
                childMaxHeight = layout.MaxHeight;
            }
            else
            {
                _prefWidth  = Math.Max( _prefWidth, child.Width );
                _prefHeight = Math.Max( _prefHeight, child.Height );
                _minWidth   = Math.Max( _minWidth, child.Width );
                _minHeight  = Math.Max( _minHeight, child.Height );

                childMaxWidth  = 0;
                childMaxHeight = 0;
            }

            if ( childMaxWidth > 0 )
            {
                _maxWidth = _maxWidth == 0 ? childMaxWidth : Math.Min( _maxWidth, childMaxWidth );
            }

            if ( childMaxHeight > 0 )
            {
                _maxHeight = _maxHeight == 0 ? childMaxHeight : Math.Min( _maxHeight, childMaxHeight );
            }
        }
    }

    public void Add( Actor actor )
    {
        AddActor( actor );
    }

    public void Layout()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        var width  = Width;
        var height = Height;

        SnapshotArray< Actor > children = Children;

        for ( int i = 0, n = children.Size; i < n; i++ )
        {
            var child = children.GetAt( i );
            child.SetBounds( 0, 0, width, height );

            if ( child is ILayout layout )
            {
                layout.Validate();
            }
        }
    }

    #region Backing data for properties

    private float _maxHeight;
    private float _maxWidth;
    private float _minHeight;
    private float _minWidth;
    private float _prefHeight;
    private float _prefWidth;
    private bool  _sizeInvalid = true;

    #endregion Backing data for properties
}
