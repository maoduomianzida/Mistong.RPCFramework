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
        public static IServiceContainer Container { get; private set; }

        public static void SetContainer(IServiceContainer container)
        {
            Container = container;
        }

        public static IServiceController ServiceController
        {
            get { return Container?.GetService<IServiceController>(); }
        }

        public static IServiceDiscoverer ServiceDiscoverer
        {
            get { return Container?.GetService<IServiceDiscoverer>(); }
        }

        public static void Start(IServiceContainer container)
        {
            SetContainer(container);
            ServiceDiscoverer.Discover();
            ServiceController.Start();
        }

        public static void Stop()
        {
            ServiceController.Stop();
        }
    }
}