using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Scenes.Scene2D
{
    public class Group : Actor, ICullable
    {
        public SnapshotArray<Actor> Children { get; set; } = new SnapshotArray<Actor>(true, 4);

        public bool RemoveActor( Actor actor, bool b )
        {
            throw new NotImplementedException();
        }

        public void SetDebug( bool enabled = true, bool recursively = true )
        {
        }

        public void SetStage( Stage stage )
        {
            throw new NotImplementedException();
        }

        public void DebugAll()
        {
            throw new NotImplementedException();
        }
    }
}

