namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class ScaleToAction : TemporalAction
{
    public float EndX { get; set; }
    public float EndY { get; set; }
    
    private float _startX;
    private float _startY;

    protected new void Begin()
    {
        _startX = Target!.ScaleX;
        _startY = Target.ScaleY;
    }

    protected override void Update( float percent )
    {
        float x, y;

        if ( percent == 0 )
        {
            x = _startX;
            y = _startY;
        }
        else if ( percent is 1.0f )
        {
            x = EndX;
            y = EndY;
        }
        else
        {
            x = _startX + ( ( EndX - _startX ) * percent );
            y = _startY + ( ( EndY - _startY ) * percent );
        }

        Target?.SetScale( x, y );
    }

    public void SetScale( float x, float y )
    {
        EndX = x;
        EndY = y;
    }

    public void SetScale( float scale )
    {
        EndX = scale;
        EndY = scale;
    }
}