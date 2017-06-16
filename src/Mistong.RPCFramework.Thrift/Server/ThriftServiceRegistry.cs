using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceRegistry : IServiceRegistry, IServiceHealthCheck
    {
        private IConsulClient _client;
        private RegistrationCenter _registrationCenter;
        private TimeSpan _waitConsulTime;

        /// <summary>
        /// 服务注册接口
        /// </summary>
        /// <param name="registrationCenter">注册中心信息</param>
        /// <param name="waitConsulTime">等待consul操作的超时时间</param>
        public ThriftServiceRegistry(RegistrationCenter registrationCenter,TimeSpan waitConsulTime)
        {
            if (registrationCenter == null) throw new ArgumentNullException(nameof(registrationCenter));
            if (waitConsulTime == TimeSpan.Zero) throw new ArgumentException("Consul操作时间不能小于等于0");

            ValidateConfig(registrationCenter);
            _registrationCenter = registrationCenter;
            _waitConsulTime = waitConsulTime;
            _client = new ConsulClient(tmp =>
            {
                tmp.Address = new Uri(_registrationCenter.Clusters.First());
            });
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
            if (services.Count == 0) return;
            Task<WriteResult>[] taskArr = services.Cast<ThriftService>().Select(service =>
            {
                var tmpTask = _client.Agent.ServiceRegister(AgentServiceHelper.CreateAgentService(service));
                tmpTask.ConfigureAwait(false);

                return tmpTask;
            }).ToArray();
            if (taskArr.Length > 0)
            {
                Task task = Task.Factory.ContinueWhenAll(taskArr, tasks =>
                {
                    foreach (Task<WriteResult> tmp in tasks)
                    {
                        if (tmp.Exception != null)
                        {
                            throw new ServiceRegisterException(_registrationCenter, "注册服务时发生错误", tmp.Exception);
                        }
                        else if (tmp.IsCanceled)
                        {
                            return;
                        }
                        else if (tmp.IsCompleted)
                        {
                            if (tmp.Result.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                throw new ServiceRegisterException(_registrationCenter, "status code : " + tmp.Result.StatusCode, tmp.Exception);
                            }
                        }
                        else if (tmp.IsFaulted)
                        {
                            throw new ServiceRegisterException(_registrationCenter, "注册服务失败", tmp.Exception);
                        }
                    }
                });
                task.ConfigureAwait(false);
                task.Wait(_waitConsulTime);
            }
        }

        public virtual void AddHeadlthCheck(ICollection<Service> services)
        {
            if (services.Count == 0) return;
            IServiceHealthCheckCreator checkCreator = GlobalSetting.GetService<IServiceHealthCheckCreator>();
            if (checkCreator == null) throw new NullReferenceException("IServiceHealthCheckCreator接口不能为空");
            AgentCheckRegistration check = checkCreator.CreateCheck();
            if (check == null) throw new NullReferenceException("无法创建健康检查实例");
            var task = _client.Agent.CheckRegister(check);
            task.ConfigureAwait(false);
            task.Wait(_waitConsulTime);
            if(task.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("添加健康检查信息失败");
            }
            checkCreator.EnableHealthCheckInterface();
        }
    }
}