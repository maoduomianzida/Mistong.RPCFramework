using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 从程序集查找服务接口以及实现类
    /// </summary>
    public interface IServiceFinder
    {
        IEnumerable<ServiceMap> Find(IServiceAssembliesResolver resolver);
    }
}