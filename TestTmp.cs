
using LibGDXSharp.Scenes.Scene2D;
using LibGDXSharp.Scenes.Scene2D.UI;

public class Tree<TNode, TValue> : WidgetGroup
    where TNode : Tree< TNode, TValue >.Node< TNode, TValue, Actor >
{
    public class Node<TNodeNode, TNodeValue, TNodeActor>
        where TNodeNode : Node< TNode, TValue, Actor >
        where TNodeValue : TValue
        where TNodeActor : Actor
    {
        /// <summary>
        /// Returns true if the specified node is this node
        /// or an descendant of this node.
        /// </summary>
        public bool IsDescendantOf( TNodeNode? node )
        {
            ArgumentNullException.ThrowIfNull( node );
            
            return ?
        }
    }
}