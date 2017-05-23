﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Mistong.Logger;
using Mistong.Logger.Message;

namespace Mistong.RPCFramework.Thrift
{
    public class ConsulServiceRegistry : IServiceRegistry
    {
        private IConsulClient _client;
        private ConfigCenter _config;
        private ILogger _logger;

        public ConsulServiceRegistry(ConfigCenter config, ILogger logger)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;
            _logger = logger;
            ValidateConfig(config);
            _client = new ConsulClient(tmp =>
            {
                tmp.Address = new Uri(config.Clusters.First());
            });
        }

        private void ValidateConfig(ConfigCenter center)
        {
            ConsulConfigCenter consulCenter = center as ConsulConfigCenter;
            if (consulCenter == null)
                throw new NullReferenceException("无法使用ConsulServiceRegistry注册非consul的配置中心");
            if (consulCenter.Clusters == null || consulCenter.Clusters.Count == 0)
                throw new Exception("未设置配置中心地址");
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
                        _logger?.Log(new ApplicationErrorMessage(tmp.Exception, "注册服务时发生错误"));
                    }
                    else if (tmp.IsCanceled)
                    {
                        return;
                    }
                    else if (tmp.IsCompleted)
                    {
                        if (tmp.Result.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            _logger?.Log(new ApplicationMessage
                            {
                                Comments = $"注册服务异常 consul:({string.Join(",", _config.Clusters)})",
                                LogLevel = Logger.Message.LogLevel.Error,
                                Message = "status code : " + tmp.Result.StatusCode
                            });
                        }
                    }
                    else if (tmp.IsFaulted)
                    {
                        _logger?.Log(new ApplicationMessage
                        {
                            Comments = $"注册服务异常 consul:({string.Join(",", _config.Clusters)})",
                            LogLevel = Logger.Message.LogLevel.Error,
                            Message = "注册服务失败"
                        });
                    }
                }
            });
            task.ConfigureAwait(false);
            task.Wait(5000);
        }
    }
}