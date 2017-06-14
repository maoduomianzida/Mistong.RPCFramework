using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ExceptionContext
    {
        public Exception Exception { get; private set; }

        public bool HandException { get; set; }

        public ExceptionContext(Exception err)
        {
            if (err == null) throw new ArgumentNullException(nameof(err));
            Exception = err;
            HandException = false;
        }
    }
}
