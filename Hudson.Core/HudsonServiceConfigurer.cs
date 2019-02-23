using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hudson.Core
{
	/// <summary>
	/// 服务配置器
	/// 当需要返回 IServiceCollection 时, 以Complete方法结束调用.
	/// </summary>
	public class HudsonServiceConfigurer
	{
		private readonly IServiceCollection services;
		private readonly IConfiguration configuration;

		public HudsonServiceConfigurer(IServiceCollection services, IConfiguration configuration)
		{
			//this.services = services;
			//this.configuration = configuration;
			this.services = services ?? throw new ArgumentException("services 不能为 NULL");
			this.configuration = configuration ?? throw new ArgumentException("configuration 不能为 NULL");

		}
		/// <summary>
		/// 通过 ServiceClientAttribute 中的信息将调用接口注册到 IServiceCollection
		/// </summary>
		/// <param name="attr"></param>
		private void AddSingleServiceByClientAttribute(ServiceClientAttribute attr, Type type)
		{
			if (attr == null)
			{
				return;
			}
			RefitServiceRegister.Instance.RegisterService(services, type, attr.GetBasePath(), attr.IsWorkWithEureka());
		}
		/// <summary>
		/// 从类型中取出 ServiceClientAttribute
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private ServiceClientAttribute GetServiceClientAttribute(Type type)
		{
			if (type != null && type.IsInterface)
			{
				return type.GetCustomAttribute<ServiceClientAttribute>(true);
			}
			return null;
		}
		/// <summary>
		/// 扫描程序集中的所有 public 类型并尝试注册到 IServiceCollection
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public HudsonServiceConfigurer AddServiceClients(Assembly assembly)
		{
			if (assembly != null)
			{
				AddServiceClients(assembly.GetExportedTypes());
			}
			return this;
		}
		/// <summary>
		/// 尝试将列表中的类型注册到 IServiceCollection
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		public HudsonServiceConfigurer AddServiceClients(IEnumerable<Type> types)
		{
			if (types != null)
			{
				foreach (var type in types)
				{
					AddServiceClient(type);
				}
			}
			return this;
		}
		/// <summary>
		/// 尝试将类型注册到 IServiceCollection
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public HudsonServiceConfigurer AddServiceClient(Type type)
		{
			var attr = GetServiceClientAttribute(type);

			AddSingleServiceByClientAttribute(attr, type);
			
			return this;
		}

		public HudsonServiceConfigurer AddServiceClient<T>()
		{
			return AddServiceClient(typeof(T));
		}
		/// <summary>
		/// 结束配置过程, 返回 IServiceCollection.
		/// </summary>
		/// <returns></returns>
		public IServiceCollection Complete()
		{
			return services;
		}
	}
}
