using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务所在的程序集
    /// </summary>
    public interface IServiceAssembliesResolver
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}