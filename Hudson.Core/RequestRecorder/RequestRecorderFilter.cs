using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Hudson.Core.RequestRecorder
{
	/// <summary>
	/// 读取action的输入和输出并记录在上下文中
	/// </summary>
	internal class RequestRecorderFilter : IAsyncActionFilter
	{
		private readonly RecorderContext recorderContext;

		public RequestRecorderFilter(RecorderContext recorderContext)
		{
			this.recorderContext = recorderContext;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			recorderContext.ActionExecutingContext = context;
			
			recorderContext.ActionExecutedContext = await next();
		}
	}
}
