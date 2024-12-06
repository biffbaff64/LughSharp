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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Maps;

/// <summary>
/// Map layer containing a set of objects and properties.
/// </summary>
[PublicAPI]
public class MapLayer
{
    private float         _offsetX;
    private float         _offsetY;
    private MapLayer?     _parent            = null!;
    private bool          _renderOffsetDirty = true;
    private float         _renderOffsetX;
    private float         _renderOffsetY;
    public  MapObjects    Objects    { get; private set; } = new();
    public  MapProperties Properties { get; private set; } = new();
    public  string?       Name       { get; set; }
    public  float         Opacity    { get; set; }
    public  bool          Visible    { get; set; } = true;

    // ========================================================================

    /// <summary>
    /// The layers X offset.
    /// </summary>
    public float OffsetX
    {
        get => _offsetX;
        set
        {
            _offsetX = value;
            InvalidateRenderOffset();
        }
    }

    /// <summary>
    /// The layers Y offset.
    /// </summary>
    public float OffsetY
    {
        get => _offsetY;
        set
        {
            _offsetY = value;
            InvalidateRenderOffset();
        }
    }

    /// <summary>
    /// The layer's X render offset, this takes into consideration all parent layers' offsets
    /// </summary>
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

    /// <summary>
    /// The layer's Y render offset, this takes into consideration all parent layers' offsets
    /// </summary>
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

    /// <summary>
    /// This layers parent MapLayer, or Null if there is no parent.
    /// </summary>
    public MapLayer? Parent
    {
        get => _parent;
        set
        {
            if ( value == this )
            {
                throw new GdxRuntimeException( "Can't set self as the parent" );
            }

            _parent = value;
        }
    }

    /// <summary>
    /// Flags that Render Offsets need to be recalculated.
    /// </summary>
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
