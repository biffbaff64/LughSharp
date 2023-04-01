using LibGDXSharp.Core;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Utils
{
    public class Timer
    {
        private readonly static object        threadLock = new object();
        private static          TimerThread?  _thread;
        private readonly        Array< Task > _tasks = new Array< Task >( false, 8 );

        public Timer()
        {
            Start();
        }

        public static Timer Instance()
        {
            lock ( threadLock )
            {
                TimerThread? thread = Thread();

                if ( thread == null ) thread.instance = new TimerThread();

                return thread.instance;
            }
        }

        private static TimerThread? Thread()
        {
            lock ( threadLock )
            {
                if ( _thread == null || _thread.files != Gdx.Files )
                {
                    if ( _thread != null ) _thread.dispose();

                    _thread = new TimerThread();
                }

                return _thread;
            }
        }

        /// <summary>
        /// Schedules a task to occur once as soon as possible, but not sooner than the start of the next frame.
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
        /// Schedules a task to occur once after the specified delay and then repeatedly at the specified interval until cancelled.
        /// </summary>
        public Task ScheduleTask( Task task, float delaySeconds, float intervalSeconds )
        {
            return ScheduleTask( task, delaySeconds, intervalSeconds, -1 );
        }

        public class TimerThread
        {
        }
    }
}
