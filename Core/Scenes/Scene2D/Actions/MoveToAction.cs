using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Moves an actor from its current position to a specific position.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class MoveToAction : TemporalAction
{
    public float StartX    { get; set; }
    public float StartY    { get; set; }
    public float EndX      { get; set; }
    public float EndY      { get; set; }
    public int   Alignment { get; set; } = Align.BottomLeft;

    protected new void Begin()
    {
        StartX = Target!.GetX( Alignment );
        StartY = Target.GetY( Alignment );
    }

    protected override void Update( float percent )
    {
        float x, y;

        switch ( percent )
        {
            case 0:
                x = StartX;
                y = StartY;

                break;

            case 1:
                x = EndX;
                y = EndY;

                break;

            default:
                x = StartX + ( ( EndX - StartX ) * percent );
                y = StartY + ( ( EndY - StartY ) * percent );

                break;
        }

        Target?.SetPosition( x, y, Alignment );
    }

    public new void Reset()
    {
        base.Reset();
        Alignment = Align.BottomLeft;
    }

    public void SetStartPosition( float x, float y )
    {
        StartX = x;
        StartY = y;
    }

    public void SetPosition( float x, float y )
    {
        EndX = x;
        EndY = y;
    }

    public void SetPosition( float x, float y, int alignment )
    {
        EndX           = x;
        EndY           = y;
        this.Alignment = alignment;
    }
}
