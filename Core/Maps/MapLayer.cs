namespace LibGDXSharp.Maps;

public class MapLayer
{
    public MapObjects    Objects    { get; private set; } = new MapObjects();
    public MapProperties Properties { get; private set; } = new MapProperties();
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
            if ( _renderOffsetDirty ) CalculateRenderOffsets();

            return _renderOffsetX;
        }
    }

    public float RenderOffsetY
    {
        get
        {
            if ( _renderOffsetDirty ) CalculateRenderOffsets();

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

    internal void InvalidateRenderOffset()
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