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

using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class Stack : WidgetGroup
{
    private float _prefWidth;
    private float _prefHeight;
    private float _minWidth;
    private float _minHeight;
    private float _maxWidth;
    private float _maxHeight;
    private bool  _sizeInvalid = true;

    public Stack()
    {
        Transform = false;
        Width     = 150;
        Height    = 150;
        Touchable = Touchable.ChildrenOnly;
    }

    public Stack( params Actor[] actors ) : this()
    {
        foreach ( Actor actor in actors )
        {
            AddActor( actor );
        }
    }

    public new void Invalidate()
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
            Actor child = children.Get( i );
            float childMaxWidth, childMaxHeight;

            if ( child is ILayout layout )
            {
                _prefWidth      = Math.Max( _prefWidth,  layout.PrefWidth );
                _prefHeight     = Math.Max( _prefHeight, layout.PrefHeight );
                _minWidth       = Math.Max( _minWidth,   layout.MinWidth );
                _minHeight      = Math.Max( _minHeight,  layout.MinHeight );
                
                childMaxWidth  = layout.MaxWidth;
                childMaxHeight = layout.MaxHeight;
            }
            else
            {
                _prefWidth      = Math.Max( _prefWidth, child.Width );
                _prefHeight     = Math.Max( _prefHeight, child.Height );
                _minWidth       = Math.Max( _minWidth, child.Width );
                _minHeight      = Math.Max( _minHeight, child.Height );

                childMaxWidth  = 0;
                childMaxHeight = 0;
            }

            if ( childMaxWidth > 0 ) _maxWidth   = _maxWidth == 0 ? childMaxWidth : Math.Min( _maxWidth, childMaxWidth );
            if ( childMaxHeight > 0 ) _maxHeight = _maxHeight == 0 ? childMaxHeight : Math.Min( _maxHeight, childMaxHeight );
        }
    }

    public void Add( Actor actor )
    {
        AddActor( actor );
    }

    public new void Layout()
    {
        if ( _sizeInvalid ) ComputeSize();

        var width  = Width;
        var height = Height;

        SnapshotArray< Actor > children = Children;

        for ( int i = 0, n = children.Size; i < n; i++ )
        {
            Actor child = children.Get( i );
            child.SetBounds( 0, 0, width, height );
            
            if ( child is ILayout layout) layout.Validate();
        }
    }

    public float GetPrefWidth()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _prefWidth;
    }

    public float GetPrefHeight()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _prefHeight;
    }

    public float GetMinWidth()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _minWidth;
    }

    public float GetMinHeight()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _minHeight;
    }

    public float GetMaxWidth()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _maxWidth;
    }

    public float GetMaxHeight()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _maxHeight;
    }
}
