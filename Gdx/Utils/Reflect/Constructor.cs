namespace LibGDXSharp.Utils.Reflect
{
    public class Constructor
    {
        public void SetAccessible( bool state )
        {
        }

        public virtual object? NewInstance(params object[] args)
        {
            try
            {
                return constructor.NewInstance(args);
            }
            catch (System.ArgumentException e)
            {
                throw new ReflectionException("Illegal argument(s) supplied to constructor for class: " + getDeclaringClass().getName(), e);
            }
            catch (InstantiationException e)
            {
                throw new ReflectionException("Could not instantiate instance of class: " + getDeclaringClass().getName(), e);
            }
            catch (IllegalAccessException e)
            {
                throw new ReflectionException("Could not instantiate instance of class: " + getDeclaringClass().getName(), e);
            }
            catch (InvocationTargetException e)
            {
                throw new ReflectionException("Exception occurred in constructor for class: " + getDeclaringClass().getName(), e);
            }
        }

        public Type GetDeclaringClass()
        {
            throw new NotImplementedException();
        }
    }
}

