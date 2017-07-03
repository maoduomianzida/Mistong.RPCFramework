using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ExceptionContext
    {
        public ActionDescriptor ActionDescriptor { get; private set; }

        public Exception Exception { get; private set; }

        public bool HandException { get; set; }

        public ExceptionContext(ActionDescriptor actionDescriptor,Exception err)
        {
            ActionDescriptor = actionDescriptor ?? throw new ArgumentNullException(nameof(err));
            Exception = err ?? throw new ArgumentNullException(nameof(err));
            HandException = false;
        }
    }
}