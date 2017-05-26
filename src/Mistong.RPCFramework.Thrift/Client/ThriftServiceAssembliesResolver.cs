using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceAssembliesResolver : IServiceAssembliesResolver
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            Assembly assembly = ThriftServiceHelper.GetThriftServiceAssembly();
            if (assembly == null)
                return Enumerable.Empty<Assembly>();

            return new Assembly[] { assembly };
        }
    }
}