using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hudson.Core.RequestRecorder
{
	public class RequestRecorderMiddleware : IMiddleware
	{
		private readonly IRequestRecorder requestRecorder = null;

		public RequestRecorderMiddleware()
		{
			this.requestRecorder = new DefaultRequestRecorder();
		}

		public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
		{
			var recorderContext = CreateRecorderContext(httpContext);

			var recorderFilter = new RequestRecorderFilter(recorderContext);
			//var actionContext = actionContextAccessor.ActionContext;
			//actionContext.ActionDescriptor.FilterDescriptors.Add(new Microsoft.AspNetCore.Mvc.Filters.FilterDescriptor(recorderFilter, 0));

			requestRecorder.OnRequest(recorderContext);
			
			try
			{
				await next(httpContext);
			}
			catch (Exception exception)
			{
				Exception exc = requestRecorder.OnException(recorderContext, exception);
				throw exc;
			}

			requestRecorder.OnResponse(recorderContext);
		}

		/// <summary>
		/// 创建记录上下文对象
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		private RecorderContext CreateRecorderContext(HttpContext httpContext)
		{
			var cloudContext = httpContext.RequestServices.GetService<CloudContext>();

			return new RecorderContext()
			{
				CloudContext = cloudContext,
				HttpContext = httpContext
			};
		}
	}
}
