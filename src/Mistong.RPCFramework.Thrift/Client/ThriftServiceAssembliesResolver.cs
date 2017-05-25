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
            string applicationName = GetApplicationName();
            try
            {
                Assembly assembly = Assembly.Load(applicationName + ".Thrift");

                return new Assembly[] { assembly };
            }
            catch(FileNotFoundException)
            {
            }

            return Enumerable.Empty<Assembly>();
        }

        protected virtual string GetApplicationName()
        {
            string name = AppDomain.CurrentDomain.FriendlyName;
            int index = name.LastIndexOf(".");
            name = name.Substring(0, index);

            return name;
        }
    }
}
