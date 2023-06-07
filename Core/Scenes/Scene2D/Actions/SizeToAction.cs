namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Moves an actor from its current size to a specific size.
/// </summary>
public class SizeToAction : TemporalAction
{
    public float StartWidth  { get; set; }
    public float StartHeight { get; set; }
    public float EndWidth    { get; set; }
    public float EndHeight   { get; set; }

    protected new void Begin()
    {
        if ( Target == null ) throw new GdxRuntimeException( "Cannot Begin with null Target Actor!" );
        
        StartWidth  = Target.Width;
        StartHeight = Target.Height;
    }

    protected override void Update( float percent )
    {
        float width, height;

        if ( percent == 0 )
        {
            width  = StartWidth;
            height = StartHeight;
        }
        else if ( percent is 1.0f )
        {
            width  = EndWidth;
            height = EndHeight;
        }
        else
        {
            width  = StartWidth + ( ( EndWidth - StartWidth ) * percent );
            height = StartHeight + ( ( EndHeight - StartHeight ) * percent );
        }

        Target?.SetSize( width, height );
    }

    public void SetSize( float width, float height )
    {
        EndWidth  = width;
        EndHeight = height;
    }
}