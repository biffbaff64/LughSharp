using LibGDXSharp.Backends.Desktop.Audio;
using LibGDXSharp.Core;

namespace LibGDXSharp.Backends.Desktop
{
    public abstract class GLApplicationBase : Application
    {
        public abstract IGLAudio CreateAudio( GLApplicationConfiguration config );

        public abstract IGLInput CreateInput( GLWindow window );
    }
}
