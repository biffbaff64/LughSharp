using LibGDXSharp.Maths;

namespace LibGDXSharp.Scenes.Scene2D
{
    public class Stage
    {

        public bool GetActionsRequestRendering()
        {
            return false;
        }

        public Vector2 StageToScreenCoordinates( Vector2 localToAscendantCoordinates )
        {
            return null;
        }

        public bool Debug { get; set; }

        public object GetDebugColor()
        {
            return null;
        }

        public Vector2 ScreenToStageCoordinates( Vector2 screenCoords )
        {
            return null;
        }

        public void CalculateScissors( Rectangle tableBounds, Rectangle scissorBounds )
        {
        }
    }
}

