using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public class CircuitBreakerException : Exception
    {
        public CircuitBreakerException()
        { }

        public CircuitBreakerException(string message) : base(message)
        { }

        public CircuitBreakerException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
