using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceRegistry : IServiceRegistry
    {
        private IConsulClient _client;
        private RegistrationCenter _config;

        public ThriftServiceRegistry()
        {
        }

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
                throw new NullReferenceException("无法使用ConsulRegistrationCenter注册非consul的注册中心");
            if (consulCenter.Clusters == null || consulCenter.Clusters.Count == 0)
                throw new Exception("未设置注册中心地址");
        }

        public virtual void Register(ICollection<Service> services)
        {
            Task<WriteResult>[] taskArr = services.Cast<ThriftService>().Select(service =>
            {
                var tmpTask = _client.Agent.ServiceRegister(AgentServiceHelper.CreateAgentService(service));
                tmpTask.ConfigureAwait(false);

                return tmpTask;
            }).ToArray();
            Task task = Task.Factory.ContinueWhenAll(taskArr, tasks =>
            {
                foreach (Task<WriteResult> tmp in tasks)
                {
                    if (tmp.Exception != null)
                    {
                        throw new ServiceRegisterException(_config,"注册服务时发生错误",tmp.Exception);
                    }
                    else if (tmp.IsCanceled)
                    {
                        return;
                    }
                    else if (tmp.IsCompleted)
                    {
                        if (tmp.Result.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            throw new ServiceRegisterException(_config, "status code : " + tmp.Result.StatusCode, tmp.Exception);
                        }
                    }
                    else if (tmp.IsFaulted)
                    {
                        throw new ServiceRegisterException(_config, "注册服务失败", tmp.Exception);
                    }
                }
            });
            task.ConfigureAwait(false);
            task.Wait(5000);
        }
    }
}