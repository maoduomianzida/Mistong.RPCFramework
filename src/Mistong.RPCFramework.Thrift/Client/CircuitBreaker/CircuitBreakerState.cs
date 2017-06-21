using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public abstract class CircuitBreakerState
    {
        protected CircuitBreaker Context { get; private set; }

        protected CircuitBreakerState(CircuitBreaker context)
        {
            Context = context;
        }

        public abstract void Initialize();

        public abstract void Clear();

        public abstract void ProcessBefore();

        public abstract void ProcessSuccess();

        public abstract void ProcessFail();
    }
}