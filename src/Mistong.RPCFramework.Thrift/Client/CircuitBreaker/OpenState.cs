using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public class OpenState : CircuitBreakerState,IDisposable
    {
        private Timer _switchHalfOpenTimer;
        private TimeSpan _switchHalfOpenTimeSpan;

        public OpenState(CircuitBreaker context, TimeSpan switchHalfOpenTimeSpan) : base(context)
        {
            if (switchHalfOpenTimeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("时间间隔不能小于0");
            }
            _switchHalfOpenTimeSpan = switchHalfOpenTimeSpan;
            _switchHalfOpenTimer = new Timer(SwitchHalfOpenState, null, -1, -1);
        }

        private void SwitchHalfOpenState(object state)
        {
            Context.MoveToHalfOpenState();
        }

        public override void Initialize()
        {
            _switchHalfOpenTimer.Restart(_switchHalfOpenTimeSpan, _switchHalfOpenTimeSpan);
        }

        public override void ProcessFail()
        {
        }

        public override void ProcessSuccess()
        {
            throw new CircuitBreakerException("熔断器开启状态下无法执行ProcessSuccess方法");
        }

        public override void ProcessBefore()
        {
            throw new CircuitBreakerException("熔断器开启状态下无法执行ProcessBefore方法");
        }

        public override void Clear()
        {
            _switchHalfOpenTimer.Stop();
        }

        public void Dispose()
        {
            _switchHalfOpenTimer?.Dispose();
            _switchHalfOpenTimer = null;
        }
    }
}
