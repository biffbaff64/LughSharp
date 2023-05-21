using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class FocusListener
{
    public sealed class FocusEvent : Event
    {
        public bool    Focused      { get; set; }
        public FeType? Type         { get; set; }
        public Actor?  RelatedActor { get; set; }

        public new void Reset()
        {
            base.Reset();
            RelatedActor = null;
        }

        public enum FeType
        {
            Keyboard,
            Scroll
        }
    }
}