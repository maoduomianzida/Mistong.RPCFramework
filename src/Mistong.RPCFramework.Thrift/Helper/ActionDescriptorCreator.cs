using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Mistong.RPCFramework.Thrift
{
    public static class ActionDescriptorCreator
    {
        private static ConcurrentDictionary<MethodInfo, ActionDescriptor> _actionDescriptor = new ConcurrentDictionary<MethodInfo, ActionDescriptor>();

        public static ActionDescriptor GetActionDescriptor(MethodInfo methodInfo)
        {
            return _actionDescriptor.GetOrAdd(methodInfo,method => new ActionDescriptor(method));
        }
    }
}