using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public static class GlobalSetting
    {
        private static readonly string _assemblyName = typeof(GlobalSetting).Assembly.FullName;

        static GlobalSetting()
        {
            Type[] types = TypeHelper.GetExtendTypes(typeof(IGlobalStarter),GlobalStarterFilter, GlobalStarterAssemblyFilter);
            if(types.Length == 1)
            {
                IGlobalStarter instance = Activator.CreateInstance(types[0]) as IGlobalStarter;
                instance?.Start();
            }
            else if(types.Length > 1)
            {
                throw new Exception("设置了多个IGlobalStarter实现类");
            }
        }

        private static bool GlobalStarterFilter(Type type)
        {
            return !type.IsAbstract && !type.IsGenericType;
        }

        private static bool GlobalStarterAssemblyFilter(Assembly assembly)
        {
            return assembly.FullName.StartsWith(_assemblyName);
        }

        public static IServiceContainer Container { get; private set; }

        public static void SetContainer(IServiceContainer container)
        {
            Container = container;
        }

        public static IServiceController ServiceController
        {
            get { return Container?.GetService<IServiceController>(); }
        }

        public static void Start(IServiceContainer container)
        {
            SetContainer(container);
            ServiceController.Start();
        }

        public static void Stop()
        {
            ServiceController.Stop();
        }
    }
}