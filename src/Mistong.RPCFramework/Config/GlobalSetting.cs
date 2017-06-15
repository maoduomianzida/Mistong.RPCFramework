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

        public static void Start()
        {
            if (Container == null) throw new NullReferenceException("IServiceContainer接口不能为空");
            ServiceConfig config = GetServiceConfig();
            ClientController.Start(config);
            ServiceController.Start(config);
        }

        public static void Start(IServiceContainer container)
        {
            SetContainer(container);
            Start();
        }

        private static ServiceConfig GetServiceConfig()
        {
            IServiceConfiguration configuration = Container.GetService<IServiceConfiguration>();
            if (configuration == null)
                throw new NullReferenceException("IServiceConfiguration接口不能为空");
            ServiceConfig serviceConfig = configuration.GetServiceConfig();
            if (serviceConfig == null)
                throw new NullReferenceException("服务配置信息不存在");

            return serviceConfig;
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