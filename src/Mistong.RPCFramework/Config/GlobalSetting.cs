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

        public static IServerController ServiceController
        {
            get { return Container?.GetService<IServerController>(); }
        }

        public static IClientController ClientController
        {
            get { return Container?.GetService<IClientController>(); }
        }

        public static void Start(IServiceContainer container)
        {
            SetContainer(container);
            ClientController.Start();
            ServiceController.Start();
        }

        public static T GetService<T>() where T : class
        {
            return Container?.GetService<T>();
        }

        public static void Stop()
        {
            ServiceController.Stop();
        }
    }
}