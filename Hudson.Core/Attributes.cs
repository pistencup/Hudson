using System;
using System.Collections.Generic;
using System.Text;

namespace Refit
{
	/// <summary>
	/// 用于指定该接口是某个远端服务的定义
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
	public class ServiceClientAttribute:Attribute
	{
		private readonly string basePath = null;
		private readonly bool workWithEureka = false;

		public ServiceClientAttribute(string basePath)
		{
			this.basePath = basePath;
		}
		public ServiceClientAttribute(string basePath, bool workWithEureka)
		{
			this.basePath = basePath;
			this.workWithEureka = workWithEureka;
		}
		/// <summary>
		/// 获取服务的基地址
		/// </summary>
		/// <returns></returns>
		public string GetBasePath()
		{
			return basePath;
		}

		public bool IsWorkWithEureka()
		{
			return workWithEureka;
		}
	}
	/// <summary>
	/// 用于指定该接口是与eureka协作的远端服务定义
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
	public class WorkWithEurekaAttribute: ServiceClientAttribute
	{
		public WorkWithEurekaAttribute(string serviceName) : base(GetEurekaBasePath(serviceName), true) { }
		
		private static string GetEurekaBasePath(string basePath)
		{
			return $"http://{basePath}";
		}
	}
}
