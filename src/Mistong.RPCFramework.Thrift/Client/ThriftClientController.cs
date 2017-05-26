using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftClientController : IClientController
    {
        public void Start()
        {
            IServiceDiscovererConfiguration configuration = GlobalSetting.Container.GetService<IServiceDiscovererConfiguration>();
            if (configuration == null)
                throw new NullReferenceException("IServiceDiscovererConfiguration接口不能为空");
            ServiceConfig serviceConfig = configuration.GetServiceConfig();
            if (serviceConfig == null)
                throw new NullReferenceException("服务配置信息不存在");
            IServiceAssembliesResolver assembliesResolver = GlobalSetting.Container.GetService<IServiceAssembliesResolver>();
            if (assembliesResolver == null)
                throw new NullReferenceException("IServiceAssembliesResolver接口不能为空");
            IServiceFinder serviceFinder = GlobalSetting.Container.GetService<IServiceFinder>();
            if (serviceFinder == null)
                throw new NullReferenceException("IServiceFinder接口不能为空");
            IEnumerable<ServiceMap> serviceMaps = FilterServiceMap(serviceFinder.Find(assembliesResolver.GetAssemblies()), serviceConfig.Server);
            IServiceDiscoverer serviceDiscoverer = GlobalSetting.Container.GetService<IServiceDiscoverer>();
            if (serviceDiscoverer == null)
                throw new NullReferenceException("IServiceDiscoverer接口不能为空");
            serviceDiscoverer.RegistrationCenter = serviceConfig.RegistrationCenter;
            IEnumerable<Service> servics = FilterServices(serviceDiscoverer.Discover(serviceMaps), serviceConfig.Client);
            IThriftClientActivator thriftClientActivator = GlobalSetting.Container.GetService<IThriftClientActivator>();
            if (thriftClientActivator == null)
                throw new NullReferenceException("IThriftClientActivator接口不能为空");
            thriftClientActivator.RegisterServices(servics);
        }

        /// <summary>
        /// 过滤掉不符合条件的ServiceMap
        /// </summary>
        /// <param name="serviceMaps">自动找到的所有符合条件的thrift 服务</param>
        /// <param name="services">服务端配置的thrift 服务</param>
        /// <returns></returns>
        protected virtual IEnumerable<ServiceMap> FilterServiceMap(IEnumerable<ServiceMap> serviceMaps, IEnumerable<Service> services)
        {
            IEnumerable<ThriftService> thriftServices = services.Cast<ThriftService>();
            foreach (ServiceMap map in serviceMaps)
            {
                if (thriftServices.SingleOrDefault(tmp => map.InheritInterface(tmp.ServiceInterfaceType)) == null)
                {
                    yield return map;
                }
            }
        }

        /// <summary>
        /// 去掉从注册发现并且在本地Client已经配置的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientServices"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Service> FilterServices(IEnumerable<Service> services, IEnumerable<Service> clientServices)
        {
            List<ThriftService> thriftServices = new List<ThriftService>();
            IEnumerable<ThriftService> thriftClientServices = clientServices.Cast<ThriftService>();
            foreach (ThriftService service in services.Cast<ThriftService>())
            {
                //说明本地没有配置该服务
                if (thriftClientServices.FirstOrDefault(tmp => tmp.ServiceInterfaceType.IsAssignableFrom(service.ServiceType)) == null)
                {
                    thriftServices.Add(service);
                }
            }
            thriftServices.AddRange(thriftClientServices);

            return thriftServices;
        }
    }
}
