using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Steeltoe.Discovery.Client;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Steeltoe.Common.Http.Discovery;
using Hudson.Core.RequestRecorder;
using Microsoft.AspNetCore.Http;
using Hudson.Core.Util;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hudson.Core
{
	public static class EurekaAssistantServiceCollectionExtensions
	{
		/// <summary>
		/// 向DI注册DiscoveryClient服务和RestService
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static AssistantServiceConfigurer BeginConfigureEurekaAssistant(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
		{
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

			//id 生成器
			services.AddSingleton<IIdWorker, GUIDWorker>();

			//云服务上下文
			services.AddScoped<CloudContext>();

			//日志记录中间件
			services.AddTransient<RequestRecorderMiddleware>();

			//eureka 相关service
			services.AddDiscoveryClient(configuration);

			services.AddTransient<DiscoveryHttpMessageHandler, EurekaAssistantHttpMessageHandler>();
			//services.AddTransient<DiscoveryHttpMessageHandler>(provider => provider.GetService<EurekaAssistantHttpMessageHandler>());

			return new AssistantServiceConfigurer(services, configuration);
		}
	}
}
