using Mistong.RPCFramework.Thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public interface IActionFilter
    {
        void ExecuteBefore(ActionContext context);
        void ExecuteAfter(ActionResult result);
    }
}