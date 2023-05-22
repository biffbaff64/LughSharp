using LibGDXSharp.Scenes.Scene2D;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp
{
    public class ActorTest : Actor
    {
    }
    
    public class Tester
    {
        private ActorTest _actor = new ActorTest();

        public void Test()
        {
            _actor.AddListener( new InputListener()
            {
                public bool Handle( Event e )
                {
                return false;
            }
            });
        }
    }
}
