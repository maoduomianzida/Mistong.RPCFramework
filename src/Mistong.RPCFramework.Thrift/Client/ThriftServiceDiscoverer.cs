using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceDiscoverer : IServiceDiscoverer
    {
        private IConsulClient _client;
        private RegistrationCenter _config;

        public RegistrationCenter RegistrationCenter
        {
            get
            {
                return _config;
            }

            set
            {
                ValidateConfig(value);
                _config = value;
                _client = new ConsulClient(tmp =>
                {
                    tmp.Address = new Uri(value.Clusters.First());
                });
            }
        }

        private void ValidateConfig(RegistrationCenter center)
        {
            ConsulRegistrationCenter consulCenter = center as ConsulRegistrationCenter;
            if (consulCenter == null)
                throw new NullReferenceException("无法使用ConsulRegistrationCenter发现非consul的注册中心");
            if (consulCenter.Clusters == null || consulCenter.Clusters.Count == 0)
                throw new Exception("未设置注册中心地址");
        }

        private Dictionary<string,AgentService> GetServicesFromConsul()
        {
            Task<Dictionary<string, AgentService>> servicesTask = _client.Agent.Services().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    throw new ServiceDiscovererException(_config, "发现服务时发生错误", task.Exception);
                }
                else if (task.IsCanceled)
                {
                    return null;
                }
                else if (task.IsCompleted)
                {
                    if (task.Result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new ServiceDiscovererException(_config, "status code : " + task.Result.StatusCode, task.Exception);
                    }

                    return task.Result.Response;
                }
                else if (task.IsFaulted)
                {
                    throw new ServiceDiscovererException(_config, "发现服务失败", task.Exception);
                }

                return null;
            });
            servicesTask.ConfigureAwait(false);
            servicesTask.Wait(5000);

            return servicesTask.Result ?? new Dictionary<string, AgentService>(0);
        }

        public IEnumerable<Service> Discover(IEnumerable<ServiceMap> serviceMaps)
        {
            Dictionary<string, AgentService> serivces = GetServicesFromConsul();
            foreach (ServiceMap serviceMap in serviceMaps)
            {
                
            }

            return null;
        }
    }
}