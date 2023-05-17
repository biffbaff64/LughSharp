namespace LibGDXSharp.Scenes.Scene2D.UI;

public class Table : WidgetGroup
{
    public enum DebugType
    {
        None,
        All,
        Table,
        Cell,
        Actor
    }

    /** Turns debug lines on or off. */
    public Table DebugLines( DebugType? debug )
    {
        base.SetDebug( debug != DebugType.None );

        if ( this.debug != debug )
        {
            this.debug = debug;

            if ( debug == DebugType.None )
            {
                ClearDebugRects();
            }
            else
            {
                Invalidate();
            }
        }

        return this;
    }
}