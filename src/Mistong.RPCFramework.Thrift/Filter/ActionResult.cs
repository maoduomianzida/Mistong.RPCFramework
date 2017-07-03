using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ActionResult
    {
        public ActionResult(ActionDescriptor actionDescriptor,object result)
        {
            ActionDescriptor = actionDescriptor ?? throw new ArgumentNullException(nameof(actionDescriptor));
            Result = result;
        }

        public object Result { get; private set; }

        public ActionDescriptor ActionDescriptor { get; private set; }
    }
}