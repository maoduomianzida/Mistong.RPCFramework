using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ActionContext
    {
        public ActionContext(ActionDescriptor actionDescriptor,object instance,IDictionary<string,object> paramValues)
        {
            ActionDescriptor = actionDescriptor ?? throw new ArgumentNullException(nameof(actionDescriptor));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            ParamValues = paramValues ?? throw new ArgumentNullException(nameof(paramValues));
        }

        public ActionDescriptor ActionDescriptor { get; private set; }

        public object Instance { get; private set; }

        public IDictionary<string,object> ParamValues { get; private set; }
    }
}