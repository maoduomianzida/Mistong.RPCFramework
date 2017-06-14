using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public interface IExceptionFilter
    {
        T HandException<T>(ExceptionContext context);
    }
}