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

using LibGDXSharp.Utils;

namespace LibGDXSharp.Maps;

public class MapLayer
{
    public MapObjects    Objects    { get; private set; } = new();
    public MapProperties Properties { get; private set; } = new();
    public string?       Name       { get; set; }
    public float         Opacity    { get; set; }
    public bool          Visible    { get; set; } = true;

    private float     _offsetX;
    private float     _offsetY;
    private float     _renderOffsetX;
    private float     _renderOffsetY;
    private bool      _renderOffsetDirty = true;
    private MapLayer? _parent            = null!;

    public float OffsetX
    {
        get => _offsetX;
        set
        {
            _offsetX = value;
            InvalidateRenderOffset();
        }
    }

    public float OffsetY
    {
        get => _offsetY;
        set
        {
            _offsetY = value;
            InvalidateRenderOffset();
        }
    }

    public float RenderOffsetX
    {
        get
        {
            if ( _renderOffsetDirty )
            {
                CalculateRenderOffsets();
            }

            return _renderOffsetX;
        }
    }

    public float RenderOffsetY
    {
        get
        {
            if ( _renderOffsetDirty )
            {
                CalculateRenderOffsets();
            }

            return _renderOffsetY;
        }
    }

    public MapLayer? Parent
    {
        get => _parent;
        set
        {
            if ( value == this )
            {
                throw new GdxRuntimeException( "Can't set self as the parent" );
            }

            this._parent = value;
        }
    }

    public virtual void InvalidateRenderOffset()
    {
        _renderOffsetDirty = true;
    }

    protected void CalculateRenderOffsets()
    {
        if ( _parent != null )
        {
            _parent.CalculateRenderOffsets();
            _renderOffsetX = _parent.RenderOffsetX + _offsetX;
            _renderOffsetY = _parent.RenderOffsetY + _offsetY;
        }
        else
        {
            _renderOffsetX = _offsetX;
            _renderOffsetY = _offsetY;
        }

        _renderOffsetDirty = false;
    }
}