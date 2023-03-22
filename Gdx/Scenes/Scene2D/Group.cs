using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Scenes.Scene2D
{
    public class Group : Actor
    {
        public SnapshotArray<Actor> Children { get; set; } = new SnapshotArray<Actor>(true, 4);
    }
}

