namespace LibGDXSharp.Scenes.Scene2D
{
    public class TouchFocus
    {
        public IEventListener? listener;
        public Actor? listenerActor;
        public Actor? target;
        public int pointer;
        public int button;

        public void Reset()
        {
            listenerActor = null;
            listener      = null;
            target        = null;
        }
    }
}
