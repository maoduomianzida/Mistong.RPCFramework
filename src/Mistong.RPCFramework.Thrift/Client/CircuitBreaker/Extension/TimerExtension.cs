using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public static class TimerExtension
    {
        public static void Stop(this Timer timer)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            timer.Change(-1, -1);
        }

        public static void Restart(this Timer timer,TimeSpan duetime,TimeSpan period)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            timer.Change(duetime, period);
        }
    }

    internal static class ReaderWriterLockSlimExtension
    {
        public static void UseWriterLock(this ReaderWriterLockSlim that, Action action)
        {
            if (action == null) return;
            try
            {
                that.EnterWriteLock();
                action();
            }
            finally
            {
                that.ExitWriteLock();
            }
        }

        public static void UseReaderLock(this ReaderWriterLockSlim that, Action action)
        {
            try
            {
                that.EnterReadLock();
                action();
            }
            finally
            {
                that.ExitReadLock();
            }
        }
    }
}
