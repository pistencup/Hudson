using Microsoft.Extensions.DependencyInjection;
using Refit;
using Steeltoe.Common.Http.Discovery;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Hudson.Core
{
	/// <summary>
	/// Refit 的服务处理帮助类
	/// </summary>
	internal class RefitServiceRegister
	{
		internal static RefitServiceRegister Instance { get; } = new RefitServiceRegister();

		private readonly RefitSettings refitSettings = null;
        private MethodInfo registBaseServiceMethod = null;
        private RefitServiceRegister()
		{
            registBaseServiceMethod = this.GetType().GetMethod("RegisterBaseService", BindingFlags.NonPublic | BindingFlags.Instance);
            refitSettings = new RefitSettings();
		}
        /// <summary>
        /// 注册 Refit 服务, 并设置基地址和 HttpMessageHandler
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="baseUrl"></param>
        /// <param name="workWithEureka"></param>
        public void RegisterService(IServiceCollection services, Type type, string baseUrl, bool workWithEureka)
		{

			var method = registBaseServiceMethod.MakeGenericMethod(type);

			var httpClientBuilder = method.Invoke(this, new object[] { services }) as IHttpClientBuilder;

			if (workWithEureka)
			{
				httpClientBuilder.AddHttpMessageHandler<DiscoveryHttpMessageHandler>();
			}

			httpClientBuilder.ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
		}
		/// <summary>
		/// 从 Refit.Factory 项目中copy的代码, 用来方便的构造一个可以被反射调用的泛型方法.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		private IHttpClientBuilder RegisterBaseService<T>(IServiceCollection services) where T : class
		{
			services.AddSingleton(provider => RequestBuilder.ForType<T>(GetRefitSettings()));
            
			return services.AddHttpClient(typeof(T).Name)
						   .AddTypedClient((client, serviceProvider) => RestService.For<T>(client, serviceProvider.GetService<IRequestBuilder<T>>()));
		}
		/// <summary>
		/// 在此处创建 RefitSetting 的配置
		/// </summary>
		/// <returns></returns>
		private RefitSettings GetRefitSettings()
		{
			return refitSettings;
		}
	}
}
