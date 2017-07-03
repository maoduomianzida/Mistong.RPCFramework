using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ActionDescriptor
    {
        public ActionDescriptor(MethodInfo method)
        {
            Action = method ?? throw new ArgumentNullException(nameof(method));
        }

        public MethodInfo Action { get; private set; }

        public string ActionName
        {
            get { return Action.Name; }
        }

        public ParameterInfo[] Params
        {
            get { return Action.GetParameters(); }
        }
    }
}