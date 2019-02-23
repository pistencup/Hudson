using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

using Steeltoe.Discovery.Client;
using Hudson.Core.RequestRecorder;

namespace Hudson.Core
{
	public static class HudsonApplicationBuilderExtensions
	{
		/// <summary>
		/// 在Configure方法中增加处理中间件, 需要放在app.UseMvc方法之前, 默认启用请求记录器
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseHudson(this IApplicationBuilder app)
		{
			return app.UseHudson(true);
		}

		/// <summary>
		/// 在Configure方法中增加处理中间件, 需要放在app.UseMvc方法之前
		/// </summary>
		/// <param name="app"></param>
		/// <param name="useRequestRecoder">是否启用请求记录器功能</param>
		/// <returns></returns>
		public static IApplicationBuilder UseHudson(this IApplicationBuilder app, bool useRequestRecoder)
		{
			if (useRequestRecoder)
			{
				app.UseMiddleware<RequestRecorderMiddleware>();
			}
			app.UseDiscoveryClient();

			return app;
		}
	}
}
