using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public class RemoteResourceException : Exception
    {
        public RemoteResourceException()
        { }

        public RemoteResourceException(string message) : base(message)
        { }

        public RemoteResourceException(string message,Exception innerException) : base(message, innerException)
        { }
    }
}
