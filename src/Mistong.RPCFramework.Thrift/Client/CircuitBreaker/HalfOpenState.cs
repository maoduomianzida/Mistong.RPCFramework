using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public class HalfOpenState : CircuitBreakerState
    {
        private readonly int _allowExecuteNums;
        private int _currentExecuteNums;
        private int _currentExecuteSuccessTimes;

        public HalfOpenState(CircuitBreaker context, int allowExecuteNums) : base(context)
        {
            if (allowExecuteNums <= 0) throw new ArgumentOutOfRangeException("允许请求的次数不能少于0次");

            _allowExecuteNums = allowExecuteNums;
        }

        public override void Initialize()
        {
        }

        public override void ProcessFail()
        {
            Context.MoveToOpenState();
        }

        public override void ProcessSuccess()
        {
            if (Interlocked.Increment(ref _currentExecuteSuccessTimes) >= _allowExecuteNums)
            {
                Context.MoveToCloseState();
            }
        }

        public override void ProcessBefore()
        {
            int executeNums = Interlocked.Increment(ref _currentExecuteNums);
            if (executeNums > _allowExecuteNums)
            {
                throw new CircuitBreakerException($"已经超过了半开状态允许请求的次数限制（{_allowExecuteNums}次）");
            }
        }

        private void ResetAllowExecuteNums()
        {
            Interlocked.Exchange(ref _currentExecuteNums, 0);
            Interlocked.Exchange(ref _currentExecuteSuccessTimes, 0);
        }

        public override void Clear()
        {
            ResetAllowExecuteNums();
        }
    }
}
