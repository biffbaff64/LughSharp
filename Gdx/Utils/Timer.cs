using Monitor = System.Threading.Monitor;

namespace LibGDXSharp.Utils
{
    public class Timer
    {
        private readonly static object       threadLock = new object();
        private static          TimerThread? _thread;
        private readonly        List< Task > _tasks = new List< Task >( 8 );

        public Timer()
        {
            Start();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public static Timer Instance()
        {
            lock ( threadLock )
            {
                TimerThread? thread = Thread();

                if ( thread == null ) throw new GdxRuntimeException( "Thread instance is null!" );

                return thread.instance ?? ( thread.instance = new Timer() );
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static TimerThread? Thread()
        {
            lock ( threadLock )
            {
                if ( _thread == null || _thread.files != Core.Gdx.Files )
                {
                    _thread?.Dispose();

                    _thread = new TimerThread();
                }

                return _thread;
            }
        }

        /// <summary>
        /// Schedules a task to occur once as soon as possible, but not sooner
        /// than the start of the next frame.
        /// </summary>
        public Task PostTask( Task task )
        {
            return ScheduleTask( task, 0, 0, 0 );
        }

        /// <summary>
        /// Schedules a task to occur once after the specified delay.
        /// </summary>
        public Task ScheduleTask( Task task, float delaySeconds )
        {
            return ScheduleTask( task, delaySeconds, 0, 0 );
        }

        /// <summary>
        /// Schedules a task to occur once after the specified delay and then repeatedly
        /// at the specified interval until cancelled.
        /// </summary>
        public Task ScheduleTask( Task task, float delaySeconds, float intervalSeconds )
        {
            return ScheduleTask( task, delaySeconds, intervalSeconds, -1 );
        }

        /// <summary>
        /// Schedules a task to occur once after the specified delay and then a
        /// number of additional times at the specified interval.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delaySeconds"></param>
        /// <param name="intervalSeconds"></param>
        /// <param name="repeatCount"> If negative, the task will repeat forever.</param>
        protected virtual Task ScheduleTask( Task task, float delaySeconds, float intervalSeconds, int repeatCount )
        {
            lock ( threadLock )
            {
                lock ( this )
                {
                    lock ( task )
                    {
                        if ( task.timer != null )
                        {
                            throw new ArgumentException( "The same task may not be scheduled twice." );
                        }

                        task.timer = this;

                        var timeMillis        = TimeUtils.NanoTime() / 1000000;
                        var executeTimeMillis = timeMillis + ( long )( delaySeconds * 1000 );

                        if ( _thread?.pauseTimeMillis > 0 )
                        {
                            executeTimeMillis -= timeMillis - _thread.pauseTimeMillis;
                        }

                        task.executeTimeMillis = executeTimeMillis;
                        task.intervalMillis    = ( long )( intervalSeconds * 1000 );
                        task.repeatCount       = repeatCount;

                        _tasks.Add( task );
                    }
                }

                Monitor.PulseAll( threadLock );
            }

            return task;
        }

        /// <summary>
        /// Stops the timer, tasks will not be executed and time that passes
        /// will not be applied to the task delays.
        /// </summary>
        public void Stop()
        {
            lock ( threadLock )
            {
                Thread().Thread().instances.RemoveValue( this );
            }
        }

        /// <summary>
        /// Starts the timer if it is not currently running.
        /// </summary>
        public void Start()
        {
            lock ( threadLock )
            {
                if ( Thread().instances.Contains( this ) ) return;

                Thread().instances.Add( this );

                Monitor.PulseAll( threadLock );
            }
        }

        /// <summary>
        /// Cancels all tasks.
        /// </summary>
        public void Clear()
        {
            for ( int i = 0, n = _tasks.Size; i < n; i++ )
            {
                if ( _tasks.Get( i ) != null )
                {
                    lock ( _tasks.Get( i )! )
                    {
                        _tasks.Get( i )!.executeTimeMillis = 0;
                        _tasks.Get( i )!.timer             = null;
                    }
                }
            }

            _tasks.Clear();
        }

        /// <summary>
        /// Returns true if the timer has no tasks in the queue.
        /// Note that this can change at any time. Synchronize on the timer instance
        /// to prevent tasks being added, removed, or updated.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            lock ( threadLock )
            {
                return _tasks.Size == 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="timeMillis"></param>
        /// <param name="waitMillis"></param>
        /// <returns></returns>
        public long Update( long timeMillis, long waitMillis )
        {
            lock ( threadLock )
            {
                for ( int i = 0, n = _tasks.Count; i < n; i++ )
                {
                    lock ( _tasks[ i ]! )
                    {
                        if ( _tasks[ i ]!.executeTimeMillis > timeMillis )
                        {
                            waitMillis = Math.Min( waitMillis, _tasks[ i ]!.executeTimeMillis - timeMillis );

                            continue;
                        }

                        if ( _tasks[ i ]!.repeatCount == 0 )
                        {
                            _tasks[ i ]!.timer = null;

                            _tasks.RemoveAt( i );

                            i--;
                            n--;
                        }
                        else
                        {
                            _tasks[ i ]!.executeTimeMillis = timeMillis + _tasks[ i ]!.intervalMillis;

                            waitMillis = Math.Min( waitMillis, _tasks[ i ]!.intervalMillis );

                            if ( _tasks[ i ]!.repeatCount > 0 )
                            {
                                _tasks[ i ]!.repeatCount--;
                            }
                        }

                        _tasks[ i ]!.Gdx.App!.PostRunnable( _tasks.Get( i )! );
                    }
                }

                return waitMillis;
            }
        }

        /// <summary>
        /// Adds the specified delay to all tasks.
        /// </summary>
        /// <param name="delayMillis"></param>
        public virtual void Delay( long delayMillis )
        {
            lock ( threadLock )
            {
                for ( int i = 0, n = _tasks.Size; i < n; i++ )
                {
                    Task task = _tasks.Get( i )!;

                    lock ( task )
                    {
                        task.executeTimeMillis += delayMillis;
                    }
                }
            }
        }

        /// <summary>
        /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
        /// </summary>
        public static Task Post( Task task )
        {
            return Instance().PostTask( task );
        }

        /// <summary>
        /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
        /// </summary>
        public static Task Schedule( Task task, float delaySeconds )
        {
            return Instance().ScheduleTask( task, delaySeconds );
        }

        /// <summary>
        /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
        /// </summary>
        public static Task Schedule( Task task, float delaySeconds, float intervalSeconds )
        {
            return Instance().ScheduleTask( task, delaySeconds, intervalSeconds );
        }

        /// <summary>
        /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
        /// </summary>
        public static Task Schedule( Task task, float delaySeconds, float intervalSeconds, int repeatCount )
        {
            return Instance().ScheduleTask( task, delaySeconds, intervalSeconds, repeatCount );
        }

        // ================================================================================
        //  Companion Classes
        // ================================================================================

        /// <summary>
        /// A <see cref="IRunnable"/> that can be scheduled on a <see cref="Timer"/>
        /// </summary>
        public abstract class Task : IRunnable
        {
            internal volatile Timer? timer;
            internal          long   executeTimeMillis;
            internal          long   intervalMillis;
            internal          int    repeatCount;

            protected Task()
            {
                if ( Gdx.App == null )
                {
                    throw new GdxRuntimeException( "Gdx.App not available!" );
                }
            }

            /// <summary>
            /// If this is the last time the task will be ran or the task is first
            /// cancelled, it may be scheduled again in this method.
            /// </summary>
            public abstract void Run();

            /// <summary>
            /// Cancels the task.
            /// It will not be executed until it is scheduled again.
            /// This method can be called at any time.
            /// </summary>
            public void Cancel()
            {
                if ( timer != null )
                {
                    lock ( timer )
                    {
                        lock ( this )
                        {
                            executeTimeMillis = 0;
                            this.timer        = null;

                            timer?._tasks.RemoveValue( this );
                        }
                    }
                }
                else
                {
                    lock ( this )
                    {
                        executeTimeMillis = 0;
                        this.timer        = null;
                    }
                }
            }

            /// <summary>
            /// Returns true if this task is scheduled to be executed in the future by a timer.
            /// The execution time may be reached at any time after calling this method,
            /// which may change the scheduled state.
            /// <p>
            /// To prevent the scheduled state from changing, synchronize on this task object, eg:
            /// <code>
            ///     lock( task )
            ///     {
            ///         if ( !task.IsScheduled() )
            ///         {
            ///             . . . . 
            ///         }
            ///     }
            /// </code>
            /// </p>
            /// </summary>
            /// <returns></returns>
            public bool IsScheduled()
            {
                return timer != null;
            }
        }

        public class TimerThread : IRunnable, ILifecycleListener
        {
            private readonly Array< Timer > _instances = new Array< Timer >( capacity: 1 );

            internal readonly IFiles? files;
            internal          Timer?  instance;
            internal          long    pauseTimeMillis;

            public TimerThread()
            {
                files = Gdx.Files;

                Gdx.App?.AddLifecycleListener( this );

                Resume();

                var thread = new Thread( new ThreadStart( this.Run ) )
                {
                    Name = "Timer"
                };

                thread.Start();
            }

            public void Run()
            {
                lock ( threadLock )
                {
                    if ( _thread != this || files != Gdx.Files ) break;


                }
            }

            public void Pause()
            {
                lock ( threadLock )
                {
                    pauseTimeMillis = TimeUtils.NanosToMillis();

                    Monitor.PulseAll( threadLock );
                }
            }

            public void Resume()
            {
                lock ( threadLock )
                {
                    var delayMillis = TimeUtils.NanosToMillis() - pauseTimeMillis;

                    for ( int i = 0, n = _instances.Size; i < n; i++ )
                    {
                        _instances.Get( i )?.Delay( delayMillis );
                    }

                    pauseTimeMillis = 0;

                    Monitor.PulseAll( threadLock );
                }
            }

            public void Dispose()
            {
                lock ( threadLock )
                {
                    if ( _thread == this ) _thread = null;

                    _instances.Clear();

                    Monitor.PulseAll( threadLock );
                }

                Gdx.App?.RemoveLifecycleListener( this );
            }
        }
    }
}
