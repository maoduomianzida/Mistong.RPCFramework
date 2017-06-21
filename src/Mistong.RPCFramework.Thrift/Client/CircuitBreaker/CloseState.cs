using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public class CloseState : CircuitBreakerState
    {
        private readonly Tuple<TimeSpan, int> _allowFailSetting;
        private Timer _timer;
        private int _currrentFailTimes;

        public CloseState(CircuitBreaker context, Tuple<TimeSpan, int> allowFailSetting) : base(context)
        {
            if (allowFailSetting == null) throw new ArgumentNullException(nameof(allowFailSetting));
            if (allowFailSetting.Item1 <= TimeSpan.Zero || allowFailSetting.Item2 <= 0)
            {
                throw new ArgumentOutOfRangeException("时间间隔不能小于0，允许失败的次数不能小于等于0次！");
            }

            _allowFailSetting = allowFailSetting;
            _timer = new Timer(CheckAllowTimes, null, -1, -1);
        }

        private void CheckAllowTimes(object state)
        {
            ResetFailTimes();
        }

        private void ResetFailTimes()
        {
            Interlocked.Exchange(ref _currrentFailTimes, 0);
        }

        public override void Initialize()
        {
            _timer.Restart(_allowFailSetting.Item1, _allowFailSetting.Item1);
        }

        public override void ProcessFail()
        {
            int failTimes = Interlocked.Increment(ref _currrentFailTimes);
            if (failTimes > _allowFailSetting.Item2)
            {
                Context.MoveToOpenState();
            }
        }

        public override void ProcessSuccess()
        {
        }

        public override void ProcessBefore()
        {
        }

        public override void Clear()
        {
            _timer.Stop();
            ResetFailTimes();
        }
    }
}
