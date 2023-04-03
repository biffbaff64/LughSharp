using LibGDXSharp.Maths;

namespace LibGDXSharp.Scenes.Scene2D.Utils
{
    public class ScissorStack
    {
        public static object PopScissors()
        {
            return null;
        }

        public static bool PushScissors( RectangleShape scissorBounds )
        {
            return false;
        }

        public static void CalculateScissors( Camera? camera,
                                              float screenX,
                                              float screenY,
                                              float screenWidth,
                                              float screenHeight,
                                              Matrix4 batchTransform,
                                              RectangleShape area,
                                              RectangleShape scissor )
        {
        }
    }
}
