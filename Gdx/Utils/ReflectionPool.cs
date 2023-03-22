namespace LibGDXSharp.Utils
{
    public class ReflectionPool<T> : Pool<T>
    {

        protected override T? NewObject()
        {
            return default;
        }
    }
}

